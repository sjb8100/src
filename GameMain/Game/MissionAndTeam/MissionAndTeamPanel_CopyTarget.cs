using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

//*******************************************************************************************
//	创建日期：	2017-8-16   16:34
//	文件名称：	MissionAndTeamPanel_CopyTarget,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	ZQB
//	说    明：	副本目标
//*******************************************************************************************


partial class MissionAndTeamPanel
{
    /// <summary>
    /// 目标
    /// </summary>
    List<CopyTargetInfo> m_listCopyTarget = new List<CopyTargetInfo>();

    /// <summary>
    /// grid缓存
    /// </summary>
    List<GameObject> m_lstTargetGridCache = new List<GameObject>();

    List<UICopyTargetGrid> m_lstCopyTargetGrid = new List<UICopyTargetGrid>();

    //static uint YanWuChangCopyId = 3000;

    ComBatCopyDataManager copyManager
    {
        get
        {
            return DataManager.Manager<ComBatCopyDataManager>();
        }
    }


    void InitCopyTargetWidget()
    {

    }

    /// <summary>
    /// 更新目标
    /// </summary>
    void UpdateCopyTarget()
    {
        if (copyManager.IsEnterCopy == false)
        {
            return;
        }

        m_listCopyTarget = copyManager.GetCopyTargetList();

        //grid
        CreateCopyTargetGrid();

        //title
        InitCopyTargetTitle();

        //五行阵显示特效
        if (DataManager.Manager<ComBatCopyDataManager>().IsWuXinZhenCopy())
        {
            //只出一次特效
            if (true == DataManager.Manager<ComBatCopyDataManager>().m_haveEnterWuXinZhen)
            {
                return;
            }
            DataManager.Manager<ComBatCopyDataManager>().m_haveEnterWuXinZhen = true;

            uint copyId = DataManager.Manager<ComBatCopyDataManager>().EnterCopyID;
            table.CopyDataBase copyDb = GameTableManager.Instance.GetTableItem<table.CopyDataBase>(copyId);
            if (copyDb == null)
            {
                return;
            }

            table.CopyTargetGuideDataBase ctGuideDb = GameTableManager.Instance.GetTableItem<table.CopyTargetGuideDataBase>(copyDb.guideId);
            if (ctGuideDb == null)
            {
                return;
            }

            PlayCopyTargetEffect();

            StartCoroutine(DelayToCloseCopyTargetEffect((float)ctGuideDb.time));
        }


    }

    /// <summary>
    /// title
    /// </summary>
    void InitCopyTargetTitle()
    {
        uint waveId = m_listCopyTarget.Count > 0 ? m_listCopyTarget[0].waveIdList[0] : 0;
        CopyTargetDataBase copyTargetDb = GameTableManager.Instance.GetTableItem<CopyTargetDataBase>(copyManager.EnterCopyID, (int)waveId);
        if (copyTargetDb != null)
        {
            m_label_copyTargetTitle.text = copyTargetDb.stepName;
        }
    }

    /// <summary>
    /// 创建grid
    /// </summary>
    void CreateCopyTargetGrid()
    {
        //grid
        ReleaseTargetGrid();

        uint copyId = DataManager.Manager<ComBatCopyDataManager>().EnterCopyID;

        int tempCount = 0;

        for (int i = 0; i < m_listCopyTarget.Count; i++)
        {
            UICopyTargetGrid copyTargetGrid = GetTargetGrid();

            copyTargetGrid.transform.parent = m_trans_copyTargetGrid;
            copyTargetGrid.transform.localScale = Vector3.one;
            copyTargetGrid.transform.localRotation = Quaternion.identity;

            float hight1 = -44 * i;

            tempCount += (m_listCopyTarget[i].waveIdList.Count - 1);

            float hight2 = -22 * tempCount;

            //演武场 更新经验 ,金币副本 (特殊处理)
            if (DataManager.Manager<ComBatCopyDataManager>().IsYanWuChangCopy(copyId) || DataManager.Manager<ComBatCopyDataManager>().IsGoldCopy(copyId) || DataManager.Manager<ComBatCopyDataManager>().IsWorldJuBao(copyId))
            {
                if (i == m_listCopyTarget.Count - 1)
                {
                    hight2 = -22 * (tempCount + 1);
                }
            }

            copyTargetGrid.transform.localPosition = new Vector3(0, hight1 + hight2, 0);

            copyTargetGrid.gameObject.SetActive(true);

            copyTargetGrid.SetGridData(m_listCopyTarget[i]);

            copyTargetGrid.RegisterUIEventDelegate(UIGridEventDelegate);

            string name = copyManager.GetCopyTargetName(m_listCopyTarget[i]);

            copyTargetGrid.SetName(name);

            m_lstCopyTargetGrid.Add(copyTargetGrid);
        }
    }

