using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// UIItemTipsLineGrid数据类
/// </summary>
public class ItemTipsLineGridData
{
    public UIItemTipsLineGrid.TipsLineType TipsLineType = UIItemTipsLineGrid.TipsLineType.Txt;
    public string Description;
    public string GemName;
    public uint AttrGrade;
    public bool IsLock = true;
    public ColorType ColorType = ColorType.White;
}

public class UIItemTipsLineGrid : UIGridBase
{
    #region define
    public enum TipsLineType
    {
        Attr = 1,
        Hole = 2,
        Txt = 4,
    }
    #endregion

    #region Property
    //描述
    private UILabel m_label_description;
    private float descriptionOffset = 15f;

    //宝石属性相关
    private Transform m_ts_holeRoot;
    private Transform m_ts_lock;
    private UITexture m_sp_icon;

    //属性
    private Transform m_ts_AttrRoot;
    private UILabel m_lab_num;
    //高度
    public float Height
    {
        get
        {
            return (null != m_label_description) ? m_label_description.height : 0;
        }
    }
    #endregion
    protected override void OnAwake()
    {
        base.OnAwake();
        m_label_description = CacheTransform.Find("Content/Description").GetComponentInChildren<UILabel>();
        m_label_description.color = Color.white;
        m_ts_holeRoot = CacheTransform.Find("Content/Hole");
        m_ts_lock = CacheTransform.Find("Content/Hole/Lock");
        m_sp_icon = CacheTransform.Find("Content/Hole/Icon").GetComponent<UITexture>();

        m_ts_AttrRoot = CacheTransform.Find("Content/Attr");
        m_lab_num = CacheTransform.Find("Content/Attr/Num").GetComponentInChildren<UILabel>();
    }

    /// <summary>
    /// 设置字体size
    /// </summary>
    /// <param name="size"></param>
    public void SetFontSize(int size)
    {
        if (null != m_label_description)
        {
            m_label_description.fontSize = size;
            m_label_description.height = size;
        }
            
    }

    /// <summary>
    /// 设置字间距
    /// </summary>
    /// <param name="gp"></param>
    public void SetLineGap(UnityEngine.Vector2 gp)
    {
        if (null != m_label_description)
        {
            m_label_description.floatSpacingX = gp.x;
            m_label_description.floatSpacingY = gp.y;
        }
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_iconCASD)
        {
            m_iconCASD.Release(true);
            m_iconCASD = null;
        }
    }

    private CMResAsynSeedData<CMTexture> m_iconCASD = null;
    public void SetGridViewData(TipsLineType lineType,string des, ColorType desColorType = ColorType.JZRY_Txt_White,uint num = 0,bool islock = false,string inlayGemName = "")
    {
        bool cansee = (lineType == TipsLineType.Hole);
        if (null != m_ts_holeRoot && m_ts_holeRoot.gameObject.activeSelf != cansee)
        {
            m_ts_holeRoot.gameObject.SetActive(cansee);
        }

        if (cansee)
        {
            if (null != m_ts_lock && m_ts_lock.gameObject.activeSelf != islock)
            {
                m_ts_lock.gameObject.SetActive(islock);
            }

            if (null != m_sp_icon && m_sp_icon.gameObject.activeSelf == islock)
            {
                m_sp_icon.gameObject.SetActive(!islock);
            }
            if (!islock)
            {
                UIManager.GetTextureAsyn(inlayGemName, ref m_iconCASD, () =>
                    {
                        if (null != m_sp_icon)
                        {
                            m_sp_icon.mainTexture = null;
                        }
                    }, m_sp_icon, false);
            }
        }

        cansee = (lineType == TipsLineType.Attr);
        if (null != m_ts_AttrRoot && m_ts_AttrRoot.gameObject.activeSelf != cansee)
        {
            m_ts_AttrRoot.gameObject.SetActive(cansee);
        }

        if (cansee)
        {
            if (null != m_lab_num)
            {
                m_lab_num.text = num.ToString();
            }
        }

        if (null != m_label_description)
        {
            m_label_description.color = ColorManager.GetColor32OfType(desColorType);
            m_label_description.text = des;
            UnityEngine.Vector3 pos = m_label_description.transform.localPosition;
            pos.x = (lineType != TipsLineType.Txt) ? descriptionOffset : -descriptionOffset;
            m_label_description.transform.localPosition = pos;
        }
    }

}