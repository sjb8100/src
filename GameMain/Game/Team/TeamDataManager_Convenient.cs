using Client;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;

public class ConvenientTeamInfo
{

    public uint active_id;

    public uint byCount;

    public uint byjob;

    public uint byLevel;

    public uint leaderID;

    public List<ConvenientTeamInfoMemData> memDataList;

    public uint teamid;

    public string teamname;

    public uint wdIcon;

    public ConvenientTeamInfo(uint activeId, uint count, uint job, uint level, uint leaderId, List<ConvenientTeamInfoMemData> lstmemdata, uint teamId, string teamName, uint icon)
    {
        this.active_id = activeId;
        this.byCount = count;
        this.byjob = job;
        this.byLevel = level;
        this.leaderID = leaderId;
        this.memDataList = lstmemdata;
        this.teamid = teamId;
        this.teamname = teamName;
        this.wdIcon = icon;
    }
}

public class ConvenientTeamInfoMemData
{
    public uint job { get; set; }
    public uint level { get; set; }
}

partial class TeamDataManager
{
    /// <summary>
    /// 附近的队伍  活动ID为1
    /// </summary>
    public const uint nearbyId = 1;

    /// <summary>
    /// 全部队伍  活动ID为2
    /// </summary>
    public const uint allTeamId = 2;

    /// <summary>
    /// 全部目标（无目标）用于队伍中的活动目标选择
    /// </summary>
    public const uint wuId = 1;

    /// <summary>
    /// 当前选中的活动目标ID  (跟自动匹配的目标ID不一样)
    /// </summary>
    uint m_conveientSelectTargetId = 0;
    public uint ConveientSelectTargetId 
    {
        get 
        {
            return m_conveientSelectTargetId;
        }
    }

    /// <summary>
    /// 当前玩家活动目标ID(自动匹配的活动目标ID)
    /// </summary>
    private uint m_conveientActivityTargetId = 0;

    public uint ConveientActivityTargetId
    {
        get
        {
            return m_conveientActivityTargetId;
        }
    }

    /// <summary>
    /// 从服务器获取的队伍列表
    /// </summary>
    List<ConvenientTeamInfo> m_listConvenientTeam = new List<ConvenientTeamInfo>();

    public List<ConvenientTeamInfo> ConvenientTeamList
    {
        get
        {
            return m_listConvenientTeam;
        }
    }

    //清除便捷组队数据
    void CleanConvenientTeamData()
    {
        m_bIsConvenientTeamMatch = false;    //便捷组队 自动匹配状态取消
        m_conveientSelectTargetId = 0;       //当前选中的目标取消
        m_conveientActivityTargetId = 0;     //自动匹配目标取消
    }

    #region Net

    /// <summary>
    /// 便捷组队创建队伍
    /// </summary>
    /// <param name="activeId"></param>
    public void ReqConveientCreateTeam(uint activeId = 0)
    {
        stCreateTeamRelationUserCmd_C cmd = new stCreateTeamRelationUserCmd_C();
        cmd.active_id = activeId;
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 获取便捷组队队伍列表
    /// </summary>
    /// <param name="copyId">副本ID</param>
    public void ReqConvenientTeamListByCopyId(uint copyId)
    {
        uint activityId = GetTeamActivityIdByCopyId(copyId);
        if (activityId != 0)
        {
            ReqConvenientTeamListByTeamActivityId(activityId);
        }
    }

    /// <summary>
    /// 由组队活动ID获得便捷组队的队伍列表
    /// </summary>
    /// <param name="activeId"></param>
    public void ReqConvenientTeamListByTeamActivityId(uint activeId)
    {
        GameCmd.stGetTeamListRelationUserCmd_C cmd = new GameCmd.stGetTeamListRelationUserCmd_C();
        cmd.active_id = activeId;
        NetService.Instance.Send(cmd);
    }

    //刷新当前队伍消息
    public void ReqUpdateCurrentTeamList(uint id)
    {

    }


    /// <summary>
    /// 获得队伍列表
    /// </summary>
    /// <param name="cmd"></param>
    public void OnConvenientTeamList(GameCmd.stGetTeamListRelationUserCmd_S cmd)
    {
        m_listConvenientTeam.Clear();
        for (int i = 0; i < cmd.data.Count; i++)
        {
            List<ConvenientTeamInfoMemData> memDataList = new List<ConvenientTeamInfoMemData>();

            for (int j = 0; j < cmd.data[i].memData.Count; j++)
            {
                ConvenientTeamInfoMemData memData = new ConvenientTeamInfoMemData { job = cmd.data[i].memData[j].job, level = cmd.data[i].memData[j].level };
                memDataList.Add(memData);
            }

            ConvenientTeamInfo convenientTeamInfo = new ConvenientTeamInfo(cmd.data[i].active_id, cmd.data[i].byCount, cmd.data[i].byjob, cmd.data[i].byLevel, cmd.data[i].leaderID, memDataList, cmd.data[i].teamid, cmd.data[i].teamname, cmd.data[i].wdIcon);

            m_listConvenientTeam.Add(convenientTeamInfo);
        }
       
        this.m_conveientSelectTargetId = cmd.active_id;

        // m_listConvenientTeam = cmd.data;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ConvenientTeamPanel) == false)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ConvenientTeamPanel);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ConvenientTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.ConvenientTeamPanel, UIMsgID.eUpdateExistedTeamList, null);
        }
    }


