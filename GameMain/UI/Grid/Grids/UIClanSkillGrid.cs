/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIClanSkillGrid
 * 版本号：  V1.0.0.0
 * 创建时间：10/24/2016 4:23:06 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UIClanSkillGrid : UIGridBase
{
    #region property
    //技能id
    private uint m_uint_id;
    public uint Id
    {
        get
        {
            return m_uint_id;
        }
    }
    //名称
    private UILabel m_lab_name;
    //等级
    private UILabel m_lab_lv;
    //图标
    private UITexture m_sp_icon;
    //遮罩
    private UISprite m_sp_mask;
    //锁
    private UISprite m_sp_lock;
    private UISprite select;
    #endregion

    #region overridemethod

    protected override void OnAwake()
    {
        base.OnAwake();
        m_lab_name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        m_lab_lv = CacheTransform.Find("Content/Lv").GetComponent<UILabel>();
        m_sp_icon = CacheTransform.Find("Content/Icon").GetComponent<UITexture>();
        m_sp_mask = CacheTransform.Find("Content/Mask").GetComponent<UISprite>();
        m_sp_lock = CacheTransform.Find("Content/Lock").GetComponent<UISprite>();
        select = CacheTransform.Find("Content/select").GetComponent<UISprite>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null != data && data is uint)
        {
            m_uint_id = (uint)data;
        }
    }
    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        select.gameObject.SetActive(hightLight);
    }
    #endregion

    #region Set
    CMResAsynSeedData<CMTexture> m_playerAvataCASD = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_playerAvataCASD != null)
        {
            m_playerAvataCASD.Release(depthRelease);
            m_playerAvataCASD = null;
        }
    }
    /// <summary>
    /// 设置UI显示
    /// </summary>
    /// <param name="name"></param>
    /// <param name="lv"></param>
    /// <param name="icon"></param>
    /// <param name="select"></param>
    /// <param name="lock"></param>
    public void SetInfo(string name, string lv, string icon, bool select, bool isLock)
    {
        //名称
        if (null != m_lab_name)
        {
            m_lab_name.text = name;
        }
        //等级
        if (null != m_lab_lv)
        {
            m_lab_lv.text = lv;
        }
        //图标
        UIManager.GetTextureAsyn(icon, ref m_playerAvataCASD, () =>
        {
            if (null != m_sp_icon)
            {
                m_sp_icon.mainTexture = null;
            }
        }, m_sp_icon);
       
        //设置选中
        //SetHightLight(select);
        //mask
        if (null != m_sp_mask && m_sp_mask.enabled != isLock)
        {
            m_sp_mask.enabled = isLock;
        }
        //锁
        if (null != m_sp_lock && m_sp_lock.enabled != isLock)
        {
            m_sp_lock.enabled = isLock;
        }
        
    }
    #endregion
}