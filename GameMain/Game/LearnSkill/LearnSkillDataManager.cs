using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using GameCmd;
using Client;
using Engine.Utility;
using UnityEngine;
class SkillInfo
{
    public uint skillID;
    public uint level;
    public uint coldTime;
    public SkillInfo(uint skillID, uint level, uint coldTime)
    {
        this.skillID = skillID;
        this.level = level;
        this.coldTime = coldTime;
    }
}
enum LearnSkillDispatchEvents
{
    SkillLevelUP,//技能升级
    SkillAutoFightSet,//挂机设置
}
public enum SkillLevelUpCode
{
    None,
    Ok,//可以升级
    NoneData,//数据错误
    NoSkill,//没有技能
    NoMoney,//没有钱
    LevelNotEnough,//等级不合法

}
class LearnSkillDataManager : BaseModuleData, IManager
{
    /*
 通用：0
战士：1－蓄势  11－磐石
法师：3－独行  13－群攻
幻师：2－治疗  12－幻化
暗巫：4－召唤  14－诅咒
 */
    /// <summary>
    /// 当前职业所有的基础数据
    /// </summary>
    public List<SkillDatabase> allBaseSkillDataBase = new List<SkillDatabase>();

    List<SkillDatabase> unLockSkillList = new List<SkillDatabase>();

    List<SkillDatabase> lockSkillList = new List<SkillDatabase>();
    /*       技能面板对应位置
     *        状态一              状态二
     *           4                      8
     *        3                     7
     *     2                     6
     *  1        0             5        0
     */
    Dictionary<string, int> stateOneLocationDic = new Dictionary<string, int>();
    Dictionary<string, int> stateTwoLocationDic = new Dictionary<string, int>();
    /// <summary>
    /// 记录面板对应的位置信息 名字 和索引
    /// </summary>
    public Dictionary<string, int> OneLocationDic
    {
        get
        {
            return stateOneLocationDic;
        }
    }

    public Dictionary<string, int> TwoLocationDic
    {
        get
        {
            return stateTwoLocationDic;
        }
    }
    /// <summary>
    /// key is location value is skillid  已经设置过的技能
    /// </summary>
    SortedDictionary<int, uint> stateOneDic = new SortedDictionary<int, uint>();
    SortedDictionary<int, uint> stateTwoDic = new SortedDictionary<int, uint>();
    /// <summary>
    /// 已经拥有的技能信息
    /// </summary>
    Dictionary<uint, SkillInfo> userSkilldic = new Dictionary<uint, SkillInfo>();

    public Dictionary<uint, SkillInfo> UserSkilldic
    {
        get
        {
            return userSkilldic;
        }
    }

    public bool bAutoAttack = false;

