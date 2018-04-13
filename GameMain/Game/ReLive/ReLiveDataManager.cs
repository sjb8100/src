using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using GameCmd;
using Engine.Utility;
using UnityEngine;


class ReLiveDataManager : BaseModuleData, IManager, ITimer
{

    public class ReLiveData
    {
        public uint reliveId;  //复活类型ID
        public int reliveCd;   //当前的复活CD
    }

    #region property

    private List<ReLiveData> m_lstReliveData = new List<ReLiveData>();
    public List<ReLiveData> ReliveDataList
    {
        get
        {
            return m_lstReliveData;
        }
    }

    private uint m_reliveTimes;
    public uint ReliveTimes
    {

        get
        {
            return m_reliveTimes;
        }
    }

    private const int RELIVE_TIMERID = 1000;

    /// <summary>
    /// 死亡时间
    /// </summary>
    private float m_deadTime = 0f;

    public float DeadTime
    {
        get
        {
            return m_deadTime;
        }
    }

    #endregion

    #region  IManager
    public void ClearData()
    {

    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        RegisterEvent(true);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RECONNECT_SUCESS, DoGameEvent); //断线重连  强制复活
    }
    /// <summary>
    /// 重置
    /// </summary>
    public void Reset(bool depthClearData = false)
    {
        //m_reLiveColdInfoList.Clear();
        m_lstReliveData.Clear();
        m_reliveTimes = 0;
        RegisterEvent(false);
    }
    /// <summary>
    /// 每帧执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Process(float deltaTime)
    {
    }
    #endregion

    #region method

    private void RegisterEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_RELIVE, DoGameEvent); //复活
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKILL_RELIVE, DoGameEvent); //技能复活
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYDEAD, DoGameEvent);//死亡

        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_RELIVE, DoGameEvent);//复活
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SKILL_RELIVE, DoGameEvent);  //技能复活
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYDEAD, DoGameEvent);//死亡

        }
    }

    void DoGameEvent(int eventID, object param)
    {
        if (eventID == (int)GameEventID.ENTITYSYSTEM_RELIVE)
        {
            stEntityRelive stRelive = (stEntityRelive)param;
            if (ClientGlobal.Instance().IsMainPlayer(stRelive.uid))
            {
                PanelID panelId = UIFrameManager.Instance.CurrShowPanelID;
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ReLivePanel);
            }
        }
        else if (eventID == (int)GameEventID.SKILL_RELIVE)
        {
            stSkillRelive skillRe = (stSkillRelive)param;
            if (!ClientGlobal.Instance().IsMainPlayer(skillRe.id))
            {
                return;
            }

            //武斗场不使用技能复活
            if (DataManager.Manager<ArenaManager>().EnterArena)
            {
                return;
            }

            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ReLivePanel);

            //如果人是活的
            if (false == Client.ClientGlobal.Instance().MainPlayer.IsDead())
            {
                return;
            }

            uint time = GameTableManager.Instance.GetGlobalConfig<uint>("ReliveCountdown");
            TipsManager.Instance.ShowTipWindow(0, time, TipWindowType.CancelOk, "是否复活", () =>
            {
                GameCmd.stOKReliveUserCmd_C cmd = new GameCmd.stOKReliveUserCmd_C();
                cmd.byType = (uint)GameCmd.ReliveType.ReliveType_Skill;
                cmd.dwUserTempID = ClientGlobal.Instance().MainPlayer.GetID();
                cmd.dwNpcID = 0;
                NetService.Instance.Send(cmd);
            }, () =>
            {
                GameCmd.stOKReliveUserCmd_C cmd = new GameCmd.stOKReliveUserCmd_C();
                cmd.byType = (uint)GameCmd.ReliveType.ReliveType_Home;
                cmd.dwUserTempID = ClientGlobal.Instance().MainPlayer.GetID();
                cmd.dwNpcID = 0;
                NetService.Instance.Send(cmd);
            });
        }
        else if (eventID == (int)GameEventID.RECONNECT_SUCESS)
        {
            stReconnectSucess reconnectSucess = (stReconnectSucess)param;
            if (reconnectSucess.isLogin)
            {
                MainPlayerRelive();
            }
        }
        else if (eventID == (int)GameEventID.ENTITYSYSTEM_ENTITYDEAD)
        {
            stEntityDead ed = (stEntityDead)param;
            if (ClientGlobal.Instance().IsMainPlayer(ed.uid))
            {
                this.m_deadTime = Time.realtimeSinceStartup;
            }
        }
    }

    /// <summary>
    /// 主角复活
    /// </summary>
    void MainPlayerRelive()
    {
        IPlayer mainPlyer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlyer != null)
        {
            if (Client.ClientGlobal.Instance().MainPlayer.IsDead())
            {
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ReLivePanel);

                Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
                if (rs != null)
                {
                    rs.EnableGray(false);
                }

                mainPlyer.SendMessage(EntityMessage.EntityCommand_SetPos, mainPlyer.GetPos());
                mainPlyer.ChangeState(CreatureState.Normal);
            }
        }
    }


    public void AddReLiveColdInfo(stReliveColdMagicUserCmd_S reliveCold)
    {
        m_lstReliveData.Clear();

        m_reliveTimes = reliveCold.dwReliveTimes;

        for (int i = 0; i < reliveCold.data.Count; i++)
        {
            table.ReliveDataBase db = GameTableManager.Instance.GetTableItem<table.ReliveDataBase>(reliveCold.data[i].byType);
            if (db == null)
            {
                continue;
            }

            ReLiveData rld = new ReLiveData { reliveId = reliveCold.data[i].byType, reliveCd = (int)db.CD };

            m_lstReliveData.Add(rld);
        }

        TimerAxis.Instance().KillTimer(RELIVE_TIMERID, this);
        TimerAxis.Instance().SetTimer(RELIVE_TIMERID, 1000, this);
    }

    #endregion

    public void OnTimer(uint uTimerID)
    {
        if (RELIVE_TIMERID == uTimerID)
        {
            IPlayer player = ClientGlobal.Instance().MainPlayer;
            if (player != null && !player.IsDead())
            {
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ReLivePanel);

                TimerAxis.Instance().KillTimer(RELIVE_TIMERID, this);
            }

            for (int i = 0; i < m_lstReliveData.Count; i++)
            {
                m_lstReliveData[i].reliveCd--;
            }
        }
    }

}

