using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class ClanTaskPanel
{
    private UIGridCreatorBase m_gridCreator = null;

    List<uint> m_list_taskInfos = new List<uint>();

    #region override
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    //在窗口第一次加载时，调用
    protected override void OnLoading()
    {
        InitWidget();
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    /// <summary>
    /// 界面显示回调
    /// </summary>
    /// <param name="data"></param>
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGlobalUIEvent(true);

        DataManager.Manager<ClanManger>().RequestClanTaskInfos();
        DataManager.Manager<ClanManger>().ReqClanTaskStep();

        InitUI();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {

        return true;
    }

    protected override void OnHide()
    {
        Release();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();

        RegisterGlobalUIEvent(false);
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_gridCreator != null)
        {
            m_gridCreator.Release(depthRelease);
        }
    }


    #endregion

    #region method

    private void RegisterGlobalUIEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANTASKCHANGED, OnGlobalUIEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANTASKCHANGED, OnGlobalUIEventHandler);
        }
    }

    private void OnGlobalUIEventHandler(int eventType, object data)
    {
        if (eventType == (int)Client.GameEventID.UIEVENTCLANTASKCHANGED)
        {
            InitUI();
        }
    }

    void InitUI()
    {
        //阶段目标
        uint step = DataManager.Manager<ClanManger>().ClanStep;

        uint stepNum = 100;
        if (step < DataManager.Manager<ClanManger>().ClanTaskStepNumLst.Count)
        {
            stepNum = DataManager.Manager<ClanManger>().ClanTaskStepNumLst[(int)step];
        }

        m_label_stepDes.text = string.Format("氏族本周第{0}阶段目标，完成每种类型任务各{1}个", step + 1, stepNum);

        //每日完成次数
        uint ClanTaskMaxTimes = DataManager.Manager<ClanManger>().ClanTaskMaxTimes;

        uint TodayFinishTimes = DataManager.Manager<ClanManger>().TodayFinishTimes;

        m_label_times.text = string.Format("本日完成次数：{0}/{1}", TodayFinishTimes, ClanTaskMaxTimes);

        //grid
        CreateGrid();

        //reward
        UpdateRewardBox();
    }

    void CreateGrid()
    {
        m_list_taskInfos.Clear();
        m_list_taskInfos.AddRange(DataManager.Manager<ClanManger>().GetClanTaskIds());
        m_gridCreator.CreateGrids(m_list_taskInfos.Count);
    }

    void InitWidget()
    {
        if (null != m_trans_clantaskscrollview)
        {
            m_gridCreator = m_trans_clantaskscrollview.GetComponent<UIGridCreatorBase>();
            if (m_gridCreator == null)
            {
                m_gridCreator = m_trans_clantaskscrollview.gameObject.AddComponent<UIGridCreatorBase>();
            }

            m_gridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            m_gridCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
            m_gridCreator.gridWidth = 840;
            m_gridCreator.gridHeight = 103;

            //GameObject obj = UIManager.GetResGameObj(GridID.Uiclantaskgrid) as GameObject;
            m_gridCreator.RefreshCheck();
            m_gridCreator.Initialize<UIClanTaskGrid>(m_trans_UIClanTaskGrid.gameObject, OnUpdateUIGrid, OnUIGridEventDlg);
        }
    }


    private void OnUpdateUIGrid(UIGridBase grid, int index)
    {
        if (null != m_list_taskInfos && m_list_taskInfos.Count > index)
        {
            uint id = m_list_taskInfos[index];
            UIClanTaskGrid clanTaskGrid = grid as UIClanTaskGrid;
            if (clanTaskGrid != null)
            {
                ClanManger.ClanQuestInfo info = DataManager.Manager<ClanManger>().GetClanQuestInfo(id);             

                clanTaskGrid.SetGridData(DataManager.Manager<ClanManger>().GetClanQuestInfo(id));
            }
        }
        else
        {
            Engine.Utility.Log.Error("ClanTask OnUpdateUIGrid error");
        }

    }

    private void OnUIGridEventDlg(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
        }
    }

    /// <summary>
    /// 奖励箱子
    /// </summary>
    void UpdateRewardBox()
    {
        for (int i = 0; i < m_trans_BoxRoot.childCount; i++)
        {
            Transform ts = m_trans_BoxRoot.GetChild(i);

            UIClanTaskRewardBoxGrid grid = ts.GetComponent<UIClanTaskRewardBoxGrid>();
            if (grid == null)
            {
                grid = ts.gameObject.AddComponent<UIClanTaskRewardBoxGrid>();
            }

            List<uint> itemIdList = DataManager.Manager<ClanManger>().ClanTaskRewardItemIdList;
            if (i < itemIdList.Count)
            {
                grid.SetGridData(itemIdList[i]);
            }

            uint step = DataManager.Manager<ClanManger>().ClanStep;
            if (i < step)
            {
                grid.SetLock(false);
            }
            else
            {
                grid.SetLock(true);
            }

            grid.RegisterUIEventDelegate(OnGridUIEvent);
        }
    }

    private void OnGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UIClanTaskRewardBoxGrid grid = data as UIClanTaskRewardBoxGrid;

            if (grid != null)
            {
                BaseItem baseItem = new BaseItem(grid.ItemId);

                TipsManager.Instance.ShowItemTips(baseItem);
            }
        }
    }

    #endregion

}

