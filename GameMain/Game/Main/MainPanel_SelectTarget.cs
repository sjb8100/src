//*******************************************************************************************
//	创建日期：	2017-8-24   20:54
//	文件名称：	MainPanel_SelectTarget,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	ZQB
//	说    明：	选择目标 自由模式下
//*******************************************************************************************
using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;


public partial class MainPanel
{
    #region perproty

    /// <summary>
    /// 旁边所有的任
    /// </summary>
    List<IPlayer> m_lstAllPlayer = new List<IPlayer>();

    /// <summary>
    /// 附近的敌人（最多取5个，由近到远）
    /// </summary>
    List<IPlayer> m_lstAllEnemy = new List<IPlayer>();

    /// <summary>
    /// 当前的目标敌人Id（最多5个，包括所的敌人）
    /// </summary>
    List<uint> m_lstTargetEnemyId = new List<uint>();

    /// <summary>
    /// 锁住的敌人
    /// </summary>
    List<uint> m_lstLockEnemyId = new List<uint>();

    /// <summary>
    /// 当前选中的
    /// </summary>
    uint m_selectEnemyId = 0;

    List<UISelectTargetGrid> m_lstTargetGrid = new List<UISelectTargetGrid>();

    GameCmd.enumPKMODE m_pkMode;

    #endregion


    #region method

    /// <summary>
    /// 初始化TAB   添加长按事件
    /// </summary>
    void InitTAB()
    {
        UIGridBase grid = m_btn_TAB.gameObject.GetComponent<UIGridBase>();
        if (grid == null)
        {
            grid = m_btn_TAB.gameObject.AddComponent<UIGridBase>();
        }
        grid.RegisterUIEventDelegate(OnTABGridUIEvent);

        InitGridList();
    }

    /// <summary>
    /// grid 事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnTABGridUIEvent(UIEventType eventType, object data, object param)
    {
        //长按
        if (eventType == UIEventType.LongPress)
        {
            IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
            if (mainPlayer != null)
            {
                this.m_pkMode = (GameCmd.enumPKMODE)mainPlayer.GetProp((int)PlayerProp.PkMode);

                ShowSelectTarget();
            }
        }
    }


    /// <summary>
    /// 显示选择目标
    /// </summary>
    void ShowSelectTarget()
    {
        m_trans_SelectTarget.gameObject.SetActive(true);

        //重置位置
        m_trans_SelectTarget.transform.localPosition = new Vector3(300, 150, 0);
    }

    /// <summary>
    /// 初始化grid
    /// </summary>
    void InitGridList()
    {
        m_lstTargetGrid.Clear();
        for (int i = 0; i < m_grid_SelectTargetGridRoot.transform.childCount; i++)
        {
            Transform ts = m_grid_SelectTargetGridRoot.transform.GetChild(i);
            UISelectTargetGrid grid = ts.gameObject.GetComponent<UISelectTargetGrid>();
            if (grid == null)
            {
                grid = ts.gameObject.AddComponent<UISelectTargetGrid>();
            }

            m_lstTargetGrid.Add(grid);

            //添加拖拽组件
            UIDragObject dragObj = ts.gameObject.GetComponent<UIDragObject>();
            if (dragObj == null)
            {
                dragObj = ts.gameObject.AddComponent<UIDragObject>();
            }

            dragObj.target = m_trans_SelectTarget.transform;
        }

        //添加拖拽组件
        UIDragObject dragObjBg = m_sprite_SelectTargetBg.gameObject.GetComponent<UIDragObject>();
        if (dragObjBg == null)
        {
            dragObjBg = m_sprite_SelectTargetBg.gameObject.AddComponent<UIDragObject>();
        }
    }

