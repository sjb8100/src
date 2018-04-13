using Engine.Utility;
using GameCmd;
using System;
using System.Collections.Generic;

public class LocalCampSignInfo
{
    //状态
    private GameCmd.eCFState m_emState = GameCmd.eCFState.CFS_Close;
    public GameCmd.eCFState State
    {
        get
        {
            return m_emState;
        }
    }

    private bool m_bSign = false;
    public bool Sign
    {
        get
        {
            return m_bSign;
        }
        set
        {
            m_bSign = value;
        }
    }



    //场次
    private uint m_uIndex = 0;
    public uint Index
    {
        get
        {
            return m_uIndex;
        }
    }

    //报名时间
    private string m_strSignTime = "";
    public string SignTime
    {
        get
        {
            return m_strSignTime;
        }
    }

    //开始时间
    private string m_strStartTime = "";
    public string StartTime
    {
        get
        {
            return m_strStartTime;
        }
    }

    //结束时间
    private string m_strEndTime = "";
    public string EndTime
    {
        get
        {
            return m_strEndTime;
        }
    }

    //阵营战持续时间（单位/秒）
    private int m_iCampDuration = 0;
    public int CampDuration
    {
        get
        {
            return m_iCampDuration;
        }
    }

    private List<uint> m_lstSignNum = new List<uint>();
    /// <summary>
    /// 根据当前角色所处等级段获取报名人数
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public uint GetSignNumByCampSection(int index)
    {
        return m_lstSignNum.Count > index ? m_lstSignNum[index] : 0;
    }

    /// <summary>
    /// 更新数量
    /// </summary>
    /// <param name="index"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    public void UpdateSignNumByCampSection(int index, uint num)
    {
        if (m_lstSignNum.Count > index)
        {
            m_lstSignNum[index] = num;
        }
    }

    public void UpdateData(GameCmd.stCampSignInfo info)
    {
        m_uIndex = info.index;
        m_emState = (GameCmd.eCFState)info.state;
        TimeSpan ts = new TimeSpan(0, 0, (int)info.sign_time);
        string tempStr1 = string.Format("{0}:{1}", ts.Hours.ToString("D2"), ts.Minutes.ToString("D2"));
        ts = new TimeSpan(0, 0, (int)info.begin_time);
        string tempStr2 = string.Format("{0}:{1}", ts.Hours.ToString("D2"), ts.Minutes.ToString("D2"));
        m_strSignTime = string.Format("{0}-{1}", tempStr1, tempStr2);
        m_strStartTime = string.Format("{0}:{1}", ts.Hours.ToString("D2"), ts.Minutes.ToString("D2"));

        ts = new TimeSpan(0, 0, (int)info.end_time);
        m_strEndTime = string.Format("{0}:{1}", ts.Hours.ToString("D2"), ts.Minutes.ToString("D2"));
        //更新
        m_lstSignNum.Clear();
        if (null != info.num)
            m_lstSignNum.AddRange(info.num);
        //持续时长
        m_iCampDuration = Math.Max(0, (int)info.end_time - (int)info.begin_time);
    }

    /// <summary>
    /// 该场阵营战剩余时间（进行中不为0，其他为0）
    /// </summary>
    /// <returns></returns>
    public int CampLeftSeconds()
    {
        int leftTimes = 0;
        if (m_emState == GameCmd.eCFState.CFS_Fighting)
        {

        }
        return leftTimes;
    }

    private LocalCampSignInfo()
    {

    }

    public static LocalCampSignInfo Create(GameCmd.stCampSignInfo info)
    {
        LocalCampSignInfo ci = new LocalCampSignInfo();
        ci.UpdateData(info);
        return ci;
    }
}

//结算
public class CampCombatResultInfo
{
    public CampCombatPlayerInfo m_MyCampCombatInfo = new CampCombatPlayerInfo();
    /// <summary>
    /// 魔
    /// </summary>
    public CampCombatResult m_camp_Red = new CampCombatResult();
    /// <summary>
    /// 神
    /// </summary>
    public CampCombatResult m_camp_Green = new CampCombatResult();
    public class CampCombatResult
    {
        public GameCmd.eCamp nType = GameCmd.eCamp.CF_None;
        public uint nKillBossNum = 0;
        public uint nReliveNum = 0;
        public uint nScore = 0;
        public List<CampCombatPlayerInfo> m_lstCampCombatPlayers = new List<CampCombatPlayerInfo>();

