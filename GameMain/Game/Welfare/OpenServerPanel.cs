using System;
using Client;
using UnityEngine;
using table;
using System.Collections.Generic;


partial class OpenServerPanel
{
    WelfareManager m_dataManager = null;
    List<OpenServerDataBase> tableList = null;
    List<uint> m_lst_dataIDs = null;
    uint selectID = 1;
    #region override
    protected override void OnLoading()
    {
        base.OnLoading();
        m_dataManager = DataManager.Manager<WelfareManager>();
        tableList = GameTableManager.Instance.GetTableList<OpenServerDataBase>();
        m_lst_dataIDs = new List<uint>();
        for (int i = 0; i < tableList.Count;i++ )
        {
            m_lst_dataIDs.Add(tableList[i].ID);
        }
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_dataManager.ValueUpdateEvent += OnUpdateList;
        RefreshUI();
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
//         tableList.Clear();
//         m_lst_dataIDs.Clear();
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        m_dataManager.ValueUpdateEvent -= OnUpdateList;
        if (m_ctor_dayRoot != null)
        {
            m_ctor_dayRoot.Release(depthRelease);
        }
    }
    #endregion

    void OnUpdateList(object obj, ValueUpdateEventArgs value)
    {
        if (value.key.Equals("OnUpdateOpenPackage"))
        {
            RefreshUI();
        }
    }
    void RefreshUI() 
    {
        m_ctor_dayRoot.RefreshCheck();
        m_ctor_dayRoot.Initialize<UIOpenServerGrid>(m_trans_UIOpenServerGrid.gameObject,  OnWelfareBtnDataUpdate, OnWelfareBtnUIEvent);
        m_ctor_dayRoot.CreateGrids(tableList.Count);
        List<uint> m_lst_gotIDs = DataManager.Manager<WelfareManager>().OpenPackageList;
        List<uint> list = new List<uint>();
        list.AddRange(m_lst_dataIDs);
        for (int i = 0; i < m_lst_gotIDs.Count;i++ )
        {
            if(list.Contains(m_lst_gotIDs[i]))
            {
               list.Remove(m_lst_gotIDs[i]);
            }
        }
        if (list.Count >0)
        {
            SeclectOpenServerGird(list[0]);
        }
       
    }
    private void OnWelfareBtnDataUpdate(UIGridBase grid, int index)
    {
        if (grid is UIOpenServerGrid)
        {
            UIOpenServerGrid tabGrid = grid as UIOpenServerGrid;
            tabGrid.SetGridData(tableList[index].ID);

        }
    }
    void OnWelfareBtnUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                UIOpenServerGrid grid = data as UIOpenServerGrid;
                if (grid != null)
                {
                    SeclectOpenServerGird(grid.CurrentDay);
                }
                break;
        }
    }

    void SeclectOpenServerGird(uint id)
    {
        UIOpenServerGrid grid = m_ctor_dayRoot.GetGrid<UIOpenServerGrid>(m_lst_dataIDs.IndexOf(selectID));
        if (grid != null)
        {
            grid.SetSeclect(false);
        }
        grid = m_ctor_dayRoot.GetGrid<UIOpenServerGrid>(m_lst_dataIDs.IndexOf(id));
        {
            grid.SetSeclect(true);
        }
        selectID = id;
    }
    void onClick_DetailBtn_Btn(GameObject caster)
    {
        uint ModelID = GameTableManager.Instance.GetGlobalConfig<uint>("OpenServerRewardPetID");
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ShowModelPanel, data: ModelID);
//         PetDataBase _db = GameTableManager.Instance.GetTableItem<PetDataBase>(petID);
//         if (_db == null)
//         {
//             return;
//         }
//         DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PetMarkPanel, panelShowAction: (pb) =>
//         {
//             if (null != pb && pb is PetMarkPanel)
//             {
//                 PetMarkPanel panel = pb as PetMarkPanel;
//                 panel.InitPetDataBase(_db);
//             }
//         });
    }
    void onClick_Colsebtn_Btn(GameObject caster)
    {
        HideSelf();
    }

}
