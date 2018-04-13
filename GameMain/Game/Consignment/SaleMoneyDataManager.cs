//*************************************************************************
//	创建日期:	2016/11/3 11:05:26
//	文件名称:	SaleMoneyDataManager
//   创 建 人:   zhudianyu	
//	版权所有:	
//	说    明:	文钱交易管理
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using table;
using GameCmd;
using Engine;
using Engine.Utility;
using UnityEngine;

public enum SaleDispatchEvents
{
    None ,
    RefreshSaleInfo ,
    RefreshAccountInfo ,
    RefreshRecordInfo ,//刷新账户交易记录
}
class SaleMoneyDataManager : BaseModuleData , IManager
{

    #region IManager
    public void Initialize()
    {

    }
    public void ClearData()
    {

    }
    public void Reset(bool depthClearData = false)
    {
        m_sellList.Clear();
        m_buyList.Clear();
        m_mySaleList.Clear();
        m_recentSaleList.Clear();
        m_recordList.Clear();
    }

    public void Process(float deltaTime)
    {

    }
    #endregion

    uint m_uBuyNum = 10;
    public uint BuyNum
    {
        get
        {
            return m_uBuyNum;
        }
        set
        {
            m_uBuyNum = value;
        }
    }
    uint m_uBuyPrice = 0;
    public uint BuyPrice
    {
        get
        {
            return m_uBuyPrice;
        }
        set
        {
            m_uBuyPrice = value;
        }
    }


    uint m_uSellNum = 10;
    public uint SellNum
    {
        get
        {
            return m_uSellNum;
        }
        set
        {
            m_uSellNum = value;
        }
    }

    uint m_uSellPrice = 0;
    public uint SellPrice
    {
        get
        {
            return m_uSellPrice;
        }
        set
        {
            m_uSellPrice = value;
        }
    }
    #region Protocol

    List<GameCmd.stSMinBMaxOrderConsignmentUserCmd_S.Data> m_sellList = new List<stSMinBMaxOrderConsignmentUserCmd_S.Data>();
    public List<stSMinBMaxOrderConsignmentUserCmd_S.Data> SellList
    {
        get
        {
            return m_sellList;
        }
    }
    List<stSMinBMaxOrderConsignmentUserCmd_S.Data> m_buyList = new List<stSMinBMaxOrderConsignmentUserCmd_S.Data>();
    public List<stSMinBMaxOrderConsignmentUserCmd_S.Data> BuyList
    {
        get
        {
            return m_buyList;
        }
    }


    public void OnRecieveSaleInfo(stSMinBMaxOrderConsignmentUserCmd_S cmd)
    {
        m_sellList = cmd.sell_data;
        m_buyList = cmd.buy_data;
        DispatchValueUpdateEvent( new ValueUpdateEventArgs( SaleDispatchEvents.RefreshSaleInfo.ToString() , null , null ) );
    }
    /// <summary>
    /// 我的交易清单
    /// </summary>
    List<stUserAccountConsignmentUserCmd_S.SelfOrder> m_mySaleList = new List<stUserAccountConsignmentUserCmd_S.SelfOrder>();
    public List<stUserAccountConsignmentUserCmd_S.SelfOrder> MySaleList
    {
        get
        {
            return m_mySaleList;
        }
    }
    /// <summary>
    /// 最近的交易记录
    /// </summary>
    List<stSuccessBuyLogConsignmentUserCmd_S.Data> m_recentSaleList = new List<stSuccessBuyLogConsignmentUserCmd_S.Data>();

    public List<stSuccessBuyLogConsignmentUserCmd_S.Data> RecentSaleList
    {
        get
        {
            return m_recentSaleList;
        }
    }
    //金币
    public uint MyGold
    {
        get;
        set;
    }
    /// <summary>
    /// 文钱
    /// </summary>
    public uint MyWenqian
    {
        get;
        set;
    }
    public void OnRecentOrderInfo(stSuccessBuyLogConsignmentUserCmd_S cmd)
    {
        m_recentSaleList = cmd.data;
    }
    public void OnReceiveCancleOrder(stCancelOrderConsignmentUserCmd_CS cmd)
    {
        for(int i = 0;i<m_mySaleList.Count;i++)
        {
            stUserAccountConsignmentUserCmd_S.SelfOrder data = m_mySaleList[i];
            if(data.index == cmd.index)
            {
                m_mySaleList.RemoveAt(i);
            }
        }
        DispatchValueUpdateEvent( new ValueUpdateEventArgs( SaleDispatchEvents.RefreshAccountInfo.ToString() , null , null ) );
    }
    public void OnGetAccountMoney(stExtractMoneyConsignmentUserCmd_CS cmd)
    {
        MyGold -= cmd.gold;
        MyWenqian -= cmd.money_ticket;
        TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Trading_Currency_quchuzhanghuzhongdejinbiXwenqianX, cmd.gold, cmd.money_ticket);
        //TipsManager.Instance.ShowTipsById( 117502 , cmd.gold , cmd.money_ticket );
        DispatchValueUpdateEvent( new ValueUpdateEventArgs( SaleDispatchEvents.RefreshAccountInfo.ToString() , null , null ) );
    }
    public void OnRecieveAccoutInfo(stUserAccountConsignmentUserCmd_S cmd)
    {
        MyGold = cmd.money;
        MyWenqian = cmd.money_ticket;
        //  m_recentSaleList = cmd.other_data;
        m_mySaleList = cmd.self_data;
        DispatchValueUpdateEvent( new ValueUpdateEventArgs( SaleDispatchEvents.RefreshAccountInfo.ToString() , null , null ) );
    }
    /// <summary>
    /// 账号交易记录
    /// </summary>
    List<stDealRecordConsignmentUserCmd_S.Data> m_recordList = new List<stDealRecordConsignmentUserCmd_S.Data>();
    public List<stDealRecordConsignmentUserCmd_S.Data> AccountRecordList
    {
        get
        {
            return m_recordList;
        }
    }
    public void OnReceiveRecordInfo(stDealRecordConsignmentUserCmd_S cmd)
    {
        m_recordList = cmd.data;
        DispatchValueUpdateEvent( new ValueUpdateEventArgs( SaleDispatchEvents.RefreshRecordInfo.ToString() , null , null ) );
    }
    #endregion

}

