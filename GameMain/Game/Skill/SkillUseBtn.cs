using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using GameCmd;
using table;
using Vector2 = UnityEngine.Vector2;
using Engine.Utility;
//
public class SkillCDInfo
{
    public int skillid;
    public float totalTime;
    public float currTime;
}


public class SkillUseBtn : MonoBehaviour, ITimer
{
    UITexture skillIcon;
    UILabel skillName;
    UISprite overlay;
    UIButton skillBtn;

    [SerializeField]
    int m_skillBtnIndex = 0;
    SkillSettingState m_skillState;
    [SerializeField]
    uint m_skillid;
    public uint SkillId { get { return m_skillid; } }
    public MainPanel parent { get; set; }
    Dictionary<uint, table.SkillDatabase> m_dictskill;
    public int SkillIndex { get { return m_skillBtnIndex; } }
    bool IsCDing = false;


    Transform m_transLock;
    UILabel m_labelUnLockLevel;

    int m_nSprwidth = 0;
    int m_nSprheight = 0;
    bool m_bSkillLongPress = false;
    private readonly uint m_uSkillUseTimeID = 1000;
    private Transform temp = null;
    Transform m_addTrans = null;
    void Awake()
    {
        skillBtn = transform.GetComponent<UIButton>();
        temp = transform.Find("Label");
        if (null != temp)
        {
            skillName = temp.GetComponent<UILabel>();
        }
        
        skillIcon = transform.Find("Sprite").GetComponent<UITexture>();
        overlay = transform.Find("Overlay").GetComponent<UISprite>();
        m_addTrans = transform.Find("jiaHao");
        if(m_addTrans != null)
        {
            UIEventListener.Get(m_addTrans.gameObject).onClick = OnAddClick;
        }
        if (overlay != null)
        {
            m_nSprheight = overlay.height;
            m_nSprwidth = overlay.width;
        }
        m_transLock = transform.Find("lock");
        if (m_transLock != null)
        {
            Transform label = m_transLock.Find("Label");
            if (label != null)
            {
                m_labelUnLockLevel = label.GetComponent<UILabel>();
            }
        }
        UIEventListener.Get(gameObject).onClick = OnSkillUse;
    }
    void OnEnable()
    {
        EventEngine.Instance().AddEventListener((int)GameEventID.SKILLSYSTEM_SETSKILLPOS, OnEvent);

    }

