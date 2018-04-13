/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Guide
 * 创建人：  wenjunhua.zqgame
 * 文件名：  GuideDefine
 * 版本号：  V1.0.0.0
 * 创建时间：2/27/2017 10:14:13 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
class GuideDefine
{
    #region define

    /// <summary>
    /// 引导循环触发类型
    /// </summary>
    public enum GuideLoopTriggerType
    {
        GLTT_UnLoop = 0,            //非循环触发引导
        GLTT_Loop = 1,              //循环引导触发
    }

    public enum GuideEffectMoveType
    {
        None = 0,
        GEMT_Circle = 1,
        GEMT_
    }

    /// <summary>
    /// 开启功能类型
    /// </summary>
    public enum FuncOpenType
    {
        Base = 1,           //基础功能
        Skill = 2,        //技能
    }

    /// <summary>
    /// 引导GUI控件从属类型
    /// </summary>
    public enum GuideGUIDependType
    {
        None = 0,           
        Knapsack = 1,       //背包
        Mall = 2,           //商城
        Daily = 3,
    }
    
    /// <summary>
    /// 引导类型
    /// </summary>
    public enum GuideType
    {
        None = 0,
        Constraint = 1,     //强制
        Unconstrain = 2,    //非强制
        Max,
    }

    /// <summary>
    /// 引导工作流类型
    /// </summary>
    public enum GuideWorkFlowType
    {
        None = 0,
        NewFuncOpen = 1,    //新功能开启   
        Guide,              //引导
    }

    /// <summary>
    /// 引导工作流数据
    /// </summary>
    public class GuideWorkFlowData
    {
        //工作流类型
        public GuideWorkFlowType FlowType = GuideWorkFlowType.None;
        //工作流数据
        public object Data;
    }

    /// <summary>
    /// 引导数据完成性检测
    /// </summary>
    public enum GuideCheckDataType
    {
        None = 0,
        ID = 1,
        Type = 2,
    }

    /// <summary>
    /// 引导样式
    /// </summary>
    public enum GuideStyle
    {
        Center = 1,     //引导样式中
        LeftUp = 2,     //箭头左上
        Left = 3,       //箭头左
        LeftDown = 4,   //箭头左下
        RightUp = 5,    //箭头右上
        Right = 6,      //箭头右
        RightDown = 7,  //箭头右下
        Up = 8,         //箭头向上
        Down = 9,       //箭头向下
    }

    /// <summary>
    /// 引导触发事件类型
    /// </summary>
    public enum GuideTriggerEventType
    {
        None = 0,       //不触发
        Trigger = 1,    //触发
    }


    /// <summary>
    /// 引导触发类型
    /// </summary>
    public enum GuideTriggerType
    {
        Invalide = 0,
        Level = 1,          //等级
        Task = 2,           //任务
        Condition = 3,      //条件
        Always = 4,         //任何时候都触发
        ChapterEnd = 5,     //章节完成
        ItemGet = 6,        //获得物品
    }

    public struct RecycleTriggerGuidePassData
    {
        public uint TaskID;
    }

    /// <summary>
    /// 任务触发子类型
    /// </summary>
    public enum TaskSubTriggerType
    {
        Invalid = 0,            //无效
        CanAccept = 1,          //可接受
        Doing= 2,               //进行中
        CanSubmit = 3,          //可提交
        Complete = 4,           //完成
    }

    /// <summary>
    /// 条件触发子类型
    /// </summary>
    public enum ConditonSubTriggerType
    {
        CST_None = 0,
        CST_FOPanel = 1,                //第一次打开面板
        CST_FGGem1_3 = 2,               //第一次获得任意1-3级生命宝石
        CST_FGTitle = 3,                //第一个称号获取
        CST_FGSkillBook = 4,            //第一次获得宠物技能书
        CST_FGActiveReward = 5,         //第一次有活跃奖励可领取
        CST_FGMuhon0 = 6,               //第一次获得任意种类0星圣魂
        CST_FGJoinClan = 7,             //加入氏族

        //特殊子触发条件，关闭功能
        CST_7TianClose = 20,                 //七天福利是否完成
        CST_ShenBinClose = 21,              //神兵关闭
        CST_FirstRechargeRewardClose =22,          //首充奖励关闭
        CST_OpenServerGiftClose = 23,          //开服豪礼关闭
        CST_RechargeRewRetClose = 24,           //内测返利关闭
        CST_QuestionClose = 25,                 //问卷功能关闭

    }

    public class GuideItemFocusData
    {
        public GuideGUIDependType DependType = GuideGUIDependType.None;
        public object Data;
    }

    /// <summary>
    /// 引导对象路径数据
    /// </summary>
    public class GuideObjPathData
    {
        /// <summary>
        /// 依赖面板
        /// </summary>
        public PanelID DependPanelId = PanelID.None;
        //相对于面板路径
        public string RelativePanelPath = "";
        //父节点相对面板路径
        public string ParentPath = "";
    }

