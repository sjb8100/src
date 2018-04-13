/************************************************************************************
 * Copyright (c) 2018  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Daily
 * 创建人：  wenjunhua.zqgame
 * 文件名：  JvBaoBossDamRankPanel
 * 版本号：  V1.0.0.0
 * 创建时间：3/26/2018 11:50:20 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class JvBaoBossDamRankPanel
{
    #region property
    private JvBaoBossWorldManager m_mgr = null;
    private UIPanelManager m_pMgr = null;
    private ComBatCopyDataManager m_cbMgr = null;
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidgets();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        UpdateConfigQuit(true);
        BuildBossDamRankList();
        RegisterGlobalEvent(true);
    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalEvent(false);
    }

    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        HideSelf();
    }

    #endregion

    #region Op
    private void InitWidgets()
    {
        m_mgr = DataManager.Manager<JvBaoBossWorldManager>();
        m_pMgr = DataManager.Manager<UIPanelManager>();
        m_cbMgr = DataManager.Manager<ComBatCopyDataManager>();
        if (null != m_ctor_BossDamRankSV && null != m_widget_UIJvBaoBossDamRankGrid)
        {
            m_ctor_BossDamRankSV.Initialize<UIJvBaoBossDamRankGrid>(m_widget_UIJvBaoBossDamRankGrid.gameObject, OnBossRankGridUpdate);
            m_ctor_BossDamRankSV.RefreshCheck();
        }
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="register"></param>
    private void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_WORLDBOSSDAMRANKREFRESH, OnGlobalUIEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_WORLDBOSSDAMRANKREFRESH, OnGlobalUIEventHandler);
        }
    }

    private void OnGlobalUIEventHandler(int eventId,object obj)
    {
        switch(eventId)
        {
            case (int) Client.GameEventID.UIEVENT_WORLDBOSSDAMRANKREFRESH:
                BuildBossDamRankList();
                break;
        }
    }

    private void OnBossRankGridUpdate(UIGridBase gridBase, int index)
    {
        if (null != gridBase)
        {
            if (gridBase is UIJvBaoBossDamRankGrid)
            {
                GameCmd.BossDamRank tempData = null;
                if (m_mgr.TryGetWorldBossDamRankData(index, out tempData))
                {
                    gridBase.SetGridData(tempData);
                }
                
            }
        }
    }

    private void BuildBossDamRankList()
    {
        if (null != m_ctor_BossDamRankSV)
        {
            int count = m_mgr.WorldBossDamRankCount;
            m_ctor_BossDamRankSV.CreateGrids(count);
        }

        //刷新我的排名
        if (null != m_label_myRanknum_label)
        {
            string tempTxt = "--";
            if (m_mgr.MyBossDamRank != 0)
                tempTxt = m_mgr.MyBossDamRank.ToString();
            m_label_myRanknum_label.text = tempTxt;
        }

        //刷新我的伤害
        if (null != m_label_myScorenum_label)
        {
            string tempTxt = "--";
            if (m_mgr.MyBossDam != 0)
                tempTxt = m_mgr.MyBossDam.ToString();
            m_label_myScorenum_label.text = tempTxt;
        }
    }

    private float lastUpdateStamp = 0;
    private void UpdateConfigQuit(bool forceUpdate = false)
    {
        if (Time.time - lastUpdateStamp < 1)
        {
            return;
        }
        lastUpdateStamp = Time.time;

        string confirmQuitTxt = "确定（{0}）";
        bool canSee = false;
        bool timeOver = false;
        if (m_mgr.IsWorldBossDead && m_pMgr.IsShowPanel(PanelID.JvBaoBossDamRankPanel))
        {
            canSee = true;
            int timeLeft = (int)m_cbMgr.CopyCountDown;
            timeOver = (timeLeft == 0);
            confirmQuitTxt = string.Format(confirmQuitTxt, timeLeft);
        }

        if (null != m_btn_BtnConfirmQuit)
        {
            if (m_btn_BtnConfirmQuit.gameObject.activeSelf != canSee)
            {
                m_btn_BtnConfirmQuit.gameObject.SetActive(canSee);
            }

            if (canSee && null != m_label_ConfirmQuitTxt)
            {
                m_label_ConfirmQuitTxt.text = confirmQuitTxt;
            }

            if (canSee && timeOver)
            {
                HideSelf();
                QuitCopy();
            }
        }
    }

    private void Update()
    {
        UpdateConfigQuit();
    }

    #endregion

    #region nguibtns
    void onClick_Btn_close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_BtnConfirmQuit_Btn(GameObject caster)
    {
        QuitCopy();
    }

    void QuitCopy()
    {
        m_mgr.QuitCopy();
    }
    #endregion
}