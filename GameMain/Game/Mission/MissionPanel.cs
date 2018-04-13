using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Text.RegularExpressions;
//1 主线 支线 日常 活动
public partial class MissionPanel : UIPanelBase
{

    enum MissionPanelPageEnum
    {
        None,
        Page_已接,
        Page_可接,
    }

    enum MissionEnum
    {
        None = 0,
        [System.ComponentModel.Description("主  线")]
        Task_Main,
        [System.ComponentModel.Description("支  线")]
        Task_Sub,
        [System.ComponentModel.Description("日  常")]
        Task_Daily,
        [System.ComponentModel.Description("氏  族")]
        Task_Clan,
        [System.ComponentModel.Description("活  动")]
        Task_Activity,
        [System.ComponentModel.Description("红  名")]
        Task_RedName,

        Max,
    }

    //当前活动的1级标签
    private uint m_uActiveFType = 0;
    //当前活动的2级标签
    private uint m_uActiveStype = 0;
    //一级分类缓存UI组件
    private Dictionary<uint, UITypeGrid> m_dicfTypes = null;
    //二级分类缓存UI组件
    private Dictionary<uint, UISecondTypeGrid> m_dicsType = null;

    GameObject missionTitlePrefab;
    //当前任务
    QuestTraceInfo m_currquest;

    UIXmlRichText richText = null;

    List<QuestTraceInfo> questList = new List<QuestTraceInfo>();
    List<QuestTraceInfo> mainQuest = new List<QuestTraceInfo>();
    List<QuestTraceInfo> branchQuest = new List<QuestTraceInfo>();
    List<QuestTraceInfo> dailyQuest = new List<QuestTraceInfo>();
    List<QuestTraceInfo> clanQuest = new List<QuestTraceInfo>();
    List<QuestTraceInfo> activeQuest = new List<QuestTraceInfo>();
    List<QuestTraceInfo> rednameQuest = new List<QuestTraceInfo>();

    List<UIItem> m_lstItem = new List<UIItem>();

    MissionPanelPageEnum m_CurrPage = MissionPanelPageEnum.None;

    //奖励物品
    UIGridCreatorBase m_rewardItemGridCreator;

    //奖励物品信息
    List<MissionRewardItemInfo> m_lstRewardItemInfo;

    //已接任务
    UISecondTabCreatorBase m_secondsTabAlreadyDoMissionCreator;

    //可接任务
    UISecondTabCreatorBase m_secondsTabCanDoMissionCreator;

    MissionEnum m_selectTaskTypeId = MissionEnum.Task_Main;

    uint m_selectTaskId = 0;

    protected override void OnLoading()
    {
        missionTitlePrefab = m_sprite_missionTile.gameObject;
        richText = m_widget_taskDesc.GetComponent<UIXmlRichText>();

        //1级分类
        m_dicfTypes = new Dictionary<uint, UITypeGrid>();
        //2级分类
        m_dicsType = new Dictionary<uint, UISecondTypeGrid>();

        InitWidget();
    }

