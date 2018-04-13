
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Client;
//stRequestSearchUserRelationUserCmd_C

public partial class FriendPanel : UIPanelBase
{
    public enum TabsType
    {
        None = 0,
        Content,
        Seceond,
    }
    public enum FriendPanelPageEnum
    {
        None = 0,
        Page_最近 = 1,
        Page_好友 = 2,
        Page_添加 = 3,
        Page_邮箱 = 4,
        Max,
    }
    public enum SecondTab
    {
        None = 0,
        Contacts,
        Interactive,
        Friend,
        Enemy,
        BlackList,
        Max,
    }
    private List<RoleRelation> m_lstRoleRelation = null;
    UIPanel m_chatPanel;
    UIScrollView m_chatScrollView;
    RelationManager m_relationMgr;
    PrivateChannelManager privateChatManager;
    GameCmd.RelationType m_currRelationList = GameCmd.RelationType.Relation_MAX;
    FriendPanelPageEnum m_Content = FriendPanelPageEnum.None;
    SecondTab m_SecondTab = SecondTab.None;
    public FriendPanelPageEnum CurrContent
    {
        get
        {
            return m_Content;
        }
        set
        {
            m_Content = value;
        }
    }
    public SecondTab CurrList
    {
        get { return m_SecondTab; }
    }

    int m_currSelectId = -1;
    bool m_bIsRobot = false;

    Dictionary<uint, UITabGrid> m_dic_tabs = new Dictionary<uint, UITabGrid>();

