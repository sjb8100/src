using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Common;
using UnityEngine;
using UnityEngine.Profiling;

namespace Common.Net
{
	public class VarlenProtobufCommandSerializer : IEnumerable<KeyValuePair<Type, NetMessageType>>
	{
        public class NetMessageTypeComparer : IEqualityComparer<NetMessageType>
        {
            public bool Equals(NetMessageType x, NetMessageType y)
            {
                return x == y;
            }
            public int GetHashCode(NetMessageType obj)
            {
                return obj.GetHashCode();
            }
        }
        private static VarlenProtobufCommandSerializer _inst = null;
        public static VarlenProtobufCommandSerializer Instance()
        {
            if(_inst == null)
            {
                _inst = new VarlenProtobufCommandSerializer();
            }

            return _inst;
        }

		private  Dictionary<Type, NetMessageType> tableType2ID = new Dictionary<Type, NetMessageType>();
      //  private Dictionary<NetMessageType, Type> tableID2Type = new Dictionary<NetMessageType, Type>(new NetMessageTypeComparer());
        private Dictionary<NetMessageType, ProtoBuf.IMessageOperator> tableID2Type = new Dictionary<NetMessageType, ProtoBuf.IMessageOperator>(new NetMessageTypeComparer());

        public readonly Cmd.CommandSerializer rawCommandSerializer = new Cmd.CommandSerializer();

		public NetMessageType this[Type type]
		{
			get
			{
				NetMessageType id;
				return tableType2ID.TryGetValue(type, out id) ? id : NetMessageType.Empty;
			}
		}

		public ProtoBuf.IMessageOperator this[NetMessageType id]
		{
			get
			{
                ProtoBuf.IMessageOperator ret;
				return tableID2Type.TryGetValue(id, out ret) ? ret : null;
			}
		}

		#region Serialize
        public Pmd.ForwardNullUserPmd_CS Serialize(ProtoBuf.IExtensible message)
		{
			if (message == null)
				return null;
			var type = this[message.GetType()];
			if (type == NetMessageType.Empty)
				return null;
            var package = new Pmd.ForwardNullUserPmd_CS()
			{
				byCmd = type.Cmd,
				byParam = type.Param,
				time = DateTime.Now.ToUnixTime(),
			};
            var msg = message as ProtoBuf.IMessage;
            
				package.data = msg.SerializeToBytes();
			return package;
		}

        public Pmd.ForwardBwNullUserPmd_CS Serialize(ISerializable message)
		{
			if (message == null)
				return null;
            return new Pmd.ForwardBwNullUserPmd_CS()
			{
				data = rawCommandSerializer.SerializeCommand(message),
			};
		}

		#endregion

		#region Deserialize
        NetMessageType netType = new NetMessageType();
        public ProtoBuf.IExtensible Deserialize(Pmd.ForwardNullUserPmd_CS package)
		{
			if (package == null)
				return null;
            netType.Cmd = package.byCmd;
            netType.Param = package.byParam;
			//var type = this[new NetMessageType() { Cmd = package.byCmd, Param = package.byParam }];
       
            var oper = this[netType];
			if (oper == null)
				return null;
			// 消息内容反序列化
          
			if (package.data == null || package.data.Length == 0)
				return oper.New() as ProtoBuf.IExtensible;
      


            return oper.Deserialize(package.data) as ProtoBuf.IExtensible;
		}

        public ISerializable Deserialize(Pmd.ForwardBwNullUserPmd_CS raw)
        {
            if (raw == null)
                return null;
            return rawCommandSerializer.DeserializeCommand(raw.data);
        }

		#endregion

		#region Register
		/// <summary>注册可被解析的消息类型</summary>
		/// <typeparam name="T">可被解析的消息类型ID</typeparam>
		/// <param name="messageTypeID"><typeparamref name="T"/>对应的<see cref="ProtoBuf.IExtensible"/>类型</param>
		public void Register<T>(NetMessageType messageTypeID) where T : ProtoBuf.IExtensible
		{
			// 反序列化预编译(初始化时，影响速度，暂时屏蔽这一句，石雄)
			//ProtoBuf.Serializer.PrepareSerializer<T>();
            
			// 注册
            tableType2ID[typeof(T)] = messageTypeID;
			tableID2Type[messageTypeID] = (ProtoBuf.IMessageOperator)typeof(T).GetField("MessageOperator").GetValue(null);
		}

