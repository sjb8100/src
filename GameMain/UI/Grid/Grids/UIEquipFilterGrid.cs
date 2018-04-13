/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIEquipFilterGrid
 * 版本号：  V1.0.0.0
 * 创建时间：8/10/2017 2:38:54 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIEquipFilterGrid : UIGridBase
{
    #region property
    private UILabel m_labName = null;
    private Transform m_tsSelectMask = null;
    private uint m_uEquipType = 0;
    public uint EquipType
    {
        get
        {
            return m_uEquipType;
        }
    }
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_labName = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        m_tsSelectMask = CacheTransform.Find("Content/Hightlight");
    }

    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        if (null != m_tsSelectMask && m_tsSelectMask.gameObject.activeSelf != hightLight)
        {
            m_tsSelectMask.gameObject.SetActive(hightLight);
        }
    }
    #endregion

    #region Op
    public void SetData(string name,uint equipType,bool select)
    {
        this.m_uEquipType = equipType;
        if (null != m_labName)
        {
            m_labName.text = name;
        }
        SetHightLight(select);
    }
    #endregion

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }
}