//*************************************************************************
//	创建日期:	2016-11-19 11:23
//	文件名称:	WelfarePanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	福利界面
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
using table;
using Engine;
using GameCmd;
partial class WelfarePanel : UIPanelBase
{
    private List<WelfareType> m_lstWelfare = null;
    private List<WelfareData> m_lstWelFareData = null;
    private List<RewardFindData> m_lstRewardFindData = null;
    //private List<CollectWordData> m_lstCollectWordData = null;
    private WelfareManager m_dataManager = null;

    protected override void OnLoading()
    {
        base.OnLoading();

        m_dataManager = DataManager.Manager<WelfareManager>();
        m_dataManager.UpdateWelfareState(1);
        m_widget_OtherPanel.alpha = 1f;
        m_widget_CheckInPanel.alpha = 1f;
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.GETVERIFYNUM, EventCallBack);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.BINDPHONESUCESS, EventCallBack);
        AddCreator(m_trans_BindRewardRoot);
        InitGrid();
    }

    void EventCallBack(int nEventID, object param)
    {

        if (nEventID == (int)Client.GameEventID.GETVERIFYNUM)
        {
            m_btn_GetBtn.isEnabled = true;
//            m_btn_GetBtn.transform.GetComponent<UISprite>().spriteName = "anniu_huang_xiao";
            RefreshBindBtns();
        }
        if (nEventID == (int)Client.GameEventID.BINDPHONESUCESS)
        {
            m_btn_BindBtn.gameObject.SetActive(false);
            bool canGet = (bool)param;
            RefreshBindBtns(true);
        }
    }
    void InitGrid()
    {
        //福利类型
        m_ctor_ToggleScrollView.RefreshCheck();
        m_ctor_ToggleScrollView.Initialize<UIWelfareTypeGrid>(m_trans_UIWelfareToggleGrid.gameObject,  OnWelfareBtnDataUpdate, OnWelfareBtnUIEvent);
/*        m_ctor_ToggleScrollView.Initialize<UIWelfareTypeGrid>()*/


        //其他福利
        m_ctor_OtherScrollView.RefreshCheck();
        m_ctor_OtherScrollView.Initialize<UIWelfareOtherGrid>(m_trans_UIWelfareOtherGrid.gameObject, OnWelfareItemDataUpdate, null);

        //月签到
        m_ctor_CheckInScrollView.RefreshCheck();
        m_ctor_CheckInScrollView.Initialize<UIWelfareCheckGrid>(m_trans_UIWelfareCheckGrid.gameObject,OnMonthItemDataUpdate, OnMonthItemClickUIEvent);

        //奖励找回
        m_ctor_RewardFindScroll.RefreshCheck();
        m_ctor_RewardFindScroll.Initialize<UIRewardFindGrid>(m_trans_UIRewardFindGrid.gameObject, OnRewardFindGridUpdate, null);

        //玩家招募
        m_ctor_FriendInviteScroll.RefreshCheck();
        m_ctor_FriendInviteScroll.Initialize<UIWelfareOtherGrid>(m_trans_UIWelfareOtherGrid.gameObject, OnWelfareItemDataUpdate, null);
        
        //集文活动

        m_ctor_CollectWordScrollView.RefreshCheck();
        m_ctor_CollectWordScrollView.Initialize<UICollectWordGrid>(m_trans_UICollectionWordGrid.gameObject, OnCollectWordGridUpdate, null);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        m_dataManager.UpdateWelfareState(1);

        m_dataManager.ValueUpdateEvent += OnUpdateList;

        if (m_ctor_ToggleScrollView != null)
        {
            if (m_lstWelfare == null)
            {
                m_lstWelfare = new List<WelfareType>();
            }
            m_lstWelfare.Clear();
            m_dataManager.GetAllWelfareType(ref m_lstWelfare);

            m_ctor_ToggleScrollView.CreateGrids(m_lstWelfare.Count);

            if (m_lstWelfare.Count > 0)
            {
                OnToggleWelfare(m_lstWelfare[0]);
            }
        }

      
    }


    
    protected override void OnJump(PanelJumpData jumpData)
    {
        base.OnJump(jumpData);

        int firstTab = -1;
        if (jumpData == null)
        {
            jumpData = new PanelJumpData();
        }
        if (firstTab == -1)
        {
            firstTab = (null != jumpData.Tabs && jumpData.Tabs.Length >= 1) ? jumpData.Tabs[0] : 1;
        }

        if (m_ctor_ToggleScrollView != null)
        {
            List<UIWelfareTypeGrid> lstgrids = m_ctor_ToggleScrollView.GetGrids<UIWelfareTypeGrid>();
            for (int i = 0; i < lstgrids .Count; i++)
            {
                if ((int)lstgrids[i].Welfare == firstTab)
                {
                    OnToggleWelfare(lstgrids[i].Welfare);
                }
            }
        }
    }

    void OnUpdateList(object obj, ValueUpdateEventArgs value)
    {
        if (value.key.Equals("OnUpdateList"))
        {
            m_lstWelFareData.Sort();
            if (m_ctor_OtherScrollView != null)
            {
                m_ctor_OtherScrollView.CreateGrids(m_lstWelFareData.Count);
                m_ctor_ToggleScrollView.UpdateActiveGridData();
            }
        }
        else if (value.key.Equals("OnUpdateInviteList"))
        {
            //m_lstWelFareData = m_dataManager.GetWelfareDatasBy2Type(WelfareType.FriendInvite, curInviteType);
            m_lstWelFareData.Sort();
            if (m_ctor_FriendInviteScroll != null)
            {
                m_ctor_FriendInviteScroll.CreateGrids(m_lstWelFareData.Count);
                m_ctor_ToggleScrollView.UpdateActiveGridData();
            }
        }
        else if (value.key.Equals("OnUpdateMonth"))
        {
            if (m_ctor_CheckInScrollView != null)
            {
                List<UIWelfareCheckGrid> grids = m_ctor_CheckInScrollView.GetGrids<UIWelfareCheckGrid>();
                for (int i = 0; i < grids.Count; i++)
                {
                    grids[i].RefreshSigne();
                }
            }
            CheckEnableSignBtn();
        }
        else if (value.key.Equals("OnUpdateSingleRewardFind")) 
        {
            m_lstRewardFindData.Sort();
            if (m_ctor_RewardFindScroll != null)
            {
                List<UIRewardFindGrid> grids = m_ctor_RewardFindScroll.GetGrids<UIRewardFindGrid>();
                for (int i = 0; i < grids.Count; i ++ )
                {
                    RewardFindData data = (RewardFindData)value.newValue;
                    if (data == null)
                    {
                        Engine.Utility.Log.Error("data == null");
                        return;
                    }
                    if (grids[i].RewardFindID == data.id)
                     {
                         grids[i].SetGridData(data);
                     }
                }       
            }
        }
        else if (value.key.Equals("OnUpdateAllRewardFind"))
        {
            m_lstRewardFindData.Sort();
            if (m_ctor_RewardFindScroll != null)
            {
                m_ctor_RewardFindScroll.CreateGrids(m_lstRewardFindData.Count);
            }
        }
        else if (value.key.Equals("OnUpdateCollectWord"))
        {          
            m_lstWelFareData = m_dataManager.GetWelfareDatasByType(WelfareType.CollectWord);
            m_lstWelFareData.Sort();
            if (m_ctor_CollectWordScrollView != null)
            {
                m_ctor_CollectWordScrollView.CreateGrids(m_lstWelFareData.Count);
            }
            if (m_ctor_ToggleScrollView != null)
            {
                m_ctor_ToggleScrollView.UpdateActiveGridData();
            }
        }
        else if (value.key.Equals("OnHideAllRewardFind"))
        {
            m_trans_NullRewardTipsContent.gameObject.SetActive(true);
            m_ctor_RewardFindScroll.SetVisible(false);
            if (m_ctor_ToggleScrollView != null)
            {
                m_ctor_ToggleScrollView.UpdateActiveGridData();
            }
        }
        else if (value.key.Equals("OnUpdateBindPhone"))
        {
            RefreshBindBtns();
            m_ctor_ToggleScrollView.UpdateActiveGridData();
        }
        else if (value.key.Equals("OnChangeInviteState"))
        {
            ChangeInviteState();
            m_lstWelFareData.Sort();
            if (m_ctor_FriendInviteScroll != null)
            {
                m_ctor_FriendInviteScroll.CreateGrids(m_lstWelFareData.Count);
                m_ctor_ToggleScrollView.UpdateActiveGridData();
            }
        }
        else if (value.key.Equals("OnUpdateRushLevel"))
        {
            m_lstWelFareData.Sort();
            if (m_ctor_OtherScrollView != null)
            {
                m_ctor_OtherScrollView.CreateGrids(m_lstWelFareData.Count);
            }
            if (m_ctor_ToggleScrollView != null)
            {
                m_ctor_ToggleScrollView.UpdateActiveGridData();
            }
        }
        else if (value.key.Equals("RecieveInvitedPlayerData"))
        {
            m_lstWelFareData = m_dataManager.GetWelfareDatasBy2Type(WelfareType.FriendInvite, curInviteType);
            m_lstWelFareData.Sort();
            if (m_ctor_FriendInviteScroll != null)
            {
                m_ctor_FriendInviteScroll.CreateGrids(m_lstWelFareData.Count);
                m_ctor_ToggleScrollView.UpdateActiveGridData();
            }
        }
        
    }

