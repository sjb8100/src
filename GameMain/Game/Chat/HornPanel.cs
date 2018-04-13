using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 喇叭使用
/// </summary>
partial class HornPanel : UIPanelBase,IChatInput
{
    Dictionary<string, string> m_dictItemLink = new Dictionary<string, string>(1);
    int m_nItemNum = 0;
    uint m_nItemPrice = 0;
    protected override void OnAwake()
    {
        base.OnAwake();
        m_input_Input.characterLimit = GameTableManager.Instance.GetGlobalConfig<int>("ChatMaxCharacter");
        m_input_Input.onChange.Add(new EventDelegate(OnChangeText));
    }



    void OnChangeText()
    {
        m_label_characterNum.text = string.Format("剩余字数:{0}", m_input_Input.characterLimit - m_input_Input.value.Length);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        uint m_baseid = GameTableManager.Instance.GetClientGlobalConst<uint>("Item", "HornID");
        OnChangeText();

        m_nItemNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_baseid);
        m_label_num.text = string.Format("{0}", m_nItemNum);


        if (m_nItemNum > 0)
        {
            m_label_goldNum.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            m_label_goldNum.transform.parent.gameObject.SetActive(true);

            table.PointConsumeDataBase pcd = GameTableManager.Instance.GetTableItem<table.PointConsumeDataBase>( m_baseid);
            if (pcd != null)
            {
                m_nItemPrice = pcd.buyPrice;
                int goldNum = MainPlayerHelper.GetMoneyNumByType(ClientMoneyType.YuanBao);
                m_label_goldNum.text = string.Format("{0}/{1}", goldNum,pcd.buyPrice);
            }
            else
            {
                Engine.Utility.Log.Error("Get PointConsumeDataBase error id {0}", m_baseid);
            }
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        m_input_Input.value = "";
        m_dictItemLink.Clear();
    }

    public void AppendText(string emoji)
    {
        m_input_Input.value += emoji;
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
        transform.localPosition = Vector3.zero;
    }

    void onClick_Horn_close_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    void onClick_Addbtn_Btn(GameObject caster)
    {    
        uint itemBaseId = GameTableManager.Instance.GetClientGlobalConst<uint>("Item", "HornID");
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: itemBaseId);
    }

    void onClick_Btn_send_Btn(GameObject caster)
    {
        string strText = m_input_Input.value;
        if (string.IsNullOrEmpty(strText))
        {
            TipsManager.Instance.ShowTips("还没有输入任何内容");
            return;
        }
        if (m_nItemNum <= 0)
        {
            int goldNum = MainPlayerHelper.GetMoneyNumByType(ClientMoneyType.YuanBao);
            if (goldNum < m_nItemPrice)
            {
                TipsManager.Instance.ShowTips("元宝数量不足");
                return;
            }
        }

        foreach (var item in m_dictItemLink)
        {
            strText = strText.Replace(item.Key, item.Value);
        }
        strText = DataManager.Manager<TextManager>().ReplaceSensitiveWord(strText, TextManager.MatchType.Max);
        m_dictItemLink.Clear();

        NetService.Instance.Send(new GameCmd.stSpeakerChatUserCmd_CS()
        {
            dwOPDes = (uint)Client.ClientGlobal.Instance().MainPlayer.GetID(),
            byChatType = GameCmd.CHATTYPE.CHAT_SPEAKER,
            byChatPos = (uint)GameCmd.ChatPos.ChatPos_Important,
            szInfo = strText,
          //  byRange = GameCmd.SpeakerRange.Speaker_World,
        });

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.EmojiPanel))
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.EmojiPanel);

        m_input_Input.value = "";
        m_input_Input.isSelected = false;
        this.HideSelf();
        TipsManager.Instance.ShowTips("发送成功");
    }

    void onClick_Btn_emoji_Btn(GameObject caster)
    {
        transform.localPosition = new Vector3(0, 125, 0);
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.EmojiPanel,data:this);
    }

    void onClick_Btn_history_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.HistoryMsgPanel, data: this);
    }
}
