using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

class UITitleActivatePropGrid : UIGridBase
{

    UILabel des;
    UILabel num;
    uint m_effectId;

    protected override void OnAwake()
    {
        base.OnAwake();
        des = this.transform.Find("des").GetComponent<UILabel>();
        num = this.transform.Find("num").GetComponent<UILabel>();
    }


    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.m_effectId = (uint)data;

        Init(this.m_effectId);
    }

    public void Init(uint effectId)
    {
        this.m_effectId = effectId;

        StateDataBase db = GameTableManager.Instance.GetTableItem<StateDataBase>(effectId);

        if (db == null)
        {
            return;
        }
        num.text = string.Format("{0}", db.param1);

        uint id = db.typeid * 100;
        StateDataBase desDb = GameTableManager.Instance.GetTableItem<StateDataBase>(id);
        if (desDb == null)
        {
            return;
        }

        des.text = string.Format("{0}", desDb.name);
    }
}

