using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using table;
using Engine;
using Client;
using GameCmd;
using System.Text;
using Common;

partial class DailyPanel
{
    enum PageInfo 
    {
        None =0,
        Daily = 1,
        Hunting =2,
        TreasureBoss = 3,
        Max = 4,
    }
    public List<string> DailyTabValues = new List<string>()
    {
        "日常活动","限时活动","即将开始","周历",
    };
    List<DailyDataBase> dailyDataList = new List<DailyDataBase>();
    List<DailyDataType> m_listDataType = null;

    List<DailyAwardDataBase> m_lst_RewardList = new List<DailyAwardDataBase>();
    //当前活动的页签
    private uint activeTabId = 0;
    private uint selectDailyId = 0;
    private List<uint> m_lstDailyID = new List<uint>();
    private List<uint> m_lstRewardID = new List<uint>();
    private UIWidget foreground;
    private DailyDataBase curDailyData = null;
    Engine.IRenderSystem rs;
    table.ResourceDataBase rd;
    string path = "";
    UIWidget m_widget = null;

    UILabel a;
    uint selectID;
    TextManager tm
    {
        get
        {
            return DataManager.Manager<TextManager>();
        }
    }
    DailyManager dm
    {
        get
        {
            return DataManager.Manager<DailyManager>();
        }
    }

    List<HuntingDataBase> list = null;

