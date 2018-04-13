using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Common;

public partial class SettingPanel : UIPanelBase
{
    public int MaxChatNum = 60;
    List<SingleChatItem> m_lstChatItem = new List<SingleChatItem>();
    GameObject m_chatItemPrefab = null;
    List<SingleChatItem> m_lstCurrChannel = new List<SingleChatItem>();
    float m_fTotalHeight = 0;
    UIPanel m_chatPanel = null;
    private void InitPrefab()
    {
        if (m_chatPanel == null)
        {
            m_chatPanel = m_scrollview_ChatScrollView.GetComponent<UIPanel>();
        }
        if (m_chatItemPrefab == null)
        {
            m_chatItemPrefab = UIManager.GetResGameObj(GridID.Uichatitemgrid) as UnityEngine.GameObject;
        }
       
    }
    void OnUpdateList(object obj, ValueUpdateEventArgs value)
    {
        if (value.key.Equals("FeedbackGM"))
        {
            List<GameCmd.FeedbackData> msg =  DataManager.Manager<Client.SettingManager>().FeedBackMsgs;
            OnFeedback(msg);
        }
    }
    private void CacheChat()
    {
        InitPrefab();
        m_fTotalHeight = 0;
        m_scrollview_ChatScrollView.ResetPosition();
        int childNum = m_trans_ChatItemRoot.childCount;
        while (childNum > 0)
        {
            SingleChatItem item = m_trans_ChatItemRoot.GetChild(0).GetComponent<SingleChatItem>();
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
                Transform t = m_trans_ChatItemRoot.GetChild(0);
                t.parent = transform;
                GameObject.Destroy(t.gameObject);
            }
            childNum = m_trans_ChatItemRoot.childCount;
        }
    }

    public void OnFeedback(List<GameCmd.FeedbackData> data)
    {
        List<ChatInfo> lstdata = new List<ChatInfo>();

        GameCmd.FeedbackData fd = null;
        for (int i = 0; i < data.Count; i++)
        {
            fd = data[i];

            ChatInfo info = new ChatInfo();
            info.Channel = GameCmd.CHATTYPE.CHAT_WORLD;
            info.Content = fd.content;
            info.IsMe = true;
            info.Id = (uint)fd.charid;
            info.job = MainPlayerHelper.GetMainPlayerJob();
            lstdata.Add(info);

            info = new ChatInfo();
            info.Channel = GameCmd.CHATTYPE.CHAT_GM;
            info.Content = string.IsNullOrEmpty(fd.reply) ? "已收到您的反馈，我们将尽快处理，请耐心等待" : fd.reply;
            info.IsMe = false;
            info.Id = (uint)fd.charid;
            info.job = 5;
            lstdata.Add(info);
        }
        OnAddText(lstdata);
    }

    /// <summary>
    /// 添加聊天信息
    /// </summary>
    /// <param name="textList"></param>
    private void OnAddText(IEnumerable<ChatInfo> textList)
    {
        var list = textList.TakeLast(MaxChatNum).ToList();
        //TODO 移除超出的聊天
        float totalheight = 0;

        Vector3[] corners = m_chatPanel.worldCorners;

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
                m_fTotalHeight += chatitem.GetHeight();
                Engine.Utility.Log.LogGroup("ZCX", "m_fTotalHeight :{0}", m_fTotalHeight);
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
            //可以立即刷新
            if (CheckCanUpdateMsg(lastChatItem, center.y))
            {
                Engine.Utility.Log.LogGroup("ZCX", "MoveRelative :{0}", totalheight);
                m_scrollview_ChatScrollView.MoveRelative(new Vector3(0, totalheight, 0));
            }
        }
        if (null != m_scrollview_ChatScrollView)
            NGUITools.MarkParentAsChanged(m_scrollview_ChatScrollView.gameObject);

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
        float offsetY = chatitem.GetOffsetY() - distance - m_chatPanel.baseClipRegion.w * 0.5f + m_chatPanel.clipSoftness.y;
        if (offsetY > height * 0.5f)//如果在panel外不能移动
        {
            return false;
        }
        return true;
    }

    SingleChatItem AddChatItem(ChatInfo text, ref List<SingleChatItem> list, GameObject prefab)
    {
        SingleChatItem chatitem = GetChatItem(ref list, prefab);
        Transform t = chatitem.transform;
        t.parent = m_trans_ChatItemRoot;
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
}