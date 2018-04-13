using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class CityWarSelectPanel
{

    #region property
    private UIGridCreatorBase m_cityInfoGridCreator = null;

    List<table.CityWarDataBase> m_cityWarCopyList;

    #endregion

    #region override

    protected override void OnLoading()
    {
        base.OnLoading();

        InitWidget();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        CreateGrid();
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        //if (msgid == UIMsgID.eNvWaNewWave)
        //{

        //}


        return true;
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_cityInfoGridCreator != null)
        {
            m_cityInfoGridCreator.Release(depthRelease);
        }
    }

    #endregion



    void InitWidget()
    {
        m_cityInfoGridCreator = m_trans_SelectCityWarContent.GetComponent<UIGridCreatorBase>();
        if (m_cityInfoGridCreator == null)
        {
            m_cityInfoGridCreator = m_trans_SelectCityWarContent.gameObject.AddComponent<UIGridCreatorBase>();
        }

        if (m_cityInfoGridCreator != null)
        {
            //UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uiteammaintargetgrid) as UnityEngine.GameObject;
            m_cityInfoGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            m_cityInfoGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            m_cityInfoGridCreator.gridWidth = 363;
            m_cityInfoGridCreator.gridHeight = 538;

            m_cityInfoGridCreator.RefreshCheck();
            m_cityInfoGridCreator.Initialize<UICityWarCityInfoGrid>(m_widget_CityWarGrid.gameObject, OnGridDataUpdate, OnGridUIEvent);
        }
    }

    void CreateGrid()
    {
        m_cityWarCopyList = DataManager.Manager<CityWarManager>().GetCityWarCopyListWithoutHuaXia();

        if (m_cityInfoGridCreator != null)
        {
            m_cityInfoGridCreator.CreateGrids(m_cityWarCopyList != null ? m_cityWarCopyList.Count : 0);
        }
    }

    private void OnGridDataUpdate(UIGridBase data, int index)
    {
        if (data is UICityWarCityInfoGrid)
        {
            if (m_cityWarCopyList != null && m_cityWarCopyList.Count > index)
            {
                UICityWarCityInfoGrid grid = data as UICityWarCityInfoGrid;
                if (grid == null)
                {
                    return;
                }

                grid.SetGridData(m_cityWarCopyList[index].CopyId);
                grid.SetBg(m_cityWarCopyList[index].Bg);
                grid.SetName(m_cityWarCopyList[index].Name);
                grid.SetDay(m_cityWarCopyList[index].OpenTime1);
                grid.SetTime(m_cityWarCopyList[index].OpenTime2);
            }
        }
    }

    private void OnGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UICityWarCityInfoGrid grid = data as UICityWarCityInfoGrid;
            if (grid == null)
            {
                return;
            }

            uint copyId = grid.CopyId;

            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CityWarRegisterPanel, data: copyId);
        }
    }



    #region click

    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }
    #endregion
}

