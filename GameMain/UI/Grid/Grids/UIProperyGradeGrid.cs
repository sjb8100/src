/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIProperyGradeGrid
 * 版本号：  V1.0.0.0
 * 创建时间：5/19/2017 3:21:24 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIProperyGradeGrid : UIGridBase
{
    #region Property
    //属性
    private UILabel property;
    //档次
    private UILabel propertyGrade;
    //选择标记
    private Transform max;
    #endregion
    protected override void OnAwake()
    {
        base.OnAwake();
        property = CacheTransform.Find("Content/Property").GetComponent<UILabel>();
        propertyGrade = CacheTransform.Find("Content/GradeContent/Grade").GetComponent<UILabel>();
        max = CacheTransform.Find("Content/Max");
    }

    #region OverrideMethod
    #endregion

    #region Set

    /// <summary>
    /// 设置格子View
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="grade"></param>
    public void SetGridView(string txt,uint grade,bool max)
    {
        if (null != propertyGrade)
            propertyGrade.text = grade.ToString();

        if (null != property)
        {
            property.text = txt;
        }

        if (null != this.max && this.max.gameObject.activeSelf != max)
        {
            this.max.gameObject.SetActive(max);
        }
    }

    #endregion
}