using UnityEngine;
using System.Collections.Generic;
using table;
using GameCmd;
using Client;
partial class AchievementPanel
{
    private UISecondTabCreatorBase mSecondTabCreator = null;
    private List<AchieveData> dailyAchieveDataList = new List<AchieveData>();
    List<AchievementDataBase> achieveList = GameTableManager.Instance.GetTableList<AchievementDataBase>();
    Dictionary<uint, List<AchievementDataBase>>  achieveDic = null;
    Dictionary<uint, List<uint>> m_uintDic = null;
    //UITable table;
    private List<uint> mlstFirstTabIds = null;
    private List<uint> mlstSecondTabIds = null;
    private uint m_uint_activeFType = 0;
    private uint m_uint_activeStype = 0;
    uint selectSecondsKey = 0;


    UIAchievemenGrid previousGrid = null;
    AchievementManager achievementManager
    {
        get
        {
            return DataManager.Manager<AchievementManager>();
        }
    }


    protected override void OnLoading()
    {
        base.OnLoading();
        UIEventListener.Get(m_toggle_HideAchievement.gameObject).onClick = OnHideAchievementChange;
        m_toggle_HideAchievement.value = true;
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        NetService.Instance.Send(new stReqAchieveDataDataUserCmd_C());
        InitWidgets();
        RefreshTabData();            
        UpdateCompletedAchievement();
       
        if (data != null && data is uint)
        {
            FocusAchievementData((uint)data);
        }
        else 
        {
            UpdateAchieveContent();
        }
        achievementManager.ValueUpdateEvent += Achievement_ValueUpdateEvent;

        if(previousGrid != null)
        {
            previousGrid.SetSelect(false);
        }
    }

    void FocusAchievementData(uint id) 
    {
        AchievementDataBase tab = GameTableManager.Instance.GetTableItem<AchievementDataBase>(id);
        if (tab != null)
        {
            SetSelectFirstType(tab.type,true);
            SetSelectSecondType(tab.childType, true);
            int index = 0;
            for (int i = 0; i < dailyAchieveDataList.Count;i++ )
            {
                if (dailyAchieveDataList[i].id == id)
                 {
                     index = i;
                 }
            }
            SetSelectAchievementGrid(index,true);
        }
    }
    void onClick_Btn_AllReceive_Btn(GameObject caster)
    {
        achievementManager.ReqGetAllAchieve();
    }

    void OnHideAchievementChange(GameObject go)
    {
        UpdateAchieveContent();  
    }
    private bool m_bInitCreator = false;
    void InitWidgets()
    {
        if (null == mSecondTabCreator)
        {
            if (null != m_scrollview_TypeScrollView && null == mSecondTabCreator)
            {
                mSecondTabCreator = m_scrollview_TypeScrollView.GetComponent<UISecondTabCreatorBase>();
                if (null == mSecondTabCreator)
                    mSecondTabCreator = m_scrollview_TypeScrollView.gameObject.AddComponent<UISecondTabCreatorBase>();
                if (null != mSecondTabCreator)
                {
//                     GameObject cloneFTemp = UIManager.GetResGameObj(GridID.Uictrtypegrid) as GameObject;
//                     GameObject cloneSTemp = UIManager.GetResGameObj(GridID.Uisecondtypegrid) as GameObject;
                    mSecondTabCreator.Initialize<UISecondTypeGrid>(m_trans_UICtrTypeGrid.gameObject, m_widget_UISecondTypeGrid.gameObject
                        , OnUpdateGridData, OnUpdateSecondTabGrid, OnGridUIEventDlg);
                }
            }
        }
        if (null != mSecondTabCreator)
        {
            List<int> secondTabsNums = new List<int>();
            achieveDic = achievementManager.GetAchieveTypeData() ;
            if (null == mlstFirstTabIds)
            {
                mlstFirstTabIds = new List<uint>();
            }
            mlstFirstTabIds.Clear();
            if (m_uintDic == null)
            {
                m_uintDic = new Dictionary<uint, List<uint>>();
            }
             List<AchievementDataBase> temp = new List<AchievementDataBase>();
             List<uint> secIDs = new List<uint>();
             foreach (var i in achieveDic)
             {
                 if (!mlstFirstTabIds.Contains(i.Key))
                 {
                    mlstFirstTabIds.Add(i.Key);
                 }
                 secIDs.Clear();
                 for (int m = 0; m < i.Value.Count; m++)
                 {
                     for (int a = 0; a < i.Value.Count; a++)
                     {
                         if (m_uintDic.ContainsKey(i.Key))
                         {
                             m_uintDic[i.Key].Add(i.Value[a].childType);
                         }
                         else
                         {
                             List<uint> li = new List<uint>();
                             li.Add(i.Value[a].childType);
                             m_uintDic.Add(i.Key, li);
                         }
                     }
                 }
                 secondTabsNums.Add(i.Value.Count);
            }
            if (null != mSecondTabCreator)
            {
                mSecondTabCreator.CreateGrids(secondTabsNums);
            }
        }
        if (null != m_ctor_AchievementScrollView)
        {
            m_ctor_AchievementScrollView.Initialize<UIAchievemenGrid>(m_trans_UIAchievemenGrid.gameObject, OnUpdateGridData, OnGridUIEventDlg);
        }
    }

