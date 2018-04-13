using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Client;
using Common;
using GameCmd;
using Engine;
using table;
using System.Text;

public partial class SettingPanel : UIPanelBase
{
    public enum RoleHMP
    {
        RoleBaseHp = 0,
        RoleBaseMp = 1,
        RoleHighHp = 2,
        PetBaseHp = 3,
        Max = 4,
    }
    public enum RightTagType
    {
        Base = 1,   //基础页签
        quality,    //画质页签
        fighting,   //挂机页签
        UseItem,    //快捷页签
        feedback,   //反馈页签
        programUse, //程序用页签    
        Max,
    }
    public enum ImageQuality
    {
        PowerSaving = 0,//省电
        Fluency = 1,//流畅
        Excellence = 2,//优秀
        Best = 3,//极佳
    }
    RightTagType m_Content = RightTagType.Base;

    // Dictionary<uint, UITabGrid> m_dic_tabs = new Dictionary<uint, UITabGrid>();
    List<RightTagType> contents = new List<RightTagType>();
    AutoRecoverGrid[] m_AutoRecoverGrids;
    GameObject LeftObj;
    uint feedBackType = 1;
    bool isRecieveFeedBackWarning = false;

    private CMResAsynSeedData<CMTexture> m_playerCASD = null;

    #region   override
    //    GameObject ShowingObject;
    GameObject obj;
    IPlayer player;
    protected override void OnLoading()
    {
        base.OnLoading();

        uint showUID = ClientGlobal.Instance().MainPlayer.GetID();
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }
        player = es.FindPlayer(showUID);
        m_label_Role_Name.text = player.GetName();
        m_label_Role_Id.text = player.GetID().ToString();
        Pmd.ZoneInfo curZoneInfo = DataManager.Manager<LoginDataManager>().GetZoneInfo();
        string zoneName = "未知";
        if (null != curZoneInfo)
        {
            zoneName = curZoneInfo.zonename;
        }