    float tempCd = 0;
    /// <summary>
    /// 定时更新
    /// </summary>
    void UpdateSelectTarget()
    {
        if (false == m_trans_SelectTarget.gameObject.activeSelf)
        {
            return;
        }

        tempCd += Time.deltaTime;
        if (tempCd > 1.0f)
        {
            Profiler.BeginSample("UpdateSelectTarget");

            //切换模式，清数据
            IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
            if (mainPlayer != null)
            {
                GameCmd.enumPKMODE pkMode = (GameCmd.enumPKMODE)mainPlayer.GetProp((int)PlayerProp.PkMode);
                if (this.m_pkMode != pkMode)
                {
                    CleanSelectTargetData();
                    ResetSelectTargetUIWidget();
                }

                this.m_pkMode = pkMode;
            }

            //非和平模式刷新列表
            if (this.m_pkMode != GameCmd.enumPKMODE.PKMODE_M_NORMAL)
            {
                UpdateUpdateSelectTargetList();
            }

            tempCd = 0;
            Profiler.EndSample();
        }

    }

    void UpdateUpdateSelectTargetList()
    {
        //刷数据
        RefreshTargetGridData();

        //UIWidget
        RefreshTargetUIWidget();

        //刷新grid
        RefreshTargetGrid();
    }

    /// <summary>
    /// 刷数据
    /// </summary>
    void RefreshTargetGridData()
    {
        IEntitySystem entitySystem = ClientGlobal.Instance().GetEntitySystem();
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;

        if (entitySystem == null)
        {
            return;
        }

        if (mainPlayer == null)
        {
            return;
        }

        ISkillPart sp = mainPlayer.GetPart(EntityPart.Skill) as ISkillPart;
        if (sp == null)
        {
            return;
        }

        m_lstAllEnemy.Clear();
        m_lstTargetEnemyId.Clear();

        //加入已经锁的敌人
        for (int i = 0; i < m_lstLockEnemyId.Count; i++)
        {
            if (m_lstTargetEnemyId.Count < 5)
            {
                m_lstTargetEnemyId.Add(m_lstLockEnemyId[i]);
            }
        }

        //所有人
        entitySystem.FindNearstPlayer(ref m_lstAllPlayer);

        //所有敌人
        for (int i = 0; i < m_lstAllPlayer.Count; i++)
        {
            int nSkillError;

            //最多选取5个可攻击敌人（减少计算量） 
            if (m_lstAllEnemy.Count >= 5)
            {
                break;
            }

            bool b = sp.CheckCanAttackTarget(m_lstAllPlayer[i], out nSkillError);
            if (b)
            {
                m_lstAllEnemy.Add(m_lstAllPlayer[i]);
            }
        }

        //加入选中的敌人
        if (m_lstTargetEnemyId.Count < 5 && false == m_lstTargetEnemyId.Contains(m_selectEnemyId))
        {
            bool b = false;
            for (int i = 0; i < m_lstAllEnemy.Count; i++)
            {
                if (m_lstAllEnemy[i].GetID() == m_selectEnemyId)
                {
                    b = true;
                    m_lstTargetEnemyId.Add(m_selectEnemyId);
                }
            }

            //无选中的
            if (false == b)
            {
                this.m_selectEnemyId = 0;
            }
        }

        //清除不在九宫格的
        //for (int i = m_lstTargetEnemyId.Count - 1; i >= 0; i--)
        //{
        //    if (m_lstLockEnemyId.Contains(m_lstTargetEnemyId[i]))
        //    {
        //        continue;
        //    }

        //    bool exist = false;
        //    for (int j = 0; j < m_lstAllEnemy.Count; j++)
        //    {
        //        if (m_lstAllEnemy[j].GetID() == m_lstTargetEnemyId[i])
        //        {
        //            exist = true;
        //        }
        //    }

        //    if (false == exist)
        //    {
        //        m_lstTargetEnemyId.Remove(m_lstTargetEnemyId[i]);
        //    }
        //}


        for (int i = 0; i < m_lstAllEnemy.Count; i++)
        {
            //排除玩家自己
            uint enemyId = m_lstAllEnemy[i].GetID();
            uint mainPlayerId = mainPlayer.GetID();

            //玩家自己，排除
            if (enemyId == mainPlayerId)
            {
                continue;
            }

            //string name = string.Format("--->>> enemyName = {0}", m_lstAllEnemy[i].GetName());
            //Debug.LogError(name);

            //已经在列表中
            if (m_lstTargetEnemyId.Contains(enemyId))
            {
                continue;
            }

            //新加入进来的(最多5个)
            if (m_lstTargetEnemyId.Count < 5)
            {
                m_lstTargetEnemyId.Add(enemyId);
            }
        }
    }

