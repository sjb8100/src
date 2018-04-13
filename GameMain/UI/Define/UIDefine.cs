using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Client{
    //重新定义此类 避免相互引用
    public class UIPanelInfo
    {
        public enum UIColliderMode
        {
            CM_不含碰撞背景 = 0,
            CM_透明背景,
            CM_半透背景,
        }

        public enum UIShowModel
        {
            SM_无操作 = 0,
            SM_关闭互斥Normal面板,//需要通用背景，topbar的是互斥
            SM_关闭所有面板,
            SM_关闭所有弹窗,
        }

        public PanelID panelId;
        public PanelType panelType;
        public string panelTitle;
        public UIColliderMode colliderMode;
        public int HidePanelMask;

        public bool needBG = false;

        public bool needTopBar = false;

        public List<int> mLstPlayerProprty = new List<int>();

       // public string rightTabRootPath = string.Empty;

        public bool NeedTabs {get;set;}
        public int TabsNum {get;set;}
        /// <summary>
        /// 代码枚举
        /// </summary>
        public List<string> mLstTabs = new List<string>(4);
        /// <summary>
        /// 页签名字
        /// </summary>
        public List<string> mLstTabsName = new List<string>(4);
    }
}
public class UIDefine
{
    #region Define
    public const string NONE_ICON_NAME = "none_icion_name";
    public const string ICON_CIRCLE_SUFFIX = "_y";
    //背景亮图片名
    public const string GRID_BG_HIGHTLIGHT = "cm_btn_liang";
    //背景暗图片名
    public const string GRID_BG_DARK = "cm_btn_an";
    //背景暗图片名
    public const string PANEL_MASK_BG = "biankuang_quanhei";
    //小锁
    public const string SMAMLL_LOCK = "icon_smalllock";
    //ui debug log enable
    public const bool UI_DEBUG_LOG_ENABLE = true;
    /// <summary>
    /// 物品tips类型
    /// </summary>
    public enum UITipsType
    {
        None = 0,
        //装备
        Equip = 1,
        //药品
        Consumption = 2,
        //材料
        Material = 3,
        //圣魂
        Muhon = 4,
        //坐骑
        Mounts = 5,
        //技能
        Skill = 6,
        //战魂坐骑蛋
        MuhonMountsEgg = 7,
        //任务道具
        MissionItem = 8,
    }

    /// <summary>
    /// 游戏对象移动状态（针对非界面animation，其中的游戏对象可见状态）
    /// </summary>
    public enum GameObjMoveStatus
    {
        None = 0,
        MoveToVisible = 1,      //开始移动，下一个状态可见
        Visible = 2,            //可见
        MoveToInvisible = 3,    //开始移动，下一个状态不可见
        Invisible = 4,          //不可见
    }

    /// <summary>
    /// 游戏对象移动状态数据
    /// </summary>
    public class GameObjMoveData
    {
        public GameObjMoveStatus Status = GameObjMoveStatus.None;
        public List<GameObject> Objs = null;
    }

    /// <summary>
    /// UI全局消息类型
    /// </summary>
    public enum UIGlobalEventType
    {
        Unknow = 0,
    }

    public delegate bool UIBoolDelegate();
    //面板遮罩模式
    public enum UIPanelColliderMode
    {
        None = 0,           //面板显示不包含碰撞背景
        Normal = 1,         //带透明背景
        TransBg = 2,        //带半透明背景
    }

    /// <summary>
    /// tips panel data
    /// </summary>
    public class TipsPanelData
    {
        //是否在在背包中显示tips
        public bool m_bool_needCompare = false;
        //显示数据
        public BaseItem m_data;
        //目标UI
        public GameObject m_obj_targetUIGameObj;
        //其他玩家 宝石
        public Dictionary<uint, uint> m_dicGem;
        public int m_nPlayerLevel = -1;
    }

    /// <summary>
    /// UI顶部栏UI模式
    /// </summary>
    public enum UITopBarUIMode
    {
        //无顶部栏
        None = 0,
        //名称
        Normal = 1,
        //货币栏
        CurrencyBar = 2,
        //帮助按钮
        Help = 4,
        //声望
        ClanSW = 8,
    }
    
    /// <summary>
    /// panel在树形结构中的层级深度
    /// </summary>
    public class UIPanelHierarchyData
    {
        //面板
        public UIPanel panel;
        //树形结构深度
        public int hierachyDepth;
    }

    /// <summary>
    /// 资源名称路径
    /// </summary>
    public class UIResPathName
    {
        public string path;
        public string name;
    }

    #endregion

    #region static Data

    #endregion

    #region StaticMethod

    /// <summary>
    /// 根据资源id获取资源类型
    /// </summary>
    /// <param name="resEnum"></param>
    /// <returns></returns>
    public static UIResourceType GetResourceTypeByResEnum(Enum resEnum)
    {
        UIResourceType resType = UIResourceType.None;
        if (resEnum is GridID)
        {
            resType = UIResourceType.Prefab;
        }else if (resEnum is PanelID)
        {
            resType = UIResourceType.Panel;
        }else if (resEnum is AtlasID)
        {
            resType = UIResourceType.Atlas;
        }else if (resEnum is TextureID)
        {
            resType = UIResourceType.Texture;
        }else if (resEnum is FontID)
        {
            resType = UIResourceType.Font;
        }
        return resType;
    }

    /// <summary>
    /// 是否开启UI调试log
    /// </summary>
    /// <returns></returns>
    public static bool IsUIDebugLogEnable
    {
        get
        {
            if (Application.isEditor)
            {
                return false;
            }
            else
            {
                return UI_DEBUG_LOG_ENABLE;
            }
        }
    }

    #endregion
}