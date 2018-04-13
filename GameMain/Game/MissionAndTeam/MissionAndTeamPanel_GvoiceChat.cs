using System;
using System.Collections.Generic;
using UnityEngine;

partial class MissionAndTeamPanel : UIPanelBase
{
//    //当前制作 有人说话时的语音动画
//    private float FramePrecent = 0.1f;
//    private float PlayTime = 3f;
//    private float PlayStartTime = 0;
//    private bool IsPlaying=false;
//    private List<UISprite> SpriteAnimList=  new List<UISprite>();
//    void InitGvoice()
//    {
//        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TEAM_LEAVE, OnVoiceEvent);
//        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TEAM_JOIN, OnVoiceEvent);
//        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANJOIN, OnVoiceEvent);
//        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANQUIT, OnVoiceEvent);

//        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CHAT_JOINROOM, OnVoiceEvent);
//        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CHAT_LEVELROOM, OnVoiceEvent);
//        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CHAT_SETMICBTN, OnVoiceEvent);
//        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CHAT_SPEEKERNOW, OnVoiceEvent);
//        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CHAT_MICKSTATE, OnVoiceEvent);

//        UIEventListener.Get(m_spriteEx_mutebtn.gameObject).onClick = OnClickMuteBtn;
//        UIEventListener.Get(m_spriteEx_voicesetting.gameObject).onClick = OnClickVoicesetting;
//        UIEventListener.Get(m_widget_btnclose.gameObject).onClick = onClick_Statebg_Btn;

//        SetMuteBtn(true);

//        m_trans_channelContainer.gameObject.SetActive(false);

//        m_sprite_stateani.type = UIBasicSprite.Type.Filled;
//        m_sprite_stateani.fillDirection = UIBasicSprite.FillDirection.Vertical;
//        m_sprite_stateani.fillAmount = 1;
//        m_sprite_stateani.gameObject.SetActive(false);
//        for (int i = 0; i < 4; i++)
//        {
//            if (i < SpriteAnimList.Count)
//            {
//                SpriteAnimList[i].transform.localPosition = m_sprite_stateani.transform.localPosition + new Vector3(i * (m_sprite_stateani.width + 1), 0, 0);
//            }
//            else
//            {
//                GameObject obj = GameObject.Instantiate(m_sprite_stateani.gameObject) as GameObject;
//                obj.SetActive(true);
//                obj.transform.parent = m_sprite_stateani.transform.parent;
//                obj.transform.localScale = Vector3.one;
//                obj.transform.localPosition = m_sprite_stateani.transform.localPosition + new Vector3(i*(m_sprite_stateani.width + 1), 0, 0);
//                UISprite sp = obj.GetComponent<UISprite>();
//                sp.type = UIBasicSprite.Type.Filled;
//                sp.fillDirection = UIBasicSprite.FillDirection.Vertical;
//                sp.fillAmount = 0.2f+i*0.2f;
//                SpriteAnimList.Add(sp);
//            }
//        }
//    }

//    void UnInit()
//    {
//        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TEAM_LEAVE, OnVoiceEvent);
//        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TEAM_JOIN, OnVoiceEvent);
//        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANJOIN, OnVoiceEvent);
//        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANQUIT, OnVoiceEvent);

//        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CHAT_JOINROOM, OnVoiceEvent);
//        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CHAT_LEVELROOM, OnVoiceEvent);
//        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CHAT_SETMICBTN, OnVoiceEvent);
//        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CHAT_SPEEKERNOW, OnVoiceEvent);
//        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CHAT_MICKSTATE, OnVoiceEvent);
//    }

//    void OnVoiceEvent(int nEventId, object param)
//    {
//        if (nEventId == (int)Client.GameEventID.TEAM_JOIN)
//        {
//            Client.stTeamJoin tj = (Client.stTeamJoin)param;
//        }
//        else if (nEventId == (int)Client.GameEventID.TEAM_LEAVE)
//        {
//             //当玩家退出队伍时，如果玩家当前在队伍聊天室，则会自动退出队伍聊天室。
//            if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Team)
//            {
//                GVoiceManger.Instance.QuitRoom(GVoiceManger.Instance.JoinRoomName);
//            }
//        }else if (nEventId == (int)Client.GameEventID.CLANQUIT)
//        {
//            Client.stClanQuit cq = (Client.stClanQuit)param;
//            if (Client.ClientGlobal.Instance().IsMainPlayer(cq.uid))
//            {
//                if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Nation)
//                {
//                    GVoiceManger.Instance.QuitRoom(GVoiceManger.Instance.JoinRoomName);
//                }
//            }
//        }
//        else if (nEventId == (int)Client.GameEventID.CHAT_LEVELROOM)
//        {
//            SetMuteBtn(true);
//        }
//        else if (nEventId == (int)Client.GameEventID.CHAT_JOINROOM)
//        {
//            Client.stVoiceJoinRoom jr = (Client.stVoiceJoinRoom)param;
//            SetMuteBtn(!jr.succ);
            
//            if (jr.succ)
//            {
//                if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Team)
//                {
//                    TipsManager.Instance.ShowTips(LocalTextType.Talk_ActualTime_jinruduiwuliaotianshi);
//                    //打开扬声器 收听语音
//                    GVoiceManger.Instance.OpenSpeaker();
//                    GVoiceManger.Instance.CloseMicInRoom();
//                    SetSettingBtnStatus(1);
//                    SetSpeek("队伍频道");
//                }
//                else if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Nation)
//                {
//                    SetSpeek("氏族频道");

//                    TipsManager.Instance.ShowTips(LocalTextType.Talk_ActualTime_jinrushizuliaotianshi);
//                    //氏族聊天 所有人都打开喇叭 管理员打开麦克风
//                    GVoiceManger.Instance.OpenSpeaker();
//                    bool manager = false;
//                    if (DataManager.Manager<ClanManger>().ClanInfo == null)
//                    {
//                        SetSettingBtnStatus(1);
//                        return;
//                    }
//                    GameCmd.stClanMemberInfo clanInfo = DataManager.Manager<ClanManger>().ClanInfo.GetMemberInfo((uint)MainPlayerHelper.GetPlayerID());

//                    if (clanInfo != null && clanInfo.duty != GameCmd.enumClanDuty.CD_Member)
//                    {
//                        //管理员打开麦克风
//                        GVoiceManger.Instance.OpenMicInRoom();
//                        SetSettingBtnStatus(3);
//                    }
//                    else
//                    {
//                        GVoiceManger.Instance.CloseMicInRoom();
//                        SetSettingBtnStatus(1);
//                    }
//                }
//            }
//        }
//        else if (nEventId == (int)Client.GameEventID.CHAT_SPEEKERNOW)
//        {
//            string name = (string)param;
//            SetSpeek(name);
//            StartPlayAnimation();
//        }
//        else if (nEventId == (int)Client.GameEventID.CHAT_MICKSTATE)
//        {
//            bool state = (bool)param;
//            if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Team)
//            {
//                SetSettingBtnStatus(state ? 2 : 1);
//            }
//            else if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Nation)
//            {
//                if (DataManager.Manager<ClanManger>().ClanInfo == null)
//                {
//                    SetSettingBtnStatus(0);
//                    return;
//                }

//                GameCmd.stClanMemberInfo clanInfo = DataManager.Manager<ClanManger>().ClanInfo.GetMemberInfo((uint)MainPlayerHelper.GetPlayerID());

//                if (clanInfo != null)
//                {
//                    if (clanInfo.duty == GameCmd.enumClanDuty.CD_Member)
//                    {
//                        SetSettingBtnStatus(state ? 2 : 1);
//                    }
//                    else
//                    {
//                        SetSettingBtnStatus(3);
//                    }
//                }
//            }
//        }
//        else if (nEventId == (int)Client.GameEventID.CHAT_SETMICBTN)
//        {
//            bool show = (bool)param;

//            if (show)
//            {
//                if (DataManager.Manager<ClanManger>().ClanInfo != null)
//                {
//                    GameCmd.stClanMemberInfo clanInfo = DataManager.Manager<ClanManger>().ClanInfo.GetMemberInfo((uint)MainPlayerHelper.GetPlayerID());

//                    if (clanInfo != null)
//                    {
//                        if (clanInfo.duty == GameCmd.enumClanDuty.CD_Member)
//                        {
//                            SetSettingBtnStatus(1);
//                        }
//                        else
//                        {
//                            SetSettingBtnStatus(3);
//                        }
//                    }
//                }
//            }
//            else
//            {
//                GVoiceManger.Instance.CloseMic();

//                if (DataManager.Manager<ClanManger>().ClanInfo != null)
//                {
//                    GameCmd.stClanMemberInfo clanInfo = DataManager.Manager<ClanManger>().ClanInfo.GetMemberInfo((uint)MainPlayerHelper.GetPlayerID());

//                    if (clanInfo != null)
//                    {
//                        if (clanInfo.duty == GameCmd.enumClanDuty.CD_Member)
//                        {
//                            SetSettingBtnStatus(0);
//                        }
//                        else
//                        {
//                            SetSettingBtnStatus(3);
//                        }
//                    }
//                }

               
//            }
//        }
//    }

//    /// <summary>
//    /// 进入上一个频道 没有就直接主播频道
//    /// </summary>
//    /// <param name="go"></param>
//    void OnClickMuteBtn(GameObject go)
//    {
//        //如果当前加入房间则退出

//        if (GVoiceManger.Instance.JoinRoomType != JoinRoomEnum.None)
//        {
//            GVoiceManger.Instance.QuitRoom(GVoiceManger.Instance.JoinRoomName);
//            return;
//        }

//        JoinRoomEnum preRoom = GVoiceManger.Instance.PreJoinRoomType;
//        if (preRoom == JoinRoomEnum.None)
//        {
//            //TODO 加入主播频道
//            JoinCommand();
//        }else if (preRoom == JoinRoomEnum.Nation)
//        {
//            JoinClan();
//        }else if (preRoom == JoinRoomEnum.Team)
//        {
//            JoinTeam();
//        }
//    }

//    /// <summary>
//    /// 1-静音 2 进入聊天
//    /// </summary>
//    /// <param name="bMute"></param>
//    void SetMuteBtn(bool bMute)
//    {
//        if (bMute)
//        {
//            m_spriteEx_mutebtn.ChangeSprite(2);
//            SetSettingBtnStatus(0);
//            SetSpeek("请选择聊天室");
//        }
//        else
//        {
//            m_spriteEx_mutebtn.ChangeSprite(1);
//            SetSpeek("");
//        }
//    }

//    void SetSpeek(string strName)
//    {
//        m_label_roomname.text = strName;
//    }
//    private void JoinTeam()
//    {
//        if (GVoiceManger.Instance.JoinRoomType != JoinRoomEnum.Team)
//        {
//            int openLevel = GameTableManager.Instance.GetGlobalConfig<int>("ChatVoiceTeam");

//            if (MainPlayerHelper.GetPlayerLevel() < openLevel)
//            {
//                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Talk_ActualTime_Xjikaiqiduiwuliaotianshi, openLevel);
//                return;
//            }

//            if (DataManager.Manager<TeamDataManager>().IsJoinTeam == false)
//            {
//                TipsManager.Instance.ShowTips(LocalTextType.Talk_ActualTime_haiweijiaruduiwuwufajinruliaotianshi);
//                return;
//            }
//            if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Nation)
//            {
//                GVoiceManger.Instance.QuitRoom(GVoiceManger.Instance.JoinRoomName, (b) =>
//                {
//                    GVoiceManger.Instance.JoinTeamRoom();
//                });
//            }
//            else
//            {
//                GVoiceManger.Instance.JoinTeamRoom();
//            }
//        }
//    }

//    private void JoinClan()
//    {
//        if (GVoiceManger.Instance.JoinRoomType != JoinRoomEnum.Nation)
//        {
//            int openLevel = GameTableManager.Instance.GetGlobalConfig<int>("ChatVoiceClan");

//            if (MainPlayerHelper.GetPlayerLevel() < openLevel)
//            {
//                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Talk_ActualTime_Xjikaiqishizuliaotianshi, openLevel);
//                return;
//            }
//            if (DataManager.Manager<ClanManger>().IsJoinClan == false)
//            {
//                TipsManager.Instance.ShowTips(LocalTextType.Talk_ActualTime_haiweijiarushizuwufajinruliaotianshi);
//                return;
//            }

//            if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Team)
//            {
//                GVoiceManger.Instance.QuitRoom(GVoiceManger.Instance.JoinRoomName, (b) =>
//                {
//                    GVoiceManger.Instance.JoinClanRoom();
//                });
//            }
//            else
//            {
//                GVoiceManger.Instance.JoinClanRoom();
//            }
//        }
//    }
//    //加入主播频道
//    private void JoinCommand()
//    {
//        TipsManager.Instance.ShowTips(LocalTextType.Talk_ActualTime_zhubozhengzaiganlaidelushang);
//    }
//    private void JoinFM()
//    {
//        if (GVoiceManger.Instance.JoinRoomType != JoinRoomEnum.FM)
//        {
////             int openLevel = GameTableManager.Instance.GetGlobalConfig<int>("ChatVoiceTeam");
//// 
////             if (MainPlayerHelper.GetPlayerLevel() < openLevel)
////             {
////                 TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Talk_ActualTime_Xjikaiqiduiwuliaotianshi, openLevel);
////                 return;
////             }

//            if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Nation)
//            {
//                GVoiceManger.Instance.QuitRoom(GVoiceManger.Instance.JoinRoomName, (b) =>
//                {
//                    //GVoiceManger.Instance.JoinTeamRoom();
//                });
//            } if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Team)
//            {
//                GVoiceManger.Instance.QuitRoom(GVoiceManger.Instance.JoinRoomName, (b) =>
//                {
//                    //GVoiceManger.Instance.JoinTeamRoom();
//                });
//            }
//            else
//            {
//                //GVoiceManger.Instance.JoinTeamRoom();
//            }
//        }
//    }
//    /// <summary>
//    /// 0 隐藏 1 静音 2 开 3 打开设置面板
//    /// </summary>
//    /// <param name="nStatus"></param>
//    void SetSettingBtnStatus(int nStatus) 
//    {
//        m_spriteEx_voicesetting.ChangeSprite(nStatus);
//    }
//    /// <summary>
//    /// 打开频道选择
//    /// </summary>
//    /// <param name="caster"></param>
    void onClick_Statebg_Btn(GameObject caster)
    {
        //m_trans_channelContainer.gameObject.SetActive(!m_trans_channelContainer.gameObject.activeSelf);
    }


