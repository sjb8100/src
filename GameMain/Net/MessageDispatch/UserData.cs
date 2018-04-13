using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using System.Xml.Linq;
public interface IXmlSerializable
{
    XElement Serialize();
    void Deserialize(XElement xml);
}

/// <summary>
/// 全局游戏数据
/// </summary>
/// 
partial class UserData :BaseModuleData
{
    #region 登录相关
    public static uint Version { get { return 20201008; } }
    /// <summary>
    /// 客户端对外的IP
    /// </summary>
    public static string ClientIP { get; set; }
    /// <summary>
    /// 游戏的运营区域，比如：大陆、海外等
    /// </summary>
    public static int GameRegion { get; set; }
    /// <summary>
    /// 网络消息的编码
    /// </summary>
    public static System.Text.Encoding CmdEncoding { get; set; }

    public static string LoginServerIP { get; set; }
    public static int LoginServerPort { get; set; }

    public static string GatewayServerIP { get; set; }
    public static int GatewayServerPort { get; set; }

    /// <summary>
    /// 服务器是否启用DEBUG状态
    /// </summary>
    public static bool ServerDebug { get; set; }
    /// <summary>
    /// 客户端所在的省市信息
    /// </summary>
    public static uint ProvinceCity { get; set; }

    /// <summary>
    /// 仅在登录时使用
    /// </summary>
    public static ulong AccountID { get; set; }
    /// <summary>
    /// 生成一个临时编号,用于校验,登陆网关时传上去
    /// </summary>
    public static ulong LoginTempID { get; set; }
    /// <summary>
    /// 随机token
    /// </summary>
    public static ulong TokenID { get; set; }

    public static uint Accid { get; set; }
    public static uint Channel { get; set; }
    //public static string Account { get; set; }

    /// <summary>
    /// 游戏ID，用来唯一代表某款游戏
    /// </summary>
    public static uint GameID { get; set; }
    /// <summary>
    /// 本游戏的名称
    /// </summary>
    public static string GameName { get; set; }

    //当前选择的区
    //public static Pmd.ZoneInfo Zone { get; set; }


    #region 金币

