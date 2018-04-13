using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Engine.Utility;
using Common;
using System.Linq;
using GameCmd;
using System;

/// <summary>
/// 主要写聊天UI逻辑
/// </summary>
public partial class FriendPanel : UIPanelBase, IChatInput
{
    public int MaxChatNum = 60;
    //Cache objs
    List<SingleChatItem> m_lstChatItem = new List<SingleChatItem>();
    //    List<SingleChatItem> m_lstChatItem_Sys = new List<SingleChatItem>();
    List<SingleChatItem> m_lstCurrChannel = new List<SingleChatItem>();
    float m_fTotalHeight = 0;
    GameObject m_chatItemPrefab;
    UISpriteEx m_voiceState = null;
    UILabel m_voiceStateLabel = null;
    //   GameObject m_chatItemSysPrefab;
    Dictionary<string, string> m_dictItemLink = new Dictionary<string, string>(1);

    List<GameObject> m_lstTimesTipsObjs = new List<GameObject>();
    DateTime m_lastdt;

  
    void InitOnShow()
    {
        UIEventListener.Get(m_btn_voice_input.gameObject).onPress = OnPreseVoice;
        m_voiceState = m_trans_voiceMicTips.Find("Spriteleft/Sprite").GetComponent<UISpriteEx>();
        m_voiceStateLabel = m_trans_voiceMicTips.Find("Spriteleft/Label").GetComponent<UILabel>();
        UIEventListener.Get(m_btn_voice_input.gameObject).onDrag = OnDragEnd;
     
    }
    void ChatInit()
    {

    }

    /// <summary>
    /// 刷新整个
    /// </summary>
    /// <param name="textList"></param>
    private void OnRefreshText(IEnumerable<ChatInfo> textList)
    {
        m_lastdt = GetTime(0);
        ResetChatWindow();
        StartCoroutine(WaitToAddText(textList));
        //OnAddText(textList);
    }

    IEnumerator WaitToAddText(IEnumerable<ChatInfo> textList)
    {
        var list = textList.TakeLast(MaxChatNum).ToList();

        Engine.Utility.Log.LogGroup("ZCX", "WaitToAddText : " + list.Count);
        m_chatPanel.alpha = 0;
        yield return new WaitForFixedUpdate();
        OnAddText(textList);
        m_chatPanel.gameObject.SetActive(false);//解决裁剪出错问题
        m_chatPanel.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        m_chatPanel.alpha = 1;
    }
    private void ResetChatWindow()
    {
        if (m_currRelationList == RelationType.Relation_Contact && m_trans_PanelInput.gameObject.activeSelf == false)
        {
            m_chatPanel.transform.parent.localPosition = Vector3.zero;
            Vector4 clip = m_chatPanel.baseClipRegion;
            clip.w = 580;//size.y
            clip.z = 700;//size.x
            m_chatPanel.baseClipRegion = clip;
            m_trans_chatroot.localPosition = new Vector3(0, 280, 0);
        }
        else
        {
            m_chatPanel.transform.parent.localPosition = new Vector3(0, 16, 0);
            Vector4 clip = m_chatPanel.baseClipRegion;
            clip.w = 460;//size.y
            clip.z = 700;//size.x
            m_chatPanel.baseClipRegion = clip;
            m_trans_chatroot.localPosition = new Vector3(3, 225, 0);
        }
        m_chatScrollView.transform.localPosition = Vector3.zero;
        m_chatPanel.clipOffset = UnityEngine.Vector2.zero;
        CacheChatItemObjs();
        m_fTotalHeight = 0;
    }

