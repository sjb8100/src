//*************************************************************************
//	创建日期:	2016-12-30 11:38
//	文件名称:	CommonUseItemPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	通用使用物品界面（坐骑，其他）
using System;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using Engine.Utility;
using table;
using Client;
using GameCmd;

#region

public interface IUserItem
{
    float CurrValue
    {
        get;
    }
    float MaxValue
    {
        get;
    }
    string Title
    {
        get;
    }
    bool MaxSendMsg
    {
        get;
        set;
    }
    void Init(UseItemCommonPanel parent);
    bool Check(uint itemid, uint itemNum);

    void OnUseSingleItem(UICommonUseItemGrid grid);
    void OnUse(UICommonUseItemGrid grid);
    void OnStartUse();

    bool TryGetItemsId(out List<UseItemCommonPanel.UseItemData> itemids);

}
public class RideTalentUserItem : IUserItem
{
    public RideManager.RideTalentEnum CurTalent
    {
        get
        {
            return m_curTalent;
        }
        set
        {
            m_curTalent = value;
        }
    }
    RideManager.RideTalentEnum m_curTalent = RideManager.RideTalentEnum.jingshen;

    float m_curTalentValue = 0;
    public float CurrValue
    {
        set
        {
            m_curTalentValue = value;
        }
        get
        {
            return m_curTalentValue;
        }
    }
    /// <summary>
    /// 资质最大值
    /// </summary>
    Dictionary<int, uint> m_totalTalent = new Dictionary<int, uint>();
    Dictionary<int, uint> m_addTalent = new Dictionary<int, uint>();
    public float MaxValue
    {
        get
        {
            return m_totalTalent[(int)CurTalent];
        }
    }

    public string Title
    {
        get
        {
            return "";
        }
    }

    public bool MaxSendMsg
    {
        get;
        set;
    }
    UseItemCommonPanel m_parent = null;
    RideManager rmg
    {
        get
        {
            return DataManager.Manager<RideManager>();
        }
    }

    public void Init(UseItemCommonPanel parent)
    {
        m_parent = parent;
        MaxSendMsg = false;

        if (m_totalTalent.Count == 0)
        {
            List<uint> totalList = GameTableManager.Instance.GetGlobalConfigList<uint>("KnightTelantItemAddMax");
            List<uint> addList = GameTableManager.Instance.GetGlobalConfigList<uint>("KnightTelantItemAdd");
            if (totalList != null)
            {
                if (totalList.Count == 5&&addList.Count == 5)
                {
                    Array ar = Enum.GetValues(typeof(RideManager.RideTalentEnum));
                    for (int i = 0; i < ar.Length; i++)
                    {
                        if (i < totalList.Count)
                        {
                            m_totalTalent.Add((int)ar.GetValue(i), totalList[i]);
                            m_addTalent.Add((int)ar.GetValue(i), addList[i]);
                            
                        }
                    }
                }
                else
                {
                    Log.Error("全局 KnightTelantItemAddMax is invalid");
                }
            }
            else
            {
                Log.Error("KnightTelantItemAddMax is not exist");
            }
        }
        InitRideTalent();
    }

