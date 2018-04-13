//*************************************************************************
//	创建日期:	2016-11-19 12:31
//	文件名称:	SevenDayPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	新服七天活动
//*************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCmd;
partial class SevenDayPanel : UIPanelBase
{
    WelfareManager m_dataManager = null;
    Dictionary<BtnType, List<SevenDayWelfare>> m_dicSeventWelfare = new Dictionary<BtnType, List<SevenDayWelfare>>();
    List<SevenDayWelfare> m_lstCurrWelfare = null;//当前选中的福利类型
    Dictionary<BtnType, UILabel> m_dicLable = new Dictionary<BtnType, UILabel>();
    Dictionary<int, UIWelfareTypeGrid> m_dicToggles = new Dictionary<int, UIWelfareTypeGrid>();
    UIToggle m_toggleContent1 = null;
    private int m_nIndex = 0;
    private int m_nIndexNum = 0;
    uint curSelectDay = 1;
    BtnType curType = BtnType.None;

    bool selectTargetTypeGrid = false;
    List<UIItemRewardData> m_lst_UIItemRewardDatas = null;
    List<SevenDayWelfare> awardList = null;
      List<uint> gotIDs = null;

    protected override void OnLoading()
    {
        base.OnLoading();
        m_dataManager = DataManager.Manager<WelfareManager>();
        m_dataManager.UpdateWelfareState(2);
        UIToggle[] toggles = m_trans_ToggleContent.GetComponentsInChildren<UIToggle>();      
        foreach (var item in toggles)
        {
            BtnType btntype = (BtnType)System.Enum.Parse(typeof(BtnType), item.name);
            EventDelegate.Add(item.onChange, () =>
            {
                if (item.value)
                {
                    this.OnContentToggleValueChange(btntype);
                }
            });
            if (btntype == BtnType.Content_1)
            {
                m_toggleContent1 = item;
            }
            m_dicLable.Add(btntype, item.transform.Find("Label").GetComponent<UILabel>());
        }
      
        UIEventListener.Get(m__Icon.gameObject).onClick = OnClickDiscountGift;
        UIEventListener.Get(m__Model.gameObject).onDrag = DragModel;
        awardList = m_dataManager.m_dicSevenDay[6];
    }

    void InitGrid()
    {
        if (m_ctor_ToggleScrollView != null)
        {
            m_ctor_ToggleScrollView.RefreshCheck();
            m_ctor_ToggleScrollView.Initialize<UIWelfareTypeGrid>(m_trans_UIWelfareToggleGrid.gameObject,OnWelfareBtnDataUpdate, OnWelfareBtnUIEvent);

            m_ctor_ToggleScrollView.CreateGrids((int)m_dataManager.ServerOpenMaxDay);
        }

        if (m_ctor_LevelAndTimeScrollView != null)
        {
            m_ctor_LevelAndTimeScrollView.RefreshCheck();
            m_ctor_LevelAndTimeScrollView.Initialize<UIWelfareOtherGrid>(m_trans_UIWelfareOtherGrid.gameObject,OnWelfareItemDataUpdate, null);
        }

        if (m_ctor_RewardRoot != null)
        {
            m_ctor_RewardRoot.RefreshCheck();
            m_ctor_RewardRoot.Initialize<UISevenDayRewardGrid>(m_trans_UIWelfareRewardGrid.gameObject, OnWelfareBtnDataUpdate, OnWelfareBtnUIEvent);
        }

    }

