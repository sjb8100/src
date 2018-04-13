/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIStrengthenSuitPropGrid
 * 版本号：  V1.0.0.0
 * 创建时间：6/7/2017 5:42:29 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UIStrengthenSuitPropGrid : UIGridBase
{
    #region property
    private UILabel m_labDes;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_labDes = CacheTransform.Find("Content/Des").GetComponent<UILabel>();
        SetTriggerEffect(false);
    }
    #endregion

    #region op
    public void SetDes(bool active,string des)
    {
        ColorType colorTyp = (active) ? ColorType.JZRY_Tips_Green : ColorType.JZRY_Tips_White;
        if (null != m_labDes && !string.IsNullOrEmpty(des))
        {
            m_labDes.text = ColorManager.GetColorString(colorTyp, des);
        }
    }
    #endregion
}