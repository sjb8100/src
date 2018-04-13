using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class UIItem : UIItemBase<UIItemCommonGrid>
{
    #region Property
    public BaseItem Data
    {
        get
        {
            return null;
            //return (Ready) ? DataManager.Manager<ItemManager>().GetItemDataById(GetGrid().Data.QWThisID) : null;
        }

    }
    #endregion

    public void RefreshUI(uint nBaseId,uint nThisId = 0 ,uint num = 0)
    {
        table.ItemDataBase db = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(nBaseId);
        if (db == null)
        {
            Engine.Utility.Log.Error("Not Found Item By ID:{0}", nBaseId);
            return ;
        }

        UIItemCommonGrid grid = this.GetGrid<UIItemCommonGrid>();
        if (grid == null)
        {
            Engine.Utility.Log.Error("Get UIItemCommonGrid error ");
            return;
        }
        grid.SetGridData(new ItemDefine.UIItemCommonData()
        {
            DwObjectId = nBaseId,
            IconName = db.itemIcon,
            Num = num,
            ItemThisId = nThisId,
            Qulity = db.quality,
            ShowGetWay = false,
        });
    }

    /// <summary>
    /// 添加 或者刷新item  此方法 默认刷新需要的图片在一个图集里面
    /// </summary>
    /// <param name="widget">父节点</param>
    /// <param name="itemName">名字</param>
    /// <param name="itemID">id</param>
    /// <param name="itemNum">数量 默认为0 会自己获取数量</param>
    static public void AttachParent(Transform widget, uint itemID, uint itemNum = 0, Action<UIItemCommonGrid> callback = null,bool showGetWay = true,uint needNum = 1)
    {
        UIManager uiMan = DataManager.Manager<UIManager>();
        string itemName = "ItemGrid" ;
        if(itemNum == 0)
        {
           itemNum = (uint)DataManager.Manager<ItemManager>().GetItemNumByBaseId( itemID );
        }
        ItemDefine.UIItemCommonData data = new ItemDefine.UIItemCommonData();
        table.ItemDataBase db = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemID);
        if (db != null)
        {
            data.IconName = db.itemIcon;
            data.Num = (uint)itemNum;
            data.DwObjectId = itemID;
            data.Qulity = db.quality;
            data.ShowGetWay = showGetWay;
            data.NeedNum = needNum;
        }
        Transform itemTrans = widget.Find( itemName );
        if(itemTrans != null)
        {
            UIItemCommonGrid grid = itemTrans.GetComponent<UIItemCommonGrid>();
            if(grid != null)
            {
                UIItem item = new UIItem( grid );
                
                item.SetGridData( data );
            }
            else
            {
                UIItem item = uiMan.GetUICommonItem(itemID, itemNum, 0, callback);
                if ( item != null )
                {
                    item.Attach( widget ,itemName);
                    item.SetGridData(data);
                }
            }
            
        }
        else
        {
            UIItem item = uiMan.GetUICommonItem( itemID , itemNum, 0, callback);
            if ( item != null )
            {
                item.Attach( widget , itemName );
                item.SetGridData(data);
            }
        }
       
    }
    #region StuctMethod
    public UIItem(UIItemCommonGrid grid)
        : base(grid)
    {
        

    }
    #endregion
   
    #region OverrideMethod
    public override void Release()
    {
        base.Release();
    }
    #endregion


    
}