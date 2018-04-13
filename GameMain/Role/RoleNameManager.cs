using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NameDataManager : BaseModuleData,IManager
{
    Dictionary<uint, string> m_dictName;
    public void Initialize()
    {
        m_dictName = new Dictionary<uint, string>();
    }
    public void ClearData()
    {

    }
    public void Reset(bool depthClearData = false)
    {
        m_dictName.Clear();
    }

    public void Process(float deltaTime)
    {

    }

    public void AddPlayerName(uint playerid,string name)
    {

    }

    public bool TryGetPlayerName(uint playerid,out string strname)
    {
        return m_dictName.TryGetValue(playerid, out strname);
    }

}