    public bool Check(uint itemid, uint willUseNum)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemid);
        if (itemCount < willUseNum)
        {
            TipsManager.Instance.ShowTipsById(6);
            return false;
        }
        if(CurrValue >= MaxValue)
        {
            TipsManager.Instance.ShowTips("已经到达最大资质");
            return false;
        }
        uint addvalue = m_addTalent[(int)m_curTalent] * willUseNum;
        CurrValue = GetRigeTalent() + addvalue;
        return true;
    }

    public void OnUseSingleItem(UICommonUseItemGrid grid)
    {
        uint itemID = grid.Data.itemid;
        if (Check(itemID, 1))
        {
            m_parent.m_uWillUseItemNum = 1;
            OnUse(grid);
        }
    }

    public void OnUse(UICommonUseItemGrid grid)
    {
        stAddKnightTelantRideUserCmd_C cmd = new stAddKnightTelantRideUserCmd_C();
        cmd.attr = ((uint)(CurTalent) + 1);
        cmd.num = m_parent.m_uWillUseItemNum;
        NetService.Instance.Send(cmd);
    }
    uint GetRigeTalent()
    {
        if (m_curTalent == RideManager.RideTalentEnum.jingshen)
        {
            return rmg.RideTalent.jingshen;
        }
        else if (m_curTalent == RideManager.RideTalentEnum.liliang)
        {
            return rmg.RideTalent.liliang;
        }
        else if (m_curTalent == RideManager.RideTalentEnum.minjie)
        {
            return rmg.RideTalent.minjie;
        }
        else if (m_curTalent == RideManager.RideTalentEnum.tizhi)
        {
            return rmg.RideTalent.tizhi;
        }
        else if (m_curTalent == RideManager.RideTalentEnum.zhili)
        {
            return rmg.RideTalent.zhili;
        }
        return 0;
    }
    void InitRideTalent()
    {
        if (m_curTalent == RideManager.RideTalentEnum.jingshen)
        {
            m_curTalentValue = rmg.RideTalent.jingshen;
        }
        else if (m_curTalent == RideManager.RideTalentEnum.liliang)
        {
            m_curTalentValue = rmg.RideTalent.liliang;
        }
        else if (m_curTalent == RideManager.RideTalentEnum.minjie)
        {
            m_curTalentValue = rmg.RideTalent.minjie;
        }
        else if (m_curTalent == RideManager.RideTalentEnum.tizhi)
        {
            m_curTalentValue = rmg.RideTalent.tizhi;
        }
        else if (m_curTalent == RideManager.RideTalentEnum.zhili)
        {
            m_curTalentValue = rmg.RideTalent.zhili;
        }
    }
    public void OnStartUse()
    {
        MaxSendMsg = false;
        InitRideTalent();
    }

    public bool TryGetItemsId(out List<UseItemCommonPanel.UseItemData> itemids)
    {
        itemids = new List<UseItemCommonPanel.UseItemData>();

        UseItemCommonPanel.UseItemData itemdata = new UseItemCommonPanel.UseItemData();
        itemdata.parent = m_parent;
        itemdata.itemid = GameTableManager.Instance.GetGlobalConfig<uint>("KnightTelantItem");
        itemdata.useNum = 1;
        itemids.Add(itemdata);

        return true;
    }
}
public class RideExpUse : IUserItem
{
    UseItemCommonPanel m_parent = null;
    public string Title
    {
        get { return string.Format("{0}级", m_nCurrLevel); }
    }
    public bool MaxSendMsg
    {
        get;
        set;
    }
    RideData m_ridedata = null;
    int m_nCurrLevel;
    int m_nCurrExp;
    public void Init(UseItemCommonPanel parent)
    {
        m_parent = parent;
        m_ridedata = DataManager.Manager<RideManager>().GetRideDataById(m_parent.UserId);
        m_nCurrLevel = m_ridedata.level;
        m_nCurrExp = m_ridedata.exp;
        MaxSendMsg = false;
    }
    public void OnStartUse()
    {
        m_ridedata = DataManager.Manager<RideManager>().GetRideDataById(m_parent.UserId);
        m_nCurrLevel = m_ridedata.level;
        m_nCurrExp = m_ridedata.exp;
        MaxSendMsg = false;
    }
    public bool Check(uint itemid, uint willUseNum)
    {
        table.ItemDataBase itemTb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemid);
        if (itemTb.maxUseTimes != 0)
        {
            if (willUseNum > itemTb.maxUseTimes)
            {
                return false;
            }
        }
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemid);
        if (itemCount < willUseNum)
        {
            TipsManager.Instance.ShowTipsById(6);
            return false;
        }

        if (!CanLevelUp(itemid))
        {
            TipsManager.Instance.ShowTips(LocalTextType.Ride_Rank_zuoqidengjiyiman);
            return false;
        }

        return true;
    }

    bool CanLevelUp(uint itemID)
    {
        bool bRet = false;

        if (m_ridedata != null)
        {
            table.RideFeedData ridedata = GameTableManager.Instance.GetTableItem<table.RideFeedData>(m_ridedata.baseid, m_nCurrLevel);
            if (ridedata != null)
            {
                if (m_nCurrLevel > ridedata.maxLevel)
                {
                    return false;
                }
                int addexp = GameTableManager.Instance.GetGlobalConfig<int>("Ride_LevelUp_Item", itemID.ToString());
                m_nCurrExp += addexp;
                while (ridedata != null && m_nCurrExp >= ridedata.upExp)
                {
                    if (m_nCurrLevel + 1 > ridedata.maxLevel)
                    {
                        break;
                    }
                    m_nCurrLevel += 1;

                    m_nCurrExp -= (int)ridedata.upExp;
                    ridedata = GameTableManager.Instance.GetTableItem<table.RideFeedData>(m_ridedata.baseid, m_nCurrLevel);
                    bRet = true;
                }

                if (!bRet)
                {
                    if (ridedata != null && m_nCurrExp < ridedata.upExp)
                    {
                        bRet = true;
                    }
                }
            }
        }
        return bRet;
    }

    public void OnUseSingleItem(UICommonUseItemGrid grid)
    {
        uint itemID = grid.Data.itemid;
        if (Check(itemID, 1))
        {
            m_parent.m_uWillUseItemNum = 1;
            OnUse(grid);
        }
    }

    public void OnUse(UICommonUseItemGrid grid)
    {
        uint itemID = grid.Data.itemid;
        if (m_parent.m_uWillUseItemNum > 0)
        {
            //DataManager.Instance.Sender.RideLevelUpRideUser(m_parent.UserId, grid.Data.itemid, false, m_parent.m_uWillUseItemNum);
        }
    }

    public bool TryGetItemsId(out List<UseItemCommonPanel.UseItemData> itemids)
    {
        itemids = new List<UseItemCommonPanel.UseItemData>();
        List<string> lstKey = GameTableManager.Instance.GetGlobalConfigKeyList("Ride_LevelUp_Item");
        for (int i = 0; i < lstKey.Count; i++)
        {
            UseItemCommonPanel.UseItemData itemdata = new UseItemCommonPanel.UseItemData();
            itemdata.parent = m_parent;
            itemdata.itemid = uint.Parse(lstKey[i]);
            itemdata.useNum = (uint)DataManager.Manager<RideManager>().GetUseitemNum(itemdata.itemid);
            itemids.Add(itemdata);
        }

        itemids.Sort(delegate(UseItemCommonPanel.UseItemData a, UseItemCommonPanel.UseItemData b)
        {
            if (a.itemid < b.itemid)
            {
                return -1;
            }
            else if (a.itemid > b.itemid)
            {
                return 1;
            }
            return 0;
        });
        return true;
    }

    public float CurrValue
    {
        get
        {
            return m_nCurrExp * 1f;
        }
    }

    public float MaxValue
    {
        get
        {
            table.RideFeedData ridedata = GameTableManager.Instance.GetTableItem<table.RideFeedData>(m_ridedata.baseid, m_nCurrLevel);
            if (ridedata != null)
            {
                return ridedata.upExp * 1f;
            }
            return 1f;
        }
    }
}

