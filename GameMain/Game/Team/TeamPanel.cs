using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
using table;

partial class TeamPanel : UIPanelBase
{

    public enum TeamPanelPageEnum
    {
        None = 0,
        Page_Team = 1,
        Page_Apply = 2,
        Max,
    }

    #region  property

    /// <summary>
    /// 我的队伍gameObj个数
    /// </summary>
    private int myTeamGoCount = 0;

    /// <summary>
    /// 申请列表gameobj个数
    /// </summary>
    private int applyGoCount = 0;

    /// <summary>
    /// 我的队伍gameObj list
    /// </summary>
    private List<GameObject> myTeamGrids = new List<GameObject>();

    /// <summary>
    /// 申请列表gameobj list
    /// </summary>
    private List<GameObject> applyListGrids = new List<GameObject>();

    /// <summary>
    /// 申请页当前index
    /// </summary>
    private int applyPageIndex = 0;           //当前页

    /// <summary>
    /// 已经选中的目标活动
    /// </summary>
    uint m_selectTargetActivityId;

    TeamDataManager TDManager
    {
        get
        {
            return DataManager.Manager<TeamDataManager>();
        }
    }

    /// <summary>
    /// 默认大页签
    /// </summary>
    TeamPanelPageEnum curPageEnum = TeamPanelPageEnum.Page_Team;
    #endregion


    #region override


    protected override void OnLoading()
    {
        base.OnLoading();

        //我的队伍
        myTeamGoCount = m_grid_grid.transform.childCount;
        myTeamGrids = new List<GameObject>();
        for (int i = 0; i < myTeamGoCount; i++)
        {
            myTeamGrids.Add(m_grid_grid.GetChild(i).gameObject);
        }

        //申请列表
        applyGoCount = m_grid_applyListContent.transform.childCount;
        applyListGrids = new List<GameObject>();
        for (int i = 0; i < applyGoCount; i++)
        {
            applyListGrids.Add(m_grid_applyListContent.GetChild(i).gameObject);
        }

    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object data)
    {
        //我的队伍
        UpdateMyTeam();

        //左侧活动目标
        InitTargetActivityUI();
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        
    }


    void ReleaseGrid() 
    {

    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }

        int firstTabData = -1;
        int secondTabData = -1;

