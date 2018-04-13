//*************************************************************************
//	创建日期:	2016-10-10 10:00
//	文件名称:	RewardPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	悬赏界面
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

partial class RewardPanel : UIPanelBase
{
    public enum RewardPanelPageEnum
    {
        None = 0,
        Page_发布 = 1,
        Page_接取 = 2,
        Max,
    }
    List<table.PublicTokenDataBase> m_lstRelease = new List<table.PublicTokenDataBase>(4);
    List<table.AcceptTokenDataBase> m_lstReceive = new List<table.AcceptTokenDataBase>(4);

    List<RewardCard> m_lstRewardCard = new List<RewardCard>(4);
    RewardPanelPageEnum m_CurrPage = RewardPanelPageEnum.None;
    public RewardPanelPageEnum CurrPage
    {
        get
        {
            return m_CurrPage;
        }
    }
    const int OFFSETX = 266;
    private int m_nLastIndex = -1;
    private float m_fClickCardTime = 0;

    protected override void OnLoading()
    {
        base.OnLoading();
        m_trans_UIRewardTaskGrid.gameObject.SetActive(false);
        m_lstRelease = GameTableManager.Instance.GetTableList<table.PublicTokenDataBase>();
        m_lstReceive = GameTableManager.Instance.GetTableList<table.AcceptTokenDataBase>();

        m_toggle_MoneyToggle.onChange.Add(new EventDelegate(OnToggleChange));
    }

