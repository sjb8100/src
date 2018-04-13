using UnityEngine;
using System.Collections.Generic;
using System.Collections;

partial class MsgSettingPanel : UIPanelBase
{
    //const string presetMsg = "PresetMsg";
    ChatDataManager m_chatManager;
   // string[] presetmessage = new string[] { "求好友一起游戏~", "求加入家族组织~", "哪个氏族收人~" };
    List<UIButton> btn_presetmessage = new List<UIButton>(3);//好友，家族，氏族
    List<UIButton> btn_historymessage = new List<UIButton>(5);
    UIInput[] m_inputs = new UIInput[3];

    IChatInput m_chatpanel;
    protected override void OnAwake()
    {
        base.OnAwake();

        m_chatManager = DataManager.Manager<ChatDataManager>();
        m_chatpanel = DataManager.Manager<UIPanelManager>().GetPanel<ChatPanel>(PanelID.ChatPanel);
        //右边面板默认不可见
        m_trans_presetmessagePanel.gameObject.SetActive(false);

        foreach (Transform child in m_trans_btn_presetmessage.transform)
        {
            if (child.GetComponent<UIButton>() != null)
            {
                btn_presetmessage.Add(child.GetComponent<UIButton>());
            }
        }
        //UIButton[] btns = m_sprite_btn_presetmessage.GetComponentsInChildren<UIButton>();
        //btn_presetmessage.AddRange(btns);
        btn_presetmessage.Sort(SortByName);

        for (int i = 0; i < btn_presetmessage.Count; i++)
        {
            btn_presetmessage[i].GetComponentInChildren<UILabel>().text = m_chatManager.GetPresetMsg(i);
            UIEventListener.Get(btn_presetmessage[i].gameObject).onClick = OnBtnSendMsg;
        }
        foreach (Transform child in m_trans_btn_historymessage.transform)
        {
            if (child.GetComponent<UIButton>() != null)
            {
                btn_historymessage.Add(child.GetComponent<UIButton>());
            }
        }
        btn_historymessage.Sort(SortByName);

        int index = 0;
        foreach (var item in btn_historymessage)
        {
            item.gameObject.name = (index++).ToString();
            item.GetComponentInChildren<UILabel>().text = "";
            UIEventListener.Get(item.gameObject).onClick = OnBtnHistory;
        }

        m_inputs[0] = m_input_addfriend_Input;
        m_inputs[1] = m_input_addfamily_Input;
        m_inputs[2] = m_input_addclan_Input;
        for (int i = 0; i < m_inputs.Length; i++)
        {
            m_inputs[i].characterLimit = 20;
        }
        UIEventListener.Get(m_widget_btn_close.gameObject).onClick = (go) => { this.HideSelf(); };
    }

    public static int SortByName(UIButton a, UIButton b) { return string.Compare(a.name, b.name); }


    public void RefreshHistoryMsg()
    {
        List<string> history = DataManager.Manager<ChatDataManager>().GetHistoryMsg();
        for (int i = 0; i < history.Count; i++)
        {
            btn_historymessage[i].GetComponentInChildren<UILabel>().text = history[i];
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        RefreshHistoryMsg();

        for (int i = 0; i < btn_presetmessage.Count; i++)
        {
            btn_presetmessage[i].GetComponentInChildren<UILabel>().text = m_chatManager.GetPresetMsg(i);
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        m_trans_presetmessagePanel.gameObject.SetActive(false);
    }
    void OnBtnSendMsg(GameObject go)
    {
        UILabel lable = go.GetComponentInChildren<UILabel>();
        if (string.IsNullOrEmpty(lable.text) == false)
        {
            DataManager.Manager<ChatDataManager>().SendChatText(lable.text);
            HideSelf();
        }
    }

    void OnBtnHistory(GameObject go)
    {
        if (m_chatpanel == null)
        {
            return;
        }

        int historyindex = int.Parse(go.name);
        string itemName = "";
        uint itemthisid;
        uint quality;
        int type;
        if (DataManager.Manager<ChatDataManager>().CheckIsLinkItem(historyindex, ref itemName, out itemthisid, out quality,out type))
        {
            m_chatpanel.AddLinkerItem(itemName, itemthisid, quality, type);
        }
        else
        {
            List<string> history = DataManager.Manager<ChatDataManager>().GetHistoryMsg();
            if (history.Count > historyindex)
            {
                m_chatpanel.AppendText(history[historyindex]);
            }
        }
    }

    void onClick_Btn_presetmessage_setting_Btn(GameObject caster)
    {
        m_trans_presetmessagePanel.gameObject.SetActive(true);

        for (int index = 0; index < 3; index++)
        {
            m_inputs[index].value = btn_presetmessage[index].GetComponentInChildren<UILabel>().text;
        }
    }

    void onClick_Btn_confirm_Btn(GameObject caster)
    {
        for (int index = 0; index < 3; index++)
        {
            if (string.IsNullOrEmpty(m_inputs[index].value) == false)
            {
                btn_presetmessage[index].GetComponentInChildren<UILabel>().text = m_inputs[index].value;
                m_chatManager.SetPresetMsg(index, m_inputs[index].value);
            }
        }

        m_trans_presetmessagePanel.gameObject.SetActive(false);
    }

    void onClick_Btn_addfriend_reset_Btn(GameObject caster)
    {
        ResetMsg(0);
    }

    void ResetMsg(int index)
    {
      //  btn_presetmessage[index].GetComponentInChildren<UILabel>().text = m_chatManager.GetNorMalPresetMsg(index);
        //m_chatManager.SetPresetMsg(index, m_chatManager.GetNorMalPresetMsg(index));
        if (index >= 0 && index < m_inputs.Length)
        {
            m_inputs[index].text = m_chatManager.GetNorMalPresetMsg(index);
        }
        //m_input_addfriend_Input.text = 
    }

    void onClick_Btn_addfamily_reset_Btn(GameObject caster)
    {
        ResetMsg(1);
    }

    void onClick_Btn_addclan_reset_Btn(GameObject caster)
    {
        ResetMsg(2);
    }
    void onClick_Btn_unclose02_Btn(GameObject caster)
    {

    }
    void onClick_Btn_unclose01_Btn(GameObject caster)
    {

    }
    void onClick_CloseEditor_Btn(GameObject caster)
    {
        m_trans_presetmessagePanel.gameObject.SetActive(false);
    }

}