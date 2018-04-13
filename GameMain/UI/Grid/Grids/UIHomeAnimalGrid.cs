
//*******************************************************************************************
//	创建日期：	2016-10-8   18:15
//	文件名称：	UIHomeAnimalGrid,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	Lin Chuang Yuan
//	说    明：	
//*******************************************************************************************
using Client;
using Engine.Utility;
using GameCmd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


class UIHomeAnimalGrid : MonoBehaviour, ITimer
{
    GameObject m_goLockBtn;  //1、 锁


    GameObject m_goExtension; //2、可扩建,未解锁
    UILabel m_lblExtension;
    GameObject m_goExtensionBtn;


    GameObject m_goIdle;      //3、空闲，可以养殖


    GameObject m_goImplant;   //4、已经养殖的
    UISprite m_Icon;
    UILabel m_lblName;
    UILabel m_lblTime;
    GameObject m_goAtOnce;//立即成熟
    GameObject m_gain;//收获

    uint m_farmId;
    uint m_animalIndexStart;
    uint m_nLeftTime;
    Dictionary<uint, uint> m_SeedIndexDic;

    private const int HOMEANIMAL_TIMERID = 1000;

    void Awake()
    {
        m_goLockBtn = this.transform.Find("lock").gameObject;
        m_goExtension = this.transform.Find("extension").gameObject;
        m_goIdle = this.transform.Find("null").gameObject;
        m_goImplant = this.transform.Find("implant").gameObject;

        m_lblExtension = this.transform.Find("extension/unLockCondition").GetComponent<UILabel>();
        m_goExtensionBtn = this.transform.Find("extension/btn_unlock").gameObject;

        m_Icon = this.transform.Find("implant/icon").GetComponent<UISprite>();
        m_lblName = this.transform.Find("implant/name").GetComponent<UILabel>();
        m_lblTime = this.transform.Find("implant/time").GetComponent<UILabel>();
        m_goAtOnce = this.transform.Find("implant/btn_nowmature").gameObject; //立即成熟
        m_gain = this.transform.Find("implant/btn_gain").gameObject; //收获

        UIEventListener.Get(m_goLockBtn).onClick = null;

        UIEventListener.Get(m_goExtensionBtn).onClick = ExtensionBtn;

        UIEventListener.Get(m_goIdle).onClick = IdleBtn;

        UIEventListener.Get(m_goAtOnce).onClick = AtOnceBtn;

        UIEventListener.Get(m_gain).onClick = GainBtn;
    }

    public void Init(uint farmId)
    {
        this.m_farmId = farmId;

        m_animalIndexStart = DataManager.Manager<HomeDataManager>().GetAnimalIndexStart();
        m_SeedIndexDic = DataManager.Manager<HomeDataManager>().SeedIndexDic;
        HomeDataManager.LandState landState = DataManager.Manager<HomeDataManager>().GetAnimalLandState(farmId);
        SetGridActivity(landState);

    }

    void SetGridActivity(HomeDataManager.LandState ls)
    {
        switch (ls)
        {
            case HomeDataManager.LandState.LockNotBuy: //锁
                {
                    m_goLockBtn.SetActive(true);
                    m_goExtension.SetActive(false);
                    m_goIdle.SetActive(false);
                    m_goImplant.SetActive(false);
                };
                break;

            case HomeDataManager.LandState.LockCanBuy://可扩展,未解锁
                {
                    m_goLockBtn.SetActive(false);
                    m_goExtension.SetActive(true);
                    m_goIdle.SetActive(false);
                    m_goImplant.SetActive(false);
                    SetExtensionLbl();
                };
                break;

            case HomeDataManager.LandState.Idle:    //空闲，可种植
                {
                    m_goLockBtn.SetActive(false);
                    m_goExtension.SetActive(false);
                    m_goIdle.SetActive(true);
                    m_goImplant.SetActive(false);
                };
                break;

            case HomeDataManager.LandState.CanGain:  //可收获（立即成熟，收获）
                {
                    m_goLockBtn.SetActive(false);
                    m_goExtension.SetActive(false);
                    m_goIdle.SetActive(false);
                    m_goImplant.SetActive(true);
                    SetGainOrAtOnce();
                };
                break;

        }
    }

