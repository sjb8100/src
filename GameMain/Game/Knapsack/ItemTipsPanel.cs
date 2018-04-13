using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

public partial class ItemTipsPanel : UIPanelBase
{
    #region property
    private ItemManager imgr = null;
    private EquipManager emgr = null;
    private TextManager tmgr = null;
    //属性条模板
    private GameObject itemTipsLinePrefab;
    //缓存属性objRoot
    private Transform cachePropertyLineRoot = null;
    //缓存属性列表
    private List<UIItemTipsLineGrid> cachePropertyList = new List<UIItemTipsLineGrid>();
    //tips Offset
    private float compareOffset = 175f;
    //物品展示数据
    private UIDefine.TipsPanelData m_data;
    //normalIcon Content
    private UIItemInfoGrid m_normalIconBase;
    private CMResAsynSeedData<CMTexture> m_normalAsynSeedData = null;

    //compareIcon Content
    private UIItemInfoGrid m_compareIconBase;
    private CMResAsynSeedData<CMTexture> m_compareAsynSeedData = null;

    UILabel[] skillLabels = null;
    UILabel[] telentLabels = null;
    public Transform Content
    {
        get
        {
            return m_trans_ItemTipsContent;
        }
    }
    private StringBuilder txtBuilder = null;

    //改名道具ID
    uint m_reNameItemId;
    #endregion