public class RideFeedUse : IUserItem
{
    UseItemCommonPanel m_parent = null;
    RideData m_ridedata = null;
    int m_nCurrReplation = 0;
    int m_nMaxReplation = 1;
    public string Title
    {
        get { return "坐骑喂食"; }
    }
    public bool MaxSendMsg
    {
        get;
        set;
    }
    public void Init(UseItemCommonPanel parent)
    {
        m_parent = parent;
        OnStartUse();
    }

    public void OnStartUse()
    {
        m_ridedata = DataManager.Manager<RideManager>().GetRideDataById(m_parent.UserId);
        m_nCurrReplation = m_ridedata.repletion;
        m_nMaxReplation = (int)m_ridedata.maxRepletion;
        MaxSendMsg = false;
    }

    public bool Check(uint itemid, uint willUseNum)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemid);
        if (itemCount < willUseNum)
        {
            TipsManager.Instance.ShowTipsById(6);
            return false;
        }

        int addReplation = (int)GameTableManager.Instance.GetGlobalConfig<uint>("FeedItemRide", itemid.ToString());
        m_nCurrReplation += addReplation;
        return true;
    }

    public void OnUse(UICommonUseItemGrid grid)
    {
        uint itemID = grid.Data.itemid;
        if (m_parent.m_uWillUseItemNum > 0)
        {
            // DataManager.Instance.Sender.RideFeedRideUser(m_parent.UserId, grid.Data.itemid, m_parent.m_uWillUseItemNum);
        }
    }

    public void OnUseSingleItem(UICommonUseItemGrid grid)
    {
        uint itemID = grid.Data.itemid;
        if (Check(itemID, 1))
        {
            m_parent.m_uWillUseItemNum = 1;
            OnUse(grid);
        }
    }

    public bool TryGetItemsId(out List<UseItemCommonPanel.UseItemData> itemids)
    {
        itemids = new List<UseItemCommonPanel.UseItemData>();
        List<string> lstKey = GameTableManager.Instance.GetGlobalConfigKeyList("FeedItemRide");
        lstKey.Sort();
        for (int i = 0; i < lstKey.Count; i++)
        {
            UseItemCommonPanel.UseItemData itemdata = new UseItemCommonPanel.UseItemData();
            itemdata.parent = m_parent;
            itemdata.itemid = uint.Parse(lstKey[i]);
            itemids.Add(itemdata);
        }
        return true;
    }

    public float CurrValue
    {
        get
        {
            return m_nCurrReplation * 1.0f;
        }
    }

    public float MaxValue
    {
        get
        {
            return m_nMaxReplation * 1f;
        }
    }
}

