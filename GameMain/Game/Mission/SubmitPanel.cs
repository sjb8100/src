using System;
using System.Collections.Generic;
using UnityEngine;


partial class SubmitPanel : UIPanelBase
{
    GameCmd.stRequestSubmitListScriptUserCmd_CS m_currTaskData;
    List<UIItem> m_lstUIItem = new List<UIItem>();
    ItemDefine.UIItemCommonData m_selectdata;
    protected override void OnLoading()
    {
        base.OnLoading();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_currTaskData = null;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_MAIN_ARROWHIDE, null);

        m_trans_select.gameObject.SetActive(false);
        if (data is GameCmd.stRequestSubmitListScriptUserCmd_CS)
        {
            m_currTaskData = data as GameCmd.stRequestSubmitListScriptUserCmd_CS;
            ShowItemList(m_currTaskData.itemid);
        }
    }


    void ShowItemList(uint itemBaseId)
    {
        List<BaseItem> itemdataList = new List<BaseItem>();

        List<BaseItem> itemdataList1 = DataManager.Manager<ItemManager>().GetItemByBaseId(itemBaseId, GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);
        itemdataList.AddRange(itemdataList1);

        table.ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemBaseId);
        if (itemdb != null && itemdb.EqualsId != 0)
        {
            List<BaseItem> itemdataList2 = DataManager.Manager<ItemManager>().GetItemByBaseId(itemdb.EqualsId, GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);
            itemdataList.AddRange(itemdataList2);
        }


        if (itemdataList.Count <= 0)
        {
            Engine.Utility.Log.Error("背包没有道具{0}", itemBaseId);
        }
        int count = m_lstUIItem.Count;
        while (count > 0)
        {
            m_lstUIItem[0].Release();
            m_lstUIItem.RemoveAt(0);
            count = m_lstUIItem.Count;
        }
        m_lstUIItem.Clear();

        for (int i = 0; i < itemdataList.Count; i++)
        {
            UIItem uiitem = DataManager.Manager<UIManager>().GetUICommonItem(itemBaseId, itemdataList[i].Num, itemdataList[i].QWThisID, OnSelectItem);
            uiitem.Attach(m_grid_root.transform);
            if (uiitem.GetGrid<UIItemCommonGrid>().GetComponent<UIDragScrollView>() == null)
            {
                uiitem.GetGrid<UIItemCommonGrid>().gameObject.AddComponent<UIDragScrollView>();
            }
            //int x = (i % 3) * 85;
            //int y = -(i / 3) * 85;
            //uiitem.SetPosition(true, new Vector3(x, y, 0));
            m_lstUIItem.Add(uiitem);
        }

        if (m_grid_root != null)
        {
            m_grid_root.Reposition();
        }

        if (itemdataList.Count > 0)
        {
            OnSelectItem(m_lstUIItem[0].GetGrid<UIItemCommonGrid>());
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        m_trans_select.transform.parent = m_grid_root.transform;
        m_selectdata = null;
        m_currTaskData = null;

        int count = m_lstUIItem.Count;
        while (count > 0)
        {
            m_lstUIItem[0].Release();
            m_lstUIItem.RemoveAt(0);
            count = m_lstUIItem.Count;
        }
        m_lstUIItem.Clear();
    }

    void OnSelectItem(UIItemCommonGrid grid)
    {
        m_trans_select.transform.parent = grid.transform;
        m_trans_select.transform.localPosition = Vector3.zero;
        m_selectdata = grid.Data;
        m_trans_select.gameObject.SetActive(true);
        // TipsManager.Instance.ShowItemTips(grid.Data.ItemThisId, grid.gameObject, false);
    }

    void onClick_Btn_close_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    void onClick_Btn_submit_Btn(GameObject caster)
    {
        if (m_selectdata == null || m_currTaskData == null)
        {
            TipsManager.Instance.ShowTips("请选择一个物品提交!");
            return;
        }
        NetService.Instance.Send(new GameCmd.stRequestSubmitListScriptUserCmd_CS()
        {
            itemid = m_selectdata.ItemThisId,
            taskid = m_currTaskData.taskid,
            userid = m_currTaskData.userid
        });
        this.HideSelf();
    }
}
