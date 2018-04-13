/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Forging
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ForgingPanel_Compound
 * 版本号：  V1.0.0.0
 * 创建时间：10/15/2016 12:10:12 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class ForgingPanel
{
    #region ProtectData
    public class ProtectAttrData
    {
        //属性
        public GameCmd.PairNumber AttrPair;
        //需要数量
        public int NeedNum;
        //当前填充数量
        public int FillNum;
        public bool Ready
        {
            get
            {
                return FillNum >= NeedNum && FillNum != 0;
            }
        }
    }
    #endregion

    #region property
    //合成辅助装备
    private List<uint> selectCompoundDeputyEquips = new List<uint>();
    //合成最大辅助装备数量
    public const int COMPOUND_DEPUTY_EQUIP_MAX = 3;
    //保护属性生成器
    private UIGridCreatorBase m_protectCreator = null;
    //属性
    private List<GameCmd.PairNumber> Attrs
    {
        get
        {
            List<GameCmd.PairNumber> attrs = new List<GameCmd.PairNumber>();
            Equip equip = Data;
            if (null != equip)
            {
                //附加属性
                attrs.AddRange(equip.GetAdditiveAttr());
                if (attrs.Count > 0)
                {
                    attrs.Sort((left, right) =>
                        {
                            return (int)left.id - (int)right.id;
                        });
                }
            }
            return attrs;
        }
    }
    private List<GameCmd.PairNumber> m_lst_equipAtts = null;
    private List<uint> m_lst_checkProtectAttr = null;
    private Dictionary<uint, ProtectAttrData> m_dicPrtAttData = null;
    private UIItemGrowShowGrid m_compoundGrowShow;
    private UIZFGrid m_zfGrowShow;

    List<UIEquipDeputySelectGrid> m_lst_equipSelects = null;
    private AttrTransData[] m_compoundAttrTransData = null;
    #endregion

    #region init
    /// <summary>
    /// 初始化合成组件
    /// </summary>
    void InitCompoundWidgets()
    {
        if (IsInitStatus(ForgingPanelMode.Compound))
        {
            return;
        }
        SetInitStatus(ForgingPanelMode.Compound, true);
        Transform tempTs = null;
        GameObject cloneObj = null;
        //組件
        if (null != m_trans_CompoundGrowShowRoot && null == m_compoundGrowShow)
        {
            tempTs = UIManager.GetObj(GridID.Uiitemgrowshowgrid);
            if (null != tempTs)
            {
                Util.AddChildToTarget(m_trans_CompoundGrowShowRoot, tempTs);
                cloneObj = tempTs.gameObject;
                if (null != cloneObj)
                {
                    m_compoundGrowShow = cloneObj.GetComponent<UIItemGrowShowGrid>();
                    if (null == m_compoundGrowShow)
                    {
                        m_compoundGrowShow = cloneObj.AddComponent<UIItemGrowShowGrid>();
                    }
                    m_compoundGrowShow.RegisterUIEventDelegate(OnUIEventCallback);
                }
            }
        }

        if (null != m_trans_CompoundMainAttrRoot)
        {
            m_compoundAttrTransData = new AttrTransData[5];
            AttrTransData tempAttrData = null;
            StringBuilder strBuilder = new StringBuilder();
            for(int i = 0,max = m_compoundAttrTransData.Length;i < max;i++)
            {
                tempAttrData = new AttrTransData();
                strBuilder.Remove(0, strBuilder.Length);
                strBuilder.Append(i + 1);
                tempAttrData.Root = m_trans_CompoundMainAttrRoot.Find(strBuilder.ToString());
                if (null == tempAttrData.Root)
                    continue;
                strBuilder.Append("/Content/Grade/Grade");
                tempTs = m_trans_CompoundMainAttrRoot.Find(strBuilder.ToString());
                if (null != tempTs)
                {
                    tempAttrData.Grade = tempTs.GetComponent<UILabel>();
                }
                strBuilder.Remove(0, strBuilder.Length);
                strBuilder.Append(i + 1);
                strBuilder.Append("/Content/Des");
                tempTs = m_trans_CompoundMainAttrRoot.Find(strBuilder.ToString());
                if (null != tempTs)
                {
                    tempAttrData.Des = tempTs.GetComponent<UILabel>();
                }
                m_compoundAttrTransData[i] = tempAttrData;
            }
        }
       
        //祝福
        if (null != m_trans_ZFRoot && null == m_zfGrowShow)
        {
            tempTs = m_trans_UIZFGrid;
            cloneObj = tempTs.gameObject;
            if (null != tempTs)
            {
                Util.AddChildToTarget(m_trans_ZFRoot, tempTs);
                if (null != cloneObj)
                {
                    m_zfGrowShow = cloneObj.GetComponent<UIZFGrid>();
                    if (null == m_zfGrowShow)
                    {
                        m_zfGrowShow = cloneObj.AddComponent<UIZFGrid>();
                    }
                }
                m_zfGrowShow.RegisterUIEventDelegate(OnUIEventCallback);
            }
        }

        
        //副武器初始化
        if (null != m_trans_CompoundDeputyContent)
        {
            m_lst_equipSelects = new List<UIEquipDeputySelectGrid>();
            
            UIEventDelegate dpdAction = (eventType, obj, param) =>
            {
                if (eventType == UIEventType.Click)
                {
                    UIEquipDeputySelectGrid esg = obj as UIEquipDeputySelectGrid;
                    if (null != param && param is bool)
                    {
                        UnloadCompoundDeputyEquipFill(esg.Index);
                    }
                    else
                    {
                        if (!IsCompoundDeputyEquipFill(esg.Index))
                            OnSelectCompoundDeputy(esg.Index);
                    }
                }
            };
            UIEquipDeputySelectGrid dps = null;
            string tempStr = "";
            for (int i = 0; i < 3; i++)
            {
                tempStr = string.Format("EDSG{0}", i + 1);
                tempTs = m_trans_CompoundDeputyContent.Find(tempStr);
                if (null == tempTs)
                    continue;

                cloneObj = tempTs.gameObject;
                if (null != cloneObj)
                {
                    dps = cloneObj.GetComponent<UIEquipDeputySelectGrid>();
                    if (null == dps)
                    {
                        dps = cloneObj.AddComponent<UIEquipDeputySelectGrid>();
                    }
                    dps.SetIndex(i);
                    dps.RegisterUIEventDelegate(dpdAction);
                    m_lst_equipSelects.Add(dps);
                }
            }
        }
       
        ResetCompound();
    }

    void InitCompoundZFWidgets()
    {
        if (IsInitStatus(ForgingPanelMode.CompoundZF))
        {
            return;
        }
        SetInitStatus(ForgingPanelMode.CompoundZF,true);
        m_lst_equipAtts = new List<GameCmd.PairNumber>();
        m_lst_checkProtectAttr = new List<uint>();
        m_dicPrtAttData = new Dictionary<uint, ProtectAttrData>();
        if (null != m_trans_ProtectScrollView && null == m_protectCreator)
        {
            m_protectCreator = m_trans_ProtectScrollView.GetComponent<UIGridCreatorBase>();
            if (null == m_protectCreator)
            {
                m_protectCreator = m_trans_ProtectScrollView.gameObject.AddComponent<UIGridCreatorBase>();
            }
            if (null != m_protectCreator && null != m_trans_UIEquipPropertyProtectGrid)
            {
                m_protectCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
                m_protectCreator.gridWidth = 350;
                m_protectCreator.gridHeight = 105;
                m_protectCreator.RefreshCheck();
                m_protectCreator.Initialize<UIEquipPropertyProtectGrid>(m_trans_UIEquipPropertyProtectGrid.gameObject, OnUpdateGridData, null);
            }
        }
    }
    #endregion

    #region Op
    /// <summary>
    /// 重置合成
    /// </summary>
    void ResetCompound()
    {
        if (!IsInitStatus(ForgingPanelMode.Compound))
        {
            return;
        }
        ResetCompoundDeputyEquip();
        if (null != m_lst_checkProtectAttr)
            m_lst_checkProtectAttr.Clear();
    }

    
    void UpdateMainEquipAttr(bool canCompound)
    {
        Equip equip = Data;
        if (null != m_trans_CompoundMainAttrRoot)
        {
            if (m_trans_CompoundMainAttrRoot.gameObject.activeSelf != canCompound)
                m_trans_CompoundMainAttrRoot.gameObject.SetActive(canCompound);

            if (canCompound)
            {
                List<GameCmd.PairNumber> pairs = equip.GetAdditiveAttr();
                AttrTransData tempTransData = null;
                GameCmd.PairNumber pair = null;
                for(int i =0 ,max = m_compoundAttrTransData.Length;i < max;i ++)
                {
                    tempTransData = m_compoundAttrTransData[i];
                    if (null == tempTransData.Root)
                        continue;

                    if (null != pairs && pairs.Count > i)
                    {
                        if (!tempTransData.Root.gameObject.activeSelf)
                        {
                            tempTransData.Root.gameObject.SetActive(true);
                        }
                        
                        pair = pairs[i];
                        if (null != tempTransData.Grade)
                        {
                            tempTransData.Grade.text = emgr.GetAttrGrade(pair).ToString();
                        }

                        if (null != tempTransData.Des)
                        {
                            tempTransData.Des.text = emgr.GetAttrDes(pair);
                        }
                    }
                    else if (tempTransData.Root.gameObject.activeSelf)
                    {
                        tempTransData.Root.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 选择合成副装备
    /// </summary>
    /// <param name="gridIndex">当前点击副装备索引</param>
    private void OnSelectCompoundDeputy(int gridIndex)
    {
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ItemChoosePanel))
        {
            return;
        }
        Equip data = Data;
        if (null == data)
        {
            TipsManager.Instance.ShowTips("没有可用的主装备");
            return;
        }
        if (!emgr.IsEquipCanCmpound(data.QWThisID))
        {
            TipsManager.Instance.ShowTips("当前装备无法进行合成操作");
            return;
        }

        //筛选满足条件的副装备
        List<uint> matchDeputyEquips = new List<uint>();
        matchDeputyEquips.AddRange(emgr.GetCompoundMatchEquip(selectEquipId));
        if (matchDeputyEquips.Count == 0)
        {
            TipsManager.Instance.ShowTips("没有找到满足合成条件的副装备");
            return;
        }

        //临时选中副装备列表
        List<uint> tempSelectdDeputyEquipList = new List<uint>();
        tempSelectdDeputyEquipList.AddRange(selectCompoundDeputyEquips);


        UILabel desLabel = null;
        Action<UILabel> desLabAction = (des) =>
        {
            desLabel = des;
            if (null != desLabel)
                desLabel.text = m_tmgr.GetLocalFormatText(LocalTextType.Local_TXT_Soul_ChooseNum
            , GetCompoundDeputyCount(tempSelectdDeputyEquipList), 3);
        };
        UIEventDelegate eventDlg = (eventTye, grid, param) =>
        {
            if (eventTye == UIEventType.Click)
            {
                UIEquipInfoSelectGrid tempGrid = grid as UIEquipInfoSelectGrid;
                Equip tempEquip = imgr.GetBaseItemByQwThisId<Equip>(tempGrid.QWThisId);
                if (null != tempEquip)
                {
                    if (tempEquip.CanCompound)
                    {
                        int selectNum = GetCompoundDeputyCount(tempSelectdDeputyEquipList);
                        bool refreshLabel = false;
                        if (tempSelectdDeputyEquipList.Contains(tempGrid.QWThisId))
                        {
                            int clickIndex = tempSelectdDeputyEquipList.IndexOf(tempGrid.QWThisId);
                            tempSelectdDeputyEquipList[clickIndex] = 0;
                            tempGrid.SetHightLight(false);
                            refreshLabel = true;
                        }
                        else if (selectNum < 3)
                        {
                            refreshLabel = true;
                            if (tempSelectdDeputyEquipList[gridIndex] == 0)
                            {
                                tempSelectdDeputyEquipList[gridIndex] = tempGrid.QWThisId;
                            }
                            else
                            {
                                for (int i = 0; i < tempSelectdDeputyEquipList.Count; i++)
                                {
                                    if (tempSelectdDeputyEquipList[i] == 0)
                                    {
                                        tempSelectdDeputyEquipList[i] = tempGrid.QWThisId;
                                        break;
                                    }

                                }
                            }
                            tempGrid.SetHightLight(true);
                        }
                        if (null != desLabel && refreshLabel)
                            desLabel.text = desLabel.text = m_tmgr.GetLocalFormatText(LocalTextType.Local_TXT_Soul_ChooseNum
                , GetCompoundDeputyCount(tempSelectdDeputyEquipList), 3);
                    }else if (tempEquip.AdditionAttrCount == 0)
                    {
                        DataManager.Manager<EffectDisplayManager>().AddTips(m_tmgr.GetLocalText(LocalTextType.forging_compose_1));
                    }
                
                }
                    
            }
        };

        ItemChoosePanel.ItemChooseShowData showData = new ItemChoosePanel.ItemChooseShowData()
        {
            createNum = matchDeputyEquips.Count,
            title = "选择副装备",
            cloneTemp = UIManager.GetResGameObj(GridID.Uiequipinfoselectgrid) as GameObject,
            onChooseCallback = () =>
            {
                selectCompoundDeputyEquips = tempSelectdDeputyEquipList;
                UpdateCompound(Data);
            },
            gridCreateCallback = (obj, index) =>
            {
                UIEquipInfoSelectGrid equipInfoGrid = obj.GetComponent<UIEquipInfoSelectGrid>();
                if (null == equipInfoGrid)
                    equipInfoGrid = obj.AddComponent<UIEquipInfoSelectGrid>();
                Equip itemData = imgr.GetBaseItemByQwThisId<Equip>(matchDeputyEquips[index]);
                equipInfoGrid.RegisterUIEventDelegate(eventDlg);
                equipInfoGrid.SetGridViewData(itemData.QWThisID, selectCompoundDeputyEquips.Contains(itemData.QWThisID)
                    ,itemData.CompoundMaskEnable ,itemData.AdditionAttrCount > 0);
            },
            desPassCallback = desLabAction,
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ItemChoosePanel, data: showData);
    }

    //副装备帅选列表
    private List<uint> compoundDeputyEquipList = new List<uint>();
    /// <summary>
    /// 获取选中装备副装备数量
    /// </summary>
    /// <returns></returns>
    private int GetCompoundDeputyCount(List<uint> equipList)
    {
        int selectCount = 0;
        if (null != equipList)
        {
            for (int i = 0; i < equipList.Count; i++)
            {
                if (equipList[i] != 0 && imgr.ExistItem(equipList[i]))
                    selectCount++;
            }
        }
        return selectCount;
    }
    /// <summary>
    /// 重置辅助合成装备
    /// </summary>
    private void ResetCompoundDeputyEquip()
    {
        for (int i = 0; i < COMPOUND_DEPUTY_EQUIP_MAX; i++)
        {
            if (selectCompoundDeputyEquips.Count <= i)
                selectCompoundDeputyEquips.Add(0);
            else
                selectCompoundDeputyEquips[i] = 0;
        }
        UpdateCompoundDeputyEquipAll();
    }

    /// <summary>
    /// 获取选择的副装备列表
    /// </summary>
    /// <returns></returns>
    private List<uint> GetSelectDeputyEquips()
    {
        List<uint> result = new List<uint>();
        for (int i = 0; i < selectCompoundDeputyEquips.Count; i++)
        {

            if (selectCompoundDeputyEquips[i] != 0)
                result.Add(selectCompoundDeputyEquips[i]);
        }
        return result;
    }

    /// <summary>
    /// 是否副装备位置被填充
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool IsCompoundDeputyEquipFill(int index)
    {
        return (selectCompoundDeputyEquips.Count <= index) ? false : (selectCompoundDeputyEquips[index] != 0);
    }

    /// <summary>
    /// 卸载装备
    /// </summary>
    /// <param name="index"></param>
    public void UnloadCompoundDeputyEquipFill(int index)
    {
        if (index >= 0 && index <= 2)
        {
            selectCompoundDeputyEquips[index] = 0;
            UpdateCompoundDeputyEquip(index);
        }
        
    }

    /// <summary>
    /// 刷新副装备UI
    /// </summary>
    /// <param name="deputyRefrehMask">刷新索引</param>
    private void UpdateCompoundDeputyEquip(int index)
    {
        uint deputyId = 0;
        Equip data = null;
        if (null != m_lst_equipSelects && m_lst_equipSelects.Count > index)
        {
            deputyId = (selectCompoundDeputyEquips.Count > index) ? selectCompoundDeputyEquips[index] : 0;
            data = (deputyId != 0) ? imgr.GetBaseItemByQwThisId<Equip>(deputyId) : null;
            uint qwThisId = 0;
            bool isNone = (null == data);
            if (!isNone)
            {
                qwThisId = data.QWThisID;
            }
            m_lst_equipSelects[index].SetGridViewData(isNone,qwThisId);
        }
    }

    private void UpdateCompoundDeputyEquipAll()
    {
        Equip data = Data;
        if (null == data)
        {
            return ;
        }
        bool canCompound = data.CanCompound;
        if (null != m_trans_CompoundDeputyContent && m_trans_CompoundDeputyContent.gameObject.activeSelf != canCompound)
        {
            m_trans_CompoundDeputyContent.gameObject.SetActive(canCompound);
        }
        if (null != m_label_CompoundWarningTxt && m_label_CompoundWarningTxt.gameObject.activeSelf == canCompound)
        {
            m_label_CompoundWarningTxt.gameObject.SetActive(!canCompound);
        }
        if (canCompound)
        {
            m_label_CompoundNoticeTxt.text = m_tmgr.GetLocalFormatText(LocalTextType.forging_compose_deputy_notice
                , m_tmgr.GetProfessionName((GameCmd.enumProfession)data.BaseData.useRole)
                , EquipDefine.GetEquipPosName(data.EPos));
            UpdateCompoundDeputyEquip(0);
            UpdateCompoundDeputyEquip(1);
            UpdateCompoundDeputyEquip(2);
        }
    }


    /// <summary>
    /// 设置合成消耗钱币
    /// </summary>
    private void SetCompoundCost()
    {
        Equip data = Data;
        if (null == data)
            return;
        bool canCompound = data.CanCompound;

        if (null != m_btn_BtnCompound && m_btn_BtnCompound.gameObject.activeSelf != canCompound)
        {
            m_btn_BtnCompound.gameObject.SetActive(canCompound);
        }
        if (canCompound && null != m_trans_UICGCompoundCost)
        {
            UICurrencyGrid currencyGrid = m_trans_UICGCompoundCost.GetComponent<UICurrencyGrid>();
            if (null == currencyGrid)
                currencyGrid = m_trans_UICGCompoundCost.gameObject.AddComponent<UICurrencyGrid>();
            currencyGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
                    MallDefine.GetCurrencyIconNameByType(GameCmd.MoneyType.MoneyType_Gold), data.CompCost));
        }
    }

    /// <summary>
    /// 更新合成数据UI
    /// </summary>
    void UpdateCompound(Equip data)
    {
        if (null == data)
            return;
        bool canCompound = data.CanCompound;
        if (null != m_compoundGrowShow)
        {
            m_compoundGrowShow.SetGridData(data.QWThisID);
        }
        uint zfId = emgr.ZFBaseId;
        BaseItem zfDataBase = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(zfId);
        int zfNum = imgr.GetItemNumByBaseId(zfId);
        bool enough = (zfNum >= 1) ? true : false;

        if (null != m_trans_CompoundZFRoot && m_trans_CompoundZFRoot.gameObject.activeSelf != canCompound)
        {
            m_trans_CompoundZFRoot.gameObject.SetActive(canCompound);
        }

        if (canCompound)
        {
            if (null != m_zfGrowShow)
            {
                m_zfGrowShow.SetGridData(zfId);
            }
            if (null != m_btn_CompoundBtnZF && m_btn_CompoundBtnZF.gameObject.activeSelf != enough)
                m_btn_CompoundBtnZF.gameObject.SetActive(enough);
        }
        UpdateMainEquipAttr(canCompound);
        //刷新副装备
        UpdateCompoundDeputyEquipAll();
        //合成消耗
        SetCompoundCost();
    }

    //属性保护Transform
    private List<UIEquipPropertyProtectGrid> zfEquipPropertyProtect = new List<UIEquipPropertyProtectGrid>();

    /// <summary>
    /// 检测保护数据
    /// </summary>
    private void CheckProtectData()
    {
        List<uint> protectAttrIds = new List<uint>();
        List<uint> attrsIds = new List<uint>();
        for (int i = 0; i < Attrs.Count; i++)
        {
            attrsIds.Add(Attrs[i].id);
        }
        protectAttrIds.AddRange(m_lst_checkProtectAttr);
        for (int i = 0, max = protectAttrIds.Count; i < max; i++)
        {
            if (!attrsIds.Contains(protectAttrIds[i]))
            {
                m_lst_checkProtectAttr.Remove(protectAttrIds[i]);
            }
            attrsIds.Add(Attrs[i].id);
        }
        CalculateProtectData();
    }

    /// <summary>
    /// 计算保护数据
    /// </summary>
    private void CalculateProtectData()
    {
        m_dicPrtAttData.Clear();
        GameCmd.PairNumber pair = null;
        int rsNeed = 0; 
        Dictionary<uint, int> holdRsNum = new Dictionary<uint, int>();
        Dictionary<uint, int> simiRsCount = new Dictionary<uint, int>();
        uint rsId = 0;
        for(int i =0 ,max = m_lst_checkProtectAttr.Count; i < max;i++)
        {
            pair = GetAttrPairByAttrId(m_lst_checkProtectAttr[i]);
            if (!emgr.TryGetRuneStoneIdByEffectId(emgr.TransformAttrToEffectId(pair), out rsId))
            {
                continue;
            }

            if (!simiRsCount.ContainsKey(rsId))
            {
                simiRsCount.Add(rsId, 1);
            }
            else
            {
                simiRsCount[rsId] += 1;
            }
        }

        bool isLast = false;
        int fillNum = 0;
        for(int i =0 ,max = m_lst_checkProtectAttr.Count; i < max;i++)
        {
            pair = GetAttrPairByAttrId(m_lst_checkProtectAttr[i]);
            if (!emgr.TryGetRuneStoneIdByEffectId(emgr.TransformAttrToEffectId(pair), out rsId))
            {
                m_dicPrtAttData.Add(pair.id, new ProtectAttrData()
                    {
                        AttrPair = pair,
                        FillNum = 0,
                        NeedNum = 0,
                    });
                continue;
            }
            if (!holdRsNum.ContainsKey(rsId))
            {
                holdRsNum.Add(rsId, imgr.GetItemNumByBaseId(rsId));
            }

            isLast = simiRsCount[rsId] == 1;
            rsNeed = GetAttrProtectNeedRSNum(pair);
            
            if (isLast)
            {
                fillNum = holdRsNum[rsId];
            }else
            {
                fillNum = (holdRsNum[rsId] >= rsNeed) ? rsNeed : holdRsNum[rsId];
            }

            
            holdRsNum[rsId] -= fillNum;
            m_dicPrtAttData.Add(pair.id, new ProtectAttrData()
                {
                    AttrPair = pair,
                    FillNum = fillNum,
                    NeedNum = rsNeed,
                });
        }
    }

    private bool IsZFCompoundAttrProtectMaterialReady()
    {
        bool ready = true;
        ProtectAttrData attrData = null;
        for (int i = 0, max = m_lst_checkProtectAttr.Count; i < max; i++)
        {
            attrData = GetCompoundZfProtectAttrData(m_lst_checkProtectAttr[i]);
            if (!attrData.Ready)
            {
                ready = false;
                break;
            }
        }
        return ready;
    }

    /// <summary>
    /// 获取保护数据
    /// </summary>
    /// <returns></returns>
    public ProtectAttrData GetCompoundZfProtectAttrData(uint attrId)
    {
        return (m_dicPrtAttData.ContainsKey(attrId) ? m_dicPrtAttData[attrId] : null) ;
    }

    /// <summary>
    /// 单个符石保护概率
    /// </summary>
    /// <returns></returns>
    private float GetSingleRSProp(GameCmd.PairNumber attrPair)
    {
        return emgr.GetZFAttrUintProtectProb(attrPair, m_lst_checkProtectAttr.Count) /100f;
    }

    /// <summary>
    /// 保护属性需要的符石数量
    /// </summary>
    /// <param name="attrPair"></param>
    /// <returns></returns>
    private int GetAttrProtectNeedRSNum(GameCmd.PairNumber attrPair)
    {
        float prop = GetSingleRSProp(attrPair);
        if (prop <= Mathf.Epsilon)
        {
            return 0;
        }
        int num = Mathf.CeilToInt(1 / prop);
        return num;
    }

    /// <summary>
    /// 获取单个概率
    /// </summary>
    /// <param name="attrPair"></param>
    /// <returns></returns>
    

    /// <summary>
    /// 获取属性id对应的属性
    /// </summary>
    /// <param name="attrId"></param>
    /// <returns></returns>
    private GameCmd.PairNumber GetAttrPairByAttrId(uint attrId)
    {
        Equip data = Data;
        if (null == data)
            return null;
        GameCmd.PairNumber attrPair = new GameCmd.PairNumber();
        attrPair.id = attrId;
        List<GameCmd.PairNumber> addtiveAttr = data.GetAdditiveAttr();
        bool match = false;
        for (int i = 0; i < addtiveAttr.Count; i++)
        {
            if (addtiveAttr[i].id == attrPair.id)
            {
                match = true;
                attrPair.value = addtiveAttr[i].value;
                break;
            }
        }

        return match ? attrPair : null;
    }

    /// <summary>
    /// 保护属性状态变化
    /// </summary>
    /// <param name="attrId"></param>
    private void OnCompoundProtectStatusChanged(uint attrId)
    {
        Equip data = Data;
        if (null == data)
            return;

        GameCmd.PairNumber attrPair = GetAttrPairByAttrId(attrId);
        if (null == attrPair)
        {
            if (m_lst_checkProtectAttr.Contains(attrId))
            {
                m_lst_checkProtectAttr.Remove(attrId);
            }
            return;
        }

        if (m_lst_checkProtectAttr.Contains(attrId))
        {
            m_lst_checkProtectAttr.Remove(attrId);
        }else
        {
            m_lst_checkProtectAttr.Add(attrId);
        }
        UpdateCompoundZF(Data);
    }

    /// <summary>
    /// 装备合成完毕响应
    /// </summary>
    /// <param name="qwThisId"></param>
    private void OnEquipCompoundComplete(uint qwThisId)
    {
        if(m_list_equips.Contains(qwThisId))
        {
            ResetCompound();
            SetSelectId(qwThisId);
            UpdateDataUIByStatus();
        }
    }

   

    /// <summary>
    /// 更新祝福合成
    /// </summary>
    /// <param name="data"></param>
    void UpdateCompoundZF(Equip data)
    {
        if (null == data || !IsInitStatus(ForgingPanelMode.CompoundZF))
            return;
        int oldCount = m_lst_equipAtts.Count;
        m_lst_equipAtts.Clear();
        m_lst_equipAtts.AddRange(Attrs);
        CheckProtectData();
        int addNum = m_lst_equipAtts.Count - oldCount;
        if (addNum > 0)
        {
            for (int i = 0; i < addNum; i++)
            {
                m_protectCreator.InsertData(m_lst_equipAtts.Count - addNum + i);
            }
        }
        else if (addNum < 0)
        {
            addNum = Mathf.Abs(addNum);
            for (int i = 0; i < addNum; i++)
            {
                m_protectCreator.RemoveData(m_lst_equipAtts.Count - 1 - i);
            }
        }
        m_protectCreator.UpdateActiveGridData();
        m_protectCreator.SetDirty();
        //m_protectCreator.SetVisible(false);
        //m_protectCreator.SetVisible(true);
    }
    #endregion

    #region UIEvent
    void onClick_CompoundBtnZF_Btn(GameObject caster)
    {
        if (IsPanelMode(ForgingPanelMode.CompoundZF))
            return;
        SetPanelMode(ForgingPanelMode.CompoundZF);
    }

    void onClick_BtnCompound_Btn(GameObject caster)
    {
        DoCompound();
    }

    /// <summary>
    /// 合成操作
    /// </summary>
    private void DoCompound()
    {
        bool zf = false;
        if (IsPanelMode(ForgingPanelMode.CompoundZF))
            zf = true;
        Equip data = imgr.GetBaseItemByQwThisId<Equip>(selectEquipId);

        if (!emgr.IsEquipCanCmpound(selectEquipId))
        {
            TipsManager.Instance.ShowTips("装备不满足合成条件");
            return;
        }
        if (null == data.LocalComposeDataBase)
        {
            TipsManager.Instance.ShowTips("合成数据错误");
            return;
        }

        if (zf && !IsZFCompoundAttrProtectMaterialReady())
        {
            string tips = "有保护属性所需消耗的符石数量不足，请将符石补足后再进行操作！";
            TipsManager.Instance.ShowTipWindow(Client.TipWindowType.CancelOk, tips, null, okstr: "确定", cancleStr: "取消");
            return;
        }

        emgr.EquipCompound(selectEquipId,       //合成装备
            GetSelectDeputyEquips(),            //副装备列表
            zf,                                 //是否为祝福合成
            ((zf) ? m_lst_checkProtectAttr : null));                    //祝福合成保护数据

    }

    void onClick_CompoundBtnBack_Btn(GameObject caster)
    {
        if (IsPanelMode(ForgingPanelMode.CompoundZF))
            return;
        SetPanelMode(ForgingPanelMode.Compound);
    }
    #endregion
}