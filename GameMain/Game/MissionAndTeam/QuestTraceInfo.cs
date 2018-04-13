using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cmd;

public class QuestTraceInfo : System.IComparable<QuestTraceInfo>
{
    private table.QuestDataBase m_clienDB = null;
    public table.QuestDataBase QuestTable
    {
        get
        {
            return m_clienDB;
        }
    }
    //xml格式后的任务名字
    private string m_strXmlName;
    public string StrFormatName
    {
        get
        {
            return m_strXmlName;
        }
    }
    public string StrState
    {
        get
        {
            GameCmd.TaskProcess process = GetTaskProcess();
            if (process == GameCmd.TaskProcess.TaskProcess_None)
            {
                return "[8fffff]可接[-]";
            }
            else if (process == GameCmd.TaskProcess.TaskProcess_CanDone)
            {
                return "[00ff00]完成[-]";
            }
            return "";
        }
    }
    //起始状态
    public uint startStatus
    {
        get
        {
            if (m_clienDB != null)
            {
                return m_clienDB.startStatus;
            }
            return 0;
        }
    }
    public GameCmd.TaskType taskType
    {
        get
        {
            if (m_clienDB != null)
            {
                return (GameCmd.TaskType)m_clienDB.dwType;
            }
            return GameCmd.TaskType.TaskType_None;
        }
    }

    public TaskSubType taskSubType
    {
        get
        {
            if (m_clienDB != null)
            {
                return (TaskSubType)m_clienDB.dwSubType;
            }
            return TaskSubType.Talk;
        }
    }
    public uint beginNpc
    {
        get
        {
            if (m_clienDB != null)
            {
                return m_clienDB.dwBeginNpc;
            }
            return 0;
        }
    }
    public uint doingNpc
    {
        get
        {
            if (m_clienDB != null)
            {
                return m_clienDB.dwDoingNpc;
            }
            return 0;
        }
    }
    public uint endNpc
    {
        get
        {
            if (m_clienDB != null)
            {
                return m_clienDB.dwEndNpc;
            }
            return 0;
        }
    }

    //完成等级
    public uint finishLevel
    {
        get
        {
            if (m_clienDB != null)
            {
                return m_clienDB.finishlevel;
            }
            return 0;
        }
    }
    /// <summary>
    /// 是否动态追踪
    /// </summary>
    public bool dynamicTrance
    {
        get
        {
            if (m_clienDB != null)
            {
                return m_clienDB.dwTranceId != 0;
            }
            return false;
        }
    }
    /// <summary>
    /// 任务详情描述
    /// </summary>
    public string strIntro
    {
        get
        {
            return m_clienDB.dwDesc;
        }
    }
    public uint taskId
    {
        get
        {
            return m_clienDB.dwID;
        }
    }
    //接受任务的地图
    public uint AcceptMapID
    {
        get
        {
            return m_clienDB.acceptMapID;
        }
    }

    public uint SubmitMapID
    {
        get
        {
            return m_clienDB.submitMapID;
        }
    }
    //瞬间跨地图后走路去接取
    public bool HelpAccept
    {
        get
        {
            return m_clienDB.dwHelpGoto;
        }
    }

    /// <summary>
    /// 瞬间跨地图执行
    /// </summary>
    public bool HelpDoing
    {
        get
        {
            return m_clienDB.dwHelpDoing;
        }
    }

    /// <summary>
    /// 瞬间跨地图完成
    /// </summary>
    public bool HlepCommit
    {
        get
        {
            return m_clienDB.dwHelpCommit;
        }
    }

    /// <summary>
    /// 副本Id
    /// </summary>
    public uint copyId
    {
        get
        {
            return m_clienDB.copyId;
        }
    }

    public string PreTask
    {
        get
        {
            return m_clienDB.dwPreTask;
        }
    }

    /// <summary>
    /// 动态道具递交（蚩尤乱世除外）
    /// </summary>
    public bool IsDynamicCommitItem
    {
        get
        {
            return m_clienDB.IsDynamicCommitItem;
        }
    }

    /// <summary>
    /// 需要提交的道具数量
    /// </summary>
    public uint commitItemNum
    {
        get;
        set;
    }

    //动态追踪下来之后强制刷新追踪UI
    public bool dynamicTranceUpdate
    {
        get;
        set;
    }



    //最大环数10
    public const int maxRing = 10;

    public QuestTraceInfo(table.QuestDataBase questdb, uint nState, uint nOperate, bool bReceived)
    {
        m_clienDB = questdb;
        state = nState;
        operate = nOperate;
        this.time = UnityEngine.Time.realtimeSinceStartup;
        Received = bReceived;
        dynamicTranceUpdate = false;
        FormatXmlName();
    }

