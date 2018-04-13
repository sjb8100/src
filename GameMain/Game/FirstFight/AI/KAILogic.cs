using System;
using System.Collections.Generic;

public class KAILogic
{
    private int m_nAIType = 0;
    private int m_nInitState = 0;
    private Dictionary<int, KAIAction> m_ActionTable = new Dictionary<int, KAIAction>();
    private Dictionary<int, KAIState> m_StateTable = new Dictionary<int, KAIState>();

    public KAILogic()
    {

    }

    public bool Setup(int nType)
    {
        m_ActionTable.Clear();
        m_StateTable.Clear();

        m_nAIType = nType;
        m_nInitState = 0;

        KAIScript_Setup.Setup(nType, this);

        return true;
    }

    public int GetInitState()
    {
        return m_nInitState;
    }

    public void SetInitState(int nInitState)
    {
        KAIState pState = null;

        pState = GetState(nInitState);
        if (pState == null)
        {
            // error
        }

        m_nInitState = nInitState;

    }

    public KAIState GetState(int nState)
    {
        if (m_StateTable.ContainsKey(nState))
        {
            return m_StateTable[nState];
        }
        return null;
    }

    public KAIAction GetAction(int nActionID)
    {
        if (m_ActionTable.ContainsKey(nActionID))
        {
            return m_ActionTable[nActionID];
        }

        return null;
    }

    public KAIState NewState(int nState)
    {
        if (nState < 1)
        {
            //error
            goto Exit0;
        }

        if (m_StateTable.ContainsKey(nState))
        {
            //error
            goto Exit0;
        }


        KAIState pAIState = new KAIState(this);
        if (pAIState == null)
        {
            //error
            goto Exit0;
        }

        m_StateTable.Add(nState, pAIState);

        return pAIState;
    Exit0:
        return null;
    }


    public KAIAction NewAction(int nActionID, int nActionKey)
    {
        if (nActionID <= KAIAction.KAI_ACTION_ID_NONE)
        {
            //error           
            goto Exit0;
        }

        if (nActionKey <= (int)KAI_ACTION_KEY.eakInvalid)
        {
            //error
            goto Exit0;
        }

        if (nActionKey < KAIAction.KAI_USER_ACTION)
        {
            KAIManager.KAI_ACTION_FUNC pFucAction = null;

            pFucAction = FirstFightMgr.Instance().m_AIManager.GetActionFunction(nActionKey);

            if (pFucAction == null)
            {
                //error
                goto Exit0;

            }

        }
        else
        {
            //error
            goto Exit0;
        }

        if (m_ActionTable.ContainsKey(nActionID))
        {
            //error
            goto Exit0;
        }

        KAIAction pAIAction = new KAIAction();
        pAIAction.m_nKey = nActionKey;

        m_ActionTable.Add(nActionID, pAIAction);

        return pAIAction;
    Exit0:
        return null;
    }


    public KAIActionHandle CallAction(KCharacter pCharacter, KAIActionHandle pActionKey)
    {
        KAIActionHandle pResult = new KAIActionHandle();
        KAIActionHandle pNextAction = new KAIActionHandle();

        KAIAction pAction = pActionKey.pAIAction;
        int nActionKey = 0;

        if (pAction == null)
            pAction = GetAction(pActionKey.nAIActionID);

        if (pAction == null)
        {
            //error
            goto Exit0;
        }

        nActionKey = pAction.m_nKey;

        if (nActionKey < KAIAction.KAI_USER_ACTION)
        {
            int nBranchIndex = 0;
            ulong ulPrevTickCount = 0;
            ulong ulPostTickCount = 0;
            KAIManager.KAI_ACTION_FUNC PAction = null;

            PAction = FirstFightMgr.Instance().m_AIManager.GetActionFunction(nActionKey);
            if (PAction == null)
            { 
                //error
                goto Exit0; 
            }


            nBranchIndex = PAction(pCharacter, pAction);

            if (nBranchIndex > 0 && nBranchIndex <= KAIAction.KAI_ACTION_BRANCH_NUM)
            {
                int nNextActionID = pAction.m_nBranch[nBranchIndex - 1];
                KAIAction pNextActionTemp = pAction.m_pBranch[nBranchIndex - 1];

                if (pNextActionTemp == null)
                {
                    pNextActionTemp = GetAction(nNextActionID);
                    pAction.m_pBranch[nBranchIndex - 1] = pNextActionTemp;

                }
                pNextAction.nAIActionID = nNextActionID;
                pNextAction.pAIAction = pNextActionTemp;
            }
            if (nBranchIndex == -1)
            {
                pNextAction.nAIActionID = KAIAction.KAI_ACTION_ID_NONE;
                pNextAction.pAIAction = null;
            }
        }
        else
        {
            // 没做
            //error
        }


        pResult = pNextAction;
    Exit0:
        return pResult;
    }


}

