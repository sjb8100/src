/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Forging
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ForgingPanel_Strengthen
 * 版本号：  V1.0.0.0
 * 创建时间：6/7/2017 7:40:38 PM
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
    private UIItemShowGrid m_strengthen = null;
    public EquipDefine.LocalGridStrengthenData StrengthenData
    {
        get
        {
            return emgr.GetCurStrengthDataByPos(m_emSelectInlayPos);
        }
    }
    #endregion

    #region init
    private void InitStrengthenWidgets()
    {
        if (IsInitStatus(ForgingPanelMode.Strengthen))
        {
            return;
        }
        SetInitStatus(ForgingPanelMode.Strengthen, true);

        GameObject preObj = UIManager.GetResGameObj(GridID.Uiitemshowgrid);
        GameObject cloneObj = null;
        if (null == m_strengthen && null != m_trans_StrengthenInfoRoot && null != preObj)
        {
            cloneObj = NGUITools.AddChild(m_trans_StrengthenInfoRoot.gameObject, preObj);
            if (null != cloneObj)
            {
                m_strengthen = cloneObj.GetComponent<UIItemShowGrid>();
                if (null == m_strengthen)
                {
                    m_strengthen = cloneObj.AddComponent<UIItemShowGrid>();
                }
            }
        }

        UICurrencyGrid cGrid = null;
        if (null != m_trans_SingleStrengthenCurrency)
        {
            cGrid = m_trans_SingleStrengthenCurrency.GetComponent<UICurrencyGrid>();
            if (null == cGrid)
            {
                cGrid = m_trans_SingleStrengthenCurrency.gameObject.AddComponent<UICurrencyGrid>();
            }
        }
    }

    /// <summary>
    /// 刷新
    /// </summary>
    private void UpdateStrengthen()
    {
        if (m_emSelectInlayPos == GameCmd.EquipPos.EquipPos_None)
            return;
        bool isMax = emgr.IsGridStrengthenMax(m_emSelectInlayPos);

        if (null != m_label_ActiveSuitLvTxt)
        {
            m_label_ActiveSuitLvTxt.text = emgr.ActiveStrengthenSuitLv.ToString();
        }
        if (null != m_strengthen)
        {
            bool equipGrid = false;
            uint equipId =0;
            
            if (emgr.IsEquipPos(m_emSelectInlayPos,out equipId))
            {
                equipGrid = true;
            }
            equipGrid = equipGrid && !isMax;
            if (m_strengthen.gameObject.active!= equipGrid)
            {
                m_strengthen.SetVisible(equipGrid);
            }
            
            if (equipGrid)
            {
                m_strengthen.SetGridData(equipId);
            }
        }

        if (null != m_label_StrengthenPosName)
        {
            m_label_StrengthenPosName.text = string.Format("{0}.部位", EquipDefine.GetEquipPosName(m_emSelectInlayPos));
        }

        if (null != m_sprite_StrengthenPosIcon)
        {
            m_sprite_StrengthenPosIcon.spriteName = EquipDefine.GetEquipPartIcon(m_emSelectInlayPos);
            m_sprite_StrengthenPosIcon.MakePixelPerfect();
        }

        //强化属性提升
        bool enable = !isMax;
        EquipDefine.LocalGridStrengthenData next = (enable)? emgr.GetNextStrengthDataByPos(m_emSelectInlayPos) : null;
        if (null != m_label_StrengthenCurLv)
        {
            if (m_label_StrengthenCurLv.gameObject.activeSelf != enable)
                m_label_StrengthenCurLv.gameObject.SetActive(enable);
            if (enable)
            {
                m_label_StrengthenCurLv.text = string.Format("强化 {0}级", emgr.GetGridStrengthenLvByPos(m_emSelectInlayPos));
            }
        }

        if (null != m_label_StrengthenTargetLv)
        {
            if (m_label_StrengthenTargetLv.gameObject.activeSelf != enable)
                m_label_StrengthenTargetLv.gameObject.SetActive(enable);
            if (enable)
            {
                m_label_StrengthenTargetLv.text = string.Format("强化 {0}级", emgr.GetGridStrengthenLvByPos(m_emSelectInlayPos) + 1);
            }
        }


        if (null != m_label_StrengthenMaxLv)
        {
            if (m_label_StrengthenMaxLv.gameObject.activeSelf != isMax)
                m_label_StrengthenMaxLv.gameObject.SetActive(isMax);
            if (isMax)
            {
                m_label_RefineMaxLv.text = string.Format("强化 {0}级", emgr.GetGridStrengthenLvByPos(m_emSelectInlayPos)); ;
            }
        }

        List<EquipDefine.EquipBasePropertyData> curBaseProData = (null != StrengthenData) ? StrengthenData.BaseProp : null;
        List<EquipDefine.EquipBasePropertyData> nextBaseProData = (enable) ?
            emgr.GetNextStrengthDataByPos(m_emSelectInlayPos).BaseProp : null;

        Transform content = null;
        Transform max = null;
        bool propertyenable = false;
        if (null != m_trans_StrengthenEquipProperty1)
        {
            propertyenable = (null != curBaseProData) && (curBaseProData.Count > 0)
                || (null != nextBaseProData) && (nextBaseProData.Count > 0);
            if (m_trans_StrengthenEquipProperty1.gameObject.activeSelf != propertyenable)
            {
                m_trans_StrengthenEquipProperty1.gameObject.SetActive(propertyenable);
            }

            if (propertyenable)
            {
                content = m_trans_StrengthenEquipProperty1.Find("Content");
                max = m_trans_StrengthenEquipProperty1.Find("Max");

                if (content.gameObject.activeSelf != enable)
                {
                    content.gameObject.SetActive(enable);
                }
                if (enable)
                {
                    content.Find("Name").GetComponent<UILabel>().text
                        = ((null != curBaseProData && curBaseProData.Count > 0) ? curBaseProData[0].Name : nextBaseProData[0].Name);
                    content.Find("CurValue").GetComponent<UILabel>().text
                        = "+" + ((null != curBaseProData && curBaseProData.Count > 0) ? curBaseProData[0].ToString() : "0");
                    content.Find("TargetValue").GetComponent<UILabel>().text = "+" + nextBaseProData[0].ToString();
                }
                if (max.gameObject.activeSelf == enable)
                {
                    max.gameObject.SetActive(!enable);
                }

                if (!enable)
                {
                    max.Find("AttrTxt").GetComponent<UILabel>().text = string.Format("{0} +{1}"
                        , curBaseProData[0].Name
                        , curBaseProData[0]);
                }

            }

        }

        if (null != m_trans_StrengthenEquipProperty2)
        {
            propertyenable = (null != curBaseProData) && (curBaseProData.Count > 1)
                || (null != nextBaseProData) && (nextBaseProData.Count > 1);
            if (m_trans_StrengthenEquipProperty2.gameObject.activeSelf != propertyenable)
            {
                m_trans_StrengthenEquipProperty2.gameObject.SetActive(propertyenable);
            }

            if (propertyenable)
            {

                content = m_trans_StrengthenEquipProperty2.Find("Content");
                max = m_trans_StrengthenEquipProperty2.Find("Max");

                if (content.gameObject.activeSelf != enable)
                {
                    content.gameObject.SetActive(enable);
                }
                if (enable)
                {
                    content.Find("Name").GetComponent<UILabel>().text
                        = ((null != curBaseProData && curBaseProData.Count > 1) ? curBaseProData[1].Name : nextBaseProData[1].Name);
                    content.Find("CurValue").GetComponent<UILabel>().text
                        = "+" + ((null != curBaseProData && curBaseProData.Count > 1) ? curBaseProData[1].ToString() : "0");
                    content.Find("TargetValue").GetComponent<UILabel>().text = "+" + nextBaseProData[1].ToString();
                }
                if (max.gameObject.activeSelf == enable)
                {
                    max.gameObject.SetActive(!enable);
                }

                if (!enable)
                {
                    max.Find("AttrTxt").GetComponent<UILabel>().text = string.Format("{0} +{1}"
                        , curBaseProData[1].Name
                        , curBaseProData[1]);
                }
            }

        }

        if (null != m_trans_StrengthenMax)
        {
            if (m_trans_StrengthenMax.gameObject.activeSelf != isMax)
            {
                m_trans_StrengthenMax.gameObject.SetActive(isMax);
            }
        }

        //设置消耗材料
        SetStrengthenCostMaterial();
        //设置消耗钱币
        SetStrengthenCost();
        RefreshBtnState();
    }

    private List<Transform> strengthenCostMaterialTransformCache = null;
    private void SetStrengthenCostMaterial()
    {
        bool isMax = emgr.IsGridStrengthenMax(m_emSelectInlayPos);

        if (null != m_trans_StrengthenCost && m_trans_StrengthenCost.gameObject.activeSelf == isMax)
        {
            m_trans_StrengthenCost.gameObject.SetActive(!isMax);
        }
        if (!isMax)
        {
            if (null == strengthenCostMaterialTransformCache)
                strengthenCostMaterialTransformCache = new List<Transform>();
            if (strengthenCostMaterialTransformCache.Count > 0)
            {
                for (int i = 0; i < strengthenCostMaterialTransformCache.Count; i++)
                {
                    strengthenCostMaterialTransformCache[i].gameObject.SetActive(false);
                }
            }
            EquipDefine.LocalGridStrengthenData next = emgr.GetNextStrengthDataByPos(m_emSelectInlayPos);
            if (null != next.ItemCostDatas && next.ItemCostDatas.Count > 0)
            {
                int cacheCount = strengthenCostMaterialTransformCache.Count;
                Transform tempTran = null;
                UIItemGrowCostGrid tempGrid = null;
                ItemManager itemMgr = imgr;
                for (int i = 0; i < next.ItemCostDatas.Count; i++)
                {
                    if (i < cacheCount)
                    {
                        tempTran = strengthenCostMaterialTransformCache[i];
                    }
                    else
                    {
                        GameObject go = NGUITools.AddChild(m_grid_StrengthenCostGridContent.gameObject, m_trans_UIItemGrowCostGrid.gameObject);
                        tempTran = go.transform;
                        strengthenCostMaterialTransformCache.Add(tempTran);
                    }
                    tempTran.gameObject.SetActive(true);                  
                    tempTran.localScale = Vector3.one;
                    tempTran.localRotation = Quaternion.identity;
                    tempGrid = tempTran.GetComponent<UIItemGrowCostGrid>();
                    if (null == tempGrid)
                        tempGrid = tempTran.gameObject.AddComponent<UIItemGrowCostGrid>();
                    tempGrid.RegisterUIEventDelegate(OnUIEventCallback);
                    tempGrid.SetGridData(next.ItemCostDatas[i].ID, num: next.ItemCostDatas[i].Num);
                   
                }
                m_grid_StrengthenCostGridContent.Reposition();
            }
            
        }
    }

    private void SetStrengthenCost()
    {
        bool isStrengthMax = emgr.IsGridStrengthenMax(m_emSelectInlayPos);
        EquipDefine.LocalGridStrengthenData next = emgr.GetNextStrengthDataByPos(m_emSelectInlayPos);
        if (null != m_trans_StrengthenBtns && m_trans_StrengthenBtns.gameObject.activeSelf == isStrengthMax)
        {
            m_trans_StrengthenBtns.gameObject.SetActive(!isStrengthMax);
        }

        if (!isStrengthMax)
        {
            UICurrencyGrid currencyGrid = m_trans_SingleStrengthenCurrency.GetComponent<UICurrencyGrid>();
            if (null == currencyGrid)
                currencyGrid = m_trans_UICGRefine.gameObject.AddComponent<UICurrencyGrid>();
            currencyGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
                    MallDefine.GetCurrencyIconNameByType((GameCmd.MoneyType)next.MoneyCostData.ID), next.MoneyCostData.Num));
        }
    }
    #endregion

    #region UIEvent
    void onClick_AllSrengthen_Btn(GameObject caster)
    {
        string msg = "一键全部强化会消耗大量金币和材料是否继续？";
        TipsManager.Instance.ShowTipWindow(Client.TipWindowType.YesNO, msg, () =>
        {
            emgr.DoGridStrengthen(true, m_emSelectInlayPos);
        }, null, okstr: "确定", cancleStr: "取消");
        
    }

    void onClick_SingleStrengthen_Btn(GameObject caster)
    {
        emgr.DoGridStrengthen(false, m_emSelectInlayPos);
    }

    void onClick_StrengthenSuitBtn_Btn(GameObject caster)
    {

        SuitPanelParam param = new SuitPanelParam()
        {
            m_type = SuitPanelParam.SuitType.Strengthen,
            vec = new Vector3(-36.47f, 140.8f, 0),
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GridStrengthenSuitPanel, data: param);
    }
    #endregion

}