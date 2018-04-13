/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Clan
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ClanPanel_Task
 * 版本号：  V1.0.0.0
 * 创建时间：10/17/2016 11:32:23 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class ClanPanel
{
    #region property
    private List<uint> m_list_taskInfos = null;
    #endregion

    #region Init
    private void InitTask()
    {
        m_trans_UpgradeContent.gameObject.SetActive(false);
        m_trans_DonateContent.gameObject.SetActive(false);
        m_trans_DeclareWarContent.gameObject.SetActive(false);
        m_trans_ActivityScrollViewContent.gameObject.SetActive(true);
//         if (IsInitMode(ClanPanelMode.Activity))
//         {
//             return;
//         }
        SetInitMode(ClanPanelMode.Activity);

        if (null != m_ctor_ActivityScrollView)
        {
            m_ctor_ActivityScrollView.RefreshCheck();
            m_ctor_ActivityScrollView.Initialize<UIClanTaskGrid>(m_trans_UIClanTaskGrid.gameObject, OnUpdateUIGrid, OnUIGridEventDlg);
        }
      
        m_list_taskInfos = new List<uint>();
        BuildTask();
    }

    /// <summary>
    /// 构建Task
    /// </summary>
    private void BuildTask()
    {
        BuildTaskUI();
    }

    /// <summary>
    /// 创建任务UI
    /// </summary>
    private void BuildTaskUI()
    {
//         if (null != m_ctor_ActivityScrollView)
//         {
//             m_list_taskInfos = new List<uint>();
//             m_list_taskInfos.AddRange(m_mgr.GetClanTaskIds());
//             m_ctor_ActivityScrollView.CreateGrids(m_list_taskInfos.Count);
//         }
    }
    #endregion

    #region Op
    /// <summary>
    /// 氏族信息改变
    /// </summary>
    /// <param name="data"></param>
    private void OnClanTaskChanged(object data)
    {
        if (IsPanelMode(ClanPanelMode.Activity))
        {
            BuildTaskUI();   
        }
    }
    /// <summary>
    /// 更新任务面板
    /// </summary>
    private void UpdateTask()
    {

    }

    /// <summary>
    /// 刷新任务
    /// </summary>
    private void DoRefresh()
    {
        //请求氏族任务信息
        DataManager.Manager<ClanManger>().RequestClanTaskInfos();
    }
    #endregion

    #region UIEvent
    void onClick_BtnRefresh_Btn(GameObject caster)
    {
        DoRefresh();
    }
    #endregion
}