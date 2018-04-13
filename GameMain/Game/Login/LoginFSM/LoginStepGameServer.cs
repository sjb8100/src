
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


class LoginStepGameServer : LoginStateBase
{

    public LoginStepGameServer(Engine.Utility.StateMachine<LoginDataManager> machine, GX.Net.JsonHttp http)
        : base(machine,http)
    {
        m_nStateID = (int)LoginSteps.LGS_GameServer;
    }
    public override void Enter(object param)
    {
        Debug.Log("enter LoginStateThreeStep  开始连接网关");
        WaitPanelShowData waitData = new WaitPanelShowData();
        waitData.type = WaitPanelType.ArenaChallenge;
        waitData.cdTime = 20;
        waitData.des = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Net_Connecting); ;
        waitData.timeOutDel = delegate() { DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel); };
        waitData.useBoxMask = false;
        waitData.showTimer = false;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonWaitingPanel, data: waitData);
        LoginDataManager ld = DataManager.Manager<LoginDataManager>();
        GVoiceManger.Instance.SetAppInfo(ld.Acount + ld.CurSelectZoneID);
        string[] iport;
        string gatewayUrl = ld.IpStr;
        if (!string.IsNullOrEmpty(gatewayUrl))
        {
            string tcpgateway = gatewayUrl;
            iport = tcpgateway.Split(':');

        }
        else
        {
            Engine.Utility.Log.Error("gatewayUrl is null");
            return;
        }
        Engine.Utility.Log.Info("帐号验证成功，准备连接到网关");
        UserData.GatewayServerIP = iport[0];
        int severpot = 0;

        if (int.TryParse(iport[1], out severpot))
        {
            try
            {
                if (string.IsNullOrEmpty(gatewayUrl))
                {
                    Engine.Utility.Log.Error("gatewayurltcp is null 无法建立tcp连接");
                    return;
                }
                UserData.GatewayServerPort = severpot;
                Engine.Utility.Log.Error("NetService.Instance.Connect{0}:{1}", UserData.GatewayServerIP, severpot);
                NetService.Instance.Connect(UserData.GatewayServerIP, severpot, ld.onConnectedGameServer);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
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