    void onClick_Teamchannel_Btn(GameObject caster)
    {
        //onClick_Statebg_Btn(null);
        //JoinTeam();
    }

    void onClick_Homechannel_Btn(GameObject caster)
    {
        //onClick_Statebg_Btn(null);
        //JoinClan();
    }

    void onClick_Commandchannel_Btn(GameObject caster)
    {
        //onClick_Statebg_Btn(null);
        //JoinCommand();
    }

//    /// <summary>
//    /// 设置按钮 打开设置面板 开关麦克风
//    /// </summary>
//    /// <param name="caster"></param>
//    void OnClickVoicesetting(GameObject caster)
//    {
//        if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.None)
//        {
//            return;
//        }

//        if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Nation)
//        {
//            bool manager = false;
//            if (DataManager.Manager<ClanManger>().ClanInfo == null)
//            {
//                SetSettingBtnStatus(0);
//                return;
//            }

//            GameCmd.stClanMemberInfo clanInfo = DataManager.Manager<ClanManger>().ClanInfo.GetMemberInfo((uint)MainPlayerHelper.GetPlayerID());

//            if (clanInfo != null && clanInfo.duty != GameCmd.enumClanDuty.CD_Member)
//            {
//                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.VoiceSetting);
//            }
//            else
//            {
//                if (GVoiceManger.Instance.IsOpenMicInRoom)
//                {
//                    GVoiceManger.Instance.CloseMicInRoom_Small();
//                }
//                else
//                {
//                    GVoiceManger.Instance.OpenMicInRoom_Small();
//                }
//            }
//        }
//        else if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Team)
//        {
//            if (DataManager.Manager<TeamDataManager>().IsJoinTeam)
//            {
//                if (GVoiceManger.Instance.IsOpenMicInRoom)
//                {
//                    GVoiceManger.Instance.CloseMicInRoom_Small();
//                }
//                else
//                {
//                    GVoiceManger.Instance.OpenMicInRoom_Small();
//                }
//            }
//        }
//    }
//    private void StartPlayAnimation()
//    {
//        GVoiceManger.Instance.SoundMute(false);
//        PlayStartTime = Time.time;
//        IsPlaying = true;
//    }

//    void GvoiceChatUpdate() 
//    {
//        if (IsPlaying && Time.frameCount % 3 == 0)
//        {
//            if (Time.time > PlayStartTime + PlayTime)
//            {
//                IsPlaying = false;
//                GVoiceManger.Instance.SoundMute(true);
//            }
//            else
//            {
//                for (int i = 0; i < SpriteAnimList.Count; i++)
//                {
//                    SpriteAnimList[i].fillAmount += FramePrecent;
//                    if (SpriteAnimList[i].fillAmount >= 1)
//                        SpriteAnimList[i].fillAmount = 0;
//                }

//            }

//        }
//    }

//    //方法移到上面了
//    //public void Update()
//    //{
//    //    if (IsPlaying && Time.frameCount % 3 == 0)
//    //    {
//    //        if (Time.time > PlayStartTime + PlayTime)
//    //        {
//    //            IsPlaying = false;
//    //            GVoiceManger.Instance.SoundMute(true);
//    //        }
//    //        else 
//    //        {
//    //            for (int i = 0; i < SpriteAnimList.Count; i++)
//    //            {
//    //                SpriteAnimList[i].fillAmount += FramePrecent;
//    //                if (SpriteAnimList[i].fillAmount >= 1)
//    //                    SpriteAnimList[i].fillAmount = 0;
//    //            }
                   
//    //        }
            
//    //    }
//    //}
}