    /// <summary>
    /// 设置可扩展条件
    /// </summary>
    void SetExtensionLbl()
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        int vip = player.GetProp((int)PlayerProp.Vip);

        List<LandAndFarmDataBase> landList = GameTableManager.Instance.GetTableList<LandAndFarmDataBase>();
        uint animalID = DataManager.Manager<HomeDataManager>().animalID;
        landList = landList.FindAll((LandAndFarmDataBase ld) => { return ld.dwID == animalID; });

        LandAndFarmDataBase data = landList.Find((LandAndFarmDataBase ld) => { return ld.indexID == this.m_farmId; });

        if (data.vipLimitLevel > 0)
        {
            m_lblExtension.text = "需达到VIP" + data.vipLimitLevel;
        }
        else
        {
            m_lblExtension.text = "";
        }
    }

    void SetGainOrAtOnce()
    {
        if (DataManager.Manager<HomeDataManager>().CanGetLeftTime((int)(this.m_farmId + m_animalIndexStart)))
        {
            if (m_SeedIndexDic.ContainsKey(m_farmId + m_animalIndexStart))
            {
                SeedAndCubDataBase cubData = GameTableManager.Instance.GetTableItem<SeedAndCubDataBase>(m_SeedIndexDic[m_farmId + m_animalIndexStart]);
                if (cubData != null)
                {
                    m_Icon.spriteName = cubData.icon;
                    m_lblName.text = cubData.name;
                }
            }

            if (DataManager.Manager<HomeDataManager>().GetLeftTimeByIndex((int)(this.m_farmId + m_animalIndexStart)) == 0)
            {
                SetGainOrAtOnceBtn(true);
            }
            else
            {
                SetGainOrAtOnceBtn(false);

                this.m_nLeftTime = DataManager.Manager<HomeDataManager>().GetLeftTimeByIndex((int)(m_farmId + m_animalIndexStart));
                if (m_nLeftTime>0)
                {
                    TimerAxis.Instance().KillTimer(HOMEANIMAL_TIMERID, this);
                    TimerAxis.Instance().SetTimer(HOMEANIMAL_TIMERID, 1000, this);
                } 
                else
                {
                    TimerAxis.Instance().KillTimer(HOMEANIMAL_TIMERID, this);
                }
            }
        }
    }

    void SetGainOrAtOnceBtn(bool gain)
    {
        if (gain)
        {
            m_goAtOnce.SetActive(false);
            m_gain.SetActive(true);//收获
            m_lblTime.gameObject.SetActive(false);
        }
        else
        {
            m_goAtOnce.SetActive(true);
            m_gain.SetActive(false);//收获
            m_lblTime.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 可扩展
    /// </summary>
    /// <param name="go"></param>
    void ExtensionBtn(GameObject go)
    {
        DataManager.Manager<HomeDataManager>().ReqUnLockAnimalLand(this.m_farmId);
    }

    /// <summary>
    /// 空闲，可播种
    /// </summary>
    /// <param name="go"></param>
    void IdleBtn(GameObject go)
    {
        DataManager.Manager<HomeDataManager>().SetSelectLandIndex((int)this.m_farmId);

        object data = EntityType.EntityType_Animal;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PlantingPanel,data:data);
    }

    /// <summary>
    /// 立即收获
    /// </summary>
    /// <param name="go"></param>
    void AtOnceBtn(GameObject go)
    {
        DataManager.Manager<HomeDataManager>().ReqAtOnceGrow(this.m_farmId);
    }

    /// <summary>
    /// 收获
    /// </summary>
    /// <param name="go"></param>
    void GainBtn(GameObject go)
    {
        if (m_SeedIndexDic.ContainsKey(this.m_farmId + m_animalIndexStart))
        {
            DataManager.Manager<HomeDataManager>().ReqGainAnimal(m_SeedIndexDic[this.m_farmId + m_animalIndexStart], this.m_farmId);
        }
    }

    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == HOMEANIMAL_TIMERID)
        {
            m_nLeftTime -= 1;
            if (m_lblTime != null)
            {
                m_lblTime.text = StringUtil.GetStringBySeconds((uint)m_nLeftTime);
            }
            if (m_nLeftTime == 0)
            {
                TimerAxis.Instance().KillTimer(HOMEANIMAL_TIMERID, this);
                SetGainOrAtOnceBtn(true);//收获
            }
        }
    }
}
