
//*************************************************************************
//	创建日期:	2017/6/13 星期二 20:09:12
//	文件名称:	LoginStateBase
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;

class LoginStateBase : Engine.Utility.State
{
    protected Engine.Utility.StateMachine<LoginDataManager> m_Statemachine = null;
    protected GX.Net.JsonHttp m_http;
    public LoginStateBase(Engine.Utility.StateMachine<LoginDataManager> machine, GX.Net.JsonHttp http)
    {
        m_Statemachine = machine;
        m_http = http;
    }
}
