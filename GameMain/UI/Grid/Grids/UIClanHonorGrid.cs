/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIClanHonorGrid
 * 版本号：  V1.0.0.0
 * 创建时间：10/25/2016 4:18:46 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UIClanHonorGrid : UIGridBase
{
    #region property
    //事件
    private UILabel m_lab_event;
    //时间
    private UILabel m_lab_time;
    //名称
    private UILabel m_lab_name;
    #endregion

    #region overrdemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_lab_event = CacheTransform.Find("Content/Event").GetComponent<UILabel>();
        m_lab_name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        m_lab_time = CacheTransform.Find("Content/Time").GetComponent<UILabel>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        GameCmd.stHonorInfo info = null;
        if (null != data && data is GameCmd.stHonorInfo)
        {
            info = data as GameCmd.stHonorInfo;
        }
        if (null != m_lab_event)
        {
            m_lab_event.text = (null != info) ? info.honor : "";
        }
        if (null != m_lab_name)
        {
            m_lab_name.text = (null != info) ? info.name : "";
        }
        if (null != m_lab_time)
        {
            string time = "";
            if (null != info)
            {
                DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
                dt = dt.AddSeconds(info.tm);
                time = dt.ToString("yyyy-MM-dd");
            }

            m_lab_time.text = time;
        }
    }
    #endregion
}