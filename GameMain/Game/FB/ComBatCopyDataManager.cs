//*************************************************************************
//	创建日期:	2016/10/18 10:41:39
//	文件名称:	ComBatCopyDataManager
//   创 建 人:   zhuidanyu	
//	版权所有:	中青宝
//	说    明:	副本数据管理
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using table;
using GameCmd;
using Engine;
using Engine.Utility;
using UnityEngine;
using Controller;

/// <summary>
/// 副本接口
/// </summary>
public interface ICopy
{
    void EnterCopy();  //进入副本

    void ExitCopy();   //退出副本
}


public enum CopyFlag
{
    Begin = 1,
    Juqing = Begin,
    Danren = 2,
    Zudui = 3,
    Huodong = 4,
    DaTi = 5,
    End = DaTi,
}

public enum CopySubFlag
{
    None = 0,
    [System.ComponentModel.Description("主线")]
    Item = 1,
    [System.ComponentModel.Description("经验")]
    Exp = 2,
    [System.ComponentModel.Description("神魔")]
    Ride = 3,//坐骑
    [System.ComponentModel.Description("限时")]
    Soul = 4,//圣魂
    [System.ComponentModel.Description("宠物")]
    Pet = 5,
}

public enum CopyDispatchEvent
{
    RefreshStatus,
}

/// <summary>
/// 表格里面定义的副本类型
/// </summary>
public enum CopyTypeTable
{
    None = 0,
    Normal = 1,     //1 普通副本
    Arena = 2,      //2 武斗场
    Camp = 3,       //3 阵营战
    CityWar = 4,    //4 城战战场
}

class CopyInfo
{
    /// <summary>
    /// 副本类型
    /// </summary>
    public uint CopyType
    {
        get;
        set;
    }

    public uint MaxCopyNum
    {
        get;
        set;
    }

    public uint CopyUseNum
    {
        get
        {
            //面板显示超过上限 
            if (m_uCopyUseNum > MaxCopyNum)
            {
                return MaxCopyNum;
            }
            else 
            {
                return m_uCopyUseNum;
            }
        }
        set
        {
            m_uCopyUseNum = value;
        }
    }
    public bool IsFinished 
    {
        get 
        {
            return m_uCopyUseNum > MaxCopyNum;
        }
    }
    uint m_uCopyUseNum;//本日使用了的副本次数
}


public class CopyCDAndExitInfo
{
    public bool bShow;
    public bool bShowBattleInfoBtn;//是否显示详情按钮
}


partial class ComBatCopyDataManager : BaseModuleData, IManager, ITimer
{
    #region property

    CopyFlag m_copyFlag = CopyFlag.Juqing;
    public CopyFlag CPFlag
    {
        get
        {
            return m_copyFlag;
        }
        set
        {
            m_copyFlag = value;
        }
    }

    CopySubFlag m_subFlag = CopySubFlag.None;
    public CopySubFlag SubFlag
    {
        get
        {
            return m_subFlag;
        }
        set
        {
            m_subFlag = value;
        }
    }

    uint m_uEnterCopyID = 0;
    public uint EnterCopyID
    {
        get
        {
            return m_uEnterCopyID;
        }
    }

    uint m_uCountDown = 30;
    public uint EnterCopyCountDown
    {
        get
        {
            return m_uCountDown;
        }
    }

    /// <summary>
    /// 副本CD
    /// </summary>
    float m_uCopyCountDown = 30 * 60f;
    public float CopyCountDown
    {
        get
        {
            return m_uCopyCountDown;
        }
        set
        {
            m_uCopyCountDown = value;
        }
    }

    uint m_uCopyFinishCountDown;

    //副本阶段奖励经验总和
    uint m_copyRewardExp = 0;
    public uint CopyRewardExp
    {
        get
        {
            return m_copyRewardExp;
        }
    }

    uint m_nLastKillWave = 0;
    public uint LaskSkillWave
    {
        get
        {
            return m_nLastKillWave;
        }
    }

    uint m_nLastTransmitWave = 0;
    public uint LastTransmitWave
    {
        get
        {
            return m_nLastTransmitWave;
        }
    }

    /// <summary>
    /// 副本中获得的金币（用于金币副本）
    /// </summary>
    uint m_gainGoldInCopy;
    public uint GainGoldInCopy
    {
        get
        {
            return m_gainGoldInCopy;
        }
    }