    /// <summary>
    /// 功能开启数据
    /// </summary>
    public class FuncOpenShowData
    {
        //功能开启类型
        public FuncOpenType FOT;
        //功能开启ID
        public uint FuncOpenId = 0;
        //功能开启名称
        public string Name = "";
        //功能开启描述
        public string Des = "";
        //功能开启图标
        public string Icon = "";
        //功能title
        public string Title
        {
            get
            {
                string title = "";
                if (FOT == FuncOpenType.Base)
                {
                    title = "开启功能";
                }else
                {
                    title = "开启技能";
                }
                return title;
            }
        }

        //功能开启目标对象
        public UnityEngine.GameObject TargetObj = null;
        //传递参数
        public object Data = null;
        
    }

    /// <summary>
    /// 新功能开启本地数据类
    /// </summary>
    public class LocalFuncOpenData
    {
        #region property
        private uint m_uint_FuncOpenId = 0;
        public uint FuncOpenId
        {
            get
            {
                return m_uint_FuncOpenId;
            }
        }
        private table.NewFUNCOpenDataBase m_tableData = null;
        /// <summary>
        /// 新功能开启表格数据
        /// </summary>
        public table.NewFUNCOpenDataBase TableData
        {
            get
            {
                if (null == m_tableData && FuncOpenId != 0)
                {
                    m_tableData = GameTableManager.Instance.GetTableItem<table.NewFUNCOpenDataBase>(FuncOpenId);
                }
                return m_tableData;
            }
        }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string Name
        {
            get
            {
                return (null != TableData) ? TableData.funcName : "";
            }
        }

        /// <summary>
        /// 功能描述
        /// </summary>
        public string Des
        {
            get
            {
                return (null != TableData) ? TableData.funcDes : "";
            }
        }

