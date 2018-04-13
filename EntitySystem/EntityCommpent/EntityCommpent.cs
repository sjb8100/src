using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;

namespace EntitySystem
{
    // 实体组件接口
    public interface IEntityCommpent
    {
        // 获取所有者
        IEntity GetOwner();
        void Update();
        // 消息处理
        object OnMessage(EntityMessage cmd, object param = null);
    }

     // 消息处理函数
    public delegate object MessageDelegate(object param =null);
    class BaseCommpent : IEntityCommpent
    {
        protected Entity m_Owner = null;
        protected Dictionary<int,MessageDelegate>   m_MessageDelegate = new Dictionary<int,MessageDelegate>();

        public IEntity GetOwner()
        {
            return m_Owner;
        }

        protected void RegisterMessageDelegate(EntityMessage msg, MessageDelegate msgProc)
        {
            int key = (int)msg;
            m_MessageDelegate[key] = msgProc;
        }
        public virtual void Update() { 
        
        }
        protected void Clear()
        {
            m_MessageDelegate.Clear();
            //ClearAllEvent();
        }

        public object OnMessage(EntityMessage cmd, object param = null)
        {
//             if (m_MessageDelegate.ContainsKey(cmd))
//             {
//                 return m_MessageDelegate[cmd].Invoke(param);
//             }
            //modify by dianyu  此处key 是枚举 有待优化
            MessageDelegate md = null;
            int key = (int)cmd;
            if(m_MessageDelegate.TryGetValue(key,out md))
            {
                return md.Invoke(param);
            }
            return null;
        }
    }
}