    /// <summary>
    /// 副本中累计伤害值
    /// </summary>
    int m_allDamageInCopy;
    int AllDamageInCopy
    {
        get
        {
            return m_allDamageInCopy;
        }
    }

    /// <summary>
    /// 记录每个队友的确认的状态
    /// </summary>
    public Dictionary<uint, bool> m_dicTeammateStatus = new Dictionary<uint, bool>();

    /// <summary>
    /// 储存每个副本的信息 key是副本次数统计类型
    /// </summary>
    Dictionary<uint, CopyInfo> m_dicCopyInfo = new Dictionary<uint, CopyInfo>();
    public Dictionary<uint, CopyInfo> CopyInfoDic
    {
        get
        {
            return m_dicCopyInfo;
        }
    }

    /// <summary>
    /// 储存每个标签页的表格数据
    /// </summary>
    Dictionary<CopyFlag, SortedDictionary<CopySubFlag, List<CopyDataBase>>> m_dicFlag = new Dictionary<CopyFlag, SortedDictionary<CopySubFlag, List<CopyDataBase>>>();

    /// <summary>
    ///副本区域进入状态
    /// </summary>
    Dictionary<int, bool> m_dicEnterZoneStatus = new Dictionary<int, bool>();
    Dictionary<uint, float> m_dicSendEnterZoneTime = new Dictionary<uint, float>();
    private readonly uint m_uCopyJumpTimerID = 900;
    private readonly uint m_uCopyAskTeamTimerID = 100;
    private readonly uint m_uCopyFinishTimerID = 2001;
    private readonly uint m_uAutoStartFightTimerID = 3001;

    /// <summary>
    /// 副本 CD 和 
    /// </summary>
    public CopyCDAndExitInfo CopyCDAndExitData { set; get; }

    /// <summary>
    /// 是否进入副本
    /// </summary>
    private bool m_bIsEnterCopy = false;

    public bool IsEnterCopy
    {
        get
        {
            return m_bIsEnterCopy;
        }
    }

    #endregion

    #region interface

    public void ClearData()
    {

    }