        /// <summary>
        /// 功能图标
        /// </summary>
        public string Icon
        {
            get
            {
                return (null != TableData) ? TableData.funcIcon : "";
            }
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public uint Priority
        {
            get
            {
                return (null != TableData) ? TableData.openFuncID : 0;
            }
        }

        /// <summary>
        /// 忽略该提示等级
        /// </summary>
        public uint IgnoreCondi
        {
            get
            {
                return (null != TableData) ? TableData.ignoreCondi : 0;
            }
        }

        /// <summary>
        /// 条件触发ID
        /// </summary>
        public uint TriggerId
        {
            get
            {
                return (null != TableData) ? TableData.triggerCondiID : 0;
            }
        }

        /// <summary>
        /// 关闭触发条件
        /// </summary>
        public uint CloseTriggerId
        {
            get
            {
                return (null != TableData) ? TableData.fnCloseConID : 0;
            }
        }

        /// <summary>
        /// 功能按钮飞向目标依赖面板
        /// </summary>
        public PanelID FlyToTargetDependPanel
        {
            get
            {
                return FlyToObjPathData.DependPanelId;
            }
        }

        private GuideObjPathData m_flyToObjPathData = null;
        /// <summary>
        /// 
        /// </summary>
        public GuideObjPathData FlyToObjPathData
        {
            get
            {
                if (null == m_flyToObjPathData)
                {
                    table.NewFUNCOpenDataBase data = TableData;
                    if (null != data && !string.IsNullOrEmpty(data.funcObjFlyToTargetPath))
                    {
                        m_flyToObjPathData = GuideDefine.ParseGuideObjPathString2PathData(data.funcObjFlyToTargetPath);
                    } else
                    {
                        m_flyToObjPathData = new GuideObjPathData();
                    }
                }
                return m_flyToObjPathData;
            }
        }

        private GameObject m_obj_flyToObj = null;
        /// <summary>
        /// 飞向目标对象
        /// </summary>
        public GameObject FlyToObj
        {
            get
            {
                if (null == m_obj_flyToObj && null != FlyToObjPathData)
                {
                    m_obj_flyToObj =
                        UIManager.FindGameObjByPanel(FlyToObjPathData.DependPanelId, FlyToObjPathData.RelativePanelPath);
                }
                return m_obj_flyToObj;
            }
        }

        /// <summary>
        /// 是否默认开启
        /// </summary>
        public bool AutoOpen
        {
            get
            {
                return (null != TableData && TableData.autoOpen == 1);
            }
        }
        private GuideObjPathData m_funcCreateObjPathData = null;
        public GuideObjPathData FuncCreateObjPathData
        {
            get
            {
                if (null == m_funcCreateObjPathData)
                {
                    table.NewFUNCOpenDataBase data = TableData;
                    if (null != data && !string.IsNullOrEmpty(data.funcObjCreatePath))
                    {
                        m_funcCreateObjPathData = GuideDefine.ParseGuideObjPathString2PathData(data.funcObjCreatePath);
                    }else
                    {
                        m_funcCreateObjPathData = new GuideObjPathData();
                    }
                }
                return m_funcCreateObjPathData;
            }
        }


        /// <summary>
        ///创建功能对象与父控件相对位置
        /// </summary>
        public UnityEngine.Vector3 FuncObjCreateOffset
        {
            get
            {
                Vector3 relateivePos = Vector3.zero;
                if (null != TableData && !string.IsNullOrEmpty(TableData.funcObjCreateOffset))
                {
                    string[] offsets = TableData.funcObjCreateOffset.Split(new char[] { '|' });
                    if (null != offsets && offsets.Length >= 2)
                    {
                        float tempfloat = 0;
                        if (float.TryParse(offsets[0],out tempfloat))
                        {
                            relateivePos.x = tempfloat;
                        }
                        if (float.TryParse(offsets[1], out tempfloat))
                        {
                            relateivePos.y = tempfloat;
                        }
                    }
                }
                return relateivePos;
            }
        }


        private GameObject m_obj_createFuncObj = null;
        /// <summary>
        /// 创建目标对象
        /// </summary>
        public GameObject CreateFuncObj
        {
            get
            {
                if (null == m_obj_createFuncObj && null != FuncCreateObjPathData)
                {
                    m_obj_createFuncObj =
                        UIManager.FindGameObjByPanel(FuncCreateObjPathData.DependPanelId, FuncCreateObjPathData.RelativePanelPath);
                }
                return m_obj_createFuncObj;
            }
        }

        /// <summary>
        /// 挂接父节点
        /// </summary>
        public GameObject MountParentObj
        {
            get
            {
                if (null == m_obj_createFuncObj && null != FuncCreateObjPathData)
                {
                      return  UIManager.FindGameObjByPanel(FuncCreateObjPathData.DependPanelId, FuncCreateObjPathData.ParentPath);
                }
                return null; 
            }
        }

        /// <summary>
        /// 功能按钮排序ID
        /// </summary>
        public int SortID
        {
            get
            {
                return (null != TableData) ? (int)TableData.funcSortID : 0;
            }
        }

        public int SortGroup
        {
            get
            {
                return (null != TableData) ? (int)TableData.funcGroup : 0;
            }
        }

        public Vector3 FuncObjGap
        {
            get
            {
                Vector3 gap = Vector3.zero;
                if (null != TableData && !string.IsNullOrEmpty(TableData.funcGapStr))
                {
                    string[] gapStr = TableData.funcGapStr.Split(new char[] { '|' });
                    if (null != gapStr && gapStr.Length == 2)
                    {
                        if (float.TryParse(gapStr[0].Trim(),out gap.x))
                        {
                            if (float.TryParse(gapStr[1].Trim(),out gap.y))
                            {
                                return gap;
                            }
                        }
                    }
                }
                return gap;
            }
        }

        /// <summary>
        /// 是否需要重新排序
        /// </summary>
        public bool NeedSort
        {
            get
            {
                return (SortID != 0);
            }
        }

        /// <summary>
        /// 功能显示数据
        /// </summary>
        public FuncOpenShowData ShowData
        {
            get
            {
                FuncOpenShowData showData = new FuncOpenShowData()
                {
                    FOT = GuideDefine.FuncOpenType.Base,
                    FuncOpenId = FuncOpenId,
                    Name = Name,
                    Des = Des,
                    Icon = Icon,
                    TargetObj = UIManager.FindGameObjByPanel(FlyToObjPathData.DependPanelId, FlyToObjPathData.RelativePanelPath),
                };

                return showData;
            }
        }
        #endregion

        #region static
        private  LocalFuncOpenData(uint id)
        {
            m_uint_FuncOpenId = id;
        }
        public static LocalFuncOpenData Create(uint id)
        {
            return new LocalFuncOpenData(id);
        }

        /// <summary>
        /// 优先级比较（用于排序）
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int CompareFuncOpenPriority(uint left,uint right)
        {
            return (int)left - (int)right;
        }
        
        /// <summary>
        /// 优先级比较（用于排序）
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int CopareFuncOpenPriority(LocalFuncOpenData left,LocalFuncOpenData right)
        {
            return CompareFuncOpenPriority(left.Priority, right.Priority);
        }
        #endregion
    }

    /// <summary>
    /// 引导本地数据类
    /// </summary>
    public class LocalGuideData
    {
        #region strucmethod
        private uint m_uint_id;
        public uint ID
        {
            get
            {
                return m_uint_id;
            }
        }

        private table.GuideDataBase m_tableData = null;
        /// <summary>
        /// 新手引导表格数据
        /// </summary>
        public table.GuideDataBase TableData
        {
            get
            {
                if (ID != 0 && null == m_tableData)
                {
                    m_tableData = GameTableManager.Instance.GetTableItem<table.GuideDataBase>(ID);
                }
                return m_tableData;
            }
        }

        public uint JumpID
        {
            get
            {
                return (null != TableData) ? TableData.jumpID : 0;
            }
        }

