using System;
using System.Collections.Generic;
using UnityEngine;
using Client;
using table;
using Engine.Utility;
using System.Text;
public partial class MainPanel : UIPanelBase
{

    UITexture m_spriteTargetIcon = null;

    UILabel m_lableTargetName = null;

    UILabel m_lableTargetLevel = null;

    UILabel m_labelHP = null;

    UISprite m_spriteTargetHp = null;

    Transform m_transHateList = null;

    Transform m_transHateListRoot = null;

    GameObject m_prefabHateListGrid = null;

    private long m_nLastNpcId;
    private bool m_bShowEnemyList = false;

    List<GameCmd.stEnmityDataUserCmd_S.Enmity> m_lstEnimtyData = null;

    CMResAsynSeedData<CMTexture> m_curTargetIconAsynSeed = null;
    public bool BShowEnemyList
    {
        get
        {
            return m_bShowEnemyList;
        }
        set
        {
            m_bShowEnemyList = value;
            if (m_bShowEnemyList)
            {
                Client.IEntity entity = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl().GetCurTarget();
                if (entity != null)
                {
                    INPC npc = entity as INPC;
                    if (npc != null)
                    {
                        if (npc.IsBoss())
                        {
                            if (!TimerAxis.Instance().IsExist(m_uRefreshEnemyTimerID, this))
                            {
                                TimerAxis.Instance().SetTimer(m_uRefreshEnemyTimerID, m_uRefreshEnemytime, this);
                            }
                        }
                    }

                }
            }
            else
            {
                TimerAxis.Instance().KillTimer(m_uRefreshEnemyTimerID, this);
            }
        }
    }

    void InitTargetChangeUI()
    {
        IEntity ie = Client.ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl().GetCurTarget();

        if (ie == null)
        {
            m_trans_target.gameObject.SetActive(false);
        }
        else
        {
            m_trans_target.gameObject.SetActive(true);
            IEntity entity = ie;
            string name = entity.GetName();
            string lvstr = entity.GetProp((int)CreatureProp.Level).ToString();
            m_label_MonsterLevel_Label.text = lvstr;
          
            bool showPackage = false;
            //怪
            if (entity.GetEntityType() == EntityType.EntityType_NPC)
            {
                INPC npc = entity as INPC;
                NpcDataBase ndb = GameTableManager.Instance.GetTableItem<NpcDataBase>((uint)npc.GetProp((int)EntityProp.BaseID));
                if (ndb == null)
                {
                    return;
                }
                showPackage = npc.IsBoss();
                if (ndb.dwMonsterType == 1)
                {
                    InitTargetUIInMonster();
                    m_widget_player.gameObject.SetActive(false);
                    m_widget_monster.gameObject.SetActive(true);
                    m_lableTargetLevel.text = entity.GetProp((int)CreatureProp.Level).ToString();
                }
                else
                {
                    InitTargetUI();
                    string iconName = UIManager.GetIconName(ndb.npcIcon, npc.IsPet());
                    UIManager.GetTextureAsyn(iconName,
                           ref m_curTargetIconAsynSeed, () =>
                           {
                               if (null != m_spriteTargetIcon)
                               {
                                   m_spriteTargetIcon.mainTexture = null;
                               }
                           }, m_spriteTargetIcon, false);

                    m_widget_player.gameObject.SetActive(true);
                    m_widget_monster.gameObject.SetActive(false);
                }
            }

            // 玩家
            else
            {
                InitTargetUI();
                m_widget_player.gameObject.SetActive(true);
                m_widget_monster.gameObject.SetActive(false);
                IPlayer player = entity as IPlayer;
                if (player != null)
                {
                    int job = player.GetProp((int)PlayerProp.Job);
                    int sex = player.GetProp((int)PlayerProp.Sex);
                    SelectRoleDataBase sdb = table.SelectRoleDataBase.Where((GameCmd.enumProfession)job, (GameCmd.enmCharSex)sex);
                    if (sdb != null)
                    {
                        UIManager.GetTextureAsyn(sdb.strprofessionIcon,
                              ref m_curTargetIconAsynSeed, () =>
                              {
                                  if (null != m_spriteTargetIcon)
                                  {
                                      m_spriteTargetIcon.mainTexture = null;
                                  }
                              }, m_spriteTargetIcon, false);

                    }
                    m_transHateList.gameObject.SetActive(false);
                }
            }
            m_toggle_packageState.gameObject.SetActive(showPackage);
            m_lableTargetName.text = name;
            UpdateTargetHp(entity);
        }
    }


