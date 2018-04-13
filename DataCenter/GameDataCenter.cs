using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utility;

public enum TableName
{
    //任务相关
    QuestDataBase,
    //游戏全局配置
    GameGlobalConfig,
}
public sealed class GameDataCenter
{
    private static GameDataCenter instance = new GameDataCenter();
    public bool IsConnectBwt = true;
    private GameDataCenter()
    {

    }
    public static GameDataCenter Instance
    {
        get
        {
            return instance;
        }
    }


    public IList<T> GetTableList<T>(TableName name)
    {
        if(name == TableName.QuestDataBase)
        {
            return (IList<T>)Table.Query<table.QuestDataBase>();
        }
        else if(name == TableName.GameGlobalConfig)
        {
            return (IList<T>)Table.Query<table.GameGlobalConfig>();
        }
        return null;
    }
    public T GetTableItem<T>(uint itemIndex,TableName name,int childID = 0)
    {
        if(name == TableName.QuestDataBase)
        {
            table.QuestDataBase item = Table.Query<table.QuestDataBase>().FirstOrDefault(i => i.dwID == itemIndex);

            return (T)(object)item;
        }
        else if(name == TableName.GameGlobalConfig)
        {
            table.GameGlobalConfig item = Table.Query<table.GameGlobalConfig>().FirstOrDefault(i => i.TableID == itemIndex);

            return (T)(object)item;
        }
        else
        {
            Log.Info("表格"+ name.ToString()+"未找到索引 "+itemIndex.ToString());
        }
        return default(T);
    }

 
}

