using System;
using System.Collections.Generic;

enum KAI_EVENT
{
    aevInvalid = 0,
    aevOnPrimaryTimer,
    aevOnSecondaryTimer,
    aevOnTertiaryTimer,
    aevOnPathSuccess,
    aevOnPathFailed,

    aevTotal,
};

public class KAIVM
{
    private static int MAX_ACTION_CALL = 64;
    private KCharacter m_pOwner = null;

    private uint m_nPrimaryTimerFrame = 0;
    private uint m_nSecondaryTimerFrame = 0;
    private uint m_nTertiaryTimerFrame = 0;
    private int m_nCurrentEvent = (int)KAI_EVENT.aevInvalid;
    private int m_nPendingEvent = (int)KAI_EVENT.aevInvalid;

    private uint m_dwPendingEventSrc = 0;
    private uint m_nPendingEventParam = 0;

    private KAILogic m_pAILogic = null;
    private KAIState m_pState = null;

    private int m_nAIType = 0;
    private int m_nStateID = 0;
    private int m_nActionID = 0;

    public KAIVM()
    {

    }


    public void FireEvent(int nEvent, uint dwEventSrc, uint nEventParam)
    {
        int nEventBlock = m_nCurrentEvent;
        int nCallCount = 0;

        KAIActionHandle pActionKey = null;

        if (m_pAILogic == null)
        {
            goto Exit0;
        }

        if (m_pOwner == null)
        {
            goto Exit0;
        }

        if (m_pState == null)
        {
            goto Exit0;
        }

        // 触发的事件预先判断在当前状态是否有托管
        pActionKey = m_pState.GetEventHandler(nEvent);
        if (pActionKey == null) { goto Exit0; }

        if (pActionKey.nAIActionID == KAIAction.KAI_ACTION_ID_NONE)
        {
            goto Exit1;
        }

        if (nEventBlock != (int)KAI_EVENT.aevInvalid)
        {
            // 重复事件替代
            if (m_nPendingEvent == (int)KAI_EVENT.aevInvalid || m_nPendingEvent == nEvent)
            {
                m_nPendingEvent = nEvent;
                m_dwPendingEventSrc = dwEventSrc;
                m_nPendingEventParam = nEventParam;
            }
            goto Exit0;
        }

        m_nCurrentEvent = nEventBlock;


        while (pActionKey.nAIActionID != KAIAction.KAI_ACTION_ID_NONE)
        {
            KAIActionHandle pNextActionKey = null;
            if (nCallCount > MAX_ACTION_CALL)
            {
                // 死循环检测
                break;
            }

            pNextActionKey = m_pAILogic.CallAction(m_pOwner, pActionKey);
            if (pNextActionKey.nAIActionID == KAIAction.KAI_ACTION_ID_ERROR)
            {
                m_pAILogic = null;  // 出错的时候就把AI清掉.
                break;
            }

            pActionKey = pNextActionKey;
            m_nActionID = pActionKey.nAIActionID;
            nCallCount++;
        }


    Exit1:
    Exit0:
        m_nCurrentEvent = nEventBlock;

        return;

    }

    public bool Setup(KCharacter pCharacter, int nAIType)
    {
        bool bResult = false;
        bool bRetCode = false;

        KAILogic pAILogic = null;
        int nInitState = 0;

        if (m_nCurrentEvent != 0) { goto Exit0; }

        m_pOwner = pCharacter;

        if (nAIType == 0)
        {
            // 清除原有AI类型
            m_pAILogic = null;
            goto Exit1;
        }


        pAILogic = FirstFightMgr.Instance().m_AIManager.GetAILogic(nAIType);
        if (pAILogic == null)
        {
            //error
            goto Exit0;
        }

        m_nAIType = nAIType;
        m_pAILogic = pAILogic;

        nInitState = m_pAILogic.GetInitState();

        bRetCode = SetState(nInitState);
        if (bRetCode == false)
        {
            //error
            goto Exit0;
        }

        m_nPrimaryTimerFrame = 0;
        m_nSecondaryTimerFrame = 0;
        m_nTertiaryTimerFrame = 0;
        m_nCurrentEvent = 0;


        FireEvent((int)KAI_EVENT.aevOnPrimaryTimer, m_pOwner.m_dwID, FirstFightMgr.Instance().m_nGameLoop);

    Exit1:
        bResult = true;
    Exit0:
        return bResult;
    }

    public void Active()
    {
        if (m_nPendingEvent != (int)KAI_EVENT.aevInvalid)
        {
            int nEvent = m_nPendingEvent;
            m_nPendingEvent = (int)KAI_EVENT.aevInvalid;

            FireEvent(nEvent, m_dwPendingEventSrc, m_nPendingEventParam);
        }

        if (m_nPrimaryTimerFrame != 0 && FirstFightMgr.Instance().m_nGameLoop > m_nPrimaryTimerFrame)
        {
            m_nPrimaryTimerFrame = 0;
            FireEvent((int)KAI_EVENT.aevOnPrimaryTimer, m_pOwner.m_dwID, FirstFightMgr.Instance().m_nGameLoop);
        }


        if (m_nSecondaryTimerFrame != 0 && FirstFightMgr.Instance().m_nGameLoop > m_nSecondaryTimerFrame)
        {
            m_nSecondaryTimerFrame = 0;
            FireEvent((int)KAI_EVENT.aevOnSecondaryTimer, m_pOwner.m_dwID, FirstFightMgr.Instance().m_nGameLoop);
        }

        if (m_nTertiaryTimerFrame != 0 && FirstFightMgr.Instance().m_nGameLoop > m_nTertiaryTimerFrame)
        {
            m_nTertiaryTimerFrame = 0;
            FireEvent((int)KAI_EVENT.aevOnTertiaryTimer, m_pOwner.m_dwID, FirstFightMgr.Instance().m_nGameLoop);
        }

    }


    public bool SetState(int nState)
    {
        bool bResult = false;
        KAIState pState = null;

        if (m_pAILogic == null)
        {
            //error
            goto Exit0;
        }

        pState = m_pAILogic.GetState(nState);
        if (pState == null)
        {
            //error
            goto Exit0;
        }

        m_pState = pState;
        m_nStateID = nState;

        bResult = true;
    Exit0:
        return bResult;
    }

    public void SetPrimaryTimer(uint nFrame)
    {
        if (nFrame <= 0)
        {
            return;
        }

        m_nPrimaryTimerFrame = FirstFightMgr.Instance().m_nGameLoop + nFrame;
    }

    public void SetSecondaryTimer(uint nFrame)
    {
        if (nFrame <= 0)
        {
            return;
        }

        m_nSecondaryTimerFrame = FirstFightMgr.Instance().m_nGameLoop + nFrame;
    }

    public void SetTertiaryTimer(uint nFrame)
    {
        if (nFrame <= 0)
        {
            return;
        }

        m_nTertiaryTimerFrame = FirstFightMgr.Instance().m_nGameLoop + nFrame;
    }


}

