using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

class UITitlePropAddGrid : UIGridBase
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

    /// <summary>
    /// 总加成用
    /// </summary>
    public void InitAllAddGrid(uint effectTypeId, int eachAllAdd)
    {
        this.m_effectId = (uint)effectTypeId;

        uint id = effectTypeId * 100;
        StateDataBase desDb = GameTableManager.Instance.GetTableItem<StateDataBase>(id);

        if (desDb != null)
        {
            des.text = string.Format("{0}", desDb.name);
            //des.text = string.Format("{0}:{1}+{2}", db.name, desDb.name,eachAllAdd);
            //des.text = string.Format("{0}+{1}", desDb.name, eachAllAdd);
        }

        num.text = string.Format("{0}", eachAllAdd);
    }
}

