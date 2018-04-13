
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

class LoginStepPlatform : LoginStateBase
{

    public LoginStepPlatform(Engine.Utility.StateMachine<LoginDataManager> machine, GX.Net.JsonHttp http)
        : base(machine, http)
    {
        m_nStateID = (int)LoginSteps.LGS_Platform;
    }
    public override void Enter(object param)
    {
        Debug.Log("enter LoginStateOneStep  请求区服列表");

        WaitPanelShowData waitData = new WaitPanelShowData();
        waitData.type = WaitPanelType.ArenaChallenge;
        waitData.cdTime = 20;
        waitData.des = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Net_RequestZoneList); ;
        waitData.timeOutDel = RequestTimeOut;
        waitData.useBoxMask = false;
        waitData.showTimer = false;
//        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonWaitingPanel, data: waitData);


        Action callback = (Action)param;
        LoginDataManager ld = DataManager.Manager<LoginDataManager>();
        if (m_http != null)
        {
            m_http.Login((sucess) =>//1-1连接 登录服务器成功
            {
                if (sucess.platinfo == null)
                {
                    ld.OnLogout(true);
                    Debug.LogError("Platform LoginData error,platinfo null");
                    return;
                }
                CommonSDKPlaform.Instance.SetLoginSuccessData(sucess.platinfo.account, sucess.platinfo.sign, sucess.platinfo.extdata);
                Engine.Utility.Log.Error("http login sucess is {0}", sucess);

                m_http.RequestLoginUserInfo((userInfo) =>
                {
                    ld.UserZoneInfoList = userInfo.userzoneinfo;
                    if(userInfo != null)
                    {
                        if(userInfo.userzoneinfo != null)
                        {
                            if(userInfo.userzoneinfo.Count> 0)
                            {
                                Engine.Utility.Log.Error("user info is {0}", userInfo.userzoneinfo[0].zoneinfo.gamename);
                            }
                        }
                    }
                });
                //sort   根据id排序，越靠后越新
                m_http.RequestZoneList((retZoneData) =>
                {
                    ld.BestZone = retZoneData.bestzoneid;
                    List<Pmd.ZoneInfo> tempList = retZoneData.zonelist;
                    tempList.Sort(
                    delegate(Pmd.ZoneInfo a, Pmd.ZoneInfo b)
                    {
                        if (a.zoneid > b.zoneid) return -1;
                        else if (a.zoneid < b.zoneid) return 1;
                        return 0;
                    });
                    ld.ZoneList = tempList;

                    Engine.Utility.Log.Error("http GetZoneList sucess is {0}", retZoneData);
                    ld.SetLoginState(LoginState.LGState_PlatformReady);
                    if (m_Statemachine.GetCurStateID() == (int)LoginSteps.LGS_Platform)
                    {
                        if (callback != null)
                        {
                            callback();
                        }
                        else 
                        {
                            //DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonWaitingPanel, data: waitData);
                        }
                        // 隐藏等待窗体
                        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
                        Action act = null;
                        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ChooseServerPanel))
                        {
                             act = () =>
                            {
                                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ChooseServerPanel);
                            };
                        }
                        

                        ld.GotoLogin(LoginPanel.ShowUIEnum.StartGame, act);
                    }
                    else
                    {
                        TipsManager.Instance.ShowTips("请求超时请重试!!");
                    }
                
                },
                 (error) =>
                 {
                     Engine.Utility.Log.Error("http GetZoneList error is {0}", error);
                 });


            },
            (error) =>
            {
                ld.OnLogout(true);
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
            });

        }
        else
        {
            ld.OnLogout(true);
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
        }
    }
    void RequestTimeOut()
    {
        DataManager.Manager<LoginDataManager>().OnLogout(true);
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
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
