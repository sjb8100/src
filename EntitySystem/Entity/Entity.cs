using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
using Engine;

namespace EntitySystem
{

    class Entity : Client.IEntity
    {
        private uint m_uID = 0;
        private long m_lID = 0;
        private string m_strName = "";
        // 对象名称
        protected string m_strObjName = "";

        protected EntityView m_EntityView = null;

        private ColliderCheckType m_ColliderCheckType = ColliderCheckType.ColliderCheckType_null;

        private IEntityCallback m_EntityCallback = null;

        // 自定义数据
        private object m_customData = null;

        // 属性数组
        private Engine.Utility.SecurityInt[] m_Prop = new Engine.Utility.SecurityInt[EntityProp.End - EntityProp.Begin];

        // 组件列表(渲染对象相关)
        private Dictionary<int, IEntityCommpent> m_Commpents = new Dictionary<int, IEntityCommpent>();

        // 部件列表(逻辑相关)
        private Dictionary<int, IEntityPart> m_Parts = new Dictionary<int, IEntityPart>();

        public IRenderObj renderObj
        {
            get
            {
                return m_EntityView == null ? null : m_EntityView.renderObj;
            }
        }

        // 实体销毁
        public void Release()
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }
            foreach (var part in m_Parts)
            {
                part.Value.Release();
            }

            EntityViewCreator.Instance.RemoveView(m_lID);

