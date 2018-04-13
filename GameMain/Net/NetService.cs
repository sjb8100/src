//*************************************************************************
//	创建日期:	2016-1-18   18:14
//	文件名称:	NetService.cs
//  创 建 人:   Even	
//	版权所有:	Even.xu
//	说    明:	网络服务
//*************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Common.Net;
using Common;
using System.Reflection;
using Engine.Utility;

public class NetService : Client.INetService
{
    private static NetService _inst = null;

    public static NetService Instance
    {
        get
        {
            if (_inst == null)
            {
                _inst = new NetService();
                Client.ClientGlobal.Instance().netService = _inst;
            }

            return _inst;
        }
      
    }

    private NetLinkSink netLinkSink = new NetLinkSink();
    private INetLink netLink = null;
    private INetLinkMonitor netLinkMoniter = new NetLinkMonitor();
    private readonly VarlenProtobufPackageSerializer packageSerializer = new VarlenProtobufPackageSerializer();

    /// <summary>
    /// 消息序列号
    /// </summary>
    private uint msgSeq = 0;
    public uint MsgSeq
    {
        get
        {
            return netLinkSink.GetLastMessageIndex();
        }
    }
    public NetLinkSink CurrentLinkSeek
    {
        get { return netLinkSink; }
    }

    public INetLinkMonitor CurrentMoniter
    {
        get { return netLinkMoniter; }
    }
    /// <summary>
    /// 标志发给后台的第一个消息 用来获取msgseq
    /// </summary>
    bool bFirstMsg = false;
    public bool FirstMsg
    {
        set
        {
            bFirstMsg = value;
        }
    }

    bool bIsCheckingTime = false;//正在对时
    public bool BCheckingTime
    {
        set
        {
            bIsCheckingTime = value;
        }
        get
        {
            return bIsCheckingTime;
        }
    }
    //public VarlenProtobufPackageSerializer PackageSerializer
    //{
    //    get { return packageSerializer; }
    //}

    /// <summary>
    /// 要忽略收发日志的消息
    /// </summary>
    private static readonly HashSet<Type> filter = new HashSet<Type>()
		{
            //typeof(Cmd.NullCmd.t_NullCmd),
            //typeof(Cmd.NullCmd.t_ClientNullCmd),
            //typeof(Cmd.RequestUserCmd.stRequestUserNameRequestUserCmd),
            //typeof(Cmd.RequestUserCmd.stRequestCorpsNameRequestUserCmd),
            //typeof(Cmd.RequestUserCmd.stRequestArmyNameRequestUserCmd),

			typeof(Pmd.ForwardBwNullUserPmd_CS),
			typeof(Pmd.TickRequestNullUserPmd_CS),
			typeof(Pmd.TickReturnNullUserPmd_CS),

            //typeof(GameCmd.stUserMoveDownPosListMoveUserCmd_S),
            //typeof(GameCmd.stUserMoveStopMoveUserCmd_CS),
            //typeof(GameCmd.stUserMoveUpPosListMoveUserCmd_C),
            //typeof(Cmd.PropertyUserCmd.stRefreshDurabilityPropertyUserCmd),
            //typeof(Cmd.PropertyUserCmd.stObtainExpPropertyUserCmd),
            //typeof(Cmd.PropertyUserCmd.stRefreshMoneyPropertyUserCmd),
            //typeof(Cmd.DataUserCmd.stChangeMoveSpeedUserCmd),
            //typeof(Cmd.DataUserCmd.stSetHPDataUserCmd),

            //typeof(Cmd.RequestUserCmd.stSelectSceneEntryRequestUserCmd),

            //typeof(GameCmd.stUserMoveUpMoveUserCmd),
            //typeof(GameCmd.stUserMoveDownMoveUserCmd),
           // typeof(GameCmd.stNpcMoveMoveUserCmd),
            typeof(GameCmd.stUserPingSceneServerPropertyUserCmd_CS),
            typeof(Pmd.SetPingTimeNullUserPmd_CS),
            typeof(GameCmd.stSendUserCurPosMoveUserCmd_S),
            //typeof(GameCmd.stAddMapNpcAndPosMapScreenUserCmd),
            //typeof(GameCmd.stRemoveMapNpcMapScreenUserCmd),
            //typeof(GameCmd.stBatchRemoveNpcMapScreenUserCmd),
            //typeof(GameCmd.stRemoveUserMapScreenUserCmd),
            //typeof(GameCmd.stRefreshDurabilityPropertyUserCmd),
            //typeof(GameCmd.stObtainExpPropertyUserCmd),
            //typeof(GameCmd.stRefreshMoneyPropertyUserCmd),
            //typeof(GameCmd.stChangeMoveSpeedDataUserCmd_S),
            //typeof(GameCmd.stSetHPDataUserCmd_S),
            //typeof(GameCmd.stRequestUserGameTimeTimerUserCmd),
           // typeof(GameCmd.stPrepareUseSkillSkillUserCmd_S),
           // typeof(GameCmd.stMultiAttackUpMagicUserCmd_C),
           // typeof(GameCmd.stMultiAttackDownMagicUserCmd_S),
          //  typeof(GameCmd.stMapDataMapScreenUserCmd),
           // typeof(GameCmd.stAddMapObjectMapScreenUserCmd_S),
            typeof(GameCmd.stRemoveMapObjectMapScreenUserCmd_S),
            typeof(GameCmd.stNpcMoveMoveUserCmd),
            //typeof(GameCmd.stRefreshAchieveDataUserCmd_S),
            typeof(GameCmd. stUserSendDynaStorePropertyUserCmd_S),
            //typeof(GameCmd.stUserMoveMoveUserCmd_C),
           // typeof(GameCmd.stUserMoveMoveUserCmd_S),
           // typeof(GameCmd.stUserStopMoveUserCmd_S),
           //  typeof(GameCmd.stUserStopMoveUserCmd_C),
		};

