
//*************************************************************************
//	创建日期:	2017/5/12 星期五 13:49:41
//	文件名称:	ChangeLinePanel
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class ChangeLinePanel
{
    MapDataManager m_mapData
    {
        get
        {
            return DataManager.Manager<MapDataManager>();
        }
    }
    List<ChangeLineInfo> m_lst_LineInfos = null;
    protected override void OnLoading()
    {
        m_mapData.ValueUpdateEvent += m_mapData_ValueUpdateEvent;
        base.OnLoading();
    }

    protected override void OnPanelBaseDestory()
    {
        m_mapData.ValueUpdateEvent -= m_mapData_ValueUpdateEvent;
        base.OnPanelBaseDestory();
    }
    void m_mapData_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
           if(e.key == MapDispatchEnum.RefreshLineInfo.ToString())
           {
               InitLineScroll();
           }
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
       
        InitLineScroll();
    }
    void OnUpdateUIGrid(UIGridBase grid, int index)
    {
        if (grid is ChangeLineGrid)
        {
            ChangeLineGrid item = grid as ChangeLineGrid;
            if(item != null)
            {
                item.gameObject.SetActive(true);
                if (m_lst_LineInfos != null)
                {
                    item.ShowInfo(m_lst_LineInfos[index]);
                }
               
            }
        }
    }
    protected override void OnHide()
    {
        base.OnHide();

    }
    private void OnUIGridEventDlg(UIEventType eventType, object data, object param)
    {
        if (null == data)
        {
            return;
        }
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (data is ChangeLineGrid)
                    {
                        ChangeLineGrid item = data as ChangeLineGrid;
                        if (item != null)
                        {
                            Client.IPlayer mainPlayer = MainPlayerHelper.GetMainPlayer();
                            if (mainPlayer != null)
                            {
                                mainPlayer.SendMessage(Client.EntityMessage.EntityCommand_StopMove,mainPlayer.GetPos());
                            }
                            stSwitchLineMapScreenUserCmd_C cmd = new stSwitchLineMapScreenUserCmd_C();
                            cmd.line = item.LineNum;
                            NetService.Instance.Send(cmd);

                        }
                        if (m_ctor_LineScrollView != null)
                        {
                            m_ctor_LineScrollView.SetSelect(item);
                        }
                    }

                }
                break;
        }
    }
    void  InitLineScroll()
    {
        GameObject go = m_trans_UIChangeLineGrid.gameObject;
        go.SetActive(false);

        m_ctor_LineScrollView.RefreshCheck();
        m_ctor_LineScrollView.Initialize<ChangeLineGrid>(go, OnUpdateUIGrid, OnUIGridEventDlg);
        if(m_lst_LineInfos == null)
        {
           m_lst_LineInfos = new List<ChangeLineInfo>();
        }
        else
        {
           m_lst_LineInfos.Clear();
        }
        m_lst_LineInfos =  m_mapData.GetLineInfoList();
        int linecount = m_lst_LineInfos.Count;
        m_ctor_LineScrollView.CreateGrids(linecount);
    }
    void onClick_Btn_Close_Btn(GameObject caster)
    {
        HideSelf();
    }
}
