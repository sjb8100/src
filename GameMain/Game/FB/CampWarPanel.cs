//*************************************************************************
//	创建日期:	2016-11-30 16:16
//	文件名称:	CampWarPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	阵营战报名
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

partial class CampWarPanel : UIPanelBase,IGlobalEvent
{
    #region property
    private List<uint> m_lstCampInfosIndex = null;
    CampCombatManager m_dataMgr = null;
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
        m_dataMgr = DataManager.Manager<CampCombatManager>();
        InitWidgets();

    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGlobalEvent(true);
        BuildCampSignInfoList();
        RefreshCampLeftTimes();
    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalEvent(false);
        tempLocalInfo = null;
        Release();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_ctor_SignScrollView != null)
        {
            m_ctor_SignScrollView.Release(depthRelease);
        }
    }

    #endregion

    #region initWidgets
    private void InitWidgets()
    {
        if (null != m_ctor_SignScrollView)
        {
            //GameObject resObj = UIManager.GetResGameObj(GridID.Uicampsignupgrid) as GameObject;
            m_ctor_SignScrollView.Initialize<UICampSignupGrid>((uint)GridID.Uicampsignupgrid, UIManager.OnObjsCreate, UIManager.OnObjsRelease, OnUpdateCameSignGridData, OnGridUIEventDlg);
        }
    }

    private LocalCampSignInfo tempLocalInfo = null;
    private void OnUpdateCameSignGridData(UIGridBase grid, int index)
    {
        if (m_lstCampInfosIndex.Count <= index)
        {
            return;
        }
        tempLocalInfo = m_dataMgr.GetLocalCampSignInfoByIndex(m_lstCampInfosIndex[index]);

        if (null != tempLocalInfo)
        {
            UICampSignupGrid cGrid = grid as UICampSignupGrid;
            cGrid.SetGridData(tempLocalInfo);
        }
    }

    private void OnGridUIEventDlg(UIEventType eventType, object data, object param)
    {

    }
    #endregion

    #region IGlobalEvent

    /// <summary>
    /// 事件注册
    /// </summary>
    /// <param name="regster"></param>
    public void RegisterGlobalEvent(bool regster)
    {
        if (regster)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RECONNECT_SUCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCAMPSIGNINFOSREFRESH, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCAMPSIGNINFOCHANGED, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCAMPLEFTTIMESREFRESH, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.RECONNECT_SUCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCAMPSIGNINFOSREFRESH, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCAMPSIGNINFOCHANGED, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCAMPLEFTTIMESREFRESH, GlobalEventHandler);
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
            case (int)Client.GameEventID.RECONNECT_SUCESS:
                {
                    m_dataMgr.GetSignCampInfo(0);
                }
                break;
            case (int)Client.GameEventID.UIEVENTCAMPLEFTTIMESREFRESH:
                {
                    RefreshCampLeftTimes();
                }
                break;
            case (int)Client.GameEventID.UIEVENTCAMPSIGNINFOSREFRESH:
                {
                    BuildCampSignInfoList();
                    RefreshCampLeftTimes();
                }
                break;
            case (int)Client.GameEventID.UIEVENTCAMPSIGNINFOCHANGED:
                {
                    uint index = (uint)data;
                    if (m_lstCampInfosIndex.Contains(index) && null != m_ctor_SignScrollView)
                    {
                        m_ctor_SignScrollView.UpdateData(m_lstCampInfosIndex.IndexOf(index));
                    }
                }
                break;
        }
    }
    #endregion

    #region Op
    private void BuildCampSignInfoList()
    {
        m_lstCampInfosIndex = m_dataMgr.GetSignCampInfosIndexs();
        int count = (null != m_lstCampInfosIndex) ? m_lstCampInfosIndex.Count : 0;
        if (null != m_ctor_SignScrollView)
        {
            m_ctor_SignScrollView.CreateGrids(count);
        }
    }

    private void RefreshCampLeftTimes()
    {
        if (null != m_label_LvSection)
        {
            m_label_LvSection.text = string.Format("{0}级",m_dataMgr.CampSectionString);
        }
        m_label_LeftNum.text = string.Format("你还能参加 {0} 次", m_dataMgr.CampCopyLeftNum);
       // m_label_LeftNum.text = string.Format("你还能参加 {0} 次", m_dataMgr.LeftJoinTimes);
    }
    #endregion

    void OnModelEventUpdate(object obj ,ModelEventArgs args)
    {
        //if (args.type.Equals("SignUpStatus"))
        //{
        //    if (args.args.Length == 3)
        //    {
        //        for (int i = 0; i < m_lstCampSignup.Count; i++)
        //        {
        //            if (m_lstCampSignup[i].Index == (int)args.args[0])
        //            {
        //                m_lstCampSignup[i].RefreshStatus((GameCmd.eCFState)args.args[1], (bool)args.args[2]);
        //            }
        //        }
        //    }
        //}
        //else if (args.type.Equals("SignUpSignNum"))
        //{
        //    if (args.args.Length == 2)
        //    {
        //        for (int i = 0; i < m_lstCampSignup.Count; i++)
        //        {
        //            if (m_lstCampSignup[i].Index == (int)args.args[0])
        //            {
        //                m_lstCampSignup[i].RefreshNum((uint)args.args[1]);
        //            }
        //        }
        //    }
        //}
        //else if (args.type.Equals("LeftJoinTimes"))
        //{
        //    m_label_LeftNum.text = string.Format("你还能参加 {0} 次", m_dataMgr.LeftJoinTimes);
        //}
    }

    #region UIEvent

    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    void onClick_BtnRefresh_Btn(GameObject caster)
    {
        DataManager.Manager<CampCombatManager>().GetSignCampInfo(0);
    }
    #endregion
}