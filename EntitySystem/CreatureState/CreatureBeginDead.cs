using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
namespace EntitySystem
{
    class CreatureBeginDead:CreatureStateBase
    {
        private Creature m_Owner = null;
        public CreatureBeginDead(Engine.Utility.StateMachine<Creature> machine)
            : base(machine)
        {
            m_nStateID = (int)CreatureState.BeginDead;
        }

        // 进入状态
        public override void Enter(object param) 
        {
            m_Owner = m_Statemachine.GetOwner();
            if(m_Owner != null)
            {
                stEntityBeginDead st = new stEntityBeginDead();
                st.uid = m_Owner.GetUID();
                m_Owner.SendMessage(EntityMessage.EntityCommand_StopMove, m_Owner.GetPos());
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINDEAD, st);
                Engine.Utility.Log.LogGroup("ZDY", "enter begin dead " + m_Owner.GetName() + "_" + m_Owner.GetID());
                m_Owner.ChangeState(CreatureState.Dead);
            }
      
        }

        // 退出状态
        public override void Leave() 
        {
        
        }

        public override void Update(float dt)
        {
        
        }

        public override void OnEvent(int nEventID, object param) 
        {
        
        }
    }
}
