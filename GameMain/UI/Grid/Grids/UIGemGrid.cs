/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIGemGrid
 * 版本号：  V1.0.0.0
 * 创建时间：11/16/2016 4:15:06 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIGemGrid : UIItemInfoGridBase
{
    #region Property
    private UILabel name;
    private UISprite bg;
    private UILabel des;
    private UILabel m_lab_get;
    private UISprite m_spr_redPoint;

    private Transform selectMask = null;

    //QWThisId
    private uint data;
    public uint Data
    {
        get
        {
            return data;
        }
    }

    #endregion

    #region Override Method
    protected override void OnAwake()
    {
        base.OnAwake();
        name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        des = CacheTransform.Find("Content/Des").GetComponent<UILabel>();
        m_spr_redPoint = CacheTransform.Find("Content/redPoint").GetComponent<UISprite>();
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
    }
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        ResetInfoGrid();
        if (null == data)
            return;
        this.data = (uint)data;
        Gem gem = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<Gem>(this.data, ItemDefine.ItemDataType.Gem);
        int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(gem.BaseId);
        bool visible = num > 1;
        if (visible)
        {
            SetNum(true, num.ToString());
        }
        if (!DataManager.IsMatchPalyerLv((int)gem.UseLv))
        {
            SetWarningMask(true);
        }
        SetIcon(true, gem.Icon);
        SetBorder(true, gem.BorderIcon);
        if (null != this.name)
        {
            this.name.text = gem.Name;
        }

        if (null != this.des)
        {
            this.des.text = gem.AttrDes;
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(false);
        }
    }
    public void SetRedPointStatus(bool show) 
    {
       if(m_spr_redPoint != null)
       {
           m_spr_redPoint.gameObject.SetActive(show);
       }
    }
    #endregion

    #region Set

    //设置选中
    public void SetSelect(bool select)
    {
        if (null != selectMask && selectMask.gameObject.activeSelf != select)
            selectMask.gameObject.SetActive(select);
    }
    #endregion
}