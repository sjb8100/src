using System;
using System.Collections.Generic;
using UnityEngine;

public class UICommonUseItemGrid :UIGridBase
{
    UILabel m_labelName = null;
    UILabel m_labelDesc = null;
    Transform m_tranIcon = null;
    UILabel m_labelUseTimes = null;

    UseItemCommonPanel.UseItemData m_useItemData;
    public UseItemCommonPanel.UseItemData Data { get { return m_useItemData; } }

    protected override void OnAwake()
    {
        m_labelName = transform.Find("name").GetComponent<UILabel>();
        m_labelDesc = transform.Find("desc").GetComponent<UILabel>();
        m_tranIcon = transform.Find("iconRoot");
        m_labelUseTimes = transform.Find("Times").GetComponent<UILabel>();
    }


    void OnGet(GameObject go)
    {
        if (m_useItemData != null)
        {
            m_useItemData.parent.OnGetWay(m_useItemData.itemid);
        }
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        if (data is UseItemCommonPanel.UseItemData)
        {
            m_useItemData = (UseItemCommonPanel.UseItemData)data;

            table.ItemDataBase itemTb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(m_useItemData.itemid);
            if (itemTb != null)
            {
                m_useItemData.maxuseNum = itemTb.maxUseTimes;

                if (m_labelName != null)
                {
                    m_labelName.text = itemTb.itemName;
                }

                if (m_labelDesc != null)
                {
                    m_labelDesc.text = itemTb.description;
                }

                if (itemTb.maxUseTimes != 0)
                {
                    if (m_labelUseTimes != null)
                    {
                        m_labelUseTimes.gameObject.SetActive(true);
                        m_labelUseTimes.text = string.Format("{0}/{1}",m_useItemData.useNum, itemTb.maxUseTimes);
                    }
                }
                else
                {
                    if (m_labelUseTimes != null)
                    {
                        m_labelUseTimes.gameObject.SetActive(false);
                    }
                }
                RefreshItemNum();
            }
        }
    }

    public void RefreshItemNum()
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_useItemData.itemid);

        if (m_tranIcon != null)
        {
            m_tranIcon.DestroyChildren();
            if (itemCount > 0)
            {
                UIItem.AttachParent(m_tranIcon, m_useItemData.itemid, (uint)itemCount);
            }
            else
            {
                UIItem.AttachParent(m_tranIcon, m_useItemData.itemid, (uint)itemCount, (grid) =>
                {
                    OnGet(null);
                });
            }
        }

    }

    public void RefreshItemNumByClientData(int itemNum)
    {
        if (m_tranIcon != null)
        {
            if (itemNum > 0)
            {
                UIItem.AttachParent(m_tranIcon, m_useItemData.itemid, (uint)itemNum);
            }
        }
    }
}