using System.Collections.Generic;
using UnityEngine;
using Common;
using Engine.Utility;
using System.Linq;
using Cmd;
using GameCmd;
using System;
using System.Collections;

/// <summary>
/// 聊天系统
/// </summary>
public partial class ChatPanel : UIPanelBase, IChatInput
{
    [Header("显示的聊天数量的最大值")]
    public int MaxChatNum = 60;
    ChatDataManager m_chatManager = null;
    public CHATTYPE m_CurrChannel = CHATTYPE.CHAT_NONE;
    GameObject m_chatItemPrefab = null;

    //Cache objs
    List<SingleChatItem> m_lstChatItem = new List<SingleChatItem>();
    List<SingleChatItem> m_lstCurrChannel = new List<SingleChatItem>();
    UIPanel m_panel = null;
    UILabel m_teamLable = null;
    GameObject m_goTabGhost = null;//神魔频道
    Transform m_transTabSys = null;//系统频道
    Transform m_transRedPacket = null;//红包
    float m_fTotalHeight = 0;
    float startPosX = -350f;
    float m_fContentWidth = 0;
    /// <summary>
    /// 需要刷新移动的高度
    /// </summary>
    float m_fMoveTotalHeight = 0;
    int m_noreadMsg = 0;

    UILabel m_btnSendLable;
    Dictionary<string, string> m_dictItemLink = new Dictionary<string, string>(1);
    List<string> m_lstdepende;

    UISpriteEx m_voiceState = null;
    UILabel m_voiceStateLabel = null;
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        if (null != m_trans_OffsetContent)
        {
            startPosX = m_trans_OffsetContent.localPosition.x;
            m_fContentWidth = NGUIMath.CalculateRelativeWidgetBounds(m_trans_OffsetContent).size.x;
        }

        m_chatManager = DataManager.Manager<ChatDataManager>();

        m_input_Input.characterLimit = GameTableManager.Instance.GetGlobalConfig<int>("ChatMaxCharacter");

        m_chatItemPrefab = UIManager.GetResGameObj(GridID.Uichatitemgrid) as UnityEngine.GameObject;

        m_panel = m_scrollview_ChatScrollView.GetComponent<UIPanel>();
        m_btnSendLable = m_btn_BtnSend.GetComponentInChildren<UILabel>();

        GameObject btnShowMsg = m_trans_newmessage_warning.Find("btnShowMsg").gameObject;
        UIEventListener.Get(btnShowMsg).onClick = OnClickRefreshMsg;

        UIToggle[] toggles = m_trans_ChanelTabs.GetComponentsInChildren<UIToggle>();
        for (int i = 0; i < toggles.Length; i++)
        {
            var item = toggles[i];
            var channelName = item.name;
            BtnType btntype = (BtnType)System.Enum.Parse(typeof(BtnType), channelName);
            EventDelegate.Add(item.onChange, () =>
            {
                if (btntype != BtnType.TabRed)
                {
                    if (item.value)
                    {
                        this.OnToggleValueChange(channelName);
                    }
                }
            });

            if (btntype == BtnType.TabTeam)
            {
                m_teamLable = item.GetComponentInChildren<UILabel>();
            }
            else if (btntype == BtnType.TabGhost)
            {
                m_goTabGhost = item.gameObject;
            }
            else if (btntype == BtnType.TabSystem)
            {
                m_transTabSys = item.transform;
            }
            else if (btntype == BtnType.TabRed)
            {
                m_transRedPacket = item.transform;
                UIEventListener.Get(item.gameObject).onClick = OnClickRedPacket;
            }
            item.transform.localPosition = new Vector3(-3, 320 - i * 65, 0);
        }
        m_scrollview_ChatScrollView.onStoppedMoving += OnDragPanelFinished;
        m_trans_voiceMicTips.gameObject.SetActive(false);
        UIEventListener.Get(m_btn_voice_input.gameObject).onPress = OnPreseVoice;
        UIEventListener.Get(m_btn_voice_input.gameObject).onDrag = OnDragEnd;

