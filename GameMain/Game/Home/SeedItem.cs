using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using GameCmd;
using table;
using Engine;
using Engine.Utility;

partial class SeedItem : UIGridBase
{
    SeedAndCubDataBase m_seedDataBase;
    ItemDataBase m_itemDataBase;
    HomeDataManager homeDM
    {
        get
        {
            return DataManager.Manager<HomeDataManager>();
        }
    }

    protected override void OnStart()
    {
        base.OnStart();
        UIEventListener.Get( this.gameObject ).onClick = OnClickSeedItem;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    public override void Init()
    {
        base.Init();
    }
    public void OnClickSeedItem(GameObject go)
    {
        if(homeDM.bAutoPlant)
        {
            for(int i = 1;i <= homeDM.LandUnlockNum;i++)
            {
                foreach (var land in homeDM.EntityStateDic)
                {
                    if (land.Value.type == EntityType.EntityType_Soil)
                    {
                        if (land.Value.index == i)
                        {
                            if (land.Value.state == (int)HomeDataManager.LandState.Idle)
                            {
                                SendSeedMessage(i);
                            } 
                        }
                    }
                }
            }
        }
        else
        {
            SendSeedMessage( homeDM.SelectLandIndex );
        }
    }
    void SendSeedMessage(int landIndex)
    {
        if ( m_itemDataBase == null )
        {
            Log.Error( "itmedatabase is null" );
            return;
        }
        if ( m_seedDataBase == null )
        {
            Log.Error( "seeddatabase is null" );
            return;
        }
        int level = MainPlayerHelper.GetPlayerLevel();
        bool bshow = level > m_itemDataBase.useLevel ? true : false;
        if ( !bshow )
        {
            TipsManager.Instance.ShowTipsById( 114506 );
            return;
        }
        //if (homeDM.HasEnoughItem(m_itemDataBase.itemID, 1))
        if ( homeDM.HasEnoughSeedAndCub( m_itemDataBase.itemID , 1 ) )
        {
            if ( m_seedDataBase.type == 0 )
            {
                stSowHomeUserCmd_CS cmd = new stSowHomeUserCmd_CS();
                cmd.seed_id = m_itemDataBase.itemID;
                cmd.land_id = (uint)landIndex;
                NetService.Instance.Send( cmd );
            }

            if ( m_seedDataBase.type == 1 )
            {
                stFeedHomeUserCmd_CS cmd = new stFeedHomeUserCmd_CS();
                cmd.seed_id = m_itemDataBase.itemID;
                cmd.land_id = (uint)landIndex;
                NetService.Instance.Send( cmd );
            }


        }
    }
    public void InitSeedItem(SeedAndCubDataBase sdb)
    {
        if ( sdb != null )
        {
            m_seedDataBase = sdb;
            UIItem.AttachParent( m_sprite_icon.transform , sdb.itemID );
            ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>( sdb.itemID );
            if ( db != null )
            {
                m_itemDataBase = db;
                m_label_seedname.text = db.itemName;
                float hour = sdb.growTime * 1.0f / 3600;
                m_label_ripetime.text = hour.ToString();
                int level = MainPlayerHelper.GetPlayerLevel();
                bool bshow = level < db.useLevel ? true : false;
                ShowWaring( bshow );
            }

        }
    }
    void ShowWaring(bool bShow)
    {
        m_label_lockwarring.gameObject.SetActive( bShow );
        m_label_ripetime.gameObject.SetActive( !bShow );
    }
}
