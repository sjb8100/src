using System;
using System.Collections.Generic;
using UnityEngine;

public class UIItemShow : UIItemInfoGridBase
{
    UISpriteEx m_spriteQualityBg = null;
    UISprite m_spriteIcon = null;
    UILabel m_labelName = null;
    UILabel m_labelNum = null;
    //uint m_itemid;

   public MissionRewardItemInfo item;

    protected override void OnAwake()
    {
 	    base.OnAwake();
        m_spriteQualityBg = transform.Find("QulityBg").GetComponent<UISpriteEx>();
        m_spriteIcon = transform.Find("Icon").GetComponent<UISprite>();
        m_labelName = transform.Find("Name").GetComponent<UILabel>();
        m_labelNum = transform.Find("num").GetComponent<UILabel>();
        UIEventListener.Get(CacheTransform.Find("ItemRoot/InfoGrid").gameObject).onClick = OnShowTips;

        InitItemInfoGrid(CacheTransform.Find("ItemRoot/InfoGrid"));
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.item = data as MissionRewardItemInfo;

        if (this.item == null)
        {
            return;
        }

        ResetInfoGrid(true);

        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(this.item.itemBaseId);

        SetIcon(true, baseItem.Icon);
        SetBorder(true, baseItem.BorderIcon);
        SetNum(true, this.item.num.ToString());
        SetName(baseItem.Name);

        //ShowWithItemIdNum(this.item.itemBaseId, this.item.num);

    }

    void OnShowTips(GameObject go)
    {
        if (this.item.itemBaseId == MainPlayerHelper.ExpID || this.item.itemBaseId == MainPlayerHelper.CoinID ||
            MainPlayerHelper.GoldID == this.item.itemBaseId)
        {
            return;
        }
        TipsManager.Instance.ShowItemTips(this.item.itemBaseId, gameObject, false);
    }

    /*
    public void ShowWithItemIdNum(uint itemId,uint num,bool showName = true)
    {
        table.ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemId);
        m_itemid = 0;
        if (itemdb != null)
        {
            m_itemid = itemId;

            UIAtlas atlas = DataManager.Manager<UIManager>().GetAtlasByIconName(itemdb.itemIcon);
            if (m_spriteIcon != null)
            {
                m_spriteIcon.atlas = atlas;
                m_spriteIcon.spriteName = itemdb.itemIcon;
                m_spriteIcon.MakePixelPerfect();
            }
            if (m_spriteQualityBg != null)
            {
                m_spriteQualityBg.ChangeSprite(itemdb.quality);
                m_spriteQualityBg.MakePixelPerfect();
            }
            if (m_labelName != null)
            {
                m_labelName.text = itemdb.itemName;
                if (num < 10000)
                {
                    m_labelNum.text = num.ToString();
                }
                else
                {
                    string Num = string.Format((Math.Round((num/ 10000.0), 2)).ToString("g0") + "万");
                    m_labelNum.text = Num;
                }
                m_labelNum.text = num.ToString();
                m_labelName.gameObject.SetActive(showName);   
            }
                   
        } 
    }
     */ 
     
    public void SetName(string name = "", bool showName = true)
    {
        if (m_labelName != null)
        {
            m_labelName.text = name;
            m_labelName.gameObject.SetActive(showName); 
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(false);
        }
        item = null;
    }
}
