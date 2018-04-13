using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


partial class TeamTargetPanel
{
    #region property


    /// <summary>
    /// 左边的活动
    /// </summary>
    Dictionary<uint, List<uint>> m_dicActivity = null;

    private UIGridCreatorBase m_TeamMainTargetCreator = null;

    private UIGridCreatorBase m_TeamIndexTargetCreator = null;

    /// <summary>
    /// 左侧大类型ID list
    /// </summary>
    List<uint> m_lstMainTargetId = new List<uint>();

    /// <summary>
    /// 右侧活动ID list
    /// </summary>
    List<uint> m_lstIndexTargetId = new List<uint>();

    /// <summary>
    /// 全部目标（无）
    /// </summary>
    private uint m_selectMainTargetId = TeamDataManager.wuId;

    /// <summary>
    /// 当前选中的活动目标
    /// </summary>
    private uint m_selectTargetId = 0;

    TeamDataManager TDManager
    {
        get
        {
            return DataManager.Manager<TeamDataManager>();
        }
    }
    #endregion


    #region override

    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    protected override void OnLoading()
    {
        base.OnLoading();

        InitWidget();
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        //初始化数据
        InitData();

        //左侧grid
        CreateMainTargetGrids();

        //第一次打开  默认选中第一个
        CleanAllMainTargetGridSelect();
        if (m_lstMainTargetId != null && m_lstMainTargetId.Count > 0)
        {
            SetSelectMainGrid(m_lstMainTargetId[0]);
        }
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

        if (m_TeamMainTargetCreator != null )
        {
            m_TeamMainTargetCreator.Release(depthRelease);
        }
          
        if (m_TeamIndexTargetCreator != null)
        {
            m_TeamIndexTargetCreator.Release(depthRelease);
        }
    }

    #endregion


    #region method

    //初始化数据
    void InitData()
    {
        m_dicActivity = TDManager.GetTeamActivityDic();
    }

    void InitWidget()
    {
        //main目标
        if (m_trans_left_Panel != null)
        {
            m_TeamMainTargetCreator = m_trans_left_Panel.GetComponent<UIGridCreatorBase>();
            if (m_TeamMainTargetCreator == null)
                m_TeamMainTargetCreator = m_trans_left_Panel.gameObject.AddComponent<UIGridCreatorBase>();

            if (m_TeamMainTargetCreator != null)
            {
                //UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uiteammaintargetgrid) as UnityEngine.GameObject;
                m_TeamMainTargetCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
                m_TeamMainTargetCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
                m_TeamMainTargetCreator.gridWidth = 200;
                m_TeamMainTargetCreator.gridHeight = 62;

                m_TeamMainTargetCreator.RefreshCheck();
                m_TeamMainTargetCreator.Initialize<UITeamMainTargetGrid>(m_trans_UITeamMainTargetGrid.gameObject, OnGridDataUpdate, OnGridUIEvent);
            }
        }

        //index目标
        if (m_trans_right_Panel != null)
        {
            m_TeamIndexTargetCreator = m_trans_right_Panel.GetComponent<UIGridCreatorBase>();
            if (m_TeamIndexTargetCreator == null)
                m_TeamIndexTargetCreator = m_trans_right_Panel.gameObject.AddComponent<UIGridCreatorBase>();

            if (m_TeamIndexTargetCreator != null)
            {
                //UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uiteamindextargetgrid) as UnityEngine.GameObject;
                m_TeamIndexTargetCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
                m_TeamIndexTargetCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
                m_TeamIndexTargetCreator.gridWidth = 460;
                m_TeamIndexTargetCreator.gridHeight = 70;

                m_TeamIndexTargetCreator.RefreshCheck();
                m_TeamIndexTargetCreator.Initialize<UITeamIndexTargetGrid>(m_trans_UITeamIndexTargetGrid.gameObject,  OnGridDataUpdate, OnGridUIEvent);
            }
        }
    }

    /// <summary>
    /// grid 跟新数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    private void OnGridDataUpdate(UIGridBase data, int index)
    {
        if (data is UITeamMainTargetGrid)
        {
            if (m_lstMainTargetId != null && index < m_lstMainTargetId.Count)
            {
                UITeamMainTargetGrid grid = data as UITeamMainTargetGrid;
                if (grid == null)
                {
                    return;
                }
                uint mainId = m_lstMainTargetId[index];

                grid.SetGridData(mainId);
                List<uint> activityIdList = m_dicActivity[mainId];
                if (mainId == TeamDataManager.wuId)
                {
                    grid.SetName("全部目标");
                }
                else
                {
                    TeamActivityDatabase db = GameTableManager.Instance.GetTableItem<TeamActivityDatabase>(activityIdList.Count > 0 ? activityIdList[0] : 0);
                    if (db != null)
                    {
                        grid.SetName(db.mainName);
                    }
                }

            }
        }
        else if (data is UITeamIndexTargetGrid)
        {
            if (m_lstIndexTargetId != null && index < m_lstIndexTargetId.Count)
            {
                UITeamIndexTargetGrid grid = data as UITeamIndexTargetGrid;
                if (grid == null)
                {
                    return;
                }

                grid.SetGridData(m_lstIndexTargetId[index]);

                if (m_lstIndexTargetId[index] == TeamDataManager.wuId)
                {
                    grid.SetName("全部目标");
                }
                else
                {
                    TeamActivityDatabase db = GameTableManager.Instance.GetTableItem<TeamActivityDatabase>(m_lstIndexTargetId[index]);
                    if (db != null)
                    {
                        grid.SetName(db.indexName);
                    }
                }
            }
        }
    }