        //有跳转数据
        if (null != jumpData.Tabs && jumpData.Tabs.Length > 0)
        {
            firstTabData = jumpData.Tabs[0];
        }
        else //无跳转数据，跳到默认
        {
            firstTabData = (int)TeamPanelPageEnum.Page_Team;
        }

        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTabData);
    }
    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Tabs = new int[1];
        pd.JumpData.Tabs[0] = (int)curPageEnum;
        return pd;
    }

    public override bool OnTogglePanel(int tabType, int pageid)
    {
        if (tabType == 1)
        {
            curPageEnum = (TeamPanelPageEnum)pageid;
            //队伍
            if (curPageEnum == TeamPanelPageEnum.Page_Team)
            {
                OnClickMyTeamBtn();
            }

            //申请
            else if (curPageEnum == TeamPanelPageEnum.Page_Apply)
            {
                OnClickApplyListBtn();
            }
        }

        return base.OnTogglePanel(tabType, pageid);
    }

    //点队伍
    void OnClickMyTeamBtn()
    {
        m_widget_myteamPanel.gameObject.SetActive(true);
        m_widget_applyListPanel.gameObject.SetActive(false);

        //我的队伍列表
        UpdateMyTeamList();
    }

    //点申请
    void OnClickApplyListBtn()
    {
        m_widget_myteamPanel.gameObject.SetActive(false);
        m_widget_applyListPanel.gameObject.SetActive(true);

        //好友申请列表
        TDManager.ReqApplyList();

        //红点提示
        TDManager.HaveNewApplyMember = false;
        UpdateApplyWarning();

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eTeamNewApply, null);
        }
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eUpdateMyTeamList)
        {
            UpdateMyTeam();
        }
        else if (msgid == UIMsgID.eUpdateApplyList)
        {
            UpdateApplyList();
        }
        else if (msgid == UIMsgID.eDisbandTeam)
        {
            CloseTeamPanel();
        }
        else if (msgid == UIMsgID.eTeamNewApply)
        {
            UpdateApplyWarning();
        }
        else if (msgid == UIMsgID.eTeamItemMode)
        {
            //设置拾取模式
            SetTeamItemMode();
        }
        else if (msgid == UIMsgID.eTeamTargetActivity)
        {
            Client.stTeamTargetActivity data = (Client.stTeamTargetActivity)param;
            UpdateTargetActivity(data.teamTargetActivityId);
        }
        else if (msgid == UIMsgID.eTeamMatch)
        {
            SetMatchState();
        }
        else if (msgid == UIMsgID.eTeamCancleMatch)
        {
            SetMatchState();
        }

        return true;
    }

    #endregion


    #region method

    /// <summary>
    /// 我的队伍
    /// </summary>
    void UpdateMyTeam()
    {
        //我的队伍列表
        UpdateMyTeamList();

        //我的队伍里面的按钮
        InitMyTeamPanelBtn();

        //申请列表警告
        UpdateApplyWarning();
    }

    /// <summary>
    /// 红点提示
    /// </summary>
    void UpdateApplyWarning()
    {
        if (m_widget_applyListPanel.gameObject.activeSelf)
        {
            TDManager.HaveNewApplyMember = false;
        }

        UITabGrid tabGrid = null;
        Dictionary<int, UITabGrid> dicTabs = null;
        if (dicUITabGrid.TryGetValue(1, out dicTabs))
        {
            if (dicTabs != null && dicTabs.TryGetValue((int)TeamPanelPageEnum.Page_Apply, out tabGrid))
            {
                tabGrid.SetRedPointStatus(TDManager.HaveNewApplyMember);
            }
        }
    }
    #endregion


    #region 左侧活动目标

    /// <summary>
    /// 活动目标
    /// </summary>
    void InitTargetActivityUI()
    {
        UpdateTargetActivity(TDManager.TeamActivityTargetId);
    }

    /// <summary>
    /// 切换目标和进入副本
    /// </summary>
    void SetEnterFbAndMatchBtn()
    {
        if (TDManager.MainPlayerIsLeader())//队长
        {
            if (m_selectTargetActivityId == 0)
            {
                m_btn_btn_match.isEnabled = false;
                //m_btn_btn_enter.gameObject.SetActive(false);
            }
            else
            {
                m_btn_btn_match.isEnabled = true;
                //m_btn_btn_enter.gameObject.SetActive(true);
            }
        }
        else
        {
            //m_btn_btn_enter.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 跟新活动目标
    /// </summary>
    void UpdateTargetActivity(uint targetActivityId)
    {
        // 活动目标变更
        UpdateActivityLabel(targetActivityId);

        m_selectTargetActivityId = targetActivityId;

        //切换到进入副本
        SetEnterFbAndMatchBtn();

        //队伍匹配状态
        SetMatchState();
    }

    /// <summary>
    /// 活动目标变更
    /// </summary>
    void UpdateActivityLabel(uint activityId)
    {
        TeamActivityDatabase activity = GameTableManager.Instance.GetTableItem<TeamActivityDatabase>(activityId);

        if (activity != null)
        {
            m_label_index_name.text = activity.indexName;
        }

        if (activityId == 0 || activityId == TeamDataManager.wuId)
        {
            m_label_index_name.text = "请选择目标";
        }
    }

    /// <summary>
    /// 设置当前匹配状态
    /// </summary>
    void SetMatchState()
    {
        if (TDManager.IsTeamMatch == true)
        {
            m_btn_btn_match.gameObject.SetActive(false);
            m_btn_btn_cancelMatch.gameObject.SetActive(true);
        }
        else if (TDManager.IsTeamMatch == false)
        {
            m_btn_btn_match.gameObject.SetActive(true);
            m_btn_btn_cancelMatch.gameObject.SetActive(false);
        }


        for (int i = 0; i < myTeamGoCount; i++)
        {
            UIMyTeamGrid grid = myTeamGrids[i].GetComponent<UIMyTeamGrid>();
            if (grid == null)
            {
                continue;
            }

            grid.SetMatchState(TDManager.IsTeamMatch);
        }
    }

    #endregion


    #region 我的队伍列表

    /// <summary>
    /// 我的队伍列表
    /// </summary>
    void UpdateMyTeamList()
    {
        if (m_widget_myteamPanel.gameObject.activeSelf == false) return;

        List<TeamMemberInfo> list = TDManager.TeamMemberList;

        for (int i = 0; i < myTeamGoCount; i++)
        {
            myTeamGrids[i].transform.localPosition = Vector3.zero;
            myTeamGrids[i].transform.localScale = Vector3.one;
            myTeamGrids[i].transform.localRotation = Quaternion.identity;
            myTeamGrids[i].SetActive(true);
            myTeamGrids[i].name = i.ToString();

            UIMyTeamGrid grid = myTeamGrids[i].GetComponent<UIMyTeamGrid>();
            if (grid == null)
                grid = myTeamGrids[i].AddComponent<UIMyTeamGrid>();

            //队员数据
            if (list.Count > i)
            {
                grid.ShowMember(true);
                grid.SetGridData(list[i]);//有数据
                grid.SetName(list[i].name);
                grid.SetLv(list[i].lv);
                grid.SetJob(list[i].job);
                grid.SetLeaderMark(list[i].id);
                grid.SetIcon(list[i].job);
            }
            //没数据
            else
            {
                grid.ShowMember(false);
                grid.SetGridData(null);
            }

            //色沪指匹配状态
            grid.SetMatchState(TDManager.IsTeamMatch);

            grid.RegisterUIEventDelegate(OnMyTeamGridEventDlg);
        }

        m_grid_grid.Reposition();
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnMyTeamGridEventDlg(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UIMyTeamGrid grid = data as UIMyTeamGrid;
            if (grid == null)
            {
                return;
            }

            uint btnIndex = (uint)param;
            //队员操作界面
            if (btnIndex == 1)
            {
                Vector3 position = grid.transform.position;
                MyTeamGridData myTeamGridData = new MyTeamGridData { teamMemberInfo = grid.teamMemberInfo, pos = position };
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.TeamMemberBtnPanel, data: myTeamGridData);
            }
            //打开添加队员界面
            if (btnIndex == 2)
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.TeamInvitePanel);//打开添加队员界面
            }
        }
    }

    /// <summary>
    /// 我的队伍里面的一些按钮
    /// </summary>
    void InitMyTeamPanelBtn()
    {
        //队长
        if (TDManager.MainPlayerIsLeader())
        {
            m_btn_btn_disbandteam.gameObject.SetActive(true);
            m_btn_btn_pickup.enabled = true;
            if (TDManager.TeamActivityTargetId == 0)
            {
                //m_btn_btn_enter.gameObject.SetActive(false);
            }
            else
            {
                //m_btn_btn_enter.gameObject.SetActive(true);
            }
        }
        //队员
        else
        {
            m_btn_btn_disbandteam.gameObject.SetActive(false);
            m_btn_btn_match.gameObject.SetActive(false);
            m_btn_btn_cancelMatch.gameObject.SetActive(false);
            m_btn_btn_pickup.enabled = false;

            //m_btn_btn_enter.gameObject.SetActive(false);
        }

        //设置拾取模式
        SetTeamItemMode();
    }

    /// <summary>
    /// 设置拾取模式
    /// </summary>
    void SetTeamItemMode()
    {
        //自由模式
        if (TDManager.TeamItemMode == GameCmd.TeamItemMode.TeamItemMode_Free)
        {
            m_label_itemModeLbl.text = "自由拾取";
        }

        //队长模式
        if (TDManager.TeamItemMode == GameCmd.TeamItemMode.TeamItemMode_Leader)
        {
            m_label_itemModeLbl.text = "队长拾取";
        }
    }

    #endregion


    #region 申请列表


    /// <summary>
    /// 申请列表
    /// </summary>
    void UpdateApplyList()
    {
        if (m_widget_applyListPanel.gameObject.activeSelf == false) return;

        List<TeamMemberInfo> list = GetCurrentPageApplyList();

        for (int i = 0; i < applyListGrids.Count; i++)
        {
            if (i < list.Count)
            {
                applyListGrids[i].SetActive(true);
                UIApplyListGrid grid = applyListGrids[i].GetComponent<UIApplyListGrid>();
                if (grid == null)
                    grid = applyListGrids[i].AddComponent<UIApplyListGrid>();

                grid.SetGridData(list[i]);
                grid.SetName(list[i].name);
                grid.SetLevel(list[i].lv);
                grid.setJob(list[i].job);
                grid.SetIcon(list[i].job);
                grid.RegisterUIEventDelegate(OnApplyGridEventDlg);

            }
            else
            {
                applyListGrids[i].SetActive(false);
            }
        }

        //申请列表里面的一些按钮
        InitApplyListPanelBtn();
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnApplyGridEventDlg(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UIApplyListGrid grid = data as UIApplyListGrid;
            if (grid == null)
            {
                return;
            }

            uint btnIndex = (uint)param;

            //同意成为队员
            if (btnIndex == 1)
            {
                DataManager.Manager<TeamDataManager>().ReqLeaderAnswerJoin(grid.teamMemberInfo.id, true);
            }

            //拒绝成为队员
            if (btnIndex == 2)
            {
                DataManager.Manager<TeamDataManager>().ReqLeaderAnswerJoin(grid.teamMemberInfo.id, false);
            }
        }
    }




    /// <summary>
    /// 当前页数下的申请人list
    /// </summary>
    /// <returns></returns>
    List<TeamMemberInfo> GetCurrentPageApplyList()
    {
        List<TeamMemberInfo> list = new List<TeamMemberInfo>();
        int index = applyPageIndex * applyGoCount;
        List<TeamMemberInfo> applyMemberList = TDManager.ApplyMemberList;

        for (int i = index; i < index + applyGoCount; i++)
        {
            if (i < applyMemberList.Count)
            {
                list.Add(applyMemberList[i]);
            }
        }

        return list;
    }

    /// <summary>
    /// 申请列表里面的一些按钮
    /// </summary>
    void InitApplyListPanelBtn()
    {
        if (applyPageIndex == 0)
        {
            m_btn_btn_previouspage.gameObject.SetActive(false);
            m_btn_btn_nextpage.gameObject.SetActive(false);
        }

        if (0 < applyPageIndex && applyPageIndex < GetPageCount() - 1)
        {
            m_btn_btn_previouspage.gameObject.SetActive(true);
            m_btn_btn_nextpage.gameObject.SetActive(true);
        }

        if (applyPageIndex >= GetPageCount() - 1)
        {
            m_btn_btn_nextpage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 申请列表页数
    /// </summary>
    /// <returns></returns>
    int GetPageCount()
    {
        int count = TDManager.ApplyMemberList.Count;
        int temp = count % applyGoCount > 0 ? 1 : 0;
        int pageCount = count / applyGoCount + temp;
        return pageCount;
    }

    /// <summary>
    /// 关闭组队界面
    /// </summary>
    void CloseTeamPanel()
    {
        HideSelf();
    }
    #endregion


    #region Click

    /// <summary>
    /// 打开选择活动目标
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Bg_target_Btn(GameObject caster)
    {
        if (TDManager.MainPlayerIsLeader())
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.TeamTargetPanel);
        }
        else
        {
            TipsManager.Instance.ShowTips(LocalTextType.Team_My_nibushidongchang);//你不是队长
        }
    }

    /// <summary>
    /// 自动匹配
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_match_Btn(GameObject caster)
    {
        uint activityId = TDManager.TeamActivityTargetId;
        if (activityId == 0)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Team_Activity_qingxianxuanzehuodongmubiao);//请选择活动目标
            return;
        }

        if (TDManager.IsTeamMatch == false)
        {
            DataManager.Manager<TeamDataManager>().ReqAutoMatch(activityId);//匹配
        }
        else
        {
            DataManager.Manager<TeamDataManager>().ReqCancelMatch();//取消匹配
        }
    }

    /// <summary>
    /// 取消匹配
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_cancelMatch_Btn(GameObject caster)
    {
        TDManager.ReqCancelMatch();
    }

    /// <summary>
    /// 喊话
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_talk_Btn(GameObject caster)
    {
        if (TDManager.TeamActivityTargetId == 0)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Team_Activity_qingxianxuanzehuodongmubiao);//请选择活动目标
            return;
        }
        else
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.TeamTalkPanel);
        }
    }

    /// <summary>
    /// 进入副本
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_enter_Btn(GameObject caster)
    {
        //TDManager.ReqAskTeamCopy();
        //TDManager.ReqAskEnterTarget();
        TDManager.GoToTarget();
    }

    //离开队伍
    void onClick_Btn_leaveteam_Btn(GameObject caster)
    {
        TDManager.ReqLeaveTeam();
    }

    //解散队伍
    void onClick_Btn_disbandteam_Btn(GameObject caster)
    {
        TDManager.ReqDisbandteam();
    }

    /// <summary>
    /// 召唤跟随   队长功能
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_callfollow_Btn(GameObject caster)
    {
        DataManager.Manager<TeamDataManager>().ReqLeaderCallFollow();
    }

    /// <summary>
    /// 取消跟随
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_canelfollow_Btn(GameObject caster)
    {
        DataManager.Manager<TeamDataManager>().ReqCancleFollow();
    }

    /// <summary>
    /// 上一页
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_previouspage_Btn(GameObject caster)
    {
        if (applyPageIndex > 0)
        {
            applyPageIndex--;
        }

        UpdateApplyList();
    }

    /// <summary>
    /// 下一页
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_nextpage_Btn(GameObject caster)
    {
        if (applyPageIndex < GetPageCount() - 1)
        {
            applyPageIndex++;
        }

        UpdateApplyList();
    }

    /// <summary>
    /// 清除申请列表
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_clean_Btn(GameObject caster)
    {
        TDManager.ReqCleanApplyList();
    }

    /// <summary>
    /// 拾取模式
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_pickup_Btn(GameObject caster)
    {
        if (m_btn_btn_pickup.enabled == true)
        {
            if (m_sprite_pickuppanle.gameObject.activeSelf == true)
            {
                m_sprite_pickuppanle.gameObject.SetActive(false);
            }
            else
            {
                m_sprite_pickuppanle.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 拾取模式
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_free_Btn(GameObject caster)
    {
        DataManager.Manager<TeamDataManager>().ReqPickMode(GameCmd.TeamItemMode.TeamItemMode_Free);
        m_sprite_pickuppanle.gameObject.SetActive(false);
    }

    void onClick_Btn_distribution_Btn(GameObject caster)
    {
        DataManager.Manager<TeamDataManager>().ReqPickMode(GameCmd.TeamItemMode.TeamItemMode_Leader);
        m_sprite_pickuppanle.gameObject.SetActive(false);
    }

    void onClick_Btn_pickuppanleClose_Btn(GameObject caster)
    {
        m_sprite_pickuppanle.gameObject.SetActive(false);
    }

    #endregion
}
