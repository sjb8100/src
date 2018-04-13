using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class ReturnBackUIData
{
    public PanelID panelid;
    public UIMsgID msgid;
    public object param;
    public object data;
}

public class ReturnBackUIMsg
{
    public int[] tabs;
    public object param;

    public override string ToString()
    {
        string str = "";
        if (tabs != null)
        {
            for (int i = 0; i < tabs.Length; i++)
            {
                str += tabs[i].ToString() + " ";
            }
        }
        return str + "  " + param.ToString();
    }
}

public class UIFrameManager : Singleton<UIFrameManager>,Engine.Utility.ITimer,IGlobalEvent
{
    const int TIMERID = 1902;
    UIPanelManager m_PanelManager = null;

    public UIPanelManager UIPManager
    {
        get
        {
            if (m_PanelManager == null)
            {
                m_PanelManager = DataManager.Manager<UIPanelManager>();
                m_PanelManager.RegisterGlobalEvent(true);
            }
            return m_PanelManager;
        }
    }

    #region IGlobalEvent

    /// <summary>
    /// 事件注册
    /// </summary>
    /// <param name="regster"></param>
    public void RegisterGlobalEvent(bool regster)
    {
        if (regster)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTTABFUNCOPEN, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTTABFUNCOPEN, GlobalEventHandler);
        }
    }

    /// <summary>
    /// 全局UI事件处理器
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    public void GlobalEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            case (int)Client.GameEventID.UIEVENTTABFUNCOPEN:
                {
                    int funcId = (int)data;
                    UITabGrid tabGrid = null;
                    if (m_dicTabFunc.TryGetValue(funcId,out tabGrid))
                    {
                        tabGrid.SetOpenStatus(true);
                    }
                }
                break;
        }
    }
    #endregion

    private bool m_bShowRightBtn = false;
    private float m_fClosePanelTime = 0f;
    private float m_fCloseRightBtnTime = 0;

    private PanelID m_currPanelID = PanelID.None;
    private PanelID m_startPanelID = PanelID.None;

    public PanelID CurrShowPanelID { get { return m_currPanelID; } }
    Stack<ReturnBackUIData[]> m_stackReturnBackUIData = new Stack<ReturnBackUIData[]>(4);


    List<string> m_lstDepende = new List<string>();
    List<string> m_lstBtnDepende = new List<string>();
    
    List<UITabGrid> m_lstTabGrid = new List<UITabGrid>(6);
    private Dictionary<int, UITabGrid> m_dicTabFunc = new Dictionary<int, UITabGrid>();
    GameObject rightTabs = null;
    public UIFrameManager()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.MAINLEFTTBTN_TOGGLE, OnEvent);
        Client.IGameOption option = Client.ClientGlobal.Instance().gameOption;
        if (option != null)
        {
            m_fCloseRightBtnTime = (float)option.GetInt("UI", "CloseRightBtnTime", 10);
        }
        InitTabGrids();
    }

    public void ShowUIFrame(UIPanelBase panelBase, params ReturnBackUIData[] returnBackUIData)
    {
        if (panelBase.PanelId == PanelID.CommonBgPanel || panelBase.PanelId == PanelID.TopBarPanel)
        {
            return;
        }

        if (panelBase.PanelInfo == null)
        {
            return;
        }
        /*
        if (panelBase.PanelInfo.NeedBg)
        {
            UIManager.ShowPanel(PanelID.CommonBgPanel);
        }

        if (panelBase.PanelInfo.NeedTopBar)
        {
            if (returnBackUIData != null && returnBackUIData.Length > 0)
            {
                m_stackReturnBackUIData.Push(returnBackUIData);
            }
            else
            {
                if (m_startPanelID == PanelID.None)
                {
                    m_startPanelID = panelBase.PanelId;
                }

                if (m_startPanelID != PanelID.None && m_currPanelID != PanelID.None && m_startPanelID != panelBase.PanelId)
                {
                    ReturnBackUIData[] returnUI = new ReturnBackUIData[1];
                    returnUI[0] = new ReturnBackUIData()
                    {
                        panelid = m_currPanelID,
                    };
                    m_stackReturnBackUIData.Push(returnUI);
                }
            }

           // Debug.LogError(uidata.strTitle + "  " + m_stackReturnBackUIData.Count);
            m_currPanelID = panelBase.PanelId;

            UIManager.ShowPanel(PanelID.TopBarPanel, data: panelBase.PanelInfo);

            if (m_fClosePanelTime > 0f)
            {
                Engine.Utility.TimerAxis.Instance().KillTimer(TIMERID, this);
                m_fClosePanelTime = 0f;
            }
        }*/

        CreateTabs(panelBase);
    }
    #region TabGrid

    void InitTabGrids()
    {
        GameObject RightTabsPrefab = UIManager.GetResGameObj(GridID.Righttabs);

        rightTabs = NGUITools.AddChild(UIRootHelper.Instance.StretchTransRoot.gameObject, RightTabsPrefab);
        if (rightTabs == null)
        {
            return;
        }
        Transform btnRoot = rightTabs.transform.Find("bg/btnRoot");
        if (btnRoot == null)
        {
            return;
        }



        GameObject m_btnPrefab = UIManager.GetResGameObj(GridID.Togglepanel);

        for (int i = 0; i < m_lstTabGrid.Capacity; i++)
        {
            GameObject go = NGUITools.AddChild(btnRoot.gameObject, m_btnPrefab);
            go.transform.localPosition = new UnityEngine.Vector3(0, -i * 93, 0);
            UITabGrid grid = go.AddComponent<UITabGrid>();
            go.SetActive(false);

            m_lstTabGrid.Add(grid);
        }
        rightTabs.SetActive(false);

    }

    protected void CreateTabs(UIPanelBase panelBase)
    {
        if (panelBase.PanelInfo != null && panelBase.PanelInfo.PanelTaData.Enable)
        {
            AddTab(panelBase);
        }
    }
 
    private void AddTab( UIPanelBase panelBase)
    {
        if (rightTabs == null)
        {
            return;
        }
        rightTabs.transform.parent = panelBase.transform;
        rightTabs.transform.localPosition = Vector3.zero;
        rightTabs.SetActive(true);

        //清空功能页签
        m_dicTabFunc.Clear();

        int index = 1;
        Dictionary<int, UITabGrid> dicTabs = null;
        if (!panelBase.dicUITabGrid.TryGetValue(index, out dicTabs))
        {
            dicTabs = new Dictionary<int, UITabGrid>(6);
            panelBase.dicUITabGrid.Add(index, dicTabs);
        }
        dicTabs.Clear();
        UITabGrid grid = null;
        List<UIPanelManager.PanelTabData.PanelTabUnit> tabUnitDatas = panelBase.PanelInfo.PanelTaData.GetTabUnitList();
        UIPanelManager.PanelTabData.PanelTabUnit tabUnit = null;
        List<int> activeTabIndex = new List<int>();
        bool tabFuncOpen = false;
        for (int i = 0; i < panelBase.PanelInfo.PanelTaData.Count; i++)
        {
            tabUnit = tabUnitDatas[i];

            grid = m_lstTabGrid[tabUnit.PosIndex];
            if (grid == null)
            {
                continue;
            }
            if (!activeTabIndex.Contains(tabUnit.PosIndex))
                activeTabIndex.Add(tabUnit.PosIndex);
            //grid.transform.parent = root;
            grid.gameObject.SetActive(true);
           // grid.transform.localPosition = new UnityEngine.Vector3(0, -i * 93, 0);
            grid.SetHightLight(false);
            grid.TabID = tabUnit.EnumValue;
            grid.TabType = index;
            grid.SetName(tabUnit.EnumName);
            dicTabs.Add(grid.TabID, grid);
            grid.SetRedPointStatus(false);
            grid.SetSoundEffectType(ButtonPlay.ButtonSountEffectType.FuncTabFirst);
            tabFuncOpen = DataManager.Manager<GuideManager>().IsTabFuncOpen(tabUnit.FuncID);
            grid.SetOpenStatus(tabFuncOpen);
            grid.gameObject.name = tabUnit.ObjName;
            if (!tabFuncOpen && tabUnit.FuncID != 0
                && !m_dicTabFunc.ContainsKey(tabUnit.FuncID))
            {
                m_dicTabFunc.Add(tabUnit.FuncID, grid);
            }
            grid.RegisterUIEventDelegate((eventType, data, param) =>
            {
                switch (eventType)
                {
                    case UIEventType.Click:
                        if (data is UITabGrid)
                        {
                            UITabGrid tabGRid = data as UITabGrid;
                            OnCilckTogglePanel(ref panelBase, grid.TabType, tabGRid.TabID);
                        }
                        break;
                    default:
                        break;
                }
            });
        }
        coudsss++;
        for (int j = 0; j < m_lstTabGrid.Count; j++)
        {
            if (!activeTabIndex.Contains(j))
            {
                m_lstTabGrid[j].gameObject.SetActive(false);
            }
        }
    }
    #endregion

    public static int coudsss = 0;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="nTabIndex">索引</param>
    /// <param name="nTabType">第几类页签</param>
    public void OnCilckTogglePanel(ref UIPanelBase panel,int nTabType ,int nTabIndex)
    {
        if (null == panel)
        {
            return ;
        }
        UIPanelManager.PanelTabData.PanelTabUnit unit = null;
        if (null == panel.PanelInfo || !panel.PanelInfo.PanelTaData.TryGetTabUnit(nTabIndex,out unit))
        {
            Engine.Utility.Log.Error("Panel LocalInfo Error id:{0}!",panel.PanelId);
            return ;
        }
        int openLv = 0;
        if (!DataManager.Manager<GuideManager>().IsTabFuncOpen(unit.FuncID, out openLv))
        {
            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Trailer_Commond_xitongyeqiankaiqi, unit.EnumName, openLv);
            return;
        }
        bool reset = panel.OnTogglePanel(nTabType,nTabIndex);
        if (reset)
        {
            panel.dicActiveTabGrid[nTabType] = nTabIndex;
            Dictionary<int, UITabGrid> dicTabs = null;
            if (panel.dicUITabGrid.TryGetValue(nTabType, out dicTabs))
            {
                foreach (var item in dicTabs)
                {
                    item.Value.SetHightLight(item.Value.TabID == nTabIndex);
                }
            }
        }
    }

    public void OnCilckTogglePanel(PanelID panelId, int nTabType, int nTabIndex)
    {
        UIPanelBase panelBase = UIPManager.GetPanel(panelId);
        if (panelBase == null)
        {
            return;
        }

        OnCilckTogglePanel(ref panelBase, nTabType, nTabIndex);
        //bool reset = panelBase.OnTogglePanel(nTabType, nTabIndex);
        //if (reset)
        //{
        //    panelBase.dicActiveTabGrid[nTabType] = nTabIndex;
        //    Dictionary<int, UITabGrid> dicTabs = null;
        //    if (panelBase.dicUITabGrid.TryGetValue(nTabType, out dicTabs))
        //    {
        //        foreach (var item in dicTabs)
        //        {
        //            item.Value.SetHightLight(item.Value.TabID == nTabIndex);
        //        }
        //    }
        //}
    }

    public void ResetTabs()
    {
        if (rightTabs != null)
        {
            rightTabs.transform.parent = UIRootHelper.Instance.StretchTransRoot;
            rightTabs.SetActive(false);
        }
        m_dicTabFunc.Clear();
    }


    public void OnPropetyClick(ClientMoneyType nType)
    {
        switch (nType)
        {
            case ClientMoneyType.Wenqian:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ExchangeMoneyPanel, data: nType);
                break;
            case ClientMoneyType.YuanBao:
                ReturnBackUIData[] returnData = new ReturnBackUIData[1];
                returnData[0] = new ReturnBackUIData();
                returnData[0].msgid = UIMsgID.eNone;
                returnData[0].panelid = this.m_currPanelID;
                returnData[0].param = null;
                UIPanelBase.PanelJumpData jumpData = new UIPanelBase.PanelJumpData();
                jumpData.Tabs = new int[1];
                jumpData.Tabs[0] = (int)(GameCmd.CommonStore)4;
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MallPanel,jumpData:jumpData);
                break;
            case ClientMoneyType.Gold:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ExchangeMoneyPanel, data:nType);
                break;
            case ClientMoneyType.YinLiang:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ExchangeMoneyPanel, data: nType);
                break;
            default:
                break;
        }
    }


    public void OnEvent(int nEventId,object param)
    {
        if (nEventId == (int)Client.GameEventID.MAINLEFTTBTN_TOGGLE)
        {
            bool show = (bool)param;
            m_bShowRightBtn = show;
            CheckCloseRightBtn();
        }
    }

    public void CheckCloseRightBtn()
    {
        if (!m_bShowRightBtn)
        {
            if (m_fClosePanelTime > 0f)
            {
                Engine.Utility.TimerAxis.Instance().KillTimer(TIMERID, this);
                m_fClosePanelTime = 0f;
            }
            return;
        }
        if (m_fClosePanelTime > 0f)
        {
            m_fClosePanelTime = UnityEngine.Time.realtimeSinceStartup;
            return;
        }
        m_fClosePanelTime = UnityEngine.Time.realtimeSinceStartup;
        Engine.Utility.TimerAxis.Instance().SetTimer(TIMERID, 1000, this);
    }

    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == TIMERID)
        {
            if (UnityEngine.Time.realtimeSinceStartup - m_fClosePanelTime > m_fCloseRightBtnTime)
            {
                m_fClosePanelTime = 0f;
                Engine.Utility.TimerAxis.Instance().KillTimer(TIMERID, this);
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINBTN_ONTOGGLE, new Client.stMainBtnSet() { isShow = false, pos = 1 });

            }
        }
    }
}