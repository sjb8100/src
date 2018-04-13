using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Client
{
    public class EntityConst
    {
        public const float MOVE_SPEED_RATE = 0.01f;     // 服务器大格子坐标和小坐标转换比率
    }

#region Entity Type
    public enum EntityType
    {
        EntityType_Null = 0,
        EntityType_Player = 1,      // 玩家
        EntityType_Monster,         // 怪物
        EntityType_NPC,             // NPC
        EntityType_Item,            // 物品
        EntityType_Pet,             // 宠物
        EntityType_Box,             // 盒子
        EntityTYpe_Robot,           // 机器人

        // 家园
        EntityType_Plant,           // 植物
        EntityType_Animal,          // 动物
        EntityType_Tree,            // 许愿树
        EntityType_Minerals,        // 矿产
        EntityType_Soil,            // 土地
        EntityType_Puppet,          // 木偶(玩家)
       
        EntityType_Max,
    }

    public enum EquipPos
    {
        EquipPos_Null = 0,          // 无效值
        EquipPos_Body = 1,          // 身体
        EquipPos_Weapon,            // 武器
        EquipPos_Back,              // 背饰
        EquipPos_Face,              // 脸饰
        EquipPos_Wing,              // 翅膀

        EquipPos_Max,
    }

    // 实体部件
    public enum EntityPart
    {
        Skill = 1,      // 技能部件
        Buff = 2,       // buff
        Equip = 3,      // 装备栏部件 存放玩家穿戴装备数据
    }

    // 实体属性
    public class EntityAttr
    {
        public int nPropIndex = 0;
        public int value = 0;

        public EntityAttr(int index, int v)
        {
            nPropIndex = index;
            value = v;
        }
    }
    // 实体外观属性（玩家使用）
    public class EntityViewProp
    {
        public int nPos = 0;
        public int value = 0;
        public EntityViewProp(int pos, int v)
        {
            nPos = pos;
            value = v;
        }
    }
    /// <summary>
    /// 实体创建数据
    /// </summary>
    public class EntityCreateData
    {
        public uint ID;         // 运行时ID
        public string strName;  // 对象名称
        public Vector2 pos;     // 位置属性
        public float fDir;      // 方向
        public bool bImmediate; // 是否立即创建
        public bool bMainPlayer = false; // 是否是主角
        public bool bViewModel = false;  // 是否是观察模型   
        public SkillSettingState eSkillState = SkillSettingState.StateOne; // 技能形态
        public int nLayer = LayerMask.NameToLayer("RenderObj");      // 对象层标识
        public EntityAttr[] PropList;  // 属性列表
        public EntityViewProp[] ViewList;  // 外观列表
    }

#endregion

    public enum ColliderCheckType
    {
        ColliderCheckType_null = 0,
        ColliderCheckType_CloseTerrain = 1, // 贴地碰撞
        ColliderCheckType_Wall = 2,         // 与地形碰撞（主要是空气墙和地表物件）
        ColliderCheckType_DynamicObj = 4,   // 动态对象 (角色对象去检测与怪物以及NPC等的碰撞 怪物对象不主动做碰撞检测)
    }

    public enum ColliderObjType
    {
        ColliderCheckType_null = 0,
        ColliderCheckType_Terrain = 1,      // 地表
        ColliderCheckType_Wall = 2,         // 空气墙
        ColliderCheckType_TerrainObj = 3,   // 地表物件
        ColliderCheckType_DynamicObj = 4,   // 动态对象
    }

    // 实体状态
    public enum CreatureState
    {
        Null = 0,
        Normal = 1, // 正常 站立
        Move,       // 移动
        Idle,   // 休闲
        BeginDead,
        Dead,   // 死亡
        Contrl,//控制
      //  Fight,  // 战斗 待定
    }

    // 实体动画名称枚举定义
    public class EntityAction
    {
        public const string Run = "Run";
        public const string Run_Combat = "Run_Combat";
        public const string Stand = "Stand";
        public const string Stand_Combat = "Stand_Combat";
        public const string Idle = "Idle";
        public const string Dead = "Die";
        public const string Vertigo = "Vertigo";
        public const string Collect = "CaiJi";
        public const string Show = "Show";
        public const string ShowLog = "Showlog";
        public const string Mining = "Mining";
        public const string FishingStart = "Angling_Start";
        public const string Fishing = "Angling";
        public const string FishingEnd = "Angling_End";
    }

    // 实体组件
    public enum EntityCommpent
    {
        EntityCommpent_Null = 0,
        EntityCommpent_Visual = 1,      // 外观
        EntityCommpent_Move = 2,        // 移动

        EntityCommpent_Max,             // 组件最大值
    }

    //// 实体消息
    public enum EntityMessage
    {
        // 外观组件
        EntityCommand_SetPos = 1,       // 设置实体位置 参数Vector3 pos
        EntityCommand_SetRotateX,       // 绕X旋转直接到指定角度 float 
        EntityCommand_SetRotateY,       // 绕Y旋转直接到指定角度 float 
        EntityCommand_SetRotate,        // 设置旋转 (欧拉角) Vector3(x,y,z)
        EntityCommand_GetRotateY,       // 获取绕Y旋转的角度
        EntityCommand_PlayAni,          // 播放动画 动画名称 playAni
        EntityCommand_PauseAni,         // 停止播放动画;
        EntityCommand_ResumeAni,        // 继续播放动画;
        EntityCommand_GetCurAni,        // 获取当前动画名称
        EntityCommand_ClearAniCallback, // 清理动画播放回调
        EntityCommand_SetAniSpeed,      // 设置动画播放速度

        EntityCommand_ChangePart,       // 更换部件 部件名称(支持多部件换装，会做Mesh合并) ChangePart
        EntityCommand_EnableShadow,     // 阴影开关 bool

        EntityCommand_AddLinkObj,       // 添加挂件 AddLinkObj 返回linkID
        EntityCommand_RemoveLinkObj,    // 删除挂件 linkID
        EntityCommand_RemoveAllLinkObj, // 删除所有挂件
        EntityCommand_AddLinkEffect,    // 添加挂件 AddLinkEffect 返回linkID
        EntityCommand_RemoveLinkEffect, // 删除挂件 linkID
        EntityCommand_RemoveLinkAllEffect, // 删除所有特效
        EntityCommand_SetVisible,       // 设置可见性
        EntityCommand_IsVisible,        // 是否可见
        EntityCommand_EnableShowModel,  // 显示模型开关 bool
        EntityCommand_IsShowModel,      // 模型显示开关
        EntityCommond_SetAlpha ,        // 隐身标志 stEntityHide
        EntityCommond_SetColor,         // 设置颜色 Color
        EntityCommand_FlashColor,       // 设置闪烁效果 FlashColor
        EntityCommand_ChangeMaterial,   // 更换材质 strShaderName
        EntityCommond_SetName,          // 设置名称 strName
        EntityCommond_ChangeNameColor,  // 修改名称颜色 color
        EntityCommand_Change,           // 变身 资源ID
        EntityCommand_Restore,          // 还原变身 无参数
        EntityCommand_IsChange,         // 是否在变身状态

        EntityCommand_ChangeEquip,      // 挂接式换装 没有mesh合并  ChangeEquip

        EntityCommand_AddText,          // 添加文字 AddText 返回linkID
        EntityCommand_RemoveText,       // 删除文字
        EntityCommand_ModifyText,       // 修改文字
        EntityCommand_ModifyColor,      // 修改文字颜色

        EntityCommand_AddHPBar,         // 血条 AddHPBar 返回linkID
        EntityCommand_RemoveHPBar,      // 删除血条
        EntityCommand_ModifyHP,         // 更新血量 ModifyHP

        EntityCommond_Ride,             // 上马 坐骑ID(int)
        EntityCommond_UnRide,           // 下马
        EntityCommond_IsRide,           // 查询坐骑状态

        EntityCommond_FlyFont,          // 飘字
        EntityCommond_FlyFontEnd,       // 飘字结束

        EntityCommand_LookAt,           // 朝向目标
        EntityCommand_RotateY,          // 绕Y旋转 在当前方向上再旋转指定角度
        EntityCommand_LookTarget,       // 朝向目标，目标y值拉回0

        // 运动组件
        EntityCommand_Move,             // 移动   相对坐标
        EntityCommand_MoveTo,           // 移动到 绝对坐标
        EntityCommand_MovePath,         // 移动 按路径移动
        EntityCommand_MoveDir,          // 移动 按指定方向
        EntityCommand_IsMove,           // 判断是否移动
        EntityCommand_StopMove,         // 停止移动 位置
        EntityCommand_ForceStopMove,    // 强制停止移动(处理主角) 位置
        EntityCommand_ServerTime,       // 获取服务器时间
        //EntityCommand_Jump,           // 跳跃
        //EntityCommand_Fly,            // 飞行
        EntityCommand_ChangeMoveSpeedFact, //改变移动速度因子(用于效果处理)
        EntityCommand_GetMoveSpeedFact,    //获取移动速度因子
        EntityCommond_ChangeMoveActionName,//更改移动动作名称 动作名称
        EntityCommond_IgnoreMoveAction,    //忽略移动动作 bool 忽略默认的移动动作
        //EntityCommond_NotSynsMessage,      //网络不同步移动消息

    }

    public class PlayAni
    {
        public PlayAni()
        {
            strAcionName = "";
            nStartFrame = 0;
            fSpeed = 1.0f;
            fBlendTime = -1.0f;
            nLoop = -1;
            aniCallback = null;
            callbackParam = null;
        }
        //动画名称...
        public string strAcionName;
        //开始帧数 ....
        public int nStartFrame ;
        //速度比率 1 normal
        public float fSpeed ;
        //混合时间 0.15f
        public float fBlendTime;
        //循环次数;.
        public int nLoop;

        // 动画播放回调
        public Engine.ActionEvent aniCallback;
        public object callbackParam;
    }
    public class ChangePart
    {
        public string strPartName = "";
        public string strResName = "";
    }

    /// EntityCommand_ChangeEquip
    // 换装参数
    public class ChangeEquip
    {
        public EquipPos pos = EquipPos.EquipPos_Body;           // 装位置
        public int nSuitID = 0;                                 // 时装ID
        public int nLayer = (int)Engine.RenderLayer.RenderObj;  // 层标识
        public int nStateType = 1;                              // 形态ID
        public Engine.CreateRenderObjEvent evt = null;          // 回调事件
        public object param = null;                             // 回调参数
    }

    public class FlashColor
    {
        public Color color = Color.white;  // 颜色
        public float fLift = 0.2f;         // 时长
    }

    // 添加文字
    public class AddText
    {
        public string strText = "";             // 文字内容
        public Color color = Color.white;       // 颜色
        public string strFontName = "Arial";    // 字体名称
        public int nFontSize = 25;              // 字体大小
        public float fCharSize = 0.15f;         // 字符大小
        public string strLocatorName = "Name";  // 绑定点名称
        public Vector3 offset = Vector3.zero;   // 偏移
    }
    // 修改文字内容
    public class ModifyText
    {
        public int nLinkID = 0;                 // 挂接ID
        public string strText = "";             // 文字内容
    }
    // 修改颜色
    public class ModifyColor
    {
        public int nLinkID = 0;                 // 挂接ID
        public Color color = Color.white;       // 颜色
    }

    // 添加血条
    public class AddHPBar
    {
        public float fProgress = 1.0f;          // 满血
        public string strLocatorName = "Name";  // 绑定点名称
        public Vector3 offset = Vector3.zero;   // 偏移
    }
    public class ModifyHP
    {
        public float fProgress = 1.0f;          // 血量百分比
    }

    public class AddLinkObj
    {
        public string strRenderObj;     // 对象
        public string strLinkName;      // 绑定点名称
        public Vector3 vOffset;         // 偏移量
        public Vector3 rotate;          // 欧拉角
        public int nFollowType;         // 跟随类型
        /// 自定义贴图（支持称号）
        public string strNodeName = "";     // 节点名称
        public string strTextureName = "";  // 贴图名称
    }

    public class AddLinkEffect
    {
        public string strEffectName;
        public string strLinkName;
        public Vector3 vOffset; // 偏移量
        public Vector3 rotate;  // 欧拉角
        public Vector3 scale = Vector3.one; //缩放
        public int nFollowType;
        public int nLevel = 0;  // 0 高 1中 2低
        public Engine.EffectCallback callback = null;   // 回调
        public object param = null;       // 回调参数
        public bool bIgnoreRide = true;    // 忽略坐骑状态
    }

    public class LookAt
    {
        public Vector3 vTarget; // 目标点
        public Vector3 vUp;     // 向上的向量
    }

    public class Move
    {
        public string strRunAct;
        public float m_dir;         // 方向参数 按照指定方向移动
        public Vector3 m_target;    // 如果move 则视为偏移 moveto 视为目标点
        public float m_speed;       // 移动速度;
        public bool m_ignoreStand;    //忽略stand动作

        public List<Vector3> path;  // 按照路径移动 绝对坐标
        ///////////////////////////////////////////////
        // 以下参数可以忽略
        ////加速度默认0;
        //public float m_AcceleratedSpeed;
        ////速度倍率默认1;
        //public bool ZeroSpeedStop;
        ////是否使用移动点 1200默认true;
        //public bool m_useMovePoint;
        // 自定义参数
        public object param;
        //public Move()
        //{
        //    strRunAct = "";
        //    m_dir = 0.0f;
        //    m_target = Vector3.zero;
        //    m_speed = 0.0f;
        //    m_bAutoStop = true;
        //    path = null;
        //    param = null;
        //}
    }
    //public struct Jump
    //{
    //    //向前方向的力; 负为受击力;
    //    public float forwardPow;
    //    //向前加速度;
    //    public float acceleratedForwardPow;
    //    //向上方向的力 
    //    public float upPow;
    //    //向上加速度... 负为重力;
    //    public float acceleratedupPow;
    //    //向前的力趋向0时取消加速度;
    //    public bool ZeroForwardStop;
    //    //向上的力趋向0时取消加速度;
    //    public bool ZeroUpStop;
    //}
    // 实体碰撞消息
    public class EntityColliderInfo
    {
        public ColliderObjType cct;   // 碰撞对象类型
        public int nColliderObjID;    // 碰撞对象ID
        public float fDis;            // 碰撞点和起点的距离
        public Vector3 pos;
        public Vector3 normal;
    }

    public class ChangeBody
    {
        public int resId;
        public Action<object> callback;
        public object param;
        public float modleScale;
    }
}