    /// <summary>
    /// grid点击事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    void UIGridEventDelegate(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UICopyTargetGrid grid = data as UICopyTargetGrid;
            if (grid == null)
            {
                return;
            }

            //五行阵副本
            bool isWuXinZhenCopy = DataManager.Manager<ComBatCopyDataManager>().IsWuXinZhenCopy();
            if (isWuXinZhenCopy)
            {
                VisitNpc(grid.copyTarget.mapId, grid.copyTarget.npcId);

                SetCombatRobot(false);
            }
        }
    }

    /// <summary>
    ///    更新grid
    /// </summary>
    /// <param name="WaveId"></param>
    void UpdateCopyTargetGrid(uint WaveId)
    {
        for (int i = 0; i < m_lstCopyTargetGrid.Count; i++)
        {
            string waveName = copyManager.GetCopyTargetName(m_lstCopyTargetGrid[i].copyTarget);
            m_lstCopyTargetGrid[i].SetName(waveName);
        }
    }

    /// <summary>
    /// 更新经验
    /// </summary>
    /// <param name="exp"></param>
    void UpdateCopyTargetGridExp(uint exp)
    {
        //演武场 更新经验
        uint copyId = DataManager.Manager<ComBatCopyDataManager>().EnterCopyID;
        if (DataManager.Manager<ComBatCopyDataManager>().IsYanWuChangCopy(copyId))
        {
            for (int i = 0; i < m_lstCopyTargetGrid.Count; i++)
            {
                if (i == m_lstCopyTargetGrid.Count - 1)
                {
                    string name = copyManager.GetCopyTargetName(m_lstCopyTargetGrid[i].copyTarget);

                    m_lstCopyTargetGrid[i].SetName(string.Format(name));
                }
            }
        }
    }

    /// <summary>
    /// 金币副本跟新金币（金银山）
    /// </summary>
    /// <param name="exp"></param>
    void UpdateCopyTargetGridGold()
    {
        uint copyId = DataManager.Manager<ComBatCopyDataManager>().EnterCopyID;
        if (DataManager.Manager<ComBatCopyDataManager>().IsGoldCopy(copyId))
        {
            for (int i = 0; i < m_lstCopyTargetGrid.Count; i++)
            {
                if (i == m_lstCopyTargetGrid.Count - 1)
                {
                    string name = copyManager.GetCopyTargetName(m_lstCopyTargetGrid[i].copyTarget);

                    m_lstCopyTargetGrid[i].SetName(string.Format(name));
                }
            }
        }
    }

    /// <summary>
    /// 世界聚宝（排行 伤害值）
    /// </summary>
    void UpdateCopyTargetGridWorldJuBaoRankAndDamage()
    {
        uint copyId = DataManager.Manager<ComBatCopyDataManager>().EnterCopyID;
        if (DataManager.Manager<ComBatCopyDataManager>().IsWorldJuBao(copyId))
        {
            for (int i = 0; i < m_lstCopyTargetGrid.Count; i++)
            {
                //排行
                if (i == 0)
                {
                    string name = copyManager.GetCopyTargetName(m_lstCopyTargetGrid[i].copyTarget);

                    m_lstCopyTargetGrid[i].SetName(string.Format(name));
                }

                //伤害
                if (i == m_lstCopyTargetGrid.Count - 1)
                {
                    string name = copyManager.GetCopyTargetName(m_lstCopyTargetGrid[i].copyTarget);

                    m_lstCopyTargetGrid[i].SetName(string.Format(name));
                }
            }
        }
    }

    /// <summary>
    /// 世界聚宝（排名）
    /// </summary>
    //void UpdateCopyTargetGridWorldJuBaoRank()
    //{
    //    uint copyId = DataManager.Manager<ComBatCopyDataManager>().EnterCopyID;
    //    if (DataManager.Manager<ComBatCopyDataManager>().IsWorldJuBao(copyId))
    //    {
    //        if (m_lstCopyTargetGrid.Count > 0 )
    //        {
    //            string name = copyManager.GetCopyTargetName(m_lstCopyTargetGrid[0].copyTarget);

    //            m_lstCopyTargetGrid[0].SetName(string.Format(name));
    //        }
    //    }
    //}

    /// <summary>
    /// 世界聚宝（鼓舞加成）
    /// </summary>
    void UpdateCopyTargetGridWorldJuBaoGuWuAdd()
    {
        uint copyId = DataManager.Manager<ComBatCopyDataManager>().EnterCopyID;
        if (DataManager.Manager<ComBatCopyDataManager>().IsWorldJuBao(copyId))
        {
            if (m_lstCopyTargetGrid.Count > 1)
            {
                string name = copyManager.GetCopyTargetName(m_lstCopyTargetGrid[1].copyTarget);

                m_lstCopyTargetGrid[1].SetName(string.Format(name));
            }
        }
    }

    void ReleaseTargetGrid()
    {
        //加入缓存
        for (int i = 0; i < m_lstCopyTargetGrid.Count; i++)
        {
            m_lstCopyTargetGrid[i].transform.parent = m_trans_copyTargetGridCache;
            m_lstCopyTargetGrid[i].gameObject.SetActive(false);
            m_lstTargetGridCache.Add(m_lstCopyTargetGrid[i].gameObject);
        }

        //清除
        m_trans_copyTargetGrid.transform.DestroyChildren();
        m_lstCopyTargetGrid.Clear();
    }


    /// <summary>
    /// 获取Grid
    /// </summary>
    /// <returns></returns>
    UICopyTargetGrid GetTargetGrid()
    {
        GameObject obj = null;
        UICopyTargetGrid grid = null;

        //有缓存先取缓存
        if (m_lstTargetGridCache.Count > 0)
        {
            obj = m_lstTargetGridCache[0];
            m_lstTargetGridCache.RemoveAt(0);

            grid = obj.GetComponent<UICopyTargetGrid>();
            if (grid == null)
            {
                grid = obj.AddComponent<UICopyTargetGrid>();
            }

            return grid;
        }

        //无缓存  立即创建
        GameObject resObj = UIManager.GetResGameObj(GridID.Uicopytargetgrid) as GameObject;
        obj = Instantiate(resObj) as GameObject;

        grid = obj.GetComponent<UICopyTargetGrid>();
        if (grid == null)
        {
            grid = obj.AddComponent<UICopyTargetGrid>();
        }

        return grid;
    }

    /// <summary>
    /// 设置机器人
    /// </summary>
    /// <param name="b"></param>
    void SetCombatRobot(bool b)
    {
        Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            return;
        }

        Client.ICombatRobot cr = cs.GetCombatRobot();
        if (cr == null)
        {
            Engine.Utility.Log.Error("ICombatRobot is null");
            return;
        }

        if (b)
        {
            ComBatCopyDataManager comBat = DataManager.Manager<ComBatCopyDataManager>();
            if (comBat == null)
            {
                return;
            }

            if (comBat.EnterCopyID != 0)
            {
                cr.StartInCopy(comBat.EnterCopyID, comBat.LaskSkillWave, comBat.LastTransmitWave);
            }
            else
            {

            }
        }
        else
        {
            cr.Stop();
        }
    }


    /// <summary>
    /// 访问Npc
    /// </summary>
    void VisitNpc(uint mapId, uint npcId)
    {
        Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            return;
        }

        Client.IController controller = cs.GetActiveCtrl();
        if (controller == null)
        {
            Engine.Utility.Log.Error("IController is null");
            return;
        }

        controller.VisiteNPC(mapId, npcId);
    }

    /// <summary>
    /// 五行阵环绕特效
    /// </summary>
    public void PlayCopyTargetEffect()
    {
        if (m_trans_copyTargetEffectRoot != null)
        {
            UIParticleWidget p = m_trans_copyTargetEffectRoot.GetComponent<UIParticleWidget>();
            if (p == null)
            {
                p = m_trans_copyTargetEffectRoot.gameObject.AddComponent<UIParticleWidget>();
                p.depth = 20;
            }
            if (p != null)
            {
                p.SetDimensions(260, 225);
                p.ReleaseParticle();
                p.AddRoundParticle();
            }
        }
    }

    IEnumerator DelayToCloseCopyTargetEffect(float delay)
    {
        yield return new WaitForSeconds(delay);
        UIParticleWidget p = m_trans_copyTargetEffectRoot.GetComponent<UIParticleWidget>();
        if (p != null)
        {
            p.ReleaseParticle();
        }
    }
}