    void InitTargetUI()
    {
        m_spriteTargetIcon = m_trans_target.Find("player/btnhead/icon").GetComponent<UITexture>();

        m_lableTargetName = m_trans_target.Find("player/Name").GetComponent<UILabel>();

        // m_lableTargetLevel = m_trans_target.Find("player/levelbg/Label").GetComponent<UILabel>();

        m_spriteTargetHp = m_trans_target.Find("player/Hp").GetComponent<UISprite>();

        m_transHateList = m_trans_target.Find("player/HateList");

        m_labelHP = m_trans_target.Find("player/HpLabel").GetComponent<UILabel>();


        m_transHateList.gameObject.SetActive(false);
        m_transHateListRoot = m_transHateList.Find("List");

        m_prefabHateListGrid = m_trans_target.Find("UIHateListGrid").gameObject;

        GameObject headBtn = m_trans_target.Find("player/btnhead").gameObject;
        UIEventListener.Get(headBtn).onClick = onClick_Btnhead_Btn;
        BShowEnemyList = false;

        m_toggle_packageState.gameObject.SetActive(false);
        if (m_transHateListRoot != null)
        {
            enemyListCreator = m_transHateListRoot.GetComponent<UIGridCreatorBase>();
            if (enemyListCreator == null)
            {
                enemyListCreator = m_transHateListRoot.gameObject.AddComponent<UIGridCreatorBase>();
                enemyListCreator.gridContentOffset = new UnityEngine.Vector2(-25, 85);
                enemyListCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
                enemyListCreator.gridWidth = 290;
                enemyListCreator.gridHeight = 30;
                enemyListCreator.RefreshCheck();
                enemyListCreator.Initialize<UIEnemyListGrid>(m_trans_UIHateListGrid.gameObject, OnEnemyGridDataUpdate, null);
            }

        }
    }

    void InitTargetUIInMonster()
    {
        m_spriteTargetIcon = m_trans_target.Find("monster/btnhead/icon").GetComponent<UITexture>();

        m_lableTargetName = m_trans_target.Find("monster/Name").GetComponent<UILabel>();

        m_lableTargetLevel = m_trans_target.Find("monster/levelbg/Label").GetComponent<UILabel>();

        m_spriteTargetHp = m_trans_target.Find("monster/Hp").GetComponent<UISprite>();

        m_transHateList = m_trans_target.Find("monster/HateList");

        m_labelHP = m_trans_target.Find("monster/HpLabel").GetComponent<UILabel>();



        m_transHateList.gameObject.SetActive(false);
        m_transHateListRoot = m_transHateList.Find("List");

        m_prefabHateListGrid = m_trans_target.Find("UIHateListGrid").gameObject;

        //         GameObject headBtn = m_trans_target.Find("player/btnhead").gameObject;
        //         UIEventListener.Get(headBtn).onClick = onClick_Btnhead_Btn;
        BShowEnemyList = false;
    }

    void onClick_Btnhead_Btn(GameObject caster)
    {
        BShowEnemyList = !BShowEnemyList;
        Client.IEntity entity = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl().GetCurTarget();
        if (entity != null)
        {
            if (entity.GetEntityType() == EntityType.EntityType_Player)
            {
                DataManager.Instance.Sender.RequestPlayerInfoForOprate(entity.GetID(), PlayerOpreatePanel.ViewType.Normal);
            }
            else if (entity.GetEntityType() == EntityType.EntityTYpe_Robot)
            {
                ShowRobotOpreate((uint)entity.GetID(), (uint)entity.GetProp((int)RobotProp.Job),
                    (uint)entity.GetProp((int)CreatureProp.Level), entity.GetName(), (uint)entity.GetProp((int)RobotProp.Sex));
            }
            else
            {


            }
        }

        if (BShowEnemyList)
        {
            if (entity != null)
            {
                NetService.Instance.Send(new GameCmd.stEnmityDataUserCmd_CS() { npcid = entity.GetID() });
            }

        }
        else
        {
            m_transHateList.gameObject.SetActive(false);
        }

    }

