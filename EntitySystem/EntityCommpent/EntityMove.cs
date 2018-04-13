using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
using Engine.Utility;

namespace EntitySystem
{
    class EntityMove : BaseCommpent//, ITimer
    {
        private Move m_param;
        private Vector3 m_vLastPos = new Vector3();
        private bool m_hasTarget = false;
        //private Vector3 m_vSpeed = new Vector3();
        private Engine.IScene m_curScene = null;
        private bool m_bMoving = false;         // 是否正在移动
        private bool m_bMoveDir = false;        // 是否按方向移动
        private bool m_bExternalStop = true;    // 是否是外部因素停止

        //移动因子  改变各类效果加速(buff 增加速度等)或减速(中毒,被重击慢速移动等)
        private float m_fSpeedFact = 1.0f;
        private float m_fSpeedTerrainFact = 1.0f;  // 地形因子
        private long m_LastTime = 0;

        private uint m_uServerTime = 0; // 服务器时间

        private long m_BeginTime = 0;
        private float m_fDistance = 0;
        
        private Vector3 m_dir = Vector3.zero;       // 移动方向
        private Vector3 m_vSpeed = Vector3.zero;    // 移动速度
        private Vector3 m_target = Vector3.zero;    // 目标点
        private int m_nPathIndex = -1;

        private Vector3 m_lastSyncPos = Vector3.zero;   // 上一个同步的位置
        private Vector3 m_lastWalkPos = Vector3.zero;   // 上一次没有阻挡的点

        private bool m_bIgnoreMoveAction = false;   // 是否忽略Move中的动作
        private string m_strIgnoreMoveAction = "";  

        //const float minSynPosTime = 0.03f; // 坐标不用每帧发送
        //const float minMoveDistance = 0.05f; // 最小有效移动距离

        public EntityMove(Entity owner)
        {
            m_Owner = owner;
        }

        public uint GetServerTime() { return m_uServerTime; }

        public void Create()
        {
            RegisterMessageDelegate(EntityMessage.EntityCommand_Move, StartMove);
            RegisterMessageDelegate(EntityMessage.EntityCommand_MoveTo, StartMoveTo);
            RegisterMessageDelegate(EntityMessage.EntityCommand_MovePath, StartMovePath);
            RegisterMessageDelegate(EntityMessage.EntityCommand_MoveDir, StartMoveDir);
            RegisterMessageDelegate(EntityMessage.EntityCommand_StopMove, StopMove);
            RegisterMessageDelegate(EntityMessage.EntityCommand_ForceStopMove, ForceStopMove);
            RegisterMessageDelegate(EntityMessage.EntityCommand_ServerTime, GetServerTime);
            RegisterMessageDelegate(EntityMessage.EntityCommand_IsMove, IsMove);
            RegisterMessageDelegate(EntityMessage.EntityCommand_ChangeMoveSpeedFact, ChangeMoveSpeedFact);
            RegisterMessageDelegate(EntityMessage.EntityCommand_GetMoveSpeedFact, GetMoveSpeedFact);
            RegisterMessageDelegate(EntityMessage.EntityCommond_ChangeMoveActionName, ChangeMoveActionName);
            RegisterMessageDelegate(EntityMessage.EntityCommond_IgnoreMoveAction, IgnoreMoveAction);
        }

        public void Close()
        {
            //Engine.Utility.TimerAxis.Instance().KillTimer(0, this);
            Clear();
        }

        private object ChangeMoveSpeedFact(object param = null)
        {
            m_fSpeedFact = (float)param;
            return null;
        }

        private object GetMoveSpeedFact(object param = null)
        {
            return (object)m_fSpeedFact;
        }

        private object ChangeMoveActionName(object param =null)
        {
            m_strIgnoreMoveAction = (string)param;
            m_Owner.PlayAction( m_strIgnoreMoveAction , 0 , m_param.m_speed );
            return null;
        }

        private object IgnoreMoveAction(object param = null)
        {
            m_bIgnoreMoveAction = (bool)param;
            return null;
        }

        private object IsMove(object param = null)
        {
            return m_bMoving;
        }

        private object StartMoveTo(object param = null)
        {
            m_param = (Move)param;
            
            // 使用Timer
            //Engine.Utility.TimerAxis.Instance().KillTimer(0, this);
            m_hasTarget = true;
            //m_bMoving = false;
            //m_param.path.Add(m_target);
            Move();
            return null;
        }

        private object StartMove(object param)
        {
            m_param = (Move)param;
            //Engine.Utility.TimerAxis.Instance().KillTimer(0, this);
            //m_bMoving = false;
            m_hasTarget = false;
            m_bMoveDir = false;
            //m_param.path.Add(m_target);
            Move();
            return null;
        }

