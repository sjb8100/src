using System;
using System.Collections.Generic;

public class KAIState
{
    private KAILogic m_pAILogic = null;

    class KAI_EVENT_HANDLE
    {
        public int nEvent;
        public KAIActionHandle ActionKey = new KAIActionHandle();
    };

    List<KAI_EVENT_HANDLE> m_EventHandleVector = new List<KAI_EVENT_HANDLE>();

    public KAIState(KAILogic pAILogic)
    {
        m_pAILogic = pAILogic;
    }


    public KAIActionHandle GetEventHandler(int nEvent)
    {
        KAIActionHandle pResult = new KAIActionHandle();
        
        pResult.nAIActionID = KAIAction.KAI_ACTION_ID_NONE;
        pResult.pAIAction = null;

        for (int i = 0; i < m_EventHandleVector.Count; i++)
        {
            if (m_EventHandleVector[i].nEvent == nEvent)
            {
                if (m_EventHandleVector[i].ActionKey.pAIAction == null)
                {
                    KAIAction pAction = null;

                    pAction = m_pAILogic.GetAction(m_EventHandleVector[i].ActionKey.nAIActionID);
                    if (pAction == null)
                    { 
                        //error
                        return null;
                    }

                    m_EventHandleVector[i].ActionKey.pAIAction = pAction;

                }
                pResult = m_EventHandleVector[i].ActionKey;
                break;
            }
        }


        return pResult;
    }
    
    public int HandleEvent(int nEvent, int nAction)
    {
        KAI_EVENT_HANDLE pHandle = new KAI_EVENT_HANDLE();

        for (int i = 0; i < m_EventHandleVector.Count; i++)
        {
            if (m_EventHandleVector[i].nEvent == nEvent)
            {
                //error
                goto Exit0;
            }
        }

        pHandle.nEvent = nEvent;
        pHandle.ActionKey.nAIActionID = nAction;
        pHandle.ActionKey.pAIAction = null;

        m_EventHandleVector.Add(pHandle);

    Exit0:
        return 0;
    }



}
    
