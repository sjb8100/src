//*******************************************************************************************
//	创建日期：	2017-8-23   16:40
//	文件名称：	CityWarFightingPanel,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	ZQB
//	说    明：	城战顶部栏grid
//*******************************************************************************************


using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class CityWarFightingPanel : UIPanelBase
{

    CityWarManager m_cityWarManger = null;

    List<UICityWarFightTotemGrid> m_lstGrid = new List<UICityWarFightTotemGrid>();

    List<CityWarTotem> m_lstCityWarTotem = null;  //

    #region override


    protected override void OnLoading()
    {
        base.OnLoading();
        m_cityWarManger = DataManager.Manager<CityWarManager>();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.MAINRIGHTBTN_TOGGLE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.MAINLEFTTBTN_TOGGLE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CITYWARTOTEMCLANNAMECHANGE, OnEvent);


        MainPanel panel = DataManager.Manager<UIPanelManager>().GetPanel<MainPanel>(PanelID.MainPanel);
        if (panel != null)
        {
            bool show = !panel.IsShowRightBtn();
            m_trans_CityWarWarContent.gameObject.SetActive(show);

            //DataManager.Manager<CityWarManager>().ReqCityWarInfo(!panel.IsShowRightBtn());
        }

        UpdateTotemGrid();
    }


    protected override void OnHide()
    {
        base.OnHide();

        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.MAINRIGHTBTN_TOGGLE, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.MAINLEFTTBTN_TOGGLE, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CITYWARTOTEMCLANNAMECHANGE, OnEvent);
    }

    #endregion

    void OnEvent(int nEventid, object param)
    {

        if ((int)Client.GameEventID.MAINRIGHTBTN_TOGGLE == nEventid | (int)Client.GameEventID.MAINLEFTTBTN_TOGGLE == nEventid)
        {
            bool show = (bool)param;
            m_trans_CityWarWarContent.gameObject.SetActive(!show);

            //DataManager.Manager<CityWarManager>().ReqCityWarInfo(!show);
        }
        else if ((int)Client.GameEventID.CITYWARTOTEMCLANNAMECHANGE == nEventid)
        {
            UpdateTotemGrid();
        }
    }


    /// <summary>
    /// 更新grid
    /// </summary>
    private void UpdateTotemGrid()
    {
        //图腾列表
        m_lstCityWarTotem = m_cityWarManger.CityWarTotemList;

        for (int i = 0; i < m_grid_TotemGridRoot.transform.childCount; i++)
        {
            Transform ts = m_grid_TotemGridRoot.transform.GetChild(i);
            UICityWarFightTotemGrid grid = ts.gameObject.GetComponent<UICityWarFightTotemGrid>();
            if (grid == null)
            {
                grid = ts.gameObject.AddComponent<UICityWarFightTotemGrid>();
            }

            grid.SetGridData(m_lstCityWarTotem[i]);
            grid.RegisterUIEventDelegate(OnTotemGridUIEvent);
            grid.gameObject.SetActive(true);

            if (m_lstCityWarTotem[i].clanId != 0)
            {
                grid.SetClanName(m_lstCityWarTotem[i].clanName);

            }
            else
            {
                grid.SetClanName("中立");
            }
        }
    }


    private void OnTotemGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UICityWarFightTotemGrid grid = data as UICityWarFightTotemGrid;
            if (grid == null)
            {
                return;
            }

            IMapSystem mapSys = ClientGlobal.Instance().GetMapSystem();

            IController ctrl = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl();
            if (ctrl != null && mapSys != null)
            {
                ctrl.MoveToTarget(new UnityEngine.Vector3(grid.CityWarTotemData.pos.x, 0, -grid.CityWarTotemData.pos.y));
            }
        }
    }

    /// <summary>
    /// 是否打开了城战
    /// </summary>
    /// <returns></returns>
    public bool IsShowCityWar()
    {
        return m_trans_CityWarWarContent.gameObject.activeSelf;
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eCityWarInfoUpdate)
        {
            UpdateTotemGrid();
        }

        return true;
    }
}