        /// <summary>
        /// 引导描述内容
        /// </summary>
        public string Des
        {
            get
            {
                return (null != TableData) ? TableData.guideDes : "";
            }
        }

        /// <summary>
        /// 引导类型
        /// </summary>
        public GuideType GType
        {
            get
            {
                GuideType gType = GuideType.None;
                if (null != TableData)
                {
                    gType = (GuideType)TableData.guideType;
                }
                return gType;
            }
        }

        /// <summary>
        /// 是否为循环触发
        /// </summary>
        public bool LoopTrigger
        {
            get
            {
                if (null != TableData)
                {
                    return (GType == GuideType.Unconstrain)
                        && (TableData.loopTrigger == (uint)GuideLoopTriggerType.GLTT_Loop);
                }
                return false;
            }
        }

        /// <summary>
        /// 是否实时刷新位置
        /// </summary>
        public bool RefreshPosInTime
        {
            get
            {
                if (null != TableData)
                {
                    return (TableData.refreshPosInTime == 1) ? true : false;
                }
                return false;
            }
        }

        /// <summary>
        /// 引导样式
        /// </summary>
        public GuideStyle GStyle
        {
            get
            {
                GuideStyle gstyle = GuideStyle.LeftUp;
                if (null != TableData)
                {
                    gstyle = (GuideStyle)TableData.guideStyle;
                }
                return gstyle;
            }
        }

        /// <summary>
        /// 引导组ID
        /// </summary>
        public uint GuideGroupID
        {
            get
            {
                return (null != TableData) ? TableData.guideGroupID : 0;
            }
        }