    void ShowRobotOpreate(uint uid, uint job, uint level, string name, uint sex)
    {
        PlayerOpreatePanel.PlayerViewInfo data = new PlayerOpreatePanel.PlayerViewInfo();
        data.isRobot = true;
        data.uid = uid;
        data.name = name;
        data.teamNum = 0;
        data.teamID = 0;
        data.level = level;
        data.job = job;
        data.sex = sex;

        data.viewType = PlayerOpreatePanel.ViewType.Normal;
        PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns btns = 0;
        btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.SendTxt;
        btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.ViewMsg;
        btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.AddFriend;
        btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.Visit;
        btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.Invite;
        data.playerViewMask = (int)btns;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PlayerOpreatePanel, data: data);
    }
    void UpdateTargetHp(Client.IEntity entity)
    {
        if (entity == null)
        {
            return;
        }
        int currHp = entity.GetProp((int)CreatureProp.Hp);
        float hp = currHp / (float)entity.GetProp((int)CreatureProp.MaxHp);
        if (m_spriteTargetHp == null)
        {
            return;
        }
        m_spriteTargetHp.fillAmount = hp;

        if (m_labelHP != null)
        {
            if (entity.GetEntityType() == EntityType.EntityType_NPC)
            {
                Client.INPC npc = (Client.INPC)entity;
                if (npc.IsMonster())
                {
                    m_labelHP.alpha = 1f;
                    m_labelHP.gameObject.SetActive(true);
                    m_labelHP.text = currHp.ToString();
                    return;
                }
            }
            m_labelHP.alpha = 0f;
            m_labelHP.gameObject.SetActive(false);
        }
    }

    //StringBuilder targetName = new StringBuilder(20);
    private void OnTargetChange(object param)
    {
        stTargetChange tc = (stTargetChange)param;
        if (tc.target == null)
        {
            m_trans_target.gameObject.SetActive(false);
        }
        else
        {
            IEntity entity = tc.target;
            string name = entity.GetName();
            string lvstr = entity.GetProp((int)CreatureProp.Level).ToString();
            m_label_MonsterLevel_Label.text = lvstr;
            bool showPackeage = false;
          
            if (entity.GetEntityType() == EntityType.EntityType_NPC)
            {
                INPC npc = entity as INPC;
                showPackeage = npc.IsBoss();
                NpcDataBase ndb = GameTableManager.Instance.GetTableItem<NpcDataBase>((uint)npc.GetProp((int)EntityProp.BaseID));
                if (ndb != null)
                {

                    if (ndb.dwMonsterType == 1)
                    {
                        InitTargetUIInMonster();
                        m_widget_player.gameObject.SetActive(false);
                        m_widget_monster.gameObject.SetActive(true);
                        m_lableTargetLevel.text = entity.GetProp((int)CreatureProp.Level).ToString();
                    }
                    else
                    {
                        InitTargetUI();
                        string iconName = UIManager.GetIconName(ndb.npcIcon, npc.IsPet());
                        UIManager.GetTextureAsyn(iconName,
                               ref m_curTargetIconAsynSeed, () =>
                               {
                                   if (null != m_spriteTargetIcon)
                                   {
                                       m_spriteTargetIcon.mainTexture = null;
                                   }
                               }, m_spriteTargetIcon, false);

                        m_widget_player.gameObject.SetActive(true);
                        m_widget_monster.gameObject.SetActive(false);
                    }
                }
                if (npc.IsMonster() == false)
                {
                    bool bVisit = true;
                    if (npc.IsSummon() || npc.IsPet())
                    {
                        bVisit = false;
                    }
                    
                    CopyTypeTable ct = DataManager.Manager<ComBatCopyDataManager>().GetCurCopyType();
                    if (ct != CopyTypeTable.Arena)
                    {
                        if (bVisit)
                        {
  
                            Client.ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl().VisiteNPC(npc.GetUID());
                        }
                    }

                }
                else
                {
                    //请
                    if (m_nLastNpcId != entity.GetUID())
                    {                       
                        BShowEnemyList = false;
                        m_transHateList.gameObject.SetActive(false);
                    }
                }
                m_nLastNpcId = entity.GetUID();
                if (m_nLastNpcId != 0)
                {
                    stRefreshNPCBelongParam npcbelongdata = NpcAscription.Instance.GetNpcBelongByNpcID(m_nLastNpcId);
                    UpdateTargetStatus(npcbelongdata.npcid, npcbelongdata.teamid, npcbelongdata.ownerid, npcbelongdata.clanid, npcbelongdata.ownerName);
                }
            }
            else
            {
                InitTargetUI();
                m_widget_player.gameObject.SetActive(true);
                m_widget_monster.gameObject.SetActive(false);
                IPlayer player = entity as IPlayer;
                if (player != null)
                {
                    int job = player.GetProp((int)PlayerProp.Job);
                    int sex = player.GetProp((int)PlayerProp.Sex);
                    SelectRoleDataBase sdb = table.SelectRoleDataBase.Where((GameCmd.enumProfession)job, (GameCmd.enmCharSex)sex);
                    if (sdb != null)
                    {
                        UIManager.GetTextureAsyn(sdb.strprofessionIcon,
                              ref m_curTargetIconAsynSeed, () =>
                              {
                                  if (null != m_spriteTargetIcon)
                                  {
                                      m_spriteTargetIcon.mainTexture = null;
                                  }
                              }, m_spriteTargetIcon, false);

                    }
                    m_transHateList.gameObject.SetActive(false);

                    //目标选择
                    OnSetSelectTargetGrid(player.GetID());
                }


            }
            m_toggle_packageState.gameObject.SetActive(showPackeage);

            m_lableTargetName.text = name;
            //m_lableTargetLevel.text = entity.GetProp((int)CreatureProp.Level).ToString();

            UpdateTargetHp(entity);

            BuffManager.TargetBuffList.Clear();


            if (entity != null)
            {
                List<Client.stAddBuff> lstBuff;
                IBuffPart part = entity.GetPart(EntityPart.Buff) as IBuffPart;
                if (part != null)
                {
                    part.GetBuffList(out lstBuff);
                    for (int i = 0; i < lstBuff.Count; ++i)
                    {
                        stAddBuff buff = lstBuff[i];
                        table.BuffDataBase db = GameTableManager.Instance.GetTableItem<table.BuffDataBase>(buff.buffid);
                        if (db != null)
                        {
                            if (db.dwShield == 1)
                            {//不显示的直接跳过
                                continue;
                            }
                        }
                        BuffManager.TargetBuffList.Add(lstBuff[i]);
                    }
                }
                else
                {
                    Engine.Utility.Log.Error("获取{0}Buff部件失败!", entity.GetName());
                }
            }
            m_trans_target.gameObject.SetActive(true);
            RefreshBuffIcon(m_trans_TargetBUffContainer, BuffManager.TargetBuffList);
        }
    }

    void OnEnemyList(GameCmd.stEnmityDataUserCmd_S cmd)
    {
        Transform listRoot = m_transHateListRoot;
        m_lstEnimtyData = cmd.data;
        if (listRoot != null && BShowEnemyList)
        {
            //listRoot.DestroyChildren();
            IEntity target = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl().GetCurTarget();
            if (target != null && cmd.npcid == target.GetID())
            {
                m_transHateList.gameObject.SetActive(cmd.data.Count > 0);
                enemyListCreator.CreateGrids(m_lstEnimtyData.Count);
            }
        }
    }

    void UpdateTargetStatus(long npcid, uint teamid, uint ownerid,uint clanid,string ownerName)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("IEntitySystem is null--");
            return;
        }
        INPC npc = es.FindEntity(npcid) as INPC;
        if (npc == null)
        {
            Engine.Utility.Log.Error("npc is null--{0}", npcid);
            return;
        }
       
        if (m_nLastNpcId != 0 && m_nLastNpcId != npcid)
        {
            return;
        }
        bool showPackage = npc.IsBoss();
        m_toggle_packageState.gameObject.SetActive(showPackage);
        string text = "";
        bool belongMe = false;


       

        if (teamid != 0)
        {
            if (DataManager.Manager<TeamDataManager>().TeamId == teamid)
            {
                belongMe = true;
            }
            else
            {
                belongMe = false;
            }
            text = string.Format("{0}的队伍", ownerName);
        }
        else if (clanid != 0)
        {
            DataManager.Manager<ClanManger>().GetClanName(clanid, (namedata) =>
            {
                if (DataManager.Manager<ClanManger>().ClanId == clanid)
                {
                    belongMe = true;
                }
                else
                {
                    belongMe = false;
                }
                text = string.Format("{0}氏族", namedata.ClanName);
            });
        }
        else
        {
            belongMe = MainPlayerHelper.GetPlayerID() == ownerid;
            text = ownerName;
        }
        //Engine.Utility.Log.Error("ownerid:" + ownerid 
        //               + "---text:" + text 
        //               + "---teamid:" + teamid 
        //               + "---clanid:" + clanid);


        m_label_TeamLeaderName.text = text;
        m_toggle_packageState.value = belongMe;

    }
    private void OnEnemyGridDataUpdate(UIGridBase data, int index)
    {
        if (data is UIEnemyListGrid)
        {
            UIEnemyListGrid grid = data as UIEnemyListGrid;
            grid.SetGridData(index);
            if (m_lstEnimtyData != null && index < m_lstEnimtyData.Count)
            {
                grid.SetDataValue(m_lstEnimtyData[index]);
            }

        }
    }
}
