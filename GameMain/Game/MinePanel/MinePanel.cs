using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using GameCmd;
using table;
using Engine;
using Engine.Utility;


partial class MinePanel : UIPanelBase
{

    HomeDataManager homeDM
    {
        get
        {
            return DataManager.Manager<HomeDataManager>();
        }
    }
    MineData mineData
    {
        get
        {
            return homeDM.GetMineData();
        }
    }
    bool autoBuy = false;
    bool bAutoConsume
    {
        get
        {
            return autoBuy;
        }
        set
        {
            autoBuy = value;
            Transform trans = m_btn_zidongbuzu.transform.Find( "Sprite" );
            if ( trans != null )
            {
                trans.gameObject.SetActive( autoBuy );
            }
        }
    }


    /// <summary>
    /// 探矿道具list
    /// </summary>
    List<CompassDataBase> compassList;
    uint m_nItemID = 0;
    protected override void OnLoading()
    {
        base.OnLoading();
        compassList = GameTableManager.Instance.GetTableList<CompassDataBase>();
        homeDM.ValueUpdateEvent += OnValueUpdateEventArgs;
        if ( m_nItemID == 0 )
        {
            SetSelectItem( 1 );
        }
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        homeDM.ValueUpdateEvent -= OnValueUpdateEventArgs;
    }

    void OnValueUpdateEventArgs(object obj , ValueUpdateEventArgs v)
    {
        if ( v.key == HomeDispatchEvent.ChangeMineState.ToString() )
        {
            RefreshMineState();
            if ( homeDM.MineState == HomeMineState.CanFind )
            {
                ShowStoneUIByData( homeDM.StoneID , homeDM.ComPassID , homeDM.GainStoneNum );
            }
        }
    }
    protected override void OnShow(object data)
    {

        base.OnShow( data );
        RefreshUI();
        //ShowStoneInfo( true );
    }
    void RefreshMineState()
    {
        m_label_status.text = homeDM.GetMineStateStr();
    }
    public void RefreshUI()
    {
        string itemName = "Item_";

        #region leftui
        for ( int i = 1; i < 4; i++ )
        {
            Transform item = m_widget_ItemContent.transform.Find( itemName + i.ToString() );
            if ( item != null )
            {
                Transform bg = item.Find("bg_" + i.ToString());
                UIEventListener.Get(bg.gameObject).onClick = OnSelectItem;
                CompassDataBase db = compassList[i - 1];
                if ( db != null )
                {
                    uint itemID = db.dwID;
                    ItemDataBase idb = GameTableManager.Instance.GetTableItem<ItemDataBase>( itemID );
                    if ( idb != null )
                    {
                        int count = DataManager.Manager<ItemManager>().GetItemNumByBaseId( itemID );
                        Transform icon = item.Find( "icon" );
                        if ( icon != null )
                        {
                            UIItem.AttachParent( icon , itemID , (uint)count );

                        }
                        Transform name = item.Find( "name" );
                        if ( name != null )
                        {
                            UILabel label = name.GetComponent<UILabel>();
                            if ( label != null )
                            {
                                label.text = idb.itemName;
                            }
                        }
                        Transform des = item.Find( "description" );
                        if ( des != null )
                        {
                            UILabel label = des.GetComponent<UILabel>();
                            if ( label != null )
                            {
                                label.text = idb.description;
                            }
                        }
                    }
                }
            }
        }
        #endregion


        ShowStoneUIByData( homeDM.StoneID , homeDM.ComPassID , homeDM.GainStoneNum );
        ShowBtn();
        RefreshMineState();
    }
    /// <summary>
    /// 显示探矿的ui
    /// </summary>
    /// <param name="mine_base_id">矿石baseid</param>
    /// <param name="item">罗盘id</param>
    /// <param name="mine_num">探矿数量</param>
    void ShowStoneUIByData(uint mine_base_id , uint item , uint mine_num)
    {
        if ( mine_num == 0 )
        {
            ShowStoneInfo( false );
            return;
        }
   
        ShowStoneInfo( true );
        ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>( (uint)mine_base_id );
        if ( db != null )
        {
            m_sprite_icon_find.spriteName = db.itemIcon;
            m_label_stonename.text = db.itemName;
            m_label_grade.text = db.grade.ToString() + "档";
            uint maxNum = 0;
            CompassDataBase cdb = GameTableManager.Instance.GetTableItem<CompassDataBase>( (uint)item );
            if ( cdb != null )
            {
                if ( cdb.mineID1 == (uint)mine_base_id )
                {
                    maxNum = cdb.maxNum1;
                }
                else if ( cdb.mineID2 == (uint)mine_base_id )
                {
                    maxNum = cdb.maxNum2;
                }
                else if ( cdb.mineID3 == (uint)mine_base_id )
                {
                    maxNum = cdb.maxNum3;
                }
            }
            m_slider_find_num.value = mine_num * 1.0f / maxNum;
            Transform sliderNum = m_slider_find_num.transform.Find("Percent");
            if ( sliderNum != null )
            {
                UILabel label = sliderNum.GetComponent<UILabel>();
                if ( label != null )
                {
                    label.text = mine_num + "/" + maxNum;
                }
            }

        }
        if ( homeDM.DigBeginTime == 0 )
        {
            ShowGainInfo( false );
        }
        else
        {
            ShowGainInfo( true );

        }
    }
    public void RefreshStone(stFindMineHomeUserCmd_CS cmd)
    {
        RefreshUI();
    }
    void OnSelectItem(GameObject go)
    {
        string name = go.name;
        char indexStr = name[name.Length - 1];
        int index = -1;
        if ( int.TryParse( indexStr.ToString() , out index ) )
        {
            SetSelectItem( index );
        }
    }
    void SetSelectItem(int index)
    {
        if ( index - 1 < compassList.Count )
        {
            CompassDataBase db = compassList[index - 1];
            if ( db != null )
            {
                m_nItemID = db.dwID;
            }
        }
        int count = DataManager.Manager<ItemManager>().GetItemNumByBaseId( m_nItemID );
        if ( count < 1 && bAutoConsume )
        {
            m_label_dianjuanxiaohao.gameObject.SetActive( true );
            Transform num = m_label_dianjuanxiaohao.transform.Find( "Label" );
            if ( num != null )
            {
                UILabel numLabel = num.GetComponent<UILabel>();
                if ( numLabel != null )
                {
                    PointConsumeDataBase db = GameTableManager.Instance.GetTableItem<PointConsumeDataBase>( m_nItemID );
                    if ( db != null )
                    {
                        numLabel.text = db.buyPrice.ToString();

                    }
                }
            }
        }
    }

