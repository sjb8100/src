//*************************************************************************
//	创建日期:	2016/11/7 10:44:16
//	文件名称:	TransactionRecordPanel
//   创 建 人:   zhudianyu	
//	版权所有:	中青宝
//	说    明:	交易记录
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


partial class TransactionRecordPanel
{
    SaleMoneyDataManager SaleDataManager
    {
        get
        {
            return DataManager.Manager<SaleMoneyDataManager>();
        }
    }
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
        //panelBaseData.m_str_name = "交易记录";
        //panelBaseData.m_int_topBarUIMask = (int)UIDefine.UITopBarUIMode.CurrencyBar; ;
        //panelBaseData.m_bool_needCMBg = true;
        //panelBaseData.m_em_colliderMode = UIDefine.UIPanelColliderMode.TransBg;
        //panelBaseData.m_em_showMode = UIDefine.UIPanelShowMode.HideNormal;
    }
    protected override void OnLoading()
    {
        base.OnLoading();
        SaleDataManager.ValueUpdateEvent += SaleDataManager_ValueUpdateEvent;
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        SaleDataManager.ValueUpdateEvent -= SaleDataManager_ValueUpdateEvent;
    }
   
    void SaleDataManager_ValueUpdateEvent(object sender , ValueUpdateEventArgs e)
    {
        if(e != null)
        {
            if(e.key == SaleDispatchEvents.RefreshRecordInfo.ToString())
            {
                UpdateRecordInfo();
            }
        }
    }
    void UpdateRecordInfo()
    {
        for(int i = 0;i<10;i++)
        {
            Transform recordItem = m_trans_Account.Find( i.ToString() );
            if(recordItem != null)
            {
                if ( i < SaleDataManager.AccountRecordList.Count )
                {
                    recordItem.gameObject.SetActive( true );
                    stDealRecordConsignmentUserCmd_S.Data dt = SaleDataManager.AccountRecordList[i];
                    Transform numTrans = recordItem.Find( "Num/Label" );
                    if(numTrans != null)
                    {
                        UILabel label = numTrans.GetComponent<UILabel>();
                        if(label != null)
                        {
                            label.text = dt.num.ToString();
                        }
                    }
                    Transform priceTrans = recordItem.Find( "UnitPrice/Label" );
                    if ( priceTrans != null )
                    {
                        UILabel label = priceTrans.GetComponent<UILabel>();
                        if ( label != null )
                        {
                            label.text = dt.price.ToString();
                        }
                    }
                    Transform timeTrans = recordItem.Find( "Time/Label" );
                    if ( timeTrans != null )
                    {
                        UILabel label = timeTrans.GetComponent<UILabel>();
                        if ( label != null )
                        {
                            label.text = StringUtil.GetStringSince1970( dt.time );
                        }
                    }
                }
                else
                {
                    recordItem.gameObject.SetActive( false );
                }
            }
       
        }
    }
    protected override void OnShow(object data)
    {
        base.OnShow( data );
        UpdateRecordInfo();
    }
   
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }
    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        HideSelf();
    }
}

