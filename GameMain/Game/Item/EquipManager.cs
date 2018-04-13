using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using table;
using System.ComponentModel;

public class EquipManager : BaseModuleData,IManager
{
    #region Property
    public const string CLASS_NAME = "EquipManager";
    //单次批量分解最大数量
    public const int CONST_BATCH_SPLIT_NUM_MAX = 20;
    private bool ready = false;
    public bool Ready
    {
        get
        {
            return ready;
        }
    }
    private float discount = 1.0f;
    private float RepairDiscount
    {
        get 
        {          
            return discount;
        }
    }
    #endregion
    #region IManager Method
    public void Initialize()
    {
        ready = true;

        //强化
        m_iActiveStrengthenSuitLv = 0;
        m_dicGridStrengthen = new Dictionary<EquipPos, uint>();
        BuildStrengthenSuitData();

        InitExchange();
        string str = GameTableManager.Instance.GetGlobalConfig<string>("RepairEquipRitio");
        if (!string.IsNullOrEmpty(str))
        {
            float.TryParse(str, out discount);
        }

        equipParticleLv = GameTableManager.Instance.GetGlobalConfigList<uint>("EquipStrengthenParticleLv");

      
    }

    public void Reset(bool depthClearData = false)
    {
        if (depthClearData)
        {
            gemInlayData.Clear();
            ResetStrengthen();
            ResetCompoundData();
        }
        
    }

    public void Process(float deltaTime)
    {
        
    }
    public void ClearData()
    {

    }
    #endregion

    private List<uint> equipParticleLv = null;
    public List<uint> EquipParticleLv
    {
        set 
        {
            value = equipParticleLv;
        }
        get 
        {
            return EquipParticleLv;
        }
    }

    private Dictionary<uint, uint> m_dic_EquipParticleID = new Dictionary<uint, uint>();
    public Dictionary<uint, uint> M_dic_EquipParticleID
    {
        set
        {
            value = m_dic_EquipParticleID;
        }
        get
        {
            return m_dic_EquipParticleID;
        }
    }

    /// <summary>
    /// 玩家是否可装备
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <returns></returns>
    public bool CanPlayerWearEquipment(uint qwThisId)
    {
        Equip data = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(qwThisId);
        if (null == data || data.BaseType != ItemBaseType.ItemBaseType_Equip)
            return false;
        //1、性别比较
        //2、职业比较
        if (!DataManager.IsMatchPalyerJob((int)data.BaseData.useRole))
        {
            return false;
        }
        //3、等级比较
        Client.IPlayer player = DataManager.Instance.MainPlayer;
        if (null == player)
        {
            return false;
        }
        int lv = player.GetProp((int)Client.CreatureProp.Level);
        if (lv < data.BaseData.useLevel)
        {
            return false;
        }
        return true;
    }

    #region Refine(精炼)
    //精炼最大等级
    public const int REFINE_MAX_LEVEL = 7;
    //最大附加属性条数
    public const int ADDITIVE_ATTR_NUM_MAX = 5;
    public const string REFINE_ACTIVE_ATTR_CONST_NAME = "Const_RefineSevenAddEffect";
    
    /// <summary>
    /// 获取装备辅助材料描述
    /// </summary>
    /// <param name="baseId"></param>
    /// <returns></returns>
    public string GetEquipAssistMaterialDes(uint baseId)
    {
        LocalTextType type = LocalTextType.LocalText_None;
        switch (baseId)
        {
            case 22001://天工石
            case 22002://天工神石
                type = LocalTextType.Local_TXT_forging_Refine_Tgdes;
                break;
            case 22003: //新月石
                type = LocalTextType.Local_TXT_forging_Refine_Xydes;
                break;
            case 22004://轮回石
            case 22005://轮回神石
                type = LocalTextType.Local_TXT_forging_Compound_Lhdes;
                break;
            case 22006://超级轮回石
                type = LocalTextType.Local_TXT_forging_Compound_Cldes;
                break;
        }
        return DataManager.Manager<TextManager>().GetLocalText(type);
    }

    /// <summary>
    /// 获取精炼七级高级属性
    /// </summary>
    /// <param name="pos">装备部位</param>
    /// <returns></returns>
    public PairNumber GetAdvanceAttrByEquipPos(int equipPos)
    {
        PairNumber pN = null;
        string effectIdString = GameTableManager.Instance.GetGlobalConfig<string>(REFINE_ACTIVE_ATTR_CONST_NAME);
        int index = equipPos - 1;
        string[] effectIdArray = effectIdString.Split(new char[] {'_'});
        if (null != effectIdArray && effectIdArray.Length > index)
        {
            StateDataBase effectDataBase = GameTableManager.Instance.GetTableItem<StateDataBase>(uint.Parse(effectIdArray[index].Trim()));
            if (null != effectDataBase)
            {
                pN = new PairNumber();
                pN.id = effectDataBase.typeid;
                pN.value = (uint)effectDataBase.param1;
                return pN;
            }
        }
        return pN; ;
    }

    /// <summary>
    /// 根据装备部位获取高级属性id
    /// </summary>
    /// <param name="equipType"></param>
    /// <returns></returns>
    public uint GetAdvanceAttrEffectIdByEquipPos(int equipType)
    {
        string effectIdString = GameTableManager.Instance.GetGlobalConfig<string>(REFINE_ACTIVE_ATTR_CONST_NAME);
        int index = equipType -1;
        string[] effectIdArray = effectIdString.Split(new char[] {'_'});
        if (equipType >= (int) GameCmd.EquipType.EquipType_Shield)
            index -= 1;
        uint effectId = 0;
        if (null != effectIdArray && effectIdArray.Length > index)
        {
            effectId = uint.Parse(effectIdArray[index].Trim());
        }
        return effectId;
    }

    /// <summary>
    /// 获取精炼辅助材料消耗数量
    /// </summary>
    /// <param name="itemBaseId"></param>
    /// <param name="refineLv"></param>
    /// <returns></returns>
    public int GetEquipRefineCostAssistMaterialsNum(uint itemBaseId,int refineLv)
    {
        return 0;
    }

    /// <summary>
    /// 获取装备基础属性
    /// </summary>
    /// <param name="baseId"></param>
    /// <returns></returns>
    public List<EquipDefine.EquipBasePropertyData> GetEquipBasePropertyData(uint baseId)
    {
        EquipDataBase dataBase = DataManager.Manager<ItemManager>().GetLocalDataBase<EquipDataBase>(baseId);
        if (null == dataBase)
            return null;
        List<EquipDefine.EquipBasePropertyData> baseList = new List<EquipDefine.EquipBasePropertyData>();
        EquipDefine.EquipBasePropertyData basePropertyData = null;
        //物攻
        if (dataBase.pdamMin != 0 && dataBase.pdamMax != 0)
        {
            basePropertyData = new EquipDefine.EquipBasePropertyData()
            {
                popertyType = EquipDefine.EquipBasePropertyType.PhyAttack,
                valueMin = dataBase.pdamMin,
                valueMax = dataBase.pdamMax,
            };
            baseList.Add(basePropertyData);
        }
        
        //法攻
        if (dataBase.mdamMin != 0 && dataBase.mdamMax != 0)
        {
            basePropertyData = new EquipDefine.EquipBasePropertyData()
            {
                popertyType = EquipDefine.EquipBasePropertyType.MagicAttack,
                valueMin = dataBase.mdamMin,
                valueMax = dataBase.mdamMax,
            };
            baseList.Add(basePropertyData);
        }

        //物防
        if (dataBase.pdefMin != 0 && dataBase.pdefMax != 0)
        {
            basePropertyData = new EquipDefine.EquipBasePropertyData()
            {
                popertyType = EquipDefine.EquipBasePropertyType.PhyDef,
                valueMin = dataBase.pdefMin,
                valueMax = dataBase.pdefMax,
            };
            baseList.Add(basePropertyData);
        }
        //法防
        if (dataBase.mdefMin != 0 && dataBase.mdefMax != 0)
        {
            basePropertyData = new EquipDefine.EquipBasePropertyData()
            {
                popertyType = EquipDefine.EquipBasePropertyType.MagicDef,
                valueMin = dataBase.mdefMin,
                valueMax = dataBase.mdefMax,
            };
            baseList.Add(basePropertyData);
        }

        return baseList;
    }

    /// <summary>
    /// 获取装备基础属性精炼提升值 
    /// 基础属性=（装备基础 + 固定总计） * （1 + 百分比总计）
    /// value = (baseValue + fixValue) *　(1 + percent)
    /// </summary>
    /// <param name="equipBaseId">装备表格id</param>
    /// <param name="refineLv">精炼等级</param>
    /// <returns></returns>
    public List<EquipDefine.EquipBasePropertyData> GetEquipRefineBasePropertyPromote(uint equipBaseId, int refineLv)
    {
        List<EquipDefine.EquipBasePropertyData> euipBasePropertyDataList = GetEquipBasePropertyData(equipBaseId);
        if (null == euipBasePropertyDataList || euipBasePropertyDataList.Count == 0)
            return null;

        uint baseValue = 0;
        uint fixValue = 0;
        float percent = 0;
        switch (refineLv)
        {
            case 0:
                fixValue = 0;
                percent = 0;
                break;
            case 1:
                fixValue = 1;
                percent = 0.05f;
                break;
            case 2:
                fixValue = 2;
                percent = 0.11f;
                break;
            case 3:
                fixValue = 4;
                percent = 0.18f;
                break;
            case 4:
                fixValue = 6;
                percent = 0.26f;
                break;
            case 5:
                fixValue = 9;
                percent = 0.35f;
                break;
            case 6:
                fixValue = 12;
                percent = 0.45f;
                break;
            case 7:
                fixValue = 16;
                percent = 0.57f;
                break;
        }

        for (int i = 0; i < euipBasePropertyDataList.Count;i ++ )
        {
            euipBasePropertyDataList[i].valueMin = (uint)((euipBasePropertyDataList[i].valueMin + fixValue) * (1 + percent));
            euipBasePropertyDataList[i].valueMax = (uint)((euipBasePropertyDataList[i].valueMax + fixValue) * (1 + percent));
        }
        return euipBasePropertyDataList;
    }

    /// <summary>
    /// 根据精炼成功概率获取对应进度条颜色
    /// </summary>
    /// <param name="baseId">装备表格id</param>
    /// <param name="refineLv">精炼等级</param>
    /// <returns></returns>
    public int GetEquipRefineProbability(uint baseId,int refineLv)
    {
        float probability = 0;
        EquipRefineDataBase refineDataBase = DataManager.Manager<ItemManager>().GetLocalDataBase<table.EquipRefineDataBase>(baseId, refineLv);
        if (null != refineDataBase)
        {
            probability = (refineDataBase.changeProb5 - refineDataBase.changeProb4) / (float)refineDataBase.changeProb5;
        }
        int percent = UnityEngine.Mathf.FloorToInt(probability * 100);
        return percent;
    }

    /// <summary>
    /// 精炼装备
    /// </summary>
    /// <param name="qwThisid">装备id</param>
    /// <param name="qwThisid">辅助道具id</param>
    /// <param name="useTickets">道具不足使用点券</param>
    public void EquipRefine(uint qwThisid, uint assistItemId, bool useTickets = false)
    {
        if (null != DataManager.Instance.Sender)
        {
            Equip data = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(qwThisid);
            if (null == data)
                return;
            DataManager.Instance.Sender.EquipRefineReq(qwThisid, assistItemId, useTickets);
        }
    }

