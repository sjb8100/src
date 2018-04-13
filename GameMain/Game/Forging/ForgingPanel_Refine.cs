/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Forging
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ForgingPanel_Refine
 * 版本号：  V1.0.0.0
 * 创建时间：10/15/2016 12:11:38 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
partial class ForgingPanel
{
    #region property
    public const string REFINE_CONST_MAX_REFINE_LEVEL_TXT_FORMAT = "激活高级属性  {0}";
    private int forgingAssistMask = 0;
    private bool m_bool_playRefAnim = true;
    private UIItemGrowShowGrid m_refienGrowShow = null;
    #endregion

    #region define
    /// <summary>
    /// 辅助道具使用类型
    /// </summary>
    public enum ForgingAssistType : int
    {
        None = 0,
        XY = 1,
        TG = 2,
        ReinfeUseDQ = 4,
        LH = 8,
        SPLH = 16,
        ComposeUseDQ = 32,
    }
    #endregion

    #region init
    /// <summary>
    /// 初始化精炼组件
    /// </summary>
    void InitRefineWidgets()
    {
        if (IsInitStatus(ForgingPanelMode.Refine))
        {
            return;
        }
        SetInitStatus(ForgingPanelMode.Refine, true);
        GameObject preObj = UIManager.GetResGameObj(GridID.Uiitemgrowshowgrid) as GameObject;
        GameObject cloneObj = null;
        if (null == m_refienGrowShow && null != m_trans_RefineInfoRoot && null != preObj)
        {
            cloneObj = NGUITools.AddChild(m_trans_RefineInfoRoot.gameObject, preObj);
            if (null != cloneObj)
            {
                m_refienGrowShow = cloneObj.GetComponent<UIItemGrowShowGrid>();
                if (null == m_refienGrowShow)
                {
                    m_refienGrowShow = cloneObj.AddComponent<UIItemGrowShowGrid>();
                }
            }
        }

        if (null != m_slider_RefineSuccessSlider)
        {
            m_slider_RefineSuccessSlider.onChange.Add(new EventDelegate(OnRefineSuccessValueChanged));
        }
    }
    #endregion

    #region Op

    /// <summary>
    /// 精炼完成
    /// </summary>
    /// <param name="code"></param>
    private void OnRefineComplete(uint code)
    {
        string txtMsg = "";
        switch (code)
        {
            case (uint)GameCmd.RefineOPRet.RefineOPRet_Up:
                {
                    Equip data = Data;
                    if (null != data && !data.IsRefineMax)
                    {
                        m_bool_playRefAnim = true;
                        SetRefineSuccessProbability(emgr.GetEquipRefineProbability(data.BaseId, (int)data.RefineLv + 1));
                    }
                    txtMsg = "精炼成功，精炼等级+1";
                }
                break;
            case (uint)GameCmd.RefineOPRet.RefineOPRet_NotChange:
                txtMsg = "精炼失败，精炼等级不变";
                break;
            case (uint)GameCmd.RefineOPRet.RefineOPRet_SubOne:
                txtMsg = "精炼失败，精炼等级-1";
                break;
            case (uint)GameCmd.RefineOPRet.RefineOPRet_Zero:
                txtMsg = "精炼失败，精炼等级归零";
                break;
        }
        TipsManager.Instance.ShowTips(txtMsg);
        
            
    }

    /// <summary>
    /// 重置精炼状态
    /// </summary>
    void ResetRefine()
    {
        m_bool_refineAutoUseDQ = false;
        ResetSelctAssistMaterial();
        m_bool_playRefAnim = true;
    }

    

