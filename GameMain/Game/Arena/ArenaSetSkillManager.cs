using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using GameCmd;
using Client;
using Engine.Utility;
using UnityEngine;
using Common;
//enum LearnSkillDispatchEvents
//{
//    SkillLevelUP,//技能升级
//}
//enum SkillSettingState
//{
//    None = 0,//通用状态
//    StateOne = 1,//状态一
//    StateTwo = 2,//状态二
//}
class ArenaSetSkillManager : BaseModuleData, IManager
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
            curState = value;
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

    public ArenaSetSkillPanel LearnUI
    {
        get
        {
            return DataManager.Manager<UIPanelManager>().GetPanel(PanelID.ArenaSetSkillPanel) as ArenaSetSkillPanel;
        }
    }
    /// <summary>
    /// 普攻长按标识
    /// </summary>
    bool longPress;
    public bool bLongPress
    {
        get
        {
            return longPress;
        }
        set
        {
            longPress = value;
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
    public void Initialize()
    {
        //Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
        //EventEngine.Instance().AddEventListener((int)GameEventID.SKILL_SHOWPETSKILL, OnEvent);

    }
    public void ClearData()
    {

    }
    public void Reset(bool depthClearData = false)
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
        ShowState = SkillSettingState.None;
        CurState = SkillSettingState.None;
        IsSettingPanel = false;
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
    /*
    void OnEvent(int eventID, object param)
    {
        switch ((Client.GameEventID)eventID)
        {
            case GameEventID.ENTITYSYSTEM_PROPUPDATE:
                {
                    stPropUpdate prop = (stPropUpdate)param;
                    if (prop.nPropIndex != (int)CreatureProp.Level)
                        return;
                    AutoSetSkill();
                    int needLevel = GameTableManager.Instance.GetGlobalConfig<int>("ChangeStateLevel");
                    int level = GetPlayerLevel();

                    if (level >= needLevel)
                    {
                        CurState = SkillSettingState.StateOne;
                        SetCurStateSkillList();
                        SkillPanel panel = DataManager.Manager<UIPanelManager>().GetPanel(PanelID.SkillPanel) as SkillPanel;
                        if (panel != null)
                        {
                            panel.SetSkillIcon();
                        }
                    }
                }
                break;
            case GameEventID.SKILL_SHOWPETSKILL:
                {
                    stShowPetSkill st = (stShowPetSkill)param;
                    m_bShowPetSkill = st.bShow;
                    DataManager.Manager<UIPanelManager>().SendMsg(PanelID.SkillPanel, UIMsgID.eSkillShowPetSkill, null);
                }
                break;
        }
    }
     */ 
      
    /// <summary>
    /// 根据服务器消息初始化用户技能信息
    /// </summary>
    /// <param name="skillID">技能baseid</param>
    /// <param name="level">技能等级</param>
    /// <param name="coldTime">冷却时间</param>
    public void InitUserSkill(uint skillID, uint level, uint coldTime)
    {
        SkillInfo info = new SkillInfo(skillID, level, coldTime);
        if (!userSkilldic.ContainsKey(skillID))
        {
            userSkilldic.Add(skillID, info);
            //DispatchValueUpdateEvent(new ValueUpdateEventArgs()
            //{
            //    key = LearnSkillDispatchEvents.SkillLevelUP.ToString(),
            //    oldValue = null,
            //    newValue = info,
            //});
        }
        else
        {
            SkillInfo oldInfo = userSkilldic[skillID];
            uint oldLevel = oldInfo.level;
            userSkilldic[skillID] = info;
            //if (oldLevel != level)
            //{
            //    DispatchValueUpdateEvent(new ValueUpdateEventArgs()
            //    {
            //        key = LearnSkillDispatchEvents.SkillLevelUP.ToString(),
            //        oldValue = null,
            //        newValue = info,

            //    });
            //}
        }
        SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID, (int)level);

        if (db != null)
        {
            if (LearnUI != null)
            {
                InitSkillID();
                LearnUI.CurDataBase = db;
            }
        }
    }
    public List<uint> GetUnLockLevelList()
    {
        List<uint> list = new List<uint>();
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        int job = mainPlayer.GetProp((int)PlayerProp.Job);
        int level = mainPlayer.GetProp((int)CreatureProp.Level);
        List<SkillDatabase> skillList = GameTableManager.Instance.GetTableList<SkillDatabase>();
        skillList = skillList.FindAll(P => P.dwJob == job && !P.IsBasic && P.wdLevel == 1);
        foreach (var skill in skillList)
        {
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
    /// 初始化表格数据
    /// </summary>
    public void InitData()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
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
        foreach (var skill in allBaseSkillDataBase)
        {//isnormal表示是否普攻
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
    public void OnSetSkillState(stUseSkillStatusArenaUserCmd_CS cmd)
    {
        //CurState = cmd.status == 0 ? SkillSettingState.StateOne : SkillSettingState.StateTwo;
        //DataManager.Manager<UIPanelManager>().SendMsg(PanelID.SkillPanel, UIMsgID.eSkillChangeState, null);
        CurState = (SkillSettingState)cmd.status;
    }
    /// <summary>
    /// 根据服务器信息 初始化设置面板
    /// </summary>
    /// <param name="cmd"></param>
    public void OnInitLocation(stGetSkillSettingArenaUserCmd_S cmd)
    {
        List<SkillUsePos> posList = cmd.status_pos;
        //if (CurState == SkillSettingState.None)
        //{
        //    CurState = cmd.cur_status == 0 ? SkillSettingState.StateOne : SkillSettingState.StateTwo;
        //}

        CurState = (SkillSettingState)cmd.status;
        foreach (var userpos in posList)
        {
            uint status = userpos.status;
            if (userpos.status == (uint)SkillSettingState.StateOne)
            {
                stateOneDic.Clear();
                foreach (var skillpos in userpos.skill_pos)
                {
                    uint totalid = skillpos.skillid >> 16;
                    uint level = skillpos.skillid & 0xffff;
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
                foreach (var skillpos in userpos.skill_pos)
                {
                    uint totalid = skillpos.skillid >> 16;
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
       // SetCurStateSkillList();
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ArenaSetSkillPanel);
        SetAllSettingItem();
       // DataManager.Manager<UIPanelManager>().SendMsg(PanelID.SkillPanel, UIMsgID.eSkillBtnRefresh, null);
    }
    //public void SetCurStateSkillList()
    //{
    //    SortedDictionary<int, uint> tempDic = null;
    //    if (CurState == SkillSettingState.StateOne)
    //    {
    //        tempDic = stateOneDic;
    //    }
    //    else if (CurState == SkillSettingState.StateTwo)
    //    {
    //        tempDic = stateTwoDic;
    //    }
    //    List<uint> skillIDList = new List<uint>();
    //   // foreach (var dic in tempDic)

    //    var iter = tempDic.GetEnumerator();
    //    while(iter.MoveNext())
    //    {
    //        var dic = iter.Current;
    //        if (dic.Value != 0)
    //        {
    //            skillIDList.Add(dic.Value);
    //        }
    //    }
    //    skillIDList.Reverse_NoHeapAlloc();
    //    uint commonSkillID = GetCommonSkillIDByJob();
    //    skillIDList.Insert(0, commonSkillID);
   
    //}

  
    public void OnSetLocation(stSkillSettingArenaUserCmd_CS cmd)
    {
        if (cmd.ret == (uint)SkillRet.SkillRet_Success)
        {
            //Log.Info("技能设置成功 error code is " + cmd.ret);
            ReqArenaSetSkillList();
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
    
        GameCmd.stSkillSettingArenaUserCmd_CS cmd = new stSkillSettingArenaUserCmd_CS();
        if (action == SkillSettingAction.Add || action == SkillSettingAction.Replace)
        {
            cmd.skill_index = (uint)loction;
            cmd.skill_id = GetSkillIdWithLv(skillid);
            cmd.skill_state = (uint)ShowState;
        }
        else if (action == SkillSettingAction.Remove)
        {
            cmd.skill_index = (uint)srcIndex;
            cmd.skill_id = GetSkillIdWithLv(srcSkill);
            cmd.skill_state = (uint)ShowState;
        }
        else if (action == SkillSettingAction.Swap || action == SkillSettingAction.Move)
        {
            cmd.skill_index = (uint)loction;
            cmd.skill_id = GetSkillIdWithLv(skillid);
            cmd.skill_state = (uint)ShowState;
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
        foreach (var dic in tempDic)
        {
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
        int job = mainPlayer.GetProp((int)PlayerProp.Job);
        List<SkillDatabase> list = GetStateListByState(job);
        return list;
    }
    public List<SkillDatabase> GetShowStateTwoList()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        int job = mainPlayer.GetProp((int)PlayerProp.Job);
        List<SkillDatabase> list = GetStateListByState(job + 10);
        return list;
    }
    List<SkillDatabase> GetStateListByState(int state)
    {
        List<SkillDatabase> list = new List<SkillDatabase>();
        foreach (var id in unLockSkillList)
        {
            if (id.dwSkillSatus == state)
            {
                list.Add(id);
            }
        }

        foreach (var id in lockSkillList)
        {
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
            foreach (var dic in userSkilldic)
            {
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
            foreach (var loc in OneLocationDic)
            {
                if (loc.Value == location)
                {
                    name = loc.Key;
                    return name;
                }
            }
        }
        if (ShowState == SkillSettingState.StateTwo)
        {
            foreach (var loc in TwoLocationDic)
            {
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
        int level = mainPlayer.GetProp((int)CreatureProp.Level);
        return level;
    }
    public int GetPlayerExp()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        int level = mainPlayer.GetProp((int)PlayerProp.Exp);
        return level;
    }
    public int GetPlayerJob()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        int level = mainPlayer.GetProp((int)PlayerProp.Job);
        return level;
    }
    public uint GetSkillThisID(SkillInfo info)
    {
        return GetSkillThisID(info.skillID, info.level);
    }
    public uint GetSkillThisID(uint skillBaseID, uint skillLevel)
    {
        uint thisID = skillBaseID << 16;
        thisID |= skillLevel;
        Log.LogGroup(GameDefine.LogGroup.User_ZDY, "thisid is " + thisID.ToString() + " baseid is " + skillBaseID.ToString() + "  level is " + skillLevel.ToString());
        ushort tempBaseID = (ushort)(thisID >> 16);
        ushort tempLevel = (ushort)thisID;
        Log.LogGroup(GameDefine.LogGroup.User_ZDY, "thisid is " + thisID.ToString() + " tempBaseID is " + tempBaseID.ToString() + "  tempLevel is " + tempLevel.ToString());
        return thisID;
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
                TipsManager.Instance.ShowTips(
                    DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Notice_Upgrade));
            }
            //氏族技能
            DataManager.Manager<ClanManger>().OnPlayerSkillUpdate(skillInfos.Keys.ToList());
        }
    }


    //武斗场技能设置的网络部分

    public void ReqArenaSetSkillList() 
    {
        stGetSkillSettingArenaUserCmd_C cmd = new stGetSkillSettingArenaUserCmd_C();
        NetService.Instance.Send(cmd);
    }

    public void ReqSkillStatus(uint status)
    {
        stUseSkillStatusArenaUserCmd_CS cmd = new stUseSkillStatusArenaUserCmd_CS();
        cmd.status = status;
        NetService.Instance.Send(cmd);
    }

    public void ReqClearArenaSetSkill(uint state)
    {
        stClearUsePosArenaUserCmd_CS cmd = new stClearUsePosArenaUserCmd_CS();
        cmd.state = state;
        NetService.Instance.Send(cmd);
    }

    uint GetSkillIdWithLv(uint skillId)
    {
        SkillInfo skillInfo = GetOwnSkillInfoById(skillId);
        uint id = skillInfo.skillID;
        uint level = skillInfo.level;
        uint idwithLv = (id << 16) & 0xffff0000;     //高16位放id
        level = level & 0xffff;                      //低16位放lv
        idwithLv = idwithLv + level;
        return idwithLv;
    }

    #endregion

}