    void OnWelfareBtnDataUpdate(UIGridBase data, int index)
    {
        if (data is UIWelfareTypeGrid)
        {
            UIWelfareTypeGrid grid = data as UIWelfareTypeGrid;
            grid.SetSevenDayGridData(index);
            if (activedType == null && index == 0)
            {
                OnToggleValueChange(grid.SevenDayIndex);
                grid.OnSevenDaySelect(true);
                activedType = grid;
            }
            if (!m_dicToggles.ContainsKey(index+1))
            {
                m_dicToggles.Add(index+1, grid);
            }
        }
        if (data is UISevenDayRewardGrid)
        {
            UISevenDayRewardGrid grid = data as UISevenDayRewardGrid;
            if (m_lst_UIItemRewardDatas != null)
            {
                if (index < m_lst_UIItemRewardDatas.Count )
               {
                   grid.SetGridData(m_lst_UIItemRewardDatas[index]);
               }
            }
           
        }
    }
    UIWelfareTypeGrid activedType = null;
    void OnWelfareBtnUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UIWelfareTypeGrid)
                {
                    UIWelfareTypeGrid grid = data as UIWelfareTypeGrid;
                    //日期限制
                    if (grid.SevenDayIndex > m_dataManager.ServerOpenCurrDay)
                    {
                        return;
                    }
                    if (grid != null)
                    {
                        curType = BtnType.Content_1;
                        OnToggleValueChange(grid.SevenDayIndex);
                        if (activedType == null)
                        {
                            activedType = grid;
                        }
                        else
                        {
                            if (activedType != grid)
                            {
                                activedType.OnSevenDaySelect(false);
                            }
                        }
                        grid.OnSevenDaySelect(true);
                        activedType = grid;
                    }
                }
                else if (data is UISevenDayRewardGrid)
                {
                    UISevenDayRewardGrid grid = data as UISevenDayRewardGrid;                 
                    List<uint> gotIDs = m_dataManager.m_lstServerOpenGotReward;
                    uint curNum = m_dataManager.CurActiveNum;
                    uint id = grid._UIItemRewardDatas.DataID;
                    if (!gotIDs.Contains(id))
                    {
                        for (int i = 0; i < awardList.Count; i++)
                        {
                            if (awardList[i].id == id && curNum >= awardList[i].param1)
                            {
                                DataManager.Instance.Sender.RequestGetServerOpenReward(id);
                            }
                        }
                    }
                }
               
                break;
        }
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        InitGrid();
        m_dataManager.OnUpdateTimeEvent = OnUpdateTime;

        m_dataManager.ValueUpdateEvent += OnUpdateList;
        m_dataManager.UpdateWelfareState(2);

        StartCoroutine(SetToggleState());

        if (selectTargetTypeGrid)
        {
            ShowModel();
            return;
        }
        if (m_ctor_LevelAndTimeScrollView != null && m_lstCurrWelfare != null)
        {
            m_lstCurrWelfare.Sort();
            m_ctor_LevelAndTimeScrollView.CreateGrids(m_lstCurrWelfare.Count);
        }

        StructData();

     
    }



    void OnUpdateTime(uint remain_time)
    {
        if (remain_time > 0)
        {
            m_label_TimeLeftNum.text = DateTimeHelper.ParseTimeSeconds((int)remain_time);
        }
    }

    IEnumerator SetToggleState()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        foreach (var item in m_dicToggles)
        {
            if (item.Key > m_dataManager.ServerOpenCurrDay)
            {
                item.Value.enabled = false;
                UIEventListener.Get(item.Value.gameObject).onClick = OnToggleBtnClick;
            }
            else
            {
                item.Value.enabled = true;
            }
        }
    }

    void OnUpdateList(object obj, ValueUpdateEventArgs value)
    {
        if (value.key.Equals("OnUpdateList"))
        {
            if (selectTargetTypeGrid)
            {
                return;
            }
            m_lstCurrWelfare.Sort();
            if (m_ctor_LevelAndTimeScrollView != null)
            {
                m_ctor_LevelAndTimeScrollView.CreateGrids(m_lstCurrWelfare.Count);
                m_ctor_ToggleScrollView.CreateGrids((int)m_dataManager.ServerOpenMaxDay);
            }
            activedType.OnSevenDaySelect(true);
            OnToggleValueChange(curSelectDay);
        }
        else if (value.key.Equals("OnUpdateDisCount"))
        {
            ResreshDiscountUI();
        }
        else if (value.key.Equals("OnUpdateFinalTargetData"))
        {
            StructData();
            m_ctor_RewardRoot.UpdateActiveGridData();

        }
    }

    void OnToggleBtnClick(GameObject go)
    {
        if (go.GetComponent<UIWelfareTypeGrid>().enabled)
        {
            return;
        }

        TipsManager.Instance.ShowTips("该页面活动还未开启！");
    }

    void OnContentToggleValueChange(BtnType type)
    {
        switch (type)
        {
            case BtnType.Content_1:
            case BtnType.Content_2:
            case BtnType.Content_3:
            case BtnType.Content_4:
                {
                    if (type == BtnType.Content_1)
                    {
                        m_toggleContent1.value = true;
                    }
                    curType = type;
                    m_lstCurrWelfare = null;
                    if (m_dicSeventWelfare.ContainsKey(type))
                    {
                        m_lstCurrWelfare = m_dicSeventWelfare[type];
                        m_lstCurrWelfare.Sort();
                    }

                    if (m_lstCurrWelfare != null && m_lstCurrWelfare.Count > 0)
                    {
                        m_widget_RightContainer.enabled = false;
                        if (m_lstCurrWelfare[0].nType == 10)//打折
                        {
                            m_widget_GiftBagPanel.alpha = 1f;
                            m_widget_LevelAndTimePanel.alpha = 0f;

                            ResreshDiscountUI();
                        }
                        else
                        {
                            m_widget_RightContainer.enabled = true;

                            m_widget_GiftBagPanel.alpha = 0f;
                            m_widget_LevelAndTimePanel.alpha = 1f;

                            m_ctor_LevelAndTimeScrollView.CreateGrids(m_lstCurrWelfare.Count);
                        }
                    }
                }
                break;
            case BtnType.Max:
                break;
            default:
                break;
        }
    }
    CMResAsynSeedData<CMTexture> m_playerAvataCASD = null;
    private void ResreshDiscountUI()
    {
        SevenDayWelfare data = null;
        if (m_lstCurrWelfare.Count > 0)
        {
            data = m_lstCurrWelfare[0];
        }
        if (data == null)
        {
            return;
        }
        m_label_OldNum.text = data.param1.ToString();
        m_label_NewNum.text = data.param2.ToString();

        if (data.lstWelfareItems.Count > 0)
        {
            table.ItemDataBase itemDb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(data.lstWelfareItems[0].itemid);
            if (itemDb != null)
            {
                UIManager.GetTextureAsyn(itemDb.itemIcon, ref m_playerAvataCASD, () =>
                {
                    if (null != m__Icon)
                    {
                        m__Icon.mainTexture = null;
                    }
                }, m__Icon);

                m_label_GiftBagName.text = itemDb.itemName;
            }
        }

        if (data.state == QuickLevState.QuickLevState_HaveGet)
        {
            m_btn_Btn_Buy.isEnabled = false;
            m_btn_Btn_Buy.GetComponentInChildren<UILabel>().text = "已购买";
        }
        else
        {
            m_btn_Btn_Buy.isEnabled = true;
            m_btn_Btn_Buy.GetComponentInChildren<UILabel>().text = "购买";
        }
    }

    void OnClickDiscountGift(GameObject go)
    {
        SevenDayWelfare data = null;
        if (m_lstCurrWelfare.Count > 0)
        {
            data = m_lstCurrWelfare[0];
        }
        if (data != null && data.lstWelfareItems.Count > 0)
        {
            TipsManager.Instance.ShowItemTips(data.lstWelfareItems[0].itemid, m__Icon.gameObject, false);
        }
    }

    void OnToggleValueChange(uint type )
    {
        m_trans_DayRoot.gameObject.SetActive(true);
        m_trans_TargetRoot.gameObject.SetActive(false);
        selectTargetTypeGrid = false;
        switch (type)
        {       case 7:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                {
                    curSelectDay = type;
                    foreach (var lable in m_dicLable)
                    {
                        lable.Value.transform.parent.gameObject.SetActive(false);
                    }
                    m_dicSeventWelfare.Clear();

                    List<SevenDayWelfare> lstdata = m_dataManager.GetSevenDayWelfareByDay((uint)type);
                    for (int ix = 0; ix < lstdata.Count; ix++)
                    {
                        for (int jx = ix;jx< lstdata.Count; jx++)
                        {
                            if (lstdata[ix].SortID > lstdata[jx].SortID)
                            {
                                SevenDayWelfare temp = lstdata[jx];
                                lstdata[jx] = lstdata[ix];
                                lstdata[ix] = temp;
                            }
                        }
                    }
                    //遍历获取有几项福利
                    List<uint> keys = new List<uint>();
                    for (int i = 0; i < lstdata.Count ; i++)
                    {
                        if (!keys.Contains(lstdata[i].nType))
                        {
                            keys.Add(lstdata[i].nType);

                        }
                    }
                    m_nIndexNum = keys.Count;
                    //存储对应的福利
                    for (int i = 0; i < keys.Count; i++)
                    {
                        BtnType btnType = (BtnType)System.Enum.Parse(typeof(BtnType), string.Format("Content_{0}", (i + 1)));
                           GameObject content = null;
                           GameObject redPoint  = null;
                        if (m_dicLable.ContainsKey(btnType) && m_dicLable[btnType] != null)
                        {
                            content = m_dicLable[btnType].transform.parent.gameObject;
                            if(content != null)
                            {
                                content.SetActive(true);
                                redPoint = content.transform.Find("Warrning").gameObject;
                            }                        
                        }

                        for (int k = 0; k < lstdata.Count; k++)
                        {
                            if (!m_dicSeventWelfare.ContainsKey(btnType))
                            {
                                m_dicSeventWelfare.Add(btnType, new List<SevenDayWelfare>());
                            }

                            if (lstdata[k].nType == keys[i])
                            {
                                m_dicSeventWelfare[btnType].Add(lstdata[k]);
                            }
                        }
                        bool hasRewardInContent = false;
                        for (int m = 0; m < m_dicSeventWelfare[btnType].Count; m++)
                        {
                            if (m_dicSeventWelfare[btnType][m].state == QuickLevState.QuickLevState_CanGet)
                            {
                                hasRewardInContent = true;
                            }
                        }
                   
                        if (redPoint != null)
                        {
                            redPoint.SetActive(hasRewardInContent);
                        }
                   
                        SetToggleContentLabel(keys[i], i);
                    }
                    OnContentToggleValueChange(curType);
                }
                break;
            default:
                break;
        }
    }

    private void SetToggleContentLabel(uint nid, int nIndex)
    {
        m_nIndex = nIndex + 1;

        LocalTextType textType = (LocalTextType)System.Enum.Parse(typeof(LocalTextType), string.Format("Local_TXT_Welfare7Day_{0}", nid));
        BtnType btnType = (BtnType)System.Enum.Parse(typeof(BtnType), string.Format("Content_{0}", m_nIndex));
        if (m_dicLable.ContainsKey(btnType) && m_dicLable[btnType] != null)
        {
            m_dicLable[btnType].text = DataManager.Manager<TextManager>().GetLocalText(textType);
        }
    }

    void OnWelfareItemDataUpdate(UIGridBase data, int index)
    {
        if (data is UIWelfareOtherGrid)
        {
            UIWelfareOtherGrid grid = data as UIWelfareOtherGrid;
            if (null != m_lstCurrWelfare && index < m_lstCurrWelfare.Count)
            {
                grid.SetGridData(m_lstCurrWelfare[index]);
            }
        }      
    }


    void onClick_Btn_Buy_Btn(GameObject caster)
    {
        if (m_lstCurrWelfare.Count > 0)
        {
            if (MainPlayerHelper.IsHasEnoughMoney(ClientMoneyType.YuanBao, m_lstCurrWelfare[0].param1))
            {
                DataManager.Instance.Sender.RequestGetServerOpenReward(m_lstCurrWelfare[0].id);
            }
        }
    }

    void OnBtnsClick(BtnType btntype)
    {

    }
    void onClick_Btn_close_Btn(GameObject caster)
    {
        HideSelf();
    }


    void onClick_FinalTarget_Btn(GameObject caster)
    {
        selectTargetTypeGrid = true;
        if (activedType != null)
        {
            activedType.OnSevenDaySelect(false);
        }
        if (! m_trans_TargetRoot.gameObject.activeSelf)
        {
            m_trans_DayRoot.gameObject.SetActive(false);
            m_trans_TargetRoot.gameObject.SetActive(true);
        }
        ShowModel();
            uint total = m_dataManager.TotalActiveNum;
            uint curNum = m_dataManager.CurActiveNum;
            float value = curNum * 1.0f / total;
            int presentNum = 0;
            uint gap = 0;
          
            StructData();
            for (int i = 0; i < awardList.Count; i++)
            {                         
                if (curNum >= awardList[i].param1)
                {
                    presentNum = i + 1;
                    gap = curNum - awardList[i].param1;
                    if (presentNum == awardList.Count)
                    {
                        gap = 0;
                    }
                }
                else
                {
                    if (presentNum == 0)
                    {
                        gap = curNum;
                    }
                }
            }
            if (awardList.Count == 0)
            {
                return;
            }
            if (presentNum > 0)
            {
                if (presentNum == awardList.Count)
                {
                    m_slider_HuoDongSlider.value = 1.0f;
                }
                else 
                {
                    m_slider_HuoDongSlider.value = 1.0f * presentNum / awardList.Count + (gap* 1.0f/ (awardList[presentNum].param1 - awardList[presentNum - 1].param1)) * (1.0f / awardList.Count);
                }
             
            }
            else
            {
                m_slider_HuoDongSlider.value = (gap * 1.0f / awardList[presentNum].param1) * (1.0f / awardList.Count) ;
            }
            m_label_HuoDongDianLabel.text = curNum.ToString();
            m_ctor_RewardRoot.CreateGrids(m_lst_UIItemRewardDatas.Count);
                
    }

    void StructData() 
    {
        if (m_lst_UIItemRewardDatas == null)
        {
            m_lst_UIItemRewardDatas = new List<UIItemRewardData>();
        }
        else 
        {
            m_lst_UIItemRewardDatas.Clear();
        }
        int tempCount = 0;
       gotIDs =  m_dataManager.m_lstServerOpenGotReward;
       uint curNum = m_dataManager.CurActiveNum;
       for (int i = 0; i < awardList.Count; i++)
       {
           for (int m = 0; m < awardList[i].lstWelfareItems.Count; m++)
           {
               m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
               {
                   itemID = awardList[i].lstWelfareItems[m].itemid,
                   num =awardList[i].param1, //奖励道具不需要数量，这个字段替换成当前表格数据的参数1用来显示 =_=
                   blockVisible = curNum < awardList[i].param1,
                   hasGot = gotIDs.Contains(awardList[i].id),
                   DataID = awardList[i].id,
               });
           }
           bool canOpenBox = !gotIDs.Contains(awardList[i].id) && curNum >= awardList[i].param1;
           if (canOpenBox)
           {
               tempCount++;
           }
       }
       m_sprite_Warrning.gameObject.SetActive(tempCount > 0); 
    }



    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_ctor_ToggleScrollView != null)
        {
            m_ctor_ToggleScrollView.Release(depthRelease);
        }
        if (m_RTObj != null)
        {
            m_RTObj.Release();
        }


        if (m_playerAvataCASD != null)
        {
            m_playerAvataCASD.Release();
            m_playerAvataCASD = null;
        }
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        Release();
    
    }

    protected override void OnHide()
    {
        base.OnHide();
        m_dataManager.ValueUpdateEvent -= OnUpdateList;
        m_dataManager.OnUpdateTimeEvent = null;
        Release();
    }


    #region model
    float rotateY = -8;
    private IRenerTextureObj m_RTObj = null;

    void ShowModel()
    {
        int npcID = GameTableManager.Instance.GetGlobalConfig<int>("FinalTargetModelID");
        table.NpcDataBase npcData = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)npcID);
        if (npcData != null)
        {
           uint modelID = npcData.dwViewModelSet;  // 使用观察模型
            if (m_RTObj != null)
            {
                m_RTObj.Release();
            }

            m_RTObj = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj((int)modelID, 800);
            if (m_RTObj == null)
            {
                return;
            }

            m_RTObj.SetCamera(new Vector3(0, 1, 0), Vector3.zero, -4.85f);
            m_RTObj.SetModelRotateY(rotateY);
            //人物 
            if (m__Model != null)
            {
                m__Model.mainTexture = m_RTObj.GetTexture();
                m__Model.MakePixelPerfect();
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
    #endregion
}
