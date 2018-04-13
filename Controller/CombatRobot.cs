//*************************************************************************
//	创建日期:	2016-12-20 11:50
//	文件名称:	CombatRobot.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	自动战斗
//*************************************************************************
using System;
using System.Collections.Generic;
using Client;
using UnityEngine.Profiling;

namespace Controller
{
    #region CombatRobotMono


    public class CombatRobotMono : UnityEngine.MonoBehaviour
    {
        public CombatRobotStatus m_status;

        public CombatRobot.RoleAction m_RoleAction;

        public List<int> m_lstSkillids = new List<int>();

        public List<string> m_lstLog = new List<string>();

        public List<GameEventID> m_lstEventID = new List<GameEventID>();

        public int logNum = 30;
        public void AddLog(string strLog)
        {
            //while (m_lstLog.Count >= logNum)
            //{
            //    m_lstLog.RemoveAt(0);
            //}

            //m_lstLog.Add(strLog);
        }

    }
    #endregion
    public class CombatRobot : ICombatRobot, Engine.Utility.ITimer
    {
        public enum RoleAction
        {
            NONE,
            USESKILL,
            PICKUPITEM,
            MOVETOWAWEPOINT,
        }

        public class WaveInfo
        {
            public uint nWaveid = 0;
            public List<UnityEngine.Vector2> m_lstPos = new List<UnityEngine.Vector2>();
        }

        #region const
        private const int MEDICAL_TIMEID = 123;
        private const int MEDICAL_TIME_INTERVAL = 1500;
        private const int COMBAT_TIMEID = 12345;
        private const int COMBAT_TIME_INTERVAL = 250;
        private const int ATONCE_TIMERID = 124;//瞬药定时器
        private const int ATONCE_LEN = 200;//瞬药间隔200ms
        #endregion

        private IController m_ActiveCtrl = null;

        private RoleAction m_RoleAction = RoleAction.NONE;
        private CombatRobotStatus m_status = CombatRobotStatus.STOP;
        //掉线时候的状态
        private CombatRobotStatus m_disconnectStatus = CombatRobotStatus.STOP;
        //配置
        private uint m_nSearchRang = 100;//寻怪范围
        private float m_fPauseCombatTime = 0;//暂停挂机恢复时间
        //skilllist
        Client.ISkillPart m_skillPart = null;
        List<table.SkillDatabase> m_lstSkills = null;//当前挂在技能面板的技能 包括普工（第一个）
        int m_nSkillUseIndex = 0;//当前使用技能索引
        uint m_nIgnoreSkillid = 0;

        //指定攻击目标id
        int m_nTargetID = 0;
        private UnityEngine.Vector3 m_centerPos = UnityEngine.Vector3.zero;//挂机中心点

        //暂停开始计时时间
        float m_fStopTime = 0;
        float m_fUsesKillTime = 0;

        //掉落物品拾取
        uint m_nPickItemId = 0;

        //副本挂机
        bool m_bInCopy = false;//是否在副本挂机
        uint m_nCopyId = 0;
        uint m_nLaskKillWave = 0;
        uint m_nLaskMovePosIndex = 0;
        Dictionary<uint, WaveInfo> m_dicWaveInfo = new Dictionary<uint, WaveInfo>();

        CombatRobotMono m_CombatRobotMono = null;
        uint m_uInsertSkill = 0;
        /// <summary>
        /// 插入的技能优先释放
        /// </summary>
        uint InsertSkillID
        {
            get
            {
                return m_uInsertSkill;
            }
            set
            {
                m_uInsertSkill = value;
                //插入技能后 清除连击记忆
                m_uNextSkill = 0;
            }
        }

        uint m_uNextSkill = 0;
        /// <summary>
        /// 连击时下一个技能
        /// </summary>
        uint NextSkill
        {
            get
            {
                return m_uNextSkill;
            }
            set
            {
                m_uNextSkill = value;

            }
        }

        public CombatRobot()
        {
            m_nSearchRang = GameTableManager.Instance.GetGlobalConfig<uint>("Auto_Combat_Rang");
            m_fPauseCombatTime = (float)GameTableManager.Instance.GetClientGlobalConst<int>("AutoCombat", "AutoCombatTime");

            AddEventListener(GameEventID.ENTITYSYSTEM_MAINPLAYERCREATE);
            AddEventListener(GameEventID.ENTITYSYSTEM_RELIVE);
            AddEventListener(GameEventID.ENTITYSYSTEM_ENTITYDEAD);

        }

        #region  ICombatRobot

        public CombatRobotStatus Status
        {
            get { return m_status; }
        }
        public int TargetId
        {
            get { return m_nTargetID; }
        }
        public void AddTips(string msg)
        {
            if (m_CombatRobotMono != null)
            {
                m_CombatRobotMono.AddLog(msg);
            }
        }

        public void Start()
        {
            m_nTargetID = 0;
            OnStart(true);
        }

        public void StartInCopy(uint nCopyId, uint nWaveid, uint nPosIndex)
        {
            m_bInCopy = true;
            m_nCopyId = nCopyId;
            m_nLaskKillWave = nWaveid;
            m_nLaskMovePosIndex = nPosIndex;

            AddEventListener(GameEventID.ROBOTCOMBAT_COPYKILLWAVE);
            InitCopyCombatData();

            OnStart(nWaveid == 0);
        }