    #region OverrideMethod
    private void ResetLabelColor()
    {
        m_label_EquipPart.color = Color.white;
        m_label_EquipGrade.color = Color.white;
        m_label_EquipPro.color = Color.white;
        m_label_EquipDur.color = Color.white; ;
        m_label_MuhonGrow.color = Color.white;
        m_label_MuhonCurLv.color = Color.white;
        m_label_SkillDes.color = Color.white;
        m_label_SkillUnlock.color = Color.white;
        m_label_MaterialDes.color = Color.white;
        m_label_MaterialGrade.color = Color.white;
        m_label_MaterialNeedLevel.color = Color.white;
        m_label_ConsumptionNeedLevel.color = Color.white;
        m_label_ConsumptionGrade.color = Color.white;
        m_label_ConsumptionDes.color = Color.white;
        m_label_ConsumptionUseLeft.color = Color.white;
        m_label_MountsSpeedAdd.color = Color.white;
        m_label_MountsLife.color = Color.white;
        m_label_MountsSkil.color = Color.white;
        m_label_MountsDes.color = Color.white;
        m_label_petGradeValue.color = Color.white;
        m_label_petYhLv.color = Color.white;
        m_label_petLift.color = Color.white;
        m_label_petCharacter.color = Color.white;
        m_label_variableLevel.color = Color.white;
        m_label_InheritingNumber.color = Color.white;
        m_label_ItemName.color = Color.white;
        m_label_StrengthenLv.color = Color.white;
        m_label_ConsumptionOwn.color = Color.white;
        m_label_MaterialOwn.color = Color.white;
        m_label_MuhonNeedLevel.color = Color.white;
        m_label_MuhonFightingPower.color = Color.white;
        m_label_MountGrade.color = Color.white;
        m_label_MountLevel.color = Color.white;
        m_label_EquipNeedLevel.color = Color.white;
        m_label_EquipFightingPower.color = Color.white;
        m_label_MountEggQuality.color = Color.white;
        m_label_MountEggLevel.color = Color.white;
        m_label_MuhonEggNeedLevel.color = Color.white;
        m_label_MuhonEggAttackType.color = Color.white;
        m_label_MuhonEggLevel.color = Color.white;
        m_label_EquipPartCp.color = Color.white;
        m_label_EquipGradeCp.color = Color.white;
        m_label_EquipProCp.color = Color.white;
        m_label_EquipDurCp.color = Color.white;
        m_label_MuhonGrowCp.color = Color.white;
        m_label_MuhonCurLvCp.color = Color.white;
        m_label_ItemNameCp.color = Color.white;
        m_label_StrengthenLvCp.color = Color.white;
        m_label_MuhonNeedLevelCp.color = Color.white;
        m_label_MuhonFightingPowerCp.color = Color.white;
        m_label_EquipNeedLevelCp.color = Color.white;
        m_label_EquipFightingPowerCp.color = Color.white;

    }
    protected override void OnLoading()
    {
        base.OnLoading();
        emgr = DataManager.Manager<EquipManager>();
        imgr = DataManager.Manager<ItemManager>();
        tmgr = DataManager.Manager<TextManager>();
        txtBuilder = new StringBuilder();
        ResetLabelColor();
        if (null == itemTipsLinePrefab)
            itemTipsLinePrefab = UIManager.GetResGameObj(GridID.Uiitemtipslinegrid) as GameObject;
        if (null == cachePropertyLineRoot)
        {
            cachePropertyLineRoot = new GameObject("CachePropertyLineRoot").transform;
            cachePropertyLineRoot.gameObject.SetActive(false);
            cachePropertyLineRoot.parent = transform;
            cachePropertyLineRoot.localPosition = Vector3.zero;
            cachePropertyLineRoot.localScale = Vector3.one;
            cachePropertyLineRoot.localRotation = Quaternion.identity;
            cachePropertyList.Clear();
        }
        InitItemBaseIconGrid(true);
        InitItemBaseIconGrid(false);

        int labelNum = 6;
        skillLabels = new UILabel[labelNum];
        for (int i = 1; i <= labelNum; i++)
        {
            skillLabels[i - 1] = m_trans_skilllabels.Find("Label" + i.ToString()).GetComponent<UILabel>();
            skillLabels[i - 1].enabled = false;
            skillLabels[i - 1].overflowMethod = UILabel.Overflow.ResizeFreely;
        }

        telentLabels = new UILabel[labelNum];
        for (int i = 1; i <= 6; i++)
        {
            telentLabels[i - 1] = m_trans_talentlabels.Find("Label" + i.ToString()).GetComponent<UILabel>();
            telentLabels[i - 1].enabled = false;
            telentLabels[i - 1].overflowMethod = UILabel.Overflow.ResizeFreely;
        }

        //改名道具ID
        m_reNameItemId = (uint)GameTableManager.Instance.GetGlobalConfig<int>("ChangeNameItemID");
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
        ResetTipUI();
        if (null != data && data is UIDefine.TipsPanelData)
        {
            this.m_data = data as UIDefine.TipsPanelData;
        }
        CreateTips();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGlobalEvent(true);
        if (null != data && data is UIDefine.TipsPanelData)
        {
            StructFnBtns(GetTipsType(this.m_data.m_data.BaseType, this.m_data.m_data.BaseData.subType));
        }
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    protected override void OnHide()
    {
        base.OnHide();
        Release();
        RegisterGlobalEvent(false);
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_normalIconBase)
        {
            m_normalIconBase.Release(false);
        }

        if (null != m_compareIconBase)
        {
            m_compareIconBase.Release(false);
        }

        if (null != m_normalAsynSeedData)
        {
            m_normalAsynSeedData.Release(depthRelease);
            m_normalAsynSeedData = null;
        }

        if (null != m_compareAsynSeedData)
        {
            m_compareAsynSeedData.Release(depthRelease);
            m_compareAsynSeedData = null;
        }
    }

    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        HideSelf();
    }
    #endregion

    #region Function

    private void InitItemBaseIconGrid(bool normal)
    {
        bool needInit = false;
        Transform ts = null;
        if ((normal && null == m_normalIconBase)
            || (!normal && null == m_compareIconBase))
        {
            needInit = true;
            ts = (normal) ? m_trans_IconContent : m_trans_IconContentCompare;
        }
        GameObject iconTep;
        UIItemInfoGrid iconBase = null;
        if (needInit)
        {
            iconTep = NGUITools.AddChild(ts.gameObject
                , UIManager.GetResGameObj(GridID.Uiiteminfogrid) as GameObject);
            if (null != iconTep)
            {
                iconBase = iconTep.GetComponent<UIItemInfoGrid>();
                if (null == iconBase)
                {
                    iconBase = iconTep.AddComponent<UIItemInfoGrid>();
                }
                iconBase.Reset();
            }
            if (normal)
            {
                m_normalIconBase = iconBase;
            }
            else
            {
                m_compareIconBase = iconBase;
            }
        }
        else
        {
            iconBase = (normal) ? m_normalIconBase : m_compareIconBase;
            if (null != iconBase)
            {
                iconBase.Reset();
            }
        }
    }
    public void ActiveRightBtn(bool active)
    {
        //m_trans_NormalContent.Find("NormalRight").gameObject.SetActive(active);
    }

    /// <summary>
    /// 重置信息
    /// </summary>
    private void ResetInfos()
    {
        //Top 
        if (null != m_trans_Equip && m_trans_Equip.gameObject.activeSelf)
            m_trans_Equip.gameObject.SetActive(false);
        if (null != m_trans_Consumption && m_trans_Consumption.gameObject.activeSelf)
            m_trans_Consumption.gameObject.SetActive(false);
        if (null != m_trans_Material && m_trans_Material.gameObject.activeSelf)
            m_trans_Material.gameObject.SetActive(false);
        if (null != m_trans_Mounts && m_trans_Mounts.gameObject.activeSelf)
            m_trans_Mounts.gameObject.SetActive(false);
        if (null != m_trans_Muhon && m_trans_Muhon.gameObject.activeSelf)
            m_trans_Muhon.gameObject.SetActive(false);
        if (null != m_trans_PowerContent && m_trans_PowerContent.gameObject.activeSelf)
            m_trans_PowerContent.gameObject.SetActive(false);
        if (null != m_trans_PowerContentCp && m_trans_PowerContentCp.gameObject.activeSelf)
            m_trans_PowerContentCp.gameObject.SetActive(false);
        if (null != m_trans_MountsEgg && m_trans_MountsEgg.gameObject.activeSelf)
            m_trans_MountsEgg.gameObject.SetActive(false);
        if (null != m_trans_MuhonEgg && m_trans_MuhonEgg.gameObject.activeSelf)
            m_trans_MuhonEgg.gameObject.SetActive(false);
        if (null != m_label_StrengthenLv && m_label_StrengthenLv.gameObject.activeSelf)
            m_label_StrengthenLv.gameObject.SetActive(false);
        if (null != m_label_StrengthenLvCp && m_label_StrengthenLvCp.gameObject.activeSelf)
            m_label_StrengthenLvCp.gameObject.SetActive(false);


        //Part
        if (null != m_trans_PartConsumption && m_trans_PartConsumption.gameObject.activeSelf)
            m_trans_PartConsumption.gameObject.SetActive(false);
        if (null != m_trans_PartMaterial && m_trans_PartMaterial.gameObject.activeSelf)
            m_trans_PartMaterial.gameObject.SetActive(false);
        if (null != m_trans_PartMuhon && m_trans_PartMuhon.gameObject.activeSelf)
            m_trans_PartMuhon.gameObject.SetActive(false);
        if (null != m_trans_PartSkill && m_trans_PartSkill.gameObject.activeSelf)
            m_trans_PartSkill.gameObject.SetActive(false);
        if (null != m_trans_PartMounts && m_trans_PartMounts.gameObject.activeSelf)
            m_trans_PartMounts.gameObject.SetActive(false);
        if (null != m_trans_PartEquip && m_trans_PartEquip.gameObject.activeSelf)
            m_trans_PartEquip.gameObject.SetActive(false);
        if (null != m_trans_PartMuhonMountsEgg && m_trans_PartMuhonMountsEgg.gameObject.activeSelf)
            m_trans_PartMuhonMountsEgg.gameObject.SetActive(false);

        //属性
        if (null != m_trans_BaseAttrContent && m_trans_BaseAttrContent.gameObject.activeSelf)
            m_trans_BaseAttrContent.gameObject.SetActive(false);
        if (null != m_trans_AdditiveAttrContent && m_trans_AdditiveAttrContent.gameObject.activeSelf)
            m_trans_AdditiveAttrContent.gameObject.SetActive(false);
        if (null != m_trans_AdvancedAttrContent && m_trans_AdvancedAttrContent.gameObject.activeSelf)
            m_trans_AdvancedAttrContent.gameObject.SetActive(false);
        if (null != m_trans_GemAttrContent && m_trans_GemAttrContent.gameObject.activeSelf)
            m_trans_GemAttrContent.gameObject.SetActive(false);

        //----------------------------------------------------------
        //比较
        if (null != m_trans_EquipCp && m_trans_EquipCp.gameObject.activeSelf)
            m_trans_EquipCp.gameObject.SetActive(false);
        if (null != m_trans_MuhonCp && m_trans_MuhonCp.gameObject.activeSelf)
            m_trans_MuhonCp.gameObject.SetActive(false);

        //Part
        if (null != m_trans_PartMuhonCp && m_trans_PartMuhonCp.gameObject.activeSelf)
            m_trans_PartMuhonCp.gameObject.SetActive(false);
        if (null != m_trans_PartEquipCp && m_trans_PartEquipCp.gameObject.activeSelf)
            m_trans_PartEquipCp.gameObject.SetActive(false);

        //属性
        if (null != m_trans_BaseAttrContentCp && m_trans_BaseAttrContentCp.gameObject.activeSelf)
            m_trans_BaseAttrContent.gameObject.SetActive(false);
        if (null != m_trans_AdditiveAttrContentCp && m_trans_AdditiveAttrContentCp.gameObject.activeSelf)
            m_trans_AdditiveAttrContentCp.gameObject.SetActive(false);
        if (null != m_trans_AdvancedAttrContentCp && m_trans_AdvancedAttrContentCp.gameObject.activeSelf)
            m_trans_AdvancedAttrContentCp.gameObject.SetActive(false);
        if (null != m_trans_GemAttrContentCp && m_trans_GemAttrContentCp.gameObject.activeSelf)
            m_trans_GemAttrContentCp.gameObject.SetActive(false);

        if (null != m_trans_TipsContentCompare && m_trans_TipsContentCompare.gameObject.activeSelf)
        {
            m_trans_TipsContentCompare.gameObject.SetActive(false);
            m_trans_TipsContentCompare.localPosition = new Vector3(-180, 0, 0);
        }
        if (null != m_trans_TipsContent)
        {
            m_trans_TipsContent.localPosition = new Vector3(0, 0, 0);
        }
    }

    /// <summary>
    /// 创建装备部分
    /// </summary>
    private void CreateEquipPart()
    {
        Equip equip = new Equip(m_data.m_data.BaseId, m_data.m_data.ServerData);
        string vTempTxt = "";
        if (null != m_trans_Equip && !m_trans_Equip.gameObject.activeSelf)
        {
            m_trans_Equip.gameObject.SetActive(true);
        }

        //装备部位
        if (null != m_label_EquipPart)
        {
            vTempTxt = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.ItemTips_Part
                , EquipDefine.GetEquipTypeName((GameCmd.EquipType)m_data.m_data.BaseData.subType));
            m_label_EquipPart.text = vTempTxt;
            //m_label_EquipPart.text = string.Format("部位：{0}"
            //    , );
        }
        //战斗力
        if (null != m_label_EquipFightingPower)
        {
            vTempTxt = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.ItemTips_Fpower
               , equip.Power);
            m_label_EquipFightingPower.text = ColorManager.GetColorString(ColorType.JZRY_Tips_Light, vTempTxt);
            //m_label_EquipFightingPower.text = string.Format("战斗力：{0}", equip.Power);
        }

        //装备标示
        if (null != m_trans_EquipMask && m_trans_EquipMask.gameObject.activeSelf != emgr.IsWearEquip(m_data.m_data.QWThisID))
        {
            m_trans_EquipMask.gameObject.SetActive(emgr.IsWearEquip(m_data.m_data.QWThisID));
        }
        bool visibleStMask = equip.IsWear && EquipDefine.CanPosStrengthen(equip.EPos);
        if (null != m_label_StrengthenLv)
        {
            if (m_label_StrengthenLv.gameObject.activeSelf != visibleStMask)
                m_label_StrengthenLv.gameObject.SetActive(visibleStMask);
            if (visibleStMask)
            {
                m_label_StrengthenLv.text = emgr.GetGridStrengthenLvByPos(equip.EPos).ToString();
            }
        }

        float gap = 0;
        Vector3 tempPos = Vector3.zero;
        Vector3 gapPos = Vector3.zero;
        tempPos.y = -64f;
        if (null != m_trans_PartEquip && !m_trans_PartEquip.gameObject.activeSelf)
        {
            m_trans_PartEquip.gameObject.SetActive(true);
        }

        //档次
        if (null != m_label_EquipGrade)
        {
            vTempTxt = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.ItemTips_Grade
               , m_data.m_data.BaseData.grade);
            m_label_EquipGrade.text = vTempTxt;
        }
        //需要等级
        if (null != m_label_EquipNeedLevel)
        {
            string nlv = equip.BaseData.useLevel.ToString();
            if (!DataManager.IsMatchPalyerLv((int)equip.BaseData.useLevel))
            {
                nlv = ColorManager.GetColorString(ColorType.JZRY_Txt_Red, equip.BaseData.useLevel.ToString());
            }
            vTempTxt = tmgr.GetLocalFormatText(LocalTextType.ItemTips_NeedLv, nlv);
            m_label_EquipNeedLevel.text = ColorManager.GetColorString(ColorType.JZRY_Tips_Light, vTempTxt);
        }
        //职业
        if (null != m_label_EquipPro)
        {
            string pstr = DataManager.Manager<TextManager>().GetProfessionName((GameCmd.enumProfession)m_data.m_data.BaseData.useRole);
            if (!DataManager.IsMatchPalyerJob((int)m_data.m_data.BaseData.useRole))
            {
                pstr = ColorManager.GetColorString(ColorType.JZRY_Txt_Red, pstr);
            }
            vTempTxt = tmgr.GetLocalFormatText(LocalTextType.ItemTips_Pro, pstr);
            m_label_EquipPro.text = ColorManager.GetColorString(ColorType.White, vTempTxt);
        }
        //耐久
        if (null != m_label_EquipDur)
        {
            string durStr = string.Format("{0}/{1}", equip.CurDisplayDur, equip.MaxDisplayDur);
            if (!equip.HaveDurable)
            {
                durStr = ColorManager.GetColorString(ColorType.JZRY_Txt_Red, durStr);
                m_label_EquipDur.text = string.Format("耐久：{0}", durStr);
            }
            else 
            {
                vTempTxt = tmgr.GetLocalFormatText(LocalTextType.ItemTips_Dur, equip.CurDisplayDur, equip.MaxDisplayDur);
                m_label_EquipDur.text = ColorManager.GetColorString(ColorType.White, vTempTxt);
            }
           
        }

        //基础属性
        if (null != m_trans_BaseAttrContent && !m_trans_BaseAttrContent.gameObject.activeSelf)
        {
            m_trans_BaseAttrContent.gameObject.SetActive(true);
        }
        m_trans_BaseAttrContent.localPosition = tempPos;

        gapPos = Vector3.zero;
        EquipDefine.LocalGridStrengthenData baseSData = (equip.IsWear && EquipDefine.CanPosStrengthen(equip.EPos))
            ? emgr.GetCurStrengthDataByPos(equip.EPos) : null;
        string strengAttrTxt = "";
        List<EquipDefine.EquipBasePropertyData> baseProData = emgr.GetEquipRefineBasePropertyPromote(m_data.m_data.BaseId, (int)(m_data.m_data.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_RefineLevel)));
        List<EquipDefine.EquipBasePropertyData> baseStrengthenData = (null != baseSData) ? baseSData.BaseProp : null;

        if (null != baseProData && baseProData.Count > 0)
        {
            for (int i = 0; i < baseProData.Count; i++)
            {
                if (null != baseStrengthenData)
                {
                    for (int j = 0; j < baseStrengthenData.Count; j++)
                    {
                        strengAttrTxt = "";
                        if (baseStrengthenData[j].popertyType == baseProData[i].popertyType)
                        {
                            strengAttrTxt = ColorManager.GetNGUIColorOfType(ColorType.JZRY_Tips_Blue)
                                + string.Format("(+{0})", baseStrengthenData[j].ToString());
                            break;
                        }
                    }
                }
                gapPos.y -= 30f;
                Add(new ItemTipsLineGridData()
                {
                    TipsLineType = UIItemTipsLineGrid.TipsLineType.Txt,
                    Description = baseProData[i].Name + baseProData[i].ToString() + strengAttrTxt,
                    ColorType = ColorType.White,
                }, m_trans_BaseAttrContent, gapPos);
            }
        }
        tempPos.y += gapPos.y;

        //附加属性
        gapPos = Vector3.zero;
        if (m_data.m_data.AdditionAttrCount > 0)
        {
            tempPos.y -= 30f;
            List<GameCmd.PairNumber> addAttr = m_data.m_data.GetAdditiveAttr();
            if (null != m_trans_AdditiveAttrContent && !m_trans_AdditiveAttrContent.gameObject.activeSelf)
            {
                m_trans_AdditiveAttrContent.gameObject.SetActive(true);
            }
            m_trans_AdditiveAttrContent.localPosition = tempPos;

            GameCmd.PairNumber pNumber;
            for (int i = 0; i < addAttr.Count; i++)
            {
                pNumber = addAttr[i];
                gapPos.y -= 30;
                Add(new ItemTipsLineGridData()
                {
                    TipsLineType = UIItemTipsLineGrid.TipsLineType.Attr,
                    AttrGrade = emgr.GetAttrGrade(pNumber),
                    Description = emgr.GetAttrDes(pNumber),
                    ColorType = ColorType.JZRY_Tips_Orange,
                }, m_trans_AdditiveAttrContent, gapPos);
            }

        }
        tempPos.y += gapPos.y;

        ////高级属性
        //tempPos.y -= 30f;
        //gapPos = Vector3.zero;
        //if (null != m_trans_AdvancedAttrContent && !m_trans_AdvancedAttrContent.gameObject.activeSelf)
        //{
        //    m_trans_AdvancedAttrContent.gameObject.SetActive(true);
        //}
        //m_trans_AdvancedAttrContent.localPosition = tempPos;

        //string advanceAttrStrDes = "";
        //GameCmd.PairNumber advancePN = emgr.GetAdvanceAttrByEquipPos((int)(equip.EPos));
        //advanceAttrStrDes += emgr.GetAttrDes(advancePN);
        //if ((m_data.m_data.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_RefineLevel) != EquipManager.REFINE_MAX_LEVEL))
        //{
        //    advanceAttrStrDes += " (精炼7级以后激活)";
        //}

        //gapPos.y -= 30f;
        //UIItemTipsLineGrid linGrid = Add(new ItemTipsLineGridData()
        //{
        //    TipsLineType = UIItemTipsLineGrid.TipsLineType.Txt,
        //    Description = advanceAttrStrDes,
        //    ColorType = ColorType.JZRY_Tips_Orange,
        //}, m_trans_AdvancedAttrContent, gapPos);
        //gapPos.y -= linGrid.Height;
        //tempPos.y += gapPos.y;

        //宝石属性
        gapPos = Vector3.zero;
        int activeGemHole = (int)m_data.m_data.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_HoleNum);
        UILabel gemAttrLab = m_trans_GemAttrContent.GetComponentInChildren<UILabel>();
        if (null != gemAttrLab)
        {
            gemAttrLab.text = "宝石属性";
        }
        tempPos.y -= 30f;
        if (null != m_trans_GemAttrContent && !m_trans_GemAttrContent.gameObject.activeSelf)
        {
            m_trans_GemAttrContent.gameObject.SetActive(true);
        }
        m_trans_GemAttrContent.localPosition = tempPos;

        bool isWear = false;
        //其他玩家的默认穿戴
        bool other = m_data.m_dicGem != null && m_data.m_nPlayerLevel > 0;
        if (other)
        {
            isWear = true;
        }
        else
        {
            isWear = emgr.IsWearEquip(m_data.m_data.QWThisID);
        }
        TextManager tmg = DataManager.Manager<TextManager>();
        for (EquipManager.EquipGridIndexType i = EquipManager.EquipGridIndexType.None + 1; i < EquipManager.EquipGridIndexType.Max; i++)
        {
            gapPos.y -= 30f;
            string des = "";
            string spriteName = ItemDefine.ICON_NULL;
            bool isUnlock = false;
            if (isWear)
            {
                if (emgr.IsUnlockEquipGridIndex(i, m_data.m_nPlayerLevel))
                {
                    isUnlock = true;
                    uint inlayGemBaseId = 0;
                    if (TryGetGemBaseID(equip.EPos, (int)i, out inlayGemBaseId) || emgr.TryGetEquipGridInlayGem(equip.EPos, i, out inlayGemBaseId))
                    {
                        Gem gem = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<Gem>(inlayGemBaseId, ItemDefine.ItemDataType.Gem);
                        spriteName = gem.Icon;
                        des = (activeGemHole > 0) ? tmg.GetLocalFormatText(LocalTextType.Local_TXT_FM_Tips_UnlockActivation, gem.AttrDes)
                        : tmg.GetLocalFormatText(LocalTextType.Local_TXT_FM_Tips_UnLockInActivation, gem.AttrDes);
                    }
                    else
                    {
                        spriteName = ItemDefine.ICON_NULL;
                        des = (activeGemHole > 0) ? tmg.GetLocalText(LocalTextType.Local_TXT_Tips_NoneActivation)
                        : tmg.GetLocalText(LocalTextType.Local_TXT_Tips_NoneInActivation);
                    }
                }
                else
                {
                    int openLevel = emgr.GetEquipGridUnlockLevel(i);
                    spriteName = UIDefine.SMAMLL_LOCK;
                    des = (activeGemHole > 0) ? string.Format(tmg.GetLocalText(LocalTextType.Local_TXT_Tips_LockActivation), openLevel)
                        : string.Format(tmg.GetLocalText(LocalTextType.Local_TXT_Tips_LockInActivation), openLevel);
                }
            }
            else
            {
                if (emgr.IsUnlockEquipGridIndex(i))
                {
                    isUnlock = true;
                    spriteName = ItemDefine.ICON_NULL;
                    des = (activeGemHole > 0) ? tmg.GetLocalText(LocalTextType.Local_TXT_Tips_NoneActivation)
                        : tmg.GetLocalText(LocalTextType.Local_TXT_Tips_NoneInActivation);
                }
                else
                {
                    int openLevel = emgr.GetEquipGridUnlockLevel(i);
                    spriteName = UIDefine.SMAMLL_LOCK;
                    des = (activeGemHole > 0) ? string.Format(tmg.GetLocalText(LocalTextType.Local_TXT_Tips_LockActivation), openLevel)
                        : string.Format(tmg.GetLocalText(LocalTextType.Local_TXT_Tips_LockInActivation), openLevel);
                }
            }
            Add(new ItemTipsLineGridData()
            {
                TipsLineType = UIItemTipsLineGrid.TipsLineType.Hole,
                GemName = spriteName,
                Description = des,
                IsLock = !isUnlock,
                ColorType = ColorType.JZRY_Tips_White,
            }, m_trans_GemAttrContent, gapPos);
            activeGemHole--;
        }
        tempPos.y += gapPos.y;

        //描述
        tempPos.y -= 45f;
        Add(new ItemTipsLineGridData()
        {
            TipsLineType = UIItemTipsLineGrid.TipsLineType.Txt,
            Description = ColorManager.GetColorString(ColorType.White, m_data.m_data.Des),
        }, m_trans_PartEquip.parent.parent, tempPos);


    }

    private bool TryGetGemBaseID(GameCmd.EquipPos equipPos, int pos, out uint baseid)
    {
        baseid = 0;
        int index = EquipDefine.BuildEquipGridId(equipPos, pos);
        if (m_data.m_dicGem != null && m_data.m_dicGem.TryGetValue((uint)index, out baseid))
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 装备对比
    /// </summary>
    /// <param name="equip"></param>
    private void CreateEquipPartCompare(Equip equip)
    {
        InitItemBaseIconGrid(false);
        string vTempTxt = "";
        if (null != m_trans_TipsContent)
        {
            m_trans_TipsContent.localPosition = new Vector3(180, 0, 0);
        }
        if (null != m_trans_EquipCp && !m_trans_EquipCp.gameObject.activeSelf)
        {
            m_trans_EquipCp.gameObject.SetActive(true);
        }

        //名称
        if (null != m_label_ItemNameCp)
        {
            //m_label_ItemNameCp.color = ColorManager.GetColor32OfType(ColorType.JZRY_Txt_White);
            m_label_ItemNameCp.text = equip.NameForTips;
        }

        //图标
        m_compareIconBase.SetIcon(true, equip.Icon);
        m_compareIconBase.SetBorder(true, equip.BorderIcon);
        //绑定表示
        m_compareIconBase.SetLockMask(equip.IsBind);
        if (null != m__ItemGradeBgCompare)
        {
            UIManager.GetTextureAsyn((uint)equip.TipsTopIcon, ref m_compareAsynSeedData, () =>
                {
                    if (null != m__ItemGradeBgCompare)
                    {
                        m__ItemGradeBgCompare.mainTexture = null;
                    }
                }, m__ItemGradeBgCompare);
        }
        //装备部位
        if (null != m_label_EquipPartCp)
        {
            m_label_EquipPartCp.text = string.Format("部位：{0}"
                , EquipDefine.GetEquipTypeName((GameCmd.EquipType)equip.BaseData.subType));
        }
        //战斗力
        if (null != m_label_EquipFightingPowerCp)
        {
            vTempTxt = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.ItemTips_Fpower
               , equip.Power);
            m_label_EquipFightingPowerCp.text = ColorManager.GetColorString(ColorType.JZRY_Tips_Light, vTempTxt);
        }

        //装备标示
        if (null != m_trans_EquipMaskCp && m_trans_EquipMaskCp.gameObject.activeSelf != equip.IsWear)
        {
            m_trans_EquipMaskCp.gameObject.SetActive(equip.IsWear);
        }

        bool visibleStMask = equip.IsWear && EquipDefine.CanPosStrengthen(equip.EPos);
        if (null != m_label_StrengthenLvCp)
        {
            if (m_label_StrengthenLvCp.gameObject.activeSelf != visibleStMask)
                m_label_StrengthenLvCp.gameObject.SetActive(visibleStMask);
            if (visibleStMask)
            {
                m_label_StrengthenLvCp.text = emgr.GetGridStrengthenLvByPos(equip.EPos).ToString();
            }
        }

        float gapCp = 0;
        Vector3 tempPosCp = Vector3.zero;
        Vector3 gapPosCp = Vector3.zero;
        tempPosCp.y = -64f;
        if (null != m_trans_PartEquipCp && !m_trans_PartEquipCp.gameObject.activeSelf)
        {
            m_trans_PartEquipCp.gameObject.SetActive(true);
        }

        //档次
        if (null != m_label_EquipGradeCp)
        {
            vTempTxt = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.ItemTips_Grade
               , equip.BaseData.grade);
            m_label_EquipGradeCp.text = vTempTxt;
        }
        //需要等级
        if (null != m_label_EquipNeedLevelCp)
        {

            string nlv = equip.BaseData.useLevel.ToString();
            if (!DataManager.IsMatchPalyerLv((int)equip.BaseData.useLevel))
            {
                nlv = ColorManager.GetColorString(ColorType.JZRY_Txt_Red, equip.BaseData.useLevel.ToString());
            }
            vTempTxt = tmgr.GetLocalFormatText(LocalTextType.ItemTips_NeedLv, nlv);
            m_label_EquipNeedLevelCp.text = ColorManager.GetColorString(ColorType.JZRY_Tips_Light, vTempTxt);

        }
        //职业
        if (null != m_label_EquipProCp)
        {
            string pstr = tmgr.GetProfessionName((GameCmd.enumProfession)equip.BaseData.useRole);
            if (!DataManager.IsMatchPalyerJob((int)equip.BaseData.useRole))
            {
                pstr = ColorManager.GetColorString(ColorType.JZRY_Txt_Red, pstr);
            }
            vTempTxt = tmgr.GetLocalFormatText(LocalTextType.ItemTips_Pro, pstr);
            m_label_EquipProCp.text = ColorManager.GetColorString(ColorType.White, vTempTxt);
        }
        //耐久
        if (null != m_label_EquipDurCp)
        {
            string durStr = string.Format("{0}/{1}", equip.CurDisplayDur, equip.MaxDisplayDur);
            if (!equip.HaveDurable)
            {
                durStr = ColorManager.GetColorString(ColorType.JZRY_Txt_Red, durStr);
                m_label_EquipDurCp.text = string.Format("耐久：{0}", durStr);
            }
            else
            {
                vTempTxt = tmgr.GetLocalFormatText(LocalTextType.ItemTips_Dur, equip.CurDisplayDur, equip.MaxDisplayDur);
                m_label_EquipDurCp.text = ColorManager.GetColorString(ColorType.White, vTempTxt);
            }
        }

        //基础属性
        if (null != m_trans_BaseAttrContentCp && !m_trans_BaseAttrContentCp.gameObject.activeSelf)
        {
            m_trans_BaseAttrContentCp.gameObject.SetActive(true);
        }
        m_trans_BaseAttrContentCp.localPosition = tempPosCp;

        gapPosCp = Vector3.zero;

        EquipDefine.LocalGridStrengthenData baseSDataCP = (equip.IsWear && EquipDefine.CanPosStrengthen(equip.EPos))
            ? emgr.GetCurStrengthDataByPos(equip.EPos) : null;
        string strengAttrTxt = "";
        List<EquipDefine.EquipBasePropertyData> baseStrengthenDataCP = (null != baseSDataCP) ? baseSDataCP.BaseProp : null;
        List<EquipDefine.EquipBasePropertyData> baseProDataCp = emgr.GetEquipRefineBasePropertyPromote(equip.BaseId, (int)equip.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_RefineLevel));
        if (null != baseProDataCp && baseProDataCp.Count > 0)
        {
            for (int i = 0; i < baseProDataCp.Count; i++)
            {
                if (null != baseStrengthenDataCP)
                {
                    for (int j = 0; j < baseStrengthenDataCP.Count; j++)
                    {
                        strengAttrTxt = "";
                        if (baseStrengthenDataCP[j].popertyType == baseProDataCp[i].popertyType)
                        {
                            strengAttrTxt = ColorManager.GetNGUIColorOfType(ColorType.JZRY_Tips_Blue)
                                + string.Format("(+{0})", baseStrengthenDataCP[j].ToString());
                            break;
                        }
                    }
                }
                gapPosCp.y -= 30f;
                Add(new ItemTipsLineGridData()
                {
                    TipsLineType = UIItemTipsLineGrid.TipsLineType.Txt,
                    Description = baseProDataCp[i].Name + baseProDataCp[i].ToString() + strengAttrTxt,
                    ColorType = ColorType.White,
                }, m_trans_BaseAttrContentCp, gapPosCp);
            }
        }
        tempPosCp.y += gapPosCp.y;

        //附加属性
        gapPosCp = Vector3.zero;
        if (equip.AdditionAttrCount > 0)
        {
            tempPosCp.y -= 30f;
            List<GameCmd.PairNumber> addAttrCp = equip.GetAdditiveAttr();
            if (null != m_trans_AdditiveAttrContentCp && !m_trans_AdditiveAttrContentCp.gameObject.activeSelf)
            {
                m_trans_AdditiveAttrContentCp.gameObject.SetActive(true);
            }
            m_trans_AdditiveAttrContentCp.localPosition = tempPosCp;

            GameCmd.PairNumber pNumberCp;
            for (int i = 0; i < addAttrCp.Count; i++)
            {
                pNumberCp = addAttrCp[i];
                gapPosCp.y -= 30;

                Add(new ItemTipsLineGridData()
                {
                    TipsLineType = UIItemTipsLineGrid.TipsLineType.Attr,
                    AttrGrade = emgr.GetAttrGrade(pNumberCp),
                    Description = emgr.GetAttrDes(pNumberCp),
                    ColorType = ColorType.JZRY_Tips_Orange
                }, m_trans_AdditiveAttrContentCp, gapPosCp);
            }

        }
        tempPosCp.y += gapPosCp.y;

        ////高级属性
        //tempPosCp.y -= 30f;
        //gapPosCp = Vector3.zero;
        //if (null != m_trans_AdvancedAttrContentCp && !m_trans_AdvancedAttrContentCp.gameObject.activeSelf)
        //{
        //    m_trans_AdvancedAttrContentCp.gameObject.SetActive(true);
        //}
        //m_trans_AdvancedAttrContentCp.localPosition = tempPosCp;

        //string advanceAttrStrDesCp = "";
        //GameCmd.PairNumber advancePNCp = emgr.GetAdvanceAttrByEquipPos((int)equip.EPos);
        //advanceAttrStrDesCp += emgr.GetAttrDes(advancePNCp);
        //if (equip.RefineLv != EquipManager.REFINE_MAX_LEVEL)
        //{
        //    advanceAttrStrDesCp += " (精炼7级以后激活)";
        //}

        //gapPosCp.y -= 30f;
        //UIItemTipsLineGrid linGridCp = Add(new ItemTipsLineGridData()
        //{
        //    TipsLineType = UIItemTipsLineGrid.TipsLineType.Txt,
        //    Description = advanceAttrStrDesCp,
        //    ColorType = ColorType.JZRY_Tips_Orange,
        //}, m_trans_AdvancedAttrContentCp, gapPosCp);
        //gapPosCp.y -= linGridCp.Height;
        //tempPosCp.y += gapPosCp.y;

        //宝石属性
        gapPosCp = Vector3.zero;
        int activeGemHoleCp = (int)equip.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_HoleNum);
        UILabel gemAttrLabCp = m_trans_GemAttrContentCp.GetComponentInChildren<UILabel>();
        if (null != gemAttrLabCp)
        {
            gemAttrLabCp.text = "宝石属性";
        }
        tempPosCp.y -= 30f;
        if (null != m_trans_GemAttrContentCp && !m_trans_GemAttrContentCp.gameObject.activeSelf)
        {
            m_trans_GemAttrContentCp.gameObject.SetActive(true);
        }
        m_trans_GemAttrContentCp.localPosition = tempPosCp;
        int equipPos = (int)equip.EPos;
        for (EquipManager.EquipGridIndexType i = EquipManager.EquipGridIndexType.None + 1; i < EquipManager.EquipGridIndexType.Max; i++)
        {
            gapPosCp.y -= 30f;
            string des = "";
            string spriteName = ItemDefine.ICON_NULL;
            bool isUnlock = false;
            if (emgr.IsUnlockEquipGridIndex(i))
            {
                isUnlock = true;
                uint inlayGemBaseId = 0;
                if (emgr.TryGetEquipGridInlayGem(equip.EPos, i, out inlayGemBaseId))
                {
                    Gem gem = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<Gem>(inlayGemBaseId, ItemDefine.ItemDataType.Gem);
                    spriteName = gem.Icon;
                    des = (activeGemHoleCp > 0) ? tmgr.GetLocalFormatText(LocalTextType.Local_TXT_FM_Tips_UnlockActivation, gem.AttrDes)
                    : tmgr.GetLocalFormatText(LocalTextType.Local_TXT_FM_Tips_UnLockInActivation, gem.AttrDes);
                }
                else
                {
                    spriteName = ItemDefine.ICON_NULL;
                    des = (activeGemHoleCp > 0) ? tmgr.GetLocalText(LocalTextType.Local_TXT_Tips_NoneActivation)
                    : tmgr.GetLocalText(LocalTextType.Local_TXT_Tips_NoneInActivation);
                }
            }
            else
            {
                int openLevel = emgr.GetEquipGridUnlockLevel(i);
                spriteName = UIDefine.SMAMLL_LOCK;
                des = (activeGemHoleCp > 0) ? string.Format(tmgr.GetLocalText(LocalTextType.Local_TXT_Tips_LockActivation), openLevel)
                    : string.Format(tmgr.GetLocalText(LocalTextType.Local_TXT_Tips_LockInActivation), openLevel);
            }

            Add(new ItemTipsLineGridData()
            {
                TipsLineType = UIItemTipsLineGrid.TipsLineType.Hole,
                GemName = spriteName,
                Description = des,
                IsLock = !isUnlock,
                ColorType = ColorType.JZRY_Tips_White,
            }, m_trans_GemAttrContentCp, gapPosCp);
            activeGemHoleCp--;
        }
        tempPosCp.y += gapPosCp.y;

        //描述
        tempPosCp.y -= 45f;
        Add(new ItemTipsLineGridData()
        {
            TipsLineType = UIItemTipsLineGrid.TipsLineType.Txt,
            Description = equip.Des,
        }, m_trans_PartEquipCp.parent.parent, tempPosCp);
    }

    /// <summary>
    /// 创建凭证信息
    /// </summary>
    private void CreateMaterialPart()
    {
        if (m_data.m_data.IsChips)
        {
            m_normalIconBase.SetChips(true);
        }
        string vTempTxt = "";
        if (null != m_trans_Material && !m_trans_Material.gameObject.activeSelf)
        {
            m_trans_Material.gameObject.SetActive(true);
        }
        //已拥有
        if (null != m_label_MaterialOwn)
        {
            m_label_MaterialOwn.text = string.Format("已拥有：{0}"
                , DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_data.m_data.BaseId));
        }

        if (null != m_trans_PartMaterial && !m_trans_PartMaterial.gameObject.activeSelf)
        {
            m_trans_PartMaterial.gameObject.SetActive(true);
        }
        //档次
        if (null != m_label_MaterialGrade)
        {
            m_label_MaterialGrade.text = string.Format("档次：{0}", m_data.m_data.BaseData.grade);
        }
        //需要等级
        if (null != m_label_MaterialNeedLevel)
        {
            string nlv = m_data.m_data.BaseData.useLevel.ToString();
            if (!DataManager.IsMatchPalyerLv((int)m_data.m_data.BaseData.useLevel))
            {
                nlv = ColorManager.GetColorString(ColorType.JZRY_Txt_Red, m_data.m_data.BaseData.useLevel.ToString());
            }

            vTempTxt = tmgr.GetLocalFormatText(LocalTextType.ItemTips_NeedLv, nlv);
            m_label_MaterialNeedLevel.text = ColorManager.GetColorString(ColorType.JZRY_Tips_Light, vTempTxt);
        }
        //描述
        if (null != m_label_MaterialDes)
        {
            m_label_MaterialDes.text = ColorManager.GetColorString(ColorType.White, m_data.m_data.Des.ToString());
        }

        if (m_data.m_data.IsRuneStone)
        {
            m_normalIconBase.SetRunestoneMask(true, (uint)m_data.m_data.Grade);
        }
    }
    /// <summary>
    /// 创建藏宝图信息
    /// </summary>
    private void CreatTreasureMapPart()
    {
        if (null != m_trans_Consumption && !m_trans_Consumption.gameObject.activeSelf)
        {
            m_trans_Consumption.gameObject.SetActive(true);
        }
        //已拥有
        if (null != m_label_ConsumptionOwn)
        {
            m_label_ConsumptionOwn.text = string.Format("已拥有：{0}"
                , DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_data.m_data.BaseId));
        }

        if (null != m_trans_PartConsumption && !m_trans_PartConsumption.gameObject.activeSelf)
        {
            m_trans_PartConsumption.gameObject.SetActive(true);
        }
        //档次
        if (null != m_label_ConsumptionGrade)
        {
            m_label_ConsumptionGrade.text = string.Format("档次：{0}", m_data.m_data.BaseData.grade);
        }
        //需要等级
        if (null != m_label_ConsumptionNeedLevel)
        {
            string nlv = m_data.m_data.BaseData.useLevel.ToString();
            if (DataManager.Instance.PlayerLv < m_data.m_data.BaseData.useLevel)
            {
                nlv = ColorManager.GetColorString(ColorType.JZRY_Txt_Red, m_data.m_data.BaseData.useLevel.ToString());
            }

            m_label_ConsumptionNeedLevel.text = string.Format("需要等级：{0}", nlv);
        }
        if (null != m_label_ConsumptionDes)
        {
            //藏宝图属性中获取地图id
            uint mapID = m_data.m_data.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Treasure_Use_Map);
            //获取位置id去读取表格拿到要寻路到的具体坐标
            uint index = m_data.m_data.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Treasure_Use_Pos);
            MapDataBase map = GameTableManager.Instance.GetTableItem<MapDataBase>(mapID);
            CangBaoTuDataBase tab = GameTableManager.Instance.GetTableItem<CangBaoTuDataBase>(mapID);
            if (map == null || tab == null)
            {
                Engine.Utility.Log.Error("藏宝图地图ID{0}的数据为空", mapID);
                m_label_ConsumptionDes.text = m_data.m_data.Des;
            }
            else 
            {
                string[] str = tab.pos_list.Split(';');
                if (index > str.Length)
                {
                    return;
                }
                string targetPos = str[(int)index];
                string[] vecStr = targetPos.Split('_');
                if (vecStr.Length == 2)
                {
                    //道具描述
                    string des = ColorManager.GetColorString(ColorType.JZRY_Tips_White, m_data.m_data.Des.ToString());
                    //地图坐标
                    string poss = string.Format("{0}({1},{2})", map.strName, vecStr[0], vecStr[1]);
                    //地图坐标加颜色
                    string positionColorText = ColorManager.GetColorString(ColorType.JZRY_Tips_Green, poss);
                    //糅合后拼接
                    string position = string.Format("{0}\n\n{1}", positionColorText, des);
                    m_label_ConsumptionDes.text = position;
                }
            }                   
        }
        //使用剩余
        if (null != m_label_ConsumptionUseLeft)
        {
            m_label_ConsumptionUseLeft.gameObject.SetActive(m_data.m_data.HasDailyUseLimit);
            //int dayLmit = m_data.m_data.MaxUseNum;
            int leftTimes = DataManager.Manager<ItemManager>().GetItemLeftUseNum(m_data.m_data.BaseId);
            string leftString = "今日还可使用{0}次";
            string leftNumColorStr = (leftTimes > 0) ? ColorManager.GetColorString(ColorType.JZRY_Tips_Green, leftTimes.ToString())
                : ColorManager.GetColorString(ColorType.JZRY_Txt_NotMatchRed, leftTimes.ToString());
            m_label_ConsumptionUseLeft.text = string.Format(leftString, leftNumColorStr);
        }
    }


    /// <summary>
    /// 创建消耗品信息
    /// </summary>
    private void CreateConsumptionPart()
    {
        if (null != m_trans_Consumption && !m_trans_Consumption.gameObject.activeSelf)
        {
            m_trans_Consumption.gameObject.SetActive(true);
        }
        //已拥有
        if (null != m_label_ConsumptionOwn)
        {
            m_label_ConsumptionOwn.text = string.Format("已拥有：{0}"
                , DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_data.m_data.BaseId));
        }

        if (null != m_trans_PartConsumption && !m_trans_PartConsumption.gameObject.activeSelf)
        {
            m_trans_PartConsumption.gameObject.SetActive(true);
        }
        //档次
        if (null != m_label_ConsumptionGrade)
        {
            m_label_ConsumptionGrade.text = string.Format("档次：{0}", m_data.m_data.BaseData.grade);
        }
        //需要等级
        if (null != m_label_ConsumptionNeedLevel)
        {
            string nlv = m_data.m_data.BaseData.useLevel.ToString();
            if (DataManager.Instance.PlayerLv < m_data.m_data.BaseData.useLevel)
            {
                nlv = ColorManager.GetColorString(ColorType.JZRY_Txt_Red, m_data.m_data.BaseData.useLevel.ToString());
            }

            m_label_ConsumptionNeedLevel.text = string.Format("需要等级：{0}", nlv);
        }
        //描述
        if (null != m_label_ConsumptionDes)
        {
            m_label_ConsumptionDes.text = ColorManager.GetColorString(ColorType.JZRY_Tips_White, m_data.m_data.Des.ToString());
        }
        //使用剩余
        if (null != m_label_ConsumptionUseLeft)
        {
            m_label_ConsumptionUseLeft.gameObject.SetActive(false);
            int dayLmit = 3;
            int leftTimes = 1;
            string leftString = "今日还可使用{0}/{1}次";
            string leftNumColorStr = (leftTimes > 0) ? ColorManager.GetColorString(ColorType.JZRY_Green, leftTimes.ToString())
                : ColorManager.GetColorString(ColorType.JZRY_Txt_NotMatchRed, leftTimes.ToString());
            m_label_ConsumptionUseLeft.text = string.Format(leftString, leftNumColorStr, dayLmit);
        }
    }

    /// <summary>
    /// 战魂坐骑蛋
    /// </summary>
    void CreateMuhonMountsEggPart()
    {
        if (m_trans_PartMuhonMountsEgg != null)
        {
            m_trans_PartMuhonMountsEgg.gameObject.SetActive(true);
        }
        uint baseid = m_data.m_data.BaseId;
        if (m_data.m_data.IsPetBall)
        {
            m_widget_skil.transform.localPosition = new UnityEngine.Vector3(m_widget_skil.transform.localPosition.x, -118, 0);
            m_widget_talent.alpha = 1f;
            ShowPetUI();
        }
        else if (m_data.m_data.IsRideBall)
        {
            m_widget_talent.alpha = 0f;
            m_widget_skil.transform.localPosition = new UnityEngine.Vector3(m_widget_skil.transform.localPosition.x, -15, 0);
            ShowRideUI();
        }
    }

    void ShowPetUI()
    {
        if (m_trans_MuhonEgg != null)
        {
            m_trans_MuhonEgg.gameObject.SetActive(true);
        }

        //等级
        if (m_label_MuhonEggLevel != null)
            m_label_MuhonEggLevel.gameObject.SetActive(true);
        uint petLv;
        if (m_data.m_data.TryGetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Pet_Lv, out petLv))
        {
            m_label_MuhonEggLevel.text = string.Format("{0}{1}", petLv.ToString(), CommonData.GetLocalString("级"));
        }
        else
        {
            m_label_MuhonEggLevel.text = string.Format("{0}{1}", 0, CommonData.GetLocalString("级"));
        }

        //携带等级
        if (m_label_MuhonEggNeedLevel != null)
            m_label_MuhonEggNeedLevel.gameObject.SetActive(true);
        uint petBaseId;
        if (m_data.m_data.TryGetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Pet_Base_Id, out petBaseId))
        {
            PetDataBase db = GameTableManager.Instance.GetTableItem<PetDataBase>(petBaseId);
            if (db != null)
            {
                m_label_MuhonEggNeedLevel.text = CommonData.GetLocalString("携带等级") + "  " + db.carryLevel;
                //UIAtlas atlas = UILoader.Instance.GetAtlas(AtlasID.Peticonatlas);
                //if (atlas != null)
                //{
                //    m_sprite_icon.atlas = atlas;
                //    m_sprite_icon.spriteName = db.icon;
                //    m_sprite_icon.MakePixelPerfect();
                //}
                //m_spriteEx_qulity.ChangeSprite(db.petQuality);

                //类型
                m_label_MuhonEggAttackType.text = string.Format("{0}{1}", CommonData.GetLocalString("类型："), db.attackType);
            }
        }

        int labelNum = 6;
        UILabel[] labels = new UILabel[labelNum];
        for (int i = 1; i <= 6; i++)
        {
            labels[i - 1] = m_trans_skilllabels.Find("Label" + i.ToString()).GetComponent<UILabel>();
            labels[i - 1].enabled = false;
        }
        for (int i = 1; i <= 6; i++)
        {
            labels[i - 1] = m_trans_talentlabels.Find("Label" + i.ToString()).GetComponent<UILabel>();
            labels[i - 1].enabled = false;
        }

        //成长状态
        uint petGradeValue =m_data.m_data.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Pet_Grade);
        string st = DataManager.Manager<PetDataManager>().GetGrowStatus((int)petGradeValue);
        string showStr = string.Format("{0}  {1}", CommonData.GetLocalString("成长状态"), st);
        m_label_petGradeValue.text = ColorManager.GetColorString(252, 230, 188, 255, showStr);
        //修为
        uint petYhLv = m_data.m_data.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Pet_Yh_Lv);
        showStr = string.Format("{0}  {1}{2}", CommonData.GetLocalString("修为"), petYhLv, CommonData.GetLocalString("级"));
        m_label_petYhLv.text = ColorManager.GetColorString(252, 230, 188, 255, showStr);


        //寿命
        uint petLift = m_data.m_data.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Pet_Life);
        m_label_petLift.gameObject.SetActive(true);
        showStr = string.Format("{0} {1}", CommonData.GetLocalString("寿命:"), petLift);
        m_label_petLift.text = ColorManager.GetColorString(252, 230, 188, 255, showStr);

        //性格
        uint petCharacter = m_data.m_data.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Pet_Character);
        string cha = DataManager.Manager<PetDataManager>().GetPetCharacterStr((int)petCharacter);
        showStr = string.Format("{0} {1}", CommonData.GetLocalString("性格:"), cha);
        m_label_petCharacter.text = ColorManager.GetColorString(252, 230, 188, 255, showStr);


        uint variableLevel = m_data.m_data.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Pet_By_Lv);
        cha = DataManager.Manager<PetDataManager>().GetJieBianString((int)variableLevel, false);
        showStr = string.Format("{0} {1}", CommonData.GetLocalString("劫变:"), cha);
        m_label_variableLevel.text = ColorManager.GetColorString(252, 230, 188, 255, showStr);


        uint InheritingNumber = m_data.m_data.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Pet_Inherit_time);
        showStr = string.Format("{0} {1}", CommonData.GetLocalString("传承次数:"), InheritingNumber);
        m_label_InheritingNumber.text = ColorManager.GetColorString(252, 230, 188, 255, showStr);


        //智力天赋 0
        uint PetTalentZhili;
        if (m_data.m_data.TryGetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Pet_Cur_Talent_Zhili, out PetTalentZhili))
        {
            UILabel label = telentLabels[0];
            label.text = string.Format("{0}  {1}", CommonData.GetLocalString("智力天赋"), PetTalentZhili);
            label.enabled = true;
            label.depth = 4;
        }

        //敏捷天赋 1
        uint PetTalentMinjie;
        if (m_data.m_data.TryGetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Pet_Cur_Talent_Minjie, out PetTalentMinjie))
        {
            UILabel label = telentLabels[1];
            label.text = string.Format("{0}  {1}", CommonData.GetLocalString("敏捷天赋"), PetTalentMinjie);
            label.enabled = true;
            label.depth = 5;
        }

        //体质天赋 2
        uint PetTalentTizhi;
        if (m_data.m_data.TryGetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Pet_Cur_Talent_Tizhi, out PetTalentTizhi))
        {
            UILabel label = telentLabels[2];
            label.text = string.Format("{0}  {1}", CommonData.GetLocalString("体质天赋"), PetTalentTizhi);
            label.enabled = true;
            label.depth = 6;
        }

        //力量天赋 3
        uint PetTalentLiLiang;
        if (m_data.m_data.TryGetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Pet_Cur_Talent_Liliang, out PetTalentLiLiang))
        {
            UILabel label = telentLabels[3];
            label.text = string.Format("{0}  {1}", CommonData.GetLocalString("力量天赋"), PetTalentLiLiang);
            label.enabled = true;
            label.depth = 7;
        }

        //精神天赋 4
        uint PetTalentJingshen;
        if (m_data.m_data.TryGetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Pet_Cur_Talent_Jingshen, out PetTalentJingshen))
        {
            UILabel label = telentLabels[4];
            label.text = string.Format("{0}  {1}", CommonData.GetLocalString("精神天赋"), PetTalentJingshen);
            label.enabled = true;
            label.depth = 8;
        }

        //技能 1 -- 6
        int skillIdAttributeCount = GameCmd.eItemAttribute.Item_Attribute_Pet_Skill_List6_Id - GameCmd.eItemAttribute.Item_Attribute_Pet_Skill_List1_Id;
        for (int i = 0; i <= skillIdAttributeCount; i++)
        {
            int bute = (int)GameCmd.eItemAttribute.Item_Attribute_Pet_Skill_List1_Id + i;
            uint skillId;
            uint skillLv;
            int skillLvBute = (int)GameCmd.eItemAttribute.Item_Attribute_Pet_Skill_List1_Id + skillIdAttributeCount + i + 1;
            UILabel label = skillLabels[i];
            if (m_data.m_data.TryGetItemAttribute((GameCmd.eItemAttribute)bute, out skillId))
            {
                if (m_data.m_data.TryGetItemAttribute((GameCmd.eItemAttribute)skillLvBute, out skillLv))
                {
                    SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillId, 1);
                    if (db != null)
                    {
                        label.text = string.Format("{0}   lv.{1}", db.strName, skillLv);// db.strName;
                        label.enabled = true;
                        label.depth = 4 + i;
                    }
                }
            }
            else
            {
                label.enabled = false;
            }
        }

        /*
        //技能 1 -- 6
        List<GameCmd.PairNumber> list = m_data.m_data.GetAdditiveAttr();
        for (int i = 0; i < list.Count; i++)
        {
            GameCmd.PairNumber pn = list[i];
            GameCmd.eItemAttribute bute = (GameCmd.eItemAttribute)pn.id;

            if (bute >= GameCmd.eItemAttribute.Item_Attribute_Pet_Skill_List1_Id && bute <= GameCmd.eItemAttribute.Item_Attribute_Pet_Skill_List6_Id)
            {
                int index = bute - GameCmd.eItemAttribute.Item_Attribute_Pet_Skill_List1_Id;

                UILabel label = skillLabels[index];
                if (pn.value != 0)
                {
                    SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(pn.value, 1);
                    if (db != null)
                    {
                        label.text = db.strName;
                        label.enabled = true;
                        label.depth = 4 + index;
                    }
                }
                else
                {
                    label.enabled = false;
                }
            }
        }
        */
    }
    private void ShowRideUI()
    {
        m_label_petLift.text = "";
        m_label_petCharacter.text = "";
        m_label_petYhLv.text = "";
        m_label_InheritingNumber.text = "";
        if (m_trans_MountsEgg != null)
        {
            m_trans_MountsEgg.gameObject.SetActive(true);
        }

        //等级
        if (m_label_MountEggLevel != null)
        {
            m_label_MountEggLevel.gameObject.SetActive(true);
        }
        uint rideLv;
        if (m_data.m_data.TryGetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Ride_Level, out rideLv))
        {
            m_label_MountEggLevel.text = string.Format("{0}{1}", rideLv.ToString(), CommonData.GetLocalString("级"));
        }
        else
        {
            m_label_MountEggLevel.text = string.Format("{0}{1}", 0, CommonData.GetLocalString("级"));
        }

        //品质
        if (m_label_MountEggQuality != null)
        {
            m_label_MountEggQuality.gameObject.SetActive(true);
        }
        uint rideBaseId;
        if (m_data.m_data.TryGetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Ride_Base_Id, out rideBaseId))
        {
            table.RideDataBase ridedata = GameTableManager.Instance.GetTableItem<table.RideDataBase>(rideBaseId);
            if (ridedata != null)
            {
                m_label_MountEggQuality.text = string.Format("品质: {0}", DataManager.Manager<RideManager>().GetRideQualityStr(ridedata.quality));
            }
        }

        for (int i = 0; i < skillLabels.Length; i++)
        {
            skillLabels[i].enabled = false;
        }

        uint baseId;
        if (m_data.m_data.TryGetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Ride_Base_Id, out baseId))
        {
            m_label_petGradeValue.text = string.Format("速度加成: {0}%", RideData.GetSpeedById_Level(baseId, (int)rideLv));
            //技能列表
            table.RideSkillData skilldata = GameTableManager.Instance.GetTableItem<table.RideSkillData>(baseId, (int)rideLv);
            if (skilldata != null)
            {
                for (int n = 0; n < skilldata.skillArray.Count; n++)
                {
                    skillLabels[n].enabled = true;
                    table.RideSkillDes rideSkillDes = GameTableManager.Instance.GetTableItem<table.RideSkillDes>(skilldata.skillArray[n]);
                    if (rideSkillDes != null)
                    {
                        skillLabels[n].text = rideSkillDes.skillName;
                    }
                }
            }
        }

        uint rideLife = m_data.m_data.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Ride_Life);
        m_label_variableLevel.text = string.Format("寿命: {0}", rideLife);
    }


    /// <summary>
    /// 创建骑魂信息
    /// </summary>
    private void CreateMountsPart()
    {
        if (null != m_trans_Mounts && !m_trans_Mounts.gameObject.activeSelf)
        {
            m_trans_Mounts.gameObject.SetActive(true);
        }
        //等级
        if (null != m_label_MountLevel)
        {
            m_label_MountLevel.text = string.Format("等级：{0}", 5);
        }
        //品质
        if (null != m_label_MountGrade)
        {
            m_label_MountLevel.text = string.Format("品质：{0}", "稀有");
        }

        if (null != m_trans_PartMounts && !m_trans_PartMounts.gameObject.activeSelf)
        {
            m_trans_PartMounts.gameObject.SetActive(true);
        }
        //速度加成
        if (null != m_label_MountsSpeedAdd)
        {
            m_label_MountsSpeedAdd.text = string.Format("速度加成：{0}", "500%");
        }
        //寿命
        if (null != m_label_MountsLife)
        {
            m_label_MountsLife.text = string.Format("寿命：{0}", "90");
        }
        //技能
        if (null != m_label_MountsSkil)
        {
            m_label_MountsSkil.text = string.Format("技能：{0}", "6");
        }
        //描述
        if (null != m_label_MountsDes)
        {
            m_label_MountsDes.text = ColorManager.GetColorString(ColorType.Gray, m_data.m_data.Des);
        }
    }

    /// <summary>
    /// 创建圣魂信息
    /// </summary>
    private void CreateMuhonPart()
    {
        string vTempTxt = "";
        Muhon muhon = new Muhon(m_data.m_data.BaseId, m_data.m_data.ServerData);
        if (null != m_trans_Muhon && !m_trans_Muhon.gameObject.activeSelf)
        {
            m_trans_Muhon.gameObject.SetActive(true);
        }

        //战斗力
        if (null != m_label_MuhonFightingPower)
        {
            vTempTxt = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.ItemTips_Fpower
               , muhon.Power);
            m_label_MuhonFightingPower.text = ColorManager.GetColorString(ColorType.JZRY_Tips_Light, vTempTxt);
        }

        //装备标示
        if (null != m_trans_MuhonEquipMask && m_trans_MuhonEquipMask.gameObject.activeSelf != emgr.IsWearEquip(muhon.QWThisID))
        {
            m_trans_MuhonEquipMask.gameObject.SetActive(emgr.IsWearEquip(muhon.QWThisID));
        }

        //是否激活
        m_normalIconBase.SetMuhonMask(true, muhon.StartLevel);
        m_normalIconBase.SetMuhonLv(true, Muhon.GetMuhonLv(muhon));
        float gap = 0;
        Vector3 tempPos = Vector3.zero;
        tempPos.y = -32f;
        Vector3 gapPos = Vector3.zero;
        if (null != m_trans_PartMuhon && !m_trans_PartMuhon.gameObject.activeSelf)
        {
            m_trans_PartMuhon.gameObject.SetActive(true);
        }

        //当前等级
        if (null != m_label_MuhonCurLv)
        {
            vTempTxt = ColorManager.GetColorString(ColorType.JZRY_Tips_Light
                , tmgr.GetLocalFormatText(LocalTextType.knapsack_Commond_24, muhon.Level, muhon.MaxLv));
            m_label_MuhonCurLv.text = vTempTxt;
        }

        //需要等级
        if (null != m_label_MuhonNeedLevel)
        {
            string nlv = muhon.BaseData.useLevel.ToString();
            if (!DataManager.IsMatchPalyerLv((int)muhon.BaseData.useLevel))
            {
                nlv = ColorManager.GetColorString(ColorType.JZRY_Txt_Red, muhon.BaseData.useLevel.ToString());
            }
            vTempTxt = tmgr.GetLocalFormatText(LocalTextType.ItemTips_NeedLv, nlv);
            m_label_MuhonNeedLevel.text = ColorManager.GetColorString(ColorType.JZRY_Tips_Light, vTempTxt);
        }


        //成长
        if (null != m_label_MuhonGrow)
        {
            m_label_MuhonGrow.text = tmgr.GetLocalFormatText(LocalTextType.knapsack_Commond_25, muhon.ExpPercentageFormat);
        }

        //基础属性
        if (null != m_trans_BaseAttrContent && !m_trans_BaseAttrContent.gameObject.activeSelf)
        {
            m_trans_BaseAttrContent.gameObject.SetActive(true);
        }
        m_trans_BaseAttrContent.localPosition = tempPos;

        gapPos = Vector3.zero;
        int level = (int)muhon.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_WeaponSoul_Level);
        if (level == 0)
            level = 1;
        List<EquipDefine.EquipBasePropertyData> baseProData = emgr.GetWeaponSoulBasePropertyData(muhon.BaseId, level);
        if (null != baseProData && baseProData.Count > 0)
        {
            for (int i = 0; i < baseProData.Count; i++)
            {
                gapPos.y -= 30f;
                Add(new ItemTipsLineGridData()
                {
                    TipsLineType = UIItemTipsLineGrid.TipsLineType.Txt,
                    Description = baseProData[i].Name + baseProData[i].ToString(),
                    ColorType = ColorType.White,
                }, m_trans_BaseAttrContent, gapPos);
            }
        }
        tempPos.y += gapPos.y;

        //附加属性
        gapPos = Vector3.zero;
        if (muhon.AdditionAttrCount > 0)
        {
            tempPos.y -= 30f;
            List<GameCmd.PairNumber> addAttr = muhon.GetAdditiveAttr();
            if (null != m_trans_AdditiveAttrContent && !m_trans_AdditiveAttrContent.gameObject.activeSelf)
            {
                m_trans_AdditiveAttrContent.gameObject.SetActive(true);
            }
            m_trans_AdditiveAttrContent.localPosition = tempPos;

            GameCmd.PairNumber pNumber;
            for (int i = 0; i < addAttr.Count; i++)
            {
                pNumber = addAttr[i];
                gapPos.y -= 30;
                Add(new ItemTipsLineGridData()
                {
                    TipsLineType = UIItemTipsLineGrid.TipsLineType.Attr,
                    AttrGrade = emgr.GetAttrGrade(pNumber),
                    Description = emgr.GetAttrDes(pNumber),
                    ColorType = ColorType.JZRY_Tips_Orange,
                }, m_trans_AdditiveAttrContent, gapPos);
            }

        }
        tempPos.y += gapPos.y;

        //描述
        tempPos.y -= 45f;
        Add(new ItemTipsLineGridData()
        {
            TipsLineType = UIItemTipsLineGrid.TipsLineType.Txt,
            Description = ColorManager.GetColorString(ColorType.White, muhon.Des),
        }, m_trans_PartMuhon.parent.parent, tempPos);


    }

    private void CreateMuhonPartCompare(Muhon muhon)
    {
        string vTempTxt = "";
        InitItemBaseIconGrid(false);
        if (null != m_trans_TipsContent)
        {
            m_trans_TipsContent.localPosition = new Vector3(180, 0, 0);
        }

        if (null != m_trans_MuhonCp && !m_trans_MuhonCp.gameObject.activeSelf)
        {
            m_trans_MuhonCp.gameObject.SetActive(true);
        }

        //名称
        if (null != m_label_ItemNameCp)
        {
            m_label_ItemNameCp.text = muhon.NameForTips;
        }
        //图标
        m_compareIconBase.SetIcon(true, muhon.Icon);
        m_compareIconBase.SetBorder(true, muhon.BorderIcon);
        if (null != m__ItemGradeBgCompare)
        {
            UIManager.GetTextureAsyn((uint)muhon.TipsTopIcon, ref m_compareAsynSeedData, () =>
                {
                    if (null != m__ItemGradeBgCompare)
                    {
                        m__ItemGradeBgCompare.mainTexture = null;
                    }
                }, m__ItemGradeBgCompare);
        }
        //绑定表示
        m_compareIconBase.SetLockMask(muhon.IsBind);
        //战斗力
        if (null != m_label_MuhonFightingPowerCp)
        {
            vTempTxt = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.ItemTips_Fpower
               , muhon.Power);
            m_label_MuhonFightingPowerCp.text = ColorManager.GetColorString(ColorType.JZRY_Tips_Light, vTempTxt);
        }

        //装备标示
        if (null != m_trans_MuhonEquipMaskCp && m_trans_MuhonEquipMaskCp.gameObject.activeSelf != muhon.IsWear)
        {
            m_trans_MuhonEquipMaskCp.gameObject.SetActive(muhon.IsWear);
        }

        //是否激活
        m_compareIconBase.SetMuhonMask(true, muhon.StartLevel);
        m_compareIconBase.SetMuhonLv(true, Muhon.GetMuhonLv(muhon));
        float gapCp = 0;
        Vector3 tempPosCp = Vector3.zero;
        tempPosCp.y = -32f;
        Vector3 gapPosCp = Vector3.zero;
        if (null != m_trans_PartMuhonCp && !m_trans_PartMuhonCp.gameObject.activeSelf)
        {
            m_trans_PartMuhonCp.gameObject.SetActive(true);
        }

        //当前等级
        if (null != m_label_MuhonCurLvCp)
        {
            //m_label_MuhonCurLvCp.text = string.Format("当前等级：{0}/{1}", muhon.Level, muhon.MaxLv);
            m_label_MuhonCurLvCp.text = tmgr.GetLocalFormatText(LocalTextType.knapsack_Commond_24, muhon.Level, muhon.MaxLv);
        }
        //需要等级
        if (null != m_label_MuhonNeedLevelCp)
        {
            string nlv = muhon.BaseData.useLevel.ToString();
            if (!DataManager.IsMatchPalyerLv((int)muhon.BaseData.useLevel))
            {
                nlv = ColorManager.GetColorString(ColorType.JZRY_Txt_Red, muhon.BaseData.useLevel.ToString());
            }
            vTempTxt = tmgr.GetLocalFormatText(LocalTextType.ItemTips_NeedLv, nlv);
            m_label_MuhonNeedLevelCp.text = ColorManager.GetColorString(ColorType.JZRY_Tips_Light, vTempTxt);
        }
        //成长
        if (null != m_label_MuhonGrowCp)
        {
            //m_label_MuhonGrowCp.text = string.Format("成长度：{0}", muhon.ExpPercentageFormat);
            m_label_MuhonGrowCp.text = tmgr.GetLocalFormatText(LocalTextType.knapsack_Commond_25, muhon.ExpPercentageFormat);
        }

        //基础属性
        if (null != m_trans_BaseAttrContentCp && !m_trans_BaseAttrContentCp.gameObject.activeSelf)
        {
            m_trans_BaseAttrContentCp.gameObject.SetActive(true);
        }
        m_trans_BaseAttrContentCp.localPosition = tempPosCp;

        gapPosCp = Vector3.zero;
        int levelCp = (int)muhon.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_WeaponSoul_Level);
        if (levelCp == 0)
            levelCp = 1;
        List<EquipDefine.EquipBasePropertyData> baseProDataCp = emgr.GetWeaponSoulBasePropertyData(muhon.BaseId, levelCp);
        if (null != baseProDataCp && baseProDataCp.Count > 0)
        {
            for (int i = 0; i < baseProDataCp.Count; i++)
            {
                gapPosCp.y -= 30f;
                Add(new ItemTipsLineGridData()
                {
                    TipsLineType = UIItemTipsLineGrid.TipsLineType.Txt,
                    Description = baseProDataCp[i].Name + baseProDataCp[i].ToString(),
                    ColorType = ColorType.White,
                }, m_trans_BaseAttrContentCp, gapPosCp);
            }
        }
        tempPosCp.y += gapPosCp.y;

        //附加属性
        gapPosCp = Vector3.zero;
        if (muhon.AdditionAttrCount > 0)
        {
            tempPosCp.y -= 30f;
            List<GameCmd.PairNumber> addAttrCp = muhon.GetAdditiveAttr();
            if (null != m_trans_AdditiveAttrContentCp && !m_trans_AdditiveAttrContentCp.gameObject.activeSelf)
            {
                m_trans_AdditiveAttrContentCp.gameObject.SetActive(true);
            }
            m_trans_AdditiveAttrContentCp.localPosition = tempPosCp;

            GameCmd.PairNumber pNumberCp;
            for (int i = 0; i < addAttrCp.Count; i++)
            {
                pNumberCp = addAttrCp[i];
                gapPosCp.y -= 30;
                Add(new ItemTipsLineGridData()
                {
                    TipsLineType = UIItemTipsLineGrid.TipsLineType.Attr,
                    AttrGrade = emgr.GetAttrGrade(pNumberCp),
                    Description = emgr.GetAttrDes(pNumberCp),
                    ColorType = ColorType.JZRY_Tips_Orange,
                }, m_trans_AdditiveAttrContentCp, gapPosCp);
            }

        }
        tempPosCp.y += gapPosCp.y;

        //描述
        tempPosCp.y -= 45f;
        Add(new ItemTipsLineGridData()
        {
            TipsLineType = UIItemTipsLineGrid.TipsLineType.Txt,
            Description = ColorManager.GetColorString(ColorType.White, muhon.Des),
        }, m_trans_PartMuhonCp.parent.parent, tempPosCp);
    }

    /// <summary>
    /// 创建Tips
    /// </summary>
    public void CreateTips()
    {
        if (null == m_data || null == m_data.m_data)
        {
            Engine.Utility.Log.Error("ItemTipsPanel data error");
            return;
        }
        InitItemBaseIconGrid(true);
        //检测使用UI
        bool useLongUI = false;
        if (m_data.m_data.BaseType == GameCmd.ItemBaseType.ItemBaseType_Equip && m_data.m_data.SubType != (uint)GameCmd.EquipType.EquipType_Office)
        {
            useLongUI = true;
        }
        if (null != m_sprite_Short && m_sprite_Short.gameObject.activeSelf == useLongUI)
        {
            m_sprite_Short.gameObject.SetActive(!useLongUI);
        }
        if (null != m_sprite_Normal && m_sprite_Normal.gameObject.activeSelf != useLongUI)
        {
            m_sprite_Normal.gameObject.SetActive(useLongUI);
        }
        if (null != m_scrollview_InfoLinesScrollView && m_scrollview_InfoLinesScrollView.gameObject.activeSelf != useLongUI)
        {
            m_scrollview_InfoLinesScrollView.gameObject.SetActive(useLongUI);
        }
        //名称
        if (null != m_label_ItemName)
        {
            //m_label_ItemName.color = ColorManager.GetColor32OfType(ColorType.JZRY_Txt_White);
            m_label_ItemName.text = m_data.m_data.NameForTips;
        }
        if (null != m__ItemGradeBgNormal)
        {
            UIManager.GetTextureAsyn((uint)m_data.m_data.TipsTopIcon, ref m_normalAsynSeedData, () =>
                {
                    if (null != m__ItemGradeBgNormal)
                    {
                        m__ItemGradeBgNormal.mainTexture = null;
                    }
                }, m__ItemGradeBgNormal);
        }
        //图标
        m_normalIconBase.SetIcon(true, m_data.m_data.Icon);
        m_normalIconBase.SetBorder(true, m_data.m_data.BorderIcon);
        //绑定表示
        m_normalIconBase.SetLockMask(m_data.m_data.IsBind);
        if (m_data.m_data.IsEquip && m_data.m_bool_needCompare)
        {
            bool fightPowerUp = false;
            if (DataManager.Manager<EquipManager>().IsEquipNeedFightPowerMask(m_data.m_data.QWThisID, out fightPowerUp))
            {
                SetNormalFightPowerMask(true, fightPowerUp);
            }
        }

        //Tips类型
        UIDefine.UITipsType tipsType = GetTipsType(m_data.m_data.BaseType, (uint)m_data.m_data.SubType);
        switch (tipsType)
        {
            case UIDefine.UITipsType.Equip:
                CreateEquipPart();
                break;
            case UIDefine.UITipsType.Consumption:
                //战魂蛋坐骑蛋
                if (m_data.m_data.BaseData.subType == (int)ItemDefine.ItemConsumerSubType.SoulBead
                    || m_data.m_data.BaseData.subType == (int)ItemDefine.ItemConsumerSubType.MountsBead)
                {
                    CreateMuhonMountsEggPart();
                }
                else if (m_data.m_data.BaseData.subType == (int)ItemDefine.ItemConsumerSubType.TreasureMap)
                {
                    CreatTreasureMapPart();
                }
                else //消耗药
                {
                    CreateConsumptionPart();
                }
                break;
            case UIDefine.UITipsType.Material:
            case UIDefine.UITipsType.MissionItem:
                CreateMaterialPart();
                break;
            case UIDefine.UITipsType.Mounts:
                CreateMountsPart();
                break;
            case UIDefine.UITipsType.Muhon:
                CreateMuhonPart();
                break;
        }
        m_em_curComparePos = GameCmd.EquipPos.EquipPos_None;
        SwitchCompare();
    }

    /// <summary>
    /// 设置战斗力提升标示
    /// </summary>
    /// <param name="needMask"></param>
    /// <param name="promote"></param>
    private void SetNormalFightPowerMask(bool needMask, bool promote = true)
    {
        if (null != m_trans_PowerContent)
        {
            if (m_trans_PowerContent.gameObject.activeSelf != needMask)
            {
                m_trans_PowerContent.gameObject.SetActive(needMask);
            }
            if (needMask)
            {
                if (null != m_sprite_PowerPromote && m_sprite_PowerPromote.gameObject.activeSelf != promote)
                {
                    m_sprite_PowerPromote.gameObject.SetActive(promote);
                }

                if (null != m_sprite_PowerFallOff && m_sprite_PowerFallOff.gameObject.activeSelf == promote)
                {
                    m_sprite_PowerFallOff.gameObject.SetActive(!promote);
                }
            }
        }
    }

    private GameCmd.EquipPos m_em_curComparePos = GameCmd.EquipPos.EquipPos_None;
    /// <summary>
    /// 设置切换对比按钮状态
    /// </summary>
    /// <param name="visible"></param>
    private void SetSwitchCompareStatus(bool visible)
    {
        if (null != m_btn_SwitchCompare && m_btn_SwitchCompare.gameObject.activeSelf != visible)
        {
            m_btn_SwitchCompare.gameObject.SetActive(visible);
        }
    }

    /// <summary>
    /// 切换对比
    /// </summary>
    private void SwitchCompare()
    {
        uint compareId = 0;
        List<GameCmd.EquipPos> equipPos = null;
        bool needCompare = m_data.m_bool_needCompare
            && m_data.m_data.IsEquip
            && !emgr.IsWearEquip(m_data.m_data.QWThisID)
            && emgr.TryGetEquipPosByType((GameCmd.EquipType)m_data.m_data.SubType, out equipPos);

        if (null != m_trans_TipsContentCompare
            && m_trans_TipsContentCompare.gameObject.activeSelf != needCompare)
        {
            m_trans_TipsContentCompare.gameObject.SetActive(needCompare);
        }

        bool needSwitchCompare = false;
        if (needCompare)
        {
            if (null != m_trans_TipsContent)
            {
                m_trans_TipsContent.localPosition = new Vector3(180, 0, 0);
            }
            GameCmd.EquipPos comparePos = m_em_curComparePos;

            if (equipPos.Count == 2)
            {
                if (comparePos != GameCmd.EquipPos.EquipPos_None)
                {
                    ReleaseCompare();
                    if (equipPos[1] != comparePos)
                        comparePos = equipPos[1];
                    else
                        comparePos = equipPos[0];
                }
                else if (emgr.TryGetEquipLowPowItem((GameCmd.EquipType)m_data.m_data.SubType, out compareId))
                {
                    comparePos = imgr.GetBaseItemByQwThisId<BaseEquip>(compareId).EPos;
                }
                needSwitchCompare = true;
            }
            else
            {
                comparePos = equipPos[0];
            }

            m_em_curComparePos = comparePos;
            if (emgr.IsEquipPos(comparePos, out compareId))
            {
                CreateCompare(compareId);
            }
            else
            {
                Engine.Utility.Log.Error("ItemTipsPanel->CreateCompare faield,data error!");
            }
        }
        SetSwitchCompareStatus(needSwitchCompare);
    }

    /// <summary>
    /// 生成对比
    /// </summary>
    /// <param name="compareId"></param>
    private void CreateCompare(uint compareId)
    {
        BaseItem baseItem = imgr.GetBaseItemByQwThisId(compareId);
        if (null == baseItem)
        {
            return;
        }

        //更新normal战斗力
        {
            BaseEquip compareEquip = (BaseEquip)baseItem;
            BaseEquip normalEquip = (BaseEquip)m_data.m_data;
            bool needMask = false;
            bool promote = false;
            if (compareEquip.Power != normalEquip.Power
                && DataManager.IsMatchPalyerJob((int)normalEquip.BaseData.useRole))
            {
                needMask = true;
                promote = (normalEquip.Power > compareEquip.Power);
            }
            SetNormalFightPowerMask(needMask, promote);
        }
        //Tips类型
        UIDefine.UITipsType tipsType = GetTipsType(baseItem.BaseType, baseItem.BaseData.subType);
        switch (tipsType)
        {
            case UIDefine.UITipsType.Equip:
                if (baseItem is Equip)
                {
                    CreateEquipPartCompare((Equip)baseItem);
                }
                break;
            case UIDefine.UITipsType.Muhon:
                if (baseItem is Muhon)
                {
                    CreateMuhonPartCompare((Muhon)baseItem);
                }
                break;
        }
    }

    /// <summary>
    /// 获取Tip展示类型
    /// </summary>
    /// <returns></returns>
    private UIDefine.UITipsType GetTipsType(GameCmd.ItemBaseType itemType, uint subType)
    {
        UIDefine.UITipsType tipsType = UIDefine.UITipsType.None;
        switch (itemType)
        {
            case GameCmd.ItemBaseType.ItemBaseType_Equip:
                if (subType == (uint)GameCmd.EquipType.EquipType_Office)
                {
                    tipsType = UIDefine.UITipsType.Material;
                }
                else if (subType == (uint)GameCmd.EquipType.EquipType_SoulOne)
                {
                    tipsType = UIDefine.UITipsType.Muhon;
                }
                else
                {
                    tipsType = UIDefine.UITipsType.Equip;
                }
                break;
            case GameCmd.ItemBaseType.ItemBaseType_Consumption:
                tipsType = UIDefine.UITipsType.Consumption;
                break;
            case GameCmd.ItemBaseType.ItemBaseType_Material:
                if (subType == 4)  //任务道具
                {
                    tipsType = UIDefine.UITipsType.MissionItem;
                }
                else
                {
                    tipsType = UIDefine.UITipsType.Material;
                }

                break;

        }
        return tipsType;
    }

    //缓存按钮功能列表
    private List<UITipsFnBtnGrid> m_list_cachefnBtns = new List<UITipsFnBtnGrid>();
    //活动按钮功能列表
    private List<UITipsFnBtnGrid> m_list_activefnBtns = new List<UITipsFnBtnGrid>();
    /// <summary>
    /// 构造功能btn
    /// </summary>
    private void StructFnBtns(UIDefine.UITipsType tipsType)
    {
        bool cansee = m_data.m_bool_needCompare;
        if (null != m_trans_FuctionBtns && m_trans_FuctionBtns.gameObject.activeSelf != cansee)
        {
            m_trans_FuctionBtns.gameObject.SetActive(cansee);
        }
        if (!cansee)
        {
            return;
        }
        cansee = (m_data.m_data.BaseType == GameCmd.ItemBaseType.ItemBaseType_Equip);
        if (null != m_sprite_FNBGNormal && m_sprite_FNBGNormal.gameObject.activeSelf != cansee)
        {
            m_sprite_FNBGNormal.gameObject.SetActive(cansee);
        }
        cansee = (m_data.m_data.BaseType != GameCmd.ItemBaseType.ItemBaseType_Equip);
        if (null != m_sprite_FNBGShort && m_sprite_FNBGShort.gameObject.activeSelf != cansee)
        {
            m_sprite_FNBGShort.gameObject.SetActive(cansee);
        }

        bool lastSetToBottom = true;
        List<ItemDefine.ItemTipsFNType> fnTyps = new List<ItemDefine.ItemTipsFNType>();
        List<ItemDefine.ItemTipsFNType> rejectFnType = new List<ItemDefine.ItemTipsFNType>();
        bool isWear = DataManager.Manager<EquipManager>().IsWearEquip(m_data.m_data.QWThisID);
        switch (tipsType)
        {
            case UIDefine.UITipsType.Equip:
            case UIDefine.UITipsType.Muhon:
                {
                    rejectFnType.Add((isWear) ? ItemDefine.ItemTipsFNType.Wear : ItemDefine.ItemTipsFNType.Unload);
                }
                break;
            case UIDefine.UITipsType.Consumption:
                {
                    rejectFnType.Add(ItemDefine.ItemTipsFNType.Carry);
                }
                break;
            case UIDefine.UITipsType.Material:
            case UIDefine.UITipsType.Mounts:
                {
                    lastSetToBottom = false;
                    lastSetToBottom = false;
                }
                break;
            case UIDefine.UITipsType.MissionItem:
                {
                    rejectFnType.Add(ItemDefine.ItemTipsFNType.Get);
                }
                break;
        }

        if (!m_data.m_data.NeedJump)
        {
            rejectFnType.Add(ItemDefine.ItemTipsFNType.Jump);
        }
        if (!m_data.m_data.CanSell2NPC)
        {
            rejectFnType.Add(ItemDefine.ItemTipsFNType.Sell);
        }
        List<uint> wayIdList;
        if (!imgr.TryGetWayIdListByBaseId(m_data.m_data.BaseId, out wayIdList))
        {
            rejectFnType.Add(ItemDefine.ItemTipsFNType.Get);
        }

        fnTyps.AddRange(ItemDefine.GetItemTipsFNTyps(tipsType, rejectFnType));
        List<UITipsFnBtnGrid.UITipsFnBtnData> btnDatas = new List<UITipsFnBtnGrid.UITipsFnBtnData>();
        if (fnTyps.Count > 0)
        {
            UITipsFnBtnGrid.UITipsFnBtnData tempData = null;
            if (fnTyps.Contains(ItemDefine.ItemTipsFNType.Jump))
            {
                List<uint> jumpIds = m_data.m_data.JumpWayIDs;
                if (null != jumpIds)
                {
                    table.ItemJumpDataBase jumpDB = null;
                    for (int j = 0; j < jumpIds.Count; j++)
                    {
                        jumpDB = GameTableManager.Instance.GetTableItem<table.ItemJumpDataBase>(jumpIds[j]);
                        if (null == jumpDB)
                        {
                            continue;
                        }
                        tempData = new UITipsFnBtnGrid.UITipsFnBtnData()
                        {
                            FnType = ItemDefine.ItemTipsFNType.Jump,
                            Data = jumpIds[j],
                            Name = jumpDB.jumpName,
                        };
                        btnDatas.Add(tempData);
                    }
                }
            }

            for (int i = 0; i < fnTyps.Count; i++)
            {
                if (fnTyps[i] == ItemDefine.ItemTipsFNType.Jump)
                {
                    continue;
                }
                else
                {
                    tempData = new UITipsFnBtnGrid.UITipsFnBtnData()
                    {
                        FnType = fnTyps[i],
                        Name = ItemDefine.GetItemTipsFNName(fnTyps[i]),
                    };
                    btnDatas.Add(tempData);
                }
            }

            UITipsFnBtnGrid fnGrid = null;
            Vector3 pos = Vector3.zero;
            for (int i = 0; i < btnDatas.Count; i++)
            {
                fnGrid = GetFnBtn(btnDatas[i]);
                fnGrid.gameObject.name = btnDatas[i].FnType.ToString();
                pos.y = (i * -55f);
                fnGrid.CacheTransform.localPosition = pos;
                fnGrid.SetVisible(true);
            }
        }

        //没有关闭耳朵
        m_trans_FuctionBtns.gameObject.SetActive(btnDatas.Count != 0);
    }

    /// <summary>
    /// 释放功能按钮
    /// </summary>
    /// <param name="fnBtn"></param>
    private bool ReleaseFnBtn(UITipsFnBtnGrid fnBtn)
    {
        bool success = false;
        if (null != fnBtn && m_list_activefnBtns.Contains(fnBtn))
        {
            m_list_cachefnBtns.Add(fnBtn);
            if (fnBtn.Visible)
                fnBtn.SetVisible(false);
            success = true;
        }
        return success;
    }

    /// <summary>
    /// 释放所有功能btn
    /// </summary>
    public void ReleaseAllFnBtn()
    {
        if (null != m_list_activefnBtns && m_list_activefnBtns.Count > 0)
        {
            for (int i = 0; i < m_list_activefnBtns.Count; i++)
            {
                ReleaseFnBtn(m_list_activefnBtns[i]);
            }
            m_list_activefnBtns.Clear();

        }
    }

    /// <summary>
    /// 获取一个已经初始化的功能按钮
    /// </summary>
    /// <param name="fnType"></param>
    /// <returns></returns>
    public UITipsFnBtnGrid GetFnBtn(UITipsFnBtnGrid.UITipsFnBtnData data)
    {
        UITipsFnBtnGrid result = null;
        if (null != m_list_cachefnBtns && m_list_cachefnBtns.Count > 0)
        {
            result = m_list_cachefnBtns[0];
            m_list_cachefnBtns.Remove(result);
        }
        else
        {
            GameObject cloneRes = UIManager.GetResGameObj(GridID.Uitipsfnbtngrid) as GameObject;
            if (null != cloneRes)
            {
                GameObject obj = NGUITools.AddChild(m_trans_FuctionBtns.gameObject, cloneRes);
                result = obj.GetComponentInChildren<UITipsFnBtnGrid>();
                if (null == result)
                    result = obj.AddComponent<UITipsFnBtnGrid>();
                result.RegisterUIEventDelegate(OnUIEventDlg);
            }
        }
        if (null != result)
        {
            result.SetVisible(true);
            result.RegisterUIEventDelegate(OnUIEventDlg);
            result.SetGridData(data);
            m_list_activefnBtns.Add(result);
        }
        return result;
    }


    /// <summary>
    /// 添加一个属性条
    /// </summary>
    /// <returns></returns>
    public UIItemTipsLineGrid Add(ItemTipsLineGridData data, Transform parent, Vector2 pos, int fontSize = 20)
    {
        UIItemTipsLineGrid result = null;
        if (null != cachePropertyList && cachePropertyList.Count > 0)
        {
            result = cachePropertyList[0];
            cachePropertyList.Remove(result);
        }
        else if (null != itemTipsLinePrefab)
        {
            GameObject obj = Instantiate(itemTipsLinePrefab, Vector3.zero, Quaternion.identity) as GameObject;
            result = obj.GetComponentInChildren<UIItemTipsLineGrid>();
            if (null == result)
                result = obj.AddComponent<UIItemTipsLineGrid>();
        }
        if (null != result)
        {
            result.CacheTransform.parent = parent;
            result.CacheTransform.localScale = Vector3.one;
            result.CacheTransform.localRotation = Quaternion.identity;
            result.CacheTransform.localPosition = pos;
            result.SetFontSize(fontSize);
            //result.SetLineGap(spaceXY);
            result.SetGridViewData(data.TipsLineType, data.Description, data.ColorType, data.AttrGrade, data.IsLock, data.GemName);
        }
        return result;
    }

    /// <summary>
    /// 添加一个属性条
    /// </summary>
    /// <returns></returns>
    public bool Release(UIItemTipsLineGrid grid)
    {
        bool success = false;
        if (null != grid && !cachePropertyList.Contains(grid))
        {
            cachePropertyList.Add(grid);
            if (null != cachePropertyLineRoot)
                grid.CacheTransform.parent = cachePropertyLineRoot;
            success = true;
        }
        return success;
    }

    /// <summary>
    /// 释放所有属性条
    /// </summary>
    public void ReleaseAll()
    {
        UIItemTipsLineGrid[] propertyArray = transform.GetComponentsInChildren<UIItemTipsLineGrid>();
        if (null != propertyArray && propertyArray.Length > 0)
        {
            foreach (UIItemTipsLineGrid grid in propertyArray)
                Release(grid);
        }
    }

    public void ReleaseCompare()
    {
        if (null != m_trans_TipsContentCompare)
        {
            UIItemTipsLineGrid[] propertyArray = m_trans_TipsContentCompare.GetComponentsInChildren<UIItemTipsLineGrid>();
            if (null != propertyArray && propertyArray.Length > 0)
            {
                foreach (UIItemTipsLineGrid grid in propertyArray)
                    Release(grid);
            }
        }
    }

    /// <summary>
    /// 重置所有tipsUI状态
    /// </summary>
    public void ResetTipUI()
    {
        ReleaseAll();
        ReleaseAllFnBtn();
        ResetInfos();
    }

    #endregion

    #region UIEvent
    void onClick_ColliderBtn_Btn(GameObject obj)
    {
        HideSelf();
    }

    /// <summary>
    /// UI事件回调
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (null != data && data is UITipsFnBtnGrid)
                    {
                        UITipsFnBtnGrid fnGrid = data as UITipsFnBtnGrid;
                        OnFnBtnClick(fnGrid.Data);
                    }
                }
                break;
        }
    }

    /// <summary>
    ///功能按钮响应
    /// </summary>
    /// <param name="fnType"></param>
    private void OnFnBtnClick(UITipsFnBtnGrid.UITipsFnBtnData data)
    {
        if (null == m_data || null == m_data.m_data)
        {
            Engine.Utility.Log.Error("ItemTipsPanel->OnFnBtnClick error data null");
            return;
        }
        switch (data.FnType)
        {
            case ItemDefine.ItemTipsFNType.Sell:
                {
                    DoSell();
                }
                break;
            case ItemDefine.ItemTipsFNType.Repair:
                {
                    DoRepair();
                }
                break;
            case ItemDefine.ItemTipsFNType.Show:
                {
                    DoShow();
                }
                break;
            case ItemDefine.ItemTipsFNType.Get:
                {
                    DoGet();
                }
                break;
            case ItemDefine.ItemTipsFNType.Carry:
                {
                    DoCarry();
                }
                break;
            case ItemDefine.ItemTipsFNType.Use:
                {
                    DoUse();
                }
                break;
            case ItemDefine.ItemTipsFNType.Unload:
                {
                    DoUnload();
                }
                break;
            case ItemDefine.ItemTipsFNType.Wear:
                {
                    DoWear();
                }
                break;
            //case ItemDefine.ItemTipsFNType.Split:
            //    {
            //        DoSplit();
            //    }
            //    break;
            case ItemDefine.ItemTipsFNType.Jump:
                {
                    if (null != data.Data)
                    {
                        DoJump((uint)data.Data);
                    }
                }
                break;
        }
    }

    void onClick_SwitchCompare_Btn(GameObject caster)
    {
        SwitchCompare();
    }

    #endregion

    #region FnBtnAction

    /// <summary>
    /// 携带
    /// </summary>
    public void DoCarry()
    {

    }

    /// <summary>
    /// 获取
    /// </summary>
    public void DoGet()
    {
        HideSelf();
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: m_data.m_data.BaseId);
    }

    /// <summary>
    /// 展示
    /// </summary>
    public void DoShow()
    {

    }

    /// <summary>
    /// 出售
    /// </summary>
    private void DoSell()
    {
        if (null != m_data && null != m_data.m_data)
        {
            if (!m_data.m_data.CanSell2NPC)
            {
                TipsManager.Instance.ShowTips("该物品无法出售");
                HideSelf();
                return;
            }
            string msg = string.Format("是否出售该道具获得{0}金币", m_data.m_data.BaseData.sellPrice * m_data.m_data.Num);
            TipsManager.Instance.ShowTipWindow(Client.TipWindowType.YesNO, msg, () =>
            {
                HideSelf();
                DataManager.Manager<MallManager>().Sell(m_data.m_data.QWThisID);

            }, null, okstr: "确定", cancleStr: "取消");
        }

    }

    /// <summary>
    /// 分解
    /// </summary>
    public void DoSplit()
    {
        if (m_data.m_data.IsEquip)
        {
            Equip equip = (Equip)m_data.m_data;
            if (equip.CanSplit)
            {
                string msg = string.Format("确定要将{0}分解吗？", m_data.m_data.Name);
                TipsManager.Instance.ShowTipWindow(Client.TipWindowType.YesNO, msg, () =>
                {
                    DataManager.Manager<EquipManager>().SplitEquip(m_data.m_data.QWThisID);
                    HideSelf();
                });
            }
            else
            {
                HideSelf();
                TipsManager.Instance.ShowTips("该道具无法进行分解！");
            }
        }


    }

    /// <summary>
    /// 穿戴
    /// </summary>
    public void DoWear()
    {
        DataManager.Manager<EquipManager>().EquipItem(m_data.m_data.QWThisID, true);
        HideSelf();
    }

    /// <summary>
    /// 卸下
    /// </summary>
    public void DoUnload()
    {
        DataManager.Manager<EquipManager>().EquipItem(m_data.m_data.QWThisID, false);
        HideSelf();
    }

    /// <summary>
    /// 跳转
    /// </summary>
    /// <param name="jumpWayID"></param>
    public void DoJump(uint jumpID)
    {

        ItemManager.DoItemJump(jumpID, m_data.m_data.BaseId, m_data.m_data.QWThisID);

        HideSelf();
    }

    #endregion

    /// <summary>
    /// 使用
    /// </summary>
    public void DoUse()
    {
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(m_data.m_data.BaseId);
        int itemNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_data.m_data.BaseId);
        if (itemNum >= 2 && m_data.m_data.BaseData.batchUse == 1)
        {
            Action overDelegate = delegate
            {
                TipsManager.Instance.ShowTips(LocalTextType.knapsack_Experience_1);//数量不足
            };

            int leftTimes = -1;
            if (baseItem.HasDailyUseLimit)
            {
                leftTimes = imgr.GetItemLeftUseNum(m_data.m_data.BaseId);
            }

            TipsManager.Instance.ShowUseItemWindow(baseItem.Name, baseItem.BaseId, okDel: UseItemDelgate, cancelDel: null, belowDel: null, overDel: overDelegate, leftUseTimes: leftTimes, setNum: true);
        }
        //使用改名道具item
        else if (baseItem.BaseId == m_reNameItemId)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ReNamePanel);
        }
        else
        {
            DataManager.Manager<ItemManager>().Use(m_data.m_data.QWThisID);
        }

        HideSelf();
    }

    void UseItemDelgate(bool useDianJunAutoBuy = false, int num = 1)
    {
        DataManager.Manager<ItemManager>().Use(m_data.m_data.QWThisID, (uint)num);
    }



    /// <summary>
    /// 修理
    /// </summary>
    public void DoRepair()
    {
        string msg = "";
        uint cost = DataManager.Manager<EquipManager>().GetEquipRepairCost(m_data.m_data.QWThisID).Num;
        if (cost == 0)
        {
            HideSelf();
            msg = "耐久度满，无需修理!";
            TipsManager.Instance.ShowTips(msg);
        }
        else
        {
            msg = string.Format("是否花费{0}金币修理此装备", cost);
            TipsManager.Instance.ShowTipWindow(Client.TipWindowType.YesNO, msg, () =>
            {
                DataManager.Manager<EquipManager>().RepairEquip(m_data.m_data.QWThisID);
                HideSelf();
            }, null);
        }
    }

    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.REFRESHITEMDURABLE, OnGlobalUIEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.REFRESHITEMDURABLE, OnGlobalUIEventHandler);
        }
    }
    private void OnGlobalUIEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            case (int)Client.GameEventID.REFRESHITEMDURABLE:
                OnUpdateItemDataUI(data);
                break;
        }
    }
    private void OnUpdateItemDataUI(object data)
    {
        if (null == data || !(data is ItemDefine.UpdateItemPassData))
            return;
        ItemDefine.UpdateItemPassData passData = data as ItemDefine.UpdateItemPassData;
        uint qwThisId = passData.QWThisId;
        if (qwThisId == 0)
        {
            Engine.Utility.Log.Warning("{0}->UpdateItemDataUI qwThisId 0！","ItemTipsPanel");
            return;
        }
        if (m_data.m_data.QWThisID == qwThisId)
        {
            RefreshEquipDurable(qwThisId);
        }
    }
    void RefreshEquipDurable(uint qwThisId) 
    {
        Equip equip = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(qwThisId);
        if (equip == null)
        {
            return;
        }
        if (null != m_label_EquipDur)
        {
            string durStr = string.Format("{0}/{1}", equip.CurDisplayDur, equip.MaxDisplayDur);
            if (!equip.HaveDurable)
            {
                durStr = ColorManager.GetColorString(ColorType.JZRY_Txt_Red, durStr);
                m_label_EquipDur.text = string.Format("耐久：{0}", durStr);
            }
            else
            {
                string vTempTxt = tmgr.GetLocalFormatText(LocalTextType.ItemTips_Dur, equip.CurDisplayDur, equip.MaxDisplayDur);
                m_label_EquipDur.text = ColorManager.GetColorString(ColorType.White, vTempTxt);
            }

        }
    }
}