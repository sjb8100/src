using System;
using System.Collections.Generic;

public class KAIManager
{
    public delegate int KAI_ACTION_FUNC(KCharacter pCharacter, KAIAction pActionData);
    private KAI_ACTION_FUNC[] m_ActionFunctionTable = new KAI_ACTION_FUNC[(int)KAI_ACTION_KEY.eakTotal];
    public class KAIInfo
    {
        public KAILogic pLogic = null;
    };
    public Dictionary<int, KAIInfo> m_AITable = new Dictionary<int, KAIInfo>();

    public KAIManager()
    {

    }

    public bool Init()
    {
        RegisterActionFunctions();
        LoadAITabFile();

        return true;
    }

    public void UnInit()
    {

    }

    private void RegisterActionFunctions()
    {
        m_ActionFunctionTable[(int)KAI_ACTION_KEY.eakSetState] = KAI_ACTION_FUNC_CODE.AISetState;
        m_ActionFunctionTable[(int)KAI_ACTION_KEY.eakSetPrimaryTimer] = KAI_ACTION_FUNC_CODE.AISetPrimaryTimer;
        m_ActionFunctionTable[(int)KAI_ACTION_KEY.eakCastSkill] = KAI_ACTION_FUNC_CODE.AICastSkill;
        m_ActionFunctionTable[(int)KAI_ACTION_KEY.eakSearchEnemy] = KAI_ACTION_FUNC_CODE.AISearchEnemy;
        m_ActionFunctionTable[(int)KAI_ACTION_KEY.eakIsInFight] = KAI_ACTION_FUNC_CODE.AIIsInFight;
        m_ActionFunctionTable[(int)KAI_ACTION_KEY.eakKeepRange] = KAI_ACTION_FUNC_CODE.AIKeepRange;
        m_ActionFunctionTable[(int)KAI_ACTION_KEY.eakIsTargetExist] = KAI_ACTION_FUNC_CODE.AIIsTargetExist;
        m_ActionFunctionTable[(int)KAI_ACTION_KEY.eakAddTargetToThreatList] = KAI_ACTION_FUNC_CODE.AIAddTargetToThreatList;
        m_ActionFunctionTable[(int)KAI_ACTION_KEY.eakRecordReturnPosition] = KAI_ACTION_FUNC_CODE.AIRecordReturnPosition;
        m_ActionFunctionTable[(int)KAI_ACTION_KEY.eakRecordOriginPosition] = KAI_ACTION_FUNC_CODE.AIRecordOriginPosition;
        m_ActionFunctionTable[(int)KAI_ACTION_KEY.eakKeepOriginDirection] = KAI_ACTION_FUNC_CODE.AIKeepOriginDirection;
        m_ActionFunctionTable[(int)KAI_ACTION_KEY.eakNpcStandardSkillSelector] = KAI_ACTION_FUNC_CODE.AINpcStandardSkillSelector;
        m_ActionFunctionTable[(int)KAI_ACTION_KEY.eakNpcKeepSkillCastRange] = KAI_ACTION_FUNC_CODE.AINpcKeepSkillCastRange;
        m_ActionFunctionTable[(int)KAI_ACTION_KEY.eakNpcCastSelectSkill] = KAI_ACTION_FUNC_CODE.AINpcCastSelectSkill;
        m_ActionFunctionTable[(int)KAI_ACTION_KEY.eakStand] = KAI_ACTION_FUNC_CODE.AIStand;

        
        
        
    }

    public KAI_ACTION_FUNC GetActionFunction(int nKey)
    {
        if (nKey > (int)KAI_ACTION_KEY.eakInvalid && nKey < (int)KAI_ACTION_KEY.eakTotal)
        {
            return m_ActionFunctionTable[nKey];
        }
        return null;
    }

    private void LoadAITabFile()
    {
        int nAIType = 1;

        KAIInfo pAIInfo = new KAIInfo();
        pAIInfo.pLogic = null;

        m_AITable[nAIType] = pAIInfo;
    }

    public KAILogic GetAILogic(int nAIType)
    {
        KAIInfo pInfo = null;
        KAILogic pLogic = null;

        if (!m_AITable.ContainsKey(nAIType))
        {
            goto Exit0; 
        }

        pInfo = m_AITable[nAIType];

        if (pInfo == null) { goto Exit0; }

        if (pInfo.pLogic == null)
        {
            pInfo.pLogic = CreateAI(nAIType);
        }

        pLogic = pInfo.pLogic;
    Exit0:
        return pLogic;
    }


    private KAILogic CreateAI(int nType)
    {
        bool bRetCode = false;

        KAILogic pResult = null;
        KAILogic pAI = null;

        pAI = new KAILogic();
        if (pAI == null) { goto Exit0; }

        bRetCode = pAI.Setup(nType);
        if (bRetCode == false) { goto Exit0; }

        pResult = pAI;
    Exit0:
        return pResult;
    }


}

