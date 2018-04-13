/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.OfflineEarnings
 * 创建人：  wenjunhua.zqgame
 * 文件名：  OfflineEarningsPanel
 * 版本号：  V1.0.0.0
 * 创建时间：5/27/2017 11:11:58 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class OfflineEarningsPanel
{
    #region property
    private bool mbDoubleEarning = false;
    private UIToggle mtCheck1 = null;
    private UIToggle mtCheck2 = null;
    private bool HaveNoble
    {
        get
        {
            return DataManager.Manager<Mall_HuangLingManager>().NobleID > 1;
        }
    }

    private OfflineManager omgr = null;
    private List<BaseItem> mlstRewardItems = null;
    #endregion

    #region overridemethod

    protected override void OnLoading()
    {
        base.OnLoading();
        omgr = DataManager.Manager<OfflineManager>();
        if (null != m_btn_OneCheck)
        {
            mtCheck1 = m_btn_OneCheck.GetComponent<UIToggle>();
        }

        if (null != m_btn_TwoCheck)
        {
            mtCheck2 = m_btn_TwoCheck.GetComponent<UIToggle>();
        }

        if (!m_ctor_ItemGridScrollView.Visible)
        {
            m_ctor_ItemGridScrollView.SetVisible(true);
        }
        if (null != m_ctor_ItemGridScrollView && null != m_trans_UIOfflineRewardGrid)
        {
            m_ctor_ItemGridScrollView.RefreshCheck();
            m_ctor_ItemGridScrollView.Initialize<UIOfflineRewardGrid>(m_trans_UIOfflineRewardGrid.gameObject, OnUpdateGridData, OnUIEventCallback);
            //m_ctor_ItemGridScrollView.Initialize<UIOfflineRewardGrid>((uint)GridID.Uiofflinerewardgrid, UIManager.OnObjsCreate, UIManager.OnObjsRelease, OnUpdateGridData, OnUIEventCallback);
        }
        mlstRewardItems = new List<BaseItem>();
    }

    protected override void OnShow(object data)
    {
        if (null != m_label_EarningTime)
        {
            int hour = (int)omgr.OfflineTime / 60;
            int min = (int)omgr.OfflineTime % 60;
            int max = (int)OfflineManager.OfflinemMaxTime /60;
            m_label_EarningTime.text = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Offline_Earning_Time
                , hour.ToString("D2"), min.ToString("D2"), max.ToString());
        }

        if (null != m_ctor_ItemGridScrollView)
        {
            mlstRewardItems.Clear();
            mlstRewardItems.AddRange(omgr.AwardItems);
            m_ctor_ItemGridScrollView.CreateGrids(mlstRewardItems.Count);
        }
        mbDoubleEarning = HaveNoble;
        UpdateExpEarningsType();
    }
    #endregion

    #region Op

    private void OnUpdateGridData(UIGridBase gridbase,int index)
    {
        if (mlstRewardItems.Count > index && gridbase is UIOfflineRewardGrid)
        {
            BaseItem rewardItem = mlstRewardItems[index];
            UIOfflineRewardGrid offlineGrid = gridbase as UIOfflineRewardGrid;
            offlineGrid.SetGridData(rewardItem);
        }
    }

    private void OnUIEventCallback(UIEventType eType,object data,object param)
    {
        if (eType == UIEventType.Click)
        {

        }
    }

    private void UpdateExpEarningsType()
    {
        if (null != m_label_EXpEarning)
        {
            long exp = omgr.OfflineExpAward * ((mbDoubleEarning) ? 2 : 1);
            m_label_EXpEarning.text = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Offline_Earning_Exp, exp);
        }
        bool visible = mbDoubleEarning && !HaveNoble;
        if (null != m_btn_Jump && m_btn_Jump.gameObject.activeSelf != visible)
        {
            m_btn_Jump.gameObject.SetActive(visible);
        }

        visible = !visible;
        if (null != m_btn_Get && m_btn_Get.gameObject.activeSelf != visible)
        {
            m_btn_Get.gameObject.SetActive(visible);
        }

        if (null != mtCheck1 && mtCheck1.value == mbDoubleEarning)
        {
            mtCheck1.value = !mbDoubleEarning;
        }

        if (null != mtCheck2 && mtCheck2.value != mbDoubleEarning)
        {
            mtCheck2.value = mbDoubleEarning;
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (null != m_ctor_ItemGridScrollView)
        {
            m_ctor_ItemGridScrollView.Release(depthRelease);
        }
    }

    #endregion

    #region UIEvent
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Get_Btn(GameObject caster)
    {
        HideSelf();
        DataManager.Manager<OfflineManager>().GetOfflineReward(mbDoubleEarning);
    }

    void onClick_OneCheck_Btn(GameObject caster)
    {
        if (mbDoubleEarning)
        {
            mbDoubleEarning = false;
        }
        UpdateExpEarningsType();
    }

    void onClick_TwoCheck_Btn(GameObject caster)
    {
        if (!mbDoubleEarning)
        {
            mbDoubleEarning = true;
        }
        UpdateExpEarningsType();
    }

    void onClick_Jump_Btn(GameObject caster)
    {
        //跳转到皇令购买
        PanelJumpData jumpData = new PanelJumpData();
        jumpData.Tabs = new int[1];
        jumpData.Tabs[0] = (int)MallPanel.TabMode.HuangLing;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MallPanel, jumpData: jumpData);
    }
    #endregion
}