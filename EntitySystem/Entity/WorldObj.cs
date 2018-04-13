using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;

namespace EntitySystem
{
    class WorldObj : Entity, IWorldObj
    {
        // 属性数组
        private Engine.Utility.SecurityInt[] m_WorldObjProp = new Engine.Utility.SecurityInt[WorldObjProp.End - WorldObjProp.Begin];

        public override bool Create(EntityCreateData data, ColliderCheckType colliderCheckType)
        {
            // 设置属性
            InitProp();
            ApplyProp(data);

            // 创建组件
            CreateCommpent();

            // 初始化逻辑部件
            CreatePart();

            // 创建外观对象
            CreateEntityView(data);

            // 创建状态机
            //CreateStateMachine();
            return base.Create(data, colliderCheckType);
        }
        //-------------------------------------------------------------------------------------------------------
        public virtual void CreateCommpent()
        {
            AddCommpent(EntityCommpent.EntityCommpent_Visual);
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nPropID">属性ID</param>
        /// <param name="nValue">属性值</param>
        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)WorldObjProp.Begin || nPropID >= (int)WorldObjProp.End)
            {
                return;
            }

            m_WorldObjProp[nPropID - (int)WorldObjProp.Begin].Number = nValue;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)WorldObjProp.Begin)
            {
                return base.GetProp(nPropID);
            }

            if (nPropID < (int)WorldObjProp.End)
            {
                return m_WorldObjProp[nPropID - (int)WorldObjProp.Begin].Number;
            }

            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        public override void InitProp()
        {
            for (int i = 0; i < WorldObjProp.End - WorldObjProp.Begin; ++i)
            {
                m_WorldObjProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }
        //-------------------------------------------------------------------------------------------------------
        public override bool HitTest(Ray ray, out RaycastHit rayHit)
        {
            if (m_EntityView != null)
            {
                return m_EntityView.HitTest(ray, out rayHit);
            }
            rayHit = new RaycastHit();
            return false;
        }
        //-------------------------------------------------------------------------------------------------------
        //public override void ChangeState(CreatureState state, object param = null)
        //{
        //    //if (m_FMS != null)
        //    //{
        //    //    //if (EntitySystem.m_ClientGlobal.IsMainPlayer(this as IEntity))
        //    //    //{
        //    //    //    UnityEngine.Debug.Log("ChangeState ..." + state);
        //    //    //}
        //    //    m_FMS.ChangeState((int)state, param);
        //    //}
        //    base.ChangeState(state, param);
        //}

        //// 获取对象状态
        //public override CreatureState GetCurState()
        //{
        //    return base.GetCurState();
        //}

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
