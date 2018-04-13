//********************************************************************
//	创建日期:	2016-10-19   17:56
//	文件名称:	RankPanel.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	排行榜UI
//********************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using Common;
using GameCmd;
using table;
using Client;

partial class RankPanel : UIPanelBase{

    List<GameCmd.stAnswerOrderListRelationUserCmd_S.Data> rankList = null;
    //1级页签
    private UISecondTabCreatorBase mSecondTabCreator = null;
    private uint m_uint_activeFType = 0;
    private uint m_uint_activeStype = 0;
    uint selectSecondsKey = 0;
    int selectIndex = -1;
    private List<uint> mlstFirstTabIds = null;
    Dictionary<uint, List<RankTypeDataBase>> rankDic = null;
    Dictionary<uint, List<uint>> m_dic = null;
    UIRankGrid previous;
    #region  Override
    protected override void OnLoading() 
    {
        base.OnLoading();
        stRequestOrderListRelationUserCmd_C cmd = new stRequestOrderListRelationUserCmd_C();
        OrderListType xx = (OrderListType)Enum.Parse(typeof(OrderListType), "OrderListType_Level");
        cmd.type = xx;
        NetService.Instance.Send(cmd);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RANKDATAREFRESH, EventCallBack);
        rankDic= DataManager.Manager<RankManager>().GetRankDataByFirstType();
        //GameObject obj = UIManager.GetResGameObj(GridID.Uirankgrid) as GameObject;
        if (m_ctor_RankScroll != null)
        {
            m_ctor_RankScroll.RefreshCheck();
            m_ctor_RankScroll.Initialize<UIRankGrid>(m_trans_UIRankGrid.gameObject, OnRankGridDataUpdate, OnRankGridUIEvent);
        }     
        if (mSecondTabCreator == null)
        {
            if (null != m_scrollview_TypeScrollView && null == mSecondTabCreator)
            {
                mSecondTabCreator = m_scrollview_TypeScrollView.GetComponent<UISecondTabCreatorBase>();
                if (null == mSecondTabCreator)
                    mSecondTabCreator = m_scrollview_TypeScrollView.gameObject.AddComponent<UISecondTabCreatorBase>();
                if (null != mSecondTabCreator)
                {
//                     GameObject cloneFTemp = UIManager.GetResGameObj(GridID.Uictrtypegrid) as GameObject;
//                     GameObject cloneSTemp = UIManager.GetResGameObj(GridID.Uisecondtypegrid) as GameObject;
                    mSecondTabCreator.Initialize<UISecondTypeGrid>(m_trans_UICtrTypeGrid.gameObject, m_widget_UISecondTypeGrid.gameObject
                        , OnRankGridDataUpdate, OnUpdateSecondTabGrid, OnRankGridUIEvent);
                }
            }
        }
        List<int> secondTabsNums = new List<int>();
        if (null == mlstFirstTabIds)
        {
            mlstFirstTabIds = new List<uint>();
        }
        if(m_dic == null)
        {
           m_dic = new Dictionary<uint, List<uint>>();
        }
        mlstFirstTabIds.Clear();
        m_dic.Clear();
        foreach(var i in rankDic)
        {
            mlstFirstTabIds.Add(i.Key);
            secondTabsNums.Add(i.Value.Count);
            for (int a = 0; a < i.Value.Count;a++ )
            {
                if (m_dic.ContainsKey(i.Key))
                {
                    m_dic[i.Key].Add(i.Value[a].childID);
                }
                else
                {
                    List<uint> li = new List<uint>();
                    li.Add(i.Value[a].childID);
                    m_dic.Add(i.Key, li);
                }
            }
        }
        if (null != mSecondTabCreator)
        {
            mSecondTabCreator.CreateGrids(secondTabsNums);
        }
   }
    protected override void OnHide()
    {
        Release();
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (mSecondTabCreator != null)
        {
            mSecondTabCreator.Release(depthRelease);
        }
        if (previous != null)
        {
            previous.Release(false);
        }
       
    }


    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.RANKDATAREFRESH, EventCallBack);
