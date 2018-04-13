using UnityEngine;
using System.Collections.Generic;

public class KObjectIndex<T>
{
    private uint m_dwNextID = 0;
    public Dictionary<uint, T> m_ObjectIndex = new Dictionary<uint, T>();

    public KObjectIndex()
    {
        m_dwNextID = 1;
    }
    public void SetPrefix(uint dwPrefix)
    {
        m_dwNextID = (uint)(dwPrefix << (sizeof(uint) * 8 - 2)) + 1;
    }
    public int GetCount()
    {
        return m_ObjectIndex.Count;
    }
    public int Register(T pObject, uint dwID)
    {
        if (dwID == 0)
        {
            dwID = m_dwNextID++;
        }

        if (pObject != null)
        {
            KBaseObject pBaseObject = pObject as KBaseObject;

            pBaseObject.m_dwID = dwID;

            m_ObjectIndex.Add(dwID, pObject);

            return 1;
        }
        return 0;
    }

    public void Unregister(T pObject)
    {
        if (pObject != null)
        {
            KBaseObject pBaseObject = pObject as KBaseObject;
            m_ObjectIndex.Remove(pBaseObject.m_dwID);
            pBaseObject.m_dwID = 0;
        }
    }

    public T GetObj(uint dwID)
    {
        if (m_ObjectIndex.ContainsKey(dwID))
        {
            return m_ObjectIndex[dwID];
        }
        ///对于对象类型返回null,数值类型返回0
        return default(T);
    }
}
