using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Client;
/// <summary>
/// 部位模式
/// </summary>
public enum EquipPartMode
{
    None = 0,
    //攻击
    AttackPart = 1,
    //防御
    DefendPart = 2,
    Max,
}
#region define
/// <summary>
/// 宝石镶嵌更新数据
/// </summary>
public class GemInlayUpdateData
{
    public enum GemInlayUpdateType
    {
        GIUT_None = 0,
        GIUT_Insert = 1,
        GIUT_Update = 2,
        GIUT_Remove = 3,
    }
    private GameCmd.GemType m_emGemType = GameCmd.GemType.GemType_None;
    //宝石类型
    public GameCmd.GemType GmType
    {
        get
        {
            return m_emGemType;
        }
    }
    public bool HaveGemCanInlay
    {
        set;
        get;
    }
    //宝石列表
    private List<uint> m_lstGemList = new List<uint>();
    //
    Dictionary<uint, int> m_dicGemNums = new Dictionary<uint, int>();
    public List<uint> m_lstGemQwThisIDList = new List<uint>();

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="qwThisIds"></param>
    public void StructData(List<uint> qwThisIds)
    {
        Reset();
        if (null == qwThisIds)
        {
            return;
        }
        m_lstGemQwThisIDList.Clear();
        m_lstGemQwThisIDList.AddRange(qwThisIds);
        Gem gem = null;
        int num = 0;
        for (int i = 0; i < qwThisIds.Count; i++)
        {
            gem = itemMgr.GetBaseItemByQwThisId<Gem>(qwThisIds[i]);
            if (null == gem || gem.Type != m_emGemType)
            {
                continue;
            }
            if (!m_lstGemList.Contains(gem.BaseId))
            {
                m_lstGemList.Add(gem.BaseId);
                num = itemMgr.GetItemNumByBaseId(gem.BaseId);
                if (m_dicGemNums.ContainsKey(gem.BaseId))
                {
                    m_dicGemNums[gem.BaseId] = num;
                }
                else
                {
                    m_dicGemNums.Add(gem.BaseId, num);
                }
            }
        }
        Sort();
    }

    public void Reset()
    {
        m_lstGemList.Clear();
        m_dicGemNums.Clear();
        HaveGemCanInlay = false;
    }

    public int Count
    {
        get
        {
            return m_lstGemList.Count;
        }
    }
    public uint PrefectGemID
    {
        get 
        {
            if (m_lstGemList.Count >0)
            {
                return m_lstGemList[0];
            }
            return 0;
        }
    }
    /// <summary>
    /// 更新镶嵌宝石数据
    /// </summary>
    /// <param name="baseId"></param>
    /// <param name="refreshUI"></param>
    public GemInlayUpdateType UpdateItem(uint baseId, out int index)
    {
        GemInlayUpdateType updateType = GemInlayUpdateType.GIUT_None;
        index = 0;
        Gem gem = itemMgr.GetTempBaseItemByBaseID<Gem>(baseId, ItemDefine.ItemDataType.Gem);
        if (null != gem)
        {
            if (gem.Type != GmType)
            {
                //非同类型
                return updateType;
            }
            int curNum =itemMgr.GetItemNumByBaseId(baseId);
            int cacheNum = m_dicGemNums.ContainsKey(baseId) ? m_dicGemNums[baseId] : 0;
            if ((curNum > cacheNum && cacheNum != 0)
                || (curNum < cacheNum && curNum != 0))
            {
                //更新
                m_dicGemNums[baseId] = curNum;
                index = m_lstGemList.IndexOf(baseId);
                updateType = GemInlayUpdateType.GIUT_Update;
            }
            else if (curNum == 0 && cacheNum != 0)
            {
                index = m_lstGemList.IndexOf(baseId);
                //删除
                if (m_lstGemList.Contains(baseId))
                {
                    m_lstGemList.Remove(baseId);
                }
                if (m_dicGemNums.ContainsKey(baseId))
                {
                    m_dicGemNums.Remove(baseId);
                }
                updateType = GemInlayUpdateType.GIUT_Remove;
            }
            else if (curNum > 0 && cacheNum == 0)
            {
                m_lstGemList.Add(baseId);
                Sort();
                index = m_lstGemList.IndexOf(baseId);
                m_dicGemNums.Add(baseId, curNum);
                updateType = GemInlayUpdateType.GIUT_Insert;
            }
        }
        return updateType;
    }

