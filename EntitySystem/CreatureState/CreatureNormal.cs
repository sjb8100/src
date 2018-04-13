using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;

namespace EntitySystem
{
    // 正常状态
    class CreatureNormal : CreatureStateBase
    {
        //private EntityNormalParam m_param;
        //private Engine.IScene m_curScene = null;
        //private bool m_bCoseTerrain = false;
        private Creature m_Owner = null;

        private float m_fStartTime = 0;
        private int m_nAction = 0;   // 0 站立 1  idle

        public CreatureNormal(Engine.Utility.StateMachine<Creature> machine)
            : base(machine)
        {
            m_nStateID = (int)CreatureState.Normal;
        }

        // 进入状态
        public override void Enter(object param)
        {

            //m_param = (EntityNormalParam)param;
            m_Owner = m_Statemachine.GetOwner();
            //if (EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner))
            //{
            //    Engine.Utility.Log.LogGroup("ZDY", "enter normal");
            //}
            //if (EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner))
            //{
            //    Engine.Utility.Log.Info("Enter................. normal");
            //}
            if (m_Owner != null)
            {
                if (param != null)
                {
                    int nFlag = (int)param;
                    switch(nFlag)
                    {
                        case 0: // 不忽略站立
                            {
                                m_Owner.PlayAction(Client.EntityAction.Stand, 0, (float)m_Owner.SendMessage(EntityMessage.EntityCommand_GetMoveSpeedFact));
                                break;
                            }
                        case 1: // 忽略站立
                            {
                                break;
                            }
                        case 2: // OnHitCallback 受击时切换到战备动作
                            {
                                if(m_Owner.GetEntityType()==EntityType.EntityType_Player)
                                {
                                    m_Owner.PlayAction(Client.EntityAction.Stand_Combat, 0, (float)m_Owner.SendMessage(EntityMessage.EntityCommand_GetMoveSpeedFact));
                                }
                                else
                                {
                                    m_Owner.PlayAction(Client.EntityAction.Stand, 0, (float)m_Owner.SendMessage(EntityMessage.EntityCommand_GetMoveSpeedFact));
                                }
                                break;
                            }
                    }
                    //if (!ignoreStand)
                    //{
                    //    if (m_Owner.GetEntityType() == EntityType.EntityType_Player)
                    //    {
                    //        m_Owner.PlayAction(EntityAction.Stand, 0, (float)m_Owner.SendMessage(EntityMessage.EntityCommand_GetMoveSpeedFact));
                    //    }
                    //    else
                    //    {
                    //        m_Owner.PlayAction(Client.EntityAction.Stand, 0, (float)m_Owner.SendMessage(EntityMessage.EntityCommand_GetMoveSpeedFact));
                    //    }
                    //}
                }
                else
                {
                    if (m_Owner.GetEntityType() == EntityType.EntityType_Player)
                    {
                        m_Owner.PlayAction(EntityAction.Stand, 0, (float)m_Owner.SendMessage(EntityMessage.EntityCommand_GetMoveSpeedFact));
                    }
                    else
                    {
                        m_Owner.PlayAction(Client.EntityAction.Stand, 0, (float)m_Owner.SendMessage(EntityMessage.EntityCommand_GetMoveSpeedFact));
                    }
                }

                // 播放休闲动作
                if (m_Owner.GetEntityType() == EntityType.EntityType_NPC)
                {
                    INPC npc = m_Owner as INPC;
                    if (npc != null && !npc.IsMonster())
                    {
                        m_fStartTime = Time.realtimeSinceStartup;
                        m_nAction = 0;
                    }
                }
            }
        }

        // 退出状态
        public override void Leave()
        {
        }

        public override void Update(float dt)
        {
            if (m_Owner == null)
            {
                return;
            }

            if (m_Owner.GetEntityType() == EntityType.EntityType_NPC)
            {
                INPC npc = m_Owner as INPC;
                if (npc != null && !npc.IsMonster())
                {
                    float fCurTime = Time.realtimeSinceStartup;
                    switch (m_nAction)
                    {
                        case 0:
                            {
                                if (fCurTime - m_fStartTime > Engine.Utility.MathLib.Instance().RandomRange(8.0f, 25.0f))
                                {
                                    PlayAni playAni = new PlayAni();
                                    playAni.strAcionName = Client.EntityAction.Idle;
                                    playAni.fSpeed = 1;
                                    playAni.nStartFrame = 0;
                                    playAni.nLoop = 1;
                                    playAni.fBlendTime = 0.1f;
                                    playAni.aniCallback = OnAniEndEvent;
                                    m_Owner.SendMessage(EntityMessage.EntityCommand_PlayAni, playAni);

                                    m_fStartTime = Time.realtimeSinceStartup;
                                    m_nAction = 1;
                                }
                                break;
                            }
                        case 1:
                            {
                                //if (fCurTime - m_fStartTime > Engine.Utility.MathLib.Instance().RandomRange(8.0f, 25.0f))
                                //{
                                //    m_Owner.PlayAction(Client.EntityAction.Stand, 0, (float)m_Owner.SendMessage(EntityMessage.EntityCommand_GetMoveSpeedFact));
                                //    m_fStartTime = Time.realtimeSinceStartup;
                                //    m_nAction = 0;
                                //}
                                break;
                            }
                    }
                }
            }
        }

        void OnAniEndEvent(ref string strEventName, ref string strAnimationName, float time, object param)
        {
            if (strEventName == "end" && strAnimationName == Client.EntityAction.Idle)
            {
                m_Owner.SendMessage(EntityMessage.EntityCommand_ClearAniCallback, null);
                m_Owner.PlayAction(Client.EntityAction.Stand, 0, (float)m_Owner.SendMessage(EntityMessage.EntityCommand_GetMoveSpeedFact));
                m_fStartTime = Time.realtimeSinceStartup;
                m_nAction = 0;
            }
        }

        public override void OnEvent(int nEventID, object param)
        {
        }
    }
}
