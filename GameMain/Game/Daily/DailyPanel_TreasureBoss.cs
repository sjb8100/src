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

partial class DailyPanel
{
    enum TreasureBossType 
    {
       Personal = 1,
       World =2,

    }

    int previousTab = 0;
    public Dictionary<uint, string> tabDic = new Dictionary<uint, string>
    {
       {(uint)TreasureBossType.Personal,"个人"},{(uint)TreasureBossType.World,"世界"}
    };

    List<uint> m_lst_TreasureDatas = null;

    uint previousBossID = 0;
    uint copyID = 0;
    void InitTreasureBossPage() 
    {
        if (m_ctor_TreasureScrollView != null)
        {
            m_ctor_TreasureScrollView.Initialize<UITreasureBossGrid>(m_toggle_UITreasureBossGrid.gameObject, OnTreasureBossGridUpdate, OnTreasureBossGridUIEvent);
        }
        if (m_ctor_TreasureTypeRoot != null)
        {
            m_ctor_TreasureTypeRoot.Initialize<UITabGrid>(m_toggle_UIHuntingToggleGrid.gameObject, OnTreasureBossGridUpdate, OnTreasureBossGridUIEvent);

            m_ctor_TreasureTypeRoot.CreateGrids(tabDic.Count);
        }
        if (m_ctor_PersonalRewardRoot != null)
        {
            m_ctor_PersonalRewardRoot.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateGridData, null);
        }
        if (m_ctor_WorldRewardRoot != null)
        {
            m_ctor_WorldRewardRoot.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateGridData, null);
        }

        if (null != m_ctor_FinalKillRewardRoot)
        {
            m_ctor_FinalKillRewardRoot.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateGridData, null);
        }
        SelectTab((int)TreasureBossType.Personal);
    }
    
    void OnTreasureBossGridUpdate(UIGridBase grid, int index)
    {
        if (grid is UITreasureBossGrid)
        {
            if (m_lst_TreasureDatas != null)
            {
                if (index < m_lst_TreasureDatas.Count)
               {
                   uint id  = m_lst_TreasureDatas[index];
                   if (id != 0)
                   {
                       UITreasureBossGrid listGrid = grid as UITreasureBossGrid;
                       listGrid.SetGridData(id);
                       listGrid.SetHightLight(false);
                       if(index == 0)
                       {
                           SelectBossGrid(listGrid);
                       }
                   }
               }
            }
           
        }
        if (grid is UITabGrid)
        {
            UITabGrid g = grid as UITabGrid;
            int tabID = index + 1;
            if (tabDic.ContainsKey((uint)tabID))
            {
                g.SetName(tabDic[(uint)tabID]);
                g.TabID = tabID;
            }           
        }
    }
    void OnTreasureBossGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UITreasureBossGrid)
                {
                    UITreasureBossGrid huntingGrid = data as UITreasureBossGrid;
                    if (huntingGrid != null)
                    {
                        SelectBossGrid(huntingGrid);
                    }
                }
                if (data is UITabGrid)
                {
                    UITabGrid g = data as UITabGrid;
                    SelectTab(g.TabID,true);
                }
                break;
        }

    }
    void SelectTab(int tabID,bool force =false) 
    {
        if (m_ctor_TreasureTypeRoot ==null )
       {
           return;
       }
        if (previousTab == tabID && force)
        {
            return;
        }
        //---------------------------------------------
        UITabGrid tabGrid = m_ctor_TreasureTypeRoot.GetGrid<UITabGrid>((int)previousTab - 1 > 0 ? (int)previousTab - 1 : 0);
        if (tabGrid != null)
        {
            tabGrid.SetHightLight(false);
        }
        tabGrid = m_ctor_TreasureTypeRoot.GetGrid<UITabGrid>((int)tabID - 1 > 0 ? (int)tabID - 1 : 0);
        if (tabGrid != null)
        {
            tabGrid.SetHightLight(true);
        }

        CreateLeftList(tabID);
        previousTab = tabID;   
    }

    void CreateLeftList(int type) 
    {
        m_lst_TreasureDatas = DataManager.Manager<DailyManager>().GetTreasureBossList(type);
        m_ctor_TreasureScrollView.CreateGrids(m_lst_TreasureDatas.Count);
    }

    
  
    void SelectBossGrid(UITreasureBossGrid grid) 
    {
        if (m_ctor_TreasureScrollView != null)
        {
            UITreasureBossGrid previous = m_ctor_TreasureScrollView.GetGrid<UITreasureBossGrid>(m_lst_TreasureDatas.IndexOf(previousBossID));
            if (null != previous)
            {
                previous.SetHightLight(false);
            }
            grid.SetHightLight(true);
        }
        previousBossID = grid.TableID;
        if (previousBossID != 0)
        {
            TreasureBossDataBase tab = GameTableManager.Instance.GetTableItem<TreasureBossDataBase>(previousBossID);
            bool showPersonal = tab.bossType == (uint)TreasureBossType.Personal;          
            m_trans_PersonalRewardContent.gameObject.SetActive(showPersonal);
            m_trans_WorldRewardContent.gameObject.SetActive(!showPersonal);
            copyID = tab.copyID;
            if (showPersonal)
            {
                ParaseReward(tab.dropItems);
                m_label_RecommandPowerValue.text = tab.recommendPower.ToString();
                m_ctor_PersonalRewardRoot.CreateGrids(m_lst_UIItemRewardDatas.Count);

                CopyInfo info = DataManager.Manager<ComBatCopyDataManager>().GetCopyInfoById(tab.copyID);
                if (info != null)
                {
                    m_label_reward_time.text = string.Format("{0}/{1}", info.CopyUseNum, info.MaxCopyNum);
                }

            }
            else
            {
                if (null != m_label_WorldBossOpenDes)
                {
                    m_label_WorldBossOpenDes.text = tab.refreshTime;
                }
                RefreshBossPreRoundInfo(previousBossID);

                ParaseReward(tab.dropItems);
                m_ctor_WorldRewardRoot.CreateGrids(m_lst_UIItemRewardDatas.Count);

                ParaseReward(tab.finalItems);
                m_ctor_FinalKillRewardRoot.CreateGrids(m_lst_UIItemRewardDatas.Count);
            }
          
            ShowModel(tab.bossID);
        }      
    }

    void ShowModel(uint mstID) 
    {
        NpcDataBase npcData = GameTableManager.Instance.GetTableItem<NpcDataBase>(mstID);
        if (npcData != null)
        {
            
            uint modelID = npcData.dwViewModelSet;  // 使用观察模型
            if (m_RTObj != null)
            {
                m_RTObj.Release();
                m_RTObj = null;
            }

            m_RTObj = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj((int)modelID, 800);
            if (m_RTObj == null)
            {
                return;
            }
            ModeDiplayDataBase modelDisp = GameTableManager.Instance.GetTableItem<ModeDiplayDataBase>(modelID);
            if (modelDisp == null)
            {
                Engine.Utility.Log.Error("BOSS模型ID为{0}的模型展示数据为空", modelID);
                return;
            }
            m_RTObj.SetDisplayCamera(modelDisp.pos800, modelDisp.rotate800,modelDisp.Modelrotation);
            m_RTObj.PlayModelAni(Client.EntityAction.Stand);
            UIRenderTexture rt = m__Model.GetComponent<UIRenderTexture>();
            if (null == rt)
            {
                rt = m__Model.gameObject.AddComponent<UIRenderTexture>();
            }
            if (null != rt)
            {
                rt.SetDepth(3);
                rt.Initialize(m_RTObj, m_RTObj.YAngle, new UnityEngine.Vector2(800, 800));
            }
        }
    }

    void ParaseReward(string strReward)
    {
        m_lst_UIItemRewardDatas.Clear();
        string[] items = strReward.Split('_');
        for (int i = 0; i < items.Length; i++)
        {
            uint itemID = uint.Parse(items[i]);
            m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
            {
                itemID = itemID,
                num = 1,
            });
        }
    }

    #region 世界Boss
    /// <summary>
    /// 是否为世界Boss模式
    /// </summary>
    private bool IsWorldBossMode
    {
        get
        {
            return (previousTab == (int)TreasureBossType.World);
        }
    }
    /// <summary>
    /// 刷新Boss信息
    /// </summary>
    /// <param name="id"></param>
    void RefreshBossStatusInfo(uint id)
    {
        if (!IsWorldBossMode)
        {
            return;
        }

        //刷新给子数据
        if (null != m_ctor_TreasureScrollView)
        {
            if (null != m_lst_TreasureDatas && m_lst_TreasureDatas.Contains(id))
            {
                m_ctor_TreasureScrollView.UpdateData(m_lst_TreasureDatas.IndexOf(id));
            }
        }
        //刷新上一轮战况
        RefreshBossPreRoundInfo(id);
    }

    /// <summary>
    /// 刷新Boss上轮战况信息
    /// </summary>
    void RefreshBossPreRoundInfo(uint id)
    {
        if (previousBossID != id)
        {
            return;
        }
        JvBaoBossWorldManager dmgr = DataManager.Manager<JvBaoBossWorldManager>();
        string tempTxt = "--";
        JvBaoBossWorldManager.LocalWorldBossInfo info = null;
        if (dmgr.TryGetWorldBossStatusInfo(id, out info) && !string.IsNullOrEmpty(info.LastDam))
        {
            tempTxt = info.LastDam;
        }
        if (null != m_label_PreRoundLastAttackName)
        {
            m_label_PreRoundLastAttackName.text = tempTxt;
        }

        if (null != m_label_PreRoundMaxDamName)
        {
            if (null != info && !string.IsNullOrEmpty(info.MaxDam))
            {
                tempTxt = info.MaxDam;
            }
            else
            {
                tempTxt = "--";
            }

            m_label_PreRoundMaxDamName.text = tempTxt;
        }
    }
    #endregion

    #region Ngui
    void onClick_TBBtn_Go_Btn(GameObject caster)
    {
        if (IsWorldBossMode)
        {
            DataManager.Manager<JvBaoBossWorldManager>().JoinWorldBoss(previousBossID);
        }
        else if (copyID != 0)
        {
            NetService.Instance.Send(new stRequestEnterCopyUserCmd_C() { copy_base_id = copyID });
        }
    }

    void onClick_TBBtn_shop_Btn(GameObject caster)
    {

    }
    #endregion


}
