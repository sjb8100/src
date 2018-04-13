/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UICompoundAttrGrid
 * 版本号：  V1.0.0.0
 * 创建时间：5/17/2017 4:43:47 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UICompoundAttrGrid : UIGridBase
{
    #region overridemethod
    private UILabel mLabel = null;
    private UILabel mDes = null;
    protected override void OnAwake()
    {
        base.OnAwake();
        mLabel = CacheTransform.Find("Content/Grade/Grade").GetComponent<UILabel>();
        mDes = CacheTransform.Find("Content/Des").GetComponent<UILabel>();
    }

    #endregion

    #region set

    public void SetData(uint grade,string des)
    {
        if (null != mLabel)
        {
            mLabel.text = grade.ToString();
        }
        if (null != mDes)
        {
            mDes.text = des;
        }
    }
    #endregion
}