#region 左边的福利类型按钮grid
    void OnWelfareBtnDataUpdate(UIGridBase data, int index)
    {
        UIWelfareTypeGrid type = data as UIWelfareTypeGrid;
        if (null != m_lstWelfare && index < m_lstWelfare.Count)
        {
            type.SetGridData(m_lstWelfare[index]);
        }
    }
    void onClick_GetBtn_Btn(GameObject caster)
    {
        List<uint> lstids = new List<uint>();
        List<WelfareDataBase> lis = GameTableManager.Instance.GetTableList<WelfareDataBase>();
        for (int i = 0; i < lis.Count;i ++ )
        {
            if (lis[i].dwType ==(uint)WelfareType.BindGift)
            {
                lstids.Add(lis[i].dwID);
            }
        }
        DataManager.Instance.Sender.ReqGetReward(ref lstids);
    }
    void onClick_BindBtn_Btn(GameObject caster)
    {
        ActiveTakeParam par = new ActiveTakeParam();
        par.type = ActiveTakeType.Bind;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ActiveTakePanel, data: par);
    }

    void RefreshItems(List<WelfareItem> items)
    {
        m_lst_UIItemRewardDatas.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
            {
                itemID = items[i].itemid,
                num = items[i].itemNum,
            });
        }
        m_ctor_UIItemRewardCreator.CreateGrids(m_lst_UIItemRewardDatas.Count);
    }
        #region UIItemRewardGridCreator
    UIGridCreatorBase m_ctor_UIItemRewardCreator;
    List<UIItemRewardData> m_lst_UIItemRewardDatas = new List<UIItemRewardData>();
    void AddCreator(Transform parent)
    {
        if (parent != null)
        {
            m_ctor_UIItemRewardCreator = parent.GetComponent<UIGridCreatorBase>();
            if (m_ctor_UIItemRewardCreator == null)
            {
                m_ctor_UIItemRewardCreator = parent.gameObject.AddComponent<UIGridCreatorBase>();
            }
            m_ctor_UIItemRewardCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            m_ctor_UIItemRewardCreator.gridWidth = 90;
            m_ctor_UIItemRewardCreator.gridHeight = 90;
            m_ctor_UIItemRewardCreator.RefreshCheck();
            m_ctor_UIItemRewardCreator.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateGridData, null);
        }
    }
    void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (grid is UIItemRewardGrid)
        {
            UIItemRewardGrid itemShow = grid as UIItemRewardGrid;
            if (itemShow != null)
            {
                if (index < m_lst_UIItemRewardDatas.Count)
                {
                    UIItemRewardData data = m_lst_UIItemRewardDatas[index];
                    uint itemID = data.itemID;
                    uint num = data.num;
                    itemShow.SetGridData(itemID, num, false);
                }
            }
        }
    }
    #endregion

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_ctor_CheckInScrollView != null)
        {
            m_ctor_CheckInScrollView.Release(depthRelease);
        }
        if (m_ctor_OtherScrollView != null)
        {
            m_ctor_OtherScrollView.Release(depthRelease);
        }
        if (m_ctor_RewardFindScroll != null)
        {
            m_ctor_RewardFindScroll.Release(depthRelease);
        }
        if (m_ctor_FriendInviteScroll != null)
        {
            m_ctor_FriendInviteScroll.Release(depthRelease);
        }
        if (m_ctor_ToggleScrollView != null)
        {
            m_ctor_ToggleScrollView.Release(depthRelease);
        }
