using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using GameCmd;
using table;
using Engine;
using Engine.Utility;

partial class HomeControlPanel: UIPanelBase
{

    protected override void OnLoading()
    {
        base.OnLoading();
        homeDM.GetTreeData();
//         Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.HOME_HELPSUCCESS, EventCallBack);
    }
    protected override void OnShow(object data)
    {
        base.OnShow( data );
    }
    protected override void OnPanelBaseDestory()
    {      
        base.OnPanelBaseDestory();
//         Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.HOME_HELPSUCCESS, EventCallBack);

    }

    public void UpdateHomeControlUI()
    {
        if (DataManager.Manager<HomeDataManager>().IsMyHome())
        {
            //自己家园UI上的显示
            m_widget_myhome.gameObject.SetActive(true);
            m_widget_theirhome.gameObject.SetActive(false);
        }
        else 
        {
            //别人家园UI上的显示
            m_widget_myhome.gameObject.SetActive(false);
            m_widget_theirhome.gameObject.SetActive(true);
        }
        
    }


    void onClick_Btn_leavehome_Btn(GameObject caster)
    {
        HomeScene.Instance.Leave();
        IMapSystem mapSys = ClientGlobal.Instance().GetMapSystem();
        if ( mapSys == null )
        {
            return;
        }

        mapSys.EnterMap(2, Vector3.zero);
    }

    void onClick_Btn_gain_Btn(GameObject caster)
    {
        homeDM.GainOnePlant();
    }

    void onClick_Btn_gainall_Btn(GameObject caster)
    {
        homeDM.GainAllPlant();
    }
    
    /// <summary>
    /// 回家
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_returnhome_Btn(GameObject caster)
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        homeDM.ReqAllUserHomeData(player.GetID());
    }


    void onClick_Btn_trade_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel( PanelID.HomeTradePanel);
    }
    
}
