
//*************************************************************************
//	创建日期:	2017/8/8 星期二 17:16:16
//	文件名称:	PetDataManager_Tujian
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using Client;
using Engine.Utility;
using GameCmd;
partial class PetDataManager
{

    bool m_bFirstLineupAutoFight = false;
    public bool bFirstLineUPPetFight
    {
        get
        {
            return m_bFirstLineupAutoFight;
        }
    }
    List<uint> noLineUpList = new List<uint>(8);
    /// <summary>
    /// 获取没有上阵的宠物列表
    /// </summary>
    /// <returns></returns>
    public List<uint> GetNOLineUPPetList()
    {
        noLineUpList.Clear();
        for (int j = 0; j < GetSortPetList().Count; j++)
        {
            IPet pet = GetSortPetList()[j];
            if (pet != null)
            {
                uint id = pet.GetID();
                bool bContain = false;
                for (int i = 0; i < m_petQuickList.Count; i++)
                {
                    uint petID = m_petQuickList[i];
                    if (petID == id)
                    {
                        bContain = true;
                        break;
                    }
                }
                if (!bContain)
                {
                    noLineUpList.Add(id);
                }
            }
        }

        return noLineUpList;
    }

    #region quicksetting
    /// <summary>
    /// 用户设置的列表 还为发送给后台的
    /// </summary>
    SortedDictionary<int, uint> m_dicUserQuickSetting = new SortedDictionary<int, uint>();
    public bool SetUserQuickList(uint petid)
    {
        int tempIndex = -1;
        foreach (var dic in m_dicUserQuickSetting)
        {
            if (dic.Value == 0)
            {
                tempIndex = dic.Key;
                break;
            }
        }
        if (tempIndex != -1)
        {

            bool bSet = SetUserQuickListByIndex(tempIndex, petid);
            //没有发给服务器 不显示设置标志
            //  DispatchValueUpdateEvent( new ValueUpdateEventArgs( PetDispatchEventString.RefreshQuickSetting.ToString() , null , null ) );
            return bSet;
        }
        return false;
    }
    public bool SetUserQuickListByIndex(int index, uint petid)
    {
        if (petid != 0)
        {
            if (m_dicUserQuickSetting.ContainsValue(petid))
            {

                return false;
            }
        }

        if (m_dicUserQuickSetting.ContainsKey(index))
        {
            m_dicUserQuickSetting[index] = petid;
        }
        else
        {
            m_dicUserQuickSetting.Add(index, petid);
        }
        DispatchValueUpdateEvent(new ValueUpdateEventArgs(PetDispatchEventString.RefreshUserQuickSetting.ToString(), null, null));
        return true;
    }


    public uint GetUserQuickSettingByIndex(int index)
    {
        if (m_dicUserQuickSetting.ContainsKey(index))
        {
            return m_dicUserQuickSetting[index];
        }
        else
        {
            Log.Error("not contain index is " + index.ToString());
            return 0;
        }
    }
    public List<uint> GetUserQuicSettingList()
    {
        return m_dicUserQuickSetting.Values.ToList<uint>();
    }

    /// <summary>
    /// 发送给服务器设置列表
    /// </summary>
    public void SendQuickListMsg()
    {
        GameCmd.stSetQuickListPetUserCmd_CS cmd = new GameCmd.stSetQuickListPetUserCmd_CS();

        foreach (var id in GetUserQuicSettingList())
        {
            cmd.quick_list.Add(id);
        }
        NetService.Instance.Send(cmd);
    }
    public void SetQuickList(List<uint> settingList)
    {
        m_petQuickList = settingList;
        //刷新界面
        DispatchValueUpdateEvent(new ValueUpdateEventArgs(PetDispatchEventString.RefreshQuickSetting.ToString(), null, null));
    }
    /// <summary>
    /// 下阵
    /// </summary>
    /// <param name="petID"></param>
    public void RemovelineUp(uint petID)
    {
        if(m_petQuickList.Contains(petID))
        {
            m_petQuickList.Remove(petID);
            GameCmd.stSetQuickListPetUserCmd_CS cmd = new GameCmd.stSetQuickListPetUserCmd_CS();
            cmd.quick_list.AddRange( m_petQuickList);
            NetService.Instance.Send(cmd);
        }
    }
    public void ReplaceLineUP(uint srcID,uint destID)
    {
        if(m_petQuickList.Contains(destID))
        {
            int index = m_petQuickList.IndexOf(destID);
            m_petQuickList.Remove(destID);
            m_petQuickList.Insert(index, srcID);
            GameCmd.stSetQuickListPetUserCmd_CS cmd = new GameCmd.stSetQuickListPetUserCmd_CS();
            cmd.quick_list.AddRange(m_petQuickList);
            NetService.Instance.Send(cmd);
        }
    }
    /// <summary>
    /// 上阵
    /// </summary>
    /// <param name="petID"></param>
    public void AddLineUp(uint petID)
    {
        if(petID == 0)
        {
            return;
        }
        if (!m_petQuickList.Contains(petID))
        {
            m_petQuickList.Add(petID);
            GameCmd.stSetQuickListPetUserCmd_CS cmd = new GameCmd.stSetQuickListPetUserCmd_CS();
            cmd.quick_list.AddRange(m_petQuickList);
            NetService.Instance.Send(cmd);
        }
    }
    /// <summary>
    /// 宠物删除时 从上阵列表中移除
    /// </summary>
    /// <param name="petID"></param>
    void RemoveFromLineUPList(uint petID)
    {
        if(m_petQuickList.Contains(petID))
        {
            m_petQuickList.Remove(petID);
        }
    }
    public void OnFirstForceFightPetUser(bool bAutofight)
    {
        m_bFirstLineupAutoFight = bAutofight;
        DispatchValueUpdateEvent(PetDispatchEventString.RefreshLineUPAutoFight.ToString(), null, null);
    }
    #endregion
}
