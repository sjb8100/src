/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.RoleBar
 * 创建人：  wenjunhua.zqgame
 * 文件名：  RoleStateBarManager1_Event
 * 版本号：  V1.0.0.0
 * 创建时间：7/26/2017 3:30:02 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;

partial class RoleStateBarManager
{
    #region IGlobalEvent
    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            // 注册事件
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CHANGENAME, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_SETHIDE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_NPCHEADSTATUSCHANGED, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSSTART, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSBREAK, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSEND, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANQUIT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANJOIN, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANREFRESHID, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANDeclareInfoRemove, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANDeclareInfoAdd, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TITLE_WEAR, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CITYWARTOTEMCLANNAMECHANGE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CITYWARWINERCLANID, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_CHANGENAME, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_SETHIDE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_NPCHEADSTATUSCHANGED, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSSTART, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSBREAK, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSEND, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANQUIT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANJOIN, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANREFRESHID, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANDeclareInfoRemove, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANDeclareInfoAdd, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TITLE_WEAR, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CITYWARTOTEMCLANNAMECHANGE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CITYWARWINERCLANID, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);
        }
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="nEventID"></param>
    /// <param name="param"></param>
    public void GlobalEventHandler(int eventID, object param)
    {
        switch(eventID)
        {
            case (int)Client.GameEventID.ENTITYSYSTEM_NPCHEADSTATUSCHANGED:
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY:
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY:
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE:
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_CHANGENAME:
                break;
            case (int)Client.GameEventID.TITLE_WEAR:
                break;
            case (int)Client.GameEventID.SKILLGUIDE_PROGRESSSTART:
                break;
            case (int)Client.GameEventID.SKILLGUIDE_PROGRESSBREAK:
                break;
            case (int)Client.GameEventID.SKILLGUIDE_PROGRESSEND:
                break;
            case (int)Client.GameEventID.CLANQUIT:
            case (int)Client.GameEventID.CLANJOIN:
            case (int)Client.GameEventID.CLANREFRESHID:
            case (int)Client.GameEventID.CITYWARWINERCLANID:
            case (int)Client.GameEventID.CLANDeclareInfoAdd:
            case (int)Client.GameEventID.CLANDeclareInfoRemove:
                {

                }
                break;
            case (int)Client.GameEventID.CITYWARTOTEMCLANNAMECHANGE:
                break;
            case (int)Client.GameEventID.SYSTEM_GAME_READY:
                break;
        }
    }
    #endregion
}