// 
//         if (iuiIconAtlas != null)
//         {
//             iuiIconAtlas.Release(depthRelease);
//             iuiIconAtlas = null;
//         }
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.GETVERIFYNUM, EventCallBack);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.BINDPHONESUCESS, EventCallBack);
        Release();
    }

    protected override void OnHide()
    {
        base.OnHide();
        m_dataManager.ValueUpdateEvent -= OnUpdateList;
        Release();
    }

    void RefreshBindBtns(bool canGet = false) 
    {
        bool isbind = DataManager.Manager<DailyManager>().isBindFinish;
        bool canget = DataManager.Manager<WelfareManager>().HasRewardInType(1, (uint)WelfareType.BindGift) || canGet;
        m_btn_BindBtn.gameObject.SetActive(!isbind);
        m_btn_GetBtn.isEnabled = isbind;
        m_sprite_m_bind_red.gameObject.SetActive(isbind && canget);
        m_sprite_Status_Received.gameObject.SetActive(isbind && !canget);
        m_btn_GetBtn.gameObject.SetActive(!(isbind && !canget));
    }
//    WelfareType curType = WelfareType.Month;
    void OnToggleWelfare(WelfareType type)
    {
        List<UIWelfareTypeGrid> lstgrid = m_ctor_ToggleScrollView.GetGrids<UIWelfareTypeGrid>(true);
        for (int i = 0; i < lstgrid.Count; i++)
        {
            lstgrid[i].OnWelfareSelect(type);
        }
//         UIWelfareTypeGrid grid = m_ctor_ToggleScrollView.GetGrid<UIWelfareTypeGrid>(m_lstWelfare.IndexOf(curType));
//         if (grid != null)
//         {
//             grid.OnWelfareSelect(false);
//         }
//         grid = m_ctor_ToggleScrollView.GetGrid<UIWelfareTypeGrid>(m_lstWelfare.IndexOf(type));
//         if (grid != null)
//         {
//             grid.OnWelfareSelect(true);
//         }
//        curType = type;
        m_widget_CheckInPanel.gameObject.SetActive(false);
        m_widget_OtherPanel.gameObject.SetActive(false);
        m_widget_RewardFindPanel.gameObject.SetActive(false);
        m_widget_BindContent.gameObject.SetActive(false);
        m_widget_FriendInviteContent.gameObject.SetActive(false);
        m_widget_CDkeyContent.gameObject.SetActive(false);
        m_widget_CollectWordContent.gameObject.SetActive(false);
        m__huodong_beijing.gameObject.SetActive(true);
        m_label_ScheduleLabel.gameObject.SetActive(type == WelfareType.CollectWord || type == WelfareType.SevenDay);
        if (type == WelfareType.FriendInvite)
        {
            m_lstWelFareData = m_dataManager.GetWelfareDatasBy2Type(type,curInviteType);
        }
        else
        {
            m_lstWelFareData = m_dataManager.GetWelfareDatasByType(type);
        }
       
        if (type == WelfareType.BindGift)
        {
            m_widget_BindContent.gameObject.SetActive(true);
            WelfareBaseData data = (WelfareBaseData)m_lstWelFareData[0];
            RefreshItems(data.lstWelfareItems);
            RefreshBindBtns();
        }
        else if (type == WelfareType.RewardFind)
        {
            m_widget_RewardFindPanel.gameObject.SetActive(true);
            bool hasReward = m_dataManager.HasRewardCanReBack();
            if (hasReward)
            {
                m_trans_NullRewardTipsContent.gameObject.SetActive(false);
                m_ctor_RewardFindScroll.SetVisible(true);
                m_lstRewardFindData = m_dataManager.M_lstReward;
                if (m_lstRewardFindData != null)
                {
                    m_lstRewardFindData.Sort();
                    if (m_ctor_RewardFindScroll != null)
                    {
                        m_ctor_RewardFindScroll.CreateGrids(m_lstRewardFindData.Count);
                    }
                }
            }
            else
            {
                m_trans_NullRewardTipsContent.gameObject.SetActive(true);
                m_ctor_RewardFindScroll.SetVisible(false);
            }
        }
        else if (type == WelfareType.FriendInvite)
        {
            m_label_InviteCode.text = MainPlayerHelper.GetPlayerID().ToString();
            m_widget_FriendInviteContent.gameObject.SetActive(true);          
            m__huodong_beijing.gameObject.SetActive(false);
            m_lstWelFareData.Sort();
            if (m_ctor_FriendInviteScroll != null)
            {
                m_ctor_FriendInviteScroll.CreateGrids(m_lstWelFareData.Count);
            }
            m_btn_InviterBtn.GetComponent<UIToggle>().value = curInviteType == InviteType.Inviter;
            m_btn_InvitedBtn.GetComponent<UIToggle>().value = curInviteType == InviteType.Invited;
            m_btn_InvitedRechargeBtn.GetComponent<UIToggle>().value = curInviteType == InviteType.InvitedRecharge;
            ChangeInviteState();
          
        }
        else if (type == WelfareType.CDKey)
        {
            m_widget_CDkeyContent.gameObject.SetActive(true);
        }
        else if (type == WelfareType.CollectWord)
        {          
            m_lstWelFareData.Sort();
            m_widget_CollectWordContent.gameObject.SetActive(true);
            if (m_ctor_CollectWordScrollView != null)
            {
                m_ctor_CollectWordScrollView.CreateGrids(m_lstWelFareData.Count);
            }
            m_label_ScheduleLabel.text = m_dataManager.GetScheduleByType((uint)type);
        }
        else if (type == WelfareType.SevenDay)
        {
            m_label_ScheduleLabel.text = m_dataManager.GetScheduleByType((uint)type);
            m_lstWelFareData.Sort();
            m_widget_OtherPanel.gameObject.SetActive(true);
            if (m_ctor_OtherScrollView != null)
            {
                m_ctor_OtherScrollView.CreateGrids(m_lstWelFareData.Count);
            }
        }
        else 
        {
            m_widget_OtherPanel.gameObject.SetActive(type != WelfareType.Month);
            m_widget_CheckInPanel.gameObject.SetActive(type == WelfareType.Month);
            m_lstWelFareData.Sort();
            if (type == WelfareType.Month)
            {
                if (m_ctor_CheckInScrollView != null)
                {
                    m_ctor_CheckInScrollView.CreateGrids(m_lstWelFareData.Count);
                }
                CheckEnableSignBtn();
            }
            else
            {
                if (type == WelfareType.RushLevel)
                {
                    NetService.Instance.Send(new stReqLevGiftNumsDataUserCmd_C());
                }
                if (m_ctor_OtherScrollView != null)
                {
                    m_ctor_OtherScrollView.CreateGrids(m_lstWelFareData.Count);
                }
            }
        }
        ChangeTexture(type);     
    }
    CMResAsynSeedData<CMTexture> iuiIconAtlas = null;
    void ChangeTexture(WelfareType type) 
    {
        uint resID = DataManager.Manager<WelfareManager>().GetTextureNameByWelfareType((uint)type);
        UIManager.GetTextureAsyn(resID, ref iuiIconAtlas,
                   () =>
                   {
                       if (m__wenzi != null) { m__wenzi.mainTexture = null; }
                   },
               m__wenzi);
    }

    void OnWelfareBtnUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                UIWelfareTypeGrid grid = data as UIWelfareTypeGrid;
                if (grid != null)
                {
                    OnToggleWelfare(grid.Welfare);
                }
                break;
        }
    }
