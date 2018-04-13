using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
using System.Collections;
using GameCmd;
using table;

partial class ConvenientTeamPanel : UIPanelBase
{
    /// <summary>
    /// 表中活动数据
    /// </summary>
    List<TeamActivityDatabase> tableActivityList;

    /// <summary>
    /// 右侧的队伍list
    /// </summary>
    UIGridCreatorBase m_existedTeamGridCreator;

    /// <summary>
    /// 左侧活动列表(带二级页签)
    /// </summary>
    UISecondTabCreatorBase m_secondsTabCreator;

    public TeamDataManager TDManager
    {
        get
        {
            return DataManager.Manager<TeamDataManager>();
        }
    }

    /// <summary>
    /// 左边的活动数据
    /// </summary>
    Dictionary<uint, List<uint>> m_dicActivity = new Dictionary<uint, List<uint>>();

    /// <summary>
    /// 大类ID
    /// </summary>
    uint m_selectTypeId;

    /// <summary>
    /// 点击中的活动ID
    /// </summary>
    uint m_selectActivityId;

    /// <summary>
    /// 目标活动ID
    /// </summary>
    uint m_targetActivityId;

    #region Override
    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidget();
        tableActivityList = GameTableManager.Instance.GetTableList<TeamActivityDatabase>();

    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);

        // DataManager.Manager<TeamDataManager>().ReqTeamListByTarget(1, 0);//附近的队伍
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        //创建左侧二级页签
        CreateActivityGrid();

        //右边的gird
        CreateTeamGrids();

        // 设置匹配按钮
        SetMatchBtn(TeamDataManager.nearbyId);
    }

    protected override void OnHide()
    {
        base.OnHide();
        CleanUIData();

        Release();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_existedTeamGridCreator != null)
        {
            m_existedTeamGridCreator.Release(depthRelease);
        }
    }

    #endregion

    #region method
    void InitWidget()
    {
        if (m_trans_TeamListScrollView != null)
        {
            m_existedTeamGridCreator = m_trans_TeamListScrollView.GetComponent<UIGridCreatorBase>();
            if (null == m_existedTeamGridCreator)
                m_existedTeamGridCreator = m_trans_TeamListScrollView.gameObject.AddComponent<UIGridCreatorBase>();

            //UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uiexistedteamgrid) as GameObject;
            m_existedTeamGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            m_existedTeamGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
            m_existedTeamGridCreator.gridWidth = 780;
            m_existedTeamGridCreator.gridHeight = 100;

            m_existedTeamGridCreator.RefreshCheck();
            m_existedTeamGridCreator.Initialize<UIExistedTeamGrid>(m_sprite_UIExistedTeamGrid.gameObject, OnTeamGridDataUpdate, OnTeamGridUIEvent);
        }

        if (m_scrollview_TargetScrollView != null)
        {
            m_secondsTabCreator = m_scrollview_TargetScrollView.gameObject.GetComponent<UISecondTabCreatorBase>();
            if (m_secondsTabCreator == null)
            {
                m_secondsTabCreator = m_scrollview_TargetScrollView.gameObject.AddComponent<UISecondTabCreatorBase>();

                //UnityEngine.GameObject fObj = UIManager.GetResGameObj(GridID.Uititlectrtypegrid) as GameObject;
                UnityEngine.GameObject fObj = UIManager.GetResGameObj(GridID.Uictrtypegrid) as GameObject;
                UnityEngine.GameObject sObj = UIManager.GetResGameObj(GridID.Uiteamactivitychildgrid) as GameObject;

                m_secondsTabCreator.Initialize<UITeamActivityChildGrid>(fObj,sObj, OnUpdateCtrTypeGrid, OnUpdateSecondGrid, OnGridUIEventDlg);
            }
        }
    }

    /// <summary>
    /// 创建左侧组队活动目标grid 
    /// </summary>
    void CreateActivityGrid()
    {
        List<int> list = new List<int>();

        m_dicActivity = DataManager.Manager<TeamDataManager>().GetConvenientTeamActivityDic();
        Dictionary<uint, List<uint>>.Enumerator enumerator = m_dicActivity.GetEnumerator();
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current.Value.Count);
        }

        m_secondsTabCreator.CreateGrids(list);

        SetStartSelectGrid(); //初始选中
    }

    /// <summary>
    /// 初始选中
    /// </summary>
    void SetStartSelectGrid()
    {
        uint selectTargetId = TDManager.ConveientSelectTargetId;

        TeamActivityDatabase teamActivityDb = GameTableManager.Instance.GetTableItem<TeamActivityDatabase>(selectTargetId);

        uint firstTypeId;
        uint secondTypeId;
        if (teamActivityDb == null)
        {
            firstTypeId = TeamDataManager.nearbyId;
            secondTypeId = TeamDataManager.nearbyId;
        }
        else
        {
            firstTypeId = teamActivityDb.mainID;
            secondTypeId = teamActivityDb.ID;
        }

        //选中页签
        SetSelectFirstType(firstTypeId);

        //选中二级页签
        SetSelectSecondType(secondTypeId);

        ////选中页签
        //SetSelectFirstType(TeamDataManager.nearbyId);

        ////选中二级页签
        //SetSelectSecondType(TeamDataManager.nearbyId);

    }

    private void OnUpdateCtrTypeGrid(UIGridBase gridBase, int index)
    {
        if (index < m_dicActivity.Keys.Count)
        {
            UICtrTypeGrid grid = gridBase as UICtrTypeGrid;
            if (grid == null)
            {
                return;
            }

            List<uint> typeIdList = m_dicActivity.Keys.ToList<uint>();
            uint typeId = typeIdList[index];

            List<uint> teamActivityIdList;
            if (m_dicActivity.TryGetValue(typeId, out teamActivityIdList))
            {
                string typeName = TDManager.GetConvenientTeamActivityFirstTypeName(typeId, teamActivityIdList.Count > 0 ? teamActivityIdList[0] : 0);
                grid.SetData(typeId, typeName, teamActivityIdList.Count);
            }
        }
    }

    private void OnUpdateSecondGrid(UIGridBase gridBase, object id, int index)
    {
        UITeamActivityChildGrid grid = gridBase as UITeamActivityChildGrid;
        if (grid == null)
        {
            return;
        }

        List<uint> teamActivityIdList;
        if (m_dicActivity.TryGetValue((uint)id, out teamActivityIdList))
        {
            if (index > teamActivityIdList.Count)
            {
                return;
            }

            grid.SetGridData(teamActivityIdList[index]);
            grid.SetSelect(m_selectActivityId == teamActivityIdList[index]);

            TeamActivityDatabase db = tableActivityList.Find((data) => { return data.ID == teamActivityIdList[index]; });
            grid.SetName(db != null ? db.indexName : "");

            grid.SetTargetMark(false);
        }
    }

    private void OnGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            if (data is UICtrTypeGrid)
            {
                UICtrTypeGrid grid = data as UICtrTypeGrid;
                if (grid == null)
                {
                    return;
                }

                SetSelectFirstType((uint)grid.ID);
            }

            if (data is UITeamActivityChildGrid)
            {
                UITeamActivityChildGrid grid = data as UITeamActivityChildGrid;
                if (grid == null)
                {
                    return;
                }

                SetSelectSecondType(grid.Id);

                //请求当前活动队伍
                DataManager.Manager<TeamDataManager>().ReqConvenientTeamListByTeamActivityId(m_selectActivityId);
                SetMatchBtn(m_selectTypeId);
            }
        }
    }

    /// <summary>
    /// 创建队伍grid list
    /// </summary>
    void CreateTeamGrids()
    {
        if (TDManager.ConvenientTeamList != null)
        {
            m_existedTeamGridCreator.CreateGrids(TDManager.ConvenientTeamList != null ? TDManager.ConvenientTeamList.Count : 0);
        }
    }

    void OnTeamGridDataUpdate(UIGridBase data, int index)
    {
        if (TDManager.ConvenientTeamList != null && index < TDManager.ConvenientTeamList.Count)
        {
            UIExistedTeamGrid grid = data as UIExistedTeamGrid;
            if (grid != null)
            {
                grid.SetGridData(TDManager.ConvenientTeamList[index]);

                grid.SetName(grid.m_teamInfo.teamname);
                grid.SetIcon(grid.m_teamInfo.byjob);
                grid.SetLv(grid.m_teamInfo.byLevel);

                TeamActivityDatabase db = GameTableManager.Instance.GetTableItem<TeamActivityDatabase>(TDManager.ConvenientTeamList[index].active_id);
                if (db != null)
                {
                    grid.SetTargetName(db.indexName);
                }
                else
                {
                    grid.SetTargetName("无");
                }
            }
        }
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    void OnTeamGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UIExistedTeamGrid grid = data as UIExistedTeamGrid;
            if (data == null)
            {
                return;
            }

            if (param != null)
            {
                int btnIndex = (int)param;

                //申请
                int applyBtnIndex = 1;
                if (btnIndex == applyBtnIndex)
                {
                    DataManager.Manager<TeamDataManager>().ReqJoinTeam(grid.m_teamInfo.leaderID);
                }

                //点icon
                int iconBtnIndex = 2;
                if (btnIndex == iconBtnIndex)
                {
                    DataManager.Instance.Sender.RequestPlayerInfoForOprate(grid.m_teamInfo.leaderID, PlayerOpreatePanel.ViewType.Normal);
                }
            }

        }
    }

    /// <summary>
    /// 设置选中第一分页
    /// </summary>
    /// <param name="type"></param>
    private void SetSelectFirstType(uint id, bool force = false)
    {
        //点到 附近grid
        if (id == TeamDataManager.nearbyId)
        {
            OnClickFuJin();
            m_selectTypeId = id;
            m_selectActivityId = 0;
            SetMatchBtn(m_selectTypeId);
            return;
        }

        //全部队伍
        if (id == TeamDataManager.allTeamId)
        {
            OnClickAllTeam();
            m_selectTypeId = id;
            m_selectActivityId = 0;
            SetMatchBtn(m_selectTypeId);
            return;
        }

        List<uint> typeIdList = m_dicActivity.Keys.ToList<uint>();

        List<uint> teamActivityIdList;
        UITeamActivityParentGrid grid;
        if (id == m_selectTypeId)
        {
            if (m_dicActivity.TryGetValue(m_selectTypeId, out teamActivityIdList) && m_secondsTabCreator.IsOpen(typeIdList.IndexOf(m_selectTypeId)))
            {
                m_secondsTabCreator.Close(typeIdList.IndexOf(m_selectTypeId));
                return;
            }
        }

        if (m_dicActivity.TryGetValue(m_selectTypeId, out teamActivityIdList))
        {
            m_secondsTabCreator.Close(typeIdList.IndexOf(m_selectTypeId));
        }

        if (m_dicActivity.TryGetValue(id, out teamActivityIdList))
        {
            m_secondsTabCreator.Open(typeIdList.IndexOf(id));
        }

        m_selectTypeId = id;

        SetMatchBtn(m_selectTypeId);
    }

    /// <summary>
    /// 选中二级页签
    /// </summary>
    /// <param name="activityId"></param>
    private void SetSelectSecondType(uint activityId)
    {
        if (m_selectActivityId == activityId)
        {
            return;
        }

        UITeamActivityChildGrid grid;

        grid = GetSecondGrid(m_selectActivityId);
        if (grid != null)
        {
            grid.SetSelect(false);
        }

        grid = GetSecondGrid(activityId);
        if (grid != null)
        {
            grid.SetSelect(true);
        }

        this.m_selectActivityId = activityId;
    }

    /// <summary>
    /// 设置匹配按钮
    /// </summary>
    /// <param name="typeId"></param>
    void SetMatchBtn(uint typeId)
    {
        if (typeId == TeamDataManager.nearbyId || typeId == TeamDataManager.allTeamId)
        {
            m_btn_btn_automatch.isEnabled = false;
            m_label_btn_automatchLabel.text = "自动匹配";
        }
        else
        {
            m_btn_btn_automatch.isEnabled = true;
        }
    }

    void CleanUIData()
    {
        m_selectTypeId = 0;

        m_selectActivityId = 0;
    }

    /// <summary>
    /// 点击附近
    /// </summary>
    void OnClickFuJin()
    {
        TDManager.ReqConvenientTeamListByTeamActivityId(TeamDataManager.nearbyId);
    }

    /// <summary>
    /// 所有队伍
    /// </summary>
    void OnClickAllTeam()
    {
        TDManager.ReqConvenientTeamListByTeamActivityId(TeamDataManager.allTeamId);
    }

    /// <summary>
    /// 自动匹配
    /// </summary>
    void AutoMatch(uint activityId)
    {
        if (activityId == TeamDataManager.nearbyId || activityId == TeamDataManager.allTeamId)
        {
            return;
        }

        //设置当前目标
        SetTarget(activityId);

        m_label_btn_automatchLabel.text = "取消匹配";
    }

    /// <summary>
    /// 取消匹配
    /// </summary>
    void CancelMatch()
    {
        m_label_btn_automatchLabel.text = "自动匹配";

        UITeamActivityChildGrid grid = GetSecondGrid(m_targetActivityId);
        if (grid != null)
        {
            grid.SetTargetMark(false);
        }

        m_targetActivityId = 0;
    }

    /// <summary>
    /// 设置当前正在匹配的目标
    /// </summary>
    /// <param name="activityId"></param>
    void SetTarget(uint activityId)
    {
        UITeamActivityChildGrid grid;

        grid = GetSecondGrid(m_targetActivityId);
        if (grid != null)
        {
            grid.SetTargetMark(false);
        }


        grid = GetSecondGrid(activityId);
        if (grid != null)
        {
            grid.SetTargetMark(true);
        }

        m_targetActivityId = activityId;
    }

    /// <summary>
    /// 获取二级页签
    /// </summary>
    /// <param name="activityId">唯一ID</param>
    /// <returns></returns>
    UITeamActivityChildGrid GetSecondGrid(uint activityId)
    {
        TeamActivityDatabase teamActivityDb = tableActivityList.Find((data) => { return data.ID == activityId; });

        if (teamActivityDb == null)
        {
            return null;
        }

        uint typeId = teamActivityDb.mainID;
        List<uint> typeIdList = m_dicActivity.Keys.ToList<uint>();
        List<uint> teamActivityIdList;

        if (m_dicActivity.TryGetValue(typeId, out teamActivityIdList))
        {
            UITeamActivityChildGrid grid = m_secondsTabCreator.GetGrid<UITeamActivityChildGrid>(typeIdList.IndexOf(typeId), teamActivityIdList.IndexOf(activityId));
            if (grid != null)
            {
                return grid;
            }
        }

        return null;
    }


    #endregion

    #region BtnClick


    void onClick_Btn_refresh_Btn(GameObject caster)
    {
        if (m_selectActivityId != 0)
        {
            DataManager.Manager<TeamDataManager>().ReqConvenientTeamListByTeamActivityId(m_selectActivityId);
        }
        else
        {
            if (m_selectTypeId == TeamDataManager.nearbyId)
            {
                DataManager.Manager<TeamDataManager>().ReqConvenientTeamListByTeamActivityId(TeamDataManager.nearbyId);
            }

            if (m_selectTypeId == TeamDataManager.allTeamId)
            {
                DataManager.Manager<TeamDataManager>().ReqConvenientTeamListByTeamActivityId(TeamDataManager.allTeamId);
            }
        }

    }

    /// <summary>
    /// 创建队伍
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_createteam_Btn(GameObject caster)
    {
        if (m_selectTypeId == TeamDataManager.nearbyId || m_selectTypeId == TeamDataManager.allTeamId)
        {
            TDManager.ReqConveientCreateTeam(m_selectTypeId);
        }
        else
        {
            if (m_selectActivityId != 0)
            {
                TDManager.ReqConveientCreateTeam(m_selectActivityId);
            }
        }

        this.HideSelf();
    }

    /// <summary>
    /// 自动匹配
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_automatch_Btn(GameObject caster)
    {
        UILabel btnLabel = m_btn_btn_automatch.GetComponentInChildren<UILabel>();

        if (TDManager.IsConvenientTeamMatch == false)
        {
            if (m_selectActivityId != 0)
            {
                DataManager.Manager<TeamDataManager>().ReqAutoMatch(m_selectActivityId);
            }
            else
            {

                Debug.LogError("请选择活动！！！！！！！！！！");
            }
        }
        else
        {
            DataManager.Manager<TeamDataManager>().ReqCancelMatch();
        }
    }

    /// <summary>
    /// 切换目标（没使用此按钮）
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_changeTarget_Btn(GameObject caster)
    {
    }

    #endregion

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eUpdateExistedTeamList)
        {
            CreateTeamGrids();
        }
        else if (msgid == UIMsgID.eTeamMatch)
        {
            stTeamActivityTarget target = (stTeamActivityTarget)param;
            AutoMatch(target.activityTargetId);
        }
        else if (msgid == UIMsgID.eTeamCancleMatch)
        {
            CancelMatch();
        }

        return true;
    }

    ////////////////////////////////////////////////////////// old Code ///////////////////////////////////////////////////////////////////////////////////////////////

    /*
    
    List<UITeamActivityParentGrid> m_parentGridList = new List<UITeamActivityParentGrid>();
    List<UITeamActivityChildGrid> m_childGridList = new List<UITeamActivityChildGrid>();
     
    /// <summary>
    /// 创建左边grid
    /// </summary>
    void CreateLeftParentGrid()
    {
        CleanGrid();

        Dictionary<uint, List<TeamActivityDatabase>>.Enumerator enumerator = m_dicActivity.GetEnumerator();

        UITeamActivityParentGrid parentGridTemp = null;
        UITeamActivityChildGrid childGridTemp = null;

        while (enumerator.MoveNext())
        {
            List<TeamActivityDatabase> list = enumerator.Current.Value;
            if (list == null)
            {
                continue;
            }

            parentGridTemp = GetTypeGrid<UITeamActivityParentGrid>(true);
            if (parentGridTemp == null)
            {
                continue;
            }

            parentGridTemp.transform.parent = m__uitable.transform;
            parentGridTemp.transform.localPosition = Vector3.zero;
            parentGridTemp.transform.localScale = Vector3.one;
            parentGridTemp.transform.localRotation = Quaternion.identity;
            parentGridTemp.gameObject.SetActive(true);

            parentGridTemp.SetGridData(enumerator.Current.Key);
            parentGridTemp.SetName(list[0].mainName);
            parentGridTemp.EnableArrow(true);
            parentGridTemp.SetHightLight(false);
            parentGridTemp.SetTargetMark(false);
            parentGridTemp.RegisterUIEventDelegate(OnGridEventDlg);

            if (enumerator.Current.Key != 1 && enumerator.Current.Key != 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    childGridTemp = GetTypeGrid<UITeamActivityChildGrid>(false);
                    if (childGridTemp == null)
                    {
                        continue;
                    }

                    childGridTemp.SetData(list[i], list[i].indexName, false);
                    childGridTemp.SetTargetMark(false);
                    childGridTemp.SetHightLight(false);
                    childGridTemp.RegisterUIEventDelegate(OnGridEventDlg);

                    parentGridTemp.Add(childGridTemp.CacheTransform);

                    m_childGridList.Add(childGridTemp);
                }
            }

            parentGridTemp.UpdatePostion();
            m_parentGridList.Add(parentGridTemp);
        }

        m__uitable.repositionNow = true;

        //选中页签
        if (TDManager.ConvenientTeamList != null)
        {
            uint mainId = 1;//1为附近的队伍
            uint indexId = 0;
            if (TDManager.ConvenientTeamList.Count > 0)
            {
                mainId = TDManager.ConvenientTeamList[0].team_active;
                indexId = TDManager.ConvenientTeamList[0].team_target;
            }

            SetSelectFirstType(mainId);
            List<TeamActivityDatabase> nowTeamActivityList;
            if (m_dicActivity.TryGetValue(mainId, out nowTeamActivityList))
            {
                TeamActivityDatabase tadb = nowTeamActivityList.Find((data) => { return data.mainID == mainId && data.indexID == indexId; });
                SetSelectSecondType(tadb);
            }
        }
    }

    private T GetTypeGrid<T>(bool firstType) where T : UIGridBase
    {
        T target = default(T);
        GameObject obj = null;
        GameObject resObj = null;
        if (firstType)
        {
            resObj = UIManager.GetResGameObj(GridID.Uiteamactivityparentgrid) as GameObject;
        }
        else
        {
            resObj = UIManager.GetResGameObj(GridID.Uiteamactivitychildgrid) as GameObject;
        }
        if (null != resObj)
        {
            obj = Instantiate(resObj) as GameObject;
            if (null != obj)
            {
                target = obj.GetComponent<T>();
                if (null == target)
                {
                    target = obj.gameObject.AddComponent<T>();
                }
                UIDragScrollView scrollView = obj.GetComponent<UIDragScrollView>();
                if (null == scrollView)
                    scrollView = obj.AddComponent<UIDragScrollView>();
            }
        }
        return target;
    }


    void CleanGrid()
    {
        m__uitable.transform.DetachChildren();//从父节点摘除
        for (int i = 0; i < m_parentGridList.Count; i++)
        {
            GameObject.Destroy(m_parentGridList[i].gameObject);
        }
        for (int i = 0; i < m_childGridList.Count; i++)
        {
            GameObject.Destroy(m_childGridList[i].gameObject);
        }

        m__uitable.transform.DestroyChildren();//再销毁
        m_parentGridList.Clear();
        m_childGridList.Clear();
    }

    private void SetSelectFirstType(uint id, bool force = false)
    {
        //全部队伍
        if (id == 0)
        {
            OnClickAllTeam();
            m_selectMainId = id;
            SetMatchBtn(m_selectMainId);
            return;
        }

        //点到 附近grid
        if (id == 1)
        {
            OnClickFuJin(id);
            m_selectMainId = id;
            SetMatchBtn(m_selectMainId);
            return;
        }

        List<uint> keyIdList = m_dicActivity.Keys.ToList<uint>();

        List<TeamActivityDatabase> teamActivityList;
        UITeamActivityParentGrid grid;
        if (id == m_selectMainId)
        {
            if (m_dicActivity.TryGetValue(m_selectMainId, out teamActivityList) && m_secondsTabCreator.IsOpen(keyIdList.IndexOf(m_selectMainId)))
            {
                m_secondsTabCreator.Close(keyIdList.IndexOf(m_selectMainId));
                return;
            }
        }

        if (m_dicActivity.TryGetValue(m_selectMainId, out teamActivityList))
        {
            m_secondsTabCreator.Close(keyIdList.IndexOf(m_selectMainId));
        }

        if (m_dicActivity.TryGetValue(id, out teamActivityList))
        {
            m_secondsTabCreator.Open(keyIdList.IndexOf(id));
        }

        m_selectMainId = id;
        SetMatchBtn(m_selectMainId);

        //grid = m_parentGridList.Find((data) => { return m_selectMainId == data.mainId; });
        //if (m_selectMainId == id && !force)
        //{
        //    if (grid != null)
        //    {
        //        grid.PlayTween(AnimationOrTween.Direction.Toggle);
        //        return;
        //    }
        //}

        ////之前的
        //if (null != grid)
        //{
        //    grid.SetHightLight(false);
        //    grid.PlayTween(AnimationOrTween.Direction.Forward, false);
        //}

        ////现在的
        //grid = m_parentGridList.Find((data) => { return id == data.mainId; });
        //if (grid != null)
        //{
        //    grid.SetHightLight(true);
        //    grid.PlayTween(AnimationOrTween.Direction.Forward, true);
        //}
    }

    /// <summary>
    /// 清除其他子节点
    /// </summary>
    /// <param name="mainId"></param>
    void CleanOtherNode(uint mainId)
    {
        for (int i = 0; i < m_childGridList.Count; i++)
        {
            UITeamActivityChildGrid grid = m_childGridList[i];

            if (grid == null) continue;

            grid.SetHightLight(false);//清除所有选中状态
        }

        for (int i = 0; i < m__uitable.transform.childCount; i++)
        {
            Transform parent = m__uitable.transform.GetChild(i);
            Transform Container = parent.transform.Find("Container");
            if (mainId != uint.Parse(parent.name))
            {
                if (Container.gameObject.activeSelf)
                {
                    Container.gameObject.SetActive(false);
                    UIPlayTween[] playArray = parent.GetComponents<UIPlayTween>();
                    for (int j = 0; j < playArray.Length; j++)
                    {
                        playArray[j].Play(false);
                    }
                }
            }
        }
      
    }*/

}