    void OnToggleChange()
    {
        for (int i = 0; i < m_lstRewardCard.Count; ++i)
        {
            m_lstRewardCard[i].UpdateItemNum();
        }

        if (m_toggle_MoneyToggle.value)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM, OnGlobalUIEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM, OnGlobalUIEventHandler);
        }
    }

    public void OnGlobalUIEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            case (int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM:
                ItemDefine.UpdateCurrecyPassData updateData = (ItemDefine.UpdateCurrecyPassData)data;
                if (updateData.MoneyType == GameCmd.MoneyType.MoneyType_Coin)
                {
                    for (int i = 0; i < m_lstRewardCard.Count; i++)
                    {
                        m_lstRewardCard[i].UpdateItemNum();
                    }
                }
                break;
        }
    }
    protected override void OnShow(object data)
    {
        //请求悬赏剩余任务
        NetService.Instance.Send(new GameCmd.stGetTokenTaskNumScriptUserCmd_CS());

        NetService.Instance.Send(new GameCmd.stRequestSelfTokenTAskScriptUserCmd_C());

        m_trans_PanelInfo.gameObject.SetActive(false);

        base.OnShow(data);
        CheckGetRewardTip();
    }

    void CheckGetRewardTip()
    {
        bool tips = DataManager.Manager<TaskDataManager>().CanGetReward();
        dicUITabGrid[UIPanelBase.FisrstTabsIndex][1].SetRedPointStatus(tips);
        m_btn_checkBtn.transform.Find("redtip").gameObject.SetActive(tips);
    }

    protected override void OnHide()
    {
        m_nLastIndex = -1;
        m_fClickCardTime = 0;

        if (m_toggle_MoneyToggle.value)
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM, OnGlobalUIEventHandler);
        }
        base.OnHide();
    }

    public override bool OnTogglePanel(int tabType, int pageid)
    {
        if (tabType == 1)
        {
            m_CurrPage = (RewardPanelPageEnum)pageid;
            m_trans_PanelInfo.transform.parent = transform;
            m_trans_PanelInfo.gameObject.SetActive(false);

            m_nLastIndex = -1;
            m_fClickCardTime = 0;

            RewardCard card = null;
            int count = 0;
            if (m_CurrPage== RewardPanelPageEnum.Page_发布)
            {
                count = m_lstRelease.Count;
            }else if (m_CurrPage == RewardPanelPageEnum.Page_接取)
            {
                count = m_lstReceive.Count;
            }
            for (int i = 0; i < count; i++)
            {
                if (i >= m_lstRewardCard.Count)
                {
                    GameObject go = NGUITools.AddChild(m_trans_gridroot.gameObject, m_trans_UIRewardTaskGrid.gameObject);
                    if (go == null)
                    {
                        break;
                    }
                    card = go.AddComponent<RewardCard>();
                    m_lstRewardCard.Add(card);
                }
                else
                {
                    card = m_lstRewardCard[i];
                }
                if (card != null)
                {
                    card.gameObject.SetActive(true);
                    card.transform.localPosition = new UnityEngine.Vector3(i * OFFSETX, 0, 0);
                    if (m_CurrPage == RewardPanelPageEnum.Page_发布)
                    {
                        card.Init(m_lstRelease[i], i, this);
                    }
                    else if (m_CurrPage == RewardPanelPageEnum.Page_接取)
                    {
                        card.Init(m_lstReceive[i], i, this);
                    }
                }
            }
            for (int i = count; i < m_lstRewardCard.Count; i++)
            {
                m_lstRewardCard[i].gameObject.SetActive(false);
            }
            SetBottomUI(m_CurrPage == RewardPanelPageEnum.Page_发布);
        }

        OnPanelStateChanged();
        return base.OnTogglePanel(tabType, pageid);
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
            firstTab = (null != jumpData.Tabs && jumpData.Tabs.Length >= 1) ? jumpData.Tabs[0] :1;
        }
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId,1,firstTab);
    }

    private void SetBottomUI(bool activeBtn)
    {
        if (activeBtn )
        {
            m_btn_checkBtn.gameObject.SetActive(true);
            m_toggle_MoneyToggle.gameObject.SetActive(true);
            m_label_lableNum.text = string.Format("今日已发布令牌任务{0}/5", DataManager.Manager<TaskDataManager>().RewardMisssionData.RewardReleaseTimes);
        }else{
            m_label_lableNum.text = string.Format("今日已接取令牌任务{0}/{1}", DataManager.Manager<TaskDataManager>().RewardMisssionData.RewardAcceptTimes, DataManager.Manager<TaskDataManager>().RewardMisssionData.RewardAcceptAllTimes);
            m_btn_checkBtn.gameObject.SetActive(false);
            m_toggle_MoneyToggle.gameObject.SetActive(false);

        }
    }

    void onClick_CheckBtn_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RewardMissionPanel);
    }

    public void OnClickCard(RewardCard card)
    {
        if (UnityEngine.Time.realtimeSinceStartup - m_fClickCardTime < 0.3f)
        {
            return;
        }

        if (m_nLastIndex == card.Index)
        {
            m_fClickCardTime = UnityEngine.Time.realtimeSinceStartup;
            for (int i = m_nLastIndex + 1 < m_lstRewardCard.Count ? m_nLastIndex + 1 : 0; i < m_lstRewardCard.Count; i++)
            {
                Vector3 pos = m_lstRewardCard[i].transform.localPosition;
                pos.x = m_nLastIndex + 1 < m_lstRewardCard.Count ? pos.x - OFFSETX : pos.x + OFFSETX;
                TweenPosition tp = TweenPosition.Begin(m_lstRewardCard[i].gameObject, 0.25f, pos);
            }
           
            if (m_nLastIndex + 1 < m_lstRewardCard.Count)
            {
                TweenPosition.Begin(m_widget_rewardInfo.gameObject, 0.15f, new UnityEngine.Vector3(-OFFSETX, 0, 0));
            }
            else
            {
               TweenPosition.Begin(m_widget_rewardInfo.gameObject, 0.15f, new UnityEngine.Vector3(OFFSETX, 0, 0));
            }

            m_nLastIndex = -1;
            return;
        }

        if (m_nLastIndex >= 0 && m_trans_PanelInfo.gameObject.activeSelf )
        {
            for (int i = m_nLastIndex + 1 < m_lstRewardCard.Count ? m_nLastIndex + 1 : 0; i < m_lstRewardCard.Count; i++)
            {
                Vector3 pos = m_lstRewardCard[i].transform.localPosition;
                pos.x = m_nLastIndex + 1 < m_lstRewardCard.Count ? pos.x - OFFSETX : pos.x + OFFSETX;
                m_lstRewardCard[i].transform.localPosition = pos;
            }
            m_widget_rewardInfo.transform.localPosition = new UnityEngine.Vector3(-OFFSETX, 0, 0);
        }
        m_nLastIndex = card.Index;


        for (int i = card.Index + 1 < m_lstRewardCard.Count ? card.Index + 1 : 0; i < m_lstRewardCard.Count; i++)
        {
            Vector3 pos = m_lstRewardCard[i].transform.localPosition;
            pos.x = card.Index + 1 < m_lstRewardCard.Count ? pos.x + OFFSETX : pos.x - OFFSETX;
            TweenPosition tp = TweenPosition.Begin(m_lstRewardCard[i].gameObject, 0.15f, pos);
        }

        m_label_release_Label.enabled = card.TaskType == 1;
        m_label_take_Label.enabled = card.TaskType == 2;

        m_widget_rewardInfo.transform.Find("Sprite/release_tips").gameObject.SetActive(card.TaskType == 1);
        m_widget_rewardInfo.transform.Find("Sprite/take_tips").gameObject.SetActive(card.TaskType == 2);

        if (card.TaskType == 1)
        {
            table.QuestDataBase taskdb = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(card.TaskId);
            //if (taskdb != null)
            //{
            //    m_label_infoexpNum.text = taskdb.dwRewardExp.ToString();
            //}
            //m_label_receiveCost.transform.parent.gameObject.SetActive(false);
            //m_label_acceptExp.text = "";
        }else if (card.TaskType == 2)
        {
            //m_label_receiveCost.transform.parent.gameObject.SetActive(true);
            //m_label_infoexpNum.text = "";
            //m_label_receiveCost.text = card.CostMoney.ToString();
            table.QuestDataBase taskdb = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(card.TaskId);
            if (taskdb != null)
            {
                List<table.RandomTargetDataBase> randomlist = GameTableManager.Instance.GetTableList<table.RandomTargetDataBase>();
                for (int i = 0, imax = randomlist.Count; i < imax; i++)
                {
                    if (randomlist[i].activity_type == 1 && taskdb.target_group == randomlist[i].group && randomlist[i].level == MainPlayerHelper.GetPlayerLevel())
                    {
                        m_label_acceptExp.text = string.Format("{0}", randomlist[i].level * randomlist[i].exp_ratio);
                        break;
                    }
                } 
            }          
        }

        m_trans_PanelInfo.transform.parent = m_trans_gridroot;
        m_trans_PanelInfo.transform.localPosition = card.transform.localPosition;
        m_trans_PanelInfo.gameObject.SetActive(true);
        if (card.Index + 1 < m_lstRewardCard.Count)
        {
            m_trans_PanelInfo.transform.localPosition = card.transform.localPosition + new Vector3(OFFSETX, 0, 0);
            m_widget_rewardInfo.transform.localPosition = new UnityEngine.Vector3(-OFFSETX, 0, 0);
            TweenPosition tp = TweenPosition.Begin(m_widget_rewardInfo.gameObject, 0.15f, Vector3.zero);
        }
        else
        {
            m_widget_rewardInfo.transform.localPosition = new UnityEngine.Vector3(OFFSETX, 0, 0);
            TweenPosition tp = TweenPosition.Begin(m_widget_rewardInfo.gameObject, 0.15f, Vector3.zero);

        }
        m_fClickCardTime = UnityEngine.Time.realtimeSinceStartup;
    }


    public bool IsUseMoney()
    {
        return m_toggle_MoneyToggle.value;
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eRewardTaskLeftNum)
        {
            Debug.Log("eRewardTaskLeftNum");

            string strCardNum = string.Format("剩余任务:{0}", param);
          //  m_label_child_reward_number.text = strCardNum;

        }
        else if (msgid == UIMsgID.eRewardTaskCardNum)//刷新任务池数量
        {
            for (int i = 0; i < m_lstRewardCard.Count; i++)
            {
                m_lstRewardCard[i].SetBottomUI();
            }
            
        }

        if (msgid == UIMsgID.eRewardTaskListRefresh)
        {
            CheckGetRewardTip();
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.RewardMissionPanel))
            {
                DataManager.Manager<UIPanelManager>().SendMsg(PanelID.RewardMissionPanel, UIMsgID.eRewardTaskListRefresh, null);
            }
            //任务完成后也要刷新按钮
            for (int i = 0; i < m_lstRewardCard.Count; i++)
            {
                m_lstRewardCard[i].SetBottomUI();
            }
            SetBottomUI(m_CurrPage == RewardPanelPageEnum.Page_发布);
        }

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
}