    private int mGold;
    /// <summary> 玩家金币 </summary>
    public int gold
    {
        get
        {
            return mGold;
        }
        set
        {
            if (mGold == value)
            {
                return;
            }
            if (value <= 0)
            {
                mGold = 0;
            }

            ValueUpdateEventArgs ve = new ValueUpdateEventArgs();
            ve.key = "gold";
            ve.oldValue = mGold;
            ve.newValue = value;
            mGold = value;
            DispatchValueUpdateEvent(ve);
        }
    }
    #endregion
    //服务器时间
    /// <summary>
    /// 得到客户端模拟的服务器当前时间
    /// </summary>
    public static DateTime ServerTime { get { return DateTime.Now.AddSeconds(server_time_delta); } }
    static double server_time_delta;
    public static double ServerTimeDelta
    {
        get { return server_time_delta; }
        set {
            DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0);
            server_time_delta = value - (DateTime.Now - UnixBase).TotalSeconds;
        }
    }

    public static uint ReConnTempID;

    static UserData()
    {
        CmdEncoding = Cmd.ConstDefine.Encoding;
        uint index = 1;
        if(GameTableManager.Instance.IsConnectBwt)
        {
            index = 2;
        }
        var item = GameTableManager.Instance.GetTableItem<table.GameGlobalConfig>(index);
        GameName = item.GameName;
        GameID = item.GameID;
        //ZoneID = 12;
        LoginServerIP = item.LoginSeverIP;
        LoginServerPort =(int) item.LoginSeverPort;

        //GameID = 2000;
        //ZoneID = 12;
        //LoginServerIP = "192.168.86.58";
        //LoginServerPort = 7000;

        CountryList = new List<GameCmd.Country_Info>();
       // RoleList = new List<GameCmd.SelectUserInfo>();
    }

    #endregion


    #region Map Info
    /// <summary>
    /// 地图配置信息,从mapinfo.xml读入
    /// </summary>
    public class MapInfo : IXmlSerializable
    {
        /// <summary>
        /// 地图id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 地图中文名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 地图文件名
        /// </summary>
        public string filename { get; set; }

        public string filename_client { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        #region IXmlSerializable 成员

        public XElement Serialize()
        {
            return null;
            //return new XElement("map",
            //    new XAttribute("id", this.id),
            //    new XAttribute("name", this.name),
            //    new XAttribute("filename", this.filename),
            //    new XAttribute("filename_client", this.filename_client),
            //    new XAttribute("width", this.width),
            //    new XAttribute("height", this.height));
        }

        public void Deserialize(XElement xml)
        {
            //this.id = xml.AttributeValue("id").Parse(0);
            //this.name = xml.AttributeValue("name");
            //this.filename = xml.AttributeValue("filename");
            //this.filename_client = xml.AttributeValue("filename_client");
            //this.width = xml.AttributeValue("width").Parse(0);
            //this.height = xml.AttributeValue("height").Parse(0);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("#{0} {1} {2}x{3} {4}", id, name, width, height, filename);
        }
    }

    public static IList<MapInfo> MapList { get;  set; }

    public static uint MapID { get; set; }
    public static MapInfo Map { get { return MapList != null ? MapList.FirstOrDefault(i => i.id == MapID) : null; } }
    #endregion

    #region Role Info
    /// <summary>
    /// 该账号对应的所有角色列表
    /// </summary>
    //public static IList<GameCmd.SelectUserInfo> RoleList { get; set; }

    private static SelectUserInfo currentRole;
    /// <summary>
    /// 当前主角基本信息
    /// </summary>
    public static SelectUserInfo CurrentRole
    {
        get
        {
            return currentRole;
        }
        set
        {
            currentRole = value;
            //skillBar = null;
            //godEvilSkill = null;
        }
    }

    public static bool IsMainRoleID(uint id)
    {
        return id == CurrentRole.id;
    }

    public static MapVector2 Pos { get; set; }

    // (文钱)
    public static int Money { get; set; }
    // (金币)
    public static int Coupon { get; set; }
    // (元宝)
    public static int Cold { get; set; }
    //积分
    public static int Score { get; set; }
    //声望
    public static int Reputation { get; set; }
    //成就
    public static int AchievePoint { get; set; }
    //战勋
    public static int CampCoin { get; set; }
    //狩猎积分
    public static int ShouLieScore { get; set; }
    //鱼币
    public static int FishingMoney { get; set; }
    //银两
    public static int YinLiang { get; set; }
    #endregion

    #region Country Info
    /// <summary>
    /// 游戏中的国家列表
    /// </summary>
    public static IList<Country_Info> CountryList { get; set; }

    ///// <summary>
    ///// 国家的状态信息
    ///// </summary>
    //public static GameCmd.stAnswerCountryInfoRelationUserCmd CountryState { get; set; }

    /// <summary>
    /// 主角所属国（国籍）ID
    /// </summary>
    public static uint HomeCountryID { get { return CurrentRole != null ? (uint)CurrentRole.homeland : 0; } }
    /// <summary>
    /// 主角所属国（国籍）信息
    /// </summary>
    public static Country_Info HomeCountry
    {
        get
        {
            var id = HomeCountryID;
            return CountryList.FirstOrDefault(i => i.id == id);
        }
    }
    /// <summary>
    /// 主角当前所在国ID
    /// </summary>
    public static uint CurrentCountryID { get; set; }
    /// <summary>
    /// 主角当前所在国信息
    /// </summary>
    public static Country_Info CurrentCountry
    {
        get
        {
            var id = CurrentCountryID;
            return CountryList.FirstOrDefault(i => i.id == id);
        }
    }

    #endregion
}
