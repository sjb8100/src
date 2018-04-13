//*************************************************************************
//	创建日期:	2016-12-30 10:57
//	文件名称:	RideFeeditem.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	坐骑自动喂食选择界面
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

public class RideFeeditem : MonoBehaviour
{
    GameObject iconGo;
    UILabel lableName;
    UILabel lableDes;
    GameObject m_goChoose = null;
    uint m_itemid;
    public uint ItemId { get { return m_itemid; } }

    UIItem uiitem;
    Action<RideFeeditem> m_callback = null;
    void Awake()
    {
        iconGo = transform.Find("icon").gameObject;
        lableName = transform.Find("name").GetComponent<UILabel>();
        lableDes = transform.Find("effect").GetComponent<UILabel>();
        m_goChoose = transform.Find("choose").gameObject;

        UIEventListener.Get(gameObject).onClick = OnClickItem;
    }

    void OnClickItem(GameObject go)
    {
        if (m_callback != null)
        {
            m_callback(this);
        }
    }

    public void InitUI(uint itemid, Action<RideFeeditem> callback)
    {
        m_itemid = itemid;
        m_callback = callback;
        if (uiitem != null)
        {
            uiitem.Release();
            uiitem = null;
        }
        SetState(false);

        table.ItemDataBase itemdata = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemid);
        if (itemdata != null)
        {
            uint itemNum = (uint)DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemid);
            uiitem = DataManager.Manager<UIManager>().GetUICommonItem(itemid, itemNum);
            if (uiitem != null && iconGo != null)
                uiitem.Attach(iconGo.transform);
            

            if (lableName != null)
                lableName.text = itemdata.itemName;
            if (lableDes != null)
                lableDes.text = itemdata.description;
        }
    }

    public void SetState(bool active)
    {
        if (m_goChoose != null)
        {
            m_goChoose.SetActive(active);
        }
    }

}
