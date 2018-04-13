using System;
using System.Collections.Generic;
using System.Text;
using Cmd;

namespace Cmd
{
	/// <summary>
	/// 程序中要用到的常量定义
	/// </summary>
	public static class ConstDefine
	{
		public static System.Text.Encoding Encoding { get; set; }

		#region 各种命令中字符串的定长规定
		/// <summary>
		/// 保险箱二级密码
		/// </summary>
		public const int MAX_SUB_PASSWORD = 8;
		public const int MAX_IP_LENGTH = 16;
		/// <summary>
		/// 密码最大长度
		/// </summary>
		public const int MAX_PASSWORD = 16;
		public static int MAX_NAMESIZE { get; set; }
		public const int MAX_ACCNAMESIZE = 48;
		public static int MAX_CHATINFO { get; set; }
		public const int MAX_REASON_LEN = 128;
		public const int MAX_SHORT_NAME = 16;

		public static int MAX_URLSIZE = 1024;

		public static int MAX_NSIZE = 32;
		public static int MAX_VSIZE = 128;
		/// <summary>
		/// 聊天内容最大
		/// </summary>
		public const int MAX_CHATMSGLEN = 256;

		/// <summary>
		/// 拍卖者名字的最大长度
		/// </summary>
		public const int MAX_AUCTIONNAMESIZE = 32;

		/// <summary>
		/// 邮件正文的最大长度
		/// </summary>
		public const int MAX_MAIL_TEXTSIZE = 256;

		/// <summary>
		/// 群宣言长度
		/// </summary>
		public const int FLOCK_DECLARATION = 32;
		/// <summary>
		/// 群公告长度
		/// </summary>
		public const int FLOCK_POST = 256;
		/// <summary>
		/// 默认物理地址
		/// </summary>
		public const ulong DEFAULT_MAC = 13916110872;

		/// <summary>
		/// 极速狂飙（爱情长跑变种)道具栏大小
		/// </summary>
		public const uint RUNNING_ITEM_SET_SIZE = 3;

		/// <summary>
		/// 跨区极速狂飙
		/// </summary>
		public const uint CSRUNNING_ITEM_SET_SIZE = 3;

		public const byte MAX_SYSSET_LEN = 32;
		public const byte MAX_NUMPASSWORD = 32;
		/// <summary>
		/// 最大角色信息个数
		/// </summary>
		public const ushort MAX_CHARINFO = 5;

		/// <summary>
		/// 社交动作距离
		/// </summary>
		public const uint SOCIAL_ACTION_DISTANCE = 2;
		#endregion

		public const int MAX_PATH = 260;

		static ConstDefine()
		{
			//Encoding = System.Text.Encoding.GetEncoding("GB2312");
            Encoding = System.Text.Encoding.Default;
			MAX_NAMESIZE = 32;
			MAX_CHATINFO = 256;
		}

