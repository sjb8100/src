/************************************************************************************
 * Copyright (c) 2018  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Daily
 * 创建人：  wenjunhua.zqgame
 * 文件名：  JvBaoBossWorldManager
 * 版本号：  V1.0.0.0
 * 创建时间：3/27/2018 11:20:42 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Engine;
using UnityEngine;
using GameCmd;
using table;
using Client;

class JvBaoBossWorldManager : IManager,ICopy
{
    #region event
    private void OnGlobalUIEventHandler(int eventId, object obj)
    {
        switch (eventId)
        {
            case (int)Client.GameEventID.SYNSERVERTIME:
                if (!firstCheckWorldBossStatus)
                {
                    firstCheckWorldBossStatus = true;
                    DoCheckWorldBossStatus();
                }
                break;
        }
    }
    #endregion

    #region Interface
    public void Initialize()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SYNSERVERTIME, OnGlobalUIEventHandler);
    }

    //上次衰减的时间节点
    private float lastReduceTime = 0;
    public void Process(float deltaTime)
    {
        if (IsWaitBossAppear)
        {
            if (Time.time -lastReduceTime >= 1)
            {
                lastReduceTime = Time.time;
                worldBossSpawnLeftTime = (uint)Mathf.Max(0, worldBossSpawnLeftTime - 1);
            }
        }
    }

    public void Reset(bool depthClearData = false)
    {
        if (depthClearData)
        {
            if (null != m_dicWorldBossInfo)
                m_dicWorldBossInfo.Clear();
            ClearJvBaoBossTempData();
            firstCheckWorldBossStatus = false;
        }
    }
    public void ClearData()
    {

    }

    public void EnterCopy()
    {
        stCopyInfo info = new stCopyInfo();
        info.bShow = true;
        info.bShowBattleInfoBtn = true;
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eShowCopyInfo, info);
        DataManager.Manager<ComBatCopyDataManager>().CopyCDAndExitData = new CopyCDAndExitInfo { bShow = true, bShowBattleInfoBtn = true };
    }

    public void ExitCopy()
    {
        ClearJvBaoBossTempData();
    }
    
    #endregion

    #region 鼓舞
    //金币鼓舞最大值
    private int goldInspireMax = -1;
    //绑元鼓舞最大值
    private int bYuanInspiremax = -1;
    /// <summary>
    /// 获取最大鼓舞次数
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public int GetInspireMax(InspireType type)
    {
        table.InspireDataBase inspireDB = null;
        if (type == InspireType.InspireType_Coin)
        {
            if (goldInspireMax == -1)
            {
                inspireDB = GameTableManager.Instance.GetTableItem<table.InspireDataBase>((uint)type, 1);
                if (null != inspireDB)
                {
                    goldInspireMax = (int)inspireDB.maxInspireTimes;
                }
                else
                {
                    goldInspireMax = 0;
                }
            }
            return goldInspireMax;
        }
        else if (type == InspireType.InspireType_Money)
        {
            if (bYuanInspiremax == -1)
            {
                inspireDB = GameTableManager.Instance.GetTableItem<table.InspireDataBase>((uint)type, 1);
                if (null != inspireDB)
                {
                    bYuanInspiremax = (int)inspireDB.maxInspireTimes;
                }
                else
                {
                    bYuanInspiremax = 0;
                }
            }
            return bYuanInspiremax;
        }
        return 0;
    }

    /// <summary>
    /// 金币鼓舞
    /// </summary>
    public void CoinInspire()
    {
        InspirePlayer(InspireType.InspireType_Coin);
    }

    /// <summary>
    /// 元宝鼓舞
    /// </summary>
    public void YuanBaoInspire()
    {
        InspirePlayer(InspireType.InspireType_Money);
    }

    /// <summary>
    /// 鼓舞角色
    /// </summary>
    /// <param name="inspireType"></param>
    private void InspirePlayer(InspireType inspireType)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.InspirePlayerReq(inspireType);
    }

    /// <summary>
    /// 刷新鼓励次数
    /// </summary>
    /// <param name="leftTime"></param>
    public void OnInspirePlayer(InspireType type, uint leftTime)
    {
        LocalInspireData insData = null;
        if (null != curInspireData && curInspireData.TryGetValue(type, out insData))
        {
            insData.LeftTimes = leftTime;
        }
        else
        {
            if (null == curInspireData)
                curInspireData = new Dictionary<InspireType, LocalInspireData>();
            insData = new LocalInspireData()
            {
                LeftTimes = leftTime,
                InsType = type,
                MaxTimes = (uint)GetInspireMax(type),
            };
            curInspireData.Add(type, insData);
        }
        CaculateInspirePileValue();
        //刷新鼓励次数
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.UIEVENT_WORLDBOSSINSPIREREFRESH, type);
    }

    //本地鼓舞数据
    public class LocalInspireData
    {
        //鼓舞类型
        public InspireType InsType = InspireType.InspireType_Coin;
        //剩余次数
        public uint LeftTimes = 0;
        //最大鼓舞次数
        public uint MaxTimes = 0;
    }

    /// <summary>
    /// 获取鼓舞数据
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count">第几次</param>
    /// <param name="db"></param>
    /// <returns></returns>
    public bool TryGetInspireDB(InspireType type, int count, out table.InspireDataBase db)
    {
        db = GameTableManager.Instance.GetTableItem<table.InspireDataBase>((uint)type, count);
        if (null == db)
        {
            return false;
        }
        return true;
    }

    //当前玩家鼓舞数据
    private Dictionary<InspireType, LocalInspireData> curInspireData = null;
    //当前鼓舞叠加值
    private uint curInspirePileValue = 0;
    public uint CurInspirePileValue
    {
        get
        {
            return curInspirePileValue;
            
        }
    }

    /// <summary>
    /// 计算鼓舞buff叠加值
    /// </summary>
    public void CaculateInspirePileValue()
    {
        curInspirePileValue = 0;
        if (null != curInspireData)
        {
            Dictionary<InspireType, LocalInspireData>.Enumerator enu = curInspireData.GetEnumerator();
            int count = 0;
            table.InspireDataBase db = null;
            while (enu.MoveNext())
            {
                count = (int)enu.Current.Value.MaxTimes - (int)enu.Current.Value.LeftTimes;
                if (!TryGetInspireDB(enu.Current.Key, count, out db))
                    continue;
                curInspirePileValue += db.addBufferValue;
            }
        }
    }

    public bool TryGetInspirePlayerData(InspireType insType, out LocalInspireData data)
    {
        data = null;
        return (null != curInspireData && curInspireData.TryGetValue(insType, out data));
    }
    /// <summary>
    /// 接收玩家鼓舞数据
    /// </summary>
    public void OnInspirePlayerDataReceive(List<InspireData> datas)
    {
        if (null != curInspireData)
        {
            curInspireData.Clear();
        }

        if (null != datas)
        {
            for (int i = 0, max = datas.Count; i < max; i++)
            {
                OnInspirePlayer(datas[i].inspire_type, datas[i].nums);
            }
        }
    }
    #endregion

    #region 聚宝-世界Boss
    private bool firstCheckWorldBossStatus = false;
    public void DoCheckWorldBossStatus()
    {
        if (null != m_dicWorldBossInfo)
        {
            Dictionary<uint, LocalWorldBossInfo>.Enumerator em = m_dicWorldBossInfo.GetEnumerator();
            while (em.MoveNext())
            {
                em.Current.Value.CheckStatus();
            }
        }
    }
    /// <summary>
    /// boss状态
    /// </summary>
    public enum JvBaoBossStatus
    {
        JBS_NotOpen =1,         //未开启
        JBS_CommingSoon,        //当日将要刷新
        JBS_Attack,             //已刷新
        JBS_Finish              //已完成
    }
    /// <summary>
    /// 清空聚宝Boss零时数据
    /// </summary>
    private void ClearJvBaoBossTempData()
    {
        if (null != curInspireData)
            curInspireData.Clear();
        if (null != worldBossDamRankData)
            worldBossDamRankData.Clear();
        myBossDam = 0;
        myBossDamRank = 0;
        curInspirePileValue = 0;
    }
    /// <summary>
    /// 世界Boss开启通知
    /// </summary>
    /// <param name="id"></param>
    public void OnWorldBossBeginNotice(uint id)
    {
        if (DataManager.Instance.PlayerLv < WorldBossOpenLv)
        {
            //等级没到不做提醒
            return;
        }
        TextManager tmgr = DataManager.Manager<TextManager>();

        Action joinWorldBossAction = () =>
            {
                JoinWorldBoss(id);
            };
        TipsManager.Instance.ShowTipWindow(15, 0, TipWindowType.CancelOk, tmgr.GetLocalText(LocalTextType.HuntingBoss_BeginNotice), joinWorldBossAction,null,
            okstr: tmgr.GetLocalText(LocalTextType.Local_TXT_Confirm)
            , cancleStr: tmgr.GetLocalText(LocalTextType.Local_TXT_Cancel));
    }

    /// <summary>
    /// 参加世界Boss聚宝
    /// </summary>
    public void JoinWorldBoss(uint worldBossTabID)
    {
        LocalWorldBossInfo worldBossInfo = null;
        if (!TryGetWorldBossStatusInfo(worldBossTabID, out worldBossInfo) || worldBossInfo.Status != JvBaoBossStatus.JBS_Attack)
        {
            TipsManager.Instance.ShowTips(LocalTextType.HuntingBoss_weikaiqi);
            return;
        }
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.JoinWorldBossReq(worldBossTabID);
    }


    //世界Boss出现剩余时间
    private uint worldBossSpawnLeftTime = 0;
    public uint WorldBossSpawnLeftTime
    {
        get
        {
            return worldBossSpawnLeftTime;
        }
    }
    //是否等待Boss出现
    public bool IsWaitBossAppear
    {
        get
        {
            return worldBossSpawnLeftTime != 0;
        }
    }

    //世界Boss开启等级
    private int worldBossOpenLv = -1;
    public int WorldBossOpenLv
    {
        get
        {
            if (worldBossOpenLv == -1)
            {
                worldBossOpenLv = GameTableManager.Instance.GetGlobalConfig<int>("WorldBossOpenLevel");
            }
            return worldBossOpenLv;
        }
    }

    /// <summary>
    ///距离世界Boss出现还剩下leftSeconds秒
    /// </summary>
    /// <param name="leftSeconds"></param>
    public void OnWorldJvBaoBossSpawnLeftTime(uint leftSeconds)
    {
        worldBossSpawnLeftTime = leftSeconds;
    }

    /// <summary>
    /// 世界boss出现副本开始
    /// </summary>
    public void OnWorldBossSpawnCopyStart()
    {
        worldBossSpawnLeftTime = 0;
    }

    //世界boss伤害排名
    private List<BossDamRank> worldBossDamRankData = null;
    //我的伤害排名
    private uint myBossDamRank = 0;
    public uint MyBossDamRank
    {
        get
        {
            return myBossDamRank;
        }
    }

    //我的伤害
    private uint myBossDam = 0;
    public uint MyBossDam
    {
        get
        {
            return myBossDam;
        }
    }

    public int WorldBossDamRankCount
    {
        get
        {
            return (null != worldBossDamRankData) ? worldBossDamRankData.Count : 0;
        }
    }
    /// <summary>
    /// 刷新世界聚宝伤害排名
    /// </summary>
    /// <param name="rankData">排名数据</param>
    /// <param name="myRank">我的排名</param>
    /// <param name="myDam">我的输出伤害</param>
    public void OnWorldJvBaoDamRankRefresh(List<BossDamRank> rankData, uint myRank, uint myDam)
    {
        worldBossDamRankData = rankData;
        myBossDamRank = myRank;
        myBossDam = myDam;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.UIEVENT_WORLDBOSSDAMRANKREFRESH);
    }

    /// <summary>
    /// 尝试获取世界Boss伤害排名数据
    /// </summary>
    /// <param name="index"></param>
    /// <param name="damRankData"></param>
    /// <returns></returns>
    public bool TryGetWorldBossDamRankData(int index, out BossDamRank damRankData)
    {
        damRankData = null;
        if (index >= 0 && WorldBossDamRankCount > index)
        {
            damRankData = worldBossDamRankData[index];
            return true;
        }
        return false;
    }

    
    public class LocalWorldBossInfo
    {
        private GameCmd.WBossStatus wBossStatus = WBossStatus.WBossStatus_UnStart;
        //id
        private uint m_id = 0;
        public uint ID
        {
            get
            {
                return m_id;
            }
        }

        //boss状态
        private JvBaoBossStatus m_status = JvBaoBossStatus.JBS_NotOpen;
        public JvBaoBossStatus Status
        {
            get
            {
                return m_status;
            }
        }

        //最后一击名称
        private string m_lastDam = "";
        public string LastDam
        {
            get
            {
                return m_lastDam;
            }
        }

        //最大伤输出玩家
        private string m_maxDam = "";
        public string MaxDam
        {
            get
            {
                return m_maxDam;
            }
        }

        public void CheckStatus()
        {
            switch(wBossStatus)
            {
                case WBossStatus.WBossStatus_UnStart:
                    {
                        table.TreasureBossDataBase db = GameTableManager.Instance.GetTableItem<table.TreasureBossDataBase>(ID);
                        bool setSuccess = false;
                        if (null != db)
                        {
                            if (!string.IsNullOrEmpty(db.scheIdStr))
                            {
                                string[] scheIds = db.scheIdStr.Split(new char[] { '_' });
                                if (null != scheIds)
                                {
                                    uint schId = 0;
                                    long leftTime = 0;
                                    long minLeftTime = 0;
                                    int daySecondsTotal = 86400;
                                    ScheduleDefine.ScheduleLocalData tempScheduleLocalData = null;
                                    for (int i = 0, max = scheIds.Length; i < max; i++)
                                    {
                                        if (!uint.TryParse(scheIds[i], out schId))
                                            continue;
                                        tempScheduleLocalData = new ScheduleDefine.ScheduleLocalData(schId);
                                        if (!tempScheduleLocalData.IsInSchedule(DateTimeHelper.Instance.Now - 28800, out leftTime))
                                        {
                                            if (leftTime < daySecondsTotal)
                                            {
                                                setSuccess = true;
                                                m_status = JvBaoBossStatus.JBS_CommingSoon;
                                                break;
                                            }
                                            else if (minLeftTime == 0 || minLeftTime > leftTime)
                                            {
                                                minLeftTime = leftTime;
                                            }
                                        }
                                    }

                                    if (!setSuccess
                                        && (minLeftTime != 0)
                                        && (minLeftTime >= daySecondsTotal))
                                    {
                                        setSuccess = true;
                                        m_status = JvBaoBossStatus.JBS_NotOpen;
                                    }
                                }
                            };
                        }
                        if (!setSuccess)
                        {
                            m_status = JvBaoBossStatus.JBS_NotOpen;
                        }
                    }
                    break;
                case WBossStatus.WBossStatus_Begin:
                    m_status = JvBaoBossStatus.JBS_Attack;
                    break;
                case WBossStatus.WBossStatus_End:
                    m_status = JvBaoBossStatus.JBS_Finish;
                    break;

            }
        }

        public void UpdateData(WorldBossInfo info)
        {
            if (ID != info.id)
            {
                m_id = info.id;
            }
            
            m_lastDam = info.last_dam;
            m_maxDam = info.max_dam;
            wBossStatus = (GameCmd.WBossStatus)info.status;
            CheckStatus();
        }
    }

    //世界Boss状态信息
    private Dictionary<uint, LocalWorldBossInfo> m_dicWorldBossInfo = null;
    /// <summary>
    /// 获取世界Boss状态
    /// </summary>
    /// <param name="worldBossTabId"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    public bool TryGetWorldBossStatusInfo(uint worldBossTabId, out LocalWorldBossInfo info)
    {
        info = null;
        return (null != m_dicWorldBossInfo && m_dicWorldBossInfo.TryGetValue(worldBossTabId, out info));
    }

    /// <summary>
    /// 请求世界Boss
    /// </summary>
    public void ReqWorldBossStatusInfo()
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.WorldBossStatusInfoReq();
    }

    /// <summary>
    /// 服务器下发世界Boss状态信息
    /// </summary>
    /// <param name="infos"></param>
    public void OnWordBossStatusInfo(List<WorldBossInfo> infos)
    {
        if (null != m_dicWorldBossInfo)
            m_dicWorldBossInfo.Clear();
        if (null != infos)
        {
            for (int i = 0, max = infos.Count; i < max; i++)
            {
                OnSingleWorldBossStatusChanged(infos[i]);
            }
        }
        //刷新UI列表
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.UIEVENT_WORLDBOSSSTATUSREFRESH);
    }


    /// <summary>
    /// 单个世界Boss状态变更
    /// </summary>
    /// <param name="info"></param>
    /// <param name="needRefreshUI"></param>
    public void OnSingleWorldBossStatusChanged(WorldBossInfo info, bool needRefreshUI = false)
    {
        LocalWorldBossInfo temp = null;
        if (null == m_dicWorldBossInfo)
        {
            m_dicWorldBossInfo = new Dictionary<uint, LocalWorldBossInfo>();
        }
        if (!m_dicWorldBossInfo.TryGetValue(info.id, out temp))
        {
            temp = new LocalWorldBossInfo();
            m_dicWorldBossInfo.Add(info.id, temp);
        }

        temp.UpdateData(info);

        if (needRefreshUI)
        {
            //刷新UI
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.UIEVENT_WORLDBOSSSTATUSREFRESH, info.id);
        }
    }

    //世界Boss是否被击杀
    private bool isWorldBossDead = false;
    public bool IsWorldBossDead
    {
        get
        {
            return isWorldBossDead;
        }
    }

    /// <summary>
    /// 世界Boss被击杀
    /// </summary>
    public void OnJvBaoBossDead()
    {
        isWorldBossDead = true;
        if (!DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.JvBaoBossDamRankPanel))
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.JvBaoBossDamRankPanel);
        }
    }

    public void QuitCopy()
    {
        isWorldBossDead = false;
        DataManager.Manager<ComBatCopyDataManager>().ReqExitCopy();
    }
    #endregion
}