    string m_strDesc;
    /// <summary>
    /// 任务追踪描述
    /// </summary>
    public string strDesc
    {
        get
        {
            if (NeedProcess(GetTaskProcess()))
            {
                return UpdateFomatProcess(m_strDesc);
            }
            return m_strDesc;
        }
    }

    public bool Received
    {
        get;
        set;
    }
    //任务当前进度
    public uint operate;
    //任务总步骤数
    public uint state;

    public uint exp;
    /// <summary>
    /// 刷新时间
    /// </summary>
    public float time;
    /// <summary>
    /// 文钱
    /// </summary>
    public uint money;
    /// <summary>
    /// 金币
    /// </summary>
    public uint gold;
    //氏族声望
    public uint clanrep;
    //族贡
    public uint clanZG;
    //氏族资金
    public uint clanZJ;
    public List<uint> Items = new List<uint>();
    public List<uint> ItemNum = new List<uint>();
    public GameCmd.TaskProcess GetTaskProcess()
    {
        if (!Received)
        {
            return GameCmd.TaskProcess.TaskProcess_None;
        }
        GameCmd.TaskProcess process = GameCmd.TaskProcess.TaskProcess_None;
        if (state == 0 && operate != 0)
        {
            process = GameCmd.TaskProcess.TaskProcess_Done;
        }
        else if (state != 0 && state <= operate)//步骤总数
        {
            process = GameCmd.TaskProcess.TaskProcess_CanDone;
        }
        else if (state != 0 && state > operate)
        {
            process = GameCmd.TaskProcess.TaskProcess_Doing;
        }
        return process;
    }

    public bool IsChangeBodyTask()
    {
        return taskSubType == TaskSubType.ChangeBody;
    }

    /// <summary>
    /// 移动到固定点使用物品 提交物品等
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool IsMoveToTargetPos(out UnityEngine.Vector2 pos)
    {
        pos = UnityEngine.Vector2.zero;
        if (taskSubType == TaskSubType.UseItem || taskSubType == TaskSubType.CallMonster)
        {
            if (!string.IsNullOrEmpty(m_clienDB.gotoPos))
            {
                string[] strPos = m_clienDB.gotoPos.Split(',');
                if (strPos.Length == 2)
                {
                    pos.x = int.Parse(strPos[0]);
                    pos.y = int.Parse(strPos[1]);
                    return true;
                }
            }

            //环任务，道具由服务器发送
            if (m_clienDB.usecommitItemID != 0 && taskType == GameCmd.TaskType.TaskType_Loop)
            {
                table.QuestItemDataBase questItemDB = GameTableManager.Instance.GetTableItem<table.QuestItemDataBase>(m_clienDB.usecommitItemID);
                if (questItemDB != null && questItemDB.dwSubType == (uint)TaskSubType.UseItem)
                {
                    pos.x = questItemDB.pos_x;
                    pos.y = questItemDB.pos_y;
                    m_clienDB.destMapID = questItemDB.mapId;
                    return true;
                }
            }

        }
        return false;
    }

    public bool IsOpenUI(out PanelID pid, out int index, out int copyID)
    {
        pid = PanelID.None;
        index = 0;
        copyID = 0;
        string strShowUI = "";

        if (!string.IsNullOrEmpty(strShowUI))
        {
            string[] strUI = strShowUI.Split('_');
            if (strUI.Length == 1)
            {
                pid = (PanelID)System.Enum.Parse(typeof(PanelID), strUI[0]);
                index = 1;
                return true;
            }
            if (strUI.Length == 2)
            {
                pid = (PanelID)System.Enum.Parse(typeof(PanelID), strUI[0]);
                index = int.Parse(strUI[1]);
                return true;
            }
            else if (strUI.Length == 3)
            {
                pid = (PanelID)System.Enum.Parse(typeof(PanelID), strUI[0]);
                index = int.Parse(strUI[1]);
                copyID = int.Parse(strUI[2]);
                return true;
            }
        }
        return false;
    }

