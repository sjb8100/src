/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Login.LoginFSM
 * 创建人：  wenjunhua.zqgame
 * 文件名：  LoginStateAuthorize
 * 版本号：  V1.0.0.0
 * 创建时间：9/18/2017 4:55:53 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class LoginStepAuthorize : LoginStateBase
    {
    public LoginStepAuthorize(Engine.Utility.StateMachine<LoginDataManager> machine, GX.Net.JsonHttp http)
        : base(machine, http)
    {
        m_nStateID = (int)LoginSteps.LGS_Authorize;
    }
    public override void Enter(object param)
    {
        Action callback = (Action)param;
        LoginDataManager ld = DataManager.Manager<LoginDataManager>();
        if (ld.IsSDKLogin)
        {
            if (ld.IsLoginStateReady(LoginState.LGState_SDKReady))
            {
                ld.DoLoginStep(LoginSteps.LGS_Platform);
            }else
            {
                WaitPanelShowData waitData = new WaitPanelShowData();
                waitData.type = WaitPanelType.Login;
                waitData.cdTime = 20;
                waitData.des = "SDK授权中...";
                waitData.timeOutDel = RequestTimeOut;
                waitData.useBoxMask = false;
                waitData.showTimer = false;
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonWaitingPanel, data: waitData, panelShowAction: (pb) =>
                {
                    CoroutineMgr.Instance.DelayInvokeMethod(0, () =>
                    {
                        ld.StartAuthorize();
                    }, true);

                });
                
            }
        }else
        {
            ld.DoLoginStep(LoginSteps.None);
        }
    }
    void RequestTimeOut()
    {
        //DataManager.Manager<LoginDataManager>().StateMachine.ChangeState((int)LoginState.None,null);
        
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