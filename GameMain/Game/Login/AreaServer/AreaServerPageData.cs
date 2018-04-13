/************************************************************************************
 * Copyright (c) 2018  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Login.AreaServer
 * 创建人：  wenjunhua.zqgame
 * 文件名：  Area
 * 版本号：  V1.0.0.0
 * 创建时间：1/9/2018 2:33:27 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AreaServerPageData
{
    #region property
    //区服类型
    private uint m_serverTypeid = 0;
    public uint ServerTypeId
    {
        get
        {
            return m_serverTypeid;
        }
    }
    //当前m_serverType里面的索引
    private int m_iIndex = 0;

    private string m_strPageName = "";
    public string PageName
    {
        get
        {
            return m_strPageName;
        }
    }

    //区
    private List<uint> m_lstZones = null;
    public List<uint> Zones
    {
        get
        {
            return m_lstZones;
        }
    }

    //可用区服数据
    private List<LoginDataManager.EnableZoneInfo> m_lstEnableZones = null;
    public List<LoginDataManager.EnableZoneInfo> EnableZones
    {
        get
        {
            return m_lstEnableZones;
        }
    }

    //数量
    public int Count
    {
        get
        {
            return (null != EnableZones) ? EnableZones.Count : 0;
        }
    }

    //是否为推荐页
    private bool m_bRecommond = false;
    public bool Recommond
    {
        get
        {
            return m_bRecommond;
        }
    }

    #endregion

    #region structmehod
    public AreaServerPageData(string name,List<LoginDataManager.EnableZoneInfo> enableZoneInfo,bool isRecommond = false)
    {
        m_strPageName = name;
        if (null != enableZoneInfo && enableZoneInfo.Count > 0)
        {
            if (null == m_lstEnableZones)
                m_lstEnableZones = new List<LoginDataManager.EnableZoneInfo>();
            m_lstEnableZones.AddRange(enableZoneInfo);
            m_bRecommond = isRecommond;
        }
    }
    #endregion

    #region OP
    public bool TryGetEanbleZoneInfo(int index,out LoginDataManager.EnableZoneInfo enableZoneInfo)
    {
        enableZoneInfo = null;
        if (index >= 0 && index < Count)
        {
            enableZoneInfo = EnableZones[index];
            return true;
        }
        return false;
    }
    #endregion
}