public class MuhonExpUse : IUserItem
{
    int m_nCurrLevel;
    int m_nCurrExp;
    private EquipManager emgr;
    private ItemManager imgr;
    private UseItemCommonPanel m_parent = null;
    private Muhon m_itemData = null;
    public float CurrValue
    {
        get { return m_nCurrExp * 1f; }
    }

    public float MaxValue
    {
        get
        {
            table.WeaponSoulUpgradeDataBase weaponSoul = GameTableManager.Instance.GetTableItem<table.WeaponSoulUpgradeDataBase>(m_itemData.BaseId, m_nCurrLevel);
            if (weaponSoul != null)
            {
                return weaponSoul.upgradeExperience * 1.0f;
            }
            return 1f;
        }
    }

    public string Title
    {
        get { return string.Format("{0}级", m_nCurrLevel); }
    }

    public bool MaxSendMsg
    {
        get;
        set;
    }

    public void Init(UseItemCommonPanel parent)
    {
        m_parent = parent;
        emgr = DataManager.Manager<EquipManager>();
        imgr = DataManager.Manager<ItemManager>();
        OnStartUse();
    }

    public bool Check(uint itemid, uint itemNum)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemid);
        if (itemCount < itemNum)
        {
            TipsManager.Instance.ShowTipsById(6);
            return false;
        }

        if (!CanUse2LevelUp(itemid))
        {
            TipsManager.Instance.ShowTips("已升至最高等级");
            return false;
        }

        return true;
    }

    bool CanUse2LevelUp(uint itemID)
    {
        if (m_itemData == null)
        {
            return false;
        }
        bool bRet = false;
        table.WeaponSoulUpgradeDataBase weaponSoul = GameTableManager.Instance.GetTableItem<table.WeaponSoulUpgradeDataBase>(m_itemData.BaseId, m_nCurrLevel);
        if (weaponSoul != null)
        {
            int addexp = GameTableManager.Instance.GetGlobalConfig<int>("AddExpItem", itemID.ToString());
            m_nCurrExp += addexp;
            //如果能升级
            while (weaponSoul != null && m_nCurrExp >= weaponSoul.upgradeExperience)
            {
                if (m_nCurrLevel + 1 > weaponSoul.maxLevel)
                {
                    break;
                }
                m_nCurrLevel += 1;

                m_nCurrExp -= (int)weaponSoul.upgradeExperience;
                weaponSoul = GameTableManager.Instance.GetTableItem<table.WeaponSoulUpgradeDataBase>(m_itemData.BaseId, m_nCurrLevel);
                bRet = true;
            }
            if (!bRet)
            {
                if (weaponSoul != null && m_nCurrExp < weaponSoul.upgradeExperience)
                {
                    bRet = true;
                }
            }
        }
        return bRet;
    }
    public void OnUseSingleItem(UICommonUseItemGrid grid)
    {
        uint itemID = grid.Data.itemid;
        if (Check(itemID, 1))
        {
            m_parent.m_uWillUseItemNum = 1;
            OnUse(grid);
        }
    }

    public void OnUse(UICommonUseItemGrid grid)
    {
        if (m_parent.m_uWillUseItemNum > 0)
        {
            emgr.AddWeaponSoulExp(m_parent.UserId, grid.Data.itemid, m_parent.m_uWillUseItemNum);
        }
    }

    public void OnStartUse()
    {
        m_itemData = imgr.GetBaseItemByQwThisId<Muhon>(m_parent.UserId);
        MaxSendMsg = false;
        m_nCurrExp = (int)m_itemData.Exp;
        m_nCurrLevel = (int)m_itemData.Level;
    }

    public bool TryGetItemsId(out List<UseItemCommonPanel.UseItemData> itemids)
    {
        itemids = new List<UseItemCommonPanel.UseItemData>();
        List<uint> muhonExpItemsId = emgr.MuhonExpItemIds;
        if (null == muhonExpItemsId || muhonExpItemsId.Count == 0)
        {
            Engine.Utility.Log.Error("获取圣魂经验丹id列表Error");
            return false;
        }
        muhonExpItemsId.Sort();
        for (int i = 0, imax = muhonExpItemsId.Count; i < imax; i++)
        {
            UseItemCommonPanel.UseItemData itemdata = new UseItemCommonPanel.UseItemData();
            itemdata.parent = m_parent;
            itemdata.itemid = muhonExpItemsId[i];
            itemdata.useNum = (uint)DataManager.Manager<RideManager>().GetUseitemNum(itemdata.itemid);
            itemids.Add(itemdata);
        }
        return true;
    }
}
#endregion

