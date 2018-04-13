using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class EquipCompoundCompletePanel : UIPanelBase
{
    #region Property
    private List<UICompoundAttrGrid> mlstGrids = null;
    private UIItemInfoGrid m_infoGrid = null;
    private uint qwThisID = 0;
    #endregion

    #region override method
    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidgets();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        qwThisID = (uint)data;
        ShowUI();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        if (m_infoGrid != null)
        {
            m_infoGrid.Release(true);
            UIManager.OnObjsRelease(m_infoGrid.CacheTransform, (uint)GridID.Uiiteminfogrid);
            m_infoGrid = null;
        }
    }

    #endregion

    #region Op
    /// <summary>
    /// 
    /// </summary>
    private void InitWidgets()
    {
        GameObject preObj = null;
        if (null == m_infoGrid && null != m_trans_InfoGridRoot)
        {
            Transform cloneObj = UIManager.GetObj((uint)GridID.Uiiteminfogrid);
            if (null != cloneObj)
            {
                Util.AddChildToTarget(m_trans_InfoGridRoot, cloneObj);
                m_infoGrid = cloneObj.GetComponent<UIItemInfoGrid>();
                if (null == m_infoGrid)
                {
                    m_infoGrid = cloneObj.gameObject.AddComponent<UIItemInfoGrid>();
                }
                if (null != m_infoGrid && !m_infoGrid.Visible)
                {
                    m_infoGrid.SetVisible(true);
                }

            }
        }

        mlstGrids = new List<UICompoundAttrGrid>();
        preObj = UIManager.GetResGameObj(GridID.Uicompoundattrgrid) as GameObject;
        if (null != m_grid_AdditiveContent)
        {
            UICompoundAttrGrid mGrid = null;
            GameObject obj = null;
            for (EquipDefine.AttrIndex i = EquipDefine.AttrIndex.First
                ; i <= EquipDefine.AttrIndex.Fifth; i++)
            {
                obj = NGUITools.AddChild(m_grid_AdditiveContent.gameObject, preObj);
                if (null != obj)
                {
                    obj.name = ((int)i).ToString();
                    mGrid = obj.GetComponent<UICompoundAttrGrid>();
                    if (null == mGrid)
                    {
                        mGrid = obj.gameObject.AddComponent<UICompoundAttrGrid>();
                    }
                    mlstGrids.Add(mGrid);
                }
            }
            m_grid_AdditiveContent.Reposition();
        }

        if (null != m_label_EquipName)
        {
            m_label_EquipName.color = Color.white;
        }
    }

    private void ShowUI()
    {
        Equip equip = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(qwThisID);
        if (null == equip)
        {
            return;
        }
        if (null != m_infoGrid)
        {
            m_infoGrid.Reset();
            m_infoGrid.SetIcon(true, equip.Icon);
            m_infoGrid.SetBorder(true, equip.BorderIcon);
            if (equip.IsBind)
                m_infoGrid.SetLockMask(true);
            //m_infoGrid.RegisterUIEventDelegate((eventType, gridData, param) =>
            //{
            //    if (eventType == UIEventType.Click)
            //    {
            //        TipsManager.Instance.ShowItemTips(equip);
            //    }
            //});
        }
        if (null != m_label_EquipName)
        {
            m_label_EquipName.text = equip.Name;
        }

        GameCmd.PairNumber pair = null;
        UICompoundAttrGrid tempGrid = null;
        int gridCount = mlstGrids.Count;
        EquipManager emgr = DataManager.Manager<EquipManager>();
        List<GameCmd.PairNumber> additiveAttrs = equip.GetAdditiveAttr();
        for (int i = 0; i < gridCount; i++)
        {
            tempGrid = mlstGrids[i];
            if (null != additiveAttrs && additiveAttrs.Count > i)
            {
                if (!tempGrid.Visible)
                {
                    tempGrid.SetVisible(true);
                }
                pair = additiveAttrs[i];
                tempGrid.SetData(emgr.GetAttrGrade(additiveAttrs[i]), emgr.GetAttrDes(additiveAttrs[i]));
            }
            else if (tempGrid.Visible)
            {
                tempGrid.SetVisible(false);
            }
        }
    }
    #endregion

    #region UIEvent
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Confirm_Btn(GameObject caster)
    {
        HideSelf();
    }

    #endregion
}