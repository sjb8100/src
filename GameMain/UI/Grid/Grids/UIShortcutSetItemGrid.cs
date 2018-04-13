using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


class UIShortcutSetItemGrid : UIGridBase
{
    UILabel m_num;

    //UILabel m_lblBarDes;

    UISprite m_bg1;

    //UISprite m_bg2;

    UITexture m_icon;

    UISprite m_border;

    GameObject m_select;

    GameObject m_btnRemove;

    public ShortCuts itemData;

    public uint clicks = 0;

    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;

    private CMResAsynSeedData<CMAtlas> iuiBorderAtlas = null;

    #region override

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != iuiBorderAtlas)
        {
            iuiBorderAtlas.Release(depthRelease);
            iuiBorderAtlas = null;
        }

        if (iuiIconAtlas != null)
        {
            iuiIconAtlas.Release(depthRelease);
            iuiIconAtlas = null;
        }

    }

    protected override void OnUIBaseDestroy()
    {
        base.OnUIBaseDestroy();
        Release();
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        m_num = this.transform.Find("num").GetComponent<UILabel>();

        //m_lblBarDes = this.transform.Find("Label").GetComponent<UILabel>();

        m_bg1 = this.transform.Find("Bg").GetComponent<UISprite>();

        //m_bg2 = this.transform.Find("bg2").GetComponent<UISprite>();

        m_icon = this.transform.Find("Icon").GetComponent<UITexture>();

        m_border = this.transform.Find("Border").GetComponent<UISprite>();

        m_select = this.transform.Find("HighLight").gameObject;

        m_btnRemove = this.transform.Find("removeBtn").gameObject;

        UIEventListener.Get(m_btnRemove).onClick = OnClickRemoveBtn;

        UIEventListener.Get(this.gameObject).onClick = OnClickBtn;

    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.itemData = data as ShortCuts;

        ItemDataBase itemDataBase = GameTableManager.Instance.GetTableItem<ItemDataBase>(this.itemData.itemid);

        if (itemDataBase == null)
        {
            m_icon.gameObject.SetActive(false);
            m_border.gameObject.SetActive(false);
            m_num.gameObject.SetActive(false);
            m_btnRemove.gameObject.SetActive(false);
            m_select.gameObject.SetActive(false);

            m_bg1.gameObject.SetActive(true);
            //m_bg2.gameObject.SetActive(true);
            return;
        }

        m_icon.gameObject.SetActive(true);
        m_border.gameObject.SetActive(true);
        m_num.gameObject.SetActive(true);
        m_btnRemove.gameObject.SetActive(true);

        m_bg1.gameObject.SetActive(false);
        //m_bg2.gameObject.SetActive(false);

        //icon
        if (m_icon != null)
        {
            BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(this.itemData.itemid);
            if (baseItem == null)
            {
                return;
            }

            SetIcon(baseItem.Icon);

            //bian kuang
            SetBorder(true, baseItem.BorderIcon,true);
        }

        //num
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(this.itemData.itemid);//道具存量
        SetNum(itemCount);

        SetSelect(true);
    }

    public void SetIcon(string iconName)
    {
        if (m_icon != null)
        {
            UIManager.GetTextureAsyn(iconName, ref iuiIconAtlas, () =>
            {
                if (null != m_icon)
                {
                    m_icon.mainTexture = null;
                }
            }, m_icon,false);

            m_icon.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 设置边框
    /// </summary>
    /// <param name="enale"></param>
    /// <param name="iconName"></param>
    public void SetBorder(bool enable, string iconName = "", bool circleStyle = false)
    {
        if (null != m_border)
        {
            if (m_border.enabled != enable)
            {
                m_border.enabled = enable;
            }
            if (enabled)
            {
                UIManager.GetAtlasAsyn(UIManager.GetIconName(iconName, circleStyle), ref iuiBorderAtlas, () =>
                {
                    if (m_border != null)
                    {
                        m_border.atlas = null;
                    }
                }, m_border,false);
            }
        }
    }

    public void SetNum(int itemNum)
    {
        if (m_num != null)
        {
            m_num.gameObject.SetActive(true);
            m_num.text = itemNum.ToString();

            if (itemNum == 0)
            {
                m_num.color = Color.red;
            }
            else
            {
                m_num.color = Color.white;
            }
        }
    }

    //public void SetBarDes(string des)
    //{
    //    if (m_lblBarDes != null)
    //    {
    //        m_lblBarDes.text = des;
    //    }
    //}

    public void SetSelect(bool b)
    {
        if (m_select != null && m_select.gameObject.activeSelf != b)
        {
            m_select.gameObject.SetActive(b);
        }
    }

    void OnClickBtn(GameObject go)
    {
        //InvokeUIDlg(UIEventType.Click, this, null);
    }

    void OnClickRemoveBtn(GameObject go)
    {
        int removeBtn = 1;
        InvokeUIDlg(UIEventType.Click, this, removeBtn);
    }

    #endregion
}