    uint privateChatOpenLv = 0;

    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();

    }

    protected override void OnLoading()
    {
        base.OnLoading();
        MailEventRegister(true);
        m_relationMgr = DataManager.Manager<RelationManager>();
        privateChatManager = DataManager.Manager<ChatDataManager>().PrivateChatManager;
        //聊天相关
        m_chatPanel = m_trans_chatroot.parent.GetComponent<UIPanel>();
        m_chatScrollView = m_chatPanel.GetComponent<UIScrollView>();
        //load prefab
        m_chatItemPrefab = m_trans_UIChatItemGrid.gameObject;
        m_trans_timeTips.gameObject.SetActive(false);
        m_sprite_bg_searchresults.gameObject.SetActive(false);
        InitFriendSecondTabs();
        InitLeftGrid();

        AddCreator(m_trans_mail_text_scroll.transform);

        privateChatOpenLv = GameTableManager.Instance.GetGlobalConfig<uint>("PrivateChatOpenLevel");
    }


    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_ctor_FriendScrollview != null)
        {
            m_ctor_FriendScrollview.Release(depthRelease);
        }
        //引用关系 不能clear add by dianyu 好友界面会变空
        //if (m_lstRoleRelation != null)
        //{
        //    m_lstRoleRelation.Clear();
        //}
        if (m_ctor_MailScroll != null)
        {
            m_ctor_MailScroll.Release(depthRelease);
        }
        DataManager.Manager<MailManager>().StopCollectAll();
    }

    private bool SetActiveFriendSecondTas(SecondTab tab,bool force = false)
    {
        if (m_SecondTab == tab && !force)
        {
            return false;
        }
        m_SecondTab = tab;
        switch (m_SecondTab)
        {
            case SecondTab.Contacts:
                m_currRelationList = GameCmd.RelationType.Relation_Contact;
                break;
            case SecondTab.Interactive:
                m_currRelationList = GameCmd.RelationType.Relation_Interactive;
                break;
            case SecondTab.Friend:
                m_currRelationList = GameCmd.RelationType.Relation_Friend;
                break;
            case SecondTab.Enemy:
                m_currRelationList = GameCmd.RelationType.Relation_Enemy;
                break;
            case SecondTab.BlackList:
                m_currRelationList = GameCmd.RelationType.Relation_Black;
                break;
            default:
                break;
        }
        RefreshLeftListUI();
        return true;
    }

    private void OnFriendSecondTabsEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (null != data && data is UITabGrid)
                    {
                        UITabGrid tab = data as UITabGrid;
                        SetActiveFriendSecondTas((SecondTab)tab.TabID);
                        
                    }
                }
                break;
        }
    }
    private Dictionary<SecondTab, UITabGrid> m_dicFriendTabGrid = null;
    private void InitFriendSecondTabs()
    {
        if (null == m_dicFriendTabGrid && null != m_trans_SecondTab)
        {
            m_dicFriendTabGrid = new Dictionary<SecondTab, UITabGrid>();
            UITabGrid tabGrid = null;
            for (SecondTab i = SecondTab.None + 1; i < SecondTab.Max; i++)
            {
                Transform ts = m_trans_SecondTab.Find(i.ToString());
                if (null == ts)
                {
                    continue;
                }
                tabGrid = ts.GetComponent<UITabGrid>();
                if (tabGrid == null)
                {
                    tabGrid = ts.gameObject.AddComponent<UITabGrid>();
                }
                if (tabGrid != null)
                {
                    tabGrid.TabID = (int)i;
                    tabGrid.SetHightLight(false);
                    if (!m_dicFriendTabGrid.ContainsKey(i))
                    {
                        m_dicFriendTabGrid.Add(i, tabGrid);
                        tabGrid.RegisterUIEventDelegate(OnFriendSecondTabsEvent);
                    }
                }
            }
        }
    }

    void InitLeftGrid()
    {
        if (m_ctor_FriendScrollview != null)
        {
            m_ctor_FriendScrollview.RefreshCheck();
            m_ctor_FriendScrollview.Initialize<UIFriendGrid>(m_trans_UIFriendGrid.gameObject, OnFriendDataUpdate, OnFriendUIEvent);
        }
    }

    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs v)
    {
        if (m_currRelationList.ToString() == v.key)
        {
            List<RoleRelation> datas = (List<RoleRelation>)v.newValue;
            m_label_labletips.text = "";
            Engine.Utility.Log.LogGroup("ZCX", "update list " + m_currRelationList.ToString() + datas.Count);
            RefreshLeftListUI();
            if (m_currRelationList == GameCmd.RelationType.Relation_Contact)
            {
                if (m_ctor_FriendScrollview != null)
                {
                    // m_FriendUIGridCreatorBase.CreateGrids(datas.Count);
                    List<UIFriendGrid> lstGrid = m_ctor_FriendScrollview.GetGrids<UIFriendGrid>(true);
                    for (int i = 0; i < lstGrid.Count; i++)
                    {
                        if (lstGrid[i].Data.uid == privateChatManager.SilentFrienduid)
                        {
                            OnSelectFriend(lstGrid[i].Data, true);
                            lstGrid[i].SetMaskState(true);
                            break;
                        }
                    }
                }
            }

        }
        else if ("SEARCH" == v.key && m_currRelationList == GameCmd.RelationType.Relation_FriendRequest)
        {
            List<RoleRelation> datas = (List<RoleRelation>)v.newValue;
            if (m_trans_chatroot != null)
            {
                m_trans_chatroot.DestroyChildren();
                for (int i = 0; i < datas.Count; i++)
                {
                    GameObject go = NGUITools.AddChild(m_trans_chatroot.gameObject, m_sprite_bg_searchresults.gameObject);
                    go.transform.localPosition = new Vector3(0, -105 * i, 0);
                    FriendSearch script = go.AddComponent<FriendSearch>();
                    go.SetActive(true);
                    script.SetInfo(datas[i], m_input_Input.value);
                }
            }

        }
        else if ("SortList" == v.key && m_currRelationList == GameCmd.RelationType.Relation_Contact)
        {
            DeActiveFrendItem();
            List<RoleRelation> datas = (List<RoleRelation>)v.newValue;
            if (m_ctor_FriendScrollview != null)
            {
                m_ctor_FriendScrollview.CreateGrids(datas.Count);
                if (privateChatManager.SilentFrienduid != 0)
                {
                    List<UIFriendGrid> lstGrid = m_ctor_FriendScrollview.GetGrids<UIFriendGrid>(true);
                    for (int i = 0; i < lstGrid.Count; i++)
                    {
                        if (lstGrid[i].Data.uid == privateChatManager.SilentFrienduid)
                        {
                            OnSelectFriend(lstGrid[i].Data, true);
                            lstGrid[i].SetMaskState(true);
                            break;
                        }
                    }
                }
            }
        }
        else if ("updateFriendLevel" == v.key)
        {
            if (m_ctor_FriendScrollview != null)
            {
                m_relationMgr.GetRelationListByType(m_currRelationList, out m_lstRoleRelation);
                m_ctor_FriendScrollview.UpdateActiveGridData();
                //                 List<UIFriendGrid> lstGrid = m_ctor_FriendScrollview.GetGrids<UIFriendGrid>(true);
                //                 for (int i = 0; i < lstGrid.Count; i++)
                //                 {
                //                     lstGrid[i].RefreshLevel();
                //                 }
            }
        }
    }

    void OnFriendDataUpdate(UIGridBase data, int index)
    {
        if (null != m_lstRoleRelation && index < m_lstRoleRelation.Count)
        {

            UIFriendGrid grid = data as UIFriendGrid;
            grid.SetParent(this);
            grid.SetGridData(m_lstRoleRelation[index]);
        }
    }

    void OnFriendUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (CurrContent != FriendPanelPageEnum.Page_添加)
                {
                    DeActiveFrendItem();
                    UIFriendGrid grid = data as UIFriendGrid;
                    if (grid != null)
                    {
                        if (grid != null)
                        {
                            grid.SetMaskState(true);
                            grid.SetTipsState(false);
                        }


                        OnSelectFriend((RoleRelation)grid.Data);
                    }
                }
                break;
        }
    }

    public override bool OnTogglePanel(int nTabType, int pageid)
    {
        if (nTabType == (int)TabsType.Content)
        {
            ToggleContent((FriendPanelPageEnum)pageid);
            return true;
        }
        return base.OnTogglePanel(nTabType, pageid);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_relationMgr.ValueUpdateEvent += OnValueUpdateEventArgs;
        InitOnShow();
        Protocol.Instance.ReqFriendLevel();

        if (data != null && data is RoleRelation)//陌生人聊天
        {
            RoleRelation role = (RoleRelation)data;
            if (m_Content == FriendPanelPageEnum.Page_好友 && m_relationMgr.IsMyFriend(role.uid))
            {
                DeActiveFrendItem();
                List<UIFriendGrid> lstGrid = m_ctor_FriendScrollview.GetGrids<UIFriendGrid>(true);
                for (int i = 0; i < lstGrid.Count; i++)
                {
                    if (lstGrid[i].Data.uid == role.uid)
                    {
                        OnSelectFriend(lstGrid[i].Data);
                        break;
                    }
                }
            }
            else if (m_Content == FriendPanelPageEnum.Page_最近 && m_relationMgr.IsInRelationList(GameCmd.RelationType.Relation_Contact, role.uid))
            {
                DeActiveFrendItem();
                List<UIFriendGrid> lstGrid = m_ctor_FriendScrollview.GetGrids<UIFriendGrid>(true);
                for (int i = 0; i < lstGrid.Count; i++)
                {
                    if (lstGrid[i].Data.uid == role.uid)
                    {
                        OnSelectFriend(lstGrid[i].Data);
                        break;
                    }
                }
            }
        }

        UpdateApplyRedPoint(FriendPanelPageEnum.Page_邮箱);
        UpdateApplyRedPoint(FriendPanelPageEnum.Page_最近);
    }

    void OnBtnsClick(BtnType type)
    {
        switch (type)
        {
            case BtnType.None:
                break;
            case BtnType.btn_search:
                {
                    if (!string.IsNullOrEmpty(m_input_Input.value))
                    {
                        uint uid = 0;
                        if (uint.TryParse(m_input_Input.value, out uid))
                        {
                            DataManager.Instance.Sender.RequestSearchUserRelation(uid, "");
                        }
                        else
                        {
                            DataManager.Instance.Sender.RequestSearchUserRelation(0, m_input_Input.value);
                        }
                    }
                    else
                    {
                        if (!m_relationMgr.CheckRecommendList())
                        {
                            DataManager.Instance.Sender.ReqRecomFriendList();
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        m_relationMgr.ValueUpdateEvent -= OnValueUpdateEventArgs;
        m_currSelectId = -1;

        if (privateChatManager != null)
        {
            privateChatManager.OnRefreshOutput = null;
            privateChatManager.OnNewChat = null;
        }
        Release();
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        privateChatManager.OnNewChat = OnAddText;

        MailEventRegister(false);
        Release();


    }
    void SetSecondTabActivity(FriendPanelPageEnum type)
    {
        foreach (var tab in m_dicFriendTabGrid.Values)
        {
            if (type == FriendPanelPageEnum.Page_最近)
            {
                if ((SecondTab)tab.TabID == SecondTab.Contacts || (SecondTab)tab.TabID == SecondTab.Interactive)
                {
                    tab.gameObject.SetActive(true);
                }
                else
                {
                    tab.gameObject.SetActive(false);
                }
            }
            else if (type == FriendPanelPageEnum.Page_好友)
            {
                if ((SecondTab)tab.TabID == SecondTab.Contacts || (SecondTab)tab.TabID == SecondTab.Interactive)
                {
                    tab.gameObject.SetActive(false);
                }
                else
                {
                    tab.gameObject.SetActive(true);
                }
            }
            else
            {
                tab.gameObject.SetActive(false);
            }
        }
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
    private void ToggleContent(FriendPanelPageEnum type)
    {
        SetSecondTabActivity(type);
        m_trans_PanelInput.gameObject.SetActive(false);
        m_label_labletips.text = "";
        privateChatManager.SilentFrienduid = 0;
        CurrContent = type;
        m_trans_MailContent.gameObject.SetActive(type == FriendPanelPageEnum.Page_邮箱);
        m_trans_FriendContent.gameObject.SetActive(type != FriendPanelPageEnum.Page_邮箱);
        switch (type)
        {
            case FriendPanelPageEnum.Page_最近:
                {
                    SetActiveFriendSecondTas(SecondTab.Contacts);
                    m_trans_addContent.gameObject.SetActive(false);
                    m_btn_btn_clear.gameObject.SetActive(false);

                    m_trans_bg_search.gameObject.SetActive(false);
                    m_trans_PanelInput.gameObject.SetActive(false);

                }
                break;
            case FriendPanelPageEnum.Page_好友:
                {
                    SetActiveFriendSecondTas(SecondTab.Friend);

                    m_trans_addContent.gameObject.SetActive(false);
                    m_btn_btn_clear.gameObject.SetActive(false);
                    m_trans_bg_search.gameObject.SetActive(false);
                    m_trans_PanelInput.gameObject.SetActive(false);

                }
                break;
            case FriendPanelPageEnum.Page_添加:
                {
                    m_relationMgr.newFriendReuquest = false;

                    m_trans_addContent.gameObject.SetActive(true);
                    m_btn_btn_clear.gameObject.SetActive(true);

                    m_trans_bg_search.gameObject.SetActive(true);
                    m_trans_PanelInput.gameObject.SetActive(false);

                    if (privateChatManager != null)
                    {
                        privateChatManager.OnRefreshOutput = null;
                        privateChatManager.OnNewChat = null;
                    }

                    privateChatManager.OnRefreshOutput = null;
                    privateChatManager.OnNewChat = null;
                    m_currRelationList = GameCmd.RelationType.Relation_FriendRequest;
                    RefreshLeftListUI();

                    if (!m_relationMgr.CheckRecommendList())
                    {
                        DataManager.Instance.Sender.ReqRecomFriendList();
                    }
                }
                break;
            case FriendPanelPageEnum.Page_邮箱:
                {
                    ShowMailPanel();
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 刷新好友二级页签状态
    /// </summary>
    private void RefreshFriendSecondTabStatus()
    {
        if (null != m_dicFriendTabGrid)
        {
            Dictionary<SecondTab, UITabGrid>.Enumerator em = m_dicFriendTabGrid.GetEnumerator();
            bool visible = false;
            while (em.MoveNext())
            {
                visible = (m_SecondTab == (SecondTab)em.Current.Value.TabID);
                em.Current.Value.SetHightLight(visible);
            }
        }
    }

    private void RefreshLeftListUI()
    {
        m_currSelectId = -1;
        m_label_labletips.text = "";
        RefreshFriendSecondTabStatus();
        m_relationMgr.GetRelationListByType(m_currRelationList, out m_lstRoleRelation);

        if (m_ctor_FriendScrollview != null)
        {
            if (m_lstRoleRelation != null)
            {
                m_ctor_FriendScrollview.CreateGrids(m_lstRoleRelation.Count);
            }

        }

        DeActiveFrendItem();
        ResetChatWindow();
    }


    /// <summary>
    /// 隐藏选中
    /// </summary>
    public void DeActiveFrendItem()
    {
        if (m_ctor_FriendScrollview == null)
        {
            return;
        }
        List<UIFriendGrid> lstGrid = m_ctor_FriendScrollview.GetGrids<UIFriendGrid>(true);
        for (int i = 0; i < lstGrid.Count; i++)
        {
            lstGrid[i].SetMaskState(false);
        }
    }

    public void OnSelectFriend(RoleRelation role, bool force = false)
    {

        if (role != null && !role.isSys)
        {
            m_bIsRobot = role.isRobot;

            if (m_currSelectId == (int)role.uid && !force)
            {
                return;
            }

            if (m_Content != FriendPanelPageEnum.Page_添加)
            {
                m_label_labletips.text = string.Format("正在与{0}聊天", role.name);
                if (privateChatManager != null)
                {
                    privateChatManager.SetCurrChatPlayer(role.uid, role.name);

                }
                m_trans_PanelInput.gameObject.SetActive(true);
            }
        }
        else
        {
            if (m_currSelectId == 0)
            {
                return;
            }
            //选择系统
            m_label_labletips.text = "";

            m_trans_PanelInput.gameObject.SetActive(false);
            if (privateChatManager != null)
            {
                privateChatManager.SetCurrChatPlayer(0, "");
            }
        }


        //设置当前私聊
        if (privateChatManager != null)
        {
            if (m_Content != FriendPanelPageEnum.Page_添加)
            {
                if (m_currSelectId != (int)role.uid)
                {
                    privateChatManager.OnRefreshOutput = OnRefreshText;
                    privateChatManager.InitCurrChatData();
                    privateChatManager.OnNewChat = OnAddText;
                }
            }
            else
            {
                privateChatManager.OnRefreshOutput = null;
                privateChatManager.OnNewChat = null;
            }
        }


        m_currSelectId = (role != null) ? (int)role.uid : 0;
    }


    void onClick_Btn_clear_Btn(GameObject caster)
    {
        DataManager.Instance.Sender.ClearRequestFriendList();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eUpdateFriendMsgTips)
        {
            //TODO系统聊天的tips
            uint id = (uint)param;
            if (m_currRelationList == GameCmd.RelationType.Relation_Friend || m_currRelationList == GameCmd.RelationType.Relation_Contact)
            {
                //                 for (int i = 0; i < m_lstSingleFrendItem.Count; i++)
                //                 {
                //                     if (m_lstSingleFrendItem[i].PlayerId == id)
                //                     {
                //                         m_lstSingleFrendItem[i].SetTipsState(true);
                //                     }
                //                 }  
            }
        }
        else if (msgid == UIMsgID.eChatWithPlayer)
        {
            RoleRelation data = (RoleRelation)param;
            ToggleContent(FriendPanelPageEnum.Page_最近);
            m_currRelationList = GameCmd.RelationType.Relation_Contact;
            RefreshLeftListUI();
            OnSelectFriend(data);
        }
        return base.OnMsg(msgid, param);
    }



    #region 有邮件未读的时候的红点提示

    private void UpdateApplyRedPoint(FriendPanelPageEnum type)
    {
        UITabGrid tabGrid = null;
        Dictionary<int, UITabGrid> dicTabs = null;
        if (dicUITabGrid.TryGetValue(1, out dicTabs))
        {
            if (dicTabs != null)
            {
                if (type == FriendPanelPageEnum.Page_邮箱)
                {
                    bool value = DataManager.Manager<MailManager>().HaveMailCanGet;
                    if (dicTabs.TryGetValue((int)FriendPanelPageEnum.Page_邮箱, out tabGrid))
                    {
                        tabGrid.SetRedPointStatus(value);
                    }
                }
                else
                {
                    bool haveChat = privateChatManager.HaveMsgFromFriend;
                    if (dicTabs.TryGetValue((int)FriendPanelPageEnum.Page_最近, out tabGrid))
                    {
                        tabGrid.SetRedPointStatus(haveChat);
                    }
                }
            }
        }
    }
    #endregion


}