    /// <summary>
    /// 设置精炼成功UI
    /// </summary>
    /// <param name="data"></param>
    private void SetRefineSuccessProbability(int percent,bool showPercent = true)
    {
        if (null != m_slider_RefineSuccessSlider && m_bool_playRefAnim)
        {
            m_bool_playRefAnim = false;
            TweenSlider ts = m_slider_RefineSuccessSlider.GetComponent<TweenSlider>();
            if (null != ts)
            {
                ts.from = 0;
                ts.to = percent / 100f;
                ts.ResetToBeginning();
                ts.enabled = true;
            }
        }

        if (null != m_label_RefineSuccessProp && m_label_RefineSuccessProp.gameObject.activeSelf != showPercent)
        {
            m_label_RefineSuccessProp.gameObject.SetActive(showPercent);
        }
    }

    private void OnRefineSuccessValueChanged()
    {
        if (null != m_slider_RefineSuccessSlider
            && null != m_label_RefineSuccessProp
            && m_label_RefineSuccessProp.gameObject.activeSelf)
        {
            int percent = UnityEngine.Mathf.FloorToInt(100 * m_slider_RefineSuccessSlider.value);
            m_label_RefineSuccessProp.text = string.Format("{0}%", percent);
        }
    }


    /// <summary>
    /// 设置精炼辅助材料
    /// </summary>
    public void SetRefineAssistMaterial()
    {
        UpdateRefineAssistMaterialUI();
    }
    private List<UIItemAssistSelectGrid> m_lst_refineAssist = null;
    private uint m_uint_refineAssistBaseId = 0;
    private bool m_bool_refineAutoUseDQ = false;

    /// <summary>
    /// 设置选中辅助道具
    /// </summary>
    /// <param name="select"></param>
    /// <param name="baseId"></param>
    private void SetSelectAssistMaterial(bool select,uint baseId = 0)
    {
        bool opSuccess = false;
        if (select && (m_uint_refineAssistBaseId == 0 || m_uint_refineAssistBaseId != baseId))
        {
            opSuccess = true;
            m_uint_refineAssistBaseId = baseId;
        }
        else if (!select && m_uint_refineAssistBaseId != 0)
        {
            opSuccess = true;
            m_uint_refineAssistBaseId = 0;
        }
        if (m_uint_refineAssistBaseId == 0 && m_bool_refineAutoUseDQ)
        {
            m_bool_refineAutoUseDQ = false;
        }
        if (opSuccess)
        {
            UpdateRefineAssistMaterialUI();
        }
    }

    /// <summary>
    /// 是否选中精炼辅助道具
    /// </summary>
    /// <param name="baseId"></param>
    /// <returns></returns>
    public bool IsSelectAssistMaterial(uint baseId)
    {
        return (m_uint_refineAssistBaseId != 0 && m_uint_refineAssistBaseId == baseId);
    }

    /// <summary>
    /// 重置选中精炼道具
    /// </summary>
    private  void ResetSelctAssistMaterial()
    {
        m_uint_refineAssistBaseId = 0;
    }

