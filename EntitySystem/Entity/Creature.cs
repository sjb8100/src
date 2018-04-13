using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EntitySystem
{
    class Creature : WorldObj, ICreature
    {
        // 属性数组
        private Engine.Utility.SecurityInt[] m_CreatureProp = new Engine.Utility.SecurityInt[CreatureProp.End - CreatureProp.Begin];
        // 状态机
        private Engine.Utility.StateMachine<Creature> m_FMS = null;

        public uint mapID
        {
            get;
            set;
        }
        bool isFighting = false;
        public bool IsFighting
        {
            set
            {
                isFighting = value;
            }
            get
            {
                return isFighting;
            }
        }
        //bool isHide = false;
        //public bool IsHide
        //{
        //    set
        //    {
        //        isHide = value;
        //    }
        //    get
        //    {
        //        return isHide;
        //    }
        //}
        public override bool Create(EntityCreateData data, ColliderCheckType colliderCheckType)
        {
            bool bRet = base.Create(data, colliderCheckType);

            // 创建状态机
            CreateStateMachine();

            return bRet;
        }
        //-------------------------------------------------------------------------------------------------------
        public override void CreateCommpent()
        {
            base.CreateCommpent();

            AddCommpent(EntityCommpent.EntityCommpent_Move);
        }


        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nPropID">属性ID</param>
        /// <param name="nValue">属性值</param>
        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)CreatureProp.Begin || nPropID >= (int)CreatureProp.End)
            {
                if (nPropID == (int)EntityProp.EntityState)
                {//保证服务器死亡时 客户端必须死亡
                    if ((nValue & (int)GameCmd.SceneEntryState.SceneEntry_Death) == (int)GameCmd.SceneEntryState.SceneEntry_Death)
                    {
                        if (GetCurState() != CreatureState.Dead && GetCurState() != CreatureState.BeginDead)
                        {
                            //Engine.Utility.Log.Error(" 服务器发来死亡标志，进入开始死亡状态 "+GetName()+"_"+GetID());
                            //// 进入死亡状态
                            ChangeState(CreatureState.BeginDead);

                        }
                    }
                   
                }
               
                return;
            }
           
            if (nPropID == (int)CreatureProp.Hp)
            {
                // 进入死亡状态
                if (nValue <= 0)
                {
                    nValue = 0;
                    stEntityDead ed = new stEntityDead();
                    ed.uid = GetUID();

                    if (GetCurState() != CreatureState.Dead)
                    {
                        if (this.GetCurState() == CreatureState.Move)
                        {
                            this.SendMessage(EntityMessage.EntityCommand_StopMove, this.GetPos());
                        }

                        if (GetCurState() == CreatureState.BeginDead)
                        {
                            
                            Engine.Utility.Log.LogGroup("ZDY", " 服务器已死 客户端扣血进入开始死亡状态 ");
                            //// 进入死亡状态
                           // ChangeState(CreatureState.Dead);
                        }

                        //// 发送死亡事件 代码改到进入死亡状态时发送
                        // Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_ENTITYDEAD, ed);
                    }
                }

            }



            m_CreatureProp[nPropID - (int)CreatureProp.Begin].Number = nValue;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)CreatureProp.Begin)
            {
                return base.GetProp(nPropID);
            }

            if (nPropID < (int)CreatureProp.End)
            {
                return m_CreatureProp[nPropID - (int)CreatureProp.Begin].Number;
            }

            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        public override void InitProp()
        {
            for (int i = 0; i < CreatureProp.End - CreatureProp.Begin; ++i)
            {
                m_CreatureProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }
        //-------------------------------------------------------------------------------------------------------
        //public override bool HitTest(Ray ray, out RaycastHit rayHit)
        //{
        //    if (m_EntityView != null)
        //    {
        //        return m_EntityView.HitTest(ray,out rayHit);
        //    }
        //    rayHit = new RaycastHit();
        //    return false;
        //}
        //-------------------------------------------------------------------------------------------------------
        public override void ChangeState(CreatureState state, object param = null)
        {
            if (m_FMS != null)
            {
                //if (EntitySystem.m_ClientGlobal.IsMainPlayer(this as IEntity))
                //{
                //    UnityEngine.Debug.Log("ChangeState ..." + state);
                //}
                m_FMS.ChangeState((int)state, param);
            }
        }

        // 获取对象状态
        public override CreatureState GetCurState()
        {
            if (m_FMS != null)
            {
                return (CreatureState)m_FMS.GetCurState().GetStateID();
            }

            return CreatureState.Null;
        }
//         public override bool IsDead()
//         {
//             if ((GetProp((int)(EntityProp.EntityState)) & (int)GameCmd.SceneEntryState.SceneEntry_Death) == (int)GameCmd.SceneEntryState.SceneEntry_Death)
//             {
//                 //Engine.Utility.Log.Error("已经死亡，通过标志位判断");
//                 return true;
//             }
//             if (GetCurState() == CreatureState.BeginDead)
//             {
//                 return true;
//             }
//             return GetCurState() == CreatureState.Dead;
//         }
        public override void Update(float dt)
        {
            base.Update(dt);

            if (m_FMS != null)
            {
                m_FMS.Update(dt);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 创建状态机
        private void CreateStateMachine()
        {
            m_FMS = new Engine.Utility.StateMachine<Creature>(this);

            m_FMS.RegisterState(new CreatureNormal(m_FMS));
            m_FMS.RegisterState(new CreatureIdle(m_FMS));
            m_FMS.RegisterState(new CreatureDead(m_FMS));
            m_FMS.RegisterState(new CreatureBeginDead(m_FMS));
            //m_FMS.RegisterState(new CreatureFitght(m_FMS));
            m_FMS.RegisterState(new CreatureMove(m_FMS));
            m_FMS.RegisterState(new CreatureContrl(m_FMS));
            m_FMS.ChangeState((int)Client.CreatureState.Normal, null); // 初始状态设置为Normal
        }
    }
}