    /// <summary>
    /// grid的点击事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            if (data is UITeamMainTargetGrid)
            {
                UITeamMainTargetGrid grid = data as UITeamMainTargetGrid;
                if (grid == null)
                {
                    return;
                }

                SetSelectMainGrid(grid.MainId);
            }
            else if (data is UITeamIndexTargetGrid)
            {
                UITeamIndexTargetGrid grid = data as UITeamIndexTargetGrid;
                if (grid == null)
                {
                    return;
                }

                SetSelectIndexGrid(grid.Id);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void CreateMainTargetGrids()
    {
        if (m_dicActivity == null)
        {
            return;
        }

        m_lstMainTargetId = new List<uint>(m_dicActivity.Keys);

        if (m_TeamMainTargetCreator != null)
        {
            m_TeamMainTargetCreator.CreateGrids((null != m_lstMainTargetId) ? m_lstMainTargetId.Count : 0);
        }
    }

    /// <summary>
    /// 创建左侧的grid
    /// </summary>
    void CreateIndexTargetGrids()
    {
        if (m_dicActivity == null)
        {
            return;
        }

        if (m_dicActivity.TryGetValue(m_selectMainTargetId, out m_lstIndexTargetId))
        {
            m_TeamIndexTargetCreator.CreateGrids((null != m_lstIndexTargetId) ? m_lstIndexTargetId.Count : 0);
        }
    }

    //清除MainTargetGrid 选中状态
    void CleanAllMainTargetGridSelect()
    {
        List<UITeamMainTargetGrid> list = m_TeamMainTargetCreator.GetGrids<UITeamMainTargetGrid>();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetSelect(false);
        }

    }

    //清除IndexTargetGrid 选中状态
    void CleanAllIndexTargetGridSelect()
    {
        List<UITeamIndexTargetGrid> list = m_TeamIndexTargetCreator.GetGrids<UITeamIndexTargetGrid>();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetSelect(false);
        }

    }

    void SetSelectMainGrid(uint mainId)
    {
        UITeamMainTargetGrid grid = m_TeamMainTargetCreator.GetGrid<UITeamMainTargetGrid>(m_lstMainTargetId.IndexOf(this.m_selectMainTargetId));
        if (grid != null)
        {
            grid.SetSelect(false);
        }

        grid = m_TeamMainTargetCreator.GetGrid<UITeamMainTargetGrid>(m_lstMainTargetId.IndexOf(mainId));
        if (grid != null)
        {
            grid.SetSelect(true);
        }

        this.m_selectMainTargetId = mainId;

        //创建格子
        CreateIndexTargetGrids();

        //清除之前的选中
        CleanAllIndexTargetGridSelect();
        //默认选择第一个格子
        if (m_dicActivity.TryGetValue(this.m_selectMainTargetId, out m_lstIndexTargetId))
        {
            if (m_lstIndexTargetId.Count >= 1)
            {
                SetSelectIndexGrid(m_lstIndexTargetId[0]);
            }
        }
    }

    /// <summary>
    /// 选中右侧grid
    /// </summary>
    /// <param name="targetId"></param>
    void SetSelectIndexGrid(uint targetId)
    {
        UITeamIndexTargetGrid grid = null;
        if (this.m_selectTargetId != 0)
        {
            grid = m_TeamIndexTargetCreator.GetGrid<UITeamIndexTargetGrid>(m_lstIndexTargetId.IndexOf(this.m_selectTargetId));
            if (grid != null)
            {
                grid.SetSelect(false);
            }
        }

        grid = m_TeamIndexTargetCreator.GetGrid<UITeamIndexTargetGrid>(m_lstIndexTargetId.IndexOf(targetId));
        if (grid != null)
        {
            grid.SetSelect(true);
        }

        this.m_selectTargetId = targetId;
    }


    #endregion


    #region click
    void onClick_Confirm_btn_Btn(GameObject caster)
    {
        if (m_selectTargetId != 0)
        {
            if (m_toggle_Match_Box.GetComponent<UIToggle>().value)
            {
                TDManager.ReqAutoMatch(m_selectTargetId);//自动匹配
            }
            else
            {
                TDManager.ReqCancelMatch();//取消匹配              
            }

            if (TDManager.MainPlayerIsLeader())
            {
                TDManager.ReqMatchActivity(m_selectTargetId);//队长选择活动目标
            }
        }

        HideSelf();
    }

    void onClick_Btn_close_Btn(GameObject caster)
    {
        HideSelf();
    }

    #endregion
}

