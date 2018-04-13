//********************************************************************
//	创建日期:	2016-11-29   15:23
//	文件名称:	HuntingGoPanel.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	狩猎面板传送子面板
//********************************************************************
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using table;
using Engine;
using Client;
using GameCmd;
using System.Text;
using Common;

partial class HuntingGoPanel : UIPanelBase
{
    uint MonsterID = 1;
    uint leftTransTime;
    protected override void OnLoading()
    {
        base.OnLoading();
    }
    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);

    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        InitPanel();
        MonsterID = DataManager.Manager<HuntingManager>().MonsterID;
    }
    protected override void OnHide()
    {
        base.OnHide();
    }
    void InitPanel()
    {
        uint nobleID = DataManager.Manager<Mall_HuangLingManager>().NobleID;
        table.NobleDataBase noble = GameTableManager.Instance.GetTableItem<table.NobleDataBase>(nobleID);
        if (noble.HuntingTransNumber >= DataManager.Manager<HuntingManager>().UsedTime)
        {
            leftTransTime = noble.HuntingTransNumber - DataManager.Manager<HuntingManager>().UsedTime;
        }
        else 
        {
            leftTransTime = 0;
        }
        m_label_Frequency.text = leftTransTime.ToString(); ;
        m_label_RemindLabel.text = string.Format("（每天{0}次免费传送次数，提高皇令等级可以增加次数）", noble.HuntingTransNumber);
       
    }
    void onClick_Close_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.HuntingGoPanel);
    }

    void onClick_Btn_move_Btn(GameObject caster)
    {
        table.HuntingDataBase hunt = GameTableManager.Instance.GetTableItem<HuntingDataBase>(MonsterID);
        if (!KHttpDown.Instance().SceneFileExists(hunt.mapID))
        {
            //打开下载界面
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.HuntingGoPanel);
            return;
        }

        IController ctrl = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl();
        ctrl.GotoMap(hunt.mapID, new UnityEngine.Vector3(hunt.transmitCoordinateX, 0, -hunt.transmitCoordinateY));
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.HuntingGoPanel);

    }

    void onClick_Btn_transmission_Btn(GameObject caster)
    {
        table.HuntingDataBase checkHunt = GameTableManager.Instance.GetTableItem<HuntingDataBase>(MonsterID);
        if (!KHttpDown.Instance().SceneFileExists(checkHunt.mapID))
        {
            //打开下载界面
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.HuntingGoPanel);
            return;
        }

        if (leftTransTime > 0)
        {
            Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
            if (cs != null)
            {
                cs.GetCombatRobot().Stop();// 停止挂机
            }

            table.HuntingDataBase hunt = GameTableManager.Instance.GetTableItem<HuntingDataBase>(MonsterID);
            DataManager.Manager<RideManager>().TryUsingRide(delegate(object o)
            {               
                NetService.Instance.Send(new stRequestTransScriptUserCmd_CS() { boss_index = MonsterID });
            }, null);
           
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.HuntingGoPanel);
            IMapSystem mapsys = Client.ClientGlobal.Instance().GetMapSystem();
            if (mapsys.GetMapID() == hunt.transmitMapID)
            {
                 DataManager.Manager<HuntingManager>().StartLoading();
            } 
        }
        else
        {
            TipsManager.Instance.ShowTips(DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Notice_NoEnoughTransmitTime));
        }
    }

    void Update()
    {
    }
}