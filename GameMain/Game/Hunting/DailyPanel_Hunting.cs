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
    List<HuntingDataBase> huntingDataList = new List<HuntingDataBase>();
    List<uint> monsterIDs = new List<uint>();
    public Dictionary<uint, string> toggleDic = new Dictionary<uint, string>
   {
    {(uint)HuntingType.wild,"野外"},{(uint)HuntingType.underground,"地下城"}
   };

    private IRenerTextureObj m_RTObj = null;
    float rotateY = -8;
    Dictionary<uint, BossRefreshInfo> boss_dic = null;
    public enum HuntingType
    {
        none =0,
        wild = 1,         //野外
        underground,     //地下城 
        activity,         //活动
        xx,               //xx
    }
    int selecttype = (int)HuntingType.none;
    uint defaultMonsterID = 0;


    void InitHuntingPanel()
    {
        if (m_ctor_HuntingTypeRoot != null)
        {
            m_ctor_HuntingTypeRoot.Initialize<UITabGrid>(m_toggle_UIHuntingToggleGrid.gameObject, OnHuntingGridUpdate, OnHuntingGridUIEvent);

        }
      
        if (null != m_ctor_ListScrollView)
        {
            m_ctor_ListScrollView.Initialize<UIHuntingListGrid>(m_toggle_UIHuntingListGrid.gameObject, OnHuntingGridUpdate, OnHuntingGridUIEvent);
        }
    }

    void OnHuntingGridUpdate(UIGridBase grid, int index)
    {
        if (grid is UIHuntingListGrid)
        {
            HuntingDataBase data = huntingDataList[index];
            if (data != null)
            {
                UIHuntingListGrid listGrid = grid as UIHuntingListGrid;
                listGrid.SetGridData(data);
                listGrid.SetHightLight(false);
            }
        }
        if (grid is UITabGrid)
        {
            UITabGrid g = grid as UITabGrid;
            g.SetName(toggleDic[(uint)index + 1]);
            g.TabID = index + 1;
        }
    }


    void OnHuntingGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UIHuntingListGrid)
                {
                    UIHuntingListGrid huntingGrid = data as UIHuntingListGrid;
                    if (huntingGrid != null)
                    {
                        SetActiveMonsterGrid(huntingGrid.MonsterID,false);
                    }
                }
                if (data is UITabGrid)
                {
                    UITabGrid g = data as UITabGrid;
                    SetActiveTab(g.TabID);
                }
                break;
        }

    }
    void SetActiveTab(int tabID,uint monsterID = 0,bool needFocus =true)
    {
        if (null != m_ctor_HuntingTypeRoot)
        {
            UITabGrid tabGrid = m_ctor_HuntingTypeRoot.GetGrid<UITabGrid>((int)selecttype - 1 > 0 ? (int)selecttype - 1 : 0);
            if (tabGrid != null)
            {
                tabGrid.SetHightLight(false);
            }
            tabGrid = m_ctor_HuntingTypeRoot.GetGrid<UITabGrid>((int)tabID - 1 > 0 ? (int)tabID - 1 : 0);
            if (tabGrid != null)
            {
                tabGrid.SetHightLight(true);
            }
            CreateHuntingUIList(tabID, monsterID, needFocus);
            selecttype = tabID;
            uint HuntingScore = DataManager.Manager<DailyManager>().DayHuntingCoin;
            m_label_LieHunLabel.text = string.Format("{0}/{1}", HuntingScore, dm.HuntingCoinLimit);
        }  
    }
    void SetActiveMonsterGrid(uint monsterID,bool needFocus =true) 
    {
        if (m_ctor_ListScrollView != null)
        {
            UIHuntingListGrid monGrid = m_ctor_ListScrollView.GetGrid<UIHuntingListGrid>(monsterIDs.IndexOf(defaultMonsterID));
            if(null != monGrid)
            {
                monGrid.SetHightLight(false);
            }
            if (needFocus)
            {
                m_ctor_ListScrollView.FocusGrid(monsterIDs.IndexOf(monsterID));
            }
            monGrid = m_ctor_ListScrollView.GetGrid<UIHuntingListGrid>(monsterIDs.IndexOf(monsterID));       
            if (monGrid != null)
            {
                monGrid.SetHightLight(true);
            }
            defaultMonsterID = monsterID;
    
            HuntingDataBase data = huntingDataList[monsterIDs.IndexOf(monsterID)];
            onClickHuntingGrid(data);
        }
     
       


    }
    void CreateHuntingUIList(int type,uint monsterID = 0,bool needFocus =true)
    {
        if (null != m_ctor_ListScrollView)
        {
            huntingDataList.Clear();
            monsterIDs.Clear();        
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].type == type)
                {
                    huntingDataList.Add(list[i]);
                    monsterIDs.Add(list[i].ID);
                }
            }
            if (huntingDataList != null && huntingDataList.Count > 0)
            {
                m_ctor_ListScrollView.CreateGrids(huntingDataList.Count);
            }
            if (!monsterIDs.Contains(defaultMonsterID))
            {
                defaultMonsterID = monsterID == 0 ? monsterIDs[0] : monsterID;
            }
            else 
            {
                defaultMonsterID = monsterID == 0 ? defaultMonsterID : monsterID;      
            }
           
            SetActiveMonsterGrid(defaultMonsterID);
        }

    }
    uint MonsterID = 1;
    void onClickHuntingGrid(HuntingDataBase huntDa)
    {
        HuntingDataBase data = huntDa;
        MonsterID = data.ID;
        DataManager.Manager<HuntingManager>().MonsterID = MonsterID;
        if (boss_dic.Count > 0 && boss_dic.ContainsKey(MonsterID))
        {
            NpcDataBase monster = GameTableManager.Instance.GetTableItem<NpcDataBase>(boss_dic[MonsterID].boss_npc_id);
            if (monster != null)
            {
                m_label_BossName.text = monster.strName;
                MapDataBase map = GameTableManager.Instance.GetTableItem<MapDataBase>(data.mapID);
                m_label_mapLabel.text = map.strName;
            }

        }
        ShowModel(data);
        onClick_DropBtn_Btn(data);
        m_label_LieHunNum.text = data.HuntingScore.ToString();
    }

    void onClick_DropBtn_Btn(HuntingDataBase data)
    {
        AddCreator(m_trans_RewardRoot);
        m_lst_UIItemRewardDatas.Clear();
        string[] items = data.dropItem.Split('_');
        for (int i = 0; i < items.Length; i++)
        {
            uint itemID = uint.Parse(items[i]);
            m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
            {
                itemID = itemID,
                num = 1,
            });
        }
        m_ctor_UIItemRewardCreator.CreateGrids(m_lst_UIItemRewardDatas.Count);
    }


    void ShowModel(HuntingDataBase data)
    {
        string[] monID = data.monsterID.Split('_');
        uint monsterid = uint.Parse(monID[0]);
        table.NpcDataBase npcData = GameTableManager.Instance.GetTableItem<NpcDataBase>(monsterid);
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

    void DragModel(GameObject go, UnityEngine.Vector2 delta)
    {
        if (m_RTObj != null)
        {
            rotateY += -0.5f * delta.x;
            m_RTObj.SetModelRotateY(rotateY);
        }
    }


    void onClick_Btn_Go_Btn(GameObject caster)
    {
        MainPlayStop();
        NetService.Instance.Send(new stRefreshNobleFreeTransTimsPropertyUserCmd_C());
    }


    void onClick_InfoBtn_Btn(GameObject caster)
    {
        m_btn_infoContent.gameObject.SetActive(true);
        m_label_tipsContent.text = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.DailyHuntingText_TipsContent);
    }
    void onClick_InfoContent_Btn(GameObject caster)
    {
        m_btn_infoContent.gameObject.SetActive(false);
    }

    void onClick_Btn_shop_Btn(GameObject caster)
    {
        ReturnBackUIData[] returnData = new ReturnBackUIData[1];
        returnData[0] = new ReturnBackUIData();
        returnData[0].msgid = UIMsgID.eNone;
        returnData[0].panelid = PanelID.DailyPanel;
       

        int[] tab = new int[1];
        tab[0] = 2;
        ReturnBackUIMsg backUIMsg = new ReturnBackUIMsg();
        backUIMsg.tabs = tab;

        returnData[0].param = backUIMsg;

        UIPanelBase.PanelJumpData jumpData = new PanelJumpData();
        jumpData.Tabs = new int[1];

        jumpData.Tabs[0] = (int)ShopPanel.TabMode.LieHun;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ShopPanel, jumpData: jumpData);
    }
}
