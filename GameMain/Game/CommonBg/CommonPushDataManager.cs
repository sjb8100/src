using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using Engine;
using UnityEngine;
using GameCmd;
using Client;
using Engine.Utility;
using Common;
enum PushType
{
    Equip,//装备
    Consum,//丹药
    Achievement, //成就
}
class PushItem
{
    public uint thisID;
    public PushType pushType;
    public string strName;
}
class CommonPushDataManager : BaseModuleData, IManager, ITimer
{
    /// <summary>
    /// 推送队列
    /// </summary>
    private Queue<PushItem> pushQueue = new Queue<PushItem>();

    private readonly uint m_uPushTimerID = 123;
    // 正在推送的道具id
    private PushItem m_uCurrentPushItem = null;
    int maxPushLevel = 60;
    uint achievementOpenLv = 0;
    #region IManager
    public void ClearData()
    {
        //EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
        //EventEngine.Instance().RemoveEventListener((int)GameEventID.SYSTEM_LOADSCENECOMPELETE, OnEvent);
        //EventEngine.Instance().RemoveEventListener((int)GameEventID.PUSH_ADDITEM, OnEvent);
        //EventEngine.Instance().RemoveEventListener((int)GameEventID.REFRESHACHIEVEMENTPUSH, OnEvent);
    }
    public void Initialize()
    {
        EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.LOGINSUCESSANDRECEIVEALLINFO, OnEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.PUSH_ADDITEM, OnEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.REFRESHACHIEVEMENTPUSH, OnEvent);
        maxPushLevel = GameTableManager.Instance.GetGlobalConfig<int>("PushMaxLimitLevel");
        achievementOpenLv = GameTableManager.Instance.GetGlobalConfig<uint>("AchievementOpenLevel");
       
    }

    public void Reset(bool depthClearData = false)
    {
        pushQueue.Clear();
        TimerAxis.Instance().KillTimer(m_uPushTimerID, this);
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonPushPanel);
    }

    public void Process(float deltaTime)
    {

    }
    #endregion
    void OnEvent(int eventID, object param)
    {
        long userID = MainPlayerHelper.GetPlayerUID();
        int level = MainPlayerHelper.GetPlayerLevel();
        if (level > maxPushLevel)
        {
            return;
        }
        if (eventID == (int)GameEventID.LOGINSUCESSANDRECEIVEALLINFO)
        {
            pushQueue.Clear();
            InitPushList();
            PopPushPanel();
        }
        else if (eventID == (int)GameEventID.ENTITYSYSTEM_PROPUPDATE)
        {
            stPropUpdate prop = (stPropUpdate)param;
            if (prop.uid == userID)
            {
                if (prop.nPropIndex == (int)CreatureProp.Level)
                {
                    pushQueue.Clear();

                    InitPushList();
                    PopPushPanel();
                }
            }
        }
        else if (eventID == (int)GameEventID.PUSH_ADDITEM)
        {
            if (param is BaseItem)
            {
                BaseItem bi = (BaseItem)param;

                InitPushList(bi);
                PopPushPanel();
            }
        }
        else if (eventID == (int)GameEventID.REFRESHACHIEVEMENTPUSH)
        {
            pushQueue.Clear();
            StructAchievePushItem();
            InitPushList();      
            PopPushPanel();
        }
    }
    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == m_uPushTimerID)
        {
            CommonPushPanel panel = DataManager.Manager<UIPanelManager>().GetPanel(PanelID.CommonPushPanel) as CommonPushPanel;
            if (panel != null)
            {
                panel.RefreshLabel();
            }
            else
            {
                TimerAxis.Instance().KillTimer(m_uPushTimerID, this);
            }
        }
    }
    public void PopPushPanel()
    {
        if (pushQueue.Count != 0)
        {
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.CommonPushPanel))
            {
                return;
            }
            PushItem item = pushQueue.Dequeue();
            if (item != null)
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonPushPanel, panelShowAction: (pb) =>
                {
                    if (null != pb && pb is CommonPushPanel)
                    {
                        CommonPushPanel panel = pb as CommonPushPanel;
                        panel.InitPushItem(item);
                        m_uCurrentPushItem = item;
                        if (!TimerAxis.Instance().IsExist(m_uPushTimerID, this))
                        {
                            TimerAxis.Instance().SetTimer(m_uPushTimerID, 1000, this);
                        }
                    }
                });
            }
        }
        else
        {
            if (!DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.CommonPushPanel))
            {
                TimerAxis.Instance().KillTimer(m_uPushTimerID, this);
            }
        }

    }
    void PushEnqueue(PushItem item)
    {
        if (item == null)
        {
            return;
        }
        BaseItem bi = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(item.thisID);
        if (bi != null)
        {
            if (bi.BaseType == ItemBaseType.ItemBaseType_Consumption)
            {
                if (!DataManager.Manager<ItemManager>().CanUseConsumerItem(bi.BaseId))
                {
                    return;
                }
            }         
        }

        if (m_uCurrentPushItem != null)
        {
            if (item.thisID == m_uCurrentPushItem.thisID)
            {
                return;
            }
        }

        if (!pushQueue.Contains(item))
        {
            pushQueue.Enqueue(item);
//             if (item.pushType == PushType.Achievement)
//             {
//                 pushQueue.Insert(0, item);
//             }
//             else 
//             {
//                 pushQueue.Add(item);
//             }          
        }
    }
    void StructAchievePushItem() 
    {
        bool achievementIsOpen = MainPlayerHelper.GetPlayerLevel() >= achievementOpenLv;
        if (!achievementIsOpen)
        {
            return;
        }
        PushItem item = DataManager.Manager<AchievementManager>().GetAchieveStatus();
        if (item != null)
        {
            PushEnqueue(item);
        }
    }
    void InitPushList(BaseItem baseItem = null)
    {
       
        List<uint> equipList = GetPushList(baseItem);
        if (equipList != null)
        {
            for (int i = 0; i < equipList.Count; i++)
            {
                PushItem item = new PushItem();
                item.thisID = equipList[i];

                item.pushType = PushType.Equip;
                PushEnqueue(item);
            }
        }
        if (baseItem != null)
        {
            if (baseItem.BaseType == GameCmd.ItemBaseType.ItemBaseType_Consumption)
            {
                if (baseItem.UseLv <= MainPlayerHelper.GetPlayerLevel())
                {
                    if (baseItem.BaseData != null)
                    {
                        if (baseItem.BaseData.IsPush)
                        {
                            PushItem item = new PushItem();
                            item.thisID = baseItem.QWThisID;
                            item.pushType = PushType.Consum;
                            PushEnqueue(item);
                        }
                    }
                }
            }

        }
        else
        {
            List<BaseItem> consumList = GetConsumList();
            if (consumList != null)
            {
                for (int i = 0; i < consumList.Count; i++)
                {
                    BaseItem bi = consumList[i];
                    PushItem item = new PushItem();
                    item.thisID = bi.QWThisID;
                    item.pushType = PushType.Consum;
                    PushEnqueue(item);
                }
            }
        }

    }
    /// <summary>
    /// 获取要推送的丹药列表
    /// </summary>
    /// <returns></returns>
    public List<BaseItem> GetConsumList()
    {
        List<BaseItem> itemList = new List<BaseItem>();
        ItemManager im = DataManager.Manager<ItemManager>();
        if (im == null)
        {
            return itemList;
        }
        List<uint> comumList = im.GetItemByType(ItemBaseType.ItemBaseType_Consumption, PACKAGETYPE.PACKAGETYPE_MAIN);
        if (comumList != null)
        {
            for (int i = 0; i < comumList.Count; i++)
            {
                uint thisID = comumList[i];
                BaseItem item = im.GetBaseItemByQwThisId(thisID);
                if (item != null)
                {
                    if (item != null)
                    {
                        if (item.UseLv <= MainPlayerHelper.GetPlayerLevel())
                        {

                            if (item.BaseData != null)
                            {
                                if (item.BaseData.IsPush)
                                {
                                
                                    if (DataManager.Manager<ItemManager>().CanUseConsumerItem(item.BaseId))
                                    {
                                        itemList.Add(item);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        itemList.Sort((x1, x2) =>
            {
                ItemDataBase db1 = x1.BaseData;
                ItemDataBase db2 = x2.BaseData;
                if (db1 != null && db2 != null)
                {
                    if (db1.sortID > db2.sortID)
                    {
                        return -1;
                    }
                    else if (db2.sortID < db2.sortID)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                return 0;
            });
        return itemList;
    }
    /// <summary>
    /// 获取要比较的装备dic
    /// </summary>
    /// <param name="isSingle">是否只比较掉落的这一个</param>
    /// <returns></returns>
    Dictionary<GameCmd.EquipType, List<BaseEquip>> GetWillCompareDic(BaseItem bi = null)
    {
        EquipManager em = DataManager.Manager<EquipManager>();
        if (em == null)
        {
            return null;
        }
        Dictionary<GameCmd.EquipType, List<BaseEquip>> dic = new Dictionary<EquipType, List<BaseEquip>>();
        if (bi != null)
        {
            if (bi.BaseType == ItemBaseType.ItemBaseType_Equip)
            {
                if (bi is BaseEquip)
                {
                    BaseEquip be = bi as BaseEquip;
                    if (be != null)
                    {
                        Array enumArray = Enum.GetValues(typeof(GameCmd.EquipType));
                        for (int i = 0; i < enumArray.Length; i++)
                        {
                            GameCmd.EquipType type = (GameCmd.EquipType)enumArray.GetValue(i);
                            if (be.EType == type)
                            {
                                List<BaseEquip> wareList = em.GetEquipsByPackageType(PACKAGETYPE.PACKAGETYPE_EQUIP, type);
                                wareList.Add(be);
                                List<BaseEquip> tempList = GetSortList(wareList);
                                dic.Add(type, tempList);
                            }
                            else
                            {
                                if (dic.ContainsKey(type))
                                {
                                    Log.Error("has contain key is " + type);
                                }
                                else
                                {
                                    dic.Add(type, null);
                                }
                            }
                        }
                    }
                }
            }

        }
        else
        {
            Array enumArray = Enum.GetValues(typeof(GameCmd.EquipType));
            for (int i = 0; i < enumArray.Length; i++)
            {
                GameCmd.EquipType type = (GameCmd.EquipType)enumArray.GetValue(i);
                List<BaseEquip> wareList = em.GetEquipsByPackageType(PACKAGETYPE.PACKAGETYPE_EQUIP, type);
                List<BaseEquip> packageList = em.GetEquipsByPackageType(PACKAGETYPE.PACKAGETYPE_MAIN, type);
                wareList.AddRange(packageList);

                List<BaseEquip> tempList = GetSortList(wareList);
                dic.Add(type, tempList);
            }
        }
        return dic;

    }
    List<uint> GetPushList(BaseItem bi = null)
    {
        EquipManager em = DataManager.Manager<EquipManager>();
        if (em == null)
        {
            return null;
        }
        Dictionary<GameCmd.EquipType, List<BaseEquip>> dic = GetWillCompareDic(bi);
        //Array enumArray = Enum.GetValues(typeof(GameCmd.EquipType));
        //for (int i = 0; i < enumArray.Length; i++)
        //{
        //    GameCmd.EquipType type = (GameCmd.EquipType)enumArray.GetValue(i);
        //    List<BaseEquip> wareList = em.GetEquipsByPackageType(PACKAGETYPE.PACKAGETYPE_EQUIP, type);
        //    List<BaseEquip> packageList = em.GetEquipsByPackageType(PACKAGETYPE.PACKAGETYPE_MAIN, type);
        //    wareList.AddRange(packageList);
        //    //if(type == EquipType.EquipType_AdornlOne)
        //    //{
        //    //    int a = 10;
        //    //}
        //    List<BaseEquip> tempList = GetSortList(wareList);
        //    dic.Add(type, tempList);
        //}

        List<uint> pushList = new List<uint>();
        Array posenumArray = Enum.GetValues(typeof(GameCmd.EquipPos));
        for (int i = 0; i < posenumArray.Length; i++)
        {
            GameCmd.EquipPos pos = (GameCmd.EquipPos)posenumArray.GetValue(i);
            if (pos != null)
            {
                EquipType type = em.GetEquipTypeByEquipPos(pos);

                if (type == EquipType.EquipType_AdornlOne || type == EquipType.EquipType_SoulOne || type == EquipType.EquipType_None)
                {
                    continue;
                }
                if (!dic.ContainsKey(type))
                {
                    continue;
                }
                List<BaseEquip> allList = dic[type];

                if (allList == null)
                {
                    continue;
                }
                uint posEquipID = 0;

                for (int m = 0; m < allList.Count; m++)
                {
                    BaseEquip tempEquip = allList[m];
                    if (em.IsWearEquip(tempEquip.QWThisID))
                    {
                        continue;
                    }
                    if (em.IsEquipPos(pos, out posEquipID))
                    {
                        ItemManager im = DataManager.Manager<ItemManager>();
                        BaseEquip wareEquip = im.GetBaseItemByQwThisId(posEquipID) as BaseEquip;

                        if (wareEquip.Power < tempEquip.Power)
                        {
                            pushList.Add(tempEquip.QWThisID);
                            allList.RemoveAt(m);
                            break;
                        }
                    }
                    else
                    {
                        pushList.Add(tempEquip.QWThisID);
                        allList.RemoveAt(m);
                        break;
                    }
                }

            }
        }

        //处理戒指和圣魂    if (type == EquipType.EquipType_AdornlOne || type ==EquipType.EquipType_SoulOne)
        if (dic.ContainsKey(EquipType.EquipType_AdornlOne))
        {
            List<BaseEquip> adornList = dic[EquipType.EquipType_AdornlOne];
            if (adornList != null)
            {
                for (int i = 0; i < adornList.Count; i++)
                {
                    if (i > 1)
                    {
                        break;
                    }
                    BaseEquip temp = adornList[i];
                    if (!em.IsWearEquip(temp.QWThisID))
                    {
                        pushList.Add(temp.QWThisID);
                    }
                }
            }
        }

        if (dic.ContainsKey(EquipType.EquipType_SoulOne))
        {
            List<BaseEquip> soulList = dic[EquipType.EquipType_SoulOne];
            if (soulList != null)
            {
                for (int i = 0; i < soulList.Count; i++)
                {
                    if (i > 1)
                    {
                        break;
                    }
                    BaseEquip temp = soulList[i];
                    if (!em.IsWearEquip(temp.QWThisID))
                    {
                        pushList.Add(temp.QWThisID);
                    }
                }
            }
        }

        return pushList;
    }

    List<BaseEquip> GetSortList(List<BaseEquip> list)
    {
        if (list != null)
        {
            EquipManager em = DataManager.Manager<EquipManager>();
            if (em == null)
            {
                return null;
            }
            List<uint> removeList = new List<uint>();
            for (int j = 0; j < list.Count; j++)
            {
                BaseEquip packageEquip = list[j];
                if (packageEquip == null)
                {
                    continue;
                }
                if (!DataManager.Manager<EquipManager>().CanPlayerWearEquipment(packageEquip.QWThisID))
                {
                    removeList.Add(packageEquip.QWThisID);
                    continue;
                }
                ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>(packageEquip.BaseId);
                if (db != null)
                {
                    if (!db.IsPush)
                    {
                        removeList.Add(packageEquip.QWThisID);
                        continue;
                    }
                }
            }
            for (int i = 0; i < removeList.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    BaseEquip packageEquip = list[j];
                    if (removeList[i] == packageEquip.QWThisID)
                    {
                        list.Remove(packageEquip);
                        break;
                    }
                }
            }
            //foreach (var equip in list)
            //{
            //    Log.Error("======================= equip powwer is " + equip.Power);
            //}
            list.Sort(new BaseEquipComparison());
            list.Reverse_NoHeapAlloc();
        }

        return list;
    }

}
public class BaseEquipComparison : IComparer<BaseEquip>
{
    public int Compare(BaseEquip x1, BaseEquip x2)
    {
        EquipManager em = DataManager.Manager<EquipManager>();

        if (x1.Power > x2.Power)
        {
            return 1;
        }
        else if (x1.Power < x2.Power)
        {
            return -1;
        }
        else
        {
            if (em.IsWearEquip(x1.QWThisID))
            {
                return 1;
            }
            if (em.IsWearEquip(x2.QWThisID))
            {
                return 1;
            }
            return 0;
        }


    }
}