        private object StartMovePath(object param = null)
        {
            m_param = (Move)param;
            //Engine.Utility.TimerAxis.Instance().KillTimer(0, this);
            //m_bMoving = false;
            m_hasTarget = true;
            m_bMoveDir = false;
            Move(true);
            return null;
        }
        private object StartMoveDir(object param = null)
        {
            m_param = (Move)param;
            //Engine.Utility.TimerAxis.Instance().KillTimer(0, this);
            m_hasTarget = false;
            m_bMoveDir = true;
            MoveDir();
            return null;
        }

        //-------------------------------------------------------------------------------------------------------
        private void MoveDir()
        {
            if (m_Owner == null)
            {
                return;
            }

            Vector3 pos = m_Owner.GetPos();
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }
            m_curScene = rs.GetActiveScene();
            if (m_curScene == null)
            {
                return;
            }

            // 先设置人物旋转
            Quaternion rotate = new Quaternion();
            rotate.eulerAngles = new Vector3(0, m_param.m_dir, 0);

            Matrix4x4 mat = new Matrix4x4();
            mat.SetTRS(Vector3.zero, rotate, Vector3.one);
            m_dir = mat.GetColumn(2);

            if (!m_bIgnoreMoveAction)
            {
                m_Owner.PlayAction(m_param.strRunAct, 0, m_param.m_speed);
            }

            stEntityBeginMove move = new stEntityBeginMove();
            move.uid = m_Owner.GetUID();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, move);