        public void StartWithTarget(int ntargetId)
        {
            m_nTargetID = ntargetId;
            OnStart(true);
        }
        public void Stop(bool stopTimer)
        {
            Stop();
            if (stopTimer)
            {
                KillTimer(MEDICAL_TIMEID);
                KillTimer(ATONCE_TIMERID);
            }
        }
        public void Stop()
        {
            // UnityEngine.Debug.Log("stop..." + m_status);
            if (m_status == CombatRobotStatus.STOP)
            {
                return;
            }
            //  AddTips("stop");
            CmdManager.Instance().Clear();//清空指令
            //取消挂机定时器
            KillTimer(COMBAT_TIMEID);

            if (m_bInCopy)
            {
                RemoveListener(GameEventID.ROBOTCOMBAT_COPYKILLWAVE);
            }

            RemoveListener(GameEventID.SKILLNONESTATE_ENTER);//播放完技能
            RemoveListener(GameEventID.ROBOTCOMBAT_NEXTCMD);//技能连招
            RemoveListener(GameEventID.SKILLGUIDE_PROGRESSBREAK);//技能使用失败
            RemoveListener(GameEventID.JOYSTICK_UNPRESS);//摇杆松开
            RemoveListener(GameEventID.JOYSTICK_PRESS);//摇杆按下
            RemoveListener(GameEventID.ENTITYSYSTEM_CREATEENTITY);//物品拾取
            RemoveListener(GameEventID.ENTITYSYSTEM_REMOVEENTITY);//物品拾取

            RemoveListener(GameEventID.SKILLSYSTEM_ADDSKILL);//技能刷新
            RemoveListener(GameEventID.SKILLSYSTEM_SKILLLISTCHANE);
            RemoveListener(GameEventID.NETWORK_CONNECTE_CLOSE);
            RemoveListener(GameEventID.RECONNECT_SUCESS);
            RemoveListener(GameEventID.SKILLSYSTEM_STIFFTIMEOVER);
            if (m_CombatRobotMono != null)
            {
                m_CombatRobotMono.m_lstLog.Clear();
            }
            m_bInCopy = false;
            m_nTargetID = 0;

            m_nCopyId = 0;
            m_nLaskKillWave = 0;
            m_nLaskMovePosIndex = 0;
            m_nPickItemId = 0;
            if (m_lstSkills != null)
            {
                m_lstSkills.Clear();
            }

            if (m_dicWaveInfo != null)
            {
                m_dicWaveInfo.Clear();
            }
            ChangeRoleAction(RoleAction.NONE);
            ChangeStatus(CombatRobotStatus.STOP);
            StopMove();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_STOP, null);
        }

        public void Pause()
        {
            m_fStopTime = UnityEngine.Time.realtimeSinceStartup;
            OnPause(!m_bInCopy);
        }
        public void InsertSkill(uint skillID)
        {
            InsertSkillID = skillID;
        }
        public void Resume()
        {

        }
        #endregion

        #region robot

        private void OnStart(bool bShowTip)
        {
            IPlayer mainPlayer = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (mainPlayer == null)
            {
                return;
            }
            m_ActiveCtrl = ControllerSystem.m_ClientGlobal.GetControllerSystem().GetActiveCtrl();

            m_centerPos = mainPlayer.GetPos();

            if (m_status == CombatRobotStatus.PAUSE)
            {
                OnResume(false);
                return;
            }
            if (m_status != CombatRobotStatus.STOP)
            {
                return;
            }
            if (Engine.RareEngine.Instance().Root != null && m_CombatRobotMono == null)
            {
                m_CombatRobotMono = Engine.RareEngine.Instance().Root.AddComponent<CombatRobotMono>();
            }

            InitSkill();
            //如果在移动 立即停止
            StopMove();

            AddEventListener(GameEventID.SKILLNONESTATE_ENTER);//播放完技能
            AddEventListener(GameEventID.ROBOTCOMBAT_NEXTCMD);//技能连招
            AddEventListener(GameEventID.SKILLGUIDE_PROGRESSBREAK);//技能使用失败
            AddEventListener(GameEventID.JOYSTICK_UNPRESS);//摇杆松开
            AddEventListener(GameEventID.JOYSTICK_PRESS);//摇杆按下
            AddEventListener(GameEventID.ENTITYSYSTEM_REMOVEENTITY);//物品拾取
            AddEventListener(GameEventID.ENTITYSYSTEM_CREATEENTITY);//物品拾取


            AddEventListener(GameEventID.SKILLSYSTEM_ADDSKILL);//技能刷新
            AddEventListener(GameEventID.SKILLSYSTEM_SKILLLISTCHANE);
            AddEventListener(GameEventID.NETWORK_CONNECTE_CLOSE);
            AddEventListener(GameEventID.RECONNECT_SUCESS);
            AddEventListener(GameEventID.SKILLSYSTEM_STIFFTIMEOVER);
            Client.ITipsManager tip = ControllerSystem.m_ClientGlobal.GetTipsManager();
            if (tip != null && bShowTip)
            {
                tip.ShowTips("开始挂机！！！");
            }

            ChangeStatus(CombatRobotStatus.RUNNING);
            ChangeRoleAction(RoleAction.NONE);

            //DoNextCMD();

            SetTimer(COMBAT_TIMEID, COMBAT_TIME_INTERVAL);

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_START, null);

