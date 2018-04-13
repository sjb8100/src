using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using UnityEngine;

namespace Controller
{
    class ControllerSystem : IControllerSystem
    {
        // 游戏全局对象
        public static IClientGlobal m_ClientGlobal = null;

        private IController m_ActiveCtrl = null;

        private ICombatRobot m_CombatRobot = null;

        private ISwitchTarget m_switchTargt = null;

        private IControllerHelper m_IControllerHelper = null;

        //private MissionRobot m_MissionRobot = null;
        public ControllerSystem(IClientGlobal clientGlobal)
        {
            m_ClientGlobal = clientGlobal;
            m_ActiveCtrl = new Controller(m_ClientGlobal);

            m_CombatRobot = new CombatRobot();

            m_switchTargt = new SwitchTarget();

            //m_MissionRobot = new MissionRobot();
        }

        // 游戏退出时使用
        public void Release()
        {
            if (m_IControllerHelper != null)
            {
                m_IControllerHelper = null;
            }
            if (m_switchTargt != null)
            {
                m_switchTargt.Relese();
                m_switchTargt = null;
            }


            if (m_ActiveCtrl != null)
            {
                m_ActiveCtrl = null;
            }

            //if (m_MissionRobot != null)
            //{
            //    m_MissionRobot = null;
            //}
            if (m_CombatRobot != null)
            {
                m_CombatRobot.Stop();
                m_CombatRobot = null;
            }
        }

        public void ActiveController(ControllerType ct = ControllerType.ControllerType_KeyBoard)
        {
            if(m_ActiveCtrl==null)
            {
                m_ActiveCtrl = new Controller(m_ClientGlobal);
            }
        }

        public IController GetActiveCtrl()
        {
            return m_ActiveCtrl;
        }

        public ICombatRobot GetCombatRobot()
        {
            return m_CombatRobot;
        }
        public ISwitchTarget GetSwitchTargetCtrl()
        {
            return m_switchTargt;
        }

        public void OnMessage(Engine.MessageCode code, object param1 = null, object param2 = null, object param3 = null)
        {
            if(m_ActiveCtrl!=null)
            {
                m_ActiveCtrl.OnMessage(code, param1, param2, param3);
            }
        }

        public void SetControllerHelper(IControllerHelper ch)
        {
            m_IControllerHelper = ch;
        }

        public IControllerHelper GetControllerHelper()
        {
            return m_IControllerHelper;
        }
    }

    public class ControllerSystemCreator
    {
        private static ControllerSystem m_ControllerSys = null;
        public static IControllerSystem CreateControllerSystem(IClientGlobal clientGlobal)
        {
            if (m_ControllerSys == null)
            {
                m_ControllerSys = new ControllerSystem(clientGlobal);
            }

            return m_ControllerSys;
        }
    }
}
