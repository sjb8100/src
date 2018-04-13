using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using UnityEngine;
using Engine.Utility;

namespace Controller
{
    class MoveController : Singleton<MoveController>, Engine.Utility.ITimer
    {
        private static uint MOVE_TIMER = 0;

        // 客户端全局对象
        private IClientGlobal m_ClientGlobal = null;

        // 是否处于Joystick状态
        private bool m_bJoystick = false;

        // 设置控制对象
        private IEntity m_Host = null;

        // 目标角度
        private float m_fTargetAngle = 0.0f;
        // 角度开始旋转时间
        private float m_fRotateStarTime = 0.0f;
        // 旋转时间倒数
        private float m_fRotateTimeR = 0.0f;
        // 计算过程中角度
        private float m_fCalcRotate = 0.0f;
        // 起始角
        private float m_fStartAngle = 0.0f;
        // 上一次设置的角度
        private float m_fLastAngle = 0.0f;
        // 旋转状态
        private bool m_bRotating = false;

        // 角度区域索引
        //private int m_nLastIndex = -1;  //
        //private int m_nTargetIndex = -1;  //
        private float m_fAngleRuleRange = 30.0f; // 

        // 前身预测最大距离
        private const int FORWARD_MAXDIS = 10;
        // 角度缓冲范围
        private const float ANGLE_RANGLE = 60.0f;
        // 角速度
        private const float ANGLE_SPEED = 1200.0f;
        //是否禁用摇杆操作
        private bool bForbiddenJoystick = false;
        //是否在连击状态
        bool m_bSkillLongPress = false;
        /**
        @brief 初始化
        @param ClientGlobal对象
        */
        public void Init(IClientGlobal ClientGlobal)
        {
            m_ClientGlobal = ClientGlobal;

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnContrllerEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYDEAD, OnContrllerEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int) (int)Client.GameEventID.SKILL_FORBIDDENJOYSTICK , OnContrllerEvent );
            Engine.Utility.EventEngine.Instance().AddEventListener((int)(int)Client.GameEventID.SKLL_LONGPRESS, OnContrllerEvent);
        }

        //-------------------------------------------------------------------------------------------------------
        public void Destroy()
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnContrllerEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYDEAD, OnContrllerEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int) (int)Client.GameEventID.SKILL_FORBIDDENJOYSTICK , OnContrllerEvent );
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)(int)Client.GameEventID.SKLL_LONGPRESS, OnContrllerEvent);
            Engine.Utility.TimerAxis.Instance().KillTimer(MoveController.MOVE_TIMER, this);
        }

        //-------------------------------------------------------------------------------------------------------
        public void SetHost(IEntity entity)
        {
            m_Host = entity;
        }

        public void OnContrllerEvent(int nEventID, object param)
        {
            if(m_Host==null)
            {
                Engine.Utility.Log.Error("MoveController.OnContrllerEvent: host is null");
                return;
            }
            if (nEventID == (int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE)
            {
                Client.stEntityStopMove stopEntity = (Client.stEntityStopMove)param;
                if (m_bJoystick && stopEntity.uid == m_Host.GetUID() && !stopEntity.bExternal) // 自行移动停止才需要继续向前预测
                {
                    // 继续向前预测
                    MoveByDir(m_fCalcRotate);
                }
            }
            else if(nEventID == (int)Client.GameEventID.SKLL_LONGPRESS)
            {
                stSkillLongPress st = (stSkillLongPress)param;
                m_bSkillLongPress = st.bLongPress;
            }
            else if (nEventID == (int)Client.GameEventID.ENTITYSYSTEM_ENTITYDEAD)
            {
                stEntityDead ed = (stEntityDead)param;
                if (m_ClientGlobal.IsMainPlayer(ed.uid))
                {
                    m_bJoystick = false;
                    m_Host.SendMessage(EntityMessage.EntityCommand_StopMove, m_ClientGlobal.MainPlayer.GetPos());
                }
            }
            else if(nEventID == (int)Client.GameEventID.SKILL_FORBIDDENJOYSTICK)
            {
                Client.stForbiddenJoystick info = (Client.stForbiddenJoystick)param;
                Client.IPlayer player = m_ClientGlobal.MainPlayer;
                if ( player == null )
                {
                    return;
                }
                if ( info.playerID == player.GetUID() )
                {
              
                    //bForbiddenJoystick = info.bFobidden;
                    //if(bForbiddenJoystick)
                    //{
                    //   // Log.LogGroup( "ZDY" , "摇杆禁用" );
                    //}
                    //else
                    //{
                    //  //  Log.LogGroup( "ZDY" , "摇杆回复" );
                    //}
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public void SetTargetAngle(float fAngle)
        {
            if (m_Host == null)
            {
                Engine.Utility.Log.Error("MoveController.OnContrllerEvent: host is null");
                return;
            }

            if (m_Host.GetCurState() == CreatureState.Dead)
            {
                return;
            }

            if (Engine.Utility.MathLib.Instance().IsEqual(fAngle, m_fTargetAngle))
            {
                return;
            }

            m_fTargetAngle = fAngle;

            //m_nTargetIndex = CalcAngleIndex(m_fTargetAngle);
            
            float fCurAngle = GetCurAngle();
            //if(m_nLastIndex == -1)
            //{
            //    m_nLastIndex = CalcAngleIndex(fCurAngle);
            //}

            //if (m_nTargetIndex == m_nLastIndex)
            //{
            //    if (!m_bJoystick)
            //    {
            //        MoveController.Instance.MoveByDir(m_fCalcRotate, true);
            //    }
            //}
            //else
            //{
            //    if (!m_bRotating)
            //    {
            //        Engine.Utility.TimerAxis.Instance().SetTimer(0, 30, this, Engine.Utility.TimerAxis.INFINITY_CALL, "MoveController.SetTargetAngle");
            //        m_fRotateStarTime = Time.realtimeSinceStartup;
            //        m_bRotating = true;

            //        m_fStartAngle = fCurAngle;
            //    }

            //    float fAngleDelta = Mathf.Abs(m_fTargetAngle - m_fStartAngle);
            //    if (fAngleDelta > 180.0f)
            //    {
            //        fAngleDelta = 360.0f - fAngleDelta;
            //    }

            //    m_fRotateTimeR = ANGLE_SPEED / fAngleDelta;
            //    //m_fLastAngle = fCurAngle;
            //}

            if (Engine.Utility.MathLib.Instance().IsEqual(fCurAngle, m_fTargetAngle))
            {
                m_fCalcRotate = m_fTargetAngle;
                if (!m_bJoystick)
                {
                    MoveController.Instance.MoveByDir(m_fCalcRotate, true);
                    m_fLastAngle = m_fCalcRotate;
                }
                //Engine.Utility.Log.Error("Jistick:1111111111111111111111111111111111111111");
            }
            else
            {
                if (!m_bRotating)
                {
                    Engine.Utility.TimerAxis.Instance().SetTimer(MoveController.MOVE_TIMER, 60, this, Engine.Utility.TimerAxis.INFINITY_CALL, "MoveController.SetTargetAngle");
                    m_bRotating = true;
                    m_fRotateStarTime = Time.realtimeSinceStartup;
                    m_fStartAngle = fCurAngle;
                }

                float fAngleDelta = Mathf.Abs(m_fTargetAngle - m_fStartAngle);
                if (fAngleDelta > 180.0f)
                {
                    fAngleDelta = 360.0f - fAngleDelta;
                }

                m_fRotateTimeR = ANGLE_SPEED / fAngleDelta;
                m_fLastAngle = fCurAngle;
            }

            m_bJoystick = true;
        }
        //-------------------------------------------------------------------------------------------------------
        public void OnTimer(uint uTimerID)
        {
            float fCurTime = Time.realtimeSinceStartup;
            float fCurAngle = GetCurAngle();

            m_fCalcRotate = CalcAngleSlerp(m_fStartAngle, m_fTargetAngle, (fCurTime - m_fRotateStarTime) * m_fRotateTimeR);
            //int nIndex = CalcAngleIndex(m_fCalcRotate);

            //if (nIndex != m_nTargetIndex)
            //{
            //    if (nIndex != m_nLastIndex)
            //    {
            //        MoveByDir(m_fCalcRotate, false);
            //        m_nLastIndex = nIndex;
            //    }
            //}
            //else
            //{
            //    //if (Engine.Utility.MathLib.Instance().IsEqual(m_fCalcRotate, m_fTargetAngle))
            //    {
            //        MoveByDir(m_fCalcRotate);
            //        //m_fRotateStarTime = fCurTime;
            //        Engine.Utility.TimerAxis.Instance().KillTimer(0, this);
            //        m_bRotating = false;
            //    }
            //}

            //MoveByDir(m_fCalcRotate, true, false);

            if (Mathf.Abs(m_fCalcRotate - m_fLastAngle) > ANGLE_RANGLE)
            {
                MoveByDir(m_fCalcRotate, true);
                m_fLastAngle = m_fCalcRotate;
            }
            else
            {
                if (Engine.Utility.MathLib.Instance().IsEqual(m_fCalcRotate, m_fTargetAngle))
                {
                    MoveByDir(m_fCalcRotate);
                    m_fRotateStarTime = fCurTime;
                    m_fLastAngle = m_fCalcRotate;
                    Engine.Utility.TimerAxis.Instance().KillTimer(MoveController.MOVE_TIMER, this);
                    m_fTargetAngle = 0.0f;
                    m_bRotating = false;
                }
                else
                {
                    // 客户端更新方向
                    MoveByDir(m_fCalcRotate, true, false);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 计算角度区域索引
        @param 
        */
        private int CalcAngleIndex(float fAngle)
        {
            // 将坐标转换到0~360之间
            if (fAngle < 0.0f)
            {
                fAngle = 360.0f + fAngle;
            }
            else if (fAngle > 360.0f)
            {
                fAngle = fAngle % 360.0f;
            }

            int nMax = Mathf.RoundToInt(360.0f / m_fAngleRuleRange);

            float fIndex = fAngle / m_fAngleRuleRange;
            int nIndex = Mathf.RoundToInt(fIndex);

            if(nIndex>=nMax)
            {
                nIndex = 0;
            }

            return nIndex;
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 计算角度 类球形插值
        @param fCurAngle 当前角度 必须是(0~360之间的值)
        @param fTargetAngle 目标角度 必须是(0~360之间的值)
        @param dt 时间(0~1之间)
        @return 返回dt时的角度 0~360之间的值
        */
        private float CalcAngleSlerp(float fCurAngle, float fTargetAngle, float dt)
        {
            int nDir = 0; // 0正方向 1负方向
            float fAngleDelta = Mathf.Abs(fCurAngle - fTargetAngle);
            if (fTargetAngle > fCurAngle)
            {
                if (fAngleDelta > 180.0f)
                {
                    nDir = 1;
                }
            }
            else
            {
                if (fAngleDelta <= 180.0f)
                {
                    nDir = 1;
                }
            }

            // 走最小的方向
            if(fAngleDelta>180.0f)
            {
                fAngleDelta = 360.0f - fAngleDelta;
            }

            // 计算角度
            float fAngle = 0;
            if (dt>=1.0f)
            {
                fAngle = fTargetAngle; // 保证可以结束
            }
            else
            {
                fAngle = (nDir == 0 ? fCurAngle + fAngleDelta * dt : fCurAngle - fAngleDelta * dt);
                if (fAngle < 0.0f)
                {
                    fAngle = 360.0f + fAngle;
                }
                else if (fAngle > 360.0f)
                {
                    fAngle = fAngle % 360.0f;
                }
            }

            return fAngle;
        }

        //-------------------------------------------------------------------------------------------------------
        // 获取主角当前旋转角
        private float GetCurAngle()
        {
            if (m_Host == null)
            {
                return 0.0f;
            }

            return (float)m_Host.SendMessage(EntityMessage.EntityCommand_GetRotateY, null);
        }

        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 移动到场景指定目标点(不支持跨地图)
        @param vTarget 目标位置
        */
        List<Vector3> movePathList = new List<Vector3>(20);
        public bool MoveToTarget(Vector3 vTarget, object moveParam = null, bool bSysncPath = true, bool bSync = true)
        {
            IMapSystem mapSys = m_ClientGlobal.GetMapSystem();
            if (mapSys == null)
            {
                return false;
            }

            if (m_Host == null)
            {
                return false;
            }

            // 需要添加一个寻路的过程
            List<Vector2> path = null;
            Vector3 curPos = m_Host.GetPos();
            if (!mapSys.FindPath(new Vector2(curPos.x, curPos.z), new Vector2(vTarget.x, vTarget.z), out path))
            {
                //string msg = string.Format("寻路失败: 地图{0} src({1},{2})->desc({3},{4})", mapSys.GetMapID(), curPos.x, curPos.z, vTarget.x, vTarget.z);
                Engine.Utility.Log.Error("寻路失败: 地图{0} src({1},{2})->desc({3},{4})", mapSys.GetMapID(), curPos.x, curPos.z, vTarget.x, vTarget.z);
                if (Application.isEditor)
                {
                    //ControllerSystem.m_ClientGlobal.netService.SendToMe(new Pmd.stMessageBoxChatUserPmd_S() { szInfo = msg });                    
                }

                return false;
            }

            // 移动
            Move move = new Move();
            move.m_speed = m_Host.GetProp((int)WorldObjProp.MoveSpeed) * EntityConst.MOVE_SPEED_RATE; // 速度为测试速度
            move.m_target = vTarget;
            move.strRunAct = Client.EntityAction.Run;
            movePathList.Clear();
            move.path = movePathList;
            move.param = moveParam;

            move.path.Add(new Vector3(curPos.x, 0,curPos.z)); // 把起点也放在路径里面
            for (int i = 0; i < path.Count; ++i)
            {
                move.path.Add(new Vector3(path[i].x, 0, path[i].y));
                Engine.Utility.Log.Trace("Move[{0}: {1},{2}]", i, path[i].x, path[i].y);
            }
            m_Host.SendMessage(EntityMessage.EntityCommand_MovePath, (object)move);


            // 把当前位置也发给服务器
           
            if (ControllerSystem.m_ClientGlobal.IsMainPlayer(m_Host))
            {
                // 同步移动
                //GameCmd.stUserMoveUpPosListMoveUserCmd_C movePath = new GameCmd.stUserMoveUpPosListMoveUserCmd_C();
                //movePath.mapid = mapSys.GetMapID();
                //for (int i = path.Count - 1; i >= 0; --i)
                //{
                //    GameCmd.Pos pp = new GameCmd.Pos();
                //    if (i != path.Count - 1)
                //    {
                //        pp.x = (uint)path[i].x;
                //        pp.y = (uint)-path[i].y;
                //    }
                //    else
                //    {
                //        pp.x = (uint)(path[i].x * 100);
                //        pp.y = (uint)(-path[i].y * 100);
                //    }

                //    //Engine.Utility.Log.Trace("Move[{0}: {1},{2}]", i, pp.x, pp.y);
                //    movePath.poslist.Add(pp);
                //}
                GameCmd.stUserMoveMoveUserCmd_C movePath = new GameCmd.stUserMoveMoveUserCmd_C();
                movePath.client_time = (uint)m_Host.SendMessage(EntityMessage.EntityCommand_ServerTime, null);
                if (bSysncPath)
                {
                    // 起点也发送给服务器
                    path.Insert(0, new Vector2(curPos.x, curPos.z));
                    //GameCmd.stUserMoveMoveUserCmd_C movePath = new GameCmd.stUserMoveMoveUserCmd_C();
                    //movePath.client_time = m_ClientGlobal.GetEntitySystem().serverTime;
                    Engine.Utility.Log.LogGroup("XXF", "Move {0}", movePath.client_time);
                    for (int i = 0; i < path.Count; ++i)
                    {
                        GameCmd.Pos pp = new GameCmd.Pos();
                        //if (i != path.Count - 1)
                        //{
                        //    pp.x = (uint)path[i].x;
                        //    pp.y = (uint)-path[i].y;
                        //}
                        //else
                        {
                            pp.x = (uint)(path[i].x * 100);
                            pp.y = (uint)(-path[i].y * 100);
                        }

                        //Engine.Utility.Log.Trace("Move[{0}: {1},{2}]", i, pp.x, pp.y);
                        movePath.poslist.Add(pp);
                    }
                }
                else // 只发送起点
                {
                    //movePath.client_time = m_ClientGlobal.GetEntitySystem().serverTime;
                    Engine.Utility.Log.LogGroup("XXF", "Move {0}", movePath.client_time);
                    GameCmd.Pos pp = new GameCmd.Pos();
                    {
                        pp.x = (uint)(curPos.x * 100);
                        pp.y = (uint)(-curPos.z * 100);
                    }

                    movePath.poslist.Add(pp);
                }
                // 同步移动消息
                m_ClientGlobal.netService.Send(movePath);
            }
            return true;
        }
        //-------------------------------------------------------------------------------------------------------
        public void StopMove()
        {
            if (m_Host == null)
            {
                return;
            }

            IMapSystem mapSys = m_ClientGlobal.GetMapSystem();
            if (mapSys == null)
            {
                return;
            }

            if (!m_bJoystick)
            {
                return;
            }

            m_bJoystick = false;

            Engine.Utility.TimerAxis.Instance().KillTimer(MoveController.MOVE_TIMER, this);
            m_bRotating = false;
            m_fTargetAngle = 0.0f;
            if(m_Host.GetCurState() == CreatureState.Move)
            {
                m_Host.SendMessage(EntityMessage.EntityCommand_StopMove, (object)m_Host.GetPos());
            }

            //GameCmd.stUserMoveStopMoveUserCmd_CS stopmove = new GameCmd.stUserMoveStopMoveUserCmd_CS();
            //stopmove.mapid = mapSys.GetMapID();
            //stopmove.charid = (uint)m_Host.GetID();
            //Vector3 pos = m_Host.GetPos();
            //GameCmd.SmallPos pp = new GameCmd.SmallPos();
            //pp.x = (uint)(pos.x * 100);
            //pp.y = (uint)(-pos.z * 100);
            //stopmove.pos = pp;

            //if (ControllerSystem.m_ClientGlobal.IsMainPlayer(m_Host))
            //{
            //    // 同步移动消息
            //    m_ClientGlobal.netService.Send(stopmove);
            //}

            ////GameCmd.stUserStopMoveUserCmd_C cmd = new GameCmd.stUserStopMoveUserCmd_C()
            ////{
            ////    client_time = m_ClientGlobal.GetEntitySystem().serverTime
            ////};

            ////Engine.Utility.Log.LogGroup("XXF", "StopMove {0}", cmd.client_time);

            ////if (m_ClientGlobal.IsMainPlayer(m_Host)) // 只有主角才同步
            ////{
            ////    m_ClientGlobal.netService.Send(cmd); // 同步网络消息
            ////}
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 按一定方向移动
        @param 
        */
        public void MoveByDir(float fAngle, bool bSyncPath = true, bool bSync = true)
        {
            if (m_bSkillLongPress)
            {
                //点击摇杆 解除长按
          
                stSkillLongPress longPress = new stSkillLongPress();
                longPress.bLongPress = false;
                EventEngine.Instance().DispatchEvent((int)GameEventID.SKLL_LONGPRESS, longPress);
            }
            //不在禁用摇杆
            //if(bForbiddenJoystick)
            //{
            //    return;
            //}
            IMapSystem mapSys = m_ClientGlobal.GetMapSystem();
            if (mapSys != null)
            {
                if (m_Host == null)
                {
                    return;
                }
                // 发起移动投票，询问其它系统，是否可以移动
                stVoteEntityMove entitymove;
                entitymove.uid = m_Host.GetUID();
                if (!Engine.Utility.EventEngine.Instance().DispatchVote((int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE, entitymove))
                {
                    // 其它系统不允许移动
                    Engine.Utility.Log.LogGroup("ZDY"," *********************DispatchVote((int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE,  其它系统不允许移动");
                    return;
                }
                else
                {
                    Engine.Utility.Log.LogGroup( "XXF", " *********************MoveByDir,  允许移动");
                }
                // 移动 向前预测一定距离，寻路
                Vector3 pos = m_Host.GetPos();
                Vector3 target = new Vector3();
                if (!FindLastTarget(pos, fAngle, out target))
                {
                    //Engine.Utility.Log.Trace("摇杆方向有障碍，停止移动!");
                    m_bJoystick = false;
                    m_Host.SendMessage(EntityMessage.EntityCommand_SetRotateY, fAngle);
                    m_Host.SendMessage(EntityMessage.EntityCommand_StopMove, pos);
                    return;
                }

                // 移动 只同步当前位置
                MoveToTarget(target, null, bSyncPath, bSync);
            }
        }

        //-------------------------------------------------------------------------------------------------------
        // 查找最近的目标点
        private bool FindLastTarget(Vector3 curPos, float fAngle, out Vector3 target)
        {
            target = new Vector3();
            IMapSystem mapSys = m_ClientGlobal.GetMapSystem();
            if (mapSys != null)
            {
                Vector3 tar = new Vector3();
                Quaternion rotate = new Quaternion();
                rotate.eulerAngles = new Vector3(0, fAngle, 0);
                Matrix4x4 mat = new Matrix4x4();
                mat.SetTRS(Vector3.zero, rotate, Vector3.one);

                // 获取矩阵中的轴向量
                //Quaternion rot = new Quaternion();
                Vector4 dir = mat.GetColumn(2);    // Z
                Vector3 dd = new Vector3(dir.x, dir.y, dir.z);

                for (int i = FORWARD_MAXDIS; i >= 1; --i)
                {
                    tar = curPos + dd * i;
                    if (mapSys.CanWalk(tar))
                    {
                        target = tar;
                        return true;
                    }
                }
            }

            return false;
        }
        public bool IsJoyStickState()
        {
            return m_bJoystick;
        }
    }
}