        m_voiceState = m_trans_voiceMicTips.Find("Spriteleft/Sprite").GetComponent<UISpriteEx>();
        m_voiceStateLabel = m_trans_voiceMicTips.Find("Spriteleft/Label").GetComponent<UILabel>();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        m_trans_inputvoice.gameObject.SetActive(false);

        SetTeamChannelTitle();
        CheckClan(m_CurrChannel);
        CheckTeamChanne(m_CurrChannel);

        if (null != m_trans_OffsetContent)
        {
            //m_trans_OffsetContent.localPosition = new Vector3(startPosX, 0, 0);
            m_trans_OffsetContent.localPosition = new Vector3(startPosX, m_trans_OffsetContent.localPosition.y, m_trans_OffsetContent.localPosition.z);
        }
    }

    public override UIPanelBase.PanelData GetPanelData()
    {
        UIPanelBase.PanelData pd = base.GetPanelData();
        pd.Data = false;
        return pd;
    }

    protected override void OnHide()
    {
        base.OnHide();
        if (null != m_trans_OffsetContent)
        {
            TweenPosition tp = m_trans_OffsetContent.GetComponent<TweenPosition>();
            if (null != tp)
            {
                tp.enabled = false;
            }
            //m_trans_OffsetContent.localPosition = new Vector3(-m_fContentWidth, 0, 0);
            m_trans_OffsetContent.localPosition = new Vector3(-m_fContentWidth, m_trans_OffsetContent.localPosition.y, m_trans_OffsetContent.localPosition.z);
        }
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        m_chatManager.OnAddOutput -= OnAddText;
    }
    void OnDragPanelFinished()
    {
        if (m_trans_newmessage_warning.gameObject.activeSelf)
        {
            if (m_lstCurrChannel.Count <= 0)
            {
                return;
            }

            Vector3[] corners = m_panel.worldCorners;

            for (int i = 0; i < 4; ++i)
            {
                Vector3 v = corners[i];
                v = m_trans_ChatItemRoot.InverseTransformPoint(v);
                corners[i] = v;
            }

            int childNum = m_lstCurrChannel.Count;

            Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);

            if (CheckCanUpdateMsg(m_lstCurrChannel[childNum - 1], center.y))
            {
                ResetNoReadMsg();
            }
        }
    }

    CHATTYPE GetChatType(BtnType type)
    {
        switch (type)
        {
            case BtnType.None:
                break;
            case BtnType.TabWorld:
                return CHATTYPE.CHAT_WORLD;
            case BtnType.TabTeam:
                {
                    if (DataManager.Manager<TeamDataManager>().IsJoinTeam == true)
                    {
                        return CHATTYPE.CHAT_TEAM;
                    }
                    else
                    {
                        return CHATTYPE.CHAT_RECRUIT;
                    }
                }
            case BtnType.TabClan:
                return CHATTYPE.CHAT_CLAN;
            case BtnType.TabNear:
                return CHATTYPE.CHAT_MAP;
            case BtnType.TabGhost:
                return CHATTYPE.CHAT_DEMON;
            case BtnType.TabSystem:
                return CHATTYPE.CHAT_SYS;
            case BtnType.Max:
                break;
            default:
                break;
        }
        return CHATTYPE.CHAT_NONE;
    }
    void OnClickRedPacket(GameObject go)
    {


        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RedEnvelopePanel, panelShowAction: (panel) =>
        {
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ChatPanel);
        });
        return;

    }
    void OnToggleValueChange(string channelName)
    {
        Log.LogGroup(GameDefine.LogGroup.User_ZCX, channelName);
        BtnType btntype = (BtnType)System.Enum.Parse(typeof(BtnType), channelName);
        if (btntype == BtnType.TabRed)
        {
            return;
        }
        CHATTYPE chatType = GetChatType(btntype);
        ResetChannel(chatType);

    }

    private void ResetChannel(CHATTYPE chatType)
    {

        m_chatManager.SetChannelFilter(chatType);
        //切换频道后，要重新绑定一下消息相应函数
        m_chatManager.OnRefresh = OnRefreshText;
        m_chatManager.InitOutputText();
        m_chatManager.OnAddOutput = OnAddText;
        m_chatManager.SetChatInputType(chatType);

        m_CurrChannel = chatType;
        m_chatManager.CurChatType = chatType;
        ResetNoReadMsg();

        m_scrollview_ChatScrollView.panel.clipOffset = UnityEngine.Vector2.zero;
        m_scrollview_ChatScrollView.transform.localPosition = new Vector3(0, 23, 0);

        Vector4 baseClipRegion = m_scrollview_ChatScrollView.panel.baseClipRegion;

        m_trans_inputnormal.parent.gameObject.SetActive(m_CurrChannel != CHATTYPE.CHAT_SYS);

        if (m_CurrChannel == CHATTYPE.CHAT_SYS || m_CurrChannel == CHATTYPE.CHAT_RECRUIT)
        {
            //m_btn_btnAutoplay.transform.parent.gameObject.SetActive(false);
            m_btn_BtnSend.transform.parent.gameObject.SetActive(false);
            //y340 sizey 700
            //m_trans_ChatItemRoot.localPosition = new Vector3(0, 330, 0);
            //baseClipRegion.w = 680;
            //m_sprite_chatBg_di.height = (int)baseClipRegion.w;

        }
        else
        {
            //m_label_autoPlayLabel.text = string.Format("自动播放{0}频道语音", DataManager.Manager<ChatDataManager>().GetChannelStr(chatType));
            // m_btn_btnAutoplay.transform.parent.gameObject.SetActive(true);
            m_btn_BtnSend.transform.parent.gameObject.SetActive(true);


        }

        m_scrollview_ChatScrollView.panel.baseClipRegion = baseClipRegion;

        m_lstCurrChannel.Clear();

        CheckClan(chatType);
        CheckTeamChanne(chatType);
        SetTeamChannelTitle();

        if (!m_chatManager.EqualsCurrChannel(CHATTYPE.CHAT_WORLD))
        {
            //m_label_worldTip.gameObject.SetActive(false);
            //Vector3 pos = m_btn_btnAutoplay.transform.localPosition;
            //pos.x = -67f;
            //m_btn_btnAutoplay.transform.localPosition = pos;
        }
        else
        {
            //Vector3 pos = m_btn_btnAutoplay.transform.localPosition;
            //pos.x = -173f;
            //m_btn_btnAutoplay.transform.localPosition = pos;
            //m_label_worldTip.gameObject.SetActive(true);
        }


        m_trans_inputvoice.gameObject.SetActive(false);
        if (m_chatManager.IsCanSendMsg())
        {
            m_btnSendLable.text = "发送";

            UILabel label = m_btn_voice_input.transform.Find("Label").GetComponent<UILabel>();
            if (label != null)
            {
                label.text = "按住说话";
            }
        }
        //m_btn_btnAutoplay.GetComponent<UIToggle>().value = DataManager.Manager<ChatDataManager>().IsAutoPlayVoice(chatType);
    }

    void CheckClan(CHATTYPE chatType)
    {
        m_trans_NoClan.gameObject.SetActive(chatType == CHATTYPE.CHAT_CLAN && !DataManager.Manager<ClanManger>().IsJoinClan);
    }

    void CheckTeamChanne(CHATTYPE chatType)
    {
        m_trans_inputnormal.gameObject.SetActive(chatType == CHATTYPE.CHAT_RECRUIT || chatType == CHATTYPE.CHAT_SYS ? false : true);
        if (chatType == CHATTYPE.CHAT_RECRUIT)
        {
            m_label_bottomTips.text = "您当前没有加入队伍，不能发言";
        }
        else if (chatType == CHATTYPE.CHAT_SYS)
        {
            m_label_bottomTips.text = "系统频道玩家无法发言";
        }
        else
        {
            m_label_bottomTips.text = "";
        }
    }

    void onClick_Btn_joinClan_Btn(GameObject caster)
    {
        if (!DataManager.Manager<ClanManger>().IsClanEnable)
        {
            TipsManager.Instance.ShowTips(string.Format("{0}级开放", ClanManger.ClanUnlockLevel));
        }
        else
        {
            onClick_Btnback_Btn(gameObject);
            if (DataManager.Manager<ClanManger>().IsJoinClan
                && DataManager.Manager<ClanManger>().IsJoinFormal)
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ClanPanel);
            }
            else
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ClanCreatePanel);
            }
        }
    }
    /// <summary>
    /// 重新全部刷新界面
    /// </summary>
    /// <param name="textList"></param>
    private void OnRefreshText(IEnumerable<ChatInfo> textList)
    {
        //cache objs
        CacheObjs();
        m_fTotalHeight = 0;

        m_scrollview_ChatScrollView.ResetPosition();
        StartCoroutine(WaitToAddText(textList));
    }

    IEnumerator WaitToAddText(IEnumerable<ChatInfo> textList)
    {
        m_panel.alpha = 0;
        yield return new WaitForFixedUpdate();
        OnAddText(textList);
        m_panel.gameObject.SetActive(false);//解决裁剪出错问题
        m_panel.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        m_panel.alpha = 1;
    }

    private void CacheObjs()
    {
        Log.LogGroup(GameDefine.LogGroup.User_ZCX, "CacheObjs: " + m_trans_ChatItemRoot.childCount);

        int childNum = m_trans_ChatItemRoot.childCount;
        while (childNum > 0)
        {
            SingleChatItem item = m_trans_ChatItemRoot.GetChild(0).GetComponent<SingleChatItem>();
            if (item != null)
            {
                m_lstChatItem.Add(item);
                item.Clear();
                item.transform.parent = transform;
                item.Release();
                item.gameObject.SetActive(false);
                item.name = "cacheItem" + childNum.ToString();

                Log.LogGroup(GameDefine.LogGroup.User_ZCX, "CacheObjs: " + item.name);
            }
            else
            {
                GameObject.Destroy(item.transform);
            }
            childNum = m_trans_ChatItemRoot.childCount;
        }
    }

    /// <summary>
    /// 添加聊天信息
    /// </summary>
    /// <param name="textList"></param>
    private void OnAddText(IEnumerable<ChatInfo> textList)
    {
        var list = textList.TakeLast(MaxChatNum).ToList();
        //TODO 移除超出的聊天
        Log.LogGroup(GameDefine.LogGroup.User_ZCX, "TextList count " + list.Count);
        float totalheight = 0;

        Vector3[] corners = m_panel.worldCorners;

        for (int i = 0; i < 4; ++i)
        {
            Vector3 v = corners[i];
            v = m_trans_ChatItemRoot.InverseTransformPoint(v);
            corners[i] = v;
        }

        //         float extents = 0;
        int childNum = m_lstCurrChannel.Count;
        //         for (int i = 0; i < childNum; i++)
        //         {
        //             SingleChatItem item = m_lstCurrChannel[i];
        //             if (item != null)
        //             {
        //                 extents += item.GetHeight();
        //             }   
        //         }
        //缓存最后一个 用于判断是否可以刷新
        SingleChatItem lastChatItem = null;
        if (childNum > 0)
        {
            lastChatItem = m_lstCurrChannel[childNum - 1];
        }
        //  extents *= 0.5f;

        Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);
        //bool allWithinRange = true;
        //float ext2 = extents * 2f;
        bool flag = false;
        SingleChatItem chatitem = null;
        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            chatitem = AddChatItem(item, ref m_lstChatItem, m_chatItemPrefab);

            if (chatitem != null)
            {
                chatitem.m_parent = this;
                //float height = chatitem.GetHeight();
                m_fTotalHeight += chatitem.GetHeight();
                Engine.Utility.Log.LogGroup("ZCX", "m_fTotalHeight :{0}", m_fTotalHeight);
                Transform t = chatitem.transform;
                float distance = t.localPosition.y - center.y;
                float offsetY = chatitem.GetOffsetY() - distance - m_panel.baseClipRegion.w * 0.5f + m_panel.clipSoftness.y;
                if (offsetY > 0)
                {
                    totalheight = offsetY;
                }
                m_lstCurrChannel.Add(chatitem);

                if (!flag && chatitem.m_chatdata != null && m_chatManager.IsCanAutoPlayVoice(chatitem.m_chatdata.Channel) && !chatitem.m_chatdata.played && !GVoiceManger.Instance.IsPlaying)
                {
                    flag = true;
                    chatitem.PlayeVoice();
                }
            }

        }

        if (totalheight > 0)
        {
            //可以立即刷新
            if (CheckCanUpdateMsg(lastChatItem, center.y))
            {
                Engine.Utility.Log.LogGroup("ZCX", "MoveRelative :{0}", totalheight);
                m_scrollview_ChatScrollView.MoveRelative(new Vector3(0, totalheight, 0));
            }
            else
            {
                m_fMoveTotalHeight = totalheight;
                m_noreadMsg += list.Count;
                UpdateNoReadMsgTips(m_noreadMsg);
            }
        }

    }

    bool CheckCanUpdateMsg(SingleChatItem chatitem, float centerY)
    {
        if (chatitem == null)
        {
            return true;
        }

        float height = chatitem.GetHeight();

        Transform t = chatitem.transform;
        float distance = t.localPosition.y - centerY;
        float offsetY = chatitem.GetOffsetY() - distance - m_panel.baseClipRegion.w * 0.5f + m_panel.clipSoftness.y;
        if (offsetY > height * 0.5f)//如果在panel外不能移动
        {
            return false;
        }
        return true;
    }

    void UpdateNoReadMsgTips(int num)
    {
        m_trans_newmessage_warning.gameObject.SetActive(true);

        UILabel noreadlable = m_trans_newmessage_warning.Find("newMessageLabel").GetComponent<UILabel>();
        noreadlable.text = string.Format("{0}条未读消息", num);
    }

    #region ChatItem

    SingleChatItem AddChatItem(ChatInfo text, ref List<SingleChatItem> list, GameObject prefab)
    {
        SingleChatItem chatitem = GetChatItem(ref list, prefab);
        Transform t = chatitem.transform;
        t.parent = m_trans_ChatItemRoot.transform;
        t.localPosition = new Vector3(0, -m_fTotalHeight + 5f, 0);
        t.localScale = Vector3.one;
        t.localRotation = Quaternion.identity;

        chatitem.SetChatInfo(Mathf.CeilToInt(m_panel.baseClipRegion.z), text);
        return chatitem;
    }

    SingleChatItem GetChatItem(ref List<SingleChatItem> list, GameObject prefab)
    {
        SingleChatItem chatItem = null;
        if (list.Count > 0)
        {
            chatItem = list[0];
            list.RemoveAt(0);
        }
        else
        {
            GameObject obj = GameObject.Instantiate(prefab) as GameObject;
            chatItem = obj.AddComponent<SingleChatItem>();
        }
        chatItem.gameObject.SetActive(true);
        return chatItem;
    }
    #endregion


    void ResetNoReadMsg()
    {
        m_trans_newmessage_warning.gameObject.SetActive(false);
        m_fMoveTotalHeight = 0;
        m_noreadMsg = 0;
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eResetChatWindowPosX)
        {
            SetTeamChannelTitle();

            if (m_CurrChannel == CHATTYPE.CHAT_TEAM || m_CurrChannel == CHATTYPE.CHAT_RECRUIT)
            {
                CHATTYPE chatType = GetChatType(BtnType.TabTeam);
                if (chatType != m_CurrChannel)
                {
                    ResetChannel(chatType);
                }
            }

            transform.GetChild(0).localPosition = new Vector3(startPosX, 0, 0);
            if (m_CurrChannel == CHATTYPE.CHAT_CLAN)
            {
                if (!DataManager.Manager<ClanManger>().IsJoinClan && m_trans_ChatItemRoot.childCount > 0)
                {
                    ResetChannel(m_CurrChannel);
                }
            }
        }
        else if (msgid == UIMsgID.eRefreshSendBtnLable)
        {
            int cd = (int)param;
            if (m_trans_inputnormal.gameObject.activeSelf)
            {
                m_btnSendLable.text = string.Format("发送({0})", cd);
                if (cd <= 0)
                {
                    m_btnSendLable.text = "发送";
                    UILabel label = m_btn_voice_input.transform.Find("Label").GetComponent<UILabel>();
                    if (label != null)
                    {
                        label.text = "按住说话";
                    }
                }
            }
            if (m_trans_inputvoice.gameObject.activeSelf)
            {
                UILabel label = m_btn_voice_input.transform.Find("Label").GetComponent<UILabel>();
                if (label != null)
                {
                    label.text = string.Format("按住说话({0})", cd);
                    if (cd <= 0)
                    {
                        label.text = "按住说话";
                        m_btnSendLable.text = "发送";
                    }
                }
            }



        }
        return base.OnMsg(msgid, param);
    }

    /// <summary>
    /// 根据组队情况设置频道名字
    /// </summary>
    private void SetTeamChannelTitle()
    {
        if (m_teamLable != null)
        {
            //if (DataManager.Manager<TeamDataManager>().TeamState == TeamState.InTeam)
            if (DataManager.Manager<TeamDataManager>().IsJoinTeam == true)
            {
                m_teamLable.text = DataManager.Manager<ChatDataManager>().GetChannelStr(CHATTYPE.CHAT_TEAM);
            }
            else
            {
                m_teamLable.text = DataManager.Manager<ChatDataManager>().GetChannelStr(CHATTYPE.CHAT_RECRUIT);
            }
        }

        if (m_goTabGhost != null)
        {
            int nlevel = GameTableManager.Instance.GetGlobalConfig<int>("ChatDemonLevel");
            m_goTabGhost.SetActive(nlevel <= MainPlayerHelper.GetPlayerLevel());
            UIToggle[] toggles = m_trans_ChanelTabs.GetComponentsInChildren<UIToggle>();
            for (int i = 0; i < toggles.Length; i++)
            {
                var item = toggles[i];
                item.transform.localPosition = new Vector3(-3, 320 - i * 65, 0);
            }
            //if (!m_goTabGhost.activeSelf && m_transTabSys != null)
            //{
            //    m_transTabSys.localPosition = m_goTabGhost.transform.localPosition;
            //}
            //else
            //{
            //    Vector3 pos = m_goTabGhost.transform.localPosition;
            //    pos.y = -92;
            //    m_transTabSys.localPosition = pos;
            //}
        }
    }

    public void AppendText(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return;
        }
        m_input_Input.value += input;

        m_input_Input.selectionStart = m_input_Input.value.Length;
    }

    public void AddLinkerItem(string itemName, uint thisID, uint quality, int type)
    {
        string name = "[" + itemName + "]";
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        string temp = "";
        if (m_dictItemLink.Count > 0)
        {
            temp = m_input_Input.value;
        }
        foreach (var item in m_dictItemLink)
        {
            temp = temp.Replace(item.Key, name);
        }

        m_dictItemLink.Clear();

        string strLink = ChatDataManager.GetItemHrefString(name, MainPlayerHelper.GetPlayerID(), thisID, quality, false, type);
        name = DataManager.Manager<TextManager>().ReplaceSensitiveWord(name, TextManager.MatchType.Max);

        m_dictItemLink.Add(name, strLink);

        if (string.IsNullOrEmpty(temp))
        {
            m_input_Input.value += name;
        }
        else
        {
            m_input_Input.value = temp;
        }
        m_input_Input.selectionStart = m_input_Input.value.Length;
    }

    public void ResetPos()
    {
        Vector3 pos = transform.GetChild(0).localPosition;
        pos.y = 0;
        transform.GetChild(0).localPosition = pos;
    }

    #region  click
    void onClick_Btnback_Btn(GameObject caster)
    {
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MsgSettingPanel))
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.MsgSettingPanel);
        HideSelf();
        //if (null != m_trans_OffsetContent)
        //{
        //    TweenPosition.Begin(m_trans_OffsetContent.gameObject, 0.1f, new Vector3(-m_fContentWidth, 0, 0));
        //}
    }



    void onClick_BtnEmoji_Btn(GameObject caster)
    {
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.EmojiPanel))
        {
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.EmojiPanel);
            Vector3 pos = transform.GetChild(0).localPosition;
            pos.y = 0;
            transform.GetChild(0).localPosition = pos;
        }
        else
        {
            Vector3 pos = transform.GetChild(0).localPosition;
            pos.y = 280;
            transform.GetChild(0).localPosition = pos;
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.EmojiPanel, data: this);
        }
    }

    void onClick_BtnSend_Btn(GameObject caster)
    {
        if (m_chatManager.EqualsCurrChannel(CHATTYPE.CHAT_WORLD))
        {
            int worldChatLimitLv = DataManager.Manager<ChatDataManager>().WorldChatLimitLv;
            if (DataManager.Instance.PlayerLv < worldChatLimitLv)
            {
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Talk_World_LvLimit, worldChatLimitLv);
                return;
            }

            if (UserData.Coupon < m_chatManager.ChatWorldCost)
            {
                TipsManager.Instance.ShowTipsById(4);
                return;
            }
        }

        if (m_chatManager.EqualsCurrChannel(CHATTYPE.CHAT_TEAM))
        {
            //if (DataManager.Manager<TeamDataManager>().TeamState != TeamState.InTeam)
            if (DataManager.Manager<TeamDataManager>().IsJoinTeam == false)
            {
                return;
            }
        }
        else if (m_chatManager.EqualsCurrChannel(CHATTYPE.CHAT_DEMON))
        {
            int nlevel = GameTableManager.Instance.GetGlobalConfig<int>("ChatDemonLevel");
            if (nlevel > MainPlayerHelper.GetPlayerLevel())
            {
                return;
            }
        }

        string strText = m_input_Input.value;
        if (string.IsNullOrEmpty(strText))
        {
            return;
        }
        Log.LogGroup(GameDefine.LogGroup.User_ZCX, strText);

        foreach (var item in m_dictItemLink)
        {
            strText = strText.Replace(item.Key, item.Value);
        }

        if (m_chatManager.SendChatText(strText))
        {
            m_dictItemLink.Clear();

            m_input_Input.value = "";
            m_input_Input.isSelected = false;
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.EmojiPanel))
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.EmojiPanel);
    }


    /// <summary>
    /// 设置默认消息
    /// </summary>
    /// <param name="caster"></param>
    void onClick_BtnMessage_Btn(GameObject caster)
    {
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MsgSettingPanel))
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.MsgSettingPanel);
        else
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MsgSettingPanel);

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.EmojiPanel))
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.EmojiPanel);
    }

    //自动播放
    public void PlayNext(uint time)
    {
        for (int i = 0; i < m_lstCurrChannel.Count; i++)
        {
            if (m_lstCurrChannel[i].m_chatdata != null && m_lstCurrChannel[i].m_chatdata.Timestamp == time)
            {
                if (i + 1 < m_lstCurrChannel.Count)
                {
                    m_lstCurrChannel[i + 1].PlayeVoice();
                }
            }
        }
    }
    void onClick_BtnAutoplay_Btn(GameObject caster)
    {
        bool flag = caster.GetComponent<UIToggle>().value;
        m_chatManager.SetAutoPlayVoice(m_CurrChannel, flag);
    }

    void OnClickRefreshMsg(GameObject go)
    {
        m_scrollview_ChatScrollView.MoveRelative(new Vector3(0, m_fMoveTotalHeight, 0));
        ResetNoReadMsg();
    }
    void OnBtnsClick(BtnType btntype)
    {

    }

    void OnPreseVoice(GameObject go, bool prese)
    {
        m_trans_voiceMicTips.gameObject.SetActive(prese);
        if (prese)
        {
            //             if (GVoiceManger.Instance.JoinRoomType != JoinRoomEnum.None)
            //             {
            //                 TipsManager.Instance.ShowTips(LocalTextType.Talk_ActualTime_liaotianshizhongwufafasongyuyinxiaoxi);
            //                 return;
            //             }
            GVoiceManger.Instance.SoundMute(false);
            m_voiceState.ChangeSprite(1);
            m_voiceStateLabel.text = "手指松开,立即发送";
            GVoiceManger.Instance.StartRecording();
        }
        else
        {
            //             if (GVoiceManger.Instance.JoinRoomType != JoinRoomEnum.None)
            //             {
            //                 return;
            //             }
            GVoiceManger.Instance.SoundMute(true);
            bool send = Focus(go);
            if (send)
            {
                GVoiceManger.Instance.StopRecording();
                GVoiceManger.Instance.UploadRecordedFile();
            }
            else
            {
                GVoiceManger.Instance.StopRecording();
                GVoiceManger.Instance.SetRealTimeModel();
            }
        }
    }

    bool Focus(GameObject go)
    {
        Camera camera = Util.UICameraObj.GetComponent<Camera>();
        UnityEngine.Vector2 touchPos = UICamera.currentTouch.pos;
        Ray ray = camera.ScreenPointToRay(touchPos);
        int mask = camera.cullingMask & (int)UICamera.current.eventReceiverMask;
        float dist = (UICamera.current.rangeDistance > 0f) ? UICamera.current.rangeDistance : camera.farClipPlane - camera.nearClipPlane;
        RaycastHit[] hits = Physics.RaycastAll(ray, dist, mask);
        if (hits.Length > 1)
        {
            for (int b = 0; b < hits.Length; ++b)
            {
                if (hits[b].collider.gameObject.name.Equals(go.name))
                {
                    return true;
                }
            }
        }
        return false;
    }
    void OnDragEnd(GameObject go, UnityEngine.Vector2 pos)
    {
        bool send = Focus(go);
        if (m_voiceState != null)
        {
            m_voiceState.ChangeSprite(send ? 1 : 2);
        }

        if (m_voiceStateLabel != null)
        {
            m_voiceStateLabel.text = send ? "手指松开,立即发送" : "手指滑开,取消发送";
        }
    }

    void onClick_BtnVoicetoteext_Btn(GameObject caster)
    {
        m_trans_inputnormal.gameObject.SetActive(true);
        m_trans_inputvoice.gameObject.SetActive(false);
    }

    void onClick_Voice_input_Btn(GameObject caster)
    {

    }

    void onClick_BtnVoice_Btn(GameObject caster)
    {
        m_trans_inputnormal.gameObject.SetActive(false);
        m_trans_inputvoice.gameObject.SetActive(true);
    }

    void onClick_HornBtn_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.HornPanel);

    }

    void onClick_SettingBtn_Btn(GameObject go)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ChannelSettingPanel);
    }
    #endregion



}

