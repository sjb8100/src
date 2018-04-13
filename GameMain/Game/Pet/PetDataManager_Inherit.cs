
//*************************************************************************
//	创建日期:	2017/8/10 星期四 19:40:00
//	文件名称:	PetDataManager_Inherit
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using GameCmd;
using Client;

partial class PetDataManager
{
    /// <summary>
    /// key is index  value is petid
    /// </summary>
    public Dictionary<int, uint> m_inhertShowDic = new Dictionary<int, uint>();
    int m_nInheritCostItemNum = 0;
    public int InheritCostItemNum
    {
        get
        {
            return m_nInheritCostItemNum;
        }
        set
        {
            if (value >= 0)
            {
                m_nInheritCostItemNum = value;
            }

        }
    }
    int m_nInhertCostMoney = 0;
    public int InHeritCostMoney
    {
        get
        {
            return m_nInhertCostMoney;
        }
        set
        {
            if(value >= 0)
            {
                m_nInhertCostMoney = value;
            }
        }
    }
    public bool bInheritExp = false;
    public bool bInheritSkill = false;
    public bool bInheritXiuwei = false;

    public void OnInhertPet(stInheritPetUserCmd_CS cmd)
    {
        ResetInheritData();
        ValueUpdateEventArgs eventArgs = new ValueUpdateEventArgs();
        eventArgs.key = PetDispatchEventString.ResetInheritPanel.ToString();
        DispatchValueUpdateEvent(eventArgs);
    }
    public void ResetInheritData()
    {
        InheritCostItemNum = 0;
        InHeritCostMoney = 0;
        bInheritExp = false;
        bInheritSkill = false;
        bInheritXiuwei = false;
    }
    public void AddInhertPet(int index, uint petID)
    {
        if (petID == 0)
        {
            return;
        }
        if (!m_inhertShowDic.ContainsKey(index))
        {
            m_inhertShowDic.Add(index, petID);
        }
    }
    public bool ContainInheritPos(int index)
    {
        return m_inhertShowDic.ContainsKey(index);
    }
    public bool ContainInheritPet(uint petID)
    {
        return m_inhertShowDic.ContainsValue(petID);
    }
    public void RemoveInheritPet(int index)
    {

        if (m_inhertShowDic.ContainsKey(index))
        {
            m_inhertShowDic.Remove(index);
        }
    }
    public IPet GetInheritPet(int index)
    {
        if (m_inhertShowDic.ContainsKey(index))
        {
            uint petID = m_inhertShowDic[index];
            return GetPetByThisID(petID);
        }
        return null;
    }
    public void ClearInheritPet()
    {
        m_inhertShowDic.Clear();
    }


}