            if (m_EntityView != null)
            {
                m_EntityView.Close();
            }
        }

        // 获取对象名称
        public string GetObjName()
        {
            return m_strObjName;
        }

        public EntityView GetView() { return m_EntityView; }


        public virtual bool Create(EntityCreateData data, ColliderCheckType colliderCheckType)
        {
            if (data == null)
            {
                Engine.Utility.Log.Error("Create entity failed!");
                return false;
            }

            //m_strName = data.strName;
            m_ColliderCheckType = colliderCheckType;
            return true;
        }

        public virtual bool CreatePart()
        {
            return true;
        }

        public virtual void Update(float dt)
        {
            Dictionary<int, IEntityCommpent>.Enumerator iter = m_Commpents.GetEnumerator();
            while (iter.MoveNext())
            {
                iter.Current.Value.Update();
            }

            Dictionary<int, IEntityPart>.Enumerator iterPart = m_Parts.GetEnumerator();
            while (iterPart.MoveNext())
            {
                iterPart.Current.Value.Update(dt);
            }

            if (m_EntityCallback != null)
            {
                m_EntityCallback.OnUpdate(this, null);
            }

            // 检测实体设置改变
            CheckConfig();
        }
        /// <summary>
        /// 服务器给的userid
        /// </summary>
        /// <returns></returns>
        public uint GetID()
        {
            return m_uID;
        }

        // 客户端使用的唯一ID
        public long GetUID()
        {
            return m_lID;
        }

        public void SetUID(long uid)
        {
            m_lID = uid;
        }

        // 获取实体名称
        public string GetName()
        {
            return m_strName;
        }

        public void SetData(object data)
        {
            m_customData = data;
        }
        public object GetData()
        {
            return m_customData;
        }

        // 设置实体运动碰撞回调 
        public void SetCallback(IEntityCallback callback)
        {
            m_EntityCallback = callback;
        }

        // 消息处理
        public object SendMessage(EntityMessage cmd, object param = null)
        {
            if(cmd == EntityMessage.EntityCommand_StopMove)
            {
               // Debug.LogError("send stopmove");
            }
            Dictionary<int, IEntityCommpent>.Enumerator iter = m_Commpents.GetEnumerator();
            object ret = null;
            while (iter.MoveNext())
            {
                ret = iter.Current.Value.OnMessage(cmd, param);
                if (ret != null)
                {
                    return ret;
                }
            }

            return null;
        }

        // 获取部件
        public IEntityPart GetPart()
        {
            return null;
        }
        public Vector3 GetPos()
        {
            if (m_EntityView != null)
            {
                return m_EntityView.GetPos();
            }
            return Vector3.zero;
        }


        // 获取方向
        public Vector3 GetDir()
        {
            if (m_EntityView != null)
            {
                return m_EntityView.GetDir();
            }

            return Vector3.zero;
        }

        // 获取旋转欧拉角
        public Vector3 GetRotate()
        {
            if (m_EntityView != null)
            {
                return m_EntityView.GetRotate();
            }

            return Vector3.zero;
        }

        // 获取半径
        public float GetRadius()
        {
            // 根据配置表读取半径
            int nID = GetProp((int)EntityProp.BaseID);
            {
                EntityType t = GetEntityType();
                if (t == EntityType.EntityType_Player)
                {
                    return 1.0f; // 人物默认半径
                }
                else if (t == EntityType.EntityType_NPC)
                {
                    var table_data = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)nID);
                    if (table_data == null)
                    {
                        Engine.Utility.Log.Error("不存在这个NPC: " + nID);
                        return 0.0f;
                    }

                    return table_data.modelrange * 0.01f;
                }

            }

            return 0.0f;
        }

        // 获取碰撞检测标识
        public ColliderCheckType GetColliderCheckFlag()
        {
            return m_ColliderCheckType;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////
        /// unity 默认的旋转顺序 yxz
        /// <summary>
        /// 绕X轴旋转
        /// </summary>
        /// <param name="fAngle">角度值</param>
        public void RotateX(float fDeltaAngle)
        {
            if (m_EntityView != null)
            {
                m_EntityView.RotateX(fDeltaAngle);
            }
        }

        /// unity 默认的旋转顺序 yxz
        /// <summary>
        /// 绕X轴旋转
        /// </summary>
        /// <param name="fAngle">角度值</param>
        public void RotateToX(float fAngle)
        {
            if (m_EntityView != null)
            {
                m_EntityView.RotateToX(fAngle);
            }
        }

        /// <summary>
        /// 绕Y轴旋转
        /// </summary>
        /// <param name="fAngle">角度值</param>
        public void RotateY(float fDeltaAngle)
        {
            if (m_EntityView != null)
            {
                m_EntityView.RotateY(fDeltaAngle);
            }
        }

        /// <summary>
        /// 绕Y轴旋转
        /// </summary>
        /// <param name="fAngle">角度值</param>
        public void RotateToY(float fAngle)
        {
            if (m_EntityView != null)
            {
                m_EntityView.RotateToY(fAngle);
            }
        }
        
        public void SetPos(ref Vector3 pos)
        {
            if (m_EntityView != null)
            {
                m_EntityView.SetPos(ref pos);
            }
        }

        // 设置旋转欧拉角
        public void SetRotate(Vector3 eulerAngle)
        {
            if (m_EntityView != null)
            {
                m_EntityView.SetRotate(eulerAngle);
            }
        }

        public void LookAt(Vector3 target, Vector3 up)
        {
            if (m_EntityView != null)
            {
                m_EntityView.LookAt(target, up);
            }
        }

        /// <summary>
        /// 播放动作 
        /// </summary>
        /// <param name="strActionName"></param>
        public void PlayAction(string strActionName, int nStartFrame = 0, float fSpeed = 1f, float fBlendTime = -1.0f, int nLoop = -1, Engine.ActionEvent callback = null, object param = null)
        {

            if (EntitySystem.m_ClientGlobal.IsMainPlayer(this))
            {
                if (strActionName == EntityAction.Stand_Combat || strActionName == EntityAction.Stand)
                {
                    Engine.Utility.Log.LogGroup("ZDY", "play " + strActionName);
                }
              //  Engine.Utility.Log.LogGroup("ZDY","play " + strActionName);
                if(strActionName == "Attack001")
                {
                  //  Debug.LogError("play Attack001");
                }
            }

            float speedFact = 1.0f;
            // 目前简单播放动画
            if (m_EntityView != null)
            {
                // 根据移动速度修正动画速度
                if (strActionName == EntityAction.Run || strActionName == EntityAction.Run_Combat)
                {
                    if (GetEntityType() == EntityType.EntityType_Player || GetEntityType() == EntityType.EntityTYpe_Robot)
                    {
                        uint speed = GameTableManager.Instance.GetGlobalConfig<uint>("Base_Move_Speed");
                        if (speed == 0)
                        {
                            speed = 1;
                        }
                        speedFact = (float)GetProp((int)WorldObjProp.MoveSpeed) / (float)speed;

                    }
                    else if (GetEntityType() == EntityType.EntityType_NPC)
                    {
                        // 速度修正
                        int nNpcID = GetProp((int)EntityProp.BaseID);
                        var table_data = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)nNpcID);
                        if (table_data != null)
                        {
                            uint speed = table_data.dwRunSpeed;
                            if (speed == 0)
                            {
                                speed = 1;
                            }
                            speedFact = (float)GetProp((int)WorldObjProp.MoveSpeed) / (float)speed;
                        }
                    }
                }

                // 对动作名称按区域类型进行修正
                string strAiName = strActionName;
                if (GetEntityType() == EntityType.EntityType_Player || GetEntityType() == EntityType.EntityTYpe_Robot)
                {
                    bool bRide = (bool)SendMessage(EntityMessage.EntityCommond_IsRide, null);
                    if (bRide)
                    {
                        if (strActionName == EntityAction.Run_Combat)
                        {
                            strAiName = EntityAction.Run;
                        }

                        if (strActionName == EntityAction.Stand_Combat)
                        {
                            strAiName = EntityAction.Stand;
                        }
                    }
                    else
                    {
                        if (strActionName == EntityAction.Run || strActionName == EntityAction.Stand)
                        {
                            MapAreaType type = EntitySystem.m_ClientGlobal.GetMapSystem().GetAreaTypeByPos(GetPos());
                            if (type != MapAreaType.Safe && type != MapAreaType.Fish)
                            {
                                strAiName = string.Format("{0}_Combat", strActionName);
                            }
                        }
                    }

                }
                m_EntityView.PlayAction(strAiName, nStartFrame, fSpeed*speedFact, fBlendTime, nLoop, callback, param);

            }
        }
        public bool IsPlayingAnim(string clipName)
        {
            if (m_EntityView==null)
            {
                return false;
            }
            return m_EntityView.IsPlayingAnim(clipName);
        }
        /// <summary>
        /// 播放动作 
        /// </summary>
        /// <param name="strActionName"></param>
        public void PauseAni()
        {
            // 目前简单播放动画
            if (m_EntityView != null)
            {
                m_EntityView.PauseAni();
            }
        }

        /// <summary>
        /// 播放动作 
        /// </summary>
        /// <param name="strActionName"></param>
        public void ResumeAni()
        {
            // 目前简单播放动画
            if (m_EntityView != null)
            {
                m_EntityView.ResumeAni();
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public void ClearAniCallback()
        {
            if (m_EntityView != null)
            {
                m_EntityView.ClearAniCallback();
            }
        }
        /// <summary>
        /// 修改部件
        /// </summary>
        /// <param name="strPartName">部件名称</param>
        /// <param name="strResourceName">资源名称</param>
        public void ChangePart(ref string strPartName, ref string strResourceName, string strMaterialName = "")
        {
            if (m_EntityView != null)
            {
                m_EntityView.ChangePart(ref strPartName, ref strResourceName, strMaterialName);
            }
        }

        // 添加LinkObj
        /// <summary>
        /// 添加link对象
        /// </summary>
        /// <param name="pRenderObj">挂接对象</param>
        /// <param name="strLinkName">挂接点名称，如果为空，则默认在原点处</param>
        /// <param name="vOffset">相对挂接点偏移</param>
        /// <param name="rotate">相对挂接点旋转</param>
        /// <param name="eFollowType">跟随类型</param>
        /// <returns>linkID,删除时使用</returns>
        public int AddLinkObj(Engine.IRenderObj pRenderObj, ref string strLocatorName, Vector3 vOffset, Quaternion rotate, Engine.LinkFollowType eFollowType = Engine.LinkFollowType.LinkFollowType_ALL)
        {
            if (m_EntityView != null)
            {
                return m_EntityView.AddLinkObj(pRenderObj, ref strLocatorName, vOffset, rotate, eFollowType);
            }

            return 0;
        }

        // 添加LinkObj
        /// <summary>
        /// 添加link对象
        /// </summary>
        /// <param name="strRenderObj">挂接对象</param>
        /// <param name="strLinkName">挂接点名称，如果为空，则默认在原点处</param>
        /// <param name="vOffset">相对挂接点偏移</param>
        /// <param name="rotate">相对挂接点旋转</param>
        /// <param name="eFollowType">跟随类型</param>
        /// <returns>linkID,删除时使用</returns>
        public int AddLinkObj(ref string strRenderObj, ref string strLocatorName, Vector3 vOffset, Quaternion rotate, Engine.LinkFollowType eFollowType = Engine.LinkFollowType.LinkFollowType_ALL)
        {
            if (m_EntityView != null)
            {
                return m_EntityView.AddLinkObj(ref strRenderObj, ref strLocatorName, vOffset, rotate, eFollowType);
            }

            return 0;
        }

        /// <summary>
        /// 删除挂接对象
        /// </summary>
        /// <param name="nLinkID">挂接对象ID</param>
        /// <param name="bRemove">是否删除对象标志</param>
        /// <returns></returns>
        public Engine.IRenderObj RemoveLinkObj(int nLinkID, bool bRemove = true)
        {
            if (m_EntityView != null)
            {
                return m_EntityView.RemoveLinkObj(nLinkID, bRemove);
            }

            return null;
        }

        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 删除所有LinkObjeect
        @param 
        */
        public void RemoveAllLinkObject()
        {
            if (m_EntityView != null)
            {
                m_EntityView.RemoveAllLinkObj();
            }
        }

        //// 添加LinkObj
        ///// <summary>
        ///// 添加link对象
        ///// </summary>
        ///// <param name="strEffect">挂接对象资源名</param>
        ///// <param name="strLocatorName">挂接点名称，如果为空，则默认在原点处</param>
        ///// <param name="vOffset">相对挂接点偏移</param>
        ///// <param name="rotate">相对挂接点旋转</param>
        ///// <param name="eFollowType">跟随类型</param>
        ///// <returns>linkID,删除时使用</returns>
        //public int AddLinkEffect(ref string strEffect, ref string strLocatorName, Vector3 vOffset, Quaternion rotate, Vector3 scale,Engine.LinkFollowType eFollowType = Engine.LinkFollowType.LinkFollowType_ALL)
        //{
        //    if (m_EntityView != null)
        //    {
        //        return m_EntityView.AddLinkEffect(ref strEffect, ref strLocatorName, vOffset, rotate,scale, eFollowType);
        //    }

        //    return 0;
        //}

        /// <summary>
        /// 删除挂接对象
        /// </summary>
        /// <param name="nLinkID">挂接对象ID</param>
        /// <param name="bRemove">是否删除对象标志</param>
        /// <returns></returns>
        public Engine.IEffect RemoveLinkEffect(int nLinkID, bool bRemove = true)
        {
            if (m_EntityView != null)
            {
                return m_EntityView.RemoveLinkEffect(nLinkID, bRemove);
            }

            return null;
        }

        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 删除所有LinkObjeect
        @param 
        */
        public void RemoveAllLinkEffect()
        {
            if (m_EntityView != null)
            {
                m_EntityView.RemoveAllLinkEffect();
            }
        }
        public void GetLocatorPos(string strLocatorName, Vector3 vOffset, Quaternion rot, ref Vector3 pos, bool bWorld = false)
        {
            if (m_EntityView != null)
            {
                m_EntityView.GetLocatorPos(strLocatorName, vOffset, rot, ref pos, bWorld);
            }
            else
            {
                pos = Vector3.zero;
            }
        }
        public void SetVisible(bool bVisible)
        {
            if (m_EntityView != null)
            {
                m_EntityView.SetVisible(bVisible);
            }
        }
        public void SetAlpha(float alpha)
        {
            if (m_EntityView != null)
            {
                m_EntityView.SetAlpha(alpha);
            }
        }

        // 获取实体类型
        public virtual EntityType GetEntityType()
        {
            return EntityType.EntityType_Null;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nPropID">属性ID</param>
        /// <param name="nValue">属性值</param>
        public virtual void SetProp(int nPropID, int nValue)
        {
            if (nPropID < (int)EntityProp.Begin || nPropID >= (int)EntityProp.End)
            {
                return;
            }
            if (nPropID == (int)EntityProp.EntityState)
            {
                if ((nValue & (int)GameCmd.SceneEntryState.SceneEntry_Hide) == (int)GameCmd.SceneEntryState.SceneEntry_Hide)
                {
                    stEntityHide st = new stEntityHide();
                    st.uid = GetUID();
                    st.bHide = true;

                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_SETHIDE, st);
                    //   SendMessage( EntityMessage.EntityCommond_SetAlpha , 0.31f );
                }
                else
                {
                    int oldValue = m_Prop[nPropID].Number;
                    if ((oldValue & (int)GameCmd.SceneEntryState.SceneEntry_Hide) == (int)GameCmd.SceneEntryState.SceneEntry_Hide)
                    {//如果原来是隐身状态 才改变状态
                        stEntityHide st = new stEntityHide();
                        st.uid = GetUID();
                        st.bHide = false;
                        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_SETHIDE, st);
                    }
                  
                    //    SendMessage( EntityMessage.EntityCommond_SetAlpha , 1f );
                }
            }
            m_Prop[nPropID].Number = nValue;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public virtual int GetProp(int nPropID)
        {
            if (nPropID < (int)EntityProp.Begin || nPropID >= (int)EntityProp.End)
            {
                return 0;
            }

            return m_Prop[nPropID].Number;
        }

        

        // 更新多条属性
        public virtual void UpdateProp(EntityCreateData data)
        {
            ApplyProp(data);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        // 添加组件
        public void AddCommpent(EntityCommpent enCommpent)
        {
            int entityCommpent = (int)enCommpent;
            if (m_Commpents.ContainsKey(entityCommpent))
            {
                return;
            }

            switch (entityCommpent)
            {
                case (int)EntityCommpent.EntityCommpent_Visual:    // 外观
                    {
                        EntityVisual visual = new EntityVisual(this);
                        visual.Create();
                        m_Commpents[entityCommpent] = visual;
                        break;
                    }
                case (int)EntityCommpent.EntityCommpent_Move:    // 移动
                    {
                        EntityMove move = new EntityMove(this);
                        move.Create();
                        m_Commpents[entityCommpent] = move;
                        break;
                    }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        // 获取组件
        public IEntityCommpent GetCommpent(EntityCommpent entityCommpent)
        {
            int key = (int)entityCommpent;
            if (!m_Commpents.ContainsKey(key))
            {
                return null;
            }
            return m_Commpents[key];

        }
        //-------------------------------------------------------------------------------------------------------
        public void AddPart(EntityPart enPart)
        {
            int entityPart = (int)enPart;
            if (m_Parts.ContainsKey(entityPart))
            {
                return;
            }

            switch (enPart)
            {
                case EntityPart.Skill:    // 技能
                    {
                        ISkillSystem skillSys = EntitySystem.m_ClientGlobal.GetSkillSystem();
                        if (skillSys == null)
                        {//编辑器模式不需要
                            break;
                        }
                        ISkillPart skillPart = skillSys.CreateSkillPart();
                        if (skillPart == null)
                        {
                            return;
                        }
                        if (!skillPart.Create(this))
                        {
                            Engine.Utility.Log.Error("AddPart failed!");
                            skillPart = null;
                            return;
                        }
                        m_Parts[entityPart] = skillPart;
                        break;
                    }
                case EntityPart.Buff: // Buff
                    {
                        ISkillSystem skillSys = EntitySystem.m_ClientGlobal.GetSkillSystem();
                        if (skillSys == null)
                        {//编辑器模式不需要
                            break;
                        }
                        IBuffPart buffpart = skillSys.CreateBuffPart();
                        if (buffpart == null)
                        {
                            return;
                        }

                        if (!buffpart.Create(this))
                        {
                            Engine.Utility.Log.Error("AddPart failed!");
                            buffpart = null;
                            return;
                        }
                        m_Parts[entityPart] = buffpart;
                        break;
                    }
                case EntityPart.Equip: // Equip 装备
                    {
                        IEquipPart equipPart = new EquipPart();
                        if (equipPart == null)
                        {
                            return;
                        }

                        if (!equipPart.Create(this))
                        {
                            Engine.Utility.Log.Error("AddPart failed!");
                            equipPart = null;
                            return;
                        }
                        m_Parts[entityPart] = equipPart;
                        break;
                    }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        // 获取组件
        public IEntityPart GetPart(EntityPart entityPart)
        {
            int key = (int)entityPart;
            if (!m_Parts.ContainsKey(key))
            {
                return null;
            }
            return m_Parts[key];

        }

        public IEntityCallback GetEntityCallback()
        {
            return m_EntityCallback;
        }

        //-------------------------------------------------------------------------------------------------------
        public void ApplyProp(EntityCreateData data)
        {
            if (data == null)
            {
                return;
            }

            if (data.PropList == null)
            {
                return;
            }

            m_uID = data.ID;

            for (int i = 0; i < data.PropList.Length; ++i)
            {
                if (data.PropList[i] != null)
                {
                    SetProp(data.PropList[i].nPropIndex, data.PropList[i].value);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public virtual void InitProp()
        {
            for (int i = 0; i < EntityProp.End - EntityProp.Begin; ++i)
            {
                m_Prop[i] = new Engine.Utility.SecurityInt(0);
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public virtual bool CreateEntityView(EntityCreateData data)
        {
            if (m_EntityView == null)
            {
                m_EntityView = new EntityView(this, data);
            }

            return true;
        }

        // Hittest
        public virtual bool HitTest(Ray ray, out RaycastHit rayHit)
        {
            rayHit = new RaycastHit();
            return false;
        }

        public virtual Engine.Node GetNode()
        {
            if (m_EntityView == null)
            {
                return null;
            }
            return m_EntityView.GetNode();
        }
        public Transform GetTransForm()
        {
            Engine.Node node = GetNode();
            if(node == null)
            {
                return null;
            }
            return node.GetTransForm();
        }

        // 获取动画状态，兼容外部接口 不建议使用
        public AnimationState GetAnimationState(string strAniName)
        {
            if (m_EntityView == null)
            {
                return null;
            }
            return m_EntityView.GetAnimationState(strAniName);
        }

        public void SetName(string name)
        {
            m_strName = name;
        }

        public void SetShowGhost(bool isShow)
        {
            if (m_EntityView != null)
            {
                m_EntityView.SetShowGhost(isShow);
            }
        }

        public virtual void ChangeState(CreatureState state, object param = null)
        {

        }

        // 获取对象状态
        public virtual CreatureState GetCurState()
        {
            return CreatureState.Null;
        }
        public bool IsHide()
        {
            if ((GetProp((int)(EntityProp.EntityState)) & (int)GameCmd.SceneEntryState.SceneEntry_Hide) == (int)GameCmd.SceneEntryState.SceneEntry_Hide)
            {
                return true;
            }
            return false;
        }
        public bool IsDead()
        {
            if ((GetProp((int)(EntityProp.EntityState)) & (int)GameCmd.SceneEntryState.SceneEntry_Death) == (int)GameCmd.SceneEntryState.SceneEntry_Death)
            {
                //Engine.Utility.Log.Error("已经死亡，通过标志位判断");
                return true;
            }
            if (GetCurState() == CreatureState.BeginDead)
            {
                return true;
            }
            return GetCurState() == CreatureState.Dead;
        }

        public virtual void Restore(object param)
        {
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        //
        private void CheckConfig()
        {
            if (m_EntityView != null)
            {
                m_EntityView.CheckConfig();
            }
        }
    }
}
