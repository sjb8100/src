using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using table;
using GameCmd;
public class TransferData : IComparable<TransferData>
{
    public uint tabID = 0;
    public uint btnIndex = 0;
    public bool isNpc = false;
    public string step;
    public uint sortID = 0;
    public int CompareTo(TransferData other)
    {
        int astate = (int)sortID;
        int bstate = (int)other.sortID;
        if (astate < bstate)
        {
            return -1;
        }
        else if (astate > bstate)
        {
            return 1;
        }
        return tabID.CompareTo(other.tabID);
    }
}
partial class NpcDeliverPanel : UIPanelBase
{
   
    LangTalkData m_langTalkData = null;

//     List<string> m_lstTabNames = new List<string>()
//     {
//         "世界","地下","首领"
//     };
    Dictionary<uint, string> m_dic_TabName = new Dictionary<uint, string>();
    List<TransferData> m_lstTransfers = new List<TransferData>();
    List<uint> m_lst_TabType = new List<uint>();
    List<table.TransferDatabase> lstTransmis = null;
    int activedIndex = 0;

    protected override void OnLoading()
    {
        base.OnLoading();
        lstTransmis = GameTableManager.Instance.GetTableList<table.TransferDatabase>();
        for (int i = 0; i < lstTransmis.Count;i++ )
        {
            if (!m_dic_TabName.ContainsKey(lstTransmis[i].type))
            {
                m_dic_TabName.Add(lstTransmis[i].type, lstTransmis[i].strTitle);
            }
        }
    }
    bool isNpcTransfer = false;
    protected override void OnShow(object data)
    {
        base.OnShow(data);     
        m_lstTransfers.Clear();
        m_lst_TabType.Clear();
        if (data is LangTalkData)
        {
            isNpcTransfer = true;
            m_label_name.text = "驿站车夫";
            m_langTalkData = (LangTalkData)data;                  
            for (int i = 0; i < m_langTalkData.buttons.Count; i++)
            {
                uint id = 0;
                uint sortID = 0;
                if (uint.TryParse(m_langTalkData.buttons[i].strBtnName, out id))
                {
                    TransferDatabase transf = GameTableManager.Instance.GetTableItem<TransferDatabase>(id);
                    if (transf != null)
                    {
                        if (!m_lst_TabType.Contains(transf.type))
                        {
                            m_lst_TabType.Add(transf.type);
                        }
                        sortID = transf.sortID;
                    }

                    m_lstTransfers.Add(new TransferData()
                    {
                        btnIndex = m_langTalkData.buttons[i].nindex,
                        tabID = id,
                        isNpc = true,
                        step = m_langTalkData.strStep,
                        sortID = transf.sortID,
                    });

                   
                }           
            }
            if (m_lstTransfers != null)
            {
                m_lstTransfers.Sort();
            }

        }
        else
        {
            isNpcTransfer = false;
            m_label_name.text = "世界地图";
            for (int i = 0; i < lstTransmis.Count; i++)
            {
                if (!m_lst_TabType.Contains(lstTransmis[i].type))
                {
                    m_lst_TabType.Add(lstTransmis[i].type);
                }
            }
            StructDatas();       
        }      
        InitGrid();         
    }

    void StructDatas() 
    {
        m_lstTransfers.Clear();
        for (int i = 0; i < lstTransmis.Count; i++)
        {
            if (activedIndex < m_lst_TabType.Count)
            {
                if (lstTransmis[i].type == m_lst_TabType[activedIndex])
                {
                    m_lstTransfers.Add(new TransferData()
                    {
                        tabID = lstTransmis[i].mapid,
                        isNpc = false,
                        sortID = lstTransmis[i].sortID,
                    });
                }   
            }
             
        }
        if (m_lstTransfers != null)
        {
            m_lstTransfers.Sort();
        }
    }

    void InitGrid() 
    {
        if (null != m_ctor_ScrollView)
        {
            m_ctor_ScrollView.RefreshCheck();
            m_ctor_ScrollView.Initialize<UIDeliverGrid>(m_trans_DeliverGrid.gameObject, OnUpdataGridData, OnGridUIEventDlg);         
            m_ctor_ScrollView.CreateGrids(m_lstTransfers.Count);
        }
        if (null != m_ctor_Right)
        {
            m_ctor_Right.RefreshCheck();
            m_ctor_Right.Initialize<UITabGrid>(m_trans_TabGrid.gameObject, OnUpdataGridData, OnGridUIEventDlg);       
            m_ctor_Right.CreateGrids(m_lst_TabType.Count);
        }
    }