        if (null != m_label_Server_Name)
        {
            m_label_Server_Name.text = zoneName;
        }
        RegisterGlobalEvent(true);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        AwakeSettingFeedBackWarning();
        if (data == null)
        {
            ChoseAndSelect(RightTagType.Base);
        }
        if (data != null)
        {
            ChoseAndSelect((RightTagType)data,false);
        }
        DataManager.Manager<Client.SettingManager>().ValueUpdateEvent += OnUpdateList;
    }

    protected override void OnHide()
    {
        base.OnHide();
        SaveInOptionAtTime(m_Content);

        SaveShortcutItemSetting();//保存快捷设置到服务器
        RegisterGlobalEvent(false);

        ReleaseShortCut();

        DataManager.Manager<Client.SettingManager>().ValueUpdateEvent -= OnUpdateList;
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        ReleaseShortCut(depthRelease);
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();

        OnPanelBaseDestoryShortCut();
    }
    #endregion

    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIItemUpdata);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SETTING_RECIEVEFEEDBACKNOTICE, OnGlobalUIItemUpdata);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIItemUpdata);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SETTING_RECIEVEFEEDBACKNOTICE, OnGlobalUIItemUpdata);
        }
    }

    private void OnGlobalUIItemUpdata(int eventType, object data)
    {
        if (eventType == (int)Client.GameEventID.UIEVENT_UPDATEITEM)
        {
            //OnUpdateItemDataUI(data);//快捷使用道具  (暂时不需要更新这里)
        }
        else if (eventType == (int)Client.GameEventID.SETTING_RECIEVEFEEDBACKNOTICE)
        {

            isRecieveFeedBackWarning = true;
            AwakeSettingFeedBackWarning();

        }
    }

    void InitSettingPanel(RightTagType type)
    {
        Client.IGameOption option = Client.ClientGlobal.Instance().gameOption;
        if (option == null)
        {
            return;
        }
        obj = m_trans_left.Find(type + "Content").gameObject;
        if (obj == null)
        {
            Engine.Utility.Log.Error("m_trans_left中找不到该对象{0}Content", type);
        }
        UIToggle[] toggles = obj.GetComponentsInChildren<UIToggle>();
        if (toggles == null)
        {
            Engine.Utility.Log.Error("这里面没有Toggle", obj.name);
        }
        else
        {
            if (obj.name != "programUseContent" && obj.name != "feedbackContent")
            {
                foreach (var tg in toggles)
                {
                    string[] tgName = tg.name.Split("_".ToCharArray());
                    if (tgName.Length == 2)
                    {
                        tg.value = option.GetInt(tgName[0], tgName[1], 1) == 1;
                        if (tg.value == false)
                        {
                            //如果没有勾选  取消滑动条的点击拖动并置灰
                            CancelClickSlider(tg);
                        }
                        if (tg.name == "Sound_CheckSound" && tg.value)
                        {
                            m_slider_Sound_soundslider.GetComponent<BoxCollider>().enabled = false;
                            m_slider_Sound_SoundEffectsslider.GetComponent<BoxCollider>().enabled = false;
                            m_slider_Sound_soundslider.transform.Find("Foreground").GetComponent<UISprite>().spriteName = "jindutiao_xiaohui";
                            m_slider_Sound_SoundEffectsslider.transform.Find("Foreground").GetComponent<UISprite>().spriteName = "jindutiao_xiaohui";
                        }

                    }

                }
            }
            if (obj.name == "programUseContent")
            {
                bool a = PlayerPrefs.GetInt(UIMsgID.eOnlyCreatMainPlayer.ToString(), 0) == 1;
                m_toggle_UI_OnMainPlayer.value = a;
                bool a1 = PlayerPrefs.GetInt(UIMsgID.eIgnoreLand.ToString(), 0) == 1;
                m_toggle_UI_IgnoreLand.value = a1;
                bool a2 = PlayerPrefs.GetInt(UIMsgID.eIgnoreGrass.ToString(), 0) == 1;
                m_toggle_UI_IgnoreGrass.value = a2;
                bool a3 = PlayerPrefs.GetInt(UIMsgID.eIgnoreStatic.ToString(), 0) == 1;
                m_toggle_UI_IgnoreStatic.value = a3;
                bool a4 = PlayerPrefs.GetInt(UIMsgID.eIgnoreScene.ToString(), 0) == 1;
                m_toggle_UI_IgnoreScene.value = a4;
                bool a5 = PlayerPrefs.GetInt(UIMsgID.eIgnoreWater.ToString(), 0) == 1;
                m_toggle_UI_IgnoreWater.value = a5;
                bool a6 = PlayerPrefs.GetInt(UIMsgID.ePreLoad.ToString(), 0) == 1;
                m_toggle_UI_PreLoad.value = a6;

            }
        }
        UISlider[] sliders = obj.GetComponentsInChildren<UISlider>();
        if (sliders == null)
        {
            Engine.Utility.Log.Error("这里面没有sliders", obj.name);
        }
        else
        {
            foreach (var sli in sliders)
            {
                string[] name = sli.name.Split("_".ToCharArray());
                if (name.Length == 2)
                {
                    if (sli.name == m_slider_OneScreen_ScreenNumberslider.name)
                    {
                        sli.value = (option.GetInt(name[0], name[1], 0) - SettingManager.MIN_PLAYER) * 1.0f / (SettingManager.MAX_PLAYER - SettingManager.MIN_PLAYER);
                        UILabel screenNumPercent = sli.transform.Find("Percent").GetComponent<UILabel>();
                        if (screenNumPercent != null)
                        {
                            screenNumPercent.text = string.Format("{0}人", option.GetInt(name[0], name[1], 0));
                        }
                    }
                    else if (sli.name == m_slider_Pick_EquipLevel.name)
                    {
                        sli.value = (option.GetInt(name[0], name[1], 0) - 1) / 6.0f;
                        UILabel lableEquip = m_slider_Pick_EquipLevel.transform.Find("Label").GetComponent<UILabel>();
                        if (lableEquip != null)
                        {
                            lableEquip.text = string.Format("{0}档以上装备", option.GetInt(name[0], name[1], 0));
                        }
                    }
                    else if (sli.name == m_slider_Shortcut_SetItemCountSlider.name)  //道具快捷设置
                    {
                        int count = option.GetInt(name[0], name[1], 25);
                        sli.value = count / 100f;
                        m_label_SetItemCountLabel.text = (count / 25).ToString();
                    }
                    else if (sli.name == m_slider_PictureEffect_GriphicSlider.name)
                    {
                        int priority = option.GetInt(name[0], name[1], 1);
                        int count = priority - 1;
                        float va = count / 3.0f;
                        m_slider_PictureEffect_GriphicSlider.value = va;
                        ChangePriorityValues((uint)priority);
                    }
                    else
                    {
                        sli.value = option.GetInt(name[0], name[1], 0) / 100.0f;
                    }
                }

            }
        }
    }
    /// <summary>
    /// 设置选中右侧页签
    /// </summary>
    /// <param name="type"></param>
    /// <param name="force"></param>
    private void SetSettingContentType(RightTagType type, bool force = false)
    {
        UITabGrid tab = null;
        if (dicUITabGrid.ContainsKey(1))
        {
            if (null != dicUITabGrid[1] && dicUITabGrid[1].TryGetValue((int)m_Content, out tab))
            {
                tab.SetHightLight(false);
            }
            m_Content = type;
            if (null != dicUITabGrid[1] && dicUITabGrid[1].TryGetValue((int)m_Content, out tab))
            {
                tab.SetHightLight(true);
            }
        }

    }



    public override bool OnTogglePanel(int tabType, int pageid)
    {
        RightTagType type = (RightTagType)pageid;
        ChoseAndSelect(type);


        return base.OnTogglePanel(tabType, pageid);
    }
    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }
        int firstTabData = -1;
        int secondTabData = -1;
        if (null != jumpData.Tabs && jumpData.Tabs.Length > 0)
        {
            firstTabData = jumpData.Tabs[0];

        }
        else
        {
            firstTabData = (int)m_Content;
        }
        OnTogglePanel(1, firstTabData);
        //UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTabData);
    }

    /// <summary>
    /// 给每页的组件注册变化时候的监听
    /// </summary>
    /// <param name="newType"></param>
    void ListenValueChangeRegister(RightTagType newType)
    {
        //         UIPopupList popList = m_sprite_texiaoLevel.GetComponent<UIPopupList>();
        //         UIEventListener.Get(popList.gameObject).onSelect = PopListControll;
        //         EventDelegate.Add(popList.onChange,() =>
        //         {
        //             SelectPopList(popList);
        //         });
        obj = m_trans_left.Find(newType + "Content").gameObject;
        UIToggle[] toggles = obj.GetComponentsInChildren<UIToggle>();
        if (toggles == null)
        {
            Engine.Utility.Log.Error("这里面没有Toggle", obj.name);
        }
        else
        {
            foreach (var item in toggles)
            {
                UIEventListener.Get(item.gameObject).onClick = ToggleControll;
                EventDelegate.Add(item.onChange, () =>
                {
                    SelectToggle(item);
                });
            }
        }

        UISlider[] sliders = obj.GetComponentsInChildren<UISlider>();
        if (sliders == null)
        {
            Engine.Utility.Log.Error("这里面没有Slider", obj.name);
        }
        else
        {
            if (obj.name == "fightingContent")
            {
                foreach (var sli in sliders)
                {
                    UIEventListener.Get(sli.gameObject).onClick = SliderControll;
                    UIEventListener.Get(sli.gameObject).onDragEnd = SliderControll;
                    if (sli.name == m_slider_Pick_EquipLevel.name)
                    {
                        UILabel lableEquip = m_slider_Pick_EquipLevel.transform.Find("Label").GetComponent<UILabel>();
                        EventDelegate.Add(m_slider_Pick_EquipLevel.onChange, () =>
                        {
                            if (lableEquip != null)
                                lableEquip.text = string.Format("{0}档以上装备", CaculateEquipLevel());
                        });
                    }
                    else
                    {
                        UILabel Hpvalue = sli.transform.Find("Label").GetComponent<UILabel>();
                        EventDelegate.Add(sli.onChange, () =>
                        {
                            if (Hpvalue != null)
                            {
                                Hpvalue.text = string.Format("{0}%", SelectSlider(sli));
                            }
                        });
                    }
                }
            }
            else
            {
                foreach (var sli in sliders)
                {
                    UIEventListener.Get(sli.gameObject).onClick = SliderControll;
                    UIEventListener.Get(sli.gameObject).onDragEnd = SliderControll;
                    if (sli.name == m_slider_OneScreen_ScreenNumberslider.name)
                    {
                        UILabel screenNumPercent = sli.transform.Find("Percent").GetComponent<UILabel>();
                        EventDelegate.Add(m_slider_OneScreen_ScreenNumberslider.onChange, () =>
                        {
                            if (screenNumPercent != null)
                            {
                                screenNumPercent.text = string.Format("{0}人", Mathf.FloorToInt(sli.value * (SettingManager.MAX_PLAYER - SettingManager.MIN_PLAYER)) + SettingManager.MIN_PLAYER);
                            }
                        });
                    }
                    else
                    {
                        EventDelegate.Add(sli.onChange, () =>
                        {
                            SelectSlider(sli);
                        });
                    }



                }
            }
        }
    }





    string SelectPopList(UIPopupList pop)
    {
        return pop.value;
    }

    void PopListControll(GameObject popObj, bool value = false)
    {
        UIPopupList pop = popObj.GetComponent<UIPopupList>();
        Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
        if (pop.value == "高")
        {
            rs.effectLevel = 0;
        }
        else if (pop.value == "中")
        {
            rs.effectLevel = 1;
        }
        else if (pop.value == "低")
        {
            rs.effectLevel = 2;
        }
        else
        {
            rs.effectLevel = 2;
        }
    }

    #region Toggle选中与否--Select函数集
    int SelectToggle(UIToggle tg)
    {
        return tg.value ? 1 : 0;
    }
    void ToggleControll(GameObject toggleObj)
    {
        UIToggle toggle = toggleObj.GetComponent<UIToggle>();
        bool value = toggle.value;
        #region 基础页
        //音乐
        if (toggleObj.name == "Sound_CheckSound")
        {
            Engine.IAudio au = Engine.RareEngine.Instance().GetAudio();
            au.Mute(value);
            if (value)
            {
                m_slider_Sound_soundslider.GetComponent<BoxCollider>().enabled = false;
                m_slider_Sound_SoundEffectsslider.GetComponent<BoxCollider>().enabled = false;
                m_slider_Sound_soundslider.transform.Find("Foreground").GetComponent<UISprite>().spriteName = "jindutiao_xiaohui";
                m_slider_Sound_SoundEffectsslider.transform.Find("Foreground").GetComponent<UISprite>().spriteName = "jindutiao_xiaohui";
            }
            else
            {
                m_slider_Sound_soundslider.GetComponent<BoxCollider>().enabled = true;
                m_slider_Sound_SoundEffectsslider.GetComponent<BoxCollider>().enabled = true;
                m_slider_Sound_soundslider.transform.Find("Foreground").GetComponent<UISprite>().spriteName = "jindutiao_jinhuang";
                m_slider_Sound_SoundEffectsslider.transform.Find("Foreground").GetComponent<UISprite>().spriteName = "jindutiao_jinhuang";
            }

        }
        //摇杆
        if (toggleObj.name == "Operate_SettingFixedRocker")
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.EJOYSTICKSTABLE, true);
        }
        if (toggleObj.name == "Operate_SettingMoveRocker")
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.EJOYSTICKSTABLE, false);
        }
        //自动拒绝组队
        if (toggleObj.name == "Social_SettingTeamInvite")
        {
            DataManager.Manager<TeamDataManager>().SetAutoRefuseTeamInvite(value);
        }
        //自动拒绝氏族
        if (toggleObj.name == "Social_SettingClanInvite")
        {
            DataManager.Manager<ClanManger>().AutoRefuseInvite = value;
        }
        //攻击目标-玩家-怪-无
        if (toggleObj.name == "AttackTarget_SettingPlayer")
        {
            DataManager.Manager<SettingManager>().AttackPriority = SettingManager.eAttackPriority.Player;
        }
        if (toggleObj.name == "AttackTarget_SettingMonster")
        {
            DataManager.Manager<SettingManager>().AttackPriority = SettingManager.eAttackPriority.Monster;
        }
        if (toggleObj.name == "AttackTarget_SettingNone")
        {
            DataManager.Manager<SettingManager>().AttackPriority = SettingManager.eAttackPriority.Creature;
        }
        //是否震屏
        if (toggleObj.name == "ShakeScreen_SettingShockScreen")
        {

        }
        //摄像机远近视角
        if (toggleObj.name == "Camera_close")
        {
            Client.EntityCreator.Instance().isFarDisCamera = false;
            Client.EntityCreator.Instance().SetCameraDistance();
        }
        if (toggleObj.name == "Camera_far")
        {
            Client.EntityCreator.Instance().isFarDisCamera = true;
            Client.EntityCreator.Instance().SetCameraDistance();
        }
        #endregion
        #region 画质页
        //玩家名称
        if (toggleObj.name == "ShowDetail_PlayerName")
        {
            RoleStateBarManager.SetPlayerNameVisible(value);
        }
        //玩家称号
        if (toggleObj.name == "ShowDetail_PlayerTitle")
        {
            DataManager.Manager<TitleManager>().SetIsShowTitle(value);
            RoleStateBarManager.SetTitleNameVisible(value);
        }
        //氏族名称
        if (toggleObj.name == "ShowDetail_ClanName")
        {
            RoleStateBarManager.SetClanNameVisible(value);
        }
        //怪物名称
        if (toggleObj.name == "ShowDetail_MonsterName")
        {
            RoleStateBarManager.SetNpcNameVisible(value);
        }
        //血条显示
        if (toggleObj.name == "ShowDetail_HpDisplay")
        {
            RoleStateBarManager.SetHpSliderVisible(value);
        }
        //经验显示
        if (toggleObj.name == "ShowDetail_ExpDisplay")
        {

        }
        //伤害显示
        if (toggleObj.name == "ShowDetail_HurtDisplay")
        {
            FlyFontDataManager.Instance.m_bShowFlyFont = value;
        }
        //自己特效
        if (toggleObj.name == "SpecialEffects_mineEffect")
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.HIDETEXIAO_MINE, value);
        }
        //其他玩家特效
        if (toggleObj.name == "SpecialEffects_otherEffect")
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.HIDETEXIAO_OTHER, value);
        }
        //屏蔽其他玩家
        if (toggleObj.name == "SpecialEffects_BlockPlayers")
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.HIDEOTHERPLAYER, value);
            //            EntitySystem.EntityHelper.ShowOtherPlayer(!value);
        }
        //屏蔽怪物
        if (toggleObj.name == "SpecialEffects_BlockMonster")
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.HIDEMONSTER, value);
            //            EntitySystem.EntityHelper.ShowMonster(!value);
        }
        //屏蔽时装
        if (toggleObj.name == "SpecialEffects_BlockFashion")
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.HIDEFASHION, value);
        }
        //草地扰动
        if (toggleObj.name == "SpecialEffects_Grassland")
        {
            IMapSystem mapSys = ClientGlobal.Instance().GetMapSystem();
            mapSys.EnableGrassForce(value);
        }
        //人物阴影显示
        if (toggleObj.name == "Render_shadow")
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs != null)
            {
                if (value)
                {
                    rs.SetShadowLevel(Engine.ShadowLevel.Height);
                }
                else
                {
                    rs.SetShadowLevel(Engine.ShadowLevel.None);
                }
            }
        }
        if (toggleObj.name == "SpecialEffects_skillShake")
        {
            CameraFollow.Instance.BEnableShake = value;
        }
        #endregion
        #region 挂机页
        if (toggleObj.name == "TeamSetting_IntoTeam")
        {
            DataManager.Manager<TeamDataManager>().SetLeaderAutoAgreeTeamApply(value);
        }
        if (toggleObj.name == "TeamSetting_Follow")
        {
            DataManager.Manager<TeamDataManager>().SetMemberAutoAllowTeamFollow(value);

        }
        if (toggleObj.name == "MedicalSetting_Hp")
        {
            m_slider_MedicalSetting_Hpvalue.GetComponent<BoxCollider>().enabled = value;
            m_slider_MedicalSetting_Hpvalue.transform.Find("Foreground").GetComponent<UISprite>().spriteName
                = (value) ? "jindutiao_jinhuang" : "jindutiao_xiaohui";
            SaveInOptionAtTime(m_Content);
        }
        if (toggleObj.name == "MedicalSetting_HpAtOnce")
        {
            m_slider_MedicalSetting_HpAtOncevalue.GetComponent<BoxCollider>().enabled = value;
            m_slider_MedicalSetting_HpAtOncevalue.transform.Find("Foreground").GetComponent<UISprite>().spriteName
                = (value) ? "jindutiao_jinhuang" : "jindutiao_xiaohui";
            SaveInOptionAtTime(m_Content);
        }
        if (toggleObj.name == "MedicalSetting_Mp")
        {
            m_slider_MedicalSetting_Mpvalue.GetComponent<BoxCollider>().enabled = value;
            m_slider_MedicalSetting_Mpvalue.transform.Find("Foreground").GetComponent<UISprite>().spriteName
                = (value) ? "jindutiao_jinhuang" : "jindutiao_xiaohui";
            SaveInOptionAtTime(m_Content);
        }
        if (toggleObj.name == "MedicalSetting_PetHp")
        {
            m_slider_MedicalSetting_PetHpvalue.GetComponent<BoxCollider>().enabled = value;
            m_slider_MedicalSetting_PetHpvalue.transform.Find("Foreground").GetComponent<UISprite>().spriteName
                = (value) ? "jindutiao_jinhuang" : "jindutiao_xiaohui";
            SaveInOptionAtTime(m_Content);
        }
        if (toggleObj.name == "MedicalSetting_AutoRepairEquip")
        {
            m_slider_MedicalSetting_Equipvalue.GetComponent<BoxCollider>().enabled = value;
            m_slider_MedicalSetting_Equipvalue.transform.Find("Foreground").GetComponent<UISprite>().spriteName
                = (value) ? "jindutiao_jinhuang" : "jindutiao_xiaohui";
        }
        if (toggleObj.name == "MedicalSetting_AutoReturn")
        {
            m_slider_MedicalSetting_HpLimtvalue.GetComponent<BoxCollider>().enabled = value;
            m_slider_MedicalSetting_HpLimtvalue.transform.Find("Foreground").GetComponent<UISprite>().spriteName
                = (value) ? "jindutiao_jinhuang" : "jindutiao_xiaohui";
        }
        if (toggleObj.name == "Pick_PickMedecal")
        {
            m_slider_Pick_EquipLevel.GetComponent<BoxCollider>().enabled = value;
            m_slider_Pick_EquipLevel.transform.Find("Foreground").GetComponent<UISprite>().spriteName
                = (value) ? "jindutiao_jinhuang" : "jindutiao_xiaohui";
        }
        #endregion
        #region 程序用
        if (toggleObj.name == "UI_ScreenPriority")
        {
            if (toggleObj.GetComponent<UIToggle>().value)
            {

            }
        }
        if (toggleObj.name == "UI_PerformancePriority")
        {
            if (toggleObj.GetComponent<UIToggle>().value)
            {

            }
        }
        if (toggleObj.name == "UI_SavingPriority")
        {
            if (toggleObj.GetComponent<UIToggle>().value)
            {

            }
        }
        if (toggleObj.name == "UI_rolepos")
        {
            EntitySystem.EntityConfig.m_bShowMainPlayerServerPosCube = value;
            NetService.Instance.Send(new stShowUserCurPosMoveUserCmd_C() { show_self_cube = value });
        }
        //头顶名称
        if (toggleObj.name == "UI_headname")
        {
            if (value)
            {
                RoleStateBarManager.ShowHeadStatus();
            }
            else
            {
                RoleStateBarManager.HideHeadStatus();
            }
        }
        //摇杆
        if (toggleObj.name == "UI_joystick")
        {
            PlayerPrefs.SetInt(UIMsgID.eJoystickStable.ToString(), value ? 1 : 0);
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eJoystickStable, value);
        }
        //天气
        if (toggleObj.name == "UI_Weather")
        {
            if (toggleObj.GetComponent<UIToggle>().value)
            {

            }
        }
        //水反射
        if (toggleObj.name == "UI_WaterReflection")
        {
            if (toggleObj.GetComponent<UIToggle>().value)
            {

            }
        }
        //草地
        if (toggleObj.name == "UI_grass")
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs != null)
            {
                Engine.IScene scene = rs.GetActiveScene();
                if (scene != null)
                {
                    scene.ShowGrass(toggleObj.GetComponent<UIToggle>().value);
                }
            }

        }
        //场景物件
        if (toggleObj.name == "UI_sod")
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs != null)
            {
                Engine.IScene scene = rs.GetActiveScene();
                if (scene != null)
                {
                    scene.ShowTerrainObj(toggleObj.GetComponent<UIToggle>().value);
                }
            }
        }
        //UI
        if (toggleObj.name == "UI_ui")
        {
            GameObject m_UIRoot = GameObject.Find("ui_root");
            if (m_UIRoot != null)
            {
                m_UIRoot.SetActive(toggleObj.GetComponent<UIToggle>().value);
            }
        }
        //震屏
        if (toggleObj.name == "UI_ShockScreen" || toggleObj.name == "ShakeScreen_SettingShockScreen")
        {
            if (toggleObj.GetComponent<UIToggle>().value)
            {

            }
        }
        //玩家同模
        if (toggleObj.name == "UI_SameMold")
        {
            EntitySystem.EntityConfig.m_bShowMainPlayerServerPosCube = toggleObj.GetComponent<UIToggle>().value;
        }
        //人物阴影显示
        if (toggleObj.name == "UI_shadow")
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs != null)
            {
                if (toggleObj.GetComponent<UIToggle>().value)
                {
                    rs.SetShadowLevel(Engine.ShadowLevel.Height);
                }
                else
                {
                    rs.SetShadowLevel(Engine.ShadowLevel.None);
                }
            }
        }
        //场景地表
        if (toggleObj.name == "UI_face")
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs != null)
            {
                Engine.IScene scene = rs.GetActiveScene();
                if (scene != null)
                {
                    scene.ShowTerrain(toggleObj.GetComponent<UIToggle>().value);
                }
            }
        }
        //滤镜
        if (toggleObj.name == "UI_Filter")
        {

        }
        //抗锯齿
        if (toggleObj.name == "UI_AntiAliasing")
        {

        }
        //世界坐标
        if (toggleObj.name == "UI_WorldCoordinates")
        {
            EntitySystem.EntityConfig.m_bShowWorldCoordinate = toggleObj.GetComponent<UIToggle>().value;
        }
        //地图阻挡显示
        if (toggleObj.name == "UI_mapArea")
        {
            ClientGlobal.Instance().GetMapSystem().SetMapAreaVisible(value);

        }
        if (toggleObj.name == "UI_map9grid")
        {
            ClientGlobal.Instance().GetMapSystem().SetMap9Grid(value);
            return;
        }
        //模型
        if (toggleObj.name == "UI_Model")
        {
            Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
            if (es != null)
            {
                es.ShowEntity(toggleObj.GetComponent<UIToggle>().value);
            }
        }
        //法线
        if (toggleObj.name == "UI_Normal")
        {

        }
        //草地扰动
        if (toggleObj.name == "UI_Grassland")
        {

        }
        //只创建主角
        if (toggleObj.name == "UI_OnMainPlayer")
        {
            PlayerPrefs.SetInt(UIMsgID.eOnlyCreatMainPlayer.ToString(), value ? 1 : 0);
        }
        //过滤地形
        if (toggleObj.name == "UI_IgnoreLand")
        {
            PlayerPrefs.SetInt(UIMsgID.eIgnoreLand.ToString(), value ? 1 : 0);
        }
        //过滤草
        if (toggleObj.name == "UI_IgnoreGrass")
        {
            PlayerPrefs.SetInt(UIMsgID.eIgnoreGrass.ToString(), value ? 1 : 0);
        }
        //过滤静态物体
        if (toggleObj.name == "UI_IgnoreStatic")
        {
            PlayerPrefs.SetInt(UIMsgID.eIgnoreStatic.ToString(), value ? 1 : 0);
        }
        //过滤场景特效
        if (toggleObj.name == "UI_IgnoreScene")
        {
            PlayerPrefs.SetInt(UIMsgID.eIgnoreScene.ToString(), value ? 1 : 0);
        }
        //过滤水
        if (toggleObj.name == "UI_IgnoreWater")
        {
            PlayerPrefs.SetInt(UIMsgID.eIgnoreWater.ToString(), value ? 1 : 0);
        }
        //预加载
        if (toggleObj.name == "UI_PreLoad")
        {
            PlayerPrefs.SetInt(UIMsgID.ePreLoad.ToString(), value ? 1 : 0);
        }
        //清空
        if (toggleObj.name == "UI_ClearPlayerPres")
        {
            PlayerPrefs.DeleteAll();
        }
        #endregion
        #region 反馈页
        if (toggleObj.name == "wenjuan")
        {
            feedBackType = 0;
        }
        if (toggleObj.name == "quexian")
        {
            feedBackType = 1;
        }
        if (toggleObj.name == "jianyi")
        {
            feedBackType = 2;
        }
        if (toggleObj.name == "chongzhi")
        {
            feedBackType = 3;
        }
        if (toggleObj.name == "qita")
        {
            feedBackType = 4;
        }
        #endregion
    }

    #endregion

    void ChangePriorityValues(uint type)
    {
        //Execute ↓
        DataManager.Manager<SettingManager>().SetSettingScreenPriority(type);
        //RefreshUI ↓
        SettingDataBase data = GameTableManager.Instance.GetTableItem<SettingDataBase>(type);
        if (data != null)
        {
            bool grassIsOpen = (data.GrassMoveGrade != 0) ? true : false;
            m_toggle_SpecialEffects_Grassland.GetComponent<UIToggle>().value = grassIsOpen;
            bool shadowIsOpen = (data.RealTime_Shadow != 0) ? true : false;
            m_toggle_Render_shadow.GetComponent<UIToggle>().value = shadowIsOpen;
            uint value = data.PlayerNum;
            if (value > SettingManager.MAX_PLAYER || value < SettingManager.MIN_PLAYER)
            {
                value = (uint)SettingManager.MIN_PLAYER;
            }
            m_slider_OneScreen_ScreenNumberslider.value = (value - SettingManager.MIN_PLAYER) * 1.0f / (SettingManager.MAX_PLAYER - SettingManager.MIN_PLAYER);
            UILabel screenNumPercent = m_slider_OneScreen_ScreenNumberslider.transform.Find("Percent").GetComponent<UILabel>();
            if (screenNumPercent != null)
            {
                screenNumPercent.text = string.Format("{0}人", value);
            }
            //特效等级
            uint ParticleGrade = data.ParticleGrade;
            //观察距离
            uint ViewDistanceGrade = data.ViewDistanceGrade;
            //地标细节
            uint GroundDetailGrade = data.GroundDetailGrade;
            //模型精度
            uint ModelPrecision = data.ModelPrecision;

            SetPriportyGrade((int)(2 - ParticleGrade), m_trans_ParticleGrade);
            SetPriportyGrade((int)ViewDistanceGrade, m_trans_ViewDistanceGrade);
            SetPriportyGrade((int)GroundDetailGrade, m_trans_GroundDetailGrade);
            SetPriportyGrade((int)ModelPrecision, m_trans_ModelPrecision);
        }
    }

    void SetPriportyGrade(int value, Transform parent)
    {
        int childCount = parent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child != null)
            {
                UIToggle tg = child.GetComponent<UIToggle>();
                if (tg != null)
                {
                    tg.value = i == value;
                }
            }
        }
    }

    #region  Slider数值变化——Caculate函数集
    int CaculateEquipLevel()
    {
        return Mathf.CeilToInt(m_slider_Pick_EquipLevel.value * 7);
    }
    int SelectSlider(UISlider sl)
    {
        int value = 0;
        if (sl.name == m_slider_PictureEffect_GriphicSlider.name)
        {
            uint type = (uint)Mathf.CeilToInt(sl.value * 3);
            float va = type / 3.0f;
            m_slider_PictureEffect_GriphicSlider.value = va;
            value = Mathf.FloorToInt(va * 3) + 1;
        }
        else
        {
            value = Mathf.FloorToInt(sl.value * 100);
        }
        return value;
    }
    void SliderControll(GameObject sliderObj)
    {
        UISlider slider = sliderObj.GetComponent<UISlider>();
        float value = slider.value;
        if (sliderObj.name == m_slider_PictureEffect_GriphicSlider.name)
        {
            uint type = (uint)Mathf.FloorToInt(value * 3) + 1;
            ChangePriorityValues(type);
        }
        if (sliderObj.name == m_slider_Pick_EquipLevel.name)
        {
            value = Mathf.CeilToInt(value * 7) / 7.0f;
            m_slider_Pick_EquipLevel.value = value;
        }
        //音乐
        if (sliderObj.name == "Sound_soundslider")
        {
            Engine.IAudio au = Engine.RareEngine.Instance().GetAudio();
            au.SetMusicVolume(value);
        }
        //音效
        if (sliderObj.name == "Sound_SoundEffectsslider")
        {
            Engine.IAudio au = Engine.RareEngine.Instance().GetAudio();
            au.SetEffectVolume(value);
        }
        //技能音效&&语音
        if (sliderObj.name == "Sound_Voiceslider")
        {

        }
        if (sliderObj.name == "OneScreen_ScreenNumberslider")
        {
            Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
            if (es != null)
            {
                es.MaxPlayer = Mathf.FloorToInt(value * (SettingManager.MAX_PLAYER - SettingManager.MIN_PLAYER)) + SettingManager.MIN_PLAYER;
            }

        }
        if (sliderObj.name == "MedicalSetting_Hpvalue")
        {

        }
        if (sliderObj.name == "MedicalSetting_HpAtOncevalue")
        {

        }
        if (sliderObj.name == "MedicalSetting_Mpvalue")
        {

        }
        if (sliderObj.name == "MedicalSetting_PetHpvalue")
        {

        }
        if (sliderObj.name == "MedicalSetting_Equipvalue")
        {


        }
        if (sliderObj.name == "MedicalSetting_HpLimtvalue")
        {

        }
        if (sliderObj.name == "Pick_EquipLevel")
        {

        }
    }
    void CancelClickSlider(UIToggle tg)
    {
        if (tg.name == "MedicalSetting_Hp")
        {
            m_slider_MedicalSetting_Hpvalue.GetComponent<BoxCollider>().enabled = false;
            m_slider_MedicalSetting_Hpvalue.transform.Find("Foreground").GetComponent<UISprite>().spriteName = "jindutiao_xiaohui";
        }
        else if (tg.name == "MedicalSetting_Mp")
        {
            m_slider_MedicalSetting_Mpvalue.GetComponent<BoxCollider>().enabled = false;
            m_slider_MedicalSetting_Mpvalue.transform.Find("Foreground").GetComponent<UISprite>().spriteName = "jindutiao_xiaohui";
        }
        else if (tg.name == "MedicalSetting_HpAtOnce")
        {
            m_slider_MedicalSetting_HpAtOncevalue.GetComponent<BoxCollider>().enabled = false;
            m_slider_MedicalSetting_HpAtOncevalue.transform.Find("Foreground").GetComponent<UISprite>().spriteName = "jindutiao_xiaohui";
        }
        else if (tg.name == "MedicalSetting_PetHp")
        {
            m_slider_MedicalSetting_PetHpvalue.GetComponent<BoxCollider>().enabled = false;
            m_slider_MedicalSetting_PetHpvalue.transform.Find("Foreground").GetComponent<UISprite>().spriteName = "jindutiao_xiaohui";
        }
        else if (tg.name == "MedicalSetting_AutoRepairEquip")
        {
            m_slider_MedicalSetting_Equipvalue.GetComponent<BoxCollider>().enabled = false;
            m_slider_MedicalSetting_Equipvalue.transform.Find("Foreground").GetComponent<UISprite>().spriteName = "jindutiao_xiaohui";
        }
        else if (tg.name == "MedicalSetting_AutoReturn")
        {
            m_slider_MedicalSetting_HpLimtvalue.GetComponent<BoxCollider>().enabled = false;
            m_slider_MedicalSetting_HpLimtvalue.transform.Find("Foreground").GetComponent<UISprite>().spriteName = "jindutiao_xiaohui";
        }
        else if (tg.name == "Pick_PickMedecal")
        {
            m_slider_Pick_EquipLevel.GetComponent<BoxCollider>().enabled = false;
            m_slider_Pick_EquipLevel.transform.Find("Foreground").GetComponent<UISprite>().spriteName = "jindutiao_xiaohui";
        }
    }
    #endregion


    /// <summary>
    /// 保存设置
    /// </summary>
    /// <param name="oldType"></param>
    void SaveInOptionAtTime(RightTagType oldType)
    {
        LeftObj = m_trans_left.Find(oldType + "Content").gameObject;
        if (m_AutoRecoverGrids != null)
        {
            for (int i = 0; i < m_AutoRecoverGrids.Length; i++)
            {
                m_AutoRecoverGrids[i].Save();
            }
        }
        Client.IGameOption option = Client.ClientGlobal.Instance().gameOption;
        if (option == null)
        {
            return;
        }
        //Toggles
        UIToggle[] toggles = LeftObj.GetComponentsInChildren<UIToggle>();
        if (toggles == null)
        {
            Engine.Utility.Log.Error("这里面没有Toggle", LeftObj.name);
        }
        else
        {
            //程序用的不存 非程序用的才会存入option.ini
            if (LeftObj.name != "programUseContent" && LeftObj.name != "feedbackContent")
            {
                foreach (var i in toggles)
                {
                    string[] wholeName = i.name.Split("_".ToCharArray());
                    if (wholeName.Length == 2)
                    {
                        option.WriteInt(wholeName[0], wholeName[1], i.value ? 1 : 0);
                    }

                }
            }
            if (LeftObj.name == "programUseContent")
            {
                PlayerPrefs.Save();
            }
        }
        //Sliders
        UISlider[] sliders = LeftObj.GetComponentsInChildren<UISlider>();
        if (sliders == null)
        {
            Engine.Utility.Log.Error("这里面没有Slider", LeftObj.name);
        }
        else
        {
            foreach (var j in sliders)
            {
                string[] name = j.name.Split("_".ToCharArray());
                if (name.Length == 2)
                {
                    if (j.name == m_slider_OneScreen_ScreenNumberslider.name)
                    {
                        // Mathf.FloorToInt(value * (SettingManager.MAX_PLAYER - SettingManager.MIN_PLAYER)) + SettingManager.MIN_PLAYER;
                        option.WriteInt(name[0], name[1], Mathf.FloorToInt(j.value * (SettingManager.MAX_PLAYER - SettingManager.MIN_PLAYER)) + SettingManager.MIN_PLAYER);
                    }
                    else if (j.name == "Pick_EquipLevel")
                    {
                        option.WriteInt(name[0], name[1], CaculateEquipLevel());
                        ResetAllBoxPickLevel();
                    }
                    else
                    {
                        option.WriteInt(name[0], name[1], SelectSlider(j));
                    }
                }


            }
        }
        option.Save();

        //保存完数据后，马上跟新吃药数据
        ClientGlobal.Instance().GetControllerSystem().GetControllerHelper().InitSetting();
    }
    void ChoseAndSelect(RightTagType type,bool needSave = true)
    {
        GameObject oldTab = m_trans_left.transform.Find(m_Content + "Content").gameObject;
        oldTab.SetActive(false);
        ListenValueChangeRegister(type);
        if (m_Content != type && needSave)
        {          
            //保存快捷设置到服务器
            if (m_Content == RightTagType.UseItem)
            {
                SaveShortcutItemSetting();//保存快捷设置到服务器
            }
            else 
            {
                SaveInOptionAtTime(m_Content);
            }

        }

       
        SetSettingContentType(type);
        GameObject g = m_trans_left.transform.Find(type + "Content").gameObject;
        g.SetActive(true);
        switch (type)
        {
            case RightTagType.Base:
                {
                    InitSettingPanel(type);

                    int job = MainPlayerHelper.GetMainPlayerJob();
                    SelectRoleDataBase roledata = table.SelectRoleDataBase.Where((GameCmd.enumProfession)job, (GameCmd.enmCharSex)1);
                    if (roledata != null && null != m__headicon)
                    {
                        UIManager.GetTextureAsyn(roledata.strprofessionIcon, ref m_playerCASD, () =>
                        {
                            if (m__headicon != null)
                            {
                                m__headicon.mainTexture = null;
                            }
                        }, m__headicon, false);
                    }
                }
                break;
            case RightTagType.quality:
                {
                    InitSettingPanel(type);
                }
                break;
            case RightTagType.fighting:
                {
                    InitSettingPanel(type);
                    m_AutoRecoverGrids = new AutoRecoverGrid[(int)AutoRecoverGrid.MedicalType.Max];
                    for (int i = 0; i < (int)RoleHMP.Max; i++)
                    {
                        GameObject go = m_trans_area_restore.Find(((RoleHMP)i).ToString()).gameObject;
                        AutoRecoverGrid grid = go.GetComponent<AutoRecoverGrid>();
                        if (grid == null)
                        {
                            grid = go.AddComponent<AutoRecoverGrid>();
                        }
                        GameObject targetTrans = grid.transform.Find(((RoleHMP)i) + "_icon").gameObject;
                        grid.Init((AutoRecoverGrid.MedicalType)i, targetTrans);
                        m_AutoRecoverGrids[i] = grid;
                        if (targetTrans.transform.childCount != 0)
                        {
                            go.transform.Find("yuandi").gameObject.SetActive(false);
                        }
                    }
                }
                break;
            case RightTagType.feedback:
                {
                    InitSettingPanel(type);
                }
                break;
            case RightTagType.programUse:
                {
                    InitSettingPanel(type);
                }
                break;
            case RightTagType.UseItem:
                {
                    InitSettingPanel(type);
                    InitShortcut();
                }
                break;
            default:
                break;
        }

    }

    void ResetAllBoxPickLevel()
    {
        Client.IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }

        List<Client.IBox> lstEntity = new List<Client.IBox>();
        es.FindAllEntity<Client.IBox>(ref lstEntity);
        if (lstEntity != null)
        {
            for (int i = 0; i < lstEntity.Count; i++)
            {
                lstEntity[i].AddTrigger(new BoxOnTrigger());
            }
        }
        lstEntity.Clear();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eReplaceMedical)
        {
            MedicalReplace.MedicalReplaceInfo info = (MedicalReplace.MedicalReplaceInfo)param;
            if (info != null)
            {
                if (m_AutoRecoverGrids != null)
                {
                    for (int i = 0; i < m_AutoRecoverGrids.Length; i++)
                    {
                        GameObject targetTrans = m_AutoRecoverGrids[i].transform.Find(((RoleHMP)i) + "_icon").gameObject;
                        if (m_AutoRecoverGrids[i].GetMedicalType() == info.type)
                        {
                            m_AutoRecoverGrids[i].UpdateItem(info.itemid, targetTrans);
                        }

                    }
                }

            }
        }
        else if (msgid == UIMsgID.eShowUI)
        {
            if (param is ReturnBackUIMsg)
            {
                ReturnBackUIMsg showInfo = (ReturnBackUIMsg)param;
                if (showInfo != null)
                {
                    if (showInfo.tabs.Length > 0)
                    {
                        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, showInfo.tabs[0]);
                    }
                }
            }
            else if (param is int)
            {
                UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, (int)param);
            }



        }
        return base.OnMsg(msgid, param);
    }
    public override UIPanelBase.PanelData GetPanelData()
    {
        UIPanelBase.PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Tabs = new int[2];
        pd.JumpData.Tabs[0] = (int)m_Content;
        return pd;
    }


    #region NGUI Btns
    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }
    void onClick_Btn_goLogin_Btn(GameObject caster)
    {
        DataManager.Manager<LoginDataManager>().ChangeAcount();

        //处理任务
        DataManager.Manager<TaskDataManager>().FirstLoginSuccess = false;
    }
    void onClick_Setting_RefusedApply_Btn(GameObject caster)
    {

    }

    void onClick_Btn_goChooseRole_Btn(GameObject caster)
    {
        GameMain.Instance.GotoReSelecetRole();

        //处理任务
        DataManager.Manager<TaskDataManager>().FirstLoginSuccess = false;
    }

    void onClick_Btn_OutStuck_Btn(GameObject caster)
    {

    }

    void onClick_Btn_LockScreen_Btn(GameObject caster)
    {
        TipsManager.Instance.ShowTips("功能暂未开放");
    }

    void onClick_Btn_RecordVideo_Btn(GameObject caster)
    {
        TipsManager.Instance.ShowTips("功能暂未开放");
    }
    void onClick_Btn_Announcement_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.LoginNoticePanel);
    }
    void onClick_Btn_Bible_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ChinaBiblePanel);
    }
    void onClick_Btn_goforum_Btn(GameObject caster)
    {
        TipsManager.Instance.ShowTips("功能暂未开放");
    }

    void onClick_Btn_gohomepage_Btn(GameObject caster)
    {
        TipsManager.Instance.ShowTips("功能暂未开放");
    }

    void onClick_Btn_gohotline_Btn(GameObject caster)
    {
        TipsManager.Instance.ShowTips("功能暂未开放");
    }

    void onClick_Btn_gopayback_Btn(GameObject caster)
    {
        OnTogglePanel(1, (int)RightTagType.feedback);
    }

    #endregion


    #region   显示吃药面板Medicinesettingpanel
    public enum MedicalType
    {
        Hp = 0,
        Mp = 1,
        HpAtOnce = 2,
        PetHp = 3,
        Max = 4,
    }
    //    MedicalType mtype;
    void onClick_RoleBaseHp_icon_Btn(GameObject caster)
    {
        //MedicalTypeClick(MedicalType.Hp);
    }

    void onClick_RoleHighHp_icon_Btn(GameObject caster)
    {
        MedicalTypeClick(MedicalType.HpAtOnce);
    }

    void onClick_RoleBaseMp_icon_Btn(GameObject caster)
    {
        //MedicalTypeClick(MedicalType.Mp);
    }

    void onClick_PetBaseHp_icon_Btn(GameObject caster)
    {
        MedicalTypeClick(MedicalType.PetHp);
    }
    void onClick_PetHpLabel_Btn(GameObject caster)
    {
        MedicalTypeClick(MedicalType.PetHp);
    }
    void onClick_MpLabel_Btn(GameObject caster)
    {
        MedicalTypeClick(MedicalType.Mp);
    }
    void onClick_HpAtOnceLabel_Btn(GameObject caster)
    {
        MedicalTypeClick(MedicalType.HpAtOnce);
    }
    void onClick_HpLabel_Btn(GameObject caster)
    {
        MedicalTypeClick(MedicalType.Hp);
    }
    void MedicalTypeClick(MedicalType type)
    {
        ReturnBackUIData[] returnUI = new ReturnBackUIData[1] { new ReturnBackUIData() };
        returnUI[0].panelid = this.PanelId;
        returnUI[0].msgid = UIMsgID.eShowUI;
        returnUI[0].param = (int)this.m_Content;
        MedicineSettingPanel.MedicineSettingParam data = new MedicineSettingPanel.MedicineSettingParam();
        data.tab = (int)this.m_Content;
        data.type = type;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MedicineSettingPanel, data: data);

    }
    #endregion

    #region 反馈页签
    void onClick_ConfirmBtn_Btn(GameObject caster)
    {
        UIInput accInput = new UIInput();
        UIInput phoInput = new UIInput();
        UIInput textInput = new UIInput();
        string account = "";
        string phone = "";
        string text = "";
        try
        {
            accInput = m_input_Account.GetComponent<UIInput>();
            account = accInput.value;
            phoInput = m_input_Phone.GetComponent<UIInput>();
            phone = phoInput.value;
            textInput = m_input_feedbackText.GetComponent<UIInput>();
            text = textInput.value;
        }
        catch (Exception e)
        {
            Engine.Utility.Log.Error(e + "输入框丢失组件");
        }

        if (TextManager.GetCharNumByStrInUnicode(text) <= 4)
        {
            TipsManager.Instance.ShowTips("为了更好的为您服务，请输入4个以上字符");
            return;
        }
        stFeedbackGMPropertyUserCmd_CS cmd = new stFeedbackGMPropertyUserCmd_CS();
        List<FeedbackData> fb = new List<FeedbackData>();
        FeedbackData fc = new FeedbackData()
        {
            phonenum = phone,
            content = text,
        };
        fb.Add(fc);
        if (cmd.data.Count > 0)
        {
            cmd.data.Clear();
        }
        cmd.data.AddRange(fb);
        //         cmd.data = new List<FeedbackData>()
        //         {
        //             phonenum = phoneNum,
        //             content = text ,
        //             /*
        //              gameid = ,
        //              zoneid = ,
        //              charid = ,
        //              charname = ,
        //              userlevel = ,
        //              viplevel = ,
        //              feedbackid = ,
        //              subject = ,
        //              star = ,
        //              recordtime = ,
        //              recordid = ,
        //              state= , 
        //              reply = ,
        //              platid = ,                               
        //              */
        //         };
        cmd.feedbacktype = feedBackType;
        //         cmd.curpage = ;
        //         cmd.maxpage = ;
        //         cmd.perpage = ;
        //         cmd.gmid = ;
        //         cmd.clientid = ;
        NetService.Instance.Send(cmd);
        //        phoInput.value = null;
        textInput.value = null;
        TipsManager.Instance.ShowTips(DataManager.Manager<TextManager>().GetLocalText(LocalTextType.FeedBack_Tips));
    }

    void onClick_NoteBtn_Btn(GameObject caster)
    {
        m_trans_chat.gameObject.SetActive(false);
        m_trans_note.gameObject.SetActive(true);
        m_btn_chatBtn.GetComponent<UISprite>().enabled = true;
        m_btn_noteBtn.GetComponent<UISprite>().enabled = false;
    }

    void onClick_ChatBtn_Btn(GameObject caster)
    {
        //服务器要求这么发   
        //1.循环五次，把所有反馈类型轮一遍  
        long beforeTime = 0;
        long nowTime = 0;
        nowTime = DateTimeHelper.Instance.Now;
        // 2。默认写个***** 三天前 *****的时间戳和当前时刻的时间戳，是否处理（0所有1未处理2已处理3忽略）259200 = 3*24*60*60
        beforeTime = DateTimeHelper.Instance.Now - 259200;
        stReqFeedbackListPropertyUserCmd_C cmd = new stReqFeedbackListPropertyUserCmd_C();
        cmd.feedbacktype = 101;
        cmd.state = 0;
        cmd.starttime = (ulong)beforeTime;
        cmd.endtime = (ulong)nowTime;
        NetService.Instance.Send(cmd);



        m_trans_chat.gameObject.SetActive(true);
        m_trans_note.gameObject.SetActive(false);
        m_btn_chatBtn.GetComponent<UISprite>().enabled = false;
        m_btn_noteBtn.GetComponent<UISprite>().enabled = true;

        isRecieveFeedBackWarning = false;
        AwakeSettingFeedBackWarning();

        CacheChat();

    }

    private bool isNumber(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return false;
        }
        foreach (char c in text)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
        }
        return true;
    }


    void AwakeSettingFeedBackWarning()
    {
        UITabGrid tabGrid = null;
        m_sprite_feedBackWarning.gameObject.SetActive(isRecieveFeedBackWarning);
        Dictionary<int, UITabGrid> dicTabs = null;
        if (dicUITabGrid.TryGetValue(1, out dicTabs))
        {
            if (dicTabs != null && dicTabs.TryGetValue((int)RightTagType.feedback, out tabGrid))
            {
                tabGrid.SetRedPointStatus(isRecieveFeedBackWarning);
            }
        }
    }
    #endregion
}
