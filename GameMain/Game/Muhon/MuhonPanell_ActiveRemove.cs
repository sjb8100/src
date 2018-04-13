/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Muhon
 * 创建人：  wenjunhua.zqgame
 * 文件名：  MuhonPanel_ActiveRemove
 * 版本号：  V1.0.0.0
 * 创建时间：10/15/2016 12:19:56 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
partial class MuhonPanel
{
    #region property
    private UIItemShowGrid m_activeRemoveGrow = null;
    private List<UIMuhonPropertySelectGrid> mlstMuhonPropertySelects = null;
    #endregion

    #region Init
    private void InitActivateRemovedWidgets()
    {
        if (IsInitStatus(TabMode.JiHuo))
        {
            return;
        }
        SetInitStatus(TabMode.JiHuo, true);
        Transform clone = null;
        if (null != m_trans_ActiveRemoveItemGrowRoot && null == m_activeRemoveGrow)
        {
            clone = UIManager.AddGridTransform(GridID.Uiitemshowgrid, m_trans_ActiveRemoveItemGrowRoot);
            if (null != clone)
            {
                m_activeRemoveGrow = clone.GetComponent<UIItemShowGrid>();
                if (null == m_activeRemoveGrow)
                    m_activeRemoveGrow = clone.gameObject.AddComponent<UIItemShowGrid>();
            }
        }
        mlstMuhonPropertySelects = new List<UIMuhonPropertySelectGrid>();
        if (null != m_grid_ActivePropertyRoot)
        {
            UIMuhonPropertySelectGrid grid = null;
            for (EquipDefine.AttrIndex i = EquipDefine.AttrIndex.First
                ; i <= EquipDefine.AttrIndex.Fifth; i++)
            {
                clone = UIManager.AddGridTransform(GridID.Uimuhonpropertyselectgrid, m_grid_ActivePropertyRoot.transform);
                if (null != clone)
                {
                    clone.name = ((int)i).ToString();
                    grid = clone.GetComponent<UIMuhonPropertySelectGrid>();
                    if (null == grid)
                    {
                        grid = clone.gameObject.AddComponent<UIMuhonPropertySelectGrid>();
                    }
                    grid.RegisterCheckBoxDlg(UpdateActiveRemovePropertySelect);
                    mlstMuhonPropertySelects.Add(grid);
                }
            }
            m_grid_ActivePropertyRoot.Reposition();
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitActiveRemoveData()
    {

    }

    public void ResetActiveRemove()
    {
        if (IsInitStatus(TabMode.JiHuo))
            mlstSelectRemoveAttrs.Clear();
    }

    #endregion

    #region Op

    /// <summary>
    /// 更新状态
    /// </summary>
    public void UpdateActiveRemoveState(Muhon data,bool isActive)
    {
        //更新按钮状态
        if (null != m_btn_ActiveBtn 
            && m_btn_ActiveBtn.gameObject.activeSelf == isActive)
        {
            m_btn_ActiveBtn.gameObject.SetActive(!isActive);
        }
        if (null != m_btn_RemoveBtn 
            && m_btn_RemoveBtn.gameObject.activeSelf != isActive)
        {
            m_btn_RemoveBtn.gameObject.SetActive(isActive);
        }
        TextManager tmg = DataManager.Manager<TextManager>();
        string txt = (isActive) ? tmg.GetLocalText(LocalTextType.Local_TXT_Soul_Relieve)
            : tmg.GetLocalText(LocalTextType.Local_TXT_Soul_Activate);

        if (null != m_label_ActiveRemoveStateTips1)
        {
            m_label_ActiveRemoveStateTips1.text = txt;
        }
        txt = (isActive) ? tmg.GetLocalText(LocalTextType.Local_TXT_Soul_Describe2)
            : tmg.GetLocalText(LocalTextType.Local_TXT_Soul_Describe1);
        if (null != m_label_ActiveRemoveStateTips2)
        {
            m_label_ActiveRemoveStateTips2.text = txt;
        }
        txt = (isActive) ? "属性解除" : "属性激活";
        if (null != m_label_Title_label)
        {
            m_label_Title_label.text = txt;
        }
    }
    /// <summary>
    /// 设置激活辅助
    /// </summary>
    private void SetActivateRemoveAssist()
    {
        Muhon data = Data;
        if (null == data)
            return;
        uint baseId = (data.IsActive) ? data.RemoveNeedItemId : data.ActiveNeedItemId;
        uint cost = 0;
        uint needItemNum = 0;
        if (!data.IsActive)
        {
            needItemNum = data.ActiveNeedItemNum;
            cost = data.ActiveCost;
        }
        else
        {
            uint selectAttrNum = (mlstSelectRemoveAttrs.Count != 0) ?(uint) mlstSelectRemoveAttrs.Count:0;
            needItemNum = selectAttrNum * data.RemoveNeedItemNum;
            cost = selectAttrNum * data.RemoveCost;
        }

        SetCommonCost(baseId, needItemNum,cost, GameCmd.MoneyType.MoneyType_Gold
            ,needItemNum * DataManager.Manager<MallManager>().GetDQPriceByItem(baseId),GameCmd.MoneyType.MoneyType_Coin);
    }

    private List<uint> mlstSelectRemoveAttrs = new List<uint>();
    private void UpdateActiveRemovePropertySelect(uint attrId,bool select)
    {
        if (mlstSelectRemoveAttrs.Contains(attrId) && !select)
        {
            mlstSelectRemoveAttrs.Remove(attrId);
        }else if (!mlstSelectRemoveAttrs.Contains(attrId) && select)
        {
            mlstSelectRemoveAttrs.Add(attrId);
        }
        SetActivateRemoveAssist();
    }

    private void CheckActiveRemovePropertySelect()
    {
        Muhon muhon = Data;
        List<uint> attrids  =(null != muhon) ?  muhon.GetAdditiveAttrIds() : null;
        List<uint> clearIds = new List<uint>();
        for (int i = 0; i < mlstSelectRemoveAttrs.Count;i++ )
        {
            if (null != attrids && attrids.Contains(mlstSelectRemoveAttrs[i]))
            {
                continue;
            }
            clearIds.Add(mlstSelectRemoveAttrs[i]);
        }

        for (int i = 0; i < clearIds.Count; i++)
        {
            mlstSelectRemoveAttrs.Remove(clearIds[i]);
        }
    }

    private void UpdateActivateRemove(Muhon data)
    {
        if (null == data)
            return;
        bool isActive = (data.AdditionAttrCount != 0) ? true : false;
        if (null != m_label_ActiveRemoveName)
        {
            m_label_ActiveRemoveName.text = data.Name;
        }
        //刷新圣魂升级信息
        if (null != m_activeRemoveGrow)
        {
            m_activeRemoveGrow.SetGridData(data.QWThisID);
        }
        if (null != m_grid_ActivePropertyRoot && m_grid_ActivePropertyRoot.gameObject.activeSelf != isActive)
        {
            m_grid_ActivePropertyRoot.gameObject.SetActive(isActive);
        }

        if (isActive)
        {
            CheckActiveRemovePropertySelect();
            List<GameCmd.PairNumber> attrPairs = data.GetAdditiveAttr();
            GameCmd.PairNumber pair = null;
            UIMuhonPropertySelectGrid tempGrid = null;
            int gridCount = mlstMuhonPropertySelects.Count;
            for (int i = 0; i < gridCount; i++)
            {
                tempGrid = mlstMuhonPropertySelects[i];
                if (null != attrPairs && attrPairs.Count > i)
                {
                    if (!tempGrid.Visible)
                    {
                        tempGrid.SetVisible(true);
                    }
                    pair = attrPairs[i];
                    tempGrid.SetGridView(pair.id, emgr.GetAttrDes(pair), emgr.GetAttrGrade(pair),mlstSelectRemoveAttrs.Contains(pair.id));
                }
                else if (tempGrid.Visible)
                {
                    tempGrid.SetVisible(false);
                }
            }
        }
        //设置辅助
        SetActivateRemoveAssist();
        UpdateActiveRemoveState(data,isActive);
    }
    #endregion

    #region UIEvent

    void onClick_ActiveBtn_Btn(GameObject caster)
    {
        Muhon data = Data;
        bool active = (data.AdditionAttrCount == 0) ? false : true;
        if (active)
        {
            return;
        }
        emgr.ActiveWeaponSoul(selectedMuhonId, m_bool_autoUseDQ);
    }

    void onClick_RemoveBtn_Btn(GameObject caster)
    {
        Muhon data = Data;
        bool active = (data.AdditionAttrCount == 0) ? false : true;
        if (!active)
        {
            return;
        }
        if (mlstSelectRemoveAttrs.Count == 0)
        {
            TipsManager.Instance.ShowTips("请勾选要解除的属性");
            return;
        }

        TipsManager.Instance.ShowTipWindow(Client.TipWindowType.CancelOk, "确认解除所勾选的属性？", () =>
        {
            emgr.RemoveWeaponSoulProperty(data.QWThisID, m_bool_autoUseDQ, mlstSelectRemoveAttrs);
        }, null,null, "解除");
    }

    private void ReleaseActiveRemove(bool depthClear = true)
    {
        uint resID = 0;
        if (depthClear && null != mlstMuhonPropertySelects)
        {
            resID = (uint)GridID.Uimuhonpropertyselectgrid;
            for(int i =0,max = mlstMuhonPropertySelects.Count;i < max;i++)
            {
                UIManager.OnObjsRelease(mlstMuhonPropertySelects[i].CacheTransform, resID);
            }
            mlstMuhonPropertySelects.Clear();
        }

        if (null != m_activeRemoveGrow)
        {
            m_activeRemoveGrow.Release(depthClear);
            if (depthClear)
            {
                resID = (uint)GridID.Uiitemshowgrid;
                UIManager.OnObjsRelease(m_activeRemoveGrow.CacheTransform, resID);
                m_activeRemoveGrow = null;
            }
        }
    }
    #endregion
}