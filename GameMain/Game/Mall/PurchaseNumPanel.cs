//*************************************************************************
//	创建日期:	2017-3-30 14:51
//	文件名称:	PurchaseNumPanel.cs
//  创 建 人:   	Chengxue.Zhao参与
//	版权所有:	中青宝
//	说    明:	随身商店购买操作
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
partial class PurchaseNumPanel
{
    #region Property
    //要购买道具数量
    private int purchaseNum = 1;
    private MallDefine.MallLocalData data;
    private UIItemInfoGrid m_baseGrid = null;
    #endregion
    #region Override Method
    protected override void OnLoading()
    {
        base.OnLoading();
        if (null != m_trans_PurchaseItemBaseGrid && null == m_baseGrid)
        {
            GameObject preObj = UIManager.GetResGameObj(GridID.Uiiteminfogrid);
            GameObject cloneObj = NGUITools.AddChild(m_trans_PurchaseItemBaseGrid.gameObject, preObj);
            if (null != cloneObj)
            {
                m_baseGrid = cloneObj.GetComponent<UIItemInfoGrid>();
                if (null == m_baseGrid)
                {
                    m_baseGrid = cloneObj.AddComponent<UIItemInfoGrid>();
                }
            }
        }
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
        this.data = data as MallDefine.MallLocalData;
        UpdatePanelUI();
    }

    
    public override void ResetPanel()
    {
        base.ResetPanel();
        purchaseNum = 1;
    }

    protected override void OnHide()
    {
        base.OnHide();
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.HandInputPanel))
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.HandInputPanel);
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        if (m_baseGrid != null)
        {
            m_baseGrid.Release(true);
            UIManager.OnObjsRelease(m_baseGrid.CacheTransform, (uint)GridID.Uiiteminfogrid);
            m_baseGrid = null;
        }

    }

    #endregion 

    /// <summary>
    /// 刷新界面UI
    /// </summary>
    private void UpdatePanelUI()
    {
        if (null == data)
        {
            Engine.Utility.Log.Error("PurchaseNumPanel -> UpdatePanelUI failed data null");
            return;
        }
        UpdatePurchaseNum(1);
        m_label_CarryShopPurchaseName.text = data.LocalItem.Name;
        //限购数量
        if (data.LocalMall.dayMaxnum <= 0)
        {
            m_label_CarryShopPurchaseQuotaNum.gameObject.SetActive(false);
        }
        else
        {
            m_label_CarryShopPurchaseQuotaNum.gameObject.SetActive(true);
            m_label_CarryShopPurchaseQuotaNum.text = data.LocalMall.dayMaxnum.ToString();
        }

        m_label_CarryShopPurchaseUseDesc.text = data.LocalItem.BaseData.description;
        m_label_CarryShopPurchaseTotalGainNum.text = data.LocalMall.buyPrice * purchaseNum + "";
        if (null != m_baseGrid)
        {
            m_baseGrid.Reset();
            m_baseGrid.SetBorder(true, data.LocalItem.BorderIcon);
            m_baseGrid.SetIcon(true, data.LocalItem.Icon);
            m_baseGrid.SetNum(true, data.LocalMall.pileNum.ToString());
        }
        if (null != m_label_CarryShopPurchaseUseLv)
        {
            ColorType color = (data.LocalItem.UseLv > DataManager.Instance.PlayerLv) ? ColorType.Red : ColorType.Green;
            m_label_CarryShopPurchaseUseLv.text = DataManager.Manager<TextManager>()
                .GetLocalFormatText(LocalTextType.Local_TXT_Mall_UselevelDescribe
                , ColorManager.GetNGUIColorOfType(color), data.LocalItem.UseLv);
        }
    }

    /// <summary>
    /// 更新购买数量
    /// </summary>
    public void UpdatePurchaseNum(int num)
    {
        purchaseNum = num;
        purchaseNum = Mathf.Min(purchaseNum, data.MaxPurchaseNum);
        m_label_CarryShopPurchaseNum.text = purchaseNum.ToString();
        m_label_CarryShopPurchaseTotalGainNum.text = (data.LocalMall.buyPrice * purchaseNum).ToString();
    }

    #region UIEvent
    void onClick_CarryShopPurchaseDialogClose_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_CarryShopPurchaseDialogConfirm_Btn(GameObject caster)
    {
        DataManager.Manager<MallManager>().PurchaseMallItem(data.LocalMall.mallItemId, data.LocalMall.storeId, purchaseNum);
        HideSelf();
    }

    void onClick_CarryShopPurchaseDialogCancel_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_CarryShopPurchaseDialogAdd_Btn(GameObject caster)
    {
        UpdatePurchaseNum(purchaseNum + 1);
        
    }

    void onClick_CarryShopPurchaseDialogSub_Btn(GameObject caster)
    {
        if (purchaseNum >= 1)
        UpdatePurchaseNum(purchaseNum - 1);
        
    }

    void onClick_CarryShopPurchaseHandInput_Btn(GameObject caster)
    {
        UIPanelManager mgr = DataManager.Manager<UIPanelManager>();
        if (mgr.IsShowPanel(PanelID.HandInputPanel))
        {
            mgr.HidePanel(PanelID.HandInputPanel);
        }
        else
        {
            transform.GetChild(0).localPosition = new UnityEngine.Vector3(-178,0,0);
            mgr.ShowPanel(PanelID.HandInputPanel, data: new HandInputPanel.HandInputInitData()
            {
                maxInputNum = (uint)data.MaxPurchaseNum,
                onInputValue = OnInputValue,
                onClose = OnCloseInputPanel,

                showLocalOffsetPosition = new Vector3(354, 0, 0),
            });
        }
        
    }

    //最大购买数量
    void onClick_CarryShopPurchaseMax_Btn(GameObject caster)
    {
        UpdatePurchaseNum(data.MaxPurchaseNum);
    }

    /// <summary>
    /// 手动输入框回调
    /// </summary>
    /// <param name="num"></param>
    void OnInputValue(int num)
    {
        UpdatePurchaseNum(num);
    }

    void OnCloseInputPanel()
    {
        transform.GetChild(0).localPosition = Vector3.zero;
        if (purchaseNum == 0)
        {
            UpdatePurchaseNum(1);
        }
    }
    #endregion
}