    public void Init()
    {
        netLink = NetWork.Instance().CreateNetLink(netLinkSink, netLinkMoniter);
        netLinkMoniter.IsOpen = true;
        VarlenProtobufCommandSerializer.Instance().Register(Assembly.GetExecutingAssembly());
    }

    public bool IsDisconnect()
    {
        if (netLink != null)
        {
            if (netLink.GetState() == SCOKET_CONNECT_STATE.CONNECTED)
            {
                return false;
            }
        }
        Engine.Utility.Log.Error("网络断开连接");
        return true;
    }
    // 连接登录服
    public void Connect(string strIP, int nPort, Engine.ConnectCallback callback)
    {
        if (netLink == null || netLinkSink == null)
        {
            Log.Error("NetService:netLink or netLinkSink is null");
            return;
        }

        netLinkSink.connectCallback = callback;
        netLinkSink.netState = NetLinkSink.NetState.NetState_Login;
        netLink.Connect(strIP, nPort);
    }

    //public void ConnectGateWay(string strIP, int nPort, NetLinkSink.ConnectCallback callback)
    //{
    //    if (netLink != null)
    //    {
    //        if (netLinkSink.netState == NetLinkSink.NetState.NetState_Login)
    //        {
    //            netLink.Disconnect();
    //        }
    //        netLink.Connect(strIP, nPort);
    //    }

    //    netLinkSink.connectCallback = callback;
    //    netLinkSink.netState = NetLinkSink.NetState.NetState_Run;
    //}

    public void Close()
    {
        if (netLink != null)
        {
            netLink.Disconnect();
        }
    }
    public void SendCheckTime(GameCmd.stTimeTickMoveUserCmd_C cmd)
    {
        BCheckingTime = true;
        Send(cmd);
    }
    public void Send(ProtoBuf.IExtensible cmd)
    {
        if (netLink == null)
        {
            return;
        }
      
        try
        {
            var package = VarlenProtobufCommandSerializer.Instance().Serialize(cmd);
            var ok = packageSerializer.Write(package,netLink,bFirstMsg);
            if (ok)
            {
                bFirstMsg = false;
                if ((cmd is Pmd.ForwardBwNullUserPmd_CS) == false && LogFilter(cmd))
                {
                    if (Engine.Utility.Log.MaxLogLevel >= Engine.Utility.LogLevel.LogLevel_Group)
                    {
                        Engine.Utility.Log.LogGroup(GameDefine.LogGroup.Net, "<color=green>[SEND]</color>{0}: {1}\n{2}", VarlenProtobufCommandSerializer.Instance()[cmd.GetType()], cmd.GetType().Name, cmd.Dump());
                    }
                }
            }
            else
            {
                if (Engine.Utility.Log.MaxLogLevel >= Engine.Utility.LogLevel.LogLevel_Group)
                {
                    Engine.Utility.Log.LogGroup(GameDefine.LogGroup.Net, "<color=red>[SEND ERROR]</color>{0}: {1}\n{2}", VarlenProtobufCommandSerializer.Instance()[cmd.GetType()], cmd.GetType().Name, cmd.Dump());
                }
            }
        }
        catch (Exception ex)
        {
            packageSerializer.OnError(ex);
        }
    }

    public void SendToMe(ProtoBuf.IExtensible cmd)
    {
      if(netLinkSink != null)
      {
          netLinkSink.SendToMe(cmd);
      }
    }

    public static bool LogFilter(object message)
    {
        return !filter.Contains(message.GetType());
    }

    
}
