using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using table;

public class CMResourceDefine
{
    #region define
    //UI资源预加载回调
    public delegate void OnPreloadUIResDlg(float progress,object param = null);

    /// <summary>
    /// 资源保存类型
    /// </summary>
    public enum ResHoldType
    {
        ByTime = 1,     ///<< 根据时间是否超时来维持资源
        ForEver = 2,    ///<< 永远不释放,除非用户主动释放
        Scene = 3,      ///<< 场景切换时释放
    }


    public class LocalResourceData
    {
        //资源ID
        private uint m_resID = 0;
        public uint ResID
        {
            get
            {
                return m_resID;
            }
        }

        //表格数据
        public table.UIResourceDataBase TabData
        {
            get
            {
                return (m_resID != 0) ? GameTableManager.Instance.GetTableItem<table.UIResourceDataBase>(m_resID) : null;
            }
        }

        //资源名称（不包含后缀）
        public string ResName
        {
            get
            {
                return (null != TabData) ? TabData.resName : string.Empty;
            }
        }
        //资源类型
        public UIResourceType ResType
        {
            get
            {
                return (null != TabData) ? (UIResourceType)TabData.resType : UIResourceType.None;
            }
        }
        //资源相对路径(相对ui/)
        public string ResRelativePath
        {
            get
            {
                return (null != TabData) ? TabData.resRelativePath.ToLower() : string.Empty;
            }
        }

        //预加载数量
        public uint PreLoadNum
        {
            get
            {
                return (null != TabData) ? TabData.resPreloadNum : 0;
            }
        }

        //资源保存类型
        public CMResourceDefine.ResHoldType HoldType
        {
            get
            {
                return (null != TabData && TabData.resKeepType != 0) ? (CMResourceDefine.ResHoldType)TabData.resKeepType
                    : CMResourceDefine.ResHoldType.ByTime;
            }
        }

        //不活跃克隆对象维持数 
        public uint InactiveKeepNum
        {
            get
            {
                //return (null != TabData) ? TabData.inactiveStayMaxNum : 0;
                return 0;
            }
        }

        //最大克隆数量
        public uint MaxCloneNum
        {
            get
            {
                //return (null != TabData) ? TabData.maxCloneNum : 0;
                return 0;
            }
        }

        private LocalResourceData(uint resID)
        {
            m_resID = resID;
        }

        public static LocalResourceData Create(uint resId)
        {
            LocalResourceData resData = new LocalResourceData(resId);
            return resData;
        }
    }
    #endregion
    //资源回收检测时间
    public const float RESOUCE_RECYCLE_GAP = 30f;
    //AssetBundle是否可用
    public const bool EDITOR_ASSET_BUNDLE_ENABLE = false;


    /// <summary>
    /// 加载资源类型
    /// </summary>
    public enum AssetType
    {
        Normal = 1,
        AssetBundle = 2,
    }

    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResType
    {
        None = 0,
        UI_panel = 1,             ///<< UI
        UI_grid = 2,              ///<< Grid                  
        UI_font = 3,              ///<< font    
        UI_atlas = 4,              ///<< atlas  
        UI_sky  = 5,               ///<< 天空
        UI_textures = 6,           ///<< 贴图                      

        Prefab_character = 10,    ///<< 角色
        Prefab_npc = 11,          ///<< NPC
        Prefab_effect = 12,       ///<< 特效
        Prefab_item = 13,        ///<< 道具
        Prefab_surface = 14,    ///<< 地表
        Prefab_monstrer = 15,    ///<<怪物
        Prefab_scene = 16,       ///<< 场景
        Material = 20,           ///<< 材质
               
        Table_config = 30,          ///<< 数据表格
        Table_local = 31,           ///<< 中英文      
        Table_data = 32,          ///<< 数据表格
        Table_battle_data = 33,          ///<< 数据表格

        Audio = 40,                 ///<< 音效                 
        Scene = 41,                 ///<< 场景
                                    ///
        Prefab_animator = 42,    ///<< 角色
                                 ///
        Prefab_animator_override = 43,///<<角色控制器
    }

    /// <summary>
    /// 是否为预制
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsResPreafb(ResType type)
    {
        return (type >= ResType.Prefab_character && type <= ResType.Prefab_scene) ? true : false;
    }

    

    /// <summary>
    /// 资源目录
    /// </summary>
    public static Dictionary<ResType, string> ResTypeDir = new Dictionary<ResType, string>()
    {
        {ResType.UI_panel,"UI/Layout/Panel/"},
        {ResType.UI_grid,"UI/Layout/Grid/"},
        {ResType.UI_atlas,"UI/Atlas/"},
        {ResType.UI_font,"UI/Font/"},

        {ResType.Prefab_character,"Prefab/Character/"},
        {ResType.Prefab_npc,"Prefab/NPC/"},
        {ResType.Prefab_effect,"Prefab/Effect/"},
        {ResType.Prefab_item,"Prefab/Item/"},
        {ResType.Prefab_monstrer,"Prefab/Monster/"},
        {ResType.Prefab_surface,"Prefab/Surface/"},
        {ResType.Prefab_scene,"Prefab/Scene/"},

        {ResType.Material,"Material/"},

        {ResType.Table_config,"Data/Config/"},
        {ResType.Table_local,"Data/Localize/"},
        {ResType.Table_data,"Data/Raw/"},
        {ResType.Table_battle_data,"Data/Battle/"},

        {ResType.Audio,"Audio/"},
        {ResType.Prefab_animator,"Animators/"},
        {ResType.Prefab_animator_override,"Animators/"},
    };

    

    

    
    
