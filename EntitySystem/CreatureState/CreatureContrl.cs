using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
namespace EntitySystem
{
    class CreatureContrl:CreatureStateBase
    {
        public CreatureContrl(Engine.Utility.StateMachine<Creature> machine)
            : base(machine)
        {
            m_nStateID = (int)CreatureState.Contrl;
        }

        // 进入状态
        public override void Enter(object param) 
        {
            Engine.Utility.Log.LogGroup("ZDY", "enter contrl state");
        }

        // 退出状态
        public override void Leave() 
        {
            Engine.Utility.Log.LogGroup("ZDY", "leave contrl state");
        }

        public override void Update(float dt) { }

        public override void OnEvent(int nEventID, object param) { }
    }
}