    void RefreshTargetGrid()
    {
        IEntitySystem entitySystem = ClientGlobal.Instance().GetEntitySystem();
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;

        if (entitySystem == null)
        {
            return;
        }

        if (mainPlayer == null)
        {
            return;
        }

        if (m_lstTargetEnemyId.Count > 5)
        {
            Engine.Utility.Log.Error("--->>>222 敌人数据超过5个！！！");
            return;
        }

        //锁定的
        for (int i = 0; i < m_lstLockEnemyId.Count; i++)
        {
            uint targetEnemyId = GetTargetEnemyById(m_lstLockEnemyId[i]);

            //在九宫格内
            if (targetEnemyId != 0)
            {
                m_lstTargetGrid[i].SetGridData(targetEnemyId);
                m_lstTargetGrid[i].RegisterUIEventDelegate(OnSelectTargetGridUIEvent);
                m_lstTargetGrid[i].SetLock(true);
            }
        }

        //未锁定的
        int index = 0;
        for (int i = 0; i < m_lstTargetEnemyId.Count; i++)
        {
            if (false == m_lstLockEnemyId.Contains(m_lstTargetEnemyId[i]))
            {
                int gridIndex = m_lstLockEnemyId.Count + index;
                if (gridIndex < m_lstTargetGrid.Count)
                {
                    m_lstTargetGrid[gridIndex].SetGridData(m_lstTargetEnemyId[i]);
                    m_lstTargetGrid[gridIndex].RegisterUIEventDelegate(OnSelectTargetGridUIEvent);
                    m_lstTargetGrid[gridIndex].SetLock(false);
                }

                index++;
            }
        }

        //选中状态
        for (int i = 0; i < m_lstTargetGrid.Count; i++)
        {
            if (m_selectEnemyId == m_lstTargetGrid[i].m_playerId)
            {
                m_lstTargetGrid[i].SetSelect(true);
            }
            else
            {
                m_lstTargetGrid[i].SetSelect(false);
            }
        }
    }

    /// <summary>
    /// UI
    /// </summary>
    void RefreshTargetUIWidget()
    {
        for (int i = 0; i < m_grid_SelectTargetGridRoot.transform.childCount; i++)
        {
            if (i < m_lstTargetEnemyId.Count)
            {
                Transform ts = m_grid_SelectTargetGridRoot.transform.GetChild(i);
                ts.gameObject.SetActive(true);
            }
            else
            {
                Transform ts = m_grid_SelectTargetGridRoot.transform.GetChild(i);
                m_lstTargetGrid[i].m_playerId = 0;
                ts.gameObject.SetActive(false);
            }
        }

        m_sprite_SelectTargetBg.SetDimensions(300, 44 + 57 * m_lstTargetEnemyId.Count);

        if (m_lstTargetEnemyId.Count > 0)
        {
            m_label_SelectTargetTitleLbl.text = "附近敌对玩家";
        }
        else
        {
            m_label_SelectTargetTitleLbl.text = "附近没有敌对玩家";
        }
    }

    uint GetTargetEnemyById(uint id)
    {
        for (int i = 0; i < m_lstTargetEnemyId.Count; i++)
        {
            if (m_lstTargetEnemyId[i] == id)
            {
                return m_lstTargetEnemyId[i];
            }
        }

        return 0;
    }

    private void OnSelectTargetGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UISelectTargetGrid grid = data as UISelectTargetGrid;
            if (grid == null)
            {
                return;
            }

