using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
// public class ConsignmentItemCompare : IComparer<ConsignmentItem>
// {
//     private static ConsignmentItemCompare m_compare = null;
//     public static ConsignmentItemCompare Create()
//     {
//         if (null == m_compare)
//         {
//             m_compare = new ConsignmentItemCompare();
//         }
//         return m_compare;
//     }
//     private ConsignmentItemCompare()
//     {
// 
//     }
//     public int Compare(uint leftID, uint rightID)
//     {
//         return ConsignmentItemCompare.CompareBaseItem(leftID, rightID);
//     }
// 
// }
public class ConsignmentItem
{
  
//     public int Compare(ConsignmentItem left, ConsignmentItem right)
//     {
//         return CompareItem(left, right);
//     }
    public ulong Market_ID
    {
        private set;
        get;
    }
    public ItemPageInfo page_info
    {
        private set;
        get;
    }
    public ItemSerialize server_data 
    {
        private set;
        get;
    }

    public ItemSellTimeInfo sell_timeInfo
    {
        private set;
        get;
    }
    public ConsignmentItem(ulong marketID, ItemPageInfo pageInfo = null,GameCmd.ItemSerialize serverdata = null,ItemSellTimeInfo selltime = null) 
    {
        UpdateData(marketID, pageInfo, serverdata, selltime);
    }
    public bool UpdateData(ulong marketID, ItemPageInfo pageInfo = null, GameCmd.ItemSerialize serverdata = null, ItemSellTimeInfo selltime = null)
    {
        bool success = false;
        if (marketID != 0)
        {
            this.Market_ID = marketID;
            if (pageInfo != null)
            {
               this.page_info = pageInfo;
            }
            if (serverdata != null)
            {
                this.server_data = serverdata;
            }
            if (selltime != null)
            {
                this.sell_timeInfo = selltime;
            }
            success = true;
        }
        return success;
    }
}