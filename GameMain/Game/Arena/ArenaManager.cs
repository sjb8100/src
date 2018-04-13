using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Client;
using table;
using Engine.Utility;

public class ArenaBattleResult
{
    public uint result;     //胜负结果
    public uint rank;       //战斗结束后的排行
    public uint changeRank; //变化的排行
    public uint score;      //获得的积分

    public ArenaBattleResult(uint result, uint rank, uint changeRank, uint score)
    {
        this.result = result;
        this.rank = rank;
        this.changeRank = changeRank;
        this.score = score;
    }
}


/// <summary>
/// 武斗场进入战斗后的状态
/// </summary>
public enum ArenaBattleState
{
    eArenaBattleInit,      //初始化武斗场界面
    eArenaStartBattleCD,   //开始3、2、1
    eArenaBattleResult,    //战斗结算
    eArenaExit,            //退出武斗场
}


public class ArenaManager : BaseModuleData, IManager, ITimer, ICopy
{

    #region property

    //战斗倒计时
    private const string CONST_ARENA_BATTLECD_NAME = "ArenaFightTime";

    public static uint ArenaBattleCD
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_ARENA_BATTLECD_NAME);
        }
    }

    //挑战在线玩家  等待别人响应的时间
    private const string CONST_CHALLENGEREJECTTIME_NAME = "Max_Reject_Time";
    public static int ChallengeRejectTime
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<int>(CONST_CHALLENGEREJECTTIME_NAME);
        }
    }


    private const int ARENA_TIMERID = 2000;

    /// <summary>
    /// 挑战间隔ID
    /// </summary>
    private const int ARENA_CHALLENGEINTERVALID = 3000;

    ///// <summary>
    ///// 战斗倒计时 3、2、1
    ///// </summary>
    //private const int ARENASTARTBATTLECD_TIMERID = 4000;

    //本人排名
    private uint m_rank;
    public uint Rank
    {
        get { return m_rank; }
    }

    /// <summary>
    /// 战报信息
    /// </summary>
    private List<ArenaBattleLog> m_lstBattlelog = null;
    public List<ArenaBattleLog> BattlelogList
    {
        get { return m_lstBattlelog; }
    }

    /// <summary>
    /// 挑战次数
    /// </summary>
    private uint m_challengeTimes;
    public uint ChallengeTimes
    {
        get { return m_challengeTimes; }
    }

    //已经重置了几次挑战
    private uint m_resetChallengeTimes;
    public uint ResetChallengeTimes
    {
        get { return m_resetChallengeTimes; }
    }

    //每日最大挑战次数
    private uint m_maxTimes = 0;
    public uint MaxTimes
    {
        get { return m_maxTimes; }
    }

    //cd
    private int m_cd;
    public int CD
    {
        get { return m_cd; }
    }

    //已经清除了几次CD
    private uint m_clearCDTimes;
    public uint ClearCDTimes
    {
        get { return m_clearCDTimes; }
    }

    //是否进入武斗场
    private bool m_bEnterArena;
    public bool EnterArena
    {
        set
        {
            m_bEnterArena = value;
        }
        get
        {
            return m_bEnterArena;
        }
    }

    //开始战斗
    private bool m_bStartBattle = false;
    public bool StartBattle
    {
        get
        {
            return m_bStartBattle;
        }
    }

    /// <summary>
    /// 三个对手
    /// </summary>
    private List<OppuserData> m_lstRival = new List<OppuserData>(); //对手list
    public List<OppuserData> RivalList
    {
        get { return m_lstRival; }
    }


    /// <summary>
    /// 头三名
    /// </summary>
    List<TopUserData> m_lstTopUser = new List<TopUserData>();

    public List<TopUserData> TopUserList
    {
        get
        {
            return m_lstTopUser;
        }
    }

    /// <summary>
    /// 排行奖励
    /// </summary>
    List<ArenaRankRewardDatabase> m_lstArenaRankRewardDb;
    List<ArenaRankRewardDatabase> ArenaRankRewardDbList
    {
        get
        {
            return m_lstArenaRankRewardDb;
        }
    }

    /// <summary>
    /// 获得排行奖励的最低排行
    /// </summary>
    uint ArenaRankRewardMinRank
    {
        get
        {
            if (m_lstArenaRankRewardDb == null)
            {
                Engine.Utility.Log.Error("--->>> 没有数据！！！！！");
            }
            return m_lstArenaRankRewardDb.Count > 0 ? m_lstArenaRankRewardDb[m_lstArenaRankRewardDb.Count - 1].rank_floor : 0;
        }
    }

    public OppuserData ChallengeTarget { set; get; }


    ArenaBattleResult m_arenaBattleResult;
    public ArenaBattleResult ArenaBattleResult
    {
        get
        {
            if (m_arenaBattleResult != null)
                return m_arenaBattleResult;

            Engine.Utility.Log.Error("--->>>武斗场战斗结果 arenaBattleResult 为null !!!!");
            return null;
        }
    }

    /// <summary>
    /// 3、2、1、开始
    /// </summary>
    private float m_arenaBattleStartCd = 4.75f;
    public float arenaBattleStartCd
    {
        get
        {
            return m_arenaBattleStartCd;
        }
    }

    #endregion


    #region Interface
    public void Initialize()
    {
        m_lstRival = new List<OppuserData>();
        m_lstBattlelog = new List<ArenaBattleLog>();
        m_maxTimes = (uint)GameTableManager.Instance.GetGlobalConfig<int>("Arena_ChallTimes");
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_LOADSCENECOMPELETE, OnMainPlayerEnterMapEvent);
        m_lstArenaRankRewardDb = GameTableManager.Instance.GetTableList<ArenaRankRewardDatabase>();
    }
    /// <summary>
    /// 重置
    /// </summary>
    public void Reset(bool depthClearData = false)
    {
        m_bEnterArena = false;
        m_lstRival.Clear(); //对手list
        m_lstTopUser.Clear();
    }

    /// <summary>
    /// 每帧执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Process(float deltaTime)
    {
        if (m_bEnterArena)
        {
            if (m_bStartBattle == false && m_arenaBattleStartCd >0)
            {
                m_arenaBattleStartCd -= deltaTime;
                if (m_arenaBattleStartCd <= 0)
                {
                    m_bStartBattle = true;

                    //开启挂机
                    StartRobot();

                    //正式开启倒计时
                    DataManager.Manager<ComBatCopyDataManager>().CopyCountDown = ArenaManager.ArenaBattleCD;
                }
            }
        }
    }
    public void ClearData()
    {

    }
    //进入副本
    public void EnterCopy()
    {
        stCopyInfo info = new stCopyInfo();
        info.bShow = true;
        info.bShowBattleInfoBtn = false;
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eShowCopyInfo, info);
        DataManager.Manager<ComBatCopyDataManager>().CopyCDAndExitData = new CopyCDAndExitInfo { bShow = true, bShowBattleInfoBtn = false };

        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.TopBarPanel);
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ArenaPanel);
        m_bEnterArena = true;
        m_bStartBattle = false;

        Engine.Utility.EventEngine.Instance().AddVoteListener((int)GameVoteEventID.AUTORECOVER, DoVoteEvent);//吃药投票
    }

    //退出副本
    public void ExitCopy()
    {
        m_bEnterArena = false;
        m_bStartBattle = false;

        Engine.Utility.EventEngine.Instance().RemoveVoteListener((int)GameVoteEventID.AUTORECOVER, DoVoteEvent);//移除吃药投票

        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ArenaBattlePanel);
    }

    #endregion


    #region method

    void OnMainPlayerEnterMapEvent(int eventID, object param)
    {
        if (eventID == (int)GameEventID.SYSTEM_LOADSCENECOMPELETE)
        {

            if (m_bEnterArena)
            {
                Engine.Utility.Log.LogGroup(GameDefine.LogGroup.User_LCY, "--->>>1  进入地图了");

                //进入后打开UI遮罩，让玩家开始不能操作
                ArenaBattleState state = ArenaBattleState.eArenaBattleInit;
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ArenaBattlePanel, data: state);

                delayTime = 1;//延时时间
                TimerAxis.Instance().KillTimer(ARENA_TIMERID, this);
                TimerAxis.Instance().SetTimer(ARENA_TIMERID, 1000, this);
            }
        }
    }


    /// <summary>
    /// 清CD消耗
    /// </summary>
    /// <returns></returns>
    public uint GetClearCDCost()
    {
        uint times = m_clearCDTimes + 1;

        List<ArenaClearCDCostDataBase> list = GameTableManager.Instance.GetTableList<ArenaClearCDCostDataBase>();

        ArenaClearCDCostDataBase ClearCD = list.Find((ArenaClearCDCostDataBase data) => times == data.times);

        if (times > list.Count) return list[list.Count - 1].cost;

        return ClearCD.cost;
    }

    /// <summary>
    /// 重置挑战次数消耗
    /// </summary>
    /// <returns></returns>
    public uint GetResetChallengeTimesCost()
    {
        uint times = m_resetChallengeTimes + 1;

        List<ArenaResetCostDataBase> list = GameTableManager.Instance.GetTableList<ArenaResetCostDataBase>();

        ArenaResetCostDataBase resetChallenge = list.Find((ArenaResetCostDataBase data) => times == data.times);

        if (times > list.Count) return list[list.Count - 1].cost;

        return resetChallenge.cost;
    }

    /// <summary>
    /// 获取当前排行奖励
    /// </summary>
    /// <param name="rankReward"></param>
    /// <returns></returns>
    public bool TryGetRankRewardByRank(out ArenaRankRewardDatabase rankReward)
    {
        rankReward = m_lstArenaRankRewardDb.Find((data) => { return m_rank >= data.rank_floor && m_rank <= data.rank_cap; });
        if (rankReward != null)
        {
            return true;
        }
        else
        {
            return false;
        }


    }

    /// <summary>
    /// 开启挂机
    /// </summary>
    void StartRobot()
    {
        Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
        if (cs != null)
        {
            Client.ICombatRobot robot = cs.GetCombatRobot();
            if (robot != null) //&& robot.Status != Client.CombatRobotStatus.STOP)
            {
                robot.Start();
            }
        }
    }

    #endregion


    #region Net
    /// <summary>
    /// 开启武斗场请求
    /// </summary>
    public void ReqOpenArena()
    {
        stMainArenaUserCmd_C cmd = new stMainArenaUserCmd_C();

        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 武斗场主界面信息
    /// </summary>
    /// <param name="cmd"></param>
    public void OnMainArenaRes(stReturnMainArenaUserCmd_S cmd)
    {
        m_rank = cmd.rank;
        m_challengeTimes = cmd.challenge_times;
        m_resetChallengeTimes = cmd.reset_times;
        m_cd = (int)cmd.challenge_cd;
        m_clearCDTimes = cmd.resetcd_times;

        //挑战CD
        TimerAxis.Instance().KillTimer(ARENA_CHALLENGEINTERVALID, this);
        if (m_cd > 0)
        {
            TimerAxis.Instance().SetTimer(ARENA_CHALLENGEINTERVALID, 1000, this);
        }

        // DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ArenaPanel);

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ArenaPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.ArenaPanel, UIMsgID.eArenaMainData, null);
        }
    }

    public void ReqArenaTopThree()
    {
        stRequestTopUserArenaUserCmd_C cmd = new stRequestTopUserArenaUserCmd_C();
        NetService.Instance.Send(cmd);
    }

    // 返回前三名数据
    public void OnArenaTopThree(stReturnTopUserArenaUserCmd_S cmd)
    {
        //前三名
        m_lstTopUser.Clear();
        for (int i = 0; i < cmd.topuserdata.Count; i++)
        {
            m_lstTopUser.Add(cmd.topuserdata[i]);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ArenaPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.ArenaPanel, UIMsgID.eArenaTopThree, null);
        }
    }

    public void ReqArenaRivalThree()
    {
        stChangeOppUserArenaUserCmd_C cmd = new stChangeOppUserArenaUserCmd_C();
        NetService.Instance.Send(cmd);
    }

    // 返回三个对手信息	
    public void OnArenaRivalThree(stReturnOppUserArenaUserCmd_S cmd)
    {
        //挑战的三个人
        m_lstRival.Clear();
        for (int i = 0; i < cmd.oppuserdata.Count; i++)
        {
            m_lstRival.Add(cmd.oppuserdata[i]);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ArenaPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.ArenaPanel, UIMsgID.eArenaRivalThree, null);
        }
    }

    /// <summary>
    /// 刷新挑战CD请求
    /// </summary>

    public void ReqRefreshCD()
    {
        stRefreshCDArenaUserCmd_CS cmd = new stRefreshCDArenaUserCmd_CS();
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 刷新挑战CD响应
    /// </summary>
    public void OnRefreshChallengeCDRes(stRefreshCDArenaUserCmd_CS cmd)
    {
        m_cd = 0;
        TimerAxis.Instance().KillTimer(ARENA_CHALLENGEINTERVALID, this);

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ArenaPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.ArenaPanel, UIMsgID.eArenaCDUpdate, null);
        }

        if (ChallengeTarget != null)
        {
            ReqChallengeInvite(ChallengeTarget.id, ChallengeTarget.name, ChallengeTarget.rank, ChallengeTarget.online_state);
        }
    }

    /// <summary>
    /// 刷新挑战次数请求
    /// </summary>
    /// <param name="cmd"></param>
    public void ReqRefreshChallengeTimes()
    {
        stRefreshTimesArenaUserCmd_CS cmd = new stRefreshTimesArenaUserCmd_CS();
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 刷新挑战次数响应
    /// </summary>
    /// <param name="cmd"></param>
    public void OnRefreshChallengeTimesRes(stRefreshTimesArenaUserCmd_CS cmd)
    {
        m_challengeTimes = (uint)GameTableManager.Instance.GetGlobalConfig<int>("Arena_ChallTimes");

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ArenaPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.ArenaPanel, UIMsgID.eArenaTimesUpdate, null);
        }

        if (ChallengeTarget != null)
        {
            ReqChallengeInvite(ChallengeTarget.id, ChallengeTarget.name, ChallengeTarget.rank, ChallengeTarget.online_state);
        }
    }

    /// <summary>
    /// 战报请求
    /// </summary>
    public void ReqBattlelog()
    {
        stReportArenaUserCmd_C cmd = new stReportArenaUserCmd_C();
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 战报返回
    /// </summary>
    /// <param name="cmd"></param>
    public void OnBattlelogRes(stReportArenaUserCmd_S cmd)
    {
        m_lstBattlelog = cmd.arena_log;

        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ArenaBattlelogPanel);
    }

    /// <summary>
    /// 发起挑战
    /// </summary>
    /// <param name="targetID"></param>
    /// <param name="name"></param>
    public void ReqChallengeInvite(uint targetID, string name, uint targetRank, bool onLine)
    {
        // 大v说了武斗场id不变151.
        if (!KHttpDown.Instance().SceneFileExists(151))
        {
            //打开下载界面
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
            return;
        }

        MainPlayStop();
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        stChallengeInviteArenaUserCmd_CS cmd = new stChallengeInviteArenaUserCmd_CS();
        cmd.offensive_id = mainPlayer.GetID();
        cmd.offensive_name = mainPlayer.GetName();
        cmd.offender_rank = m_rank;
        cmd.defensive_id = targetID;
        cmd.defensive_name = name;
        cmd.defendser_rank = targetRank;
        NetService.Instance.Send(cmd);

        if (onLine)  //在线，会弹出等待框
        {
            WaitPanelShowData waitData = new WaitPanelShowData();
            waitData.type = WaitPanelType.ArenaChallenge;
            waitData.cdTime = ChallengeRejectTime;
            waitData.des = "挑战邀请中，请稍后...";
            waitData.timeOutDel = delegate { DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel); };
            waitData.useBoxMask = false;

            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonWaitingPanel, data: waitData);
        }
    }

    void MainPlayStop()
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            player.SendMessage(EntityMessage.EntityCommand_StopMove, player.GetPos());
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, false);//关闭自动寻路中
        }
        Controller.CmdManager.Instance().Clear();//清除寻路
    }

    /// <summary>
    /// 发起人收到对方同意或拒绝消息
    /// </summary>
    /// <param name="cmd"></param>
    public void OnInviteResultRes(stInviteResultArenaUserCmd_CS cmd)
    {
        // DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ArenaWaitingPanel);
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
    }


    /// <summary>
    /// 玩家已经进入地图
    /// </summary>
    public void ReqEnterArenaMap()
    {
        stEnterMapArenaUserCmd_CS cmd = new stEnterMapArenaUserCmd_CS();
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 进入竞技场地图了
    /// </summary>
    /// <param name="cmd"></param>
    public void OnEnterArenaMapRes(stEnterMapArenaUserCmd_CS cmd)
    {
        ChallengeTarget = null;

        object data = true;
        Engine.Utility.Log.LogGroup(GameDefine.LogGroup.User_LCY, "--->>>2  打开武斗场战斗UI ");

        ArenaBattleState state = ArenaBattleState.eArenaBattleInit;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ArenaBattlePanel, data: state);
        //DataManager.Manager<UIPanelManager>().SendMsg(PanelID.ArenaBattlePanel, UIMsgID.eArenaBattleInit, null);

    }

    /// <summary>
    /// 武斗场吃药投票
    /// </summary>
    /// <param name="nEventID"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    bool DoVoteEvent(int nEventID, object param)
    {
        if (nEventID == (int)GameVoteEventID.AUTORECOVER)
        {
            return false;
        }
        return true;
    }

    public void OnStartBattleCDRes(stStartBattleArenaUserCmd_S cmd)
    {
        Engine.Utility.Log.LogGroup(GameDefine.LogGroup.User_LCY, "--->>>3  开始战斗3、2、1 ");

        uint CDTime = GameTableManager.Instance.GetGlobalConfig<uint>("ArenaFightTime"); //倒计时 
        object data = CDTime;

        m_arenaBattleStartCd = 4.75f;

        ArenaBattleState state = ArenaBattleState.eArenaStartBattleCD;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ArenaBattlePanel, data: state);
    }

    /// <summary>
    /// 通知双方战斗结束
    /// </summary>
    /// <param name="cmd"></param>
    public void OnBattleEnd(stBattleFinalArenaUserCmd_S cmd)
    {
        m_arenaBattleResult = new ArenaBattleResult(cmd.result, cmd.cur_rank, cmd.change, cmd.score);
        this.m_rank = cmd.cur_rank;

        if (m_bEnterArena)
        {
            ArenaBattleState state = ArenaBattleState.eArenaBattleResult;
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ArenaBattlePanel, data: state);
        }
    }
    #endregion


    uint delayTime = 1;
    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == ARENA_TIMERID)
        {
            delayTime--;
            if (delayTime == 0)
            {
                ReqEnterArenaMap();
                TimerAxis.Instance().KillTimer(ARENA_TIMERID, this);
            }
        }

        if (uTimerID == ARENA_CHALLENGEINTERVALID)
        {
            if (m_cd > 0)
            {
                m_cd--;
            }

            if (m_cd <= 0)
            {
                TimerAxis.Instance().KillTimer(ARENA_CHALLENGEINTERVALID, this);
            }
        }

        //if (uTimerID == ARENASTARTBATTLECD_TIMERID)
        //{
        //    if (m_arenaBattleStartCd >= 0)
        //    {
        //        m_arenaBattleStartCd -= 0.05f;
        //    }
        //    else
        //    {
        //        //开启挂机
        //        StartRobot();

        //        m_bStartBattle = true;

        //        //正式开启倒计时
        //        DataManager.Manager<ComBatCopyDataManager>().CopyCountDown = ArenaManager.ArenaBattleCD;

        //        TimerAxis.Instance().KillTimer(ARENASTARTBATTLECD_TIMERID, this);
        //    }
        //}
    }
}