        //public static GXColor GetChatColor(GameCmd.CHATTYPE chatType)
        //{
        //    return GetChatColor((CHATTYPE)(int)chatType);
        //}
        public static GXColor GetChatColor(GameCmd.CHATTYPE chatType)
        {
            switch (chatType)
            {
                case GameCmd.CHATTYPE.CHAT_NONE: return GXColor.BlueSky;
                case GameCmd.CHATTYPE.CHAT_NINE: return GXColor.White;
                case GameCmd.CHATTYPE.CHAT_MAP: return GXColor.Violet;
                case GameCmd.CHATTYPE.CHAT_TEAM: return GXColor.GreenLight;
               // case GameCmd.CHATTYPE.CHAT_ARMY: return GXColor.GreenYellow;
                case GameCmd.CHATTYPE.CHAT_SILENT: return GXColor.Magenta;
               // case GameCmd.CHATTYPE.CHAT_COUNTRY: return GXColor.YellowP;
                case GameCmd.CHATTYPE.CHAT_WORLD: return GXColor.Orange;
               // case GameCmd.CHATTYPE.CHAT_NEIGHBOR: return GXColor.Tomato;
                case GameCmd.CHATTYPE.CHAT_PRIVATE: return GXColor.Turp;
                case GameCmd.CHATTYPE.CHAT_SYS: return GXColor.Orange;
                case GameCmd.CHATTYPE.CHAT_MESSAGEBOX: return GXColor.ListTitle;
                case GameCmd.CHATTYPE.CHAT_GM: return GXColor.Red;
               // case GameCmd.CHATTYPE.CHAT_PROVINCE: return GXColor.OrangeRed;
               // case GameCmd.CHATTYPE.CHAT_CITY: return GXColor.Tomato;
              //  case GameCmd.CHATTYPE.CHAT_GUILD: return GXColor.Solid;
                case GameCmd.CHATTYPE.CHAT_GMTOOL: return GXColor.Turp;
               // case GameCmd.CHATTYPE.CHAT_PROVINCE_CHANGEZONE: return GXColor.Orange;
               // case GameCmd.CHATTYPE.CHAT_WORLD_CHANGEZONE: return GXColor.BlueLight;
               // case GameCmd.CHATTYPE.CHAT_GUILD_CHANGEZONE: return GXColor.ListTitle;
               // case GameCmd.CHATTYPE.CHAT_CORPS: return GXColor.GreenDark;
                case GameCmd.CHATTYPE.CHAT_GMTOOL_SPEAK: return GXColor.Turp;
              //  case GameCmd.CHATTYPE.CHAT_NINE_SELL: return GXColor.White;
              //  case GameCmd.CHATTYPE.CHAT_THINK_ALOUD: return GXColor.White;
              //  case GameCmd.CHATTYPE.CHAT_PEEPWILD: return GXColor.White;
              //  case GameCmd.CHATTYPE.CHAT_NETBAR: return GXColor.Blue;
              //  case GameCmd.CHATTYPE.CHAT_UNION: return GXColor.Violet;
                case GameCmd.CHATTYPE.CHAT_SPEAKER: return GXColor.Yellow;
                default: return GXColor.BlueSky;
            }
        }

        public static GXColor GetItemColor(uint qulity)
        {
            switch (qulity)
            {
                case 1:
                    return GXColor.White;
                case 2:
                    return GXColor.Green;
                case 3:
                    return GXColor.Blue;
                case 4:
                    return GXColor.Violet;
                case 5:
                    return GXColor.Orange;
                default:
                    return GXColor.BlueSky;
            }
        }
        public static GXColor ToGXColor(this GameCmd.TaskColor color)
        {
            switch (color)
            {
                case GameCmd.TaskColor.TaskColor_White:
                    return GXColor.White;
                case GameCmd.TaskColor.TaskColor_Blue:
                    return GXColor.Blue;
                case GameCmd.TaskColor.TaskColor_Yellow:
                    return GXColor.YellowP;
                case GameCmd.TaskColor.TaskColor_Green:
                    return GXColor.Green;
                case GameCmd.TaskColor.TaskColor_Purple:
                    return GXColor.Turp;
            }
            return GXColor.White;
        }

		public static string GetProfessionNameByType(GameCmd.enumProfession type)
		{
			return GetProfessionNameByType((Cmd.enumProfession)(int)type);
		}

		public static string GetProfessionNameByType(enumProfession type)
		{
			switch (type)
			{
				case enumProfession.Profession_Soldier:
					return "战士";
				//case enumProfession.Profession_Spy:
				//	return "刺客";
				case enumProfession.Profession_Gunman:
					return "弓手";
				//case enumProfession.Profession_Blast:
				//	return "魔炮";
				case enumProfession.Profession_Freeman:
					return "法师";
				case enumProfession.Profession_Doctor:
					return "牧师";
				default:
					return "无效职业";
			}
		}

