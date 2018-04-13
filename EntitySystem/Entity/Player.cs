using Client;
using System;
using System.Collections.Generic;
using UnityEngine;
using Engine.Utility;
namespace EntitySystem
{
    class Player : FightCreature, IPlayer,ITimer
    {
        private EntityCreateData m_Data = null;
        // 属性数组
        private Engine.Utility.SecurityInt[] m_PlayerProp = new Engine.Utility.SecurityInt[PlayerProp.End - PlayerProp.Begin];
        //变身恢复回调
        private Action<object> m_ChangeEventHandler = null;
        private object m_ChangeParam;
        private bool m_bChangeRestore = false; // 变身还原

        public override EntityType GetEntityType()
        {
            return EntityType.EntityType_Player;
        }

        /**
        @brief 刷新服务器位置
        @param 
        */
        public void RefrushServerPos(Vector3 pos)
        {
            if( m_EntityView == null )
            {
                return;
            }

            m_EntityView.RefrushServerPos(pos);
        }

        /**
        @brief 设置主角属性，只有主角使用
        @param 
        */
        public void SetExtraProp(GameCmd.t_MainUserData data)
        {

        }
        //-------------------------------------------------------------------------------------------------------
        // 获取时装数据
        public void GetSuit(out List<GameCmd.SuitData> lstSuit)
        {
            lstSuit = null;
            if(m_EntityView!=null)
            {
                m_EntityView.GetSuit(out lstSuit);
            }
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nPropID">属性ID</param>
        /// <param name="nValue">属性值</param>
        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)PlayerProp.Begin || nPropID >= (int)PlayerProp.End)
            {
                return;
            }
            if(nPropID == (int)PlayerProp.StateBit)
            {
                if ( ( nValue & (int)GameCmd.MainUserStateBit.MainUserStateBit_SkillPreRelive ) == (int)GameCmd.MainUserStateBit.MainUserStateBit_SkillPreRelive )
                {
                    stSkillRelive relive = new stSkillRelive();
                    relive.id = GetID();
                    if ( GetProp( (int)PlayerProp.StateBit ) != (int)GameCmd.MainUserStateBit.MainUserStateBit_SkillPreRelive )
                    {
                        Engine.Utility.EventEngine.Instance().DispatchEvent((int) (int)GameEventID.SKILL_RELIVE , relive );
                    }
                }
                else if ((nValue & (int)GameCmd.MainUserStateBit.MainUserStateBit_UnableMove) == (int)GameCmd.MainUserStateBit.MainUserStateBit_UnableMove)
                {
                    SendMessage(EntityMessage.EntityCommand_StopMove, GetPos());
                }
                else if ((nValue & (int)GameCmd.MainUserStateBit.MainUserStateBit_SerUnableMove) == (int)GameCmd.MainUserStateBit.MainUserStateBit_SerUnableMove)
                {
                    SendMessage(EntityMessage.EntityCommand_StopMove, GetPos());
                }
            }
            else if(nPropID == (int)PlayerProp.SkillStatus)
            {
                if(nValue != 0)
                {
                    int oldValue = GetProp((int)PlayerProp.SkillStatus);
                    if (oldValue != 0 && oldValue != nValue)
                    {
                        uint delay = GameTableManager.Instance.GetGlobalConfig<uint>("SkillChangeDelayTime");
                        TimerAxis.Instance().SetTimer(m_uChangePlayerModelTimer, delay, this, 1);
                    }
                }
               
            }
            m_PlayerProp[nPropID - (int)PlayerProp.Begin].Number = nValue;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)PlayerProp.Begin)
            {
                return base.GetProp(nPropID);
            }

            if (nPropID < (int)PlayerProp.End)
            {
                return m_PlayerProp[nPropID - (int)PlayerProp.Begin].Number;
            }

            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        public override void InitProp()
        {
            for (int i = 0; i < PlayerProp.End - PlayerProp.Begin; ++i)
            {
                m_PlayerProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }

        public override bool CreateEntityView(EntityCreateData data)
        {
            //Profiler.BeginSample("CreateEntityView");
            m_Data = data;
            base.CreateEntityView(data);
            if (GetEntityType() != EntityType.EntityType_Player)
            {
                m_bChangeRestore = false;
                Engine.Utility.Log.Error("CreateEntityView failed:{0}", GetName());
                return false;
            }

            string strResName = "";
            //if (data.PropList != null)
            //{
            //    for (int i = 0; i < data.PropList.Length; ++i )
            //    {
            //        if (data.PropList[i].nPropIndex == (int)PlayerProp.TransModelResId)
            //        {
            int ntransModelID = GetProp((int)PlayerProp.TransModelResId);//.PropList[i].value;
            if (ntransModelID != 0)
            {
                table.ResourceDataBase resData = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>((uint)ntransModelID);
                if (resData != null)
                {
                    strResName = resData.strPath;
                }
            }
            //            break;
            //        }
            //    }
            //}

            table.SuitDataBase table_data = null;
            if (string.IsNullOrEmpty(strResName) && data.ViewList != null)
            {
                //Profiler.BeginSample("CreateEntityView:Table:Query");
                for (int i = 0; i < data.ViewList.Length; ++i)
                {
                    if(data.ViewList[i]==null)
                    {
                        continue;
                    }

                    if (data.ViewList[i].nPos == (int)EquipPos.EquipPos_Body)
                    {
                        table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)data.ViewList[i].value);
                        if(table_data == null)
                        {
                            GameCmd.enumProfession profession = (GameCmd.enumProfession)GetProp((int)PlayerProp.Job);
                            GameCmd.enmCharSex sex = (GameCmd.enmCharSex)GetProp((int)PlayerProp.Sex);
                            var role_data = table.SelectRoleDataBase.Where(profession, sex);
                            if (role_data == null)
                            {
                                Engine.Utility.Log.Error("CreateEntityView:job{0}或者sex{1}数据非法!", profession, sex);
                                return false;
                            }

                            table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)role_data.bodyPathID);
                        }
                        if (table_data != null)
                        {
                            table.ResourceDataBase db = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(table_data.resid);
                            if (db == null)
                            {
                                Debug.LogError("ResourceDataBase is null bodypahtid is " + table_data.resid + "  请检查resource 表格");
                                return false;
                            }
                            strResName = db.strPath;
                        }
                        //else
                        //{
                        //    strResName = "";
                        //}

                        //if(m_EntityView!=null)
                        //{
                        //    m_EntityView.AddSuitData(EquipPos.EquipPos_Body, data.ViewList[i].value, (int)table_data.resid);
                        //}

                    }
                }
                //Profiler.EndSample();
            }

            // 修改技能形态值
            m_Data.eSkillState = (SkillSettingState)GetProp((int)PlayerProp.SkillStatus);

            // 设置名称
            SetName(data.strName);
            string strPath, strFileName, strExt, strFileNameNoExt;
            Engine.Utility.StringUtility.ParseFileName(ref strResName, out strPath, out strFileName, out strFileNameNoExt, out strExt);
            m_strObjName = strFileNameNoExt;

            if (m_Data.bImmediate)
            {
                // 立即创建
                if (!m_EntityView.Create(ref strResName, OnCreateRenderObj, m_Data.bImmediate))
                {
                    Engine.Utility.Log.Error("CreateEntityView failed:{0}", strResName);
                    return false;
                }
            }
            else
            {
                EntityViewCreator.Instance.AddView(GetUID(), m_EntityView, ref strResName, OnCreateRenderObj);
            }

            //Profiler.EndSample();
            //Profiler.EndSample();
            return true;
        }
        //-------------------------------------------------------------------------------------------------------
        public override bool CreatePart()
        {
            AddPart(EntityPart.Skill);
            AddPart(EntityPart.Buff);
            AddPart(EntityPart.Equip);
            return true;
        }
      
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void OnCreateRenderObj(Engine.IRenderObj obj, object param)
        {
            // 加载部件
            if(m_EntityView==null)
            {
                m_bChangeRestore = false;
                return;
            }

            // 设置层信息
            if (EntitySystem.m_ClientGlobal.IsMainPlayer(this))
            {
                m_EntityView.renderObj.SetLayer(LayerMask.NameToLayer("MainPlayer"));
            }
            
            // 根据全局配置设置阴影
            int nShadowLevel = EntitySystem.m_ClientGlobal.gameOption.GetInt("Render","shadow",1);
            m_EntityView.SetShadowLevel(nShadowLevel);

            Vector3 currPos = GetPos();
            SetPos(ref currPos);
            Vector3 dir = GetRotate();
            SetRotate(dir);

            // 还原时回调
            if (m_ChangeEventHandler != null)
            {
                m_ChangeEventHandler(m_ChangeParam);
                m_ChangeEventHandler = null;
                m_ChangeParam = null;
            }
            
            // 根据外部设置来设置
            SendMessage(EntityMessage.EntityCommond_SetColor, new Color(1, 1, 1));
            SendMessage(EntityMessage.EntityCommand_EnableShowModel, EntityConfig.m_bShowEntity);

            // 播放下动画
            PlayAction(m_EntityView.GetCurAniName());
            
            // 先做回调
            if (!m_bChangeRestore)
            {
                Client.stCreateEntity createEntity = new Client.stCreateEntity();
                createEntity.uid = GetUID();
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_CREATEENTITY, createEntity);
            }
            else
            {
                m_bChangeRestore = false;
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ENTITYSYSTEM_RESTORE,
                    new Client.stPlayerChange() { status = 2, uid = GetID() });
            }
        }

        public bool IsMainPlayer()
        {
            return EntitySystem.m_ClientGlobal.IsMainPlayer( this );
        }

        public override void Restore(object param)
        {
            if (param != null)
            {
                Client.ChangeBody changeParam = (Client.ChangeBody)param;
                m_ChangeEventHandler = changeParam.callback;
                m_ChangeParam = changeParam.param;
            }

            m_bChangeRestore = true;
            CreateEntityView(m_Data);

            //解决变身回来后会播放一个无关动作的问题
            PlayAni anim_param = new PlayAni();
            anim_param.strAcionName = EntityAction.Stand;
            anim_param.fSpeed = 1;
            anim_param.nStartFrame = 0;
            anim_param.nLoop = -1;
            anim_param.fBlendTime = 0.1f;
            SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
        }
        private uint m_uChangePlayerModelTimer = 1000;
        public void OnTimer(uint uTimerID)
        {
           if(uTimerID == m_uChangePlayerModelTimer)
           {
               stSkillChangeModel st = new stSkillChangeModel();
               st.userID = GetID();
               st.skillStatus = GetProp((int)PlayerProp.SkillStatus);
               EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSTEM_CHANGEMODEL, st);
           
           }
        }
    }
}