    private DateTime GetTime(uint timeStamp)
    {
        DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0);
        //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        //TimeSpan toNow = new TimeSpan((long)timeStamp);
        return DateTime.Now.AddSeconds(timeStamp - (DateTime.Now - UnixBase).TotalSeconds);
    }

    private void AddTimeTipsByDataTime(DateTime dt)
    {
        if (m_lastdt == null)
        {
            m_lastdt = dt;
        }

        if (m_lastdt.Hour != m_lastdt.Hour || m_lastdt.Minute != dt.Minute)
        {
            GameObject go = NGUITools.AddChild(m_trans_chatroot.gameObject, m_trans_timeTips.gameObject);
            go.GetComponentInChildren<UILabel>().text = dt.ToString("yyyy-MM-dd HH:mm");
            go.transform.localPosition = new Vector3(0, -m_fTotalHeight - 13f, 0);
            go.gameObject.SetActive(true);
            m_fTotalHeight += 35;
            m_lstTimesTipsObjs.Add(go);
        }
        m_lastdt = dt;
    }

    void AddTimeTips(uint timestamp)
    {
        DateTime dt = GetTime(timestamp);
        AddTimeTipsByDataTime(dt);
    }

    private void OnAddText(IEnumerable<ChatInfo> textList)
    {
        var list = textList.TakeLast(MaxChatNum).ToList();
        Engine.Utility.Log.LogGroup("ZCX", "OnAddText : " + list.Count);

        if (list.Count <= 0)
        {
            return;
        }

        if (m_chatPanel == null)
        {
            //Engine.Utility.Log.Error("m_chatPanel为空！！！");
            return;
        }

        Vector3[] corners = m_chatPanel.worldCorners;

        for (int i = 0; i < 4; ++i)
        {
            Vector3 v = corners[i];
            v = m_trans_chatroot.InverseTransformPoint(v);
            corners[i] = v;
        }

        float totalheight = 0;
        Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);
        SingleChatItem chatitem = null;
        foreach (var item in list)
        {
            AddTimeTips(item.Timestamp);
            chatitem = AddChatItem(item, ref m_lstChatItem, m_chatItemPrefab);
            if (chatitem != null)
            {
                float height = chatitem.GetHeight();
                m_fTotalHeight += chatitem.GetHeight();

                Transform t = chatitem.transform;
                float distance = t.localPosition.y - center.y;
                float offsetY = chatitem.GetOffsetY() - distance - m_chatPanel.baseClipRegion.w * 0.5f + m_chatPanel.clipSoftness.y;
                if (offsetY > 0)
                {
                    totalheight = offsetY;
                }
                m_lstCurrChannel.Add(chatitem);


            }
        }

        if (totalheight > 0)
        {
            m_chatScrollView.MoveRelative(new Vector3(0, totalheight, 0));
        }
    }

    SingleChatItem AddChatItem(ChatInfo text, ref List<SingleChatItem> list, GameObject prefab)
    {
        SingleChatItem chatitem = GetChatItem(ref list, prefab);
        Transform t = chatitem.transform;
        t.parent = m_trans_chatroot.transform;
        t.localPosition = new Vector3(0, -m_fTotalHeight + 5f, 0);
        t.localScale = Vector3.one;
        t.localRotation = Quaternion.identity;

        chatitem.SetChatInfo(Mathf.CeilToInt(m_chatPanel.baseClipRegion.z), text);
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

    private void CacheChatItemObjs()
    {
        Log.LogGroup(GameDefine.LogGroup.User_ZCX, "CacheObjs: " + m_trans_chatroot.childCount);

        int childNum = m_trans_chatroot.childCount;
        while (childNum > 0)
        {
            SingleChatItem item = m_trans_chatroot.GetChild(0).GetComponent<SingleChatItem>();
            if (item != null)
            {

                m_lstChatItem.Add(item);
                item.Clear();
                item.transform.parent = transform;
                item.gameObject.SetActive(false);
                item.name = "cacheItem" + childNum.ToString();

                //Log.LogGroup(GameDefine.LogGroup.User_ZCX, "CacheObjs: " + item.name);
            }
            else
            {
                //查找好友的对象列表
                Transform t = m_trans_chatroot.GetChild(0);
                t.parent = transform;
                GameObject.Destroy(t.gameObject);
            }
            childNum = m_trans_chatroot.childCount;
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
    void OnPreseVoice(GameObject go, bool prese)
    {
        m_trans_voiceMicTips.gameObject.SetActive(prese);
        if (prese)
        {
       
            m_voiceState.ChangeSprite(1);
            m_voiceStateLabel.text = "手指松开,立即发送";
            GVoiceManger.Instance.StartRecording();
        }
        else
        {
         
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
    void onClick_BtnVoice_Btn(GameObject caster)
    {
        //         TipsManager.Instance.ShowTips("敬请期待!!");
        //         return;
        m_trans_textinput.gameObject.SetActive(false);
        m_trans_inputvoice.gameObject.SetActive(true);
    }
    void onClick_BtnVoicetoteext_Btn(GameObject caster)
    {
        //         TipsManager.Instance.ShowTips("敬请期待!!");
        //         return;
        m_trans_textinput.gameObject.SetActive(true);
        m_trans_inputvoice.gameObject.SetActive(false);
    }

    void onClick_Voice_input_Btn(GameObject caster)
    {

    }
    void onClick_BtnEmoji_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.EmojiPanel, data: this);
    }

    void onClick_BtnSend_Btn(GameObject caster)
    {
        string strText = m_input_InputMsg.value;
        if (string.IsNullOrEmpty(strText))
        {
            return;
        }
        if (MainPlayerHelper.GetPlayerLevel() < privateChatOpenLv)
        {
            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Talk_System_NotMatchChatLevel, privateChatOpenLv);
            return;
        }

        foreach (var item in m_dictItemLink)
        {
            strText = strText.Replace(item.Key, item.Value);
        }
        DataManager.Manager<ChatDataManager>().SendPrivateChat(strText, m_bIsRobot);
        m_input_InputMsg.value = "";
    }

    /// <summary>
    /// 历史消息
    /// </summary>
    /// <param name="caster"></param>
    void onClick_BtnMessage_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.HistoryMsgPanel, data: this);
    }

    public void AppendText(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return;
        }
        m_input_InputMsg.value += input;

        m_input_InputMsg.selectionStart = m_input_InputMsg.value.Length;
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
            temp = m_input_InputMsg.value;
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
            m_input_InputMsg.value += name;
        }
        else
        {
            m_input_InputMsg.value = temp;
        }
        m_input_InputMsg.selectionStart = m_input_InputMsg.value.Length;
    }

    public void ResetPos()
    {

    }
}