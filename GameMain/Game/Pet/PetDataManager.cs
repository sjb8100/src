//=========================================================================================
//
//    Author: zhudianyu
//    宠物数据管理
//    CreateTime:  #CreateTime#
//
//=========================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using Engine.Utility;
using Engine;
using table;
using GameCmd;
//dolua AddPet(2)
public enum PetDispatchEventString
{
    ChangePet,//切换宠物
    ChangeFight,//切换出站状态
    ChangePetNum,//扩充宠物上限
    AddPet,//添加宠物
    DeletePet,//删除宠物
    ShowCountDown,//显示倒计时
    RefreshQuickSetting,//刷新petlistpanel快捷设置
    RefreshUserQuickSetting,//刷新四个快捷键设置
    LockSkill,//锁定技能刷新
    PetRefreshProp,//宠物属性刷新
    PetSkillInit,//宠物技能初始化
    PetGuiYuanSucess,//宠物归元成功
    RefreshCountDown,//刷新复活或者出战倒计时
    RefreshLineUPAutoFight,//刷新布阵第一位是否自动出站
    SelectPet,//点击选中宠物
    ResetInheritPanel,//重置创建面板

}
public enum PetGrowState
{
    Common = 1,
    Good = 2,
    Better = 3,
    Execllence = 4,
    Perfect = 5,
}
partial class PetDataManager : BaseModuleData, IManager, ITimer
{
    #region property
    Dictionary<uint, IPet> petDic = new Dictionary<uint, IPet>();
    public PetPanel PetUI
    {
        get
        {
            return DataManager.Manager<UIPanelManager>().GetPanel(PanelID.PetPanel) as PetPanel;
        }
    }
    uint curPetThisID;
    public uint CurPetThisID
    {
        get
        {
            return curPetThisID;
        }
        set
        {
            if (value != curPetThisID)
            {
                curPetThisID = value;
                ValueUpdateEventArgs args = new ValueUpdateEventArgs();
                args.key = PetDispatchEventString.ChangePet.ToString();
                DispatchValueUpdateEvent(args);
                InitPlanPoint();

            }
            ValueUpdateEventArgs eventArgs = new ValueUpdateEventArgs();
            eventArgs.key = PetDispatchEventString.SelectPet.ToString();
            DispatchValueUpdateEvent(eventArgs);
        }
    }