    private void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (grid is UICtrTypeGrid)
        {
            UICtrTypeGrid tabGrid = grid as UICtrTypeGrid;
            if (tabGrid != null)
            {
                if (index == 0)
                {
                    int haveNum = 0;
                    int totalNum = 0;
                    achievementManager.GetGainAchieveNum(out haveNum, out totalNum);
                    string titleTips = "综合";
                    tabGrid.SetRedPointStatus(achievementManager.HaveCanReceiveAchieveByType((uint)index));
                    if (index < mlstFirstTabIds.Count)
                    {
                        tabGrid.SetData(mlstFirstTabIds[index], titleTips, 0);
                    }
                  
                }
                else
                {
                    AchievementDataBase achievementData = achievementManager.GetParentAchievementData(index);
                    if (achievementData != null)
                    {
                        int haveNum = 0;
                        int totalNum = 0;
                        int count = 0;
                        if (achieveDic.ContainsKey((uint)index))
                        {
                             count =  achieveDic[(uint)(index)].Count;
                        }                       
                        achievementManager.GetGainAchieveByType(achievementData.type, out haveNum, out totalNum);
                        string titleTips = string.Format("{0}({1}/{2})", achievementData.typeName, haveNum, totalNum);
                        tabGrid.SetRedPointStatus(achievementManager.HaveCanReceiveAchieveByType((uint)index));
                        if (index < mlstFirstTabIds.Count)
                        {
                            tabGrid.SetData(mlstFirstTabIds[index], titleTips, count);
                        }
                       
                    }
                }
            }
        }
        else if (grid is UIAchievemenGrid)
        {
            UIAchievemenGrid achieveGrid = grid as UIAchievemenGrid;
            if (null != achieveGrid)
            {
                AchieveData achieveData = dailyAchieveDataList.Count > index ? dailyAchieveDataList[index] : null;
                achieveGrid.UpdateItemInfo(achieveData);
                achieveGrid.SetGridData(index);
                achieveGrid.SetSelect(false);               
            }
        }
    }
    private void OnUpdateSecondTabGrid(UIGridBase grid, object id, int index)
    {
        if (grid is UISecondTypeGrid)
        {
            UISecondTypeGrid sGrid = grid as UISecondTypeGrid;
            if (achieveDic.ContainsKey((uint)id))
            {
                List<AchievementDataBase> list = achieveDic[(uint)id];
                bool value = achievementManager.HaveCanRecieveByChildType((int)list[index].type, list[index].childType);
                sGrid.SetRedPoint(value);
                sGrid.SetData(list[index].childType, list[index].childTypeName, m_uint_activeStype == m_uintDic[m_uint_activeFType][index]);
            }        
        }
    }


    /// <summary>
    /// 刷新左侧选项的数据
    /// </summary>
    private void RefreshTabData()
    {
        mSecondTabCreator.UpdateActive();
    }

    private void OnGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UICtrTypeGrid)
                {
                    UICtrTypeGrid tabGrid = data as UICtrTypeGrid;
                    SetSelectFirstType((uint)tabGrid.ID);
                    if (m_uintDic.ContainsKey((uint)tabGrid.ID))
                    {
                        if (m_uintDic[(uint)tabGrid.ID].Count >0)
                       {
                          SetSelectSecondType( m_uintDic[(uint)tabGrid.ID][0],true);
                       }
                    }
                }
                if (data is UIAchievemenGrid)
                {
                    UIAchievemenGrid achieveGrid = data as UIAchievemenGrid;
                    if (null != achieveGrid)
                    {
                        SetSelectAchievementGrid(achieveGrid.index);
                    }
                }
                if (data is UISecondTypeGrid)
                {
                    UISecondTypeGrid sec = data as UISecondTypeGrid;
                    SetSelectSecondType(sec.Data);          
                }
                break;
        }
    }
    private void SetSelectFirstType(uint type, bool force = false)
    {
        if (null == mSecondTabCreator)
        {
            return;
        }
        if( type == 0)
        {
            mSecondTabCreator.DoToggle(0, true, true);
            m_uint_activeFType = 0;
            UpdateAchieveContent();
            return;
        }
        if (m_uint_activeFType == type && !force)
        {
            mSecondTabCreator.DoToggle(mlstFirstTabIds.IndexOf(m_uint_activeFType), true, true);
            return;
        }
        m_uint_activeFType = type;
        mSecondTabCreator.Open(mlstFirstTabIds.IndexOf(m_uint_activeFType), true);

    }
    private void SetSelectSecondType(uint type, bool force = false)
    {
        if (null == mSecondTabCreator)
        {
            return;
        }
        if (m_uint_activeStype == type && !force)
            return;
        UISecondTypeGrid sGrid = null;
        if (m_uint_activeFType != 0)
        {
            sGrid = mSecondTabCreator.GetGrid<UISecondTypeGrid>(mlstFirstTabIds.IndexOf(m_uint_activeFType), m_uintDic[m_uint_activeFType].IndexOf(m_uint_activeStype));
            if (null != sGrid)
            {
                sGrid.SetHightLight(false);
            }
        }

        m_uint_activeStype = type;
        if (m_uint_activeFType != 0)
        {
            sGrid = mSecondTabCreator.GetGrid<UISecondTypeGrid>(mlstFirstTabIds.IndexOf(m_uint_activeFType), m_uintDic[m_uint_activeFType].IndexOf(m_uint_activeStype));
            if (null != sGrid)
            {
                sGrid.SetHightLight(true);
            }
        }
        SecondTypeClick(m_uint_activeStype);

        if (null != previousGrid)
        {
            previousGrid.SetSelect(false);
        }
    }

    void SetSelectAchievementGrid(int index,bool force = false) 
    {
        if (null != m_ctor_AchievementScrollView)
        {        
            if (null != previousGrid)
            {
                previousGrid.SetSelect(false);
            }
            m_ctor_AchievementScrollView.FocusGrid(index);
          
           UIAchievemenGrid grid = m_ctor_AchievementScrollView.GetGrid<UIAchievemenGrid>(index);
            if (null != grid)
            {
                grid.SetSelect(true);
            }
            previousGrid = grid;
        }
    }
    /// <summary>
    /// 更新成就内容
    /// </summary>
    private void UpdateAchieveContent()
    {
        if (null != m_ctor_AchievementScrollView)
        {
            if (m_uint_activeFType == 0)
            {
                List<AchievementDataBase> list = GameTableManager.Instance.GetTableList<AchievementDataBase>();
                dailyAchieveDataList.Clear();
                for (int i = 1; i < list.Count; i++)
                {
                        uint id = list[i].id;
                        AchieveData achieveData = achievementManager.GetAchieveData(id);
                        if (achieveData != null)
                        {
                            if (m_toggle_HideAchievement.value && achieveData.status == (uint)AchieveStatus.AchieveStatus_HaveReceive)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            achieveData = new AchieveData();
                            achieveData.id = list[i].id;
                        }
                        dailyAchieveDataList.Add(achieveData);
                }
             
              
            }
            else
            {
                AchievementDataBase typeData = achievementManager.GetParentAchievementData((int)m_uint_activeFType);
                if (typeData == null)
                {
                    return;
                }
                List<AchievementDataBase> list = achievementManager.GetSubAchievementDataList(typeData.type);
                dailyAchieveDataList.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].type == m_uint_activeFType && list[i].childType == m_uint_activeStype)
                    {
                        uint id = list[i].id;
                        AchieveData achieveData = achievementManager.GetAchieveData(id);
                        if (achieveData != null)
                        {
                            if (m_toggle_HideAchievement.value && achieveData.status == (uint)AchieveStatus.AchieveStatus_HaveReceive)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            achieveData = new AchieveData();
                            achieveData.id = list[i].id;
                        }
                        dailyAchieveDataList.Add(achieveData);
                    }
                }             
            }
            dailyAchieveDataList.Sort(SortAchieveData);
            m_ctor_AchievementScrollView.CreateGrids(dailyAchieveDataList.Count);
        }
    }

    int SortAchieveData(AchieveData dataA, AchieveData dataB)
    {
        if (dataA == null || dataB == null)
        {
            return 0;
        }

        if (dataA.status == (uint)AchieveStatus.AchieveStatus_CanReceive)
        {
            if (dataB.status == (uint)AchieveStatus.AchieveStatus_CanReceive)
            {
                return (int)dataA.id - (int)dataB.id;
            }
            else
            {
                return -1;
            }
        }
        else if (dataB.status == (uint)AchieveStatus.AchieveStatus_CanReceive)
        {
            return 1;
        }
        else
        {
            if (dataA.status == (uint)AchieveStatus.AchieveStatus_Proceed)
            {
                if (dataB.status == (uint)AchieveStatus.AchieveStatus_Proceed)
                {
                    return (int)dataA.id - (int)dataB.id;
                }
                else
                {
                    return -1;
                }
            }
            else if (dataB.status == (uint)AchieveStatus.AchieveStatus_Proceed)
            {
                return 1;
            }
            else
            {
                return (int)dataA.id - (int)dataB.id;
            }
        }
    }

    void UpdateCompletedAchievement()
    {
        uint haveNum = 0;
        uint totalNum = 0;
        int tempNum =0;
        IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        tempNum = UserData.AchievePoint;
        achievementManager.GetGainAchieveDot(out haveNum, out totalNum);
        m_label_CompletedAchievement.text = string.Format("{0}/{1}/{2}",tempNum , haveNum, totalNum);
        m_btn_Btn_AllReceive.isEnabled = achievementManager.HaveCanReceiveAchieve();
    }

    /// <summary>
    /// 处理服务器消息返回事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Achievement_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if (e != null)
        {
            if (e.key == AchieveDispatchEvents.GetAchieveReward.ToString())
            {
                RefreshTabData();
                UpdateAchieveContent();
                UpdateCompletedAchievement();
            }
            else if (e.key == AchieveDispatchEvents.RefreshAchieveInfo.ToString())
            {
                RefreshTabData();
                UpdateAchieveContent();
                UpdateCompletedAchievement();
            }
        }
    }

  
    void SecondTypeClick(uint childID)
    {
        AchievementDataBase typeData = achievementManager.GetParentAchievementData((int)m_uint_activeFType);
        if (typeData == null)
        {
            return;
        }
        List<AchievementDataBase> list = achievementManager.GetSubAchievementDataList(typeData.type);
        dailyAchieveDataList.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].childType == childID)
            {
                uint id = list[i].id;
                AchieveData achieveData = achievementManager.GetAchieveData(id);
                if (achieveData != null)
                {
                    if (m_toggle_HideAchievement.value && achieveData.status == (uint)AchieveStatus.AchieveStatus_HaveReceive)
                    {
                        continue;
                    }
                }
                else
                {
                    achieveData = new AchieveData();
                    achieveData.id = list[i].id;
                }
                dailyAchieveDataList.Add(achieveData);
            }
        }
        dailyAchieveDataList.Sort(SortAchieveData);
        m_ctor_AchievementScrollView.CreateGrids(dailyAchieveDataList.Count);
    }
    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