        public void Reset()
        {
            m_lstCampCombatPlayers.Clear();
            nType = 0;
            nKillBossNum = 0;
            nReliveNum = 0;
            nScore = 0;
            nType = GameCmd.eCamp.CF_None;
        }
    }

    public class CampCombatPlayerInfo
    {
        public GameCmd.eCamp camp;
        public uint userid;
        public uint nRank;
        public string strName;
        public uint nScore;
        public uint nKill;
        public uint nDead;
        public uint nAssist;

        public void Reset()
        {
            camp = GameCmd.eCamp.CF_None;
            userid = 0;
            nRank = 0;
            strName = "";
            nScore = 0;
            nKill = 0;
            nDead = 0;
            nAssist = 0;
        }
    }

    public void Reset()
    {
        m_MyCampCombatInfo.Reset();
        m_camp_Red.Reset();
        m_camp_Green.Reset();
    }
}

class CampCombatManager : BaseModuleData, IManager, IGlobalEvent, ICopy, ITimer
{
    private const int TIME_ID = 893;

    private int m_campStartTipsCount = 3;

    //是否已经分配阵营
    private bool m_bAssignCamp = false;
    public bool AssignCamp
    {
        get
        {
            return m_bAssignCamp;
        }
    }

    //战况列表是否可用
    public bool CampBattleEnable
    {
        get
        {
            return m_bAssignCamp
                || m_CampCombatResultInfo.m_camp_Green.m_lstCampCombatPlayers.Count != 0
                || m_CampCombatResultInfo.m_camp_Red.m_lstCampCombatPlayers.Count != 0;
        }
    }
    //战斗结算
    CampCombatResultInfo m_CampCombatResultInfo = null;
    public CampCombatResultInfo CampCombatResultData
    {
        get { return m_CampCombatResultInfo; }
    }
    private ValueUpdateEventArgs m_valueUpdateEventArgs = null;

    //剩余参加次数
    public uint LeftJoinTimes { get; set; }

    public uint FightingIndex { get; set; }
    public bool isEnterScene { get; set; }

