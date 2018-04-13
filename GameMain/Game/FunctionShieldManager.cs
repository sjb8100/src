//********************************************************************
//	创建日期:	2016-10-19   17:57
//	文件名称:	RankManager.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	排行榜数据管理
//********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;
using GameCmd;
using table;
using Client;


public class FunctionShieldManager : BaseModuleData, IManager
{
    enum ShieldPlat 
    {
       Andriod =1,
       IPhone =2,
    }
    /// <summary>
    /// cdk兑换是否屏蔽
    /// </summary>
    public bool IsCdkClose
    {
        private set;
        get;
    }
    /// <summary>
    /// 手机绑定是否屏蔽
    /// </summary>
    public bool IsPhoneBindClose
    {
        private set;
        get;
    }

    /// <summary>
    /// 问卷功能是否屏蔽
    /// </summary>
    public bool IsQuestionClose
    {
        private set;
        get;
    }
   
    public void Initialize()
    {
//         FunctionShieldDataBase database = null;
//         if (Application.platform == RuntimePlatform.IPhonePlayer)
//         {
//             database = GameTableManager.Instance.GetTableItem<FunctionShieldDataBase>((uint)ShieldPlat.IPhone);
//         }
//         else 
//         {
//             database = GameTableManager.Instance.GetTableItem<FunctionShieldDataBase>((uint)ShieldPlat.Andriod);
//         }
//         if (database != null)
//         {
//             IsCdkClose = database.cdkey == 1;
//             IsPhoneBindClose = database.phoneBind == 1;
//             IsQuestionClose = database.question == 1;
//         }
        RegisterEvent(true);
    }
    /// <summary>
    /// 接收到服务器屏蔽功能的消息
    /// </summary>
    public void OnRecieveServerShied(List<FunctionData> list) 
    {
        bool isIos = false;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            isIos = true;
        }
         List<FunctionState> state = new List<FunctionState>();
        for (int i = 0; i < list.Count;i++ )
        {
            if (isIos)
            {
                if (list[i].apptype == AppType.AppType_Ios)
                {
                    state = list[i].info;
                }
            }
            else
            {
                if (list[i].apptype == AppType.AppType_Android)
                {
                    state = list[i].info;
                }
            }      
        }
        if (state != null)
        {
            for (int i = 0; i < state.Count; i++)
            {
                if (state[i].kind == FunctionType.FunctionType_CDKey)
                {
                    IsCdkClose = state[i].state;
                }
                else if (state[i].kind == FunctionType.FunctionType_PhoneBind)
                {
                    IsPhoneBindClose = state[i].state;
                }
                else if (state[i].kind == FunctionType.FunctionType_Question)
                {
                    IsQuestionClose = state[i].state;
                }
            }
        }     
    }

    void RegisterEvent(bool reg)
    {
        if (reg)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);
        }
    }

    public void GlobalEventHandler(int nEventID, object param)
    {
        switch (nEventID)
        {
            case (int)Client.GameEventID.SYSTEM_GAME_READY:
                {
                    if (IsQuestionClose)
                     {
                         //此平台的问卷已经屏蔽
                         Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.QUESTIONSTATUS, IsQuestionClose);
                     }
                    DataManager.Manager<WelfareManager>().RemoveSpecialKey(IsCdkClose,IsPhoneBindClose);
                }
                break;
        }
    }
    public void Process(float deltime)
    {

    }
    public void Reset(bool depthClearData = false)
    {

    }

    public void ClearData()
    {
        RegisterEvent(false);
    }
}
