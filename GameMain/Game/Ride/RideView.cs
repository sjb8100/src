//*************************************************************************
//	创建日期:	2017-2-17 11:07
//	文件名称:	RideView.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	坐骑 图鉴界面
//*************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class RidePanel
{
    //Uirideviewgrid
    List<table.RideDataBase> m_lstRidedatasShow = new List<table.RideDataBase>();
    bool m_bInitViewGrid = false;
    //分类页签 qulity  tabindex
    SortedDictionary<uint, int> m_sortDicTabs = new SortedDictionary<uint, int>();
    List<string> nameList = new List<string>();
    void InitViewGrid()
    {
        if (null == m_ctor_tujianscroll)
        {
            return;
        }
        m_ctor_tujianscroll.RefreshCheck();
        m_ctor_tujianscroll.Initialize<UIRideViewGrid>(m_widget_UIRideViewGrid.gameObject, OnRideViewGridDataUpdate, OnRideViewGridUIEvent);     
    }

    void OnRideViewGridDataUpdate(UIGridBase data, int index)
    {
        if (data is UITabGrid)
        {
            UITabGrid tab = data as UITabGrid;
            tab.SetGridData(index+1);
            tab.SetName(nameList[index]);
            bool active = (index == 0);
            tab.SetHightLight(active);
            if (active)
            {
                rideViewTabIndex = index;
                OnSelectTabs(1);
            }
        }
        else if (data is UIRideViewGrid)
        {

             if (m_lstRidedatasShow != null && index < m_lstRidedatasShow.Count)
            {
                data.SetGridData(m_lstRidedatasShow[index]);
            }
        }
       
    }


    private int rideViewTabIndex = -1;
    void OnRideViewGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UITabGrid)
                {
                    UITabGrid tab = data as UITabGrid;
                    int curIndex = (int)tab.Data - 1;
                    if (rideViewTabIndex == curIndex)
                    {
                        return;
                    }
                    if (rideViewTabIndex != -1 && null != m_ctor_rideQRoot)
                    {
                        UITabGrid pre = m_ctor_rideQRoot.GetGrid<UITabGrid>(rideViewTabIndex);
                        if (null != pre)
                        {
                            pre.SetHightLight(false);
                        }
                    }
                    tab.SetHightLight(true);

                    rideViewTabIndex = curIndex;
                    OnSelectTabs((int)tab.Data);
                }
                else if (data is UIRideViewGrid)
                {
                    UIRideViewGrid grid = data as UIRideViewGrid;
                    if (grid != null)
                    {
                        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RideMarkPanel, data: grid.Data);
                    }
                }
              
                break;
        }
    }
    void InitRidePreViewUI()
    {
        if (!m_bInitViewGrid)
        {
            InitViewGrid();
            m_bInitViewGrid = true;

            m_sortDicTabs.Add(0,1);
            nameList.Add("全部");
            List<table.RideDataBase> ridedatas = GameTableManager.Instance.GetTableList<table.RideDataBase>();
            for (int i = 0,imax = ridedatas.Count; i < imax; i++)
            {
                if (!m_sortDicTabs.ContainsKey(ridedatas[i].quality))
                {
                    m_sortDicTabs.Add(ridedatas[i].quality, m_sortDicTabs.Count + 1);
                    string strTabName = "";
                    LocalTextType tType = (LocalTextType)Enum.Parse(typeof(LocalTextType), "Ride_Illustrated_" + ridedatas[i].quality.ToString());
                    strTabName = DataManager.Manager<TextManager>().GetLocalText(tType);
                    nameList.Add(strTabName);
                }
            }
            if (m_ctor_rideQRoot != null)
            {
                m_ctor_rideQRoot.RefreshCheck();
                m_ctor_rideQRoot.Initialize<UITabGrid>((uint)GridID.Uitabgrid, UIManager.OnObjsCreate, UIManager.OnObjsRelease, OnRideViewGridDataUpdate, OnRideViewGridUIEvent);
                
            }
        }
        int createCount = (null != nameList) ? nameList.Count : 0;

        m_ctor_rideQRoot.CreateGrids(createCount);
        
    }

    private void OnSelectTabs(int nTabIndex)
    {
        List<table.RideDataBase> ridedatas = GameTableManager.Instance.GetTableList<table.RideDataBase>();
        m_lstRidedatasShow.Clear();

        if (nTabIndex == 1)
        {
            m_lstRidedatasShow.AddRange(ridedatas);
        }
        else
        {
            uint qulity = 0;
            for (int i = 0; i < m_sortDicTabs.Count; i++)
            {
                if (m_sortDicTabs.ElementAt(i).Value == nTabIndex)
                {
                    qulity = m_sortDicTabs.ElementAt(i).Key;
                }
            }
            for (int index = 0; index < ridedatas.Count; index++)
            {
                if (ridedatas[index].quality == qulity)
                {
                    m_lstRidedatasShow.Add(ridedatas[index]);
                }
            }
        }

        if (m_ctor_tujianscroll != null)
        {
            m_ctor_tujianscroll.CreateGrids(m_lstRidedatasShow.Count);
        }
    }
}
