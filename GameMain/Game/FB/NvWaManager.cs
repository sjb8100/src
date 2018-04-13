using Client;
using Engine.Utility;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
//*******************************************************************************************
//	创建日期：	2016-12-5   17:53
//	文件名称：	NvWaManager,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	Lin Chuang Yuan
//	说    明：	女娲副本
//*******************************************************************************************

class NvWaManager : BaseModuleData, IManager, ICopy, ITimer
{



    #region property

    public enum ENvWaState
    {
        None,         //无状态
        Init,         //界面初始化，还没点开始战斗
        StartBattle,  //已经开始战斗
        Exit,         //已经退出女娲
    }

    /// <summary>
    /// 女娲当家状态
    /// </summary>

    ENvWaState m_nvWaState = ENvWaState.None;
    public ENvWaState NvWaState
    {
        get
        {
            return m_nvWaState;
        }

        set
        {
            m_nvWaState = value;
        }
    }

    /// <summary>
    /// 女娲id及等级数据
    /// </summary>
    public class GuardData
    {
        public uint id;
        public uint lv;
        public uint num;

    }

    List<GuardData> m_listGuardData = new List<GuardData>();

    public List<GuardData> GuardDataList
    {
        get
        {
            return m_listGuardData;
        }
    }

    /// <summary>
    /// 女娲徽章
    /// </summary>
    uint m_nvwaCap = 0;
    public uint NvWaCap
    {
        get
        {
            return m_nvwaCap;
        }
    }

    /// <summary>
    /// 波数
    /// </summary>
    uint m_copyWave = 1;
    public uint CopyWave
    {
        get
        {
            return m_copyWave;
        }
    }


    bool m_nvwaResult = false;
    public bool NvWaResult
    {
        get
        {
            return m_nvwaResult;
        }
    }

    Dictionary<uint, List<string>> m_dicGuardName = new Dictionary<uint, List<string>>();

    public Dictionary<uint, List<string>> GuardNameDic
    {
        get
        {
            return m_dicGuardName;
        }
    }

    /// <summary>
    /// 当前剩下的士兵
    /// </summary>
    uint m_nvWaCuardNum = 0;
    public uint NvWaCuardNum
    {
        get
        {
            return m_nvWaCuardNum;
        }
    }

    /// <summary>
    /// 是否进入nvwa
    /// </summary>
    bool m_bEnterNvWa = false;
    public bool EnterNvWa
    {
        get
        {
            return m_bEnterNvWa;
        }
    }

    /// <summary>
    /// 鱼龙真符
    /// </summary>
    public uint NvWaJumpItemId { set; get; }

    private uint m_NvWaFinishCd = 10;

    private const uint m_uNvWaTimerID = 1000;

    float m_nvWaStartTime = 0;
    public float NvWaStartTime
    {
        get
        {
            return m_nvWaStartTime;
        }
    }

    #endregion

    #region interface
    public void ClearData()
    {

    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        InitGuard();//初始化守卫数据

        NvWaJumpItemId = (uint)GameTableManager.Instance.GetGlobalConfig<int>("NWJumpItem");
    }


    /// <summary>
    /// 重置
    /// </summary>
    public void Reset(bool depthClearData = false)
    {
        //m_bEnterNvWa = false;

        if (depthClearData == true)
        {
            //重置女娲数据
            ResetNvwaData();
        }
    }


    /// <summary>
    /// 每帧执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Process(float deltaTime)
    {

    }

