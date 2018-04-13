using Client;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

partial class TeamTalkPanel : UIPanelBase
{

    uint MaxNum = 50;

    string m_defaultText = string.Empty;

    //输入内容
    private string m_str_inputString = "";
    //输入字符数量
    public uint InputNum
    {
        get
        {
            if (string.IsNullOrEmpty(m_str_inputString))
            {
                return 0;
            }
            uint charNum = TextManager.GetCharNumByStrInUnicode(m_str_inputString);
            return TextManager.TransforCharNum2WordNum(charNum, true);
        }
    }

    #region override

    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    protected override void OnLoading()
    {
        base.OnLoading();

        m_input_input.onChange.Add(new EventDelegate(OnChangeDelgate));

        m_input_input.onSubmit.Add(new EventDelegate(OnSubmitDelgate));


        this.m_defaultText = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Team_Recruit_hanhua);
        m_input_input.defaultText = this.m_defaultText;

        m_input_input.characterLimit = (int)MaxNum;
        m_label_wordnumber.text = string.Format("最多可以输入{0}个字", MaxNum);
    }

    void OnChangeDelgate()
    {
        m_str_inputString = TextManager.GetTextByWordsCountLimitInUnicode(m_input_input.value
                      , MaxNum);

        m_str_inputString = DataManager.Manager<TextManager>().ReplaceSensitiveWord(m_str_inputString, TextManager.MatchType.Max);

        m_input_input.value = m_str_inputString;

        m_label_wordnumber.text = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_TXT_InputLimit, MaxNum, MaxNum - InputNum);

    }

    void OnSubmitDelgate()
    {
        m_str_inputString = TextManager.GetTextByWordsCountLimitInUnicode(m_input_input.value
                     , MaxNum);

        m_str_inputString = DataManager.Manager<TextManager>().ReplaceSensitiveWord(m_str_inputString, TextManager.MatchType.Max);

        m_input_input.value = m_str_inputString;

        m_label_wordnumber.text = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_TXT_InputLimit, MaxNum, MaxNum - InputNum);
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);

    }

    protected override void OnShow(object data)
    {
        m_input_input.defaultText = this.m_defaultText;
    }

    protected override void OnHide()
    {
        base.OnHide();
        m_input_input.value = "";
        m_input_input.isSelected = false;
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        return true;
    }
    #endregion

    #region methon

    CHATTYPE GetSelectBtnIndex()
    {
        for (int i = 0; i < m_trans_selectBtns.childCount; i++)
        {
            Transform btnTransf = m_trans_selectBtns.GetChild(i);
            UIToggle toggle = btnTransf.GetComponent<UIToggle>();
            if (toggle != null)
            {
                if (toggle.value)
                {
                    if (i == 0)
                    {
                        return CHATTYPE.CHAT_CLAN;
                    }
                    else if (i == 1)
                    {
                        return CHATTYPE.CHAT_WORLD;
                    }
                    else if (i == 2)
                    {
                        return CHATTYPE.CHAT_RECRUIT;
                    }
                }
            }
        }

        return CHATTYPE.CHAT_CLAN;
    }


    #endregion

    #region click

    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_close_Btn(GameObject caster)
    {
        HideSelf();
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_send_Btn(GameObject caster)
    {
        //if (!string.IsNullOrEmpty(m_input_input.value))
        {
            //string inputText = m_input_input.value;
            string inputText = string.IsNullOrEmpty(m_input_input.value) ? this.m_defaultText : m_input_input.value;  //不打字就默认 “懂的来”

            uint leaderId = DataManager.Manager<TeamDataManager>().LeaderId;
            uint activityId = DataManager.Manager<TeamDataManager>().TeamActivityTargetId;
            string sendText = string.Empty;
            if (activityId != 0)
            {
                TeamActivityDatabase data = GameTableManager.Instance.GetTableItem<TeamActivityDatabase>(activityId);
                if (data != null)
                {
                    string name = ClientGlobal.Instance().MainPlayer.GetName();
                    string strText = LangTalkData.GetTextById(20007);
                    sendText = string.Format(strText, data.mainName, data.indexName, data.min, data.max, name, inputText, leaderId);
                }

                if (sendText == string.Empty)
                {
                    TipsManager.Instance.ShowTips(LocalTextType.Team_Activity_qingxianxuanzehuodongmubiao);//请选择活动目标
                }
            }



            if (DataManager.Manager<TeamDataManager>().MainPlayerIsMember() && sendText != string.Empty)
            {
                bool b = false;
                string channelName = "";

                //氏族
                if (GetSelectBtnIndex() == CHATTYPE.CHAT_CLAN)
                {
                    channelName = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_TaskTitle_Clan);//氏族
                    bool IsJoinClan = DataManager.Manager<ClanManger>().IsJoinClan;
                    if (IsJoinClan == false)
                    {
                        TipsManager.Instance.ShowTips("你还没有加入氏族");//你还没有加入氏族
                        return;
                    }

                    b = DataManager.Manager<ChatDataManager>().SendText(CHATTYPE.CHAT_CLAN, sendText);
                }

                //世界
                else if (GetSelectBtnIndex() == CHATTYPE.CHAT_WORLD)
                {
                    channelName = "世界";

                    //判断钱够不够
                    if (UserData.Coupon < DataManager.Manager<ChatDataManager>().ChatWorldCost)
                    {
                        TipsManager.Instance.ShowTipsById(4);
                        return;
                    }

                    b = DataManager.Manager<ChatDataManager>().SendText(CHATTYPE.CHAT_WORLD, sendText);
                }

                //招募
                else if (GetSelectBtnIndex() == CHATTYPE.CHAT_RECRUIT)
                {
                    channelName = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Team_Recruit_zhaomu);
                    b = DataManager.Manager<ChatDataManager>().SendText(CHATTYPE.CHAT_RECRUIT, sendText);
                }

                if (b)
                {
                    m_input_input.value = "";
                    m_input_input.isSelected = false;

                    //已经发送招募信息至{0}
                    TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Team_Recruit_zhaomuxinxifasongchenggong, channelName);
                    HideSelf();
                }
            }
        }

        m_input_input.Start();
    }

    /// <summary>
    /// 表情
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_emoji_Btn(GameObject caster)
    {

    }

    void onClick_Btn_joke_Btn(GameObject caster)
    {

    }
    #endregion
}