    SkillSettingState curState = SkillSettingState.None;
    public SkillSettingState CurState
    {
        get
        {
            return curState;
        }
        set
        {
            if (curState != value)
            {
                curState = value;
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SKILLSYSTEM_SKILLSTATECHANE, (int)curState);
            }

        }
    }


    /// <summary>
    /// 技能设置面板展示的状态
    /// </summary>
    SkillSettingState showState = SkillSettingState.StateOne;
    public SkillSettingState ShowState
    {
        get
        {
            return showState;
        }
        set
        {
            showState = value;
        }
    }

    public LearnSkillPanel LearnUI
    {
        get
        {
            return DataManager.Manager<UIPanelManager>().GetPanel(PanelID.LearnSkillPanel) as LearnSkillPanel;
        }
    }

    public bool IsSettingPanel = false;

    bool m_bShowPetSkill = false;
    public bool bShowPetSkill
    {
        get
        {
            return m_bShowPetSkill;
        }
        set
        {
            m_bShowPetSkill = value;
        }
    }

    #region IManager
    public void ClearData()
    {
        allBaseSkillDataBase.Clear();
        unLockSkillList.Clear();
        lockSkillList.Clear();
        stateOneDic.Clear();
        stateTwoDic.Clear();
        stateOneLocationDic.Clear();
        stateTwoLocationDic.Clear();
        OneLocationDic.Clear();
        TwoLocationDic.Clear();
        userSkilldic.Clear();
        ShowState = SkillSettingState.StateOne;
        CurState = SkillSettingState.StateOne;
        IsSettingPanel = false;

        RegisterEvent(false);
    }
    public void Initialize()
    {
        RegisterEvent(true);
        curState = SkillSettingState.StateOne;
        showState = SkillSettingState.StateOne;

    }
    public void Reset(bool depthClearData = false)
    {
        RegisterEvent(true);
        InitData();
    }
    void RegisterEvent(bool bReg)
    {
        if (bReg)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
            EventEngine.Instance().AddEventListener((int)GameEventID.SKILL_SHOWPETSKILL, OnEvent);
            EventEngine.Instance().AddEventListener((int)GameEventID.SYSTEM_GAME_READY, OnEvent);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILL_SHOWPETSKILL, OnEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.SYSTEM_GAME_READY, OnEvent);
        }
    }
    public void Process(float deltaTime)
    {

    }
    #endregion
    /// <summary>
    /// 获取我拥有的技能列表
    /// </summary>
    /// <returns></returns>
    public List<SkillInfo> GetMySkillInfoList()
    {
        if (userSkilldic != null)
        {
            List<SkillInfo> list = new List<SkillInfo>();
            list.AddRange(userSkilldic.Values);
            return list;
        }
        return null;
    }
    void OnEvent(int eventID, object param)
    {
        switch ((Client.GameEventID)eventID)
        {
            case GameEventID.ENTITYSYSTEM_PROPUPDATE:
                {
                    stPropUpdate prop = (stPropUpdate)param;
                    if (prop.uid != MainPlayerHelper.GetPlayerUID())
                    {
                        return;
                    }
                    if (prop.nPropIndex != (int)CreatureProp.Level)
                        return;

                    DispatchRedPoingMsg();
                    AutoSetSkill();
                    int needLevel = GameTableManager.Instance.GetGlobalConfig<int>("ChangeStateLevel");
                    int level = GetPlayerLevel();

                    if (level >= needLevel)
                    {
                        // CurState = SkillSettingState.StateOne;
                        SetCurStateSkillList();
                        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eSkillBtnRefresh, null);

                    }
                }
                break;
            case GameEventID.SYSTEM_GAME_READY:
            case GameEventID.HEARTSKILLGODDATA:
                {
                    DispatchRedPoingMsg();
                }
                break;
            case GameEventID.SKILL_SHOWPETSKILL:
                {
                    stShowPetSkill st = (stShowPetSkill)param;
                    m_bShowPetSkill = st.bShow;
                    DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eSkillShowPetSkill, null);
                }
                break;
        }
    }
    /// <summary>
    /// 技能只判断等级
    /// </summary>
    public void DispatchRedPoingMsg()
    {
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint();
        st.modelID = (int)WarningEnum.LearnSkill;
        st.direction = (int)WarningDirection.Left;

        bool haveHeartSkillUpgrade = DataManager.Manager<HeartSkillManager>().HaveHeartSkillEnableUpgrade();

        if (haveHeartSkillUpgrade || HaveSkillUpgrade())
        {
            st.bShowRed = true;
            EventEngine.Instance().DispatchEvent((int)GameEventID.MAINPANEL_SHOWREDWARING, st);
        }
        else
        {
            st.bShowRed = false;
            EventEngine.Instance().DispatchEvent((int)GameEventID.MAINPANEL_SHOWREDWARING, st);
        }
    }
    /// <summary>
    /// 根据服务器消息初始化用户技能信息
    /// </summary>
    /// <param name="skillID">技能baseid</param>
    /// <param name="level">技能等级</param>
    /// <param name="coldTime">冷却时间</param>
    /// <param name="bHook">是否可以设置挂机</param>
    public void InitUserSkill(uint skillID, uint level, uint coldTime, uint bHook)
    {
        SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID, (int)level);
        if (db == null)
        {
            return;
        }
        if (db.dwType != 1)
        {//只处理主角技能 不处理其他技能（氏族 宠物）
            return;
        }
        SkillInfo info = new SkillInfo(skillID, level, coldTime);
        if (!userSkilldic.ContainsKey(skillID))
        {
            userSkilldic.Add(skillID, info);
            DispatchValueUpdateEvent(new ValueUpdateEventArgs()
            {
                key = LearnSkillDispatchEvents.SkillLevelUP.ToString(),
                oldValue = null,
                newValue = info,
            });
        }
        else
        {
            SkillInfo oldInfo = userSkilldic[skillID];
            uint oldLevel = oldInfo.level;
            userSkilldic[skillID] = info;
            if (oldLevel != level)
            {
                DispatchValueUpdateEvent(new ValueUpdateEventArgs()
                {
                    key = LearnSkillDispatchEvents.SkillLevelUP.ToString(),
                    oldValue = null,
                    newValue = info,

                });
            }
        }


        if (db != null)
        {
            if (LearnUI != null)
            {

                LearnUI.CurDataBase = db;
            }
            ///设置挂机状态
            if (!db.isnormal)
            {
                SetHookSkillStatus(skillID, bHook);
                SetClientHookSkillStatus(skillID, bHook);
            }
        }


    }
    public List<uint> GetUnLockLevelList()
    {
        List<uint> list = new List<uint>();
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
        {
            return null;
        }
        int job = mainPlayer.GetProp((int)PlayerProp.Job);
        int level = mainPlayer.GetProp((int)CreatureProp.Level);
        List<SkillDatabase> skillList = GameTableManager.Instance.GetTableList<SkillDatabase>();
        skillList = skillList.FindAll(P => P.dwJob == job && !P.isnormal && P.wdLevel == 1);

        for (int i = 0; i < skillList.Count; i++)
        {
            var skill = skillList[i];
            list.Add(skill.dwNeedLevel);
        }
        list.Sort((x1, x2) =>
        {
            if (x1 > x2)
            {
                return 1;
            }
            else if (x1 < x2)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        });
        return list;
    }
    /// <summary>
    /// 获取人物所有0级技能 除普攻
    /// </summary>
    /// <returns></returns>
    public List<SkillDatabase> GetRoleSkillList()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        int job = mainPlayer.GetProp((int)PlayerProp.Job);
        int level = mainPlayer.GetProp((int)CreatureProp.Level);
        List<SkillDatabase> skillList = GameTableManager.Instance.GetTableList<SkillDatabase>();
        List<SkillDatabase> tempList = skillList.FindAll(P => P.dwJob == job && !P.IsBasic && P.wdLevel == 1);
        tempList.Sort((x1, x2) =>
            {
                if (x1.dwNeedLevel > x2.dwNeedLevel)
                {
                    return 1;
                }
                else if (x1.dwNeedLevel < x2.dwNeedLevel)
                {
                    return -1;
                }
                return 0;
            });
        return tempList;
    }
    /// <summary>
    /// 升级后自动设置技能
    /// </summary>
    void AutoSetSkill()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        int job = mainPlayer.GetProp((int)PlayerProp.Job);
        int level = mainPlayer.GetProp((int)CreatureProp.Level);
        List<SkillDatabase> list = GetRoleSkillList();

        SkillDatabase db = list.Find((x) => { return x.dwNeedLevel == level && x.wdLevel == 1; });
        if (db != null)
        {
            int loc = GameTableManager.Instance.GetGlobalConfig<int>("UnlockSkillLocation", level.ToString());
            if (loc != 0)
            {
                GameCmd.stSetUsePosSkillUserCmd_CS cmd = new stSetUsePosSkillUserCmd_CS();
                cmd.index = (uint)loc;
                cmd.skillid = db.wdID;
                cmd.status = (uint)1;
                NetService.Instance.Send(cmd);
            }
        }


    }
    /// <summary>
    /// 初始化表格数据
    /// </summary>
    public void InitData()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
        {
            return;
        }
        int job = mainPlayer.GetProp((int)PlayerProp.Job);
        int level = mainPlayer.GetProp((int)CreatureProp.Level);
        List<SkillDatabase> skillList = GameTableManager.Instance.GetTableList<SkillDatabase>();
        allBaseSkillDataBase = skillList.FindAll(P => P.dwJob == job);

        for (int i = 0; i < 5; i++)
        {
            string name = "skill_" + i.ToString();
            if (!stateOneLocationDic.ContainsKey(name))
            {
                stateOneLocationDic.Add(name, i);
            }
            if (!stateTwoLocationDic.ContainsKey(name))
            {
                stateTwoLocationDic.Add(name, i == 0 ? 0 : i + 4);
            }

        }
        InitSkillID();

    }

    void InitSkillID()
    {
        lockSkillList.Clear();
        unLockSkillList.Clear();
        int level = GetPlayerLevel();

        for (int i = 0; i < allBaseSkillDataBase.Count; i++)
        {//isnormal表示是否普攻
            var skill = allBaseSkillDataBase[i];
            if (!skill.isnormal && skill.wdLevel == 0)
            {
                if (skill.dwNeedLevel <= level)
                {
                    AddUnLockSkill(skill);
                }
                else
                {
                    AddLockSkill(skill);
                }
            }
        }

    }
    public void OnAutoAttack(stSetAutoFlagSkillUserCmd_CS cmd)
    {
        if (cmd.ret == (uint)SkillRet.SkillRet_Success)
        {
            bAutoAttack = cmd.flag == 1 ? true : false;
        }
        else
        {
            Log.Error("设置自动攻击失败 error code is " + cmd.ret);
        }
    }
    public void OnSetSkillState(stSendSkillStatusSkillUserCmd_S cmd)
    {
        if (MainPlayerHelper.IsMainPlayer(cmd.userid))
        {
            CurState = cmd.status == 0 ? SkillSettingState.StateOne : SkillSettingState.StateTwo;

            if (MainPlayerHelper.GetMainPlayer() != null)
            {
                MainPlayerHelper.GetMainPlayer().SetProp((int)PlayerProp.SkillStatus, (int)CurState);
            }
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eSkillChangeState, null);
        }
        else
        {
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                return;
            }
            IPlayer otherPlayer = es.FindEntity<IPlayer>(cmd.userid);
            if (otherPlayer != null)
            {
                otherPlayer.SetProp((int)PlayerProp.SkillStatus, (int)(cmd.status + 1));
            }
        }
    }
    /// <summary>
    /// 根据服务器信息 初始化设置面板
    /// </summary>
    /// <param name="cmd"></param>
    public void OnInitLocation(stSendUsePosListSkillUserCmd_S cmd)
    {
        List<SkillUsePos> posList = cmd.status_pos;
        //  if (CurState == SkillSettingState.None)
        {
            CurState = cmd.cur_status == 0 ? SkillSettingState.StateOne : SkillSettingState.StateTwo;

        }


        for (int i = 0; i < posList.Count; i++)
        {
            var userpos = posList[i];
            uint status = userpos.status;
            if (userpos.status == (uint)SkillSettingState.StateOne)
            {
                stateOneDic.Clear();

                for (int j = 0; j < userpos.skill_pos.Count; j++)
                {
                    var skillpos = userpos.skill_pos[j];
                    uint totalid = skillpos.skillid;
                    //ushort skillID = (ushort)( totalid >> 16 );
                    //ushort level = (ushort)totalid;
                    if (!stateOneDic.ContainsKey((int)skillpos.index))
                    {
                        stateOneDic.Add((int)skillpos.index, totalid);

                    }
                    else
                    {
                        stateOneDic[(int)skillpos.index] = totalid;
                    }
                }
            }
            else if (userpos.status == (uint)SkillSettingState.StateTwo)
            {
                stateTwoDic.Clear();
                for (int j = 0; j < userpos.skill_pos.Count; j++)
                {
                    var skillpos = userpos.skill_pos[j];
                    uint totalid = skillpos.skillid;
                    //ushort skillID = (ushort)( totalid >> 16 );
                    //ushort level = (ushort)totalid;
                    if (!stateTwoDic.ContainsKey((int)skillpos.index))
                    {
                        stateTwoDic.Add((int)skillpos.index, totalid);
                    }
                    else
                    {
                        stateTwoDic[(int)skillpos.index] = totalid;
                    }
                }
            }
        }
        SetCurStateSkillList();
        SetAllSettingItem();
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eSkillBtnRefresh, null);
    }

    public void SetCurStateSkillList()
    {
        SortedDictionary<int, uint> tempDic = null;
        if (CurState == SkillSettingState.StateOne)
        {
            tempDic = stateOneDic;
        }
        else if (CurState == SkillSettingState.StateTwo)
        {
            tempDic = stateTwoDic;
        }
        List<uint> skillIDList = new List<uint>();

        var iter = tempDic.GetEnumerator();
        while (iter.MoveNext())
        {
            var dic = iter.Current;
            if (dic.Value != 0)
            {
                if (m_autoFightSkillStatus.ContainsKey(dic.Value))
                {
                    if (m_autoFightSkillStatus[dic.Value] == 1)
                    {
                        skillIDList.Add(dic.Value);
                    }
                }
            }
        }
        //  skillIDList.Reverse();
        uint commonSkillID = GetCommonSkillIDByJob();
        skillIDList.Insert(0, commonSkillID);
        ISkillPart skillPart = Protocol.SkillHelper.GetMainPlayerSkillPart();
        if (skillPart != null)
        {
            skillPart.SetCurStateList(skillIDList);
        }

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SKILLSYSTEM_SKILLLISTCHANE, (int)CurState);
    }
    #region 读取globalconst
    /// <summary>
    /// 存取两种状态下的普攻
    /// </summary>
    Dictionary<uint, uint> m_commonSKillDic = new Dictionary<uint, uint>();
    /// <summary>
    /// 存取两种状态下的切换技能
    /// </summary>
    Dictionary<uint, uint> m_changeSkillDic = new Dictionary<uint, uint>();
    public uint GetCommonSkillIDByJob()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        uint job = 1;
        if (mainPlayer != null)
        {
            job = (uint)mainPlayer.GetProp((int)PlayerProp.Job);
        }
        else
        {
            Log.Error("获取普攻时 主角尚未初始化");
            return 0;
        }
        uint skillID = 0;
        job = (int)CurState == 1 ? job : job + 10;
        if (!m_commonSKillDic.TryGetValue(job, out skillID))
        {
            skillID = GameTableManager.Instance.GetGlobalConfig<uint>("CommonSkillID", job.ToString());
            m_commonSKillDic.Add(job, skillID);
        }


        return skillID;
    }

    public uint GetCurStateSkillIDByJob()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        uint job = 1;
        if (mainPlayer != null)
        {
            job = (uint)mainPlayer.GetProp((int)PlayerProp.Job);
        }
        uint skillID = 0;
        job = (int)CurState == 1 ? job : job + 10;
        if (!m_changeSkillDic.TryGetValue(job, out skillID))
        {
            skillID = GameTableManager.Instance.GetGlobalConfig<uint>("ChangeStateSkillID", job.ToString());
            m_changeSkillDic.Add(job, skillID);
        }
        return skillID;
    }
    #endregion
    public void OnSetLocation(stSetUsePosSkillUserCmd_CS cmd)
    {
        if (cmd.ret == (uint)SkillRet.SkillRet_Success)
        {
            //Log.Info("技能设置成功 error code is " + cmd.ret);
            stSetSkillPos sp = new stSetSkillPos();
            sp.pos = cmd.index;
            sp.skillid = cmd.skillid;
            uint level = GetUnLockLevelByLoc((uint)cmd.index);
            if (MainPlayerHelper.GetPlayerLevel() > level)
            {
                DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eSkillBtnRefresh, null);
            }
            else
            {
                EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSTEM_SETSKILLPOS, sp);
            }

        }
        else
        {
            SetAllSettingItem();
            Log.Error("技能设置错误 error code is " + cmd.ret);
        }

    }
    /// <summary>
    /// 是否已经拖到设置面板
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool IsSetLocation(int location)
    {
        SortedDictionary<int, uint> tempDic = null;
        if (ShowState == SkillSettingState.StateOne)
        {
            tempDic = stateOneDic;
        }
        else if (ShowState == SkillSettingState.StateTwo)
        {
            tempDic = stateTwoDic;
        }
        return tempDic.ContainsKey(location);
    }
    /// <summary>
    /// 通过位置获取解锁技能等级
    /// </summary>
    /// <param name="loc"></param>
    /// <returns></returns>
    public uint GetUnLockLevelByLoc(uint loc)
    {

        uint level = GameTableManager.Instance.GetGlobalConfig<uint>("GetUnLockLevelByLocation", loc.ToString());

        return level;
    }
    public bool TryGetLocationSkillId(SkillSettingState state, int location, out uint skillid)
    {
        SortedDictionary<int, uint> tempDic = null;
        if (state == SkillSettingState.StateOne)
        {
            tempDic = stateOneDic;
        }
        else if (state == SkillSettingState.StateTwo)
        {
            tempDic = stateTwoDic;
        }
        return tempDic.TryGetValue(location, out skillid);
    }

    public void SendSetSkillMessage(int loction, uint skillid, SkillSettingAction action, uint srcIndex = 0, uint srcSkill = 0)
    {
        GameCmd.stSetUsePosSkillUserCmd_CS cmd = new stSetUsePosSkillUserCmd_CS();
        if (action == SkillSettingAction.Add || action == SkillSettingAction.Replace)
        {
            cmd.index = (uint)loction;
            cmd.skillid = skillid;
            cmd.status = (uint)ShowState;
        }
        else if (action == SkillSettingAction.Remove)
        {
            cmd.src_index = (uint)srcIndex;
            cmd.src_skillid = srcSkill;
            cmd.status = (uint)ShowState;
        }
        else if (action == SkillSettingAction.Swap || action == SkillSettingAction.Move)
        {
            cmd.index = (uint)loction;
            cmd.skillid = skillid;
            cmd.status = (uint)ShowState;
            cmd.src_index = srcIndex;
            cmd.src_skillid = srcSkill;
        }

        NetService.Instance.Send(cmd);
    }
    /// <summary>
    /// 把拖动的技能保存起来
    /// </summary>
    /// <param name="index">设置的位置</param>
    /// <param name="skillid">技能id</param>
    public void AddStateSkillItem(int location, uint skillid)
    {
        SortedDictionary<int, uint> tempDic = GetSkillSettingState();

        if (tempDic.ContainsKey(location))
        {
            tempDic[location] = skillid;
        }
        else
        {
            tempDic.Add(location, skillid);
        }

        SetAllSettingItem();
    }
    public void DeleteStateSkill(int location)
    {
        SortedDictionary<int, uint> tempDic = GetSkillSettingState();

        if (tempDic.ContainsKey(location))
        {
            tempDic.Remove(location);
        }

        SetAllSettingItem();
    }
    /// <summary>
    /// 根据状态获取保存当前设置面板的信息
    /// </summary>
    /// <returns></returns>
    public SortedDictionary<int, uint> GetSkillSettingState()
    {

        if (ShowState == SkillSettingState.StateOne)
        {
            return stateOneDic;
        }
        else if (ShowState == SkillSettingState.StateTwo)
        {
            return stateTwoDic;
        }
        return null;
    }
    /// <summary>
    /// 技能是否已经保存到设置面板
    /// </summary>
    /// <param name="skillid">技能id</param>
    /// <param name="index">技能保存的位置</param>
    /// <returns></returns>
    public bool IsSkillSave(uint skillid, ref int index)
    {
        SortedDictionary<int, uint> tempDic = GetSkillSettingState();
        if (tempDic == null)
        {
            return false;
        }
        var iter = tempDic.GetEnumerator();
        while (iter.MoveNext())
        {
            var dic = iter.Current;
            if (dic.Value == skillid)
            {
                index = dic.Key;
                return true;
            }
        }
        return false;
    }
    public int GetLocation(string name)
    {
        if (ShowState == SkillSettingState.StateOne)
        {
            if (OneLocationDic.ContainsKey(name))
            {
                return OneLocationDic[name];
            }

        }
        else if (ShowState == SkillSettingState.StateTwo)
        {
            if (TwoLocationDic.ContainsKey(name))
            {
                return TwoLocationDic[name];
            }

        }
        Log.Error("get location error ");
        return -1;
    }
    #region lock and unlock list
    void AddUnLockSkill(SkillDatabase skillid)
    {
        if (!unLockSkillList.Contains(skillid))
        {
            unLockSkillList.Add(skillid);
        }
    }

    void AddLockSkill(SkillDatabase skillid)
    {
        if (!lockSkillList.Contains(skillid))
        {
            lockSkillList.Add(skillid);
        }
    }

    #endregion
    public List<SkillDatabase> GetCommonSkillDataBase()
    {
        List<SkillDatabase> list = GetStateListByState(0);

        return list;
    }
    public List<SkillDatabase> GetShowStateOneList()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
        {
            return null;
        }
        int job = mainPlayer.GetProp((int)PlayerProp.Job);
        List<SkillDatabase> list = GetStateListByState(job);
        return list;
    }
    public List<SkillDatabase> GetShowStateTwoList()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
        {
            return null;
        }
        int job = mainPlayer.GetProp((int)PlayerProp.Job);
        List<SkillDatabase> list = GetStateListByState(job + 10);
        return list;
    }
    List<SkillDatabase> GetStateListByState(int state)
    {
        List<SkillDatabase> list = new List<SkillDatabase>();

        for (int i = 0; i < unLockSkillList.Count; i++)
        {
            var id = unLockSkillList[i];
            if (id.dwSkillSatus == state)
            {
                list.Add(id);
            }
        }

        for (int i = 0; i < lockSkillList.Count; i++)
        {
            var id = lockSkillList[i];
            if (id.dwSkillSatus == state)
            {
                list.Add(id);
            }
        }
        list.Sort((x1, x2) =>
        {
            SkillDatabase temp1 = GameTableManager.Instance.GetTableItem<SkillDatabase>(x1.wdID, 1);
            SkillDatabase temp2 = GameTableManager.Instance.GetTableItem<SkillDatabase>(x2.wdID, 1);
            if (temp1 != null && temp2 != null)
            {
                if (temp1.dwNeedLevel > temp2.dwNeedLevel)
                    return 1;
                else if (temp1.dwNeedLevel < temp2.dwNeedLevel)
                    return -1;
                else
                    return 0;
            }
            else
            {
                return 0;
            }
        });

        for (int i = 0; i < list.Count; i++)
        {
            SkillDatabase db = list[i];
            var iter = userSkilldic.GetEnumerator();
            while (iter.MoveNext())
            {
                var dic = iter.Current;
                if (db.wdID == dic.Key)
                {
                    SkillInfo info = dic.Value;
                    SkillDatabase data = GameTableManager.Instance.GetTableItem<SkillDatabase>(info.skillID, (int)info.level);
                    list[i] = data;
                    break;
                }
            }
        }
        return list;
    }
    public string GetParentNameByLocation(int location)
    {
        string name = "";
        if (ShowState == SkillSettingState.StateOne)
        {
            var iter = OneLocationDic.GetEnumerator();
            while (iter.MoveNext())
            {
                var loc = iter.Current;
                if (loc.Value == location)
                {
                    name = loc.Key;
                    return name;
                }
            }
        }
        if (ShowState == SkillSettingState.StateTwo)
        {

            var iter = TwoLocationDic.GetEnumerator();
            while (iter.MoveNext())
            {
                var loc = iter.Current;
                if (loc.Value == location)
                {
                    name = loc.Key;
                    return name;
                }
            }
        }
        return name;
    }
    public void SetAllSettingItem()
    {
        if (LearnUI != null)
        {
            LearnUI.SetAllSkillSettingItem();
            LearnUI.ResetLeftItem();
        }
    }
    public bool GetAutoAttack()
    {
        return bAutoAttack;
    }
    public int GetPlayerLevel()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
        {
            return 0;
        }
        int level = mainPlayer.GetProp((int)CreatureProp.Level);
        return level;
    }
    public int GetPlayerExp()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
        {
            return 0;
        }
        int level = mainPlayer.GetProp((int)PlayerProp.Exp);
        return level;
    }
    public int GetPlayerJob()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
        {
            return 0;
        }
        int level = mainPlayer.GetProp((int)PlayerProp.Job);
        return level;
    }
    public uint GetSkillThisID(SkillInfo info)
    {
        return GetSkillThisID(info.skillID, info.level);
    }
    public uint GetSkillThisID(uint skillBaseID, uint skillLevel)
    {
        uint thisID = skillBaseID << 6;
        thisID |= skillLevel;
        Log.LogGroup(GameDefine.LogGroup.User_ZDY, "thisid is " + thisID.ToString() + " baseid is " + skillBaseID.ToString() + "  level is " + skillLevel.ToString());
        ushort tempBaseID = (ushort)(thisID >> 16);
        ushort tempLevel = (ushort)thisID;
        Log.LogGroup(GameDefine.LogGroup.User_ZDY, "thisid is " + thisID.ToString() + " tempBaseID is " + tempBaseID.ToString() + "  tempLevel is " + tempLevel.ToString());
        return thisID;
    }
    public string GetStatus(uint status)
    {
        if (status == 1)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_xushi);
        }
        else if (status == 11)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_panshi);
        }
        else if (status == 3)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_duxing);
        }
        else if (status == 13)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_qugong);
        }
        else if (status == 2)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_zhiliao);
        }
        else if (status == 12)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_huanhua);
        }
        else if (status == 4)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_zhaohuan);
        }
        else if (status == 14)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_zuzhou);
        }
        else
        {
            return CommonData.GetLocalString("通用");
        }
    }
    #region LearnSkill
    /// <summary>
    /// 根据技能id获取角色已经学习的技能信息
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public SkillInfo GetOwnSkillInfoById(uint skillId)
    {
        SkillInfo skill = null;
        if (null != userSkilldic
                && userSkilldic.TryGetValue(skillId, out skill))
        {
            return skill;
        }
        return null;
    }

    /// <summary>
    /// 是否角色学习过该技能
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public bool IsLearnSkill(uint skillId)
    {
        SkillInfo skill = GetOwnSkillInfoById(skillId);
        return (null != skill) ? true : false;
    }

    /// <summary>
    /// 学习技能
    /// </summary>
    /// <param name="skillId">技能id</param>
    /// <param name="nextLv">下一个技能等级</param>
    public void LearnSkill(uint skillId, uint nextLv)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.LearnSkillReq(skillId, nextLv);
        }
    }

    /// <summary>
    /// 服务器下发技能
    /// </summary>
    /// <param name="skillInfos">技能列表</param>
    /// <param name="addType">是首次推送或者升级</param>
    public void OnSkillAdd(Dictionary<uint, SkillInfo> skillInfos, GameCmd.SkillAddType addType)
    {
        if (null != skillInfos && skillInfos.Count != 0)
        {
            if (addType == SkillAddType.SkillAddType_Add)
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SKILLSYSTEM_ADDSKILL, skillInfos);
                //提示有歧义 是技能升级 还是玩家等级升级？
                //TipsManager.Instance.ShowTips(
                //   DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Notice_Upgrade));
            }

        }
    }
    #endregion

    public HeartSkillDataBase GetHeartSkillData(uint skillID)
    {
        string des = "";
        SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID, 1);
        bool hasLearn = false;
        uint heartID = db.heartID;
        uint heartLevel = 0;
        if (db != null)
        {
            List<GameCmd.HeartSkill> heartList = DataManager.Manager<HeartSkillManager>().OwnedHeartSkillList;

            for (int i = 0; i < heartList.Count; i++)
            {
                HeartSkill hs = heartList[i];
                if (hs.skill_id == heartID)
                {
                    hasLearn = true;
                    heartLevel = hs.level;
                    break;
                }
            }
        }
        if (hasLearn)
        {
            HeartSkillDataBase hdb = GameTableManager.Instance.GetTableItem<HeartSkillDataBase>(heartID, (int)heartLevel);
            if (hdb != null)
            {
                return hdb;
            }
        }
        return null;
    }
    public string GetHeartDes(uint skillID)
    {
        HeartSkillDataBase db = GetHeartSkillData(skillID);
        if (db != null)
        {
            return db.des;
        }
        return "";
    }
    public uint GetHeartCd(uint skillID)
    {
        uint cd = 0;
        HeartSkillDataBase db = GetHeartSkillData(skillID);
        if (db != null)
        {
            string effectStr = db.enhanceEffect;
            string[] strArray = effectStr.Split(';');
            for (int i = 0; i < strArray.Length; i++)
            {
                List<uint> idList = StringUtil.GetSplitStringList<uint>(strArray[i], '_');
                if (idList == null)
                {
                    return cd;
                }
                if (idList.Count <= 0)
                {
                    return cd;
                }
                if (idList[0] == 12)
                {
                    //12表示减cd
                    return idList[1];
                }
            }
        }
        return cd;
    }

    /// <summary>
    /// 是否可升级
    /// </summary>
    /// <param name="sdb"></param>
    /// <returns></returns>
    public bool IsSkillEnableUpgrade(SkillDatabase sdb)
    {
        bool isEnableUpgrade = true;

        if (sdb == null)
        {
            isEnableUpgrade = false;
            return isEnableUpgrade;
        }
        if (sdb.isnormal)
        {
            return false;
        }
        if (!UserSkilldic.ContainsKey(sdb.wdID))
        {
            return false;
        }
        IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            //需要等级
            int playerLv = player.GetProp((int)CreatureProp.Level);

            uint nextLevel = sdb.wdLevel + 1;
            SkillDatabase nextDataBase = GameTableManager.Instance.GetTableItem<SkillDatabase>(sdb.wdID, (int)nextLevel);
            if (nextDataBase != null)
            {
                if (playerLv < nextDataBase.dwNeedLevel)
                {
                    return false;
                }
                //                 //消耗金币
                //                 if (player.GetProp((int)Client.PlayerProp.Coupon) < nextDataBase.dwMoney)
                //                 {
                //                     return false;
                //                 }
                // 
                //                 //消耗经验
                //                 if (player.GetProp((int)PlayerProp.Exp) < nextDataBase.dwExp)
                //                 {
                //                     return false;
                //                 }
            }
            else
            {
                return false;
            }

        }

        //前置技能
        SkillDatabase preSkilldb = GameTableManager.Instance.GetTableItem<SkillDatabase>(sdb.dwNeedSkill1, (int)sdb.dwNeedSkill1Level);
        if (preSkilldb != null)
        {
            SkillInfo skill;
            if (userSkilldic.TryGetValue(preSkilldb.wdID, out skill))
            {
                if (skill.level < sdb.dwNeedSkill1Level)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return isEnableUpgrade;
    }

    /// <summary>
    /// 是否可升级
    /// </summary>
    /// <param name="skillInfo"></param>
    /// <returns></returns>
    public bool IsSkillEnableUpgrade(SkillInfo skillInfo)
    {
        bool isEnableUpgrade = true;
        SkillDatabase skilldb = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillInfo.skillID, (int)skillInfo.level);
        if (skilldb == null)
        {
            isEnableUpgrade = false;
            return isEnableUpgrade;
        }

        isEnableUpgrade = IsSkillEnableUpgrade(skilldb);
        return isEnableUpgrade;
    }

    /// <summary>
    /// 是否有技能可升级
    /// </summary>
    /// <returns></returns>
    public bool HaveSkillUpgrade()
    {
        bool isEnableUpgrade = false;

        Dictionary<uint, SkillInfo>.Enumerator enumerator = userSkilldic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            isEnableUpgrade = IsSkillEnableUpgrade(enumerator.Current.Value);
            if (isEnableUpgrade)
            {
                return true;
            }
        }

        return isEnableUpgrade;
    }

    /// <summary>
    /// 判断技能升级是否有足够的钱
    /// </summary>
    /// <returns></returns>
    public SkillLevelUpCode HaveEnoughMoney()
    {
        var iter = userSkilldic.GetEnumerator();
        uint skillID = 0;
        uint minLevel = 1000;
        uint skillLv = 0;
        while (iter.MoveNext())
        {
            SkillDatabase temp = GameTableManager.Instance.GetTableItem<SkillDatabase>(iter.Current.Value.skillID, (int)iter.Current.Value.level + 1);
            if (temp != null)
            {
                if (temp.IsBasic)
                {
                    continue;
                }
                if (temp.dwNeedLevel <= minLevel)
                {
                    minLevel = temp.dwNeedLevel;
                    skillID = iter.Current.Value.skillID;
                    skillLv = iter.Current.Value.level;
                }
            }

        }
        SkillDatabase sdb = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID, (int)skillLv);
        if (sdb == null)
        {
            return SkillLevelUpCode.NoneData;
        }

        if (!UserSkilldic.ContainsKey(sdb.wdID))
        {
            return SkillLevelUpCode.NoSkill;
        }
        IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            //需要等级
            int playerLv = player.GetProp((int)CreatureProp.Level);

            uint nextLevel = sdb.wdLevel + 1;
            SkillDatabase nextDataBase = GameTableManager.Instance.GetTableItem<SkillDatabase>(sdb.wdID, (int)nextLevel);
            if (nextDataBase != null)
            {
                if (playerLv < nextDataBase.dwNeedLevel)
                {
                    return SkillLevelUpCode.LevelNotEnough;
                }
                //消耗金币
                if (player.GetProp((int)Client.PlayerProp.Coupon) < nextDataBase.dwMoney)
                {
                    return SkillLevelUpCode.NoMoney;
                }
            }
        }
        return SkillLevelUpCode.Ok;
    }

    /// <summary>
    /// 技能等级之和（普攻不计算在内）
    /// </summary>
    /// <returns></returns>
    public uint GetUseSkillLvSum()
    {
        Dictionary<uint, SkillInfo>.Enumerator enumerator = userSkilldic.GetEnumerator();

        uint lvSum = 0;
        while (enumerator.MoveNext())
        {
            SkillInfo skillInfo = enumerator.Current.Value;

            SkillDatabase skilldb = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillInfo.skillID, (int)skillInfo.level);

            //普攻不计算在内
            if (skilldb.isnormal)
            {
                continue;
            }

            lvSum += skillInfo.level;
        }

        return lvSum;
    }
    #region 自动挂机设置
    /// <summary>
    /// 自动挂机设置 key是skillid value 是否设置
    /// </summary>
    Dictionary<uint, uint> m_autoFightSkillStatus = new Dictionary<uint, uint>();

    /// <summary>
    /// 客户端自动挂机设置 未保存的
    /// </summary>
    Dictionary<uint, uint> m_clientHookSkillStatus = new Dictionary<uint, uint>();
    /// <summary>
    /// ui设置自动挂机技能 未保存
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="bSet">true是添加false是移除</param>
    public void SetAutoSkill(uint skillID, bool bSet)
    {
        int set = bSet ? 1 : 0;
        if (m_clientHookSkillStatus.ContainsKey(skillID))
        {
            m_clientHookSkillStatus[skillID] = (uint)set;
        }
        else
        {
            m_clientHookSkillStatus.Add(skillID, (uint)set);
        }

    }
    public bool IsSkillAutoSet(uint skillID)
    {
        if (m_clientHookSkillStatus.ContainsKey(skillID))
        {
            return m_clientHookSkillStatus[skillID] == 1 ? true : false;
        }
        return false;
    }

    public void SendAutoSkillSet()
    {
        stSetSkillHookStatusMagicUserCmd_CS cmd = new stSetSkillHookStatusMagicUserCmd_CS();
        foreach (var dic in m_clientHookSkillStatus)
        {
            SkillHookStatus st = new SkillHookStatus();

            st.skillid = dic.Key;
            if (dic.Value == 1)
            {
                st.status = dic.Value;
                cmd.hook_status.Add(st);
            }

        }
        NetService.Instance.Send(cmd);
    }
    /// <summary>
    /// 判断技能是否解锁可学习
    /// </summary>
    /// <param name="skillID"></param>
    /// <returns>true 是解锁</returns>
    public bool IsSkillUnLock(uint skillID, uint skillLv = 1)
    {
        IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            //需要等级
            int playerLv = player.GetProp((int)CreatureProp.Level);
            SkillDatabase sdb = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID, (int)skillLv);
            if (sdb != null)
            {
                if (playerLv >= sdb.dwNeedLevel)
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 服务器设置设置技能 保存后的
    /// </summary>
    /// <param name="cmd"></param>
    public void OnReceiveAutoFightInfo(stSetSkillHookStatusMagicUserCmd_CS cmd)
    {
        TipsManager.Instance.ShowTips(LocalTextType.Skill_Commond_jinengshezhibaocunchenggong);
        m_autoFightSkillStatus.Clear();
        m_clientHookSkillStatus.Clear();
        foreach (var info in cmd.hook_status)
        {
            SetHookSkillStatus(info.skillid, info.status);
            SetClientHookSkillStatus(info.skillid, info.status);
        }
        SetCurStateSkillList();
        DispatchValueUpdateEvent(new ValueUpdateEventArgs()
        {
            key = LearnSkillDispatchEvents.SkillAutoFightSet.ToString(),
            oldValue = null,
            newValue = null,

        });
    }
    /// <summary>
    /// 设置技能自动挂机状态 1是设置 0是不设置
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="status"></param>
    void SetHookSkillStatus(uint skillID, uint status)
    {
        if (m_autoFightSkillStatus.ContainsKey(skillID))
        {
            m_autoFightSkillStatus[skillID] = status;
        }
        else
        {
            m_autoFightSkillStatus.Add(skillID, status);
        }
    }
    /// <summary>
    /// 客户端自动挂机设置 未保存
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="status"></param>
    void SetClientHookSkillStatus(uint skillID, uint status)
    {
        if (m_clientHookSkillStatus.ContainsKey(skillID))
        {
            m_clientHookSkillStatus[skillID] = status;
        }
        else
        {
            m_clientHookSkillStatus.Add(skillID, status);
        }
    }
    #endregion
}

