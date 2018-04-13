using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;

namespace EntitySystem
{
    class CreatureMove : CreatureStateBase
    {
        public CreatureMove(Engine.Utility.StateMachine<Creature> machine)
            : base(machine)
        {
            m_nStateID = (int)CreatureState.Move;
        }

        // 进入状态
        public override void Enter(object param) {
            Creature m_Owner = m_Statemachine.GetOwner();

            //if (EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner))
            //{
            //    Engine.Utility.Log.Info("Enter .............CreatureMove ");
            //}
        }

        // 退出状态
        public override void Leave() { }

        public override void Update(float dt) { }

        public override void OnEvent(int nEventID, object param) { }
    }
}