            if (!m_bMoving)
            {
                Engine.Utility.Log.Error("MoveDir----------- {0}-------------------------------------", m_param.m_dir);

                m_vLastPos = pos;
                m_lastSyncPos = m_vLastPos;

                m_bMoving = true;
                if ( !m_bIgnoreMoveAction )
                {
                    m_Owner.PlayAction( m_param.strRunAct, 0, m_param.m_speed );
                }

                //Engine.Utility.TimerAxis.Instance().KillTimer(0, this);
                //Engine.Utility.TimerAxis.Instance().SetTimer(0, 33, this, TimerAxis.INFINITY_CALL, "EntityMove");
                m_LastTime = Engine.Utility.TimeHelper.GetTickCount(); 
            }
        }

        private void Move(bool bPath = false)
        {
            if (m_Owner == null)
            {
                return;
            }

            Vector3 pos = m_Owner.GetPos();
            //如果目标是当前位置
            if (pos.x == m_param.m_target.x && pos.z == m_param.m_target.z)
            {
                StopMove(m_param.m_target);
                return;
            }

            // 获取服务器时间
            if (EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner))
            {
                m_uServerTime = EntityConfig.serverTime;
            }

            pos.y = 0.0f;
            if (m_hasTarget)
            {
                if (!bPath)
                {
                    if (m_param.path == null)
                    {
                        m_param.path = new List<Vector3>();
                    }
                    else
                    {
                        m_param.path.Clear();
                    }

                    m_param.path.Add(pos);
                    m_param.path.Add(m_param.m_target);
                }
            }
            else
            {
                if (!bPath)
                {
                    if (m_param.path == null)
                    {
                        m_param.path = new List<Vector3>();
                    }
                    else
                    {
                        m_param.path.Clear();
                    }

                    m_param.path.Add(pos); // 起始点
                    Vector3 target = pos + m_param.m_target;
                    m_param.path.Add(target);
                }
            }

            if(m_param.path == null)
            {
                return;
            }

            if(m_param.path.Count <=0)
            {
                return;
            }

            // 开始移动事件 先发事件再处理
            stEntityBeginMove move = new stEntityBeginMove();
            move.uid = m_Owner.GetUID();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, move);

            //开始移动时也同步下位置
            //SyncPos(true);

            // 路径点切换
            m_nPathIndex = 0;
            m_target = pos;
            if (pos.Equals(m_param.path[m_nPathIndex])) // 去除第一个点
            {
                m_nPathIndex++;
                if (m_nPathIndex>=m_param.path.Count)
                {
                    return;
                }
            }

            m_lastWalkPos = pos;
            m_fDistance = 0.0f;

            // 切换目标点
            SwitchTarget(m_param.path[m_nPathIndex], ref pos);
            // 贴地处理
            CloseTerrainPos(ref pos);

            m_vLastPos = pos;
            m_lastSyncPos = m_vLastPos;

            m_bMoving = true;
            if (!m_bIgnoreMoveAction)
            {
                m_Owner.PlayAction(m_param.strRunAct,0,m_fSpeedFact);
            }

            if (m_LastTime == 0)
            {
                m_LastTime = Engine.Utility.TimeHelper.GetTickCount();
            }

            m_BeginTime = m_LastTime;
            //Engine.Utility.Log.Error("Move begine: ({0},{1})->({3},{4}) {2} ", pos.x, pos.z, m_BeginTime, m_param.path[m_param.path.Count - 1].x, m_param.path[m_param.path.Count - 1].z);

            // 进入move状态
            ICreature owner = m_Owner as ICreature;
            if (owner != null)
            {
                owner.ChangeState(CreatureState.Move);
            }
        }

        //public virtual void OnTimer(uint uTimerID)
        //{
        //    MoveUpdate();
        //}
        public override void Update()
        {
            if (m_Owner == null)
            {
                return;
            }

            if (m_curScene == null)
            {
                return;
            }

            if (!m_bMoving) return;

            // 实时取对象身上的速度值
            m_param.m_speed = m_Owner.GetProp((int)WorldObjProp.MoveSpeed) * EntityConst.MOVE_SPEED_RATE;
            m_vSpeed = (m_param.m_speed * m_fSpeedFact * m_fSpeedTerrainFact) * m_dir;

            long nCurTime = Engine.Utility.TimeHelper.GetTickCount();
            Vector3 ds = m_vSpeed * ((nCurTime - m_LastTime) * 0.001f);
            Vector3 pos = m_vLastPos + ds;
            m_LastTime = nCurTime;

            // 获取服务器时间
            if (EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner))
            {
                m_uServerTime = EntityConfig.serverTime;
            }

            if (pos.Equals(m_vLastPos))
            {
                return;
            }
                
            Client.IMapSystem mapSys = EntitySystem.m_ClientGlobal.GetMapSystem();
            if (mapSys == null)
            {
                return;
            }

            m_fDistance += ds.magnitude;

            // 检测是否到达目标点
            if (ChkMove(pos) && !m_bMoveDir) // pos 已经超过目标点
            {
                m_nPathIndex++;
                if (m_nPathIndex < m_param.path.Count) // 路径上的点还没有走完
                {
                    // 处理路径上点切换
                    SwitchTarget(m_param.path[m_nPathIndex], ref pos);
 
                    // 贴地处理
                    CloseTerrainPos(ref pos);

                    if (mapSys.CanWalk(pos))
                    {
                        m_lastWalkPos = pos;
                    }

                    // 设置当前位置
                    m_Owner.SetPos(ref pos);
                    // 同步玩家位置 发送实体移动事件
                    SyncPos();
                    m_vLastPos = pos;
                }
                else
                {
                    Vector3 pp = m_target;

                    // 贴地处理
                    CloseTerrainPos(ref pp);

                    if (mapSys.CanWalk(pp))
                    {
                        m_lastWalkPos = pp;
                    }

                    //pp.y = pos.y;
                    m_bMoving = false; // 停止移动

                    m_bExternalStop = false; // 非外部因素
                    StopMove(pp);
                    m_bExternalStop = true;
                }
            }
            else
            {
                // 设置新位置
                //if (mapSys.CanWalk(pos))
                {
                    // 贴地处理
                    CloseTerrainPos(ref pos);

                    if (mapSys.CanWalk(pos))
                    {
                        m_lastWalkPos = pos;
                    }

                    //Engine.Utility.Log.Error("SetPos:{0},{1}", pos.x, pos.z);
                    m_Owner.SetPos(ref pos);
                    SyncPos();
                    m_vLastPos = pos;
                }
                //else
                //{
                //    // 遇到障碍就停止
                //    m_Owner.SetPos(ref m_vLastPos);
                //    StopMove(m_vLastPos);
                //}
            }

            // 设置场景草扰动
            if (m_Owner.GetEntityType() == EntityType.EntityType_Player)
            {
                mapSys.AddGrassWaveForce(pos, 0.8f, 1.2f);
            }
        }
        //-------------------------------------------------------------------------------------------------------
        private void CloseTerrainPos(ref Vector3 curPos)
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }

            // 再计算移动速度
            m_curScene = rs.GetActiveScene();
            if (m_curScene == null)
            {
                return;
            }

            //和周围墙做碰撞修正
            // 计算得到新位置，还需要做一次碰撞检测，看是否可以行走
            //Ray rayLine = new Ray(m_vLastPos, pos - m_vLastPos);
            //Engine.ColliderInfo colliderInfo = new Engine.ColliderInfo();
            //float fDis = Vector3.Distance(pos, m_vLastPos);
            //if (m_curScene.GetColliderInfo(ref rayLine, ref colliderInfo))
            //{
            //    //Engine.Utility.Log.Trace("Dis:{0}", colliderInfo.distance);
            //    if (colliderInfo.distance < 1.5f) // 应当是玩家的半径
            //    {
            //        //                     if (colliderInfo.distance < fDis)
            //        //                     {
            //        //                         pos = m_vLastPos + Vector3.Normalize(m_vSpeed) * colliderInfo.distance;
            //        //                     }
            //        //                     else
            //        //                     {
            //        //            m_jumpSpeed =  Vector3.up * _jumpUp;
            //        //             m_vSpeed = m_jumpSpeed ;
            //        m_vSpeed = m_vSpeed - Vector3.Dot(m_vSpeed, colliderInfo.normal) * colliderInfo.normal;
            //        pos = m_vLastPos + m_vSpeed * Time.deltaTime;
            //        /*                    }*/
            //    }
            //}

            Engine.TerrainInfo info;
            if (m_curScene.GetTerrainInfo(ref curPos, out info))
            {
                //斜坡速度衰减.... 去掉
                //Vector3 right = Vector3.Cross(info.normal, m_dir);
                //Vector3 speed = Vector3.Cross(right, info.normal);
                //float cos = Vector3.Dot(m_dir, speed);

                //m_fSpeedTerrainFact = cos;
            }

            curPos.y = info.pos.y;
        }

        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 切换目标
        @param target 要切换的目标
        @param curPos 当前位置
        */
        private void SwitchTarget(Vector3 target, ref Vector3 curPos)
        {
            if (m_Owner == null)
            {
                return;
            }

            m_target.y = 0.0f; // 贴地行走，忽略绕x轴的旋转
            target.y = 0.0f;
            curPos.y = 0.0f;

            float fDis = Vector3.Distance(curPos, m_target);
            m_dir = target - m_target;
            m_dir.Normalize();

            curPos = m_target + m_dir * fDis;
            m_target = target;

            // 先设置人物旋转
            if (m_dir != Vector3.zero)
            {
                Quaternion rotate = new Quaternion();
                rotate.SetLookRotation(m_dir, Vector3.up);
                m_Owner.SetRotate(rotate.eulerAngles);
            }
        }

        // 检测是否靠近当前目标点
        private bool ChkMove(Vector3 pos)
        {
            if(!m_bMoving)
            {
                return true;
            }

            Vector3 pp = pos;
            float y = pp.y;
            pp.y = 0;
            Vector3 dtPos = pp - m_target;
            dtPos.Normalize();
            if (Vector3.Dot(dtPos,m_dir)>0) // 移动结束
            {
                return true;
            }

            return false;
        }

        
        private object StopMove(object param)
        {
            if(m_Owner==null)
            {
                return null;
            }

            Client.IMapSystem mapSys = EntitySystem.m_ClientGlobal.GetMapSystem();
            if (mapSys == null)
            {
                return null;
            }
         
            if( param != null )
            {
                Vector3 pos = (Vector3)param;

                if (m_bMoving)
                {
                    m_param.m_speed = m_Owner.GetProp((int)WorldObjProp.MoveSpeed) * EntityConst.MOVE_SPEED_RATE;
                    m_vSpeed = (m_param.m_speed * m_fSpeedFact * m_fSpeedTerrainFact) * m_dir;

                    long nCurTime = Engine.Utility.TimeHelper.GetTickCount();
                    Vector3 ds = m_vSpeed * ((nCurTime - m_LastTime) * 0.001f);
                    pos = m_vLastPos + ds;
                    m_fDistance += ds.magnitude;
                    m_LastTime = nCurTime;

                    if (EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner))
                    {
                        m_uServerTime = EntityConfig.serverTime;
                    }
                }
                
                // 停止时如果是阻挡点客户端做一下修正 目前的同步方案可能会和服务器之间不同步 测试再看效果
                if (!mapSys.CanWalk(pos))
                {
                    pos = m_lastWalkPos;
                }

                m_Owner.SendMessage(EntityMessage.EntityCommand_SetPos, pos);

                m_LastTime = Engine.Utility.TimeHelper.GetTickCount();
                //Engine.Utility.Log.Error("Move stop: ({0},{1}) {2} {3} speed:{4}", pos.x, pos.z, m_LastTime, m_fDistance, m_fDistance / (m_LastTime - m_BeginTime));

                // 切换到Normal状态
                Creature owner = m_Owner as Creature;
                if (owner != null)
                {
                    if (!m_bIgnoreMoveAction && !owner.IsDead())
                    {
                        if(m_param != null)
                        {
                            owner.ChangeState(CreatureState.Normal, (object)(m_param.m_ignoreStand?1:0));
                        }
                        else
                        {
                            owner.ChangeState(CreatureState.Normal);
                        }
                    }
                }
                else
                {
                    m_Owner.PlayAction(EntityAction.Stand,0,m_fSpeedFact);
                }
            }

            m_LastTime = 0;

            // 强制同步
            SyncPos(true);  // 停止同步
        
            Client.stEntityStopMove stopEntity = new Client.stEntityStopMove();
            stopEntity.uid = m_Owner.GetUID();
            stopEntity.bExternal = m_bExternalStop;

            m_vSpeed =Vector3.zero;
            m_nPathIndex = -1;
            if (m_param != null)
            {
                m_param.m_speed = 0;
            }
            m_bMoving = false;
            m_hasTarget = false;
            //Engine.Utility.TimerAxis.Instance().KillTimer(0, this);
            if (m_Owner.GetEntityCallback() != null && m_param != null)
            {
                m_Owner.GetEntityCallback().OnMoveEnd(m_Owner, m_param.param);
            }
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, stopEntity);

            return null;
        }
        private object ForceStopMove(object param)
        {
            if (m_Owner == null)
            {
                return null;
            }

            if (param != null)
            {
                Vector3 pos = (Vector3)param;

                m_Owner.SendMessage(EntityMessage.EntityCommand_SetPos, pos);

                // 切换到Normal状态
                Creature owner = m_Owner as Creature;
                if (owner != null)
                {
                    if (!m_bIgnoreMoveAction && !owner.IsDead())
                    {
                        if (m_param != null)
                        {
                            owner.ChangeState(CreatureState.Normal, (object)(m_param.m_ignoreStand?1:0));
                        }
                        else
                        {
                            owner.ChangeState(CreatureState.Normal);
                        }
                    }
                }
                else
                {
                    m_Owner.PlayAction(EntityAction.Stand, 0, m_fSpeedFact);
                }
            }

            if (m_Owner.GetEntityCallback() != null)
            {
                m_Owner.GetEntityCallback().OnMoveEnd(m_Owner, m_param.param);
            }

            m_LastTime = 0;

            // 强制同步
            SyncPos();  // 不需要同步主角位置

            Client.stEntityStopMove stopEntity = new Client.stEntityStopMove();
            stopEntity.uid = m_Owner.GetUID();

            m_vSpeed = Vector3.zero;
            m_nPathIndex = -1;
            if (m_param != null)
            {
                m_param.m_speed = 0;
            }
            m_bMoving = false;
            m_hasTarget = false;

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, stopEntity);

            return null;
        }

        // 获取服务器时间
        private object GetServerTime(object param)
        {
            return m_uServerTime;
        }

        //-------------------------------------------------------------------------------------------------------
        // 发送实体移动事件
        private void SyncPos(bool bForce = false)
        {
            if(m_Owner ==null)
            {
                return;
            }
            
            // 同步位置给服务器
            if (bForce)
            {
                //GameCmd.stUserMoveStopMoveUserCmd_CS cmd = new GameCmd.stUserMoveStopMoveUserCmd_CS();
                //if (EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner)) // 只有主角才同步
                //{
                //    Vector3 pos = m_Owner.GetPos();
                //    cmd.pos = new GameCmd.SmallPos();
                //    cmd.charid = m_Owner.GetID();
                //    cmd.pos.x = (uint)(pos.x * 100.0f);
                //    cmd.pos.y = (uint)(-pos.z * 100.0f);

                //    EntitySystem.m_ClientGlobal.netService.Send(cmd); // 同步网络消息
                //}

                if (EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner)) // 只有主角才同步
                {
                    Vector3 pos = m_Owner.GetPos();
                    GameCmd.stUserStopMoveUserCmd_C cmd = new GameCmd.stUserStopMoveUserCmd_C()
                    {
                        client_time = EntityConfig.serverTime,
                        stop_pos_x = (uint)(pos.x * 100.0f),
                        stop_pos_y = (uint)(-pos.z * 100.0f),
                    };
                    Engine.Utility.Log.LogGroup("XXF", "StopMove {0}", cmd.client_time);
                    EntitySystem.m_ClientGlobal.netService.Send(cmd); // 同步网络消息
                }
            }

            Client.stEntityMove moveEntity = new Client.stEntityMove();
            moveEntity.uid = m_Owner.GetUID();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_ENTITYMOVE, moveEntity);
        }
    }
}
