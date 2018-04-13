/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：
 * 命名空间：GameMain.UI.Grid.Grids
 * 文件名：  UISplitGetGrid
 * 版本号：  V1.0.0.0
 * 唯一标识：6efdeaf1-5d6e-40a8-bcbd-184c88ee5346
 * 当前的用户域：USER-20160526UC
 * 电子邮箱：XXXX@sina.cn
 * 创建时间：10/10/2016 11:09:27 AM
 * 描述：
 ************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UISplitGetGrid : UIItemInfoGridBase
{
    #region property
    //名称
    private UILabel m_lab_name;
    //数量
    private UILabel m_lab_num;
    //表格id
    private uint baseId;
    public uint BaseId
    {
        get
        {
            return baseId;
        }
    }
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_lab_name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        m_lab_num = CacheTransform.Find("Content/Num").GetComponent<UILabel>();
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
    }

    #endregion

    #region Set
    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="num"></param>
    public void Set(uint baseId,string num)
    {
        ResetInfoGrid();
        this.baseId = baseId;
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(baseId);
        if (null != m_lab_name)
        {
            m_lab_name.text = (baseItem.IsBaseReady) ? baseItem.Name : "金币";
        }

        if (null != m_lab_num)
        {
            m_lab_num.text = num;
        }

        SetIcon(true, ((baseItem.IsBaseReady) ? baseItem.Icon : "1"));
        SetBorder(true, baseItem.BorderIcon);
        //SetBindMask(false);
        //SetTimeLimitMask(false);
        //SetFightUp(false);
        if (baseItem.IsBaseReady)
        {
            if (baseItem.IsMuhon)
            {
                SetMuhonMask(true, Muhon.GetMuhonStarLevel(baseItem.BaseId));
                SetMuhonLv(true, Muhon.GetMuhonLv(baseItem));
            }
            else if (baseItem.IsRuneStone)
            {
                SetRuneStoneMask(true, (uint)baseItem.Grade);
            }
        }
    }
    #endregion
}