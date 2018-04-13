/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Recharge
 * 创建人：  wenjunhua.zqgame
 * 文件名：  RechargeManager
 * 版本号：  V1.0.0.0
 * 创建时间：12/5/2017 3:27:21 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
class RechargeManager:IManager
{
    #region define
    /// <summary>
    /// 支付渠道类型
    /// </summary>
    public enum PayChannelType
    {
        PCT_General = 0,    //通用
        PCT_IOS = 1,        //IOS平台
        PCT_Andorid = 2,    //Android平台
    }

    /// <summary>
    /// 充值类型
    /// </summary>
    public enum RechargeType
    {
        RT_MallRecharge = 1, //商城充值
        RT_HuangLing = 2,    //皇令充值
        RT_GiftBag = 3,      //礼包充值
        RT_Foundation = 4,   //购买基金
    }
    #endregion

    #region property
    //充值是否开启
    private bool m_bRechargeOpen = true;
    public bool RechargeOpen
    {
        get
        {
            return m_bRechargeOpen;
        }
    }
    //当前充值渠道
    private uint m_curPayChannelType = (uint)PayChannelType.PCT_General;
    public uint CurPayChannelType 
    {
        get 
        {
            return m_curPayChannelType;
        }
    }
    #endregion

    #region IManager Method
    
