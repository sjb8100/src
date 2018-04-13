using System;
using System.Collections.Generic;
using Engine;
using Client;
public class BoxTimeUpManager : IManager
{
    Dictionary<long, uint> m_dicBoxTime = new Dictionary<long, uint>();
    void OnEvent(int nEventid,object param)
    {
        if (nEventid == (int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY)
        {
            Client.stCreateEntity createEntity = (Client.stCreateEntity)param;
            
            Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
            if ( es != null )
            {
                Client.IEntity e = es.FindEntity(createEntity.uid);
                if(e==null)
                {
                    return;
                }
                if (e.GetEntityType() != Client.EntityType.EntityType_Box)
                {
                    return;
                }

                if (!m_dicBoxTime.ContainsKey(createEntity.uid))
                {
                    m_dicBoxTime.Add(createEntity.uid, createEntity.boxTime);
                }
                else
                {
                    Engine.Utility.Log.Error("重复添加box{0}", createEntity.uid);
                }
            }
        }
        else if (nEventid == (int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY)
        {
            Client.stRemoveEntity removeEntiy = (Client.stRemoveEntity)param;
            Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
            if (es != null)
            {
                Client.IEntity e = es.FindEntity(removeEntiy.uid);
                if (e == null)
                {
                    return;
                }
                if (e.GetEntityType() != Client.EntityType.EntityType_Box)
                {
                    return;
                }
                if (m_dicBoxTime.ContainsKey(removeEntiy.uid))
                {
                    m_dicBoxTime.Remove(removeEntiy.uid);
                }
            }
        }
    }

    public void ClearData()
    {

    }
    public void Initialize()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY, OnEvent);
    }

    public void Reset(bool depthClearData = false)
    {
        m_dicBoxTime.Clear();

    }

    public void Process(float deltaTime)
    {
        Dictionary<long, uint>.Enumerator iter = m_dicBoxTime.GetEnumerator();
        while (iter.MoveNext())
        {
            if ((long)iter.Current.Value <= DateTimeHelper.Instance.Now)
            {
                stEntityChangename e = new stEntityChangename();
                e.uid = iter.Current.Key; 
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ENTITYSYSTEM_CHANGENAME, e);
                m_dicBoxTime.Remove(iter.Current.Key);
                break;
            }
        }
    }
}
