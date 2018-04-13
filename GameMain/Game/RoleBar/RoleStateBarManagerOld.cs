//*************************************************************************
//	创建日期:	2017-3-30 9:53
//	文件名称:	RoleStateBarManager.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	头顶状态条管理
//*************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using Engine.Utility;
using table;

public enum HeadTipType
{
    None = 0,
    Hp,             //血量
    Name,           //姓名
    Clan,           //氏族
    Title,          //称号
    Collect,        //采集
    HeadMaskIcon,   //头顶图标标识
    TaskStatus,
    Max
}
public enum HpSpriteType 
{
    None = 0,
    NoneBg,
    Me   ,
    Friend,
    Enemy,
    PlayerBg,
    Monster,
    MonsterBg,
    MyPet,
    EnemyPet,
    PetBg,
    MySummon,
    EnemySummon,
    SummonBg,
}


public class RoleStateBarManagerOld : MonoBehaviour,IGlobalEvent
{
    #region
    static RoleStateBarManagerOld ms_instance = null;

    List<RoleStateBar> m_lstAllStateBar = new List<RoleStateBar>();
    List<Transform> m_lstUnUseStateBarTrans = new List<Transform>();
    //头顶标识
    private Dictionary<long, RoleStateBar> m_dicActiveBar = new Dictionary<long, RoleStateBar>();

    Camera m_maincamera = null;
    Camera m_uicamera = null;
    UIWidget m_widgetRoot;
    bool m_visible = true;
    public bool disableIfInvisible = true;
    const int MAXBAR_NUM = 60;

    private bool m_bPlayerNameVisible = true;
    private bool m_bPlayerClanNameVisible = true;
    private bool m_bPlayerTitleVisible = true;
    private bool m_bNpcNameVisible = true;
    private bool m_bHpSliderVisible = true;

    private bool m_bInit = false;
    #endregion

