//********************************************************************
//	创建日期:	2016-12-5   11:36
//	文件名称:	HuntingManager.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	狩猎数据管理类
//********************************************************************
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Engine;
using UnityEngine;
using GameCmd;
using table;
using System;

public class HuntingManager : BaseModuleData, IManager
{
    public uint usedTime = 0;
    public uint UsedTime
    {
        get
        {
            return usedTime;
        }
        set
        {
            usedTime = value;
        }
    }
    public uint boss_state = 0;
    public uint Boss_State
    {
        get
        {
            return boss_state;
        }
        set
        {
            boss_state = value;
        }

    }
    public uint MonsterID = 1;

    public Dictionary<uint, BossRefreshInfo> boss_dic = new Dictionary<uint, BossRefreshInfo>();

    public void RecieveAllBossInfo(stReqAllBossRefTimeScriptUserCmd_S cmd)
    {
        List<BossRefreshInfo> data = cmd.boss_info;
        if (boss_dic.Count > 0)
        {
            boss_dic.Clear();
        }
        for (int i = 0; i < data.Count; i++)
        {
            boss_dic.Add(data[i].boss_index, data[i]);
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.HUNT_CHANGEBOSSSTATE, null);

    }
    public void RecieveBossLeftTime(stReqBossRefTimeScriptUserCmd_CS cmd)
    {
        boss_dic[cmd.boss_info.boss_index] = cmd.boss_info;
    }
    public void GetUsedTime(stRefreshNobleFreeTransTimsPropertyUserCmd_S cmd)
    {
        UsedTime = cmd.times;
        if(DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.DailyPanel))
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.HuntingGoPanel);        
        }
       
    }


    public void Initialize()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.PLAYER_LOGIN_SUCCESS, OnPlayerLoginSuccess);
    }
    public void ClearData()
    {

    }
    private void OnPlayerLoginSuccess(int eventid, object cmd)
    {
        if (eventid == (int)Client.GameEventID.PLAYER_LOGIN_SUCCESS)
        {
            NetService.Instance.Send(new stReqAllBossRefTimeScriptUserCmd_C());
            //请求传送剩余次数
            NetService.Instance.Send(new stRefreshNobleFreeTransTimsPropertyUserCmd_C());
        }
    }
    public bool m_bool_loadingAnimStart = false;
    public float m_f_loadingAnimDur = 2;
    public float m_f_loadingAnimTime = 0;
    public void StartLoading()
    {
        DataManager.Manager<UIPanelManager>().ShowLoading("马上进入场景",1);
        m_bool_loadingAnimStart = true;
        m_f_loadingAnimTime = 0;
    }
   
    public void Process(float deltime)
    {
        if (m_bool_loadingAnimStart)
        {
            m_f_loadingAnimTime += Time.deltaTime;
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.LoadingPanel, UIMsgID.eLoadingProcess, Mathf.Clamp01(m_f_loadingAnimTime / m_f_loadingAnimDur));
            if (m_f_loadingAnimTime >= m_f_loadingAnimDur)
            {
                m_bool_loadingAnimStart = false;
                m_f_loadingAnimTime = 0;

                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.LoadingPanel);
                DataManager.Manager<UIPanelManager>().ShowMain();
            }
        }
        

    }
    public void Reset(bool depthClearData = false)
    {
        usedTime = 0;
        boss_state = 0;
        boss_dic.Clear();
    }
}

