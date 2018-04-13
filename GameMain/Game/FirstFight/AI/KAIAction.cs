using System;
using System.Collections.Generic;

enum AI_SKILL_TYPE
{
    aistInvalid = 0,

    aistTotal
};

enum KAI_ACTION_KEY
{
    eakInvalid = 0,
    eakSetState,                // 切换AI状态
    eakSetPrimaryTimer,
    eakSetSecondaryTimer,
    eakSetTertiaryTimer,

    eakCastSkill,

    eakSearchEnemy,
    eakIsInFight,
    eakKeepRange,
    eakAddTargetToThreatList,
    eakRecordReturnPosition,
    eakRecordOriginPosition,
    eakKeepOriginDirection,

    eakNpcStandardSkillSelector,
    eakIsTargetExist,
    eakNpcKeepSkillCastRange,
    eakNpcCastSelectSkill,
    eakStand,

    eakTotal
}

public class KAIActionHandle
{
    public int nAIActionID = KAIAction.KAI_ACTION_ID_ERROR;
    public KAIAction pAIAction = null;
};

public class KAIAction
{
    public static int MIN_TIMER_INTERVAL = (16 / 16);

    public static int KAI_USER_ACTION = 10000;
    public static int KAI_ACTION_PARAM_NUM = 5;
    public static int KAI_ACTION_BRANCH_NUM = 3;
    public static int KAI_ACTION_ID_ERROR = -1;
    public static int KAI_ACTION_ID_NONE = 0;

    public int m_nKey = 0;

    public int[] m_nParam = new int[KAI_ACTION_PARAM_NUM];
    public int[] m_nBranch = new int[KAI_ACTION_BRANCH_NUM];
    public KAIAction[] m_pBranch = new KAIAction[KAI_ACTION_BRANCH_NUM];

    public KAIAction()
    {
        for (int i = 0; i < KAI_ACTION_BRANCH_NUM; ++i)
        {
            m_nBranch[i] = KAI_ACTION_ID_ERROR;
        }
    }


    public void SetParam(int nValue0)
    {
        m_nParam[0] = nValue0;
    }

    public void SetParam(int nValue0, int nValue1)
    {
        m_nParam[0] = nValue0;
        m_nParam[1] = nValue1;
    }

    public void SetParam(int nValue0, int nValue1, int nValue2)
    {
        m_nParam[0] = nValue0;
        m_nParam[1] = nValue1;
        m_nParam[2] = nValue2;
    }

    public void SetParam(int nValue0, int nValue1, int nValue2, int nValue3)
    {
        m_nParam[0] = nValue0;
        m_nParam[1] = nValue1;
        m_nParam[2] = nValue2;
        m_nParam[3] = nValue3;
    }

    public void SetParam(int nValue0, int nValue1, int nValue2, int nValue3, int nValue4)
    {
        m_nParam[0] = nValue0;
        m_nParam[1] = nValue1;
        m_nParam[2] = nValue2;
        m_nParam[3] = nValue3;
        m_nParam[4] = nValue4;
    }

    public void SetBranch(int nBranch0)
    {
        if (nBranch0 <= KAI_ACTION_ID_NONE)
        {
            //error
            return;
        }

        m_nBranch[0] = nBranch0;
    }

    public void SetBranch(int nBranch0, int nBranch1)
    {
        if (nBranch0 <= KAI_ACTION_ID_NONE)
        {
            //error
            return;
        }
        if (nBranch1 <= KAI_ACTION_ID_NONE)
        {
            //error
            return;
        }

        m_nBranch[0] = nBranch0;
        m_nBranch[1] = nBranch1;
    }

    public void SetBranch(int nBranch0, int nBranch1, int nBranch2)
    {
        if (nBranch0 <= KAI_ACTION_ID_NONE)
        {
            //error
            return;
        }
        if (nBranch1 <= KAI_ACTION_ID_NONE)
        {
            //error
            return;
        }
        if (nBranch2 <= KAI_ACTION_ID_NONE)
        {
            //error
            return;
        }

        m_nBranch[0] = nBranch0;
        m_nBranch[1] = nBranch1;
        m_nBranch[2] = nBranch2;
    }

}


public class KAI_ACTION_FUNC_CODE
{
    public static int AISetState(KCharacter pCharacter, KAIAction pActionData)
    {
        int nState = pActionData.m_nParam[0];
        pCharacter.m_AIVM.SetState(nState);
        return -1;
    }

    public static int AISetPrimaryTimer(KCharacter pCharacter, KAIAction pActionData)
    {
        int nBranchSuccess = 1;
        int nFrame = pActionData.m_nParam[0];

        if (nFrame < KAIAction.MIN_TIMER_INTERVAL)
            nFrame = KAIAction.MIN_TIMER_INTERVAL;

        pCharacter.m_AIVM.SetPrimaryTimer((uint)nFrame);

        return nBranchSuccess;
    }

