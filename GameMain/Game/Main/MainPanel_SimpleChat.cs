/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Main
 * 创建人：  wenjunhua.zqgame
 * 文件名：  MainPanel_SimpleChat
 * 版本号：  V1.0.0.0
 * 创建时间：7/10/2017 2:10:10 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Common;

partial class MainPanel
{
    #region property
    ChatDataManager m_chatdata = null;
    GameObject m_chatItemPrefab = null;
    [Header("显示的聊天数量的最大值")]
    public int MaxChatNum = 10;
    float lastPosY = 0;
    UIScrollView m_chatScrollview;
    List<UIXmlRichText> m_lstxmlText = new List<UIXmlRichText>();
    UIPanel m_UIPanel;
    List<ChatInfo> m_lsttextList = new List<ChatInfo>();

    UIParticleWidget m_particleW = null;

    public GameObject m_goPrefab = null;

    List<SysMsgPushBtn> m_lstSysMsgPushBtn = new List<SysMsgPushBtn>();

    const int OFFSETX = 62;


    //voice
    GameObject m_goClan = null;
    GameObject m_goteam = null;
    GameObject m_gomute = null;
    UISpriteEx m_spreteEx = null;
    UISpriteEx m_spreteVoiceEx = null;
    GameObject m_ClanVoice = null;
    GameObject m_TeamVoice = null;
    GameObject m_WorldVoice = null;
    GameObject m_MicLeft = null;
    GameObject m_MicRight = null;
    float PressTime = 0;
    #endregion
    #region InitChat
    private void InitChat()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ROBOTCOMBAT_STOP, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ROBOTCOMBAT_START, OnEvent);

        m_ClanVoice = m_trans_AnchorBottom.Find("ClanVoice").gameObject;
        m_TeamVoice = m_trans_AnchorBottom.Find("TeamVoice").gameObject;
        m_WorldVoice = m_trans_AnchorBottom.Find("WorldVoice").gameObject;
        m_MicLeft = m_trans_voiceMicTips.Find("Spriteleft").gameObject;
        m_MicRight = m_trans_voiceMicTips.Find("Spriteright").gameObject;

        m_goPrefab = m_widget_BtnPush.gameObject;
        m_particleW = m_goPrefab.GetComponent<UIParticleWidget>();
        if (null == m_particleW)
        {
            m_particleW = m_goPrefab.gameObject.AddComponent<UIParticleWidget>();
        }
        m_goPrefab.SetActive(false);

        m_chatScrollview = m_trans_chatItemRoot.parent.GetComponent<UIScrollView>();
        m_chatdata = DataManager.Manager<ChatDataManager>();

        m_chatItemPrefab = m_widget_ChatItem.gameObject;
        m_chatItemPrefab.gameObject.SetActive(false);
        lastPosY = 0;

        ///全部置为0 做prafb的时候就要算好
        m_chatScrollview.panel.clipOffset = Vector2.zero;
        m_chatScrollview.transform.localPosition = Vector3.zero;

        //预先加载表情
        UIAtlas atlas = UIManager.GetAtlas((uint)AtlasID.Emojiatlas).GetAtlas();
        if (atlas != null)
        {
            UIXmlRichText.dicAtlas["emojiatlas"] = atlas;
        }

        m_UIPanel = m_trans_chatItemRoot.transform.parent.GetComponent<UIPanel>();



        UIEventListener.Get(m_sprite_btn_chat_shrink.gameObject).onClick = onClick_Btn_chat_shrink_Btn;
        m_ClanVoice.AddComponent<LongPress>().InitLongPress(OnDownLongPressClanVoice, OnPress_Btn_ClanVoice, 500);
        m_TeamVoice.AddComponent<LongPress>().InitLongPress(OnDownLongPressTeamVoice, OnPress_Btn_TeamVoice, 500);
        m_WorldVoice.AddComponent<LongPress>().InitLongPress(OnDownLongPressWorldVoice, OnPress_Btn_WorldVoice, 500);
        UIEventListener.Get(m_TeamVoice).onDrag = OnDrag_Btn_Voice;
        UIEventListener.Get(m_ClanVoice).onDrag = OnDrag_Btn_Voice;
        UIEventListener.Get(m_WorldVoice).onDrag = OnDrag_Btn_Voice;
        UIEventListener.Get(m_sprite_chatsetting_btn.gameObject).onClick = OnClick_ChatSeting_btn;

        //m_trans_offset.localPosition = new Vector3(0, 0, 0);
        //m_spriteEx_Sprite.ChangeSprite(1);
        SetPanelClip(false);
        Invoke("Init", 1.5f);
        InitVoice();
    }
    #endregion

    #region Voice
    void InitVoice()
    {

        //         Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TEAM_LEAVE, OnVoiceEvent);
        //         Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TEAM_JOIN, OnVoiceEvent);
        //         Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANJOIN, OnVoiceEvent);
        //         Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANQUIT, OnVoiceEvent);
        // 
        //         Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CHAT_JOINROOM, OnVoiceEvent);
        //         Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CHAT_LEVELROOM, OnVoiceEvent);
        //         Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CHAT_SETMICBTN, OnVoiceEvent);
        //         Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CHAT_SPEEKERNOW, OnVoiceEvent);
        //         SetExpand(false);
        // 
        //         SetStatus("mute");
        //         SetSettingBtn("mute");
        //         SetSpeek("");
    }


    void ReleaseVoice()
    {
        //         Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TEAM_LEAVE, OnVoiceEvent);
        //         Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TEAM_JOIN, OnVoiceEvent);
        //         Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANJOIN, OnVoiceEvent);
        //         Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANQUIT, OnVoiceEvent);
        // 
        //         Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CHAT_JOINROOM, OnVoiceEvent);
        //         Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CHAT_LEVELROOM, OnVoiceEvent);
        //         Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CHAT_SETMICBTN, OnVoiceEvent);
        //         Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CHAT_SPEEKERNOW, OnVoiceEvent);
    }
    //void OnVoiceEvent(int nEventId, object param)
    //{
    //    if (nEventId == (int)Client.GameEventID.TEAM_JOIN)
    //    {
    //        Client.stTeamJoin tj = (Client.stTeamJoin)param;
    //        if (string.IsNullOrEmpty(GVoiceManger.Instance.JoinRoomName))
    //        {
    //            GVoiceManger.Instance.JoinTeamRoom();
    //        }
    //    }
    //    else if (nEventId == (int)Client.GameEventID.TEAM_LEAVE)
    //    {
    //        if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Team && !string.IsNullOrEmpty(GVoiceManger.Instance.JoinRoomName))
    //        {
    //            GVoiceManger.Instance.QuitRoom(GVoiceManger.Instance.JoinRoomName);
    //        }
    //    }
    //    else if (nEventId == (int)Client.GameEventID.CLANQUIT)
    //    {
    //        Client.stClanQuit cq = (Client.stClanQuit)param;
    //        if (cq.uid == MainPlayerHelper.GetPlayerUID())
    //        {
    //            if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Nation && !string.IsNullOrEmpty(GVoiceManger.Instance.JoinRoomName))
    //            {
    //                GVoiceManger.Instance.QuitRoom(GVoiceManger.Instance.JoinRoomName);
    //            }
    //        }
    //    }
    //    else if (nEventId == (int)Client.GameEventID.CLANJOIN)
    //    {
    //        Client.stClanJoin cj = (Client.stClanJoin)param;

    //    }
    //    else if (nEventId == (int)Client.GameEventID.CHAT_LEVELROOM)
    //    {
    //        TipsManager.Instance.ShowTips(LocalTextType.Talk_ActualTime_tuichusuoyouliaotianshi);
    //        SetStatus("mute");
    //    }
    //    else if (nEventId == (int)Client.GameEventID.CHAT_JOINROOM)
    //    {
    //        Client.stVoiceJoinRoom jr = (Client.stVoiceJoinRoom)param;
    //        if (jr.succ)
    //        {

    //            if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Team)
    //            {
    //                TipsManager.Instance.ShowTips(LocalTextType.Talk_ActualTime_jinruduiwuliaotianshi);
    //                SetStatus("team");
    //                GVoiceManger.Instance.OpenSpeaker();
    //            }
    //            else if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Nation)
    //            {
    //                TipsManager.Instance.ShowTips(LocalTextType.Talk_ActualTime_jinrushizuliaotianshi);
    //                SetStatus("clan");
    //                //氏族聊天 所有人都打开喇叭 管理员打开麦克风
    //                GVoiceManger.Instance.OpenSpeaker();
    //                bool manager = false;
    //                if (DataManager.Manager<ClanManger>().ClanInfo != null)
    //                {
    //                    GameCmd.stClanMemberInfo clanInfo = DataManager.Manager<ClanManger>().ClanInfo.GetMemberInfo((uint)MainPlayerHelper.GetPlayerID());

    //                    if (clanInfo != null && clanInfo.duty != GameCmd.enumClanDuty.CD_Member)
    //                    {
    //                        GVoiceManger.Instance.OpenMic();
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    else if (nEventId == (int)Client.GameEventID.CHAT_SETMICBTN)
    //    {
    //        bool show = (bool)param;

    //        //           if (show)
    //        //             {
    //        //                 if (DataManager.Manager<ClanManger>().ClanInfo != null)
    //        //                 {
    //        //                     GameCmd.stClanMemberInfo clanInfo = DataManager.Manager<ClanManger>().ClanInfo.GetMemberInfo((uint)MainPlayerHelper.GetPlayerID());
    //        // 
    //        //                     if (clanInfo != null)
    //        //                     {
    //        //                         m_btn_voiceBtn.gameObject.SetActive(true);
    //        //                         if (clanInfo.duty == GameCmd.enumClanDuty.CD_Member)
    //        //                         {
    //        //                             if (GVoiceManger.Instance.IsOpenMic)
    //        //                             {
    //        //                                 m_spreteEx.ChangeSprite(3);
    //        //                             }
    //        //                             else
    //        //                             {
    //        //                                 m_spreteEx.ChangeSprite(2);
    //        //                             }
    //        //                         }
    //        //                         else
    //        //                         {
    //        //                             m_spreteEx.ChangeSprite(1);
    //        //                         }
    //        //                     }
    //        //                 }
    //        //             }
    //        //             else
    //        //             {
    //        //                 if (DataManager.Manager<ClanManger>().ClanInfo != null)
    //        //                 {
    //        //                     GameCmd.stClanMemberInfo clanInfo = DataManager.Manager<ClanManger>().ClanInfo.GetMemberInfo((uint)MainPlayerHelper.GetPlayerID());
    //        // 
    //        //                     if (clanInfo != null)
    //        //                     {
    //        //                         if (clanInfo.duty == GameCmd.enumClanDuty.CD_Member)
    //        //                         {
    //        //                         }
    //        //                     }
    //        //                 }
    //        // 
    //        //                 GVoiceManger.Instance.CloseMic();
    //        //             }
    //    }
    //    else if (nEventId == (int)Client.GameEventID.CHAT_SPEEKERNOW)
    //    {
    //        string name = (string)param;

    //    }
    //}

    void SetStatus(string status)
    {
        if (m_goClan != null && m_goteam != null && m_gomute != null)
        {
            bool mute = status.Equals("mute");

            m_goClan.SetActive(status.Equals("clan"));
            m_goteam.SetActive(status.Equals("team"));
            m_gomute.SetActive(mute);

            SetSettingBtn(status);


            bool showSetting = false;
            if (DataManager.Manager<ClanManger>().ClanInfo != null)
            {
                GameCmd.stClanMemberInfo clanInfo = DataManager.Manager<ClanManger>().ClanInfo.GetMemberInfo((uint)MainPlayerHelper.GetPlayerID());

                if (clanInfo != null)
                {
                    showSetting = clanInfo.duty != GameCmd.enumClanDuty.CD_Member;
                }
            }
        }
    }

    void SetExpand(bool open)
    {
        m_spreteVoiceEx.ChangeSprite(open ? 2 : 1);
        //         m_sprite_expand.alpha = open ? 1f : 0f;
        //         if (GVoiceManger.Instance.JoinRoomType != JoinRoomEnum.None)
        //         {
        //             m_label_Speekname.transform.parent.gameObject.SetActive(!open);
        //         }
        //         else
        //         {
        //             m_label_Speekname.transform.parent.gameObject.SetActive(false);
        //         }
        // 
        //         m_btn_voiceChannel.transform.Find("Sprite").GetComponent<UISpriteEx>().ChangeSprite(open ? 2 : 1);
    }

    public void SetSettingBtn(string status)
    {
        //         if (status.Equals("team"))
        //         {
        //             m_btn_voiceBtn.gameObject.SetActive(true);
        //             if (GVoiceManger.Instance.IsOpenMic)
        //             {
        //                 m_spreteEx.ChangeSprite(3);//语音开
        //             }
        //             else
        //             {
        //                 m_spreteEx.ChangeSprite(2);//语音关
        //             }
        //         }
        //         else if (status.Equals("clan"))
        //         {
        //             if (DataManager.Manager<ClanManger>().ClanInfo != null)
        //             {
        //                 GameCmd.stClanMemberInfo clanInfo = DataManager.Manager<ClanManger>().ClanInfo.GetMemberInfo((uint)MainPlayerHelper.GetPlayerID());
        // 
        //                 if (clanInfo != null)
        //                 {
        //                     if (clanInfo.duty != GameCmd.enumClanDuty.CD_Member)
        //                     {
        //                         m_btn_voiceBtn.gameObject.SetActive(true);
        //                         m_spreteEx.ChangeSprite(1);
        //                     }
        //                     else
        //                     {
        //                         GameCmd.VoChatMode mode = DataManager.Manager<ChatDataManager>().ClanChatMode;
        //                         if (mode == GameCmd.VoChatMode.VoChatMode_Freedom || mode == GameCmd.VoChatMode.VoChatMode_None)
        //                         {
        //                             Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CHAT_SETMICBTN, true);
        //                         }
        //                         else if (mode == GameCmd.VoChatMode.VoChatMode_Leader)
        //                         {
        //                             Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CHAT_SETMICBTN, false);
        //                         }
        //                     }
        //                 }
        //             }
        //         }
        //         else
        //         {
        //             m_btn_voiceBtn.gameObject.SetActive(false);
        //         }
    }

    //void SetSpeek(string name)
    //{
    //    //         if (string.IsNullOrEmpty(name))
    //    //         {
    //    //             m_label_Speekname.transform.parent.gameObject.SetActive(false);
    //    //         }
    //    //         else
    //    //         {
    //    //             m_label_Speekname.transform.parent.gameObject.SetActive(true);
    //    //             m_label_Speekname.text = name;
    //    //         }
    //}

    void onClick_VoiceChannel_Btn(GameObject caster)
    {
        //         if (m_sprite_expand.alpha < 1f)
        //         {
        //             SetExpand(true);
        //         }else if (m_sprite_expand.alpha > 0f)
        //         {
        //             SetExpand(false);
        //         }
    }

    //void onClick_TeamBtn_Btn(GameObject caster)
    //{
    //    Engine.Utility.Log.Error("onClick_TeamBtn_Btn");
    //    SetExpand(false);

    //    JoinTeam();
    //}

    //private void JoinTeam()
    //{
    //    Engine.Utility.Log.Error(" simplechat JoinTeam");
    //    if (GVoiceManger.Instance.JoinRoomType != JoinRoomEnum.Team)
    //    {
    //        int openLevel = GameTableManager.Instance.GetGlobalConfig<int>("ChatVoiceTeam");

    //        if (MainPlayerHelper.GetPlayerLevel() < openLevel)
    //        {
    //            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Talk_ActualTime_Xjikaiqiduiwuliaotianshi, openLevel);
    //            return;
    //        }

    //        if (DataManager.Manager<TeamDataManager>().IsJoinTeam == false)
    //        {
    //            TipsManager.Instance.ShowTips(LocalTextType.Talk_ActualTime_haiweijiaruduiwuwufajinruliaotianshi);
    //            return;
    //        }
    //        if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Nation)
    //        {
    //            GVoiceManger.Instance.QuitRoom(GVoiceManger.Instance.JoinRoomName, (b) =>
    //            {
    //                GVoiceManger.Instance.JoinTeamRoom();
    //            });
    //        }
    //        else
    //        {

    //            Engine.Utility.Log.Error("   GVoiceManger.Instance.JoinTeamRoom();");
    //            GVoiceManger.Instance.JoinTeamRoom();
    //        }
    //    }
    //}

    //void onClick_ClanBtn_Btn(GameObject caster)
    //{
    //    SetExpand(false);
    //    JoinClan();
    //}

    //private void JoinClan()
    //{
    //    if (GVoiceManger.Instance.JoinRoomType != JoinRoomEnum.Nation)
    //    {
    //        int openLevel = GameTableManager.Instance.GetGlobalConfig<int>("ChatVoiceClan");

    //        if (MainPlayerHelper.GetPlayerLevel() < openLevel)
    //        {
    //            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Talk_ActualTime_Xjikaiqishizuliaotianshi, openLevel);
    //            return;
    //        }
    //        if (DataManager.Manager<ClanManger>().IsJoinClan == false)
    //        {
    //            TipsManager.Instance.ShowTips(LocalTextType.Talk_ActualTime_haiweijiarushizuwufajinruliaotianshi);
    //            return;
    //        }

    //        if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Team)
    //        {
    //            GVoiceManger.Instance.QuitRoom(GVoiceManger.Instance.JoinRoomName, (b) =>
    //            {
    //                GVoiceManger.Instance.JoinClanRoom();
    //            });
    //        }
    //        else
    //        {
    //            GVoiceManger.Instance.JoinClanRoom();
    //        }
    //    }
    //}
    //void onClick_MuteBtn_Btn(GameObject caster)
    //{
    //    SetExpand(false);

    //    if (GVoiceManger.Instance.JoinRoomType != JoinRoomEnum.None)
    //    {
    //        GVoiceManger.Instance.QuitRoom(GVoiceManger.Instance.JoinRoomName);
    //    }
    //    //  m_label_Speekname.transform.parent.gameObject.SetActive(false);
    //}

    //void onClick_VoiceBtn_Btn(GameObject caster)
    //{
    //    if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Team)
    //    {
    //        if (GVoiceManger.Instance.IsOpenMic)
    //        {
    //            GVoiceManger.Instance.CloseMic();
    //            m_spreteEx.ChangeSprite(2);
    //        }
    //        else
    //        {
    //            GVoiceManger.Instance.OpenMic();
    //            m_spreteEx.ChangeSprite(3);
    //        }
    //    }
    //    else if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Nation)
    //    {
    //        if (DataManager.Manager<ClanManger>().ClanInfo != null)
    //        {
    //            GameCmd.stClanMemberInfo clanInfo = DataManager.Manager<ClanManger>().ClanInfo.GetMemberInfo((uint)MainPlayerHelper.GetPlayerID());

    //            if (clanInfo != null)
    //            {
    //                if (clanInfo.duty != GameCmd.enumClanDuty.CD_Member)
    //                {
    //                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.VoiceSetting);
    //                }
    //                else
    //                {
    //                    if (GVoiceManger.Instance.IsOpenMic)
    //                    {
    //                        GVoiceManger.Instance.CloseMic();
    //                        m_spreteEx.ChangeSprite(2);
    //                    }
    //                    else
    //                    {
    //                        GVoiceManger.Instance.OpenMic();
    //                        m_spreteEx.ChangeSprite(3);
    //                    }
    //                }
    //            }
    //        }

    //    }
    //}
    #endregion

    #region Op

    private void RefreshBtns()
    {
        List<PushMsg> lstTransmitMsg = DataManager.Manager<FunctionPushManager>().M_lstTransmit;
        if (lstTransmitMsg.Count <= 0)
        {
            if (m_trans_PushRoot.gameObject.activeSelf)
            {
                m_trans_PushRoot.gameObject.SetActive(false);
            }
        }
        else
        {
            if (m_trans_PushRoot.gameObject.activeSelf == false)
            {
                m_trans_PushRoot.gameObject.SetActive(true);
            }
        }

        SysMsgPushBtn msgBtn = null;
        int i = 0;
        Dictionary<PushMsg.MsgType, Dictionary<uint, PushMsg>>.Enumerator iter = DataManager.Manager<FunctionPushManager>().M_dicTransmitMsg.GetEnumerator();
        while (iter.MoveNext())
        {
            Dictionary<uint, PushMsg>.Enumerator iter2 = iter.Current.Value.GetEnumerator();
            while (iter2.MoveNext())
            {
                if (i >= m_lstSysMsgPushBtn.Count)
                {
                    msgBtn = GetSysMsgPushBtn();
                    if (msgBtn != null)
                    {
                        m_lstSysMsgPushBtn.Add(msgBtn);
                    }
                }
                else
                {
                    msgBtn = m_lstSysMsgPushBtn[i];
                }
                msgBtn.gameObject.SetActive(msgBtn.m_l_leftSeconds > 0);
                msgBtn.SetTransmitUI(iter2.Current.Value);
                msgBtn.transform.localPosition = new UnityEngine.Vector3(-i * OFFSETX, 0, 0);
                i++;
            }
        }
        for (int k = i; k < m_lstSysMsgPushBtn.Count; k++)
        {
            m_lstSysMsgPushBtn[k].gameObject.SetActive(false);
        }
    }
    SysMsgPushBtn GetSysMsgPushBtn()
    {
        SysMsgPushBtn msgBtn = null;
        GameObject go = NGUITools.AddChild(m_trans_PushRoot.gameObject, m_goPrefab);
        if (go != null)
        {
            msgBtn = go.AddComponent<SysMsgPushBtn>();
        }
        UIParticleWidget wight = msgBtn.GetComponent<UIParticleWidget>();
        if (null == wight)
        {
            wight = msgBtn.gameObject.AddComponent<UIParticleWidget>();
            wight.depth = 100;
        }

        //只出现了一个特效  如果要改成全都加特效  就不要判断effect
        if (wight != null)
        {
            UIParticleWidget p = wight.GetComponent<UIParticleWidget>();
            if (p == null)
            {
                p = wight.gameObject.AddComponent<UIParticleWidget>();
                p.depth = 20;
            }
            if (p != null)
            {

                p.SetDimensions(1, 1);
                p.ReleaseParticle();
                p.AddParticle(50043);
            }
        }

        return msgBtn;
    }

    void SetPanelClip(bool expand)
    {
        m_sprite_btn_chat_shrink.transform.localRotation = Quaternion.Euler(new Vector3(expand ? 180 : 0, 0, 0));
        int panelOffsetY = 52;
        m_sprite_Background.height = expand ? bgHeight + panelOffsetY : bgHeight;

        float offsetY = m_chatScrollview.panel.clipOffset.y + 1;
        if (offsetY < 0)
        {
            m_chatScrollview.MoveRelative(new Vector3(0, offsetY));//取反 往下相对移动
        }

        Vector4 clip = m_UIPanel.baseClipRegion;
        clip.w = m_sprite_Background.height;//size.y
        clip.z = m_sprite_Background.width;//size.x

        Vector3 pos = m_chatScrollview.transform.parent.transform.localPosition;
        pos.y = clip.w * 0.5f;

        m_chatScrollview.transform.parent.transform.localPosition = pos;

        m_trans_chatItemRoot.localPosition = new Vector3(-clip.z * 0.5f + 5, clip.w * 0.5f - 5, 0);

        m_UIPanel.baseClipRegion = clip;
        m_UIPanel.clipOffset = Vector2.zero;

        pos = m_trans_PushContent.transform.localPosition;
        pos.y = m_sprite_Background.height;
        m_trans_PushContent.transform.localPosition = pos;

        UIWidget showWindowbtnWidget = m_btn_btn_ShowWindow.GetComponent<UIWidget>();
        if (showWindowbtnWidget != null)
        {
            showWindowbtnWidget.height = m_sprite_Background.height;
            showWindowbtnWidget.ResizeCollider();
        }
    }

    void Init()
    {
        m_chatdata.OnSimpleRefresh = OnRefreshText;
        m_chatdata.InitSimpleOutputText();
        m_chatdata.OnSimpleAddOutput = OnAddText;
    }
    void ReleaseChat()
    {
        if (m_chatdata != null)
        {
            m_chatdata.OnSimpleRefresh = null; ;
            m_chatdata.OnSimpleAddOutput = null;
            m_chatdata = null;
        }

    }
    void OnClickUrl(UIWidget sender, string url)
    {
        Debug.Log("url :" + url);
        if (!string.IsNullOrEmpty(url))
        {
            DataManager.Manager<ChatDataManager>().OnUrlClick(url);
        }
    }

    private void OnRefreshText(IEnumerable<ChatInfo> textList)
    {
        OnAddText(textList);
    }

    protected override void OnEnable()
    {
        if (m_lsttextList.Count > 0)
        {
            OnAddText(m_lsttextList);
            m_lsttextList.Clear();
        }
        RefreshBtns();
    }
    /// <summary>
    /// 新添加
    /// </summary>
    /// <param name="textList"></param>
    private void OnAddText(IEnumerable<ChatInfo> textList)
    {
        if (!gameObject.activeSelf)
        {
            m_lsttextList.AddRange(textList);
            return;
        }
        var list = textList.TakeLast(MaxChatNum).ToList();
        foreach (var item in list)
        {
            if (item.IsRedPacket)
            {//红包不显示
                continue;
            }
            UIXmlRichText xmlText;
            if (!RemoveOverfloorChat(out xmlText))
            {
                GameObject go = GameObject.Instantiate(m_chatItemPrefab) as GameObject;
                xmlText = go.transform.Find("richtext").GetComponent<UIXmlRichText>();
                go.GetComponent<UIWidget>().width = (int)m_UIPanel.baseClipRegion.z - 10;
                xmlText.UrlClicked += OnClickUrl;
            }
            xmlText.fontSize = 20;
            Transform root = xmlText.transform.parent;
            root.gameObject.SetActive(true);
            xmlText.AddXml(item.Title + item.Content);
            xmlText.gameObject.SetActive(true);
            Transform t = xmlText.transform;
            root.parent = m_trans_chatItemRoot;
            root.localPosition = Vector3.zero;
            root.localScale = Vector3.one;
            root.localRotation = Quaternion.identity;

            UILabel[] uilabelArray = t.GetComponentsInChildren<UILabel>();
            if (uilabelArray.Length > 0)
            {
                UILabel lableTitle = uilabelArray[0];
                if (lableTitle != null)
                {
                    UISpriteEx spriteBg = root.transform.Find("titlebg").GetComponent<UISpriteEx>();

                    if (spriteBg != null)
                    {
                        spriteBg.ChangeSprite(SingleChatItem.GetChannelIndex(item.Channel));
                    }

                    // spriteBg.transform.parent = title;
                    spriteBg.pivot = lableTitle.pivot;
                    spriteBg.transform.localPosition = new Vector3(lableTitle.transform.localPosition.x - 3, lableTitle.transform.localPosition.y, 0);
                    spriteBg.height = lableTitle.height + 5;
                    spriteBg.width = lableTitle.width + 3;

                }
            }

            float height = xmlText.GetTotalHeight() + 5;
            foreach (var moveItem in m_lstxmlText)
            {
                Vector3 pos = moveItem.transform.parent.localPosition;
                pos.y -= height;
                moveItem.transform.parent.localPosition = pos;

                xmlText.name = pos.y.ToString();
            }
            m_lstxmlText.Add(xmlText);
        }

        float offsetY = m_chatScrollview.panel.clipOffset.y + 1;
        if (offsetY < 0)
        {
            m_chatScrollview.MoveRelative(new Vector3(0, offsetY));//取反 往下相对移动
        }
    }

    /// <summary>
    /// 移除超过聊天数目限制的信息
    /// </summary>
    /// <param name="addingCount"></param>
    private bool RemoveOverfloorChat(out UIXmlRichText richtext)
    {
        richtext = null;
        if (m_lstxmlText.Count >= MaxChatNum)
        {
            richtext = m_lstxmlText[0];//移除第一个
            //Transform title = richtext.transform.Find("titlebg");
            // if (title != null)
            // {
            //                 Transform spriteBg = title.Find("titlebg");
            //                 if (spriteBg != null)
            //                 {
            //                     spriteBg.parent = richtext.transform;
            //                 }
            //   title.parent = this.transform;
            //}
            m_lstxmlText.RemoveAt(0);
            richtext.Clear();
            // title.parent = richtext.transform;
            return true;
        }
        return false;
    }
    #endregion

    #region UIEvent
    const int bgHeight = 105;
    void onClick_Btn_chat_shrink_Btn(GameObject go)
    {
        SetPanelClip(m_UIPanel.baseClipRegion.w == bgHeight);
    }

    void onClick_Btn_chat_setting_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ChannelSettingPanel);
    }

    void onClick_Btn_yuyin_Btn(GameObject caster)
    {

    }

    void onClick_Btn_ShowWindow_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ChatPanel, data: true);
    }
    void OnClick_ChatSeting_btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ChannelSettingPanel);
    }
    void OnPress_Btn_ClanVoice()
    {
        m_trans_voiceMicTips.gameObject.SetActive(false);
        if (PressTime != 0 && Time.time - PressTime >= 0.4f)
        {

            if (DataManager.Manager<ClanManger>().IsJoinClan)
            {
                GVoiceManger.Instance.StopRecording();
                bool send = Focus(m_ClanVoice);
                if (send)
                {
                    m_chatdata.SetChatInputType(GameCmd.CHATTYPE.CHAT_CLAN);
                    GVoiceManger.Instance.UploadRecordedFile();
                }
                else
                {
                    TipsManager.Instance.ShowTips("语音取消发送");
                }
            }
        }
        else
        {
            TipsManager.Instance.ShowTips("点击切换语音频道，长按可发言");
            m_ClanVoice.SetActive(false);
            m_TeamVoice.SetActive(true);
            m_WorldVoice.SetActive(false);
        }
        GVoiceManger.Instance.SoundMute(true);
        PressTime = 0;
    }
    void OnPress_Btn_TeamVoice()
    {
        if (PressTime != 0 && Time.time - PressTime >= 0.4f)
        {
            m_trans_voiceMicTips.gameObject.SetActive(false);

            if (DataManager.Manager<TeamDataManager>().IsJoinTeam)
            {
                GVoiceManger.Instance.StopRecording();
                bool send = Focus(m_TeamVoice);
                if (send)
                {
                    m_chatdata.SetChatInputType(GameCmd.CHATTYPE.CHAT_TEAM);
                    GVoiceManger.Instance.UploadRecordedFile();
                }
                else
                {
                    TipsManager.Instance.ShowTips("语音取消发送");
                }
            }
        }
        else
        {
            TipsManager.Instance.ShowTips("点击切换语音频道，长按可发言");
            m_ClanVoice.SetActive(false);
            m_TeamVoice.SetActive(false);
            m_WorldVoice.SetActive(true);
        }
        PressTime = 0;
        GVoiceManger.Instance.SoundMute(true);
    }
    void OnPress_Btn_WorldVoice()
    {
        if (PressTime != 0 && Time.time - PressTime >= 0.4f)
        {
            m_trans_voiceMicTips.gameObject.SetActive(false);
            GVoiceManger.Instance.StopRecording();
            bool send = Focus(m_WorldVoice);
            if (send)
            {
                m_chatdata.SetChatInputType(GameCmd.CHATTYPE.CHAT_WORLD);
                GVoiceManger.Instance.UploadRecordedFile();
            }
            else
            {
                TipsManager.Instance.ShowTips("语音取消发送");
            }
        }
        else
        {
            TipsManager.Instance.ShowTips("点击切换语音频道，长按可发言");
            m_ClanVoice.SetActive(true);
            m_TeamVoice.SetActive(false);
            m_WorldVoice.SetActive(false);
        }
        PressTime = 0;
        GVoiceManger.Instance.SoundMute(true);
    }
    void OnDrag_Btn_Voice(GameObject caster, UnityEngine.Vector2 pos)
    {
        bool send = Focus(caster);
        m_MicLeft.SetActive(send);
        m_MicRight.SetActive(!send);
    }
    bool Focus(GameObject go)
    {
        Camera camera = Util.UICameraObj.GetComponent<Camera>();
        UnityEngine.Vector2 touchPos = UICamera.currentTouch.pos;
        Ray ray = camera.ScreenPointToRay(touchPos);
        int mask = camera.cullingMask & (int)UICamera.current.eventReceiverMask;
        float dist = (UICamera.current.rangeDistance > 0f) ? UICamera.current.rangeDistance : camera.farClipPlane - camera.nearClipPlane;
        RaycastHit[] hits = Physics.RaycastAll(ray, dist, mask);
        if (hits.Length >= 1)
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
    void OnDownLongPressClanVoice()
    {
        if (DataManager.Manager<ClanManger>().IsJoinClan)
        {
            OnDownLongPressWorldVoice();
        }
        else
        {
            TipsManager.Instance.ShowTipsById(403003);
        }

    }
    void OnDownLongPressTeamVoice()
    {
        if (DataManager.Manager<TeamDataManager>().IsJoinTeam)
        {
            OnDownLongPressWorldVoice();
        }
        else
        {
            TipsManager.Instance.ShowTipsById(403004);
        }
    }
    void OnDownLongPressWorldVoice()
    {
        m_trans_voiceMicTips.gameObject.SetActive(true);
        GVoiceManger.Instance.SoundMute(false);
        PressTime = Time.time;
        GVoiceManger.Instance.StartRecording();
        m_MicLeft.SetActive(true);
        m_MicRight.SetActive(false);
    }

    void StopRecording()
    {
        GVoiceManger.Instance.StopRecording();
        if (null != m_trans_voiceMicTips && m_trans_voiceMicTips.gameObject.activeSelf)
            m_trans_voiceMicTips.gameObject.SetActive(false);
    }
    #endregion

}