    /// <summary>
    /// 更新精炼辅助材料UI
    /// </summary>
    private void UpdateRefineAssistMaterialUI()
    {
        Equip data = Data;
        if (null == data)
            return;

        bool isMax = data.IsRefineMax;
        bool isCanRefine = data.CanRefine;
        bool needShow = isCanRefine && !isMax;
        if (null != m_trans_RefineAssist && m_trans_RefineAssist.gameObject.activeSelf != needShow)
        {
            m_trans_RefineAssist.gameObject.SetActive(needShow);
        }
        if (needShow)
        {
            if (null == m_lst_refineAssist)
            {
                m_lst_refineAssist = new List<UIItemAssistSelectGrid>(2);
            }

            List<uint> assistMaterials = new List<uint>(2);
            List<uint> assistMaterialsNum = new List<uint>(2);
            if (data.NextRefineDataBase.assistId1 != 0 && data.NextRefineDataBase.assistNum1 > 0)
            {
                assistMaterials.Add(data.NextRefineDataBase.assistId1);
                assistMaterialsNum.Add(data.NextRefineDataBase.assistNum1);
            }
            if (data.NextRefineDataBase.assistId2 != 0 && data.NextRefineDataBase.assistNum2 > 0 && !assistMaterials.Contains(data.NextRefineDataBase.assistId2))
            {
                assistMaterials.Add(data.NextRefineDataBase.assistId2);
                assistMaterialsNum.Add(data.NextRefineDataBase.assistNum2);
            }

            UIEventDelegate dlg = (eventType, obj, param) =>
            {
                if (eventType == UIEventType.Click)
                {
                    if (obj is UIItemInfoGrid)
                    {
                        UIItemInfoGrid grid = obj as UIItemInfoGrid;
                        if (grid.NotEnough && param != null && param is uint)
                        {
                            ShowItemGet((uint)param);
                        }
                    }
                    else if (obj is UIItemAssistSelectGrid)
                    {
                        UIItemAssistSelectGrid grid = obj as UIItemAssistSelectGrid;
                        if (param != null && param is bool)
                        {
                            bool select = (bool)param;
                            SetSelectAssistMaterial(select, grid.BaseId);
                        }
                    }
                }
            };

            GameObject preObj = UIManager.GetResGameObj(GridID.Uiitemassistselectgrid) as GameObject;
            GameObject cloneObj = null;
            UIItemAssistSelectGrid assistGrid = null;
            int size = Mathf.Max(assistMaterials.Count, m_lst_refineAssist.Count);
            for (int i = 0; i < size; i++)
            {
                if (i < assistMaterials.Count)
                {
                    if (m_lst_refineAssist.Count > i)
                    {
                        assistGrid = m_lst_refineAssist[i];

                    }
                    else if (null != preObj && null != m_grid_AssistGridContent)
                    {
                        cloneObj = NGUITools.AddChild(m_grid_AssistGridContent.gameObject, preObj);
                        if (null != cloneObj)
                        {
                            assistGrid = cloneObj.GetComponent<UIItemAssistSelectGrid>();
                            if (null == assistGrid)
                            {
                                assistGrid = cloneObj.AddComponent<UIItemAssistSelectGrid>();
                            }
                            if (null != assistGrid)
                            {
                                assistGrid.RegisterUIEventDelegate(dlg);
                                m_lst_refineAssist.Add(assistGrid);
                            }
                        }
                    }

                    if (null != assistGrid)
                    {
                        uint dqcost = assistMaterialsNum[i] * DataManager.Manager<MallManager>().GetDQPriceByItem(assistMaterials[i]);
                        if (!assistGrid.Visible)
                            assistGrid.SetVisible(true);
                        assistGrid.gameObject.name = i.ToString();
                        assistGrid.SetGridData(assistMaterials[i]
                                , IsSelectAssistMaterial(assistMaterials[i])
                                , num: assistMaterialsNum[i]
                                , useDQ: m_bool_refineAutoUseDQ
                                , costNum: dqcost
                                , mType: GameCmd.MoneyType.MoneyType_Coin);
                    }
                }
                else if (i < m_lst_refineAssist.Count)
                {
                    assistGrid = m_lst_refineAssist[i];
                    if (null != assistGrid && assistGrid.Visible)
                    {
                        assistGrid.SetVisible(false);
                    }
                }
            }


            if (null != m_btn_UseDQToggleRefine)
            {
                UIToggle autoUseToggle = m_btn_UseDQToggleRefine.GetComponent<UIToggle>();
                if (null != autoUseToggle)
                {
                    autoUseToggle.value = m_bool_refineAutoUseDQ;
                }
            }

            //重置
            if (assistMaterials.Count > 0 && null != m_grid_AssistGridContent)
            {
                m_grid_AssistGridContent.Reposition();
            }
        }

        
    }

