using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using GameCmd;
using table;
using Engine;
using Engine.Utility;


partial class MiningPanel: UIPanelBase
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
    int factor = 1;
    int Factor
    {
        set
        {
            factor = value;
            RefreshUI();

        }
        get
        {
            return factor;
        }
    }
    protected override void OnShow(object data)
    {
        base.OnShow( data );
        RefreshUI();
    }
    protected override void OnLoading()
    {
        base.OnLoading();
        foreach(var go in m_label_multiple.transform.GetComponentsInChildren<UIButton>())
        {
            UIEventListener.Get( go.gameObject ).onClick = OnFactorClick;
        }
    }
    private CMResAsynSeedData<CMTexture> m_CASD = null;
    void RefreshUI()
    {
        uint mineStoneID = homeDM.StoneID;
        ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>( mineStoneID );
        if(db != null)
        {
            m_label_Mine_Name.text = db.itemName;
            m_label_all_income.text =( homeDM.GainStoneNum * factor).ToString();
            MineDataBase mdb = GameTableManager.Instance.GetTableItem<MineDataBase>( mineStoneID );
            if(mdb != null)
            {
                uint gainTime = mdb.mineGainTime*(uint)factor;
             
                m_label_reward_time.text = StringUtil.GetStringBySeconds( gainTime );
                ItemDataBase compassData = GameTableManager.Instance.GetTableItem<ItemDataBase>( homeDM.ComPassID );
                if(compassData != null)
                {
                    m_label_name.text = compassData.itemName;
                    string spriteName = string.IsNullOrEmpty(compassData.itemIcon) ? ItemDefine.ICON_NULL : compassData.itemIcon;
                    UIManager.GetTextureAsyn(spriteName, ref m_CASD, () =>
                    {
                        if (null != m__icon)
                        {
                            m__icon.mainTexture = null;
                        }
                    }, m__icon);
                    int count = DataManager.Manager<ItemManager>().GetItemNumByBaseId( homeDM.ComPassID );
                    int needLuoPan = GameTableManager.Instance.GetGlobalConfig<int>( "MineDig" , factor.ToString() );
                    m_label_num.text = StringUtil.GetNumNeedString( count , needLuoPan );
                }
        

            }
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        if (null != m_CASD)
        {
            m_CASD.Release(true);
            m_CASD = null;
        }
    }
    void OnFactorClick(GameObject go)
    {
        string name = go.name;
        int fac = 1;
        if(int.TryParse(name ,out fac))
        {
           
           Factor = fac;
        }
    }
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Btn_quxiao_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Btn_queding_Btn(GameObject caster)
    {
        if(homeDM.StoneID == 0)
        {
            TipsManager.Instance.ShowTipsById( 114518 );
            return;
        }
        int needLuoPan = GameTableManager.Instance.GetGlobalConfig<int>( "MineDig" , factor.ToString() );
        if ( homeDM.HasEnoughItem( homeDM.ComPassID , needLuoPan ) )
        {
            TipsManager.Instance.ShowTipsById( 114517 );
            stDigMineHomeUserCmd_C cmd = new stDigMineHomeUserCmd_C();
            cmd.is_vip = homeDM.IsMineVIP;
            cmd.dig_num = factor;
            cmd.auto_buy = false;
            cmd.item = (int)homeDM.ComPassID;
            NetService.Instance.Send( cmd );
            HideSelf();
        }
       
    }


}