    public const string CAMP_TOTAL_TIMES_NAME = "CF_CampFightTimes";
    public static int CampFightTimes
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<int>(CAMP_TOTAL_TIMES_NAME);
        }
    }

    //当前阵营战持续时长
    private uint m_uCampFightDuration = 0;
    public uint CampFightDuration
    {
        get
        {
            return m_uCampFightDuration;
        }
    }

    /// <summary>
    /// 阵营战已用时长
    /// </summary>
    public int CampFightUseTime
    {
        get
        {
            LocalCampSignInfo local = GetCurLocalCampSignInfo();
            if (null != local)
            {
                return Math.Max(0, local.CampDuration - (int)DataManager.Manager<ComBatCopyDataManager>().CopyCountDown);
            }
            return 0;
        }
    }

    /// <summary>
    /// 当天进入阵营站次数
    /// </summary>
    uint m_enterCampTimes = 0;

    public uint EnterCampTimes
    {
        get
        {
            return m_enterCampTimes;
        }
    }

    /// <summary>
    /// 每天阵营战最大次数
    /// </summary>
    public uint CampCopyMaxNum
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>("CF_JoinTimesPerPlayer");
        }
    }

    /// <summary>
    /// 每日阵营战剩余次数
    /// </summary>
    public uint CampCopyLeftNum
    {
        get
        {
            return this.CampCopyMaxNum - this.m_enterCampTimes;
        }
    }

    /// <summary>
    /// 更新阵营战剩余场次
    /// </summary>
    /// <param name="alreadyDoNum"></param>
    public void UpdateCampFightLeftTimes(int alreadyDoNum)
    {
        uint lastleftJoinTimes = LeftJoinTimes;
        LeftJoinTimes = (uint)Math.Max(0, CampFightTimes - alreadyDoNum);
        if (lastleftJoinTimes != LeftJoinTimes)
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCAMPLEFTTIMESREFRESH);
        }
    }

    //  public bool 
    public void Initialize()
    {
        m_valueUpdateEventArgs = new ValueUpdateEventArgs();
        m_CampCombatResultInfo = new CampCombatResultInfo();
        m_dicLocalCampInfos = new Dictionary<uint, LocalCampSignInfo>();
        RegisterGlobalEvent(true);
    }

    public void Reset(bool depthClearData = false)
    {
        m_CampCombatResultInfo.Reset();
        isEnterScene = false;
        if (depthClearData)
        {
            m_dicLocalCampInfos.Clear();
            //角色所处等级段
            m_iCampSectionIndex = 0;
        }
    }

    public void Process(float deltaTime)
    {
        // ProccessCampNotice();
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
        SetCampNpOnTrigger(m_CampCombatResultInfo.m_MyCampCombatInfo.camp);

        Engine.Utility.EventEngine.Instance().AddVoteListener((int)Client.GameVoteEventID.AUTOAtOnceRECOVER, DoVoteEvent);//吃瞬药投票
    }

    /// <summary>
    /// 设置阵营
    /// </summary>
    public void OnSetCamp()
    {
        m_bAssignCamp = true;

        //重置阵营战战前提醒
        ResetCampNotice();
        //设置阵营战斗打响,弹出战斗提示
        DataManager.Manager<EffectDisplayManager>().AddTips(CampNoticeStart);
    }

    //退出副本
    public void ExitCopy()
    {
        isEnterScene = false;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.CampFightingPanel))
        {
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CampFightingPanel);
        }

        OnQuitCamp();

        Engine.Utility.EventEngine.Instance().RemoveVoteListener((int)Client.GameVoteEventID.AUTOAtOnceRECOVER, DoVoteEvent);//移除吃瞬药投票
    }

    /// <summary>
    /// 吃瞬药投票
    /// </summary>
    /// <param name="nEventID"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    bool DoVoteEvent(int nEventID, object param)
    {
        if (nEventID == (int)Client.GameVoteEventID.AUTOAtOnceRECOVER)
        {
            return false;
        }
        return true;
    }

    #region (CampNotice)阵营战战斗站前提醒
    /// <summary>
    /// 开启阵营战战前提醒
    /// </summary>
    private void EnableCampNotice()
    {
        ResetCampNotice();
        m_bCampNoticeEnable = true;

        //新的方式  阵营战战前提醒
        this.m_campStartTipsCount = 3;
        TimerAxis.Instance().SetTimer(TIME_ID, 1000, this);
    }

    /// <summary>
    /// 重置阵营战站前提醒
    /// </summary>
    private void ResetCampNotice()
    {
        m_bCampNoticeEnable = false;
        m_fLastPlayCampNoticeTime = 0;
        m_curPlayCampNoticeIndex = 0;
    }

    private void ResetCampNoticeTips()
    {

    }

    //阵营战是否可用
    private bool m_bCampNoticeEnable = false;
    public bool CampNoticeEnable
    {
        get
        {
            return m_bCampNoticeEnable && null != CampNoticeContent && CampNoticeContent.Length > 0;
        }
    }
    //阵营战战站前提醒间隔时间
    private const float CAMPNOICE_GAP_TIME = 1.0f;
    //最近一次显示阵营战的时间
    private float m_fLastPlayCampNoticeTime = 0;
    //前一个阵营战战前提醒索引
    private int m_curPlayCampNoticeIndex = 0;
    //阵营战站前提醒内容配置名称
    private const string CAMPNOTICE_KEYS_NAME = "CampnoticeKeys";
    //阵营战站前提醒内容
    private string[] m_campNoticeContent = null;
    public string[] CampNoticeContent
    {
        get
        {
            if (null == m_campNoticeContent)
            {
                TextManager tmgr = DataManager.Manager<TextManager>();
                m_campNoticeContent = new string[3];
                m_campNoticeContent[0] = tmgr.GetLocalText(LocalTextType.Camp_Notice_Begin1);
                m_campNoticeContent[1] = tmgr.GetLocalText(LocalTextType.Camp_Notice_Begin2);
                m_campNoticeContent[2] = tmgr.GetLocalText(LocalTextType.Camp_Notice_Begin3);
            }
            return m_campNoticeContent;
        }
    }

    /// <summary>
    /// 阵营战开始提示
    /// </summary>
    public string CampNoticeStart
    {
        get
        {
            return DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Camp_Notice_Start);
        }
    }

    /// <summary>
    /// 执行阵营战站前提醒
    /// </summary>
    private void ProccessCampNotice()
    {
        if (!CampNoticeEnable)
        {
            return;
        }

        float timeSineStartUp = UnityEngine.Time.realtimeSinceStartup;
        if (timeSineStartUp - m_fLastPlayCampNoticeTime >= CAMPNOICE_GAP_TIME)
        {
            m_fLastPlayCampNoticeTime = timeSineStartUp;
            if (m_curPlayCampNoticeIndex >= 0 && m_curPlayCampNoticeIndex < CampNoticeContent.Length)
                DataManager.Manager<EffectDisplayManager>().AddTips(CampNoticeContent[m_curPlayCampNoticeIndex]);
            int nextIndex = m_curPlayCampNoticeIndex + 1;
            if (nextIndex < 0 || nextIndex >= CampNoticeContent.Length)
                nextIndex = 0;
            m_curPlayCampNoticeIndex = nextIndex;
        }
    }

    #endregion

    public void OnQuitCamp()
    {
        m_CampCombatResultInfo.Reset();
        FightingIndex = 0;

        m_bAssignCamp = false;
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CampWarResultPanel);

        isEnterScene = false;
        //Engine.Utility.EventEngine.Instance().RemoveVoteListener((int)Client.GameVoteEventID.TASK_VISITNPC_COLLECT, OnVote);
        ResetCampNotice();

    }

    public void OnEnterCamp(GameCmd.stOnEnterCampUserCmd_S cmd)
    {
        isEnterScene = true;
        //Engine.Utility.EventEngine.Instance().AddVoteListener((int)Client.GameVoteEventID.TASK_VISITNPC_COLLECT, OnVote);

        DataManager.Manager<ComBatCopyDataManager>().CopyCountDown = cmd.time_leave;
        SetMyCamp(cmd.camp);
        SetCampNpOnTrigger(cmd.camp);

        //进入阵营战阵营战战前提醒
        EnableCampNotice();
    }

    private List<Client.INPC> m_lstNpc = new List<Client.INPC>();
    void SetCampNpOnTrigger(GameCmd.eCamp mycamp)
    {
        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es != null)
        {
            es.FindAllEntity<Client.INPC>(ref m_lstNpc);
            for (int i = 0; i < m_lstNpc.Count; i++)
            {
                Client.INPC npc = m_lstNpc[i];
                GameCmd.eCamp camp = (GameCmd.eCamp)npc.GetProp((int)Client.CreatureProp.Camp);
                if (mycamp != camp)
                {
                    CampNpcOnTrigger callback = new CampNpcOnTrigger();
                    npc.SetCallback(callback);
                    UnityEngine.Debug.Log("SetCallback : " + npc.GetName());
                }
            }
            m_lstNpc.Clear();
        }

    }

    #region  Sign

    public void UpdateIfSignUp(List<uint> indexs)
    {
        //if (m_lstCampSignUpInfo == null)
        //{
        //    Engine.Utility.Log.Error("m_lstCampSignUpInfo   is null");
        //    return;
        //}

        //for (int i = 0; i < m_lstCampSignUpInfo.Count; i++)
        //{
        //    m_lstCampSignUpInfo[i].bSign = false;
        //}
        //for (int i = 0; i < indexs.Count; i++)
        //{
        //    for (int k = 0; k < m_lstCampSignUpInfo.Count; k++)
        //    {
        //        if (m_lstCampSignUpInfo[k].nIndex == (int)indexs[i])
        //        {
        //            m_lstCampSignUpInfo[k].bSign = true;
        //        }
        //    }
        //}

        //ModelEventArgs arg = new ModelEventArgs("LeftJoinTimes", LeftJoinTimes);
        //DispatchModelEvent(arg);
    }


    /// <summary>
    /// 刷新阵营战信息
    /// </summary>
    /// <param name="msg"></param>
    public void UpdateCampUserBattleData(GameCmd.stCampInfoCampUserCmd_S msg)
    {
        //1:请求我方信息，2请求敌方信息
        UpdateCampUsersInfo(GameCmd.eCamp.CF_Green, msg.camp_info_green);
        UpdateCampUsersInfo(GameCmd.eCamp.CF_Red, msg.camp_info_red);
        RefreshMyInfo(msg.my_info);
        DispatchValueEvent("RefreshBattleData", null, null);
    }

    /// <summary>
    ///发送数据更新事件
    /// </summary>
    /// <param name="key"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private void DispatchValueEvent(string key, object oldValue, object newValue)
    {
        m_valueUpdateEventArgs.Reset();
        m_valueUpdateEventArgs.key = key;
        m_valueUpdateEventArgs.oldValue = oldValue;
        m_valueUpdateEventArgs.newValue = newValue;
        DispatchValueUpdateEvent(m_valueUpdateEventArgs);
    }


    /// <summary>
    /// 刷新阵营战信息
    /// </summary>
    /// <param name="ecamp"></param>
    /// <param name="lstInfo"></param>
    public void UpdateCampUsersInfo(GameCmd.eCamp ecamp, GameCmd.stCampInfo campInfo)
    {
        CampCombatResultInfo.CampCombatResult campResultInfo = null;
        if (ecamp == GameCmd.eCamp.CF_Green)
        {
            campResultInfo = m_CampCombatResultInfo.m_camp_Green;
        }
        else if (ecamp == GameCmd.eCamp.CF_Red)
        {
            campResultInfo = m_CampCombatResultInfo.m_camp_Red;
        }

        if (campResultInfo != null)
        {
            campResultInfo.nKillBossNum = campInfo.kill_boss;
            campResultInfo.nReliveNum = campInfo.relive_num;
            campResultInfo.nScore = campInfo.score;
            campResultInfo.nType = ecamp;

            List<GameCmd.stCampMemberInfo> lstInfo = campInfo.member_info;
            campResultInfo.m_lstCampCombatPlayers.Clear();

            for (int k = 0; k < lstInfo.Count; k++)
            {
                CampCombatResultInfo.CampCombatPlayerInfo playerInfo = new CampCombatResultInfo.CampCombatPlayerInfo();
                GameCmd.stCampMemberInfo info = lstInfo[k];
                playerInfo.userid = info.userid;
                playerInfo.nAssist = info.assist;
                playerInfo.nDead = info.killed;
                playerInfo.nKill = info.kill;
                playerInfo.nRank = info.rank;
                playerInfo.strName = info.name;
                playerInfo.nScore = (uint)info.score;
                playerInfo.camp = info.camp;
                campResultInfo.m_lstCampCombatPlayers.Add(playerInfo);
            }

            SortCampMemberRank(ref campResultInfo.m_lstCampCombatPlayers);
        }
    }

    /// <summary>
    /// 排名信息
    /// </summary>
    /// <param name="infos"></param>
    private void SortCampMemberRank(ref List<CampCombatResultInfo.CampCombatPlayerInfo> infos)
    {
        if (null == infos)
        {
            return;
        }

        //1、优先按照得分高优先；
        //2、得分一样则按照杀敌数优先排；
        //3、杀敌数一样则按照助攻数优先排；
        //4、助攻数一样则按照死亡数降序排；
        infos.Sort((left, right) =>
            {
                if (left.nScore != right.nScore)
                {
                    return (int)right.nScore - (int)left.nScore;
                }
                else if (left.nKill != right.nKill)
                {
                    return (int)right.nKill - (int)left.nKill;
                }
                else if (left.nAssist != right.nAssist)
                {
                    return (int)right.nAssist - (int)left.nAssist;
                }
                else if (left.nDead != right.nDead)
                {
                    return (int)left.nDead - (int)right.nDead;
                }
                return 0;
            });

        //重设排名
        for (int i = 0, max = infos.Count; i < max; i++)
        {
            infos[i].nRank = (uint)i + 1;
        }
    }

    public void RefreshMyInfo(GameCmd.stCampMemberInfo info)
    {
        if (m_CampCombatResultInfo != null && info != null)
        {
            m_CampCombatResultInfo.m_MyCampCombatInfo.userid = info.userid;
            m_CampCombatResultInfo.m_MyCampCombatInfo.nAssist = info.assist;
            m_CampCombatResultInfo.m_MyCampCombatInfo.nDead = info.killed;
            m_CampCombatResultInfo.m_MyCampCombatInfo.nKill = info.kill;
            m_CampCombatResultInfo.m_MyCampCombatInfo.nRank = info.rank;
            m_CampCombatResultInfo.m_MyCampCombatInfo.strName = info.name;
            m_CampCombatResultInfo.m_MyCampCombatInfo.nScore = (uint)info.score;
            m_CampCombatResultInfo.m_MyCampCombatInfo.camp = info.camp;
        }
    }

    public void SetMyCamp(GameCmd.eCamp camp)
    {
        if (m_CampCombatResultInfo != null)
        {
            m_CampCombatResultInfo.m_MyCampCombatInfo.camp = camp;
        }
    }

    public void RefreshFighingInfo(GameCmd.stCampKillInfoCampUserCmd_S cmd)
    {
        int index = (int)GameCmd.eCamp.CF_Green - 1;

        m_CampCombatResultInfo.m_camp_Green.nKillBossNum = (null != cmd.kill_boss && cmd.kill_boss.Count > index) ? cmd.kill_boss[index] : 0;
        m_CampCombatResultInfo.m_camp_Green.nScore = (null != cmd.score && cmd.score.Count > index) ? cmd.score[index] : 0;
        m_CampCombatResultInfo.m_camp_Green.nReliveNum = (null != cmd.relive_num && cmd.relive_num.Count > index) ? cmd.relive_num[index] : 0;

        index = (int)GameCmd.eCamp.CF_Red - 1;
        m_CampCombatResultInfo.m_camp_Red.nKillBossNum = (null != cmd.kill_boss && cmd.kill_boss.Count > index) ? cmd.kill_boss[index] : 0;
        m_CampCombatResultInfo.m_camp_Red.nScore = (null != cmd.score && cmd.score.Count > index) ? cmd.score[index] : 0;
        m_CampCombatResultInfo.m_camp_Red.nReliveNum = (null != cmd.relive_num && cmd.relive_num.Count > index) ? cmd.relive_num[index] : 0;

        DispatchValueEvent("CampKillInfo", null, cmd);
    }

    public void RefreshMemberInfo(GameCmd.stRefreshMemberInfoCampUserCmd_S cmd)
    {
        if (m_CampCombatResultInfo == null)
        {
            return;
        }

        CampCombatResultInfo.CampCombatResult campCombatResult = m_CampCombatResultInfo.m_camp_Green;
        if (campCombatResult != null)
        {
            for (int i = 0; i < campCombatResult.m_lstCampCombatPlayers.Count; i++)
            {
                if (cmd.userid == campCombatResult.m_lstCampCombatPlayers[i].userid)
                {
                    campCombatResult.m_lstCampCombatPlayers[i].nKill = cmd.kill;
                    campCombatResult.m_lstCampCombatPlayers[i].nAssist = cmd.assist;
                    campCombatResult.m_lstCampCombatPlayers[i].nDead = cmd.killed;
                    campCombatResult.m_lstCampCombatPlayers[i].nScore = cmd.score;
                }
            }
        }

        campCombatResult = m_CampCombatResultInfo.m_camp_Red;
        if (campCombatResult != null)
        {
            for (int i = 0; i < campCombatResult.m_lstCampCombatPlayers.Count; i++)
            {
                if (cmd.userid == campCombatResult.m_lstCampCombatPlayers[i].userid)
                {
                    campCombatResult.m_lstCampCombatPlayers[i].nKill = cmd.kill;
                    campCombatResult.m_lstCampCombatPlayers[i].nAssist = cmd.assist;
                    campCombatResult.m_lstCampCombatPlayers[i].nDead = cmd.killed;
                    campCombatResult.m_lstCampCombatPlayers[i].nScore = cmd.score;
                }
            }
        }

        SortCampMemberRank(ref campCombatResult.m_lstCampCombatPlayers);
        DispatchValueEvent("RefreshBattleData", null, null);

    }

    /// <summary>
    /// 组队副本
    /// </summary>
    /// <param name="copyId"></param>
    public void ReqAskTeamrCopy(uint copyId)
    {
        GameCmd.stAskTeamrCopyUserCmd_CS cmd = new GameCmd.stAskTeamrCopyUserCmd_CS();
        cmd.copy_base_id = copyId;
        NetService.Instance.Send(cmd);
    }


    /// <summary>
    /// 每日已经完成阵营战次数
    /// </summary>
    public void OnCampTimes(stEnterCampUserCmd_S cmd)
    {
        this.m_enterCampTimes = cmd.num;
    }

    #endregion

    #region IGlobalEvent

    /// <summary>
    /// 事件注册
    /// </summary>
    /// <param name="regster"></param>
    public void RegisterGlobalEvent(bool regster)
    {
        if (regster)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_LOADSCENECOMPELETE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEVELUP, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SYSTEM_LOADSCENECOMPELETE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEVELUP, GlobalEventHandler);
        }
    }

    /// <summary>
    /// 全局UI事件处理器
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    public void GlobalEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            case (int)Client.GameEventID.SYSTEM_LOADSCENECOMPELETE:
                {
                    Client.stLoadSceneComplete loadScene = (Client.stLoadSceneComplete)data;
                    if (loadScene.nMapID == 159)
                    {
                        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CampFightingPanel);
                    }
                }
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_LEVELUP:
                {
                    CaculateCampSectionIndex();
                }
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY:
                {
                    //             if (isEnterScene == false)
                    //             {
                    //                 return;
                    //             }

                    Client.stCreateEntity npcEntity = (Client.stCreateEntity)data;

                    Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
                    if (es == null)
                    {
                        return;
                    }

                    Client.IEntity npc = es.FindEntity(npcEntity.uid);
                    if (npc == null)
                    {
                        return;
                    }

                    int npcBaseId = npc.GetProp((int)Client.EntityProp.BaseID);
                    //采集物
                    table.NpcDataBase npctable = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)npcBaseId);
                    if (npctable != null && npctable.dwType == (uint)GameCmd.enumNpcType.NPC_TYPE_COLLECT_PLANT)//采集物
                    {
                        GameCmd.eCamp camp = (GameCmd.eCamp)npc.GetProp((int)Client.CreatureProp.Camp);
                        if (m_CampCombatResultInfo != null && m_CampCombatResultInfo.m_MyCampCombatInfo.camp != camp)
                        {
                            CampNpcOnTrigger callback = new CampNpcOnTrigger();
                            npc.SetCallback(callback);
                        }
                    }
                }
                break;
        }
    }
    #endregion

    #region Camp（报名）
    private Dictionary<uint, LocalCampSignInfo> m_dicLocalCampInfos = null;
    //角色所处等级段
    private int m_iCampSectionIndex = -1;
    private string m_strCampSectionString = "";
    public string CampSectionString
    {
        get
        {
            return m_strCampSectionString;
        }
    }
    public int CampSectionIndex
    {
        get
        {
            if (m_iCampSectionIndex == -1)
            {
                CaculateCampSectionIndex();
            }
            return m_iCampSectionIndex;
        }
    }

    /// <summary>
    /// 计算角色所处的阵营战等级段索引
    /// </summary>
    /// <returns></returns>
    private void CaculateCampSectionIndex()
    {
        List<string> lvSectionKeys = GameTableManager.Instance.GetGlobalConfigList<string>("CF_Level_Client");
        int playerLv = DataManager.Instance.PlayerLv;
        if (null != lvSectionKeys)
        {
            string[] lvArray = null;
            int minLv = 0;
            int maxLv = 0;
            for (int i = 0, max = lvSectionKeys.Count; i < max; i++)
            {
                lvArray = lvSectionKeys[i].Split(new char[] { '_' });
                if (null == lvArray || lvArray.Length != 2)
                {
                    continue;
                }
                if (null == lvArray || lvArray.Length != 2
                    || !int.TryParse(lvArray[0], out minLv)
                    || !int.TryParse(lvArray[01], out maxLv)
                    || playerLv > maxLv || playerLv < minLv)
                {
                    continue;
                }
                m_iCampSectionIndex = i;
                m_strCampSectionString = string.Format("{0}-{1}", minLv, maxLv);
                break;
            }
        }
    }

    /// <summary>
    /// 获取阵营战场次列表
    /// </summary>
    /// <returns></returns>
    public List<uint> GetSignCampInfosIndexs()
    {
        List<uint> indexs = new List<uint>();
        indexs.AddRange(m_dicLocalCampInfos.Keys);
        indexs.Sort((left, right) =>
            {
                return (int)left - (int)right;
            });
        return indexs;
    }

    /// <summary>
    /// 获取阵营战场次信息
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public LocalCampSignInfo GetLocalCampSignInfoByIndex(uint index)
    {
        return m_dicLocalCampInfos.ContainsKey(index) ? m_dicLocalCampInfos[index] : null;
    }

    /// <summary>
    /// 获取当前阵营战信息
    /// </summary>
    /// <returns></returns>
    public LocalCampSignInfo GetCurLocalCampSignInfo()
    {
        return (m_dicLocalCampInfos.ContainsKey(FightingIndex) ? m_dicLocalCampInfos[FightingIndex] : null);
    }

    /// <summary>
    /// 请求阵营战信息
    /// </summary>
    /// <param name="index">0：所有 其他：单个</param>
    public void GetSignCampInfo(uint index)
    {
        DataManager.Instance.Sender.SignCampInfoReq(index);
    }

    /// <summary>
    /// 返回阵营站信息
    /// </summary>
    /// <param name="cmd"></param>
    public void OnGetSignCampInfo(GameCmd.stSignInfoCampUserCmd_S cmd)
    {
        bool rebuild = cmd.req_index == 0;
        //更新所有
        if (rebuild)
        {
            m_dicLocalCampInfos.Clear();
        }
        UpdateCampFightLeftTimes((null != cmd.indexs) ? cmd.indexs.Count : 0);
        if (null != cmd.sign_info)
        {
            for (int i = 0, max = cmd.sign_info.Count; i < max; i++)
            {
                if (m_dicLocalCampInfos.ContainsKey(cmd.sign_info[i].index))
                {
                    m_dicLocalCampInfos[cmd.sign_info[i].index].UpdateData(cmd.sign_info[i]);
                }
                else
                {
                    m_dicLocalCampInfos.Add(cmd.sign_info[i].index, LocalCampSignInfo.Create(cmd.sign_info[i]));
                }
                m_dicLocalCampInfos[cmd.sign_info[i].index].Sign = (null != cmd.indexs && cmd.indexs.Contains(cmd.sign_info[i].index));
                if (!rebuild)
                {
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCAMPSIGNINFOCHANGED, cmd.sign_info[i].index);
                }
            }
        }
        if (rebuild)
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCAMPSIGNINFOSREFRESH);
        }
    }

    /// <summary>
    /// 执行报名操作（报名或者取消报名）
    /// </summary>
    /// <param name="index"></param>
    /// <param name="sign"></param>
    public void DoCampSignOp(uint index, bool sign)
    {
        if (!sign)
        {
            DoSignCamp(index);
        }
        else
        {
            DoCancelSignCamp(index);
        }
    }

    /// <summary>
    /// 报名阵营战
    /// </summary>
    /// <param name="index"></param>
    public void DoSignCamp(uint index)
    {
        DataManager.Instance.Sender.SignCampReq(index);
    }

    /// <summary>
    /// 报名成功
    /// </summary>
    /// <param name="index"></param>
    /// <param name="num"></param>
    public void OnDosignCamp(uint index, List<uint> num)
    {
        GetSignCampInfo(index);
        TipsManager.Instance.ShowTips("报名成功");
    }

    /// <summary>
    /// 执行取消报名请求
    /// </summary>
    /// <param name="index"></param>
    public void DoCancelSignCamp(uint index)
    {
        DataManager.Instance.Sender.CancelCampSignReq(index);
    }

    /// <summary>
    /// 取消报名
    /// </summary>
    /// <param name="index"></param>
    /// <param name="num"></param>
    public void OnDoCancelSignCamp(uint index)
    {
        LocalCampSignInfo info = null;
        if (m_dicLocalCampInfos.TryGetValue(index, out info) && info.Sign)
        {
            info.Sign = false;
            //请求对应场次阵营站信息
            GetSignCampInfo(index);
            TipsManager.Instance.ShowTips("取消报名成功");
        }
    }
    #endregion

    #region Timer
    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == TIME_ID)
        {
            if (this.m_campStartTipsCount > 0)
            {
                this.m_campStartTipsCount--;
                string tips = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Camp_Notice_Begin3);
                DataManager.Manager<EffectDisplayManager>().AddTips(tips);
            }
            else
            {
                TimerAxis.Instance().KillTimer(TIME_ID, this);
            }
        }
    }
    #endregion
}
