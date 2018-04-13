using System;
using System.Collections.Generic;
using GameCmd;
using table;
using UnityEngine;
using Client;

public enum AchieveDispatchEvents
{
    None,
    GetAchieveReward,
    RefreshAchieveInfo,
}

class AchievementManager : BaseModuleData, IManager
{
    //大类
    private List<AchievementDataBase> parAchievementData = new List<AchievementDataBase>();
    //小类
    public   Dictionary<uint, List<AchievementDataBase>> subAchievementData = new Dictionary<uint, List<AchievementDataBase>>();



    private Dictionary<uint, AchieveData> achievementServerData = new Dictionary<uint, AchieveData>();

    private uint haveAchievePoint = 0;
    public Dictionary<uint, List<AchievementDataBase>> GetAchieveTypeData() 
    {
        Dictionary<uint, Dictionary<uint, AchievementDataBase>> dic = new Dictionary<uint, Dictionary<uint, AchievementDataBase>>();
        List<AchievementDataBase> dataList = GameTableManager.Instance.GetTableList<AchievementDataBase>();
        Dictionary<uint, List<AchievementDataBase>> data = new Dictionary<uint, List<AchievementDataBase>>();
        if (null == dataList)
        {
            Engine.Utility.Log.Error("AchievementDataBase is null");
            return null;
        }
        for (int i = 0; i < parAchievementData.Count; i++)
        {
            if (!dic.ContainsKey(parAchievementData[i].type))
            {
                dic.Add(parAchievementData[i].type, new Dictionary<uint, AchievementDataBase>());
            }
            else
            {
                dic[parAchievementData[i].type] = new Dictionary<uint, AchievementDataBase>();
            }
        }
        foreach(var par in dic )
        {
            for(int a = 0 ; a < dataList.Count;a ++)
            {
                if(dataList[a].type == par.Key && !par.Value.ContainsKey(dataList[a].childType))
                {
                   par.Value.Add(dataList[a].childType,dataList[a]);
                   if (data.ContainsKey(par.Key))
                   {
                       data[par.Key].Add(dataList[a]);
                   }
                   else
                   {
                       List<AchievementDataBase> list = new List<AchievementDataBase>();
                       list.Add(dataList[a]);
                       data.Add(par.Key, list);

                   }
                }
            }
        }
        data[0] = new List<AchievementDataBase>();
        return data;

    }
    #region IManager
    public void Initialize()
    {
        InitTableData();
    }

    public void Reset(bool depthClearData = false)
    {
//         parAchievementData.Clear();
//         achievementServerData.Clear();

    }

    public void Process(float deltaTime)
    {

    }
    public void ClearData()
    {

    }
    #endregion

    public void InitTableData()
    {
        List<AchievementDataBase> dataList = GameTableManager.Instance.GetTableList<AchievementDataBase>();
        if (null == dataList)
        {
            Engine.Utility.Log.Error("AchievementDataBase is null");
            return;
        }
        for (int i = 0; i < dataList.Count; i++)
        {
            AchievementDataBase item = dataList[i];
            if (item != null)
            {
                uint type = item.type;
                if (!subAchievementData.ContainsKey(type))
                {
                    if(item.whetherShow == 1)
                    {
                        parAchievementData.Add(item);
                    }
                }
                List<AchievementDataBase> list = null;
                if (subAchievementData.TryGetValue(type, out list))
                {
                    if (list != null)
                    {
                        list.Add(item);
                    }
                }
                else
                {
                    list = new List<AchievementDataBase> { item };
                    subAchievementData.Add(type, list);
                }
            }
        }
        parAchievementData.Sort(CompareAchievementData);
    }

    int CompareAchievementData(AchievementDataBase a, AchievementDataBase b)
    {
        return (int)a.type - (int)b.type;
    }

    public AchievementDataBase GetParentAchievementData(int index)
    {
        
        if (parAchievementData != null && parAchievementData.Count > index)
        {
            return parAchievementData[index];
        }
        return null;
    }
    public List<AchievementDataBase> GetSubAchievementDataList(uint type)
    {
        return subAchievementData.ContainsKey(type) ? subAchievementData[type] : null;
    }

    public AchievementDataBase GetAchievementTableData(uint id)
    {
        return GameTableManager.Instance.GetTableList<AchievementDataBase>().Query(id);
    }

    public int GetParentDataCount()
    {
        return parAchievementData != null ? parAchievementData.Count : 0;
    }

    public int GetSubDataCount(uint type)
    {
        return subAchievementData.ContainsKey(type) ? subAchievementData[type].Count : 0;
    }

