using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Common.Net;
using Common;
using Cmd.NullCmd;
using Engine.Utility;
using System.Collections;
using UnityEngine;
public class NetLinkSink : Engine.INetLinkSink
 {
    private readonly MessageDispatcher<ISerializable> dispatcherRaw = new MessageDispatcher<ISerializable>();
    private readonly ConcurrentQueue<ISerializable> receivedQueueRaw = new ConcurrentQueue<ISerializable>();
    private readonly MessageDispatcher<ProtoBuf.IExtensible> dispatcherProtobuf = new MessageDispatcher<ProtoBuf.IExtensible>();
    //private readonly ConcurrentQueue<ProtoBuf.IExtensible> receivedQueueProtobuf = new ConcurrentQueue<ProtoBuf.IExtensible>();

    private readonly Queue<ProtoBuf.IExtensible> receivedQueueMessage = new Queue<ProtoBuf.IExtensible>();
    //标识重连是否走登录流程
    bool isReConnectByLogin = false;

    // 网络解析
    private List<Engine.PackageIn> m_lstPackageIn = new List<Engine.PackageIn>();
    private ObjPool<Engine.PackageIn> PackageInPool = new ObjPool<Engine.PackageIn>();

    public NetLinkSink()
    {
       // dispatcherProtobuf.StaticRegister();
        dispatcherProtobuf.Register(Protocol.Instance);
    }
    public enum NetState
    {
        NetState_Login = 1, // 登录
        NetState_Run,       // 运行
    }

    public NetState netState
    {
        get;
        set;
    }

    ConnectCallback m_connectCallback = null;
    public ConnectCallback connectCallback
    {
        set { m_connectCallback = value; }
    }
    uint msgIndex = 0;
    public uint GetLastMessageIndex()
    {
        return msgIndex;
    }
    // 连接断开
    public void OnDisConnect()
    {
        //Engine.Utility.Log.Trace("断开网络连接");
        //TipsManager.Instance.ShowTips("断开网络连接!");
        //Protocol.Instance.OnExcuteCloseSocket();
    }

    // 网络关闭
    public void OnClose()
    {
        Engine.Utility.Log.Trace("网络关闭");
    }

    public void OnConnectError(NetWorkError e)
    {
        if (m_connectCallback != null)
        {
            m_connectCallback(e == NetWorkError.NetWorkError_ConnectSuccess);
        }
    }

    // 重新连接
    public void OnReConnected()
    {

    }

    public void OnReceive(Engine.PackageIn msg)
    {
        Pmd.ForwardNullUserPmd_CS package = Pmd.ForwardNullUserPmd_CS.DeserializeLengthDelimited(msg);
        OnPackage(package);
        DispatchMessage();
    }

    // 网络包解析 在网络接收线程中调用，注意API的使用
    public void ParsePackage(byte[] buff, out List<PackageIn> lstPackage)
    {
        lstPackage = m_lstPackageIn;
        Engine.PackageIn msg = new Engine.PackageIn(buff);
        Pmd.ForwardNullUserPmd_CS package = Pmd.ForwardNullUserPmd_CS.Deserialize(msg);
        ParsePackage(package,buff.Length);
        msg.Close();
    }
    // 网络包解析结束 在网络接收线程中调用，注意API的使用
    public void EndParsePackage()
    {
        for(int i = 0; i < m_lstPackageIn.Count;++i)
        {
            m_lstPackageIn[i].Position = 0;
            PackageInPool.Free(m_lstPackageIn[i]);
        }
        m_lstPackageIn.Clear();
    }
    // 包解析
    private void ParsePackage(Pmd.ForwardNullUserPmd_CS package,int buffLen = 0)
    {
        if (package == null)
        {
            return;
        }

        // 消息解压
        if ((package.bitmask & (uint)Pmd.FrameHeader.Bitmask_Compress) != 0)
        {
            package.bitmask &= ~(uint)Pmd.FrameHeader.Bitmask_Compress; // 去掉压缩标记
            package.data = Common.ZlibCodec.Decode(package.data);
        }

        // 嵌套打包
        if ((package.bitmask & (uint)Pmd.FrameHeader.Bitmask_Header) != 0)
        {
            package.bitmask &= ~(uint)Pmd.FrameHeader.Bitmask_Header; // 去掉嵌套打包标记

            using (var mem = new MemoryStream(package.data))
            {
                while (mem.Position < mem.Length)
                {
                    var embed = Pmd.ForwardNullUserPmd_CS.DeserializeLengthDelimited(mem);
                    ParsePackage(embed);
                }
                if (mem.Position >= mem.Length)
                {
                    return;
                }
            }
        }

        Engine.PackageIn pack = PackageInPool.Alloc();
        pack.buffLen = buffLen;
        try
        {
            package.SerializeLengthDelimited(pack);
        }
        catch (System.Exception ex)
        {
            //Engine.Utility.Log.Error("");
            //string str = ex.ToString();
        }
        
        m_lstPackageIn.Add(pack);
    }

    void DispatchMessage()
    {
        ProtoBuf.IExtensible proto = null;

        //if (receivedQueueProtobuf.TryDequeue(out proto))
        //{
        //    if (dispatcherProtobuf.Dispatch(proto) == false)
        //    {
        //        var tag = VarlenProtobufCommandSerializer.Instance().rawCommandSerializer[proto.GetType()];
        //        Engine.Utility.Log.Trace("<color=red>[RECV]</color>未处理的消息: {0} {1}\n{2}", tag, proto.GetType(), proto.Dump());
        //    }
        //}
  
        while (receivedQueueMessage.Count > 0)
        {
            proto = receivedQueueMessage.Dequeue();
            /*
            INetLinkMonitor montitor = NetService.Instance.CurrentMoniter;
            if (montitor != null)
            {
                var dic = montitor.GetMessageData();
                string name = proto.GetType().Name;
                if (dic.ContainsKey(name))
                {
                    uint num = dic[name];
                    dic[name] = num + 1;
                }
                else
                {
                    montitor.SetMontitorMessage(name, 1);
                }
            }
            */
            if (dispatcherProtobuf.Dispatch(proto) == false)
            {
              
                if (Engine.Utility.Log.MaxLogLevel >= Engine.Utility.LogLevel.LogLevel_Group)
                {
                    var tag = VarlenProtobufCommandSerializer.Instance().rawCommandSerializer[proto.GetType()];
                    Engine.Utility.Log.Trace("<color=red>[RECV]</color>未处理的消息: {0} {1}\n{2}", tag, proto.GetType(), proto.Dump());
                }
            }
        }
     
    }

    private void OnPackage(Pmd.ForwardNullUserPmd_CS package)
    {

        if (package == null)
        {
            return;
        }

        // 网络解析包
        PushCmd(package);
   
    }
    public void SendToMe(ProtoBuf.IExtensible message)
    {
        PushCmd(message);
    }
    private void PushCmd(ProtoBuf.IExtensible message)
    {
        if (message == null)
            return;

        //老协议，转换
        var raw = message as Pmd.ForwardBwNullUserPmd_CS;
        if (raw != null)
        {
            Execute(raw);
            return;
        }


        var rawNull = message as Pmd.ForwardNullUserPmd_CS;
        if (rawNull != null)
        {
            Execute(rawNull);
            return;
        }
        if (Protocol.Instance.IsReconnecting)
        {
            if (Engine.Utility.Log.MaxLogLevel >= Engine.Utility.LogLevel.LogLevel_Group)
            {
                Log.LogGroup("ZDY", "重连第一个message name " + message.GetType().Name);
            }
            if (message.GetType().Namespace.Equals("GameCmd"))
            {
                if (message is GameCmd.stLoginStepSelectUserCmd)
                {
                    isReConnectByLogin = true;
                    GameCmd.stLoginStepSelectUserCmd cmd = message as GameCmd.stLoginStepSelectUserCmd;
                    if (cmd.step == GameCmd.LoginStep.LOGIN_SCENE)
                    {
                        Log.Error("收到消息是LOGIN_SCENE，走重新登录流程");
                        isReConnectByLogin = false;
                        Protocol.Instance.SetReconnect(false, true);
                    }

                }
            }
            else
            {//非登录流程 任意一个消息代表重连成功
             
                if (!isReConnectByLogin)
                {
                    if (Engine.Utility.Log.MaxLogLevel >= Engine.Utility.LogLevel.LogLevel_Group)
                    {
                        Log.LogGroup("ZDY", "不走重新登录 重连第一个message name " + message.GetType().Name);
                    }
                    Protocol.Instance.SetReconnect(false, false);
                }

            }
       
        }

        //receivedQueueProtobuf.Enqueue(message);
        receivedQueueMessage.Enqueue(message);

        if (NetService.LogFilter(message))
        {
            if(Engine.Utility.Log.MaxLogLevel >= Engine.Utility.LogLevel.LogLevel_Group)
            {
                Engine.Utility.Log.LogGroup(GameDefine.LogGroup.Net, "<color=yellow>[RECV]</color>{0}: {1}\n{2}", VarlenProtobufCommandSerializer.Instance()[message.GetType()], message.GetType().Name, message.Dump());
            }
            // [RECV]的网络接收日志
        }
    }

    private void Execute(Pmd.ForwardNullUserPmd_CS cmd)
    {
        var raw = VarlenProtobufCommandSerializer.Instance().Deserialize(cmd);
        msgIndex = cmd.seq;
        PushCmd(raw);
    }

    public void Execute(Pmd.ForwardBwNullUserPmd_CS cmd)
    {
        try
        {
            ISerializable msg = VarlenProtobufCommandSerializer.Instance().Deserialize(cmd);
            PushCmd(msg);
        }
        catch (System.Exception ex)
        {
            Engine.Utility.Log.Error("解析网络消息出错！");
        }


    }

    public void PushCmd(ISerializable message)
    {
        if (message == null)
            return;

        var batch = message as t_ZipCmdPackNullCmd;
        if (batch != null)
        {
            foreach (var c in batch.Parse(VarlenProtobufCommandSerializer.Instance().rawCommandSerializer))
                PushCmd(c);
            return;
        }
        receivedQueueRaw.Enqueue(message);
        // [RECV]的网络接收日志
       // Engine.Utility.Log.Trace("old msg <color=yellow>[RECV]</color>{0}: {1}\n{2}", VarlenProtobufCommandSerializer.Instance().rawCommandSerializer[message.GetType()], message.GetType().Name, message.Dump());
    }

    ///// <summary>
    ///// 解析剥离<see cref="Pmd.ForwardNullUserPmd_CS"/>的压缩、多次打包等修饰，得到独立的可直接解析的原生消息包
    ///// </summary>
    ///// <param name="package"></param>
    ///// <returns></returns>
    //public static IEnumerable<Pmd.ForwardNullUserPmd_CS> Parse(Pmd.ForwardNullUserPmd_CS package)
    //{
    //    if (package == null)
    //        yield break;

    //    // 消息解压
    //    if ((package.bitmask & (uint)Pmd.FrameHeader.Bitmask_Compress) != 0)
    //    {
    //        package.bitmask &= ~(uint)Pmd.FrameHeader.Bitmask_Compress; // 去掉压缩标记
    //        package.data = Common.ZlibCodec.Decode(package.data);
    //    }

    //    // 嵌套打包
    //    if ((package.bitmask & (uint)Pmd.FrameHeader.Bitmask_Header) != 0)
    //    {
    //        package.bitmask &= ~(uint)Pmd.FrameHeader.Bitmask_Header; // 去掉嵌套打包标记

    //        using (var mem = new MemoryStream(package.data))
    //        {
    //            while (mem.Position < mem.Length)
    //            {
    //                var embed = ProtoBuf.Serializer.DeserializeWithLengthPrefix<Pmd.ForwardNullUserPmd_CS>(mem, ProtoBuf.PrefixStyle.Base128);
    //                foreach (var e in Parse(embed)) // 递归解析，支持多层打包
    //                    yield return e;
    //            }
    //        }
    //        yield break; // 忽略打包消息本身
    //    }

    //    yield return package;
    //}

    //private void ParsePackage(Pmd.ForwardNullUserPmd_CS package)
    //{
    //    if (package == null)
    //    {
    //        return;
    //    }

    //    // 消息解压
    //    if ((package.bitmask & (uint)Pmd.FrameHeader.Bitmask_Compress) != 0)
    //    {
    //        package.bitmask &= ~(uint)Pmd.FrameHeader.Bitmask_Compress; // 去掉压缩标记
    //        package.data = Common.ZlibCodec.Decode(package.data);
    //    }

    //    // 嵌套打包
    //    if ((package.bitmask & (uint)Pmd.FrameHeader.Bitmask_Header) != 0)
    //    {
    //        package.bitmask &= ~(uint)Pmd.FrameHeader.Bitmask_Header; // 去掉嵌套打包标记

    //        using (var mem = new MemoryStream(package.data))
    //        {
    //            while (mem.Position < mem.Length)
    //            {
    //                var embed = ProtoBuf.Serializer.DeserializeWithLengthPrefix<Pmd.ForwardNullUserPmd_CS>(mem, ProtoBuf.PrefixStyle.Base128);
    //                ParsePackage(embed);
    //            }
    //            if (mem.Position >= mem.Length)
    //            {
    //                return;
    //            }
    //        }
    //    }

    //    PushCmd(package);
    //}
}
