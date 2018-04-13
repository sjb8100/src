using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cmd;
using GameCmd;
using table;


public class Quest : System.IComparable<Quest>
{
    /// <summary>
    /// 返回结束的NPCID
    /// </summary>
    /// <returns></returns>
    public uint GetEndNpcID()
    {
        return (this.ServerInfo.vars.FirstOrDefault(i => i.id == (ushort)TaskVariable.MvarIndex_EndNpcID) ?? GameCmd.PairNumber.Empty).value;
    }

    /// <summary>
    /// 返回是否是最后一个步骤
    /// </summary>
    /// <returns></returns>
    public bool IsLastStep()
    {
        var subtaskid = (int)(this.ServerInfo.vars.FirstOrDefault(i => i.id == (ushort)TaskVariable.MvarIndex_SubTask) ?? new GameCmd.PairNumber() { value = 1 }).value;
        var subtasknum = (int)(this.ServerInfo.vars.FirstOrDefault(i => i.id == (ushort)TaskVariable.MvarIndex_SubTaskNum) ?? new GameCmd.PairNumber() { value = 1 }).value;
        if (subtaskid >= subtasknum)
            return true;
        return false;
    }

    /// <summary>
    /// 返回是否是子任务的索引
    /// </summary>
    /// <returns></returns>
    public uint GetSubTaskID()
    {
        var subtaskid = (this.ServerInfo.vars.FirstOrDefault(i => i.id == (ushort)TaskVariable.MvarIndex_SubTask) ?? new GameCmd.PairNumber() { value = 1 }).value;
        return subtaskid;
    }

    /// <summary>
    /// 返回总步骤数
    /// </summary>
    /// <returns></returns>
    public uint GetSubTaskNum()
    {
        var subtasknum = (this.ServerInfo.vars.FirstOrDefault(i => i.id == (ushort)TaskVariable.MvarIndex_SubTaskNum) ?? new GameCmd.PairNumber() { value = 1 }).value;
        return subtasknum;
    }

    //***********************************************//
    public Quest(table.QuestDataBase config)
    {
        tableInfo = config;
    }
    public Quest()
    {

    }
    /// <summary>
    /// 动态描述(任务追踪)
    /// </summary>
    public string Desc { get; set; }
    /// <summary>
    /// 服务器任务数据
    /// </summary>
    public GameCmd.SerializeTask ServerInfo { get; set; }
    /// <summary>
    /// 配置表数据
    /// </summary>
    private table.QuestDataBase tableInfo;
    public table.QuestDataBase TableInfo
    {
        get
        {
            if (ServerInfo == null)
                return null;
            if (tableInfo != null && ServerInfo.id == tableInfo.dwID)
                return tableInfo;

            tableInfo = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(ServerInfo.id);
            return tableInfo;
        }
    }

    #region IComparable<Quest> 成员

    public int CompareTo(Quest other)
    {
        if (other == null)
            return 1;
        if (object.ReferenceEquals(this, other) || this.TableInfo.dwID == other.TableInfo.dwID)
            return 0;
        return this.TableInfo.dwMinLevel.CompareTo(other.TableInfo.dwMinLevel);
    }

    #endregion
}
