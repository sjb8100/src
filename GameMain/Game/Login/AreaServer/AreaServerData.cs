/************************************************************************************
 * Copyright (c) 2018  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Login.AreaServer
 * 创建人：  wenjunhua.zqgame
 * 文件名：  AreaServerData
 * 版本号：  V1.0.0.0
 * 创建时间：1/9/2018 10:05:46 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AreaServerData
{
    #region property
    //id
    private uint m_uid;
    public uint ID
    {
        get
        {
            return m_uid;
        }
    }

    //区间
    private SeverSection m_section;
    public SeverSection Section
    {
        get
        {
            return m_section;
        }
    }

    //名称
    private string m_strName = "";
    public string Name
    {
        get
        {
            return m_strName;
        }
    }
    #endregion

    #region structmehod

    public AreaServerData (uint areaServerId,string name,string serverSection)
    {
        m_uid = areaServerId;
        m_section = new SeverSection(serverSection);
        m_strName = string.IsNullOrEmpty(name) ? "" : name;
    }
    #endregion

    #region public method
    public bool IsContainID(uint serverID)
    {
        return (null != m_section && m_section.IsContainID(serverID)); 
    }
    #endregion

    #region private method
    
    #endregion
}