/*
partial class RewardPanel : UIPanelBase
{
    RewardCard[] m_RewardCardArrar = null;
    bool m_bLoading = false;
    RewardCard m_SelectRewardCard = null;
    int m_nCardOffset = 265;

    RewardMission[] m_RewardMissionArrar = null;

    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
        panelBaseData.m_bool_needCMBg = true;
        panelBaseData.m_int_topBarUIMask = (int)UIDefine.UITopBarUIMode.CurrencyBar;
        panelBaseData.m_str_name = "悬赏";
        panelBaseData.m_em_showMode = UIDefine.UIPanelShowMode.HideNormal;
        panelBaseData.m_em_colliderMode = UIDefine.UIPanelColliderMode.TransBg;
    }
    public override void OnLoading()
    {
        base.OnLoading();
        CreateCards();
        CreateTaskItems();
        AddToggleEvent(m_toggle_Access);//接取
        AddToggleEvent(m_toggle_Release);//发布
        AddToggleEvent(m_toggle_hall);//悬赏大厅
        AddToggleEvent(m_toggle_mine);//我的悬赏


        m_bLoading = true;


    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        //请求悬赏剩余任务
        List<table.RewardTaskDataBase> lstData = GameTableManager.Instance.GetTableList<table.RewardTaskDataBase>();
        for (int i = 0; i < lstData.Count; ++i)
        {
            NetService.Instance.Send(new GameCmd.stGetTokenTaskNumScriptUserCmd_CS()
            {
                tokentaskid = lstData[i].id,
            });
        }
        NetService.Instance.Send(new GameCmd.stRequestSelfTokenTAskScriptUserCmd_C());
        if (m_bLoading)
        {
            m_bLoading = false;
            if (data != null)
            {
                string strOpen = (string)data;
                if (m_toggle_Release.name.Equals(strOpen) )
                {
                    m_toggle_Release.startsActive = true;
                    m_toggle_Access.startsActive = false;
                }
                else if (m_toggle_Access.name.Equals(strOpen) && !m_toggle_Access.value)
                {
                    m_toggle_Release.startsActive = false;
                    m_toggle_Access.startsActive = true;
                }
                else if (m_toggle_hall.name.Equals(strOpen) && !m_toggle_hall.value)
                {
                    m_toggle_mine.startsActive = false;
                    m_toggle_hall.startsActive = true;
                }
                else if (m_toggle_mine.name.Equals(strOpen) && !m_toggle_mine.value)
                {
                    m_toggle_hall.startsActive = false;
                    m_toggle_mine.startsActive = true;
                }
            }
        }
        else
        {
            if (!m_toggle_hall.value)
            {
                m_toggle_hall.gameObject.SendMessage("OnClick", m_toggle_hall.gameObject, SendMessageOptions.RequireReceiver);
            }

            if (data != null)
            {
                string strOpen = (string)data;
                OnToggleClick(strOpen);
            }
            else
            {
                if (!m_toggle_Release.value)
                {
                    m_toggle_Release.gameObject.SendMessage("OnClick", m_toggle_Release.gameObject, SendMessageOptions.RequireReceiver);
                }
                else
                {
                    OnToggle(m_toggle_Release.name);
                }
            }
        }
    }

    void OnToggleClick(string strOpen)
    {
        if (m_toggle_Release.name.Equals(strOpen))
        {
            if (!m_toggle_Release.value)
            {
                m_toggle_Release.gameObject.SendMessage("OnClick", m_toggle_Release.gameObject, SendMessageOptions.RequireReceiver);
            }
            else
            {
                OnToggle(m_toggle_Release.name);
            }
        }
        else if (m_toggle_Access.name.Equals(strOpen))
        {
            if (!m_toggle_Access.value)
            {
                m_toggle_Access.gameObject.SendMessage("OnClick", m_toggle_Access.gameObject, SendMessageOptions.RequireReceiver);
            }
            else
            {
                OnToggle(m_toggle_Access.name);
            }
        }
        else if (m_toggle_hall.name.Equals(strOpen) && !m_toggle_hall.value)
        {
            m_toggle_hall.gameObject.SendMessage("OnClick", m_toggle_hall.gameObject, SendMessageOptions.RequireReceiver);
        }
        else if (m_toggle_mine.name.Equals(strOpen) && !m_toggle_mine.value)
        {
            m_toggle_mine.gameObject.SendMessage("OnClick", m_toggle_mine.gameObject, SendMessageOptions.RequireReceiver);
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    void AddToggleEvent(UIToggle toggle)
    {
        EventDelegate.Add(toggle.onChange, () =>
        {
            if (toggle.value)
            {
                OnToggle(toggle.name);
            }
        });
    }

    void OnToggle(string strBtnName)
    {
        switch (strBtnName)
        {
            case "Release":
                {
                    if (m_RewardCardArrar == null)
                    {
                        return;
                    }
                    List<table.RewardTaskDataBase> lstData = GameTableManager.Instance.GetTableList<table.RewardTaskDataBase>();
                    int nindex = 0;
                    for (int i = 0; i < lstData.Count; ++i )
                    {
                        if (lstData[i].type == 1)
                        {
                            m_RewardCardArrar[nindex].InitUI(nindex,this,lstData[i]);
                            nindex++;
                        }
                    }
                    ResetPos();
                }
                break;
            case "Access":
                {
                    if (m_RewardCardArrar == null)
                    {
                        return;
                    }        
                    
                    List<table.RewardTaskDataBase> lstData = GameTableManager.Instance.GetTableList<table.RewardTaskDataBase>();
                    int nindex = 0;
                    for (int i = 0; i < lstData.Count; ++i)
                    {
                        if (lstData[i].type == 2)
                        {
                            //请求悬赏剩余任务
                            NetService.Instance.Send(new GameCmd.stGetTokenTaskNumScriptUserCmd_CS()
                            {
                                tokentaskid = lstData[i].id,
                            });
                            m_RewardCardArrar[nindex].InitUI(nindex, this, lstData[i]);
                            nindex++;
                        }
                    }
                    ResetPos();
                }
                break;
            case "hall":
                {
                    m_widget_hallContent.alpha = 1f;
                    m_widget_mineContent.alpha = 0f;
                }
                break;
            case "mine": 
                {
                    m_widget_hallContent.alpha = 0f;
                    m_widget_mineContent.alpha = 1f;

                    SetMissionListUI();

                }
                break;
            default:
                break;
        }
    }

    private void SetMissionListUI()
    {
        List<RewardMisssionInfo> lstMission = DataManager.Manager<TaskDataManager>().ReleaseRewardList;
        if (m_RewardMissionArrar != null)
        {
            for (int i = 0; i < m_RewardMissionArrar.Length - 1; ++i)
            {
                if (lstMission.Count > i)
                {
                    m_RewardMissionArrar[i].InitUI(lstMission[i], 1, this);
                }
                else
                {
                    m_RewardMissionArrar[i].InitUI(null, 1, this);
                }
            }
            m_RewardMissionArrar[m_RewardMissionArrar.Length - 1].InitUI(DataManager.Manager<TaskDataManager>().receiveReward, 2, this);
        }
        Transform transLable = m_widget_Mine_Release.transform.Find("frequency");
        if (transLable != null)
        {
            transLable.GetComponent<UILabel>().text = string.Format("{0}/{1}", DataManager.Manager<TaskDataManager>().RewardReleaseRemain, 5);
            transLable = null;
        }
        transLable = m_widget_Mine_Access.transform.Find("frequency");
        if (transLable != null)
        {
            transLable.GetComponent<UILabel>().text = string.Format("{0}/{1}", DataManager.Manager<TaskDataManager>().RewardAcceptRemain, 5);
        }
    }
    void CreateCards()
    {
        m_RewardCardArrar = new RewardCard[4];
        for (int i = 0; i < m_RewardCardArrar.Length; i++)
        {
            GameObject go = NGUITools.AddChild(m_trans_itemRoot.gameObject, m_widget_RewardCard.gameObject);
            m_RewardCardArrar[i] = go.AddComponent<RewardCard>();
            go.transform.localPosition = new UnityEngine.Vector3(i * 265,0,0);
        }
        m_widget_RewardCard.gameObject.SetActive(false);
    }

    void CreateTaskItems()
    {
        m_RewardMissionArrar = new RewardMission[6];
        int ntotalHeight = 62;
        for (int i = 0; i < m_RewardMissionArrar.Length; i++)
        {
            GameObject go = NGUITools.AddChild(m_trans_taskroot.gameObject,m_trans_Mission.gameObject);
            m_RewardMissionArrar[i] = go.AddComponent<RewardMission>();
            if (i == m_RewardMissionArrar.Length - 1)
            {
                ntotalHeight -= 63;
                ntotalHeight += 22;
                m_widget_Mine_Access.transform.localPosition = new UnityEngine.Vector3(0,-ntotalHeight,0);
                ntotalHeight += 63;
                go.transform.localPosition = new UnityEngine.Vector3(0, -ntotalHeight, 0);
                ntotalHeight += 105;
            }
            else
            {
                go.transform.localPosition = new UnityEngine.Vector3(0, -ntotalHeight, 0);
                ntotalHeight += 105;
            }
        }
        m_trans_Mission.gameObject.SetActive(false);
    }

    void ResetPos()
    {
        if (m_RewardCardArrar == null)
        {
            return;
        }
        for (int i = 0; i < m_RewardCardArrar.Length; i++)
        {
            m_RewardCardArrar[i].transform.localPosition = new UnityEngine.Vector3(i * m_nCardOffset, 0, 0);
        }

        m_widget_childContent.alpha = 0f;
    }

   
    void onClick_Btn_GoRelease_Btn(GameObject caster)
    {

    }

    void onClick_Btn_GoAccess_Btn(GameObject caster)
    {
    }


    public void OnSelectCard(int nIndex)
    {
        if (m_RewardCardArrar == null || (m_RewardCardArrar.Length < nIndex))
        {
            return;
        }
        m_SelectRewardCard = m_RewardCardArrar[nIndex];
        float fmovex = m_RewardCardArrar[nIndex].transform.localPosition.x;

        for (int i = 0; i <= nIndex; i++)
        {
            Vector3 pos = m_RewardCardArrar[i].transform.localPosition;
            pos.x -= fmovex;
            TweenPosition tp = TweenPosition.Begin(m_RewardCardArrar[i].gameObject, 0.25f, pos);
            if (i == nIndex)
            {
                tp.eventReceiver = gameObject;
                tp.callWhenFinished = "OnMoveEnd";
            }
        }

        for (int i = nIndex + 1; i < m_RewardCardArrar.Length; i++)
        {
            Vector3 pos = m_RewardCardArrar[i].transform.localPosition;
            pos.x += 1000;
            TweenPosition.Begin(m_RewardCardArrar[i].gameObject, 0.25f, pos);
        }
    }

    public void OnReset(int nindex)
    {
        TweenPosition.Begin(m_widget_childContent.gameObject, 0.15f, new UnityEngine.Vector3(-820, 0, 0));
        TweenAlpha ta = TweenAlpha.Begin(m_widget_childContent.gameObject, 0.15f, 0f);
        ta.eventReceiver = gameObject;
        ta.callWhenFinished = "OnMoveEndToResetPos";
    }

    void OnMoveEndToResetPos()
    {
        uint nType = 1;
        if (m_SelectRewardCard != null)
        {
            int nIndex = m_SelectRewardCard.Index;
            float fmovex = nIndex * m_nCardOffset;

            for (int i = 0; i <= nIndex; i++)
            {
                Vector3 pos = m_RewardCardArrar[i].transform.localPosition;
                pos.x += fmovex;
                TweenPosition tp = TweenPosition.Begin(m_RewardCardArrar[i].gameObject, 0.25f, pos);
            }

            for (int i = nIndex + 1; i < m_RewardCardArrar.Length; i++)
            {
                Vector3 pos = m_RewardCardArrar[i].transform.localPosition;
                pos.x -= 1000;
                TweenPosition.Begin(m_RewardCardArrar[i].gameObject, 0.25f, pos);
            }
            m_SelectRewardCard.ChangeState(false);
            if (m_SelectRewardCard.Data != null)
            {
                nType = m_SelectRewardCard.Data.type;
            }
        }
        //重新刷新下界面
        List<table.RewardTaskDataBase> lstData = GameTableManager.Instance.GetTableList<table.RewardTaskDataBase>();
        int nindex = 0;
        for (int i = 0; i < lstData.Count; ++i)
        {
            if (lstData[i].type == m_SelectRewardCard.Data.type)
            {
                m_RewardCardArrar[nindex].InitUI(nindex, this, lstData[i]);
                nindex++;
            }
        }
    }

    /// <summary>
    /// 列表移动完毕
    /// </summary>
    void OnMoveEnd()
    {
        if (m_SelectRewardCard == null || m_SelectRewardCard.Data == null)
        {
            return;
        }

        m_label_child_reward_name.text = m_SelectRewardCard.Data.title;
        m_sprite_child_reward_icon.spriteName = m_SelectRewardCard.Data.icon;

        string strCardNum = "";
        string strTime = "";
        string strTaskNum = "";

        table.QuestDataBase quest = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(m_SelectRewardCard.Data.taskid);
        if (quest != null)
        {
            m_label_rewardexp.text = quest.dwRewardExp.ToString();
        }

        if (m_SelectRewardCard.Data.type == 1)//发布
        {
            strCardNum = string.Format("拥有令牌:{0}", DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_SelectRewardCard.Data.itemID));
            if (quest != null)
            {
                strTime = string.Format("{0}小时候可自动完成", quest.dwLimitTime);
            }
            strTaskNum = string.Format("今日已发布{0}/{1}次悬赏任务", DataManager.Manager<TaskDataManager>().RewardReleaseRemain, 5);
            m_toggle_release_xiaohaoSprite.transform.parent.gameObject.SetActive(true);
            m_label_xiaohao.gameObject.SetActive(false);
            m_btn_btn_release.GetComponentInChildren<UILabel>().text = "发布";
        }
        else if (m_SelectRewardCard.Data.type == 2)//接取
        {
            m_btn_btn_release.GetComponentInChildren<UILabel>().text = "接取";

            m_toggle_release_xiaohaoSprite.transform.parent.gameObject.SetActive(false);
            m_label_xiaohao.gameObject.SetActive(true);
            m_label_xiaohao.text = m_SelectRewardCard.Data.cost.ToString();
            
            if (quest != null)
            {
                strTime = string.Format("时间限制: {0}小时", quest.dwLimitTime);
            }
            strTaskNum = string.Format("今日已接取{0}/{1}次悬赏任务", DataManager.Manager<TaskDataManager>().RewardAcceptRemain, 5);

        }

        m_label_frequency.text = strTaskNum;
        m_label_child_reward_number.text = strCardNum;

        m_widget_childContent.transform.localPosition = new UnityEngine.Vector3(-820, 0, 0);
        TweenPosition.Begin(m_widget_childContent.gameObject, 0.15f, Vector3.zero);
        TweenAlpha.Begin(m_widget_childContent.gameObject, 0.15f, 1f);

        m_SelectRewardCard.ChangeState(true);
    }

    void onClick_Btn_release_Btn(GameObject caster)
    {
        if (m_SelectRewardCard == null || m_SelectRewardCard.Data == null)
        {
            return;
        }

        int nlevel = Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.CreatureProp.Level);
        if (m_SelectRewardCard.Data.type == 1)
        {
//             if (nlevel < m_SelectRewardCard.Data.openLvele)
//             {
//                 TipsManager.Instance.ShowTips(LocalTextType.Arena_Commond_1);
//                 return;
//             }
            //发布
            NetService.Instance.Send(new GameCmd.stPublicTokenTaskScriptUserCmd_CS()
            {
                tokentaskid = m_SelectRewardCard.Data.id,
                userid = Client.ClientGlobal.Instance().MainPlayer.GetID(),
                isusemoney = m_toggle_release_xiaohaoSprite.value ? (uint)1 : 0,
            });
        }
        else if (m_SelectRewardCard.Data.type == 2)
        {
//             if (nlevel < m_SelectRewardCard.Data.openLvele)
//             {
//                 TipsManager.Instance.ShowTipsById(1);
//                 return;
//             }
            //接取
            NetService.Instance.Send(new GameCmd.stAcceptTokenTaskScriptUserCmd_CS()
            {
                tokentaskid = m_SelectRewardCard.Data.id,
                userid = Client.ClientGlobal.Instance().MainPlayer.GetID(),
            });
        }
    }
    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eRewardTaskLeftNum)
        {
            string strCardNum = string.Format("剩余任务:{0}", param);
            m_label_child_reward_number.text = strCardNum;

        }
        else if (msgid == UIMsgID.eRewardTaskCardNum)
        {
            if (m_RewardCardArrar != null)
            {
                for (int i = 0; i < m_RewardCardArrar.Length; i++)
                {
                    if (m_RewardCardArrar[i].Data != null && m_RewardCardArrar[i].Data.id == (int)param)
                    {
                        m_RewardCardArrar[i].RefreshTaskNum();
                    }
                }
            }
        }

        if (msgid == UIMsgID.eRewardTaskListRefresh)
        {
            if (m_widget_mineContent.alpha == 1f)
            {
                SetMissionListUI();
            }
            else if (m_SelectRewardCard != null && m_SelectRewardCard.Data != null)
            {
                string strTaskNum = "";
                if (m_SelectRewardCard.Data.type == 1)//发布
                {
                    strTaskNum = string.Format("今日已发布{0}/{1}次悬赏任务", DataManager.Manager<TaskDataManager>().RewardReleaseRemain, 5);
                }
                else if (m_SelectRewardCard.Data.type == 2)//接取
                {
                    strTaskNum = string.Format("今日已接取{0}/{1}次悬赏任务", DataManager.Manager<TaskDataManager>().RewardAcceptRemain, 5);
                }
                m_label_frequency.text = strTaskNum;
            }
        }
        return base.OnMsg(msgid, param);
    }

    public void GoToRelease()
    {
        m_toggle_hall.gameObject.SendMessage("OnClick", m_toggle_hall.gameObject, SendMessageOptions.RequireReceiver);
        m_toggle_Release.gameObject.SendMessage("OnClick", m_toggle_Release.gameObject, SendMessageOptions.RequireReceiver);
    }

    public void GoToReceive()
    {
        m_toggle_hall.gameObject.SendMessage("OnClick", m_toggle_mine.gameObject, SendMessageOptions.RequireReceiver);
        m_toggle_Access.gameObject.SendMessage("OnClick", m_toggle_Access.gameObject, SendMessageOptions.RequireReceiver);
    }
}


*/