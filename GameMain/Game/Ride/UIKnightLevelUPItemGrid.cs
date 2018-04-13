
//*************************************************************************
//	创建日期:	2018/2/1 星期四 16:39:03
//	文件名称:	UIKnightLevelUPItemGrid
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

class UIKnightLevelUPItemGrid:UIItemInfoGrid
{
    public uint ItemID
    {
        get;
        set;
    }
    public uint ItemNum
    {
        get;
        set;
    }
    public bool IsBreak = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        RegisterGlobalUIEvent(true);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        RegisterGlobalUIEvent(false);
    }
    void RegisterGlobalUIEvent(bool register = true)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
        }
    }
    private void OnGlobalUIEventHandler(int eventType, object data)
    {
        switch (eventType)
        {

            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                OnUpdateItem((ItemDefine.UpdateItemPassData)data);
              
                break;
        }
    }
    void OnUpdateItem(ItemDefine.UpdateItemPassData passData)
    {
        if (null == passData)
        {
            return;
        }
       if(passData.BaseId == ItemID)
       {
           int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(ItemID);
           SetNum(true, num.ToString());
       }
    }
    uint m_needNum = 0;
    public void SetGridInfo( string ItemID,uint needNum = 1)
    {
        m_needNum = needNum;
        UIKnightLevelUPItemGrid grid = this;
        uint id = 0;
        if (uint.TryParse(ItemID, out id))
        {
            ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>(id);
            if (db != null)
            {
                grid.Init();
                grid.SetIcon(true, db.itemIcon);
                int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(id);
                string numstr = StringUtil.GetNumNeedString(num,needNum);
                grid.SetNum(true, numstr);
                string name = ItemDefine.GetItemBorderIcon(db.quality);
                grid.SetBorder(true, name);
                grid.ItemID = id;
                grid.ItemNum = (uint)num;
                grid.RegisterUIEventDelegate(UIItemInfoEventDelegate);
                //获取途径
                if (num < needNum)
                {
                    grid.SetNotEnoughGet(true);
                }
                else
                {
                    grid.SetNotEnoughGet(false);
                }
            }
        }
    }
    void UIItemInfoEventDelegate(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            if (data is UIKnightLevelUPItemGrid)
            {
                UIKnightLevelUPItemGrid grid = data as UIKnightLevelUPItemGrid;
                if (grid != null)
                {
                    if (grid.ItemNum < m_needNum)
                    {
                        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: grid.ItemID);
                    }
                    else
                    {
                        if (grid.IsBreak)
                        {
                            //stAddKnightRankRideUserCmd_C cmd = new stAddKnightRankRideUserCmd_C();
                            //NetService.Instance.Send(cmd);
                        }
                        else
                        {
                            stAddKnightExpRideUserCmd_C cmd = new stAddKnightExpRideUserCmd_C();
                            cmd.item = grid.ItemID;
                            cmd.num = 1;
                            NetService.Instance.Send(cmd);
                        }

                    }
                }
            }


        }
    }
}
