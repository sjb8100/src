//********************************************************************
//	创建日期:	2016-12-21   13:57
//	文件名称:	GetWayPanel.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	道具获取
//********************************************************************
using UnityEngine;
using System;
using System.Collections.Generic;
using Engine;
using Client;
using GameCmd;
using Common;
public class GetWayDescriptionData 
{
    public bool bShowLabel = false;
    public string des = "";
}
partial class GetWayPanel : UIPanelBase
{

    uint m_nItemId = 0;
    List<uint> wayIDList = null;
    List<uint> getList = null;
    List<table.ItemGetDataBase> i_list = null;
    private Vector3 mv3ContentPos = Vector3.zero;
    bool desContentIsShowing = false;
    protected override void OnLoading()
    {
        base.OnLoading();
        i_list = GameTableManager.Instance.GetTableList<table.ItemGetDataBase>();
        getList = new List<uint>();
        for (int m = 0; m < i_list.Count; m++)
        {
            getList.Add(i_list[m].ID);
        }
        if (null != m_ctor_GetWayRoot)
        {
            m_ctor_GetWayRoot.RefreshCheck();
            m_ctor_GetWayRoot.Initialize<UIGetWayGrid>(m_trans_UIGetWayGrid.gameObject, OnUpdataGridData, OnGridUIEventDlg);
        }

        if (null != m_trans_Content)
        {
            mv3ContentPos = m_trans_Content.localPosition;
        }
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_ctor_GetWayRoot != null)
        {
            m_ctor_GetWayRoot.Release(depthRelease);
        }

    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        Release();
    }

    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        HideSelf();
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        UpdatePanelContentPos();
        InitPanel(wayIDList);       
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
        if (data == null)
        {
            return;
        }
        if (data is uint)//兼容
        {
            m_nItemId = (uint)data;
        }
        wayIDList=new List<uint>();
        if(!DataManager.Manager<ItemManager>().TryGetWayIdListByBaseId(m_nItemId, out wayIDList))
        {
            Engine.Utility.Log.Error("这个物品没有获取途径!");
        }
    }
    protected override void OnHide()
    {
        base.OnHide();
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ItemTipsPanel))
        {
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ItemTipsPanel);
        }

        Release();
    }

    private void UpdatePanelContentPos()
    {
        if (null != m_trans_Content)
        {
            m_trans_Content.transform.localPosition = mv3ContentPos;
            bool isShowItemTips = DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ItemTipsPanel);
            Vector3 pos = mv3ContentPos;
            if (isShowItemTips)
            {
                ItemTipsPanel tipsPanel = DataManager.Manager<UIPanelManager>().GetPanel<ItemTipsPanel>(PanelID.ItemTipsPanel);
                if (null != tipsPanel)
                {
                    Bounds tipsBounds = NGUIMath.CalculateRelativeWidgetBounds(m_trans_Content, tipsPanel.Content);
                    Bounds contentBounds = NGUIMath.CalculateRelativeWidgetBounds(m_trans_Content, m_trans_Content);

                    UnityEngine.Vector2 offset = UnityEngine.Vector2.zero;
                    offset.x = tipsBounds.max.x - contentBounds.min.x;
                    offset.y = tipsBounds.max.y - contentBounds.max.y;
                    pos.x += offset.x;
                    pos.y += offset.y;
                }
                
            }
            m_trans_Content.localPosition = pos;
        }
    }

    void onClick_Btn_close_Btn(GameObject caster)
    {
       
        if (desContentIsShowing)
        {
            ShowDescribeContent(false);     
        }
        else 
        {
            HideSelf();
        }
    }
    void InitPanel(List<uint> wayIDList)
    {
        m_ctor_GetWayRoot.CreateGrids(wayIDList.Count);
        //m_trans_GetWayScrollView.GetComponent<UICacheScrollView>().ResetPosition();
    }

    void OnUpdataGridData(UIGridBase grid, int index)
    {
        if (grid is UIGetWayGrid)
        {
            if (wayIDList == null)
            {
                return;
            }

            table.ItemGetDataBase data = GameTableManager.Instance.GetTableItem<table.ItemGetDataBase>(wayIDList[index]);
            if (data != null)
            {
                UIGetWayGrid itemGrid = grid as UIGetWayGrid;
                itemGrid.SetGridData((uint)index);
                itemGrid.SetWayData(data);
                itemGrid.onClickItemGetGrid = onClickItemGetGrid;
            }
        }
    }
    private void OnGridUIEventDlg(UIEventType eventType, object data, object param) 
    {
       switch(eventType)
       {
           case UIEventType.Click:
               if (data is UIGetWayGrid)
               {
                   UIGetWayGrid grid = data as UIGetWayGrid;
                   if(null != grid)
                   {
                       table.ItemGetDataBase tab = GameTableManager.Instance.GetTableItem<table.ItemGetDataBase>(grid.WayIndex);
                       onClickItemGetGrid(tab, grid.WayIndex);
                   }
               }
               break;
       
       
       }
    
    }
    void onClickItemGetGrid(table.ItemGetDataBase data,uint wayIndex)
    {
        ExecuteGoto(data);
    }

    void ExecuteGoto(table.ItemGetDataBase data)
    {
         ItemManager.DoGetJump(data.jumpID, m_nItemId);    
    }

    void onClick_Btn_unclose_Btn(GameObject caster)
    {

    }




    void ShowDescribeContent(bool value,string des = "")
    {
        m_label_DesLabel.gameObject.SetActive(value);
        m_ctor_GetWayRoot.SetVisible(!value);
        m_label_DesLabel.text = des;
        desContentIsShowing = value;
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eRefreshGetWayParam)
        {
            if (param != null && param is stGetWayDescription)
            {
                stGetWayDescription desData = (stGetWayDescription)param;
                ShowDescribeContent(desData.bShow, desData.des);                
            }                  
        }
        return base.OnMsg(msgid, param);
    }

}
