/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Guide
 * 创建人：  wenjunhua.zqgame
 * 文件名：  GuideManager_TabFuncOpen
 * 版本号：  V1.0.0.0
 * 创建时间：6/27/2017 3:51:10 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

partial class GuideManager
{
    #region property
    private List<int> m_lstAlreadyOpenTabFuncId = new List<int>();

    #endregion

    #region TabFunction(页签功能)

    /// <summary>
    /// 重置
    /// </summary>
    private void ResetTabFunction()
    {
        m_lstAlreadyOpenTabFuncId.Clear();
    }

    private void CheckTabFuncOpen(bool sendEvent)
    {
        List<table.TabFuncDataBase> tabFuncs = GameTableManager.Instance.GetTableList<table.TabFuncDataBase>();
        if (null == tabFuncs || tabFuncs.Count == 0)
            return;
        int funcId = 0;
        int playerLv = DataManager.Instance.PlayerLv;
        for(int i = 0,max = tabFuncs.Count;i < max;i++)
        {
            funcId = (int)tabFuncs[i].id;
            if (m_lstAlreadyOpenTabFuncId.Contains(funcId))
            {
                continue;
            }
            if (playerLv >= tabFuncs[i].openLv)
            {
                AddOpenTabFuncId(funcId);
                if(sendEvent)
                {
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTTABFUNCOPEN, funcId);
                }
            }
        }
    }

    /// <summary>
    /// 添加一个页签开启功能到缓存
    /// </summary>
    /// <param name="tabFuncID"></param>
    private void AddOpenTabFuncId(int tabFuncID)
    {
        if (!m_lstAlreadyOpenTabFuncId.Contains(tabFuncID))
        {
            m_lstAlreadyOpenTabFuncId.Add(tabFuncID);
        }
    }

    /// <summary>
    /// 页签功能是否默认开启
    /// </summary>
    /// <param name="funcID"></param>
    /// <returns></returns>
    public static bool IsTabFunDefaultOpen(int funcID)
    {
        return funcID == 0;
    }

    /// <summary>
    /// 页签功能是否开启
    /// </summary>
    /// <param name="tabFuncId"></param>
    /// <param name="openLv"></param>
    /// <returns></returns>
    public bool IsTabFuncOpen(int tabFuncId, out int openLv)
    {
        openLv = 0;
        if (IsTabFunDefaultOpen(tabFuncId) || m_lstAlreadyOpenTabFuncId.Contains(tabFuncId))
        {
            return true;
        }
        table.TabFuncDataBase tdb = GameTableManager.Instance.GetTableItem<table.TabFuncDataBase>((uint)tabFuncId);
        if (null != tdb)
        {
            if (DataManager.Instance.PlayerLv >= tdb.openLv)
            {
                AddOpenTabFuncId(tabFuncId);
                return true;
            }
            openLv = (int)tdb.openLv;
        }
        
        return false;
    }

    /// <summary>
    /// 页签功能是否开启
    /// </summary>
    /// <param name="tabFuncId"></param>
    /// <returns></returns>
    public bool IsTabFuncOpen(int tabFuncId)
    {
        int openLv = 0;
        return IsTabFuncOpen(tabFuncId, out openLv);
    }
    #endregion
}