/*
using UnityEngine;
using Client;
using table;
using GameCmd;
using Engine.Utility;
using System.Collections.Generic;
partial class PetPanel
{
    //PetDataManager petDataManager;
    //IPet curPet;
    //public IPet CurPet
    //{
    //    get
    //    {
    //        return petDataManager.CurPet;
    //    }
    //}

    //public uint PetBaseID
    //{
    //    get
    //    {
    //        if ( CurPet != null )
    //        {
    //            return CurPet.PetBaseID;
    //        }
    //        Log.Error( "get petbaseid error" );
    //        return 0;
    //    }
    //}
    uint m_nGaojiItemID = 0;
    //protected override void OnInitPanelData()
    //{
    //    base.OnInitPanelData();
    //    panelBaseData.m_str_name = "高级归元";
    //    panelBaseData.m_int_topBarUIMask = (int)UIDefine.UITopBarUIMode.CurrencyBar;
    //    panelBaseData.m_bool_needCMBg = true;
    //    panelBaseData.m_em_colliderMode = UIDefine.UIPanelColliderMode.TransBg;
    //    panelBaseData.m_em_showMode = UIDefine.UIPanelShowMode.HideNormal;
    //}

    //public override void OnLoading()
    //{
    //    base.OnLoading();
    //    petDataManager = DataManager.Manager<PetDataManager>();
    //    petDataManager.ValueUpdateEvent += OnValueUpdateEventArgs;
    //    InitData();
    //}
    //protected override void OnPanelBaseDestory()
    //{
    //    base.OnPanelBaseDestory();
    //    petDataManager.ValueUpdateEvent -= OnValueUpdateEventArgs;
    //}
    //void OnValueUpdateEventArgs(object obj , ValueUpdateEventArgs v)
    //{
    //    InitData();
    //}
    void InitGaojiGuiyuanData()
    {
        if ( CurPet == null )
        {
            Log.Error( " curpet is null" );
            return;
        }
        PetDataBase pdb = GameTableManager.Instance.GetTableItem<PetDataBase>( CurPet.PetBaseID );
        if ( pdb == null )
        {
            Log.Error( " petdata base is null baseid is " + CurPet.PetBaseID.ToString() );
            return;
        }
        int status = CurPet.GetProp( (int)PetProp.PetGuiYuanStatus );
        PetGuiYuanDataBase guiyuandb = GameTableManager.Instance.GetTableItem<PetGuiYuanDataBase>( pdb.petQuality , status );
        if ( guiyuandb != null )
        {
            m_label_guiyuanCommon_xiaohaogold.text = guiyuandb.consumeMoney.ToString();
        }
        List<uint> itemList = GameTableManager.Instance.GetGlobalConfigList<uint>( "GYItem" , "1" );

     
        if ( itemList.Count != 4 )
        {
            Log.Error( "全局配置表GYItem 丹药数量不够" );
            return;
        }
        uint itemID = 0;
        uint qua = pdb.petQuality;
        if ( qua == 8 )
        {
            itemID = itemList[3];
        }
        else
        {
            int level = (int)pdb.carryLevel;
            int firstLevel = GameTableManager.Instance.GetGlobalConfig<int>( "PetFirstCarryLevel" );
            int secondLevel = GameTableManager.Instance.GetGlobalConfig<int>( "PetSecondCarryLevel" );
            if ( level < firstLevel )
            {
                itemID = itemList[0];
            }
            else if ( level > secondLevel )
            {
                itemID = itemList[2];
            }
            else
            {
                itemID = itemList[1];
            }
        }
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId( itemID );
        m_nGaojiItemID = itemID;
        UIItem.AttachParent( m_sprite_guiyuanCommon_icon.transform , itemID , (uint)itemCount );
        ShowGaoJiGuiyuanColdLabel(itemID);
        ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>( itemID );
        if ( db != null )
        {
            m_label_guiyuanCommon_name.text = db.itemName;
        }

        m_label_guiyuanCommon_number.text = StringUtil.GetNumNeedString( itemCount , 1 );
    }
    //protected override void OnShow(object data)
    //{
    //    base.OnShow( data );
    //    if ( m_widget_PetMessage.gameObject.GetComponent<PetMessage>() == null )
    //    {
    //        m_widget_PetMessage.gameObject.AddComponent<PetMessage>();
    //    }
    //    if ( m_widget_SliderContaier.gameObject.GetComponent<PetSliderGroup>() == null )
    //    {
    //        m_widget_SliderContaier.gameObject.AddComponent<PetSliderGroup>();
    //    }
    //    if ( m_widget_GuiyuanQianSliderContaier.GetComponent<PetSliderGroup>() == null )
    //    {
    //        m_widget_GuiyuanQianSliderContaier.gameObject.AddComponent<PetSliderGroup>();
    //    }
    //    if ( m_widget_GuiyuanHouSliderContaier.GetComponent<PetGuiYuanHouSliderGroup>() == null )
    //    {
    //        m_widget_GuiyuanHouSliderContaier.gameObject.AddComponent<PetGuiYuanHouSliderGroup>();
    //    }
    //    InitLableData();
    //    m_widget_guiyuanqian.gameObject.SetActive( true );
    //    m_widget_guiyuanhou.gameObject.SetActive( false );
    //    InitData();
    //}
    //public void InitLableData()
    //{
    //    m_label_gjguiyuan_growstate.text = petDataManager.GetCurPetGrowStatus();
    //    m_label_gjguiyuanyuan_growstate.text = petDataManager.GetCurPetGrowStatus();
    //    m_label_gjguiyuanxin_growstate.text = petDataManager.GetCurPetGrowStatus();

    //}
    bool bAutoBuy = false;
    void onClick_Gjguiyuan_zidongbuzu_Btn(GameObject caster)
    {
        bAutoBuy = !bAutoBuy;
        Transform spr = caster.transform.Find( "Sprite" );
        if ( spr != null )
        {
            spr.gameObject.SetActive( bAutoBuy );
        }
        ShowGaoJiGuiyuanColdLabel(m_nGaojiItemID);
    }
    void ShowColdByAutoBuy(bool bAuto)
    {

    }
    void onClick_Gjguiyuan_leftturn_Btn(GameObject caster)
    {

    }

    void onClick_Gjguiyuan_rightturn_Btn(GameObject caster)
    {

    }

    void onClick_Ptguiyuan_buzu_Btn(GameObject caster)
    {

    }

    void onClick_Baocuntianfu_Btn(GameObject caster)
    {
        if ( CurPet == null )
        {
            Log.Error( " curpet is null" );
            return;
        }

        m_trans_GjguiyuanContent.gameObject.SetActive(true);
       // m_widget_guiyuanhou.gameObject.SetActive( false );
        if ( CurPet != null )
        {
            stReplaceTalentPetUserCmd_C cmd = new stReplaceTalentPetUserCmd_C();
            cmd.id = CurPet.GetID();
            NetService.Instance.Send( cmd );
        }

    }
    bool ShowGaoJiGuiyuanColdLabel(uint itemID)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId( itemID );
        if ( itemCount <= 0 )
        {
            if ( bAutoBuy )
            {
                string needStr = "";
                if ( petDataManager.GetItemNeedColdString( itemID , ref needStr ) )
                {
                    m_label_guiyuanCommon_number.gameObject.SetActive( false );
                    m_label_guiyuanCommon_dianjuanxiaohao.gameObject.SetActive( true );
                    m_label_guiyuanCommon_dianjuanxiaohao.text = needStr;
                    return true;
                }
                else
                {
                    m_label_guiyuanCommon_number.gameObject.SetActive(false);
                    m_label_guiyuanCommon_dianjuanxiaohao.gameObject.SetActive(true);
                    m_label_guiyuanCommon_dianjuanxiaohao.text = needStr;
                    return false;
                }
            }
            else
            {
                m_label_guiyuanCommon_number.gameObject.SetActive(true);
                m_label_guiyuanCommon_dianjuanxiaohao.gameObject.SetActive(false);
            }

        }
        else
        {
            m_label_guiyuanCommon_number.gameObject.SetActive(true);
            m_label_guiyuanCommon_dianjuanxiaohao.gameObject.SetActive(false);
        }
        return true;
    }
    bool CheckGaojiItem()
    {
        int count = DataManager.Manager<ItemManager>().GetItemNumByBaseId( m_nGaojiItemID );
        if ( count <= 0 )
        {
            if ( bAutoBuy )
            {

                if ( !ShowColdLabel( m_nGaojiItemID  ) )
                {
                    TipsManager.Instance.ShowTipsById( 5 );
                    return false;
                }
            }
            else
            {
                m_label_guiyuanCommon_number.gameObject.SetActive(true);
                m_label_guiyuanCommon_dianjuanxiaohao.gameObject.SetActive(false);
                TipsManager.Instance.ShowTipsById( 6 );
                return false;
            }

        }
        return true;
    }
    void onClick_Gaojiguiyuan_gaojiguiyuan_Btn(GameObject caster)
    {
        if ( CurPet != null )
        {
            if (!CheckGaojiItem())
                return;
            stGuiYuanPetUserCmd_CS cmd = new stGuiYuanPetUserCmd_CS();
            cmd.id = CurPet.GetID();
            cmd.adv = true;
            cmd.auto_buy = bAutoBuy;
            NetService.Instance.Send( cmd );

            m_trans_GjguiyuanContent.gameObject.SetActive(false);
           // m_widget_guiyuanhou.gameObject.SetActive( true );
        }
    }


}
*/