    public static int AISetSecondaryTimer(KCharacter pCharacter, KAIAction pActionData)
    {
        int nBranchSuccess = 1;
        int nFrame = pActionData.m_nParam[0];

        if (nFrame < KAIAction.MIN_TIMER_INTERVAL)
            nFrame = KAIAction.MIN_TIMER_INTERVAL;

        pCharacter.m_AIVM.SetSecondaryTimer((uint)nFrame);

        return nBranchSuccess;
    }


    public static int AISetTertiaryTimer(KCharacter pCharacter, KAIAction pActionData)
    {
        int nBranchSuccess = 1;
        int nFrame = pActionData.m_nParam[0];

        if (nFrame < KAIAction.MIN_TIMER_INTERVAL)
            nFrame = KAIAction.MIN_TIMER_INTERVAL;

        pCharacter.m_AIVM.SetTertiaryTimer((uint)nFrame);

        return nBranchSuccess;
    }

    public static int AIRecordOriginPosition(KCharacter pCharacter, KAIAction pActionData)
    {
        int nResult = 0;
        int nBranchSuccess = 1;

        nResult = nBranchSuccess;
    Exit0:
        return nResult;
    }

    public static int AIKeepOriginDirection(KCharacter pCharacter, KAIAction pActionData)
    {
        int nResult = 0;
        int nBranchSuccess = 1;

        nResult = nBranchSuccess;
    Exit0:
        return nResult;
    }


    public static int AINpcStandardSkillSelector(KCharacter pCharacter, KAIAction pActionData)
    {
        int nResult = 0;
        int nBranchSuccess = 1;
        KNpc pNpc = null;

        pNpc = (KNpc)pCharacter;


        pNpc.m_nSkillSelectIndex = 0;


        nResult = nBranchSuccess;
    Exit0:
        return nResult;
    }


    public static int AINpcKeepSkillCastRange(KCharacter pCharacter, KAIAction pActionData)
    {
        int nResult = 0;
        int nRetCode = 0;
        int nBranchInRange = 1;
        int nBranchKeep = 2;
        float fMinRange = 0f;
        float fMaxRange = 0f;
        uint dwSkillID = 0;
        KNpc pNpc = null;
        KSkill pSkill = null;

        pNpc = (KNpc)pCharacter;


        dwSkillID = pNpc.m_pTemplate.dwSkillIDList[pNpc.m_nSkillSelectIndex];

        if (dwSkillID == 10001)
        {
            int gg = 0;
        }

        pSkill = FirstFightMgr.Instance().m_SkillManager.GetSkill(dwSkillID);
        if(pSkill == null)
        {
            goto Exit0;
        }

        fMinRange = pSkill.m_fMinRadius;
        fMaxRange = pSkill.m_fMaxRadius;

        nRetCode = KeepRange(pCharacter, fMinRange, fMaxRange);
        if (!(nRetCode > 0)) { goto Exit0; }


        if (nRetCode == 1)
        {
            nResult = nBranchInRange;
            goto Exit0;
        }

        nResult = nBranchKeep;
    Exit0:
        return nResult;
    }


    public static int AINpcCastSelectSkill(KCharacter pCharacter, KAIAction pActionData)
    {
        int nResult = 0;
        int nRetCode = 0;
        int nBranchSuccess = 1;
        int nBranchFailed = 2;
        int nSelectIndex = 0;
        KNpc pNpc = null;
        uint dwSkillID = 0;

        pNpc = (KNpc)pCharacter;

        nSelectIndex = pNpc.m_nSkillSelectIndex;
        dwSkillID = pNpc.m_pTemplate.dwSkillIDList[nSelectIndex];


        if (FirstFightMgr.Instance().m_nGameLoop < pNpc.m_nSkillCommomCD)
        {
            return nBranchFailed;
        }
        if (FirstFightMgr.Instance().m_nGameLoop < pNpc.m_nSkillCastFrame[nSelectIndex])
        {
            return nBranchFailed;
        }


        nRetCode = CastSkill(pCharacter, dwSkillID, 0, (int)AI_SKILL_TYPE.aistInvalid);
        if (nRetCode <= 0)
        {
            goto Exit0;
        }

        if (nRetCode == 1)
        {
            pNpc.m_nSkillCastFrame[nSelectIndex] = (int)FirstFightMgr.Instance().m_nGameLoop + pNpc.m_pTemplate.nSkillCastInterval[nSelectIndex];
            pNpc.m_nSkillCommomCD = (int)FirstFightMgr.Instance().m_nGameLoop + 1;
            
            nResult = nBranchSuccess;
            goto Exit0;
        }

        nResult = nBranchFailed;
    Exit0:
        return nResult;
    }

