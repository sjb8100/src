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

    [Execute]
    public void OnRecieveOpenNpcShop(stOpenShopScriptUserCmd_S cmd)
    {
        DataManager.Manager<Mall_NpcShopManager>().OnOpenNpcShopCommond(cmd.shop_list, cmd.npc_base_id);
    }
}