    public bool IsOpenUI(out uint jumpId)
    {
        jumpId = 0;
        if (GetTaskProcess() == GameCmd.TaskProcess.TaskProcess_Doing || GetTaskProcess() == GameCmd.TaskProcess.TaskProcess_None)
        {
            jumpId = m_clienDB.doingShowUI;
        }
        else if (GetTaskProcess() == GameCmd.TaskProcess.TaskProcess_CanDone)
        {
            jumpId = m_clienDB.commitShowUI;
        }

        if (jumpId == 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    /// <summary>
    /// 动态道具递交（蚩尤乱世除外）
    /// </summary>
    public void DoJump()
    {
        if (taskSubType != TaskSubType.DeliverItem)
        {
            return;
        }

        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: QuestTable.usecommitItemID);
    }

    public bool IsVisitCollectNpc(out uint nNpcid)
    {
        nNpcid = 0;
        if (taskSubType == TaskSubType.Collection)
        {
            nNpcid = m_clienDB.collect_npc;
            return true;
        }
        return false;
    }

    public bool IsKillMonster(out uint nNpcId)
    {
        nNpcId = 0;
        if (taskSubType == TaskSubType.KillMonster || taskSubType == TaskSubType.KillMonsterCollect)
        {
            nNpcId = m_clienDB.monster_npc;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 是否直接跳副本
    /// </summary>
    /// <param name="copyId"></param>
    /// <returns></returns>
    public bool IsDirectlyVisitCopy(uint copyId)
    {
        if (copyId != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsDeleverItem(out uint nNpcId)
    {
        nNpcId = 0;
        if (taskSubType == TaskSubType.DeliverItem)
        {
            GameCmd.TaskProcess process = GetTaskProcess();
            if (process == GameCmd.TaskProcess.TaskProcess_Doing)
            {
                nNpcId = m_clienDB.dwDoingNpc;
            }
            else if (process == GameCmd.TaskProcess.TaskProcess_CanDone)
            {
                nNpcId = m_clienDB.dwEndNpc;
            }
            return true;
        }
        return false;
    }
    /// <summary>
    //第一次接取 调用  可以提交调用下 
    /// </summary>
    public void UpdateDesc()
    {
        GameCmd.TaskProcess process = GetTaskProcess();

        m_strDesc = GetTranceDescByProcess(process);
        m_strDesc = ReplaceTag(m_strDesc, process);
        //m_strDesc = string.Format("<br /><n><color value=\"#fff8dd\">{0}</color></n>", m_strDesc);
        m_strDesc = string.Format("[fff8dd]{0}[-]", m_strDesc);

        if (NeedProcess(process))
        {
            m_strDesc = AddProcessFormat(m_strDesc);
        }

        if (NeedCd(process))
        {
            m_strDesc = AddCdFormat(m_strDesc);
        }
    }

    string GetTranceDescByProcess(GameCmd.TaskProcess process)
    {
        if (!Received)
        {
            return m_clienDB.strTaskReceive;
        }

        string strDesc = "";
        if (process == GameCmd.TaskProcess.TaskProcess_Doing)
        {
            strDesc = m_clienDB.strTaskTraceBegin;
        }
        else
        {
            if (taskSubType == TaskSubType.SubmitLimit)
            {
                if (finishLevel > MainPlayerHelper.GetPlayerLevel())
                {
                    strDesc = m_clienDB.strTaskTraceBegin;
                }
            }
            else
            {
                strDesc = m_clienDB.strTaskTraceEnd;
            }
        }
        return strDesc.Replace("\n", "");
    }

    /// <summary>
    /// 是否添加任务进度显示
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public bool NeedProcess(GameCmd.TaskProcess process)
    {
        if (process != GameCmd.TaskProcess.TaskProcess_Doing && process != GameCmd.TaskProcess.TaskProcess_CanDone)
        {
            return false;
        }
        if (taskSubType == TaskSubType.DeliverItem || taskSubType == TaskSubType.KillMonster ||
            taskSubType == TaskSubType.KillMonsterCollect || taskSubType == TaskSubType.Collection)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 是否需要附加时间显示(悬赏任务)
    /// </summary>
    /// <returns></returns>
    public bool NeedCd(GameCmd.TaskProcess process)
    {
        return this.taskType == GameCmd.TaskType.TaskType_Token && process == GameCmd.TaskProcess.TaskProcess_Doing;
    }

    public string UpdateFomatProcess(string strDesc)
    {
        try
        {
            return string.Format(strDesc, operate, state);
        }
        catch (System.Exception ex)
        {
            Engine.Utility.Log.Error(strDesc + taskId);
        }
        return strDesc;
    }

    /// <summary>
    /// 添加进度显示
    /// </summary>
    /// <param name="strDesc"></param>
    /// <returns></returns>
    string AddProcessFormat(string strDesc)
    {
        //string strcolor = "red";
        //if (GetTaskProcess() == GameCmd.TaskProcess.TaskProcess_CanDone)
        //{
        //    strcolor = "green";
        //}
        //string strProcess = "<color value=\"" + strcolor + "\">({0}/{1})</color>";
        string strProcess = "[ff0000]({0}/{1})[-]";
        if (GetTaskProcess() == GameCmd.TaskProcess.TaskProcess_CanDone)
        {
            strProcess = "[00ff00]({0}/{1})[-]";
        }
        strDesc = strDesc.Insert(strDesc.Length, strProcess);
        return strDesc;
    }

    /// <summary>
    /// 添加cd显示
    /// </summary>
    /// <returns></returns>
    string AddCdFormat(string strDesc)
    {
        RewardMisssionMgr rewardMisssionMgr = DataManager.Manager<TaskDataManager>().RewardMisssionData;
        if (rewardMisssionMgr != null)
        {
            if (rewardMisssionMgr.receiveReward != null)
            {
                uint leftTime = (uint)rewardMisssionMgr.receiveReward.nleftTime;
                //string strCd = string.Format("<color value=\"red\">{0}</color>", StringUtil.GetStringBySeconds(leftTime));
                string strCd = string.Format("[ff0000]{0}[-]", StringUtil.GetStringBySeconds(leftTime));

                strDesc = strDesc.Insert(strDesc.Length, strCd);
            }
        }
        return strDesc;
    }

    public static table.QuestDataBase GetTableTaskByID(uint taskId)
    {
        var task = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(taskId);
        return task;
    }

    public void FormatXmlName()
    {
        if (taskType == GameCmd.TaskType.TaskType_Normal)
        {
            m_strXmlName = string.Format("[主]{0}", m_clienDB.strName);
        }
        else if (taskType == GameCmd.TaskType.TaskType_Sub)
        {
            m_strXmlName = string.Format("[支]{0}", m_clienDB.strName);
        }

        else if (taskType == GameCmd.TaskType.TaskType_Clan)
        {
            m_strXmlName = string.Format("[{0}]{1}",
                DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Clan)
                , m_clienDB.strName);
        }
        //日环
        else if (taskType == GameCmd.TaskType.TaskType_Loop)
        {
            int Ring = DataManager.Manager<TaskDataManager>().Ring;

            Ring = Ring > 10 ? Ring - 10 : Ring;

            if (Ring > 0)
            {
                m_strXmlName = string.Format("[日]{0}({1}/{2})", m_clienDB.strName, Ring, maxRing);
            }
            else
            {
                m_strXmlName = string.Format("[日]{0}", m_clienDB.strName);
            }
        }
        //如果是悬赏任务 要加上已经接了多少次
        else if (taskType == GameCmd.TaskType.TaskType_Token)
        {
            RewardMisssionMgr rmm = DataManager.Manager<TaskDataManager>().RewardMisssionData;
            m_strXmlName = string.Format("[日]{0}({1}/{2})", m_clienDB.strName, rmm.RewardAcceptTimes, rmm.RewardAcceptAllTimes);
        }
        else
        {
            m_strXmlName = string.Format("[日]{0}", m_clienDB.strName);
        }


    }

    string ReplaceTag(string strDesc, GameCmd.TaskProcess process)
    {
        if (process == GameCmd.TaskProcess.TaskProcess_None)
        {
            strDesc = ReplaceNpc(strDesc, this.m_clienDB.dwBeginNpc, this.m_clienDB.acceptMapID);
        }
        else if (process == GameCmd.TaskProcess.TaskProcess_Doing)
        {
            switch (taskSubType)
            {
                case TaskSubType.Collection:
                    {
                        strDesc = ReplaceNpc(strDesc, this.m_clienDB.collect_npc, this.m_clienDB.destMapID);
                    }
                    break;
                case TaskSubType.CallMonster:
                case TaskSubType.KillMonster:
                    {
                        strDesc = ReplaceNpc(strDesc, this.m_clienDB.monster_npc, this.m_clienDB.destMapID);
                    }
                    break;
                case TaskSubType.KillMonsterCollect:
                    {
                        if (taskType == GameCmd.TaskType.TaskType_Loop)
                        {
                            strDesc = ReplaceItem(strDesc, this.m_clienDB.usecommitItemID, this.m_clienDB.destMapID);
                        }
                        else
                        {
                            strDesc = ReplaceNpc(strDesc, this.m_clienDB.monster_npc, this.m_clienDB.destMapID);
                        }
                    }
                    break;
                case TaskSubType.DeliverItem:
                    {
                        strDesc = ReplaceItem(strDesc, this.m_clienDB.usecommitItemID);
                        strDesc = ReplaceNpc(strDesc, this.m_clienDB.dwDoingNpc, this.m_clienDB.destMapID);
                    }
                    break;
                case TaskSubType.UseItem:
                    {
                        strDesc = ReplaceItem(strDesc, this.m_clienDB.usecommitItemID, this.m_clienDB.destMapID);
                    }
                    break;
                case TaskSubType.Talk:
                    {
                        strDesc = ReplaceNpc(strDesc, this.m_clienDB.dwEndNpc, this.m_clienDB.destMapID);
                    }
                    break;
                default:
                    break;
            }
        }
        else if (process == GameCmd.TaskProcess.TaskProcess_CanDone)
        {
            switch (taskSubType)
            {
                case TaskSubType.DeliverItem:
                    {
                        strDesc = ReplaceItem(strDesc, this.m_clienDB.usecommitItemID);
                        strDesc = ReplaceNpc(strDesc, this.m_clienDB.dwEndNpc, this.m_clienDB.submitMapID);
                    }
                    break;
                default:
                    strDesc = ReplaceNpc(strDesc, this.m_clienDB.dwEndNpc, this.m_clienDB.submitMapID);
                    break;
            }
        }

        return strDesc;
    }

    string ReplaceNpc(string strDesc, uint nNpcId)
    {
        //strDesc = strDesc.Replace("npc", string.Format("<color value=\"green\">{0}</color>", GetNpcName(nNpcId)));
        strDesc = strDesc.Replace("npc", string.Format("[00ff00]{0}[-]", GetNpcName(nNpcId)));
        return strDesc;
    }

    string ReplaceNpc(string strDesc, uint nNpcId, uint mapId)
    {
        //strDesc = strDesc.Replace("npc", string.Format("<color value=\"green\">{0}({1})</color>", GetNpcName(nNpcId), GetMapName(mapId)));
        strDesc = strDesc.Replace("npc", string.Format("[00ff00]{0}({1})[-]", GetNpcName(nNpcId), GetMapName(mapId)));
        return strDesc;
    }

    string ReplaceItem(string strDesc, uint nItemId)
    {
        table.ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(nItemId);
        if (itemdb != null)
        {
            //strDesc = strDesc.Replace("item", string.Format("<color value=\"green\">{0}</color>", itemdb.itemName));
            strDesc = strDesc.Replace("item", string.Format("[00ff00]{0}[-]", itemdb.itemName));
        }
        return strDesc;
    }

    string ReplaceItem(string strDesc, uint nItemId, uint mapId)
    {
        table.ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(nItemId);
        if (itemdb != null)
        {
            //strDesc = strDesc.Replace("item", string.Format("<color value=\"green\">{0}({1})</color>", itemdb.itemName, GetMapName(mapId)));
            strDesc = strDesc.Replace("item", string.Format("[00ff00]{0}({1})[-]", itemdb.itemName, GetMapName(mapId)));
        }
        return strDesc;
    }

    string GetNpcName(uint nNpcId)
    {
        table.NpcDataBase npcdb = GameTableManager.Instance.GetTableItem<table.NpcDataBase>(nNpcId);
        if (npcdb != null)
        {
            return npcdb.strName;
        }
        return "";
    }

    string GetMapName(uint mapId)
    {
        table.MapDataBase mapdb = GameTableManager.Instance.GetTableItem<table.MapDataBase>(mapId);
        if (mapdb != null)
        {
            return mapdb.strName;
        }
        return "";
    }

    /// <summary>
    /// 放入背包道具的任务  先判断背包是否满
    /// </summary>
    /// <param name="quest"></param>
    /// <returns></returns>
    public bool TaskItemCanPutInKanpsack()
    {
        if (taskSubType == TaskSubType.DeliverItem || taskSubType == TaskSubType.KillMonsterCollect || taskSubType == TaskSubType.Collection)
        {
            uint itemBaseId = m_clienDB.usecommitItemID;
            uint itemNum = state;
            return DataManager.Manager<KnapsackManager>().CanPutInKanpsack(itemBaseId, itemNum);
        }

        return true;
    }

    #region IComparable<QuestTraceInfo> 成员

    public int CompareTo(QuestTraceInfo other)
    {
        if (other == null)
            return 1;
        if (object.ReferenceEquals(this, other) || this.taskId == other.taskId)
            return 0;
        int n = 0;

        var quest1 = GetTableTaskByID(this.taskId);
        var quest2 = GetTableTaskByID(other.taskId);
        if (quest1 == null && quest2 == null)
            return 0;
        if (quest1 == null)
            return -1;
        if (quest2 == null)
            return 1;

        // 任务类型
        var type1 = quest1.Type;
        var type2 = quest2.Type;
        n = type1.CompareTo(type2);
        if (n != 0)
            return n;

        // 最小等级
        n = quest1.dwMinLevel.CompareTo(quest2.dwMinLevel);
        if (n != 0)
            return n;

        return 0;
    }

    #endregion
}
