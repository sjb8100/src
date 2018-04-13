//*************************************************************************
//	创建日期:	2017-5-10 11:41
//	文件名称:	MainMissionArrow.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	主线任务箭头指引
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
using Client;
class MainMissionArrow :Engine.Utility.ITimer
{
    const int TIMER_ID = 1029;

    int m_nStartTimes = 0;
    int m_nStandTime = 0;
    int m_nignoreLevel = 0;
    GameObject m_arrowObj = null;

    bool addTimer = false;
    bool m_arrowShow = false;
    public void Init(GameObject go)
    {
        m_arrowObj = go;
        Client.IGameOption option = Client.ClientGlobal.Instance().gameOption;
        if (option != null && m_nStandTime == 0)
        {
            m_nStandTime = option.GetInt("UI", "TaskRoleStandTime", 3);
        }

        if (option != null)
        {
            m_nignoreLevel = option.GetInt("UI", "TaskTipIgnoreLevel", 30);
            if (m_nignoreLevel <= MainPlayerHelper.GetPlayerLevel())
            {
                ResetArrow();
                return;
            }
        }

       

        RegisterEvent(true);
        
        ResetArrow();
        if (CheckCondition())
        {
            SetTime();
        }
    }

    private void RegisterEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_DONING, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.UIEVENTGUIDESHOWOUT, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SKILLSYSTEM_USESKILL, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.STORY_PLAY_OVER, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKILLNONESTATE_ENTER, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.COMBOT_ENTER_EXIT, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSSTART, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSEND, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_MAIN_ARROWHIDE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTGUIDECOMPLETE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTGUIDESKIP, OnEvent);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_DONING, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.UIEVENTGUIDESHOWOUT, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLSYSTEM_USESKILL, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.STORY_PLAY_OVER, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SKILLNONESTATE_ENTER, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.COMBOT_ENTER_EXIT, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSSTART, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSEND, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_MAIN_ARROWHIDE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTGUIDECOMPLETE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTGUIDESKIP, OnEvent);

        }
    }

    void OnEvent(int nEvent, object param)
    {
        Client.GameEventID ge = (Client.GameEventID)nEvent;
        if (ge == Client.GameEventID.TASK_DONING || ge == GameEventID.UIEVENTGUIDESHOWOUT || ge == GameEventID.SKILLSYSTEM_USESKILL)
        {
            ResetArrow();
            KillTimer();
        }
        else if (ge == GameEventID.STORY_PLAY_OVER)
        {
            ShowArrow();
        }
        else if (ge == GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE)
        {
            stEntityBeginMove move = (stEntityBeginMove)param;
            if (Client.ClientGlobal.Instance().IsMainPlayer(move.uid))
            {
                ResetArrow();
                KillTimer();
            }
        }
        else if (ge == GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE)
        {
            Client.stEntityStopMove stopEntity = (Client.stEntityStopMove)param;
            if (ClientGlobal.Instance().IsMainPlayer(stopEntity.uid))
            {
                if (CheckCondition())
                {
                     SetTime();
                }
            }
        }
        else if (ge == Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED)
        {
            UIPanelManager.PanelFocusData fdata = (UIPanelManager.PanelFocusData)param;
            if (fdata.ID == PanelID.MissionAndTeamPanel)
            {
                if (fdata.GetFocus)
                {
                    if (CheckCondition())
                    {
                        SetTime();
                    }
                }
                else
                {
                    ResetArrow();
                    KillTimer();
                }
            }
        }
        else if (ge == Client.GameEventID.ENTITYSYSTEM_PROPUPDATE)
        {
            Client.stPropUpdate prop = (Client.stPropUpdate)param;
            if (Client.ClientGlobal.Instance().IsMainPlayer(prop.uid))
            {
                if (m_nignoreLevel <= MainPlayerHelper.GetPlayerLevel())
                {
                    RegisterEvent(false);
                    ResetArrow();
                    KillTimer();
                }
            }
        }
        else if (ge == GameEventID.SKILLNONESTATE_ENTER)
        {
            stSkillStateEnter state = (stSkillStateEnter)param;
            if (ClientGlobal.Instance().IsMainPlayer(state.uid))
            {
                if (CheckCondition())
                {
                    SetTime();
                }
            }
        }
        else if (ge == Client.GameEventID.COMBOT_ENTER_EXIT)
        {
            stCombotCopy cc = (stCombotCopy)param;
            if (cc.exit)
            {
                if (CheckCondition())
                {
                    SetTime();
                }
            }else if (cc.enter)
            {
                ResetArrow();
                KillTimer();
            }
        }
        else if (ge == GameEventID.SKILLGUIDE_PROGRESSSTART)
        {
            ResetArrow();
            KillTimer();
        }
        else if (ge == GameEventID.SKILLGUIDE_PROGRESSEND)
        {
            if (CheckCondition())
            {
                SetTime();
            }
        }
        else if (ge == Client.GameEventID.TASK_MAIN_ARROWHIDE)
        {
            ResetArrow();
            KillTimer();
        }
        else if (ge == Client.GameEventID.UIEVENTGUIDECOMPLETE || ge == Client.GameEventID.UIEVENTGUIDESKIP)
        {
            if (CheckCondition())
            {
                SetTime();
            }
        }
    }

    bool CheckCondition()
    {
        IEntity mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
        {
            return false;
        }

        if (!DataManager.Manager<UIPanelManager>().IsPanelFocus(PanelID.MissionAndTeamPanel))
        {
            return false;
        }

        if (!mainPlayer.IsPlayingAnim(Client.EntityAction.Stand) && !mainPlayer.IsPlayingAnim(Client.EntityAction.Stand_Combat))
        {
            return false;
        }

        if (DataManager.Manager<GuideManager>().IsShowGuildUI())
        {
            return false;
        }
        //if (DataManager.Manager<ComBatCopyDataManager>().EnterCopyID != 0)
        if (true == DataManager.Manager<ComBatCopyDataManager>().IsEnterCopy)
        {
            return false;
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainUsePanel))
        {
            return false;
        }
        if (m_RootHide)
        {
            return false;
        }
        return true;
       
    }

    void SetTime()
    {
        addTimer = true;
        if (Engine.Utility.TimerAxis.Instance().IsExist(TIMER_ID ,this))
        {
            Engine.Utility.TimerAxis.Instance().KillTimer(TIMER_ID, this);
        }
        Engine.Utility.TimerAxis.Instance().SetTimer(TIMER_ID,1000,this);
    }

    void KillTimer()
    {
        if (addTimer)
        {
            addTimer = false;
            Engine.Utility.TimerAxis.Instance().KillTimer(TIMER_ID,this);
        }
    }

    public void OnTimer(uint uTimerID)
    {
        if (uTimerID != TIMER_ID)
        {
            return;
        }

        if (!CheckCondition())
        {
            KillTimer();
        }

        m_nStartTimes++;

        if (m_nStartTimes >= m_nStandTime)
        {
            ShowArrow();
            KillTimer();
        }
    }

    private void ShowArrow()
    {
        if (m_arrowObj != null)
        {
            m_nStartTimes = 0;
            if (!m_arrowObj.activeSelf)
            {
                m_arrowShow = true;
                m_arrowObj.SetActive(true);
            }
        }
    }

    public void Clear()
    {
        RegisterEvent(false);
        ResetArrow();
    }

    public void ResetArrow()
    {
        if (m_arrowObj != null)
        {
            m_nStartTimes = 0;
            if (m_arrowObj.activeSelf)
            {
                m_arrowShow = false;
                m_arrowObj.SetActive(false);
            }
        }
    }

    bool m_RootHide = false;
    bool isActivePre = false;
    public void HideWhenRootHide()
    {
        m_RootHide = true;
        if (m_arrowObj.activeSelf)
        {
            isActivePre = true;
            m_arrowObj.SetActive(false);
            KillTimer();
        }else{
            isActivePre = false;
        }
    }

    public void ShowWhenRootShow()
    {
        m_RootHide = false;

        if (isActivePre && m_arrowShow)
        {
            m_arrowObj.SetActive(true);
        }
        else
        {
            CheckCondition();
        }
    }
}
