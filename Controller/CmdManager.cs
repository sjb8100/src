//*************************************************************************
//	创建日期:	2016-3-29   10:14
//	文件名称:	CmdManager.cs
//  创 建 人:   Even	
//	版权所有:	Even.xu
//	说    明:	命令管理器
//*************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
using Engine.Utility;

namespace Controller
{
    // 命令管理器
    public enum Cmd
    {
        NULL = 0,       // 空命令
        CloseTo = 1,    // 逼近命令 如果 lTarget==0 param1则为目标位置 否则选取目标位置 param3为目标半径
        MoveTo,         // 移动到指定位置 param1 目标点 如果param2参数不为空,param2则为NPC的baseid,则需要去查找NPC对象是否存在,如果找到对象，则转为CloseTo命令
        Attack,         // 攻击 lTarget > 0 param1技能ID param2技能等级,主目标 lTarget==0 则是客户端空放技能 param1 技能id param2 等级 param3 释放位置
        GotoMap,        // 跨地图指令 lTarget 目标地图  param1,param2 坐标
        GotoMapDirecte,  // 直接跨递图  lTarget 目标地图  param1,param2 坐标
        GotoNPC,        // 走向NPC ltarget 目标地图id  param1 npcbaseid
        GotoNPCDirecte,  // 访问可见的npc 走向NPC ltarget npcUID  
        VisiteNpc,      // 访问npc ltarget npcuid
        VisiteNpcDirecte, //      访问npc ltarget npcuid taskid (x y)
        GoToMapPosition,  //传送后寻路
    }

    // 命令描述
    public class CmdInfo
    {
        public Cmd cmd;            // 命令ID
        public long lExecutor;      // 命令执行者
        public long lTarget;        // 目标对象
        public float fAttackDis;    // 攻击距离
        public object param1;       // 参数1
        public object param2;       // 参数2
        public object param3;       // 参数3
        public object param4;       //参数4
    }

    public class CmdManager : Engine.Utility.ITimer
    {
        static CmdManager s_Inst = null;
        public static CmdManager Instance()
        {
            if (null == s_Inst)
            {
                s_Inst = new CmdManager();
            }

            return s_Inst;
        }

        // 定时器ID
        private const int CMD_TIMERID = 0;

        // 命令队列
        private List<CmdInfo> m_lstCmd = new List<CmdInfo>();

        private CmdInfo m_curCmd = null;    // 当前命令
        private Vector3 m_targetPos = Vector3.zero; // 目标位置
        private Client.INPC m_visiNpc;
        private float m_fRotY;