    //进入副本
    public void EnterCopy()
    {
        m_bEnterNvWa = true;

        if (this.m_nvWaState != ENvWaState.StartBattle)
        {
            NvWaStartPanel.NvWaStartParam param = new NvWaStartPanel.NvWaStartParam
            {
                itemId = this.NvWaJumpItemId,
                OkCd = 10,
                OkDelegate = OkBtnDelegate,
                CancelDelegate = CancelBtnDelegate
            };

            this.m_nvWaStartTime = UnityEngine.Time.realtimeSinceStartup + 10; //开始界面最多10s
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.NvWaStartPanel, data: param);
        }

        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.NvWaPanel);

        stCopyInfo info = new stCopyInfo();
        info.bShow = true;
        info.bShowBattleInfoBtn = false;
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eShowCopyInfo, info);
        DataManager.Manager<ComBatCopyDataManager>().CopyCDAndExitData = new CopyCDAndExitInfo { bShow = true, bShowBattleInfoBtn = false };
    }

    void OkBtnDelegate()
    {
        ReqStartNvWaCopy(false, false);
    }

    void CancelBtnDelegate(bool useItem, bool useYuanBaoAutoBuy)
    {
        ReqStartNvWaCopy(useItem, useYuanBaoAutoBuy);
    }

    //退出副本
    public void ExitCopy()
    {
        //重置女娲数据
        ResetNvwaData();
    }

    #endregion

    #region method

    /// <summary>
    /// 重置女娲数据
    /// </summary>
    void ResetNvwaData()
    {
        m_bEnterNvWa = false;
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.NvWaPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.NvWaPanel, UIMsgID.eNvWaExit, null);
        }

        //重新初始化数据（守卫等级置1）
        InitGuard();

        m_nvWaState = ENvWaState.Exit;

        TimerAxis.Instance().KillTimer(m_uNvWaTimerID, this);
    }


    /// <summary>
    /// 守卫的名称
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void GetGuardNameListById(uint id)
    {

    }

    /// <summary>
    /// 初始化守卫数据
    /// </summary>
    public void InitGuard()
    {
        //当前守卫数量
        this.m_nvWaCuardNum = 0;

        m_dicGuardName.Clear();
        m_listGuardData.Clear();
        List<NWGuardDataBase> dbList = GameTableManager.Instance.GetTableList<NWGuardDataBase>();
        if (dbList != null)
        {
            for (int i = 0; i < dbList.Count; i++)
            {
                if (m_dicGuardName.ContainsKey(dbList[i].id) == false)
                {
                    List<string> nameList = GetNameList(dbList[i].name);
                    m_dicGuardName.Add(dbList[i].id, nameList);
                }

                GuardData guardData = new GuardData { id = dbList[i].id, lv = 1 };
                m_listGuardData.Add(guardData);
            }
        }
    }

    public List<string> GetNameList(string nameStr)
    {
        List<string> s = new List<string>();

        string[] arr = nameStr.Split(';');

        s.AddRange(arr);

        return s;
    }

    /// <summary>
    /// 招募消耗
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<int> GetRecruitCostList(uint id)
    {
        List<int> list = new List<int>();

        NWGuardDataBase NWGuarddb = GameTableManager.Instance.GetTableItem<NWGuardDataBase>(id);
        if (NWGuarddb != null)
        {
            list.Add(NWGuarddb.guard_cost1);
            list.Add(NWGuarddb.guard_cost2);
            list.Add(NWGuarddb.guard_cost3);
            list.Add(NWGuarddb.guard_cost4);

            return list;
        }
        else
        {
            Engine.Utility.Log.Error("读取表格数据出错！！！");
            return null;
        }
    }

    /// <summary>
    /// 升级消耗
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<int> GetLvUpCostList(uint id)
    {
        List<int> list = new List<int>();

        NWGuardDataBase NWGuarddb = GameTableManager.Instance.GetTableItem<NWGuardDataBase>(id);
        if (NWGuarddb != null)
        {
            list.Add(NWGuarddb.lvup_cost1);
            list.Add(NWGuarddb.lvup_cost2);
            list.Add(NWGuarddb.lvup_cost3);

            return list;
        }
        else
        {
            Engine.Utility.Log.Error("读取表格数据出错！！！");
            return null;
        }

    }

    #endregion


    #region net


    /// <summary>
    /// 女娲徽章
    /// </summary>
    public void OnNvWaCap(stRefreshCapCopyUserCmd_S cmd)
    {
        this.m_nvwaCap = cmd.cap_num;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eNvWaCap, null);
        }
    }

    /// <summary>
    /// 每一波怪物开始
    /// </summary>
    /// <param name="cmd"></param>
    public void OnWaveStart(stWaveStartCopyUserCmd_S cmd)
    {
        this.m_copyWave = cmd.wave;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.NvWaPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.NvWaPanel, UIMsgID.eNvWaNewWave, null);
        }
    }

    /// <summary>
    /// 升级守卫  
    /// </summary>
    /// <param name="guardId">守卫ID</param>
    public void ReqNvWaLvUpGuard(uint guardId)
    {
        stLvUpGuardCopyUserCmd_CS cmd = new stLvUpGuardCopyUserCmd_CS();
        cmd.npc_base_id = guardId;

        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 升级守卫
    /// </summary>
    /// <param name="cmd"></param>
    public void OnNvWaLvUpGuard(stLvUpGuardCopyUserCmd_CS cmd)
    {
        GuardData guardData = m_listGuardData.Find((data) => { return data.id == cmd.npc_base_id; });
        if (guardData != null)
        {
            guardData.lv = cmd.level;

            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
            {
                DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eNvWaLvUp, guardData);
            }
        }
    }

    /// <summary>
    /// 购买守卫
    /// </summary>
    /// <param name="guardId">守卫Id</param>
    public void ReqNvWaBuyGuard(uint guardId)
    {
        stBuyGuardCopyUserCmd_CS cmd = new stBuyGuardCopyUserCmd_CS();
        cmd.npc_base_id = guardId;

        NetService.Instance.Send(cmd);

    }


    /// <summary>
    /// 购买守卫成功
    /// </summary>
    /// <param name="cmd"></param>
    public void OnNvWaBuyGuard(stBuyGuardCopyUserCmd_CS cmd)
    {
        uint id = cmd.npc_base_id;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eNvWaGuardNumUpdate, null);
        }
    }

    /// <summary>
    /// 当前守卫剩余数量
    /// </summary>
    public void OnNvWaGuardNum(stNWGuardNumCopyUserCmd_S cmd)
    {
        for (int i = 0; i < m_listGuardData.Count; i++)
        {
            if (cmd.npc_base_id == m_listGuardData[i].id)
            {
                m_listGuardData[i].num = cmd.num;
            }
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eNvWaGuardNumUpdate, null);
        }
    }


    /// <summary>
    ///  开始女娲副本 是否使用跳关付,道具不足自动购买
    /// </summary>
    /// <param name="cmd"></param>
    public void ReqStartNvWaCopy(bool useJump, bool autoBuy)
    {
        stStarNWCopyUserCmd_CS cmd = new stStarNWCopyUserCmd_CS();
        cmd.jump = useJump;
        cmd.auto_buy = autoBuy;

        NetService.Instance.Send(cmd);
    }

    /// <summary>
    ///  开始女娲副本 是否使用跳关付
    /// </summary>
    /// <param name="cmd"></param>
    public void OnStartNvWaCopy(stStarNWCopyUserCmd_CS cmd)
    {
        this.m_nvWaState = ENvWaState.StartBattle;
    }

    /// <summary>
    /// 女娲结算
    /// </summary>
    public void OnNvWaResult(stFinishNWCopyUserCmd_S cmd)
    {
        m_nvwaResult = cmd.succ;

        bool b = cmd.succ;

        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.FBResult, data: b);

        m_NvWaFinishCd = 10;

        TimerAxis.Instance().SetTimer(m_uNvWaTimerID, 1000, this, 10);
    }

    #endregion


    public void OnTimer(uint uTimerID)
    {
        if (m_uNvWaTimerID == uTimerID)
        {
            if (m_NvWaFinishCd > 0)
            {
                m_NvWaFinishCd -= 1;
                if (this.NvWaResult)
                {
                    TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Copy_Commond_fubenguanbitips, m_NvWaFinishCd);
                }
                else
                {
                    TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Copy_Commond_fubenguanbitips2, m_NvWaFinishCd);
                }

            }

            if (m_NvWaFinishCd == 0)
            {
                TimerAxis.Instance().KillTimer(m_uNvWaTimerID, this);
            }
        }
    }
}