    /// <summary>
    ///服务器下发装备精炼响应
    /// </summary>
    /// <param name="errorCode"></param>
    public void OnEquipRefineRes(uint errorCode)
    {
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTEQUIPREFINECOMPLETE, errorCode);
        
    }
    #endregion

    #region Combine(合成)
    public class CompoudSelectAttrData
    {
        public uint Index = 0;
        public List<GameCmd.PairNumber> Attrs = null;
        public uint BaseID;
        public uint CostItemID;
        public int CostItemNum;
        public GameCmd.MoneyType ReplaceCostMoneyType;
        public int ReplaceCostMoneyNum;
        public bool IsBind = false;
    }

    //合成服务器返回数据
    private GameCmd.stEquipComposeItemWorkUserCmd_CS m_curCompoundData;
    public GameCmd.stEquipComposeItemWorkUserCmd_CS CompoundData
    {
        get
        {
            return m_curCompoundData;
        }
    }

    //选择索引index
    private uint m_compoundSelectIndex = 0;
    public uint CompoundSelectIndex
    {
        get
        {
            return m_compoundSelectIndex;
        }
    }
    //已开启合成结果Mask
    private int m_openCompoundResultMask = 0;

    //祝福卷轴本地id名称
    public const string CONST_COMPOUND_ZF_ID_NAME = "Const_CompoundZFID";
    /// <summary>
    /// 重置合成数据
    /// </summary>
    private void ResetCompoundData()
    {
        m_compoundSelectIndex = 0;
        m_openCompoundResultMask = 0;
        m_curCompoundData = null;
    }

    /// <summary>
    /// 设置选中合成结果索引
    /// </summary>
    /// <param name="index"></param>
    public void SetSelectCompoundResult(uint index)
    {
        m_compoundSelectIndex = index;
    }

    /// <summary>
    /// 是否装备可以合成
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <returns></returns>
    public bool IsEquipCanCmpound(uint qwThisId)
    {
        Equip data = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(qwThisId);
        return (null != data && data.CanCompound);
    }

    /// <summary>
    /// 合成装备是否匹配主装备（职业 、部位 、绑定状态）
    /// </summary>
    /// <param name="mainEquipQwThisId"></param>
    /// <param name="compundQwThisId"></param>
    /// <returns></returns>
    public bool IsCompoundEquipMatchMainEquip(uint mainEquipQwThisId,uint compundQwThisId)
    {
        if (mainEquipQwThisId == compundQwThisId)
            return false;
        Equip mainEquipData = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(mainEquipQwThisId);
        Equip compoundEquipData = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(compundQwThisId);
        if (null != mainEquipData && mainEquipData.BaseType == ItemBaseType.ItemBaseType_Equip
            && null != compoundEquipData && compoundEquipData.BaseType == ItemBaseType.ItemBaseType_Equip)
        {
            //比较职业
            if (mainEquipData.BaseData.useRole != compoundEquipData.BaseData.useRole)
                return false;
            //比较部位
            if (!IsEquipPosMatch(mainEquipData.BaseData.subType, compoundEquipData.BaseData.subType))
                return false;
            //取消绑定状态比对
            ////比较绑定状态
            //if (mainEquipData.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Bind) != compoundEquipData.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Bind))
            //    return false;

            return true;
        }
        return false;
    }

    /// <summary>
    /// 装备合成
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="zf">是否为祝福</param>
    /// <param name="deputyIds"></param>
    /// <param name="assistItemId"></param>
    /// <param name="protectData"></param>
    public void EquipCompound(uint qwThisId,List<uint> deputyIds,bool zf,List<uint> protectAttrID)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.EquipCompoundReq(qwThisId, deputyIds, zf, protectAttrID);
        }
    }


    /// <summary>
    /// 合成选择属性
    /// </summary>
    /// <param name="index"></param>
    /// <param name="qwThisId"></param>
    public void DoEquipCompoundSelectResult()
    {
        if (null == m_curCompoundData)
        {
            Engine.Utility.Log.Error("EquipManager->EquipCompoundSelect faield,not exist CompoundData!");
            return;
        }
        if (m_compoundSelectIndex < 1 || m_compoundSelectIndex > 3)
        {
            m_compoundSelectIndex = 1;
            Engine.Utility.Log.Warning("EquipManager->EquipCompoundSelect out of range default Select index = 1!");
        }
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.EquipCompoundSelectReq((uint)m_compoundSelectIndex, m_curCompoundData.thisid);
        }
    }

    /// <summary>
    /// 装备合成响应
    /// </summary>
    /// <param name="msg"></param>
    public void OnEquipCompoud(uint qwThisId)
    {
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTEQUIPCOMPOUNDCOMPLETE, qwThisId);
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.EquipCompoundCompletePanel, data: qwThisId);
    }

    public void OnEquipSelectCompoud(GameCmd.stEquipComposeItemWorkUserCmd_CS msg)
    {
        ResetCompoundData();
        m_curCompoundData = msg;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.EquipCompoundChoosePanel, data: msg);
    }

    /// <summary>
    /// 打开合成结果
    /// </summary>
    /// <param name="index"></param>
    public void OpenEquipCompoundResult(uint index)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.OpenEquipCompoundCardReq(index);
    }

    /// <summary>
    /// 是否合成结果已开启
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool IsEquipCompoundResultOpen(uint index)
    {
        if (null != m_curCompoundData)
        {
            return (m_openCompoundResultMask & (1 << (int)index)) != 0; 
        }
        return false;
    }

    /// <summary>
    /// 成功打开合成结果
    /// </summary>
    /// <param name="index"></param>
    public void OnOpenEquipCompoundResult(uint index)
    {
        if (null != m_curCompoundData)
        {
            if (m_compoundSelectIndex == 0)
            {
                m_compoundSelectIndex = index;
            }
            m_openCompoundResultMask |= (1 << (int)index);        
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTEQUIPCOMPOUNDOPENRESULT,index);
    }


    /// <summary>
    /// 获取合成匹配装备
    /// </summary>
    /// <param name="compoundEquipId"></param>
    /// <returns></returns>
    public List<uint> GetCompoundMatchEquip(uint compoundEquipId)
    {
        List<uint> equipList = new List<uint>();
        equipList.AddRange(GetEquips(PACKAGETYPE.PACKAGETYPE_MAIN));
        List<uint> matchList = new List<uint>();
        if (equipList.Count > 0)
        {
            for(int i = 0;i < equipList.Count;i++)
            {
                if (IsCompoundEquipMatchMainEquip(compoundEquipId, equipList[i]))
                    matchList.Add(equipList[i]);
            }
        }
        matchList.Sort((left, right) =>
            {
                Equip leftE = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(left);
                Equip rightE = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(right);

                if (rightE.CanCompound)
                {
                    return 1;
                }
                else if (leftE.CanCompound)
                {
                    return -1;
                }else if (rightE.Power != leftE.Power)
                {
                    return rightE.AdditionAttrCount - leftE.AdditionAttrCount;
                }else if (rightE.IsBind)
                {
                    return 1;
                }else if (leftE.IsBind)
                {
                    return -1;
                }
                return 0;
            });
        return matchList;
    }

    /// <summary>
    ///获取祝福合成单个属性保护概率
    /// </summary>
    /// <param name="attrPair">属性</param>
    /// <param name="protectAttrNum">保护属性总条数</param>
    /// <returns></returns>
    public float GetZFAttrUintProtectProb(GameCmd.PairNumber attrPair, int protectAttrNum)
    {
        float propUint = 0;
        uint matchRuneStoneId = 0;
        if (!TryGetRuneStoneIdByEffectId(TransformAttrToEffectId(attrPair), out matchRuneStoneId))
        {
            Engine.Utility.Log.Error("TryGetRuneStoneIdByEffectId failed attr pair = [id,{0}]-[value,{1}]", attrPair.id, attrPair.value);
            return 0;
        }
        RuneStone runeStone = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<RuneStone>(matchRuneStoneId, ItemDefine.ItemDataType.RuneStone);
        float protectDivideValue = (protectAttrNum > 0) ? (float)System.Math.Pow(2, protectAttrNum - 1) : 1;
        propUint = ItemDefine.TransformTableValue2RealPercentage(runeStone.ProtectProp) / protectDivideValue;
        return propUint;
    }

    /// <summary>
    /// 获取祝福卷轴BaseId
    /// </summary>
    /// <returns></returns>
    public uint ZFBaseId
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_COMPOUND_ZF_ID_NAME);
        }
    }

    /// <summary>
    /// 获取匹配属性的符石
    /// </summary>
    /// <param name="pair"></param>
    /// <returns></returns>
    public uint GetMatchRunStoneByAttr(GameCmd.PairNumber pair)
    {
        if (null == pair)
        {
            return 0;
        }
        //带宝石孔属性的直接返回宝石孔符石
        if (pair.id == (uint)GameCmd.eItemAttribute.Item_Attribute_HoleNum)
        {
            return HoleRuenStoneId;
        }
        uint attrGarde = GetAttrGrade(pair);
        uint stoneBaseId = 0;
        List<table.RunestoneDataBase> runStoneList = GameTableManager.Instance.GetTableList<table.RunestoneDataBase>(); 
        if (null != runStoneList)
        {
            for (int i = 0,max = runStoneList.Count; i < max; i++)
            {
                if (runStoneList[i].grade == attrGarde)
                {
                    stoneBaseId = runStoneList[i].stoneId;
                    break;
                }
            }
        }
        return stoneBaseId;
    }

    #endregion

    #region Mosaic(镶嵌)
    /// <summary>
    /// 获取装备部位对应可镶嵌宝石类型
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    List<GameCmd.GemType> gemTypes = new List<GemType>();
    public List<GameCmd.GemType> GeCanInlaytGemTypeByPos(GameCmd.EquipPos pos)
    {
        gemTypes.Clear();
        for(GameCmd.GemType i = GemType.GemType_None +1;i < GameCmd.GemType.GemType_Max;i++)
        {
            if (IsGemTypeCanInlayPos(i,pos))
            {
                gemTypes.Add(i);
            }
        }
        return gemTypes;
    }
    /// <summary>
    /// 是否该部位可镶嵌该类型的宝石
    /// </summary>
    /// <param name="gemType"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool IsGemTypeCanInlayPos(GameCmd.GemType gemType,GameCmd.EquipPos pos)
    {
        bool cando = false;
        switch(pos)
        {
            case GameCmd.EquipPos.EquipPos_Equip:
                if (gemType == GemType.GemType_PhysicalAttact
                   || gemType == GemType.GemType_MagicAttact
                    || gemType == GemType.GemType_Exp)
                    cando = true;
                break;
            case GameCmd.EquipPos.EquipPos_AdornlOne:
            case GameCmd.EquipPos.EquipPos_AdornlTwo:
            case GameCmd.EquipPos.EquipPos_Shield:
                if (gemType == GemType.GemType_PhysicalAttact
                    || gemType == GemType.GemType_MagicAttact)
                    cando = true;
                break;
            case GameCmd.EquipPos.EquipPos_Hat:
            case GameCmd.EquipPos.EquipPos_Coat:
            case GameCmd.EquipPos.EquipPos_Leg:
            case GameCmd.EquipPos.EquipPos_Necklace:
                if (gemType == GemType.GemType_MagicDefent
                    || gemType == GemType.GemType_PhysicalDefent)
                    cando = true;
                break;
            case GameCmd.EquipPos.EquipPos_Cuff:
            case GameCmd.EquipPos.EquipPos_Shoes:
            case GameCmd.EquipPos.EquipPos_Shoulder:
            case GameCmd.EquipPos.EquipPos_Belf:
                if (gemType == GemType.GemType_UpgradeLife)
                    cando = true;
                break;
        }
        return cando;
    }

    /// <summary>
    /// 装备格类型
    /// </summary>
    public enum EquipGridIndexType
    {
        None = 0,
        First ,
        Second,
        Third,
        Max,
    }
    //第一个装备格开启角色等级名称
    public const string FIRST_GEM_EQUIP_LEVEL_NAME = "FirstGemEquipLevel";
    //第一个装备格开启角色等级名称
    public const string SECOND_GEM_EQUIP_LEVEL_NAME = "SecondGemEquipLevel";
    //第一个装备格开启角色等级名称
    public const string THIRD_GEM_EQUIP_LEVEL_NAME = "ThirdGemEquipLevel";
    //宝石镶嵌数据<id =(部位-1)*3 + index，镶嵌宝石BaseId>
    private Dictionary<int, uint> gemInlayData = new Dictionary<int, uint>();
    public Dictionary<int, uint> GemInlayData
    {
        set 
        {
            gemInlayData = value;
        }
        get 
        {
            return gemInlayData;
        }
    
    }
    

    /// <summary>
    /// 变换服务端下发的索引 成 EquipPos 和 index
    /// </summary>
    /// <param name="serverIndex"></param>
    /// <param name="pos"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool TransformEquipGridDataIndex (int serverIndex,ref int pos,ref int index)
    {
        bool success = false;
        index = serverIndex % 3;
        if (index == 0)
            index = 3;
        int mod = serverIndex % 3;
        pos = serverIndex / 3 + ((mod != 0) ? 1 : 0);

        if (index >= 1 && index <= 3 && pos >= (int)GameCmd.EquipPos.EquipPos_Hat && pos <= (int)GameCmd.EquipPos.EquipPos_Necklace)
            success = true;
        return success;
    }
    /// <summary>
    /// 尝试获取装备格上装备的宝石
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="gridIndexType"></param>
    /// <param name="gemBaseId"></param>
    /// <returns></returns>
    public bool TryGetEquipGridInlayGem(GameCmd.EquipPos pos, EquipGridIndexType gridIndexType, out uint gemBaseId)
    {
        int id = EquipDefine.BuildEquipGridId(pos, (int)gridIndexType);
        return gemInlayData.TryGetValue(id, out gemBaseId);
    }

    /// <summary>
    /// 装备个上是否镶嵌宝石
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="gridIndexType"></param>
    /// <returns></returns>
    public bool IsEquipGridIndexEmpty(GameCmd.EquipPos pos, EquipGridIndexType gridIndexType)
    {
        uint gemBaseId = 0;
        if (!TryGetEquipGridInlayGem(pos,gridIndexType,out gemBaseId))
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 是否已解锁该装备格
    /// </summary>
    /// <param name="equipGridType"></param>
    /// <returns></returns>
    public bool IsUnlockEquipGridIndex(EquipGridIndexType equipGridType,int playerLevel = -1)
    {
        int unlockNum = GetEquipGridUnlockNum(playerLevel);

        return (unlockNum >= (int)equipGridType) ? true : false;
    }

    /// <summary>
    /// 获取装备格开启等级
    /// </summary>
    /// <param name="gridType"></param>
    /// <returns></returns>
    Dictionary<string, int> EquipGridIndexTypeDic = null;
    public int GetEquipGridUnlockLevel(EquipGridIndexType gridType)
    {
        string constName = "";
        int value = 0;
        switch (gridType)
        {
            case EquipGridIndexType.First:
                constName = FIRST_GEM_EQUIP_LEVEL_NAME;
                break;
            case EquipGridIndexType.Second:
                constName = SECOND_GEM_EQUIP_LEVEL_NAME;
                break;
            case EquipGridIndexType.Third:
                constName = THIRD_GEM_EQUIP_LEVEL_NAME;
                break;
        }
        if (EquipGridIndexTypeDic == null)
        {
            EquipGridIndexTypeDic = new Dictionary<string, int>();
        }
        if (EquipGridIndexTypeDic.ContainsKey(constName))
        {
            value = EquipGridIndexTypeDic[constName];
        }
        else 
        {
            value = GameTableManager.Instance.GetGlobalConfig<int>(constName);
            EquipGridIndexTypeDic.Add(constName, value);
        }
        return value;
    }

    /// <summary>
    /// 获取装备格开启数量
    /// </summary>
    /// <returns></returns>
    public int GetEquipGridUnlockNum(int nPlayerLevel = -1)
    {
        int level = 0;
        if (nPlayerLevel == -1)
        {
            level = MainPlayerHelper.GetPlayerLevel();
        }
        else
        {
            level = nPlayerLevel;
        }
        
        int unlockNum = 0;
        if (level >= GetEquipGridUnlockLevel(EquipGridIndexType.First))
        {
            unlockNum += 1;
        }

        if (level >= GetEquipGridUnlockLevel(EquipGridIndexType.Second))
        {
            unlockNum += 1;
        }

        if (level >= GetEquipGridUnlockLevel(EquipGridIndexType.Third))
        {
            unlockNum += 1;
        }
        return unlockNum;
    }

    /// <summary>
    /// 宝石镶嵌
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="EquipGridIndexType"></param>
    /// <param name="gemBaseId"></param>
    public void GemInlay(GameCmd.EquipPos pos, EquipGridIndexType equipGridIndex, uint gemBaseId)
    {
        if (null != DataManager.Instance.Sender)
        {
            BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(gemBaseId);
            if (!DataManager.IsMatchPalyerLv((int)baseItem.UseLv))
            {
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Gemstone_Commond_6, baseItem.UseLv);
            }else
            {
                DataManager.Instance.Sender.InlayGemReq(pos, (uint)equipGridIndex, gemBaseId);
            }
        }
            
    }
    /// <summary>
    /// 宝石镶嵌响应
    /// </summary>
    /// <param name="pos">装备部位</param>
    /// <param name="equipGridIndex">宝石格索引</param>
    /// <param name="gemId">宝石Id</param>
    public void OnGemInlay(GameCmd.EquipPos pos, int equipGridIndex, uint gemId)
    {
        int buildId = EquipDefine.BuildEquipGridId(pos, equipGridIndex);
        UpdateGemInlayData(buildId, gemId);
    }

    /// <summary>
    /// 从装备格上卸下宝石
    /// </summary>
    /// <param name="pos">装备部位</param>
    /// <param name="equipGridIndex"></param>
    public void GemUnload(GameCmd.EquipPos pos, EquipGridIndexType gridIndexType)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.GemUnloadReq(pos, (uint)gridIndexType);
    }

    public void OnGemUnload(GameCmd.EquipPos pos, int equipGridIndex)
    {
        int buildId = EquipDefine.BuildEquipGridId(pos, equipGridIndex);
        UpdateGemInlayData(buildId, 0);
    }

    /// <summary>
    /// 宝石合成
    /// </summary>
    /// <param name="gemBaseId"></param>
    /// <param name="composeType"></param>
    public void GemCompose(uint gemBaseId, uint composeType)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.GemComposeReq(gemBaseId, composeType);
    }

    /// <summary>
    /// 宝石合成响应
    /// </summary>
    public void OnGemCompose(uint gemBaseId)
    {
        TipsManager.Instance.ShowTips("恭喜，合成成功");
    }

    /// <summary>
    /// 服务端推送
    /// </summary>
    /// <param name="gemInlayData"></param>
    public void OnAddGemInlayListData(List<GameCmd.GemInlayList> gemInlayList)
    {
        this.gemInlayData.Clear();
        if (null != gemInlayList)
        {
            int pos = 0;
            int index = 0;
            int id = 0;
            foreach (GameCmd.GemInlayList gemInlay in gemInlayList)
            {
                if (TransformEquipGridDataIndex((int)gemInlay.index, ref pos, ref index))
                {
                    id = EquipDefine.BuildEquipGridId((GameCmd.EquipPos)pos, index);
                    if (!gemInlayData.ContainsKey(id) && gemInlay.base_id != 0)
                        gemInlayData.Add(id, gemInlay.base_id);
                }
                else
                {
                    Engine.Utility.Log.Warning(CLASS_NAME + "->TransformEquipGridData error index = {0}", gemInlay.index);
                }

            }

        }
    }

    /// <summary>
    /// 更新镶嵌数据
    /// </summary>
    /// <param name="equipPos"></param>
    /// <param name="index"></param>
    /// <param name="baseId"></param>
    public void UpdateGemInlayData(GameCmd.EquipPos equipPos, int index, uint baseId)
    {
        int buildId = EquipDefine.BuildEquipGridId(equipPos, index);
        UpdateGemInlayData(buildId, baseId);
    }

    /// <summary>
    /// 更新镶嵌数据
    /// </summary>
    /// <param name="buildId"></param>
    /// <param name="baseId"></param>
    public void UpdateGemInlayData(int buildId, uint baseId)
    {
        if (gemInlayData.ContainsKey(buildId))
        {
            if (baseId == 0)
            {
                gemInlayData.Remove(buildId);
            }
            else
            {
                gemInlayData[buildId] = baseId;
                TipsManager.Instance.ShowTips("已成功更换该部位镶嵌的宝石");
            }
        }
        else if (baseId != 0)
        {
            gemInlayData.Add(buildId, baseId);
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTGEMINLAYCHANGED, baseId);
        //背包界面的镶嵌套装状态改变
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_EQUIPSTONESUITCHANGE, null);

    }
    #endregion

    #region EquipPropery Op（装备属性提升、消除）

    /// <summary>
    /// 装备属性提升
    /// </summary>
    /// <param name="qwThisId">装备id</param>
    /// <param name="multistrikeBaseId">符石id</param>
    /// <param name="popertyEnumId">附加属性id</param>
    public void EquipPropertyPromote(uint qwThisId, uint multistrikeBaseId, uint popertyEnumId)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.EquipPropPromoteReq(qwThisId, multistrikeBaseId, popertyEnumId);
    }

    /// <summary>
    /// 属性提升响应
    /// </summary>
    /// <param name="msg"></param>
    public void OnEquipPropertyPromote(GameCmd.stEquipPropPromoteItemWorkUserCmd_CS msg)
    {
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTEQUIPROPERTYPPROMOTE, (msg.ret == (uint)GameCmd.PropPromoteOPRet.PropPromoteOPRet_OK) ? true : false);
    }

    /// <summary>
    /// 移除装备附加属性
    /// </summary>
    /// <param name="qwThisId">装备id</param>
    /// <param name="multistrikeBaseId">符石id</param>
    /// <param name="popertyEnumInt">附加属性id</param>
    public void EquipPropertyRemove(uint qwThisId, uint multistrikeBaseId, uint popertyEnumId)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.EquipPropRemoveReq(qwThisId, multistrikeBaseId, popertyEnumId);
    }
     
    public void OnEquipPropertyRemove(GameCmd.stEquipPropRemoveItemWorkUserCmd_CS msg)
    {
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTEQUIPROPERTYPREMOVE, msg.prop_index);
        TipsManager.Instance.ShowTips("成功消除一条附加属性！");
    }
    #endregion

    #region RuneStone(符石)

    //未激活符石id名称
    public const string CONST_INACTIVE_RUNESTONE_NAME = "Const_Inactive_Rune";
    //符石激活消费金币
    public const string CONST_RUNEACTIVATION_MONEY_COST_NAME = "Const_RuneActivation_MoneyCost";
    //带孔符石id名称
    public const string CONST_HOLE_RUNESTONE_ID_NAME = "Const_Hole_Rune";
    /// <summary>
    /// 获取未激活符石baseid
    /// </summary>
    /// <returns></returns>
    public uint InactiveRuneStoneBaseId
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_INACTIVE_RUNESTONE_NAME);
        }
    }
    /// <summary>
    /// 获取符石激活消耗钱币数量
    /// </summary>
    /// <returns></returns>
    public uint RuneStoneActivationCost
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_RUNEACTIVATION_MONEY_COST_NAME);
        }
    }

    /// <summary>
    /// 带孔符石id
    /// </summary>
    public uint HoleRuenStoneId
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_HOLE_RUNESTONE_ID_NAME);
        }
    }

    /// <summary>
    /// 获取属性提升概率
    /// </summary>
    /// <param name="gradeDiff"></param>
    /// <returns></returns>
    public float GetPropertyPromoteProp(uint gradeDiff)
    {
        RSPropertyPromoteDataBase database = GameTableManager.Instance.GetTableItem<RSPropertyPromoteDataBase>(gradeDiff);
        float prop = 0;
        if (null != database)
        {
            prop = ItemDefine.TransformTableValue2RealPercentage(database.promoteProp);
        }
        return prop;
    }

    /// <summary>
    /// 根据effectid获取对应键值对
    /// </summary>
    /// <param name="effectId"></param>
    /// <returns></returns>
    public GameCmd.PairNumber GetAttrPairNumberById(uint effectId)
    {
        GameCmd.PairNumber pair = null;
        table.StateDataBase sdb = GameTableManager.Instance.GetTableItem<table.StateDataBase>(effectId);
        if (null != sdb)
        {
            pair = new PairNumber()
            {
                id = sdb.typeid,
                value = (uint)sdb.param1,
            };
        }
        return pair;
    }

    /// <summary>
    /// 根据属性键值对获取下一级属性id
    /// </summary>
    /// <param name="attr"></param>
    /// <returns></returns>
    public uint GetNextEffectId(GameCmd.PairNumber attr)
    {
        return GetNextEffectId(TransformAttrToEffectId(attr));
    }

    /// <summary>
    /// 获取effectId的下一级属性
    /// </summary>
    /// <param name="effectId"></param>
    /// <returns></returns>
    public uint GetNextEffectId(uint effectId)
    {
        if (effectId == 0 || IsAttrGradeMax(effectId))
        {
            return 0;
        }
        table.StateDataBase sdb = GameTableManager.Instance.GetTableItem<table.StateDataBase>(effectId);
        if (null != sdb)
        {
            return sdb.nextAttrId;
        }
        return 0;
    }

    /// <summary>
    /// attr是否达到最大档次
    /// </summary>
    /// <param name="attr"></param>
    /// <returns></returns>
    public bool IsAttrGradeMax(GameCmd.PairNumber attr)
    {
        return IsAttrGradeMax(TransformAttrToEffectId(attr));
    }

    /// <summary>
    /// 是否effect属性为最大档次
    /// </summary>
    /// <param name="effectId"></param>
    /// <returns></returns>
    public bool IsAttrGradeMax(uint effectId)
    {
        if (effectId != 0)
        {
            table.StateDataBase sdb = GameTableManager.Instance.GetTableItem<table.StateDataBase>(effectId);
            if (null != sdb)
            {
                return (sdb.grade == sdb.maxGrade);
            }
        }
        return false;
    }

    /// <summary>
    /// 获取属性档次
    /// </summary>
    /// <param name="attr"></param>
    /// <returns></returns>
    public uint GetAttrGrade(GameCmd.PairNumber attr)
    {
        uint grade = 1;
        if (null == attr)
        {
            return grade;
        }
        uint effectId = TransformAttrToEffectId((GameCmd.eItemAttribute)attr.id, attr.value);
        if (effectId != 0)
        {
            table.StateDataBase sdb = GameTableManager.Instance.GetTableItem<table.StateDataBase>(effectId);
            if (null != sdb)
            {
                grade = sdb.grade;
            }
        }
        return grade;
    }

    /// <summary>
    /// 尝试根据Effectid获取符石baseId
    /// </summary>
    /// <param name="effectId"></param>
    /// <returns></returns>
    public bool TryGetRuneStoneIdByEffectId(uint effectId, out uint runeStoneId)
    {
        runeStoneId = 0;
        bool success = false;
        table.StateDataBase sdb = GameTableManager.Instance.GetTableItem<table.StateDataBase>(effectId);
        if (null == sdb)
        {
            return false;
        }
        List<RunestoneDataBase> runeStoneDataBases = GameTableManager.Instance.GetTableList<RunestoneDataBase>();
        if (null != runeStoneDataBases && runeStoneDataBases.Count > 0)
        {
            foreach (RunestoneDataBase runeStone in runeStoneDataBases)
            {
                if (runeStone.grade == sdb.grade)
                {
                    runeStoneId = runeStone.stoneId;
                    success = true;
                    break;
                }
            }
        }
        return success;
    }

    /// <summary>
    /// 获取与属性相匹配的符石id 档次相等或者更高
    /// </summary>
    /// <param name="attrPair"></param>
    /// <returns></returns>
    public List<uint> GetMatchProperyRunestone(PairNumber attrPair, bool filterEqualityGrade = false)
    {
        List<uint> matchRuneStoneList = new List<uint>();
        uint matchRuneStoneId = GetMatchAttrRuneStone(attrPair);
        if (matchRuneStoneId == 0)
        {
            Engine.Utility.Log.Error("Get Match runestoneid,AttrPair[{0},{1}]", attrPair.id, attrPair.value);
            return matchRuneStoneList;
        }
        RuneStone current = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<RuneStone>(matchRuneStoneId, ItemDefine.ItemDataType.RuneStone);
        List<uint> subType = new List<uint>();
        subType.Add((int)ItemDefine.ItemMaterialSubType.Runestone);
        List<uint> runeStoneList = DataManager.Manager<ItemManager>().GetItemByType(ItemBaseType.ItemBaseType_Material, subType, PACKAGETYPE.PACKAGETYPE_MAIN);
        RuneStone itemData = null;

        List<uint> unmatchRuneStoneList = new List<uint>();
        if (null != runeStoneList && runeStoneList.Count > 0)
        {
            for (int i = 0; i < runeStoneList.Count; i++)
            {
                itemData = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<RuneStone>(runeStoneList[i]);
                if (null == itemData || null == itemData.LocalDataBase)
                    continue;
                if (current.Grade > itemData.Grade && !unmatchRuneStoneList.Contains(itemData.BaseId))
                {
                    unmatchRuneStoneList.Add(itemData.BaseId);
                }
                else if (current.Grade < itemData.Grade && !matchRuneStoneList.Contains(itemData.BaseId))
                {
                    matchRuneStoneList.Add(itemData.BaseId);
                }else if (current.Grade == itemData.Grade)
                {
                    if (filterEqualityGrade && !unmatchRuneStoneList.Contains(itemData.BaseId))
                    {
                        unmatchRuneStoneList.Add(itemData.BaseId);
                    }
                    else if (!filterEqualityGrade && !matchRuneStoneList.Contains(itemData.BaseId))
                    {
                        matchRuneStoneList.Add(itemData.BaseId);
                    }
                }
            }
        }

        //排序
        RuneStone leftRs = null;
        RuneStone rightRs = null;
        matchRuneStoneList.Sort((left, right) =>
        {
            leftRs = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<RuneStone>(left, ItemDefine.ItemDataType.RuneStone);
            rightRs = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<RuneStone>(right, ItemDefine.ItemDataType.RuneStone);
            return (int) rightRs.Grade - (int) leftRs.Grade;
        });

        unmatchRuneStoneList.Sort((left, right) =>
        {
            leftRs = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<RuneStone>(left,ItemDefine.ItemDataType.RuneStone);
            rightRs = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<RuneStone>(right, ItemDefine.ItemDataType.RuneStone);
            return (int)rightRs.Grade - (int)leftRs.Grade;
        });
        matchRuneStoneList.AddRange(unmatchRuneStoneList);

        return matchRuneStoneList;
    }

    /// <summary>
    /// 根据属性获取匹配的符石
    /// </summary>
    /// <param name="attrPair"></param>
    /// <returns></returns>
    public uint GetMatchAttrRuneStone(GameCmd.PairNumber attrPair)
    {
        uint matchAttrRuneStoneId = 0;
        if (TryGetRuneStoneIdByEffectId(TransformAttrToEffectId(attrPair), out matchAttrRuneStoneId))
        {
            return matchAttrRuneStoneId;
        }
        return 0;
    }
    /// <summary>
    /// 激活符石
    /// </summary>
    /// <param name="equipThisId">装备唯一id</param>
    public void ActiveRuneStone(uint equipThisId)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.RunStoneActiveReq(InactiveRuneStoneBaseId, equipThisId);
    }

    /// <summary>
    /// 服务器下发符石激活成功
    /// </summary>
    /// <param name="runStoneId"></param>
    /// <param name="equipThisId"></param>
    public void OnActiveRuneStone(uint runStoneId, uint equipThisId)
    {
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTRUNESTONEACTIVE, runStoneId);
    }

    /// <summary>
    /// 符石合成响应
    /// </summary>
    /// <param name="runStoneId"></param>
    /// <param name="composeType"></param>
    public void OnComposeRuneStone(uint runStoneId, uint composeType)
    {

    }

    #endregion

    #region WeaponSoul(圣魂养成)
    public const string CONST_EXP_ITEMS_ID_NAME = "AddExpItem";
    /// <summary>
    /// 圣魂经验丹列表
    /// </summary>
    public List<uint> MuhonExpItemIds
    {
        get
        {
            List<uint> expItemsId = new List<uint>();
            List<string> stringKeys = GameTableManager.Instance.GetGlobalConfigKeyList(CONST_EXP_ITEMS_ID_NAME);
            if (null != stringKeys && stringKeys.Count > 0)
            {
                string temp = null;
                uint itemId = 0;
                for (int i = 0; i < stringKeys.Count; i++)
                {
                    temp = stringKeys[i];
                    if (string.IsNullOrEmpty(temp))
                        continue;
                    temp = temp.Trim();
                    if (uint.TryParse(temp, out itemId))
                    {
                        expItemsId.Add(itemId);
                    }
                }

            }
            return expItemsId;
        }
    }

    /// <summary>
    /// 该物品是否为圣魂
    /// </summary>
    /// <param name="baseItem"></param>
    /// <returns></returns>
    public bool IsMuhon(BaseItem baseItem)
    {
        return (null != baseItem 
            && baseItem.BaseType == ItemBaseType.ItemBaseType_Equip 
            && baseItem.SubType == (uint)GameCmd.EquipType.EquipType_SoulOne);
    }

    /// <summary>
    /// 获取圣魂增加经验值
    /// </summary>
    /// <param name="muhonBaseId">唯一id</param>
    /// <returns></returns>
    public uint GetMuhonExpOfId(uint muhonBaseId)
    {
        return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_EXP_ITEMS_ID_NAME, muhonBaseId.ToString());
    }
    /// <summary>
    /// 获取圣魂数据
    /// </summary>
    /// <returns></returns>
    public List<uint> GetWeaponSoulDataList()
    {
        List<uint> weaponSoulList = new List<uint>();
        weaponSoulList.AddRange(GetEquipByEquipType(GameCmd.EquipType.EquipType_SoulOne, GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN));
        weaponSoulList.AddRange(GetEquipByEquipType(GameCmd.EquipType.EquipType_SoulOne, GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP));
        return weaponSoulList;
    }

    /// <summary>
    /// 获取物品外框和底框图标名称
    /// </summary>
    /// <param name="qwThisId">物品id</param>
    /// <returns></returns>
    public string[] GetItemBorderAndBaseIconName(uint qwThisId)
    {
        string borderName = "";
        string borderBaseName = "";
        string[] iconNames = new string[2];
        BaseItem data = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(qwThisId);
        if (null != data)
        {
            int addtiveAttrNum = data.GetAdditiveAttr().Count;
            switch (addtiveAttrNum)
            {
                case 0:
                case 1:
                    borderName = "border_gray";
                    borderBaseName = "border_gray_base";
                    break;
                case 2:
                    borderName = "border_green";
                    borderBaseName = "border_green_base";
                    break;
                case 3:
                    borderName = "border_blue";
                    borderBaseName = "border_blue_base";
                    break;

                case 4:
                    borderName = "border_purple";
                    borderBaseName = "border_purple_base";
                    break;
                case 5:
                    borderName = "border_orange";
                    borderBaseName = "border_orange_base";
                    break;
            }
        }
        iconNames[0] = borderName;
        iconNames[1] = borderBaseName;
        return iconNames;
    }



    #region WeaponSoul（升级）

    /// <summary>
    /// 是否达到圣魂最大等级
    /// </summary>
    /// <param name="baseId"></param>
    /// <param name="currentLevel"></param>
    /// <returns></returns>
    public bool IsWeaponSoulMaxLevel(uint baseId, uint currentLevel)
    {
        return (currentLevel >= GetWeaponSoulMaxLevel(baseId));
    }

    /// <summary>
    /// 获取圣魂当前星级最大等级
    /// </summary>
    /// <param name="baseId"></param>
    /// <returns></returns>
    public uint GetWeaponSoulMaxLevel(uint baseId)
    {
        WeaponSoulUpgradeDataBase dataBase = DataManager.Manager<ItemManager>().GetLocalDataBase<WeaponSoulUpgradeDataBase>(baseId, 1);
        return (null != dataBase) ? dataBase.maxLevel : 0;
    }

    /// <summary>
    /// 获取圣魂基础属性
    /// </summary>
    /// <param name="baseId"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public List<EquipDefine.EquipBasePropertyData> GetWeaponSoulBasePropertyData(uint baseId, int level)
    {
        List<EquipDefine.EquipBasePropertyData> baseProperty = null;
        WeaponSoulUpgradeDataBase dataBase = DataManager.Manager<ItemManager>().GetLocalDataBase<WeaponSoulUpgradeDataBase>(baseId, level);
        List<EquipDefine.EquipBasePropertyData> basePropertyList = new List<EquipDefine.EquipBasePropertyData>();
        EquipDefine.EquipBasePropertyData basePropertyData = null;
        if (null == dataBase)
            return null;
        //物攻
        if (dataBase.phyAttack != 0)
        {
            basePropertyData = new EquipDefine.EquipBasePropertyData()
            {
                popertyType = EquipDefine.EquipBasePropertyType.PhyAttack,
                valueMin = dataBase.phyAttack,
                valueMax = dataBase.phyAttack,
            };
            basePropertyList.Add(basePropertyData);
        }
        //法攻
        if (dataBase.magicAttack != 0)
        {
            basePropertyData = new EquipDefine.EquipBasePropertyData()
            {
                popertyType = EquipDefine.EquipBasePropertyType.MagicAttack,
                valueMin = dataBase.magicAttack,
                valueMax = dataBase.magicAttack,
            };
            basePropertyList.Add(basePropertyData);
        }
        //物防
        if (dataBase.phyDef != 0)
        {
            basePropertyData = new EquipDefine.EquipBasePropertyData()
            {
                popertyType = EquipDefine.EquipBasePropertyType.PhyDef,
                valueMin = dataBase.phyDef,
                valueMax = dataBase.phyDef,
            };
            basePropertyList.Add(basePropertyData);
        }

        //法防
        if (dataBase.magicDef != 0)
        {
            basePropertyData = new EquipDefine.EquipBasePropertyData()
            {
                popertyType = EquipDefine.EquipBasePropertyType.MagicDef,
                valueMin = dataBase.magicDef,
                valueMax = dataBase.magicDef,
            };
            basePropertyList.Add(basePropertyData);
        }
        return basePropertyList;
    }
    /// <summary>
    /// 吃圣魂经验丹
    /// </summary>
    /// <param name="qwThisId">圣魂id</param>
    /// <param name="expItemId">经验丹id</param>
    public void AddWeaponSoulExp(uint qwThisId, uint expItemId,uint num)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.AddExpWeaponSoulReq(qwThisId, expItemId,num);
    }

    /// <summary>
    ///吃圣魂经验丹响应
    /// </summary>
    /// <param name="qwThisId">圣魂id</param>
    /// <param name="exp">经验值</param>
    public void OnAddWeaponSoulExp(uint qwThisId,uint exp)
    {
        BaseItem itemData = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId((uint)qwThisId);
        if (null != itemData)
        {
            itemData.UpdateItemAttribute(GameCmd.eItemAttribute.Item_Attribute_WeaponSoul_Exp
                , itemData.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_WeaponSoul_Exp) + exp);
            ItemSerialize serverData = itemData.ServerData;
            stWildChannelCommonChatUserCmd_CS cmd = new stWildChannelCommonChatUserCmd_CS();
            cmd.byChatType = CHATTYPE.CHAT_SYS;
            cmd.szInfo = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_TXT_Soul_GetMuhonExpTips, exp);
            cmd.dwOPDes = 0;
            cmd.timestamp = (uint)DateTimeHelper.Instance.Now;
            ChatChannel channel = DataManager.Manager<ChatDataManager>().GetChannelByType(CHATTYPE.CHAT_SYS);
            if (channel != null)
            {
                channel.Add(channel.ToChatInfo(cmd));
            }

        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_REFRESHMUHONEXP, exp);
    }

    /// <summary>
    /// 圣魂升级
    /// </summary>
    public void OnWeaponSoulLevelUp(ItemSerialize data)
    {
        DataManager.Manager<ItemManager>().OnAddItemData(data, (uint)AddItemAction.AddItemAction_Refresh,true);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTMUHONUPGRADE);
    }



    #endregion

    #region WeaponSoul（激活消除）
    /// <summary>
    /// 激活圣魂
    /// </summary>
    /// <param name="qwThisId">圣魂id</param>
    /// <param name="autoUseDQ"></param>
    public void ActiveWeaponSoul(uint qwThisId, bool autoUseDQ)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.ActivationWeaponSoulReq(qwThisId, autoUseDQ);
    }

    /// <summary>
    /// 圣魂激活响应
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="data"></param>
    public void OnActiveWeaponSoul(ItemSerialize data)
    {
        DataManager.Manager<ItemManager>().OnAddItemData(data, (uint)AddItemAction.AddItemAction_Refresh);
    }

    /// <summary>
    /// 移除圣魂属性
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="autoUseDQ">属性索引</param>
    public void RemoveWeaponSoulProperty(uint qwThisId, bool autoUseDQ,List<uint> propertyIds)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.UnActivationWeaponSoulReq(qwThisId, autoUseDQ, propertyIds);
    }

    /// <summary>
    /// 圣魂属性移除响应
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="data"></param>
    public void OnRemoveWeaponSoulProperty(ItemSerialize data)
    {
        DataManager.Manager<ItemManager>().OnAddItemData(data, (uint)AddItemAction.AddItemAction_Refresh);
    }
    #endregion

    #region WeaponSoul（融合）

    /// <summary>
    /// 通过属性id 和 值反向查询EffectId
    /// </summary>
    /// <param name="attr"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public uint TransformAttrToEffectId(GameCmd.eItemAttribute attr, uint value)
    {
        List<table.StateDataBase> effectDataBases = GameTableManager.Instance.GetTableList<table.StateDataBase>();
        if (null == effectDataBases || effectDataBases.Count == 0)
            return 0;
        foreach (table.StateDataBase db in effectDataBases)
        {
            if (db.typeid == (uint)attr && db.param1 == value)
                return db.dwID;
        }
        return 0;
    }

    /// <summary>
    /// 通过属性pair获取对应的effectId
    /// </summary>
    /// <param name="attrPair"></param>
    /// <returns></returns>
    public uint TransformAttrToEffectId(GameCmd.PairNumber attrPair)
    {
        return (null != attrPair) ? TransformAttrToEffectId((GameCmd.eItemAttribute)attrPair.id, attrPair.value) : 0;
    }


    /// <summary>
    /// 根据属性和值获取属性档次
    /// </summary>
    /// <param name="attr"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public uint GetWeaponsoulPropertyGrade(GameCmd.eItemAttribute attr, uint value)
    {
        uint effectId = TransformAttrToEffectId(attr, value);
        if (effectId == 0)
            return 0;
        table.WeaponSoulPropertyGradeDataBase gradeDataBase = DataManager.Manager<ItemManager>().GetLocalDataBase<table.WeaponSoulPropertyGradeDataBase>(effectId);
        if (null != gradeDataBase)
        {
            return gradeDataBase.grade;
        }
        return 0;
    }

    /// <summary>
    /// 根据effectid获取属性档次
    /// </summary>
    /// <param name="attrPair"></param>
    /// <returns></returns>
    public uint GetWeaponsoulPropertyGrade(GameCmd.PairNumber attrPair)
    {
        return GetWeaponsoulPropertyGrade((GameCmd.eItemAttribute)attrPair.id, attrPair.value);
    }

    /// <summary>
    /// 获取effect dataBase
    /// </summary>
    /// <param name="effectId"></param>
    /// <returns></returns>
    public table.StateDataBase GetStateDataBase(uint effectId)
    {
        return GameTableManager.Instance.GetTableItem<table.StateDataBase>(effectId);
    }

    /// <summary>
    /// 获取Effect DataBase
    /// </summary>
    /// <param name="attr"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public table.StateDataBase GetStateDataBase(GameCmd.eItemAttribute attr, uint value)
    {
        uint effectId = TransformAttrToEffectId(attr, value);
        table.StateDataBase stateDataBase = GetStateDataBase(effectId);
        if (null == stateDataBase)
        {
            Engine.Utility.Log.Error(" GetStateDataBase null id:{0},attr:{1},value:{2}", effectId, attr, value);
        }
        return stateDataBase;
    }

    /// <summary>
    /// 获取属性名称
    /// </summary>
    /// <param name="attr"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public string GetAttrName(GameCmd.eItemAttribute attr, uint value)
    {
        table.StateDataBase stateDataBase = GetStateDataBase(attr, value);
        return (null != stateDataBase) ? stateDataBase.name : "未知";
    }

    public string GetAttrDes(uint effectId)
    {
        table.StateDataBase stateDataBase = GetStateDataBase(effectId);
        return (null != stateDataBase) ? stateDataBase.des : "未知";
    }

    public string GetAttrDes(GameCmd.eItemAttribute attr, uint value)
    {
        table.StateDataBase stateDataBase = GetStateDataBase(attr, value);
        return (null != stateDataBase) ? stateDataBase.des : "未知";
    }

    /// <summary>
    /// 获取属性描述
    /// </summary>
    /// <param name="pair"></param>
    /// <returns></returns>
    public string GetAttrDes(GameCmd.PairNumber pair)
    {
        return GetAttrDes((GameCmd.eItemAttribute)pair.id, pair.value);
    }

    /// <summary>
    /// 获取属性名称
    /// </summary>
    /// <param name="pair"></param>
    /// <returns></returns>
    public string GetAttrName(GameCmd.PairNumber pair)
    {
        return GetAttrName((GameCmd.eItemAttribute)pair.id, pair.value);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pair"></param>
    /// <returns></returns>
    public table.StateDataBase GetStateDataBase(GameCmd.PairNumber pair)
    {
        return GetStateDataBase((GameCmd.eItemAttribute)pair.id, pair.value);
    }

    /// <summary>
    /// 圣魂融合
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="otherId">消耗圣魂id</param>
    /// <param name="autoDQ">自动使用点券</param>
    public void BlendWeaponSoul(uint qwThisId, uint otherId, bool autoDQ)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.UnionWeaponSoulReq(qwThisId, otherId, autoDQ);
    }

    /// <summary>
    /// 圣魂融合响应
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    public void OnBlendWeaponSoul(ItemSerialize data)
    {
        DataManager.Manager<ItemManager>().OnAddItemData(data, (uint)AddItemAction.AddItemAction_Refresh);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTMUHONBLEND, data.qwThisID);
    }

    #endregion

    #region WeaponSoul（进化）
    public enum EvolveAnimStep
    {
        None,
        MaterialBorderBoom = 50028,         //材料框爆
        MaterialFlyEffect =50029,                //飞向目标
        TargetBoom =50030,                 //目标框爆
        StarLight =50031,                  //新型爆点
        FlyToTarget,
    }
    private Dictionary<EvolveAnimStep, uint> muhonStepEffectId = null;
    public const string MUHON_EVLOVE_ANIM_EFFECT_NAME = "MuhonEvloveAnimEffectID";
    /// <summary>
    /// 获取特效ID
    /// </summary>
    /// <param name="step"></param>
    /// <returns></returns>
    public uint GetEvloveAnimEffectIdByStep(EvolveAnimStep step)
    {
        if (step == EvolveAnimStep.FlyToTarget || step == EvolveAnimStep.None)
            return 0;
        if (null == muhonStepEffectId)
            muhonStepEffectId = new Dictionary<EvolveAnimStep, uint>();
        uint effectId = 0;
        if (!muhonStepEffectId.TryGetValue(step,out effectId))
        {
            effectId = GameTableManager.Instance.GetGlobalConfig<uint>(MUHON_EVLOVE_ANIM_EFFECT_NAME, step.ToString());
            muhonStepEffectId.Add(step, effectId);
        }
        return effectId;
    }
    /// <summary>
    /// 圣魂进化
    /// </summary>
    /// <param name="qwThisd"></param>
    /// <param name="otherId"></param>
    /// <param name="autoUseDQ"></param>
    public void EvolutionWeaponSoul(uint qwThisd, List<uint> deputyIds, bool autoUseDQ)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.EvolutionWeaponSoulReq(qwThisd, deputyIds, autoUseDQ);
    }

    /// <summary>
    /// 圣魂进化响应
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="data"></param>
    public void OnEvolutionWeaponSoul(ItemSerialize data)
    {
        DataManager.Manager<ItemManager>().OnAddItemData(data, (uint)AddItemAction.AddItemAction_Refresh);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTMUHONEVOLUTION, data.qwThisID);
    }

    #endregion


    #endregion

    #region Common ()
    /// <summary>
    /// 获取eType装备类型，已装备战力较小的id
    /// </summary>
    /// <param name="eType"></param>
    /// <param name="qwThisId"></param>
    /// <returns></returns>
    public bool TryGetEquipLowPowItem(GameCmd.EquipType eType,out uint qwThisId)
    {
        qwThisId = 0;
        GameCmd.EquipPos[] eposArray = EquipDefine.GetEquipPosByEquipType(eType);
        if (null == eposArray || eposArray.Length == 0)
        {
            return false;
        }
        BaseEquip equipItem = null;
        int minPowerEquip = -1;
        uint tempEquipId = 0;
        for (int i = 0; i < eposArray.Length; i++)
        {
            if (IsEquipPos(eposArray[i], out tempEquipId))
            {
                equipItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseEquip>(tempEquipId);
                if (null != equipItem)
                {
                    if (minPowerEquip == -1 || minPowerEquip > equipItem.Power)
                    {
                        minPowerEquip = (int)equipItem.Power;
                        qwThisId = tempEquipId; 
                    }
                }
            }
        }
        return (qwThisId != 0);
    }

    /// <summary>
    /// 和装备栏比较是否有战斗力提升
    /// </summary>
    /// <param name="id">唯一id或者表格id</param>
    /// <returns></returns>
    public bool IsEquipNeedFightPowerMask(uint id,out bool powerUp)
    {
        powerUp = false;
        BaseEquip baseEquip = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseEquip>(id);
        if (null == baseEquip)
        {
            baseEquip = new BaseEquip(id,null);
        }
        if (!baseEquip.IsEquip || baseEquip.IsWear)
        {
            return false;
        }
        if (!DataManager.IsMatchPalyerJob((int)baseEquip.BaseData.useRole))
        {
            return false;
        }
        GameCmd.EquipPos[] eposArray = EquipDefine.GetEquipPosByEquipType(baseEquip.EType);
        if (null == eposArray || eposArray.Length == 0)
        {
            return false;
        }
        uint qwThisID = 0;
        BaseEquip equipItem = null;
        int minPowerEquip = -1;
        for (int i = 0; i < eposArray.Length;i++ )
        {
            if (IsEquipPos(eposArray[i],out qwThisID))
            {
                equipItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseEquip>(qwThisID);
                if (null != equipItem)
                {
                    if (minPowerEquip == -1 || minPowerEquip > equipItem.Power)
                    {
                        minPowerEquip = (int)equipItem.Power;
                    }
                }
            }
            else
            {
                minPowerEquip = 0;
            }
            
        }
        if (minPowerEquip < baseEquip.Power)
        {
            powerUp = true;
            return true;
        }else if (minPowerEquip >baseEquip.Power)
        {
            powerUp = false;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 是否qwThisId有附加属性或者宝石孔
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="checkHole">是否检查宝石孔</param>
    /// <returns></returns>
    public bool IsItemHaveAdditiveAttrOrGemHole(uint qwThisId, bool checkHole = false)
    {
        bool active = false;
        BaseItem data = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(qwThisId);
        if (null != data)
        {

            if (data.AdditionAttrCount > 0  //1、附件属性条数大于0
                || (checkHole && data.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_HoleNum) > 0)) //2、具有宝石孔
                active = true;
        }
        return active;
    }

    /// <summary>
    /// 分解装备 
    /// </summary>
    /// <param name="qwThisId"></param>
    public void SplitEquip(uint qwThisId)
    {
        List<uint> equiplist = new List<uint>();
        equiplist.Add(qwThisId);
        SplitEquip(equiplist);
    }

    /// <summary>
    /// 分解装备(列表)
    /// </summary>
    /// <param name="qwThisId"></param>
    public void SplitEquip(List<uint> equipIds)
    {
        if (null == equipIds || equipIds.Count == 0)
            return;

        foreach (uint id in equipIds)
        {
            if (!IsEquip(id))
            {
                Engine.Utility.Log.Error(CLASS_NAME + "->SplitEquip failed qwThisId={0} not equip！", id);
                return;
            }

        }

        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.EquipDecomposeReq(equipIds);
    }

    /// <summary>
    /// 分解响应
    /// </summary>
    /// <param name="baseIds">表格id</param>
    public void OnSplitEquip(List<uint> baseIds)
    {
        //if (null == baseIds || baseIds.Count == 0)
        //{
        //    return;
        //}

        //List<string> tips = new List<string>();
        ////分解获得的金币
        //uint splitGold = 0;
        ////分解获得的物品
        //Dictionary<uint, uint> splitGetItemsDic = new Dictionary<uint, uint>();
        //table.EquipSplitDataBase spDb = null;
        //for (int i = 0; i < baseIds.Count; i++)
        //{
        //    spDb = GameTableManager.Instance.GetTableItem<table.EquipSplitDataBase>(baseIds[i]);
        //    if (null == spDb)
        //    {
        //        continue;
        //    }

        //    if (spDb.splitGetMoney > 0)
        //    {
        //        splitGold += spDb.splitGetMoney;
        //    }

        //    if (spDb.splitGetItem != 0 && spDb.splitGetItemNum > 0)
        //    {
        //        if (splitGetItemsDic.ContainsKey(spDb.splitGetItem))
        //        {
        //            splitGetItemsDic[spDb.splitGetItem] += spDb.splitGetItemNum;
        //        }
        //        else
        //        {
        //            splitGetItemsDic.Add(spDb.splitGetItem, spDb.splitGetItemNum);
        //        }
        //    }

        //}
        //string format = "获得{0}x{1}";
        ////合成tips
        ////1金币
        //if (splitGold > 0)
        //{
        //    tips.Add(string.Format(format, "金币", splitGold));
        //}
        //if (splitGetItemsDic.Count > 0)
        //{
        //    BaseItem item = null;
        //    foreach (KeyValuePair<uint, uint> pair in splitGetItemsDic)
        //    {
        //        item = new BaseItem(pair.Key);
        //        if (null == item.BaseData)
        //        {
        //            continue;
        //        }
        //        tips.Add(string.Format(format, item.Name, pair.Value));
        //    }
        //}

        //DataManager.Manager<CoroutineMgr>().StartCorountine(ShowSplitEquipTipsCor(tips));
    }

    /// <summary>
    /// 显示装备分解tips
    /// </summary>
    /// <param name="baseIds">分解装备表格id</param>
    /// <returns></returns>
    public System.Collections.IEnumerator ShowSplitEquipTipsCor(List<string> tips)
    {
        yield return null;
        if (null == tips || tips.Count == 0)
        {
            yield break;
        }
        for (int i = 0; i < tips.Count; i++)
        {
            TipsManager.Instance.ShowTips(tips[i]);
            yield return new UnityEngine.WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    ///  获取精炼装备数据
    /// </summary>
    /// <returns></returns>
    public List<uint> GetForgingEquips()
    {
        List<uint> equipList = new List<uint>();
        List<GameCmd.EquipType> filterEquipType = new List<GameCmd.EquipType>();
        filterEquipType.Add(GameCmd.EquipType.EquipType_SoulOne);
        filterEquipType.Add(GameCmd.EquipType.EquipType_Office);
        equipList.AddRange(DataManager.Manager<EquipManager>().GetEquips(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN, filterEquipType));
        equipList.AddRange(DataManager.Manager<EquipManager>().GetEquips(GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP, filterEquipType));
        return equipList;
    }

    /// <summary>
    /// 获取背包中匹配部位的装备
    /// </summary>
    /// <param name="matchType"></param>
    /// <returns></returns>
    public List<uint> GetMatchEquipInKnapsack(uint matchType)
    {
        if (!ready)
            return null;
        List<uint> equips = DataManager.Manager<ItemManager>().DoFilterItemData(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN, ItemBaseType.ItemBaseType_Equip);
        BaseEquip data = null;
        List<uint> result = new List<uint>();
        if (null != equips && equips.Count > 0)
        {
            result.AddRange(equips);
            for(int i = 0,max = equips.Count;i < max;i++)
            {
                data = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseEquip>(equips[i]);
                if (data.SubType != matchType)
                {
                    result.Remove(data.QWThisID);
                }
            }
        }
        if (result.Count > 0)
        {
            ItemManager.SortItemListBySortId(ref result);
        }
        return result;
    }

    /// <summary>
    /// 获取所有装备（已装备和未装备）
    /// </summary>
    /// <returns></returns>
    public List<uint> GetEquips(GameCmd.PACKAGETYPE ptype, List<GameCmd.EquipType> filterType = null)
    {
        List<uint> result = new List<uint>();
        if (!ready)
            return result;
        List<uint> equips = DataManager.Manager<ItemManager>().DoFilterItemData(ptype, ItemBaseType.ItemBaseType_Equip);
        result.AddRange(equips);
        if (null != filterType && filterType.Count != 0)
        {
            BaseEquip data = null;
            for (int i = 0; i < equips.Count; i++)
            {
                data = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseEquip>(equips[i]);
                if (null == data)
                    continue;
                for (int j = 0; j < filterType.Count; j++)
                {
                    if (data.SubType == (uint)filterType[j])
                    {
                        result.Remove(data.QWThisID);
                        break;
                    }
                }

            }

        }
        if (result.Count > 0)
        {
            ItemManager.SortItemListBySortId(ref result);
        }
        return result;
    }

    /// <summary>
    /// 获取身上穿戴装备
    /// </summary>
    /// <returns></returns>
    public List<uint> GetWearEquips()
    {
        List<uint> result = null;
        if (!ready)
            return result;
        result = DataManager.Manager<ItemManager>().DoFilterItemData(GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP, ItemBaseType.ItemBaseType_Equip);
        
        return result;
    }

    /// <summary>
    /// 获取背包里面目标类型的装备列表
    /// </summary>
    /// <param name="ptype"></param>
    /// <param name="targetType"></param>
    /// <returns></returns>
    public List<BaseEquip> GetEquipsByPackageType(GameCmd.PACKAGETYPE ptype, GameCmd.EquipType targetType)
    {
        if (!ready)
            return null;
        List<BaseItem> equips = DataManager.Manager<ItemManager>().DoFilterItemDataByType(ptype, ItemBaseType.ItemBaseType_Equip);
        List<BaseEquip> result = new List<BaseEquip>();
        for (int i = 0; i < equips.Count; i++)
        {
            BaseEquip data = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseEquip>(equips[i].QWThisID);

            if (data.SubType == (uint)targetType)
            {
                result.Add(equips[i] as BaseEquip);
            }
        }

        return result;
    }
    public List<BaseEquip> GetEquipsByPackageType(GameCmd.PACKAGETYPE ptype, List<GameCmd.EquipType> filterType = null)
    {
        if (!ready)
            return null;
        List<BaseItem> equips = DataManager.Manager<ItemManager>().DoFilterItemDataByType(ptype, ItemBaseType.ItemBaseType_Equip);
        List<BaseEquip> result = new List<BaseEquip>();
        for (int i = 0; i < equips.Count; i++)
        {
            result.Add(equips[i] as BaseEquip);
        }
        if (null != filterType && filterType.Count != 0)
        {
            BaseEquip data = null;
            for (int i = 0; i < equips.Count; i++)
            {
                data = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseEquip>(equips[i].QWThisID);
                for (int j = 0; j < filterType.Count; j++)
                {
                    if (data.SubType == (uint)filterType[j])
                    {
                        result.Remove(data);
                        break;
                    }
                }

            }
        }
        return result;
    }
    /// <summary>
    /// 物品是否为装备
    /// </summary>
    /// <param name="qwThisid"></param>
    /// <returns></returns>
    public bool IsEquip(uint qwThisid)
    {
        Equip data = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(qwThisid);
        return (null != data) ? (data.BaseType == ItemBaseType.ItemBaseType_Equip) : false;
    }

    /// <summary>
    /// 是否两装备位置匹配
    /// </summary>
    /// <param name="equipType1">装备类型1</param>
    /// <param name="equipType2">装备类型2</param>
    /// <returns></returns>
    public bool IsEquipPosMatch(uint equipType1, uint equipType2)
    {
        return (equipType1 == equipType2);
    }

    /// <summary>
    /// 是否已经装备该装备类型的道具
    /// </summary>
    /// <param name="equipType">装备类型</param>
    /// <param name="equipId">已装备id</param>
    /// <returns></returns>
    public bool IsEquipType(GameCmd.EquipType equipType, out uint equipId)
    {
        equipId = 0;
        GameCmd.EquipPos[] canEquipPos = EquipDefine.GetEquipPosByEquipType(equipType);
        for (int i = 0; i < canEquipPos.Length; i++)
        {
            if (IsEquipPos(canEquipPos[i], out equipId))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="equipType"></param>
    /// <param name="equipIds"></param>
    /// <returns></returns>
    public bool TryGetEquipPosByType(GameCmd.EquipType equipType, out List<GameCmd.EquipPos> equipPos)
    {
        equipPos = null;
        GameCmd.EquipPos[] canEquipPos = EquipDefine.GetEquipPosByEquipType(equipType);
        if (null != canEquipPos)
        {
            for (int i = 0; i < canEquipPos.Length; i++)
            {
                if (IsEquipPos(canEquipPos[i]))
                {
                    if (null == equipPos)
                    {
                        equipPos = new List<EquipPos>();
                    }
                    equipPos.Add(canEquipPos[i]);
                }
            }
        }
        return (null != equipPos);
    }

    /// <summary>
    /// 该位置是否已经穿戴装备
    /// </summary>
    /// <param name="pos">装备位置</param>
    /// <param name="qwThisID">id</param>
    /// <returns></returns>
    public bool IsEquipPos(GameCmd.EquipPos pos, out uint qwThisID)
    {
        qwThisID = 0;
        List<uint> equipDataList = DataManager.Manager<ItemManager>().GetItemDataByPackageType(PACKAGETYPE.PACKAGETYPE_EQUIP);
        ItemDefine.ItemLocation localLocation;
        if (null != equipDataList && equipDataList.Count > 0)
        {
            BaseItem data = null;
            foreach (uint qwId in equipDataList)
            {
                data = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseItem>(qwId);
                if (null == data)
                    continue;
                if ((int)data.Position.y == (int)pos)
                {
                    qwThisID = data.QWThisID;
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 是否该位置已经装备
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool IsEquipPos(GameCmd.EquipPos pos)
    {
        uint qwThisId = 0;
        return IsEquipPos(pos, out qwThisId);
    }

    /// <summary>
    /// 是否物品已经装备
    /// </summary>
    /// <param name="qwThisID">物品唯一id</param>
    /// <returns></returns>
    public bool IsWearEquip(uint qwThisID)
    {
        bool equip = false;
        if (DataManager.Manager<ItemManager>().ItemDataDic.ContainsKey(qwThisID))
        {
            equip = (DataManager.Manager<ItemManager>().ItemDataDic[qwThisID].PackType == PACKAGETYPE.PACKAGETYPE_EQUIP) ? true : false;
        }
        return equip;
    }

    /// <summary>
    /// 卸下装备
    /// </summary>
    /// <param name="qwThisID"></param>
    public void UnloadEquip(uint qwThisID)
    {
        if (null != DataManager.Instance.Sender)
        {
            uint loc = 0;
            if (DataManager.Manager<KnapsackManager>().TryGetEmptyGridInPackage(PACKAGETYPE.PACKAGETYPE_MAIN, out loc))
            {
                BaseItem data = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseItem>(qwThisID);
                DataManager.Instance.Sender.SwapItemReq(qwThisID, 0, new GameCmd.tItemLocation()
                    {
                        loc = data.ServerLocaltion,
                    }, new GameCmd.tItemLocation()
                {
                    loc = loc,
                });
            }
            else
            {
                DataManager.Manager<EffectDisplayManager>().AddTips("背包空间不足，无法卸下！");
            }
        }
    }

    /// <summary>
    /// 装备物品
    /// </summary>
    /// <param name="srcThisID"></param>
    public void EquipItem(uint qwThisID, bool equip = true)
    {
        if (!DataManager.Manager<ItemManager>().ItemDataDic.ContainsKey(qwThisID))
        {
            Engine.Utility.Log.Error(CLASS_NAME + "-> EquipItem Failed qwThidID = {0} not exist!", qwThisID);
            return;
        }
        if (equip)
        {
            BaseItem data = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseItem>(qwThisID);
            GameCmd.EquipPos equipPos = GameCmd.EquipPos.EquipPos_None;
            GameCmd.EquipPos[] canEquipPos = EquipDefine.GetEquipPosByEquipType((GameCmd.EquipType)data.SubType);
            if (null == canEquipPos || canEquipPos.Length == 0)
            {
                return;
            }
            equipPos = canEquipPos[0];
            if (data.IsEquip && canEquipPos.Length == 2)
            {
                bool fillPower = false;
                //装备替换战斗力低的
                BaseEquip equipItem = null;
                uint equipId = 0;
                uint fightPower= 0;
                for (int i = 0; i < canEquipPos.Length; i++)
                {
                    if (IsEquipPos(canEquipPos[i], out equipId))
                    {
                        equipItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseEquip>(equipId);
                        if (null == equipItem)
                        {
                            continue;
                        }
                        if (!fillPower)
                        {
                            fightPower = equipItem.Power;
                        }
                        if (equipItem.Power < fightPower
                            || (equipItem.Power == fightPower && !fillPower))
                        {
                            equipPos = canEquipPos[i];
                            fightPower = equipItem.Power;
                        }
                        if (!fillPower)
                        {
                            fillPower = true;
                        }
                    }else
                    {
                        equipPos = canEquipPos[i];
                        break;
                    }
                }
            }

            uint desLocalInt = ItemDefine.TransformLocal2ServerLocation(PACKAGETYPE.PACKAGETYPE_EQUIP, new UnityEngine.Vector2(0f, (int)equipPos));
            GameCmd.tItemLocation desLocation = new tItemLocation()
            {
                loc = desLocalInt,
            };
            uint desThisId = 0;
            if (!IsEquipPos(equipPos, out desThisId) || !equip)
            {
                desThisId = 0;
            }

            if (null != DataManager.Instance.Sender)
                DataManager.Instance.Sender.SwapItemReq(qwThisID, desThisId, data.ServerData.pos, desLocation);
        }
        else
        {
            DataManager.Manager<EquipManager>().UnloadEquip(qwThisID);
        }

    }

    /// <summary>
    /// 修理装备
    /// </summary>
    /// <param name="equipList">装备列表</param>
    public void RepairEquip(List<uint> equipList)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.EquipRepairReq(equipList);
    }

    /// <summary>
    /// 修理装备
    /// </summary>
    /// <param name="qwThisId"></param>
    public void RepairEquip(uint qwThisId)
    {
        List<uint> repairIds = new List<uint>();
        repairIds.Add(qwThisId);
        RepairEquip(repairIds);
    }

    /// <summary>
    /// 修理所有已经装备的装备
    /// </summary>
    public void RepairAllEquip()
    {
        List<uint> equipList = GetEquips(PACKAGETYPE.PACKAGETYPE_EQUIP);
        if (null == equipList || equipList.Count == 0)
        {
            TipsManager.Instance.ShowTips("没有要修理的装备");
            return;
        }
        RepairEquip(equipList);
    }

    /// <summary>
    /// 修理装备需要的费用
    /// </summary>
    /// <param name="qwThisIds">装备列表</param>
    /// <returns></returns>
    public MallDefine.CurrencyData GetEquipRepairCost(List<uint> qwThisIds)
    {
        MallDefine.CurrencyData data = new MallDefine.CurrencyData(MoneyType.MoneyType_Gold, 0);
        if (null == qwThisIds || qwThisIds.Count == 0)
            return data;
        uint cost = 0;
        for (int i = 0; i < qwThisIds.Count;i++ )
        {
            data = GetEquipRepairCost(qwThisIds[i]);
            if (null != data)
                cost += data.Num;
        }
        data.Num = cost;
        return data;
    }

    /// <summary>
    /// 修理装备需要的费用(单件装备修理金币价格=（（最大耐久－当前耐久）/ 最大耐久）×装备的购买价格)
    /// </summary>
    /// <param name="qwThisId">单个装备</param>
    /// <returns></returns>
    public MallDefine.CurrencyData GetEquipRepairCost(uint qwThisId)
    {
        MallDefine.CurrencyData data = new MallDefine.CurrencyData(MoneyType.MoneyType_Gold, 0);
        Equip itemData = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(qwThisId);
        int cost = 0;
        if (null != itemData)
        {
            cost = UnityEngine.Mathf.CeilToInt((itemData.MaxDur - itemData.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Dur)) *1.0f
                / itemData.MaxDur * itemData.BaseData.buyPrice * RepairDiscount);
            data.Num = (uint)cost;
        }
        return data;
    }

    /// <summary>
    ///通过装备小类获取pType里的装备列表
    /// </summary>
    /// <param name="eType">装备小类</param>
    /// <param name="pType">背包类型</param>
    /// <returns></returns>
    public List<uint> GetEquipByEquipType(GameCmd.EquipType eType,GameCmd.PACKAGETYPE pType = PACKAGETYPE.PACKAGETYPE_MAIN)
    {
        ItemManager iMgr = DataManager.Manager<ItemManager>();
        List<uint> equipList = new List<uint>();
        List<uint> resultList = new List<uint>();
        equipList.AddRange(iMgr.GetItemByType(ItemBaseType.ItemBaseType_Equip, pType));
        if(equipList.Count > 0)
        {
            BaseItem data = null;
            for(int i =0;i< equipList.Count;i++)
            {
                data = iMgr.GetBaseItemByQwThisId(equipList[i]);
                if (null == data)
                    continue;
                if (data.BaseData.subType == (uint)eType)
                    resultList.Add(equipList[i]);
            }
        }
        return resultList;
    }

    public GameCmd.EquipType GetEquipTypeByEquipPos(GameCmd.EquipPos pos)
    {
        EquipType type = (EquipType)(int)pos;
        if (pos == EquipPos.EquipPos_None)
        {
            return EquipType.EquipType_None;
        }
        else if (pos == EquipPos.EquipPos_Hat)
        {
            return EquipType.EquipType_Hat;
        }
        else if (pos == EquipPos.EquipPos_Shoulder)
        {
            return EquipType.EquipType_Shoulder;
        }
        else if (pos == EquipPos.EquipPos_Coat)
        {
            return EquipType.EquipType_Coat;
        }
        else if (pos == EquipPos.EquipPos_Leg)
        {
            return EquipType.EquipType_Leg;
        }
        else if (pos == EquipPos.EquipPos_AdornlOne)
        {
            return EquipType.EquipType_AdornlOne;
        }
        else if (pos == EquipPos.EquipPos_AdornlTwo)
        {
            return EquipType.EquipType_AdornlOne;
        }
        else if (pos == EquipPos.EquipPos_Shield)
        {
            return EquipType.EquipType_Shield;
        }
        else if (pos == EquipPos.EquipPos_Equip)
        {
            return EquipType.EquipType_Equip;
        }
        else if (pos == EquipPos.EquipPos_Shoes)
        {
            return EquipType.EquipType_Shoes;
        }
        else if (pos == EquipPos.EquipPos_Cuff)
        {
            return EquipType.EquipType_Cuff;
        }
        else if (pos == EquipPos.EquipPos_Belf)
        {
            return EquipType.EquipType_Belf;
        }
        else if (pos == EquipPos.EquipPos_Capes)
        {
            return EquipType.EquipType_Capes;
        }
        else if (pos == EquipPos.EquipPos_Necklace)
        {
            return EquipType.EquipType_Necklace;
        }
        else if (pos == EquipPos.EquipPos_Office)
        {
            return EquipType.EquipType_Office;
        }
        else if (pos == EquipPos.EquipPos_SoulOne)
        {
            return EquipType.EquipType_SoulOne;
        }
        else if (pos == EquipPos.EquipPos_SoulTwo)
        {
            return EquipType.EquipType_SoulOne;
        }
        else if (pos == EquipPos.EquipPos_Max)
        {
            return EquipType.EquipType_Max;
        }
        return type;
    }
    #endregion


    #region Strengthen（强化）
    private Dictionary<GameCmd.EquipPos, uint> m_dicGridStrengthen = null;

    public Dictionary<GameCmd.EquipPos, uint> StrengthenGridDic 
    {
        get 
        {
            return m_dicGridStrengthen;
        }
    }

    /// <summary>
    /// 重置强化
    /// </summary>
    private void ResetStrengthen()
    {
        m_iActiveStrengthenSuitLv = 0;
        m_dicGridStrengthen.Clear();
        m_CurGroupSuitData = null;
    }
    /// <summary>
    /// 执行装备格强化
    /// </summary>
    /// <param name="strengthenAll"></param>
    /// <param name="pos"></param>
    public void DoGridStrengthen(bool strengthenAll, GameCmd.EquipPos pos = EquipPos.EquipPos_None)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.GridStrengthenReq(strengthenAll, pos);
        }
    }

    /// <summary>
    /// 强化完成
    /// </summary>
    /// <param name="strengthenAll"></param>
    /// <param name="pos"></param>
    public void OnGridStrengthenCompelte(bool strengthenAll,GameCmd.EquipPos pos)
    {
        DataManager.Manager<EffectDisplayManager>().AddEquipGridStrengthenEffect();
       
    }

    /// <summary>
    /// 服务器下发强化数据
    /// </summary>
    /// <param name="msg"></param>
    public void OnGridStrengthenProp(stStrengthListPropertyUserCmd_S msg)
    {
        if (!msg.is_refresh)
        {
            m_dicGridStrengthen.Clear();
        }

        if (null != msg.list && msg.list.Count > 0)
        {
            StrengthList stData = null;
            for(int i = 0; i < msg.list.Count;i ++)
            {
                stData = msg.list[i];
                if (!m_dicGridStrengthen.ContainsKey(stData.equip_pos))
                {
                    m_dicGridStrengthen.Add(stData.equip_pos, stData.level);
                }else
                {
                    m_dicGridStrengthen[stData.equip_pos] = stData.level;
                }
            }
        }
        //发送强化属性变更消息
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_GRIDSTRENGTHENLVCHANGED, msg.list);
        CheckActiveStrengthenSuit();
        
    }

    /// <summary>
    /// 获取装备格强化等级
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public uint GetGridStrengthenLvByPos(GameCmd.EquipPos pos)
    {
        return m_dicGridStrengthen.ContainsKey(pos) ? m_dicGridStrengthen[pos] : 0;
    }

    /// <summary>
    /// 当前位置是否达到强化最大等级
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool IsGridStrengthenMax(GameCmd.EquipPos pos)
    {
        EquipDefine.LocalGridStrengthenData data = GetNextStrengthDataByPos(pos);
        return (null == data) ? true : false;
    }

    /// <summary>
    /// 获取当前部位的强化数据
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public EquipDefine.LocalGridStrengthenData GetLocalStrengthDataByLvAndPos(uint lv, GameCmd.EquipPos pos)
    {
        if (lv == 0)
        {
            return null ;
        }
        EquipDefine.LocalGridStrengthenData data = EquipDefine.LocalGridStrengthenData.Create((uint)DataManager.Job(), (uint)pos, lv);
        return (null != data && null != data.TableData) ? data : null;
    }

    /// <summary>
    /// 获取当前部位强化数据
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public EquipDefine.LocalGridStrengthenData GetCurStrengthDataByPos(GameCmd.EquipPos pos)
    {
        return GetLocalStrengthDataByLvAndPos(GetGridStrengthenLvByPos(pos), pos);
    }


    /// <summary>
    /// 获取当前部位下一级强化数据
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public EquipDefine.LocalGridStrengthenData GetNextStrengthDataByPos(GameCmd.EquipPos pos)
    {
        uint nextLv = GetGridStrengthenLvByPos(pos) + 1;
        if (MainPlayerHelper.GetPlayerLevel() < nextLv)
        {
            return null;
        }
        return GetLocalStrengthDataByLvAndPos(nextLv, pos);
    }

    /// <summary>
    /// 获取格子强化<等级-数量>对应的字典
    /// </summary>
    /// <returns></returns>
    public Dictionary<uint,uint> GetGridStrengthenLvNum()
    {
        Dictionary<uint, uint> datas = new Dictionary<uint, uint>();
        List<uint> lvs = new List<uint>(m_dicGridStrengthen.Values);
        for (int i = 0; i < lvs.Count;i++ )
        {
            if (lvs[i] == 0)
            {
                continue;
            }

            if (!datas.ContainsKey(lvs[i]))
            {
                datas.Add(lvs[i], 1);
            }else
            {
                datas[lvs[i]]++;
            }
        }
        return datas;
    }

    /// <summary>
    /// 构建强化套装数据
    /// </summary>
    public void BuildStrengthenSuitData()
    {
        List<table.GridStrengthenSuitDataBase> datas = GameTableManager.Instance.GetTableList<table.GridStrengthenSuitDataBase>();
        m_dicGridSuitStrengthenData = new Dictionary<uint,EquipDefine.LocalGridStrengthenGroupSuitData>();
        if (null != datas)
        {
            table.GridStrengthenSuitDataBase db = null;
            for(int i = 0;i < datas.Count;i++)
            {
                db = datas[i];
                if (!m_dicGridSuitStrengthenData.ContainsKey(db.job))
                {
                    m_dicGridSuitStrengthenData.Add(db.job
                        , EquipDefine.LocalGridStrengthenGroupSuitData.Create(db.job,db.triggerPosNum));
                }
                m_dicGridSuitStrengthenData[db.job].AddData(db.suitlv);
            }
        }
    }

    //激活强化套装属性等级
    private uint m_iActiveStrengthenSuitLv = 0;
    public uint ActiveStrengthenSuitLv
    {
        get
        {
            return m_iActiveStrengthenSuitLv;
        }
    }
    //激活颜色套装属性等级
    private uint m_iActiveColorSuitLv = 0;
    public uint ActiveColorSuitLv
    {
        get
        {
            m_iActiveColorSuitLv = 0;
            List<ColorSuitDataBase> colors = GameTableManager.Instance.GetTableList<ColorSuitDataBase>();
            List<uint> thisIdList = GetWearEquips();
            List<BaseItem> items = new List<BaseItem>();
            for (int i = 0; i < thisIdList.Count; i++)
            {
                BaseItem baseItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseItem>(thisIdList[i]);
                if (baseItem != null)
                {
                    if (baseItem.EquipColor >= EquipDefine.EquipColor.Orange)
                    {
                        items.Add(baseItem);
                    }
                }
            }
            for (int j = 0; j < colors.Count; j++)
            {
                uint count = 0;
                for (int x = 0; x < items.Count; x++)
                {
                    if (items[x].Grade >= colors[j].grade)
                    {
                        count++;
                    }
                }
                if (count >= colors[j].equip_num)
                {
                    m_iActiveColorSuitLv = colors[j].level;
                }
            }
            return m_iActiveColorSuitLv;
        }
    }
    //激活宝石套装属性等级
    private uint m_iActiveStoneSuitLv = 0;
    public uint ActiveStoneSuitLv
    {
        get
        {
            m_iActiveStoneSuitLv = 0;
            uint value = 0;
            Dictionary<int, uint> gemInlayData = DataManager.Manager<EquipManager>().GemInlayData;
            List<GemSuitDataBase> gemTables = GameTableManager.Instance.GetTableList<GemSuitDataBase>();         
            List<uint> thisIdList = GetWearEquips();
            for (int m = 0; m < thisIdList.Count; m ++ )
            {
                uint gemid = 0;
                BaseItem baseItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseItem>(thisIdList[m]);
                Equip e = new Equip(baseItem.BaseId,baseItem.ServerData);
                for (EquipManager.EquipGridIndexType i = EquipManager.EquipGridIndexType.First; i < EquipManager.EquipGridIndexType.Max; i++)
                {
                    if (TryGetEquipGridInlayGem(e.EPos, i, out gemid))
                    {
                        BaseItem bs = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(gemid);
                        value += (uint)bs.Grade;
                    }
                }             
            }
        
            for (int i = 0; i < gemTables.Count; i++ )
            {
                if (value >= gemTables[i].gem_all_level)
                {
                    m_iActiveStoneSuitLv = gemTables[i].level;
                }
            }
            return m_iActiveStoneSuitLv;
        }
    }

    /// <summary>
    /// 是否当前等级套装属性激活
    /// </summary>
    /// <param name="suitLv"></param>
    /// <returns></returns>
    public bool IsStrengthenSuiltLvActive(uint suitLv)
    {
        return m_iActiveStrengthenSuitLv >= suitLv;
    }

    private Dictionary<uint, EquipDefine.LocalGridStrengthenGroupSuitData> m_dicGridSuitStrengthenData = null;
    public bool TryGetGridSuitStrengthenGroupData(uint job 
        ,out EquipDefine.LocalGridStrengthenGroupSuitData groupSuitData)
    {
        return m_dicGridSuitStrengthenData.TryGetValue(job, out groupSuitData);
    }

    //本职业最大强化套装属性组数据
    private EquipDefine.LocalGridStrengthenGroupSuitData m_CurGroupSuitData = null;
    public EquipDefine.LocalGridStrengthenGroupSuitData CurGroupSuitData
    {
        get
        {
            if (null == m_CurGroupSuitData)
            {
                if (TryGetGridSuitStrengthenGroupData((uint)DataManager.Job(), out m_CurGroupSuitData))
                {
                    m_CurGroupSuitData.Sort();
                }
            }
            return m_CurGroupSuitData;
        }
    }

    //本职业最大强化套装属性
    public uint MaxSuitStrengthlv
    {
        get
        {
            return CurGroupSuitData.MaxStrengthenSuitLv;
        }
    }

    /// <summary>
    /// 检测激活套装属性
    /// </summary>
    public void CheckActiveStrengthenSuit()
    {
        if (null != CurGroupSuitData)
        {
            List<uint> datas = CurGroupSuitData.GetStrengthenLvDatas();
            if (null != datas)
            {
                EquipDefine.LocalGridStrengthenSuitData suitData = null;
                Dictionary<uint, uint> strengthenLvNums = GetGridStrengthenLvNum();
                List<uint> strengthLvKes = new List<uint>(strengthenLvNums.Keys);
                uint matchNum = 0;
                uint tempActiveSuitLv = m_iActiveStrengthenSuitLv;
                for(int i = 0;i < datas.Count;i++)
                {
                    matchNum = 0;
                    if (datas[i] <= m_iActiveStrengthenSuitLv)
                    {
                        continue;
                    }
                    suitData = CurGroupSuitData.GetLocalStrengthenData(datas[i]);
                    if (null == suitData)
                    {
                        continue;
                    }

                    for (int j = 0; j < strengthLvKes.Count;j++ )
                    {
                        if (strengthLvKes[j] >= suitData.TriggerStrengLv)
                            matchNum += strengthenLvNums[strengthLvKes[j]];
                    }

                    if (matchNum >= CurGroupSuitData.MatchNeedNum)
                    {
                        tempActiveSuitLv++;
                    }else
                    {
                        break;
                    }
                }


                if (m_iActiveStrengthenSuitLv != tempActiveSuitLv)
                {
                    m_iActiveStrengthenSuitLv = tempActiveSuitLv;
                    //发送套装属性激活等级变更消息
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_STRENGTHENACTIVESUITCHANGED, m_iActiveStrengthenSuitLv);
                }
            }
        }
    }

    #endregion

    #region Exchange(装备兑换)
    private Category exchangeCategory = null;
    public Category ExchangeCategory
    {
        get
        {
            return exchangeCategory;
        }
    }
    private Dictionary<uint, EquipDefine.LocalExchangeDB> m_dicExchangeLocalData = null;
    private bool initExchange = false;
    /// <summary>
    /// 初始化兑换数据
    /// </summary>
    private void InitExchange()
    {
        if (initExchange)
        {
            return;
        }
        m_dicExchangeLocalData = new Dictionary<uint, EquipDefine.LocalExchangeDB>();

        exchangeCategory = Category.Create();
        List<table.EquipExchangeDataBase> tableDatas = GameTableManager.Instance.GetTableList<table.EquipExchangeDataBase>();
        if (null != tableDatas)
        {
            table.EquipExchangeDataBase tempDB = null;
            uint cateID = 0;
            Category tempfCategory = null;
            Category tempsCategory = null;
            for(int i = 0,max = tableDatas.Count;i < max;i++)
            {
                tempDB = tableDatas[i];
                //1级页签
                if (!exchangeCategory.TryGetCategory(tempDB.fCateID, out tempfCategory))
                {
                    tempfCategory = Category.Create(tempDB.fCateID, tempDB.fCateName);
                    exchangeCategory.AddCategory(tempfCategory);
                }

                //2级页签
                if (!tempfCategory.TryGetCategory(tempDB.sCateID, out tempsCategory))
                {
                    tempsCategory = Category.Create(tempDB.sCateID, tempDB.sCateName);
                    tempfCategory.AddCategory(tempsCategory);
                }

                //加数据
                tempsCategory.AddData(tempDB.ID);

                m_dicExchangeLocalData.Add(tempDB.ID, EquipDefine.LocalExchangeDB.Create(tempDB.ID));
            }

            exchangeCategory.SortCategoryData((left, right) =>
            {
                return (int)left.Id - (int)right.Id;
            });
            EquipDefine.LocalExchangeDB leftDB = null;
            EquipDefine.LocalExchangeDB rightDB = null;
            exchangeCategory.SortData((left, right) =>
                {
                    leftDB = GetExchangeLocalDB(left);
                    rightDB = GetExchangeLocalDB(right);
                    if (null != leftDB && null != rightDB)
                    {
                        return (int)leftDB.SorID - (int)rightDB.SorID;
                    }
                    return 0;
                });
        }

        
        initExchange = true;
    }

    /// <summary>
    /// 获取兑换本地数据
    /// </summary>
    /// <param name="exchangeID"></param>
    /// <returns></returns>
    public EquipDefine.LocalExchangeDB GetExchangeLocalDB(uint exchangeID)
    {
        EquipDefine.LocalExchangeDB db = null;
        if (m_dicExchangeLocalData.TryGetValue(exchangeID,out db))
        {
            return db;
        }
        return null;
    }

    /// <summary>
    /// 获取兑换分类数据
    /// </summary>
    /// <param name="fCateID">一级分类ID</param>
    /// <param name="sCateID">二级分类ID</param>
    /// <returns></returns>
    public List<uint> GetExchangeCateData(uint fCateID,uint sCateID)
    {
        List<uint> result = null;
        Category tempfC = null;
        Category tempsC = null;
        if (exchangeCategory.TryGetCategory(fCateID ,out tempfC)
            && tempfC.TryGetCategory(sCateID,out tempsC))
        {
            result = tempsC.Datas; 
        }
        return result;
    }

    /// <summary>
    /// 装备兑换
    /// </summary>
    /// <param name="exchangeID"></param>
    /// <param name="num"></param>
    public void DoExchange(uint exchangeID,uint num)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.EquipExchange(exchangeID, num);
        }
    }

    public void OnExchanged(uint exchangeID, uint num)
    {
        EquipDefine.LocalExchangeDB db =  GetExchangeLocalDB(exchangeID);
        if (null != db && num > 0)
        {
            BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(db.TargetID);
            TipsManager.Instance.ShowTips(string.Format("成功兑换{0}x{1}", baseItem.LocalName, db.TargetNum * num));
        }
    }


    #endregion

    public uint GetEquipParticleIDByStrengthenLevel(uint strengthenLv) 
    {
        uint particle_lv= 0;
        for(int i = 0 ; i <equipParticleLv.Count ;i++ )
        {
            if(strengthenLv >=equipParticleLv[i] )
            {
                particle_lv = equipParticleLv[i];
            }
        }
        
        uint particle_id =0;
        if (m_dic_EquipParticleID != null && particle_lv !=0)
        {
           if(m_dic_EquipParticleID.ContainsKey(particle_lv))
           {
                particle_id = m_dic_EquipParticleID[particle_lv];
           }
           else
           {
                particle_id = GameTableManager.Instance.GetGlobalConfig<uint>("EquipStrengthenParticleDic", particle_lv.ToString());
                m_dic_EquipParticleID.Add(particle_lv, particle_id);
           }
        }


        return particle_id;
    }
    Dictionary<uint, uint> m_dic_StonePartice = null;
   public void AddEquipStoneSuitParticle(IRenerTextureObj m_RTObj, uint stoneLv)
    {
        uint stoneParticleid = 0;
        if (m_dic_StonePartice == null)
        {
            m_dic_StonePartice = new Dictionary<uint, uint>();
        }
        if (m_dic_StonePartice.ContainsKey(stoneLv))
        {
            stoneParticleid = m_dic_StonePartice[stoneLv];
        }
        else
        {
            table.GemSuitDataBase table = GameTableManager.Instance.GetTableItem<table.GemSuitDataBase>(stoneLv);
            if (table != null)
            {
                stoneParticleid = table.particle;
                m_dic_StonePartice.Add(stoneLv, stoneParticleid);
            }
        }
      
        if (stoneParticleid != 0)
        {         
            if (m_RTObj != null)
            {
                m_RTObj.AddLinkEffectWithoutEntity(stoneParticleid);
            }
        }

    }
//     public int GetEquipLvByPlayer(Client.IPlayer player)
//     {
//         BaseEquip equipItem = null;
//         uint tempEquipId = 0;
//         int equipLevel = 0;
//         if (IsEquipPos(GameCmd.EquipPos.EquipPos_Coat,out tempEquipId))
//         {
//             equipItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseEquip>(tempEquipId);
//             if (equipItem != null)
//             {
//                 equipLevel= (int)equipItem.EquipGrade;
//             }
//         }
//         return equipLevel;
//    }
}