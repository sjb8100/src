//*************************************************************************
//	创建日期:	2016/11/4 9:59:01
//	文件名称:	ConsignmentPanel
//   创 建 人:   zhudianyu	
//	版权所有:	中青宝
//	说    明:	寄售界面
//*************************************************************************
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using Engine.Utility;
using table;
using GameCmd;
using DG.Tweening;
using Vector2 = UnityEngine.Vector2;

partial class ConsignmentPanel
{
    public enum ConsignmentPanelPageEnum
    {
        None = 0,
        Page_Item = 1,
        Page_Coin = 2,
        Max,
    }
    ConsignmentItemMode m_saleUIToggle = ConsignmentItemMode.Buy;
    ShowWhatItem curShowWhatItem = ShowWhatItem.ShowNormalItem;
    private UIGridCreatorBase tabCreator = null;
    UITabGrid previous = null;


    SaleMoneyDataManager SaleDataManager
    {
        get
        {
            return DataManager.Manager<SaleMoneyDataManager>();
        }
    }
    #region panel override
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }
    protected override void OnLoading()
    {
        base.OnLoading();
      
      
    }
    void OnEvent(int nEventID,object param) 
    {
        switch (nEventID) 
        {
            case (int)Client.GameEventID.SHOWCONSIGNMENTLIST:
                ItemPriceParam info = (ItemPriceParam)param;
                OnShowItemPriceListArea(info);

                break;
        }
    }
    protected override void OnHide()
    {
        Release();
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        RegisterGlobalEvent(false);
        if (tabCreator != null)
        {
            tabCreator.Release(depthRelease);
        }
        if (m_ctor_Toggles != null)
        {
            m_ctor_Toggles.Release(depthRelease);
        }
        if (m_ctor_ItemGridScrollView != null)
        {
            m_ctor_ItemGridScrollView.Release(depthRelease);
        }
        if (m_ctor_PetGridScrollView != null)
        {
            m_ctor_PetGridScrollView.Release(depthRelease);
        }
        if (m_ctor_ItemListGrid != null)
        {
            m_ctor_ItemListGrid.Release(depthRelease);
        }
        if (m_ctor_SellListScrollViewContent != null)
        {
            m_ctor_SellListScrollViewContent.Release(depthRelease);
        }
        if (m_ctor_SellItemPriceList != null)
        {
            m_ctor_SellItemPriceList.Release(depthRelease);
        }
        if (mSecondTabCreator != null)
        {
            mSecondTabCreator.Release(depthRelease);
        }
        if (previous != null)
        {
            previous.Release(false);
        }
        if(m_sellItemGrid != null)
        {
            m_sellItemGrid.Release(false);
        }
        if (TargetBuyGrid != null)
        {
            TargetBuyGrid.Release(false);
        }
        SaleItemDataManager.ValueUpdateEvent -= SaleItemDataManager_ValueUpdateEvent;
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();

//         if (previous != null)
//         {
//             UIManager.OnObjsRelease(previous.CacheTransform, (uint)GridID.Uitabgrid);
//             previous = null;
//         }
//         if (m_sellItemGrid != null)
//         {
//             UIManager.OnObjsRelease(m_sellItemGrid.CacheTransform, (uint)GridID.Uiitemgrid);
//             m_sellItemGrid = null;
//         }
//         if (TargetBuyGrid != null)
//         {
//             UIManager.OnObjsRelease(TargetBuyGrid.CacheTransform, (uint)GridID.Uiconsignmentitemlistgrid);
//             TargetBuyGrid = null;
//         }
        SaleDataManager.ValueUpdateEvent -= SaleDataManager_ValueUpdateEvent;
       
    }
    private void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SHOWCONSIGNMENTLIST, OnEvent);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SHOWCONSIGNMENTLIST, OnEvent);
        }
    }
    void InitSaleMoney()
    {
        if (null == tabCreator)
        {
            tabCreator = m_trans_up.gameObject.AddComponent<UIGridCreatorBase>();
        }  
        tabCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
        tabCreator.gridWidth = 117;
        tabCreator.gridHeight = 50;
        tabCreator.RefreshCheck();

        tabCreator.Initialize<UITabGrid>(m_trans_UITabGrid.gameObject, OnUIGridUpdate, OnUIGridEventDlg);   
   

        tabCreator.CreateGrids(3);
        UIEventListener.Get(m_sprite_Num_Input.gameObject).onClick = OnNumInputClick;
        UIEventListener.Get(m_sprite_TotalPrice_Input.gameObject).onClick = OnTotalPriceClick;
        InitText();
        SaleDataManager.ValueUpdateEvent += SaleDataManager_ValueUpdateEvent;
        ShowUIByToggle(ConsignmentItemMode.Buy);
        ResetArrow(ConsignmentItemMode.Buy);
    }
    void ResetArrow(ConsignmentItemMode type)
    {
        if (type == ConsignmentItemMode.Buy)
        {
            m_sprite_sellspr.transform.localEulerAngles = new Vector3(0, 0, 0);
            m_sprite_buyspr.transform.localEulerAngles = new Vector3(0, 0, -180);
        }
        else if (type == ConsignmentItemMode.Sell)
        {
            m_sprite_sellspr.transform.localEulerAngles = new Vector3(0, 0, -180);
            m_sprite_buyspr.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
    void InitText()
    {
        UpdateNumInputLabel();
        UpdatePriceInputLabel();
    
    }
    void SaleDataManager_ValueUpdateEvent(object sender , ValueUpdateEventArgs e)
    {
        //功能暂时注释
        //if ( e != null )
        //{
        //    if ( e.key == SaleDispatchEvents.RefreshSaleInfo.ToString() )
        //    {
        //        UpdateSaleInfo();
        //    }
        //    else if(e.key == SaleDispatchEvents.RefreshAccountInfo.ToString())
        //    {
        //        UpdateAccountInfo();
        //    }
        //}
    }

 
    public void OnDestroy()
    {
   
    }

    protected override void OnShow(object data)
    {
        base.OnShow( data );
        RegisterGlobalEvent(true);
       // InitSaleMoney(); 此次测试功能注释
        InitSaleItem();
        SendSaleInfo();
  
        UpdateAccountInfo();
        InitConsignItemOnShow();
    }
    public override bool OnTogglePanel(int tabType, int pageid)
    {
        if (tabType == 1)
        {
            ConsignmentPanelPageEnum mode = (ConsignmentPanelPageEnum)pageid;
            SetConsignmentMode(mode, m_saleUIToggle);
        }
        return base.OnTogglePanel(tabType, pageid);
    }
    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (UIMsgID.eShowUI == msgid)
        {
            ReturnBackUIMsg uimsg = (ReturnBackUIMsg)param;
            if (uimsg.tabs != null)
            {
                if (uimsg.tabs.Length > 0)
                {
                    ConsignmentPanelPageEnum mode = (ConsignmentPanelPageEnum)uimsg.tabs[0];
                    SetConsignmentMode(mode);
                }
                if (uimsg.tabs.Length > 1)
                {
                    if (!IsConsignmentItemMode((ConsignmentItemMode)uimsg.tabs[1]))
                    {
                        SetConsignmentItemMode((ConsignmentItemMode)uimsg.tabs[1]);
                    }
  
                }
            }

            if (uimsg.param != null)
            {
                string itemName = (string)uimsg.param;
                m_input_SearchInput.value = itemName;
                ReqSearchConsignment(m_input_SearchInput.value);
            }
        }
        return base.OnMsg(msgid, param);
    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }
        int firstTabData = -1;
     
        int secondTabData = -1;
        if (null != jumpData.Tabs && jumpData.Tabs.Length > 0)
        {
            firstTabData = jumpData.Tabs[0];
            if( jumpData.Tabs.Length > 1)
            {
                m_saleUIToggle = (ConsignmentItemMode)jumpData.Tabs[1];
            }           
        }
        else
        {
            firstTabData = 1;
        }
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTabData);
        if (jumpData.Param is uint && m_saleUIToggle == ConsignmentItemMode.Buy)
        {
            uint itemID = (uint)jumpData.Param;
            ConsignmentCanSellItem table = GameTableManager.Instance.GetTableItem<ConsignmentCanSellItem>(itemID);
            ItemDataBase itemTable = GameTableManager.Instance.GetTableItem<ItemDataBase>(itemID);
            if (table != null)
            {
                uint m_firstType = table.FirstType;
                uint m_secondType = table.SecondTyoe;
                if (m_firstType != 0 && m_secondType != 0)
                {

                    SetSelectFirstType(m_firstType, true);
                    SetSelectSecondType(m_secondType, true);
                    mSecondTabCreator.FocusData(mlstFirstTabIds.IndexOf(m_firstType), m_uintDic[m_firstType].IndexOf(m_secondType));
                    ReturnBackUIMsg data = new ReturnBackUIMsg();
                    if (itemTable != null)
                    {
                         data.param = itemTable.itemName;
                         DataManager.Manager<UIPanelManager>().SendMsg(PanelID.ConsignmentPanel, UIMsgID.eShowUI, data);
                    }
               
                }
                else
                {
                    Engine.Utility.Log.Error("寄售表格中的物品ID为{0}的大类ID或小类ID有误", itemID);
                }
            }

        }
        else if (jumpData.Param is uint && m_saleUIToggle == ConsignmentItemMode.Sell && canConsignItemList.Count>0)
        {
            int index = 0;
            for (int i = 0; i < canConsignItemList.Count;i++ )
            {
                if (canConsignItemList[i] == (uint)jumpData.Param)
                {
                    index = i;
                }
            }
            UIItemGrid itemGrid = m_ctor_ItemGridScrollView.GetGrid<UIItemGrid>(index);
            if (itemGrid != null)
            {
                SetSelectSellItem(itemGrid);
            }
        }
    }

    #endregion
    #region 输入区域控制
    void OnNumInputClick(GameObject go)
    {
        UIPanelManager mgr = DataManager.Manager<UIPanelManager>();
        if ( mgr.IsShowPanel( PanelID.HandInputPanel ) )
        {
            mgr.HidePanel( PanelID.HandInputPanel );
        }
        else
        {
            mgr.ShowPanel( PanelID.HandInputPanel , data: new HandInputPanel.HandInputInitData()
            {
                maxInputNum = 10000 ,
                onInputValue = OnNumConfirm,
                showLocalOffsetPosition = new Vector3( 322 , -40 , 0 ) ,
            } );
        }
    }
    void OnTotalPriceClick(GameObject go)
    {
        UIPanelManager mgr = DataManager.Manager<UIPanelManager>();
        if ( mgr.IsShowPanel( PanelID.HandInputPanel ) )
        {
            mgr.HidePanel( PanelID.HandInputPanel );
        }
        else
        {
            mgr.ShowPanel( PanelID.HandInputPanel , data: new HandInputPanel.HandInputInitData()
            {
                maxInputNum = 10000 ,
                onInputValue = OnTotalPriceCofirm,
                showLocalOffsetPosition = new Vector3( 322 , -110 , 0 ) ,
            } );
        }
    }
    uint buyFactor = 10;
    uint sellFactor = 10;
    /// <summary>
    /// 手动输入框回调
    /// </summary>
    /// <param name="num"></param>
    void OnNumConfirm(int num)
    {
        uint factor = sellFactor;
        if (m_saleUIToggle == ConsignmentItemMode.Buy)
        {
            factor = buyFactor;
        }
        int temp = (int)factor;
        num = num / temp;
        num = num * temp;
        if (m_saleUIToggle == ConsignmentItemMode.Buy)
        {
            SaleDataManager.BuyNum = (uint)num;
        }
        else if (m_saleUIToggle == ConsignmentItemMode.Sell)
        {
            SaleDataManager.SellNum = (uint)num;
        }
        UpdateNumInputLabel();
    }
    void UpdateNumInputLabel()
    {
        if (m_saleUIToggle == ConsignmentItemMode.Buy)
        {
         
            SetInputLabel(m_sprite_Num_Input.transform, SaleDataManager.BuyNum.ToString());
            UpdateBuyUnitPrice();
        }
        else if (m_saleUIToggle == ConsignmentItemMode.Sell)
        {
        
            SetInputLabel(m_sprite_Num_Input.transform, SaleDataManager.SellNum.ToString());
            UpdateSellUnitPrice();
        }
    }
    void SetInputLabel(Transform parent,string num)
    {
        Transform labelTrans = parent.transform.Find("Label");
        if(labelTrans != null)
        {
            UILabel label = labelTrans.GetComponent<UILabel>();
            if(label != null)
            {
                label.text = num;
            }
        }
    }
    void onClick_Num_less_Btn(GameObject caster)
    {
        if (m_saleUIToggle == ConsignmentItemMode.Buy)
        {
            if(SaleDataManager.BuyNum > 0)
            {
                SaleDataManager.BuyNum -= buyFactor;
            }
        
        }
        else if (m_saleUIToggle == ConsignmentItemMode.Sell)
        {
            if ( SaleDataManager.SellNum > 0 )
            {
                SaleDataManager.SellNum -= sellFactor;
            }
        }
        UpdateNumInputLabel();
    }

    void onClick_Num_add_Btn(GameObject caster)
    {
        if (m_saleUIToggle == ConsignmentItemMode.Buy)
        {
            if ( SaleDataManager.BuyNum >= 0 )
            {
                SaleDataManager.BuyNum += buyFactor;
            }

        }
        else if (m_saleUIToggle == ConsignmentItemMode.Sell)
        {
            if ( SaleDataManager.SellNum >= 0 )
            {
                SaleDataManager.SellNum += sellFactor;
            }
        }
        UpdateNumInputLabel();
    }

    void onClick_TotalPrice_less_Btn(GameObject caster)
    {
        if (m_saleUIToggle == ConsignmentItemMode.Buy)
        {
            if(SaleDataManager.BuyPrice > 0)
            {
                SaleDataManager.BuyPrice -= 1;
            }
         
        }
        else if (m_saleUIToggle == ConsignmentItemMode.Sell)
        {
            if ( SaleDataManager.SellPrice > 0 )
            {
                SaleDataManager.SellPrice -= 1;
            }
        }
        UpdatePriceInputLabel();
    }

    void onClick_TotalPrice_add_Btn(GameObject caster)
    {
        if (m_saleUIToggle == ConsignmentItemMode.Buy)
        {
            if ( SaleDataManager.BuyPrice >= 0 )
            {
                SaleDataManager.BuyPrice += 1;
            }

        }
        else if (m_saleUIToggle == ConsignmentItemMode.Sell)
        {
            if ( SaleDataManager.SellPrice >= 0 )
            {
                SaleDataManager.SellPrice += 1;
            }
        }
        UpdatePriceInputLabel();
    }
    void UpdateBuyUnitPrice()
    {
        float unitPrice = 0;
        if ( SaleDataManager.BuyNum != 0 )
        {
            unitPrice = (float)( SaleDataManager.BuyPrice * 10000.0f / SaleDataManager.BuyNum );
        }

        m_label_UnitPriceNum.text = StringUtil.GetBigMoneyStr( unitPrice );//unitPrice.ToString();
        int taxRate = GameTableManager.Instance.GetGlobalConfig<int>( "BuyGoldTax" );

        m_label_FormalitiesPriceNum.text = Mathf.CeilToInt( SaleDataManager.BuyNum * taxRate*1.0f/10000 ).ToString();
    }
    void UpdateSellUnitPrice()
    {
        float unitPrice = 0;
        if ( SaleDataManager.SellNum != 0 )
        {
            unitPrice = (float)( SaleDataManager.SellPrice * 10000.0f / SaleDataManager.SellNum );
        }
        m_label_UnitPriceNum.text = StringUtil.GetBigMoneyStr( unitPrice );
        int taxRate = GameTableManager.Instance.GetGlobalConfig<int>( "BuyTicketTax" );

        m_label_FormalitiesPriceNum.text = Mathf.CeilToInt( SaleDataManager.SellPrice * taxRate ).ToString();
    }
    void UpdatePriceInputLabel()
    {
        if (m_saleUIToggle == ConsignmentItemMode.Buy)
        {
           // m_input_TotalPrice_Input.value = SaleDataManager.BuyPrice.ToString();
            SetInputLabel(m_sprite_TotalPrice_Input.transform, SaleDataManager.BuyPrice.ToString());
            UpdateBuyUnitPrice();
        }
        else if (m_saleUIToggle == ConsignmentItemMode.Sell)
        {
          //  m_input_TotalPrice_Input.value = SaleDataManager.SellPrice.ToString();
            SetInputLabel(m_sprite_TotalPrice_Input.transform, SaleDataManager.SellPrice.ToString());
            UpdateSellUnitPrice();
        }
    }
    void OnTotalPriceCofirm(int num)
    {
        if (m_saleUIToggle == ConsignmentItemMode.Buy)
        {
            SaleDataManager.BuyPrice = (uint)num;
        }
        else if (m_saleUIToggle == ConsignmentItemMode.Sell)
        {
            SaleDataManager.SellPrice = (uint)num;
        }
        UpdatePriceInputLabel();
    }
    #endregion
    #region 金钱列表刷新
    bool m_bFirst = false;
    /// <summary>
    /// 刷新交易区信息
    /// </summary>
    void UpdateSaleInfo()
    {
        RefreshSellInfo();
        RefreshBuyInfo();
        if(!m_bFirst)
        {
            if (m_saleUIToggle == ConsignmentItemMode.Buy)
            {
                if ( m_bIsBuyTitleUP )
                {
                    ShowSellUI();
                }
            }
            m_bFirst = true;
        }
      
    }
    /// <summary>
    /// 刷新卖钱区域
    /// </summary>
    void RefreshSellInfo()
    {
        List<stSMinBMaxOrderConsignmentUserCmd_S.Data> sellList = SaleDataManager.SellList;
        if ( sellList.Count == 0 )
        {
            m_btn_TypeGrid_sell.isEnabled = false;
        }
        else
        {
            m_btn_TypeGrid_sell.isEnabled = true;
        }
        ShowFirstGridInfo( m_trans_SellListGrid_0 , sellList );
        ShowContainerGridInfo( m_trans_SellContainer , sellList );
    }
    /// <summary>
    /// 显示下面四个grid的信息
    /// </summary>
    /// <param name="containerTrans"></param>
    /// <param name="dataList"></param>
    void ShowContainerGridInfo(Transform containerTrans , List<stSMinBMaxOrderConsignmentUserCmd_S.Data> dataList)
    {
        for ( int i = 1; i < 5; i++ )
        {
            string gridName = "ListGrid_" + i;
            Transform transItem = containerTrans.Find( gridName );
            if ( transItem != null )
            {
                if ( i < dataList.Count )
                {
                    transItem.gameObject.SetActive( true );
                    SetSingleGridInfo( transItem , dataList[i] );
                }
                else
                {
                    transItem.gameObject.SetActive( false );
                }
            }
        }
    }
    /// <summary>
    /// 显示第一个grid的信息
    /// </summary>
    /// <param name="firstTrans"></param>
    /// <param name="dataList"></param>
    void ShowFirstGridInfo(Transform firstTrans , List<stSMinBMaxOrderConsignmentUserCmd_S.Data> dataList)
    {
        if ( dataList.Count > 0 )
        {
            ShowNoDataInfo( firstTrans , false );
            SetSingleGridInfo( firstTrans , dataList[0] );
        }
        else
        {
            ShowNoDataInfo( firstTrans , true );
        }
    }
    /// <summary>
    /// 刷新买钱区域
    /// </summary>
    void RefreshBuyInfo()
    {
        List<stSMinBMaxOrderConsignmentUserCmd_S.Data> buyList = SaleDataManager.BuyList;
        if ( buyList.Count == 0 )
        {
            m_btn_TypeGrid_buy.isEnabled = false;
        }
        else
        {
            m_btn_TypeGrid_buy.isEnabled = true;
        }
        
        ShowFirstGridInfo( m_trans_BuyListGrid_0 , buyList );
        ShowContainerGridInfo( m_trans_BuyContainer , buyList );
    }
    /// <summary>
    /// 显示没有数据的提示
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="bShow"></param>
    void ShowNoDataInfo(Transform trans , bool bShow)
    {
        Transform peney = trans.Find( "ListGridPenny" );
        if ( peney != null )
        {
            peney.gameObject.SetActive( !bShow );
        }
        Transform gold = trans.Find( "ListGridGold" );
        if ( gold != null )
        {
            gold.gameObject.SetActive( !bShow );
        }
        Transform label = trans.Find( "Label" );
        if ( label != null )
        {
            label.gameObject.SetActive( bShow );
        }
    }
    /// <summary>
    /// 设置单个格子的信息
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="data"></param>
    void SetSingleGridInfo(Transform trans , stSMinBMaxOrderConsignmentUserCmd_S.Data data)
    {
        UILabel pengyLabel = trans.Find( "ListGridPenny/Label" ).GetComponent<UILabel>();
        if ( pengyLabel != null )
        {
            pengyLabel.text = StringUtil.GetBigMoneyStr( data.num ); 
        }
        UILabel goldLabel = trans.Find( "ListGridGold/Label" ).GetComponent<UILabel>();
        if ( goldLabel != null )
        {
            float v = data.price;
            if(data.num != 0)
            {
                v = data.price * 1.0f / data.num;
            }
          
            goldLabel.text = StringUtil.GetBigMoneyStr( v ); 
        }
    }
    #endregion
    #region 菜单栏点击控制
    void ShowUIByToggle(ConsignmentItemMode type)
    {
        m_ctor_SellItemPriceList.SetVisible(false);
        m_saleUIToggle = type;
        if (type == ConsignmentItemMode.Buy)
        {
            m_trans_CommonContent.gameObject.SetActive( true );
            m_trans_BuyMoneyContent.gameObject.SetActive( true );
            m_trans_SellMoneyContent.gameObject.SetActive( false );
            m_trans_AccountContent.gameObject.SetActive( false );
            ResetArrow(type);
            InitText();
            if (mSecondTabCreator != null)
            {
                UICtrTypeGrid grid = mSecondTabCreator.GetGrid<UICtrTypeGrid>(0);
                if (grid != null)
                {
                    grid.SetName("我的关注");
                }
            }
          
        }
        else if (type == ConsignmentItemMode.Sell)
        {
            m_trans_CommonContent.gameObject.SetActive( true );
            m_trans_BuyMoneyContent.gameObject.SetActive( false );
            m_trans_SellMoneyContent.gameObject.SetActive( true );
            m_trans_AccountContent.gameObject.SetActive( false );
            ResetArrow(type);
            InitText();
        }
        else if (type == ConsignmentItemMode.Account)
        {
            m_trans_CommonContent.gameObject.SetActive( false );
            m_trans_BuyMoneyContent.gameObject.SetActive( false );
            m_trans_SellMoneyContent.gameObject.SetActive( false );
            m_trans_AccountContent.gameObject.SetActive( true );
        }
        else if(type == ConsignmentItemMode.ShowItem)
        {
            if (mSecondTabCreator != null)
            {
                UICtrTypeGrid grid = mSecondTabCreator.GetGrid<UICtrTypeGrid>(0);
                if (grid != null)
                {
                    grid.SetName("所有公示");
                }
            }   
        }

    }
    void SendSaleInfo()
    {
        stSMinBMaxOrderConsignmentUserCmd_C cmd = new stSMinBMaxOrderConsignmentUserCmd_C();
        NetService.Instance.Send( cmd );
        //请求寄售的收藏的物品id
        NetService.Instance.Send(new stGetStarIDListConsignmentUserCmd_C());
    }

    void OnClickItemPageTab(ConsignmentItemMode mode) 
    {
        ShowUIByToggle(mode); 
    
    }
    void onClick_BuyMoney_Btn()
    {
        ShowUIByToggle(ConsignmentItemMode.Buy);
        if(!m_bIsBuyTitleUP)
        {
            return;
        }
        ShowSellUI();

    }

    void onClick_SellMoney_Btn()
    {
        ShowUIByToggle(ConsignmentItemMode.Sell);
        if(m_bIsBuyTitleUP)
        {
            return;
        }
        ShowBuyUI();
   
    }

    void onClick_ShowItem_Btn() 
    {
        ShowUIByToggle(ConsignmentItemMode.ShowItem);
    }
    void onClick_Account_Btn()
    {
        ShowUIByToggle(ConsignmentItemMode.Account);
        ReqAccountInfo();
    }
   #endregion
    #region do animation for left ui
    float m_fAniDur = 0.15f;
    /// <summary>
    ///显示买钱区域
    /// </summary>
    bool m_bShowBuy = true;
    /// <summary>
    ///显示卖钱区域
    /// </summary>
    bool m_bShowSell = false;
    /// <summary>
    /// 买title是否在上面
    /// </summary>
    bool m_bIsBuyTitleUP = true;
    public float m_fBuyDownPosition = -152;
    public float m_fBuyUpPosition = 98.7f;
    /// <summary>
    ///  收缩买钱区域
    /// </summary>
    /// <param name="bShousuo">true是收缩 false是展开</param>
    void ShouSuoBuy(bool bShousuo)
    {
        if ( bShousuo )
        {
            m_trans_BuyContainer.transform.DOScaleY( 0 , m_fAniDur );
        }
        else
        {
            m_trans_BuyContainer.gameObject.SetActive( true );
            m_trans_BuyContainer.transform.DOScaleY( 1 , m_fAniDur );
        }

    }
    /// <summary>
    ///  收缩卖钱区域
    /// </summary>
    /// <param name="bShousuo">true是收缩 false是展开</param>
    void ShousuoSell(bool bShousuo)
    {
        if ( bShousuo )
        {
            m_trans_SellContainer.transform.DOScaleY( 0 , m_fAniDur );
        }
        else
        {
            m_trans_SellContainer.gameObject.SetActive( true );
            m_trans_SellContainer.transform.DOScaleY( 1 , m_fAniDur );
        }
    }
    void ShowBuyUI()
    {
        if ( SaleDataManager.SellList.Count > 0 )
        {
            if ( m_bIsBuyTitleUP )
            {//收缩buy
                DoBuyUpToDownAni( true );
                ShouSuoBuy( true );
                if ( SaleDataManager.SellList.Count > 0 )
                {
                    ShousuoSell( false );
                }
            }
            else
            {
                DoBuyUpToDownAni( false );
                ShouSuoBuy( false );
                if ( SaleDataManager.SellList.Count > 0 )
                {
                    ShousuoSell( true );
                }
            }
        }
    }
    void onClick_TypeGrid_sell_Btn(GameObject caster)
    {
        ShowBuyUI();
    }
    /// <summary>
    /// 移动buy trans
    /// </summary>
    /// <param name="bDown">true 向下，false向上</param>
    void DoBuyUpToDownAni(bool bDown)
    {
        if ( bDown )
        {
            if ( (int)m_btn_TypeGrid_buy.transform.localPosition.y == (int)m_fBuyUpPosition )
            {
                m_btn_TypeGrid_buy.transform.DOLocalMoveY( m_fBuyDownPosition , m_fAniDur ).OnComplete( () =>
                {
                    m_bIsBuyTitleUP = false;
                } );
            }
        }
        else
        {
            if ( (int)m_btn_TypeGrid_buy.transform.localPosition.y == (int)m_fBuyDownPosition )
            {
                m_btn_TypeGrid_buy.transform.DOLocalMoveY( m_fBuyUpPosition , m_fAniDur ).OnComplete( () =>
                {
                    m_bIsBuyTitleUP = true;
                } );
            }
        }
    }

    void ShowSellUI()
    {
        if ( SaleDataManager.SellList.Count > 0 )
        {
            if ( m_bIsBuyTitleUP )
            {//收缩sell
                DoBuyUpToDownAni( true );
                ShousuoSell( false );
                if ( SaleDataManager.BuyList.Count > 0 )
                {
                    ShouSuoBuy( true );
                }
            }
            else
            {
                DoBuyUpToDownAni( false );
                ShousuoSell( true );
                if ( SaleDataManager.BuyList.Count > 0 )
                {
                    ShouSuoBuy( false );
                }
            }
        }
        else
        {

        }
    }
    void onClick_TypeGrid_buy_Btn(GameObject caster)
    {
        ShowSellUI();
    }
    #endregion


    void onClick_Btn_Order_Btn(GameObject caster)
    {
        if (m_saleUIToggle == ConsignmentItemMode.Buy)
        {
            int wenqian = MainPlayerHelper.GetMoneyNumByType( ClientMoneyType.Gold );
            if ( wenqian < SaleDataManager.BuyPrice*10000 )
            {
                TipsManager.Instance.ShowTips(LocalTextType.Trading_Currency_jinbibuzuxiadanshibai);
                return;
            }
            stBuyMoneyConsignmentUserCmd_CS cmd = new stBuyMoneyConsignmentUserCmd_CS();
            cmd.num = SaleDataManager.BuyNum;
            cmd.price = SaleDataManager.BuyPrice * 10000;
            NetService.Instance.Send( cmd );
        }
        else if (m_saleUIToggle == ConsignmentItemMode.Sell)
        {
            int wenqian = MainPlayerHelper.GetMoneyNumByType( ClientMoneyType.Wenqian );
            if(wenqian < SaleDataManager.SellNum)
            {
                TipsManager.Instance.ShowTips(LocalTextType.Trading_Currency_wenqianbuzuxiadanshibai);            
                return;
            }
            stSellMoneyConsignmentUserCmd_CS cmd = new stSellMoneyConsignmentUserCmd_CS();
            cmd.num = SaleDataManager.SellNum;
            cmd.price = SaleDataManager.SellPrice * 10000;
            NetService.Instance.Send( cmd );
        }
    }

    #region Account UI
    void ReqAccountInfo()
    {
        stUserAccountConsignmentUserCmd_C cmd = new stUserAccountConsignmentUserCmd_C();
        NetService.Instance.Send( cmd );
    }
    void UpdateAccountInfo()
    {
        m_label_OwnGoldNum.text = StringUtil.GetBigMoneyStr( SaleDataManager.MyGold ); //SaleDataManager.MyGold.ToString();
        m_label_OwnPennyNum.text = StringUtil.GetBigMoneyStr( SaleDataManager.MyWenqian );//SaleDataManager.MyMoney.ToString();
        RefreshOtherOrderInfo();
        RefreshMyOrderInfo();
    }
    void RefreshMyOrderInfo()
    {
        for (int i = 0; i < 3; i++)
        {
            Transform item = m_trans_MySaleList.Find(i.ToString());
            if (item != null)
            {
                if (i < SaleDataManager.MySaleList.Count)
                {
                    item.gameObject.SetActive(true);
                    stUserAccountConsignmentUserCmd_S.SelfOrder orderData = SaleDataManager.MySaleList[i];
                    Transform saleType = item.Find("saletype");
                    if (saleType != null)
                    {
                        UILabel typeLabel = saleType.GetComponent<UILabel>();
                        if (typeLabel != null)
                        {
                            if (orderData.type == 1)
                            {
                                typeLabel.text = "购买";
                            }
                            else
                            {
                                typeLabel.text = "出售";
                            }
                        }
                    }

                    Transform wenqianTrans = item.Find("wenqiannum");
                    if (wenqianTrans != null)
                    {
                        UILabel num = wenqianTrans.GetComponent<UILabel>();
                        if (num != null)
                        {
                            num.text = StringUtil.GetBigMoneyStr( orderData.num );
                        }
                    }

                    Transform goldTrans = item.Find("goldnum");
                    if (goldTrans != null)
                    {
                        UILabel num = goldTrans.GetComponent<UILabel>();
                        if ( num != null )
                        {
                            float v = orderData.price;
                            if(orderData.num != 0)
                            {
                                v = orderData.price * 1.0f / orderData.num;
                            }
                            num.text = StringUtil.GetBigMoneyStr( v );
                        }
                    }
                    Transform cancelBtn = item.Find("canclebtn");
                    if (cancelBtn != null)
                    {
                        UIEventListener.Get(cancelBtn.gameObject).onClick = CancleOrderClick;
                    }
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }
    void RefreshOtherOrderInfo()
    {
        for (int i = 0; i < 4; i++)
        {
            Transform item = m_trans_LastSaleList.Find(i.ToString());
            if (item != null)
            {
                if ( i < SaleDataManager.RecentSaleList.Count )
                {
                    item.gameObject.SetActive(true);
                    stSuccessBuyLogConsignmentUserCmd_S.Data orderData = SaleDataManager.RecentSaleList[i];
                    Transform saleType = item.Find("saletype");
                    if (saleType != null)
                    {
                        UILabel typeLabel = saleType.GetComponent<UILabel>();
                        if (typeLabel != null)
                        {
                            typeLabel.text = "购买";
                        }
                    }

                    Transform wenqianTrans = item.Find("wenqiannum");
                    if (wenqianTrans != null)
                    {
                        UILabel num = wenqianTrans.GetComponent<UILabel>();
                        if ( num != null )
                        {
                            num.text = StringUtil.GetBigMoneyStr( orderData.num );
                        }
                    }

                    Transform goldTrans = item.Find("goldnum");
                    if (goldTrans != null)
                    {
                        UILabel num = goldTrans.GetComponent<UILabel>();
                        if ( num != null )
                        {
                            float v = orderData.price;
                            if ( orderData.num != 0 )
                            {
                                v = orderData.price * 1.0f / orderData.num;
                            }
                            num.text = StringUtil.GetBigMoneyStr( v );
                        }
                     
                    }
                    Transform saleTime = item.Find("saletime");
                    if (saleTime != null)
                    {
                        UILabel timelabel = saleTime.GetComponent<UILabel>();
                        if (timelabel != null)
                        {
                            timelabel.text = StringUtil.GetStringSince1970(orderData.time);
                        }
                    }
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }
    void CancleOrderClick(GameObject go)
    {
        string name = go.transform.parent.name;
        uint index = 0;
        if(uint.TryParse(name,out index))
        {
            if(index < SaleDataManager.MySaleList.Count)
            {
                stUserAccountConsignmentUserCmd_S.SelfOrder data = SaleDataManager.MySaleList[(int)index];
                stCancelOrderConsignmentUserCmd_CS cmd = new stCancelOrderConsignmentUserCmd_CS();
                cmd.index = data.index;
                NetService.Instance.Send( cmd );
            }
          
        }
    }
    #endregion

    void onClick_Btn_AllTakeOut_Btn(GameObject caster)
    {
        stExtractMoneyConsignmentUserCmd_CS cmd = new stExtractMoneyConsignmentUserCmd_CS();
        NetService.Instance.Send( cmd );
    }

    void onClick_Btn_TransactionRecord_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel( PanelID.TransactionRecordPanel );
        stDealRecordConsignmentUserCmd_C cmd = new stDealRecordConsignmentUserCmd_C();
        NetService.Instance.Send( cmd );
      
    }
    #region Control面板控制
    private ConsignmentPanelPageEnum m_em_mode = ConsignmentPanelPageEnum.Max;


    /// <summary>
    /// 是否当前模式为mode
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    private bool IsConsignmentMode(ConsignmentPanelPageEnum mode)
    {
        return (m_em_mode == mode);
    }

    /// <summary>
    /// 设置面板模式
    /// </summary>
    /// <param name="mode"></param>
    private void SetConsignmentMode(ConsignmentPanelPageEnum mode, ConsignmentItemMode topTab = ConsignmentItemMode.Buy)
    {
//         if (m_em_mode == mode && m_saleUIToggle == topTab)
//         {
//             return;
//         }   
        m_em_mode = mode;
        bool value = mode == ConsignmentPanelPageEnum.Page_Coin;
        m_trans_CurrencyContent.gameObject.SetActive(value);
        m_trans_ItemContent.gameObject.SetActive(!value);
        UITabGrid tab = null;
        int topTabIndex = (int)topTab-1;
        if (value)
        {
            tab = tabCreator.GetGrid<UITabGrid>(topTabIndex);
         
        }
        else 
        {
            tab = m_ctor_Toggles.GetGrid<UITabGrid>(topTabIndex);
        }
        if(tab != null)
        {
            tab.gameObject.SendMessage("OnClick", tab.gameObject, UnityEngine.SendMessageOptions.RequireReceiver);
        }
    }
    #endregion

    #region UIDlg事件委托
    /// <summary>
    /// UI事件委托
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnUIGridEventDlg(UIEventType eventType,object data,object param)
    {
        if (eventType == UIEventType.Click)
        {
            if (null != data)
            {
                if (data is UITabGrid)
                {
                     UITabGrid tab = data as UITabGrid;
                     if (tab.Data is ConsignmentItemMode)
                     {
                         SetLeftTopTabModel(tab);
                     }
                }
            }
        }
    }
    void SetLeftTopTabModel(UITabGrid tab) 
    {
        ConsignmentItemMode type = (ConsignmentItemMode)tab.Data;
        if (m_em_mode == ConsignmentPanelPageEnum.Page_Item)
        {          
            SetConsignmentItemMode(type);
            OnClickItemPageTab(type);
        }
        else if (m_em_mode == ConsignmentPanelPageEnum.Page_Coin)
        {
            if (type == ConsignmentItemMode.Buy)
            {
                onClick_BuyMoney_Btn();
            }
            else if (type == ConsignmentItemMode.Sell)
            {
                onClick_SellMoney_Btn();
            }
            else if(type == ConsignmentItemMode.ShowItem)
            {
                onClick_ShowItem_Btn();
            }
            else
            {
                onClick_Account_Btn();
            }
        }
        if (previous == null)
        {
            previous = tab;
        }
        else
        {
            previous.SetHightLight(false);
        }
        if(tab != null)
        {
           tab.SetHightLight(true);
           previous = tab;
        }
     
    }

    /// <summary>
    /// 单元格刷新委托
    /// </summary>
    /// <param name="gridBase"></param>
    /// <param name="index"></param>
    private void OnUIGridUpdate(UIGridBase grid,int index)
    {
        if(grid is UITabGrid)
        {
            UITabGrid tab = grid as UITabGrid;
            tab.SetGridData((ConsignmentItemMode)(index+1));          
            tab.SetName(tabNames[index]);
            if(previous == null && index == 0)
            {
                tab.SetHightLight(true);
                previous = tab;
            }
        }
    }

    List<string> tabNames = new List<string>()
    {
        "购买","出售","公示","账户"
    }; 
    #endregion
    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Tabs = new int[2];
        pd.JumpData.Tabs[0] = (int)m_uint_activeFType;
        pd.JumpData.Tabs[1] = (int)m_uint_activeStype;
        return pd;
    }

}

