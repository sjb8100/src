
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


class LoginStepSelectZone : LoginStateBase
{

    public LoginStepSelectZone(Engine.Utility.StateMachine<LoginDataManager> machine, GX.Net.JsonHttp http)
        : base(machine,http)
    {
        m_nStateID = (int)LoginSteps.LGS_SelectZone;
    }
    public override void Enter(object param)
    {
        Debug.Log("enter LoginStateTwoStep  请求ip和端口");
        string text = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Net_Logining);
        if(param != null)
        {
            text = (string)param;
        }
        LoginDataManager ld = DataManager.Manager<LoginDataManager>();
        ld.KillCheckServerStateTimer();
        m_http.ZoneID = ld.CurSelectZoneID;
        if(!string.IsNullOrEmpty(ld.IpStr))
        {
            if (m_Statemachine.GetCurStateID() != (int)LoginSteps.LGS_GameServer)
            {
                ld.StateMachine.ChangeState((int)LoginSteps.LGS_GameServer, null);
            }
            return;
        }

        
        WaitPanelShowData waitData = new WaitPanelShowData();
        waitData.type = WaitPanelType.Login;
        waitData.cdTime = 20;
        waitData.des = text ;
        waitData.timeOutDel = ConnectTimeOut;
        waitData.useBoxMask = false;
        waitData.showTimer = false;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonWaitingPanel, data: waitData);


      
        Engine.Utility.Log.Info("SelectZone : {0}", m_http.ZoneID);
        m_http.SelectZone(() =>
        {

            if (m_Statemachine.GetCurStateID() == (int) LoginSteps.LGS_SelectZone)
            {
                if(!string.IsNullOrEmpty(m_http.GatewayUrl))
                {
                    ld.IpStr = m_http.GatewayUrl;
                    Engine.Utility.Log.Info("http SelectZone sucess ");
                    ld.StateMachine.ChangeState((int)LoginSteps.LGS_GameServer, null);
                }
            }
        
        }, (error) =>
        {
            Engine.Utility.Log.Error("http SendSelectZone error is {0}", error);
        });
    }
    void ConnectTimeOut()
    {
        m_Statemachine.ChangeState((int)LoginState.None, null);
        TipsManager.Instance.ShowTips("网络连接失败，请重试！");
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
        NetService.Instance.Close();
       
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