    public void OnDisable()
    {
        EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLSYSTEM_SETSKILLPOS, OnEvent);

    }

    void OnEvent(int eventID, object data)
    {
        if (eventID == (int)GameEventID.SKILLSYSTEM_SETSKILLPOS)
        {
            stSetSkillPos sp = (stSetSkillPos)data;
            if (sp.pos == (uint)m_skillBtnIndex)
            {
                
                SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(sp.skillid, 1);
                if (db != null)
                {
                    GuideDefine.FuncOpenShowData showData = new GuideDefine.FuncOpenShowData()
                    {
                        FOT = GuideDefine.FuncOpenType.Skill,
                        Icon = UIManager.BuildCircleIconName(db.iconPath),
                        TargetObj = this.gameObject,
                        Name = db.strName,
                        Data = data,
                    };
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTNEWFUNCOPENADD, showData);
                }
            }
        }
       
    }
    void OnAddClick(GameObject go)
    {
        //if(DataManager.Manager<RideManager>().IsRide)
        //{
        //    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RidePanel, jumpData: new UIPanelBase.PanelJumpData() { Tabs = new int[] { 2 } });
        //}
        //else
        {
           if( m_skillBtnIndex == 9)
           {
               //宠物
               int lv = MainPlayerHelper.GetPlayerLevel();
               int openLv = GameTableManager.Instance.GetGlobalConfig<int>("PetSkillOpenLevel");
               if (lv >= openLv)
               {
                   DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PetLearnSkill);
               }
               else
               {
                   TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Pet_Commond_kaifangchongwujinengxitong, openLv);
               }
           }
        }
    }
    void OnSkillUse(GameObject go)
    {
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINBTN_ONTOGGLE, new Client.stMainBtnSet() { pos = 1, isShow = false });

        if (parent != null)
        {
            Client.IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
            if (mainPlayer == null)
            {
                Log.Error("mainPlayer is null");
                return;
            }

            if (IsCDing)
            {
                Log.Error("IsCDing");
                return;
            }

            bool bRide = DataManager.Manager<RideManager>().IsRide;
            if (bRide)
            {//使用任何一个技能都下马
                if (m_skillBtnIndex != 0)
                {
                    parent.ReqUseSkill((uint)m_skillid);
                }
                else
                {
                    DataManager.Instance.Sender.RideDownRide();
                    parent.ReqUseSkill((uint)m_skillid);
                }
            }
            else
            {
                if (m_skillBtnIndex != 10)
                {
                    LearnSkillDataManager skilldata = DataManager.Manager<LearnSkillDataManager>();
                    uint commonSkill = skilldata.GetCommonSkillIDByJob();
                    if (commonSkill != m_skillid)
                    {//普工不走cd
                        if (m_skillBtnIndex == 9)
                        {
                            if (CheckItem())
                            {
                                var cmd = new GameCmd.stMultiAttackUpMagicUserCmd_C();
                                cmd.wdSkillID = (uint)m_skillid;
                                Client.IController cs = GetController();
                                if(cs != null)
                                {
                                    IEntity target = cs.GetCurTarget();
                                    if (target != null)
                                    {
                                        stMultiAttackUpMagicUserCmd_C.Item item = new stMultiAttackUpMagicUserCmd_C.Item();
                                        item.byEntryType = (target.GetEntityType() == EntityType.EntityType_Player ? SceneEntryType.SceneEntry_Player : SceneEntryType.SceneEntry_NPC);
                                        item.dwDefencerID = target.GetID();
                                        cmd.data.Add(item);
                                        ReqUsePetSkill(cmd);
                                    }
                                    else
                                    {
                                        SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(m_skillid);
                                        if(db != null)
                                        {
                                            if(db.targetType != 0)
                                            {
                                                ReqUsePetSkill(cmd);
                                            }
                                            else
                                            {
                                                if(db.targetParam == 4 ||db.targetParam == 7||db.targetParam == 8)
                                                {
                                                    ReqUsePetSkill(cmd);
                                                }
                                                else
                                                {
                                                    TipsManager.Instance.ShowTips("该技能需要目标");
                                                }
                                              
                                            }
                                        }
                                    }
                                }
                               
                                //宠物技能使用
                             
                          
                            }

                        }
                        else
                        {
                            parent.ReqUseSkill((uint)m_skillid);
                        }

                    }

                }
                else
                {
                    parent.ChangeSkill(this.gameObject);
                }
            }
            parent.OnClickSkillBtn(go, 50010);
        }
        else
        {
            Log.Error("parent is null");
        }
    }
    void ReqUsePetSkill(stMultiAttackUpMagicUserCmd_C cmd)
    {
        IPlayer player = MainPlayerHelper.GetMainPlayer();
        if(player == null)
        {
            return;
        }
        ISkillPart sp = player.GetPart(EntityPart.Skill) as ISkillPart;
        if(sp == null)
        {
            return;
        }
        sp.RequestUsePetSkill(cmd);
    }
    Client.IController GetController()
    {

        Client.IControllerSystem ctrlSys = ClientGlobal.Instance().GetControllerSystem();
        if (ctrlSys == null)
        {
            return null;
        }

        Client.IController ctrl = ctrlSys.GetActiveCtrl();
        if (ctrl == null)
        {
            return null;
        }
        return ctrl;
    }
    public void AddEffectWhenCDEnd()
    {
        if (this.gameObject != null)
        {
            parent.OnClickSkillBtn(this.gameObject, 50020);
        }

    }
    bool CheckItem()
    {
        SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>((uint)m_skillid, 1);
        if (db == null)
        {
            return false;
        }
        string needstr = db.skillPlayCostItem;
        string[] itemArray = needstr.Split('_');
        if (itemArray.Length > 1)
        {
            uint itemID = 0, itemCount = 0;
            if (uint.TryParse(itemArray[0], out itemID))
            {
                if (uint.TryParse(itemArray[1], out itemCount))
                {
                    int count = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
                    if (count < itemCount)
                    {
                        ItemDataBase idb = GameTableManager.Instance.GetTableItem<ItemDataBase>(itemID);
                        if (idb != null)
                        {
                            TipsManager.Instance.ShowTips(idb.itemName + CommonData.GetLocalString("不足"));
                        }
                        return false;
                    }
                }
            }
        }
        return true;
    }
    /// <summary>
    /// 1-8
    /// </summary>
    /// <param name="skillBtnIndex"></param>
    public void Init(int skillBtnIndex)
    {
        m_skillBtnIndex = skillBtnIndex;
        m_skillState = SkillSettingState.StateOne;
        if (skillBtnIndex >= 5)
        {
            m_skillState = SkillSettingState.StateTwo;
        }

        if (m_skillBtnIndex == 0)//0 是普工  9是坐骑技能 10在未骑乘下是切换 骑乘下是坐骑技能
        {
          
        }
        if (m_skillBtnIndex == 10)
        {
            m_skillState = DataManager.Manager<LearnSkillDataManager>().CurState;
        }
        LearnSkillDataManager dm = DataManager.Manager<LearnSkillDataManager>();
        List<uint> levelList = dm.GetUnLockLevelList();
        if (levelList == null)
        {
            return;
        }
        int index = skillBtnIndex - 1;
        if (index >= 0)
        {
            if (index < levelList.Count)
            {
                uint lev = levelList[index];
                if (m_labelUnLockLevel != null)
                {
                    m_labelUnLockLevel.text = lev.ToString();
                }
            }
        }
    }

    public void Refresh()
    {
        IsCDing = false;
     
        Client.IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
            return;
        //bool bRide = DataManager.Manager<RideManager>().IsRide;
        //if (bRide)
        //{
        //    //SetRideSkills();
        //}
        //else
        {
            SetPlayerSkills();
        }
        SetCommonSkill();
    }

    public void SetSkillLockStatus(bool block)
    {
        if (m_transLock != null)
        {
            if (block)
            {
                m_transLock.gameObject.SetActive(true);
                if (!IsShowLock((int)m_skillBtnIndex))
                {
                    if (m_labelUnLockLevel != null)
                    {
                        m_labelUnLockLevel.text = "";
                    }
                }
            }
            else
            {
                if (!IsShowLock((int)m_skillBtnIndex))
                {
                    m_transLock.gameObject.SetActive(false);
                }
            }
        }
    }
    /// <summary>
    /// 设置宠物技能
    /// </summary>
    void SetPetSkill()
    {
        LearnSkillDataManager data = DataManager.Manager<LearnSkillDataManager>();
        gameObject.SetActive(data.bShowPetSkill);

        PetDataManager petData = DataManager.Manager<PetDataManager>();
        if (petData != null)
        {
            IPet pet = petData.GetPetByThisID(petData.CurFightingPet);
            if (pet != null)
            {
                List<PetSkillObj> list = pet.GetPetSkillList();
                if (list.Count == 0)
                {
                    skillIcon.mainTexture = null;
                    SetLongPressEvent(false);
                    ShowAdd(true);
                    return;
                }

           
                for (int i = 0; i < list.Count;i++ )
                {
                    var skill = list[i];
                    uint skillID = (uint)skill.id;
                    SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID, skill.lv);
                    if (db != null)
                    {
                        if (db.petType == (int)PetSkillType.Initiative)
                        {
                            m_skillid = skillID;
                            UIManager.GetTextureAsyn(DataManager.Manager<UIManager>().GetResIDByFileName(false, db.iconPath, true), ref iuiIconAtlas, () =>
                            {
                                if (null != skillIcon)
                                {
                                    skillIcon.mainTexture = null;
                                }
                            }, skillIcon, false);
                            ShowAdd(false);
                            SetLongPressEvent(true);
                            break;
                        }
                        else
                        {
                            SetLongPressEvent(false);
                            ShowAdd(true);
                            skillIcon.mainTexture = null;
                        }
                    }
                }
            }
        }
        if (data.bShowPetSkill)
        {
            RunCD();
        }
    }
    /// <summary>
    /// 设置切换技能
    /// </summary>
    void SetChangeStateSkill()
    {
        Client.IPlayer player = ClientGlobal.Instance().MainPlayer;
        int level = player.GetProp((int)CreatureProp.Level);
        int job = player.GetProp((int)PlayerProp.Job);
        string secondKey = job.ToString();
        SkillSettingState curState = DataManager.Manager<LearnSkillDataManager>().CurState;
        if (curState == SkillSettingState.StateTwo)
        {
            secondKey = (10 + job).ToString();
        }
        m_skillid = GameTableManager.Instance.GetGlobalConfig<uint>("ChangeStateSkillID", secondKey);
        int needLevel = GameTableManager.Instance.GetGlobalConfig<int>("ChangeStateLevel");
        if (level < needLevel)
        {
            ResetBtn();
            //skillIcon.spriteName = "icon_lock";
            skillIcon.mainTexture = null;
            if (skillName != null)
            {
                skillName.text = needLevel.ToString();
            }
            SetBtnEnable(false);
            SetLongPressEvent(false);
            return;
        }
        else
        {
            if (skillBtn != null)
            {
                ResetBtn();
            }
            table.SkillDatabase db = GameTableManager.Instance.GetTableItem<table.SkillDatabase>((uint)m_skillid, 1);
            if (db != null)
            {
                UIManager.GetTextureAsyn(DataManager.Manager<UIManager>().GetResIDByFileName(false, db.iconPath, true), ref iuiIconAtlas, () =>
                            {
                                if (null != skillIcon)
                                {
                                    skillIcon.mainTexture = null;
                                }
                            }, skillIcon,false);
            }
        }
        if(skillIcon != null)
        {
            skillIcon.width = overlay.width;
            skillIcon.height = overlay.height;
            skillIcon.alpha = 1f;
        }
   
        if (skillBtn != null)
        {
            SetLongPressEvent(true);
            SetBtnEnable(true);
        }

        RunCD();
    }
    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;
    void SetSkills()
    {
        LearnSkillDataManager skilldata = DataManager.Manager<LearnSkillDataManager>();
        uint skillid = 0;

        if (skilldata.TryGetLocationSkillId(m_skillState, m_skillBtnIndex, out skillid))
        {
            m_skillid = skillid;
            if (skillid == 0)
            {
                if (IsShowLock((int)m_skillBtnIndex))
                {
                    ShowLock();
                }
            }
            else
            {
                table.SkillDatabase db = GameTableManager.Instance.GetTableItem<table.SkillDatabase>(skillid, 1);
                if (db != null)
                {
                    if (skillIcon != null)
                    {
                        skillIcon.alpha = 1f;
                        UIManager.GetTextureAsyn(DataManager.Manager<UIManager>().GetResIDByFileName(false, db.iconPath, true), ref iuiIconAtlas
                            , () =>
                            {
                                if (null != skillIcon)
                                {
                                    skillIcon.mainTexture = null;
                                }
                            }, skillIcon
                            , false);
                        SetLongPressEvent(true);
                        m_transLock.gameObject.SetActive(false);
                    }
                }
                ResetBtn();
                RunCD();
            }
        }
        else
        {
            m_skillid = skillid;

            if (m_skillBtnIndex < 5)
            {
                if (IsShowLock((int)m_skillBtnIndex))
                {
                    ShowLock();
                }
                else
                {
                    m_transLock.gameObject.SetActive(false);
                    SetLongPressEvent(false);
                }
            }
        }
        if (m_skillid == 0)
        {
            skillIcon.mainTexture = null;
        }
    }
    void SetCommonSkill()
    {
        if (m_skillBtnIndex == 0)
        {
            uint commonID = DataManager.Manager<LearnSkillDataManager>().GetCommonSkillIDByJob();
            //普工
            SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(commonID);
            if (db != null)
            {
                m_skillid = commonID;
                UIManager.GetTextureAsyn(DataManager.Manager<UIManager>().GetResIDByFileName(false, db.iconPath, true), ref iuiIconAtlas, () =>
                {
                    if (null != skillIcon)
                    {
                        skillIcon.mainTexture = null;
                    }
                }, skillIcon, false);
            }
            return;
        }
    }
    void ShowAdd(bool bShow)
    {
        if(m_addTrans != null)
        {
            m_addTrans.gameObject.SetActive(bShow);
        }
    }
    private void SetPlayerSkills()
    {
        ShowAdd(false);
        if (m_skillBtnIndex == 9)//宠物技能
        {
            SetPetSkill();
            return;
        }
        else if (m_skillBtnIndex == 10)//状态切换
        {
            SetChangeStateSkill();
            return;
        }
        else if(m_skillBtnIndex == 0)
        {
            return;
        }
        SetSkills();

    }
    bool IsShowLock(int index)
    {
        List<uint> levelList = DataManager.Manager<LearnSkillDataManager>().GetUnLockLevelList();
        if (index < levelList.Count)
        {
            int n = index - 1;
            if (n >= 0 && n < levelList.Count)
            {
                uint unLockLevel = levelList[n];
                if (unLockLevel <= MainPlayerHelper.GetPlayerLevel())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }
        return false;
    }
    void ShowLock()
    {
        SetBtnEnable(false);

        m_transLock.gameObject.SetActive(true);
        SetLongPressEvent(false);
    }
    /// <summary>
    /// 设置长按
    /// </summary>
    /// <param name="bSet">true是设置 false是清空</param>
    void SetLongPressEvent(bool bSet)
    {
        //暂时去除长按tips功能
        return;
        LongPress lp = this.gameObject.GetComponent<LongPress>();
        if (lp == null)
        {
            lp = this.gameObject.AddComponent<LongPress>();
        }
        if (bSet)
        {
            lp.InitLongPress(() =>
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PlayerSkillTipsPanel, panelShowAction: (pb) =>
                {
                    if (null != pb && pb is PlayerSkillTipsPanel)
                    {
                        PlayerSkillTipsPanel panel = pb as PlayerSkillTipsPanel;
                        Client.IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
                        if (mainPlayer == null)
                            return;
                        panel.InitParentTransform(transform, new Vector2(m_nSprwidth, m_nSprheight));

                        //bool bRide = DataManager.Manager<RideManager>().IsRide;
                        //if (!bRide)
                        {
                            table.SkillDatabase db = GameTableManager.Instance.GetTableItem<table.SkillDatabase>((uint)m_skillid, 1);
                            if (db != null)
                            {
                                if (m_skillBtnIndex == 9)//宠物技能
                                {
                                    panel.ShowUI(PlayerSkillTipsPanel.SkillTipsType.Pet, db);
                                }
                                else
                                {
                                    panel.ShowUI(PlayerSkillTipsPanel.SkillTipsType.Player, db);
                                }
                            }
                        }
                        //else
                        //{
                        //    table.RideSkillDes rideSkill = GameTableManager.Instance.GetTableItem<table.RideSkillDes>((uint)m_skillid);
                        //    if (rideSkill != null)
                        //    {
                        //        panel.ShowUI(PlayerSkillTipsPanel.SkillTipsType.Ride, rideSkill);
                        //    }
                        //}
                    }
                });
                
            },
            () =>
            {
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.PlayerSkillTipsPanel);
            }, 500);

        }
        else
        {
            lp.InitLongPress(null, null);
        }

    }
    string GetItemStr(string dbCostStr)
    {
        string showStr = "";
        string needstr = dbCostStr;
        string[] itemArray = needstr.Split('_');
        if (itemArray.Length > 1)
        {
            uint itemID = 0, itemCount = 0;
            if (uint.TryParse(itemArray[0], out itemID))
            {
                if (uint.TryParse(itemArray[1], out itemCount))
                {
                    ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>(itemID);
                    if (db != null)
                    {
                        showStr = db.itemName + "*" + itemCount;
                    }
                }
            }
        }
        return showStr;
    }
    private void SetRideSkills()
    {
        RideData rideData = DataManager.Manager<RideManager>().GetCurrRideData();
        if (rideData == null)
        {
            return;
        }
        ResetBtn();
        bool canuse = false;
        int skillIndex = 0;

        int offset = 0;
        LearnSkillDataManager data = DataManager.Manager<LearnSkillDataManager>();
        if (data.CurState == SkillSettingState.StateTwo)
        {
            offset = 4;
        }
        if (m_skillBtnIndex == 9)//第一个技能
        {
            gameObject.SetActive(true);
            skillIndex = 0;
            canuse = rideData.skill_ids.Count > skillIndex;
        }
        else if (m_skillBtnIndex == 1 + offset)
        {
            skillIndex = 1;
            canuse = rideData.skill_ids.Count > skillIndex;
        }
        else if (m_skillBtnIndex == 2 + offset)
        {
            skillIndex = 2;
            canuse = rideData.skill_ids.Count > skillIndex;
        }
        else if (m_skillBtnIndex == 3 + offset)
        {
            skillIndex = 3;
            canuse = rideData.skill_ids.Count > skillIndex;
        }
        else if (m_skillBtnIndex == 4 + offset)
        {
            skillIndex = 4;
            canuse = rideData.skill_ids.Count > skillIndex;
        }
        else if (m_skillBtnIndex == 10)
        {
            skillIndex = 5;
            canuse = rideData.skill_ids.Count > skillIndex;
        }
        else
        {
            return;
        }
        ShowAdd(!canuse);
        if (canuse)
        {

            m_skillid = (uint)rideData.skill_ids[skillIndex];
            SetLongPressEvent(true);

            table.RideSkillDes rideSkill = GameTableManager.Instance.GetTableItem<table.RideSkillDes>((uint)m_skillid);
            if (rideSkill != null)
            {
                if (skillIcon != null)
                {
                    bool bAdjust = m_skillBtnIndex == 9 ? false : true;
                    UIManager.GetTextureAsyn(DataManager.Manager<UIManager>().GetResIDByFileName(false, rideSkill.skillIcon, true), ref iuiIconAtlas, () =>
                    {
                        if (null != skillIcon)
                        {
                            skillIcon.mainTexture = null;
                        }
                    }, skillIcon, false);
                }
            }
            RunCD();
        }
        else
        {
            SetLongPressEvent(false);
   
            m_skillid = 0;
        }
        SetBtnEnable(canuse);
        if (skillIcon != null)
        {
            skillIcon.alpha = canuse ? 1f : 0f;
        }
    }

    public void ShowPublicCd()
    {
        if (IsCDing)
            return;
        DataManager.Manager<SkillCDManager>().AddCommonSkillCD(SkillId);
        RunCD();
    }
    void SetBtnEnable(bool bEnable)
    {
      
        if (skillBtn != null)
        {
            skillBtn.isEnabled = bEnable;
        }

    }
    void ResetBtn()
    {
        IsCDing = false;
        if (skillBtn != null)
        {
            skillBtn.isEnabled = true;
        }
        overlay.fillAmount = 0f;
        if(null != skillName)
            skillName.text = "";
    }

    public void RunCD()
    {
        if (IsCDing)
        {
            Log.LogGroup("ZDY", "run cd and is cding ");
            return;
        }


        Client.IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
        {
            Log.Error("main player is null");
            return;
        }

        SkillCDInfo cdInfo = DataManager.Manager<SkillCDManager>().GetSkillCDBySkillId((uint)m_skillid);

        if (cdInfo != null)
        {
            IsCDing = true;
            SetBtnEnable(false);
            if (!TimerAxis.Instance().IsExist(m_uSkillUseTimeID, this))
            {
                //   Log.LogGroup("ZDY", "start cd " );
                TimerAxis.Instance().SetTimer(m_uSkillUseTimeID, 30, this);
            }
        }
        else
        {
            SetBtnEnable(true);
            //Log.LogGroup("ZDY", "run cd info is null " + m_skillid);
        }
    }

    public void ReleaseSkillBtn()
    {
        if(iuiIconAtlas != null)
        {
            iuiIconAtlas.Release();
            iuiIconAtlas = null;
        }
    }

    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == m_uSkillUseTimeID)
        {
            if (overlay == null)
            {
                ResetBtn();
                TimerAxis.Instance().KillTimer(m_uSkillUseTimeID, this);
                //  Log.Error("over lay is null cd timer");
                return;
            }
            // Log.LogGroup("ZDY", "cd timer running");
            SkillCDInfo cdInfo = DataManager.Manager<SkillCDManager>().GetSkillCDBySkillId((uint)m_skillid);
            if (cdInfo != null)
            {
                if (cdInfo.currTime > 0 && cdInfo.totalTime != 0)
                {
                    overlay.fillAmount = cdInfo.currTime / cdInfo.totalTime;
                    //Log.LogGroup("ZDY", "cdInfo.currTime is "+cdInfo.currTime+ " fillamount is "+overlay.fillAmount);
                    if(skillName != null)
                    {
                        skillName.text = ((int)cdInfo.currTime + 1).ToString();
                    }
                    return;
                }
            }

            ResetBtn();
            //     Log.LogGroup("ZDY","cdinfo is null skillid is " + m_skillid);
            TimerAxis.Instance().KillTimer(m_uSkillUseTimeID, this);


        }
    }
}
