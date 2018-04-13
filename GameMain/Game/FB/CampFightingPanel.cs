//*************************************************************************
//	创建日期:	2016-11-30 21:11
//	文件名称:	CampFightingPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	阵营战
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
using Client;


partial class CampFightingPanel : UIPanelBase
{
    CampCombatManager m_dataMgr = null;

    long m_lCloseNpcid = 0;
    protected override void OnLoading()
    {
        base.OnLoading();
        m_dataMgr = DataManager.Manager<CampCombatManager>();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_dataMgr = DataManager.Manager<CampCombatManager>();
        m_dataMgr.ValueUpdateEvent += OnValueUpdateEventArgs;
        
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.MAINRIGHTBTN_TOGGLE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.MAINLEFTTBTN_TOGGLE, OnEvent);

       // Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CAMP_ADDCOLLECTNPC, OnEvent);

        MainPanel panel = DataManager.Manager<UIPanelManager>().GetPanel<MainPanel>(PanelID.MainPanel);
        if (panel != null)
        {
            m_trans_CampWarContent.gameObject.SetActive(!panel.IsShowRightBtn());
        }

        RefreshCampBattleData();
    }

    /// <summary>
    /// 重置信息
    /// </summary>
    private void ResetCampBattleInfo()
    {
        if (null != m_label_DemonScore)
        {
            m_label_DemonScore.text = "0";
        }

        if (null != m_label_GodScore)
        {
            m_label_GodScore.text = "0";
        }

        if (null != m_label_RankNum)
        {
            m_label_RankNum.text = "--";
        }

        if (null != m_label_ScoreNum)
        {
            m_label_ScoreNum.text = "--";
        }
    }



    protected override void OnHide()
    {
      //  Debug.Log("=====================================");
        base.OnHide();
       // Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CAMP_ADDCOLLECTNPC, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.MAINRIGHTBTN_TOGGLE, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.MAINLEFTTBTN_TOGGLE, OnEvent);
        m_dataMgr.ValueUpdateEvent -= OnValueUpdateEventArgs;
    }

    void OnEvent(int nEventid,object param)
    {
        //if (nEventid == (int)Client.GameEventID.CAMP_ADDCOLLECTNPC)
        //{
        //    stCampCollectNpc npc = (stCampCollectNpc)param;
        //    if (npc.enter)
        //    {
        //        m_btn_Btn_Pick.gameObject.SetActive(true);
        //        m_lCloseNpcid = npc.npcid;
        //    }
        //    else
        //    {
        //        m_lCloseNpcid = 0;
        //        m_btn_Btn_Pick.gameObject.SetActive(false);
        //    }
        //}else 
        if ((int)Client.GameEventID.MAINRIGHTBTN_TOGGLE == nEventid | (int)Client.GameEventID.MAINLEFTTBTN_TOGGLE == nEventid)
        {
            bool show = (bool)param;
            //Debug.Log("show :" + show);
            m_trans_CampWarContent.gameObject.SetActive(!show);
        }
    }

     
    void onClick_Btn_Pick_Btn(GameObject caster)
    {   /*
        if (m_lCloseNpcid != 0)
        {
            IEntity npc = ClientGlobal.Instance().GetEntitySystem().FindEntity(m_lCloseNpcid);
            if (npc != null)
            {
                bool ride = (bool)ClientGlobal.Instance().MainPlayer.SendMessage(EntityMessage.EntityCommond_IsRide, null);
                if (ride)
                {
                    ClientGlobal.Instance().netService.Send(new GameCmd.stDownRideUserCmd_C() { });
                    ClientGlobal.Instance().MainPlayer.SendMessage(EntityMessage.EntityCommond_UnRide, null);
                }
                PlayAni anim_param = new PlayAni();
                anim_param.strAcionName = EntityAction.Collect;
                anim_param.fSpeed = 1;
                anim_param.nStartFrame = 0;
                anim_param.nLoop = -1;
                anim_param.fBlendTime = 0.2f;
                ClientGlobal.Instance().MainPlayer.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);

                NetService.Instance.Send(new GameCmd.stClickNpcScriptUserCmd_C()
                {
                    dwNpcTempID = npc.GetID(),
                   // baseid = (uint)npc.GetProp((int)Client.EntityProp.BaseID),
                });
            }
        }*/
    }
    
    /// <summary>
    /// 刷新战斗信息
    /// </summary>
    /// <param name="data"></param>
    private void RefreshCampBattleData()
    {
        if (!m_dataMgr.CampBattleEnable)
        {
            ResetCampBattleInfo();
        }

        CampCombatResultInfo campCombatResultInfo = m_dataMgr.CampCombatResultData;
        if (campCombatResultInfo == null)
        {
            return;
        }

        if (null != m_label_DemonScore)
        {
            m_label_DemonScore.text = "0";
        }

        CampCombatResultInfo.CampCombatResult campCombatResult = null;
        if (null != m_label_GodScore)
        {
            campCombatResult = campCombatResultInfo.m_camp_Green;
            if (null != campCombatResult)
            {
                m_label_GodScore.text = campCombatResult.nScore.ToString();
            }
        }

        if (null != m_label_DemonScore)
        {
            campCombatResult = campCombatResultInfo.m_camp_Red;
            if (null != m_label_DemonScore)
            {
                m_label_DemonScore.text = campCombatResult.nScore.ToString();
            }
        }

        CampCombatResultInfo.CampCombatPlayerInfo campPlayerInfo = campCombatResultInfo.m_MyCampCombatInfo;
        if (null != campPlayerInfo)
        {
            if (null != m_label_RankNum)
            {
                m_label_RankNum.text = campPlayerInfo.nRank.ToString();
            }

            if (null != m_label_ScoreNum)
            {
                m_label_ScoreNum.text = campPlayerInfo.nScore.ToString();
            }
        }
    }

    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs value)
    {
        if (value.key.Equals("CampKillInfo") || value.key.Equals("RefreshBattleData"))
        {
            RefreshCampBattleData();
        }
    }



    void onClick_BtnBattle_Btn(GameObject caster)
    {

        //
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CampWarBattleListPanel,prePanelData:GetPanelData());
        DataManager.Instance.Sender.RequestCampInfoCamp(0, 0, DataManager.Manager<CampCombatManager>().FightingIndex);

        HideSelf();
    }
}