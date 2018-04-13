using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class UIItemCommonGrid : UIGridBase
{
    #region Property
    //图标
    private UITexture icon = null;
    private UISprite Bg = null;
    //数量
    private UILabel num = null;
    private UILabel getlable = null;
    private UISprite blackSprite = null;
    private UILabel lableName = null;
    private UISprite BorderIcon = null;
    //数据
    private ItemDefine.UIItemCommonData data;
    public ItemDefine.UIItemCommonData Data
    {
        get
        {
            return data;
        }
    }
    #endregion
    #region OverrideMethod

    public override void Init()
    {
        base.Init();
        icon = transform.Find("Icon").GetComponent<UITexture>();
        num = transform.Find("Num").GetComponent<UILabel>();
        getlable = transform.Find("gettip").GetComponent<UILabel>();
        blackSprite = transform.Find("black").GetComponent<UISprite>();
        Bg = transform.Find("Bg").GetComponent<UISprite>();
        lableName = transform.Find("Name").GetComponent<UILabel>();
        BorderIcon = transform.Find("qulity").GetComponent<UISprite>();
        if (lableName != null)
        {
            lableName.enabled = false;
        }

        UIWidget uiwidget = GetComponent<UIWidget>();
        if (uiwidget != null)
        {
            uiwidget.depth = 4;
        }
    }
    private CMResAsynSeedData<CMTexture> m_iconCASD = null;
    private CMResAsynSeedData<CMAtlas> m_bgCASD = null;
    public override void SetGridData(object data)
    {
        if (null == data)
            return;
        this.data = data as ItemDefine.UIItemCommonData;
        if (null != icon)
        {
            UIManager.GetTextureAsyn(this.data.IconName, ref m_iconCASD, () =>
                {
                    if (null != icon)
                    {
                        icon.mainTexture = null;
                    }
                }, icon);
        }

        if (BorderIcon != null  )
        {
            string bgName = ItemDefine.GetItemBorderIcon(this.data.Qulity);
            UIManager.GetAtlasAsyn(bgName, ref m_bgCASD, () =>
            {
                if (null != BorderIcon)
                {
                    BorderIcon.atlas = null;
                }
            }, BorderIcon);
        }
        if (null != num)
        {
            if (this.data.Num >= this.data.NeedNum)
            {
                num.text = this.data.Num >= 1 ? this.data.Num.ToString() : "";
                if (getlable != null)
                    getlable.gameObject.SetActive(false);
                if (blackSprite != null)
                    blackSprite.gameObject.SetActive(false);
            }
            else 
            {
                num.text = "";
                SetShowGetWay(this.data.ShowGetWay);
            }
        }
    }

    public void SetShowGetWay(bool bShow)
    {
        if (getlable != null)
            getlable.gameObject.SetActive(bShow);
        if (blackSprite != null)
            blackSprite.gameObject.SetActive(bShow);
    }

    public override void Reset()
    {
        base.Reset();
        Release();
        if (null != num)
        {
            num.text = "";
        }
        if (lableName != null)
        {
            lableName.enabled = false;
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_iconCASD)
        {
            m_iconCASD.Release(true);
        }
        data = null;
    }
    
    #endregion

    public void SetItemName(string strName)
    {
        if (lableName != null)
        {
            lableName.enabled = true;
            lableName.text = strName;
        }
    }
}