    IPet curPet;
    public IPet CurPet
    {
        get
        {
            return GetPetByThisID(curPetThisID);
        }
    }
    SkillDatabase skilldb;
    /// <summary>
    /// 当前选中的宠物技能
    /// </summary>
    public SkillDatabase SelectSkillDataBase
    {
        get
        {
            return skilldb;
        }
        set
        {
            skilldb = value;
            if (PetUI != null)
            {
                PetUI.InitSkillPanel();
            }
        }
    }
    private bool bLock = false;
    public bool bLockSkill
    {
        get
        {
            return bLock;
        }
        set
        {
            bLock = value;
            DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = PetDispatchEventString.PetSkillInit.ToString() });
        }
    }
    uint lockSkillNum = 0;
    public uint LockSkillNum
    {
        get
        {
            return lockSkillNum;
        }
        set
        {
            lockSkillNum = value;
            PetLearnSkill panel = DataManager.Manager<UIPanelManager>().GetPanel(PanelID.PetLearnSkill) as PetLearnSkill;
            if (panel != null)
            {
                panel.SetLockSkillNum(lockSkillNum);
            }
        }
    }
    uint curFightingPet = 0;
    /// <summary>
    /// 当前出站的宠物
    /// </summary>
    public uint CurFightingPet
    {
        get
        {
            return curFightingPet;
        }
        set
        {
            curFightingPet = value;
            // Log.Error("curFightingPet  is " + curFightingPet);

        }
    }
    /// <summary>
    /// 宠物复活cd
    /// </summary>
    Dictionary<uint, int> m_dicPetRelieveCountDown = new Dictionary<uint, int>();
    /// <summary>
    /// 宠物出站cd
    /// </summary>
    Dictionary<uint, int> m_dicPetFightCountDown = new Dictionary<uint, int>();
    /// <summary>
    /// 宠物和npc对应的关系
    /// key是petid
    /// value is npcid
    /// </summary>
    Dictionary<uint, uint> petToNpcDic = new Dictionary<uint, uint>();

    List<uint> m_petQuickList = new List<uint>();


    public bool IsHasPetFight
    {
        get
        {
            return isPetFight;
        }
        set
        {
            isPetFight = value;
            stShowPetSkill showPet = new stShowPetSkill();

            showPet.bShow = isPetFight;


            EventEngine.Instance().DispatchEvent((int)GameEventID.SKILL_SHOWPETSKILL, showPet);
            DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = PetDispatchEventString.ChangeFight.ToString() });

        }
    }
    bool isPetFight = false;
    public List<uint> PetQuickList
    {
        get
        {
            return m_petQuickList;
        }
    }
    #endregion

    #region cd
    public void SetPetReleveCDTime(uint petid, int time)
    {
        if (m_dicPetRelieveCountDown.ContainsKey(petid))
        {
            m_dicPetRelieveCountDown[petid] = time;
        }
        else
        {
            m_dicPetRelieveCountDown.Add(petid, time);
        }
    }
    public int GetRelieveCDTime(uint petid)
    {
        if (m_dicPetRelieveCountDown.ContainsKey(petid))
        {
            return m_dicPetRelieveCountDown[petid];
        }
        return 0;
    }
    public void SetPetFightCDTime(uint petid, int time)
    {
        // Log.Error("set cd  is ===========" + time + "  pet id is " + petid);
        if (m_dicPetFightCountDown.ContainsKey(petid))
        {


            m_dicPetFightCountDown[petid] = time;
        }
        else
        {
            m_dicPetFightCountDown.Add(petid, time);
        }
    }
    void DeletePetFightCd(uint petid)
    {
        if (m_dicPetFightCountDown.ContainsKey(petid))
        {
            m_dicPetFightCountDown.Remove(petid);
        }
    }
    public int GetPetFightCDTime(uint petid)
    {
        if (m_dicPetFightCountDown.ContainsKey(petid))
        {
            return m_dicPetFightCountDown[petid];
        }
        return 0;
    }
    #endregion
    #region npc
    public void AddPetToNpc(uint petid, uint npcid)
    {
        curFightingPet = petid;
        if (!petToNpcDic.ContainsKey(petid))
        {
            petToNpcDic.Add(petid, npcid);
        }
        else
        {
            petToNpcDic[petid] = npcid;
        }
    }
    public bool NpcIsPet(uint npcid)
    {
        foreach (var dic in petToNpcDic)
        {
            if (dic.Value == npcid)
            {
                return true;
            }
        }
        return false;
    }
    public uint GetPetIDByNpcID(uint npcid)
    {
        foreach (var dic in petToNpcDic)
        {
            if (dic.Value == npcid)
            {
                return dic.Key;
            }
        }
        return 0;
    }
    public uint GetNpcIDByPetID(uint petid)
    {
        if (petToNpcDic.ContainsKey(petid))
        {
            return petToNpcDic[petid];
        }
        return 0;
    }

    public INPC GetNpcByPetID(uint petID)
    {
        uint npcID = GetNpcIDByPetID(petID);
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return null;
        }
        INPC npc = es.FindNPC(npcID);
        return npc;
    }
    #endregion
    #region fighting
    void AddFightPet(uint petid)
    {
        //   if(CurFightingPet != petid)
        {
            CurFightingPet = petid;
            int fightCd = GameTableManager.Instance.GetGlobalConfig<int>("UsingPetCD");
            foreach (var dic in petDic)
            {
                SetPetFightCDTime(dic.Key, fightCd);
            }
            ShowTipsEnum(LocalTextType.Pet_Commond_zhanhunchuzhanchenggong);
        }


    }

    void DeleteFightPet(uint petid)
    {
        //DeletePetFightCd(petid);
        if (CurFightingPet != 0)
        {
            CurFightingPet = 0;
        }
    }
    public void OnAddFightPet(stUseFightPetUserCmd_CS cmd)
    {
        AddFightPet(cmd.id);
        IsHasPetFight = true;
        IPet pet = GetPetByThisID(cmd.id);
        if (pet != null)
        {
            uint petID = pet.PetBaseID;
            PetDataBase pdb = GameTableManager.Instance.GetTableItem<PetDataBase>(petID);
            if (pdb != null)
            {
                //PlayCreateAudio(pdb.fightAudio);
            }
        }
    }
    uint m_uAudioID = 0;
    public void PlayCreateAudio(uint audioID)
    {
   
        Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
        if (audio != null)
        {
            audio.StopEffect(m_uAudioID);
        }
        if (audioID == 0)
        {
            return;
        }
        table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(audioID);


        if (audio != null && resDB != null)
        {
            if (MainPlayerHelper.GetMainPlayer() != null)
            {
                if (MainPlayerHelper.GetMainPlayer().GetNode() != null)
                {
                    if (MainPlayerHelper.GetMainPlayer().GetTransForm() != null)
                    {
                        m_uAudioID = audio.PlayEffect(MainPlayerHelper.GetMainPlayer().GetNode().GetTransForm().gameObject, resDB.strPath,false,true);
                    }
                }
            }

        }


    }
    public void OnCallBackPet(stCallBackPetUserCmd_CS cmd)
    {
        DeleteFightPet(cmd.id);
        IsHasPetFight = false;
    }
    /// <summary>
    /// 判断当前宠物是否出站
    /// </summary>
    /// <returns></returns>
    public bool IsCurPetFighting()
    {
        if (CurPet != null)
        {
            if (CurPet.GetID() == CurFightingPet)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 出战宠物的属性
    /// </summary>
    Dictionary<uint, stFightPetAttrPetUserCmd_S> m_fightAttrDic = new Dictionary<uint, stFightPetAttrPetUserCmd_S>();
    public void OnFightPetAttr(stFightPetAttrPetUserCmd_S cmd)
    {
        if (CurFightingPet == 0)
        {
            Log.Error("CurFightingPet is 0");
            return;
        }
        if (m_fightAttrDic.ContainsKey(CurFightingPet))
        {
            m_fightAttrDic[CurFightingPet] = cmd;
        }
        else
        {
            m_fightAttrDic.Add(CurFightingPet, cmd);
        }
        UpdatePetProp();
    }
    public int GetFightPetAttr(int propID)
    {
        if (m_fightAttrDic.ContainsKey(CurFightingPet))
        {
            stFightPetAttrPetUserCmd_S cmd = m_fightAttrDic[CurFightingPet];
            if (propID == (int)CreatureProp.MaxHp)
            {
                return cmd.maxhp;
            }
            else if (propID == (int)FightCreatureProp.PhysicsAttack)
            {
                return cmd.pdam;
            }
            else if (propID == (int)FightCreatureProp.MagicAttack)
            {
                return cmd.mdam;
            }
            else if (propID == (int)FightCreatureProp.PhysicsDefend)
            {
                return cmd.pdef;
            }
            else if (propID == (int)FightCreatureProp.EleDefend)
            {
                return cmd.lightdef;
            }
            else if (propID == (int)FightCreatureProp.FireDefend)
            {
                return cmd.heatdef;
            }
            else if (propID == (int)FightCreatureProp.WitchDefend)
            {
                return cmd.wavedef;
            }
            else if (propID == (int)FightCreatureProp.IceDefend)
            {
                return cmd.biochdef;
            }
            else if (propID == (int)FightCreatureProp.Hit)
            {
                return cmd.hit;
            }
            else if (propID == (int)FightCreatureProp.Dodge)
            {
                return cmd.hide;
            }
            else if (propID == (int)FightCreatureProp.PhysicsCrit)
            {
                return cmd.plucky;
            }
            else if (propID == (int)FightCreatureProp.MagicCrit)
            {
                return cmd.mlucky;
            }
            else if (propID == (int)FightCreatureProp.MagicDefend)
            {
                return cmd.mdef;
            }
            else
            {
                FightCreatureProp prop = (FightCreatureProp)propID;
                Log.Error("no attr " + prop.ToString());
                return 0;
            }

        }
        else
        {
            //Log.Error("no data");
            return 0;
        }
    }
    #endregion
    public void SetSkillDatabase(SkillDatabase skill)
    {
        skilldb = skill;
    }
    public override void Destroy()
    {

    }
    #region petmanager
    /// <summary>
    /// 拥有过的宠物列表
    /// </summary>
    List<uint> m_listHasPossess = new List<uint>();
    public List<uint> HasPossessPetList
    {
        set
        {
            m_listHasPossess.Clear();
            m_listHasPossess = value;
        }
        get
        {
            return m_listHasPossess;
        }
    }
    public void AddHasPossessPet(uint petID)
    {
        if (!m_listHasPossess.Contains(petID))
        {
            m_listHasPossess.Add(petID);
        }
    }

    public Dictionary<uint, IPet> GetPetDic()
    {

        return petDic;
    }
    public void InitCurPet()
    {

        if (petDic != null)
        {
            List<IPet> petList = GetSortPetList();
            if (petList.Count > 0)
            {
                CurPetThisID = petList[0].GetID();
            }
        }

    }
    public List<IPet> GetSortPetList()
    {
        List<IPet> petList = new List<IPet>(GetPetDic().Values);
        if (petList == null)
        {
            return null;
        }
        petList.Sort((x1, x2) =>
        {
            if (x1 == null || x2 == null)
            {
                return 0;
            }
            int lv1 = x1.GetProp((int)CreatureProp.Level);
            int lv2 = x2.GetProp((int)CreatureProp.Level);
            if (lv1 < lv2)
            {
                return 1;
            }
            else if (lv1 > lv2)
            {
                return -1;
            }
            return 0;
        });
        int curPetIndex = 0;
        for (int i = 0; i < petList.Count; i++)
        {
            IPet pet = petList[i];
            if (pet.GetID() == CurFightingPet)
            {
                curPetIndex = i;
                break;
            }
        }
        if (curPetIndex < petList.Count)
        {
            IPet curTempPet = petList[curPetIndex];
            if (curTempPet != null)
            {
                petList.RemoveAt(curPetIndex);
                petList.Insert(0, curTempPet);
            }
        }
        return petList;
    }
    public void OnPetDead(uint npcid)
    {
        uint petID = npcid;//GetPetIDByNpcID(npcid);
        int time = GameTableManager.Instance.GetGlobalConfig<int>("PetReFightSec");
        SetPetReleveCDTime(petID, time);
        IsHasPetFight = false;
    }
    public void RemovePet(stRemovePetUserCmd_CS cmd)
    {
        uint petid = cmd.id;
        IPet pet = GetPetByThisID(petid);
        if (pet != null)
        {
            AddHasPossessPet(pet.PetBaseID);
        }
        string txt = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_fengyinzhanhun, GetPetName(pet));
        ChatDataManager.SendToChatSystem(txt);
        DeletePet(petid);
        DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = PetDispatchEventString.DeletePet.ToString() });
        RemoveFromLineUPList(petid);
        //         int key = 0;
        //         foreach (var dic in m_dicUserQuickSetting)
        //         {
        //             if (cmd.id == dic.Value)
        //             {
        //                 key = dic.Key;
        //                 break;
        //             }
        //         }
        //         SetUserQuickListByIndex(key, 0);
        //         SendQuickListMsg();
    }
    public void AddPet(uint petThisID, IPet pet)
    {
        if (petDic.ContainsKey(petThisID))
        {
            Log.Error("已经添加pet petid is" + petThisID);
        }
        else
        {
            petDic.Add(petThisID, pet);
            DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = PetDispatchEventString.AddPet.ToString() });
            if (petDic.Count == 1)
            {
                InitCurPet();
            }
        }
    }
    public void DeletePet(uint petThisID)
    {
        if (petDic.ContainsKey(petThisID))
        {
            petDic.Remove(petThisID);
            InitCurPet();
        }
        else
        {
            Log.Error("not cotains pet key is " + petThisID);
        }
    }
    public IPet GetPetByThisID(uint petThisID)
    {
        if (petThisID == 0)
        {
            List<uint> petList = new List<uint>(petDic.Keys);
            if (petList.Count > 0)
            {
                petThisID = petList[0];
            }
        }
        if (petDic.ContainsKey(petThisID))
        {
            return petDic[petThisID];
        }
        else
        {
            //  Log.LogGroup("ZDY", "GetPetByThisID not cotains pet key is " + petThisID);
            return null;
        }
    }
    #endregion

    #region 宠物合成
    List<uint> canMakeList = new List<uint>();
    public void AllCanMakePets(stYouCanExchangePetUserCmd_S cmd)
    {
        foreach (var id in cmd.pet)
        {
            if (!canMakeList.Contains(id))
            {
                canMakeList.Add(id);
            }
        }
    }
    List<PetDataBase> canMakePetDataBaseList = new List<PetDataBase>();
    public List<PetDataBase> GetAllCanMakePets()
    {
        if (canMakePetDataBaseList.Count != 0)
            return canMakePetDataBaseList;
        foreach (var id in canMakeList)
        {
            PetDataBase db = GetPetDataBase(id);
            if (db != null)
            {
                if (!canMakePetDataBaseList.Contains(db))
                {
                    canMakePetDataBaseList.Add(db);
                }
            }
        }
        return canMakePetDataBaseList;
    }

    /// <summary>
    /// 获取能合成的宠物
    /// </summary>
    public List<PetDataBase> GetCanComposePetList()
    {
        List<PetDataBase> canComposeList = new List<PetDataBase>();
        List<PetDataBase> dbList = GetAllCanMakePets();
        for (int i = 0; i < dbList.Count; i++)
        {
            var db = dbList[i];
            int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(db.fragmentID);
            uint needNum = db.fragmentNum;
            if (itemCount >= needNum)
            {
                canComposeList.Add(db);
            }
        }
        return canComposeList;
    }
    #endregion

    #region petprop
    public int GetPropByThisID(uint petThisID, PetProp propID)
    {
        IPet pet = GetPetByThisID(petThisID);
        if (pet != null)
        {
            return pet.GetProp((int)propID);
        }
        Log.Error("get pet prop failed propid is " + propID);
        return -1;
    }
    public void OnPetLevelUP(stLevelUpPetUserCmd_S cmd)
    {
        IPet pet = GetPetByThisID(cmd.id);
        if (pet != null)
        {
            pet.SetProp((int)CreatureProp.Level, cmd.lv);
            pet.SetProp((int)PetProp.LevelExp, cmd.exp);
            pet.SetProp((int)PetProp.MaxPoint, cmd.max_point);
            UpdatePetProp();
            string txt = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_zhanhunshengji, GetPetName(pet), cmd.lv);
            ChatDataManager.SendToChatSystem(txt);
            INPC npc = GetNpcByPetID(cmd.id);
            if (npc != null)
            {
                npc.SetProp((int)CreatureProp.Level, cmd.lv);
                stPropUpdate prop = new stPropUpdate();
                prop.uid = npc.GetUID();
                prop.nPropIndex = (int)CreatureProp.Level;
                prop.newValue = (int)cmd.lv;

                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_PROPUPDATE, prop);
            }

        }
    }
    public void OnGuiyuanReturn(stGuiYuanPetUserCmd_CS cmd)
    {
        if (cmd != null)
        {
            DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = PetDispatchEventString.PetGuiYuanSucess.ToString(), newValue = cmd.adv });
        }
    }
    public void OnChangeNpcName(stBroadCastNamePetUserCmd_S cmd)
    {
        if (cmd != null)
        {
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
                return;
            }
            INPC npc = es.FindNPC(cmd.id);
            if (npc != null)
            {
                npc.SendMessage(EntityMessage.EntityCommond_SetName, cmd.name);
            }
        }
    }
    public void OnPetGuiYuan(stTalentPetUserCmd_S cmd)
    {
        IPet pet = GetPetByThisID(cmd.id);
        if (pet != null)
        {
            ShowTipsEnum(LocalTextType.Pet_GuiYuan_guiyuanchenggong);
            //ShowTips(108510);
            SetPetTalent(cmd.talent, cmd.id);
            pet.SetProp((int)PetProp.PetGuiYuanStatus, cmd.guiyuanstatus);
            UpdatePetProp();
        }
    }
    public void OnPetYinHun(stYinHunPetUserCmd_CS cmd)
    {
        IPet pet = GetPetByThisID(cmd.id);
        if (pet != null)
        {
            int lv = pet.GetProp((int)PetProp.YinHunLevel);
            if (lv == cmd.yh_lv)
            {
                int oldExp = pet.GetProp((int)PetProp.YinHunExp);
                int delta = Math.Abs(cmd.yh_exp - oldExp);
                ShowTipsEnum(LocalTextType.Pet_YinHun_yinhunchenggonghuodelingqiX, delta);
                // TipsManager.Instance.ShowTipsById(108513, delta);
            }
            else
            {
                ShowTipsEnum(LocalTextType.Pet_YinHun_yinhundengjitisheng);
                //  TipsManager.Instance.ShowTipsById(108514);
            }
            pet.SetProp((int)PetProp.YinHunExp, cmd.yh_exp);
            pet.SetProp((int)PetProp.YinHunLevel, cmd.yh_lv);
            UpdatePetProp();
        }
    }
    public void OnAddPoint(stAttrPointPetUserCmd_S cmd)
    {
        IPet pet = GetPetByThisID(cmd.id);
        if (pet != null)
        {
            pet.SetFreeResetNum(cmd.free_reset_attr_time);
        }
        if (cmd.type != 0)
        {
            TipsManager.Instance.ShowTips(DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Notice_AllotPointOver));
            return;
        }
        if (cmd.type == 0)
        {
            TipsManager.Instance.ShowTips(DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Notice_ResetPointOver));
            return;
        }
    }
    void SetPetTalent(PetTalent pt, uint petid)
    {
        IPet pet = GetPetByThisID(petid);
        if (pet != null)
        {
            pet.SetProp((int)FightCreatureProp.Strength, (int)pt.liliang);
            pet.SetProp((int)FightCreatureProp.Corporeity, (int)pt.tizhi);
            pet.SetProp((int)FightCreatureProp.Intelligence, (int)pt.zhili);
            pet.SetProp((int)FightCreatureProp.Spirit, (int)pt.jingshen);
            pet.SetProp((int)FightCreatureProp.Agility, (int)pt.minjie);

        }
    }
    /// <summary>
    /// 对宠物使用物品加寿命
    /// </summary>
    /// <param name="cmd"></param>

    public void OnAddPetLife(stLifePetUserCmd_S cmd)
    {
        IPet pet = GetPetByThisID(cmd.id);
        if (pet != null)
        {
            string str = "";
            int curLife = pet.GetProp((int)PetProp.Life);
            if (curLife < cmd.life)
            {
                int life = cmd.life - curLife;
                str = "+" + life.ToString();
            }
            else
            {
                int life = curLife - cmd.life;
                str = ColorManager.GetColorString(ColorType.Red, (-life).ToString());
            }
            ShowTipsEnum(LocalTextType.Pet_Age_zhanhunshoumingzengjianleX, pet.GetName(), str);
            pet.SetProp((int)PetProp.Life, cmd.life);
        }
        DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = PetDispatchEventString.PetRefreshProp.ToString() });

    }
    /// <summary>
    /// 宠物当前经验 战斗中获得的经验不用这条消息 
    /// </summary>
    /// <param name="cmd"></param>

    public void OnPetCurExp(stExpPetUserCmd_S cmd)
    {
        IPet pet = GetPetByThisID(cmd.id);
        if (pet != null)
        {
            int oldExp = pet.GetProp((int)PetProp.LevelExp);
            pet.SetProp((int)PetProp.LevelExp, cmd.exp);
            int delta = cmd.exp - oldExp;
            if (delta > 0)
            {

                string txt = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_zhanghunhuodejingyan, GetPetName(pet), delta);
                ChatDataManager.SendToChatSystem(txt);
            }

        }
        DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = PetDispatchEventString.PetRefreshProp.ToString() });
    }
    void UpdatePetProp()
    {

        if (CurPet != null)
        {
            DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = PetDispatchEventString.PetRefreshProp.ToString() });
        }
    }
    public void RefreshPetProp()
    {
        UpdatePetProp();
    }
    public int GetCurPetLevel()
    {
        if (CurPet != null)
        {
            return CurPet.GetProp((int)CreatureProp.Level);
        }
        return -1;
    }
    public string GetPetLvelStr(uint petID)
    {
        string str = CommonData.GetLocalString("级");
        int lv = 0;
        IPet pet = GetPetByThisID(petID);
        if (pet != null)
        {
            lv = pet.GetProp((int)CreatureProp.Level);
        }
        if (lv != 0)
        {
            str = lv.ToString() + str;
        }
        return str;
    }
    public string GetCurPetLevelStr()
    {
        if (CurPet != null)
            return GetPetLvelStr(CurPet.GetID());
        return "";
    }
    public void OnRefreshPetAttr(stRefreshAttrPetUserCmd_S cmd)
    {
        IPet pet = GetPetByThisID(cmd.id);
        if (pet != null)
        {
            pet.SetProp((int)FightCreatureProp.PhysicsAttack, (int)cmd.attr.pdam);
            pet.SetProp((int)FightCreatureProp.PhysicsDefend, (int)cmd.attr.pdef);
            pet.SetProp((int)FightCreatureProp.MagicAttack, (int)cmd.attr.mdam);
            pet.SetProp((int)FightCreatureProp.PhysicsCrit, (int)cmd.attr.plucky);
            pet.SetProp((int)FightCreatureProp.MagicCrit, (int)cmd.attr.mlucky);
            pet.SetProp((int)FightCreatureProp.Hit, (int)cmd.attr.hit);
            pet.SetProp((int)FightCreatureProp.Dodge, (int)cmd.attr.hide);
            pet.SetProp((int)FightCreatureProp.FireDefend, (int)cmd.attr.heatdef);
            pet.SetProp((int)FightCreatureProp.IceDefend, (int)cmd.attr.biochdef);
            pet.SetProp((int)FightCreatureProp.EleDefend, (int)cmd.attr.lightdef);
            pet.SetProp((int)FightCreatureProp.WitchDefend, (int)cmd.attr.wavedef);
            pet.SetProp((int)FightCreatureProp.Power, (int)cmd.attr.fight_power);
            pet.SetProp((int)CreatureProp.MaxHp, (int)cmd.attr.maxhp);
        }
        DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = PetDispatchEventString.PetRefreshProp.ToString() });
    }
    public void OnFightPower(stFightPowerPetUserCmd_S cmd)
    {
        IPet pet = GetPetByThisID(cmd.id);
        if (pet != null)
        {
            pet.SetProp((int)FightCreatureProp.Power, (int)cmd.fight_power);
        }
        DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = PetDispatchEventString.PetRefreshProp.ToString() });
    }
    public void OnPetChangeName(stChangeNamePetUserCmd_CS cmd)
    {

        IPet pet = GetPetByThisID(cmd.id);
        if (pet != null)
        {
            // pet.SetName( cmd.name );
            pet.SendMessage(EntityMessage.EntityCommond_SetName, cmd.name);
        }
        RefreshPetProp();
    }
    public int maxPetNum = 0;
    public void OnPetMaxNum(stAddMaxNumPetUserCmd_CS cmd)
    {
        maxPetNum = cmd.max_pet;
        DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = PetDispatchEventString.ChangePetNum.ToString() });
    }
    public string GetCurPetName()
    {
        if (CurPet != null)
        {
            return GetPetName(CurPet);
        }
        return "";
    }
    public string GetPetName(IPet pet)
    {
        table.PetDataBase petdb = GetPetDataBase(pet.PetBaseID);
        if (petdb != null)
        {
            int status = pet.GetProp((int)PetProp.PetGuiYuanStatus);
            string name = pet.GetName();
            if (name.Length == 0)
            {
                name = petdb.petName;
            }
            if (status == 1)
            {
                return ColorManager.GetColorString(ColorType.JZRY_Whitle, name);
            }
            else if (status == 2)
            {
                return ColorManager.GetColorString(ColorType.JZRY_Green, name);
            }
            else if (status == 3)
            {
                return ColorManager.GetColorString(ColorType.JZRY_Blue, name);
            }
            else if (status == 4)
            {
                return ColorManager.GetColorString(ColorType.JZRY_Purple, name);
            }
            else if (status == 5)
            {
                return ColorManager.GetColorString(ColorType.JZRY_Oranger, name);
            }
        }
        return "";
    }
    public int GetCurPetType()
    {
        if (CurPet != null)
        {
            PetDataBase pb = GetPetDataBase(CurPet.PetBaseID);
            if (pb != null)
            {
                return (int)pb.petType;
            }
        }
        return -1;
    }
    public string GetPetCharacterStr(int character)
    {
        return GameTableManager.Instance.GetGlobalConfig<string>("PetCharacter", character.ToString());
    }
    public string GetCurPetStrType()
    {
        PetDataBase pb = GetPetDataBase(CurPet.PetBaseID);
        if (pb != null)
        {
            return pb.attackType;
        }
        return "";
    }
    string GetTypeStringByIndex(int type)
    {

        string conList = GameTableManager.Instance.GetGlobalConfig<string>("PetType", type.ToString());
        if (conList == null)
        {
            return "";
        }

        return conList;
    }
    public string GetPetStrType(PetDataBase db)
    {
        int type = GetPetType(db);

        return GetTypeStringByIndex(type);

    }
    public string GetPetAttackStrType(PetDataBase db)
    {
        //  int type = GetPetType(db);

        // return GetTypeStringByIndex(type);
        return db.attackType;
    }
    private int GetPetType(PetDataBase db)
    {
        if (db != null)
        {
            return (int)db.petType;
        }
        return -1;
    }
    public string GetCurPetGrowStatus()
    {
        if (CurPet != null)
        {
            int status = CurPet.GetProp((int)PetProp.PetGuiYuanStatus);
            string str = GetGrowStatus(status);
            return str;
        }
        return "";
    }

    public string GetGrowStatus(int status)
    {
        switch (status)
        {
            case 1:
                return CommonData.GetLocalString("普通");
                break;
            case 2:
                return CommonData.GetLocalString("优秀");
                break;
            case 3:
                return CommonData.GetLocalString("杰出");
                break;
            case 4:
                return CommonData.GetLocalString("卓越");
                break;
            case 5:
                return CommonData.GetLocalString("完美");
                break;
            default:
                return CommonData.GetLocalString("未知");
                break;
        }
    }

    int GetEnumByString(string prop)
    {
        int propid = -1;
        if (Enum.IsDefined(typeof(CreatureProp), prop))
        {
            CreatureProp pp = (CreatureProp)Enum.Parse(typeof(CreatureProp), prop);
            return (int)pp;
        }
        if (Enum.IsDefined(typeof(FightCreatureProp), prop))
        {
            FightCreatureProp pp = (FightCreatureProp)Enum.Parse(typeof(FightCreatureProp), prop);
            return (int)pp;
        }
        if (Enum.IsDefined(typeof(PetProp), prop))
        {
            PetProp pp = (PetProp)Enum.Parse(typeof(PetProp), prop);
            return (int)pp;
        }
        Log.Error("get enum error prop is " + prop);
        return propid;
    }
    public int GetPropByString(string prop)
    {
        if (prop == FightCreatureProp.Strength.ToString() ||
           prop == FightCreatureProp.Corporeity.ToString() ||
           prop == FightCreatureProp.Intelligence.ToString() ||
           prop == FightCreatureProp.Spirit.ToString() ||
           prop == FightCreatureProp.Agility.ToString())
        {
            return (int)GetCurPetBaseProp(prop);
        }
        int propid = GetEnumByString(prop);
        if (propid != -1)
        {
            return CurPet.GetProp(propid);
        }

        Log.Error("parse prop str error ,prop str is " + prop);
        return -1;
    }
    public int GetCurPetBaseProp(FightCreatureProp prop)
    {
        if (CurPet != null)
        {
            return (int)GetCurPetBaseProp(prop.ToString());
        }
        return 0;
    }
    /// <summary>
    /// 获取基础属性
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="isAddPointUI">是不是加点界面，加点界面不算增加的天赋值是最基本的属性</param>
    /// <returns></returns>
    public uint GetCurPetBaseProp(string prop, bool isAddPointUI = false)
    {
        if (CurPet != null)
        {
            return GetBaseProp(prop, (int)CurPet.PetBaseID, (uint)GetCurPetLevel(), isAddPointUI);
        }
        return 0;
    }
    public uint GetBaseProp(string prop, int petBaseID, uint level, bool isAddPointUI = false)
    {
        PetUpGradeDataBase db = GetPetUpGradeDataBase(level, petBaseID);
        if (db != null)
        {
            PetTalent tal = CurPet.GetAttrTalent();
            if (prop == FightCreatureProp.Strength.ToString())
            {
                if (isAddPointUI)
                {
                    return db.power;
                }
                return db.power + tal.liliang;
            }
            else if (prop == FightCreatureProp.Corporeity.ToString())
            {
                if (isAddPointUI)
                {
                    return db.tizhi;
                }
                return db.tizhi + tal.tizhi;
            }
            else if (prop == FightCreatureProp.Intelligence.ToString())
            {
                if (isAddPointUI)
                {
                    return db.zhili;
                }
                return db.zhili + tal.zhili;
            }
            else if (prop == FightCreatureProp.Spirit.ToString())
            {
                if (isAddPointUI)
                {
                    return db.jingshen;
                }
                return db.jingshen + tal.jingshen;
            }
            else if (prop == FightCreatureProp.Agility.ToString())
            {
                if (isAddPointUI)
                {
                    return db.minjie;
                }
                return db.minjie + tal.minjie;
            }
        }
        return 0;
    }
    #endregion

    #region skill
    public void OnLockPetSkill(stLockSkillPetUserCmd_CS cmd)
    {
        IPet pet = GetPetByThisID(cmd.id);
        if (pet != null)
        {
            pet.SetPetSkillLock((int)cmd.skill, cmd.@lock);
        }
        DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = PetDispatchEventString.LockSkill.ToString(), newValue = cmd });
    }
    public uint GetSkillBookID(uint skillID, out int num)
    {
        SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID, 1);
        string itemArray = db.needItemArray;
        string[] itemIDArray = itemArray.Split(';');
        if (itemIDArray.Length < 1)
        {
            num = 0;
            return 0;
        }
        string[] itemNum = itemIDArray[0].Split('_');
        if (itemNum.Length < 2)
        {
            num = 0;
            return 0;
        }
        uint itemID = uint.Parse(itemNum[0]);
        num = int.Parse(itemNum[1]);
        return itemID;
    }
    public void OnLearnSkill(stLearnSkillPetUserCmd_CS cmd)
    {
        IPet pet = GetPetByThisID(cmd.id);
        if (pet != null)
        {
            ShowTipsEnum(LocalTextType.Pet_Skill_jinengxuexichenggong);
            // ShowTips(108507);
            pet.AddPetSkill(cmd.skill, cmd.skill_idx);
        }
        DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = PetDispatchEventString.PetSkillInit.ToString() });
        if (CurFightingPet != 0)
        {
            stShowPetSkill showPet = new stShowPetSkill();
            showPet.bShow = true;
            EventEngine.Instance().DispatchEvent((int)GameEventID.SKILL_SHOWPETSKILL, showPet);
        }
        stPetGetSkill ps = new stPetGetSkill();
        ps.petID = cmd.id;
        ps.skillID = (uint)cmd.skill;
        EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSTEM_PETGETSKILL, ps);
    }
    public void DeletePetSkill(stRemoveSkillPetUserCmd_S cmd)
    {

        IPet pet = GetPetByThisID(cmd.id);
        if (pet != null)
        {
            pet.DeletePetSkill(cmd.skill);
        }
        DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = PetDispatchEventString.PetRefreshProp.ToString() });
    }
    public void OnSkillUpGrade(stUpSkillPetUserCmd_CS cmd)
    {
        IPet pet = GetPetByThisID(cmd.id);
        if (pet != null)
        {
            ShowTipsEnum(LocalTextType.Pet_Skill_jinengshengjichenggong);

            // ShowTips(108508);
            pet.PetSkillUpGrade(cmd.skill);
        }
        DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = PetDispatchEventString.PetRefreshProp.ToString() });
    }
    public PetSkillObj GetPetSkillInfo(uint petID, uint skillID)
    {
        IPet pet = GetPetByThisID(petID);
        if (pet == null)
        {
            return null;
        }
        List<PetSkillObj> list = pet.GetPetSkillList();
        for (int i = 0; i < list.Count; i++)
        {
            if (skillID == list[i].id)
            {
                return list[i];
            }
        }
        return null;
    }
    public PetSkillObj GetCurPetSkillInfo(uint skillID)
    {
        if (CurPet == null)
        {
            return null;
        }
        return GetPetSkillInfo(CurPet.GetID(), skillID);

    }
    #endregion

    #region rendertexture
    Dictionary<uint, IRenerTextureObj> m_dicRTObjs = new Dictionary<uint, IRenerTextureObj>();
    public void AddRenderObj(uint id, IRenerTextureObj obj)
    {
        if (!m_dicRTObjs.ContainsKey(id))
        {
            m_dicRTObjs.Add(id, obj);
        }
    }
    public void ReleaseRenderObj()
    {
        Dictionary<uint, IRenerTextureObj>.Enumerator iter = m_dicRTObjs.GetEnumerator();
        while (iter.MoveNext())
        {
            if (iter.Current.Value != null)
            {
                iter.Current.Value.Release();
            }
        }

        m_dicRTObjs.Clear();
    }

    #endregion


    public uint GetItemNeedColdNum(uint itemID)
    {
        uint num = 0;
        PointConsumeDataBase db = GameTableManager.Instance.GetTableItem<PointConsumeDataBase>(itemID);
        if (db != null)
        {
            return db.buyPrice;
        }
        return num;
    }
    /// <summary>
    /// 获取返回需要消耗的字符串
    /// </summary>
    /// <param name="itemID">道具id</param>
    /// <param name="needStr">返回的字符串</param>
    /// <returns>有足够的数量返回true</returns>
    public bool GetItemNeedColdString(uint itemID, ref string needStr, uint num = 1)
    {
        int totalCold = UserData.Cold;
        uint needCold = GetItemNeedColdNum(itemID);
        needStr = StringUtil.GetNumNeedString(totalCold, needCold * num);
        if (totalCold < needCold)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    public string GetSkillTypeStr(PetSkillType type)
    {
        string str = "";
        if (type == PetSkillType.Auto)
        {
            str = CommonData.GetLocalString("自动");
        }
        else if (type == PetSkillType.Initiative)
        {
            str = CommonData.GetLocalString("主动");
        }
        else if (type == PetSkillType.Passive)
        {
            str = CommonData.GetLocalString("被动");
        }
        else if (type == PetSkillType.Recommend)
        {
            str = CommonData.GetLocalString("推荐");
        }
        str = str + CommonData.GetLocalString("技能");
        return str;
    }
    #region tabledatabase
    public PetDataBase GetPetDataBase(uint index)
    {
        return GameTableManager.Instance.GetTableItem<PetDataBase>(index);
    }
    public PetGuiYuanDataBase GetPetGuiYuanDataBase(uint quality, int grawstatus)
    {
        return GameTableManager.Instance.GetTableItem<PetGuiYuanDataBase>(quality, grawstatus);
    }

    public PetYinHunDataBase GetPetYinHunDataBase(uint index)
    {
        return GameTableManager.Instance.GetTableItem<PetYinHunDataBase>(index);
    }
    public PetUpGradeDataBase GetPetUpGradeDataBase(uint level, int type)
    {
        return GameTableManager.Instance.GetTableItem<PetUpGradeDataBase>(level, type);
    }
    public PetSkillLearnDataBase GetPetSkillLearnDataBase(uint index)
    {
        return GameTableManager.Instance.GetTableItem<PetSkillLearnDataBase>(index);
    }

    #endregion
    //public void ShowTips(uint error, params object[] args)
    //{
    //    TipsManager.Instance.ShowTipsById(error, args);
    //}
    public void ShowTipsEnum(LocalTextType error, params object[] args)
    {
        TipsManager.Instance.ShowLocalFormatTips(error, args);
        // TipsManager.Instance.ShowTipsById(error, args);
    }
    void SkillEvent(int eventID, object param)
    {
        if (ClientGlobal.Instance().MainPlayer == null)
        {
            return;
        }
        long userUID = ClientGlobal.Instance().MainPlayer.GetUID();
        if (eventID == (int)GameEventID.ENTITYSYSTEM_ENTITYDEAD)
        {
            stEntityDead ed = (stEntityDead)param;
            if (ed.uid == userUID)
            {
                DeleteFightPet(0);
            }
            if (CurPet != null)
            {

                INPC npc = GetNpcByPetID(CurPet.GetID());
                if (npc != null)
                {
                    if (ed.uid == npc.GetUID())
                    {
                        DeleteFightPet(0);
                    }
                }

            }

        }
    }
    public string GetJieBianString(int lv,bool needStr = true)
    {
        string str = needStr ? CommonData.GetLocalString("劫变") : "";
        string temp = "";
        lv = lv - 1;
        switch (lv)
        {
            case 1:
                temp = CommonData.GetLocalString("一");
                break;
            case 2:
                temp = CommonData.GetLocalString("二");
                break;
            case 3:
                temp = CommonData.GetLocalString("三");
                break;
            case 4:
                temp = CommonData.GetLocalString("四");
                break;
            case 5:
                temp = CommonData.GetLocalString("五");
                break;
            case 6:
                temp = CommonData.GetLocalString("六");
                break;
            case 7:
                temp = CommonData.GetLocalString("七");
                break;
            default:
                temp = CommonData.GetLocalString("无");
                break;

        }
        return str + temp;
    }

    private readonly uint m_uPetTimerID = 100;
    #region interface
    public void Initialize()
    {
        EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYDEAD, SkillEvent);
        TimerAxis.Instance().SetTimer(m_uPetTimerID, 1000, this);
    }

    public void Reset(bool depthClearData = false)
    {
        ReleaseRenderObj();
        petDic.Clear();
        curPetThisID = 0;
        curPet = null;
        skilldb = null;
        lockSkillNum = 0;
        curFightingPet = 0;
        petToNpcDic.Clear();
        m_petQuickList.Clear();

    }

    public void Process(float deltaTime)
    {

    }
    public void ClearData()
    {

    }
    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == m_uPetTimerID)
        {
            List<uint> keyList = m_dicPetRelieveCountDown.Keys.ToList<uint>();


            for (int i = 0; i < keyList.Count; ++i)
            {
                int time = m_dicPetRelieveCountDown[keyList[i]];
                if (time > 0)
                {
                    m_dicPetRelieveCountDown[keyList[i]] = time - 1;
                    //  DispatchValueUpdateEvent(PetDispatchEventString.RefreshCountDown.ToString(), null, null);
                }

            }

            List<uint> fightKeyList = m_dicPetFightCountDown.Keys.ToList<uint>();


            for (int i = 0; i < fightKeyList.Count; ++i)
            {
                int fightcd = m_dicPetFightCountDown[fightKeyList[i]];
                if (GetRelieveCDTime(fightKeyList[i]) > 0)
                {
                    break;
                }
                if (fightcd > 0)
                {
                    // Log.Error("fightcd is ===========" + fightcd);
                    m_dicPetFightCountDown[fightKeyList[i]] = fightcd - 1;
                    DispatchValueUpdateEvent(PetDispatchEventString.RefreshCountDown.ToString(), null, null);
                }
            }

        }
    }
    #endregion
}

