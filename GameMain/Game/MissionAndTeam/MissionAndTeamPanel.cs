using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.Utility;
using Client;
using Cmd;
using GameCmd;

partial class MissionAndTeamPanel : UIPanelBase
{
    public enum BtnStatus
    {
        None,
        Mission,       //任务
        Team,          //队伍
        CopyTarget,    //副本目标
        CopyBattleInfo,//副本战况
        NvWa,          //女娲 
        Answer,        //答题
    }

    List<QuestTraceItem> m_lstQuestTransInfo = new List<QuestTraceItem>();

    List<string> m_lstDependece = null;
    BtnStatus m_BtnStatus = BtnStatus.None;
    UIPanel m_taskPanel = null;
    public float m_maxPanelClipRegionY = 0;
    QuestTraceItem m_mainQuestUI = null;

    /// <summary>
    /// 悬赏任务（用于悬赏倒计时刷新）
    /// </summary>
    QuestTraceItem m_tokenQuestTraceItem = null;

    /// <summary>
    /// 任务 list
    /// </summary>
    List<QuestTraceInfo> traceTask;

    public TeamDataManager TDManager
    {
        get
        {
            return DataManager.Manager<TeamDataManager>();
        }
    }

    void RegisterEvent(bool b)
    {
        if (b)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKILLBUTTON_CLICK, DoGameEvent); //点技能按钮
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ROBOTCOMBAT_START, DoGameEvent); //
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.JOYSTICK_PRESS, DoGameEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ROBOTCOMBAT_COPYKILLWAVE, DoGameEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.COPY_REWARDEXP, DoGameEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED, DoGameEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTGUIDECOMPLETE, DoGameEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_WORLDBOSSDAMRANKREFRESH, DoGameEvent); //世界聚宝排行、伤害
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_WORLDBOSSINSPIREREFRESH, DoGameEvent); //世界聚宝鼓舞
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, DoGameEvent); //背包道具更新
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SKILLBUTTON_CLICK, DoGameEvent); //点技能按钮
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ROBOTCOMBAT_START, DoGameEvent); //
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.JOYSTICK_PRESS, DoGameEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ROBOTCOMBAT_COPYKILLWAVE, DoGameEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.COPY_REWARDEXP, DoGameEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED, DoGameEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTGUIDECOMPLETE, DoGameEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_WORLDBOSSDAMRANKREFRESH, DoGameEvent); //世界聚宝排行、伤害    
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_WORLDBOSSINSPIREREFRESH, DoGameEvent); //世界聚宝排行、伤害 
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, DoGameEvent); //背包道具更新
        }

    }

    protected override void OnLoading()
    {
        base.OnLoading();
        m_sprite_QuestTraceItem.transform.localPosition = new Vector3(10000, 10000, 0);
        m_taskPanel = m_trans_grid.parent.GetComponent<UIPanel>();
        m_maxPanelClipRegionY = m_taskPanel.baseClipRegion.w;

        maxGuard = GameTableManager.Instance.GetGlobalConfig<uint>("NWMaxGuard");

        GameObject go = NGUITools.AddChild(m_trans_QuestTraceItemRoot.gameObject, m_sprite_QuestTraceItem.gameObject);
        m_mainQuestUI = go.AddComponent<QuestTraceItem>();
        //Vector3 pos = new Vector3(m_trans_grid.localPosition.x + m_trans_ScrollViewRoot.localPosition.x,m_trans_grid.localPosition.y,m_trans_grid.localPosition.z);
        //m_mainQuestUI.transform.localPosition = pos;
        m_mainQuestUI.transform.localPosition = Vector3.zero;


        m_btn_btnArrow.transform.localPosition = m_trans_ArrowPosShow.localPosition;
        m_widget_team.gameObject.SetActive(false);
        UIEventListener.Get(m_spriteEx_btnMission.gameObject).onClick = OnClickMission;
        UIEventListener.Get(m_spriteEx_btnTeam.gameObject).onClick = OnClickTeam;
        UIEventListener.Get(m_widget_TeamMemberBtnClose.gameObject).onClick = OnClickTeamMemberBtnClose;

        m_trans_TeamMemberBtnRoot.gameObject.SetActive(false);

        m_widget_Offset.transform.localPosition = m_trans_OffsetPosShow.transform.localPosition;
        //InitGvoice();
        OnClickMission(null);
        RegisterEvent(true);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        UpdateTeamList();

        ShowUpBtn();
    }

    void ShowUpBtn()
    {
        if (IsShowUpBtnInCopy())//默认切到上面(副本中)
        {
            this.m_BtnStatus = DataManager.Manager<ComBatCopyDataManager>().GetCopyLeftShowType();
            OnClickUpBtn();
        }
        else
        {
            if (m_BtnStatus != BtnStatus.Team)
            {
                OnClickUpBtn();
            }
        }
    }

    void OnClickUpBtn()
    {
        if (m_BtnStatus == BtnStatus.Team)
        {
            return;
        }

        m_widget_team.gameObject.SetActive(false);
        m_spriteEx_btnMission.ChangeSprite(1);
        m_spriteEx_btnMission.flip = UIBasicSprite.Flip.Nothing;
        m_spriteEx_btnTeam.ChangeSprite(2);
        m_spriteEx_btnTeam.flip = UIBasicSprite.Flip.Nothing;

        this.m_BtnStatus = DataManager.Manager<ComBatCopyDataManager>().GetCopyLeftShowType();
        if (this.m_BtnStatus == BtnStatus.Mission)
        {
            m_label_missionlabel.text = "任务";
            m_trans_mission.gameObject.SetActive(true);
            m_trans_copyTarget.gameObject.SetActive(false);
            m_trans_copyBattleInfo.gameObject.SetActive(false);
            m_trans_nvWa.gameObject.SetActive(false);
            m_trans_answer.gameObject.SetActive(false);

            UpdateTaskList();
        }
        else if (this.m_BtnStatus == BtnStatus.CopyTarget)
        {
            //副本目标
            m_label_missionlabel.text = "目标";
            m_trans_mission.gameObject.SetActive(false);
            m_trans_copyTarget.gameObject.SetActive(true);
            m_trans_copyBattleInfo.gameObject.SetActive(false);
            m_trans_nvWa.gameObject.SetActive(false);
            m_trans_answer.gameObject.SetActive(false);

            UpdateCopyTarget();
        }
        else if (this.m_BtnStatus == BtnStatus.CopyBattleInfo)
        {
            //副本战况
            m_label_missionlabel.text = "战况";
            m_trans_mission.gameObject.SetActive(false);
            m_trans_copyTarget.gameObject.SetActive(false);
            m_trans_copyBattleInfo.gameObject.SetActive(true);
            m_trans_nvWa.gameObject.SetActive(false);
            m_trans_answer.gameObject.SetActive(false);

            InitCopyBattleInfoWidget();

            UpdateCopyBattleInfo();
        }
        else if (this.m_BtnStatus == BtnStatus.NvWa)
        {
            //女娲
            m_label_missionlabel.text = "招募";
            m_trans_mission.gameObject.SetActive(false);
            m_trans_copyTarget.gameObject.SetActive(false);
            m_trans_copyBattleInfo.gameObject.SetActive(false);
            m_trans_nvWa.gameObject.SetActive(true);
            m_trans_answer.gameObject.SetActive(false);

            InitNvWa();
        }

        else if (this.m_BtnStatus == BtnStatus.Answer)
        {
            //答题
            m_label_missionlabel.text = "目标";
            m_trans_mission.gameObject.SetActive(false);
            m_trans_copyTarget.gameObject.SetActive(false);
            m_trans_copyBattleInfo.gameObject.SetActive(false);
            m_trans_nvWa.gameObject.SetActive(false);
            m_trans_answer.gameObject.SetActive(true);

            InitAnswer();
        }
    }

    /// <summary>
    /// 是否默认选择上面的按钮
    /// </summary>
    /// <returns></returns>
    bool IsShowUpBtnInCopy()
    {
        if (false == DataManager.Manager<ComBatCopyDataManager>().IsEnterCopy)
        {
            return false;
        }
        else
        {
            uint copyId = DataManager.Manager<ComBatCopyDataManager>().EnterCopyID;
            table.CopyDataBase copyDb = GameTableManager.Instance.GetTableItem<table.CopyDataBase>(copyId);
            if (copyDb == null)
            {
                return false;
            }
            else
            {
                return copyDb.bShowCopyTarget;
            }
        }
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        //UnInit();//搬到mainpanel了
        RegisterEvent(false);
    }

    protected override void OnHide()
    {
        base.OnHide();
        m_mainQuestUI.Clear();
        for (int i = 0; i < m_lstQuestTransInfo.Count; i++)
        {
            m_lstQuestTransInfo[i].Clear();
        }
    }

    void Update()
    {
        //语音
        //GvoiceChatUpdate();//搬到mainpanel了

        //悬赏任务
        TokenTaskUpdate();
    }

    /// <summary>
    /// 更新悬赏任务
    /// </summary>
    float tempTime = 0;
    void TokenTaskUpdate()
    {
        tempTime += Time.deltaTime;
        if (tempTime > 0.95f)
        {
            RefreshTokenTaskCD();
            tempTime = 0;
        }
    }

    /// <summary>
    /// 刷新令牌悬赏任务CD
    /// </summary>
    void RefreshTokenTaskCD()
    {
        if (m_tokenQuestTraceItem != null)
        {
            QuestTraceInfo questTraceInfo = m_tokenQuestTraceItem.GetTask();
            if (questTraceInfo != null)
            {
                questTraceInfo.UpdateDesc();
                m_tokenQuestTraceItem.UpdateUI(questTraceInfo);
            }
        }
    }

    void OnClickMission(GameObject go)
    {
        if (m_BtnStatus == BtnStatus.Mission)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MissionPanel);
            return;
        }

        m_spriteEx_btnMission.ChangeSprite(1);
        m_spriteEx_btnMission.flip = UIBasicSprite.Flip.Nothing;

        m_spriteEx_btnTeam.ChangeSprite(2);
        m_spriteEx_btnTeam.flip = UIBasicSprite.Flip.Nothing;

        BtnStatus btnStatus = DataManager.Manager<ComBatCopyDataManager>().GetCopyLeftShowType();
        ChangeStatus(btnStatus);
    }

    void OnClickTeam(GameObject go)
    {
        if (m_BtnStatus == BtnStatus.Team)
        {
            if (DataManager.Manager<TeamDataManager>().IsJoinTeam)
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.TeamPanel);
                return;
            }
        }
        m_spriteEx_btnTeam.ChangeSprite(1);
        m_spriteEx_btnTeam.flip = UIBasicSprite.Flip.Both;

        m_spriteEx_btnMission.ChangeSprite(2);
        m_spriteEx_btnMission.flip = UIBasicSprite.Flip.Both;

        ChangeStatus(BtnStatus.Team);
    }

    void ChangeStatus(BtnStatus s)
    {
        m_BtnStatus = s;
        if (m_BtnStatus == BtnStatus.Team)
        {
            //开启左边的组队选项或组队成员列表根obj
            m_trans_mission.gameObject.SetActive(false);
            m_trans_copyTarget.gameObject.SetActive(false);
            m_trans_copyBattleInfo.gameObject.SetActive(false);
            m_trans_nvWa.gameObject.SetActive(false);
            m_trans_answer.gameObject.SetActive(false);
            m_widget_team.gameObject.SetActive(true);
            m_label_missionlabel.color = new Color(111 * 1.0f / 255, 94 * 1.0f / 255, 87 * 1.0f / 255);
            m_label_TeamLbl.color = new Color(55 * 1.0f / 255, 44 * 1.0f / 255, 39 * 1.0f / 255);

            UpdateTeamList();
        }
        else
        {
            ShowUpBtn();
            m_widget_team.gameObject.SetActive(false);
            m_label_TeamLbl.color = new Color(111 * 1.0f / 255, 94 * 1.0f / 255, 87 * 1.0f / 255);
            m_label_missionlabel.color = new Color(55 * 1.0f / 255, 44 * 1.0f / 255, 39 * 1.0f / 255);
        }
    }


    void onClick_BtnArrow_Btn(GameObject caster)
    {
        bool show = m_widget_Offset.transform.localPosition.Equals(m_trans_OffsetPosShow.transform.localPosition);
        UIDefine.GameObjMoveStatus moveStatus = (show) ? UIDefine.GameObjMoveStatus.MoveToInvisible : UIDefine.GameObjMoveStatus.MoveToVisible;
        UIDefine.GameObjMoveStatus moveStatusEnd = (moveStatus == UIDefine.GameObjMoveStatus.MoveToInvisible)
            ? UIDefine.GameObjMoveStatus.Invisible : UIDefine.GameObjMoveStatus.Visible;
        PlayUIContentAnim(show);

        CoroutineMgr.Instance.StartCorountine(SendGameObjMoveStatusEvent(moveStatus));
        CoroutineMgr.Instance.StartCorountine(SendGameObjMoveStatusEvent(moveStatusEnd, 0.2f));
    }

    public void OnArrowMoveEnd()
    {
        if (m_mainQuestUI != null)
        {
            m_mainQuestUI.ShowArrow();
        }
        TweenPosition tp = TweenPosition.Begin(m_widget_Offset.gameObject, 0.2f, m_trans_OffsetPosShow.transform.localPosition);
    }

    public void OnOffsetMoveEnd()
    {
        if (m_mainQuestUI != null)
        {
            m_mainQuestUI.HideArrow();
        }
        TweenPosition tp = TweenPosition.Begin(m_btn_btnArrow.gameObject, 0.2f, m_trans_ArrowPosHide.transform.localPosition);
    }

    /// <summary>
    /// 播放UI动画
    /// </summary>
    /// <param name="show"></param>
    private void PlayUIContentAnim(bool show)
    {
        if (show)
        {
            TweenPosition tp = TweenPosition.Begin(m_widget_Offset.gameObject, 0.2f, m_trans_OffsetPosHide.transform.localPosition);
            tp.eventReceiver = gameObject;
            tp.callWhenFinished = "OnOffsetMoveEnd";
        }
        else
        {
            TweenPosition tp = TweenPosition.Begin(m_btn_btnArrow.gameObject, 0.2f, m_trans_ArrowPosShow.transform.localPosition);
            tp.eventReceiver = gameObject;
            tp.callWhenFinished = "OnArrowMoveEnd";
        }
    }

    /// <summary>
    /// 播放任务箭头指向引导
    /// </summary>
    /// <param name="play"></param>
    private void PlayTaskGuideArrow(bool play)
    {
        if (null != m_sprite_arrow)
        {

        }
    }



    private List<GameObject> m_lstGuideMoveObjs = new List<GameObject>();
    private System.Collections.IEnumerator SendGameObjMoveStatusEvent(UIDefine.GameObjMoveStatus status, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        m_lstGuideMoveObjs.Clear();
        if (null != m_trans_grid)
        {
            Transform ts = null;
            for (int i = 0; i < m_trans_grid.childCount; i++)
            {
                ts = m_trans_grid.GetChild(i);
                if (null != ts && !m_lstGuideMoveObjs.Contains(ts.gameObject))
                {
                    m_lstGuideMoveObjs.Add(ts.gameObject);
                }
            }
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTGAMEOBJMOVESTATUSCHANGED, new UIDefine.GameObjMoveData()
        {
            Status = status,
            Objs = m_lstGuideMoveObjs,
        });
    }



    //创建队伍
    void onClick_Btn_createteam_Btn(GameObject caster)
    {
        DataManager.Manager<TeamDataManager>().ReqCreateTeam();
    }

    //快捷组队
    void onClick_Btn_convenientteam_Btn(GameObject caster)
    {
        Engine.Utility.Log.LogGroup(GameDefine.LogGroup.User_LCY, "快捷组队");
        TDManager.ReqConvenientTeamListByTeamActivityId(TeamDataManager.nearbyId);//附近的队伍
    }

    #region  Task

    int SortQuest(QuestTraceInfo a, QuestTraceInfo b)
    {
        if (a.taskType == TaskType.TaskType_Normal && b.taskType != TaskType.TaskType_Normal)
        {
            return -1;
        }
        else if (a.taskType != TaskType.TaskType_Normal && b.taskType == TaskType.TaskType_Normal)
        {
            return 1;
        }

        ////主线   完成   正在做
        //if (a.taskType == TaskType.TaskType_Normal || b.taskType == TaskType.TaskType_Normal)
        //{
        //    return b.taskType == TaskType.TaskType_Normal ? 1 : -1;
        //}
        //else if (a.GetTaskProcess() == TaskProcess.TaskProcess_CanDone || b.GetTaskProcess() == TaskProcess.TaskProcess_CanDone)
        //{
        //    return b.GetTaskProcess() == TaskProcess.TaskProcess_CanDone ? 1 : -1;
        //}
        //else if (a.GetTaskProcess() == TaskProcess.TaskProcess_Doing || b.GetTaskProcess() == TaskProcess.TaskProcess_Doing)
        //{
        //    return b.GetTaskProcess() == TaskProcess.TaskProcess_Doing ? 1 : -1;
        //}

        return (int)b.time - (int)a.time;
    }

    void SetTaskPanelClip(float height)
    {
        Vector4 clip = m_taskPanel.baseClipRegion;
        clip.w = m_maxPanelClipRegionY - height;
        m_taskPanel.baseClipRegion = clip;
        Vector3 target = m_trans_ScrollViewRoot.transform.localPosition;
        target.y = -height * 0.5f - 2f;
        target.z = 0;
        m_trans_ScrollViewRoot.localPosition = target;

        Vector3 pos = m_trans_grid.localPosition;
        pos.y = clip.w * 0.5f - 2;
        m_trans_grid.localPosition = pos;
    }

    public void UpdateTaskList()
    {
        if (m_BtnStatus != BtnStatus.Mission)
        {
            return;
        }

        //切换帐号，切换角色，重登。
        //if (QuestTranceManager.Instance.m_bResetData)
        //{
        //    m_trans_grid.DestroyChildren();
        //    QuestTranceManager.Instance.m_bResetData = false;
        //}


        //List<QuestTraceInfo> traceTask;
        TaskDataManager taskdata = DataManager.Manager<TaskDataManager>();

        taskdata.GetAllQuestTraceInfo(out traceTask, null);
        traceTask.Sort(SortQuest);

        if (m_mainQuestUI != null)
        {
            if (traceTask.Count > 0 && traceTask[0].taskType == TaskType.TaskType_Normal)
            {
                m_mainQuestUI.UpdateUI(traceTask[0]);
                if (!m_mainQuestUI.gameObject.activeSelf)
                {
                    m_mainQuestUI.gameObject.SetActive(true);
                    //m_mainQuestUI.SetEffect(true);
                }
            }
            else
            {
                m_mainQuestUI.Clear();
            }

            m_mainQuestUI.SetArrow(m_sprite_arrow.gameObject);

            SetTaskPanelClip(m_mainQuestUI.Height);
        }

        int index = 0;
        int totalheight = 0;
        bool haveTakenTask = false;

        //忽略第一个主线任务
        //m_trans_grid.parent.GetComponent<UIScrollView>().ResetPosition();
        for (int i = 1; i < traceTask.Count; i++)
        {
            QuestTraceInfo taskInfo = traceTask[i];
            QuestTraceItem questTraceGrid = null;
            if (index >= m_lstQuestTransInfo.Count)
            {
                GameObject go = GetTaskItem();
                questTraceGrid = go.AddComponent<QuestTraceItem>();
                m_lstQuestTransInfo.Add(questTraceGrid);
            }
            else
            {
                questTraceGrid = m_lstQuestTransInfo[index];
            }

            if (!questTraceGrid.gameObject.activeSelf)
            {
                questTraceGrid.gameObject.SetActive(true);
            }

            questTraceGrid.UpdateUI(taskInfo);

            questTraceGrid.transform.localPosition = new UnityEngine.Vector3(0, -totalheight, 0);
            totalheight += questTraceGrid.Height;
            index++;

            //悬赏任务
            if (traceTask[i].taskType == TaskType.TaskType_Token)
            {
                haveTakenTask = true;
                m_tokenQuestTraceItem = questTraceGrid;
            }
        }

        //悬赏任务
        if (false == haveTakenTask)
        {
            m_tokenQuestTraceItem = null;
        }

        for (int i = index; i < m_lstQuestTransInfo.Count; i++)
        {
            m_lstQuestTransInfo[i].Clear();
        }

        m_trans_grid.parent.GetComponent<UIScrollView>().ResetPosition();
    }

    GameObject GetTaskItem()
    {
        GameObject go = NGUITools.AddChild(m_trans_grid.gameObject, m_sprite_QuestTraceItem.gameObject);
        return go;
    }
    #endregion

    #region copy

    /// <summary>
    /// 进入副本
    /// </summary>
    void EnterCopy()
    {
        //ShowUpBtn();
    }

    /// <summary>
    /// 离开副本
    /// </summary>
    void ExitCopy()
    {
        ReleaseTargetGrid();
        DataManager.Manager<CampCombatManager>().ValueUpdateEvent -= OnValueUpdateEventArgs;
    }

    #endregion

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eUpdateTaskList)
        {
            UpdateTaskList();
        }
        else if (msgid == UIMsgID.eTask_Refresh_QuestInfo)
        {
            uint taskId = (uint)param;
            QuestTraceInfo quest = QuestTranceManager.Instance.GetQuestTraceInfo(taskId);
            if (quest != null)
            {
                int totalheight = 0;
                int mainHeight = m_mainQuestUI.Height;
                if (m_mainQuestUI.TaskID == taskId)
                {
                    m_mainQuestUI.UpdateUI(quest);

                    Engine.Utility.Log.LogGroup("LCY", "刷新主线任务进度{0}/{1}", quest.operate, quest.state);
                    SetTaskPanelClip(m_mainQuestUI.Height);
                    if (mainHeight == m_mainQuestUI.Height)
                    {
                        return true;
                    }
                }

                m_trans_grid.parent.GetComponent<UIScrollView>().ResetPosition();
                for (int i = 0; i < m_lstQuestTransInfo.Count; i++)
                {
                    if (m_lstQuestTransInfo[i].TaskID == taskId)
                    {
                        m_lstQuestTransInfo[i].UpdateUI(quest);

                        Engine.Utility.Log.LogGroup("LCY", "刷新主线任务进度{0}/{1}", quest.operate, quest.state);

                    }

                    m_lstQuestTransInfo[i].transform.localPosition = new UnityEngine.Vector3(0, -totalheight, 0);
                    totalheight += m_lstQuestTransInfo[i].Height;
                }
            }
            else
            {
                Engine.Utility.Log.Error("刷新任务进度出错！！id:{0}", taskId);
            }
        }

        if (msgid == UIMsgID.eUpdateMyTeamList || msgid == UIMsgID.eDisbandTeam)
        {
            UpdateTeamList();
        }

        else if (msgid == UIMsgID.eTeamNewApply)
        {
            NewApplyWarrning();
        }

        else if (msgid == UIMsgID.eTeamMemberBtn)
        {
            ShowTeamMemberBtn(param);
        }

        else if (msgid == UIMsgID.eCopyEnter)
        {
            EnterCopy();
        }

        else if (msgid == UIMsgID.eCopyExit)
        {
            ExitCopy();
        }

        else if (msgid == UIMsgID.eNvWaLvUp)
        {
            NvWaManager.GuardData data = param as NvWaManager.GuardData;
            //SetGuardLvUp(data);
            SetGuardLvUp();
        }

        else if (msgid == UIMsgID.eNvWaCap)
        {
            SetNvWaCap();
        }

        else if (msgid == UIMsgID.eNvWaGuardNumUpdate)
        {
            SetNvWaGuardNum();
            SetGuardLvUp();
        }

        else if (msgid == UIMsgID.eCopyGold)
        {
            UpdateCopyTargetGridGold();
        }

        else if (msgid == UIMsgID.eAnswerCurInfo)
        {
            InitAnswer();
        }
        

        return true;
    }


}