    public void Initialize()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            m_curPayChannelType = (uint)PayChannelType.PCT_IOS;
        }else
        {
            m_curPayChannelType = (uint)PayChannelType.PCT_Andorid;
        }
    }

    public void Reset(bool depthClearData = false)
    {
        if (depthClearData)
        {
        
        }
    }

    public void Process(float deltaTime)
    {

    }

    public void ClearData()
    {

    }
    #endregion

    #region Pay

    /// <summary>
    /// 根据充值类型获取充值数据列表
    /// </summary>
    /// <param name="rechargeType"></param>
    /// <returns></returns>
    public List<uint> GetRechargeIDsByType(RechargeType rechargeType)
    {
        List<uint> rechargeIDs = null;
        List<table.RechargeDataBase> rechargeDatas = GameTableManager.Instance.GetTableList<table.RechargeDataBase>();
        if (null != rechargeDatas)
        {
            table.RechargeDataBase tempDB = null;
            uint rechargeuIntType = (uint) rechargeType;
            uint payChannelType = (uint) PayChannelType.PCT_General;
            for(int i = 0,max = rechargeDatas.Count;i < max;i ++)
            {
                tempDB = rechargeDatas[i];
                if ((tempDB.rechargeType == rechargeuIntType )
                    && ((tempDB.type == m_curPayChannelType) || (tempDB.type == payChannelType)))
                {
                    if (null == rechargeIDs)
                    {
                        rechargeIDs = new List<uint>();
                    }

                    if (!rechargeIDs.Contains(tempDB.dwID))
                    {
                        rechargeIDs.Add(tempDB.dwID);
                    }
                }
            }
        }
        return rechargeIDs;
    }

    /// <summary>
    /// 充值
    /// </summary>
    /// <param name="rechargeID"></param>
    public void DoRecharge(uint rechargeID)
    {
        if (!DataManager.Manager<LoginDataManager>().SDKLoginEnable)
        {
            TipsManager.Instance.ShowTips("当前平台充值不可用");
            return;
        }
        if (!m_bRechargeOpen)
        {
            TipsManager.Instance.ShowTips("充值暂未开放");
            return;
        }
        UIPanelManager pmgr = DataManager.Manager<UIPanelManager>();
        WaitPanelShowData waitData = new WaitPanelShowData();
        waitData.type = WaitPanelType.Waitting;
        waitData.cdTime = 20;
        waitData.des = "充值中...";
        waitData.useBoxMask = false;
        waitData.showTimer = false;
        if (!pmgr.IsShowPanel(PanelID.CommonWaitingPanel))
        {
            pmgr.ShowPanel(PanelID.CommonWaitingPanel, data: waitData);
        }
        RequestRechargeOrder(rechargeID);
    }
    
    /// <summary>
    /// 向服务器请求订单
    /// </summary>
    /// <param name="rechargeID"></param>
    private void RequestRechargeOrder(uint rechargeID)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.RechargeOrderReq(rechargeID);
    }
    const float CURRENCY_RATIO = 0.1f;
    /// <summary>
    /// 服务器返回订单信息
    /// </summary>
    public void OnRequestRechargeOrder(GameCmd.stCreatePlatOrderPropertyUserCmd_S msg)
    {
        Client.IPlayer mainPlayer = DataManager.Instance.MainPlayer;
        if (null == mainPlayer)
            return;
        uint characterID = mainPlayer.GetID();
        int characterLv = mainPlayer.GetProp((int)Client.CreatureProp.Level);
        ClanDefine.LocalClanInfo clanInfo = DataManager.Manager<ClanManger>().ClanInfo;
        string clanName = "";
        if (null != clanInfo)
        {
            clanName = clanInfo.Name;
        }

        Pmd.ZoneInfo zoneInfo = DataManager.Manager<LoginDataManager>().GetZoneInfo();
        string currencyName = "元宝";
        string roleName = mainPlayer.GetName();
        table.RechargeDataBase rechargeDb = GameTableManager.Instance.GetTableItem<table.RechargeDataBase>(msg.goodid);
        //平台支付
        DoPlatformPay(msg, characterID, characterLv, roleName, clanName, zoneInfo, currencyName, CURRENCY_RATIO, rechargeDb);
    }

    public void DoPlatformPay(GameCmd.stCreatePlatOrderPropertyUserCmd_S msg
        ,uint characterId,int characterlv,string roleName,
        string clanName,Pmd.ZoneInfo zoneInfo,string currencyName,float currencyRatio,table.RechargeDataBase rechargeDb)
    {
        LoginDataManager lg = DataManager.Manager<LoginDataManager>();
        int payMoney = (int)rechargeDb.money;
        int fenPayMoney = payMoney * 100;
        string moneyStr = fenPayMoney.ToString();
        string characterIDStr = characterId.ToString();
        string gameOrder = msg.gameorder;
        string token = lg.LoginToken;
        string serverNo = lg.ServerNo;
        string serverName = lg.ServerName;
        string account = lg.Acount;
        string accountIDStr = lg.AccountID;
        string characterLvStr = characterlv.ToString();

        if (Application.platform == RuntimePlatform.Android)
        {
            CommonSDKPlaform.Instance.Pay(msg, characterId, characterlv, roleName, clanName, zoneInfo, currencyName, currencyRatio, rechargeDb);
        }else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            ZqgameSDKController.Instance.Pay(moneyStr, account, characterIDStr, gameOrder, accountIDStr, token, rechargeDb.iosIAPId, serverNo, characterLvStr);
        }
        
    }
    
    /// <summary>
    /// 平台返回支付结果
    /// </summary>
    /// <param name="success"></param>
    /// <param name="orderID"></param>
    /// <param name="transaction">IOS特有</param>
    public void OnPlatformPayResult(bool success, string orderID, string transaction = null)
    {
        UIPanelManager pmgr = DataManager.Manager<UIPanelManager>();
        if (pmgr.IsShowPanel(PanelID.CommonWaitingPanel))
        {
            pmgr.HidePanel(PanelID.CommonWaitingPanel);
        }
    }

    /// <summary>
    /// 服務器返回充值結果
    /// </summary>
    /// <param name="success"></param>
    /// <param name="rechargeID"></param>
    /// <param name="num"></param>
    public void OnServerBackPayResult(bool success,uint rechargeID,uint num)
    {
        UIPanelManager pmgr = DataManager.Manager<UIPanelManager>();
        if (pmgr.IsShowPanel(PanelID.CommonWaitingPanel))
        {
            pmgr.HidePanel(PanelID.CommonWaitingPanel);
        }

    }
    #endregion

    #region Query
    private long last_query_time = 0;
    private uint query_cd_time = 0;
    public uint QueryCDTime
    {
        get
        {
            if (query_cd_time == 0)
            {
                query_cd_time = GameTableManager.Instance.GetGlobalConfig<uint>("ReqRechargeResultCD");
            }
            return query_cd_time;
        }
    }
    /// <summary>
    /// 请求刷新充值状态
    /// </summary>
    public void QueryRefreshRechargeStatus()
    {
        if (DateTimeHelper.Instance.Now - last_query_time <= QueryCDTime)
        {
            TipsManager.Instance.ShowTips("请求刷新充值结果CD中...");
            return;
        }
        if (null != DataManager.Instance.Sender)
        {
            last_query_time = DateTimeHelper.Instance.Now;
            DataManager.Instance.Sender.QueryRechargeResultReq();
        }
    }
    #endregion

}