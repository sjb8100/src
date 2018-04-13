using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class FishingRankPanel
{
    #region interface

    List<FishingRankInfo> m_fishingRankInfoList;

    private UIGridCreatorBase m_FishingRankCreator = null;

    #endregion

    #region override


    protected override void OnLoading()
    {
        base.OnLoading();

        InitWidget();
    }

    void InitWidget()
    {
        m_FishingRankCreator = m_trans_fishingrankscrollview.GetComponent<UIGridCreatorBase>();
        if (m_FishingRankCreator == null)
            m_FishingRankCreator = m_trans_fishingrankscrollview.gameObject.AddComponent<UIGridCreatorBase>();

        if (m_FishingRankCreator != null)
        {
            m_FishingRankCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            m_FishingRankCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
            m_FishingRankCreator.gridWidth = 1150;
            m_FishingRankCreator.gridHeight = 48;

            m_FishingRankCreator.RefreshCheck();
            m_FishingRankCreator.Initialize<UIFishingRankGrid>(m_widget_UIFishingRankGrid.gameObject, OnGridDataUpdate, OnGridUIEvent);
        }
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object data)
    {
        CreateGrid();

        UpdateMyRank();
    }

    protected override void OnHide()
    {
        base.OnHide();

        Release();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();

    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_FishingRankCreator != null)
        {
            m_FishingRankCreator.Release(depthRelease);
        }
    }

    void ReleaseGrid()
    {

    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);

    }




    public override bool OnMsg(UIMsgID msgid, object param)
    {

        if (msgid == UIMsgID.eFishingRank)
        {
            CreateGrid();

            UpdateMyRank();
        }


        return true;
    }

    #endregion

    #region method

    void CreateGrid()
    {
        m_fishingRankInfoList = DataManager.Manager<FishingManager>().FishingRankInfoList;

        if (m_FishingRankCreator != null)
        {
            m_FishingRankCreator.CreateGrids(m_fishingRankInfoList != null ? m_fishingRankInfoList.Count : 0);
        }
    }

    /// <summary>
    /// 跟新排行
    /// </summary>
    void UpdateRankList()
    {

    }

    void UpdateMyRank()
    {
        m_label_myRanknum_label.text = DataManager.Manager<FishingManager>().FishingRank.ToString();

        m_label_myScorenum_label.text = DataManager.Manager<FishingManager>().FishingScore.ToString();
    }

    private void OnGridDataUpdate(UIGridBase data, int index)
    {
        UIFishingRankGrid grid = data as UIFishingRankGrid;
        if (data != null)
        {
            if (m_fishingRankInfoList != null && index < m_fishingRankInfoList.Count)
            {
                grid.SetGridData(m_fishingRankInfoList[index]);
                grid.SetRank(m_fishingRankInfoList[index].rank);
                grid.SetName(m_fishingRankInfoList[index].name);
                grid.SetClanName(m_fishingRankInfoList[index].clanName);
                grid.SetNum(m_fishingRankInfoList[index].num);
                grid.SetScore(m_fishingRankInfoList[index].score);
            }
        }
    }

    private void OnGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {

        }
    }

    #endregion

    #region click
    void onClick_Btn_close_Btn(GameObject caster)
    {
        HideSelf();
    }

    #endregion

}