    void SendFindMine()
    {

        stFindMineHomeUserCmd_CS cmd = new stFindMineHomeUserCmd_CS();
        cmd.is_vip = homeDM.IsMineVIP;
        cmd.item = (int)m_nItemID;
        cmd.auto_buy = bAutoConsume;
        NetService.Instance.Send( cmd );

    }
    void ShowGainInfo(bool bShow)
    {
        m_label_all_income.gameObject.SetActive( bShow );
        m_label_robbed_income.gameObject.SetActive( bShow );
        m_label_reward_time.gameObject.SetActive( bShow );
        if ( bShow )
        {
            m_label_all_income.text = homeDM.GainStoneNum.ToString();
            m_label_robbed_income.text = homeDM.RobbedNum.ToString();

        }
    }
    void Update()
    {
        if ( m_label_reward_time.gameObject.activeSelf )
        {
            string facStr = string.Format( "({0}倍开采）" , homeDM.DigFactor );
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            if ( homeDM.MineLeftTime == 0 )
            {
                str.Append( "开采完毕" );
            }
            else
            {
                str.Append( StringUtil.GetStringBySeconds( homeDM.MineLeftTime ) );

            }
            str.Append( facStr );
            m_label_reward_time.text = str.ToString();
        }

    }
    void ShowBtn()
    {
        if ( homeDM.MineState == HomeMineState.CanFind )
        {
            m_widget_status_NoMining.gameObject.SetActive( true );
            m_widget_status_Mining.gameObject.SetActive( false );
            m_widget_status_MiningEnd.gameObject.SetActive( false );
        }
        else if ( homeDM.MineState == HomeMineState.Mining )
        {
            m_widget_status_NoMining.gameObject.SetActive( false );
            m_widget_status_Mining.gameObject.SetActive( true );
            m_widget_status_MiningEnd.gameObject.SetActive( false );
        }
        else if ( homeDM.MineState == HomeMineState.CanGain )
        {
            m_widget_status_NoMining.gameObject.SetActive( false );
            m_widget_status_Mining.gameObject.SetActive( false );
            m_widget_status_MiningEnd.gameObject.SetActive( true );
        }
    }
    void ShowStoneInfo(bool bShow)
    {
        m_widget_Mine.gameObject.SetActive( bShow );
        m_widget_NoMine.gameObject.SetActive( !bShow );
    }
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Normal_Btn(GameObject caster)
    {
        homeDM.IsMineVIP = false;
    }

