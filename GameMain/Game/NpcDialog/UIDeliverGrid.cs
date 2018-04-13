using System;
using System.Collections.Generic;
using UnityEngine;
using table;
class UIDeliverGrid : UIGridBase
{
    UILabel m_lableName = null;
    UILabel m_lableCost = null;
    UISprite m_spriteIcon = null;
    UITexture m_spriteBg = null;

    public int Index
    {
        set;
        get;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        m_spriteBg = CacheTransform.Find("Texture").GetComponent<UITexture>();
        m_lableCost = CacheTransform.Find("Cost/Cost_label").GetComponent<UILabel>();
        m_lableName = CacheTransform.Find("value").GetComponent<UILabel>();
        m_spriteIcon = CacheTransform.Find("Cost/Icon").GetComponent<UISprite>();
    }



    private CMResAsynSeedData<CMTexture> m_bgCASD = null;

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if(data != null && data is int)
        {
            Index = (int)data;
        }
    }
    public void SetData(TransferData data) 
    {
        TransferDatabase tab = GameTableManager.Instance.GetTableItem<TransferDatabase>(data.tabID);
        if (tab != null)
        {
            if (m_lableName != null)
            {
                m_lableName.text = tab.strTransmitMap;
            }
            RefreshBg(tab.bgResID);
            if (m_lableCost != null)
            {
                m_lableCost.text = tab.costValue.ToString();
            }
        }
    }


    public void RefreshBg(uint resID)
    {
        if (m_spriteBg != null)
        {
            UIManager.GetTextureAsyn(resID, ref m_bgCASD, () =>
            {
                if (null != m_spriteBg)
                {
                    m_spriteBg.mainTexture = null;
                }
            }, m_spriteBg, true);
        }
    }


    public void Release()
    {
        if (null != m_bgCASD)
        {
            m_bgCASD.Release(true);
            m_bgCASD = null;
        }
    }
}