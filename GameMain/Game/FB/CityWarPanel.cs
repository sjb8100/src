using Client;
using Engine.Utility;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Controller;


partial class CityWarPanel : ITimer
{
    #region porperty

    private const string CONST_CITYTOTEMHP_NAME = "CityWarTotemHP";//图腾位置配置

    private List<uint> m_lstTotemMaxHp = null;

    List<CityWarHero> m_lstCityWarHero = null; //

    List<CityWarTotem> m_lstCityWarTotem = null;  //

    UIGridCreatorBase m_WarInfoListGridCreator;  //战斗详情列表

    UIGridCreatorBase m_TotemListGridCreator;    //图腾列表

    private const uint CITYWAREXIT_TIMERID = 1000;

    private uint m_exitTime = 10;

    CityWarManager m_cityWarManger
    {
        get
        {
            return DataManager.Manager<CityWarManager>();
        }
    }

    #endregion



    #region override

    protected override void OnLoading()
    {
        base.OnLoading();
        m_lstTotemMaxHp = GameTableManager.Instance.GetGlobalConfigList<uint>(CONST_CITYTOTEMHP_NAME);

        InitWidget();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        DataManager.Manager<CityWarManager>().ReqCityWarInfo(true);
        InitCityWar();

        if (data != null)
        {
            bool showFinish = (bool)data;
            if (showFinish)
            {
                CityWarFinish();
            }
        }
    }

    protected override void OnHide()
    {
        base.OnHide();

        Release();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eCityWarInfoUpdate)
        {
            InitCityWar();
        }
        else if (msgid == UIMsgID.eCityWarSelfInfoUpdate)
        {
            UpdateMyCityWarInfo();
        }
        else if (msgid == UIMsgID.eCityWarFinish)
        {
            //InitCityWar();
            //UpdateMyCityWarInfo();
            CityWarFinish();
        }

