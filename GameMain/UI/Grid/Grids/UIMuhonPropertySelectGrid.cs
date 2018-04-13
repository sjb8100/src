/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIMuhonPropertySelectGrid
 * 版本号：  V1.0.0.0
 * 创建时间：5/19/2017 3:55:57 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIMuhonPropertySelectGrid : UIGridBase
{
    #region Property
    //属性
    private UILabel property;
    private UILabel propertyGrade;
    private UIToggle checkBox = null;

    private uint attrId = 0;
    #endregion
    protected override void OnAwake()
    {
        base.OnAwake();
        property = CacheTransform.Find("Content/Property").GetComponent<UILabel>();
        propertyGrade = CacheTransform.Find("Content/GradeContent/Grade").GetComponent<UILabel>();   
        checkBox = CacheTransform.Find("Content/CheckBox").GetComponent<UIToggle>();
        checkBox.instantTween = true;
        UIEventListener.Get(checkBox.gameObject).onClick = (obj) =>
            {
                if (null != mdlg)
                {
                    mdlg.Invoke(attrId,checkBox.value);
                }
            };
    }

    #region Set

    /// <summary>
    /// 设置格子View
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="select"></param>
    public void SetGridView(uint attrId,string txt,uint grade, bool select)
    {
        this.attrId = attrId;
        if (null != property)
        {
            property.text = txt;
        }
        if (null != propertyGrade)
        {
            propertyGrade.text = grade.ToString();
        }
        if (null != checkBox && checkBox.value != select)
        {
            checkBox.value = select;
        }
    }
    Action<uint, bool> mdlg = null;
    public void RegisterCheckBoxDlg(Action<uint,bool> dlg)
    {
        this.mdlg = dlg;
    }

    #endregion
}