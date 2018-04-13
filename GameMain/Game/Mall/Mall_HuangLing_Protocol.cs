//********************************************************************
//	创建日期:	2016-11-7   17:20
//	文件名称:	Vip_Protocol.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	皇令Protocol
//********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using table;
using GameCmd;
using Common;
using Engine.Utility;

partial class Protocol
{
    /// <summary>
    /// 购买皇令成功返回的消息
    /// </summary>
    /// <param name="cmd"></param>
    
    [Execute]
    public void BuyNobleSuccess(stBuyNoblePropertyUserCmd_CS cmd) 
    {
        DataManager.Manager<Mall_HuangLingManager>().OnBuyNobel(cmd);
    }
    /// <summary>
    /// 每天领取货币成功
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void GetNobleMoneySuccess(stNobleGiveDayFreeCoinPropertyUserCmd_CS cmd)
    {
        DataManager.Manager<Mall_HuangLingManager>().GetNobleMoneySuccess(cmd);
    }
    [Execute]
    public void FirstRechargeList(stAllRechTypeListPropertyUserCmd_S cmd) 
    {
        DataManager.Manager<Mall_HuangLingManager>().OnFirstRechargeList(cmd);
    }

    [Execute]
    public void OnRecieveMsg(stNobleInfoPropertyUserCmd_S cmd) 
    {

        DataManager.Manager<Mall_HuangLingManager>().OnRecieveMsg(cmd);
    }
    [Execute]
    public void OnExchangeMoney(stRechargeCoinPropertyUserCmd_CS cmd) 
    {
//         ExchangeMoneyPanel panel =(ExchangeMoneyPanel)DataManager.Manager<UIPanelManager>().GetPanel(PanelID.ExchangeMoneyPanel);
//         panel.ShowByMoneyType((ClientMoneyType)cmd.recharge_type);
//         TipsManager.Instance.ShowTips("兑换成功");
    }
}
