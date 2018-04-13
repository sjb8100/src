//*************************************************************************
//	创建日期:	2016-11-30 17:33
//	文件名称:	CampWarBattleListPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	阵营战 结算面板
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;


partial class CampWarBattleListPanel : UIPanelBase,Engine.Utility.ITimer
{
    CampCombatManager m_dataMgr = null;
    List<CampCombatResultInfo.CampCombatPlayerInfo> m_lstCampCombatGod = null;
    List<CampCombatResultInfo.CampCombatPlayerInfo> m_lstCampCombatDemon = null;
    const uint REFRESH_CAMP_USE_TIME_ID = 10086;
    protected override void OnLoading()
    {
        base.OnLoading();
        m_dataMgr = DataManager.Manager<CampCombatManager>();
        OnInitGrid();
    }


    void OnInitGrid()
    {
        UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uicampwarbattlelistgrid);
        if (null != m_ctor_GodScrollView)
        {
            m_ctor_GodScrollView.RefreshCheck();
            m_ctor_GodScrollView.Initialize<UICampWarBattleListGrid>((uint)GridID.Uicampwarbattlelistgrid, UIManager.OnObjsCreate, UIManager.OnObjsRelease, OnGodCampDataUpdate, null);
        }

        if (null != m_ctor_DemonScrollView)
        {
            m_ctor_DemonScrollView.RefreshCheck();
            m_ctor_DemonScrollView.Initialize<UICampWarBattleListGrid>((uint)GridID.Uicampwarbattlelistgrid, UIManager.OnObjsCreate, UIManager.OnObjsRelease, OnDemonCampDataUpdate, null);
        }
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_dataMgr.ValueUpdateEvent += OnValueUpdateEventArgs;
        Engine.Utility.TimerAxis.Instance().SetTimer(REFRESH_CAMP_USE_TIME_ID, 1000, this, Engine.Utility.TimerAxis.INFINITY_CALL);
        RefreshBattleList(true);
    }

    
    protected override void OnHide()
    {
        base.OnHide();
        m_dataMgr.ValueUpdateEvent -= OnValueUpdateEventArgs;
        Engine.Utility.TimerAxis.Instance().KillTimer(REFRESH_CAMP_USE_TIME_ID, this);

        Release();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_ctor_GodScrollView != null)
        {
            m_ctor_GodScrollView.Release(depthRelease);
        }

        if (m_ctor_DemonScrollView != null)
        {
            m_ctor_DemonScrollView.Release(depthRelease);
        }
    }

    /// <summary>
    /// 根据角色当前的阵营更新玩家信息的位置
    /// </summary>
    private void UpdateMyInfoPosByCamp()
    {
        if (null != m_trans_MyRankMessage)
        {
            Vector3 pos = m_trans_MyRankMessage.transform.localPosition;
            pos.x = (m_dataMgr.CampCombatResultData.m_MyCampCombatInfo.camp 
                == GameCmd.eCamp.CF_Green) ? -Mathf.Abs(pos.x) : Mathf.Abs(pos.x);
            m_trans_MyRankMessage.localPosition = pos;
        }
    }

    private void RefreshCampUseTime()
    {
        TimeSpan ts = new TimeSpan(0, 0, m_dataMgr.CampFightUseTime);
        m_label_BattleTimeNum.text = string.Format("{0}:{1}", ts.Minutes.ToString("D2"), ts.Seconds.ToString("D2"));
    }

    private void ResetBattleListData()
    {
        m_label_BattleTimeNum.text = "00:00";
        m_label_GodScore.text = "0";
        m_label_DemonScore.text = "0";
        m_label_GodNum.text = "0";
        m_label_DemonNum.text = "0";

        m_label_MyName.text = "--";
        m_label_MyKill.text = "0";
        m_label_MyDead.text = "0";
        m_label_MyRank.text = "0";
        m_label_MyAssists.text = "0";
        m_label_MyScore.text = "0";

        if (m_ctor_GodScrollView != null)
        {
            m_ctor_GodScrollView.CreateGrids(0);
        }

        if (m_ctor_DemonScrollView != null)
        {
            m_ctor_DemonScrollView.CreateGrids(0);
        }
    }

    private void RefreshBattleList(bool build = false)
    {
        if (!m_dataMgr.CampBattleEnable)
        {
            ResetBattleListData();
            return;
        }
        CampCombatResultInfo campCombatResultInfo = m_dataMgr.CampCombatResultData;
        if (campCombatResultInfo == null)
        {
            return;
        }
        SetMyRankUI(campCombatResultInfo.m_MyCampCombatInfo);
        CampCombatResultInfo.CampCombatResult campCombatResult = campCombatResultInfo.m_camp_Green;
        if (campCombatResult != null)
        {
            m_lstCampCombatGod = campCombatResult.m_lstCampCombatPlayers;

            m_label_CampWarGodBossNum.text = campCombatResult.nKillBossNum.ToString();
            m_label_CampWarGodReLivePointNum.text = campCombatResult.nReliveNum.ToString();
            m_label_GodScore.text = campCombatResult.nScore.ToString();
            m_label_GodNum.text = (null != campCombatResult.m_lstCampCombatPlayers)
                ? campCombatResult.m_lstCampCombatPlayers.Count.ToString() : "0";
        }

        campCombatResult = campCombatResultInfo.m_camp_Red;
        if (campCombatResult != null)
        {
            m_lstCampCombatDemon = campCombatResult.m_lstCampCombatPlayers;

            m_label_DemonScore.text = campCombatResult.nScore.ToString();
            m_label_CampWarDemonBossNum.text = campCombatResult.nKillBossNum.ToString();
            m_label_CampWarDemonReLivePointNum.text = campCombatResult.nReliveNum.ToString();
            m_label_DemonNum.text = (null != campCombatResult.m_lstCampCombatPlayers)
                ? campCombatResult.m_lstCampCombatPlayers.Count.ToString() : "0";
        }
        

        if (build)
        {
            int count = (null != m_lstCampCombatGod) ? m_lstCampCombatGod.Count : 0;
            if (m_ctor_GodScrollView != null)
            {
                m_ctor_GodScrollView.CreateGrids(count);
            }

            count = (null != m_lstCampCombatDemon) ? m_lstCampCombatDemon.Count : 0;
            if (m_ctor_DemonScrollView != null)
            {
                m_ctor_DemonScrollView.CreateGrids(count);
            }
        }else
        {
            if (m_ctor_GodScrollView != null)
            {
                m_ctor_GodScrollView.UpdateActiveGridData();
            }

            if (m_ctor_DemonScrollView != null)
            {
                m_ctor_DemonScrollView.UpdateActiveGridData();
            }
        }
        RefreshCampUseTime();
        UpdateMyInfoPosByCamp();
    }

    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs value)
    {
        if (value.key.Equals("CampKillInfo") || value.key.Equals("RefreshBattleData"))
        {
            RefreshBattleList();
        }
    }

    private void SetMyRankUI(CampCombatResultInfo.CampCombatPlayerInfo selfCampCombatInfo)
    {
        if (selfCampCombatInfo != null)
        {
            m_label_MyName.text = selfCampCombatInfo.strName;
            m_label_MyKill.text = selfCampCombatInfo.nKill.ToString();
            m_label_MyDead.text = selfCampCombatInfo.nDead.ToString();
            m_label_MyRank.text = selfCampCombatInfo.nRank.ToString();
            m_label_MyAssists.text = selfCampCombatInfo.nAssist.ToString();
            m_label_MyScore.text = selfCampCombatInfo.nScore.ToString();
        }
    }

    void OnGodCampDataUpdate(UIGridBase data, int index)
    {
        if (null != m_lstCampCombatGod && index < m_lstCampCombatGod.Count)
        {
            data.SetGridData(m_lstCampCombatGod[index]);
        }
    }

    void OnDemonCampDataUpdate(UIGridBase data, int index)
    {
        if (null != m_lstCampCombatDemon && index < m_lstCampCombatDemon.Count)
          {
              data.SetGridData(m_lstCampCombatDemon[index]);
          }
    }

    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    public void OnTimer(uint timerID)
    {
        if (timerID == REFRESH_CAMP_USE_TIME_ID)
            RefreshCampUseTime();
    }

    
}