    /// <summary>
    /// 收到服务器下发的成就数据
    /// </summary>
    /// <param name="cmd"></param>
    public void OnResponAchieveData(stAchieveDataDataUserCmd_S cmd)
    {
        achievementServerData.Clear();
        if (cmd != null)
        {
            for (int i = 0; i < cmd.data.Count; i++)
            {
                achievementServerData.Add(cmd.data[i].id, cmd.data[i]);
                if (cmd.data[i].status == (uint)AchieveStatus.AchieveStatus_HaveReceive)
                {
                    AchievementDataBase table = GameTableManager.Instance.GetTableItem<AchievementDataBase>(cmd.data[i].id);
                    if (table != null)
                    {
                        haveAchievePoint += table.get_point;
                    }
                }
            }
        }
//         if (HaveCanReceiveAchieve())
//         {
//             Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.REFRESHACHIEVEMENTPUSH, null);
//         }   
       DispatchValueUpdateEvent(new ValueUpdateEventArgs(AchieveDispatchEvents.RefreshAchieveInfo.ToString(), null, null));
    }

    /// <summary>
    /// 获取指定成就数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public AchieveData GetAchieveData(uint id)
    {
        return achievementServerData.ContainsKey(id) ? achievementServerData[id] : null;
    }

    /// <summary>
    /// 获取对应类型已获得的成就数量
    /// </summary>
    /// <param name="type"></param>
    /// <param name="haveNum"></param>
    /// <param name="totalNum"></param>
    public void GetGainAchieveByType(uint type, out int haveNum, out int totalNum)
    {
        if (!subAchievementData.ContainsKey(type))
        {
            haveNum = 0;
            totalNum = 0;
            return;
        }

        haveNum = 0;
        List<AchievementDataBase> list = subAchievementData[type];
        totalNum = list.Count;
        for (int i = 0; i < list.Count; i++)
        {
            AchieveData achieveData = null;
            if (achievementServerData.TryGetValue(list[i].id, out achieveData))
            {
                if (achieveData != null && achieveData.status > 0)
                {
                    haveNum++;
                }
            }
        }
    }