    void onClick_High_Btn(GameObject caster)
    {
        if(!homeDM.MineVIPIsOpen)
        {
            int needDianjuan = GameTableManager.Instance.GetGlobalConfig<int>( "UnLockMineCostCoin" );
            if ( homeDM.HasEnoughDianJuan( needDianjuan ) )
            {
                string tips = DataManager.Manager<TextManager>().GetLocalFormatText(114530, needDianjuan);
                TipsManager.Instance.ShowTipWindow(TipWindowType.CancelOk, tips, () =>
                {
                    stUnlockMineHomeUserCmd_CS cmd = new stUnlockMineHomeUserCmd_CS();
                    NetService.Instance.Send(cmd);

                });
            }
        }
        else
        {
            homeDM.IsMineVIP = true;
        }
        
    }

    void onClick_Zidongbuzu_Btn(GameObject caster)
    {
        bAutoConsume = !bAutoConsume;
    }

    void onClick_Btn_battlelist_Btn(GameObject caster)
    {

    }

    void onClick_Mining_Btn(GameObject caster)
    {

    }

    void onClick_GrabMine_Btn(GameObject caster)
    {

    }
    void onClick_Btn_NoMine_change_Btn(GameObject caster)
    {
        SendFindMine();
    }

    void onClick_Btn_mining_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel( PanelID.MiningPanel );
    }

    void onClick_Btn_change_Btn(GameObject caster)
    {

        SendFindMine();
    }
    void onClick_Btn_nowgain_Btn(GameObject caster)
    {
        MineDataBase db = GameTableManager.Instance.GetTableItem<MineDataBase>( homeDM.StoneID );
        if ( db != null )
        {
            int count = (int)Math.Ceiling( (double)homeDM.MineLeftTime / db.immediatelyGainTime );
            int gainTime = (int)homeDM.MineGainTime;
            if(homeDM.IsMineVIP)
            {
                gainTime = (int)homeDM.VipGainTime;
            }
            IncreaseDataBase idb = GameTableManager.Instance.GetTableItem<IncreaseDataBase>( 3 , gainTime );
            if ( idb != null )
            {
                count = (int)Math.Ceiling( count * idb.increase );
            }
            string tips = DataManager.Manager<TextManager>().GetLocalFormatText(114531, count);
            TipsManager.Instance.ShowTipWindow(TipWindowType.CancelOk, tips, () =>
            {
                if (homeDM.HasEnoughDianJuan(count))
                {
                    stImmediMineHomeUserCmd_CS cmd = new stImmediMineHomeUserCmd_CS();
                    cmd.is_vip = homeDM.IsMineVIP;
                    NetService.Instance.Send(cmd);
                }
                else
                {
                    Log.Error("元宝不足");
                }
            });

        }

    }

    void onClick_Btn_gain_Btn(GameObject caster)
    {
        stGainMineHomeUserCmd_CS cmd = new stGainMineHomeUserCmd_CS();
        cmd.is_vip = homeDM.IsMineVIP;
        NetService.Instance.Send( cmd );
    }

}
