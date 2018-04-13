//*************************************************************************
//	创建日期:	2016-11-24 18:53
//	文件名称:	ReplenishSignPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	签到补签弹窗
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
using GameCmd;

partial class ReplenishSignPanel : UIPanelBase
{
    int m_nOneCost = 0;
    int m_nAllCost = 0;
    List<WelfareData> lstWelFareData = null;

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        WelfareManager dataMgr = DataManager.Manager<WelfareManager>();
        lstWelFareData = dataMgr.GetWelfareDatasByType(WelfareType.Month);
        int money = GameTableManager.Instance.GetGlobalConfig<int>("SupplementSignCost");
        m_nOneCost = money;
        int canSignDay = (int)(dataMgr.CurrDay - dataMgr.SignDay);
        m_nAllCost = canSignDay * money;

        m_label_LabelAll.text = m_nAllCost.ToString();
        m_label_LabelOne.text = m_nOneCost.ToString();

        string str = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_FM_WelfareMonth_ReSign);
        m_label_LabelDes.text = string.Format(str, canSignDay);
    }

    void onClick_Btnclose_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    void onClick_Btn_1_Btn(GameObject caster)
    {
//         if (!MainPlayerHelper.IsHasEnoughMoney(ClientMoneyType.YuanBao, (uint)m_nOneCost))
//         {
//             return;
//         }
        List<uint> lstIDs = new List<uint>();
        WelfareManager dataMgr = DataManager.Manager<WelfareManager>();
       
        WelfareData matchedData = lstWelFareData.Find(P => P.param == dataMgr.SignDay + 1);
        if (matchedData != null)
        {
            lstIDs.Add(matchedData.id);
            DataManager.Instance.Sender.ReqGetReward(ref lstIDs);
        }
        else
        {
            Engine.Utility.Log.Error("找不到福利id：{0}",dataMgr.SignDay + 1);
        }
        this.HideSelf();
    }

    void onClick_Btn_2_Btn(GameObject caster)
    {
//         if (!MainPlayerHelper.IsHasEnoughMoney(ClientMoneyType.YuanBao, (uint)m_nAllCost))
//         {
//             return;
//         }
        WelfareManager dataMgr = DataManager.Manager<WelfareManager>();    
        int canSignDay = (int)(dataMgr.CurrDay - dataMgr.SignDay);
        List<uint> lstIDs = new List<uint>();
        for (int i = 0; i < lstWelFareData.Count; i++)
        {
            if (lstWelFareData[i].state == QuickLevState.QuickLevState_HaveGet)
            {
                continue;
            }

            if (lstWelFareData[i].param > dataMgr.CurrDay)
            {
                continue;
            }
            lstIDs.Add(lstWelFareData[i].id);
        }
        DataManager.Instance.Sender.ReqGetReward(ref lstIDs);

        this.HideSelf();
    }
}