		public static string GetCountryNameByID(GameCmd.CountryName countryId)
		{
			return GetCountryNameByID((Cmd.CountryName)(int)countryId);
		}
		public static string GetCountryNameByID(CountryName countryId)
		{
			switch (countryId)
			{
				case CountryName.Sky:
					return "天之国";
				case CountryName.Fire:
					return "火之国";
				case CountryName.Water:
					return "水之国";
				case CountryName.Land:
					return "沙之国";
				case CountryName.unknow:
					return "未知国度";
				default:
					return "无效国家";
			}
		}

		/// <summary>
		/// 检查某个bit状态是否设置
		/// </summary>
		/// <param name="state">用来存放bit记录的字节数组</param>
		/// <param name="teststate">从左到右要测试的第多少位的bit</param>
		/// <returns>该bit位的值</returns>
		public static bool isset_state(byte[] state, int teststate)
		{
			return 0 != (state[teststate / 8] & (0xff & (1 << (teststate % 8))));
		}
		/// <summary>
		/// 设置某个状态
		/// </summary>
		/// <param name="state">用来存放bit记录的字节数组</param>
		/// <param name="teststate">从左到右要设置的第多少位的bit</param>
		public static void set_state(byte[] state, int teststate)
		{
			state[teststate / 8] |= (byte)(0xff & (1 << (teststate % 8)));
		}
		/// <summary>
		/// 清除某个状态
		/// </summary>
		/// <param name="state">用来存放bit记录的字节数组</param>
		/// <param name="teststate">从左到右要清除的第多少位的bit</param>
		public static void clear_state(byte[] state, int teststate)
		{
			state[teststate / 8] &= (byte)(0xff & (~(1 << (teststate % 8))));
		}

        //public static GXColor ToGXColor(this QuestUserCmd.TaskColor color)
        //{
        //    switch (color)
        //    {
        //        case QuestUserCmd.TaskColor.TaskColor_White:
        //            return Cmd.GXColor.White;
        //        case QuestUserCmd.TaskColor.TaskColor_Blue:
        //            return Cmd.GXColor.Blue;
        //        case QuestUserCmd.TaskColor.TaskColor_Yellow:
        //            return Cmd.GXColor.YellowP;
        //        case QuestUserCmd.TaskColor.TaskColor_Green:
        //            return Cmd.GXColor.Green;
        //        case QuestUserCmd.TaskColor.TaskColor_Purple:
        //            return Cmd.GXColor.Turp;
        //    }
        //    return Cmd.GXColor.White;
        //}
	}


	public enum GXColor : int
	{
		None = GXRGB.None,
		White = GXRGB.White,
		Blue = GXRGB.Blue,
		YellowP = GXRGB.YellowP,
		Green = GXRGB.Green,
		Turp = GXRGB.Turp,
		Red = GXRGB.Red,
		GreenLight = GXRGB.GreenLight,
		Orange = GXRGB.Orange,
		BlueLight = GXRGB.BlueLight,
		Black = GXRGB.Black,
		Gray = GXRGB.Gray,
		BackGround = GXRGB.BackGround,
		ListTitle = GXRGB.ListTitle,
		Solid = GXRGB.Solid,
		Yellow = GXRGB.Yellow,
		Tab = GXRGB.Tab,
		BlueSky = GXRGB.BlueSky,
		Pink = GXRGB.Pink,
		WhiteRice = GXRGB.WhiteRice,
		Tan = GXRGB.Tan,
		Chocolate = GXRGB.Chocolate,
		OrangeRed = GXRGB.OrangeRed,
		Tomato = GXRGB.Tomato,
		Violet = GXRGB.Violet,
		RedPig = GXRGB.RedPig,
		GreenDark = GXRGB.GreenDark,
		GreenYellow = GXRGB.GreenYellow,
		Magenta = GXRGB.Magenta,
		BackGround200 = (150 << 24) | (GXRGB.Black & 0xFFFFFF),
		GreenSky = (250 << 24) | (GXRGB.GreenSky & 0xFFFFFF),
		BackTips = (250 << 24) | (GXRGB.BackTips & 0xFFFFFF),
	}