public partial class UseItemCommonPanel : UIPanelBase, ITimer
{
    public class UseItemData
    {
        public UseItemCommonPanel parent;
        public PanelID parentId;
        public uint itemid;
        public uint useNum;
        public uint maxuseNum;
    }
    /// <summary>
    /// onshow显示参数 
    /// </summary>
    public class UseItemParam
    {
        public UseItemEnum type;
        public uint userId;//使用者id
        public int[] tabs;
        public object customParam;
        public override string ToString()
        {
            string str = "";
            if (tabs != null)
            {
                for (int i = 0; i < tabs.Length; i++)
                {
                    str += tabs[i] + ":";
                }
            }
            return type.ToString() + " " + userId.ToString() + "  " + str;
        }
    }

    public enum UseItemEnum
    {
        None,
        RideExp,//坐骑经验药水
        RideFeed,//坐骑饱食丹
        PetLife,//宠物生命
        PetExp,//宠物经验
        MuhonExp,//圣魂经验
        RideTalent,//坐骑资质
    }
    //长按结束时要用的丹药的数量
    public uint m_uWillUseItemNum = 0;
    UseItemParam m_UseItemParam = null;
    public UseItemEnum CurrUseItemEnum { get { return m_UseItemParam.type; } }
    public uint UserId { get { return m_UseItemParam.userId; } }
    private UIGridCreatorBase m_UIGridCreatorBase = null;
    List<UseItemData> m_lstUseItemData = new List<UseItemData>();

    UICommonUseItemGrid m_currUICommonUseItemGrid = null;
    //长按timerid
    private readonly uint m_uUseItemTimerID = 12345;
    //长按时间
    private float m_pressTime = 0;
    //全局配置表里的变换定时器时间列表
    List<float> m_TimerKeyList = new List<float>();
    //长按时间列表索引
    int m_nTimerIndex = 0;
    //是否长按
    bool m_bLongPress = false;

    int m_nCurPetExp = 0;
    int m_nCurPetLife = 0;
    int m_nCurPetLevel = 0;