        /// <summary>
        /// 引导组步骤
        /// </summary>
        public int GuideGroupStep
        {
            get
            {
                return (null != TableData) ? (int)TableData.guideStep : 0;
            }
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority
        {
            get
            {
                return (null != TableData) ? (int)TableData.priority : 0;
            }
        }

        /// <summary>
        /// 触发事件类型
        /// </summary>
        public GuideTriggerEventType TriggerEventType
        {
            get
            {
                return (null != TableData) ? (GuideTriggerEventType)TableData.triggerEventType
                    : GuideTriggerEventType.None;
            }
        }

        /// <summary>
        /// 延迟执行时间
        /// </summary>
        public float DelayTime
        {
            get
            {
                float delayTime = 0;
                if (null != TableData)
                {
                    delayTime = TableData.guideDelayTime / 1000f;
                }
                return delayTime;
            }
        }

        /// <summary>
        /// 相对位置
        /// </summary>
        public Vector3 RelativeOffset
        {
            get
            {
                Vector3 offset = Vector3.zero;
                float tempf = 0f;
                if (null != TableData && !string.IsNullOrEmpty(TableData.guideGUIRelativePos))
                {
                    string[] offsets = TableData.guideGUIRelativePos.Split(new char[] { '|' });
                    if (null != offsets && offsets.Length == 0)
                    {
                        float.TryParse(offsets[0],out offset.x);
                        float.TryParse(offsets[1], out offset.y);
                    }
                }
                return offset;
            }
        }

        /// <summary>
        /// 引导GUI控件从属类型
        /// </summary>
        public GuideDefine.GuideGUIDependType GDType
        {
            get
            {
                GuideDefine.GuideGUIDependType gdtype = GuideGUIDependType.None;
                if (null != TableData)
                {
                    gdtype = (GuideDefine.GuideGUIDependType)TableData.guideGUIDependType;
                }
                return gdtype;
            }
        }

        /// <summary>
        /// 引导GUI控件参数
        /// </summary>
        public List<uint> GDParam
        {
            get
            {
                List<uint> ids = new List<uint>();
                if (!string.IsNullOrEmpty(TableData.guideGUIDependParam))
                {
                    string[] idStrs = TableData.guideGUIDependParam.Split(new char[] { '|' });
                    if (null != idStrs)
                    {
                        for (int i = 0; i < idStrs.Length; i++)
                        {
                            if (GDType == GuideGUIDependType.Daily)
                            {
                                uint activeId = 0;
                                if (!uint.TryParse(idStrs[i],out activeId))
                                {
                                    continue;
                                }
                                ids.Add(activeId);

                            }else if (GDType == GuideGUIDependType.Knapsack)
                            {
                                ids.AddRange(DataManager.Manager<ItemManager>().GetItemListByBaseId(uint.Parse(idStrs[i])));
                            }
                        }
                    }
                }

                return ids;
            }
        }

        /// <summary>
        /// 引导数据检测类型
        /// </summary>
        public GuideCheckDataType GuideCheckType
        {
            get
            {
                GuideCheckDataType checkType = GuideCheckDataType.None;
                if (null != TableData)
                {
                    checkType = (GuideCheckDataType)TableData.guideCheckType;
                }
                return checkType;
            }
        }


        /// <summary>
        /// 引导数据物品大类检测类型
        /// </summary>
        public GameCmd.ItemBaseType GuideCheckItemType
        {
            get
            {
                GameCmd.ItemBaseType checkItemType = GameCmd.ItemBaseType.ItemBaseType_None;
                if (null != TableData)
                {
                    checkItemType = (GameCmd.ItemBaseType)TableData.guideCheckType;
                }
                return checkItemType;
            }
        }

        /// <summary>
        /// 引导目标依赖面板
        /// </summary>
        public PanelID GuideTargetDependPanel
        {
            get
            {
                return (null != GuideTargetObjPathData) ? GuideTargetObjPathData.DependPanelId : PanelID.None;
            }
        }
        private GuideObjPathData m_guideTargetObjPathData = null;
        /// <summary>
        /// 引导指向目标数据
        /// </summary>
        public GuideObjPathData GuideTargetObjPathData
        {
            get
            {
                if (null == m_guideTargetObjPathData)
                {
                    GuideObjPathData pathData = new GuideObjPathData();
                    if (null != TableData && !string.IsNullOrEmpty(TableData.guiGUIWidgetPath))
                    {
                        m_guideTargetObjPathData = GuideDefine.ParseGuideObjPathString2PathData(TableData.guiGUIWidgetPath);
                    }else
                    {
                        m_guideTargetObjPathData = new GuideObjPathData();
                    }
                }

                return m_guideTargetObjPathData;
            }
        }

        private GameObject m_obj_guideTargetObj = null;
        /// <summary>
        /// 引导指向目标对象
        /// </summary>
        public GameObject GuideTargetObj
        {
            get
            {
                if (null != m_obj_guideTargetObj)
                {
                    return m_obj_guideTargetObj;
                }

                if (GDType == GuideGUIDependType.None)
                {
                    if (null != GuideTargetObjPathData)
                    {
                        m_obj_guideTargetObj =
                            UIManager.FindGameObjByPanel(GuideTargetObjPathData.DependPanelId, GuideTargetObjPathData.RelativePanelPath);
                    }
                }else
                {
                    List<uint> targetQwThisIds = GDParam;
                    if (targetQwThisIds.Count > 0)
                    {
                        switch(GDType)
                        {
                            case GuideGUIDependType.Knapsack:
                                {
                                    //背包对象获取
                                    KnapsackPanel kp = DataManager.Manager<UIPanelManager>().GetPanel<KnapsackPanel>(PanelID.KnapsackPanel);
                                    if (null != kp)
                                    {
                                        for(int i = 0; i < targetQwThisIds.Count;i++ )
                                        {
                                            m_obj_guideTargetObj = kp.GetGuideTargetObjByQwThisId(targetQwThisIds[i]);
                                            if (null != m_obj_guideTargetObj)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                break;
                            case GuideGUIDependType.Daily:
                                {
                                    //日常活动指引
                                    DailyPanel kp = DataManager.Manager<UIPanelManager>().GetPanel<DailyPanel>(PanelID.DailyPanel);
                                    if (null != kp)
                                    {
                                        for (int i = 0; i < targetQwThisIds.Count; i++)
                                        {
                                            m_obj_guideTargetObj = kp.GetDailyGuideTargetObj(targetQwThisIds[i]);
                                            if (null != m_obj_guideTargetObj)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }

                if (null != m_obj_guideTargetObj)
                {
                    m_v3_targetLocal = m_obj_guideTargetObj.transform.localPosition;
                }
                    
                return m_obj_guideTargetObj;
            }
            set
            {
                m_obj_guideTargetObj = value;
            }
        }

        private GameObject m_obj_guideTargetObjParent = null;
        /// <summary>
        /// 引导指向目标的父节点
        /// </summary>
        public GameObject GuideTargetObjParent
        {
            get
            {
                if (null == m_obj_guideTargetObjParent && null != GuideTargetObjPathData)
                {
                    m_obj_guideTargetObjParent =
                        UIManager.FindGameObjByPanel(GuideTargetObjPathData.DependPanelId, GuideTargetObjPathData.ParentPath);
                }
                return m_obj_guideTargetObjParent;
            }
        }

        /// <summary>
        /// 本地相对边界
        /// </summary>
        public Bounds GuideTargetObjLocalBounds
        {
            get
            {
                Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
                if (null != GuideTargetObj)
                {
                    bounds = NGUIMath.CalculateRelativeWidgetBounds(GuideTargetObj.transform, GuideTargetObj.transform, false, true);
                }
                return bounds;
            }
        }

        /// <summary>
        /// 目标边界世界坐标
        /// </summary>
        public Bounds GuideTargetObjWorldBounds
        {
            get
            {
                Bounds bounds = GuideTargetObjLocalBounds;
                if (null != GuideTargetObj)
                {
                    Matrix4x4 local2WorldMatrix = GuideTargetObj.transform.localToWorldMatrix;
                    Vector3 min = local2WorldMatrix.MultiplyPoint3x4(bounds.min);
                    Vector3 max = local2WorldMatrix.MultiplyPoint3x4(bounds.max);
                    bounds = new Bounds(min, Vector3.zero);
                    bounds.Encapsulate(max);
                }
                return bounds;
            }
        }

        /// <summary>
        /// 指向目标世界坐标
        /// </summary>
        public Vector3 GuideTargetWorldPos
        {
            get
            {
                Vector3 pos = Vector3.zero;
                GameObject target = GuideTargetObj;
                if (null != target)
                {
                    pos = target.transform.position;
                }
                return pos;
            }
        }

        private Vector3 m_v3_targetLocal = Vector3.zero;
        public Vector3 GuideTargetLocalPos
        {
            get
            {
                return m_v3_targetLocal;
            }
        }

        /// <summary>
        /// 是否为引导组内第一个步骤
        /// </summary>
        public bool IsStartStep
        {
            get
            {
                bool startStep = false;
                LocalGuideGroupData localGroupData = GroupData;
                uint id = 0;
                if (null != localGroupData
                    && localGroupData.TryGetStep(0, out id)
                    && id == ID)
                {
                    startStep = true;
                }
                return startStep;
            }
        }

        /// <summary>
        /// 获取当前引导的前置引导
        /// </summary>
        /// <param name="guideId"></param>
        /// <returns></returns>
        public bool TryGetPreStep(out uint guideId)
        {
            guideId = 0;
            if (null != TableData)
            {
                if (TableData.guideStep == 1)
                {
                    return false;
                }else
                {
                    LocalGuideGroupData localGroupData = GroupData;
                    int stepIndex = 0;
                    if (null != localGroupData
                        && localGroupData.TryGetStepIndex(ID, out stepIndex)
                        && localGroupData.TryGetStep(stepIndex - 1, out guideId))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取下一个步骤
        /// </summary>
        /// <param name="guideId"></param>
        /// <returns></returns>
        public bool TryGetNextStep(out uint guideId)
        {
            guideId = 0;
            if (HaveNextStep)
            {
                LocalGuideGroupData localGroupData = GroupData;
                int stepIndex = 0;
                if (null != localGroupData
                    && localGroupData.TryGetStepIndex(ID, out stepIndex)
                    && localGroupData.TryGetStep(stepIndex + 1, out guideId))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否有下一个引导步骤
        /// </summary>
        public bool HaveNextStep
        {
            get
            {
                return !IsEndStep;
            }
        }

        /// <summary>
        /// 组数据
        /// </summary>
        public LocalGuideGroupData GroupData
        {
            get
            {
                LocalGuideGroupData localGroupData = null;
                if (DataManager.Manager<GuideManager>().TryGetGuideGroupData(GuideGroupID, out localGroupData))
                {
                    return localGroupData;
                }
                return null;
            }
        }

        /// <summary>
        /// 是否为引导组内最后一个步骤
        /// </summary>
        public bool IsEndStep
        {
            get
            {
                bool endStep = false;
                LocalGuideGroupData localGroupData = GroupData;
                if (null != localGroupData)
                {
                    endStep = (localGroupData.LastStepGuideID == ID);
                }
                return endStep;
            }
        }
        #endregion

        private LocalGuideData(uint baseid)
        {
            m_uint_id = baseid;
        }

        #region Static Create
        public  static LocalGuideData Create(uint baseId)
        {
            return new LocalGuideData(baseId);
        }
        #endregion
    }

    //本地引导组数据
    public class LocalGuideGroupData
    {
        #region property
        //组ID
        private uint m_uint_groupId;
        public uint GroupId
        {
            get
            {
                return m_uint_groupId;
            }
        }

        /// <summary>
        /// 是否已完成引导
        /// </summary>
        /// <param name="guideStep"></param>
        /// <returns></returns>
        public bool IsCompleteStep (uint guideStep)
        {
            if (GroupSteps.Contains(guideStep) && LastestDoStep != 0)
            {
                int lastIndex = GroupSteps.IndexOf(LastestDoStep);
                int targetIndex = GroupSteps.IndexOf(guideStep);
                return lastIndex >= targetIndex;
            }
            return false;
        }

        private uint m_uint_lastestStep = 0;
        //最近做的一个引导步骤
        public uint LastestDoStep
        {
            get
            {
                return m_uint_lastestStep;
            }
            set
            {
                if (null != m_lst_groupSteps && m_lst_groupSteps.Contains(value))
                {
                    if (m_uint_lastestStep == 0 
                        || m_lst_groupSteps.IndexOf(m_uint_lastestStep) < m_lst_groupSteps.IndexOf(value))
                    {
                        m_uint_lastestStep = value;
                    }
                }
            }
        }

        /// <summary>
        /// 当前正在做的引导
        /// </summary>
        private uint m_uint_curStep = 0;
        public uint CurStep
        {
            get
            {
                return m_uint_curStep;
            }
        }

        public void EndCur()
        {
            m_uint_curStep = 0;
        }

        public void Reset()
        {
            m_uint_lastestStep = 0;
            m_uint_curStep = 0;
        }

        //条件触发码
        private int m_itriggerMask = 0;
        private int TriggerMask
        {
            get
            {
                return m_itriggerMask;
            }
        }

        /// <summary>
        /// 触发条件
        /// </summary>
        private List<uint> m_lstTriggerIds = null;
        public List<uint> TriggerIds
        {
            get
            {
                return m_lstTriggerIds;
            }
        }

        /// <summary>
        /// 忽略触发条件
        /// </summary>
        private List<uint> m_lstIgnoreTriggerIds = null;
        public List<uint> IgnoreTriggerIds
        {
            get
            {
                return m_lstIgnoreTriggerIds;
            }
        }

        //引导组第一步
        private uint m_uFirstStepId = 0;
        public uint FirstStep
        {
            get
            {
                if (m_uFirstStepId == 0)
                {
                    TryGetStep(0,out m_uFirstStepId); 
                }
                return m_uFirstStepId;
            }
        }

        //引导类型
        private GuideDefine.GuideType m_guideType = GuideType.None;
        public GuideDefine.GuideType GType
        {
            get
            {
                return m_guideType;
            }
        }

        //是否为循环触发引导组
        private bool m_bLoopTrigger = false;
        public bool LoopTrigger
        {
            get
            {
                return m_bLoopTrigger;
            }
        }

        /// <summary>
        /// 移动到下一个引导步骤
        /// </summary>
        /// <param name="nextStep"></param>
        /// <returns></returns>
        public bool MoveToNextStep()
        {
            if (TryGetNextStep(out m_uint_curStep))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 尝试获取下一个可执行的步骤
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public bool TryGetNextStep(out uint step)
        {
            bool succes = false;
            step = 0;
            if (LastestDoStep == 0 )
            {
                if (TryGetStep(0,out step))
                {
                    succes = true;
                }
            }
            else if (null != GroupSteps && GroupSteps.Contains(LastestDoStep))
            {
                int index = GroupSteps.IndexOf(LastestDoStep);
                if (index < GroupSteps.Count-1)
                {
                    step = GroupSteps[index + 1];
                    succes = true;
                }
            }
            return succes;
        }

        /// <summary>
        /// 最后一步的引导ID
        /// </summary>
        /// <returns></returns>
        public uint LastStepGuideID
        {
            get
            {
                return (GroupStepCount != 0) ? m_lst_groupSteps[GroupStepCount - 1] : 0;
            }
        }


        //引导组步骤总数
        public int GroupStepCount
        {
            get
            {
                return m_lst_groupSteps.Count;
            }
        }

        //优先级
        private int m_int_priority;
        public int Priority
        {
            get
            {
                return m_int_priority;
            }
        }
        #endregion

        #region Op

        private bool m_bInit = false;
        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData()
        {
            if (m_bInit)
                return;
            m_bInit = true;
            GuideDefine.LocalGuideData localData = GuideDefine.LocalGuideData.Create(FirstStep);
            if (null == localData)
            {
                Engine.Utility.Log.Error("LocalGuideGroupData->InitData failed,data error id:{0}" + FirstStep);
                return ;
            }
            
            //1、触发条件
            m_lstTriggerIds = new List<uint>();
            //2、忽略条件
            m_lstIgnoreTriggerIds = new List<uint>();

            if (null != localData.TableData)
            {
                uint tempId = 0;
                string[] idArrayStr = null;
                if (!string.IsNullOrEmpty(localData.TableData.triggerCondiIDStr))
                {
                    idArrayStr = localData.TableData.triggerCondiIDStr.Split(new char[] { '|' });
                    if (null != idArrayStr && idArrayStr.Length > 0)
                    {

                        for (int i = 0, max = idArrayStr.Length; i < max; i++)
                        {
                            if (string.IsNullOrEmpty(idArrayStr[i]))
                                continue;
                            if (!uint.TryParse(idArrayStr[i].Trim(), out tempId))
                                continue;
                            if (!m_lstTriggerIds.Contains(tempId))
                                m_lstTriggerIds.Add(tempId);
                        }
                    }
                }


                if (!string.IsNullOrEmpty(localData.TableData.ignoreCondiStr))
                {
                    idArrayStr = localData.TableData.ignoreCondiStr.Split(new char[] { '|' });
                    if (null != idArrayStr && idArrayStr.Length > 0)
                    {
                        for (int i = 0, max = idArrayStr.Length; i < max; i++)
                        {
                            if (string.IsNullOrEmpty(idArrayStr[i]))
                                continue;
                            if (!uint.TryParse(idArrayStr[i].Trim(), out tempId))
                                continue;
                            if (!m_lstIgnoreTriggerIds.Contains(tempId))
                                m_lstIgnoreTriggerIds.Add(tempId);
                        }
                    }
                }

            }

            //3、优先级
            m_int_priority = localData.Priority;

            //4、循环触发
            m_bLoopTrigger = localData.LoopTrigger;

            //5、引导类型
            m_guideType = localData.GType;
        }

        //引导组内步骤
        private List<uint> m_lst_groupSteps = null;
        public List<uint> GroupSteps
        {
            get
            {
                return m_lst_groupSteps;
            }
        }

        /// <summary>
        /// 添加引导步骤
        /// </summary>
        /// <param name="guideStepData"></param>
        /// <returns></returns>
        public bool Add(LocalGuideData guideStepData)
        {
            if (guideStepData.GuideGroupID == GroupId && !m_lst_groupSteps.Contains(guideStepData.ID))
            {
                m_lst_groupSteps.Add(guideStepData.ID);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 引导优先级排序
        /// </summary>
        public void SortGuidePriority()
        {
            if (null == m_lst_groupSteps)
            {
                return;
            }

            //排序
            m_lst_groupSteps.Sort((left, right) =>
            {
                return (int)LocalGuideData.Create(left).GuideGroupStep - (int)LocalGuideData.Create(right).GuideGroupStep;
            });
        }

        /// <summary>
        /// 尝试获取步骤索引
        /// </summary>
        /// <param name="guideId"></param>
        /// <returns></returns>
        public bool TryGetStepIndex(uint guideId,out int stepIndex)
        {
            stepIndex = 0;
            if (null != m_lst_groupSteps && m_lst_groupSteps.Contains(guideId))
            {
                stepIndex = m_lst_groupSteps.IndexOf(guideId);
                return true;
            }
            return false;

        }

        /// <summary>
        /// 尝试获取组内步骤
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        /// <param name="guideId">引导ID</param>
        /// <returns></returns>
        public bool TryGetStep(int stepIndex, out uint guideId)
        {
            guideId = 0;
            if (GroupStepCount > stepIndex)
            {
                guideId = m_lst_groupSteps[stepIndex];
                return true;
            }
            return false;
        }

        #endregion

        #region Static
        private LocalGuideGroupData(uint groupId)
        {
            this.m_uint_groupId = groupId;
            m_lst_groupSteps = new List<uint>();
        }

        /// <summary>
        /// 创建组数据
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static LocalGuideGroupData Create(uint groupId)
        {
            return new LocalGuideGroupData(groupId);
        }
        #endregion

    }
    #endregion

    #region static method
    public static string[] PanelEnumNames = null;
    public static GuideObjPathData ParseGuideObjPathString2PathData(string path)
    {
        GuideObjPathData pathData = new GuideObjPathData();
        if (!string.IsNullOrEmpty(path))
        {
            string panelString = path.Substring(0, path.IndexOf('/'));
            if (null == PanelEnumNames)
            {
                PanelEnumNames = Enum.GetNames(typeof(PanelID));;
            }
            if (null != PanelEnumNames && !string.IsNullOrEmpty(panelString))
            {
                bool define = false;
                for (int i = 0; i < PanelEnumNames.Length; i++)
                {
                    if (PanelEnumNames[i].Equals(panelString, StringComparison.CurrentCultureIgnoreCase))
                    {
                        define = true;
                        break;
                    }
                }
                if (define)
                {
                    pathData.DependPanelId = (PanelID)Enum.Parse(typeof(PanelID), panelString, true);
                }
            }
            
            pathData.RelativePanelPath = path.Substring(path.IndexOf('/') + 1);
            if (!string.IsNullOrEmpty(pathData.RelativePanelPath) && pathData.RelativePanelPath.Contains("/"))
                pathData.ParentPath = pathData.RelativePanelPath.Substring(0, pathData.RelativePanelPath.LastIndexOf('/'));
            else
                pathData.ParentPath = "";
        }
        return pathData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="triggerType"></param>
    /// <returns></returns>
    public static bool IsMaskMatchTriggerType(int mask ,uint triggerType)
    {
        return IsMaskMatchTriggerType(mask, (int)triggerType);
    }

    public static bool IsMaskMatchTriggerType(int mask,GuideTriggerType triggerType)
    {
        return IsMaskMatchTriggerType(mask, (int)triggerType);
    }

    public static bool IsMaskMatchTriggerType(int mask,int triggerType)
    {
        return (mask & GetTriggerMaskByType(triggerType)) != 0;
    }

    /// <summary>
    /// 获取触发类型mask
    /// </summary>
    /// <param name="triggerType"></param>
    /// <returns></returns>
    public static int GetTriggerMaskByType(int triggerType)
    {
        return (1 << (triggerType));
    }

    public static int GetTriggerMaskByType(GuideTriggerType triggerType)
    {
        return GetTriggerMaskByType((int)triggerType);
    }
    #endregion
}