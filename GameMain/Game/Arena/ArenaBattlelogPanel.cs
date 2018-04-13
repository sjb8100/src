using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameCmd;
using table;


/// <summary>
/// 战报
/// </summary> 
partial class ArenaBattlelogPanel : UIPanelBase
{
    private UIGridCreatorBase battlelogGridCreator;
    private List<ArenaBattleLog> battlelogList;

    #region override
    protected override void OnLoading()
    {

    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    /// <summary>
    /// 界面显示回调
    /// </summary>
    /// <param name="data"></param>
    protected override void OnShow(object data)
    {
        base.OnShow(data);

        InitBattlelog();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eArenaBattlelog)
        {
            InitBattlelog();
        }

        return true;
    }

    protected override void OnHide()
    {
        Release();
    }


    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (battlelogGridCreator != null)
        {
            battlelogGridCreator.Release(depthRelease);
        }

    }

    #endregion



    void InitBattlelog()
    {
        battlelogList = DataManager.Manager<ArenaManager>().BattlelogList;

        // if (rankGridCreator == null)
        {
            //UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uiarenabattleloggrid) as UnityEngine.GameObject;
            battlelogGridCreator = m_trans_UIBattlelogScrollView.GetComponent<UIGridCreatorBase>();
            if (battlelogGridCreator == null)
                battlelogGridCreator = m_trans_UIBattlelogScrollView.gameObject.AddComponent<UIGridCreatorBase>();
            battlelogGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            battlelogGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            battlelogGridCreator.gridWidth = 766;
            battlelogGridCreator.gridHeight = 65;
            battlelogGridCreator.rowcolumLimit = 1;
            battlelogGridCreator.RefreshCheck();
            battlelogGridCreator.Initialize<UIArenaBattlelogGrid>(m_trans_UIArenaBattlelogGrid.gameObject, OnBattlelogGridDataUpdate, OnBattlelogGridUIEvent);
            battlelogGridCreator.CreateGrids((null != battlelogList) ? battlelogList.Count : 0);
        }
    }

    private void OnBattlelogGridDataUpdate(UIGridBase data, int index)
    {
        if (null != battlelogList && index < battlelogList.Count)
        {
            UIArenaBattlelogGrid grid = data as UIArenaBattlelogGrid;
            if (grid != null)
            {
                grid.SetGridData(battlelogList[index]);

                //名字
                grid.SetName(battlelogList[index].name);
                grid.SetJob(battlelogList[index].job, battlelogList[index].sex);
            }
        }
    }

    /// <summary>
    /// 格子点击事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnBattlelogGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                UIArenaBattlelogGrid grid = data as UIArenaBattlelogGrid;
                if (grid != null)
                {
                    DataManager.Instance.Sender.RequestPlayerInfoForOprate(grid.battlelogInfo.userid, PlayerOpreatePanel.ViewType.Normal);
                }
                break;
        }
    }


    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }
}