//         if (previous != null)
//         {
//             UIManager.OnObjsRelease(previous.CacheTransform, (uint)GridID.Uirankgrid);
//             previous = null;
//         }
        rankDic.Clear();
        m_dic.Clear();
        mlstFirstTabIds.Clear();
        rankList.Clear();
        if (m_ctor_RankScroll != null)
        {
            m_ctor_RankScroll.Release(true);
        }
    }
    uint rankID = 1;
    void EventCallBack(int nEventID, object param)
    {
        if (nEventID == (int)Client.GameEventID.RANKDATAREFRESH)
        {
            Client.stRankType rt = (Client.stRankType)param;
            rankID = rt.rankId;
            ShowUI();
        }
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (data != null)
        {
            stRequestOrderListRelationUserCmd_C cmd = new stRequestOrderListRelationUserCmd_C();
            OrderListType xx = (OrderListType)Enum.Parse(typeof(OrderListType), data.ToString());
            cmd.type = xx;
            NetService.Instance.Send(cmd);
        }    
        ShowUI();
        if (previous != null)
        {
            previous.SetSelect(false);
        }
    }


    #endregion



    void ShowUI()
    {
        ShowMyRank();
        ShowTitle();
        rankList = DataManager.Manager<RankManager>().M_RankList;
        m_ctor_RankScroll.CreateGrids((null != rankList) ? rankList.Count : 0);
    }
    void ShowTitle() 
    {
        RankDataBase table = GameTableManager.Instance.GetTableItem<RankDataBase>(rankID);
        m_label_Label_1.text = table.line1;
        m_label_Label_2.text = table.line2;
        m_label_Label_3.text = table.line3;
        m_label_Label_4.text = table.line4;
        m_label_Label_5.text = table.line5;
        m_label_power_label.transform.Find("Label").GetComponent<UILabel>().text = "我的" + table.line5+":";
    }
    void ShowMyRank()
    {
        uint rank = DataManager.Manager<RankManager>().SelfRank;
        uint power = DataManager.Manager<RankManager>().SelfYiju;
        if(rank ==0 ||  rank>100 )
        {
         m_label_rank_label.text ="未上榜";
        }
        else
        {
          m_label_rank_label.text =rank.ToString();
        }

        m_label_power_label.text = power.ToString();
    }


   
    private void OnRankGridDataUpdate(UIGridBase data, int index) 
    {

        if (data is UIRankGrid)
        {
            UIRankGrid rg = data as UIRankGrid;
            rg.SetRankIndex((uint)index);
            if (index < rankList.Count)
            {
                rg.SetGridData(rankList[index]);
                rg.SetSelect(false);
            }


        }
        else if (data is UICtrTypeGrid)
        {
            if (index < mlstFirstTabIds.Count)
            {
                UICtrTypeGrid grid = data as UICtrTypeGrid;
                grid.SetRedPointStatus(false);
                if (rankDic.ContainsKey((uint)(index + 1)))
                {
                    List<RankTypeDataBase> d = rankDic[(uint)(index + 1)];
                    if (index < mlstFirstTabIds.Count && d.Count > 0)
                    {
                        grid.SetData(mlstFirstTabIds[index], d[0].mainName, d.Count);
                    }
                }


            }
        }
    }
    private void OnUpdateSecondTabGrid(UIGridBase grid, object id, int index)
    {
        if (grid is UISecondTypeGrid)
        {
            UISecondTypeGrid sGrid = grid as UISecondTypeGrid;
            sGrid.SetRedPoint(false);
            List<RankTypeDataBase> list = rankDic[(uint)id];
            sGrid.SetData(list[index].childID, list[index].childName, m_uint_activeStype == m_dic[m_uint_activeFType][index]);
            sGrid.name = list[index].param;
        }
    }


    void UpdataTypeGrid(int index) 
    {
        List<RankTypeDataBase> d = rankDic[(uint)(index + 1)];
        UICtrTypeGrid tabGrid = mSecondTabCreator.GetGrid<UICtrTypeGrid>(index);
        if (tabGrid != null)
        {
            tabGrid.SetGridData((uint)index);  
            tabGrid.SetName(d[0].mainName);
            tabGrid.SetRedPointStatus(false);
        }
    }
 
  
    private void OnRankGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if(data is UISecondTypeGrid)
                {
                    if (previous != null)
                    {
                        previous.SetSelect(false);
                    }
                    UISecondTypeGrid sec = data as UISecondTypeGrid;
                    SetSelectSecondType(sec.Data);                               
                }
                else if(data is UIRankGrid)
                {
                    UIRankGrid grid = data as UIRankGrid;
                    grid.SetSelect(true);
                    
                    if (previous == null)
                    {
                        previous = grid;
                    }
                    else
                   {
                       if (selectIndex != (int)grid.RankIndex)
                        {
                            previous.SetSelect(false);
                            previous = grid;
                            selectIndex = (int)grid.RankIndex;
                       }
                    }
                    DataManager.Instance.Sender.RequestPlayerInfoForOprate(grid.PlayID, PlayerOpreatePanel.ViewType.Normal);
                }
                if (data is UICtrTypeGrid)
                {
                    if (previous != null)
                    {
                        previous.SetSelect(false);
                    }
                    UICtrTypeGrid tabGrid = data as UICtrTypeGrid;
                    SetSelectFirstType((uint)tabGrid.ID);
                }
                break;
        }
    }
    private void SetSelectFirstType(uint type, bool force = false)
    {
        if (null == mSecondTabCreator)
        {
            return;
        }
        if (m_uint_activeFType == type && !force)
        {
            mSecondTabCreator.DoToggle(mlstFirstTabIds.IndexOf(m_uint_activeFType), true, true);
            return;
        }
            m_uint_activeFType = type;
            mSecondTabCreator.Open(mlstFirstTabIds.IndexOf(m_uint_activeFType), true);      
         selectSecondsKey = m_dic[m_uint_activeFType][0];
         SetSelectSecondType(selectSecondsKey, m_uint_activeStype==0);
    }
    private void SetSelectSecondType(uint type, bool force = false)
    {
        if (null == mSecondTabCreator)
        {
            return;
        }
        if (m_uint_activeStype == type && !force)

            return;
        UISecondTypeGrid sGrid = null;
        if ( m_uint_activeFType != 0)
        {
            sGrid = mSecondTabCreator.GetGrid<UISecondTypeGrid>(mlstFirstTabIds.IndexOf(m_uint_activeFType), m_dic[m_uint_activeFType].IndexOf(m_uint_activeStype));
            if (null != sGrid)
            {
                sGrid.SetHightLight(false);
            }
        }

        m_uint_activeStype = type;
        if ( m_uint_activeFType != 0)
        {
            sGrid = mSecondTabCreator.GetGrid<UISecondTypeGrid>(mlstFirstTabIds.IndexOf(m_uint_activeFType), m_dic[m_uint_activeFType].IndexOf(m_uint_activeStype));
            if (null != sGrid)
            {
                sGrid.SetHightLight(true);
            }
        }
        stRequestOrderListRelationUserCmd_C cmd = new stRequestOrderListRelationUserCmd_C();
        if (sGrid != null)
        {
           OrderListType xx = (OrderListType)Enum.Parse(typeof(OrderListType), sGrid.name);
           cmd.type = xx;
           NetService.Instance.Send(cmd);
        }
     
    }
}
