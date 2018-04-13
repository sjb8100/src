using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;

namespace EntitySystem
{
    class CreatureIdle : CreatureStateBase
    {
        public CreatureIdle(Engine.Utility.StateMachine<Creature> machine)
            : base(machine)
        {
            m_nStateID = (int)CreatureState.Idle;
        }

        // 进入状态
        public override void Enter(object param) { }

        // 退出状态
        public override void Leave() { }

        public override void Update(float dt) { }

        public override void OnEvent(int nEventID, object param) { }
    }
}