    #endregion

    /// <summary>
    /// 由副本id  获取组队活动ID
    /// </summary>
    /// <param name="copyId"></param>
    /// <param name="mainId"></param>
    /// <param name="indexId"></param>
    /// <returns></returns>
    uint GetTeamActivityIdByCopyId(uint copyId)
    {
        uint teamActivityId = 0;
        List<CopyDataBase> copyList = GameTableManager.Instance.GetTableList<CopyDataBase>();
        CopyDataBase copyDb = copyList.Find((d) => { return d.copyId == copyId; });
        if (copyDb != null)
        {
            teamActivityId = copyDb.TeamActivityID;
        }

        return teamActivityId;
    }

    /// <summary>
    /// 组队活动dic
    /// </summary>
    /// <returns></returns>
    public Dictionary<uint, List<uint>> GetTeamActivityDic()
    {
        Dictionary<uint, List<uint>> dicTeamActivity = new Dictionary<uint, List<uint>>();

        //无（全部目标）
        List<uint> allTeamIdList = new List<uint>();
        allTeamIdList.Add(wuId);
        dicTeamActivity.Add(wuId, allTeamIdList);

        int lv = ClientGlobal.Instance().MainPlayer.GetProp((int)Client.CreatureProp.Level);
        List<TeamActivityDatabase> activityList = GameTableManager.Instance.GetTableList<TeamActivityDatabase>();

        //等级限制(策划要求去掉等级限制 )
        //activityList = activityList.FindAll((TeamActivityDatabase data) => { return lv >= data.min && lv <= data.max; });

        for (int i = 0; i < activityList.Count; i++)
        {
            List<uint> idList;
            if (dicTeamActivity.TryGetValue(activityList[i].mainID, out idList))
            {
                if (idList.Contains(activityList[i].ID) == false)
                {
                    idList.Add(activityList[i].ID);
                }
            }
            else
            {
                List<uint> list = new List<uint>();
                list.Add(activityList[i].ID);
                dicTeamActivity.Add(activityList[i].mainID, list);
            }
        }

        return dicTeamActivity;
    }


    /// <summary>
    /// 便捷组队活动dic   key为大类ID list为唯一ID
    /// </summary>
    /// <returns></returns>
    public Dictionary<uint, List<uint>> GetConvenientTeamActivityDic()
    {
        Dictionary<uint, List<uint>> dicConvenientTeamActivity = new Dictionary<uint, List<uint>>();

        //附近的队伍
        List<uint> nearbyIdList = new List<uint>();
        dicConvenientTeamActivity.Add(nearbyId, nearbyIdList);

        //全部队伍
        List<uint> allTeamIdList = new List<uint>();
        dicConvenientTeamActivity.Add(allTeamId, allTeamIdList);

        int lv = ClientGlobal.Instance().MainPlayer.GetProp((int)Client.CreatureProp.Level);
        List<TeamActivityDatabase> activityList = GameTableManager.Instance.GetTableList<TeamActivityDatabase>();

        //等级限制(策划要求去掉等级限制 )
        // activityList = activityList.FindAll((TeamActivityDatabase data) => { return lv >= data.min && lv <= data.max; });

        for (int i = 0; i < activityList.Count; i++)
        {
            List<uint> idList;
            if (dicConvenientTeamActivity.TryGetValue(activityList[i].mainID, out idList))
            {
                if (idList.Contains(activityList[i].ID) == false)
                {
                    idList.Add(activityList[i].ID);
                }
            }
            else
            {
                List<uint> list = new List<uint>();
                list.Add(activityList[i].ID);
                dicConvenientTeamActivity.Add(activityList[i].mainID, list);
            }
        }

        return dicConvenientTeamActivity;
    }

    /// <summary>
    /// 获取获得的大类型名
    /// </summary>
    /// <param name="firstTypeId">大类ID </param>
    /// <param name="id">ID  唯一ID</param>
    /// <returns></returns>
    public string GetConvenientTeamActivityFirstTypeName(uint firstTypeId, uint id = 0)
    {
        string typeName = "";

        if (firstTypeId == nearbyId)
        {
            typeName = "附近的队伍";
            return typeName;
        }

        if (firstTypeId == allTeamId)
        {
            typeName = "全部队伍";
            return typeName;
        }

        TeamActivityDatabase teamActivityDb = GameTableManager.Instance.GetTableItem<TeamActivityDatabase>(id);
        if (teamActivityDb != null)
        {
            typeName = teamActivityDb.mainName;
        }

        return typeName;
    }
}
