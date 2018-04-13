/************************************************************************************
 * Copyright (c) 2018  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Login.LoginFSM
 * 创建人：  wenjunhua.zqgame
 * 文件名：  LoginStepFetchAreaServerFilterData
 * 版本号：  V1.0.0.0
 * 创建时间：2/24/2018 9:39:52 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class LoginStepFetchASFilterData : LoginStateBase
{
    public LoginStepFetchASFilterData(Engine.Utility.StateMachine<LoginDataManager> machine, GX.Net.JsonHttp http)
        : base(machine, http)
    {
        m_nStateID = (int)LoginSteps.LGS_FetchASFilterData;
    }

    public override void Enter(object param)
    {
        LoginDataManager ld = DataManager.Manager<LoginDataManager>();

        WaitPanelShowData waitData = new WaitPanelShowData();
        waitData.type = WaitPanelType.Waitting;
        waitData.cdTime = 20;
        waitData.des = "获取区服配置中...";
        waitData.timeOutDel = RequestTimeOut;
        waitData.useBoxMask = false;
        waitData.showTimer = false;

        Action<bool, string> fetchCallback = (success, errorMsg) =>
            {
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
                if (success)
                {
                    //成功开始登陆平台
                    ld.DoLoginPlatform();
                }else 
                {
                    TipsManager.Instance.ShowTips("获取区服配置数据失败");
                    if (!string.IsNullOrEmpty(errorMsg))
                    {
                        Debug.LogError("LoginStepFetchASFilterData failed error:" + errorMsg);
                    }
                    //失败，重试
                    RetryFetchAreaSeverFilterData();
                }
            };

        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonWaitingPanel, data: waitData,panelShowAction:(pb) =>
            {
                ld.FetchAreaSeverFilterData(fetchCallback);
            });
        
        
    }

    /// <summary>
    /// 重新拉取提示
    /// </summary>
    void RetryFetchAreaSeverFilterData()
    {
        TipsManager.Instance.ShowTipWindow(Client.TipWindowType.CancelOk, "获取区服配置失败，是否重试？", () =>
        {
            //重新拉取
            DataManager.Manager<LoginDataManager>().DoLoginStep(LoginSteps.LGS_FetchASFilterData);
        },
             () =>
             {
                 UnityEngine.Application.Quit();
             },
             () =>
             {
                 UnityEngine.Application.Quit();
             }, "提示", "重试", "退出");
    }

    /// <summary>
    /// 拉取超时
    /// </summary>
    void RequestTimeOut()
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
        RetryFetchAreaSeverFilterData();
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