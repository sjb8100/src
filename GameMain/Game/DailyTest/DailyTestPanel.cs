using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


partial class DailyTestPanel
{
    #region property

    UIGridCreatorBase m_dailyTestGridCreator;

    List<DailyTestInfo> m_lstDailyTestInfo;

    private UIItemInfoGrid m_baseGrid = null;

    private uint m_ItemAddTimes;  //福瑞珠加速倍数

    BaseItem baseItem;

    #endregion


    #region override

    protected override void OnLoading()
    {
        base.OnLoading();

        InitWidget();

        this.m_baseGrid = m_trans_UIItemInfoGrid.GetComponent<UIItemInfoGrid>();
        if (this.m_baseGrid == null)
        {
            this.m_baseGrid = m_trans_UIItemInfoGrid.gameObject.AddComponent<UIItemInfoGrid>();
        }

        this.m_ItemAddTimes = GameTableManager.Instance.GetGlobalConfig<uint>("FuRuiZhuSpeedupTimes");
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        DataManager.Manager<DailyTestManager>().ReqFuruizhuInfo();

        ShowUI();

        CreateGrid();

        RegistEvent(true);
    }


    protected override void OnHide()
    {
        base.OnHide();

        Release();

        RegistEvent(false);
    }


    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eUpdateDailyTest)
        {
            ShowUI();
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

        if (m_dailyTestGridCreator != null)
        {
            m_dailyTestGridCreator.Release(depthRelease);
        }
    }


    #endregion

    #region event
    void RegistEvent(bool b)
    {
        if (b)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnEvent);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnEvent);
        }
    }

    void OnEvent(int eventID, object param)
    {
        if (eventID == (int)Client.GameEventID.UIEVENT_UPDATEITEM)
        {
            InitItem();
        }
    }

    #endregion


    #region method

    void InitWidget()
    {
        //上面的页签
        if (m_trans_testpanel != null)
        {
            m_dailyTestGridCreator = m_trans_testpanel.GetComponent<UIGridCreatorBase>();
            if (m_dailyTestGridCreator == null)
            {
                m_dailyTestGridCreator = m_trans_testpanel.gameObject.AddComponent<UIGridCreatorBase>();
            }

            if (m_dailyTestGridCreator != null)
            {
                //UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uidailytestgrid) as UnityEngine.GameObject;
                m_dailyTestGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
                m_dailyTestGridCreator.gridWidth = 271;
                m_dailyTestGridCreator.gridHeight = 410;
                m_dailyTestGridCreator.RefreshCheck();

                m_dailyTestGridCreator.Initialize<UIDailyTestGrid>(m_trans_UIdailytestgrid.gameObject, OnGridDataUpdate, OnGridUIEvent);
            }
        }
    }

    void ShowUI()
    {
        //高倍经验怪物数量
        UpdateMonsterNum();

        // 福瑞珠按钮
        UpdateFuRuiZhuBtn();

        //初始化item
        InitItem();

        //关tips
        m_trans_tipsContent.gameObject.SetActive(false);
        m_label_tipsContentLbl.text = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.TanHao_Commond_meirishilian);

        //福瑞珠加速倍数
        m_label_ItemAddTimes_label.text = this.m_ItemAddTimes.ToString();

        //福瑞珠加速数量
        m_label_ItemAddNum_label.text = DataManager.Manager<DailyTestManager>().FuRuiZhuSpeedupNum.ToString();
    }

    /// <summary>
    /// //高倍经验怪物数量
    /// </summary>
    void UpdateMonsterNum()
    {
        uint TotleMonsterNum = DataManager.Manager<DailyTestManager>().TotleMonsterNum;
        m_slider_Expslider.value = DataManager.Manager<DailyTestManager>().MonsterNum / (float)TotleMonsterNum;
        m_label_exp_percent.text = string.Format("{0}/{1}", DataManager.Manager<DailyTestManager>().MonsterNum, TotleMonsterNum);
    }

    /// <summary>
    /// 福瑞珠按钮
    /// </summary>
    void UpdateFuRuiZhuBtn()
    {
        m_label_left_Label.text = string.Format("使用福瑞珠({0})", DataManager.Manager<DailyTestManager>().DailyCanUseFuRuiZhuNum);
    }

    void CreateGrid()
    {
        m_lstDailyTestInfo = DataManager.Manager<DailyTestManager>().GetDailyTestInfoList();

        if (m_dailyTestGridCreator != null)
        {
            m_dailyTestGridCreator.CreateGrids(m_lstDailyTestInfo != null ? m_lstDailyTestInfo.Count : 0);
        }
    }

    private void OnGridDataUpdate(UIGridBase data, int index)
    {
        if (data is UIDailyTestGrid)
        {
            if (m_lstDailyTestInfo != null && index < m_lstDailyTestInfo.Count)
            {
                UIDailyTestGrid grid = data as UIDailyTestGrid;
                if (grid != null)
                {
                    grid.SetGridData(m_lstDailyTestInfo[index].id);
                    grid.SetTitleName(m_lstDailyTestInfo[index].name);

                    string expDesc = string.Format("{0}倍经验", m_lstDailyTestInfo[index].expMultiple);
                    grid.SetExpDesc(expDesc);

                    string lvDesc = string.Format("怪物等级：{0}-{1}", m_lstDailyTestInfo[index].lvMin, m_lstDailyTestInfo[index].lvMax);
                    grid.SetLvDesc(lvDesc);

                    grid.SetMark(m_lstDailyTestInfo[index].isRecommend);

                    grid.SetBg(m_lstDailyTestInfo[index].bgId);
                }
            }
        }
    }

    private void OnGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UIDailyTestGrid grid = data as UIDailyTestGrid;
            if (grid != null)
            {
                GoToWindow(grid.Id);

                //关tips
                m_trans_tipsContent.gameObject.SetActive(false);
            }
        }
    }

    void GoToWindow(uint id)
    {
        string des = string.Format("建议组队前往");

        DailyTestDataBase dailyTestDb = GameTableManager.Instance.GetTableItem<DailyTestDataBase>(id);
        if (dailyTestDb == null)
        {
            return;
        }

        Action YES = delegate
        {
            GoToMapAlone(dailyTestDb.JumpId);
        };

        Action NO = delegate
        {
            GoToMapWithTeam(dailyTestDb.TeamTargetId);
        };

        TipsManager.Instance.ShowTipWindow(Client.TipWindowType.YesNO, des, YES, NO, okstr: "单人前往", cancleStr: "组队前往");
    }

    void GoToMapAlone(uint jumpId)
    {
        ItemManager.DoJump(jumpId);
    }

    void GoToMapWithTeam(uint teamTargetId)
    {
        if (DataManager.Manager<TeamDataManager>().IsJoinTeam)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.TeamPanel);

            if (DataManager.Manager<TeamDataManager>().MainPlayerIsLeader())
            {
                DataManager.Manager<TeamDataManager>().ReqMatchActivity(teamTargetId);
            }
        }
        else
        {
            DataManager.Manager<TeamDataManager>().ReqConvenientTeamListByTeamActivityId(teamTargetId);
        }
    }

    void InitItem()
    {
        uint itemId = DataManager.Manager<DailyTestManager>().FuRuiZhuItemId;

        if (itemId == 0)
        {
            return;
        }

        if (m_baseGrid != null)
        {
            if (this.baseItem == null)
            {
                this.baseItem = new BaseItem(itemId);
            }

            m_baseGrid.Reset();
            m_baseGrid.SetBorder(true, this.baseItem.BorderIcon);
            m_baseGrid.SetIcon(true, this.baseItem.Icon);
            int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemId);//道具存量
            m_baseGrid.SetNum(true, itemCount.ToString());

            //获取途径
            if (itemCount < 1)
            {
                m_baseGrid.SetNotEnoughGet(true);
                m_baseGrid.RegisterUIEventDelegate(UIItemInfoEventDelegate);
            }
            else
            {
                m_baseGrid.SetNotEnoughGet(false);
                m_baseGrid.UnRegisterUIEventDelegate();
            }
        }
    }

    /// <summary>
    /// 点击弹出获取item面板
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    void UIItemInfoEventDelegate(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: DataManager.Manager<DailyTestManager>().FuRuiZhuItemId);
        }
    }
    #endregion


    #region click

    /// <summary>
    /// 使用福瑞珠
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_Consume_Btn(GameObject caster)
    {
        if (DataManager.Manager<DailyTestManager>().DailyCanUseFuRuiZhuNum > 0)
        {
            uint itemBaseId = DataManager.Manager<DailyTestManager>().FuRuiZhuItemId;

            List<BaseItem> itemList = DataManager.Manager<ItemManager>().GetItemByBaseId(itemBaseId);

            if (itemList != null && itemList.Count > 0)
            {
                DataManager.Manager<ItemManager>().Use(itemList[0].QWThisID);
            }
            else
            {
                TipsManager.Instance.ShowTips("福瑞珠数量不足");
            }
        }
        else
        {
            TipsManager.Instance.ShowTips("超过每日使用上限");
        }
    }

    /// <summary>
    /// 提示
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_Tips_Btn(GameObject caster)
    {
        if (m_trans_tipsContent.gameObject.activeSelf)
        {
            m_trans_tipsContent.gameObject.SetActive(false);
        }
        else
        {
            m_trans_tipsContent.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// +  获取福瑞珠
    /// </summary>
    /// <param name="caster"></param>
    //void onClick_Btn_add_Btn(GameObject caster)
    //{
    //    uint itemBaseId = DataManager.Manager<DailyTestManager>().FuRuiZhuItemId;

    //    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: itemBaseId);
    //}
    #endregion

}