    private List<Transform> reineCostMaterialTransformCache = null;
    /// <summary>
    /// 设置消耗材料
    /// </summary>
    public void SetRefineCostMaterial()
    {
        Equip data = Data;
        if (null == data)
            return;

        bool isMax = data.IsRefineMax;
        bool isCanRefine = data.CanRefine;
        bool needShow = isCanRefine && !isMax;
        if (null != m_trans_RefineCost && m_trans_RefineCost.gameObject.activeSelf != needShow)
        {
            m_trans_RefineCost.gameObject.SetActive(needShow);
        }
        if (needShow)
        {
            if (null == reineCostMaterialTransformCache)
                reineCostMaterialTransformCache = new List<Transform>();
            if (reineCostMaterialTransformCache.Count > 0)
            {
                for (int i = 0; i < reineCostMaterialTransformCache.Count; i++)
                {
                    reineCostMaterialTransformCache[i].gameObject.SetActive(false);
                }
            }
            Dictionary<uint, uint> costMaterialDic = new Dictionary<uint, uint>();
            if (null != data.NextRefineDataBase)
            {
                if (data.NextRefineDataBase.costItem1 != 0 && data.NextRefineDataBase.costNum1 > 0)
                {
                    costMaterialDic.Add(data.NextRefineDataBase.costItem1, data.NextRefineDataBase.costNum1);
                }
                if (data.NextRefineDataBase.costItem2 != 0 && data.NextRefineDataBase.costNum2 > 0)
                {
                    costMaterialDic.Add(data.NextRefineDataBase.costItem2, data.NextRefineDataBase.costNum2);
                }
                if (data.NextRefineDataBase.costItem3 != 0 && data.NextRefineDataBase.costNum3 > 0)
                {
                    costMaterialDic.Add(data.NextRefineDataBase.costItem3, data.NextRefineDataBase.costNum3);
                }
                if (data.NextRefineDataBase.costItem4 != 0 && data.NextRefineDataBase.costNum4 > 0)
                {
                    costMaterialDic.Add(data.NextRefineDataBase.costItem4, data.NextRefineDataBase.costNum4);
                }
                if (data.NextRefineDataBase.costItem5 != 0 && data.NextRefineDataBase.costNum5 > 0)
                {
                    costMaterialDic.Add(data.NextRefineDataBase.costItem5, data.NextRefineDataBase.costNum5);
                }
                int costCount = costMaterialDic.Count;
                if (costCount == 0)
                    return;
                List<uint> costIds = new List<uint>(costMaterialDic.Keys);
                int cacheCount = reineCostMaterialTransformCache.Count;
                int createCount = 0;
                Transform tempTran = null;
                UIItemGrowCostGrid tempGrid = null;
                GameObject cloneTep = m_trans_UIItemGrowCostGrid.gameObject;
                ItemManager itemMgr = imgr;
                BaseItem itemData = null;
                bool enableNotEnoughGet = false;
                while (costCount > createCount)
                {
                    if (createCount < cacheCount)
                    {
                        tempTran = reineCostMaterialTransformCache[createCount];
                    }
                    else
                    {
                        tempTran = GameObject.Instantiate(cloneTep).transform;
                        reineCostMaterialTransformCache.Add(tempTran);
                    }
                    tempTran.gameObject.SetActive(true);
                    tempTran.parent = m_grid_CostGridContent.transform;
                    tempTran.localScale = Vector3.one;
                    tempTran.localRotation = Quaternion.identity;
                    tempGrid = tempTran.GetComponent<UIItemGrowCostGrid>();
                    if (null == tempGrid)
                        tempGrid = tempTran.gameObject.AddComponent<UIItemGrowCostGrid>();
                    tempGrid.RegisterUIEventDelegate(OnUIEventCallback);
                    tempGrid.SetGridData(costIds[createCount], num: costMaterialDic[costIds[createCount]]);
                    createCount++;
                }
                m_grid_CostGridContent.Reposition();
            }
        }
    }

