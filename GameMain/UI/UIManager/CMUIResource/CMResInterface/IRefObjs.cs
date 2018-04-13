/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.UIManager.CMUIResource.CMResInterface
 * 创建人：  wenjunhua.zqgame
 * 文件名：  IRefObjs
 * 版本号：  V1.0.0.0
 * 创建时间：9/6/2017 10:51:01 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class IRefObjs : IUIRef  
{
    #region property
    private int m_irefCount = 0;
    public int RefCount
    {
        get
        {
            return m_irefCount;
        }
    }
    #endregion 

    #region IUIRef
    //添加引用
    public void AddRef()
    {
        m_irefCount++;
    }

    //移除引用
    public void RemoveRef()
    {
        if (m_irefCount > 0)
        {
            m_irefCount--;
        }
        if (m_irefCount == 0)
        {
            OnObjsRefChange2Zero();
        }
    }
    #endregion

    #region RefGoToZero
    /// <summary>
    /// 资源
    /// </summary>
    public virtual void OnObjsRefChange2Zero()
    {

    }
    #endregion

}