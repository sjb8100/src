
//*************************************************************************
//	创建日期:	2017/4/8 星期六 16:04:39
//	文件名称:	MainPlayerHelper
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using GameCmd;
using table;
using Engine;
using Engine.Utility;
using System.Collections;

public enum ClientMoneyType
{
    [System.ComponentModel.Description("无效")]
    Invalid = 0,
    [System.ComponentModel.Description("金币")]
    Gold = 1,
    [System.ComponentModel.Description("绑元")]
    Wenqian = 2,
    [System.ComponentModel.Description("元宝")]
    YuanBao = 4,
    [System.ComponentModel.Description("积分")]
    JiFen = 8,
    [System.ComponentModel.Description("声望")]
    ShengWang = 16,
    [System.ComponentModel.Description("成就点")]
    ChengJiuDian = 32,
    [System.ComponentModel.Description("阵营战积分")]
    ZhenYingZhanJiFen = 128,
    [System.ComponentModel.Description("猎魂")]
    HuntingCoin = 256,
    [System.ComponentModel.Description("钓鱼积分")]
    FishingCoin = 512,
    [System.ComponentModel.Description("鱼币")]
    FishingMoney = 1024,
    [System.ComponentModel.Description("银两")]
    YinLiang = 2048,
    
}

class MainPlayerHelper
{
    /// <summary>
    /// 文钱
    /// </summary>
    public static uint MoneyTicketID = 60002;
    /// <summary>
    /// 金币
    /// </summary>
    public static uint GoldID = 60001;
    /// <summary>
    /// 元宝
    /// </summary>
    public static uint CoinID = 60003;

    public static uint ExpID = 60006;

    public static uint GetItemIDByClientMoneyType(ClientMoneyType type)
    {
        switch (type)
        {
            case ClientMoneyType.Gold:
                return 60001;
            case ClientMoneyType.Wenqian:
                return 60002;
            case ClientMoneyType.YuanBao:
                return 60003;
            case ClientMoneyType.JiFen:
                return 60008;
            case ClientMoneyType.ShengWang:
                return 60005;
            case ClientMoneyType.ChengJiuDian:
                return 600012;
            case ClientMoneyType.ZhenYingZhanJiFen:
                return 60011;
            case ClientMoneyType.YinLiang:
                return 60016;
            default:
                return 0;
        }
    }
    public static IPlayer GetMainPlayer()
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            return player;
        }
        //Engine.Utility.Log.Error("mainpalyer is null");
        return null;
    }
    public static long GetPlayerUID()
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            return player.GetUID();
        }
        return 0;
    }
    public static uint GetPlayerID()
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            return player.GetID();
        }
        return 0;
    }
    public static bool IsMainPlayer(uint userID)
    {
        if (userID == GetPlayerID())
        {
            return true;
        }
        return false;
    }
    public static bool IsMainPlayer(long userUID)
    {
        if (userUID == GetPlayerUID())
        {
            return true;
        }
        return false;
    }
    public static bool IsMainPlayer(IEntity en)
    {
        if (en == null)
        {
            return false;
        }
        if (en.GetUID() == GetPlayerUID())
        {
            return true;
        }
        return false;
    }
    public static int GetMainPlayerJob()
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            return player.GetProp((int)PlayerProp.Job);
        }
        return 0;
    }
    public static int GetPlayerVip()
    {//暂时没有
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            return 100;
        }
        return 0;
    }
    public static bool HasEnoughVipLevel(uint vipLevel)
    {
        if (GetPlayerLevel() >= vipLevel)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static int GetPlayerLevel()
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            int level = player.GetProp((int)CreatureProp.Level);
            return level;
        }
        return 0;
    }
    public static string GetMoneyIconByType(int type)
    {
        return GetMoneyIconByType((ClientMoneyType)type);
    }
    public static string GetMoneyIconByType(ClientMoneyType type)
    {
        if (type == ClientMoneyType.Wenqian)
        {
            return "tubiao_huobi_yin";
        }
        else if (type == ClientMoneyType.Gold)
        {
            return "tubiao_huobi_tong";
        }
        else if (type == ClientMoneyType.YuanBao)
        {

            return "tubiao_huobi_jin";
        }
        else if (type == ClientMoneyType.YinLiang)
        {
            return "tubiao_huobi_yinliang";
        }
        return "tubiao_huobi_jin";
    }
    public static int GetMoneyNumByType(ClientMoneyType type)
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        int num = 0;
        if (player != null)
        {
            switch(type)
            {
                case  ClientMoneyType.Wenqian:
                     num = player.GetProp((int)PlayerProp.Money);
                    break;
                case  ClientMoneyType.Gold:
                     num = player.GetProp((int)PlayerProp.Coupon);
                    break;
                case  ClientMoneyType.YuanBao:
                     num = player.GetProp((int)PlayerProp.Cold);
                    break;
                case  ClientMoneyType.FishingMoney:
                    num = player.GetProp((int)PlayerProp.FishingMoney);
                    break;
                case ClientMoneyType.YinLiang:
                    num = player.GetProp((int)PlayerProp.YinLiang);
                    break;
            }
        }
        return num;
    }
    public static string GetMoneyNameByType(uint type)
    {
        string str = "绑元";
        if (type == (uint)ClientMoneyType.Wenqian)
        {
            return str;
        }
        else if (type == (uint)ClientMoneyType.Gold)
        {
            return "金币";
        }
        else if (type == (uint)ClientMoneyType.YuanBao)
        {
            return "元宝";
        }
        else if (type == (uint)ClientMoneyType.YinLiang)
        {
            return "银两";
        }
        return str;
    }
    public static bool IsHasEnoughMoney(uint type, uint needMoney, bool bShowTips = true)
    {
        ClientMoneyType ct = (ClientMoneyType)Enum.ToObject(typeof(ClientMoneyType), type);
        if (ct != null)
        {
            return IsHasEnoughMoney(ct, needMoney,bShowTips);
        }
        else
        {
            Log.Error("ClientMoneyType enum parse error type is{0}" , type.ToString());
        }
        return false;

        //EntitySystem.EntityHelper.GetEntity(0);
    }
    public static bool IsHasEnoughMoney(ClientMoneyType type, uint needMoney, bool bShowTips = true)
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            if (type == ClientMoneyType.Wenqian)
            {
                int num = player.GetProp((int)PlayerProp.Money);
                if (num >= needMoney)
                {
                    return true;
                }
                else
                {
                    if (bShowTips)
                    {
                        TipsManager.Instance.ShowTipsById(3);
                    }
                }
            }
            else if (type == ClientMoneyType.Gold)
            {
                int num = player.GetProp((int)PlayerProp.Coupon);
                if (num >= needMoney)
                {
                    return true;
                }
                else
                {
                    if (bShowTips)
                    {
                        TipsManager.Instance.ShowTipsById(4);
                    }
                }
            }
            else if (type == ClientMoneyType.YuanBao)
            {
                int num = player.GetProp((int)PlayerProp.Cold);
                if (num >= needMoney)
                {
                    return true;
                }
                else
                {
                    if (bShowTips)
                    {
                        TipsManager.Instance.ShowTipsById(5);
                    }
                }
            }
            else if (type == ClientMoneyType.YinLiang)
            {
                int num = player.GetProp((int)PlayerProp.YinLiang);
                if (num >= needMoney)
                {
                    return true;
                }
                else
                {
                    if (bShowTips)
                    {
                        TipsManager.Instance.ShowTipsById(5);
                    }
                }
            }
        }
        return false;
    }


}
