using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 聊天历史记录消息面板
/// </summary>
partial class HistoryMsgPanel : UIPanelBase
{
    IChatInput m_chatInput;
    List<UIButton> btn_historymessage = new List<UIButton>(5);
    protected override void OnLoading()
    {
        base.OnLoading();

        UIButton[] btns = m_sprite_btn_historymessage.GetComponentsInChildren<UIButton>();
        btn_historymessage.AddRange(btns);
        btn_historymessage.Sort(MsgSettingPanel.SortByName);
        int index = 0;
        foreach (var item in btn_historymessage)
        {
            item.GetComponentInChildren<UILabel>().text = "";
            UIEventListener.Get(item.gameObject).onClick = OnBtnHistory;
            item.gameObject.name = (index++).ToString();
        }
    }

    void OnBtnHistory(GameObject go)
    {
        if (m_chatInput == null) return;
        int historyindex = int.Parse(go.name);
        string itemName = "";
        uint itemthisid;
        uint quality;
        int type;
        if (DataManager.Manager<ChatDataManager>().CheckIsLinkItem(historyindex, ref itemName, out itemthisid, out quality,out type))
        {
            m_chatInput.AddLinkerItem(itemName, itemthisid, quality, type);
        }
        else
        {
            List<string> history = DataManager.Manager<ChatDataManager>().GetHistoryMsg();
            if (historyindex >= 0 && historyindex < history.Count)
            {
                m_chatInput.AppendText(history[historyindex]);
            }
        }
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_chatInput = null;
        if (data is IChatInput)
        {
            m_chatInput = (IChatInput)data;
        }

        if (m_chatInput != null)
        {
            if (m_chatInput is FriendPanel)
            {
                transform.GetChild(0).localPosition = new Vector3(-114,-156,0);
            }
            else if (m_chatInput is HornPanel)
            {
                transform.GetChild(0).localPosition = Vector3.zero;
            }
        }
        List<string> history = DataManager.Manager<ChatDataManager>().GetHistoryMsg();
        for (int i = 0; i < history.Count; i++)
        {
            btn_historymessage[i].GetComponentInChildren<UILabel>().text = history[i];
        }
    }

    void onClick_Btn_close_Btn(GameObject caster)
    {
        this.HideSelf();
    }
}