            UnityEngine.Debug.Log("start....");
        }

        private void OnPause(bool bShowTip)
        {
            if (m_status == CombatRobotStatus.PAUSE)
            {
                return;
            }

            ChangeStatus(CombatRobotStatus.PAUSE);
            ChangeRoleAction(RoleAction.NONE);
            if (bShowTip)
            {
                Client.ITipsManager tip = ControllerSystem.m_ClientGlobal.GetTipsManager();
                if (tip != null)
                {
                    tip.ShowTipsById(701);
                }
            }
            CmdManager.Instance().Clear();//清空指令
            RemoveListener(GameEventID.SKILLINFO_REFRESH);
            RemoveListener(GameEventID.SKILLNONESTATE_ENTER);
            RemoveListener(GameEventID.ROBOTCOMBAT_NEXTCMD);
        }

        private void OnResume(bool bShowTip)
        {
            if (m_status == CombatRobotStatus.RUNNING)
            {
                return;
            }

            IPlayer mainPlayer = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (mainPlayer == null)
            {
                return;
            }


            //如果挂机打指定怪的时候跑到别的地方 找不到指定怪则不打指定怪了
            if (m_nTargetID != 0)
            {
                IEntitySystem es = ControllerSystem.m_ClientGlobal.GetEntitySystem();
                INPC npc = es.FindNPCByBaseId(m_nTargetID);
                if (npc == null)
                {
                    m_nTargetID = 0;
                }
            }

            ChangeStatus(CombatRobotStatus.RUNNING);
            ChangeRoleAction(RoleAction.NONE);
            if (bShowTip)
            {
                Client.ITipsManager tip = ControllerSystem.m_ClientGlobal.GetTipsManager();
                if (tip != null)
                {
                    tip.ShowTipsById(702);
                }
            }

            m_centerPos = mainPlayer.GetPos();
            m_fStopTime = 0;

            AddEventListener(GameEventID.SKILLINFO_REFRESH);
            AddEventListener(GameEventID.SKILLNONESTATE_ENTER);
            AddEventListener(GameEventID.ROBOTCOMBAT_NEXTCMD);

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_START, null);
        }

        private void ChangeStatus(CombatRobotStatus status)
        {
            m_status = status;
            //Engine.Utility.Log.LogGroup("ZCX", "CombatRobot -> ChangeStatus: {0}", status.ToString());
            if (m_status == CombatRobotStatus.STOP)
            {
                Engine.Utility.Log.LogGroup("ZCX", "CombatRobot -> ChangeStatus: {0}", status.ToString());
            }
            if (m_CombatRobotMono != null)
            {
                m_CombatRobotMono.m_status = status;
            }

        }

        private void ChangeRoleAction(RoleAction action)
        {
            m_RoleAction = action;
            if (m_RoleAction == RoleAction.USESKILL)
            {
                m_fUsesKillTime = UnityEngine.Time.realtimeSinceStartup;
            }
            if (m_CombatRobotMono != null)
            {
                m_CombatRobotMono.m_RoleAction = m_RoleAction;
            }
            if (m_RoleAction == RoleAction.NONE)
            {
                //Engine.Utility.Log.Error("CombatRobot -> ChangeStatus: {0}", m_RoleAction.ToString());
            }
        }

        void UpdateCenterPos()
        {
            m_centerPos = ControllerSystem.m_ClientGlobal.MainPlayer.GetPos();
        }

        private void StopMove()
        {
            IEntity mainplayer = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (mainplayer != null)
            {
                mainplayer.SetCallback(null);
                if (m_skillPart != null && m_skillPart.GetCurSkillState() == (int)Client.SkillState.None)
                {
                    Engine.Utility.Log.Info("暂停挂机 停止移动");
                    mainplayer.SendMessage(Client.EntityMessage.EntityCommand_StopMove, mainplayer.GetPos());
                }
            }
        }

        private void DoNextCMD()
        {
            if (m_RoleAction == RoleAction.PICKUPITEM)
            {
                DoPickUpItem();
            }
            else if (m_RoleAction == RoleAction.USESKILL)
            {
                DoAttack();
            }
            else if (m_RoleAction == RoleAction.MOVETOWAWEPOINT)
            {
                if (m_bInCopy)
                {
                    if (FindMonsterWhileMoving() == false)
                    {
                        MoveToWavePos();
                    }
                    else
                    {
                        DoAttack();
                    }
                }
            }
        }

        private void DoAttack()
        {
            Profiler.BeginSample("CombatRobot_DoAttack");

            if (!CmdManager.Instance().IsMoving() || !IsMoving())
            {
                IEntity target = null;
                if (TryGetCurrTarget(out target)) //如果当前有攻击目标
                {
                    uint skillid = GetNextSkillID();
                    if (skillid != 0 && m_skillPart.IsSkillCanUse(skillid))
                    {
                        AddAttackCmd(target, skillid);
                    }
                    else
                    {
                        ChangeRoleAction(RoleAction.NONE);
                        //  AddTips(string.Format("CombatRobot_DoAttack 技能{0} 不可使用 {1}", skillid, UnityEngine.Time.realtimeSinceStartup));
                    }
                }
                else //寻找目标开始攻击(如果没有可以捡的物品前提下)
                {
                    if (TryPickUpNow() == false)
                    {
                        FindMonsterAddAttack();
                    }
                    else
                    {
                        ChangeRoleAction(RoleAction.PICKUPITEM);
                        DoNextCMD();
                    }
                }
            }
            else
            {
                // AddTips("移动不能攻击");
                //StopMove();
                //                 if (m_RoleAction == RoleAction.USESKILL)
                //                 {
                // 
                //                 }
                //ChangeRoleAction(RoleAction.NONE);
            }
            Profiler.EndSample();
        }

        private bool IsMoving()
        {
            Client.IEntity player = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                return false;
            }
            bool moving = (bool)player.SendMessage(EntityMessage.EntityCommand_IsMove, null);
            return moving;
        }

        private void DoPickUpItem()
        {
            if (m_nPickItemId != 0)
            {
                uint itemid = m_nPickItemId;
                Client.IEntitySystem es = ControllerSystem.m_ClientGlobal.GetEntitySystem();
                if (es != null)
                {
                    Client.IBox box = es.FindBox(itemid);
                    if (box == null)
                    {
                        if (TryPickUpNow() == false)
                        {
                            OnCombat();
                        }
                    }
                    else if (box.CanAutoPick())
                    {
                        if (m_ActiveCtrl != null)
                        {
                            m_ActiveCtrl.MoveToTarget(box.GetPos());
                        }
                    }
                }
            }
            else
            {
                UnityEngine.Debug.LogError("pick up item OnCombat");
                OnCombat();
            }
        }

        private void AddAttackCmd(IEntity target, uint skillid)
        {
            Client.IPlayer player = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (target != null && skillid != 0)
            {
                if (m_CombatRobotMono != null)
                {
                    m_CombatRobotMono.AddLog(string.Format("添加攻击命令 技能id:{0} {1}", skillid, UnityEngine.Time.realtimeSinceStartup));
                }
                CmdManager.Instance().AddCmd(Cmd.Attack, player.GetUID(), target.GetUID(), skillid);
            }
            else
            {
                ChangeRoleAction(RoleAction.NONE);
            }
        }

        private IEntity GetNearestMonster()
        {
            Client.IPlayer mainplayer = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (mainplayer == null)
            {
                return null;
            }
            IEntitySystem es = ControllerSystem.m_ClientGlobal.GetEntitySystem();
            if (es == null)
            {
                return null;
            }

            IMapSystem ms = ControllerSystem.m_ClientGlobal.GetMapSystem();
            if (ms == null)
            {
                return null;
            }

            IEntity monster = null;
            if (m_nTargetID != 0)
            {
                monster = es.FindNPCByBaseId(m_nTargetID, true);
            }
            else
            {
                PLAYERPKMODEL pkmodel = (PLAYERPKMODEL)mainplayer.GetProp((int)PlayerProp.PkMode);
                MapAreaType atype = ms.GetAreaTypeByPos(mainplayer.GetPos());
                IControllerHelper ch = GetControllerHelper();
                if (ch != null)
                {
                    uint attackPriority = ch.GetAttackPriority();
                    monster = es.FindEntityByArea_PkModel(atype, pkmodel, m_centerPos, m_nSearchRang, attackPriority);
                }
            }

            return monster;
        }

        private void FindMonsterAddAttack()
        {
            IEntity target = GetNearestMonster();
            if (target != null)
            {
                //目标没死亡
                ICreature c = (target as ICreature);
                if (c != null && !c.IsDead())
                {
                    m_ActiveCtrl.UpdateTarget(target);

                    uint skillid = GetNextSkillID();
                    if (skillid != 0 && m_skillPart.IsSkillCanUse(skillid))
                    {
                        AddAttackCmd(target, skillid);
                    }
                    else
                    {
                        ChangeRoleAction(RoleAction.NONE);
                        //  AddTips(string.Format("CombatRobot->FindMonsterAddAttack 技能{0} 不可使用 {1}", skillid, UnityEngine.Time.realtimeSinceStartup));
                    }
                }
            }
            else
            {
                if (m_RoleAction != RoleAction.NONE)
                {
                    if (m_bInCopy)
                    {
                        MoveToWavePos();
                    }
                    else
                    {
                        Engine.Utility.Log.LogGroup("ZCX", "找不到攻击目标");
                        Client.IEntity player = ControllerSystem.m_ClientGlobal.MainPlayer;
                        if (player != null)
                        {
                            PlayAni anim_param = new PlayAni();
                            anim_param.strAcionName = EntityAction.Stand;
                            anim_param.fSpeed = 1;
                            anim_param.nStartFrame = 0;
                            anim_param.nLoop = -1;
                            anim_param.fBlendTime = 0.2f;
                            player.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
                        }
                        ChangeRoleAction(RoleAction.NONE);
                    }

                }
            }
        }
        /// <summary>
        /// 判断当前目标是否可以攻击
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private bool TryGetCurrTarget(out IEntity target)
        {
            target = null;
            if (m_ActiveCtrl == null)
            {
                return false;
            }

            target = m_ActiveCtrl.GetCurTarget();
            if (target != null)
            {
                if (target.IsDead())
                {
                    return false;
                }
                if (m_nTargetID != 0 && m_nTargetID != target.GetProp((int)EntityProp.BaseID))
                {
                    return false;
                }
                int error = 0;
                return m_skillPart.CheckCanAttackTarget(target, out error);

            }
            return false;
        }
        /// <summary>
        ///  副本自动战斗数据
        /// </summary>
        void InitCopyCombatData()
        {
            m_dicWaveInfo.Clear();
            table.MonsterWaveDatabase wavedata = GameTableManager.Instance.GetTableItem<table.MonsterWaveDatabase>(m_nCopyId);
            if (wavedata != null)
            {
                string[] strWaves = wavedata.strWaves.Split(';');
                for (int i = 0; i < strWaves.Length; i++)
                {
                    string strWave = strWaves[i];
                    if (string.IsNullOrEmpty(strWave))
                    {
                        continue;
                    }

                    string[] strWaveSplit = strWave.Split(':');
                    if (strWaveSplit.Length == 2)
                    {
                        WaveInfo waveInfo = new WaveInfo();
                        waveInfo.nWaveid = uint.Parse(strWaveSplit[0]);

                        string[] strPoss = strWaveSplit[1].Split('_');
                        for (int k = 0; k < strPoss.Length; k++)
                        {
                            string[] strpos = strPoss[k].Split(',');
                            if (strpos.Length == 2)
                            {
                                waveInfo.m_lstPos.Add(new UnityEngine.Vector2(int.Parse(strpos[0]), -int.Parse(strpos[1])));
                            }
                        }
                        m_dicWaveInfo.Add(waveInfo.nWaveid, waveInfo);
                    }
                }
            }
        }
        #endregion

        #region  Event
        private void AddEventListener(GameEventID eventId)
        {
            if (m_CombatRobotMono != null)
            {
                m_CombatRobotMono.m_lstEventID.Add(eventId);
            }
            Engine.Utility.EventEngine.Instance().AddEventListener((int)eventId, OnEvent);
        }

        private void RemoveListener(GameEventID eventId)
        {
            if (m_CombatRobotMono != null)
            {
                m_CombatRobotMono.m_lstEventID.Remove(eventId);
            }
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)eventId, OnEvent);
        }

        private void OnEvent(int nEventID, object param)
        {
            GameEventID eventId = (GameEventID)nEventID;
            switch (eventId)
            {
                case GameEventID.ENTITYSYSTEM_MAINPLAYERCREATE:
                    {
                        SetTimer(MEDICAL_TIMEID, MEDICAL_TIME_INTERVAL);
                        SetTimer(ATONCE_TIMERID, ATONCE_LEN);
                    }
                    break;
                case GameEventID.ENTITYSYSTEM_RELIVE:
                    {
                        stEntityRelive er = (stEntityRelive)param;
                        if (ControllerSystem.m_ClientGlobal.IsMainPlayer(er.uid))
                        {
                            SetTimer(MEDICAL_TIMEID, MEDICAL_TIME_INTERVAL);
                            SetTimer(ATONCE_TIMERID, ATONCE_LEN);
                        }
                    }
                    break;
                case GameEventID.ENTITYSYSTEM_ENTITYDEAD:
                    {
                        stEntityDead ed = (stEntityDead)param;
                        if (ControllerSystem.m_ClientGlobal.IsMainPlayer(ed.uid))
                        {
                            KillTimer(MEDICAL_TIMEID);
                            KillTimer(ATONCE_TIMERID);
                        }
                    }
                    break;
                case GameEventID.SKILLSYSTEM_SKILLLISTCHANE:
                case GameEventID.SKILLSYSTEM_ADDSKILL:
                    InitSkill();
                    break;
                case GameEventID.SKILLNONESTATE_ENTER:
                    {
                        NextSkill = 0;
                        OnCombat();
                    }
                    break;
                //case GameEventID.ROBOTCOMBAT_NEXTCMD:
                //    {
                //        if (param != null)
                //        {
                //            Client.stSkillDoubleHit skillDhHit = (Client.stSkillDoubleHit)param;
                //            OnUseDoubleHitSkill(skillDhHit);
                //        }
                //    }
                //    break;
                case GameEventID.SKILLSYSTEM_STIFFTIMEOVER:
                    {//挂机状态下 硬直结束(并且没有插入技能)只处理普攻前两招，第三招释放时如果有插入 就不播放收招 
                        stNextSkill st = (stNextSkill)param;
                        NextSkill = st.nextSkillID;
                        if (m_status == CombatRobotStatus.RUNNING && NextSkill != 0)
                        {
                            DoAttack();
                        }
                        if (m_status == CombatRobotStatus.RUNNING && NextSkill == 0 && InsertSkillID != 0)
                        {
                            DoAttack();

                        }
                    }
                    break;
                case GameEventID.SKILLGUIDE_PROGRESSBREAK:
                    {
                        if (param != null)
                        {
                            stGuildBreak skillFailed = (stGuildBreak)param;
                            if (skillFailed.action == GameCmd.UninterruptActionType.UninterruptActionType_SkillCJ)
                            {
                                if (ControllerSystem.m_ClientGlobal.IsMainPlayer(skillFailed.uid))
                                {
                                    m_nIgnoreSkillid = skillFailed.skillid;

                                    // AddTips(string.Format("使用技能失败id:{0} ret{1}", skillFailed.skillid, skillFailed.msg));
                                }
                                ChangeRoleAction(RoleAction.NONE);
                            }
                        }
                        //ChangeRoleAction(RoleAction.USESKILL);
                        //DoNextCMD();
                    }
                    break;
                case GameEventID.JOYSTICK_UNPRESS:
                    m_fStopTime = UnityEngine.Time.realtimeSinceStartup;

                    break;
                case GameEventID.JOYSTICK_PRESS:
                    m_fStopTime = 0;
                    OnPause(true);
                    break;
                case GameEventID.ENTITYSYSTEM_CREATEENTITY:
                    {
                        Client.stCreateEntity createEntity = (Client.stCreateEntity)param;
                        Client.IEntitySystem es = ControllerSystem.m_ClientGlobal.GetEntitySystem();
                        Client.IEntity entity = es.FindEntity(createEntity.uid);
                        if (entity != null)
                        {
                            if (entity.GetEntityType() == EntityType.EntityType_Box)
                            {
                                ShowBoxTips(entity.GetID());
                                OnPickUpItem();
                            }
                        }
                    }
                    break;
                case GameEventID.ENTITYSYSTEM_REMOVEENTITY:
                    {
                        Client.stRemoveEntity removeEntiy = (Client.stRemoveEntity)param;
                        Client.IEntitySystem es = ControllerSystem.m_ClientGlobal.GetEntitySystem();
                        Client.IEntity entity = es.FindEntity(removeEntiy.uid);
                        if (entity != null)
                        {
                            if (entity.GetEntityType() == EntityType.EntityType_Box)
                            {
                                OnPickUpItem();
                            }
                        }
                    }
                    break;
                case GameEventID.ROBOTCOMBAT_COPYKILLWAVE:
                    {
                        if (param != null)
                        {
                            stCopySkillWave copyWave = (stCopySkillWave)param;
                            if (m_bInCopy)
                            {
                                m_nLaskKillWave = copyWave.waveId;
                                m_nLaskMovePosIndex = copyWave.posIndex;
                                //  AddTips(string.Format("副本wave{0} posIndex{1}", m_nLaskKillWave, m_nLaskMovePosIndex));
                                if (m_nLaskMovePosIndex != 0)
                                {
                                    // SetTimer(COPYCOMBAT_TIMEID, 1600);
                                    //OnPause(false);
                                }
                                //DoNextCMD();
                            }
                        }

                    }
                    break;
                case GameEventID.NETWORK_CONNECTE_CLOSE:
                    {
                        m_disconnectStatus = m_status;
                        if (m_disconnectStatus != CombatRobotStatus.STOP)
                        {
                            Stop();
                            Engine.Utility.Log.Error("掉线了 挂机停止！！！");
                        }
                    }
                    break;
                case GameEventID.RECONNECT_SUCESS:
                    {
                        if (m_disconnectStatus != CombatRobotStatus.STOP)
                        {
                            Start();
                            if (m_disconnectStatus == CombatRobotStatus.PAUSE)
                            {
                                Pause();
                            }
                            m_disconnectStatus = CombatRobotStatus.STOP;
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private bool FindMonsterWhileMoving()
        {
            UpdateCenterPos();
            IEntity target = GetNearestMonster();
            if (target != null)
            {
                StopMove();

                ChangeRoleAction(RoleAction.NONE);
                return true;
            }
            return false;
        }

        private void OnUseDoubleHitSkill(stSkillDoubleHit doubleHit)
        {
            if (doubleHit.doubleHitEnd)
            {
                m_nSkillUseIndex = 1;
                // AddTips(string.Format("连击重置{0} {1} time {2}", doubleHit.skillID, m_status.ToString(), UnityEngine.Time.realtimeSinceStartup));
                //ChangeRoleAction(RoleAction.NONE);
                //DoNextCMD();
                if (m_skillPart.GetCurSkillState() == (int)Client.SkillState.None)
                {
                    ChangeRoleAction(RoleAction.NONE);
                    OnCombat();
                }
            }
            else
            {
                // AddTips(string.Format("接着连击{0}  {1}", doubleHit.skillID, UnityEngine.Time.realtimeSinceStartup));
                if (InsertSkillID == 0)
                {
                    ChangeRoleAction(RoleAction.USESKILL);
                    m_nSkillUseIndex = 0;
                    DoNextCMD();
                }


            }
        }

        void OnPickUpItem()
        {
            if (TryPickUpNow())
            {
                ChangeRoleAction(RoleAction.PICKUPITEM);
                DoNextCMD();
            }
        }


        List<Client.IBox> lstBox = new List<Client.IBox>();
        private bool TryPickUpNow()
        {
            m_nPickItemId = 0;
            if (m_ActiveCtrl == null)
            {
                return false;
            }

            int skillStatus = m_skillPart.GetCurSkillState();

            //暂停期间不能拾取
            if (m_status == CombatRobotStatus.PAUSE)
            {
                return false;
            }

            Client.IEntitySystem es = ControllerSystem.m_ClientGlobal.GetEntitySystem();
            es.FindAllEntity<IBox>(ref lstBox);
            if ((skillStatus == (int)Client.SkillState.None) && (lstBox.Count > 1 ? true : IsMoving() == false))
            {
                for (int i = 0; i < lstBox.Count; i++)
                {
                    if (lstBox[i].CanAutoPick())
                    {
                        m_nPickItemId = lstBox[i].GetID();
                        return true;
                    }
                }
                lstBox.Clear();
            }
            return false;
        }

        /// <summary>
        /// 背包已满提示
        /// </summary>
        /// <param name="itemId"></param>
        void ShowBoxTips(uint itemId)
        {
            Client.IEntitySystem es = ControllerSystem.m_ClientGlobal.GetEntitySystem();
            IBox box = es.FindBox(itemId);
            if (box == null)
            {
                return;
            }


            Client.IControllerHelper controllerhelper = GetControllerHelper();
            if (controllerhelper != null)
            {
                int itemBaseId = box.GetProp((int)EntityProp.BaseID);
                int itemNum = box.GetProp((int)BoxProp.Number);

                //1、捡到的item  为金币
                if (itemBaseId == 60001)
                {
                    return;
                }

                //2、不可放入背包
                if (false == controllerhelper.CanPutInKanpsack((uint)itemBaseId, (uint)itemNum))
                {
                    Client.ITipsManager tip = ControllerSystem.m_ClientGlobal.GetTipsManager();
                    if (tip != null)
                    {
                        tip.ShowTips("背包空间不足");
                    }
                }
            }

        }

        private void MoveToWavePos()
        {
            if (m_ActiveCtrl != null)
            {
                if (m_RoleAction == RoleAction.MOVETOWAWEPOINT)
                {
                    return;
                }

                WaveInfo waveinfo = null;
                if (m_dicWaveInfo.TryGetValue(m_nLaskKillWave, out waveinfo))
                {
                    if (waveinfo.m_lstPos.Count > m_nLaskMovePosIndex)
                    {
                        // OnPause(false);

                        //SetTimer(COPYCOMBAT_TIMEID, COPYCOMBAT_TIME_INTERVAL);

                        EntityMoveEndCallBack callback = new EntityMoveEndCallBack();
                        callback.callback = OnMoveToTargetPos;
                        ControllerSystem.m_ClientGlobal.MainPlayer.SetCallback(callback);

                        int index = (int)m_nLaskMovePosIndex;
                        UnityEngine.Vector2 pos = new UnityEngine.Vector3(waveinfo.m_lstPos[index].x, waveinfo.m_lstPos[index].y);
                        bool canMove = m_ActiveCtrl.MoveToTarget(new UnityEngine.Vector3(pos.x, 0, pos.y), pos);

                        while (!canMove && index < waveinfo.m_lstPos.Count)
                        {
                            pos = new UnityEngine.Vector2(waveinfo.m_lstPos[index].x, waveinfo.m_lstPos[index].y);
                            m_nLaskMovePosIndex = (uint)index;

                            canMove = m_ActiveCtrl.MoveToTarget(new UnityEngine.Vector3(pos.x, 0, pos.y), pos);
                            index++;
                        }
                        if (canMove)
                        {
                            ChangeRoleAction(RoleAction.MOVETOWAWEPOINT);
                        }
                        else
                        {
                            ChangeRoleAction(RoleAction.NONE);
                        }
                        //注意要用我们的log
                        // string msg = string.Format("副本 移动 {2}wave{0} posIndex{1}  pos {3}", m_nLaskKillWave, m_nLaskMovePosIndex, canMove, pos);
                        // UnityEngine.Debug.Log(msg);
                        // AddTips(msg);
                    }
                }
            }
        }

        void OnMoveToTargetPos(object param)
        {
            ControllerSystem.m_ClientGlobal.MainPlayer.SetCallback(null);
            if (param is UnityEngine.Vector2)
            {
                UnityEngine.Vector2 targetPos = (UnityEngine.Vector2)param;
                UnityEngine.Vector3 pos = ControllerSystem.m_ClientGlobal.MainPlayer.GetPos();
                if (targetPos.x == pos.x && targetPos.y == pos.z)
                {
                    m_centerPos = pos;

                    //  AddTips(string.Format("移动到{0}", targetPos));
                }
                else
                {
                    //    AddTips(string.Format("没有移动到{0}", targetPos));
                }
            }
            // UnityEngine.Debug.Log("OnMoveToTargetPos");
            ChangeRoleAction(RoleAction.NONE);
        }

        #endregion

        #region SKILL
        void InitSkill()
        {
            IPlayer mainPlayer = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (mainPlayer == null)
            {
                Engine.Utility.Log.Error("MainPlayer is NULL");
                return;
            }
            m_skillPart = mainPlayer.GetPart(Client.EntityPart.Skill) as ISkillPart;
            if (m_skillPart == null)
            {
                Engine.Utility.Log.Error("MainPlayer SKILLPART is NULL");
                return;
            }

            m_nSkillUseIndex = 1;
            m_uNextSkill = 0;
            if (m_skillPart != null)
            {
                m_lstSkills = m_skillPart.GetCurStateSkillList();

                if (m_lstSkills == null)
                {
                    Engine.Utility.Log.Error("MainPlayer SKILLPART is NULL");
                    return;
                }

                for (int i = 0; i < m_lstSkills.Count; i++)
                {
                    if (!m_lstSkills[i].autoplay)
                    {
                        m_lstSkills.RemoveAt(i);
                        i--;
                    }
                }

                if (m_CombatRobotMono != null)
                {
                    m_CombatRobotMono.m_lstSkillids.Clear();
                    if (m_lstSkills != null)
                    {
                        for (int i = 0; i < m_lstSkills.Count; i++)
                        {
                            m_CombatRobotMono.m_lstSkillids.Add((int)m_lstSkills[i].wdID);
                        }
                    }
                }
            }
        }

        uint GetNextSkillID(bool ignoreNormalSkill = false)
        {
            if (InsertSkillID != 0)
            {
                if (m_skillPart != null)
                {
                    uint skill = InsertSkillID;
                    if (m_skillPart.IsSkillCanUse(skill))
                    {
                        InsertSkillID = 0;
                        return skill;
                    }
                }

            }
            if (m_lstSkills != null && m_skillPart != null && m_lstSkills.Count > 0)
            {
                while (m_nSkillUseIndex < m_lstSkills.Count)
                {
                    if (m_lstSkills[m_nSkillUseIndex].autoplay)
                    {
                        uint skillid = m_lstSkills[m_nSkillUseIndex].wdID;
                        if (skillid == m_nIgnoreSkillid)
                        {
                            m_nSkillUseIndex++;
                            m_nIgnoreSkillid = 0;
                        }
                        else if (m_skillPart.IsSkillCanUse(skillid))
                        {
                            //有可能 技能都cd 了 接着使用技能 不用普工

                            if (++m_nSkillUseIndex >= m_lstSkills.Count)
                            {
                                m_nSkillUseIndex = 1;

                            }
                            return skillid;
                        }
                        else
                        {
                            m_nSkillUseIndex++;
                        }
                    }
                    else
                    {
                        m_nSkillUseIndex++;
                    }
                }
                m_nSkillUseIndex = 1;
                if (!ignoreNormalSkill)
                {
                    if (NextSkill != 0)
                    {
                        return NextSkill;
                    }
                    return m_lstSkills[0].wdID;
                }
            }
            else
            {
                Engine.Utility.Log.Error("技能使用出错！！m_nSkillUseIndex{0} {1}", m_nSkillUseIndex, m_lstSkills.Count);
            }
            return 0;
        }

        /// <summary>
        /// 玩家现在是否是变身状态
        /// </summary>
        /// <returns></returns>
        bool IsChangeBody()
        {
            IPlayer mainPlayer = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (mainPlayer != null)
            {
                bool isChangeBody = (bool)mainPlayer.SendMessage(Client.EntityMessage.EntityCommand_IsChange, null);

                if (isChangeBody)
                {
                    Client.ITipsManager tip = ControllerSystem.m_ClientGlobal.GetTipsManager();
                    if (tip != null)
                    {
                        tip.ShowTips("变身后不可使用技能");
                    }
                }

                return isChangeBody;
            }
            return false;
        }

        IControllerHelper GetControllerHelper()
        {
            Client.IControllerSystem cs = ControllerSystem.m_ClientGlobal.GetControllerSystem();
            if (cs == null)
            {
                Engine.Utility.Log.Error("ControllerSystem 为 null !!!");
            }

            return cs.GetControllerHelper();
        }

        #endregion

        #region Timer

        private void SetTimer(uint nTimeid, uint nInterval)
        {
            Engine.Utility.Log.Info("SetTimer: {0}", nTimeid);
            Engine.Utility.TimerAxis.Instance().SetTimer(nTimeid, nInterval, this, Engine.Utility.TimerAxis.INFINITY_CALL, "Robot start");
        }

        private void KillTimer(uint nTimerid)
        {
            Engine.Utility.Log.Info("KillTimer : {0}", nTimerid);
            Engine.Utility.TimerAxis.Instance().KillTimer(nTimerid, this);
        }

        public void OnTimer(uint uTimerID)
        {
            IPlayer mainPlayer = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (mainPlayer == null)
            {
                Engine.Utility.Log.Error("MainPlayer is null");
                Stop();
                return;
            }

            if (mainPlayer.IsDead())
            {
                Engine.Utility.Log.Error("MainPlayer is dead");
                Stop();
                return;
            }

            if (uTimerID == MEDICAL_TIMEID)
            {
                OnUseMedecal();
                return;
            }

            if (uTimerID == COMBAT_TIMEID)
            {
                OnCombat();
                return;
            }
            if (uTimerID == ATONCE_TIMERID)
            {
                UseAtOnceMedice();
                return;
            }
        }
        #endregion
        void OnUseMedecal()
        {
            Client.IControllerHelper controllerhelper = GetControllerHelper();
            if (controllerhelper == null)
            {
                return;
            }
            controllerhelper.CheckUseMedicine();
        }

        void UseAtOnceMedice()
        {
            Client.IControllerHelper controllerhelper = GetControllerHelper();
            if (controllerhelper == null)
            {
                return;
            }
            controllerhelper.UseAtOnceMedicine();
        }
        private void OnCopyFindMonster()
        {
            FindMonsterWhileMoving();
        }

        void OnCombat()
        {
            if (m_status == CombatRobotStatus.STOP)
            {
                KillTimer(COMBAT_TIMEID);
                return;
            }

            if (m_RoleAction == RoleAction.MOVETOWAWEPOINT)
            {
                OnCopyFindMonster();
                return;
            }

            if (m_status == CombatRobotStatus.PAUSE)
            {
                if (m_fStopTime > 0 && UnityEngine.Time.realtimeSinceStartup - m_fStopTime >= m_fPauseCombatTime)
                {
                    OnResume(true);
                }
                return;
            }

            if (m_RoleAction == RoleAction.USESKILL)
            {

                if (m_skillPart.GetCurSkillState() != (int)Client.SkillState.None)
                {
                    return;
                }
                else
                {
                    ChangeRoleAction(RoleAction.NONE);
                }

            }

            if (m_RoleAction == RoleAction.PICKUPITEM)
            {
                if (m_nPickItemId != 0)
                {
                    if (IsMoving())
                    {
                        return;
                    }
                }
            }

            if (TryPickUpNow())
            {
                OnPickUpItem();
                return;
            }
            else
            {
                //变身状态不可使用技能
                if (IsChangeBody())
                {
                    return;
                }

                ChangeRoleAction(RoleAction.USESKILL);
            }

            DoNextCMD();
        }
    }

    public class EntityMoveEndCallBack : IEntityCallback
    {
        public MoveEndCallback callback = null;

        public delegate void MoveEndCallback(object param = null);

        public void OnMoveEnd(IEntity entity, object param = null)
        {
            if (callback != null)
            {
                callback(param);
            }
        }

        // 落地
        public void OnToGround(IEntity entity, object param = null)
        {

        }

        // 飞行结束
        public void OnFlyEnd(IEntity entity, object param = null)
        {

        }

        public void OnUpdate(IEntity entity, object param = null)
        {

        }

        /// <summary>
        /// 发生碰撞
        /// </summary>
        /// <param name="obj">检测对象</param>
        /// <param name="obj">碰撞信息</param>
        public void OnCollider(IEntity obj, ref EntityColliderInfo entityCollierInfo)
        {

        }
    }
}