        bool m_bJoySticPress = false;//摇杆是否按下
        public bool IsMoving()
        {
            if (m_curCmd != null)
            {
                return m_curCmd.cmd == Cmd.CloseTo || m_curCmd.cmd == Cmd.MoveTo;
            }

            return false;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Create()
        {
            // 注册事件
            Engine.Utility.EventEngine.Instance().AddEventListener((int)(int)Client.GameEventID.SYSTEM_LOADSCENECOMPELETE, OnEventCallback);  // 地图加载完毕
            Engine.Utility.EventEngine.Instance().AddEventListener((int)(int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEventCallback); // 实体停止移动
            Engine.Utility.EventEngine.Instance().AddEventListener((int)(int)Client.GameEventID.ENTITYSYSTEM_RESETROTATION, OnEventCallback); // 访问npc结束
            Engine.Utility.EventEngine.Instance().AddEventListener((int)(int)Client.GameEventID.JOYSTICK_PRESS, OnEventCallback); // 
            Engine.Utility.EventEngine.Instance().AddEventListener((int)(int)Client.GameEventID.JOYSTICK_UNPRESS, OnEventCallback); //
            Engine.Utility.EventEngine.Instance().AddEventListener((int)(int)Client.GameEventID.CHECKINGTIME, OnEventCallback); //
        }

        //-------------------------------------------------------------------------------------------------------
        public void Close()
        {
            Clear();
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)(int)Client.GameEventID.SYSTEM_LOADSCENECOMPELETE, OnEventCallback);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)(int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEventCallback);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)(int)Client.GameEventID.ENTITYSYSTEM_RESETROTATION, OnEventCallback); // 访问npc结束
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)(int)Client.GameEventID.JOYSTICK_PRESS, OnEventCallback); // 
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)(int)Client.GameEventID.JOYSTICK_UNPRESS, OnEventCallback); // 
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)(int)Client.GameEventID.CHECKINGTIME, OnEventCallback); // 

        }

        //-------------------------------------------------------------------------------------------------------
        void OnEventCallback(int nEventID, object param)
        {
            if (nEventID == (int)Client.GameEventID.SYSTEM_LOADSCENECOMPELETE)
            {
                IControllerHelper ch = GetControllerHelper();
                if (ch == null)
                {
                    return;
                }

                if (ch.IsCheckingTime())
                {
                    //正在对时，不执行cmd
                }
                else
                {
                    NextCmd();
                }
            }
            if (nEventID == (int)Client.GameEventID.CHECKINGTIME)
            {
                bool isCheckingTime = (bool)param;

                IControllerHelper ch = GetControllerHelper();
                if (ch == null)
                {
                    return;
                }

                if (ch.IsCheckingTime() == true && isCheckingTime == false)
                {
                    NextCmd();
                }

            }
            if (nEventID == (int)Client.GameEventID.JOYSTICK_PRESS)
            {
                m_bJoySticPress = true;
            }
            if (nEventID == (int)Client.GameEventID.JOYSTICK_UNPRESS)
            {
                m_bJoySticPress = false;
            }
            if (nEventID == (int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE)
            {
                if (m_curCmd == null)
                {
                    return;
                }

                if (m_curCmd.cmd != Cmd.CloseTo)
                {
                    return;
                }

                stEntityStopMove stStopMove = (stEntityStopMove)param;
                if (m_curCmd.lExecutor != stStopMove.uid)
                {
                    return;
                }

                Engine.Utility.TimerAxis.Instance().KillTimer(CMD_TIMERID, this);

                NextCmd(); // 执行下一个指令
            }

            if (nEventID == (int)Client.GameEventID.ENTITYSYSTEM_RESETROTATION)
            {
                if (m_visiNpc != null)
                {
                    m_visiNpc.SendMessage(EntityMessage.EntityCommand_SetRotateY, m_fRotY);
                    m_visiNpc = null;
                }
            }
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

        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 添加命令
        @param cmdid     命令ID
        @param lExecutor 执行者 执行者永远都只能是主角
        @param lTarget   命令目标 主目标
        */
        public void AddCmd(Cmd cmdid, long lExecutor, long lTarget = 0, object param1 = null, object param2 = null, object param3 = null, object param4 = null)
        {

            CmdInfo cmd = new CmdInfo();
            cmd.cmd = cmdid;
            cmd.lExecutor = lExecutor;
            cmd.lTarget = lTarget;
            cmd.param1 = param1;
            cmd.param2 = param2;
            cmd.param3 = param3;
            cmd.param4 = param4;

            if (cmdid == Cmd.Attack || cmdid == Cmd.GotoMap || cmdid == Cmd.GotoMapDirecte || cmdid == Cmd.GotoNPC || cmdid == Cmd.VisiteNpcDirecte || cmdid == Cmd.GotoNPCDirecte || cmdid == Cmd.GoToMapPosition) // 如果是攻击命令或者走地图指令
            {
                Clear();
                m_lstCmd.Add(cmd);
                NextCmd();
            }
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 清理队列中的命令
        @param 
        */
        public void Clear()
        {
            m_lstCmd.Clear();
            m_curCmd = null;

            Engine.Utility.TimerAxis.Instance().KillTimer(CMD_TIMERID, this);
        }

        //-------------------------------------------------------------------------------------------------------
        // 计时器
        public void OnTimer(uint uTimerID)
        {
            if (m_curCmd == null)
            {
                Engine.Utility.TimerAxis.Instance().KillTimer(CMD_TIMERID, this);
                return;
            }
            switch (m_curCmd.cmd)
            {
                case Cmd.CloseTo:
                    {
                        CloseTo_Timer();
                        break;
                    }
                case Cmd.MoveTo:
                    {
                        MoveTo_Timer();
                        break;
                    }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region TimerProcess
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 逼近定时器处理
        @param 
        */
        private void CloseTo_Timer()
        {
            if (m_curCmd == null)
            {
                return;
            }

            IPlayer player = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: player is null");
                return;
            }

            IEntitySystem es = ControllerSystem.m_ClientGlobal.GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: EntitySystem is null");
                return;
            }

            Vector3 pos = player.GetPos();
            float fAttackRadius = player.GetRadius();
            Vector3 targetPos = Vector3.zero;
            IEntity target = es.FindEntity(m_curCmd.lTarget);
            if (target != null)
            {

                targetPos = target.GetPos();
            }
            else
            {
                targetPos = m_targetPos;
            }

            float visiDistance = (m_curCmd.fAttackDis + (float)m_curCmd.param3 + fAttackRadius);
            //visiDistance *= visiDistance;
            float currDistance = 10000;// Vector3.SqrMagnitude(targetPos - pos);
            IMapSystem ms = ControllerSystem.m_ClientGlobal.GetMapSystem();
            if (ms != null)
            {
                ms.CalcDistance(new Vector2(pos.x, -pos.z), new Vector2(targetPos.x, -targetPos.z), out currDistance, TileType.TileType_None);
            }
            if (currDistance < visiDistance) // 认为已经到达目标
            {
                //   Log.Error("close to ============================ stopmove");
                // 同步位置给服务器
                player.SendMessage(EntityMessage.EntityCommand_StopMove, (object)pos);

                // 执行下一指令
                NextCmd();
                // 关闭Close定时器
                Engine.Utility.TimerAxis.Instance().KillTimer(CMD_TIMERID, this);
            }
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 
        @param 
        */
        private void MoveTo_Timer()
        {
            if (m_curCmd == null)
            {
                return;
            }

            IPlayer player = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: player is null");
                return;
            }

            IEntitySystem es = ControllerSystem.m_ClientGlobal.GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: EntitySystem is null");
                return;
            }

            IMapSystem map = ControllerSystem.m_ClientGlobal.GetMapSystem();
            if (map == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: IMapSystem is null");
                return;
            }

            if (m_curCmd.param2 == null)
            {
                return;
            }

            if (map.GetMapID() == (uint)m_curCmd.lTarget)
            {
                uint npcid = (uint)m_curCmd.param2;
                INPC npc = es.FindNPCByBaseId((int)npcid);
                if (npc != null)
                {
                    CmdInfo newCmd = new CmdInfo();
                    newCmd.cmd = Cmd.VisiteNpc;
                    newCmd.lExecutor = player.GetUID();
                    newCmd.lTarget = npc.GetUID();
                    newCmd.param2 = m_curCmd.param2;
                    InsterCmd(newCmd);

                    Engine.Utility.TimerAxis.Instance().KillTimer(CMD_TIMERID, this);

                    CmdInfo newCmd2 = new CmdInfo();
                    newCmd2.cmd = Cmd.CloseTo;
                    //newCmd2.fAttackDis = 0; // 攻击距离
                    table.NpcDataBase npcTable = GameTableManager.Instance.GetTableItem<table.NpcDataBase>(npcid);
                    if (npcTable != null)
                    {
                        newCmd2.fAttackDis = npcTable.dwCallDis * 0.01f;
                    }
                    newCmd2.lExecutor = player.GetUID();
                    newCmd2.lTarget = npc.GetUID();
                    newCmd2.param2 = m_curCmd.param2;
                    newCmd2.param3 = npc.GetRadius();

                    m_curCmd = null;

                    InsterCmd(newCmd2);

                    NextCmd();
                }
            }

        }
        #endregion

        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 插入命令
        @param 
        */
        private void InsterCmd(CmdInfo cmd)
        {
            m_lstCmd.Insert(0, cmd);
        }
        //-------------------------------------------------------------------------------------------------------
        private void PushCmd(CmdInfo cmd)
        {
            m_lstCmd.Add(cmd);
        }
        //-------------------------------------------------------------------------------------------------------
        private bool NextCmd()
        {
            if (m_lstCmd.Count > 0)
            {
                //Debug.Log("NextCmd  " + m_lstCmd[0].cmd + m_lstCmd.Count);

                ExecuteCmd(m_lstCmd[0]);
                return true;
            }
            return false;
        }

        // 删除队列中头一个指令
        private void RemoveFirst()
        {
            if (m_lstCmd.Count > 0)
            {
                //Debug.Log(" RemoveFirst " + m_lstCmd[0].cmd + m_lstCmd.Count);

                m_lstCmd.RemoveAt(0);
            }
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 执行命令
        @param 
        */
        private void ExecuteCmd(CmdInfo cmd)
        {
            m_curCmd = null;
            //Engine.Utility.Log.LogGroup("ZCX", "ExecuteCmd {0}", cmd.cmd.ToString());
            switch (cmd.cmd)
            {
                case Cmd.Attack:
                    {
                        DoAttack(cmd);
                        break;
                    }
                case Cmd.MoveTo:
                    {
                        DoMoveTo(cmd);
                        break;
                    }
                case Cmd.CloseTo:
                    {
                        DoCloseTo(cmd);
                        break;
                    }
                case Cmd.GotoMap:
                    {
                        DoGotoMap(cmd);
                        break;
                    }
                case Cmd.GotoMapDirecte:
                    {
                        DoGotoMapDirecte(cmd);
                        break;
                    }
                case Cmd.GotoNPC:
                    {
                        DoGotoNpc(cmd);
                        break;
                    }
                case Cmd.GotoNPCDirecte:
                    {
                        DoGotoNpcDirecte(cmd);
                        break;
                    }
                case Cmd.VisiteNpc:
                    {
                        DoVisiteNpc(cmd);
                        break;
                    }
                case Cmd.VisiteNpcDirecte:
                    {
                        DoVisiteNpcDirecte(cmd);
                        break;
                    }
                case Cmd.GoToMapPosition:
                    {
                        DoGoToMapPosition(cmd);
                        break;
                    }
            }
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region DoCmd
        private void DoAttack(CmdInfo cmd)
        {
            IPlayer player = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: player is null");
                return;
            }

            float fAttackRadius = player.GetRadius();
            Vector3 pos = player.GetPos();

            IEntitySystem es = ControllerSystem.m_ClientGlobal.GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: EntitySystem is null");
                return;
            }

            uint nSkillID = 0;
            Vector3 targetPos = Vector3.zero;
            float fRadius = 0.0f;
            IEntity target = es.FindEntity(cmd.lTarget);

            if (cmd.lTarget > 0)
            {
                if (target != null)
                {
                    if (target.GetEntityType() != EntityType.EntityType_Player && target.IsDead())
                    {
                        return;
                    }
                }

                if (target != null)
                {
                    targetPos = target.GetPos();
                    fRadius = target.GetRadius();
                }
                else
                {
                    Engine.Utility.Log.Error("ExecuteCmd: FindEntity {0} failed", cmd.lTarget);
                    //目标可能下线了 直接移除指令 执行下一个
                    RemoveFirst();
                    //                     if ( !NextCmd() )//如果被攻击者死了，在挂机状态下要继续寻找下一个目标执行
                    //                     {
                    //                         Client.IControllerSystem cs = ControllerSystem.m_ClientGlobal.GetControllerSystem();
                    //                         if ( cs != null )
                    //                         {
                    //                             Client.IRobot robot = cs.GetRobot();
                    //                             if ( robot != null )
                    //                             {
                    //                                 if ( robot.IsRunning )
                    //                                 {
                    //                                     Engine.Utility.EventEngine.Instance().DispatchEvent((int) (int)GameEventID.ROBOTCOMBAT_NEXTCMD , null );
                    //                                 }
                    //                             }
                    //                         }
                    //                     }
                    return;
                }
                nSkillID = (uint)cmd.param1;
            }
            else
            {
                targetPos = (Vector3)cmd.param3;
                nSkillID = (uint)cmd.param1;
                fRadius = 0;
            }


            // 检查攻击距离
            table.SkillDatabase skill = GameTableManager.Instance.GetTableItem<table.SkillDatabase>(nSkillID);
            if (skill == null)
            {
                Engine.Utility.Log.Error("DoAttack: 没有找到技能{0}配置", nSkillID);
                return;
            }
            if (skill.dwAttackDis > 0 && skill.dwAttackDis + fRadius + fAttackRadius < Vector3.Distance(pos, targetPos))
            {
                if (m_bJoySticPress)
                {
                    stSkillLongPress longPress = new stSkillLongPress();
                    longPress.bLongPress = false;
                    EventEngine.Instance().DispatchEvent((int)GameEventID.SKLL_LONGPRESS, longPress);
                    RemoveFirst();
                    Engine.Utility.Log.LogGroup("ZDY", "摇杆按下 不去寻怪");
                    return;
                }
                CmdInfo newCmd = new CmdInfo();
                newCmd.cmd = Cmd.CloseTo;
                newCmd.fAttackDis = skill.dwAttackDis; // 攻击距离
                newCmd.lExecutor = player.GetUID();
                newCmd.lTarget = cmd.lTarget;
                if (newCmd.lTarget == 0)
                {
                    newCmd.param1 = cmd.param3;
                }

                newCmd.param2 = cmd.param2;
                newCmd.param3 = fRadius;

                InsterCmd(newCmd);
                NextCmd();

                return;
            }


            ISkillPart skillPart = player.GetPart(EntityPart.Skill) as ISkillPart;
            if (skillPart == null)
            {
                Engine.Utility.Log.Error("ReqUseSkill:skillPart is null");
                return;
            }

            // 停止
            if (player.GetCurState() == CreatureState.Move)
            {
                player.SendMessage(EntityMessage.EntityCommand_StopMove, pos);
            }
            //在找目标时会更新目标
            //IController controller = ControllerSystem.m_ClientGlobal.GetControllerSystem().GetActiveCtrl();
            //if ( controller != null )
            //{
            //    controller.UpdateTarget( target );
            //}
            // 调用技能部件 使用技能
            skillPart.ReqUseSkill(nSkillID);
            Engine.Utility.Log.LogGroup("ZCX", "自动攻击-------执行技能：目技能{0}---------", nSkillID);

            m_curCmd = cmd;
            RemoveFirst();
        }

        private void DoCloseTo(CmdInfo cmd)
        {
            IEntitySystem es = ControllerSystem.m_ClientGlobal.GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: EntitySystem is null");
                return;
            }

            Vector3 targetPos = Vector3.zero;
            float fRadius = 0.0f;
            if (cmd.lTarget > 0)
            {
                IEntity target = es.FindEntity(cmd.lTarget);
                if (target != null)
                {
                    targetPos = target.GetPos();
                    fRadius = target.GetRadius();
                }
                else
                {
                    Engine.Utility.Log.Error("ExecuteCmd: FindEntity {0} failed", cmd.lTarget);
                }
            }
            else
            {
                targetPos = (Vector3)cmd.param1;
            }

            // 移动到目标
            IControllerSystem cs = ControllerSystem.m_ClientGlobal.GetControllerSystem();
            if (cs == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: ControllerSystem is null");
                return;
            }

            IPlayer player = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: player is null");
                return;
            }

            //Vector3 pos = player.GetPos();
            //Vector3 dir = targetPos - pos;
            //dir.Normalize();
            //targetPos = targetPos - dir * fRadius;

            IController ctrl = cs.GetActiveCtrl();
            if (ctrl != null)
            {
                stVoteEntityMove move;
                move.uid = player.GetUID();
                if (!Engine.Utility.EventEngine.Instance().DispatchVote((int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE, move))
                {
                    Engine.Utility.Log.LogGroup("ZDY", "can not move");
                    Clear();
                    return;
                }

                if (!ctrl.MoveToTarget(targetPos))
                {
                    IEntity target = es.FindEntity(cmd.lTarget);
                    if (target != null)
                    {
                        Engine.Utility.Log.Error("寻路失败 npc id:{0}", target.GetID());
                    }
                }
            }

            m_curCmd = cmd;
            m_targetPos = targetPos; // 目标位置
            RemoveFirst();
            Engine.Utility.TimerAxis.Instance().KillTimer(CMD_TIMERID, this);
            Engine.Utility.TimerAxis.Instance().SetTimer(CMD_TIMERID, 30, this, Engine.Utility.TimerAxis.INFINITY_CALL, "CmdManager.ExecuteCmd:MoveToTarget");
        }

        private void DoMoveTo(CmdInfo cmd)
        {
            IEntitySystem es = ControllerSystem.m_ClientGlobal.GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: EntitySystem is null");
                return;
            }

            Vector3 targetPos = (Vector3)cmd.param1;

            // 移动到目标
            IControllerSystem cs = ControllerSystem.m_ClientGlobal.GetControllerSystem();
            if (cs == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: ControllerSystem is null");
                return;
            }

            IPlayer player = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: player is null");
                return;
            }

            if (player.GetPos() != targetPos)
            {
                IController ctrl = cs.GetActiveCtrl();
                if (ctrl != null)
                {
                    ctrl.MoveToTarget(targetPos);
                }
                if (cmd.param2 is uint)
                {
                    table.NpcDataBase npcdb = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)cmd.param2);
                    if (npcdb == null || !npcdb.IsMonster())
                    {
                        m_curCmd = cmd;
                        m_targetPos = targetPos; // 目标位置
                    }
                    else
                    {
                        //如果是找到怪物群的点 置空
                        m_curCmd = null;
                    }
                }
                else
                {
                    m_curCmd = cmd;
                    m_targetPos = targetPos; // 目标位置
                }
            }
            RemoveFirst();
        }

        private void DoGotoMap(CmdInfo cmd)
        {
            //暂停挂机
            Client.IControllerSystem cs = ControllerSystem.m_ClientGlobal.GetControllerSystem();
            if (cs != null)
            {
                Client.ICombatRobot robot = cs.GetCombatRobot();
                if (robot != null && robot.Status != CombatRobotStatus.STOP)
                {
                    robot.Stop();
                }
            }
            IPlayer player = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: player is null");
                return;
            }

            IMapSystem mapSys = ControllerSystem.m_ClientGlobal.GetMapSystem();
            if (mapSys == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: mapSys is null");
                return;
            }

            Vector3 pos = Vector3.zero;
            uint uCurMapID = mapSys.GetMapID();
            if (uCurMapID == cmd.lTarget) // 当前地图与目标地图一样 则直接位移
            {
                pos = new Vector3((float)cmd.param1, 0, (float)cmd.param2);
                // 移除当前指令
                RemoveFirst();
            }
            else
            {
                if (!mapSys.FindMapLinkPoint(uCurMapID, (uint)cmd.lTarget, out pos))
                {
                    Engine.Utility.Log.Error("查找地图{0}->{1}的传送点失败", uCurMapID, cmd.lTarget);

                    Client.ITipsManager tip = ControllerSystem.m_ClientGlobal.GetTipsManager();
                    if (tip != null)
                    {
                        tip.ShowTips("该地图无法寻路");
                    }

                    return;
                }

                pos.z = -pos.z; // 
            }

            m_curCmd = cmd;

            // 执行Goto命令
            CmdInfo newCmd = new CmdInfo();
            newCmd.cmd = Cmd.MoveTo;
            newCmd.lExecutor = player.GetUID();
            newCmd.lTarget = 0;
            newCmd.param1 = pos;
            newCmd.param2 = (uint)0; // npcid
            InsterCmd(newCmd);
            NextCmd();
        }

        private void DoGotoMapDirecte(CmdInfo cmd)
        {
            IPlayer player = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: player is null");
                return;
            }

            IMapSystem mapSys = ControllerSystem.m_ClientGlobal.GetMapSystem();
            if (mapSys == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: mapSys is null");
                return;
            }

            uint uCurMapID = mapSys.GetMapID();

            // 移除当前指令
            RemoveFirst();

            m_curCmd = cmd;

            Vector3 newPos = new Vector3((float)cmd.param2, 0, (float)cmd.param3);

            // 执行Goto命令
            CmdInfo newCmd = new CmdInfo();
            newCmd.cmd = Cmd.MoveTo;
            newCmd.lExecutor = player.GetUID();
            newCmd.lTarget = 0;
            newCmd.param1 = newPos;
            newCmd.param2 = (uint)0; // npcid
            InsterCmd(newCmd);

            if (uCurMapID != (uint)cmd.lTarget)
            {
                GameCmd.sTaskHelpGoToScriptUserCmd_CS sendcmd = new GameCmd.sTaskHelpGoToScriptUserCmd_CS();
                //UnityEngine.Vector2 pos = (UnityEngine.Vector2)cmd.param3;
                //sendcmd.x = (uint)pos.x;
                //sendcmd.y = (uint)pos.y;
                sendcmd.taskid = (uint)cmd.param1;
                //sendcmd.npcid = (uint)cmd.param1;
                //sendcmd.mapid = (uint)cmd.lTarget;
                //切换地图前 主角停止移动

                bool ismoving = (bool)player.SendMessage(Client.EntityMessage.EntityCommand_IsMove, null);
                if (ismoving)
                {
                    player.SendMessage(Client.EntityMessage.EntityCommand_StopMove, player.GetPos());
                }

                ControllerSystem.m_ClientGlobal.netService.Send(sendcmd);
            }
            else
            {
                NextCmd();
            }
        }

        //-------------------------------------------------------------------------------------------------------
        private void DoGotoNpc(CmdInfo cmd)
        {
            //暂停挂机
            Client.IControllerSystem cs = ControllerSystem.m_ClientGlobal.GetControllerSystem();
            if (cs != null)
            {
                Client.ICombatRobot robot = cs.GetCombatRobot();
                if (robot != null && robot.Status != CombatRobotStatus.STOP)
                {
                    robot.Stop();
                }
            }
            IPlayer player = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: player is null");
                return;
            }

            IMapSystem mapSys = ControllerSystem.m_ClientGlobal.GetMapSystem();
            if (mapSys == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: mapSys is null");
                return;
            }

            IEntitySystem es = ControllerSystem.m_ClientGlobal.GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: EntitySystem is null");
                return;
            }

            m_curCmd = cmd;
            Vector3 pos = Vector3.zero;
            uint uCurMapID = mapSys.GetMapID();
            uint npcid = (uint)cmd.param1;  // baseid
            if (uCurMapID == cmd.lTarget) // 当前地图与目标地图一样 则直接位移
            {
                // 移除当前指令
                RemoveFirst();

                table.NpcDataBase npcdb = GameTableManager.Instance.GetTableItem<table.NpcDataBase>(npcid);
                if (npcdb != null && npcdb.IsMonster())
                {
                    UnityEngine.Vector2 npcPos = UnityEngine.Vector2.zero;
                    if (!mapSys.GetClienNpcPos((int)npcid, out npcPos))
                    {
                        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_VISITNPC,
                            new Client.stVisitNpc() { npcid = npcid, state = false });
                        Engine.Utility.Log.Error("DoGotoMap: 找不到地图{0}({1})上的npc{2}", mapSys.GetMapID(), mapSys.GetMapName(), npcid);
                        return;
                    }
                    pos = new UnityEngine.Vector3(npcPos.x, 0, -npcPos.y);

                    // 添加MoveTo指令
                    CmdInfo newCmd = new CmdInfo();
                    newCmd.cmd = Cmd.MoveTo;
                    newCmd.lExecutor = player.GetUID();
                    newCmd.lTarget = cmd.lTarget;
                    newCmd.param1 = pos;
                    newCmd.param2 = cmd.param1;  // npcid
                    InsterCmd(newCmd);
                    NextCmd();
                }
                else
                {
                    // 先查找附近的npc 支持动态npc 只查找附近的
                    INPC npc = es.FindNPCByBaseId((int)npcid);
                    if (npc != null)
                    {
                        pos = npc.GetPos();
                        table.NpcDataBase npcTable = GameTableManager.Instance.GetTableItem<table.NpcDataBase>(npcid);
                        if (npcTable != null)
                        {
                            Vector3 mainPos = player.GetPos();
                            if (VisitNpc(ref es, ref npcTable, ref mainPos, ref pos))
                            {
                                return;
                            }
                        }
                        CmdInfo newCmd = new CmdInfo();
                        newCmd.cmd = Cmd.VisiteNpc;
                        newCmd.lExecutor = player.GetUID();
                        newCmd.lTarget = npc.GetUID();
                        newCmd.param2 = m_curCmd.param2;
                        InsterCmd(newCmd);

                        Engine.Utility.TimerAxis.Instance().KillTimer(CMD_TIMERID, this);

                        CmdInfo newCmd2 = new CmdInfo();
                        newCmd2.cmd = Cmd.CloseTo;

                        if (npcTable != null)
                        {
                            newCmd2.fAttackDis = npcTable.dwCallDis * 0.01f;
                        }

                        newCmd2.lExecutor = player.GetUID();
                        newCmd2.lTarget = npc.GetUID();
                        newCmd2.param2 = m_curCmd.param2;
                        newCmd2.param3 = npc.GetRadius();

                        InsterCmd(newCmd2);
                        NextCmd();
                    }
                    else
                    {
                        Vector2 npspos = Vector2.zero;
                        // 再去查找地图上静态表里的npc
                        if (!mapSys.GetClienNpcPos((int)npcid, out npspos))
                        {
                            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_VISITNPC,
                                new Client.stVisitNpc() { npcid = npcid, state = false });
                            Engine.Utility.Log.Error("DoGotoMap: 找不到地图{0}({1})上的npc{2}", mapSys.GetMapID(), mapSys.GetMapName(), npcid);
                            return;
                        }

                        pos = new Vector3(npspos.x, 0, -npspos.y);

                        // 添加MoveTo指令
                        CmdInfo newCmd = new CmdInfo();
                        newCmd.cmd = Cmd.MoveTo;
                        newCmd.lExecutor = player.GetUID();
                        newCmd.lTarget = cmd.lTarget;
                        newCmd.param1 = pos;
                        newCmd.param2 = cmd.param1;  // npcid
                        InsterCmd(newCmd);
                        NextCmd();

                        Engine.Utility.TimerAxis.Instance().KillTimer(CMD_TIMERID, this);
                        Engine.Utility.TimerAxis.Instance().SetTimer(CMD_TIMERID, 30, this, Engine.Utility.TimerAxis.INFINITY_CALL, "CmdManager.ExecuteCmd:MoveToTarget");
                    }
                }
            }
            else
            {
                //                 // 添加MoveTo指令
                //                 CmdInfo newCmd = new CmdInfo();
                //                 newCmd.cmd = Cmd.MoveTo;
                //                 newCmd.lExecutor = player.GetUID();
                //                 newCmd.lTarget = cmd.lTarget;
                //                 newCmd.param1 = pos;
                //                 newCmd.param2 = cmd.param1;  // npcid
                //                 InsterCmd(newCmd);
                //                 NextCmd();
                if (mapSys != null)
                {
                    //切换地图前 主角停止移动
                    if (player != null)
                    {
                        bool ismoving = (bool)player.SendMessage(Client.EntityMessage.EntityCommand_IsMove, null);
                        if (ismoving)
                        {
                            player.SendMessage(Client.EntityMessage.EntityCommand_StopMove, player.GetPos());
                        }
                    }
                    mapSys.RequestEnterMap((uint)cmd.lTarget, 0);
                }

            }


        }
        private void DoGotoNpcDirecte(CmdInfo cmd)
        {
            //暂停挂机
            Client.IControllerSystem cs = ControllerSystem.m_ClientGlobal.GetControllerSystem();
            if (cs != null)
            {
                Client.ICombatRobot robot = cs.GetCombatRobot();
                if (robot != null && robot.Status != CombatRobotStatus.STOP)
                {
                    robot.Stop();
                }
            }
            IPlayer player = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: player is null");
                return;
            }

            IEntitySystem es = ControllerSystem.m_ClientGlobal.GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: EntitySystem is null");
                return;
            }

            IEntity npc = es.FindEntity(cmd.lTarget);
            if (npc == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: Not found entity {0}", cmd.lTarget);

                return;
            }

            uint npcbaseid = (uint)npc.GetProp((int)Client.EntityProp.BaseID);
            table.NpcDataBase npcTable = GameTableManager.Instance.GetTableItem<table.NpcDataBase>(npcbaseid); ;

            Vector3 pos = npc.GetPos();
            if (npcTable != null)
            {
                Vector3 mainPos = player.GetPos();
                pos.y = mainPos.y;

                bool visit = VisitNpc(ref es, ref npcTable, ref mainPos, ref pos);
                if (visit)
                {
                    RemoveFirst();
                    return;
                }
            }

            RemoveFirst();

            CmdInfo newCmd = new CmdInfo();
            newCmd.cmd = Cmd.VisiteNpc;
            newCmd.lExecutor = player.GetUID();
            newCmd.lTarget = npc.GetUID();
            newCmd.param2 = npcbaseid;
            InsterCmd(newCmd);

            CmdInfo newCmd2 = new CmdInfo();
            newCmd2.cmd = Cmd.CloseTo;

            newCmd2.fAttackDis = 0; // 攻击距离

            newCmd2.lExecutor = player.GetUID();
            newCmd2.lTarget = npc.GetUID();
            newCmd2.param2 = npcbaseid;
            newCmd2.param3 = npc.GetRadius();

            InsterCmd(newCmd2);
            NextCmd();
        }

        //-------------------------------------------------------------------------------------------------------
        private void DoVisiteNpc(CmdInfo cmd)
        {
            IPlayer player = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: player is null");
                return;
            }

            IEntitySystem es = ControllerSystem.m_ClientGlobal.GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: EntitySystem is null");
                return;
            }

            if (cmd.lTarget == 0)
            {
                return;
            }
            INPC npc = null;
            if (npc == null)
            {
                npc = es.FindNPC((uint)cmd.lTarget);
            }

            if (npc == null && cmd.param2 != null)
            {
                uint npcID = (uint)cmd.param2;
                npc = es.FindNPCByBaseId((int)npcID, true);
            }

            if (npc == null)
            {
                return;
            }
            //需要判断访问距离
            table.NpcDataBase npcTable = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)npc.GetProp((int)EntityProp.BaseID));
            if (npcTable != null)
            {
                if (npcTable.IsMonster())
                {
                    RemoveFirst();
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_ATTACKMONSTER, (int)npcTable.dwID);
                    return;
                }
                Vector3 mainPos = player.GetPos();
                Vector3 pos = npc.GetPos();
                bool visit = VisitNpc(ref es, ref npcTable, ref mainPos, ref pos, npc.GetID());
                if (visit)
                {
                    m_curCmd = null;
                    RemoveFirst();
                }
            }
        }

        private void DoVisiteNpcDirecte(CmdInfo cmd)
        {

            IMapSystem mapSys = ControllerSystem.m_ClientGlobal.GetMapSystem();
            if (mapSys == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: mapSys is null");
                return;
            }
            RemoveFirst();

            CmdInfo newCmd = new CmdInfo();
            newCmd.cmd = Cmd.GotoNPC;
            newCmd.lExecutor = cmd.lExecutor;
            newCmd.lTarget = cmd.lTarget;
            newCmd.param1 = cmd.param1;// npcid
            InsterCmd(newCmd);

            uint uCurMapID = mapSys.GetMapID();
            if (uCurMapID != (uint)cmd.lTarget)
            {
                GameCmd.sTaskHelpGoToScriptUserCmd_CS sendcmd = new GameCmd.sTaskHelpGoToScriptUserCmd_CS();
                UnityEngine.Vector2 pos = (UnityEngine.Vector2)cmd.param3;
                //sendcmd.x = (uint)pos.x;
                //sendcmd.y = (uint)pos.y;
                sendcmd.taskid = (uint)cmd.param2;
                //sendcmd.npcid = (uint)cmd.param1;
                //sendcmd.mapid = (uint)cmd.lTarget;
                //切换地图前 主角停止移动
                Client.IEntity player = ControllerSystem.m_ClientGlobal.MainPlayer;
                if (player != null)
                {
                    bool ismoving = (bool)player.SendMessage(Client.EntityMessage.EntityCommand_IsMove, null);
                    if (ismoving)
                    {
                        player.SendMessage(Client.EntityMessage.EntityCommand_StopMove, player.GetPos());
                    }
                }
                ControllerSystem.m_ClientGlobal.netService.Send(sendcmd);
            }
            else
            {
                NextCmd();
            }
        }

        #endregion




        public static bool CmdManager_LoadPackageConfigFile(string strConfigFile, ref bool bSmallPackage)
        {
            byte[] configBytes;
            string strIniFile = "";
            if (Application.isEditor)
            {
                strIniFile = Engine.Utility.FileUtils.Instance().FullPathFileName(ref strConfigFile, FileUtils.UnityPathType.UnityPath_CustomPath);
            }
            else
            {
                //该配置不做更新
                strIniFile = Engine.Utility.FileUtils.Instance().FullPathFileName(ref strConfigFile, FileUtils.UnityPathType.UnityPath_StreamAsset);
            }
            if (!FileUtils.Instance().ReadFile(strIniFile, out configBytes))
            {
                return false;
            }

            byte[] desBytes;
            if (!Engine.Utility.Security.EncryptUtil.Decode(ref configBytes, out desBytes))
            {
                configBytes = null;
                return false;
            }
            configBytes = null;
            string configJsonStr = System.Text.UTF8Encoding.UTF8.GetString(desBytes);
            desBytes = null;
            Engine.JsonNode jsonRoot = Engine.RareJson.ParseJson(configJsonStr);
            if (jsonRoot == null)
            {
                Engine.Utility.Log.Error("GlobalConfig LoadPackageConfigFile 解析{0}文件失败!", configJsonStr);
                return false;
            }


            try
            {
                bSmallPackage = ((int)jsonRoot["smallPackage"] == 1) ? true : false;
            }
            catch
            {
                bSmallPackage = false;
            }

            return true;
        }

        static bool s_IsReadPackageConfigFile = false;
        static bool s_bSmallPackage = false;
        public bool SceneFileExists(uint mapid)
        {
            if (s_IsReadPackageConfigFile == false)
            {
                CmdManager_LoadPackageConfigFile("PackageConfig.json", ref s_bSmallPackage);

                if (s_bSmallPackage == true)// 小包安装过的也算整包.
                {
                    string strFullPackage = "IsFullPackage.txt";
                    string strUnZipFileSuccess = Engine.Utility.FileUtils.Instance().FullPathFileName(ref strFullPackage, Engine.Utility.FileUtils.UnityPathType.UnityPath_CustomPath);
                    if (System.IO.File.Exists(strUnZipFileSuccess))
                    {
                        s_bSmallPackage = false;
                    }
                }

                s_IsReadPackageConfigFile = true;
            }

            if (s_bSmallPackage == false)
                return true;


            table.MapDataBase mapDB = GameTableManager.Instance.GetTableItem<table.MapDataBase>(mapid);
            if (mapDB == null)
            {
                Engine.Utility.Log.Error("MapSystem:找不到地图配置数据{0}", mapid);
                return false;
            }

            table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(mapDB.dwResPath);
            if (resDB == null)
            {
                Engine.Utility.Log.Error("MapSystem:找不到地图资源路径配置{0}", mapDB.dwResPath);
                return false;
            }

            string strMapName = resDB.strPath.ToLower();

            if (Application.platform == RuntimePlatform.Android)
            {
                string strPath = System.IO.Path.Combine(Application.persistentDataPath + "/assets/", strMapName);
                bool exists = System.IO.File.Exists(strPath);
                if (exists == false)
                {
                    strPath = System.IO.Path.Combine(Application.streamingAssetsPath, strMapName);
                    exists = System.IO.File.Exists(strPath);
                }
                return exists;

            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                string strPath = Application.persistentDataPath + "/" + strMapName;
                bool exists = System.IO.File.Exists(strPath);
                if (exists == false)
                {
                    strPath = System.IO.Path.Combine(Application.streamingAssetsPath, strMapName);
                    exists = System.IO.File.Exists(strPath);
                }
                return exists;
            }

            return true;
        }

        /// <summary>
        /// Returns 'true' if visit success.
        /// </summary>
        private bool VisitNpc(ref Client.IEntitySystem es, ref table.NpcDataBase npcTable, ref Vector3 mainPos, ref Vector3 pos, uint npcid = 0)
        {
            float currDistance = 10000;
            IMapSystem ms = ControllerSystem.m_ClientGlobal.GetMapSystem();
            if (ms != null)
            {
                ms.CalcDistance(new Vector2(mainPos.x, -mainPos.z), new Vector2(pos.x, -pos.z), out currDistance, TileType.TileType_None);
            }

            INPC npc = null;
            if (npcid != 0)
            {
                npc = es.FindNPC(npcid);
            }
            else
            {
                npc = es.FindNPCByBaseId((int)npcTable.dwID);
            }

            if (npc != null)
            {
                float visidis = npcTable.dwCallDis * 0.01f + ControllerSystem.m_ClientGlobal.MainPlayer.GetRadius() + npc.GetRadius();
                if (currDistance <= visidis)
                {
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_VISITNPC, npcTable.dwID);
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, false);
                    GameCmd.stClickNpcScriptUserCmd_C msg = new GameCmd.stClickNpcScriptUserCmd_C();
                    msg.dwNpcTempID = npc.GetID();

                    if (npcTable.visitRotation == 1)
                    {
                        m_visiNpc = npc;
                        m_fRotY = (float)npc.SendMessage(EntityMessage.EntityCommand_GetRotateY, null);
                        npc.SendMessage(EntityMessage.EntityCommand_LookTarget, mainPos);
                    }

                    ControllerSystem.m_ClientGlobal.MainPlayer.SendMessage(EntityMessage.EntityCommand_LookTarget, npc.GetPos());

                    if (npcTable.dwType == (uint)GameCmd.enumNpcType.NPC_TYPE_TRANSFER) // npc为传送阵
                    {
                        table.DeliverDatabase tal = GameTableManager.Instance.GetTableItem<table.DeliverDatabase>(npcTable.dwID);
                        if (!SceneFileExists(tal.dwDestMapID))
                        {
                            //打开下载界面
                            //DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
                            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SHOWHTTPDOWNUI);
                            return true;
                        }

                        if (ControllerSystem.m_ClientGlobal.MainPlayer != null)
                        {
                            ControllerSystem.m_ClientGlobal.MainPlayer.SendMessage(EntityMessage.EntityCommand_StopMove, mainPos);
                        }
                    }
                    else if (npcTable.dwType == (uint)GameCmd.enumNpcType.NPC_TYPE_COLLECT_PLANT)
                    {
                        bool bCollect = Engine.Utility.EventEngine.Instance().DispatchVote((int)Client.GameVoteEventID.TASK_VISITNPC_COLLECT, npcTable.dwID);
                        if (!bCollect)
                        {
                            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, false);
                            return true;
                        }

                        Client.stTaskNpcItem stData = new Client.stTaskNpcItem();
                        stData.type = 1;   //type =1 为npc
                        stData.Id = msg.dwNpcTempID;

                        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_ITEM_COLLECT_USE, stData);

                        /*ControllerSystem.m_ClientGlobal.GetControllerSystem().GetControllerHelper().TryUnRide((obj) =>
                        {
                            ControllerSystem.m_ClientGlobal.netService.Send(msg);
                        }, null);*/
                        return true;
                    }

                    ControllerSystem.m_ClientGlobal.netService.Send(msg);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Engine.Utility.Log.LogGroup("ZCX", "无需移动 找不到npc");
                return false;
            }
        }
        public void DoGoToMapPosition(CmdInfo cmd)
        {
            //暂停挂机
            Client.IControllerSystem cs = ControllerSystem.m_ClientGlobal.GetControllerSystem();
            if (cs != null)
            {
                Client.ICombatRobot robot = cs.GetCombatRobot();
                if (robot != null && robot.Status != CombatRobotStatus.STOP)
                {
                    robot.Stop();
                }
            }
            IPlayer player = ControllerSystem.m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: player is null");
                return;
            }

            IMapSystem mapSys = ControllerSystem.m_ClientGlobal.GetMapSystem();
            if (mapSys == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: mapSys is null");
                return;
            }

            Vector3 pos = Vector3.zero;
            uint uCurMapID = mapSys.GetMapID();
            if (uCurMapID == cmd.lTarget) // 当前地图与目标地图一样 则直接位移
            {
                pos = new Vector3((float)cmd.param1, 0, (float)cmd.param2);
                // 移除当前指令
                RemoveFirst();
                IController ctrl = cs.GetActiveCtrl();
                if (ctrl != null)
                {
                    ctrl.MoveToTarget(pos);
                }
            }
            else
            {

                if (mapSys != null)
                {
                    GameCmd.stHelpGoToScriptPropertyUserCmd_CS sendcmd = new GameCmd.stHelpGoToScriptPropertyUserCmd_CS();
                    sendcmd.item_id = (uint)cmd.param3;
                    //切换地图前 主角停止移动
                    if (player != null)
                    {
                        bool ismoving = (bool)player.SendMessage(Client.EntityMessage.EntityCommand_IsMove, null);
                        if (ismoving)
                        {
                            player.SendMessage(Client.EntityMessage.EntityCommand_StopMove, player.GetPos());
                        }
                    }
                    ControllerSystem.m_ClientGlobal.netService.Send(sendcmd);
                }
            }



        }
    }
}