    #region IGlobalEvent
    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            // 注册事件
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_HPUPDATE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CHANGENAME, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_SETHIDE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_NPCHEADSTATUSCHANGED, GlobalEventHandler);

            EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSSTART, GlobalEventHandler);
            EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSBREAK, GlobalEventHandler);
            EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSEND, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANQUIT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANJOIN, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANREFRESHID, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANDeclareInfoRemove, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANDeclareInfoAdd, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TITLE_WEAR, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CITYWARTOTEMCLANNAMECHANGE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CITYWARWINERCLANID, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_HPUPDATE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_CHANGENAME, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_SETHIDE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_NPCHEADSTATUSCHANGED, GlobalEventHandler);

            EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSSTART, GlobalEventHandler);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSBREAK, GlobalEventHandler);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSEND, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANQUIT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANJOIN, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANREFRESHID, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANDeclareInfoRemove, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANDeclareInfoAdd, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TITLE_WEAR, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CITYWARTOTEMCLANNAMECHANGE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CITYWARWINERCLANID, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);
        }
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="nEventID"></param>
    /// <param name="param"></param>
    public void GlobalEventHandler(int eventID, object param)
    {
        if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_NPCHEADSTATUSCHANGED)
        {
            if (null != param && param is Client.INPC)
                UpdateNpcHeadMask((Client.INPC)param);
        }
        else if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY)
        {
            Client.stCreateEntity ce = (Client.stCreateEntity)param;
            OnCretateEntity(ref ce);
        }
        else if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY)
        {
            Client.stRemoveEntity removeEntiy = (Client.stRemoveEntity)param;
            RemoveBar(removeEntiy);
        }
        else if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE)
        {
            stPropUpdate prop = (stPropUpdate)param;
            OnPropUpdate(ref prop);
        }
        else if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_HPUPDATE)
        {
            stPropUpdate prop = (stPropUpdate)param;
            OnPropUpdate(ref prop);
        }
        else if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_CHANGENAME)
        {
            stEntityChangename e = (stEntityChangename)param;
            OnChangeName(e.uid);
        }
        else if (eventID == (int)(int)Client.GameEventID.ENTITYSYSTEM_SETHIDE)
        {
            stEntityHide st = (stEntityHide)param;
            OnSetEntityHide(ref st);
            return;

        }
        else if (eventID == (int)(int)Client.GameEventID.TITLE_WEAR)//佩戴称号
        {
            Client.stTitleWear data = (Client.stTitleWear)param;
            OnTitleWear(data.uid);
        }
        else if (eventID == (int)GameEventID.SKILLGUIDE_PROGRESSSTART)
        {
            Client.stUninterruptMagic uninterrupt = (Client.stUninterruptMagic)param;
            OnStartCollectSlider(ref uninterrupt);
        }
        else if (eventID == (int)GameEventID.SKILLGUIDE_PROGRESSBREAK)
        {
            if (param != null)
            {
                stGuildBreak guildbreak = (stGuildBreak)param;
                if (ShowCollectTip(guildbreak.action))
                {
                    long uid = EntitySystem.EntityHelper.MakeUID(EntityType.EntityType_Player, guildbreak.uid);
                    RoleStateBar bar = GetRoleBarByUID(uid);
                    if (bar != null)
                    {
                        bar.SetWidgetState(HeadTipType.Collect, false);
                    }
                }
            }
        }
        else if (eventID == (int)GameEventID.SKILLGUIDE_PROGRESSEND)
        {
            if (param != null)
            {
                stGuildEnd guildEnd = (stGuildEnd)param;
                if (ShowCollectTip(guildEnd.action))
                {
                    long uid = EntitySystem.EntityHelper.MakeUID(EntityType.EntityType_Player, guildEnd.uid);
                    RoleStateBar bar = GetRoleBarByUID(uid);
                    if (bar != null)
                    {
                        bar.SetWidgetState(HeadTipType.Collect, false);
                    }
                }
            }

        }
        else if (eventID == (int)Client.GameEventID.CLANQUIT || eventID == (int)Client.GameEventID.CLANJOIN ||
            eventID == (int)Client.GameEventID.CLANREFRESHID || eventID == (int)Client.GameEventID.CITYWARWINERCLANID ||
            eventID == (int)Client.GameEventID.CLANDeclareInfoAdd || eventID == (int)Client.GameEventID.CLANDeclareInfoRemove)
        {
            OnRefreshAllClanTitile();
        }
        else if (eventID == (int)Client.GameEventID.CITYWARTOTEMCLANNAMECHANGE)
        {
            long uid = EntitySystem.EntityHelper.MakeUID(EntityType.EntityType_NPC, (uint)param);
            RoleStateBar bar = GetRoleBarByUID(uid);
            if (bar != null)
            {
                Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
                if (es == null)
                {
                    return;
                }
                RefreshClanName(es.FindEntity(uid), bar);
            }
        }
        else if (eventID == (int)Client.GameEventID.SYSTEM_GAME_READY)
        {
            OnRefreshAllHpSlider();
        }
    }
    #endregion
    public static RoleStateBarManagerOld Instance()
    {
        if (ms_instance == null)
        {
            GameObject go = new GameObject("HudRoot");
            ms_instance = go.AddComponent<RoleStateBarManagerOld>();

            go.transform.parent = DataManager.Manager<UIPanelManager>().StretchTransRoot;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
        }
        return ms_instance;
    }

    public void Init()
    {
        if (m_widgetRoot == null)
            m_widgetRoot = gameObject.AddComponent<UIWidget>(); // 方便设置隐藏 显示

        if (m_maincamera == null)
            m_maincamera = Util.MainCameraObj.GetComponent<Camera>();
        if (m_uicamera == null)
            m_uicamera = Util.UICameraObj.GetComponent<Camera>();

        m_bInit = true;
    }

    void Awake()
    {
        RegisterGlobalEvent(true);
    }


    /// <summary>
    /// 隐藏所有
    /// </summary>
    public void Hide()
    {
        StartCoroutine(DelayHide());
    }

    IEnumerator DelayHide()
    {
        while (!m_bInit)
        {
            yield return new WaitForEndOfFrame();
        }
        m_widgetRoot.alpha = 0f;
        m_visible = false;
    }
    /// <summary>
    /// 显示所有
    /// </summary>
    public void Show()
    {
        m_visible = true;
        StartCoroutine(DelayShow());//等位置变化完成在设置可见
    }

    IEnumerator DelayShow()
    {
        while (!m_bInit)
        {
            yield return new WaitForEndOfFrame();
        }
        m_widgetRoot.alpha = 1f;
        yield return null;
    }


    IEntity GetEntity(long uid)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es != null)
        {
            return es.FindEntity(uid);
        }
        return null;
    }

    RoleStateBar GetRoleBarByUID(long uid)
    {
        for (int i = 0, imax = m_lstAllStateBar.Count; i < imax; i++)
        {
            if (m_lstAllStateBar[i].entity.GetUID() == uid)
            {
                return m_lstAllStateBar[i];
            }
        }
        return null;
    }
    /// <summary>
    /// 根据uid获取
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    private RoleStateBar GetRoleBarByUId(long uid)
    {
        if (null != m_dicActiveBar && m_dicActiveBar.ContainsKey(uid))
        {
            return m_dicActiveBar[uid];
        }
        return null;
    }

    bool Contain(long uid)
    {
        return GetRoleBarByUID(uid) != null;
    }
    /// <summary>
    /// 清空所有血条
    /// </summary>
    public void Clear()
    {
        if (m_lstAllStateBar != null)
        {
            foreach (var item in m_lstAllStateBar)
            {
                if (item != null)
                {
                    item.Destroy();
                }
            }
            m_lstAllStateBar.Clear();
        }
        if (m_lstUnUseStateBarTrans != null)
        {
            foreach (var item in m_lstUnUseStateBarTrans)
            {
                if (item != null)
                {
                    GameObject.Destroy(item.gameObject);
                }
            }
            m_lstUnUseStateBarTrans.Clear();
        }

    }

    void SetBarVisible(long uid, bool bShow,float alp = 1.0f)
    {
        RoleStateBar bar = GetRoleBarByUID(uid);
        if (bar != null)
        {
            if (bShow)
            {
                bar.widget.alpha = alp;
            }
            else 
            {
                bar.widget.alpha = 0f;
            }
        }
    }

    //-----------------------------------------------------------------------------------------------
    private void OnStartCollectSlider(ref Client.stUninterruptMagic uninterrupt)
    {
        if (ShowCollectTip(uninterrupt.type))
        {
            Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();

            if (es == null)
            {
                return;
            }

            Client.IEntity entity = es.FindEntity(uninterrupt.uid);
            if (entity != null && Client.ClientGlobal.Instance().IsMainPlayer(entity.GetUID()) == false)
            {
                RoleStateBar bar = GetRoleBarByUID(uninterrupt.uid);
                if (bar != null)
                {
                    HeadTipData data = new HeadTipData(entity, HeadTipType.Collect, true);
                    data.SetCollectType(uninterrupt.type);
                    bar.UpdateWidget(data);
                }
            }
        }
    }
    private void OnTitleWear(uint uid)
    {
        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();

        if (es == null)
        {
            return;
        }

        Client.IPlayer entity = es.FindPlayer(uid);
        if (entity != null)
        {
            RoleStateBar bar = GetRoleBarByUID(entity.GetUID());
            if (bar != null)
            {
                bar.UpdateWidget(new HeadTipData(entity, HeadTipType.Title, m_bPlayerTitleVisible));
            }
        }
    }
    private void OnSetEntityHide(ref stEntityHide st)
    {
        IEntity entity = GetEntity(st.uid);
        if (entity == null)
        {
            Engine.Utility.Log.Error("找不到对象------------" + st.uid);
            return;
        }
        IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            return;
        }
        if (st.bHide)
        {
            IEntity currtarget = cs.GetActiveCtrl().GetCurTarget();
            if (currtarget != null)
            {
                if (currtarget.GetUID() == entity.GetUID())
                {
                    Client.stTargetChange targetChange = new Client.stTargetChange();
                    targetChange.target = null;
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_TARGETCHANGE, targetChange);
                }
            }

        }

        IPlayer mainPalyer = MainPlayerHelper.GetMainPlayer();
        if (entity.GetEntityType() == EntityType.EntityType_Player && mainPalyer != null)
        {
            if (mainPalyer.GetUID() != st.uid)
            {
                IControllerHelper ch = cs.GetControllerHelper();
                if (ch == null)
                {
                    return;
                }
                if (st.bHide)
                {
                    if (ch.IsSameTeam(entity))
                    {
                        SetBarVisible(entity.GetUID(), true, 0.3f);
                        entity.SendMessage(EntityMessage.EntityCommond_SetAlpha, 0.3f);
                    }
                    else
                    {
                        SetBarVisible(entity.GetUID(), false);
                        entity.SendMessage(EntityMessage.EntityCommand_SetVisible, false);
                    }
                }
                else
                {
                    SetBarVisible(entity.GetUID(), true);
                    entity.SendMessage(EntityMessage.EntityCommond_SetAlpha, 1.0f);
                    entity.SendMessage(EntityMessage.EntityCommand_SetVisible, true);
                }

            }
            else
            {
                if (st.bHide)
                {
                    SetBarVisible(entity.GetUID(), true,0.3f);
                    entity.SendMessage(EntityMessage.EntityCommond_SetAlpha, 0.3f);
                }
                else
                {
                    SetBarVisible(entity.GetUID(), true);
                    entity.SendMessage(EntityMessage.EntityCommond_SetAlpha, 1.0f);
                    entity.SendMessage(EntityMessage.EntityCommand_SetVisible, true);
                }
            }
        }
        else
        {
            if (st.bHide)
            {
                SetBarVisible(entity.GetUID(), false);
                entity.SendMessage(EntityMessage.EntityCommand_SetVisible, false);
            }
            else
            {
                SetBarVisible(entity.GetUID(), true);
                entity.SendMessage(EntityMessage.EntityCommond_SetAlpha, 1.0f);
                entity.SendMessage(EntityMessage.EntityCommand_SetVisible, true);
            }
        }
    }
    private void OnPropUpdate(ref stPropUpdate prop)
    {
        Client.IEntity entity = ClientGlobal.Instance().GetEntitySystem().FindEntity(prop.uid);
        if (prop.nPropIndex == (int)CreatureProp.Hp || prop.nPropIndex == (int)CreatureProp.MaxHp)
        {
            if (entity != null)
            {
                RoleStateBar bar = GetRoleBarByUID(prop.uid);
                if (bar != null)
                {
                    HeadTipData data = new HeadTipData(entity, HeadTipType.Hp, m_bHpSliderVisible);
                    bar.UpdateWidget(data);
                }
            }
        }
        else if (prop.nPropIndex == (int)PlayerProp.GoodNess || prop.nPropIndex == (int)CreatureProp.Camp)
        {
            OnChangeName(prop.uid);
        }
    }

    void OnChangeName(long uid)
    {
        Client.IEntity entity = ClientGlobal.Instance().GetEntitySystem().FindEntity(uid);
        if (entity != null)
        {
            RoleStateBar bar = GetRoleBarByUID(uid);
            if (bar != null)
            {
                HeadTipData data = new HeadTipData(entity, HeadTipType.Name, GetNameVisible(entity));
                bar.UpdateWidget(data);
            }
        }
    }
    private void OnClanNameRefresh(uint clanID, uint uid)
    {
        RoleStateBar bar = GetRoleBarByUID(uid);
        if (bar != null)
        {
            if (clanID != 0)
            {
                Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();

                if (es == null)
                {
                    return;
                }
            }
            else
            {
                bar.SetWidgetState(HeadTipType.Clan, false);
            }
        }
    }

    private void OnCretateEntity(ref Client.stCreateEntity ce)
    {
        IEntity entity = GetEntity(ce.uid);
        if (entity == null)
        {
            Engine.Utility.Log.Error("找不到对象------------{0}", ce.uid);
            return;
        }

        if (entity.GetEntityType() == EntityType.EntityType_Pet)
        {
            return;
        }

        if (entity.GetEntityType() == EntityType.EntityType_NPC)
        {
            table.NpcDataBase npctable = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)entity.GetProp((int)Client.EntityProp.BaseID));
            if (npctable != null)
            {
                if (npctable.dwType == (uint)GameCmd.enumNpcType.NPC_TYPE_TRAP)
                {
                    return;
                }
            }
        }

        if (!Contain(entity.GetUID()))
        {
            AddBar(entity);
        }
    }

    private RoleStateBar CreateHeadText(IEntity entity, List<HeadTipData> lstdata)
    {
        RoleStateBar hudtext = new RoleStateBar(GetHudText(), entity, lstdata);
        if (hudtext != null)
        {
            if (Application.isEditor)
            {
                if (entity.GetEntityType() == EntityType.EntityType_Player)
                {
                    hudtext.mtrans.name = entity.GetName() + entity.GetID().ToString();
                }
                else
                {
                    hudtext.mtrans.name = entity.GetName() + "_" + entity.GetID().ToString();
                }
                hudtext.name = hudtext.mtrans.name;
            }

                m_lstAllStateBar.Add(hudtext);
           
            return hudtext;
        }
        return null;
    }

    private void OnRefreshAllClanTitile()
    {
        for (int i = 0, imax = m_lstAllStateBar.Count; i < imax; i++)
        {
            RoleStateBar bar = m_lstAllStateBar[i];
            if (bar == null)
            {
                continue;
            }

            if (bar.entity.GetEntityType() != EntityType.EntityType_Player)
            {
                continue;
            }
            RefreshClanName(bar.entity, bar);
        }
    }
    public void OnRefreshAllHpSlider()
    {
        for (int i = 0, imax = m_lstAllStateBar.Count; i < imax; i++)
        {
            RoleStateBar bar = m_lstAllStateBar[i];
            if (bar == null)
            {
                continue;
            }
            RefreshHpSlider(bar.entity, bar);
        }
    }
    void RefreshHpSlider(Client.IEntity entity, RoleStateBar bar) 
    {
        if (entity == null)
        {
            Engine.Utility.Log.Error("RefreshHpSlider entity is null");
            return;
        }
        if (bar == null)
        {
            Engine.Utility.Log.Error("RefreshHpSlider bar is null");
            return;
        }
        bool addHpSlider = IsNeedHpSlider(entity);
        HeadTipData data = null;
        if (addHpSlider)
        {
            bool visible = true;
            IPlayer player = ClientGlobal.Instance().MainPlayer;
            if (player == null)
            {
                return;
            }
            if (entity.GetUID() == player.GetUID())
            {
                visible = true;
            }
            else
            {
                visible = m_bHpSliderVisible;
            }
            data = new HeadTipData(entity, HeadTipType.Hp, visible);
        }
        else
        {
            data = new HeadTipData(entity, HeadTipType.Hp, addHpSlider);
        }
        if (data != null)
        {
            bar.UpdateWidget(data);
        }
         
    }
    void RefreshClanName(Client.IEntity entity, RoleStateBar bar)
    {
        if (entity == null)
        {
            Engine.Utility.Log.Error("AddClanName entity is null");
            return;
        }
        if (bar == null)
        {
            Engine.Utility.Log.Error("AddClanName bar is null");
            return;
        }

        EntityType entityType = entity.GetEntityType();
        if (entityType == EntityType.EntityType_Player)
        {
            uint clanIdLow = (uint)entity.GetProp((int)CreatureProp.ClanIdLow);
            uint clanIdHigh = (uint)entity.GetProp((int)CreatureProp.ClanIdHigh);
            uint clanId = (clanIdHigh << 16) | clanIdLow;

            //int clanId = entity.GetProp((int)Client.CreatureProp.ClanId);
            if (clanId != 0)
            {
                DataManager.Manager<ClanManger>().GetClanName((uint)clanId, (namedata) =>
                {
                    string winerCityName = string.Empty;
                    string name = string.Empty;
                    if (DataManager.Manager<CityWarManager>().GetWinerOfCityWarCityName((uint)clanId, out winerCityName))
                    {
                        //name = namedata.ClanName + winerCityName;
                        name = string.Format("{0}【{1}】", winerCityName, namedata.ClanName);
                    }
                    else
                    {
                        //name = namedata.ClanName;
                        name = string.Format("【{0}】", namedata.ClanName);
                    }

                    bool visible = m_bPlayerClanNameVisible;
                    if (entity.GetUID() == Client.ClientGlobal.Instance().MainPlayer.GetUID())
                    {
                        visible = true;
                    }
                    HeadTipData data = new HeadTipData(entity, HeadTipType.Clan, visible);
                    data.value = name;
                    bar.UpdateWidget(data);
                });
            }
            else
            {
                bar.SetWidgetState(HeadTipType.Clan, false);
            }
        }
        else if (entity.GetEntityType() == EntityType.EntityType_NPC)
        {
            CityWarTotem cityWarTotemInfo = null;
            if (DataManager.Manager<CityWarManager>().GetCityWarTotemClanName((uint)entity.GetProp((int)Client.EntityProp.BaseID), out cityWarTotemInfo))
            {
                HeadTipData data = new HeadTipData(entity, HeadTipType.Clan, m_bPlayerClanNameVisible);
                data.value = cityWarTotemInfo.clanName;
                bar.UpdateWidget(data);
            }
            else
            {
                bar.SetWidgetState(HeadTipType.Clan, false);
            }
        }
    }

    bool ShowCollectTip(GameCmd.UninterruptActionType action)
    {
        //if (!DataManager.Manager<CampCombatManager>().isEnterScene && !DataManager.Manager<CityWarManager>().EnterCityWar)
        //{
        //    return false;
        //}
        return action != GameCmd.UninterruptActionType.UninterruptActionType_SkillCJ && action != GameCmd.UninterruptActionType.UninterruptActionType_DEMON;
    }

   

    private void RemoveBar(Client.stRemoveEntity removeEntiy)
    {
        RoleStateBar bar = GetRoleBarByUID(removeEntiy.uid);
        if (bar != null)
        {
            if (m_lstUnUseStateBarTrans.Count < MAXBAR_NUM)
            {
                m_lstUnUseStateBarTrans.Add(bar.mtrans);
                bar.mtrans.name = "UnUse_" + m_lstUnUseStateBarTrans.Count;
                bar.obj.SetActive(false);
            }
            else
            {
                GameObject.Destroy(bar.mtrans.gameObject);
            }
            m_lstAllStateBar.Remove(bar);
        }
    }

    bool GetNameVisible(IEntity entity)
    {
        if (entity == null)
        {
            return false;
        }
        bool visible = true;
        EntityType entityType = entity.GetEntityType();
        if (entityType == EntityType.EntityType_NPC)
        {
            visible = m_bNpcNameVisible;
        }
        else if (entityType == EntityType.EntityType_Player)
        {
            if (ClientGlobal.Instance().IsMainPlayer(entity))
            {
                visible = true;
            }
            else
            {
                visible = m_bPlayerNameVisible;
            }
        }
        return visible;
    }
    /// <summary>
    /// 添加或者刷新血条
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="refresh">是否刷新已有的血条</param>
    void AddBar(IEntity entity)
    {
        if (entity == null)
        {
            return;
        }
        List<HeadTipData> lstdata = new List<HeadTipData>();

        lstdata.Add(new HeadTipData(entity, HeadTipType.Name, GetNameVisible(entity)));
        EntityType entityType = entity.GetEntityType();
        if (entityType != EntityType.EntityType_Box)
        {
            bool addHpSlider = IsNeedHpSlider(entity);
            if (addHpSlider)
            {
                bool visible = true;
                IPlayer player = ClientGlobal.Instance().MainPlayer;
                if (player == null)
                {
                    return;
                }
                if (entity.GetUID() == player.GetUID())
                {
                    visible = true;
                }
                else
                {
                    visible = m_bHpSliderVisible;
                }
                lstdata.Add(new HeadTipData(entity, HeadTipType.Hp, visible));
            }
            else 
            {
                lstdata.Add(new HeadTipData(entity, HeadTipType.Hp, addHpSlider));
            }
            //称号
            int titleId = entity.GetProp((int)PlayerProp.TitleId);
            if (titleId != 0)
            {
                lstdata.Add(new HeadTipData(entity, HeadTipType.Title, m_bPlayerTitleVisible));
            }
        }
        //添加头顶NPC icon
        if (RoleStateBarManager.IsEntityHaveHeadIconMask(entity) 
            && !NpcHeadTipsManager.Instance.IsHaveTips((Client.INPC)entity))
        {
            lstdata.Add(new HeadTipData(entity, HeadTipType.HeadMaskIcon, m_bHpSliderVisible));
        }
        RoleStateBar bar = this.CreateHeadText(entity, lstdata);
        if (bar != null)
        {
            //clan Name
            RefreshClanName(entity, bar);
        }
    }

   public bool IsNeedHpSlider(Client.IEntity entity)
    {
        if (entity.GetEntityType() == EntityType.EntityType_NPC)
        {                  
                INPC npc = entity as INPC;
                //是可以攻击的NPC
                if (npc.IsCanAttackNPC())
                {
                    Client.ISkillPart skillPart = MainPlayerHelper.GetMainPlayer().GetPart(EntityPart.Skill) as Client.ISkillPart;
                    bool canAttack = true;
                    if (skillPart != null)
                    {
                        int skillerror = 0;
                        canAttack = skillPart.CheckCanAttackTarget(entity, out skillerror);
                    }
                    if (npc.IsPet())
                    {
                        if (npc.IsMainPlayerSlave())
                        {
                            return true;
                        }
                        else
                        {
                            if (canAttack)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                    else if (npc.IsSummon())
                    {
                        if (npc.IsMainPlayerSlave())
                        {
                            return true;
                        }
                        else
                        {
                            if (canAttack)
                            {
                                return true;
                            }                         
                        }
                        return false;
                    }
                    else if (npc.IsMonster())
                    {
                        int hp = entity.GetProp((int)CreatureProp.Hp);
                        int maxhp = entity.GetProp((int)CreatureProp.MaxHp);
                        return hp < maxhp;
                    }
                }
                else
                {
                    table.NpcDataBase npcdb = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)entity.GetProp((int)Client.EntityProp.BaseID));
                    if (npcdb != null)
                    {
                        if (npcdb.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_TRANSFER || npcdb.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_COLLECT_PLANT
                            || npcdb.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_FUNCTION)
                        {
                            return false;
                        }
                    }
                }   
         }
        return true;
    }

    #region 头顶标识(图标)

    private void UpdateNpcHeadMask(Client.INPC npc)
    {
        if (null != npc)
        {
            return;
        }

        RoleStateBar bar = GetRoleBarByUID(npc.GetUID());
        bool enable = IsEntityHaveHeadIconMask(npc) && !NpcHeadTipsManager.Instance.IsHaveTips(npc);
        bar.SetWidgetState(HeadTipType.HeadMaskIcon, enable);
        if (enable)
        {
            bar.UpdateWidget(new HeadTipData(npc, HeadTipType.HeadMaskIcon, m_bHpSliderVisible));
        }
    }

    public static table.NpcHeadMaskDataBase GetNPCHeadMaskDB(Client.IEntity entity)
    {
        table.NpcHeadMaskDataBase db = null;
        if (null != entity
            //&& entity.GetEntityType() == EntityType.EntityType_NPC
            )
        {
            db = GetNPCHeadMaskDB((uint)entity.GetProp((int)Client.EntityProp.BaseID));
        }

        return db;
    }

    public static table.NpcHeadMaskDataBase GetNPCHeadMaskDB(uint npcBaseId)
    {
        table.NpcHeadMaskDataBase db = null;
        if (npcBaseId != 0
            //&& entity.GetEntityType() == EntityType.EntityType_NPC
            )
        {
            table.NpcDataBase npcdb
                = GameTableManager.Instance.GetTableItem<table.NpcDataBase>(npcBaseId);
            if (null != npcdb && npcdb.npcHeadMaskID != 0)
            {
                db = GameTableManager.Instance.GetTableItem<table.NpcHeadMaskDataBase>(npcdb.npcHeadMaskID);
            }
        }

        return db;
    }

    /// <summary>
    /// 是否有头顶标识
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static bool IsEntityHaveHeadIconMask(Client.IEntity entity)
    {
        return (GetNPCHeadMaskDB(entity) != null);
    }

    /// <summary>
    /// 是否有头顶标识
    /// </summary>
    /// <param name="npcBaseID"></param>
    /// <returns></returns>
    public static bool IsEntityHaveHeadIconMask(uint npcBaseID)
    {
        return (GetNPCHeadMaskDB(npcBaseID) != null);
    }
    #endregion

    void LateUpdate()
    {
        if (m_visible == false)
        {
            return;
        }
        for (int i = 0, imax = m_lstAllStateBar.Count; i < imax; i++)
        {
            var item = m_lstAllStateBar[i];
            UpdateBarPos(item);
        }
    }

    void UpdateBarPos(RoleStateBar item)
    {
        if (item == null)
        {
            return;
        }

        Vector3 pos = Vector3.zero;
        //性能优化 先去掉频繁的sendmessage 此处会造成gc modify by zhudianyu
        //if (item.entity.GetEntityType() == EntityType.EntityType_Player)
        //{
        //    bool bRide = (bool)item.entity.SendMessage(Client.EntityMessage.EntityCommond_IsRide, null);
        //    if (bRide)
        //    {
        //        pos = item.GetNodeWorldPos("MountName");
        //    }
        //    else
        //    {
        //        pos = item.GetNodeWorldPos("Name");
        //    }
        //}
        //else
        //{
        //    pos = item.GetNodeWorldPos("Name");
        //}
        pos = item.GetNodeWorldPos("Name");
        //Profiler.BeginSample("WorldToViewportPoint");

        if (m_maincamera == null)
        {
            Engine.Utility.Log.Error("--->>> m_maincamera 为空！！！");
            m_maincamera = Util.MainCameraObj.GetComponent<Camera>();
        }

        pos = m_maincamera.WorldToViewportPoint(pos);
        bool isVisible = (m_maincamera.orthographic || pos.z > 0f) && (!disableIfInvisible || (pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f));
        if (item.obj.activeSelf != isVisible)
        {
            item.obj.SetActive(isVisible);
        }
        //Profiler.EndSample();
        if (!isVisible)
        {
            return;
        }

        if (m_uicamera == null)
        {
            Engine.Utility.Log.Error("--->>> m_uicamera 为空！！！");
            m_uicamera = Util.UICameraObj.GetComponent<Camera>();
        }

        pos = m_uicamera.ViewportToWorldPoint(pos);
        item.mtrans.position = pos;

        pos = item.mtrans.localPosition;
        pos.x = Mathf.FloorToInt(pos.x);
        pos.y = Mathf.FloorToInt(pos.y);
        pos.z = 0f;
        item.mtrans.localPosition = pos;
    }

    #region GetHudText

    GameObject prefab;
    Transform GetHudText()
    {
        if (prefab == null)
        {
            UnityEngine.Object headStateObj = UIManager.GetResGameObj(GridID.Uirolestatebar);
            prefab = GameObject.Instantiate(headStateObj, Vector3.zero, Quaternion.identity) as GameObject;
            prefab.transform.parent = transform;
            prefab.transform.localScale = Vector3.one;
            prefab.name = "prefab";
            prefab.SetActive(false);
        }
        if (prefab != null)
        {
            return GetRoleStateBarTrans();
        }
        return null;
    }

    Transform GetRoleStateBarTrans()
    {
        Transform bar = null;
        if (m_lstUnUseStateBarTrans.Count > 0)
        {
            bar = m_lstUnUseStateBarTrans[0];
            m_lstUnUseStateBarTrans.RemoveAt(0);
        }
        else
        {
            GameObject go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            go.transform.parent = transform;
            go.transform.localScale = Vector3.one;
            bar = go.transform;
        }
        bar.gameObject.SetActive(true);
        return bar;
    }
    #endregion

    void OnDestroy()
    {
        RegisterGlobalEvent(false);
        Clear();
    }

    #region set
    public void SetPlayerNameVisible(bool visible)
    {
        m_bPlayerNameVisible = visible;

        for (int i = 0, imax = m_lstAllStateBar.Count; i < imax; i++)
        {
            RoleStateBar bar = m_lstAllStateBar[i];
            if (bar == null)
            {
                continue;
            }
            if (bar.entity == null)
            {
                continue;
            }
            if (bar.entity.GetEntityType() != EntityType.EntityType_Player)
            {
                continue;
            }

            if (bar.entity.GetUID() == ClientGlobal.Instance().MainPlayer.GetUID())
            {
                continue;
            }

            bar.SetWidgetState(HeadTipType.Name, m_bPlayerNameVisible);
        }
    }

    public void SetPlayerClanNameVisible(bool visible)
    {
        m_bPlayerClanNameVisible = visible;

        for (int i = 0, imax = m_lstAllStateBar.Count; i < imax; i++)
        {
            RoleStateBar bar = m_lstAllStateBar[i];

            if (bar == null)
            {
                continue;
            }

            if (bar.entity == null)
            {
                continue;
            }

            if (bar.entity.GetEntityType() != EntityType.EntityType_Player || bar.entity.GetEntityType() != EntityType.EntityType_NPC)
            {
                continue;
            }

            if (bar.entity.GetUID() == ClientGlobal.Instance().MainPlayer.GetUID())
            {
                continue;
            }

            bar.SetWidgetState(HeadTipType.Clan, m_bPlayerClanNameVisible);
        }
    }

    public void SetPlayerTitleVisible(bool visible)
    {
        m_bPlayerTitleVisible = visible;

        for (int i = 0, imax = m_lstAllStateBar.Count; i < imax; i++)
        {
            RoleStateBar bar = m_lstAllStateBar[i];
            if (bar == null)
            {
                continue;
            }
            if (bar.entity == null)
            {
                continue;
            }
            if (bar.entity.GetEntityType() != EntityType.EntityType_Player)
            {
                continue;
            }

            if (bar.entity.GetUID() == ClientGlobal.Instance().MainPlayer.GetUID())
            {
                continue;
            }

            bar.SetWidgetState(HeadTipType.Clan, m_bPlayerTitleVisible);
        }
    }

    public void SetNpcNameVisible(bool visible)
    {
        m_bNpcNameVisible = visible;
        for (int i = 0, imax = m_lstAllStateBar.Count; i < imax; i++)
        {
            RoleStateBar bar = m_lstAllStateBar[i];
            if (bar == null)
            {
                continue;
            }
            if (bar.entity == null)
            {
                continue;
            }
            if (bar.entity.GetEntityType() != EntityType.EntityType_NPC)
            {
                continue;
            }

            bar.SetWidgetState(HeadTipType.Name, m_bNpcNameVisible);
        }
    }

    public void SetHpSliderVisible(bool visible)
    {
        m_bHpSliderVisible = visible;
        for (int i = 0, imax = m_lstAllStateBar.Count; i < imax; i++)
        {
            RoleStateBar bar = m_lstAllStateBar[i];
            if (bar == null)
            {
                continue;
            }
            if (bar.entity == null)
            {
                continue;
            }
            //             if (bar.entity.GetUID() == ClientGlobal.Instance().MainPlayer.GetUID())
            //             {
            //                 continue;
            //             }

            bar.SetWidgetState(HeadTipType.Hp, m_bHpSliderVisible);
        }
    }
    #endregion

    public static string GetHpSpriteName(HpSpriteType type)
    {
        return GameTableManager.Instance.GetGlobalConfig<string>("HpSpriteName", type.ToString());
    }
}


