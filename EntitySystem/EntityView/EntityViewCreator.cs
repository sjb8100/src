using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntitySystem
{
    class EntityViewCreator : Singleton<EntityViewCreator>, Engine.Utility.ITimer
    {
        private static uint ENTITY_CREATOR_TIME = 0;
        // 实体视图信息
        struct EntityViewInfo
        {
            public long id;
            public EntityView   m_View;
            public string m_strResName;
            public Engine.CreateRenderObjEvent callback;
        }

        private Dictionary<long, EntityViewInfo> m_dicViewData = new Dictionary<long, EntityViewInfo>();

        public void Init()
        {
            //Engine.Utility.Log.Error("Add Timer EntityViewCreator!!!");
            Engine.Utility.TimerAxis.Instance().SetTimer(EntityViewCreator.ENTITY_CREATOR_TIME, 300, this, Engine.Utility.TimerAxis.INFINITY_CALL, "EntityViewCreator::Init");
        }

        public void Close()
        {
            //Engine.Utility.Log.Error("Remove Timer EntityViewCreator!!!");
            Engine.Utility.TimerAxis.Instance().KillTimer(EntityViewCreator.ENTITY_CREATOR_TIME, this);
        }

        public void AddView(long id, EntityView view, ref string strResName, Engine.CreateRenderObjEvent callback)
        {
            if(m_dicViewData.ContainsKey(id))
            {
                return;
            }

            EntityViewInfo info = new EntityViewInfo();
            info.m_View = view;
            info.m_strResName = strResName;
            info.callback = callback;
            info.id = id;

            m_dicViewData.Add(id, info);
        }

        public void RemoveView(long id)
        {
            m_dicViewData.Remove(id);
        }

        public void OnTimer(uint uTimerID)
        {
            Dictionary<long, EntityViewInfo>.Enumerator iter = m_dicViewData.GetEnumerator();
            while (iter.MoveNext())
            {
                if(iter.Current.Value.m_View!=null)
                {
                    string strRes = iter.Current.Value.m_strResName;
                    iter.Current.Value.m_View.Create(ref strRes, iter.Current.Value.callback);
                }

                m_dicViewData.Remove(iter.Current.Value.id);
                break;
            }
        }
    }
}
