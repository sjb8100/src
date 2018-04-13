/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Clan
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ClanDeclareWarPanel
 * 版本号：  V1.0.0.0
 * 创建时间：12/23/2016 1:57:30 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class ClanDeclareWarPanel
{
    #region define
    public enum ClanDeclareMode
    {
        None = 0,
        History,
        Search,
        Max,
    }
    #endregion

    #region property
    private string m_str_inpuSearchInfo = "";
    private Dictionary<ClanDeclareMode, UITabGrid> m_dic_tabs = null;
    private Dictionary<ClanDeclareMode, Transform> m_dic_ts = null;
    private ClanDeclareMode m_em_cur = ClanDeclareMode.None;
    private List<uint> m_lst_datas = null;
    #endregion

    #region overridemethod

    protected override void OnLoading()
    {
        base.OnLoading();
        RegisterGlobalUIEvent(true);
        m_lst_datas = new List<uint>();
        if (null != m_input_Input)
        {
            m_input_Input.onChange.Add(new EventDelegate(() =>
            {
                m_str_inpuSearchInfo = TextManager.GetTextByWordsCountLimitInUnicode(m_input_Input.value
                    , TextManager.CONST_NAME_MAX_WORDS);
                m_input_Input.value = m_str_inpuSearchInfo;
            }));
            m_input_Input.onSubmit.Add(new EventDelegate(() =>
            {
                m_str_inpuSearchInfo = TextManager.GetTextByWordsCountLimitInUnicode(m_input_Input.value
                     , TextManager.CONST_NAME_MAX_WORDS);
                m_input_Input.value = m_str_inpuSearchInfo;
            }));
        }

        m_dic_tabs = new Dictionary<ClanDeclareMode, UITabGrid>();
        m_dic_ts = new Dictionary<ClanDeclareMode, Transform>();
        UITabGrid tab = null;
        Transform ts = null;
        for (ClanDeclareMode i = ClanDeclareMode.None + 1; i < ClanDeclareMode.Max; i++)
        {
            if (null != m_trans_ToggleContent)
            {
                ts = m_trans_ToggleContent.Find(i.ToString());
                if (null == ts)
                {
                    continue;
                }
                tab = ts.GetComponent<UITabGrid>();
                if (null == tab)
                {
                    tab = ts.gameObject.AddComponent<UITabGrid>();
                }
                tab.SetGridData(i);
                tab.RegisterUIEventDelegate(OnGridEventDlg);
                m_dic_tabs.Add(i, tab);
            }
            
            if (null != m_trans_Content)
            {
                ts = m_trans_Content.Find(i.ToString());
                if (null == ts)
                {
                    continue;
                }
                m_dic_ts.Add(i, ts);
            }
        }

        SetMode(ClanDeclareMode.History,true);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        SetMode(ClanDeclareMode.History);
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        RegisterGlobalUIEvent(false);
    }

    #endregion

    #region Op

    /// <summary>
    /// 注册全局事件
    /// </summary>
    /// <param name="register"></param>
    private void RegisterGlobalUIEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANRIVALRYHISTORYREFRESH, OnGloablUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANDECLARESEARCHREFRESH, OnGloablUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANSTARTDECLAREWAR, OnGloablUIEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANRIVALRYHISTORYREFRESH, OnGloablUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANDECLARESEARCHREFRESH, OnGloablUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANSTARTDECLAREWAR, OnGloablUIEventHandler);
        }
    }

    /// <summary>
    ///全局事件处理 
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    private void OnGloablUIEventHandler(int eventType,object data)
    {
        switch(eventType)
        {
            case (int)Client.GameEventID.UIEVENTCLANRIVALRYHISTORYREFRESH:
                BuildHistoryList();
                break;
            case (int)Client.GameEventID.UIEVENTCLANDECLARESEARCHREFRESH:
                BuildSearchList();
                break;
            case (int)Client.GameEventID.UIEVENTCLANSTARTDECLAREWAR:
                HideSelf();
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="force"></param>
    private void SetMode(ClanDeclareMode mode,bool force = false)
    {
        if (m_em_cur == mode && !force)
        {
            return;
        }
        UITabGrid tabGrid = null;
        if (null != m_dic_tabs && m_dic_tabs.TryGetValue(m_em_cur,out tabGrid))
        {
            tabGrid.SetHightLight(false);
        }
        if (null != m_dic_tabs && m_dic_tabs.TryGetValue(mode, out tabGrid))
        {
            tabGrid.SetHightLight(true);
        }
        m_em_cur = mode;
        UpdateVisibleWidgets();
        if (mode == ClanDeclareMode.History)
        {
            BuildHistoryList();
        }else if (mode == ClanDeclareMode.Search)
        {
            ClearSearchInfo();
        }
    }

    /// <summary>
    /// 刷新可见组件
    /// </summary>
    private void UpdateVisibleWidgets()
    {
        Transform ts = null;
        if (null != m_dic_ts)
        {
            for (ClanDeclareMode i = ClanDeclareMode.None + 1; i < ClanDeclareMode.Max; i++)
            {
                if (m_dic_ts.TryGetValue(i,out ts) && ts.gameObject.activeSelf != (i==m_em_cur))
                {
                    ts.gameObject.SetActive(i == m_em_cur);
                }
            }
        }
        
    }

    /// <summary>
    /// 构建历史列表
    /// </summary>
    private void BuildHistoryList()
    {
        if (!m_bInitHistoryCreator)
        {
            m_bInitHistoryCreator = true;
            if (null != m_ctor_HistoryScrollView)
            {
                GameObject obj = UIManager.GetResGameObj(GridID.Uiclandecarewarrivalrygrid) as GameObject;
                m_ctor_HistoryScrollView.RefreshCheck();
                m_ctor_HistoryScrollView.Initialize<UIClanDecareWaRivalryGrid>(obj, OnUpdateGridBase, OnGridEventDlg);
            }
        }
        if (null != m_ctor_HistoryScrollView)
        {
            if (null == m_lst_datas)
            {
                m_lst_datas = new List<uint>();
            }
            m_lst_datas.Clear();
            m_lst_datas.AddRange(DataManager.Manager<ClanManger>().GetClanRivalryHistoryList());
            m_ctor_HistoryScrollView.CreateGrids(m_lst_datas.Count);
        }
    }

    /// <summary>
    /// 清空搜索
    /// </summary>
    public void ClearSearchInfo()
    {
        m_str_inpuSearchInfo = "";
        if (null != m_input_Input)
        {
            m_input_Input.value = m_str_inpuSearchInfo;
        }

        if (null != m_ctor_SearchScrollView)
        {
            m_ctor_SearchScrollView.CreateGrids(0);
        }
    }
    private bool m_bInitHistoryCreator = false;
    private bool m_bInitCreator = false;
    /// <summary>
    /// 构建搜索列表
    /// </summary>
    private void BuildSearchList()
    {
        if (!m_bInitCreator)
        {
            m_bInitCreator = true;
            if (null != m_ctor_SearchScrollView)
            {
                GameObject obj = UIManager.GetResGameObj(GridID.Uiclandecarewarrivalrygrid) as GameObject;
                m_ctor_SearchScrollView.RefreshCheck();
                m_ctor_SearchScrollView.Initialize<UIClanDecareWaRivalryGrid>(obj, OnUpdateGridBase, OnGridEventDlg);
            }
        }
        if (null != m_ctor_SearchScrollView)
        {
            if (null == m_lst_datas)
            {
                m_lst_datas = new List<uint>();
            }
            m_lst_datas.Clear();
            m_lst_datas.AddRange(DataManager.Manager<ClanManger>().GetDeclareWarSerchInfoList());
            m_ctor_SearchScrollView.CreateGrids(m_lst_datas.Count);
        }
    }

    #endregion

    #region UIEvent

    private void OnUpdateGridBase(UIGridBase grid,int index)
    {
        if (grid is UIClanDecareWaRivalryGrid)
        {
            if (null == m_lst_datas || m_lst_datas.Count <= index)
            {
                return;
            }
            UIClanDecareWaRivalryGrid dwGrid = grid as UIClanDecareWaRivalryGrid;
            GameCmd.stWarClanInfo info = null;
            if (m_em_cur == ClanDeclareMode.History)
            {
                info = DataManager.Manager<ClanManger>().GetClanRivalryHistoryInfo(m_lst_datas[index]);
            }else if (m_em_cur == ClanDeclareMode.Search)
            {
                info = DataManager.Manager<ClanManger>().GetClanDeclareWarSerchInfo(m_lst_datas[index]);
            }
            dwGrid.SetGridData(info);
        }
    }

    private void OnGridEventDlg(UIEventType eventType,object data,object param)
    {
        switch(eventType)
        {
            case UIEventType.Click:
                if (data is UITabGrid)
                {
                    UITabGrid tabGrid = data as UITabGrid;
                    SetMode((ClanDeclareMode)tabGrid.Data);
                }
                break;
        }
    }
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_BtnSearch_Btn(GameObject caster)
    {
        if (string.IsNullOrEmpty(m_str_inpuSearchInfo))
        {
            TipsManager.Instance.ShowTips("输入不能为空");
            return;
        }
        DataManager.Manager<ClanManger>().GetDeclareWarSearchInfos(m_str_inpuSearchInfo);
    }
    #endregion
}