    public static int AIRecordReturnPosition(KCharacter pCharacter, KAIAction pActionData)
    {
        int nBranchSuccess = 1;

        return nBranchSuccess;
    }
    public static int AIIsInFight(KCharacter pCharacter, KAIAction pActionData)
    {
        int nResult = 0;
        int nBranchNotOut = 1;
        int nBranchOutFight = 2;

        nResult = nBranchNotOut;
    Exit0:
        return nResult;
    }


    static float angle_360(ref UnityEngine.Vector3 from, ref UnityEngine.Vector3 to)
    {
        //两点的x、y值
        float x = from.x - to.x;
        float z = from.z - to.z;

        //斜边长度
        float hypotenuse = UnityEngine.Mathf.Sqrt(UnityEngine.Mathf.Pow(x, 2f) + UnityEngine.Mathf.Pow(z, 2f));

        //求出弧度
        float cos = x / hypotenuse;
        float radian = UnityEngine.Mathf.Acos(cos);

        //用弧度算出角度    
        float angle = 180 / (UnityEngine.Mathf.PI / radian);

        if (z < 0)
        {
            angle = -angle;
        }
        else if ((z == 0) && (x < 0))
        {
            angle = 180;
        }
        return angle;
    }

    static int KeepRange(KCharacter pCharacter, float fMinRange, float fMaxRange)
    {
        int nResult = 0;
        int nRetCode = 0;

        float fCurrentDistance = 0f;
        float fKeepRange = 0f;
        float fFarRange = 0f;
        float fMoveDistance = 0f;
        float fAngle = 0f;
        float fSelfDstX = 0f;
        float fSelfDstZ = 0f;

        KCharacter pTarget = null;


        if (pCharacter.m_pSelectTarget.GetTargetType() != TARGET_TYPE.ttNpc && pCharacter.m_pSelectTarget.GetTargetType() != TARGET_TYPE.ttPlayer)
        {
            goto Exit0;
        }

        nRetCode = pCharacter.m_pSelectTarget.GetTarget(ref pTarget);
        if (nRetCode == 0 && pTarget == null) { goto Exit0; }

        // 自己和自己不用保持距离
        if ((pCharacter.m_dwID != pTarget.m_dwID) == false) { return 1; }

        UnityEngine.Vector3 pos = pCharacter.m_pEntity.GetPos();
        UnityEngine.Vector3 targetPos = pTarget.m_pEntity.GetPos();

        fCurrentDistance = KAI_SEARCH_CHARACTER.s_GetDistance2(pos.x, pos.z, targetPos.x, targetPos.z);

        fKeepRange = (pCharacter.m_fTouchRange + pTarget.m_fTouchRange);
        fFarRange = (fMaxRange + pCharacter.m_fTouchRange + pTarget.m_fTouchRange);

        fKeepRange = Math.Max(fKeepRange, fMinRange);
        fFarRange = Math.Max(fFarRange, fKeepRange);


        if ((fCurrentDistance <= fKeepRange || fCurrentDistance >= fFarRange) == false) { return 1; }


        if (fCurrentDistance <= fKeepRange)
        {
        }
        else if (fCurrentDistance > fFarRange)
        {
            fMoveDistance = fCurrentDistance - (fFarRange + fKeepRange);
            
            //fAngle = angle_360(ref pos, ref targetPos);
            //fAngle = fAngle * 180f / 3.141592f;
        }

        if ((fMoveDistance > 0.5f) == false) { return 1; }

        // 需要移动 * UnityEngine.Mathf.Deg2Rad
        //fSelfDstX = pos.x + fMoveDistance * UnityEngine.Mathf.Cos(fAngle);
        //fSelfDstZ = pos.z + fMoveDistance * UnityEngine.Mathf.Sin(fAngle);
        
        fSelfDstX = targetPos.x;
        fSelfDstZ = targetPos.z;

        pCharacter.RunTo(fSelfDstX, fSelfDstZ);

        nResult = 2;
    Exit0:
        return nResult;
    }
    public static int AIKeepRange(KCharacter pCharacter, KAIAction pActionData)
    {
        int nResult = 0;
        int nRetCode = 0;
        int nBranchInRange = 1;
        int nBranchKeep = 2;
        int nMinRange = pActionData.m_nParam[0];
        int nMaxRange = pActionData.m_nParam[1];

        nRetCode = KeepRange(pCharacter, (float)nMinRange, (float)nMaxRange);
        if (!(nRetCode > 0)) { goto Exit0; }

        if (nRetCode == 1)
        {
            nResult = nBranchInRange;
            goto Exit0;
        }

        nResult = nBranchKeep;
    Exit0:
        return nResult;
    }
    public static int AIIsTargetExist(KCharacter pCharacter, KAIAction pActionData)
    {
        int nResult = 0;
        int nBranchExist = 1;
        int nBranchNotExist = 2;

        KCharacter pTarget = null;

        pCharacter.m_pSelectTarget.GetTarget(ref pTarget);
        if (pTarget == null)
        {
            return nBranchNotExist;
        }

        if ((pTarget.m_eMoveState != CHARACTER_MOVE_STATE.cmsOnDeath) == false)
        {
            return nBranchNotExist;
        }

        nResult = nBranchExist;
    Exit0:
        return nResult;

    }
    public static int AISearchEnemy(KCharacter pCharacter, KAIAction pActionData)
    {
        int nResult = 0;
        int nRetCode = 0;

        int nBranchFound = 1;
        int nBranchNotFound = 2;
        int nRange = pActionData.m_nParam[0];
        TARGET_TYPE eTargetType = TARGET_TYPE.ttInvalid;

        KSearchForAnyCharacter Tactic = new KSearchForAnyCharacter();
        Tactic.m_pSelf = pCharacter;
        Tactic.m_fDistance2 = (float)nRange;
        Tactic.m_nRelation = (int)SCENE_OBJ_RELATION_TYPE.sortEnemy;

        KAI_SEARCH_CHARACTER.AISearchCharacter<KSearchForAnyCharacter>(Tactic);
        if (Tactic.m_pResult == null)
        {
            return nBranchNotFound;
        }

        if (KCharacter.IS_PLAYER(Tactic.m_pResult.m_dwID))
        {
            eTargetType = TARGET_TYPE.ttPlayer;
        }
        else
        {
            eTargetType = TARGET_TYPE.ttNpc;
        }

        nRetCode = pCharacter.SelectTarget(eTargetType, Tactic.m_pResult.m_dwID);
        if (nRetCode == 0)
        {
            goto Exit0;
        }

        nResult = nBranchFound;
    Exit0:
        return nResult;
    }