#endregion


#region 右边福利领取

    void OnWelfareItemDataUpdate(UIGridBase data, int index)
    {
        if (null != m_lstWelFareData && index < m_lstWelFareData.Count)
        {
            data.SetGridData(m_lstWelFareData[index]);

        }
    }
    void OnWelfareInviteDataGrid(UIGridBase data, int index) 
    {
        if (null != m_lstWelFareData && index < m_lstWelFareData.Count)
        {
            data.SetGridData(m_lstWelFareData[index]);

        }
    }
    void OnRewardFindGridUpdate(UIGridBase data, int index)
    {
        if (null != m_lstRewardFindData && index < m_lstRewardFindData.Count)
        {
            data.SetGridData(m_lstRewardFindData[index]);

        }
    }
    void OnCollectWordGridUpdate(UIGridBase data, int index)
    {
        if (null != m_lstWelFareData && index < m_lstWelFareData.Count)
        {
            data.SetGridData(m_lstWelFareData[index]);

        }
    }
#endregion


#region 月签到
    void OnMonthItemDataUpdate(UIGridBase data, int index)
    {
        if (null != m_lstWelFareData && index < m_lstWelFareData.Count)
        {
            data.SetGridData(m_lstWelFareData[index]);
        }
    }

    void OnMonthItemClickUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                UIWelfareCheckGrid grid = data as UIWelfareCheckGrid;
                if (grid != null)
                {
                    if (grid.State == QuickLevState.QuickLevState_CanGet)//当天可以签到
                    {
                        List<uint> lstids = new List<uint>();
                        lstids.Add(grid.WelfareId);
                        DataManager.Instance.Sender.ReqGetReward(ref lstids);
                    }
                    else
                    {
                        TipsManager.Instance.ShowItemTips(grid.ItemId, grid.gameObject, false);
                    }
                }
                break;
        }
    }
