using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Common;
using Engine;
using GameCmd;
using table;

namespace Client
{
    public class SettingManager : BaseModuleData, IManager
    {
        //        private static string GAME_OPTION = "Config/Option.ini";
        public static int MAX_PLAYER = GameTableManager.Instance.GetClientGlobalConst<int>("Setting", "MAX_PLAYER");
        public static int MIN_PLAYER = GameTableManager.Instance.GetClientGlobalConst<int>("Setting", "MIN_PLAYER");

        private Engine.Utility.IniFile m_OptionFile = new Engine.Utility.IniFile();
        Client.IGameOption option = Client.ClientGlobal.Instance().gameOption;

        public int FixedShortcutItemCount { get; set; }  //快捷道具使用固定的item数量

        public enum eAttackPriority
        {
            Player = 1,   //人
            Monster,      //怪
            Creature,     //生物（不限）
        }

        public eAttackPriority AttackPriority { set; get; }

        public void ClearData()
        {

        }
        public void Initialize()
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SYSTEM_GAME_READY, OnEvent);
            OptionApplication();
        }
        public void Reset(bool depthClearData = false)
        { }
        public void Process(float deltime)
        { }

        void OnEvent(int eventID, object param)
        {
            if (eventID == (int)GameEventID.SYSTEM_GAME_READY)
            {
                OptionApplication();
            }
        }
        //全部应用
        public void OptionApplication()
        {
            SetPriority();


            SetRenderShadow();   //去除重复
            SetIntoTeam();       //ok
            SetFollow();         //ok
            SetHp();             //ok
            SetHpvalue();        //ok
            SetHpitemid();       //ok
            SetMp();             //ok
            SetMpvalue();        //ok
            SetMpitemid();       //ok
            SetHpAtOnce();       //ok
            SetHpAtOncevalue();  //ok
            SetHpAtOnceitemid(); //ok
            SetPetHp();          //ok
            SetPetHpvalue();     //ok
            SetPetHpitemid();    //ok
            SetAutoRepairEquip();
            SetRepairValue();
            SetAutoReturn();
            SetHpLimtvalue();
            SetPickMedecal();
            SetEquipLevel();
            SetCheckSound();        //ok
            //SetCheckSoundEffects();
            //SetCheckVoice();
            Setsoundslider();//ok
            SetSoundEffectsslider();//ok
            //SetVoiceslider();
            SetSettingFixedRocker();
            SetSettingTeamInvite(); //ok
            SetSettingClanInvite(); //ok
            SetSettingPlayer();
            SetSettingMonster();
            SetSettingNone();
            //SetSettingShockScreen();
            SetPlayerName();         //ok
            SetPlayerTitle();
            SetClanName();           //ok
            SetMonsterName();        //ok
            SetHpDisplay();          //ok
            //SetExpDisplay();
            SetHurtDisplay();        //ok
            SetmineEffect();
            SetotherEffect();
            SetBlockPlayers();       //ok
            SetBlockMonster();       //ok
            SetBlockFashion();
            SetGrassland();          //ok
            //Setshadow();             //ok
            SetScreenNumberslider(); //ok
            SetCameraDistance();
            SetSkillShake();
            SetFixedShortcutItemCount();


            SetOnlyCreatMainPlayer();
            SetIgnoreLand();
            SetIgnoreGrass();
            SetIgnoreStatic();
            SetIgnoreScene();

        }

        public void SetRenderShadow()
        {
            int nShadowLevel = ClientGlobal.Instance().gameOption.GetInt("Render", "shadow", 0);
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs != null)
            {
                rs.SetShadowLevel((Engine.ShadowLevel)nShadowLevel);
            }
        }
        #region 挂机页
        //设置自动入队
        public void SetIntoTeam()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("TeamSetting", "IntoTeam", 1) == 1;
            DataManager.Manager<TeamDataManager>().SetLeaderAutoAgreeTeamApply(value);
        }
        //设置自动跟随
        public void SetFollow()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("TeamSetting", "Follow", 1) == 1;
            DataManager.Manager<TeamDataManager>().SetMemberAutoAllowTeamFollow(value);
        }
        //血药开关
        public void SetHp()
        {
            int value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "Hp", 1);
        }
        //血药slider
        public void SetHpvalue()
        {
            float value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "Hpvalue", 1) / 100.0f;
        }
        //血药图
        public void SetHpitemid()
        {
            int value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "Hpitemid", 1);

        }
        //蓝药开关
        public void SetMp()
        {
            int value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "Mp", 1);
        }
        //蓝药slider
        public void SetMpvalue()
        {
            float value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "Mpvalue", 1) / 100.0f;
        }
        //蓝药图片
        public void SetMpitemid()
        {
            int value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "Mpitemid", 1);

        }


        //瞬回血药开关
        public void SetHpAtOnce()
        {
            int value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "HpAtOnce", 1);
        }
        //瞬回血药slider
        public void SetHpAtOncevalue()
        {
            float value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "HpAtOncevalue", 1) / 100.0f;
        }
        //瞬回血药图
        public void SetHpAtOnceitemid()
        {
            int value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "HpAtOnceitemid", 1);

        }


        // 宠物血药开关
        public void SetPetHp()
        {
            int value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "PetHp", 1);
        }
        //宠物血药slider
        public void SetPetHpvalue()
        {
            float value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "PetHpvalue", 1) / 100.0f;

        }
        //宠物喂食的物品图
        public void SetPetHpitemid()
        {
            int value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "PetHpitemid", 1);

        }


        //自动修理装备开关
        public void SetAutoRepairEquip()
        {
            int value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "AutoRepairEquip", 1);

        }
        //装备耐久低于多少时slider
        public void SetRepairValue()
        {
            float value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "Equipvalue", 1) / 100.0f;

        }


        //自动回城
        public void SetAutoReturn()
        {
            int value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "AutoReturn", 1);

        }
        //生命低于多少时自动回城slider
        public void SetHpLimtvalue()
        {
            float value = ClientGlobal.Instance().gameOption.GetInt("MedicalSetting", "HpLimtvalue", 1) / 100.0f;

        }


        //自动拾取装备开关
        public void SetPickMedecal()
        {
            int value = ClientGlobal.Instance().gameOption.GetInt("Pick", "PickMedecal", 1);
        }
        //拾取装备的等级slider
        public void SetEquipLevel()
        {
            float value = ClientGlobal.Instance().gameOption.GetInt("Pick", "EquipLevel", 1) / 7.0f;
        }

        /// <summary>
        /// 设置快捷使用道具 固定栏数量
        /// </summary>
        public void SetFixedShortcutItemCount()
        {
            FixedShortcutItemCount = ClientGlobal.Instance().gameOption.GetInt("Shortcut", "SetItemCountSlider", 25) / 25;
        }

        public void SetFixedShortcutItem1(uint itemId)
        {

        }

        #endregion


        #region  基础页
        //静音
        bool cancelSound = false;
        public void SetCheckSound()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("Sound", "CheckSound", 1) == 1;
            Engine.IAudio au = Engine.RareEngine.Instance().GetAudio();
            au.Mute(value);
            cancelSound = value;
        }
        //         //音效开关
        //         public void SetCheckSoundEffects()
        //         {
        //             int value = ClientGlobal.Instance().gameOption.GetInt("Sound", "CheckSoundEffects", 1);
        //         }
        //         //技能声音开关
        //         public void SetCheckVoice()
        //         {
        //             int value = ClientGlobal.Instance().gameOption.GetInt("Sound", "CheckVoice", 1);
        //         }
        //音量滑动条
        public void Setsoundslider()
        {
            if (!cancelSound)
            {
                float value = ClientGlobal.Instance().gameOption.GetInt("Sound", "soundslider", 0) / 100.0f;
                Engine.IAudio au = Engine.RareEngine.Instance().GetAudio();
                au.SetMusicVolume(value);
            }
        }
        //音效滑动条
        public void SetSoundEffectsslider()
        {
            float value = ClientGlobal.Instance().gameOption.GetInt("Sound", "SoundEffectsslider", 0) / 100.0f;
            Engine.IAudio au = Engine.RareEngine.Instance().GetAudio();
            au.SetEffectVolume(value);
        }
        //技能声音滑动条
        public void SetVoiceslider()
        {
            float value = ClientGlobal.Instance().gameOption.GetInt("Sound", "Voiceslider", 0) / 100.0f;

        }
        void SetSettingFixedRocker()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("Operate", "SettingFixedRocker", 1) == 1;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.EJOYSTICKSTABLE, value);
        }
        //自动组队
        public void SetSettingTeamInvite()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("Social", "SettingTeamInvite", 1) == 1;
            DataManager.Manager<TeamDataManager>().SetAutoRefuseTeamInvite(value);
        }
        //自动接收氏族邀请
        public void SetSettingClanInvite()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("Social", "SettingClanInvite", 1) == 1;
            DataManager.Manager<ClanManger>().AutoRefuseInvite = value;
        }
        //攻击目标--玩家优先
        public void SetSettingPlayer()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("AttackTarget", "SettingPlayer", 1) == 1;
            if (value)
            {
                this.AttackPriority = eAttackPriority.Player;
            }
        }
        //攻击目标--怪物优先
        public void SetSettingMonster()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("AttackTarget", "SettingMonster", 1) == 1;
            if (value)
            {
                this.AttackPriority = eAttackPriority.Monster;
            }
        }
        //攻击目标--无
        public void SetSettingNone()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("AttackTarget", "SettingNone", 1) == 1;
            if (value)
            {
                this.AttackPriority = eAttackPriority.Creature;
            }
        }
        //是否震屏
        public void SetSettingShockScreen()
        {

        }
        public void SetCameraDistance()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("Camera", "close", 1) == 1;
            Client.EntityCreator.Instance().isFarDisCamera = value;
        }
        #endregion


        #region 画质页

        //设置画质效果的等级   包括下面三个SetSettingScreenPriority +   SetPerformancePriority    +     SetSavingPriority
        void SetPriority()
        {
            int type = ClientGlobal.Instance().gameOption.GetInt("PictureEffect", "GriphicSlider", 1);
            SetSettingScreenPriority((uint)type);
        }
        public void SetSettingScreenPriority(uint priority)
        {
            SettingDataBase data = GameTableManager.Instance.GetTableItem<SettingDataBase>(priority);
            if (data != null)
            {
                //草地扰动
                bool grassIsOpen = (data.GrassMoveGrade != 0) ? true : false;
                IMapSystem mapSys = ClientGlobal.Instance().GetMapSystem();
                mapSys.EnableGrassForce(grassIsOpen);

                Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
                if (rs != null)
                {
                    //实时阴影
                    rs.SetShadowLevel((Engine.ShadowLevel)data.RealTime_Shadow);

                    //特效等级
                    rs.effectLevel = (int)data.ParticleGrade;
                }

                //同屏人数
                Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
                int value = (int)data.PlayerNum;
                if (es != null)
                {
                    if (value > MAX_PLAYER || value < MIN_PLAYER)
                    {
                        value = MIN_PLAYER;
                    }
                    es.MaxPlayer = value;
                }
            }
        }

        //玩家名称
        public void SetPlayerName()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("ShowDetail", "PlayerName", 1) == 1;
            RoleStateBarManager.SetPlayerNameVisible(value);
        }
        public void SetPlayerTitle()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("ShowDetail", "PlayerTitle", 1) == 1;
            DataManager.Manager<TitleManager>().SetIsShowTitle(value);
        }
        public void SetClanName()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("ShowDetail", "ClanName", 1) == 1;
            DataManager.Manager<ClanManger>().ShowClanName = value;
        }
        public void SetMonsterName()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("ShowDetail", "MonsterName", 1) == 1;
            RoleStateBarManager.SetNpcNameVisible(value);
        }
        public void SetHpDisplay()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("ShowDetail", "HpDisplay", 1) == 1;
            RoleStateBarManager.SetHpSliderVisible(value);
        }

        public void SetHurtDisplay()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("ShowDetail", "HurtDisplay", 1) == 1;
            // FlyFontDataManager.Instance.m_bShowFlyFont = value;
        }


        //特效
        public void SetmineEffect()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("SpecialEffects", "mineEffect", 1) == 1;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.HIDETEXIAO_MINE, value);
        }
        public void SetotherEffect()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("SpecialEffects", "otherEffect", 1) == 1;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.HIDETEXIAO_OTHER, value);
        }
        public void SetBlockPlayers()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("SpecialEffects", "BlockPlayers", 1) == 1;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.HIDEOTHERPLAYER, value);
            //            EntitySystem.EntityHelper.ShowOtherPlayer(!value);
        }
        public void SetBlockMonster()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("SpecialEffects", "BlockMonster", 1) == 1;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.HIDEMONSTER, value);
            //            EntitySystem.EntityHelper.ShowMonster(!value);
        }
        public void SetBlockFashion()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("SpecialEffects", "BlockFashion", 1) == 1;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.HIDEFASHION, value);
        }
        //草地扰动
        public void SetGrassland()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("SpecialEffects", "Grassland", 1) == 1;
            IMapSystem mapSys = ClientGlobal.Instance().GetMapSystem();
            mapSys.EnableGrassForce(value);
        }
        public void SetSkillShake()
        {
            bool value = ClientGlobal.Instance().gameOption.GetInt("SpecialEffects", "skillShake", 0) == 1;
            CameraFollow.Instance.BEnableShake = value;
        }
        //         //设置阴影
        //         public void Setshadow()
        //         {
        //             bool value = ClientGlobal.Instance().gameOption.GetInt("SpecialEffects", "shadow", 1) == 1;
        //             Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
        //             if (rs != null)
        //             {
        //                 if (value)
        //                 {
        //                     rs.SetShadowLevel(Engine.ShadowLevel.Height);
        //                 }
        //                 else
        //                 {
        //                     rs.SetShadowLevel(Engine.ShadowLevel.None);
        //                 }
        //             }
        // 
        //         }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        List<ShortCuts> m_shortcutSetItemList = new List<ShortCuts>();

        bool haveShortcutData = false;

        /// <summary>
        /// 快捷使用道具 用于快捷设置UI
        /// </summary>
        /// <returns></returns>
        public List<ShortCuts> GetShortcutSetItemList()
        {
            if (AlreadySetShortcut())
            {
                return m_shortcutSetItemList;
            }
            else
            {
                List<uint> itemList = GameTableManager.Instance.GetGlobalConfigList<uint>("InitialShortcut");
                if (itemList != null)
                {
                    for (int i = 0; i < itemList.Count; i++)
                    {
                        ShortCuts sc = new ShortCuts { id = (uint)i, itemid = itemList[i] };
                        m_shortcutSetItemList.Add(sc);
                    }
                }

                return m_shortcutSetItemList;
            }

            //存在客户端写法
            //m_shortcutSetItemList.Clear();
            //for (uint i = 0; i <4;i++)
            //{
            //    string itemName = string.Format("ShortcutItem_" + i);
            //    int value = ClientGlobal.Instance().gameOption.GetInt("Shortcut", itemName, 0);

            //    ShortCuts sc = new ShortCuts();
            //    sc.id = i;
            //    sc.itemid = (uint)value;
            //    m_shortcutSetItemList.Add(sc);
            //}
            //return m_shortcutSetItemList;
        }

        /// <summary>
        /// 用于左侧的itemList
        /// </summary>
        /// <returns></returns>
        public List<uint> GetItemList()
        {
            List<ItemDataBase> idbList = GameTableManager.Instance.GetTableList<ItemDataBase>();
            List<ItemDataBase> list = idbList.FindAll((data) => { return data.isShortCut == 1; });

            List<uint> itemList = new List<uint>();
            for (int i = 0; i < list.Count; i++)
            {
                itemList.Add(list[i].itemID);
            }

            return itemList;
        }

        /// <summary>
        /// 是否设置过
        /// </summary>
        /// <returns></returns>
        bool AlreadySetShortcut()
        {
            if (m_shortcutSetItemList.Count != 0)
            {
                return true;
            }

            return false;
        }


        #endregion

        //同屏人数
        public void SetScreenNumberslider()
        {
            Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
            int value = ClientGlobal.Instance().gameOption.GetInt("OneScreen", "ScreenNumberslider", 0);
            if (es != null)
            {
                if (value > MAX_PLAYER || value < MIN_PLAYER)
                {
                    value = MIN_PLAYER;
                }
                es.MaxPlayer = value;
            }

        }


        #region 程序用
        public void SetOnlyCreatMainPlayer()
        {
            bool select = PlayerPrefs.GetInt(UIMsgID.eOnlyCreatMainPlayer.ToString(), 1) == 1;
        }
        public void SetIgnoreLand()
        {
            bool select = PlayerPrefs.GetInt(UIMsgID.eIgnoreLand.ToString(), 1) == 1;
        }
        public void SetIgnoreGrass()
        {
            bool select = PlayerPrefs.GetInt(UIMsgID.eIgnoreGrass.ToString(), 1) == 1;
        }
        public void SetIgnoreStatic()
        {
            bool select = PlayerPrefs.GetInt(UIMsgID.eIgnoreStatic.ToString(), 1) == 1;
        }
        public void SetIgnoreScene()
        {
            bool select = PlayerPrefs.GetInt(UIMsgID.eIgnoreScene.ToString(), 1) == 1;
        }
        #endregion

        #region Net

        public void ReqAllShortCutItemList(List<ShortCuts> shortcut)
        {
            stSendAllShortCutPropertyUserCmd_CS cmd = new stSendAllShortCutPropertyUserCmd_CS();
            for (int i = 0; i < shortcut.Count; i++)
            {
                cmd.shortcut.Add(shortcut[i]);
            }
            NetService.Instance.Send(cmd);
        }

        public void OnAllShortCutItemList(stSendAllShortCutPropertyUserCmd_CS cmd)
        {
            this.m_shortcutSetItemList.Clear();
            for (int i = 0; i < cmd.shortcut.Count; i++)
            {
                ShortCuts sc = new ShortCuts();
                sc.id = cmd.shortcut[i].id;
                sc.itemid = cmd.shortcut[i].itemid;

                this.m_shortcutSetItemList.Add(sc);
            }


            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainPanel))
            {
                DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eShortcutList, null);
            }
        }

        #endregion

        #region 反馈返回
        List<GameCmd.FeedbackData> m_lst_feedData = null;
        public List<GameCmd.FeedbackData> FeedBackMsgs
        {
            get 
            {
                if (m_lst_feedData == null)
                {
                    m_lst_feedData = new List<FeedbackData>();
                }
                return m_lst_feedData;
            }
            set
            {
                m_lst_feedData = value;
            }
        }
        public void OnRecieveFeedBackMsg(stFeedbackGMPropertyUserCmd_CS cmd)
        {
            m_lst_feedData = cmd.data;
            ValueUpdateEventArgs arg = new ValueUpdateEventArgs("FeedbackGM", null, null);
            DispatchValueUpdateEvent(arg);
//             if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.SettingPanel))
//             {
//                 SettingPanel panel = DataManager.Manager<UIPanelManager>().GetPanel<SettingPanel>(PanelID.SettingPanel);
//                 if (panel != null)
//                 {
//                      panel.OnFeedback(cmd.data);
//                 }
//             }

        }

        public void OnRecieveFeedBackWarning(stFeedbackNoticePropertyUserCmd_S cmd)
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SETTING_RECIEVEFEEDBACKNOTICE, null);
        }
        #endregion

    }
}