    public static int AIAddTargetToThreatList(KCharacter pCharacter, KAIAction pActionData)
    {
        int nResult = 0;
        int nBranchSuccess = 1;


        nResult = nBranchSuccess;
    Exit0:
        return nResult;
    }

    public static int CastSkill(KCharacter pCharacter, uint dwSkillID, uint dwSkillLevel, int nAISkillType)
    {
        int nResult = 0;
        int nRetCode = 0;
        KCharacter pTarget = null;
        KSkill pSkill = null;

        if (!(pCharacter.m_eMoveState < CHARACTER_MOVE_STATE.cmsOnDeath)) { return 2; }

        pCharacter.m_pSkillTarget = pCharacter.m_pSelectTarget;

        if (pCharacter.m_pSkillTarget.GetTargetType() == TARGET_TYPE.ttNpc || pCharacter.m_pSkillTarget.GetTargetType() == TARGET_TYPE.ttPlayer)
        {
            nRetCode = pCharacter.m_pSkillTarget.GetTarget(ref pTarget);
            if (nRetCode == 0 || pTarget == null)
            {
                goto Exit0;
            }
        }


        pSkill = FirstFightMgr.Instance().m_SkillManager.GetSkill(dwSkillID);
        if (pSkill == null) { goto Exit0; }

        nRetCode = (int)pSkill.CanCast(pCharacter, ref pCharacter.m_pSkillTarget);
        switch (nRetCode)
        {
            case (int)SKILL_RESULT_CODE.srcInvalidTarget:
            case (int)SKILL_RESULT_CODE.srcTooCloseTarget:
            case (int)SKILL_RESULT_CODE.srcTooFarTarget:
                break;
        }
        if (nRetCode != (int)SKILL_RESULT_CODE.srcSuccess) { return 2; }

        nRetCode = (int)pCharacter.CastSkill(dwSkillID, ref pCharacter.m_pSkillTarget);
        if (nRetCode != (int)SKILL_RESULT_CODE.srcSuccess) { return 2; }

        nResult = 1;
    Exit0:
        return nResult;
    }

    public static int AICastSkill(KCharacter pCharacter, KAIAction pActionData)
    {
        int nResult = 0;
        int nRetCode = 0;
        int nBranchSuccess = 1;
        int nBranchFailed = 2;
        uint dwSkillID = (uint)pActionData.m_nParam[0];
        uint dwSkillLevel = (uint)pActionData.m_nParam[1];

        nRetCode = CastSkill(pCharacter, dwSkillID, dwSkillLevel, (int)AI_SKILL_TYPE.aistInvalid);
        if (nRetCode <= 0)
        {
            goto Exit0;
        }

        if (nRetCode == 2)
        {
            nResult = nBranchFailed;
            goto Exit0;
        }

        nResult = nBranchSuccess;
    Exit0:
        return nResult;
    }

    public static int AIStand(KCharacter pCharacter, KAIAction pActionData)
    {
        int nBranchSuccess = 1;

        pCharacter.Stand();

        return nBranchSuccess;
    }


}