    void IManager.Initialize()
    {
        EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTERMAP, OnEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.SYSTEM_LOADSCENECOMPELETE, OnEvent);
        //EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM, OnEvent);
    }

    void IManager.Reset(bool depthClearData = false)
    {
        if (depthClearData)
        {
            m_dicTeammateStatus.Clear();
            m_dicCopyInfo.Clear();
            m_dicFlag.Clear();
            m_dicEnterZoneStatus.Clear();
            m_dicSendEnterZoneTime.Clear();

            //强制清副本数据
            m_uEnterCopyID = 0;

            ClearCopyData();
        }
    }

    void ClearCopyData()
    {
        //退出副本
        stExitCopyUserCmd_CS cmd = new stExitCopyUserCmd_CS() { copy_base_id = 0 };

        Client.IMapSystem mapsys = ClientGlobal.Instance().GetMapSystem();
        if (mapsys != null)
        {
            mapsys.SetEnterZoneCallback(null);
        }

        ICopy copy = GetCopyByCopyID(m_uEnterCopyID);
        if (copy != null)
        {
            copy.ExitCopy();
        }

        m_uEnterCopyID = 0;
        m_nLastKillWave = 0;
        m_nLastTransmitWave = 0;
        m_dicEnterZoneStatus.Clear();
        m_dicSendEnterZoneTime.Clear();

        this.CopyCDAndExitData = null;

        //清副本目标数据
        CleanCopyTargetData();

        stCopyInfo info = new stCopyInfo();
        info.bShow = false;
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eShowCopyInfo, info);

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eCopyExit, null);
        }

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.COMBOT_ENTER_EXIT, new Client.stCombotCopy() { copyid = cmd.copy_base_id, exit = true });

        IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            Engine.Utility.Log.Error("ExecuteCmd: ControllerSystem is null");
            return;
        }

        if (cs.GetCombatRobot().Status != CombatRobotStatus.STOP)
        {
            cs.GetCombatRobot().Stop();
        }

        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            player.SendMessage(EntityMessage.EntityCommand_StopMove, player.GetPos());
            CmdManager.Instance().Clear();//清除寻路
        }

        //退出副本标志
        m_bIsEnterCopy = false;
    }

    void IManager.Process(float deltaTime)
    {
        if (this.m_uCopyCountDown > 0)
        {
            this.m_uCopyCountDown -= deltaTime;
        }

        if (this.m_worldJuBaoCD > 0)
        {
            this.m_worldJuBaoCD -= deltaTime;
        }
    }

    #endregion


    #region   method

    void OnEvent(int eventID, object param)
    {
        if (eventID == (int)GameEventID.ENTITYSYSTEM_ENTERMAP)
        {
            uint mapID = (uint)param;
            CopyDataBase copyBb = GameTableManager.Instance.GetTableItem<CopyDataBase>(this.m_uEnterCopyID);
            if (copyBb == null)
            {
                return;
            }

            //当前题图与副本地图不一样，退出副本了
            if (copyBb.mapId != mapID)
            {
                stExitCopyUserCmd_CS cmd = new stExitCopyUserCmd_CS() { copy_base_id = 0 };
                OnExitCopy(cmd);
            }
        }
        else if (eventID == (int)GameEventID.SYSTEM_LOADSCENECOMPELETE)
        {
            TimerAxis.Instance().SetTimer(m_uAutoStartFightTimerID, 1000, this, 1);
        }
        //else if (eventID == (int)GameEventID.UIEVENT_REFRESHCURRENCYNUM)
        //{
        //    if (false == this.IsEnterCopy)
        //    {
        //        return;//非副本，退出
        //    }

        //    ItemDefine.UpdateCurrecyPassData data = (ItemDefine.UpdateCurrecyPassData)param;
        //    if (data.MoneyType == MoneyType.MoneyType_Gold)
        //    {
        //        AddGoldInCopy(data.ChangeNum);
        //    }
        //}
    }

    /// <summary>
    /// 是否为剧情副本
    /// </summary>
    /// <param name="copyID"></param>
    /// <returns></returns>
    public bool IsStoryCopy(uint copyID)
    {
        bool storyCopy = false;
        table.CopyDataBase cdb = GameTableManager.Instance.GetTableItem<table.CopyDataBase>(copyID);
        if (null != cdb && (cdb.copyFlag == (uint)CopyFlag.Juqing))
        {
            storyCopy = true;
        }
        return storyCopy;
    }


    void StartAutoFight()
    {
        if (m_bIsEnterCopy)
        {
            CopyDataBase cdb = GameTableManager.Instance.GetTableItem<CopyDataBase>(m_uEnterCopyID);
            if (cdb != null)
            {
                //进入副本 如果还是组队跟随状态 跟随状态挂起，进入挂机状态
                Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
                if (cs != null)
                {
                    Client.ICombatRobot robot = cs.GetCombatRobot();
                    if (robot != null) //&& robot.Status != Client.CombatRobotStatus.STOP)
                    {
                        if (DataManager.Manager<TeamDataManager>().IsJoinTeam && DataManager.Manager<TeamDataManager>().IsFollow)
                        {
                            robot.StartInCopy(m_uEnterCopyID, LaskSkillWave, LastTransmitWave);
                        }

                        if (cdb.IsAutoFight)
                        {
                            robot.StartInCopy(m_uEnterCopyID, LaskSkillWave, LastTransmitWave);
                        }

                    }
                }
            }
        }
    }

    void AddCopyInfo(uint copyType, CopyInfo info)
    {
        if (m_dicCopyInfo.ContainsKey(copyType))
        {
            m_dicCopyInfo[copyType] = info;
        }
        else
        {
            m_dicCopyInfo.Add(copyType, info);
        }
    }

    public CopyInfo GetCopyInfoById(uint copyID)
    {
        CopyDataBase db = GameTableManager.Instance.GetTableItem<CopyDataBase>(copyID);
        if (db == null)
        {
            return null;
        }
        if (m_dicCopyInfo.ContainsKey(db.staNumType))
        {
            return m_dicCopyInfo[db.staNumType];
        }
        return null;
    }

    void ChangeCopyNum(uint copyType, stNumCopyUserCmd_S cmd)
    {
        if (m_dicCopyInfo.ContainsKey(copyType))
        {
            CopyInfo info = m_dicCopyInfo[copyType];
            info.CopyUseNum = cmd.copy_num;
            info.MaxCopyNum = cmd.max_copy_num;
        }
        else
        {
            CopyInfo info = new CopyInfo();
            info.CopyType = copyType;
            info.CopyUseNum = cmd.copy_num;
            info.MaxCopyNum = cmd.max_copy_num;

            AddCopyInfo(copyType, info);
        }
    }


    /// <summary>
    /// 获取当前副本类型
    /// </summary>
    /// <returns></returns>
    public CopyTypeTable GetCurCopyType()
    {
        return GetCopyTypeByCopyID(m_uEnterCopyID);
    }

    public CopyTypeTable GetCopyTypeByCopyID(uint copyID)
    {
        CopyDataBase cdb = GameTableManager.Instance.GetTableItem<CopyDataBase>(copyID);
        if (cdb != null)
        {
            if (cdb.copyType == 1)
            {
                return CopyTypeTable.Normal;
            }
            else if (cdb.copyType == 2)
            {
                return CopyTypeTable.Arena;
            }
            else if (cdb.copyType == 3)
            {
                return CopyTypeTable.Camp;
            }
            else if (cdb.copyType == 4)
            {
                return CopyTypeTable.CityWar;
            }
        }
        return CopyTypeTable.None;
    }

    /// <summary>
    /// 副本借口
    /// </summary>
    /// <param name="copyID"></param>
    /// <returns></returns>
    public ICopy GetCopyByCopyID(uint copyID)
    {
        CopyDataBase cdb = GameTableManager.Instance.GetTableItem<CopyDataBase>(copyID);
        if (cdb != null)
        {
            if (cdb.clientCopyType == 2)
            {
                return DataManager.Manager<ArenaManager>();
            }
            else if (cdb.clientCopyType == 3)
            {
                return DataManager.Manager<CampCombatManager>();
            }
            else if (cdb.clientCopyType == 4)
            {
                return DataManager.Manager<NvWaManager>();
            }
            else if (cdb.clientCopyType == 7)
            {
                return DataManager.Manager<CityWarManager>();
            }
            else if (cdb.clientCopyType == 10)
            {
                return DataManager.Manager<JvBaoBossWorldManager>();
            }
            else if (cdb.clientCopyType == 11)
            {
                return DataManager.Manager<AnswerManager>();
            }
        }

        return null;
    }

    /// <summary>
    /// 通过副本id获取同类型副本列表
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public List<CopyDataBase> GetCopyListByCopyByCopyID(uint copyid)
    {
        CopyDataBase db = GameTableManager.Instance.GetTableItem<CopyDataBase>(copyid);
        if (db != null)
        {
            if (db.staNumType == 0)
            {
                return new List<CopyDataBase>() { db };
            }
            return GetCopyListByCopyStaticCount(db.staNumType);
        }
        return null;
    }

    /// <summary>
    /// 通过次数统计类型获取副本列表
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    List<CopyDataBase> GetCopyListByCopyStaticCount(uint count)
    {
        List<CopyDataBase> list = GameTableManager.Instance.GetTableList<CopyDataBase>();
        if (list != null)
        {
            return list.FindAll((x) => { return x.staNumType == count; });
        }
        return null;
    }

    public bool IsSingleShowCard(uint copyID)
    {
        CopyDataBase db = GameTableManager.Instance.GetTableItem<CopyDataBase>(copyID);
        if (db != null)
        {
            return db.bSingleShow;
        }
        return false;
    }

    /// <summary>
    /// 副本中金币增加数（用于金币副本）
    /// </summary>
    /// <param name="goldNum"></param>
    void AddGoldInCopy(uint goldNum)
    {
        if (goldNum > 0)
        {
            this.m_gainGoldInCopy += goldNum;
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
            {
                DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eCopyGold, null);
            }
        }
    }


    /// <summary>
    /// 是否是阵营战副本
    /// </summary>
    public bool IsCampCopy(uint copyId)
    {
        return copyId == 4001 || copyId == 4003;  //副本类型为3  是神魔大战（即阵营战）
    }

    /// <summary>
    /// 是否五行阵副本
    /// </summary>
    /// <returns></returns>
    public bool IsWuXinZhenCopy()
    {
        return this.m_uEnterCopyID == 3010 || this.m_uEnterCopyID == 3011 || this.m_uEnterCopyID == 3012;
    }

    /// <summary>
    /// 是否演武场副本
    /// </summary>
    /// <param name="copyId"></param>
    /// <returns></returns>
    public bool IsYanWuChangCopy(uint copyId)
    {
        return copyId == 3000;
    }

    /// <summary>
    /// 是否金币副本（金银山）
    /// </summary>
    /// <returns></returns>
    public bool IsGoldCopy(uint copyId)
    {
        return copyId == 6003;
    }

    /// <summary>
    /// 是否世界聚宝副本(世界首领)
    /// </summary>
    /// <returns></returns>
    public bool IsWorldJuBao(uint copyId)
    {
        return copyId == 5010 || copyId == 5011;
    }

    #endregion


    #region msg
    public void OnEnterCopy(stEntertCopyUserCmd_S cmd)
    {
        m_dicTeammateStatus.Clear();
        m_dicEnterZoneStatus.Clear();
        m_dicSendEnterZoneTime.Clear();

        CopyDataBase cdb = GameTableManager.Instance.GetTableItem<CopyDataBase>(cmd.copy_base_id);
        if (cdb != null)
        {
            m_uEnterCopyID = cmd.copy_base_id;

            uint campCopyId = 4001;   //阵营战
            if (m_uEnterCopyID != campCopyId)
            {
                m_uCopyCountDown = cdb.keepTime - cmd.copy_live_time;
            }

            Client.IMapSystem mapsys = ClientGlobal.Instance().GetMapSystem();
            if (mapsys != null)
            {
                mapsys.SetEnterZoneCallback(OnEnterZone);
            }

            //进入副本时 初始化波数数据
            InitWaveIdListByCopyId();

            //进入副本接口
            ICopy copy = GetCopyByCopyID(m_uEnterCopyID);
            if (copy != null)
            {
                copy.EnterCopy();
            }
            else
            {
                stCopyInfo info = new stCopyInfo();
                info.bShow = true;
                info.bShowBattleInfoBtn = false;

                DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eShowCopyInfo, info);

                this.CopyCDAndExitData = new CopyCDAndExitInfo { bShow = true, bShowBattleInfoBtn = false };
            }

            //副本中关闭消息推送界面
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MessagePushPanel))
            {
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.MessagePushPanel);
            }

            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
            {
                DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eCopyEnter, null);
            }

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.COMBOT_ENTER_EXIT, new Client.stCombotCopy() { copyid = cmd.copy_base_id, enter = true });

            //进入副本标志
            m_bIsEnterCopy = true;
        }

    }

    void OnEnterZone(int nZoneid)
    {
        if (!m_dicEnterZoneStatus.ContainsKey(nZoneid))
        {
            m_dicEnterZoneStatus.Add(nZoneid, true);
            SendEnterZoneCmd((uint)nZoneid);
        }
        else if (m_dicEnterZoneStatus[nZoneid] == false)//如果服务器下发没有触发成功 继续发送触发消息
        {
            m_dicEnterZoneStatus[nZoneid] = true;
            SendEnterZoneCmd((uint)nZoneid);
        }
    }

    void SendEnterZoneCmd(uint nzoneid)
    {
        if (!m_dicSendEnterZoneTime.ContainsKey(nzoneid))
        {
            m_dicSendEnterZoneTime.Add(nzoneid, Time.realtimeSinceStartup);
        }
        Engine.Utility.Log.LogGroup("ZCX", "time :{0}", Time.realtimeSinceStartup);
        NetService.Instance.Send(new stEnterZoneCopyUserCmd_CS() { zone_id = nzoneid });
    }

    /// <summary>
    /// 服务器通知进入区域触发成功
    /// </summary>
    /// <param name="cmd"></param>
    public void OnEnterZoneCopyUser(stEnterZoneCopyUserCmd_CS cmd)
    {
        if (m_dicEnterZoneStatus.ContainsKey((int)cmd.zone_id))
        {
            m_dicEnterZoneStatus[(int)cmd.zone_id] = cmd.succ;
        }

        if (m_dicSendEnterZoneTime.ContainsKey(cmd.zone_id))
        {
            m_dicSendEnterZoneTime.Remove(cmd.zone_id);
        }
    }

    public void OnWaveFinish(stWaveFinishCopyUserCmd_S cmd)
    {
        m_nLastKillWave = cmd.wave;

        if (false == m_listCompletedWaveId.Contains(cmd.wave))
        {
            m_listCompletedWaveId.Add(cmd.wave);
        }

        //跟新当前阶段
        UpdateCopyTargetStepByCopyWave();

        Engine.Utility.Log.LogGroup("ZCX", "================副本挂机第{0}波死了", cmd.wave);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_COPYKILLWAVE, new Client.stCopySkillWave() { waveId = cmd.wave, posIndex = 0 });
    }

    public void OnJumpCopy(stJumpCopyUserCmd_S cmd)
    {
        m_nLastTransmitWave = cmd.pos - 1;

        m_nLastKillWave = cmd.wave;
        ClientGlobal.Instance().GetMapSystem().Process = 0f;
        TimerAxis.Instance().SetTimer(m_uCopyJumpTimerID, 50, this, 20);
    }

    private void InvorkRobot()
    {
        TimerAxis.Instance().KillTimer(m_uCopyJumpTimerID, this);

        IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            Engine.Utility.Log.Error("ExecuteCmd: ControllerSystem is null");
            return;
        }
        if (cs.GetCombatRobot().Status != CombatRobotStatus.RUNNING)
        {
            if (EnterCopyID != 0)
            {
                cs.GetCombatRobot().StartInCopy(EnterCopyID, m_nLastKillWave, m_nLastTransmitWave);
            }
            else
            {
                cs.GetCombatRobot().Start();
            }
        }
        DataManager.Manager<UIPanelManager>().ShowMain();
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.LoadingPanel);
    }

    public void OnAskTeamrCopy(stAskTeamrCopyUserCmd_CS cmd)
    {
        m_uEnterCopyID = cmd.copy_base_id;
        m_dicTeammateStatus.Clear();
        TeamDataManager teamData = DataManager.Manager<TeamDataManager>();
        m_dicTeammateStatus.Add(teamData.LeaderId, true);


        CopyDataBase db = GameTableManager.Instance.GetTableItem<CopyDataBase>(cmd.copy_base_id);
        if (db == null)
        {
            return;
        }
        if (!KHttpDown.Instance().SceneFileExists(db.mapId))
        {
            stAnsTeamCopyUserCmd_CS sendCmd = new stAnsTeamCopyUserCmd_CS();
            sendCmd.ans = false;
            sendCmd.copy_base_id = cmd.copy_base_id;
            NetService.Instance.Send(cmd);
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);

            //TipsManager.Instance.ShowTips(LocalTextType.Team_Limit_nindedongwurenshuyiman);//您的队伍人数已满
            return;
        }
        else
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.FBConfirmPanel);
        }

        m_uCountDown = GameTableManager.Instance.GetGlobalConfig<uint>("EnterCopyCountdown");
        TimerAxis.Instance().KillTimer(m_uCopyAskTeamTimerID, this);
        TimerAxis.Instance().SetTimer(m_uCopyAskTeamTimerID, 1000, this);
    }


    public void OnAnsTeamCopy(stAnsTeamCopyUserCmd_CS cmd)
    {
        if (m_dicTeammateStatus.ContainsKey(cmd.charid))
        {
            m_dicTeammateStatus[cmd.charid] = cmd.ans;
        }
        else
        {
            m_dicTeammateStatus.Add(cmd.charid, cmd.ans);
        }

        if (!cmd.ans)
        {
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es != null)
            {
                IPlayer player = es.FindEntity<IPlayer>(cmd.charid);
                if (player != null)
                {
                    TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Copy_Commond_xweitongyijinrufuben, player.GetName());

                }
            }
            return;
        }

        DispatchValueUpdateEvent(new ValueUpdateEventArgs(CopyDispatchEvent.RefreshStatus.ToString(), null, null));
        TeamDataManager teamData = DataManager.Manager<TeamDataManager>();
        if (teamData.MainPlayerIsLeader())
        {
            List<TeamMemberInfo> memList = teamData.TeamMemberList;
            bool bAllagree = true;
            foreach (var meminfo in memList)
            {
                if (m_dicTeammateStatus.ContainsKey(meminfo.id))
                {
                    bAllagree = m_dicTeammateStatus[meminfo.id];
                    if (!bAllagree)
                    {
                        break;
                    }
                }
                else
                {
                    bAllagree = false;
                    break;
                }
            }
            if (bAllagree)
            {
                //stRequestEnterCopyUserCmd_C copyCmd = new stRequestEnterCopyUserCmd_C();
                //copyCmd.copy_base_id = EnterCopyID;
                //NetService.Instance.Send(copyCmd);

                DataManager.Manager<ComBatCopyDataManager>().ReqEnterCopy(EnterCopyID);
            }
        }

    }

    /// <summary>
    /// 直接请求退出副本
    /// </summary>
    public void ReqExitCopy()
    {
        ComBatCopyDataManager copyData = DataManager.Manager<ComBatCopyDataManager>();
        if (copyData != null)
        {
            GameCmd.stExitCopyUserCmd_CS cmd = new GameCmd.stExitCopyUserCmd_CS();
            cmd.copy_base_id = copyData.EnterCopyID;
            NetService.Instance.Send(cmd);
        }
    }

    //退出副本
    public void OnExitCopy(stExitCopyUserCmd_CS cmd)
    {
        //退出副本
        Client.IMapSystem mapsys = ClientGlobal.Instance().GetMapSystem();
        if (mapsys != null)
        {
            mapsys.SetEnterZoneCallback(null);
        }

        ICopy copy = GetCopyByCopyID(m_uEnterCopyID);
        if (copy != null)
        {
            copy.ExitCopy();
        }

        m_uEnterCopyID = 0;
        m_uCopyCountDown = 0;
        m_nLastKillWave = 0;
        m_nLastTransmitWave = 0;
        m_dicEnterZoneStatus.Clear();
        m_dicSendEnterZoneTime.Clear();
        this.m_gainGoldInCopy = 0;//清副本中获得金币数

        this.CopyCDAndExitData = null;

        //清副本目标数据
        CleanCopyTargetData();

        if (TimerAxis.Instance().IsExist(m_uCopyFinishTimerID, this))
        {
            TimerAxis.Instance().KillTimer(m_uCopyFinishTimerID, this);
        }
        stCopyInfo info = new stCopyInfo();
        info.bShow = false;
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eShowCopyInfo, info);


        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eCopyExit, null);
        }

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.COMBOT_ENTER_EXIT, new Client.stCombotCopy() { copyid = cmd.copy_base_id, exit = true });

        IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            Engine.Utility.Log.Error("ExecuteCmd: ControllerSystem is null");
            return;
        }

        if (cs.GetCombatRobot().Status != CombatRobotStatus.STOP)
        {
            cs.GetCombatRobot().Stop();
        }

        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            player.SendMessage(EntityMessage.EntityCommand_StopMove, player.GetPos());
            CmdManager.Instance().Clear();//清除寻路
        }

        m_haveEnterWuXinZhen = false;
        m_haveShowCopyGuide = false;

        //退出副本标志
        m_bIsEnterCopy = false;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MessagePushPanel);
    }


    public void OnCompeleteCopy(stCompleteCopyUserCmd_S cmd)
    {
        m_uCopyCountDown = cmd.close_time;
        m_uCopyFinishCountDown = cmd.close_time;
        CopyDataBase cdb = GameTableManager.Instance.GetTableItem<CopyDataBase>(m_uEnterCopyID);
        if (cdb != null)
        {
            if (cdb.IsShowFinish)
            {
                UIPanelManager upMger = DataManager.Manager<UIPanelManager>();
                upMger.ShowPanel(PanelID.FBResult);
                TimerAxis.Instance().SetTimer(m_uCopyFinishTimerID, 1000, this, cmd.close_time);
                //延迟两秒显示副本奖励
                CoroutineMgr.Instance.DelayInvokeMethod(2f,()=>
                {
                    if (upMger.IsShowPanel(PanelID.FBResult))
                        upMger.HidePanel(PanelID.FBResult);
                    if (cdb.havePassReward != 0 && !string.IsNullOrEmpty(cdb.rewardList))
                    {
                        string[] rewardListStr = cdb.rewardList.Split(new char[] { ';' });
                        List<CommonAwardData> cmADDAtas = null;
                        CommonAwardData tempcmAD = null;
                        string[] tempRewardArray = null;
                        uint tempItemID = 0;
                        uint tempNum = 0;
                        if (null != rewardListStr && rewardListStr.Length > 0)
                        {
                            for(int i =0,max = rewardListStr.Length;i < max;i++)
                            {
                                if (string.IsNullOrEmpty(rewardListStr[i]))
                                    continue;
                                tempRewardArray = rewardListStr[i].Split(new char[] { '_' });
                                if (null == tempRewardArray || tempRewardArray.Length != 2)
                                    continue;
                                if (!string.IsNullOrEmpty(tempRewardArray[0]) && uint.TryParse(tempRewardArray[0].Trim(),out tempItemID)
                                    && !string.IsNullOrEmpty(tempRewardArray[1]) && uint.TryParse(tempRewardArray[1].Trim(), out tempNum)
                                    )
                                {
                                    tempcmAD = new CommonAwardData(tempItemID, tempNum);
                                    if (null == cmADDAtas)
                                    {
                                        cmADDAtas = new List<CommonAwardData>();
                                    }
                                    CopyInfo info = GetCopyInfoById(m_uEnterCopyID);
                                    if (!info.IsFinished)
                                    {
                                        cmADDAtas.Add(tempcmAD);
                                    }                               
                                }
                            }
                            //超过收益次数  还是显示奖励界面但是没有奖励道具
                            if (null != cmADDAtas)
                            {
                                upMger.ShowPanel(PanelID.FBPassAwardPanel, data: cmADDAtas);
                            }
                        }
                    }
                });
            }
        }
    }

    public void OnDieInCopy(stDieInCopyUserCmd_S cmd)
    {

    }

    public void OnCountDownCopy(stCountDownCopyUserCmd_S cmd)
    {
        uint cdTime = cmd.time;
        string des = System.Text.Encoding.UTF8.GetString(cmd.data);
        stCopyBattleInfo info = new stCopyBattleInfo { des = des, cd = cdTime };

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.NvWaPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.NvWaPanel, UIMsgID.eNvWaDesCD, info);
        }
    }

    public void OnCopyNumChange(stNumCopyUserCmd_S cmd)
    {
        ChangeCopyNum(cmd.type_id, cmd);
    }

    /// <summary>
    /// 进入副本（朝歌山单人）
    /// </summary>
    /// <param name="cmd"></param>
    public void ReqEnterCopy(uint copyId)
    {
        CopyDataBase copyData = GameTableManager.Instance.GetTableItem<CopyDataBase>(copyId);
        if (copyData == null)
        {
            return;
        }

        //地图检查,如果没有，前往下载
        if (!KHttpDown.Instance().SceneFileExists(copyData.mapId))
        {
            //打开下载界面
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
            return;
        }

        stRequestEnterCopyUserCmd_C sendCmd = new stRequestEnterCopyUserCmd_C();
        sendCmd.copy_base_id = copyId;
        NetService.Instance.Send(sendCmd);
    }

    /// <summary>
    /// 副本阶段奖励经验总和
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAllRewardExpCopy(stAllRewardExpCopyUserCmd_S cmd)
    {
        this.m_copyRewardExp = cmd.exp;

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.COPY_REWARDEXP, new Client.stCopyRewardExp() { exp = this.m_copyRewardExp });
    }

    public void OnAddGoldCopyMoney(stAddGoldCopyMoneyPropertyUserCmd_S cmd)
    {
        AddGoldInCopy(cmd.add_num);
    }

    #endregion


    #region ITimer

    public void OnTimer(uint uTimerID)
    {
        //模拟加载场景loading
        if (uTimerID == m_uCopyJumpTimerID)
        {
            ClientGlobal.Instance().GetMapSystem().Process += 0.05f;
            Engine.Utility.Log.LogGroup("ZCX", "....{0}", ClientGlobal.Instance().GetMapSystem().Process);
            if (ClientGlobal.Instance().GetMapSystem().Process >= 0.99f)
            {
                InvorkRobot();
            }
        }

        if (uTimerID == m_uCopyAskTeamTimerID)
        {
            if (m_uCountDown > 0)
            {
                m_uCountDown -= 1;
            }
        }
        else if (uTimerID == m_uCopyFinishTimerID)
        {
            if (m_uCopyFinishCountDown > 0)
            {
                m_uCopyFinishCountDown -= 1;
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Copy_Commond_fubenguanbitips, m_uCopyFinishCountDown);
            }

            return;
        }
        else if (uTimerID == m_uAutoStartFightTimerID)
        {
            StartAutoFight();
            TimerAxis.Instance().KillTimer(m_uAutoStartFightTimerID, this);
            return;
        }

        Dictionary<uint, float>.Enumerator iter = m_dicSendEnterZoneTime.GetEnumerator();
        List<uint> lstRemoveKeys = new List<uint>();
        while (iter.MoveNext())
        {
            if (Time.realtimeSinceStartup - iter.Current.Value > 5f)
            {
                if (m_dicEnterZoneStatus.ContainsKey((int)iter.Current.Key))
                {
                    m_dicEnterZoneStatus[(int)iter.Current.Key] = false;//超时没有返回置false 才能重新触发
                    lstRemoveKeys.Add(iter.Current.Key);
                }
            }
        }

        for (int i = 0; i < lstRemoveKeys.Count; i++)
        {
            m_dicSendEnterZoneTime.Remove(lstRemoveKeys[i]);
        }
    }

    #endregion

}