	public enum GXRGB : int
	{
		None = 0,
		White = (255 << 24) | (229 << 16) | (242 << 8) | (242 << 0),
		Blue = (255 << 24) | (7 << 16) | (175 << 8) | (211 << 0),
		YellowP = (255 << 24) | (255 << 16) | (160 << 8) | (0 << 0),
		Green = (255 << 24) | (24 << 16) | (255 << 8) | (0 << 0),
		Turp = (255 << 24) | (210 << 16) | (0 << 8) | (255 << 0),
		Red = (255 << 24) | (255 << 16) | (0 << 8) | (0 << 0),
		GreenLight = (255 << 24) | (0 << 16) | (255 << 8) | (127 << 0),
		Orange = (255 << 24) | (255 << 16) | (116 << 8) | (73 << 0),
		BlueLight = (255 << 24) | (120 << 16) | (160 << 8) | (220 << 0),
		Black = (255 << 24) | (0 << 16) | (0 << 8) | (0 << 0),
		Gray = (255 << 24) | (130 << 16) | (130 << 8) | (130 << 0),
		BackGround = (255 << 24) | (33 << 16) | (30 << 8) | (21 << 0),
		ListTitle = (255 << 24) | (160 << 16) | (151 << 8) | (115 << 0),
		Solid = (255 << 24) | (153 << 16) | (109 << 8) | (84 << 0),
		Yellow = (255 << 24) | (253 << 16) | (255 << 8) | (60 << 0),
		Tab = (255 << 24) | (242 << 16) | (232 << 8) | (143 << 0),
		BlueSky = (255 << 24) | (0 << 16) | (255 << 8) | (255 << 0),
		Pink = (255 << 24) | (228 << 16) | (176 << 8) | (189 << 0),
		WhiteRice = (255 << 24) | (215 << 16) | (217 << 8) | (195 << 0),
		Tan = (255 << 24) | (210 << 16) | (180 << 8) | (140 << 0),
		Chocolate = (255 << 24) | (210 << 16) | (105 << 8) | (30 << 0),
		OrangeRed = (255 << 24) | (255 << 16) | (69 << 8) | (0 << 0),
		Tomato = (255 << 24) | (255 << 16) | (99 << 8) | (71 << 0),
		Violet = (255 << 24) | (238 << 16) | (130 << 8) | (238 << 0),
		RedPig = (255 << 24) | (115 << 16) | (0 << 8) | (0 << 0),
		GreenDark = (255 << 24) | (124 << 16) | (252 << 8) | (0 << 0),
		GreenYellow = (255 << 24) | (154 << 16) | (205 << 8) | (50 << 0),
		Magenta = (255 << 24) | (228 << 16) | (0 << 8) | (127 << 0),
		GreenSky = (255 << 24) | (98 << 16) | (255 << 8) | (137 << 0),
		BackTips = (255 << 24) | (71 << 16) | (54 << 8) | (28 << 0),
	}

	public enum enumProfession
	{
		Profession_None = 0,
		/// <summary>
		/// 战士
		/// </summary>
		Profession_Soldier = 1,
		/// <summary>
		/// 刺客
		/// </summary>
		[Obsolete]
		Profession_Spy = 2,
		/// <summary>
		/// 弓手
		/// </summary>
		Profession_Gunman = 3,
		/// <summary>
		/// 魔炮
		/// </summary>
		[Obsolete]
		Profession_Blast = 4,
		/// <summary>
		/// 法师
		/// </summary>
		Profession_Freeman = 5,
		/// <summary>
		/// 牧师
		/// </summary>
		Profession_Doctor = 6,
		Profession_Max = 7,
	}

	public enum CountryName
	{
		unknow = 1,
		/// <summary>
		/// 天之国
		/// </summary>
		Sky = 2,
		/// <summary>
		/// 火之国
		/// </summary>
		Fire = 3,
		/// <summary>
		/// 水之国
		/// </summary>
		Water = 4,
		/// <summary>
		/// 沙之国
		/// </summary>
		Land = 5,
	}