    /// <summary>
    /// 根据资源类型获取文件后缀
    /// </summary>
    /// <param name="dataType"></param>
    /// <returns></returns>
    public static string GetSuffixWithDataType(ResType resType)
    {
        string suffix = "unity3d";

#if UNITY_EDITOR
        if (EDITOR_ASSET_BUNDLE_ENABLE)
        {
            if (resType == ResType.Table_config || resType == ResType.Table_local)
                suffix = "csv";
            else if (resType == ResType.Table_data || resType == ResType.Table_battle_data)
                suffix = "data";            
            else
                suffix = "unity3d";
        }
        else
        {
            switch (resType)
            {

                case ResType.Table_config:
                case ResType.Table_local:
                    suffix = "csv";
                    break;
                case ResType.Table_data:
                case ResType.Table_battle_data:
                    suffix = "data";
                    break;
                case ResType.Prefab_npc:
                case ResType.Prefab_monstrer:
                case ResType.Prefab_character:
                case ResType.Prefab_item:
                case ResType.Prefab_surface:
                case ResType.Prefab_scene:
                case ResType.Prefab_effect:
                case ResType.UI_panel:
                case ResType.UI_atlas:
                case ResType.UI_font:
                case ResType.UI_grid:
                    suffix = "prefab";
                    break;
                case ResType.UI_textures:
                case ResType.UI_sky:
                    break;
                case ResType.Material:
                    suffix = "mat";
                    break;
                case ResType.Audio:
                    suffix = "mp3";
                    break;
                case ResType.Prefab_animator:
                    suffix = "controller";
                    break;
                case ResType.Prefab_animator_override:
                    suffix = "overrideController";
                    break;               
            }
        }
#else
            if (resType == ResType.Table_config || resType == ResType.Table_local)
                suffix = "csv";
            else if (resType == ResType.Table_data || resType == ResType.Table_battle_data)
                suffix = "data";
            else
                suffix = "unity3d";
#endif

        return suffix;
    }

    /// <summary>
    /// 构建URL
    /// </summary>
    /// <param name="resName"></param>
    /// <param name="resType"></param>
    /// <returns></returns>
    public static string BuildUrl(string resName, ResType resType)
    {
        return PathDefine.WWW_Path(BuildPath(resName, resType));
    }

    /// <summary>
    /// 构建路径
    /// </summary>
    /// <param name="resName"></param>
    /// <param name="resType"></param>
    /// <returns></returns>
    public static string BuildPath(string resName, ResType resType)
    {
        string path = (ResTypeDir.ContainsKey(resType) ? (ResTypeDir[resType] + resName + "." + GetSuffixWithDataType(resType)) : "");
        return (PathDefine.ResPath +  path.ToLower());
    }

    public static string BuildConfigPath(string resName, ResType resType)
    {
        string path = (ResTypeDir.ContainsKey(resType) ? (ResTypeDir[resType] + resName + "." + GetSuffixWithDataType(resType)) : "");
#if UNITY_EDITOR
        path = Application.dataPath + "/res/" + path;
#else
        path = PathDefine.ResPath + path;
#endif
        return path;
        
    }
}

public class PathDefine
{
    //EditorRespath
    public const string EDITOR_RES_PATH = "../EditorData/";
    //NEW WORLD RES PATH DIR
    public const string NEWWORLD_RES_DIR = "newworld/res/";
    //Editor Res Path
    public const string EDITOR_RUN_RES_PATH = "Assets/Res/";
    //资源路径
    public static string ResPath
    {
        get
        {
#if UNITY_EDITOR
            string path = "";
            if (ResourceDefine.EDITOR_ASSET_BUNDLE_ENABLE)
            {
                string platformName = AssetBundleLoaderMgr.GetRuntimePlatformName();
                path = Path.GetFullPath(EDITOR_RES_PATH + "AssetBundle/" + platformName + "/").Replace("\\", "/");
            }
            else
            {
                path = EDITOR_RUN_RES_PATH;
            }
            return path;

#elif UNITY_ANDROID || UNITY_IPHONE
            return Application.persistentDataPath + "/res/";
#endif
            return "";
        }
    }

    /// <summary>
    /// 构建本地WWW文件路径
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string WWW_Local_File_Path(string url)
    {
        if (url.StartsWith("file://"))
            return url;
        return "file://" + url;
    }

    /// <summary>
    /// 构建www加载Url
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string WWW_Path(string url)
    {
        string ret = url;
        if (File.Exists(url))
        {
            ret = WWW_Local_File_Path(url);
        }
        else
        {
            //Log.Warning("WWW_Path( " + ret + " )");
        }
            
        return ret;
    }

    /// <summary>
    ///StreamingAssetsPath
    /// </summary>
    public static string StreamingAssetsPath
    {
        get
        {
            return Application.streamingAssetsPath;
        }
    }

    /// <summary>
    /// PersistentDataPath
    /// </summary>
    public static string PersistentDataPath
    {
        get
        {
            return Application.persistentDataPath;
        }
    }
}