//         if (null == jumpData)
//         {
//             jumpData = new PanelJumpData();
//         }
//         int firstTabData = (null != jumpData.Tabs && jumpData.Tabs.Length >= 1) ? jumpData.Tabs[0] : 0;
//         int secondTabData =( null != jumpData.Tabs && jumpData.Tabs.Length >= 2) ? jumpData.Tabs[1]: 0;
//         SetSelectFirstType((uint)firstTabData);
//         SetSelectSecondType((uint)secondTabData);         
    }
    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Tabs = new int[2];
        pd.JumpData.Tabs[0] = (int)m_uint_activeFType;
        pd.JumpData.Tabs[1] = (int)m_uint_activeStype;
        return pd;
    }
    protected override void OnHide()
    {
        achievementManager.ValueUpdateEvent -= Achievement_ValueUpdateEvent;
        Release();
   
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_ctor_AchievementScrollView != null)
        {
            m_ctor_AchievementScrollView.Release(depthRelease);
        }
        if (mSecondTabCreator != null)
        {
            mSecondTabCreator.Release(depthRelease);
        }
        if (dailyAchieveDataList != null)
        {
            dailyAchieveDataList.Clear();
        }       
        if (achieveDic != null)
        {
            achieveDic.Clear();
        }
        if (m_uintDic != null)
       {
           m_uintDic.Clear();
       }
        m_uint_activeFType = 0;
        m_uint_activeStype = 0;
        if (null != previousGrid)
        {
            previousGrid.SetSelect(false);
            previousGrid = null;
        }
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }
}
