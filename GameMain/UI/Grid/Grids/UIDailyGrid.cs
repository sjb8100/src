using UnityEngine;
using System;
using table;
using System.Collections.Generic;
using System.Linq;
using GameCmd;
using Client;
class UIDailyGrid : UIGridBase
{
    private UISprite icon;
    private UILabel name;
    //格子背景
    private UISprite state;
    private UILabel m_label_Times;
    private UILabel m_label_Active;
    private UIButton m_btn_Go;
    private UISprite m_spr_Mask;
    List<LivenessData> activeList;
    UIToggle toggle;
    //商品id

    DailyDataBase dailyData = null;
    public delegate void OnClickDailyGrid(DailyDataBase dailyData, uint DailyID);
    public OnClickDailyGrid onClickDailyGrid;
    public uint DailyID
    {
        set;
        get;
    }

    TextManager tm = DataManager.Manager<TextManager>();
    UILabel btn_go_label;
    long leftSeconds = 0;
    bool isInSchedule = true;

    private GameObject m_guideTargetObj = null;
    protected override void OnAwake()
    {
        base.OnAwake();
        icon = CacheTransform.Find("Content/IconContent/Icon").GetComponent<UISprite>();
        name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        state =CacheTransform.Find("Content/Sign").GetComponent<UISprite>();
        m_label_Times = CacheTransform.Find("Content/Frequency/Frequency_Num").GetComponent<UILabel>();
        m_label_Active = CacheTransform.Find("Content/Active/Active_Num").GetComponent<UILabel>();
        m_btn_Go = CacheTransform.Find("Content/btn").GetComponent<UIButton>();
        toggle = CacheTransform.Find("Toggle").GetComponent<UIToggle>();
        btn_go_label = CacheTransform.Find("Content/btn/Label").GetComponent<UILabel>();
        m_spr_Mask = CacheTransform.Find("Content/mask").GetComponent<UISprite>();
        UIEventListener.Get(m_btn_Go.gameObject).onClick = onClickGoBtn;        
    }


    public override void SetGridData(object data)
    {
        base.SetGridData(data);      
    }

    public void SetDailyData(DailyDataBase data,LivenessData list)
    {
        dailyData = data;
        DailyID = data.id;
        UpdateUI(list);
    }

    public void UpdateUI(LivenessData list)
    {
        if (null == dailyData)
        {
            return;
        }      
        SetName(dailyData.name);
        SetIcon(dailyData.icon);
        SetState(dailyData.recommend);
        isInSchedule = DataManager.Manager<DailyManager>().UpdateDataLeftTime(dailyData, out leftSeconds);
        if (list.time >= dailyData.MaxTimes && dailyData.MaxTimes != 0)
        {
            m_btn_Go.isEnabled = false;
            btn_go_label.text = tm.GetLocalText(LocalTextType.Local_TXT_Notice_Daily_Finish);
        }
        else
        {
            IPlayer player = MainPlayerHelper.GetMainPlayer();
            
            if (player != null && player.GetProp((int)CreatureProp.Level) < dailyData.minLevel)
            {
                SetOpenBtn(false, dailyData.minLevel);
            }
            else
            {
                SetBtnSchedule(dailyData);
            }        
        }
        SetLabelValue(list);
      
     }
    void SetBtnSchedule(DailyDataBase   dailyData) 
    {   
        if (isInSchedule)
        {
            m_btn_Go.isEnabled = true;
            btn_go_label.text = "参加";
        }
        else
        {
            if (leftSeconds > 0)
            {
                btn_go_label.text = DataManager.Manager<DailyManager>().GetCloserScheduleTimeByID(dailyData.id);
                m_btn_Go.isEnabled = false;
            }
            else 
            {
                btn_go_label.text = "已结束";
                m_btn_Go.isEnabled = false;
            }
         
        }
    }
    

    private void SetLabelValue(LivenessData list) 
    {
        bool ShowMask = dailyData.MaxTimes != 0 && list.time >= dailyData.MaxTimes;
        if (dailyData.MaxTimes == 0)
        {
            m_label_Times.text = ColorManager.GetColorString(ColorType.JZRY_Gray, "次数:不限");
        }
        else 
        {
            string text = string.Format("次数:{0}/{1}", list.time, dailyData.MaxTimes);
            if (list.time < dailyData.MaxTimes)
            {           
                m_label_Times.text = ColorManager.GetColorString(ColorType.JZRY_Gray, text);
                  
            }
            else
            {
                m_label_Times.text = ColorManager.GetColorString(ColorType.JZRY_Green, text);               
            }
            
        }
        m_spr_Mask.gameObject.SetActive(ShowMask);
        string text2 = string.Format("活跃:{0}/{1}", list.liveness_num, dailyData.MaxActive);
        if (list.liveness_num < dailyData.MaxActive)
        {
            m_label_Active.text = ColorManager.GetColorString(ColorType.JZRY_Gray, text2);               
        }
        else 
        {
            m_label_Active.text = ColorManager.GetColorString(ColorType.JZRY_Green, text2);            
        }
      
    }
    private void SetOpenBtn(bool open,uint level) 
    {
        m_btn_Go.isEnabled = open;
        btn_go_label.text = string.Format("{0}级开启", level);
    }



    /// <summary>
    /// 名称
    /// </summary>
    /// <param name="name"></param>
    private void SetName(string name)
    {
        if (null != this.name)
            this.name.text = name;
    }

    /// <summary>
    /// 图标
    /// </summary>
    /// <param name="iconName"></param>
    private void SetIcon(string iconName)
    {
        UIManager.GetAtlasAsyn(iconName, ref m_playerAvataCASD, () =>
        {
            if (null != icon)
            {
                icon.atlas = null;
            }
        }, icon);
        
    }
    CMResAsynSeedData<CMAtlas> m_playerAvataCASD = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_playerAvataCASD != null)
        {
            m_playerAvataCASD.Release(depthRelease);
            m_playerAvataCASD = null;
        }
    }
    private void SetState(uint stateValue) 
    {
        state.gameObject.SetActive(true);
        if (stateValue == 1)
        {
            state.spriteName = "jiaobiao_zidi";
            state.GetComponentInChildren<UILabel>().text = "推荐";
        }
        else if (stateValue == 2)
        {
            state.spriteName = "jiaobiao_zidi_lan";
            state.GetComponentInChildren<UILabel>().text = "限时";
        }
        else 
        {
            state.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 设置选中
    /// </summary>
    /// <param name="select"></param>
    public void SetSelect(bool select)
    {
        if (null != toggle)
        {
            toggle.value = select;
        }
    }

    private void onClickGoBtn(GameObject go)
    {
        if (null != onClickDailyGrid)
        {
            onClickDailyGrid(dailyData, DailyID);
        }
    }

    #region GuideTargetObjGet
    public GameObject GetGuideTargetObj()
    {
        if (null != m_btn_Go)
        {
            return m_btn_Go.gameObject;
        }
        return null;
    }
    #endregion
}
