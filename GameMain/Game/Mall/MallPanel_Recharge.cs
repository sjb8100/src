using System;
using UnityEngine;
using Client;
using GameCmd;
using table;
using Engine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utility;
using Common;
partial class MallPanel : UIPanelBase
{
    private BlockGridScrollView m_blockGridCreator = null;
    List<uint> rechargeIDs = new List<uint>();
 
    void CreateRechargeGrids()
    {
        rechargeIDs.Clear();
        List<uint> tempIds = DataManager.Manager<RechargeManager>().GetRechargeIDsByType(RechargeManager.RechargeType.RT_MallRecharge);
        if (null != tempIds)
        {
            rechargeIDs.AddRange(tempIds);
        }
        if (null != m_blockGridCreator)
        {
            m_blockGridCreator.CreateView(rechargeIDs.Count);
        }
    }


    private void OnRechargeGridDataUpdate(UIGridBase data, int index)
    {
        if (data is UIRechargeGrid)
        {
            UIRechargeGrid rg = data as UIRechargeGrid;
            if (index < rechargeIDs.Count)
            {
                rg.SetGridData(rechargeIDs[index]);
            }
           
        }
    }
    private void OnRechargeGridEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UIRechargeGrid)
                {
                    UIRechargeGrid grid = data as UIRechargeGrid;
                    if (grid != null)
                    {
                        OnRecharge(grid.RechargeID);
                    }
                   
                }
                break;
        }
    }
    void OnRecharge(uint id)
    {
        if (Application.isEditor)
        {
            NetService.Instance.Send(new stRequstRechargePropertyUserCmd_CS() { id = id });
        }
        else
        {
            DataManager.Manager<RechargeManager>().DoRecharge(id);
        }
//         table.RechargeDataBase table = GameTableManager.Instance.GetTableItem<RechargeDataBase>(id);
//         Action cancle = delegate
//         {
//             TipsManager.Instance.ShowTips("充值失败，请重试");
//         };
//         Action yes = delegate
//         {
//             if (Application.isEditor)
//             {
//                 NetService.Instance.Send(new stRequstRechargePropertyUserCmd_CS() { id = id });
//             }else
//             {
//                 DataManager.Manager<RechargeManager>().DoRecharge(id);
//             }
//             
//         };
//        TipsManager.Instance.ShowTipWindow(TipWindowType.YesNO, string.Format(DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Notice_Recharge_RechargeNum), table.money, table.kehuodianquan), yes, cancle,null, "提示", "确定", "取消");

    }

    void onClick_RefreshBtn_Btn(GameObject obj)
    {
        DataManager.Manager<RechargeManager>().QueryRefreshRechargeStatus();
    }
 
//     void RefreshRechargeGrid() 
//     {
//         int index =(int)DataManager.Manager<Mall_HuangLingManager>().NobleID-2;
//         UIGridBase b = m_blockGridCreator.GetGrid(index);
//         OnNobleGridDataUpdate(b,index);        
//     }

}