    /// <summary>
    /// 设置精炼消耗货币
    /// </summary>
    public void SetRefineCost()
    {
        Equip data = Data;
        if (null == data)
            return;

        bool isMax = data.IsRefineMax;
        bool isCanRefine = data.CanRefine;
        bool needShow = isCanRefine && !isMax;

        if (null != m_btn_BtnRefine && m_btn_BtnRefine.gameObject.activeSelf != needShow)
        {
            m_btn_BtnRefine.gameObject.SetActive(needShow);
        }

        if (needShow)
        {
            UICurrencyGrid currencyGrid = m_trans_UICGRefine.GetComponent<UICurrencyGrid>();
            if (null == currencyGrid)
                currencyGrid = m_trans_UICGRefine.gameObject.AddComponent<UICurrencyGrid>();
            currencyGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
                    MallDefine.GetCurrencyIconNameByType(GameCmd.MoneyType.MoneyType_Gold), data.NextRefineDataBase.costConey));
        }
    }

    /// <summary>
    /// 更新精炼数据UI
    /// </summary>
    void UpdateRefine(Equip data)
    {
        if (null == data)
            return;
        bool isRefineEnable = data.CanRefine;
        bool isMax = isRefineEnable && data.IsRefineMax;
        if (null != m_trans_RefineEquipPropertyContent
            && m_trans_RefineEquipPropertyContent.gameObject.activeSelf != isRefineEnable)
        {
            m_trans_RefineEquipPropertyContent.gameObject.SetActive(isRefineEnable);
        }

        if (null != m_refienGrowShow)
        {
            m_refienGrowShow.SetGridData(data.QWThisID);
        }

        if (isRefineEnable)
        {
            //精炼属性提升
            bool enable = !isMax;
            if (null != m_label_RefineCurLv)
            {
                if (m_label_RefineCurLv.gameObject.activeSelf != enable)
                    m_label_RefineCurLv.gameObject.SetActive(enable);
                if (enable)
                {
                    m_label_RefineCurLv.text = string.Format("当前等级:{0}", data.RefineLv);
                }
            }

            if (null != m_label_RefineTargetLv)
            {
                if (m_label_RefineTargetLv.gameObject.activeSelf != enable)
                    m_label_RefineTargetLv.gameObject.SetActive(enable);
                if (enable)
                {
                    m_label_RefineTargetLv.text = string.Format("下一等级:{0}", data.RefineLv + 1);
                }
            }

            
            if (null != m_label_RefineMaxLv)
            {
                if (m_label_RefineMaxLv.gameObject.activeSelf != isMax)
                    m_label_RefineMaxLv.gameObject.SetActive(isMax);
                if (isMax)
                {
                    m_label_RefineMaxLv.text = string.Format("当前等级:{0}", data.RefineLv); ;
                }
            }

            List<EquipDefine.EquipBasePropertyData> curBaseProData = emgr.GetEquipRefineBasePropertyPromote(data.BaseId, (int)data.RefineLv);
            List<EquipDefine.EquipBasePropertyData> nextBaseProData = (enable) ? 
                emgr.GetEquipRefineBasePropertyPromote(data.BaseId, (int)data.RefineLv + 1) :null;
            
            Transform content = null;
            Transform max = null;
            bool propertyenable = false;
            if (null != m_trans_RefineEquipProperty1)
            {
                propertyenable = curBaseProData.Count > 0;
                if (m_trans_RefineEquipProperty1.gameObject.activeSelf != propertyenable)
                {
                    m_trans_RefineEquipProperty1.gameObject.SetActive(propertyenable);
                }

                if (propertyenable)
                {
                    content = m_trans_RefineEquipProperty1.Find("Content");
                    max = m_trans_RefineEquipProperty1.Find("Max");

                    if (content.gameObject.activeSelf != enable)
                    {
                        content.gameObject.SetActive(enable);
                    }
                    if (enable)
                    {
                        content.Find("Name").GetComponent<UILabel>().text 
                            = ((EquipDefine.EquipBasePropertyType)curBaseProData[0].popertyType).GetEnumDescription() ;
                        content.Find("CurValue").GetComponent<UILabel>().text = curBaseProData[0].ToString();
                        content.Find("TargetValue").GetComponent<UILabel>().text = nextBaseProData[0].ToString();
                    }
                    if (max.gameObject.activeSelf == enable)
                    {
                        max.gameObject.SetActive(!enable);
                    }

                    if (!enable)
                    {
                        max.Find("AttrTxt").GetComponent<UILabel>().text = string.Format("{0} {1}"
                            , ((EquipDefine.EquipBasePropertyType)curBaseProData[0].popertyType).GetEnumDescription()
                            , curBaseProData[0]);
                    }
                
                }
                
            }

            if (null != m_trans_RefineEquipProperty2)
            {
                propertyenable = curBaseProData.Count > 1;
                if (m_trans_RefineEquipProperty2.gameObject.activeSelf != propertyenable)
                {
                    m_trans_RefineEquipProperty2.gameObject.SetActive(propertyenable);
                }

                if (propertyenable)
                {
                    content = m_trans_RefineEquipProperty2.Find("Content");
                    max = m_trans_RefineEquipProperty2.Find("Max");

                    if (content.gameObject.activeSelf != enable)
                    {
                        content.gameObject.SetActive(enable);
                    }
                    if (enable)
                    {
                        content.Find("Name").GetComponent<UILabel>().text = 
                            ((EquipDefine.EquipBasePropertyType)curBaseProData[1].popertyType).GetEnumDescription();
                        content.Find("CurValue").GetComponent<UILabel>().text = curBaseProData[1].ToString();
                        content.Find("TargetValue").GetComponent<UILabel>().text = nextBaseProData[1].ToString();
                    }
                    if (max.gameObject.activeSelf == enable)
                    {
                        max.gameObject.SetActive(!enable);
                    }

                    if (!enable)
                    {
                        max.Find("AttrTxt").GetComponent<UILabel>().text = string.Format("{0} {1}"
                            , ((EquipDefine.EquipBasePropertyType)curBaseProData[1].popertyType).GetEnumDescription()
                            , curBaseProData[1]);
                    }
                }
                
            }
        }

        if (null != m_trans_RefineMax)
        {
            if (m_trans_RefineMax.gameObject.activeSelf != isMax)
            {
                m_trans_RefineMax.gameObject.SetActive(isMax);
            }
            if (isMax && null != m_label_RefineMaxActiveProp)
            {
                m_label_RefineMaxActiveProp.text = emgr.GetAttrDes(emgr.GetAdvanceAttrByEquipPos((int)data.EPos));
            }
        }

        if (null != m_trans_RefineDisable && m_trans_RefineDisable.gameObject.activeSelf == isRefineEnable)
        {
            m_trans_RefineDisable.gameObject.SetActive(!isRefineEnable);
        }

        //设置成功率UI
        int refineSuccssProp = (isRefineEnable && !isMax) ? emgr.GetEquipRefineProbability(data.BaseId, (int)data.RefineLv + 1) :0;
        SetRefineSuccessProbability(refineSuccssProp, showPercent: (isRefineEnable && !isMax));
        //设置辅助材料
        SetRefineAssistMaterial();
        //设置消耗材料
        SetRefineCostMaterial();
        //设置精炼消耗钱币
        SetRefineCost();
    }
    #endregion

    #region UIEvent

    void onClick_UseDQToggleRefine_Btn(GameObject caster)
    {
        if (m_uint_refineAssistBaseId != 0)
        {
            m_bool_refineAutoUseDQ = !m_bool_refineAutoUseDQ;
        }else 
        {
            m_bool_refineAutoUseDQ = false;
            TipsManager.Instance.ShowTips("请先勾选辅助道具");
        }
        SetRefineAssistMaterial();
    }
    #endregion
}