        Type[] tArray = new Type[] { typeof(NetMessageType) };
		/// <summary>注册可被解析的消息类型</summary>
		/// <param name="messageTypeID">可被解析的消息类型ID</param>
		/// <param name="messageType"><paramref name="messageTypeID"/>对应的<see cref="ProtoBuf.IExtensible"/>类型</param>
		/// <remarks>对泛型重载Register&lt;T&gt;的非泛型包装</remarks>
		public void Register(NetMessageType messageTypeID, Type messageType)
		{
            if (methodRegister == null)
                methodRegister = this.GetType().GetMethod("Register", tArray);//this.GetType().GetRuntimeMethod("Register", typeof(NetMessageType));
			// Call Register<messageType>(messageTypeID) by refelect.
			this.methodRegister
				.MakeGenericMethod(messageType)
				.Invoke(this, new object[] { messageTypeID });
 
		}
		private System.Reflection.MethodInfo methodRegister;

		public void Register(Assembly assembly)
		{
           // SpanUtil.Start();

            foreach (var msg in Parse(typeof(Pmd.PlatCommand)))
                Register(msg.Key, msg.Value);

//SpanUtil.Stop("plat register");

            foreach (var msg in Parse(typeof(GameCmd.ClientCommand)))
                Register(msg.Key, msg.Value);

          //  SpanUtil.Stop("client register");

		//	rawCommandSerializer.Register(assembly);

          //  SpanUtil.Stop("raw register");
		}

		/// <summary>
		/// 解析消息号和消息类型对应表
		/// </summary>
		/// <returns></returns>
		private static IEnumerable<KeyValuePair<NetMessageType, Type>> Parse(Type categoryIdType)
		{
            Assembly assembly = null;//Assembly.GetExecutingAssembly();
            foreach (var assemb in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                //Engine.Utility.Log.Error(assemb.GetName().ToString());
                if (assemb.GetName().ToString().Contains("DataCenter"))
                { 
                    assembly = assemb;
                }
            }
           
			var ret = new Dictionary<NetMessageType, Type>(new NetMessageTypeComparer());
			foreach (var cName in Enum.GetNames(categoryIdType))
			{
				var cValue = Convert.ToByte(Enum.Parse(categoryIdType, cName));
				var pre = categoryIdType.Name + "_";
				var name1 = categoryIdType.Namespace + "." + (cName.StartsWith(pre) ? cName.Substring(pre.Length) : cName) + "+Param";
				var typeIdType = assembly.GetType(name1);
				if (typeIdType == null)
				{
					UnityEngine.Debug.LogWarning("Can't find type by name: " + name1);
					continue;
				}
				foreach (var tName in Enum.GetNames(typeIdType))
				{
					var tValue = Convert.ToByte(Enum.Parse(typeIdType, tName));
					var name2 = categoryIdType.Namespace + "." + tName;
					var messageType = assembly.GetType(name2);
					if (messageType == null)
					{
						UnityEngine.Debug.LogWarning("Can't find type by name: " + name2);
						continue;
					}
                    if ( ret.ContainsKey( new NetMessageType() { Cmd = cValue , Param = tValue } ) )
                    {
                        Type mess = ret[new NetMessageType() { Cmd = cValue , Param = tValue }];
                        Engine.Utility.Log.Error( "重复注册 cmd is  "+ cValue+" param is "+tValue+" 已经注册过的名字 "+mess.ToString()+" 要注册的是 " + tName+" 检查proto号是否重复" );
                    }
                    else
                    {
                        ret.Add( new NetMessageType() { Cmd = cValue , Param = tValue } , messageType );
                    }
					
				}
			}
			return ret;
		}
		#endregion

		public override string ToString()
		{
			var sb = new StringBuilder();
			foreach (var pair in tableType2ID)
			{
				sb.AppendLine(pair.Value + " " + pair.Key);
			}
			return sb.ToString();
		}

		#region IEnumerable<KeyValuePair<Type,NetMessageType>> 成员

		public IEnumerator<KeyValuePair<Type, NetMessageType>> GetEnumerator()
		{
			return tableType2ID.GetEnumerator();
		}

		#endregion

		#region IEnumerable 成员

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
