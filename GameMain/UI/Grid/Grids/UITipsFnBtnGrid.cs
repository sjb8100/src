/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：
 * 命名空间：GameMain.UI.Grid.Grids
 * 文件名：  UITipsFnBtnGrid
 * 版本号：  V1.0.0.0
 * 唯一标识：1c29bcff-e7c7-47e2-a7b0-b2ddef4ad08a
 * 当前的用户域：USER-20160526UC
 * 电子邮箱：XXXX@sina.cn
 * 创建时间：9/29/2016 11:01:15 AM
 * 描述：
 ************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UITipsFnBtnGrid : UIGridBase
{
    #region define
    public class UITipsFnBtnData
    {
        public ItemDefine.ItemTipsFNType FnType = ItemDefine.ItemTipsFNType.None;
        public string Name = "";
        //数据
        public object Data;
    }
    
    #endregion

    #region property

    private UITipsFnBtnData m_data;
    public UITipsFnBtnData Data
    {
        get
        {
            return m_data;
        }
    }

    //名称label
    private UILabel m_lab_name;
    #endregion
    
    #region overridemethod

    protected override void OnAwake()
    {
        base.OnAwake();
        m_lab_name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null != data && data is UITipsFnBtnData)
        {
            m_data = (UITipsFnBtnData)data;

            if (null != m_lab_name)
                m_lab_name.text = m_data.Name;
        }
    }

    #endregion

    #region EventCallback
    private void OnFnTypeClick(UnityEngine.GameObject go)
    {
        InvokeUIDlg(UIEventType.Click, this, m_data);
    }
    #endregion
}