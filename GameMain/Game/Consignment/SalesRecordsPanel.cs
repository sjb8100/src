using UnityEngine;
using System.Collections;
using GameCmd;

partial class SalesRecordsPanel
{
    ConsignmentManager SaleItemDataManager
    {
        get
        {
            return DataManager.Manager<ConsignmentManager>();
        }
    }
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
       // panelBaseData.m_em_colliderMode = UIDefine.UIPanelColliderMode.TransBg;
    }

    protected override void OnLoading()
    {
        base.OnLoading();
        SaleItemDataManager.ValueUpdateEvent += SaleItemDataManager_ValueUpdateEvent;
        if (m_ctor_SellListScrollViewContent != null)
        {
            m_ctor_SellListScrollViewContent.RefreshCheck();
            m_ctor_SellListScrollViewContent.Initialize<UIConsignmentSalesRecordsGrid>((uint)GridID.Uiconsignmentsalesrecordsgrid,
                   UIManager.OnObjsCreate, UIManager.OnObjsRelease, OnUpdateGridData, null);
        }
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        SaleItemDataManager.ValueUpdateEvent -= SaleItemDataManager_ValueUpdateEvent;
        if (m_ctor_SellListScrollViewContent != null)
        {
            m_ctor_SellListScrollViewContent.Release(true);
        }
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        SaleItemDataManager.ReqItemSellLog();
    }

    void UpdateRecordInfo()
    {
        if (m_ctor_SellListScrollViewContent != null)
        {
            stItemSellLogConsignmentUserCmd_S sellLogInfo = SaleItemDataManager.GetItemSellLogInfo();
            if (sellLogInfo != null && sellLogInfo.log_list != null)
            {
                m_ctor_SellListScrollViewContent.CreateGrids(sellLogInfo.log_list.Count);
            }
        }
    }

    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void SaleItemDataManager_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if (e != null)
        {
            if (e.key == SaleItemDispatchEvents.RefreshSaleRecord.ToString())
            {
                UpdateRecordInfo();
            }
        }
    }

    void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (grid is UIConsignmentSalesRecordsGrid)
        {
            UIConsignmentSalesRecordsGrid recordsGrid = grid as UIConsignmentSalesRecordsGrid;
            stItemSellLogConsignmentUserCmd_S sellLogInfo = SaleItemDataManager.GetItemSellLogInfo();
            if (sellLogInfo != null && sellLogInfo.log_list != null)
            {
                ItemTradeLog tradeLog = (sellLogInfo.log_list.Count > index) ? sellLogInfo.log_list[index] : null;
                recordsGrid.SetGridData(tradeLog);
            }
        }
    }
    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        HideSelf();
    }
}
