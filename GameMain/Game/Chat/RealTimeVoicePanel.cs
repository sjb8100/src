using System;
using System.Collections.Generic;
using UnityEngine;

public partial class RealTimeVoicePanel : UIPanelBase
{
    protected override void OnLoading()
    {
        base.OnLoading();

        m_toggle_mic.onChange.Add(new EventDelegate(() => { 
            if (m_toggle_mic.value)
            {
                GVoiceManger.Instance.OpenMic();
            }
            else
            {
                GVoiceManger.Instance.CloseMic();
            }
        }));

        m_toggle_speaker.onChange.Add(new EventDelegate(() =>
        {
            if (m_toggle_speaker.value)
            {
                GVoiceManger.Instance.OpenSpeaker();
            } 
            else
            {
                GVoiceManger.Instance.CloseSpeaker();
            }
        }));

        m_label_roleid.text = "";
    }


    void onClick_Jointeam_Btn(GameObject caster)
    {
        GVoiceManger.Instance.SetModel(gcloud_voice.GCloudVoiceMode.RealTime);
        //GVoiceManger.Instance.JoinTeamRoom(m_input_Inputroom.value);
    }

    void onClick_Joinnation_Btn(GameObject caster)
    {
        GVoiceManger.Instance.SetModel(gcloud_voice.GCloudVoiceMode.RealTime);
        //GVoiceManger.Instance.JoinNationalRoom(m_input_Inputroom.value,m_toggle_nationrole.value ? gcloud_voice.GCloudVoiceRole.ANCHOR : gcloud_voice.GCloudVoiceRole.AUDIENCE);
    }

    void onClick_Quitroom_Btn(GameObject caster)
    {
        if (m_bJoin)
        {
            GVoiceManger.Instance.QuitRoom(roomName);
        }
    }

    bool m_bJoin = false;
    string roomName = "";
    public void OnJoinRoom(string name,int id)
    {
        m_bJoin = true;
        roomName = name;
        m_label_roleid.text = string.Format("join {0} {1}",roomName,id);
    }

    public void OnQuitRoom(string name,int id)
    {
        m_bJoin = false;
        m_label_roleid.text = string.Format("quit {0} {1}", name, id);
    }

    public void OnMember(string strMsg)
    {
        m_label_rolestate.text = strMsg;
    }
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

}
