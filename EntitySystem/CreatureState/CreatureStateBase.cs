using System;
using System.Collections.Generic;
using System.Text;
using Client;

namespace EntitySystem
{
    class CreatureStateBase : Engine.Utility.State
    {
        protected Engine.Utility.StateMachine<Creature> m_Statemachine = null;

        public CreatureStateBase(Engine.Utility.StateMachine<Creature> machine)
        {
            m_Statemachine = machine;
        }
    }
}