    void OnUpdataGridData(UIGridBase grid, int index)
    {
        if (grid is UIDeliverGrid)
        {
            UIDeliverGrid tab = grid as UIDeliverGrid;
            if (index < m_lstTransfers.Count )
            {
                tab.SetGridData(index);
                tab.SetData(m_lstTransfers[index]);
            }
        }
        if (grid is UITabGrid)
        {
            UITabGrid tab = grid as UITabGrid;
            if (index < m_lst_TabType.Count)
            {
                tab.SetGridData(index);
                if (m_dic_TabName.ContainsKey(m_lst_TabType[index]))
                {
                    tab.SetName(m_dic_TabName[m_lst_TabType[index]]);
                }
                tab.SetHightLight(index == activedIndex);
            }
           
        }
    }
    private void OnGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UIDeliverGrid)
                {
                    UIDeliverGrid grid = data as UIDeliverGrid;
                    if (null != grid)
                    {
                        SetSelectGrid(grid.Index);
                    }
                }
                if(data is UITabGrid)
                {
                    UITabGrid grid = data as UITabGrid;
                    if (null != grid)
                    {
                        SetSelectTab((int)grid.Data);
                    }
                }
                break;


        }
    }
    void SetSelectTab(int index,bool force = false) 
    {
        if (index == activedIndex && !force || isNpcTransfer)
        {
            return;
        }
        UITabGrid tab = m_ctor_Right.GetGrid<UITabGrid>(activedIndex);
        if (tab != null)
        {
            tab.SetHightLight(false);
        }
        activedIndex = index;
        tab = m_ctor_Right.GetGrid<UITabGrid>(index);
        if (tab != null)
        {
            tab.SetHightLight(true);
        }
        StructDatas();
        m_ctor_ScrollView.CreateGrids(m_lstTransfers.Count);
    }
    void SetSelectGrid(int index) 
    {
        if (index < m_lstTransfers.Count)
        {
            TransferData m_data = m_lstTransfers[index];
            if (m_data != null)
            {
                if (m_data.isNpc)
                {
                    TransferDatabase transferdata = GameTableManager.Instance.GetTableItem<TransferDatabase>(m_data.tabID);
                    if (!KHttpDown.Instance().SceneFileExists(transferdata.mapid))
                    {
                        //打开下载界面
                        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
                        HideSelf();
                        return;
                    }

                    NetService.Instance.Send(new stDialogSelectScriptUserCmd_C()
                    {
                        step = m_data.step,
                        dwChose = m_data.btnIndex,
                    });
                }
                else
                {
                    TransferDatabase transferdata = GameTableManager.Instance.GetTableItem<TransferDatabase>(m_data.tabID);
                    if (transferdata == null)
                    {
                        return;
                    }
                    string strMsg = string.Format("是否花费 [ff0000]{0}{1}[-] 传送到{2}", transferdata.costValue, ((ClientMoneyType)transferdata.costType).GetEnumDescription(), transferdata.strTransmitMap);
                    TipsManager.Instance.ShowTipWindow(Client.TipWindowType.CancelOk, strMsg, delegate()
                    {
                        DataManager.Manager<TeamDataManager>().TeamMemberCheckAndCancelFollow();//取消队员跟随

                        Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
                        if (cs != null)
                        {
                            cs.GetCombatRobot().Stop();// 停止挂机
                        }


                        ClientMoneyType montype = (ClientMoneyType)transferdata.costType;
                        if (MainPlayerHelper.IsHasEnoughMoney(montype, transferdata.costValue))
                        {
                            Client.IMapSystem mapsys = Client.ClientGlobal.Instance().GetMapSystem();
                            if (mapsys != null)
                            {

                                if (mapsys.GetMapID() == transferdata.mapid)
                                {
                                    //你就在这个场景
                                    TipsManager.Instance.ShowTipsById(514);
                                    return;
                                }

                                //httpdown
                                if (!KHttpDown.Instance().SceneFileExists(transferdata.mapid))
                                {
                                    //打开下载界面
                                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
                                    return;
                                }


                                mapsys.RequestEnterMap(transferdata.mapid, 1);
                                HideSelf();
                            }
                        }
                    });
                }
            }
        }
    }
    #region overridemethod

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if(m_ctor_Right != null)
        {
            m_ctor_Right.Release(depthRelease);
        }
        if(m_ctor_ScrollView != null)
        {
            m_ctor_ScrollView.Release(depthRelease);
        }
        activedIndex = 0;
    }

    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }


    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        Release();
    }



    #endregion

    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }
}
