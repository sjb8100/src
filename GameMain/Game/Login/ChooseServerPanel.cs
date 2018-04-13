//*************************************************************************
//	创建日期:	2017-5-19 15:58
//	文件名称:	ChooseServerPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	服务器列表选择
//*************************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;

//选服
public partial class ChooseServerPanel : UIPanelBase
{
    #region property
    //服务器页索引数据
    private int m_pages = 0;
    //当前选中的页
    private int m_cur_page_index = -1;

    private LoginDataManager m_ldMgr = null;
    public LoginDataManager Mgr
    {
        get
        {
            return m_ldMgr;
        }
    }

    private UIServerListGrid m_MyServerListGrid = null;
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        m_ldMgr = DataManager.Manager<LoginDataManager>();
        GameObject lastNode = NGUITools.AddChild(m_trans_topRoot.gameObject, m_sprite_SeverItemPrefab.gameObject);
        lastNode.SetActive(true);
        lastNode.transform.parent = m_trans_topRoot;
        lastNode.transform.localPosition = Vector3.zero;
        lastNode.transform.localScale = Vector3.one;
        lastNode.name = "-1";
        m_MyServerListGrid = lastNode.AddComponent<UIServerListGrid>();
        m_MyServerListGrid.RegisterUIEventDelegate(OnGridUIEvent);
        m_MyServerListGrid.isLastZone = true;
        InitWidgets();
    }

    protected override void OnShow(object data)
    {
        //获取服务器区服数据
        m_pages = m_ldMgr.GetAreaServerPagesCount();
        if (m_pages == 0)
        {
            Engine.Utility.Log.Error("ChooseServerPanel-> 获取服务器区服数据错误!");
            return;
        }
        m_cur_page_index = 0;
        if (null != m_ctor_LeftPanel)
        {
            m_ctor_LeftPanel.CreateGrids(m_pages);
        }
        ShowAreaServerData();
    }

    protected override void OnHide()
    {
        base.OnHide();
        m_cur_page_index = -1;
        if (null != m_ctor_LeftPanel)
        {
            m_ctor_LeftPanel.Release();
        }

        if (null != m_ctor_RightPanel)
        {
            m_ctor_RightPanel.Release();
        }

        if (null != m_ctor_ServerlistPanel)
        {
            m_ctor_ServerlistPanel.Release();
        }
    }
    #endregion



    #region InitWidgets
    private void InitWidgets()
    {
        if (null != m_sprite_SeverItemPrefab)
        {
            if (null != m_ctor_RightPanel)
            {
                m_ctor_RightPanel.RefreshCheck();
                m_ctor_RightPanel.Initialize<UIServerListGrid>(m_sprite_SeverItemPrefab.gameObject
                    , OnGridDataUpdate
                    , OnGridUIEvent);
            }

            if (null != m_ctor_ServerlistPanel)
            {
                m_ctor_ServerlistPanel.RefreshCheck();
                m_ctor_ServerlistPanel.Initialize<UIServerListGrid>(m_sprite_SeverItemPrefab.gameObject
                    , OnGridDataUpdate
                    , OnGridUIEvent);
            }

            m_sprite_SeverItemPrefab.transform.localPosition = new UnityEngine.Vector3(1000, 10000, 0);
        }


        if (null != m_sprite_page_btnPrefab)
        {
            if (null != m_ctor_LeftPanel)
            {
                m_ctor_LeftPanel.RefreshCheck();
                m_ctor_LeftPanel.Initialize<UIZonePageGrid>(m_sprite_page_btnPrefab.gameObject
                    , OnGridDataUpdate
                    , OnGridUIEvent);
            }

            m_sprite_page_btnPrefab.transform.localPosition = new UnityEngine.Vector3(1000, 10000, 0);
        }
    }
    #endregion

    #region Op
    /// <summary>
    /// 显示区服
    /// </summary>
    private void ShowAreaServerData()
    {
        if (m_pages != 0 
            && m_cur_page_index >= 0
            && m_cur_page_index < m_pages)
        {
            AreaServerPageData pageData = m_ldMgr.GetPageData(m_cur_page_index);
            if (null == pageData)
            {
                return;
            }
            if (pageData.Recommond)
            {
                m_trans_recommond.localPosition = Vector3.zero;
                m_trans_ServerlistRoot.localPosition = new Vector3(10000, 0, 0);

                SetMyZone();
                if (m_ctor_RightPanel != null)
                {
                    m_ctor_RightPanel.CreateGrids(pageData.Count);
                }
            }else
            {
                m_trans_recommond.localPosition = new Vector3(10000, 0, 0);

                m_trans_ServerlistRoot.localPosition = Vector3.zero;

                if (m_ctor_ServerlistPanel != null)
                {
                    m_ctor_ServerlistPanel.CreateGrids(pageData.Count);
                }
            }
        }
    }
    #endregion

    #region UI

    void OnGridDataUpdate(UIGridBase data, int index)
    {
        if (m_pages != 0
              && m_cur_page_index >= 0
              && m_cur_page_index < m_pages)
        {
            AreaServerPageData pageData = null;
            if (data is UIServerListGrid)
            {
                UIServerListGrid listGrid = data as UIServerListGrid;
                pageData = m_ldMgr.GetPageData(m_cur_page_index);
                if (null == pageData)
                    return;
                LoginDataManager.EnableZoneInfo enableZoneInfo = null;
                if (pageData.TryGetEanbleZoneInfo(index,out enableZoneInfo))
                {
                    listGrid.SetServerListGridData(m_ldMgr.GetZoneInfoByIndex(enableZoneInfo.Index), enableZoneInfo.Index);
                }
            }else if (data is UIZonePageGrid)
            {
                pageData = m_ldMgr.GetPageData(index);
                if (null == pageData)
                    return;
                UIZonePageGrid zoneGrid = data as UIZonePageGrid;
                if (null != zoneGrid)
                {
                    bool active = (m_cur_page_index == index);
                    zoneGrid.SetServerPageData(pageData.PageName, index, active);
                }
            }
        }
    }

    void OnGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                
                if (data is UIServerListGrid)
                {
                    UIServerListGrid serverGrid = data as UIServerListGrid;
                    if (serverGrid != null)
                    {
                        HideSelf();
                        Mgr.SetCurSelectZoneIndex(serverGrid.Index);
                        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.LoginPanel, data: LoginPanel.ShowUIEnum.StartGame);
                    }
                }
                else if (data is UIZonePageGrid)
                {
                    UIZonePageGrid zoneGrid = data as UIZonePageGrid;
                    if (zoneGrid != null)
                    {
                        if (m_cur_page_index != zoneGrid.Index)
                        {
                            m_cur_page_index = zoneGrid.Index;
                            m_ctor_LeftPanel.SetSelect(zoneGrid);
                            ShowAreaServerData();
                        }
                    }
                }
                break;
        }
    }
    #endregion

    /// <summary>
    /// 设置我的区服  目前只显示一个
    /// </summary>
    private void SetMyZone()
    {
        if (m_MyServerListGrid != null)
        {
            bool currentAreaServerEnable = Mgr.CurAreaServerEnable;
            if (m_MyServerListGrid.gameObject.activeSelf != currentAreaServerEnable)
                m_MyServerListGrid.gameObject.SetActive(currentAreaServerEnable);
            if (currentAreaServerEnable)
            {
                m_MyServerListGrid.SetServerListGridData(Mgr.GetZoneInfo(),Mgr.CurrentSelectZoneIndex);
            }
        }
    }

    

    void onClick_CloseBtn_Btn(GameObject caster)
    {
        HideSelf();
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.LoginPanel, data: LoginPanel.ShowUIEnum.StartGame);
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eServerListStateRefresh)
        {
            ShowAreaServerData();
        }
        return true;
    }
}
