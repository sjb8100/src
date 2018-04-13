/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.DataManager
 * 创建人：  wenjunhua.zqgame
 * 文件名：  EffectDisplayManager
 * 版本号：  V1.0.0.0
 * 创建时间：6/28/2017 10:12:04 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class EffectDisplayManager : IManager,IGlobalEvent
{
    #region define
    /// <summary>
    /// 展示数据
    /// </summary>
    public class EffectDisplayData
    {
        /// <summary>
        /// 展示效果类型
        /// </summary>
        public enum EffectDisplayType
        {
            Disp_MapName = 1,       //地图名称
            Disp_Partical = 2,      //屏幕中央粒子特效
            Disp_Tips = 3,          //屏幕中飘字
        }
        //展示效果类型
        public EffectDisplayType DisPlayType = EffectDisplayType.Disp_MapName;
        public float CacheStartTime = 0;
        //传递数据
        public object Data;
    }
    #endregion

    #region Imanager
    // <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        m_cacheDisplayDatas = new Queue<EffectDisplayData>();
        m_dicTipsCache = new Dictionary<int, List<TipsData>>();
        RegisterGlobalEvent(true);
    }

    /// <summary>
    /// 重置
    /// </summary>
    /// <param name="depthClearData">是否深度清除管理器数据</param>
    public void Reset(bool depthClearData = false)
    {
        if (depthClearData)
        {
            m_cacheDisplayDatas = new Queue<EffectDisplayData>();
            m_isDoingDisplayEffect = true;
            m_dicTipsCache = new Dictionary<int, List<TipsData>>();
            ClearTips();
        }
    }
    /// <summary>
    /// 每帧执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Process(float deltaTime)
    {
        ProcessTips(deltaTime);
    }
    /// <summary>
    /// 清理数据
    /// </summary>
    public void ClearData()
    {

    }
    #endregion

    #region IGlobalEvent
    //注册/反注册全局事件
    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTADDDISPLAYEFFECT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLEARDISPLAYCACHE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEVELUP, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_DONE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTDISPLAYEFFECTCOMPLETE, GlobalEventHandler);
        }else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTADDDISPLAYEFFECT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLEARDISPLAYCACHE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEVELUP, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_DONE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTDISPLAYEFFECTCOMPLETE, GlobalEventHandler);
        }
    }

    //全局事件处理
    public void GlobalEventHandler(int eventid, object data)
    {
        switch(eventid)
        {
            case (int)Client.GameEventID.UIEVENTADDDISPLAYEFFECT:
                {
                    uint mapId = (uint)((int)data);
                    table.MapDataBase mapDb = GameTableManager.Instance.GetTableItem<table.MapDataBase>(mapId);
                    if (null != mapDb)
                    {
                        AddMapDisplayEffect(mapDb.strName);
                    }
                }
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_LEVELUP:
                {
                    Client.stEntityLevelUp lvUp = (Client.stEntityLevelUp)data;
                    if (lvUp.uid == DataManager.Instance.UId)
                    {
                        AddLevelUpEffect();
                    }
                }
                break;
            case (int)Client.GameEventID.TASK_DONE:
                {
                    AddTaskCompelteEffect();
                }
                break;
            case (int)Client.GameEventID.UIEVENTDISPLAYEFFECTCOMPLETE:
                {
                    m_isDoingDisplayEffect = false;
                    DoNextParticalDisplayEffect();
                }
                break;
            case (int) Client.GameEventID.UIEVENTCLEARDISPLAYCACHE:
                {
                    m_cacheDisplayDatas.Clear();
                    m_isDoingDisplayEffect = false;
                    //清空tips
                    ClearTips();
                }
                break;
        }
    }
    #endregion

    #region DisplayCommon
    System.Collections.Generic.Queue<EffectDisplayData> m_cacheDisplayDatas = null;
    private bool m_isDoingDisplayEffect = false;
    public static float CenterDisplayEffectCacheLT
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<float>("CenterDisplayEffectCacheLT");
        }
    }
    /// <summary>
    /// 是否可以展示中央特效
    /// </summary>
    /// <returns></returns>
    public bool CanShowDisPlayEffect()
    {
        return DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.EffectDiplayPanel);
    }

    /// <summary>
    /// 添加地图名称展示
    /// </summary>
    /// <param name="mapName"></param>
    public void AddMapDisplayEffect(string mapName)
    {
        if (!CanShowDisPlayEffect())
        {
            return;
        }
        EffectDisplayData displayData = new EffectDisplayData()
        {
            DisPlayType = EffectDisplayData.EffectDisplayType.Disp_MapName,
            Data = mapName,
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTPlayDISPLAYEFFECT, displayData);
    }

    /// <summary>
    /// 添加粒子效果展示
    /// </summary>
    /// <param name="resId"></param>
    public void AddParticalDisplayEffect(uint resId)
    {
        if (!CanShowDisPlayEffect())
        {
            return;
        }
        EffectDisplayData displayData = new EffectDisplayData()
        {
            DisPlayType = EffectDisplayData.EffectDisplayType.Disp_Partical,
            Data = resId,
            CacheStartTime = Time.time,
        };

        if (m_cacheDisplayDatas.Count >= MAX_TIPS_CACHE_NUM)
        {
            //移除前面的，腾出空间放下一个
            m_cacheDisplayDatas.Dequeue();
            Engine.Utility.Log.Warning("EffectDisplayManager->AddParticalDisplayEffect warning,over max num limit");
        }
        m_cacheDisplayDatas.Enqueue(displayData);
        DoNextParticalDisplayEffect();
    }

    /// <summary>
    /// 任务完成
    /// </summary>
    public void AddTaskCompelteEffect()
    {
        AddParticalDisplayEffect(50012);
    }

    /// <summary>
    /// 强化成功
    /// </summary>
    public void AddEquipGridStrengthenEffect()
    {
        AddParticalDisplayEffect(50024);
    }

    /// <summary>
    /// 升级
    /// </summary>
    public void AddLevelUpEffect()
    {
        AddParticalDisplayEffect(50011);
    }

    /// <summary>
    /// 执行下一个粒子效果展示
    /// </summary>
    private void DoNextParticalDisplayEffect()
    {
        if (!CanShowDisPlayEffect())
        {
            m_cacheDisplayDatas.Clear();
            return;
        }

        if (m_isDoingDisplayEffect)
        {
            return;
        }

        if (m_cacheDisplayDatas.Count == 0)
        {
            return;
        }

        EffectDisplayData data = null;
        bool success = false;
        while (m_cacheDisplayDatas.Count > 0)
        {
            data = m_cacheDisplayDatas.Dequeue();
            if (Time.time -  data.CacheStartTime <= CenterDisplayEffectCacheLT)
            {
                //是否超过缓存最大时间
                success = true;
                break;
            }
        }
        if (success)
        {
            m_isDoingDisplayEffect = true;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTPlayDISPLAYEFFECT, data);
        }
    }

    /// <summary>
    /// 粒子展示效果完成
    /// </summary>
    private void OnParticalDisplayCompelte()
    {

    }

    #endregion

    #region NewTips
    private Dictionary<int, List<TipsData>> m_dicTipsCache = null;
    private float m_fTipsShowSinceLast = 0;

    private float m_fTipsShowGap = -1f;
    //tips 显示间隔
    public float TipsShowGap
    {
        get
        {
            if (m_fTipsShowGap == -1)
            {
                m_fTipsShowGap = GameTableManager.Instance.GetGlobalConfig<float>("TipsShowGap");
            }
            return m_fTipsShowGap;
        }
    }

    //tips缓存时长
    public static float TipsCacheTime
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<float>("TipsCacheLifeTime");
        }
    }

    //tips UI间隔Y
    public static float TipsUIGapY
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<float>("TipsUIGapY");
        }
    }

    //最大在屏幕中同时出现的tips数量
    public static float MaxTipAliveNum
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<int>("MaxTipAliveNum");
        }
    }

    //tips 优先级
    public enum TipsPriority
    {
        Low,
        Normal,
        High,
    }

    /// <summary>
    /// Tips数据类
    /// </summary>
    public class TipsData
    {
        //文本
        private string m_strTxt = "";
        public string Txt
        {
            get
            {
                return m_strTxt;
            }
        }

        //缓存时间戳
        private float m_fCacheTime = 0;
        public float CacheTime
        {
            get
            {
                return m_fCacheTime;
            }
        }
        //优先级
        private TipsPriority m_emPriority = TipsPriority.Normal;
        public TipsPriority Priority
        {
            get
            {
                return m_emPriority;
            }
        }


        /// <summary>
        /// 刷新缓存时间
        /// </summary>
        /// <param name="cacheTime"></param>
        public void RefreshCacheTime(long cacheTime)
        {
            this.m_fCacheTime = cacheTime;
        }

        private TipsData(string txt, float cacheTime, TipsPriority priority)
        {
            this.m_strTxt = txt;
            this.m_fCacheTime = cacheTime;
            this.m_emPriority = priority;
        }

        public static TipsData Create(string txt, float cacheTime = 0, TipsPriority priority = TipsPriority.Normal)
        {
            return new TipsData(txt, cacheTime, priority);
        }
    }

    //最大缓存tips数量
    public const int MAX_TIPS_CACHE_NUM = 30;
    /// <summary>
    /// 添加一个tips
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="priority"></param>
    public void AddTips(string txt, TipsPriority priority = TipsPriority.Normal)
    {
        int priorityInt = (int)priority;
        //if (!CanShowDisPlayEffect())
        //{
            
                  
        //}

        if (DataManager.Instance.Ready)
        {
            if (DataManager.Manager<UIPanelManager>() != null)
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.EffectDiplayPanel);
            }
        }
        if (!m_dicTipsCache.ContainsKey(priorityInt))
        {
            m_dicTipsCache.Add(priorityInt, new System.Collections.Generic.List<TipsData>());
        }
        TextManager.ClearStrNGUIColor(ref txt);
        if (m_dicTipsCache[priorityInt].Count >= MAX_TIPS_CACHE_NUM)
        {
            Engine.Utility.Log.Warning("EffectTipsManager->AddTips faield,over max num limit");
            return;
        }
        m_dicTipsCache[priorityInt].Add(TipsData.Create(txt, Time.time, priority));
        ProcessTips(0);
    }

    /// <summary>
    /// 执行tips
    /// </summary>
    private bool DoShowNextTips()
    {
        if (!CanShowDisPlayEffect())
        {
            //不清空数据
            //ClearTips();
            return false;
        }

        float curTime = Time.time;
        EffectDisplayData displayData = null;
        int priorityInt = 0;
        for (TipsPriority i = TipsPriority.High, min = TipsPriority.Low; i >= min; i--)
        {
            priorityInt = (int)i;
            if (!m_dicTipsCache.ContainsKey(priorityInt))
                continue;

            while (null != m_dicTipsCache[priorityInt] && m_dicTipsCache[priorityInt].Count > 0)
            {
                if (curTime - m_dicTipsCache[priorityInt][0].CacheTime < TipsCacheTime)
                {
                    displayData = new EffectDisplayData()
                    {
                        DisPlayType = EffectDisplayData.EffectDisplayType.Disp_Tips,
                        Data = m_dicTipsCache[priorityInt][0].Txt,

                    };
                    m_dicTipsCache[priorityInt].RemoveAt(0);
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTPlayDISPLAYEFFECT, displayData); 
                    return true;
                }
                else
                {
                    m_dicTipsCache[priorityInt].RemoveAt(0);
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 检测tips展示
    /// </summary>
    private void ProcessTips(float deltaTime)
    {
        if (Time.time - m_fTipsShowSinceLast >= TipsShowGap)
        {
            DoShowNextTips();
            m_fTipsShowSinceLast = Time.time;
        }
    }

    /// <summary>
    /// 清空Tips
    /// </summary>
    private void ClearTips()
    {
        m_fTipsShowSinceLast = 0;
        m_dicTipsCache.Clear();
    }

    #endregion
}