    bool isInSchedule = false; 
    protected override void OnLoading()
    {
        base.OnLoading();
        rs = Engine.RareEngine.Instance().GetRenderSystem();
        if (rs != null)
        {
            rd = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(50003);
            path = rd.strPath;
        }
        m_widget = GetComponent<UIWidget>();

       
         m_listDataType =  new List<DailyDataType>()
        {
            DailyDataType.RiChangHuoDong, DailyDataType.XianShiHuoDong,DailyDataType.JiJiangKaiShi,DailyDataType.ZhouLi,
        };

         m_lst_RewardList = GameTableManager.Instance.GetTableList<DailyAwardDataBase>();
         for (int i = 0; i < m_lst_RewardList.Count; i++)
         {
             if (!m_lstRewardID.Contains(m_lst_RewardList[i].ID))
             {
                 m_lstRewardID.Add(m_lst_RewardList[i].ID);
             }
         }

         UIEventListener.Get(m__Model.gameObject).onDrag = DragModel;
    }
    protected override void OnHide()
    {
        Release();
        m_btn_infoContent.gameObject.SetActive(false);

       
        if (boss_dic != null)
        {
            boss_dic.Clear();
        }   
        RegisterEvent(false);

        selectDailyId = 0;
    }
    #region UIItemRewardGridCreator
    UIGridCreatorBase m_ctor_UIItemRewardCreator;
    List<UIItemRewardData> m_lst_UIItemRewardDatas = new List<UIItemRewardData>();
    void AddCreator(Transform parent)
    {
        if (parent != null)
        {
            m_ctor_UIItemRewardCreator = parent.GetComponent<UIGridCreatorBase>();
            if (m_ctor_UIItemRewardCreator == null)
            {
                m_ctor_UIItemRewardCreator = parent.gameObject.AddComponent<UIGridCreatorBase>();
            }
            m_ctor_UIItemRewardCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            m_ctor_UIItemRewardCreator.gridWidth = 80;
            m_ctor_UIItemRewardCreator.gridHeight = 80;
            m_ctor_UIItemRewardCreator.RefreshCheck();
            m_ctor_UIItemRewardCreator.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateGridData, null);
        }
    }
    void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (grid is UIItemRewardGrid)
        {
            UIItemRewardGrid itemShow = grid as UIItemRewardGrid;
            if (itemShow != null)
            {
                if (index < m_lst_UIItemRewardDatas.Count)
                {
                    UIItemRewardData data = m_lst_UIItemRewardDatas[index];
                    uint itemID = data.itemID;
                    uint num = data.num;
                    itemShow.SetGridData(itemID, num, false,false,false);
                    itemShow.transform.localScale = new Vector3(0.83f, 0.83f, 1.0f);
                }
            }
        }
    }
    #endregion
    void ReleaseBoxList() 
    {
        if (null != boxList)
        {
            Transform trans = null;
            while (boxList.Count > 0)
            {
                trans = boxList[0];
                boxList.Remove(trans);
                if (null == trans)
                    continue;
               // UIManager.OnObjsRelease(trans, (uint)GridID.Uidailyrewardgrid);

            }
        }
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_ctor_TabRoot != null)
        {
            m_ctor_TabRoot.Release(depthRelease);
        }
        if(m_ctor_ContentScrollView != null)
        {
            m_ctor_ContentScrollView.Release(depthRelease);
        }
        if (m_playerAvataCASD != null)
        {
            m_playerAvataCASD.Release(depthRelease);
            m_playerAvataCASD = null;
        }
        if (m_ctor_ListScrollView != null)
        {
            m_ctor_ListScrollView.Release(depthRelease);
        }
        if (m_ctor_HuntingTypeRoot != null)
        {
            m_ctor_HuntingTypeRoot.Release(depthRelease);
        }
        if (m_ctor_TreasureScrollView != null)
        {
            m_ctor_TreasureScrollView.Release(depthRelease);
        }
        if (m_ctor_TreasureTypeRoot != null)
        {
            m_ctor_TreasureTypeRoot.Release(depthRelease);
        }
        if (m_RTObj != null)
        {
            m_RTObj.Release();
            m_RTObj = null;
        }
        if (m__Model != null)
        {
            m__Model.mainTexture = null;
        }
    }


    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        Release();
    }
    void EventCallBack(int nEventID, object param)
    {
        if (nEventID == (int)Client.GameEventID.DAILY_RESALLDATA)
        {
            ChooseRightTab(1);
        }else if (nEventID == (int)Client.GameEventID.DAILY_RESSINGLEDATA)
        {
            RefreshSingleData();
        }else if (nEventID == (int)Client.GameEventID.DAILY_GETREWARDBOXOVER)
        {
            RefreshRewardBox();
        }else if (nEventID == (int)Client.GameEventID.HUNT_CHANGEBOSSSTATE)
        {
            CreateHuntingUIList(selecttype);
        }
        else if (nEventID == (int)Client.GameEventID.UIEVENT_WORLDBOSSSTATUSREFRESH)
        {
            if (null != param && param is uint)
            {
                RefreshBossStatusInfo((uint)param);
            }
        }
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterEvent(true);
        ChooseRightTab(1);
        //dm.ValueUpdateEvent += OnUpdateList;
    }


    void OnUpdateList(object obj, ValueUpdateEventArgs value)
    {
        if (value.key.Equals("OnUpdateTime"))
        {
            long lefttime = (long)value.newValue;
            OnUpdateTime(lefttime);
        }
    }
    void RegisterEvent(bool register) 
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.HUNT_CHANGEBOSSSTATE, EventCallBack);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.DAILY_RESALLDATA, EventCallBack);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.DAILY_RESSINGLEDATA, EventCallBack);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.DAILY_GETREWARDBOXOVER, EventCallBack);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_WORLDBOSSSTATUSREFRESH, EventCallBack);
            
        }
        else 
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.DAILY_RESALLDATA, EventCallBack);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.DAILY_RESSINGLEDATA, EventCallBack);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.DAILY_GETREWARDBOXOVER, EventCallBack);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.HUNT_CHANGEBOSSSTATE, EventCallBack);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_WORLDBOSSSTATUSREFRESH, EventCallBack);
        }
    }
    void ChooseRightTab(int pageid)
    {
        m__Model.gameObject.SetActive(pageid != (int)PageInfo.Daily);
        switch (pageid)
        {
            case (int)PageInfo.Daily:
            
                m_trans_DailyPage.gameObject.SetActive(true);
                m_trans_HuntingPage.gameObject.SetActive(false);
                m_trans_TreasureBossPage.gameObject.SetActive(false);
                if (null != m_ctor_ContentScrollView)
                {
                    m_ctor_ContentScrollView.Initialize<UIDailyGrid>(m_trans_UIDailyGrid.gameObject, OnUpdateMallGridData, OnGridUIEventDlg);
                }
                     
                ShowActiveAward();
                     
                if (null != m_ctor_TabRoot)
                {
                    m_ctor_TabRoot.RefreshCheck();
                    m_ctor_TabRoot.Initialize<UITabGrid>(m_trans_UITabGrid.gameObject, OnUpdateMallGridData, OnDailyTabGridUIEvent);
                    CreateTab();
                }
                SetActiveDailyTab(DailyDataType.RiChangHuoDong);
                   
                break;
            case (int)PageInfo.Hunting:
                    //狩猎
                    boss_dic = DataManager.Manager<HuntingManager>().boss_dic;
                    InitHuntingPanel();
                   
                    
                    list = GameTableManager.Instance.GetTableList<HuntingDataBase>();


                     IPlayer player = MainPlayerHelper.GetMainPlayer();
                    int HuntingOpenLevel = GameTableManager.Instance.GetGlobalConfig<int>("HuntingOpenLevel");
                    if (HuntingOpenLevel <= player.GetProp((int)CreatureProp.Level))
                    {
                        m_trans_DailyPage.gameObject.SetActive(false);
                        m_trans_HuntingPage.gameObject.SetActive(true);
                        m_trans_TreasureBossPage.gameObject.SetActive(false);
                        NetService.Instance.Send(new stReqAllBossRefTimeScriptUserCmd_C());

                        m_ctor_HuntingTypeRoot.CreateGrids(toggleDic.Count);
                        SetActiveTab(1);
                    }
                    else
                    {
                        TipsManager.Instance.ShowTips(string.Format(DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.DailyHuntingOpen_Level, HuntingOpenLevel)));
                        return;
                    }
                   
                break;

            case (int)PageInfo.TreasureBoss:
                        m_trans_DailyPage.gameObject.SetActive(false);
                        m_trans_HuntingPage.gameObject.SetActive(false);
                        m_trans_TreasureBossPage.gameObject.SetActive(true);
                        InitTreasureBossPage();
                break;
        }
    }
    /// <summary>
    /// 商城格子数据刷新
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="index"></param>
    private void OnUpdateMallGridData(UIGridBase grid, int index)
    {
        if (grid is UITabGrid)
        {
            if (index > DailyTabValues.Count)
            {
                Engine.Utility.Log.Error("DailyPanel OnUpdateMallGridData faield,mallTabDic data errir");
                return;
            }
  
            UITabGrid tabGrid = grid as UITabGrid;
            tabGrid.SetName(DailyTabValues[index]);
            tabGrid.SetHightLight(activeTabId == index ? true : false);
            tabGrid.SetGridData(index);
        }
        else if (grid is UIDailyGrid)
        {
            DailyDataBase data = dailyDataList[index];
            if(DataManager.Manager<DailyManager>().ActiveDic.ContainsKey(data.id))
            {
                LivenessData list  = DataManager.Manager<DailyManager>().ActiveDic[data.id];
                if (data != null && list != null)
                {
                    UIDailyGrid dailyGrid = grid as UIDailyGrid;
                    dailyGrid.SetDailyData(data, list);
                    dailyGrid.SetSelect(index == m_lstDailyID.IndexOf(selectDailyId));
                    dailyGrid.onClickDailyGrid = onClickDailyGrid;
                }
            }        
        }
        else if(grid is UIDailyCalendarGrid)
        {
            UIDailyCalendarGrid data = grid as UIDailyCalendarGrid;
            if (data != null)
            {
                data.SetGridData(dailyIDs[index]);
                data.SetBg(todayIsMatch);
            }
        }
        else if (grid is UIDailyRewardGrid)
        {
            UIDailyRewardGrid data = grid as UIDailyRewardGrid;
            if (data != null)
            {
                if (index < m_lstRewardID.Count)
                {
                    data.SetGridData(m_lstRewardID[index]);
                }
                
            }
        }
    }

    /// <summary>
    /// 商城格子UI事件委托
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UITabGrid)
                {
                    UITabGrid tabGrid = data as UITabGrid;
                    if (null != tabGrid)
                    {
                        SetActiveTab(tabGrid.TabID);
                    }
                }
                else if (data is UIDailyGrid)
                {
                    UIDailyGrid dailyGrid = data as UIDailyGrid;
                    if (null != dailyGrid)
                    {
                        SetSelectDailyGrid(dailyGrid.DailyID);
                    }
                }
                break;
        }
    }


    void OnDailyTabGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UITabGrid)
                {
                    UITabGrid g = data as UITabGrid;
                    if (g.Data != null && g.Data is int)
                    {
                        int index = (int)g.Data;
                        if (index < m_listDataType.Count)
                        {
                            SetActiveDailyTab(m_listDataType[index]);
                        }
                        
                    }
                 
                  
                }
                if(data is UIDailyCalendarGrid)
                {
                    UIDailyCalendarGrid grid = data as UIDailyCalendarGrid;
                    ShowDailyActivityInfo(grid);
                }
                break;
        }
    }
    DailyDataType curDailyType = DailyDataType.RiChangHuoDong;
    void SetActiveDailyTab(DailyDataType type)
    {
        if (m_ctor_TabRoot != null)
        {
            UITabGrid tabGrid = m_ctor_TabRoot.GetGrid<UITabGrid>(m_listDataType.IndexOf(curDailyType));
            if (tabGrid != null)
            {
                tabGrid.SetHightLight(false);
            }
            tabGrid = m_ctor_TabRoot.GetGrid<UITabGrid>(m_listDataType.IndexOf(type));
            if (tabGrid != null)
            {
                tabGrid.SetHightLight(true);
            }

            m_trans_DailyNormalRoot.gameObject.SetActive(type != DailyDataType.ZhouLi);
            m_trans_CalendarPage.gameObject.SetActive(type == DailyDataType.ZhouLi);
            curDailyType = type;
            switch (type)
            {
                case DailyDataType.RiChangHuoDong:
                    CreateDailyUIList();
                    break;
                case DailyDataType.XianShiHuoDong:
                    CreateDailyUIList();
                    break;
                case DailyDataType.JiJiangKaiShi:
                    CreateDailyUIList();
                    break;
                case DailyDataType.ZhouLi:
                    InitCalendarCreators();
                    break;
            
            }
        }
        //面板状态改变
        OnPanelStateChanged();
    }
    UIDailyCalendarGrid previous = null;
    void ShowDailyActivityInfo(UIDailyCalendarGrid grid) 
    {
        if (previous != null)
        {
            previous.SetSelect(false);
        }
        grid.SetSelect(true);
        previous = grid;
        ShowCalendarInfo(grid.dailyID);
    }

    public void CreateTab()
    {
        if (null != m_ctor_TabRoot)
        {
            m_ctor_TabRoot.CreateGrids(DailyTabValues.Count);
        }
    }

    private void CreateDailyUIList()
    {
        if (null != m_ctor_ContentScrollView)
        {
            dailyDataList.Clear();
            List<DailyDataBase> tempDaliyList = new List<DailyDataBase>();
            DataManager.Manager<DailyManager>().ExecuteSort();
            IPlayer mp = ClientGlobal.Instance().MainPlayer;
            if (mp == null)
            {
                return;
            }
            int playerLevel = mp.GetProp((int)CreatureProp.Level);
            List<LivenessData> allList = DataManager.Manager<DailyManager>().FormDailyData(curDailyType);
             for (int i = 0; i < allList.Count; i++)
            {
                uint id = allList[i].type;
                table.DailyDataBase data = GameTableManager.Instance.GetTableItem<DailyDataBase>(id);
                if (playerLevel >= data.minLevel)
                {
                    dailyDataList.Add(data);
                }
                else
                {
                    tempDaliyList.Add(data);
                }
            }
            if (tempDaliyList.Count > 0)
            {
                dailyDataList.AddRange(tempDaliyList);
            }
            m_lstDailyID.Clear();
            for (int i = 0; i < dailyDataList.Count;i++ )
            {
                m_lstDailyID.Add(dailyDataList[i].id);
            }

            if (dailyDataList != null)
            {
                m_ctor_ContentScrollView.CreateGrids(dailyDataList.Count);
            }
            if (m_lstDailyID.Count >0 )
            {
                SetSelectDailyGrid(m_lstDailyID[0]);
            }
           
        }
    }

    #region 日常
    /// <summary>
    /// 设置选中数据
    /// </summary>
    /// <param name="mallItemId"></param>
    public void SetSelectDailyGrid(uint DailyID)
    {
        if (this.selectDailyId == DailyID)
            return;
        if (null != m_ctor_ContentScrollView)
        {
            UIDailyGrid grid = m_ctor_ContentScrollView.GetGrid<UIDailyGrid>(m_lstDailyID.IndexOf(selectDailyId));
            if (null != grid)
            {
                grid.SetSelect(false);
            }
            m_ctor_ContentScrollView.FocusGrid(m_lstDailyID.IndexOf(DailyID));
            grid = m_ctor_ContentScrollView.GetGrid<UIDailyGrid>(m_lstDailyID.IndexOf(DailyID));
            if (null != grid)
            {
                grid.SetSelect(true);
            }
           
        }
         selectDailyId = DailyID;
         curDailyData = GameTableManager.Instance.GetTableItem<DailyDataBase>(DailyID);
         ShowDailInfo();
    }
    void OnUpdateTime(long lefttime)
    {
        if (lefttime > 0)
        {
            m_btn_JoinBtn.isEnabled = false;
            m_btn_JoinBtn.GetComponentInChildren<UILabel>().text = DateTimeHelper.ParseTimeSecondsFliter((int)lefttime);
        }
        else 
        {
            m_btn_JoinBtn.isEnabled = true;
            m_btn_JoinBtn.GetComponentInChildren<UILabel>().text = "参加";
        }
    }
    /// <summary>
    /// 显示日常的详细信息
    /// </summary>
    /// <param name="index">索引</param>
    void ShowDailInfo()
    {
         if (null == curDailyData)
         {
             return;
         }
        m_label_Name.text = curDailyData.name;
        m_sprite_Icon.spriteName = curDailyData.icon;
        m_sprite_Icon.MakePixelPerfect();
        uint times = 0;
        uint liveness_num = 0;
        if (dm.ActiveDic.ContainsKey(curDailyData.id))
        {
            times = dm.ActiveDic[curDailyData.id].time;
            liveness_num = dm.ActiveDic[curDailyData.id].liveness_num;
        }
        else 
        {
            Engine.Utility.Log.Error("服务器下发的数据中对应不到表格中id为{0}的数据", curDailyData.id);
        }     
        if (curDailyData.MaxTimes == 0)
        {
            m_label_Times.text = "不限";
        }
        else
        {
            m_label_Times.text = string.Format("{0}/{1}", times, curDailyData.MaxTimes);
        }
        m_label_Active.text = string.Format("{0}/{1}", liveness_num, curDailyData.MaxActive);
        m_label_Require.text = curDailyData.activityType;
        m_label_ActivityTime.text = curDailyData.activityTime;
        m_label_ActivityDesc.text = curDailyData.description;
        string[] items = curDailyData.awardItem.Split(';');
        AddCreator(m_trans_RewardGrid.transform);
        m_lst_UIItemRewardDatas.Clear();
        for (int i = 0; i < items.Length; i++)
        {
            uint itemID = uint.Parse(items[i]);
            m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
            {
                itemID = itemID,
                num = 1,
            });
        }  
        m_ctor_UIItemRewardCreator.CreateGrids(m_lst_UIItemRewardDatas.Count);
    }
    void SetBtnSchedule(DailyDataBase dailyData)
    {
        long leftSeconds = 0;
        isInSchedule = DataManager.Manager<DailyManager>().UpdateDataLeftTime(dailyData, out leftSeconds);
        if (isInSchedule)
        {
            m_btn_JoinBtn.isEnabled = true;
             m_btn_JoinBtn.GetComponentInChildren<UILabel>().text = "参加";
        }
        else
        {
            if (leftSeconds > 0)
            {
                m_btn_JoinBtn.GetComponentInChildren<UILabel>().text = DataManager.Manager<DailyManager>().GetCloserScheduleTimeByID(dailyData.id);
                m_btn_JoinBtn.isEnabled = false;
            }
            else
            {
                m_btn_JoinBtn.GetComponentInChildren<UILabel>().text = "已结束";
                m_btn_JoinBtn.isEnabled = false;
            }

        }   
    }

    void SetDetailOpenBtn(bool open, uint level)
    {
        m_btn_JoinBtn.isEnabled = open;
        m_btn_JoinBtn.GetComponentInChildren<UILabel>().text = string.Format("{0}级开启", level);
    }



    List<Transform> boxList = new List<Transform>();
    void ShowActiveAward()
    {
        if (m_ctor_DailyRewardRoot != null)
        {
            m_ctor_DailyRewardRoot.Initialize<UIDailyRewardGrid>(m_trans_UIDailyRewardGrid.gameObject, OnUpdateMallGridData, null);
            m_ctor_DailyRewardRoot.CreateGrids(m_lstRewardID.Count);
        }
        m_label_MyActive_Num.text = dm.ActiveTotalValue.ToString();

        uint gap = 0;
        int presentNum = 0;

        for (int i = 0; i < m_lst_RewardList.Count;i++ )
        {
            if (dm.ActiveTotalValue >= m_lst_RewardList[i].liveness)
            {
                presentNum = i + 1;
                gap = dm.ActiveTotalValue - m_lst_RewardList[i].liveness;
            }
            else
            {
                if (presentNum == 0)
                {
                    gap = dm.ActiveTotalValue;
                }
            }
        }
        if (presentNum > 0)
         {
             if (presentNum == m_lst_RewardList.Count)
             {
                 m_slider_Activeslider.value = 1.0f;
             }
             else
             {
                 m_slider_Activeslider.value = presentNum * 1.0f / m_lst_RewardList.Count + (gap * 1.0f / (m_lst_RewardList[presentNum].liveness - m_lst_RewardList[presentNum - 1].liveness)) * 0.2f;
             }

         }
         else
         {
             m_slider_Activeslider.value = (gap * 1.0f / m_lst_RewardList[presentNum].liveness) * 0.2f;
         }   
    }


    private List<uint> tempBoxIdList = new List<uint>();
    void onClickDailyGrid(DailyDataBase dailyData, uint DailyID)
    {
        MainPlayStop();

        ExecuteGoto(dailyData);
        SetSelectDailyGrid(DailyID);
       
    }

    void MainPlayStop()
    {
        Client.IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            player.SendMessage(Client.EntityMessage.EntityCommand_StopMove, player.GetPos());
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, false);//关闭自动寻路中
        }
        Controller.CmdManager.Instance().Clear();//清除寻路
    }

    void ExecuteGoto(DailyDataBase dailyData)
    {
        if (null == dailyData)
        {
            return;
        }
        if (dailyData.isClosePanel == 1)
        {
            HideSelf();
        }
        if (dailyData.taskID > 0)
        {
            table.QuestDataBase questDb = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(dailyData.taskID);
            if (questDb != null)
            {
                //如果是魔族任务 找当前在做的魔族任务执行 否则直接执行当前配置任务id
                if (questDb.dwType == (uint)GameCmd.TaskType.TaskType_Demons)
                {
                    List<QuestTraceInfo> lstdata = null;
                    DataManager.Manager<TaskDataManager>().GetAllQuestTraceInfo(out lstdata);                  
                    for (int i = 0; i < lstdata.Count; i++)
                    {
                        if (lstdata[i].taskType != TaskType.TaskType_Demons)
                        {
                            continue;
                        }
                        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DONING,
                                new Client.stDoingTask() { taskid = lstdata[i].taskId });
                        return;
                    }
                }
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DONING,
                       new Client.stDoingTask() { taskid = dailyData.taskID });          
            }
        }
        else if (dailyData.puGuaID > 0)
        {
            List<QuestTraceInfo> lstdata = new List<QuestTraceInfo>();          
            DataManager.Manager<TaskDataManager>().GetAllQuestTraceInfo(out lstdata);
            for (int i = 0; i < lstdata.Count; i++)
            {
                if (lstdata[i].taskType != TaskType.TaskType_Loop)
                {
                    continue;
                }
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DONING,
                        new Client.stDoingTask() { taskid = lstdata[i].taskId });
                return;
            }
            if (dailyData.jumpID > 0)
            {
                ItemManager.DoJump(dailyData.jumpID);
            }
        }
        else if (dailyData.jumpID > 0)
        {
            ItemManager.DoJump(dailyData.jumpID);
        }  
    }

    void RefreshSingleData()
    {
         uint id = DataManager.Manager<DailyManager>().DataID;
         m_ctor_ContentScrollView.UpdateData(m_lstDailyID.IndexOf(id));

         uint boxID = DataManager.Manager<DailyManager>().RewardID;
         m_ctor_DailyRewardRoot.UpdateData(m_lstRewardID.IndexOf(boxID));
    }

    void RefreshRewardBox()
    {
        uint boxID = DataManager.Manager<DailyManager>().RewardID;
        m_ctor_DailyRewardRoot.UpdateData(m_lstRewardID.IndexOf(boxID));
    }

    #endregion


    public override bool OnTogglePanel(int tabType, int pageid)
    {
        ChooseRightTab(pageid);
        OnPanelStateChanged();
        return base.OnTogglePanel(tabType, pageid);
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eShowUI)
        {
            ReturnBackUIMsg showInfo = (ReturnBackUIMsg)param;
            if (showInfo != null)
            {
                if (showInfo.tabs.Length > 0)
                {
                    UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, showInfo.tabs[0]);
                }
            }
        }
        return base.OnMsg(msgid, param);
    }
    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }
        int firstTabData = -1;
        int secondTabData = -1;
        if (null != jumpData.Tabs && jumpData.Tabs.Length > 0)
        {
            firstTabData = jumpData.Tabs[0];
        }
        else
        {
            firstTabData = 1;
        }
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTabData);


        if (null != jumpData.Tabs && jumpData.Tabs.Length > 1)
        {
            if (null != jumpData.Param && jumpData.Param is uint)
            {
                if (jumpData.Tabs[0] == 1)
                {
                    SetActiveDailyTab((DailyDataType)jumpData.Tabs[1]);
                    SetSelectDailyGrid((uint)jumpData.Param);
                }
                else 
                {
                    SetActiveTab(jumpData.Tabs[1], (uint)jumpData.Param);
                }
               
            }
         
        }
       
    }
    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.Data = false;
        return pd;
    }

    #region GuideObjGet (引导对象获取)
    /// <summary>
    /// 获取活动指引对象
    /// </summary>
    /// <param name="activeID"></param>
    /// <returns></returns>
    public GameObject GetDailyGuideTargetObj(uint activeID)
    {
        GameObject targetObj = null;
        if (curDailyType == DailyDataType.RiChangHuoDong)
        {
            if (null != m_lstDailyID && m_lstDailyID.Contains(activeID))
            {
                int index = m_lstDailyID.IndexOf(activeID);
                if (null != m_ctor_ContentScrollView)
                {
                    m_ctor_ContentScrollView.FocusGrid(index);
                    UIDailyGrid targetGrid = m_ctor_ContentScrollView.GetGrid<UIDailyGrid>(index);
                    if (null != targetGrid)
                    {
                        targetObj = targetGrid.GetGuideTargetObj();
                    }
                }
            }
        }
        return targetObj;
    }



    #endregion
}