            if (param == null)
            {
                SetSelectGrid(grid.m_playerId);
                SelectTargetChange(grid.m_playerId);

                //移动到目标附近
                //MoveToTarget(grid.m_playerId);
            }
            else
            {
                int btnIndex = (int)param;
                if (btnIndex == 2)
                {
                    if (m_lstLockEnemyId.Contains(grid.m_playerId))
                    {
                        grid.SetLock(false);
                        m_lstLockEnemyId.Remove(grid.m_playerId);
                    }
                    else
                    {
                        grid.SetLock(true);
                        if (m_lstLockEnemyId.Count < 5)
                        {
                            m_lstLockEnemyId.Add(grid.m_playerId);
                        }
                    }
                }
            }
        }

    }

    /// <summary>
    /// 选中grid
    /// </summary>
    /// <param name="id"></param>
    void SetSelectGrid(uint id)
    {
        if (id == this.m_selectEnemyId)
        {
            return;
        }

        UISelectTargetGrid grid = GetSelectTargetGrid(this.m_selectEnemyId);
        if (grid != null)
        {
            grid.SetSelect(false);
        }

        grid = GetSelectTargetGrid(id);
        if (grid != null)
        {
            grid.SetSelect(true);
        }

        this.m_selectEnemyId = id;
    }

    /// <summary>
    /// 移动到目标位置
    /// </summary>
    /// <param name="targetId"></param>
    void MoveToTarget(uint targetId)
    {
        for (int i = 0; i < m_lstAllEnemy.Count; i++)
        {
            if (m_lstAllEnemy[i].GetID() == targetId)
            {
                IController ctrl = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl();
                if (ctrl != null)
                {
                    ctrl.MoveToTarget(m_lstAllEnemy[i].GetPos());
                }

                break;
            }
        }

    }

    void OnSetSelectTargetGrid(uint id)
    {
        if (false == m_trans_SelectTarget.gameObject.activeSelf)
        {
            return;
        }

        SetSelectGrid(id);
    }

    /// <summary>
    /// 切换目标
    /// </summary>
    /// <param name="id"></param>
    void SelectTargetChange(uint id)
    {
        //切换目标
        IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es != null)
        {
            IPlayer m_curTarget = es.FindPlayer(id);
            if (m_curTarget != null)
            {
                //检测视野(策划要求不做视野检测)
                //if (es.CheckEntityVisible(m_curTarget))
                //检测搜索范围
                if (es.CheckSearchRange(m_curTarget))
                {
                    Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
                    if (cs != null)
                    {
                        //如果在挂机先停止（清任务目标），再开始，打指定目标
                        if (cs.GetCombatRobot().Status == CombatRobotStatus.RUNNING)
                        {
                            cs.GetCombatRobot().Stop();
                            cs.GetCombatRobot().Start();
                        }
                    }

                    Client.ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl().UpdateTarget(m_curTarget);

                    Client.stTargetChange targetChange = new Client.stTargetChange();
                    targetChange.target = m_curTarget;
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSYTEM_TAB, targetChange);

                   // Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_TARGETCHANGE, targetChange);
                }
                else
                {
                    TipsManager.Instance.ShowTips("目标不在附近");
                }
            }
            else
            {
                TipsManager.Instance.ShowTips("目标不在附近");
            }
        }
    }

    UISelectTargetGrid GetSelectTargetGrid(uint id)
    {
        for (int i = 0; i < m_lstTargetGrid.Count; i++)
        {
            if (m_lstTargetGrid[i].m_playerId == id)
            {
                return m_lstTargetGrid[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 清数据
    /// </summary>
    void CleanSelectTargetData()
    {
        m_lstAllEnemy.Clear();

        m_lstTargetEnemyId.Clear();

        m_lstLockEnemyId.Clear();

        m_selectEnemyId = 0;

    }

    /// <summary>
    /// 重置UI
    /// </summary>
    void ResetSelectTargetUIWidget()
    {
        m_label_SelectTargetTitleLbl.text = "附近没有敌对玩家";

        m_sprite_SelectTargetBg.SetDimensions(300, 44);

        for (int i = 0; i < m_lstTargetGrid.Count; i++)
        {
            m_lstTargetGrid[i].SetSelect(false);
            m_lstTargetGrid[i].SetLock(false);
            m_lstTargetGrid[i].gameObject.SetActive(false);
        }
    }


    #endregion


    #region click

    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="caster"></param>
    void onClick_SelectTargetBtnClose_Btn(GameObject caster)
    {
        m_trans_SelectTarget.gameObject.SetActive(false);

        CleanSelectTargetData();

        ResetSelectTargetUIWidget();
    }

    #endregion
}