    //IUserItem
    IUserItem m_currUserItem = null;
    protected override void OnAwake()
    {
        base.OnAwake();

        OnCreateGrid();
        InitTimerKey();
    }

    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    void OnCreateGrid()
    {
        UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uicommonuseitemgrid) as UnityEngine.GameObject;
        m_UIGridCreatorBase = m_trans_ItemScrollView.GetComponent<UIGridCreatorBase>();
        if (m_UIGridCreatorBase == null)
            m_UIGridCreatorBase = m_trans_ItemScrollView.gameObject.AddComponent<UIGridCreatorBase>();
        m_UIGridCreatorBase.gridContentOffset = new UnityEngine.Vector2(0, 0);
        m_UIGridCreatorBase.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
        m_UIGridCreatorBase.gridWidth = 403;
        m_UIGridCreatorBase.gridHeight = 105;
        m_UIGridCreatorBase.rowcolumLimit = 1;
        m_UIGridCreatorBase.RefreshCheck();
        m_UIGridCreatorBase.Initialize<UICommonUseItemGrid>(obj, OnItemGridDataUpdate, OnItemGridUIEvent);
    }
    void RegisterGlobalUIEvent(bool register = true)
    {
        if (register)
        {
            if (m_UseItemParam != null)
            {
                if (m_UseItemParam.type == UseItemEnum.PetExp || m_UseItemParam.type == UseItemEnum.PetLife)
                {
                    petDataManager.ValueUpdateEvent += PetDataManager_ValueUpdateEvent;
                }
            }
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
        }
        else
        {
            if (m_UseItemParam != null)
            {
                if (m_UseItemParam.type == UseItemEnum.PetExp || m_UseItemParam.type == UseItemEnum.PetLife)
                {
                    petDataManager.ValueUpdateEvent -= PetDataManager_ValueUpdateEvent;
                }
            }
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
        }
    }


    private void OnGlobalUIEventHandler(int eventType, object data)
    {
        switch (eventType)
        {

            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                //if (m_UseItemParam.type == UseItemEnum.PetLife || m_UseItemParam.type == UseItemEnum.PetExp)
                //{
                //    RefreshPetSliderData();
                //}
                if (m_currUICommonUseItemGrid != null)
                {
                    m_currUICommonUseItemGrid.RefreshItemNum();
                }
                break;
        }
    }
    void OnItemGridDataUpdate(UIGridBase data, int index)
    {
        if (m_lstUseItemData != null && index < m_lstUseItemData.Count)
        {
            data.SetGridData(m_lstUseItemData[index]);
        }
    }
    void InitPetData()
    {
        if (CurPet == null)
        {
            return;
        }
        m_nCurPetLevel = CurPet.GetProp((int)CreatureProp.Level);
        m_nCurPetLife = CurPet.GetProp((int)PetProp.Life);
        m_nCurPetExp = CurPet.GetProp((int)PetProp.LevelExp);
    }
    void OnItemGridUIEvent(UIEventType eventType, object data, object param)
    {
        UICommonUseItemGrid grid = data as UICommonUseItemGrid;
        m_currUICommonUseItemGrid = grid;


        switch (eventType)
        {
            case UIEventType.Click:
                {
                    switch (CurrUseItemEnum)
                    {
                        case UseItemCommonPanel.UseItemEnum.None:
                            break;
                        case UseItemCommonPanel.UseItemEnum.RideExp:
                        case UseItemCommonPanel.UseItemEnum.RideFeed:
                        case UseItemCommonPanel.UseItemEnum.RideTalent:
                        case UseItemCommonPanel.UseItemEnum.MuhonExp:
                            if (grid.Data.maxuseNum != 0 && grid.Data.useNum >= grid.Data.maxuseNum)
                            {
                                return;
                            }
                            m_currUserItem.OnStartUse();
                            m_currUserItem.OnUseSingleItem(grid);
                            RefreshSliderByClientData();
                            break;
                        case UseItemCommonPanel.UseItemEnum.PetLife:
                            InitPetData();
                            OnPetUseSingeItem(grid);
                            break;
                        case UseItemCommonPanel.UseItemEnum.PetExp:
                            InitPetData();
                            OnPetUseSingeItem(grid);
                            break;
                        default:
                            break;
                    }
                }
                break;
            case UIEventType.LongPress:
                {
                    m_uWillUseItemNum = 0;

                    m_bLongPress = true;
                    if (m_UseItemParam.type == UseItemEnum.PetLife || m_UseItemParam.type == UseItemEnum.PetExp)
                    {
                        InitPetData();
                    }
                }
                break;
            case UIEventType.Press:
                bool press = (bool)param;
                if (!press)
                {
                    m_bLongPress = false;
                    if (m_uWillUseItemNum <= 0)
                    {
                        return;
                    }
                    if (m_UseItemParam.type == UseItemEnum.PetLife || m_UseItemParam.type == UseItemEnum.PetExp)
                    {
                        OnPetUse(m_currUICommonUseItemGrid);
                    }
                    else if (m_UseItemParam.type == UseItemEnum.RideExp || m_UseItemParam.type == UseItemEnum.RideFeed
                        || m_UseItemParam.type == UseItemEnum.MuhonExp)
                    {
                        if (grid.Data.maxuseNum != 0 && grid.Data.useNum >= grid.Data.maxuseNum)
                        {
                            return;
                        }
                        if (m_currUserItem != null && !m_currUserItem.MaxSendMsg)
                        {
                            m_currUserItem.OnUse(grid);
                        }
                    }
                }
                else
                {
                    if (grid.Data.maxuseNum != 0 && grid.Data.useNum >= grid.Data.maxuseNum)
                    {
                        return;
                    }
                    if (m_currUserItem != null)
                    {
                        m_currUserItem.OnStartUse();
                    }
                }
                break;
        }

    }
    void Update()
    {
        if (m_bLongPress)
        {
            m_pressTime += Time.deltaTime;
            if (m_nTimerIndex < m_TimerKeyList.Count)
            {
                float time = m_TimerKeyList[m_nTimerIndex];
                if (m_pressTime >= time)
                {
                    m_nTimerIndex++;
                    TimerAxis.Instance().KillTimer(m_uUseItemTimerID, this);

                    uint delta = GameTableManager.Instance.GetGlobalConfig<uint>("QuickUseItemTime", time.ToString());
                    Log.Error("set timer " + delta.ToString());
                    TimerAxis.Instance().SetTimer(m_uUseItemTimerID, delta, this);
                }
            }
        }
        else
        {

            if (TimerAxis.Instance().IsExist(m_uUseItemTimerID, this))
            {
                TimerAxis.Instance().KillTimer(m_uUseItemTimerID, this);
            }

            m_uWillUseItemNum = 0;
            m_pressTime = 0;
            m_nTimerIndex = 0;
        }
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_uWillUseItemNum = 0;
        DataManager.Manager<RideManager>().ValueUpdateEvent += OnUpdateList;
        RegisterGlobalUIEvent(true);
        if (data is UseItemParam)
        {
            m_UseItemParam = (UseItemParam)data;

            switch (m_UseItemParam.type)
            {
                case UseItemEnum.None:
                    break;
                case UseItemEnum.RideExp:
                    m_currUserItem = new RideExpUse();
                    break;
                case UseItemEnum.RideFeed:
                    m_currUserItem = new RideFeedUse();
                    break;
                case UseItemEnum.MuhonExp:
                    m_currUserItem = new MuhonExpUse();
                    break;
                case UseItemEnum.PetLife:
                    InitPetData(UseItemEnum.PetLife);
                    break;
                case UseItemEnum.PetExp:
                    InitPetData(UseItemEnum.PetExp);
                    break;
                case UseItemEnum.RideTalent:
                    RideTalentUserItem item = new RideTalentUserItem();
                    item.CurTalent = (RideManager.RideTalentEnum)m_UseItemParam.customParam;
                    m_currUserItem = item;
                    break;
                default:
                    break;
            }

            InitUI();

            if (m_UIGridCreatorBase != null)
            {
                m_UIGridCreatorBase.CreateGrids(m_lstUseItemData.Count);
            }
        }
    }

    private void InitTimerKey()
    {
        List<string> timeList = GameTableManager.Instance.GetGlobalConfigKeyList("QuickUseItemTime");
        for (int i = 0; i < timeList.Count; i++)
        {
            string key = timeList[i];
            float time = 0;
            if (float.TryParse(key, out time))
            {
                if (!m_TimerKeyList.Contains(time))
                {
                    m_TimerKeyList.Add(time);
                }
            }
        }
        m_TimerKeyList.Sort((x1, x2) =>
            {
                if (x1 < x2)
                {
                    return -1;
                }
                else if (x1 > x2)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });
    }
    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalUIEvent(false);
        m_currUserItem = null;
        DataManager.Manager<RideManager>().ValueUpdateEvent -= OnUpdateList;
    }
    void OnUpdateList(object obj, ValueUpdateEventArgs value)
    {
        if (value.key.Equals("RideUsePanelRefresh"))
        {

            int curr = (int)value.newValue;
            float max = m_currUserItem.MaxValue;
            m_slider_Slider.value = curr / max;
            m_label_process.text = string.Format("{0}/{1}", curr, max);
        }
    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        if (jumpData != null)
        {

        }
    }
    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        return pd;
    }

    void UpdateSlider()
    {
        switch (m_UseItemParam.type)
        {
            case UseItemEnum.None:
                break;
            case UseItemEnum.RideExp:
            case UseItemEnum.RideFeed:
            case UseItemEnum.RideTalent:
            case UseItemEnum.MuhonExp:
                {
                    if (m_currUserItem != null)
                    {
                        float curr = m_currUserItem.CurrValue;
                        float max = m_currUserItem.MaxValue;
                        m_slider_Slider.value = curr / max;
                        if (curr > max)
                        {
                            curr = max;
                        }
                        m_label_process.text = string.Format("{0}/{1}", curr, max);
                    }
                }
                break;
            case UseItemEnum.PetExp:
                RefreshPetSliderData();
                break;
            case UseItemEnum.PetLife:
                RefreshPetSliderData();
                break;
            default:

                break;
        }
    }
    void ShowSlider(int curValue, int totalValue)
    {
        if (m_slider_Slider != null)
        {
            if (curValue > totalValue)
            {
                curValue = totalValue;
            }
            m_slider_Slider.value = curValue * 1.0f / totalValue;
            m_label_process.text = string.Format("{0}/{1}", curValue, totalValue);
        }
    }

    void ShowSlider(float curValue, float totalValue)
    {
        if (m_slider_Slider != null)
        {
            if (totalValue > 0)
            {
                if (curValue > totalValue)
                {
                    curValue = totalValue;
                }
                m_slider_Slider.value = curValue / totalValue;
                m_label_process.text = string.Format("{0}/{1}", curValue, totalValue);

                if (m_UseItemParam.type == UseItemEnum.RideExp ||
                    m_UseItemParam.type == UseItemEnum.MuhonExp)
                {
                    if (curValue == totalValue)
                    {
                        m_label_process.text = "已满级";
                    }
                }
            }
            else
            {
                if (m_UseItemParam.type == UseItemEnum.RideExp ||
                    m_UseItemParam.type == UseItemEnum.MuhonExp)
                {
                    m_slider_Slider.value = 1f;
                    m_label_process.text = "已满级";
                }
            }
        }
    }

    public void OnGetWay(uint itemid)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: itemid);
    }

    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }
    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        HideSelf();
    }
    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (UIMsgID.eUseItemRefresh == msgid)
        {
            if (m_currUICommonUseItemGrid != null)
            {
                UseItemEnum type = (UseItemEnum)param;
                switch (type)
                {
                    case UseItemEnum.None:
                        break;
                    case UseItemEnum.RideExp:
                        m_currUICommonUseItemGrid.Data.useNum = (uint)DataManager.Manager<RideManager>().GetUseitemNum(m_currUICommonUseItemGrid.Data.itemid);

                        break;
                    default:
                        break;
                }

                m_currUICommonUseItemGrid.RefreshItemNum();
                UpdateSlider();
            }
        }
        else if (UIMsgID.eShowUI == msgid)
        {
            UseItemCommonPanel.UseItemParam showInfo = (UseItemCommonPanel.UseItemParam)param;
            OnShow(param);
        }

        return base.OnMsg(msgid, param);
    }

    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == m_uUseItemTimerID)
        {
            int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_currUICommonUseItemGrid.Data.itemid);
            uint num = m_uWillUseItemNum + 1;
            if (m_UseItemParam.type == UseItemEnum.PetLife || m_UseItemParam.type == UseItemEnum.PetExp)
            {
                if (Check(m_currUICommonUseItemGrid.Data.itemid, num))
                {
                    m_uWillUseItemNum = num;
                    // Log.Error("useitemnum is " + num);
                    if (m_UseItemParam.type == UseItemEnum.PetLife || m_UseItemParam.type == UseItemEnum.PetExp)
                    {
                        RefreshPetSliderByClientData();
                        m_currUICommonUseItemGrid.RefreshItemNumByClientData(itemCount - (int)m_uWillUseItemNum);
                    }
                }
                else
                {
                    if (TimerAxis.Instance().IsExist(m_uUseItemTimerID, this))
                    {
                        TimerAxis.Instance().KillTimer(m_uUseItemTimerID, this);
                    }
                }
            }
            else if (m_UseItemParam.type == UseItemEnum.RideExp
                || m_UseItemParam.type == UseItemEnum.RideFeed
                || m_UseItemParam.type == UseItemEnum.RideTalent
                || m_UseItemParam.type == UseItemEnum.MuhonExp)
            {
                if (m_currUserItem != null)
                {
                    if (m_currUserItem.Check(m_currUICommonUseItemGrid.Data.itemid, num))
                    {
                        m_uWillUseItemNum = num;
                        Log.LogGroup("ZCX", "useitemnum is " + num);
                        RefreshSliderByClientData();

                        m_currUICommonUseItemGrid.RefreshItemNumByClientData(itemCount - (int)m_uWillUseItemNum);
                    }
                    else
                    {
                        if (!m_currUserItem.MaxSendMsg)
                        {
                            m_currUserItem.MaxSendMsg = true;
                            m_currUserItem.OnUse(m_currUICommonUseItemGrid);
                        }
                        if (TimerAxis.Instance().IsExist(m_uUseItemTimerID, this))
                        {
                            TimerAxis.Instance().KillTimer(m_uUseItemTimerID, this);
                        }
                    }
                }
                else
                {
                    if (TimerAxis.Instance().IsExist(m_uUseItemTimerID, this))
                    {
                        TimerAxis.Instance().KillTimer(m_uUseItemTimerID, this);
                    }
                }
            }
        }
    }

    #region

    void InitUI()
    {
        if (m_currUserItem == null)
        {
            return;
        }

        m_currUserItem.Init(this);
        m_currUserItem.TryGetItemsId(out m_lstUseItemData);
        m_label_title.text = m_currUserItem.Title;

        UpdateSlider();
    }

    void RefreshSliderByClientData()
    {
        if (m_currUserItem == null)
        {
            return;
        }
        ShowSlider(m_currUserItem.CurrValue, m_currUserItem.MaxValue);
        if (m_UseItemParam.type == UseItemEnum.MuhonExp || m_UseItemParam.type == UseItemEnum.RideExp)
        {
            m_label_title.text = m_currUserItem.Title;
        }
    }

    #endregion
}

