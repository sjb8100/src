using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class MuhonBlendCompletePanel
{
    #region property
    private uint blendMuhonId = 0;
    private List<UIProperyGradeGrid> mlstGrids = null;
    private UIItemShowGrid m_showGrid = null;
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
        GameObject preObj = null;
        if (null == m_showGrid && null != m_trans_InfoGridRoot)
        {
            preObj = UIManager.GetResGameObj(GridID.Uiitemshowgrid);
            if (null != preObj)
            {
                GameObject cloneObj = NGUITools.AddChild(m_trans_InfoGridRoot.gameObject, preObj);
                if (null != cloneObj)
                {
                    m_showGrid = cloneObj.GetComponent<UIItemShowGrid>();
                    if (null == m_showGrid)
                    {
                        m_showGrid = cloneObj.AddComponent<UIItemShowGrid>();
                    }
                    if (null != m_showGrid && !m_showGrid.Visible)
                    {
                        m_showGrid.SetVisible(true);
                        m_showGrid.RegisterUIEventDelegate((eventType, data, param) =>
                            {
                                if (eventType == UIEventType.Click)
                                {
                                    Muhon muhon = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Muhon>(blendMuhonId);
                                    if (null != muhon)
                                    {
                                        TipsManager.Instance.ShowItemTips(muhon);
                                    }
                                }
                            });
                    }
                }
            }
        }

        mlstGrids = new List<UIProperyGradeGrid>();
        preObj = UIManager.GetResGameObj(GridID.Uiproperygradegrid) as GameObject;
        if (null != m_grid_AdditiveContent)
        {
            UIProperyGradeGrid mGrid = null;
            GameObject obj = null;
            for (EquipDefine.AttrIndex i = EquipDefine.AttrIndex.First
                ; i <= EquipDefine.AttrIndex.Fifth; i++)
            {
                obj = NGUITools.AddChild(m_grid_AdditiveContent.gameObject, preObj);
                if (null != obj)
                {
                    obj.name = ((int)i).ToString();
                    mGrid = obj.GetComponent<UIProperyGradeGrid>();
                    if (null == mGrid)
                    {
                        mGrid = obj.gameObject.AddComponent<UIProperyGradeGrid>();
                    }
                    mlstGrids.Add(mGrid);
                }
            }
            m_grid_AdditiveContent.Reposition();
        }
        
    }

    private List<UIProperyGradeGrid> mlstActive = new List<UIProperyGradeGrid>();

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (null == data)
            return;
        blendMuhonId = (uint)data;
        UpdateUI();
    }
    protected override void OnHide()
    {
        base.OnHide();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }
    #endregion

    #region RefreshUI
    private void UpdateUI()
    {
        Muhon itemData = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Muhon>(blendMuhonId);
        if (null == itemData)
        {
            Engine.Utility.Log.Error("MuhonBlendCompletePanel UpdateUI faield get itemData null");
            return;
        }
        EquipManager emgr = DataManager.Manager<EquipManager>();
        if (null != m_showGrid)
        {
            m_showGrid.SetGridData(blendMuhonId);
        }
        if (null != m_label_EquipName)
        {
            m_label_EquipName.text = itemData.Name;
        }

        //更新属性信息
        List<GameCmd.PairNumber> attrPairs = itemData.GetAdditiveAttr();
        GameCmd.PairNumber pair = null;
        UIProperyGradeGrid tempGrid = null;
        int gridCount = mlstGrids.Count;
        for (int i = 0; i < gridCount; i++)
        {
            tempGrid = mlstGrids[i];
            if (null != attrPairs && attrPairs.Count > i)
            {
                if (!tempGrid.Visible)
                {
                    tempGrid.SetVisible(true);
                }
                pair = attrPairs[i];
                tempGrid.SetGridView(emgr.GetAttrDes(pair), emgr.GetAttrGrade(pair), emgr.IsAttrGradeMax(pair));
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