	/// <summary>
	/// 关系枚举
	/// </summary>
	public enum RelationType : byte
	{
		/// <summary>
		/// 好友
		/// </summary>
		Relation_Friend = 0,		
		/// <summary>
		/// 恋人
		/// </summary>
		Relation_Honey = 1,		    
		/// <summary>
		/// 夫妻
		/// </summary>
		Relation_Spouse = 2,		
		/// <summary>
		/// 仇人成员
		/// </summary>
		Relation_Enemy = 3,		  
  		/// <summary>
		/// 黑名单成员
  		/// </summary>
		Relation_Black = 4,		  
		/// <summary>
		/// 战士
		/// </summary>
		Relation_Soldier = 5,		
		/// <summary>
		/// 组队
		/// </summary>
		Relation_Team = 6,		    
		/// <summary>
		/// 团
		/// </summary>
		Relation_Corps = 7,		   
 		/// <summary>
		/// 军
 		/// </summary>
		Relation_Army = 8,		   
		/// <summary>
		/// 国
		/// </summary>
		Relation_Country = 9,		
		/// <summary>
		/// 世界
		/// </summary>
		Relation_World = 10,		
		/// <summary>
		/// 战队
		/// </summary>
		Relation_FightTeam = 11,	
		/// <summary>
		/// 最大数值
		/// </summary>
		Relation_MAX				
	};

	/// <summary>
	/// 队伍模式
	/// </summary>
	public enum TeamMode : byte
	{
		TeamMode_Normal,	/**< 普通组队 */
		TeamMode_Friend,	/**< 好友模式 */
		TeamMode_Max,		/**< 最大值 */
	}


	/// <summary>
	/// 物件类型
	/// </summary>
	public enum SceneEntryType
	{
		SceneEntry_Player = 0,		/**< 玩家角色*/
		SceneEntry_NPC,			/**< NPC*/
		SceneEntry_Object,		/**< 地上道具*/
		SceneEntry_AIEntity,	/**< ainpc*/
		SceneEntry_MAX
	}

	public enum enmCharSex
	{
		None,
		/// <summary>
		/// 男人
		/// </summary>
		MALE = 1,
		/// <summary>
		/// 女人
		/// </summary>
		FEMALE = 2,
	}
	/// <summary>
	/// 团及军信息刷新请求
	/// </summary>
	public enum enumInfoRequest
	{
		/// <summary>
		/// 团属性
		/// </summary>
		Corps_Property = 1,
		/// <summary>
		/// 团出勤人员
		/// </summary>
		Corps_WorkerList = 2,
		/// <summary>
		/// 团职务列表
		/// </summary>
		Corps_DutyList = 3,
		/// <summary>
		/// 占领信息
		/// </summary>
		Corps_OccupyList = 4,
		/// <summary>
		/// 团战积分榜
		/// </summary>
		Corps_GradeList = 5,
		/// <summary>
		/// 团副本积分榜
		/// </summary>
		Corps_CopyGradeList = 6,
		/// <summary>
		/// 繁荣度榜单
		/// </summary>
		Corps_GloryList = 7,
		/// <summary>
		/// 健康指数榜单
		/// </summary>
		Corps_HealthList = 8,
		/// <summary>
		/// 贡献度榜单
		/// </summary>
		Corps_OfferList = 9,
		/// <summary>
		/// 军属性
		/// </summary>
		Army_Property = 101,
		/// <summary>
		/// 军实力排行
		/// </summary>
		Army_GradeList = 102,
		/// <summary>
		/// 军的团战情况列表
		/// </summary>
		Army_BattleList = 103,
		/// <summary>
		/// 军的团占领列表
		/// </summary>
		Army_OccupyList = 104,
		/// <summary>
		/// 军的团列表
		/// </summary>
		Army_CorpsList = 105,
		/// <summary>
		/// 军占领
		/// </summary>
		Army_Occupy = 106,
	}
}