    protected override void OnHide()
    {
        m_CurrPage = MissionPanelPageEnum.None;
        Release();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_rewardItemGridCreator != null)
        {
            m_rewardItemGridCreator.Release(depthRelease);
        }

    }

    void InitWidget()
    {
        if (m_trans_taskRewardRoot != null)
        {
            m_rewardItemGridCreator = m_trans_taskRewardRoot.gameObject.GetComponent<UIGridCreatorBase>();
            if (m_rewardItemGridCreator == null)
            {
                m_rewardItemGridCreator = m_trans_taskRewardRoot.gameObject.AddComponent<UIGridCreatorBase>();
            }

            //UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uiitemshow) as UnityEngine.GameObject;
            m_rewardItemGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            m_rewardItemGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            m_rewardItemGridCreator.gridWidth = 90;
            m_rewardItemGridCreator.gridHeight = 120;

            m_rewardItemGridCreator.RefreshCheck();
            m_rewardItemGridCreator.Initialize<UIItemShow>((uint)GridID.Uiitemshow, UIManager.OnObjsCreate, UIManager.OnObjsRelease, OnGridDataUpdate, OnGridUIEvent);
        }

        //已接任务
        if (m_scrollview_alreadyDoMission != null)
        {
            m_secondsTabAlreadyDoMissionCreator = m_scrollview_alreadyDoMission.gameObject.GetComponent<UISecondTabCreatorBase>();
            if (m_secondsTabAlreadyDoMissionCreator == null)
            {
                m_secondsTabAlreadyDoMissionCreator = m_scrollview_alreadyDoMission.gameObject.AddComponent<UISecondTabCreatorBase>();
                GameObject cloneFTemp = UIManager.GetResGameObj(GridID.Uictrtypegrid) as GameObject;
                GameObject cloneSTemp = UIManager.GetResGameObj(GridID.Uisecondtypegrid) as GameObject;

                m_secondsTabAlreadyDoMissionCreator.Initialize<UISecondTypeGrid>(cloneFTemp, cloneSTemp, OnUpdateMissionCtrTypeGrid, OnUpdateMissionSecondGrid, OnMissionCtrTypeGridUIEventDlg);
            }
        }

        //可接任务
        if (m_scrollview_canDoMission != null)
        {
            m_secondsTabCanDoMissionCreator = m_scrollview_canDoMission.gameObject.GetComponent<UISecondTabCreatorBase>();
            if (m_secondsTabCanDoMissionCreator == null)
            {
                m_secondsTabCanDoMissionCreator = m_scrollview_canDoMission.gameObject.AddComponent<UISecondTabCreatorBase>();
                GameObject cloneFTemp = UIManager.GetResGameObj(GridID.Uictrtypegrid) as GameObject;
                GameObject cloneSTemp = UIManager.GetResGameObj(GridID.Uisecondtypegrid) as GameObject;

                m_secondsTabCanDoMissionCreator.Initialize<UISecondTypeGrid>(cloneFTemp, cloneSTemp, OnUpdateMissionCtrTypeGrid, OnUpdateMissionSecondGrid, OnMissionCtrTypeGridUIEventDlg);
            }
        }

    }

    private void OnGridDataUpdate(UIGridBase gridData, int index)
    {
        if (m_lstRewardItemInfo != null && m_lstRewardItemInfo.Count > index)
        {
            UIItemShow grid = gridData as UIItemShow;
            if (gridData == null)
            {
                return;
            }

            grid.SetGridData(m_lstRewardItemInfo[index]);
        }

    }

    private void OnGridUIEvent(UIEventType eventType, object data, object param)
    {

    }

    #region GET Grid
    public T GetTypeGrid<T>(bool firstType) where T : UIGridBase
    {
        T target = default(T);
        GameObject obj = null;
        GameObject resObj = null;
        if (firstType)
        {
            resObj = UIManager.GetResGameObj(GridID.Uitypegrid) as GameObject;
        }
        else
        {
            resObj = UIManager.GetResGameObj(GridID.Uisecondtypegrid) as GameObject;
        }
        if (null != resObj)
        {
            obj = Instantiate(resObj) as GameObject;
            if (null != obj)
            {
                target = obj.GetComponent<T>();
                if (null == target)
                {
                    target = obj.gameObject.AddComponent<T>();
                }
                UIDragScrollView scrollView = obj.GetComponent<UIDragScrollView>();
                if (null == scrollView)
                    scrollView = obj.AddComponent<UIDragScrollView>();
            }
        }
        return target;
    }

    UITypeGrid GetFirstTypeGrid()
    {
        UITypeGrid grid = null;
        grid = GetTypeGrid<UITypeGrid>(true);
        grid.gameObject.SetActive(false);

        return grid;
    }
    UISecondTypeGrid GetSecondTypeGrid()
    {
        UISecondTypeGrid grid = null;
        grid = GetTypeGrid<UISecondTypeGrid>(false);
        return grid;
    }
    #endregion
    /// <summary>
    /// 1
    /// </summary>
    void CacheGrids()
    {
        m__missonRoot.transform.DestroyChildren();
        m_dicsType.Clear();
        m_dicfTypes.Clear();
    }

    /// <summary>
    /// 2
    /// </summary>
    /// <param name="breceived"></param>
    private void RefreshData(bool breceived)
    {
        questList.Clear();
        mainQuest.Clear();
        branchQuest.Clear();
        dailyQuest.Clear();
        clanQuest.Clear();
        rednameQuest.Clear();

        QuestTranceManager.Instance.GetQuestTraceInfoList(breceived, ref questList);

        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].taskType == GameCmd.TaskType.TaskType_Normal)
            {
                mainQuest.Add(questList[i]);
            }
            else if (questList[i].taskType == GameCmd.TaskType.TaskType_Sub)
            {
                branchQuest.Add(questList[i]);
            }
            else if (questList[i].taskType == GameCmd.TaskType.TaskType_Loop ||
                questList[i].taskType == GameCmd.TaskType.TaskType_Token ||
                questList[i].taskType == GameCmd.TaskType.TaskType_Demons)
            {
                dailyQuest.Add(questList[i]);
            }
            else if (questList[i].taskType == GameCmd.TaskType.TaskType_Clan)
            {
                clanQuest.Add(questList[i]);
            }
            else if (questList[i].taskType == GameCmd.TaskType.TaskType_RedName)
            {
                rednameQuest.Add(questList[i]);
            }
        }

    }

    /// <summary>
    /// 3
    /// </summary>
    private void UpdateGrids()
    {
        return;

        m_dicfTypes.Clear();
        m_dicsType.Clear();
        m__missonRoot.gameObject.SetActive(false);
        //1级分类
        UITypeGrid tGridTemp = null;
        //2级别分类
        UISecondTypeGrid sGridTemp = null;
        List<QuestTraceInfo> taskList = null;
        for (int i = (int)MissionEnum.None + 1; i < (int)MissionEnum.Max; i++)
        {
            MissionEnum missionType = (MissionEnum)i;
            uint index = (uint)i;
            taskList = GetQuestListByType(missionType);
            if (taskList.Count <= 0)
            {
                continue;
            }

            tGridTemp = GetFirstTypeGrid();
            if (null == tGridTemp)
            {
                break;
            }
            tGridTemp.gameObject.SetActive(true);
            tGridTemp.gameObject.name = missionType.ToString();
            tGridTemp.transform.parent = m__missonRoot.transform;
            tGridTemp.transform.localPosition = new Vector3(0, -100, 0);
            tGridTemp.transform.localScale = Vector3.one;
            tGridTemp.transform.localRotation = Quaternion.identity;
            tGridTemp.SetGridData(index);

            tGridTemp.EnableRedPoint(false);
            tGridTemp.SetName(missionType.GetEnumDescription());
            tGridTemp.EnableArrow(true);
            tGridTemp.SetHightLight(false);
            tGridTemp.RegisterUIEventDelegate(OnGridEventDlg);
            m_dicfTypes.Add(index, tGridTemp);

            for (int n = 0; n < taskList.Count; n++)
            {
                sGridTemp = GetSecondTypeGrid();
                if (sGridTemp == null)
                {
                    continue;
                }
                sGridTemp.gameObject.SetActive(true);
                sGridTemp.SetRedPoint(false);
                sGridTemp.gameObject.name = taskList[n].taskId.ToString();
                uint sIndex = taskList[n].taskId;
                sGridTemp.SetData(sIndex, QuestTraceInfo.GetTableTaskByID(taskList[n].taskId).strName, false);
                sGridTemp.RegisterUIEventDelegate(OnGridEventDlg);
                //添加到上级页签
                tGridTemp.Add(sGridTemp.CacheTransform);
                if (!m_dicsType.ContainsKey(sIndex))
                {
                    m_dicsType.Add(sIndex, sGridTemp);
                }
            }
            tGridTemp.UpdatePostion();
        }

        m__missonRoot.Reposition();
        m__missonRoot.transform.parent.GetComponent<UIScrollView>().ResetPosition();
    }

    List<MissionEnum> taskTypeList = new List<MissionEnum>();
    /// <summary>
    /// 创建左侧grid
    /// </summary>
    void CreateMissionGrid(MissionPanelPageEnum pageEnum)
    {
        taskTypeList.Clear();

        List<QuestTraceInfo> taskList = null;

        List<int> list = new List<int>();

        for (int i = (int)MissionEnum.None + 1; i < (int)MissionEnum.Max; i++)
        {
            MissionEnum missionType = (MissionEnum)i;
            uint index = (uint)i;
            taskList = GetQuestListByType(missionType);
            if (taskList != null && taskList.Count > 0)
            {
                taskTypeList.Add(missionType);
                list.Add(taskList.Count);
            }
        }

        if (pageEnum == MissionPanelPageEnum.Page_已接)
        {
            m_scrollview_alreadyDoMission.gameObject.SetActive(true);
            m_scrollview_canDoMission.gameObject.SetActive(false);
            m_secondsTabAlreadyDoMissionCreator.CreateGrids(list);
        }
        else if (pageEnum == MissionPanelPageEnum.Page_可接)
        {
            m_scrollview_alreadyDoMission.gameObject.SetActive(false);
            m_scrollview_canDoMission.gameObject.SetActive(true);
            m_secondsTabCanDoMissionCreator.CreateGrids(list);
        }

    }

    /// <summary>
    /// 跟新一级页签数据
    /// </summary>
    /// <param name="gridBase"></param>
    /// <param name="index"></param>
    private void OnUpdateMissionCtrTypeGrid(UIGridBase gridBase, int index)
    {
        if (taskTypeList != null && taskTypeList.Count > index)
        {
            UICtrTypeGrid grid = gridBase as UICtrTypeGrid;
            if (grid == null)
            {
                return;
            }

            List<QuestTraceInfo> taskList = GetQuestListByType(taskTypeList[index]);

            //数据
            grid.SetData(taskTypeList[index], taskTypeList[index].GetEnumDescription(), taskList != null ? taskList.Count : 0);
            grid.SetRedPointStatus(false);
        }
    }

    /// <summary>
    /// 更新二级页签数据
    /// </summary>
    /// <param name="gridBase"></param>
    /// <param name="id"></param>
    /// <param name="index"></param>
    private void OnUpdateMissionSecondGrid(UIGridBase gridBase, object id, int index)
    {
        UISecondTypeGrid grid = gridBase as UISecondTypeGrid;
        if (grid == null)
        {
            return;
        }

        MissionEnum missionType = (MissionEnum)id;

        List<QuestTraceInfo> taskList = GetQuestListByType(missionType);
        if (taskList == null)
        {
            return;
        }

        if (taskList.Count > index)
        {
            string name = QuestTraceInfo.GetTableTaskByID(taskList[index].taskId).strName;
            grid.SetData(taskList[index].taskId, name, false);

            grid.SetRedPoint(false);
        }

    }

    /// <summary>
    /// grid事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnMissionCtrTypeGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            //一级页签
            if (data is UICtrTypeGrid)
            {
                UICtrTypeGrid grid = data as UICtrTypeGrid;
                if (grid == null)
                {
                    return;
                }

                SetSelectCtrTypeGrid((MissionEnum)grid.ID);

                ////默认选中第一个
                //List<QuestTraceInfo> taskList = GetQuestListByType(m_selectTaskTypeId);
                //if (taskList != null && taskList.Count > 0)
                //{
                //    SetSelectSecondTypeGrid(taskList[0].taskId);
                //}

            }

            //二级页签
            if (data is UISecondTypeGrid)
            {
                UISecondTypeGrid grid = data as UISecondTypeGrid;
                if (grid == null)
                {
                    return;
                }

                SetSelectSecondTypeGrid(grid.Data);
            }
        }
    }

    void SetSelectCtrTypeGrid(MissionEnum id)
    {

        UISecondTabCreatorBase creator;
        if (m_CurrPage == MissionPanelPageEnum.Page_已接)
        {
            creator = m_secondsTabAlreadyDoMissionCreator;
        }
        else
        {
            creator = m_secondsTabCanDoMissionCreator;
        }

        if (id == m_selectTaskTypeId && taskTypeList.Contains(id))
        {
            if (creator.IsOpen(this.taskTypeList.IndexOf(id)))
            {
                creator.Close(this.taskTypeList.IndexOf(id), true);
                return;
            }
        }

        if (taskTypeList == null)
        {
            return;
        }
        if (taskTypeList.Contains(m_selectTaskTypeId))
        {
            if (creator.IsOpen(taskTypeList.IndexOf(m_selectTaskTypeId)))
            {
                creator.Close(taskTypeList.IndexOf(m_selectTaskTypeId), true);
            }
        }

        if (taskTypeList.Contains(id))
        {
            creator.Open(taskTypeList.IndexOf(id), false);
        }

        this.m_selectTaskTypeId = id;
    }

    void SetSelectSecondTypeGrid(uint id)
    {
        UISecondTabCreatorBase creator = GetSecondTabCreator();
        if (creator == null)
        {
            return;
        }

        List<QuestTraceInfo> taskList = GetQuestListByType(m_selectTaskTypeId);
        if (taskList == null)
        {
            return;
        }

        QuestTraceInfo quest = taskList.Find((data) => { return data.taskId == m_selectTaskId; });
        if (quest != null && taskTypeList.Contains(m_selectTaskTypeId))
        {
            UISecondTypeGrid grid = creator.GetGrid<UISecondTypeGrid>(taskTypeList.IndexOf(m_selectTaskTypeId), taskList.IndexOf(quest));
            if (grid != null)
            {
                grid.SetHightLight(false);
            }
        }

        QuestTraceInfo q = taskList.Find((data) => { return data.taskId == id; });
        if (q != null && taskTypeList.Contains(m_selectTaskTypeId))
        {
            UISecondTypeGrid grid = creator.GetGrid<UISecondTypeGrid>(taskTypeList.IndexOf(m_selectTaskTypeId), taskList.IndexOf(q));
            if (grid != null)
            {
                grid.SetHightLight(true);
                ShowTaskInfo(q);
                this.m_selectTaskId = id;
            }
        }
    }

    UISecondTabCreatorBase GetSecondTabCreator()
    {
        UISecondTabCreatorBase creator;
        if (m_CurrPage == MissionPanelPageEnum.Page_已接)
        {
            creator = m_secondsTabAlreadyDoMissionCreator;
        }
        else
        {
            creator = m_secondsTabCanDoMissionCreator;
        }

        return creator;
    }


    protected override void OnJump(PanelJumpData jumpData)
    {
        base.OnJump(jumpData);

        int firstTab = -1;
        if (jumpData == null)
        {
            jumpData = new PanelJumpData();
        }
        if (firstTab == -1)
        {
            firstTab = (null != jumpData.Tabs && jumpData.Tabs.Length >= 1) ? jumpData.Tabs[0] : 1;
        }
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTab);
    }

    public override bool OnTogglePanel(int tabType, int pageid)
    {
        if (tabType == 1)
        {
            if (pageid == (int)m_CurrPage)
            {
                return true;
            }

            m_CurrPage = (MissionPanelPageEnum)pageid;
            CacheGrids();
            if (m_CurrPage == MissionPanelPageEnum.Page_已接)
            {
                RefreshData(true);
                //UpdateGrids();


                CreateMissionGrid(m_CurrPage);

                //默认选中第一个
                if (taskTypeList != null && taskTypeList.Count > 0)
                {
                    SetSelectCtrTypeGrid(taskTypeList[0]);
                }

                List<QuestTraceInfo> taskList = GetQuestListByType(m_selectTaskTypeId);
                if (taskList != null && taskList.Count > 0)
                {
                    SetSelectSecondTypeGrid(taskList[0].taskId);
                }
            }
            else if (m_CurrPage == MissionPanelPageEnum.Page_可接)
            {
                RefreshData(false);
                //UpdateGrids();

                CreateMissionGrid(m_CurrPage);

                //默认选中第一个
                if (taskTypeList != null && taskTypeList.Count > 0)
                {
                    SetSelectCtrTypeGrid(taskTypeList[0]);
                }

                List<QuestTraceInfo> taskList = GetQuestListByType(m_selectTaskTypeId);
                if (taskList != null && taskList.Count > 0)
                {
                    SetSelectSecondTypeGrid(taskList[0].taskId);
                }
            }

            for (uint i = (uint)MissionEnum.None + 1; i < (uint)MissionEnum.Max; i++)
            {
                if (m_dicfTypes.ContainsKey(i))
                {
                    SetSelectFirstType(i, true);
                    break;
                }
            }
            m__missonRoot.gameObject.SetActive(true);
            m__missonRoot.GetComponent<UITable>().repositionNow = true;

        }
        return base.OnTogglePanel(tabType, pageid);
    }

    private void OnGridEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UITypeGrid)
                    {
                        UITypeGrid tGrid = data as UITypeGrid;
                        SetSelectFirstType(tGrid.Data);
                    }
                    else if (data is UISecondTypeGrid)
                    {
                        UISecondTypeGrid sGrid = data as UISecondTypeGrid;
                        MissionEnum missionType = (MissionEnum)m_uActiveFType;
                        List<QuestTraceInfo> taskList = GetQuestListByType(missionType);

                        if (taskList != null && taskList.Count > 0)
                        {
                            SetSelectSecondType(taskList.Find(P => P.taskId == sGrid.Data), false);
                        }

                    }

                }
                break;

        }
    }

    private List<QuestTraceInfo> GetQuestListByType(MissionEnum missionType)
    {
        List<QuestTraceInfo> taskList = null;
        if (missionType == MissionEnum.Task_Main)
        {
            taskList = mainQuest;
        }
        else if (missionType == MissionEnum.Task_Sub)
        {
            taskList = branchQuest;
        }
        else if (missionType == MissionEnum.Task_Daily)
        {
            taskList = dailyQuest;
        }
        else if (missionType == MissionEnum.Task_Activity)
        {
            taskList = activeQuest;
        }
        else if (missionType == MissionEnum.Task_Clan)
        {
            taskList = clanQuest;
        }
        else if (missionType == MissionEnum.Task_RedName)
        {
            taskList = rednameQuest;
        }

        return taskList;
    }
    /// <summary>
    /// 设置选中第一分页
    /// </summary>
    /// <param name="type"></param>
    private void SetSelectFirstType(uint type, bool force = false)
    {
        if (m_uActiveFType == type && !force)
        {
            if (m_dicfTypes.ContainsKey(m_uActiveFType))
            {
                m_dicfTypes[m_uActiveFType].PlayTween(AnimationOrTween.Direction.Toggle);
            }
            return;
        }
        UITypeGrid tGrid = (m_dicfTypes.ContainsKey(m_uActiveFType)) ? m_dicfTypes[m_uActiveFType] : null;
        if (null != tGrid)
        {
            tGrid.PlayTween(AnimationOrTween.Direction.Forward, false);
        }
        m_uActiveFType = type;
        tGrid = (m_dicfTypes.ContainsKey(m_uActiveFType)) ? m_dicfTypes[m_uActiveFType] : null;
        if (null != tGrid)
        {
            tGrid.PlayTween(AnimationOrTween.Direction.Forward, true);
        }

        if (tGrid.ChildCount > 0)
        {
            MissionEnum missionType = (MissionEnum)type;
            List<QuestTraceInfo> taskList = GetQuestListByType(missionType);
            if (taskList != null && taskList.Count > 0)
            {
                SetSelectSecondType(taskList[0], true);
            }
        }
    }

    /// <summary>
    /// 设置选中二级分页
    /// </summary>
    /// <param name="type"></param>
    /// <param name="force"></param>
    private void SetSelectSecondType(QuestTraceInfo quest, bool force = false)
    {
        if (quest == null)
        {
            return;
        }
        if (m_uActiveStype == quest.taskId && !force)
            return;
        UISecondTypeGrid sGrid = (m_dicsType.ContainsKey(m_uActiveStype)) ? m_dicsType[m_uActiveStype] : null;
        if (null != sGrid)
        {
            sGrid.SetHightLight(false);
        }
        m_uActiveStype = quest.taskId;
        sGrid = (m_dicsType.ContainsKey(m_uActiveStype)) ? m_dicsType[m_uActiveStype] : null;
        if (null != sGrid)
        {
            sGrid.SetHightLight(true);
        }
        ShowTaskInfo(quest);
    }
    QuestTraceInfo GetQuestTraceInfo(uint taskid)
    {
        return questList.Find(P => P.taskId == taskid);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        TaskDataManager taskdata = DataManager.Manager<TaskDataManager>();

        List<QuestTraceInfo> list = new List<QuestTraceInfo>();
        taskdata.GetCanReceiveQuest(ref list);

        if (dicUITabGrid.ContainsKey(UIPanelBase.FisrstTabsIndex))
        {
            if (dicUITabGrid[UIPanelBase.FisrstTabsIndex].ContainsKey(2))
            {
                dicUITabGrid[UIPanelBase.FisrstTabsIndex][2].gameObject.SetActive(list.Count > 0);
            }
        }
    }


    void onClick_BtnGiveUp_Btn(GameObject caster)
    {
        Action OK = delegate
        {
            if (m_currquest == null) return;
            Engine.Utility.Log.Trace("删除任务：{0}", m_currquest.taskId);
            table.QuestDataBase quest = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(m_currquest.taskId);
            if (quest != null)
            {
                if (quest.dwType == (uint)GameCmd.TaskType.TaskType_Token)
                {
                    if (DataManager.Manager<TaskDataManager>().RewardMisssionData.receiveReward != null)
                    {
                        NetService.Instance.Send(new GameCmd.stAcceptTokenTaskGiveupScriptUserCmd_C()
                        {
                            tokentaskid = DataManager.Manager<TaskDataManager>().RewardMisssionData.receiveReward.id,
                            //userid = Client.ClientGlobal.Instance().MainPlayer.GetID(),
                        });
                    }
                }
                else
                {
                    NetService.Instance.Send(new GameCmd.stDelRoleTaskScriptUserCmd_CS() { dwTaskID = m_currquest.taskId });
                }
            }

        };

        Action Cancel = delegate
        {

        };
        string des = "放弃任务？";
        TipsManager.Instance.ShowTipWindow(0, 10, Client.TipWindowType.CancelOk, des, OK, Cancel);


    }

    void onClick_BtnGoOn_Btn(GameObject caster)
    {
        onClick_Close_Btn(null);

        if (m_currquest == null) return;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DONING,
                new Client.stDoingTask() { taskid = m_currquest.taskId });

        //修改字符串成URL
        //         if (m_currquest.taskType == GameCmd.TaskType.TaskType_Token)
        //         {
        //             Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DONING,
        //                 new Client.stDoingTask() { taskid = m_currquest.taskId });
        // 
        //             table.QuestDataBase taskDb = QuestTraceInfo.GetTableTaskByID(m_currquest.taskId);
        //             if (taskDb != null)
        //             {
        //                 string url = taskDb.strTaskReceive.Substring(taskDb.strTaskReceive.IndexOf("<goto"));
        //                 url = url.Substring(0, url.IndexOf("/>") + 2).Replace("goto", "mgs");
        //                 DataManager.Manager<TaskDataManager>().OnUrlClick(url, true);
        //             }
        //         }
        //         else
        //         {
        //             string url = m_currquest.strDesc.Substring(m_currquest.strDesc.IndexOf("<goto"));
        //             url = url.Substring(0, url.IndexOf("/>") + 2).Replace("goto", "mgs");
        //             DataManager.Manager<TaskDataManager>().OnUrlClick(url, true);
        //         }

    }

    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void OnClickTaskItem(GameObject go)
    {
        int pos = go.name.IndexOf('_');
        uint missionType = uint.Parse(go.name.Substring(0, pos));
        uint missionId = uint.Parse(go.name.Substring(pos + 1));

        //TODO 刷新任务详情界面
        ShowTaskInfo(GetQuestTraceInfo(missionId));
    }

    void ShowTaskInfo(QuestTraceInfo quest)
    {
        if (quest == null)
        {
            m_label_taskName.text = "";
            richText.Clear();
            m_trans_detailInfo.gameObject.SetActive(false);
            return;
        }
        m_trans_detailInfo.gameObject.SetActive(true);



        if (m_CurrPage != MissionPanelPageEnum.Page_已接)
        {
            m_btn_btnGiveUp.gameObject.SetActive(false);
        }
        else
        {
            if (quest.taskSubType == TaskSubType.Guild)
            {
                m_btn_btnGiveUp.gameObject.SetActive(false);
            }
            else
            {
                m_btn_btnGiveUp.gameObject.SetActive(quest.taskType != GameCmd.TaskType.TaskType_Normal);
            }
        }



        //暂时因为版本问题关闭氏族的放弃按钮
        //if (quest.taskType == GameCmd.TaskType.TaskType_Clan)
        //{
        //    m_btn_btnGiveUp.gameObject.SetActive(false);
        //}
       


        m_label_taskReward.gameObject.SetActive(quest.taskType != GameCmd.TaskType.TaskType_Token);
        m_currquest = quest;

        m_label_taskName.text = quest.QuestTable.strName;

        richText.Clear();
        string desc = string.Format("<size value=\"24\"><color value=\"#1c2850\">{0} </color></size>", quest.strIntro);

        richText.AddXml(RichXmlHelper.RichXmlAdapt(desc));

        if (quest.Items.Count <= 0) //请求任务奖励
        {
            NetService.Instance.Send(new GameCmd.stRequestTaskRewardScriptUserCmd_C() { task_id = quest.taskId });
        }
        else
        {
            m_lstRewardItemInfo = GetRewardInfoList(quest.exp, quest.money, quest.gold, quest.Items, quest.ItemNum);
            m_rewardItemGridCreator.CreateGrids(m_lstRewardItemInfo != null ? m_lstRewardItemInfo.Count : 0);
            //ShowReward(quest.Items, quest.ItemNum, quest.gold, quest.money, quest.exp);
        }
    }

    List<MissionRewardItemInfo> GetRewardInfoList(uint exp, uint money, uint gold, List<uint> itemIds, List<uint> itemNums)
    {
        List<MissionRewardItemInfo> rewardItemInfoList = new List<MissionRewardItemInfo>();

        //经验
        if (exp > 0)
        {
            MissionRewardItemInfo item = new MissionRewardItemInfo() { itemBaseId = MainPlayerHelper.ExpID, num = exp };
            rewardItemInfoList.Add(item);
        }

        //文钱
        if (money > 0)
        {
            MissionRewardItemInfo item = new MissionRewardItemInfo() { itemBaseId = MainPlayerHelper.MoneyTicketID, num = money };
            rewardItemInfoList.Add(item);
        }

        //金币
        if (gold > 0)
        {
            MissionRewardItemInfo item = new MissionRewardItemInfo() { itemBaseId = MainPlayerHelper.GoldID, num = gold };
            rewardItemInfoList.Add(item);
        }

        //其他奖励
        for (int i = 0; i < itemIds.Count; i++)
        {
            if (i < itemNums.Count)
            {
                MissionRewardItemInfo item = new MissionRewardItemInfo() { itemBaseId = itemIds[i], num = itemNums[i] };
                rewardItemInfoList.Add(item);
            }
        }

        return rewardItemInfoList;
    }


    void ShowReward(List<uint> itembaseid, List<uint> itemNum, uint gold, uint money, uint exp)
    {
        foreach (var item in m_lstItem)
        {
            item.Release();
        }
        m_lstItem.Clear();
        ShowMoneyExpLabel(exp, money, gold);
        ShowItems(itembaseid, itemNum);
    }

    void ShowMoneyExpLabel(uint exp, uint money, uint gold)
    {

        UIItem uiitem = DataManager.Manager<UIManager>().GetUICommonItem(MainPlayerHelper.ExpID, exp);
        uiitem.Attach(m_trans_taskRewardRoot);
        uiitem.SetPosition(true, new Vector3(90 * m_lstItem.Count, 0, 0));
        m_lstItem.Add(uiitem);

        if (money > 0)
        {
            uiitem = DataManager.Manager<UIManager>().GetUICommonItem(MainPlayerHelper.MoneyTicketID, money);
            uiitem.Attach(m_trans_taskRewardRoot);
            uiitem.SetPosition(true, new Vector3(90 * m_lstItem.Count, 0, 0));
            m_lstItem.Add(uiitem);
        }
        if (gold > 0)
        {
            uiitem = DataManager.Manager<UIManager>().GetUICommonItem(MainPlayerHelper.GoldID, gold);
            uiitem.Attach(m_trans_taskRewardRoot);
            uiitem.SetPosition(true, new Vector3(90 * m_lstItem.Count, 0, 0));
            m_lstItem.Add(uiitem);
        }
    }

    void ShowItems(List<uint> item_baseid, List<uint> itemsNum)
    {
        for (int i = 0; i < item_baseid.Count && i < itemsNum.Count; ++i)
        {
            uint itembaseid = item_baseid[i];
            uint itemNum = itemsNum[i];
            UIItem item = DataManager.Manager<UIManager>().GetUICommonItem(itembaseid, itemNum);
            if (item != null)
            {
                item.Attach(m_trans_taskRewardRoot);
                item.SetPosition(true, new Vector3(90 * m_lstItem.Count, 0, 0));
                m_lstItem.Add(item);
            }
        }
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        switch (msgid)
        {
            case UIMsgID.eRefreshTaskDesc:
                GameCmd.stTaskRewardScriptUserCmd_S taskIntro = (GameCmd.stTaskRewardScriptUserCmd_S)param;
                if (m_currquest != null && m_currquest.taskId == taskIntro.task_id)
                {
                    m_lstRewardItemInfo = GetRewardInfoList(taskIntro.exp, taskIntro.money, taskIntro.gold, taskIntro.item_base_id, taskIntro.item_num);
                    m_rewardItemGridCreator.CreateGrids(m_lstRewardItemInfo != null ? m_lstRewardItemInfo.Count : 0);
                    //ShowReward(taskIntro.item_base_id, taskIntro.item_num, taskIntro.gold, taskIntro.money, taskIntro.exp);
                }
                break;
            case UIMsgID.eDeleteTask:
                {
                    uint taskid = (uint)param;

                    UISecondTabCreatorBase creator = GetSecondTabCreator();
                    if (creator != null)
                    {
                        //数据删除
                        creator.RemoveIndex((int)m_selectTaskTypeId, (int)taskid);
                    }

                    RefreshData(true);

                    CreateMissionGrid(m_CurrPage);

                    //默认选中第一个
                    if (taskTypeList != null && taskTypeList.Count > 0)
                    {
                        SetSelectCtrTypeGrid(taskTypeList[0]);
                    }

                    List<QuestTraceInfo> taskList = GetQuestListByType(m_selectTaskTypeId);
                    if (taskList != null && taskList.Count > 0)
                    {
                        SetSelectSecondTypeGrid(taskList[0].taskId);
                    }


                    /*
                    UISecondTypeGrid removeGrid = null;
                    if (m_dicsType.TryGetValue(taskid, out removeGrid))
                    {
                        removeGrid.transform.parent = transform;
                        removeGrid.gameObject.SetActive(false);
                        m_dicsType.Remove(taskid);
                    }

                    UITypeGrid grid;
                    if (m_dicfTypes.TryGetValue(m_uActiveFType, out grid))
                    {
                        grid.UpdatePostion();
                        if (grid.ChildCount <= 0)
                        {
                            grid.SetHightLight(false);
                            grid.transform.parent = transform;
                            grid.gameObject.SetActive(false);
                            for (uint i = (uint)MissionEnum.None + 1; i < (uint)MissionEnum.Max; i++)
                            {
                                if (m_dicfTypes.ContainsKey(i))
                                {
                                    SetSelectFirstType(i, true);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (grid.ChildCount > 0)
                            {
                                MissionEnum missionType = (MissionEnum)grid.Data;
                                List<QuestTraceInfo> taskList = GetQuestListByType(missionType);
                                if (taskList != null && taskList.Count > 0)
                                {
                                    SetSelectSecondType(taskList[0], true);
                                }
                            }
                        }
                    }*/
                }
                break;
            default:
                break;
        }
        return true;
    }
}
