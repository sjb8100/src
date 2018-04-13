using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Client
{
    public interface INetService
    {
        // 连接登录服
        void Connect(string strIP, int nPort, Engine.ConnectCallback callback);


        void Close();

        void Send(ProtoBuf.IExtensible cmd);

        void SendToMe(ProtoBuf.IExtensible cmd);
        /// <summary>
        /// 发送对时消息
        /// </summary>
        /// <param name="cmd"></param>
        void SendCheckTime(GameCmd.stTimeTickMoveUserCmd_C cmd);

    }
}
