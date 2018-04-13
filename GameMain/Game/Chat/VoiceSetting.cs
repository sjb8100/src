using System;
using System.Collections.Generic;
using UnityEngine;


partial class VoiceSetting : UIPanelBase
{
    protected override void OnLoading()
    {
        base.OnLoading();

        m_slider_slidertop.onDragFinished = () =>
        {
            if (m_slider_slidertop.value < 0.5f)
            {
                GVoiceManger.Instance.CloseMicInRoom();
                m_slider_slidertop.value = 0f;
            }
            else
            {
                m_slider_slidertop.value = 1f;
                GVoiceManger.Instance.OpenMicInRoom();
            }
        };

        m_slider_sliderbuttom.onDragFinished = () =>
        {
            GameCmd.stSetVoiceChatModeChatUserCmd_CS cmd = new GameCmd.stSetVoiceChatModeChatUserCmd_CS();
            cmd.chtype = GameCmd.VoChatType.VoChatType_Clan;
            cmd.posid = (uint)GVoiceManger.Instance.MemberId;

            if (m_slider_sliderbuttom.value < 0.5f)
            {
                m_slider_sliderbuttom.value = 0f;
                cmd.mode = GameCmd.VoChatMode.VoChatMode_Freedom;
            }
            else
            {
                m_slider_sliderbuttom.value = 1f;
                cmd.mode = GameCmd.VoChatMode.VoChatMode_Leader;
            }

            NetService.Instance.Send(cmd);
        };
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (GVoiceManger.Instance.IsOpenMicInRoom)
        {
            if (m_slider_slidertop.value < 1f)
            {
                m_slider_slidertop.value = 1f;
            }
        }
        else
        {
            if (m_slider_slidertop.value > 0f)
            {
                m_slider_slidertop.value = 0f;
            }
        }

        if (DataManager.Manager<ChatDataManager>().ClanChatMode == GameCmd.VoChatMode.VoChatMode_Leader)
        {
            if (m_slider_sliderbuttom.value < 1f)
            {
                m_slider_sliderbuttom.value = 1f;
            }
        }
        else
        {
            if (m_slider_sliderbuttom.value > 0f)
            {
                m_slider_sliderbuttom.value = 0f;
            }
        }

        if (DataManager.Manager<ChatDataManager>().ClanCallLeftTime <= 0)
        {
            m_label_timeLabel.text = "召集";
        }
        else
        {
            m_label_timeLabel.text = string.Format("召集{0}",
                DateTimeHelper.ParseTimeSeconds(Mathf.CeilToInt(DataManager.Manager<ChatDataManager>().ClanCallLeftTime)));
        }
    }

    void Update()
    {
        float time = DataManager.Manager<ChatDataManager>().ClanCallLeftTime ;

        if (time < 0)
        {
            m_label_timeLabel.text = "召集";
            return;
        }

        m_label_timeLabel.text = string.Format("召集{0}",DateTimeHelper.ParseTimeSeconds(Mathf.CeilToInt(time)));

    }

    void onClick_Call_Btn(GameObject caster)
    {
        if (DataManager.Manager<ChatDataManager>().ClanCallLeftTime > 0)
        {
            TipsManager.Instance.ShowTips("召集冷却中");
            return;
        }

        NetService.Instance.Send(new GameCmd.stCallVoiceChatMemberChatUserCmd_CS() { chtype = GameCmd.VoChatType.VoChatType_Clan });
    }

    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }
}