    public void GetGainAchieveNum(out int haveNum, out int totalNum)
    {
        haveNum = 0;
        totalNum = 0;
        foreach (var item in subAchievementData)
        {
            List<AchievementDataBase> list = item.Value;
            totalNum += list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                AchieveData achieveData = null;
                if (achievementServerData.TryGetValue(list[i].id, out achieveData))
                {
                    if (achieveData != null && achieveData.status > 0)
                    {
                        haveNum++;
                    }
                }
            }
        }
    }
    public void GetGainAchieveDot(out uint haveDot, out uint totalDot) 
    {
        haveDot = 0;
        totalDot = 0;
        List<AchievementDataBase> list = GameTableManager.Instance.GetTableList<AchievementDataBase>();
        for (int i = 0; i < list.Count; i++)
        {
            totalDot += list[i].get_point;
        }
        haveDot = haveAchievePoint;
    }

    /// <summary>
    /// 领取成就奖励
    /// </summary>
    /// <param name="id"></param>
    public void ReqGetAchieveReward(uint id)
    {
        stGetAchieveRewardDataUserCmd_CS cmd = new stGetAchieveRewardDataUserCmd_CS();
        cmd.id.Add(id);
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 领取奖励成功
    /// </summary>
    /// <param name="cmd"></param>
    public void OnResponGetAchieveReward(stGetAchieveRewardDataUserCmd_CS cmd)
    {
        if (cmd != null)
        {
            for (int i = 0; i < cmd.id.Count; i++)
            {
                uint id = cmd.id[i];
                if (achievementServerData.ContainsKey(id))
                {
                    achievementServerData[id].status = (uint)AchieveStatus.AchieveStatus_HaveReceive;
                    table.AchievementDataBase table = GameTableManager.Instance.GetTableItem<table.AchievementDataBase>(id);
                    if (table == null)
                    {
                        Engine.Utility.Log.Error("成就表格找不到对应ID为{0}的数据", id);
                    }
                    else 
                    {
                        haveAchievePoint += table.get_point;
                        if (table.title != 0)
                        {
                            TitleDataBase title = GameTableManager.Instance.GetTableItem<TitleDataBase>(table.title);
                            TipsManager.Instance.ShowTips(DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_TXT_Notice_GetTitle, title.strName));
                        }
                    }
                   
                }
            }
        }
        if (HaveCanReceiveAchieve())
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.REFRESHACHIEVEMENTPUSH, null);
            stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.Achievement,
                direction = (int)WarningDirection.Left,
                bShowRed = true,
            };
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);       
        }

        DispatchValueUpdateEvent(new ValueUpdateEventArgs(AchieveDispatchEvents.GetAchieveReward.ToString(), null, null));    
    }
    /// <summary>
    /// 刷新成就数据
    /// </summary>
    /// <param name="cmd"></param>
    public void OnResponRefreshAchieveData(List<AchieveData> data, uint achieve_num)
    {
        if (data != null)
        {
            for (int i = 0; i < data.Count; i++)
            {
                uint id = data[i].id;
                if (achievementServerData.ContainsKey(id))
                {
                    achievementServerData[id].progress = data[i].progress;
                    achievementServerData[id].status = data[i].status;
                }
                else
                {
                    achievementServerData.Add(id, data[i]);
                }

                if (data[i].status == (uint)AchieveStatus.AchieveStatus_CanReceive)
                {
                     table.AchievementDataBase table = GameTableManager.Instance.GetTableItem<table.AchievementDataBase>(id);
                     if (table != null)
                     {
                         TipsManager.Instance.ShowTips(table.tips);
                         DataManager.Manager<ChatDataManager>().PrivateChatManager.AddChat(new GameCmd.stCommonMChatUserCmd_CS()
                         {
                             szInfo = table.tips,
                             byChatType = CHATTYPE.CHAT_SYS,
                             dwOPDes = 0,
                             szOPDes = "系统",
                             timestamp = (uint)DateTimeHelper.Instance.Now,
                         });
                     }
                }
                else if (data[i].status == (uint)AchieveStatus.AchieveStatus_HaveReceive)
                {
                     table.AchievementDataBase table = GameTableManager.Instance.GetTableItem<table.AchievementDataBase>(id);
                     if (table != null)
                     {
                         haveAchievePoint += table.get_point;
                     }
                }
            }
        }
        if (HaveCanReceiveAchieve())
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.REFRESHACHIEVEMENTPUSH, null);
            stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.Achievement,
                direction = (int)WarningDirection.Left,
                bShowRed = true,
            };
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
        }
        DispatchValueUpdateEvent(new ValueUpdateEventArgs(AchieveDispatchEvents.RefreshAchieveInfo.ToString(), null, null));
    }

    /// <summary>
    /// 一键获取
    /// </summary>
    public void ReqGetAllAchieve()
    {
        stGetAchieveRewardDataUserCmd_CS cmd = null;
        foreach (var item in achievementServerData)
        {
            if (item.Value.status == (uint)AchieveStatus.AchieveStatus_CanReceive)
            {
                if (cmd == null)
                {
                    cmd = new stGetAchieveRewardDataUserCmd_CS();
                }
                cmd.id.Add(item.Value.id);
            }
        }
        if (cmd != null)
        {
            NetService.Instance.Send(cmd);
        }
    }

    /// <summary>
    /// 是否有可以领取的成就
    /// </summary>
    public bool HaveCanReceiveAchieve()
    {
        Dictionary<uint, AchieveData>.Enumerator iter = achievementServerData.GetEnumerator();
        bool matched = false;
        while (iter.MoveNext())
        {
            if (iter.Current.Value.status == (uint)AchieveStatus.AchieveStatus_CanReceive)
            {
                matched = true;                        
            }
        }
        return matched;
    }
    public bool HaveCanReceiveAchieveByType(uint type)
    {
        Dictionary<uint, AchieveData>.Enumerator iter = achievementServerData.GetEnumerator();
        while (iter.MoveNext())
        {
            AchievementDataBase achievementData = GetAchievementTableData(iter.Current.Key);
            if (achievementData != null && achievementData.type == type)
            {
                if (iter.Current.Value.status == (uint)AchieveStatus.AchieveStatus_CanReceive)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public PushItem GetAchieveStatus() 
    {    
        uint achieve_id = 0;
        PushItem pushItem = null;
        string strName = "";
        Dictionary<uint, AchieveData>.Enumerator iter = achievementServerData.GetEnumerator();
        while (iter.MoveNext())
        {
            AchievementDataBase achievementData = GetAchievementTableData(iter.Current.Key);
            if (achievementData != null && iter.Current.Value.status == (uint)AchieveStatus.AchieveStatus_CanReceive)
            {
                achieve_id = iter.Current.Key;
                strName = achievementData.name;
            }
        }
        if (achieve_id != 0)
        {
            pushItem = new PushItem()
            {
                 pushType = PushType.Achievement,
                 strName = strName,
                 thisID = achieve_id,
            };
        }
        return pushItem;
    }
    public bool HaveCanRecieveByChildType(int tabId, uint childType)
    {
        bool value = false;
        List<uint> list = new List<uint>();
        Dictionary<uint, AchieveData>.Enumerator iter = achievementServerData.GetEnumerator();
        while (iter.MoveNext())
        {
            AchievementDataBase achievementData = GetAchievementTableData(iter.Current.Key);
            if (achievementData != null && achievementData.type == tabId && achievementData.childType == childType)
            {
                if (iter.Current.Value.status == (uint)AchieveStatus.AchieveStatus_CanReceive)
                {
                    list.Add(iter.Current.Key);
                }
            }
        }
        if (list.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
}