        return true;
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_WarInfoListGridCreator != null)
        {
            m_WarInfoListGridCreator.Release(depthRelease);
        }

        if (m_TotemListGridCreator != null)
        {
            m_TotemListGridCreator.Release(depthRelease);
        }

    }

    #endregion override

    /// <summary>
    /// 初始化组件
    /// </summary>
    private void InitWidget()
    {
        //战斗详情
        UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uicitywargrid) as UnityEngine.GameObject;
        m_WarInfoListGridCreator = m_trans_WarInfoScrollView.GetComponent<UIGridCreatorBase>();
        if (m_WarInfoListGridCreator == null)
            m_WarInfoListGridCreator = m_trans_WarInfoScrollView.gameObject.AddComponent<UIGridCreatorBase>();
        m_WarInfoListGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
        m_WarInfoListGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
        m_WarInfoListGridCreator.gridWidth = 826;
        m_WarInfoListGridCreator.gridHeight = 46;

        m_WarInfoListGridCreator.RefreshCheck();
        m_WarInfoListGridCreator.Initialize<UICityWarMemberInfoGrid>(obj, OnWarInfoGridDataUpdate, OnWarInfoGridUIEvent);


        //图腾列表
        UnityEngine.GameObject totemObj = UIManager.GetResGameObj(GridID.Uicitywartotemgrid) as UnityEngine.GameObject;
        m_TotemListGridCreator = m_trans_TotemScrollView.GetComponent<UIGridCreatorBase>();
        if (m_TotemListGridCreator == null)
            m_TotemListGridCreator = m_trans_TotemScrollView.gameObject.AddComponent<UIGridCreatorBase>();
        m_TotemListGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
        m_TotemListGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
        m_TotemListGridCreator.gridWidth = 300;
        m_TotemListGridCreator.gridHeight = 76;

        m_TotemListGridCreator.RefreshCheck();
        m_TotemListGridCreator.Initialize<UICityWarTotemGrid>(totemObj, OnTotemGridDataUpdate, OnTotemGridUIEvent);
    }

    #region 战斗详情

    void InitCityWar()
    {
        UpdateCityWarHeroListUI();//前20名信息
        UpdateCityWarTower();     //6个图腾信息
        UpdateMyCityWarInfo();    //自己的信息
    }

    //玩家个人的信息
    void UpdateMyCityWarInfo()
    {
        string killAnddeath = string.Format("{0}/{1}", m_cityWarManger.CityWarKillNum, m_cityWarManger.CityWarDeathNum);

        m_label_MyKill.text = killAnddeath;
        m_label_num.text = m_cityWarManger.CityWarRank.ToString();

        if (m_cityWarManger.CityWarTotemList.Count > 0)
        {
            m_label_unlock_name.text = m_cityWarManger.CityWarTotemList[0].clanId != 0 ? m_cityWarManger.CityWarTotemList[0].clanName : "未占领";
        }
    }

    /// <summary>
    /// //前20名信息
    /// </summary>
    void UpdateCityWarHeroListUI()
    {
        m_lstCityWarHero = m_cityWarManger.CityWarHeroList;

        if (m_WarInfoListGridCreator != null)
        {
            m_WarInfoListGridCreator.CreateGrids(m_lstCityWarHero != null ? m_lstCityWarHero.Count : 0);
        }
    }

    /// <summary>
    /// 6个图腾信息
    /// </summary>
    void UpdateCityWarTower()
    {
        m_lstCityWarTotem = m_cityWarManger.CityWarTotemList;

        if (m_TotemListGridCreator != null)
        {
            m_TotemListGridCreator.CreateGrids(m_lstCityWarTotem != null ? m_lstCityWarTotem.Count : 0);
        }
    }


    private void OnWarInfoGridDataUpdate(UIGridBase data, int index)
    {
        if (null != m_lstCityWarHero && index < m_lstCityWarHero.Count)
        {
            UICityWarMemberInfoGrid grid = data as UICityWarMemberInfoGrid;
            if (grid != null)
            {
                grid.SetGridData(m_lstCityWarHero[index]);
                grid.SetRank(index + 1);  //排行
                grid.SetName(m_lstCityWarHero[index].name);
                grid.SetKillNum(m_lstCityWarHero[index].kill_cnt);
                grid.SetDeathNum(m_lstCityWarHero[index].die_cnt);

                if (m_cityWarManger.CityWarClanIdList.Contains(m_lstCityWarHero[index].clan_id))
                {
                    int clanIndex = m_cityWarManger.CityWarClanIdList.IndexOf(m_lstCityWarHero[index].clan_id);
                    string clanName = clanIndex < m_cityWarManger.CityWarClanNameList.Count ? m_cityWarManger.CityWarClanNameList[clanIndex] : "";
                    grid.SetClanName(clanName);
                }

            }
        }
    }


    private void OnWarInfoGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UICityWarMemberInfoGrid grid = data as UICityWarMemberInfoGrid;
            if (grid == null)
            {
                return;
            }
        }
    }


    #endregion


    #region 图腾


    private void OnTotemGridDataUpdate(UIGridBase data, int index)
    {
        if (null != m_lstCityWarTotem && index < m_lstCityWarTotem.Count)
        {
            UICityWarTotemGrid grid = data as UICityWarTotemGrid;
            if (grid != null)
            {
                grid.SetGridData(m_lstCityWarTotem[index]);
                grid.SetIcon(m_lstCityWarTotem[index].iconName);
                grid.SetHp(m_lstCityWarTotem[index].hp, m_lstTotemMaxHp[index]);

                if (m_lstCityWarTotem[index].clanId != 0)
                {
                    grid.SetClanName(m_lstCityWarTotem[index].clanName);
                    //if (m_clanManger.CityWarClanIdList.Contains(m_lstCityWarTotem[index].clanId))
                    //{
                    //    int clanIndex = m_clanManger.CityWarClanIdList.IndexOf(m_lstCityWarTotem[index].clanId);
                    //    string clanName = clanIndex < m_clanManger.CityWarClanNameList.Count ? m_clanManger.CityWarClanNameList[clanIndex] : "";
                    //    grid.SetClanName(clanName);
                    //}
                }
                else
                {
                    grid.SetClanName("中立");
                }

            }
        }
    }


    private void OnTotemGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UICityWarTotemGrid grid = data as UICityWarTotemGrid;
            if (grid == null)
            {
                return;
            }

            IMapSystem mapSys = ClientGlobal.Instance().GetMapSystem();

            IController ctrl = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl();
            if (ctrl != null && mapSys != null)
            {
                //ctrl.GotoMap(mapSys.GetMapID(), new UnityEngine.Vector3(grid.CityWarTotemData.pos.x, 0, -grid.CityWarTotemData.pos.y));
                ctrl.MoveToTarget(new UnityEngine.Vector3(grid.CityWarTotemData.pos.x, 0, -grid.CityWarTotemData.pos.y));
            }
        }
    }

    void SetTotemGridSelect()
    {

    }

    /// <summary>
    /// 结算
    /// </summary>
    void CityWarFinish()
    {
        m_btn_ExitBtn.gameObject.SetActive(true);
        m_exitTime = 10;
        TimerAxis.Instance().KillTimer(CITYWAREXIT_TIMERID, this);
        TimerAxis.Instance().SetTimer(CITYWAREXIT_TIMERID, 1000, this);
    }

    #endregion



    void onClick_Unlock_close_Btn(GameObject caster)
    {
        CityWarFightingPanel panel = DataManager.Manager<UIPanelManager>().GetPanel<CityWarFightingPanel>(PanelID.CityWarFightingPanel);
        if (panel != null)
        {
            //是否请求刷新数据
            DataManager.Manager<CityWarManager>().ReqCityWarInfo(panel.IsShowCityWar());
        }

        this.HideSelf();
    }

    void onClick_ExitBtn_Btn(GameObject caster)
    {
        DataManager.Manager<ComBatCopyDataManager>().ReqExitCopy();
    }


    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == CITYWAREXIT_TIMERID)
        {
            if (m_exitTime > 0)
            {
                m_exitTime--;
                m_btn_ExitBtn.gameObject.SetActive(true);
                m_label_ExitBtnLabel.text = string.Format("退出（{0}）", m_exitTime);
            }

            if (m_exitTime == 0)
            {
                TimerAxis.Instance().KillTimer(CITYWAREXIT_TIMERID, this);
                m_label_ExitBtnLabel.text = string.Format("退出");
                m_btn_ExitBtn.gameObject.SetActive(false);
            }
        }
    }
}

