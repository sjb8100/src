using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Engine;
using Engine.Utility;
using Client;
using table;

public class NpcShopTabs
{
    public Dictionary<uint, List<int>> names = new Dictionary<uint, List<int>>();
    public Dictionary<uint, Dictionary<uint, List<StoreDataBase>>> param = new Dictionary<uint, Dictionary<uint, List<StoreDataBase>>>();
}
partial class Mall_NpcShopManager : BaseModuleData, IManager
{
    List<StoreDataBase> tables = null;
    private Dictionary<uint, List<int>> tabDic = new Dictionary<uint, List<int>>();
    public Dictionary<uint, List<int>> Tabs
    {
        //         set
        //         {
        //             tabDic = value;
        //         }
        get
        {
            return tabDic;

        }
    }
    private Dictionary<uint, string> tabNames = new Dictionary<uint, string>();
    private List<uint> tabList = null;
    public void OnOpenNpcShopCommond(List<uint> tabs,uint npc_id) 
    {
        Reset();
        tabList = tabs;
        NpcShopTabs data = new NpcShopTabs();
        Dictionary<uint, List<StoreDataBase>> dic = new Dictionary<uint, List<StoreDataBase>>();
        for (int i = 0; i < tabs.Count;i++ )
        {
            for (int j = 0; j < tables.Count;j++ )
            {
                if(tables[j].storeId == tabs[i])
                {
                    if (!tabDic.ContainsKey(tabs[i]))
                    {
                        List<int> sec = new List<int>();
                        sec.Add((int)tables[j].tag);
                        tabDic.Add(tabs[i], sec);
                        tabNames.Add(tabs[i], tables[j].NpcTag);
                    }
                    else 
                    {
                        if(!tabDic[tabs[i]].Contains((int)tables[j].tag))
                        {
                            tabDic[tabs[i]].Add((int)tables[j].tag);
                        }
                        
                    }
                    if (!dic.ContainsKey(tables[j].tag))
                    {
                        List<StoreDataBase> list = new List<StoreDataBase>();                                    
                        list.Add(tables[j]);
                        dic.Add(tables[j].tag, list);
                    }
                    else 
                    {
                        dic[tables[j].tag].Add(tables[j]);
                        
                    }
                }
            }
            data.param.Add(tabs[i], dic);
           
        }


        data.names = tabDic;
        uint taskid = DataManager.Manager<TaskDataManager>().DoingTaskID;
            if (taskid != 0)
            {
                QuestTraceInfo questInfo = QuestTranceManager.Instance.GetQuestTraceInfo(taskid);
                if (questInfo != null)
                {
                    if (questInfo.taskSubType == TaskSubType.DeliverItem && questInfo.doingNpc == npc_id)
                    {
                        table.ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(questInfo.QuestTable.usecommitItemID);
                        if (itemdb != null)
                        {
                            if (!string.IsNullOrEmpty(itemdb.MissionNpcParam))
                            {
                                string[] split = itemdb.MissionNpcParam.Split('_');
                                if (split.Length == 2)
                                {
                                    int obj = int.Parse(split[1]);
                                    ItemManager.DoJump(uint.Parse(split[0]), obj);
                                }
                                else
                                {
                                    Engine.Utility.Log.Error("道具表格中ID为{0}的任务NPC跳转参数缺失,长度不够!", questInfo.QuestTable.usecommitItemID);
                                }
                            }
                            else
                            {
                                Engine.Utility.Log.Error("道具表格中ID为{0}的任务NPC跳转参数为空!", questInfo.QuestTable.usecommitItemID);
                            }
                        }
                    }
                    else 
                    {
                        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.NpcShopPanel, data: new NpcShopTabs()
                        {
                            names = data.names,
                            param = data.param,
                        });
                    }
                }
                else
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.NpcShopPanel, data: new NpcShopTabs()
                    {
                        names = data.names,
                        param = data.param,
                    });
                }
            }
            else
           {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.NpcShopPanel, data: new NpcShopTabs()
                {
                    names = data.names,
                    param = data.param,
                });   
           }
           
    }
    

    public string GetNpcShopName(int index)
    {
        if (tabNames.ContainsKey(tabList[index]))
        {
            return tabNames[tabList[index]];
        }
        return "";
    }
    public uint GetNpcShopKey(int index)
    {
        if (tabList.Count > index)
        {
            return tabList[index];
        }
        return 0;
    }
    public List<uint> GetNpcShopKeys()
    {
        if (tabList != null)
        {
            return tabList;
        }
        return null;
    }
    public void Reset(bool depthClearData = false)
    {
        if (tabList != null)
        {
            tabList.Clear();
        }
        if(tabDic != null)
        {
          tabDic.Clear();
        }
        if (tabNames != null)
        {
            tabNames.Clear();
        }
        
    }
    public void Process(float deltime)
    { }
    public void Initialize()
    {
        tables = GameTableManager.Instance.GetTableList<StoreDataBase>();
    }
    public void ClearData()
    {

    }
}
