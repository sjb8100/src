using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using UnityEngine;
using Engine.Utility;
namespace EntitySystem
{
    class CreatureDead : CreatureStateBase,ITimer
    {
        // 状态宿主
        private Creature m_Owner = null;

        string m_strAudioPath = "";
        uint m_timerID = 10000;
        public CreatureDead(Engine.Utility.StateMachine<Creature> machine)
            : base(machine)
        {
            m_nStateID = (int)CreatureState.Dead;
        }

        // 进入状态
        public override void Enter(object param) 
        {
            m_Owner = m_Statemachine.GetOwner();
            if(m_Owner != null)
            {
                m_Owner.IsFighting = false;

                UnRide();
                PlayAudio();
                Color c = new Color(1, 1, 1);
                m_Owner.SendMessage(EntityMessage.EntityCommond_SetColor, c);
                m_Owner.SendMessage(EntityMessage.EntityCommand_StopMove, m_Owner.GetPos());
                //m_Owner.SendMessage(EntityMessage.EntityCommand_RemoveAllLinkObj, null);
                m_Owner.SendMessage(EntityMessage.EntityCommand_RemoveLinkAllEffect, null);
                m_Owner.PlayAction(Client.EntityAction.Dead, 0, 1f, -1.0f, 1,null,null);
                stEntityDead ed = new stEntityDead();
                ed.uid = m_Owner.GetUID();
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_ENTITYDEAD, ed);
                Engine.Utility.Log.LogGroup("ZDY", "enter ====== dead " + m_Owner.GetName() + "_" + m_Owner.GetID());
            }
      
        }

        // 退出状态
        public override void Leave() { }

        public override void Update(float dt) { }

        public override void OnEvent(int nEventID, object param) { }
        void PlayAudio()
        {
        
            Transform trans = m_Owner.GetTransForm();
            if(trans != null)
            {
                uint resID = 0;
                uint delayTime = 0;
                if(m_Owner.GetEntityType() == EntityType.EntityType_Player)
                {
                    int profession = m_Owner.GetProp((int)PlayerProp.Job);
                    int nSex = m_Owner.GetProp((int)PlayerProp.Sex);
                    var database = table.SelectRoleDataBase.Where((GameCmd.enumProfession)profession, (GameCmd.enmCharSex)nSex);
                    if(database != null)
                    {
                        resID = database.deadAudio;
                        delayTime = database.deadAudioDelay;
                    }
                }
                else
                {
                    INPC npc = m_Owner as INPC;
                    if(npc != null)
                    {
                        int baseID = npc.GetProp((int)EntityProp.BaseID);
                        table.NpcDataBase db = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)baseID);
                        if(db != null)
                        {
                            resID = db.dwDeathMagicSoundID;
                            delayTime = db.dwDeathMagicSoundDelay;
                        }
                    }
                }
                if(resID != 0)
                {
                    table.ResourceDataBase rdb = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(resID);
                    if(rdb != null)
                    {
                        m_strAudioPath = rdb.strPath;
                        TimerAxis.Instance().SetTimer(m_timerID, delayTime, this, 1);
                    }
                }
                
            }
        }

        void UnRide()
        {
            if (m_Owner.GetEntityType() == EntityType.EntityType_Player)
            {
                Client.IControllerSystem cs = EntitySystem.m_ClientGlobal.GetControllerSystem();
                if (cs != null)
                {
                    //这里客户端只需要下马 不需要请求服务器下马 服务器死亡最后会广播下马状态
                    bool bRide = (bool)m_Owner.SendMessage(Client.EntityMessage.EntityCommond_IsRide, null);
                    if (bRide)
                    {
                        m_Owner.SendMessage(Client.EntityMessage.EntityCommond_UnRide, null);
                    }
                }
            }
        }

        public void OnTimer(uint uTimerID)
        {
            if(uTimerID == m_timerID)
            {
               if(!string.IsNullOrEmpty(m_strAudioPath))
               {
                   Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
                   if (audio == null)
                   {
                       return;
                   }
                     Transform trans = m_Owner.GetTransForm();
                     if (trans != null)
                     {
                         audio.PlayEffect(trans.gameObject,m_strAudioPath);
                     }
               }
            }
        }
    }
}
