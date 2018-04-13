using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using table;
using Engine;
using Client;
using GameCmd;
using System.Text;
using Common;
partial class ActivityRewardListPanel
{
    List<RecRetRewardData> m_lst_RewDatas = null;
    protected override void OnLoading()
    {
        base.OnLoading();
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        DataManager.Manager<ActivityManager>().ValueUpdateEvent += OnUpdateList;
        InitPanel();
    }
    void InitPanel() 
    {
        if (m_ctor_ScrollView != null)
        {
            m_ctor_ScrollView.Initialize<UIActivityRewardListGrid>(m_trans_UIActivityRewardListGrid.gameObject, OnActivityListUpdate, OnActivityGridUIEvent);
            m_lst_RewDatas = DataManager.Manager<ActivityManager>().RewRetReward;
            if (m_lst_RewDatas != null)
            {
                m_ctor_ScrollView.CreateGrids(m_lst_RewDatas.Count);
            }
        }
    }
    void OnActivityListUpdate(UIGridBase grid, int index)
    {
        if (grid is UIActivityRewardListGrid)
        {
            UIActivityRewardListGrid data = grid as UIActivityRewardListGrid;
            if (m_lst_RewDatas != null)
            {
                if (index < m_lst_RewDatas.Count)
               {
                   data.SetGridData(m_lst_RewDatas[index]);
               }
            }           
        }
    }

    void OnActivityGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UIActivityRewardListGrid)
                {
                    UIActivityRewardListGrid toggleGrid = data as UIActivityRewardListGrid;
                    if (toggleGrid != null)
                    {
                        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ActivityRewardPanel, data: toggleGrid.m_data);
                    }
                }
                break;
        }
    }
   
    protected override void OnHide()
    {
        base.OnHide();
        DataManager.Manager<ActivityManager>().ValueUpdateEvent -= OnUpdateList;
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void OnUpdateList(object obj, ValueUpdateEventArgs value)
    {
        if (value.key.Equals("OnGetRechargeRewRet"))
        {
            ulong id = (ulong)value.oldValue;
            for (int i = 0; i < m_lst_RewDatas.Count;i++ )
            {
                if (m_lst_RewDatas[i].rewardid == id)
                {
                    uint ret = (uint)value.newValue;
                    m_lst_RewDatas[i].state = ret;
                    m_ctor_ScrollView.UpdateActiveGridData();
                }
            }
        }
    }
}