#endregion

    void CheckEnableSignBtn()
    {
        m_btn_Btn_Check.isEnabled = m_dataManager.IsSignCurrDay;

        if (m_dataManager.IsSignCurrDay && m_lstWelFareData.Count >= m_dataManager.CurrDay)
        {
            if (m_lstWelFareData[(int)m_dataManager.CurrDay - 1].state == QuickLevState.QuickLevState_HaveGet)//没有可补签的
            {
                m_btn_Btn_Check.isEnabled = false;
            }
            else
            {
                m_btn_Btn_Check.isEnabled = true;
            }

            m_ctor_ToggleScrollView.CreateGrids(m_lstWelfare.Count);
        }
        //string str = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_FM_WelfareMonth_Sign);

        m_label_CheckDayNum.text = m_dataManager.SignDay.ToString();
    }

    void onClick_Btn_Check_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ReplenishSignPanel);
    }
    void onClick_Btn_close_Btn(GameObject caster)
    {
        HideSelf();
    }


    void onClick_PasteBtn_Btn(GameObject caster)
    {
//         Event ev = new Event();
//         ev.keyCode = KeyCode.V;
//         ev.modifiers = EventModifiers.Control;
//         m_input_keyInput.ProcessEvent(ev);
    }

    void onClick_ConvertBtn_Btn(GameObject caster)
    {
        if (string.IsNullOrEmpty(m_input_keyInput.value))
        {
            TipsManager.Instance.ShowTips("输入框为空");
        }
        else 
        {
            NetService.Instance.Send(new GameCmd.stCDKeyExchangeDataUserCmd_CS() { cd_key = m_input_keyInput.value });

            WaitPanelShowData waitData = new WaitPanelShowData();
            waitData.type = WaitPanelType.CDKey;
            waitData.cdTime = 5;
            waitData.des = "请耐心等待...";
            waitData.timeOutDel = delegate { DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel); };
            waitData.useBoxMask = false;
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonWaitingPanel, data: waitData);
        }
    }
}
