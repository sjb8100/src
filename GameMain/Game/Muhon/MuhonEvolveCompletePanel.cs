using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class MuhonEvolveCompletePanel
{
    #region property
    private uint evolveMuhonId = 0;
    private UIItemShowGrid mShowGrid = null;

    private List<Transform> m_lstBaseAttr = null;
    private List<Transform> m_lstAddtiveAttr = null;
    private Vector3 gapWidget = Vector3.zero;
    #endregion

    #region overridemethod

    protected override void OnLoading()
    {
        base.OnLoading();
        GameObject preObj = UIManager.GetResGameObj(GridID.Uiitemshowgrid);
        GameObject cloneObj = null;
        UIEventDelegate dlg = (eventType, data, param) =>
            {
                if (eventType == UIEventType.Click)
                {
                    TipsManager.Instance.ShowItemTips(evolveMuhonId);
                }
            };
        if (null != preObj)
        {
            if (null == mShowGrid && null != m_trans_InfoGridRoot)
            {
                cloneObj = NGUITools.AddChild(m_trans_InfoGridRoot.gameObject, preObj);
                if (null != cloneObj)
                {
                    mShowGrid = cloneObj.GetComponent<UIItemShowGrid>();
                    if (null == mShowGrid)
                    {
                        mShowGrid = cloneObj.AddComponent<UIItemShowGrid>();
                    }
                    if (null != mShowGrid && !mShowGrid.Visible)
                    {
                        mShowGrid.SetVisible(true);
                        mShowGrid.RegisterUIEventDelegate(dlg);
                    }
                }
            }
        }
        Transform tempTrans = null;
        if (null != m_trans_BaseAttr)
        {
            m_lstBaseAttr = new List<Transform>();
            for(int i = 1;i <= 4;i ++)
            {
                tempTrans = m_trans_BaseAttr.Find(i.ToString());
                if (null != tempTrans)
                {
                    m_lstBaseAttr.Add(tempTrans);
                }
            }
        }

        if (null != m_trans_AdditiveAttr)
        {
            m_lstAddtiveAttr = new List<Transform>();
            for (int i = 1; i <= 5; i++)
            {
                tempTrans = m_trans_AdditiveAttr.Find(i.ToString());
                if (null != tempTrans)
                {
                    m_lstAddtiveAttr.Add(tempTrans);
                }
            }
        }

        if (null != m_widget_GapWidget)
        {
            gapWidget = m_widget_GapWidget.gameObject.transform.localPosition;
        }
        
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (null == data)
            return;
        evolveMuhonId = (uint)data;
        UpdateUI();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        onClick_Close_Btn(null);
    }
    #endregion

    #region RefreshUI

    private void UpdateUI()
    {
        EquipManager emgr = DataManager.Manager<EquipManager>();
        Muhon curMuhon = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Muhon>(evolveMuhonId);
        if (null == curMuhon || null == curMuhon.Pre)
        {
            TipsManager.Instance.ShowTips("进化完成数据错误！");
            return;
        }

        if (null != mShowGrid)
        {
            mShowGrid.SetGridData(evolveMuhonId);
        }
        if (null != m_label_MuhonName)
        {
            m_label_MuhonName.text = curMuhon.Name;
        }
        if (null != m_label_MuhonLv)
        {
            m_label_MuhonLv.text = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_TXT_Soul_ColorLv
                , "", "", curMuhon.Level,curMuhon.MaxLv);
        }

        if (null != m_label_AttrNumPre)
        {
            m_label_AttrNumPre.text = DataManager.Manager<TextManager>()
                .GetLocalFormatText(LocalTextType.Local_TXT_Soul_Num,curMuhon.Pre.MuhonAttrUpLimit);
        }

        if (null != m_label_AttrNumCur)
        {
            m_label_AttrNumCur.text = DataManager.Manager<TextManager>()
                .GetLocalFormatText(LocalTextType.Local_TXT_Soul_Num, curMuhon.MuhonAttrUpLimit);
        }
        List<EquipDefine.EquipBasePropertyData> curBaseProperyList = emgr.GetWeaponSoulBasePropertyData(curMuhon.BaseId, 1);
        List<EquipDefine.EquipBasePropertyData> preBaseProperyList = emgr.GetWeaponSoulBasePropertyData(curMuhon.Pre.BaseId, (int)curMuhon.Pre.MaxLv);
        EquipDefine.EquipBasePropertyData temp = null;
        Transform tempTrans = null;
        Vector3 baseLastPos = gapWidget;
        if (null != m_lstBaseAttr)
        {
            for(int i = 0,max = m_lstBaseAttr.Count;i < max;i++)
            {
                tempTrans = m_lstBaseAttr[i];
                if (null == tempTrans)
                    continue;
                if (null != curBaseProperyList && curBaseProperyList.Count > i
                    && null != preBaseProperyList && preBaseProperyList.Count > i)
                {
                    if (!tempTrans.gameObject.activeSelf)
                        tempTrans.gameObject.SetActive(true);
                    tempTrans.Find("Content/Name").GetComponent<UILabel>().text = curBaseProperyList[i].Name;
                    tempTrans.Find("Content/CurV").GetComponent<UILabel>().text = preBaseProperyList[i].ToString();
                    tempTrans.Find("Content/NextV").GetComponent<UILabel>().text = curBaseProperyList[i].ToString();
                }
                else
                {
                    if (tempTrans.gameObject.activeSelf)
                        tempTrans.gameObject.SetActive(false);
                    if (i == (max -1))
                    {
                        Vector3 tempV = tempTrans.position;
                        tempV = tempTrans.TransformPoint(tempV);
                        tempV = m_scrollview_AttrContent.transform.InverseTransformPoint(tempV);
                        baseLastPos.y = tempV.y;
                    }
                }
            }
        }

        List<GameCmd.PairNumber> addtive = curMuhon.GetAdditiveAttr();

        if (null != m_lstAddtiveAttr)
        {
            for (int i = 0, max = m_lstAddtiveAttr.Count; i < max; i++)
            {
                tempTrans = m_lstAddtiveAttr[i];
                if (null == tempTrans)
                    continue;
                if (null != addtive && addtive.Count > i)
                {
                    if (!tempTrans.gameObject.activeSelf)
                        tempTrans.gameObject.SetActive(true);
                    tempTrans.Find("Grade").GetComponent<UILabel>().text = emgr.GetAttrGrade(addtive[i]).ToString();
                    tempTrans.Find("Des").GetComponent<UILabel>().text = emgr.GetAttrDes(addtive[i]);
                }
                else if (tempTrans.gameObject.activeSelf)
                {
                    tempTrans.gameObject.SetActive(false);
                }
            }
        }

        bool additiveVisible = (null != addtive && addtive.Count != 0);
        if (null != m_trans_AddtiveAttrContent)
        {
            if (m_trans_AddtiveAttrContent.gameObject.activeSelf != additiveVisible)
            {
                m_trans_AddtiveAttrContent.gameObject.SetActive(additiveVisible);
            }

            if (additiveVisible)
            {
                m_trans_AddtiveAttrContent.transform.localPosition = gapWidget;
            }

        }

        if (null != m_scrollview_AttrContent)
        {
            m_scrollview_AttrContent.ResetPosition();
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
        onClick_Close_Btn(null);
    }
    #endregion
}