    public bool TryGetCanInlayGem(out uint gemBaseId)
    {
        bool success = false;
        gemBaseId = 0;
        if (Count > 0)
        {
            Gem gem = null;
            for (int i = 0; i < m_lstGemList.Count; i++)
            {
                gem = itemMgr.GetTempBaseItemByBaseID<Gem>(m_lstGemList[i], ItemDefine.ItemDataType.Gem);
                if (gem.UseLv <= MainPlayerHelper.GetPlayerLevel())
                {
                    success = true;
                    gemBaseId = m_lstGemList[i];
                    break;
                }
            }
        }
        return success;
    }

    /// <summary>
    /// 尝试根据列表索引获取baseid
    /// </summary>
    /// <param name="index"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool TryGetBaseIdByIndex(int index, out uint id)
    {
        bool success = false;
        id = 0;
        if (m_lstGemList.Count > index)
        {
            id = m_lstGemList[index];
            success = true;
        }
        return success;
    }

    /// <summary>
    /// 排序
    /// </summary>
    public void Sort()
    {
        BaseItem leftItem = null;
        BaseItem rightItem = null;
        m_lstGemList.Sort((left, right) =>
        {
            leftItem = itemMgr.GetTempBaseItemByBaseID<BaseItem>(left);
            rightItem = itemMgr.GetTempBaseItemByBaseID<BaseItem>(right);
            //降序排列
            return (int)rightItem.BaseData.sortID - (int)leftItem.BaseData.sortID;
        });
    }
    ItemManager itemmgr = null;
    ItemManager itemMgr
    {
        set 
        {
            itemmgr = value;
        }
        get 
        {
           if(itemmgr == null)
           {
               itemmgr = DataManager.Manager<ItemManager>();
           }
           return itemmgr;
        }
    }
    public GemInlayUpdateData(GameCmd.GemType gmType)
    {
        this.m_emGemType = gmType;
        Reset();
        itemmgr = DataManager.Manager<ItemManager>();
    }
}
#endregion
public class ForgingManager :IManager
{
    EquipManager emgr ;
    ItemManager imgr;
    TextManager  m_tmgr;
    public bool CanStrengthenAttack = false;
    public uint OpenLv
    {
        set;
        get;
    }
    public void ClearData()
    {
        RegisterEvent(false);
    }
    public void Initialize()
    {
         emgr = DataManager.Manager<EquipManager>();
         imgr = DataManager.Manager<ItemManager>();     
        m_tmgr = DataManager.Manager<TextManager>();
        OpenLv = GameTableManager.Instance.GetGlobalConfig<uint>("ForgingOpenLevel");
        m_dicInlayParts = new Dictionary<uint, List<GameCmd.EquipPos>>();
        RegisterEvent(true);
    }
    public void OnGridStrengthenCompelte(bool strengthenAll, GameCmd.EquipPos pos)
    {
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_GRIDSTRENGTHENLVCHANGED, strengthenAll);       
    }
    #region  强化
    /// <summary>
    /// 是不是可以强化
    /// </summary>
    /// <returns></returns>
    public bool HaveEquipCanStrengthen()
    {
        //用来记录有几个页签满足强化条件
        int count = 0;
        for (EquipPartMode i = EquipPartMode.AttackPart; i < EquipPartMode.Max; i++)
        {
            if (HaveEquipCanStrengthenByEquipPartMode(i))
            {
                count++;
            }         
        }
        return count >0;
    }
    /// <summary>
    /// 根据分类判断是不是可以强化 
    /// </summary>
    /// <param name="mode">攻击 VS 防御 </param>
    /// <returns></returns>
    public bool HaveEquipCanStrengthenByEquipPartMode(EquipPartMode mode) 
    {
        uint key = (uint)mode;
        List<GameCmd.EquipPos> lst = StructEquipPartInlayData()[key];
        for (int j = 0; j < lst.Count; j++)
        {
            if (JudgeEquipPosCanStrengthen(lst[j]))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 判断当前装备位置是不是可以强化
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool JudgeEquipPosCanStrengthen(GameCmd.EquipPos pos)
    {
        //部位的当前强化等级＜主角当前等级
        bool isStrengthMax = emgr.IsGridStrengthenMax(pos);
        bool moneyEnough = false;
        bool itemEnough = false;
        EquipDefine.LocalGridStrengthenData next = emgr.GetNextStrengthDataByPos(pos);
        if (next != null)
        {
            ClientMoneyType type = (ClientMoneyType)next.MoneyCostData.ID;
            uint num = next.MoneyCostData.Num;
            moneyEnough = MainPlayerHelper.GetMoneyNumByType(type) >= num;
            //满足下面要求的   道具  的数量
            int matchItemNum = 0;
            for (int i = 0; i < next.ItemCostDatas.Count; i++)
            {
                uint itemID = next.ItemCostDatas[i].ID;
                uint itemNum = next.ItemCostDatas[i].Num;
                int holdNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
                if (holdNum >= itemNum)
                {
                    matchItemNum++;
                }
            }
            itemEnough = matchItemNum == next.ItemCostDatas.Count;
        }
        return itemEnough && moneyEnough;
    }
#endregion

    #region 镶嵌
    public bool HaveEquipCanInlayByIndex() 
    {
        int count = 0;
        for (EquipPartMode i = EquipPartMode.AttackPart; i < EquipPartMode.Max; i++)
        {
            if (HaveEquipCanInlayByEquipPartMode(i))
            {
                count++;
            }            
        }
        return count>0;
    }
    public bool HaveGemCanImprove(GameCmd.EquipPos pos, GameCmd.GemType gemType) 
    {
        for (EquipManager.EquipGridIndexType i = EquipManager.EquipGridIndexType.First; i < EquipManager.EquipGridIndexType.Max; i++)
        {
            if (JudgeEquipPosCanInlay(pos, i, gemType))
            {
                return true;
            }
        }
        return false;
    }
    public bool HaveEquipCanInlayByEquipPartMode(EquipPartMode mode)
    {
        uint key = (uint)mode;
        List<GameCmd.EquipPos> lst = StructEquipPartInlayData()[key];
      
        for (int j = 0; j < lst.Count; j++)
        {
            if (null == mlstCanInlayGemType)
            {
                mlstCanInlayGemType = new List<GameCmd.GemType>();
            }
            mlstCanInlayGemType.Clear();
            mlstCanInlayGemType.AddRange(emgr.GeCanInlaytGemTypeByPos(lst[j]));
             for (EquipManager.EquipGridIndexType i = EquipManager.EquipGridIndexType.First; i < EquipManager.EquipGridIndexType.Max; i++)
             {
                 for (int x = 0; x < mlstCanInlayGemType.Count; x++)
                 {
                     if (JudgeEquipPosCanInlay(lst[j], i, mlstCanInlayGemType[x]))
                     {
                         return true;
                     }
                 }
               
             }
           
        }
        return false;
    }

    public bool EquipPosCanInlay(GameCmd.EquipPos pos, EquipManager.EquipGridIndexType index) 
    {
        if (null == mlstCanInlayGemType)
        {
            mlstCanInlayGemType = new List<GameCmd.GemType>();
        }
        mlstCanInlayGemType.Clear();
        mlstCanInlayGemType.AddRange(emgr.GeCanInlaytGemTypeByPos(pos));
        for (int x = 0; x < mlstCanInlayGemType.Count; x++)
        {
            if (JudgeEquipPosCanInlay(pos, index, mlstCanInlayGemType[x]))
            {
                return true;
            }
        }
        return false;
    }
    public bool JudgeEquipPosCanInlay(GameCmd.EquipPos pos, EquipManager.EquipGridIndexType index, GameCmd.GemType gemType) 
    {
        uint inlayBaseId = 0;
        int matchNum = 0;
        if (!emgr.IsUnlockEquipGridIndex(index))
        {
            return false;
        }
        else if (emgr.TryGetEquipGridInlayGem(pos, index, out inlayBaseId))
        {
             Gem InlayItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<Gem>(inlayBaseId, ItemDefine.ItemDataType.Gem);
            //镶嵌的宝石小与背包的宝石？
            StructGemInlayDatas(pos);
            GemInlayUpdateData updateData= GetGemInlayUpdateData(gemType);           
            if (null != updateData)
            {
                updateData.HaveGemCanInlay = false;
                uint canInlayBaseId = 0;
                if (null != updateData && updateData.TryGetCanInlayGem(out canInlayBaseId))
                {
                    Gem canInlayItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<Gem>(canInlayBaseId, ItemDefine.ItemDataType.Gem);
                    if (InlayItem != null && canInlayItem != null)
                    {
                        bool higher = canInlayItem.FightPowerNum > InlayItem.FightPowerNum;
                        if (higher)
                        {
                            matchNum++;
                            updateData.HaveGemCanInlay = true;
                        }
                    }
                }
            }

               
        }
        else
        {
            StructGemInlayDatas(pos);
            GemInlayUpdateData  updateData = GetGemInlayUpdateData(gemType);               
            if (null != updateData)
            {
                updateData.HaveGemCanInlay = false;
                uint canInlayBaseId = 0;
                if (null != updateData && updateData.TryGetCanInlayGem(out canInlayBaseId))
                {
                    matchNum++;
                    updateData.HaveGemCanInlay = true;
                }                   
            }        
        }
        return matchNum > 0;
    }
     public   GemInlayUpdateData GetGemInlayUpdateData(GameCmd.GemType gemType)
    {
        return mdicCanInlayDatas.ContainsKey(gemType) ? mdicCanInlayDatas[gemType] : null;
    }
    #endregion
     Dictionary<uint, List<GameCmd.EquipPos>> m_dicInlayParts = null;
  
    public Dictionary<uint, List<GameCmd.EquipPos>> StructEquipPartInlayData() 
    {

        uint em = (uint)EquipPartMode.AttackPart;
      
        for (GameCmd.EquipPos i = GameCmd.EquipPos.EquipPos_Hat; i < GameCmd.EquipPos.EquipPos_Max; i++)
        {
            em = (uint)GetEquipModeByEquipPos(i);
            if (em == (uint)EquipPartMode.None)
            {
                continue;
            }
            if (!m_dicInlayParts.ContainsKey(em))
            {
                List<GameCmd.EquipPos> epsList = new List<GameCmd.EquipPos>();
                epsList.Add(i);
                m_dicInlayParts.Add(em, epsList);
            }
            else 
            {
                if (!m_dicInlayParts[em].Contains(i))
               {
                   m_dicInlayParts[em].Add(i);
               }
            }       
        }
        return m_dicInlayParts;
    }
    Dictionary<GameCmd.GemType, GemInlayUpdateData> mdicCanInlayDatas = null;
    private List<GameCmd.GemType> mlstCanInlayGemType = null;
    //当前展开的宝石类型
    private GameCmd.GemType m_emActiveGemType = GameCmd.GemType.GemType_None;
    /// <summary>
    /// 构建宝石镶嵌数据
    /// </summary>
    ///   //获取宝石列表
    List<uint> gemItemList = null;
    List<uint> subTypes = new List<uint>();
    public Dictionary<GameCmd.GemType, GemInlayUpdateData> StructGemInlayDatas(GameCmd.EquipPos pos)
    {     
        //可镶嵌数据
        if (null == mdicCanInlayDatas)
        {
            mdicCanInlayDatas = new Dictionary<GameCmd.GemType, GemInlayUpdateData>();
        }
        //mdicCanInlayDatas.Clear();

        //获取可镶嵌宝石列表
        if (null == mlstCanInlayGemType)
        {
            mlstCanInlayGemType = new List<GameCmd.GemType>();
        }
        mlstCanInlayGemType.Clear();
        mlstCanInlayGemType.AddRange(emgr.GeCanInlaytGemTypeByPos(pos));
        if (subTypes.Count == 0)
        {
            subTypes.Add((uint)ItemDefine.ItemMaterialSubType.Gem);
        }     
        if (gemItemList == null)
        {
            gemItemList = new List<uint>();
            gemItemList.AddRange(imgr.GetItemByType(GameCmd.ItemBaseType.ItemBaseType_Material, subTypes, GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN));
            gemQwThisIDsChanged = false;
        }
        else 
        {
            if (gemQwThisIDsChanged)
            {
                gemItemList.Clear();
                gemItemList.AddRange(imgr.GetItemByType(GameCmd.ItemBaseType.ItemBaseType_Material, subTypes, GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN));
                gemQwThisIDsChanged = false;
            }  
        }        
        GemInlayUpdateData gemInlayUpdateData = null;
        for (int i = 0; i < mlstCanInlayGemType.Count; i++)
        {
            if (mdicCanInlayDatas.ContainsKey(mlstCanInlayGemType[i]))
            {
                gemInlayUpdateData = mdicCanInlayDatas[mlstCanInlayGemType[i]];
                if (null != gemInlayUpdateData)
                {
                    if (!gemInlayUpdateData.m_lstGemQwThisIDList.Equals(gemItemList))
                    {
                        gemInlayUpdateData.StructData(gemItemList);
                    }               
                }             
            }
            else 
            {
                gemInlayUpdateData = new GemInlayUpdateData(mlstCanInlayGemType[i]);
                mdicCanInlayDatas.Add(mlstCanInlayGemType[i], gemInlayUpdateData);
            }
          
        }
        return mdicCanInlayDatas;
    }
    public EquipPartMode GetEquipModeByEquipPos(GameCmd.EquipPos ep)
    {
        EquipPartMode em = EquipPartMode.None;
        //圣魂、官印、披风无法镶嵌
        if (ep == GameCmd.EquipPos.EquipPos_SoulOne || ep == GameCmd.EquipPos.EquipPos_SoulTwo
            || ep == GameCmd.EquipPos.EquipPos_Office || ep == GameCmd.EquipPos.EquipPos_Capes)
        {
            em = EquipPartMode.None;
        }
        else if (ep == GameCmd.EquipPos.EquipPos_Equip
                || ep == GameCmd.EquipPos.EquipPos_AdornlOne
                || ep == GameCmd.EquipPos.EquipPos_AdornlTwo
                || ep == GameCmd.EquipPos.EquipPos_Shield)
        {
            em = EquipPartMode.AttackPart;
        }
        else
        {
            em = EquipPartMode.DefendPart;
        }
        return em;
    }
   
   public void Reset(bool depthClearData = false) 
   {
     
   }
   public void Process(float deltime) 
   { 
   
   }

   void RegisterEvent(bool reg) 
   {
       if (reg)
       {
           Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, GlobalEventHandler);
           Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);
       }
       else
       {
           Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, GlobalEventHandler);
           Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);
       }  
   }

   private bool curForgingWarningIsShowing = false;
   private bool gemQwThisIDsChanged = false;
   public void GlobalEventHandler(int nEventID, object param)
   {
       switch (nEventID)
       {
           case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
               {
                   if (null != param && param is ItemDefine.UpdateItemPassData)
                   {
                         ItemDefine.UpdateItemPassData passData = param as ItemDefine.UpdateItemPassData;
                         BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(passData.BaseId);
                         if (baseItem.IsGem || baseItem.IsForgingEquip)
                         {
                             gemQwThisIDsChanged = true; 
//                              bool open = MainPlayerHelper.GetPlayerLevel() >= DataManager.Manager<ForgingManager>().OpenLv;
//                              bool value = open && (HaveEquipCanStrengthen() || HaveEquipCanInlayByIndex());
//                              if (curForgingWarningIsShowing != value)
//                              {
//                                  stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
//                                  {
//                                      modelID = (int)WarningEnum.Forging,
//                                      direction = (int)WarningDirection.Left,
//                                      bShowRed = value,
//                                  };
//                                  Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
//                                  curForgingWarningIsShowing = value;
//                              }
                            
                         }
                   }
               }
               break;
           case (int)Client.GameEventID.SYSTEM_GAME_READY:
               {
                   bool open = MainPlayerHelper.GetPlayerLevel() >= DataManager.Manager<ForgingManager>().OpenLv;
                   bool value = open && (HaveEquipCanStrengthen() || HaveEquipCanInlayByIndex());                
                   if (value)
                   {
                       stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
                       {
                           modelID = (int)WarningEnum.Forging,
                           direction = (int)WarningDirection.Left,
                           bShowRed = value,
                       };
                       Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
                       curForgingWarningIsShowing = value;

                   }
               }
               break;
       }
   }
}
