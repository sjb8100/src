using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


class UIShortcutAbleGrid : UIItemInfoGridBase, IUIItemInfoGrid
{
    #region property

    //格子数据
    private BaseItem data;
    public BaseItem Data
    {
        get
        {
            return data;
        }
    }

    #endregion


    #region override Method
    protected override void OnAwake()
    {
        base.OnAwake();
       


        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));


    }





    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        bool empty = (null == data);
        ResetInfoGrid(!empty);
        if (empty)
        {
            this.data = null;
            return;
        }
        this.data = data as BaseItem;
        SetIcon(true, this.data.Icon);
        SetBorder(true, this.data.BorderIcon);
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(this.data.BaseId);//道具存量
        bool enable = itemCount > 1;
        SetNum(true, itemCount.ToString());
        SetBindMask(this.data.IsBind);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void Reset()
    {
        base.Reset();
        data = null;
        //location = 0;
        SetNum(false);
    }

    protected override void OnUIBaseDestroy()
    {
        base.OnUIBaseDestroy();
        UnRegisterUIEventDelegate();
    }
    #endregion


}

