
//*************************************************************************
//	创建日期:	2017/6/13 星期二 20:11:40
//	文件名称:	LoginStateOneStep
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class LoginStepNone : LoginStateBase
{

    public LoginStepNone(Engine.Utility.StateMachine<LoginDataManager> machine, GX.Net.JsonHttp http)
        : base(machine,http)
    {
        m_nStateID = (int)LoginSteps.None;
    }
    public override void Enter(object param)
    {
        Debug.Log("enter LoginStateNone");
    }

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
