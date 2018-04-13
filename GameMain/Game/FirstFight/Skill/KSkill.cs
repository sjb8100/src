using System;
using System.Collections.Generic;

public enum IN_RANGE_RESULT
{
    irrInvalid = 0,

    irrInRange,
    irrTooClose,
    irrTooFar,

    irrTotal
};

// 技能的释放方式
public enum KSKILL_CAST_MODE
{
    scmInvalid,
    scmSector,          // 扇形范围,扇心固定在自己
    scmCasterArea,      // 以自己为中心的圆形区域
    scmTargetArea,      // 以目标为中心的圆形区域
    scmCasterSingle,    // 对单体对象(限于自己)施放
    scmTargetSingle,    // 对单体对象(指定目标)施放
    scmTotal
};
public struct KSKILL_BASE_INFO
{
    public uint dwSkillID;
    public KSKILL_CAST_MODE nCastMode;
    public int nHitDelay;
};

public class KSkill
{
    static IN_RANGE_RESULT SkillInRange(float fSrcX, float fSrcZ, float fDstX, float fDstZ, float fMinRange, float fMaxRange)
    {
        IN_RANGE_RESULT nResult = IN_RANGE_RESULT.irrInvalid;

        float fDistanceXY = 0f;
        fDistanceXY = KAI_SEARCH_CHARACTER.s_GetDistance2(fSrcX, fSrcZ, fDstX, fDstZ);

        if ((fDistanceXY - 0.5f > fMaxRange))
        {
            return IN_RANGE_RESULT.irrTooFar;
        }
        if ((fDistanceXY < fMinRange))
        {
            return IN_RANGE_RESULT.irrTooClose;
        }

        nResult = IN_RANGE_RESULT.irrInRange;
    Exit0:
        return nResult;
    }

    public KSKILL_BASE_INFO m_pBaseInfo;
    public float m_fMinRadius = 0f;
    public float m_fMaxRadius = 0f;
    public float m_fBulletVelocity = 1f;
    public int Init()
    {
        return 1;
    }

    public void UnInit()
    {

    }

    public SKILL_RESULT_CODE CanCast(KCharacter pSrcCharacter, ref KTarget rTarget)
    {
        SKILL_RESULT_CODE nResult = SKILL_RESULT_CODE.srcFailed;
        SKILL_RESULT_CODE nRetCode = SKILL_RESULT_CODE.srcFailed;

        switch (m_pBaseInfo.nCastMode)
        {
            case KSKILL_CAST_MODE.scmCasterSingle: // 对单体对象(限于自己)施放

                break;
            case KSKILL_CAST_MODE.scmCasterArea: // 以自己为中心的圆形区域

                break;
            case KSKILL_CAST_MODE.scmTargetArea: // 以目标为中心的圆形区域
            case KSKILL_CAST_MODE.scmTargetSingle:// 对单体对象(指定目标)施放

                nRetCode = CheckTargetRange(pSrcCharacter, ref rTarget);
                if (nRetCode != SKILL_RESULT_CODE.srcSuccess) { return nRetCode; }

                break;

            case KSKILL_CAST_MODE.scmSector:// 扇形范围,扇心固定在自己




                break;

            default:
                goto Exit0;
                break;
        }


        nResult = SKILL_RESULT_CODE.srcSuccess;
    Exit0:
        return nResult;

    }

    public SKILL_RESULT_CODE Cast(KCharacter pDisplayCaster, KCharacter pLogicCaster, ref  KTarget rTarget)
    {
        SKILL_RESULT_CODE nResult = SKILL_RESULT_CODE.srcFailed;
        int nRetCode = 0;
        KCharacter pTarget = null;

        if (pDisplayCaster == null)
        {
            goto Exit0;
        }

        if (pLogicCaster == null)
        {
            goto Exit0;
        }
        //...

        switch (rTarget.GetTargetType())
        {
            case TARGET_TYPE.ttPlayer:
            case TARGET_TYPE.ttNpc:

                nRetCode = rTarget.GetTarget(ref pTarget);
                if (nRetCode == 0) { goto Exit0; }

                break;
            default:
                break;
        }

        switch (m_pBaseInfo.nCastMode)
        {
            case KSKILL_CAST_MODE.scmSector:// 扇形范围,扇心固定在自己
                nRetCode = (int)CastOnSector(pDisplayCaster, pLogicCaster);
                break;

            case KSKILL_CAST_MODE.scmCasterArea:// 以自己为中心的圆形区域
                nRetCode = (int)CastOnCasterArea(pDisplayCaster, pLogicCaster);
                break;

            case KSKILL_CAST_MODE.scmTargetSingle: // 对单体对象(指定目标)施放
                nRetCode = (int)CastOnTargetSingle(pDisplayCaster, pLogicCaster, ref rTarget);
                break;

            case KSKILL_CAST_MODE.scmCasterSingle:// 对单体对象(限于自己)施放
                nRetCode = (int)CastOnCasterSingle(pDisplayCaster, pLogicCaster);
                break;

            case KSKILL_CAST_MODE.scmTargetArea:// 以目标为中心的圆形区域
                nRetCode = (int)CastOnTargetArea(pDisplayCaster, pLogicCaster, ref rTarget);
                break;

            default:
                goto Exit0;
                break;
        }

        if (nRetCode != (int)SKILL_RESULT_CODE.srcSuccess) { return (SKILL_RESULT_CODE)nRetCode; }


        nResult = SKILL_RESULT_CODE.srcSuccess;
    Exit0:
        return nResult;
    }

    SKILL_RESULT_CODE CastOnSector(KCharacter pDisplayCaster, KCharacter pLogicCaster)
    {
        SKILL_RESULT_CODE nResult = SKILL_RESULT_CODE.srcFailed;
        int nRetCode = 0;

        KSKILL_BULLET pBullet = new KSKILL_BULLET();
        KTarget pTarget = new KTarget();

        nRetCode = pTarget.SetTarget(pDisplayCaster);
        if (nRetCode == 0) { return SKILL_RESULT_CODE.srcFailed; }

        nRetCode = SetupBullet(pBullet, pLogicCaster, ref pTarget);
        if (nRetCode == 0) { goto Exit0; }
        
        nRetCode = (int)ApplyOnSector(pBullet);
        if (nRetCode != (int)SKILL_RESULT_CODE.srcSuccess)
        {
            return (SKILL_RESULT_CODE)nRetCode;
        }

        nResult = SKILL_RESULT_CODE.srcSuccess;
    Exit0:
        return nResult;
    }

    SKILL_RESULT_CODE CastOnCasterArea(KCharacter pDisplayCaster, KCharacter pLogicCaster)
    {
        SKILL_RESULT_CODE nResult = SKILL_RESULT_CODE.srcFailed;
        int nRetCode = 0;
        KTarget pTarget = new KTarget();
        KSKILL_BULLET pBullet = new KSKILL_BULLET();
        float fX = 0f;
        float fZ = 0f;

        nRetCode = pTarget.SetTarget(pDisplayCaster);
        if (nRetCode == 0) { goto Exit0; }

        nRetCode = SetupBullet(pBullet, pLogicCaster, ref pTarget);
        if (nRetCode == 0) { goto Exit0; }

        if (pBullet.nEndFrame > FirstFightMgr.Instance().m_nGameLoop)
        {
            pLogicCaster.AddBullet(pBullet);
        }
        else
        {
            pDisplayCaster.GetAbsoluteCoordinate(ref fX, ref fZ);
            nRetCode = (int)ApplyOnArea(pBullet, fX, fZ);
        }

        nResult = SKILL_RESULT_CODE.srcSuccess;
    Exit0:
        return nResult;
    }

    SKILL_RESULT_CODE CastOnTargetSingle(KCharacter pDisplayCaster, KCharacter pLogicCaster, ref KTarget rTarget)
    {
        SKILL_RESULT_CODE nResult = SKILL_RESULT_CODE.srcFailed;
        int nRetCode = 0;

        KSKILL_BULLET pBullet = new KSKILL_BULLET();

        nRetCode = SetupBullet(pBullet, pLogicCaster, ref rTarget);
        if (nRetCode == 0) { goto Exit0; }

        if (pBullet.nEndFrame > FirstFightMgr.Instance().m_nGameLoop)
        {
            pLogicCaster.AddBullet(pBullet);
        }
        else
        {
            ApplyOnSingle(pBullet);
        }


        nResult = SKILL_RESULT_CODE.srcSuccess;
    Exit0:
        return nResult;

    }

    SKILL_RESULT_CODE CastOnCasterSingle(KCharacter pDisplayCaster, KCharacter pLogicCaster)
    {
        SKILL_RESULT_CODE nResult = SKILL_RESULT_CODE.srcFailed;
        int nRetCode = 0;

        KSKILL_BULLET pBullet = new KSKILL_BULLET();
        KTarget pTarget = new KTarget();

        nRetCode = pTarget.SetTarget(pDisplayCaster);
        if (nRetCode == 0) { goto Exit0; }

        nRetCode = SetupBullet(pBullet, pLogicCaster, ref pTarget);
        if (nRetCode == 0) { goto Exit0; }

        if (pBullet.nEndFrame > FirstFightMgr.Instance().m_nGameLoop)
        {
            pLogicCaster.AddBullet(pBullet);
        }
        else
        {
            ApplyOnSingle(pBullet);
        }


        nResult = SKILL_RESULT_CODE.srcSuccess;
    Exit0:
        return nResult;
    }

    //以目标为中心的圆形区域
    SKILL_RESULT_CODE CastOnTargetArea(KCharacter pDisplayCaster, KCharacter pLogicCaster, ref KTarget rTarget)
    {
        SKILL_RESULT_CODE nResult = SKILL_RESULT_CODE.srcFailed;
        int nRetCode = 0;
        KSKILL_BULLET pBullet = new KSKILL_BULLET();

        float fTargetX = 0f;
        float fTargetZ = 0f;

        nRetCode = SetupBullet(pBullet, pLogicCaster, ref rTarget);
        if (nRetCode == 0) { goto Exit0; }


        if (pBullet.nEndFrame > FirstFightMgr.Instance().m_nGameLoop)
        {
            pLogicCaster.AddBullet(pBullet);
        }
        else
        {
            nRetCode = rTarget.GetTarget(ref fTargetX, ref fTargetZ);
            if (nRetCode == 0) { goto Exit0; }

            nRetCode = (int)ApplyOnSingle(pBullet);
            if (nRetCode != (int)SKILL_RESULT_CODE.srcSuccess) { return (SKILL_RESULT_CODE)nRetCode; }

            nRetCode = (int)ApplyOnArea(pBullet, fTargetX, fTargetZ);
            if (nRetCode != (int)SKILL_RESULT_CODE.srcSuccess) { return (SKILL_RESULT_CODE)nRetCode; }
        }


        nResult = SKILL_RESULT_CODE.srcSuccess;
    Exit0:
        return nResult;
    }

    int SetupBullet(KSKILL_BULLET pBullet, KCharacter pCaster, ref KTarget rTarget)
    {
        int nResult = 0;
        int nRetCode = 0;
        float fCasterX = 0f;
        float fCasterZ = 0f;
        float fTargetX = 0f;
        float fTargetZ = 0f;
        KCharacter pTarget = null;
        float fTouchRange = 0f;
        float fDistance = 0f;
        float fHitDelay = 0f;

        if (pBullet == null) { goto Exit0; }
        if (pCaster == null) { goto Exit0; }
        if (rTarget == null) { goto Exit0; }

        pBullet.dwBulletID = FirstFightMgr.Instance().m_SkillManager.m_dwBulletIDIndex++;
        pBullet.pTarget = rTarget;
        pBullet.pSkillSrc = pCaster;
        pBullet.dwSkillSrcID = pCaster.m_dwID;
        pBullet.pSkillPointer = this;
        pBullet.pNext = null;

        switch (m_pBaseInfo.nCastMode)
        {
            case KSKILL_CAST_MODE.scmTargetSingle:
                break;
            default:
                break;
        }

        pCaster.GetAbsoluteCoordinate(ref fCasterX, ref fCasterZ);

        rTarget.GetTarget(ref fTargetX, ref fTargetZ);

        nRetCode = rTarget.GetTarget(ref pTarget);
        if (pTarget != null)
        {
            fTouchRange = pTarget.m_fTouchRange;
        }

        fDistance = KAI_SEARCH_CHARACTER.s_GetDistance2(fCasterX, fCasterZ, fTargetX, fTargetZ);
        if (fDistance > fTouchRange)
        {
            fDistance -= fTouchRange;
        }

        if (m_fBulletVelocity > 0f)
        {
            fHitDelay = (fDistance / m_fBulletVelocity);
        }

        int nHitDelay = ((int)(fHitDelay * 16)) + (int)FirstFightMgr.Instance().m_nGameLoop + m_pBaseInfo.nHitDelay;

        pBullet.nEndFrame = nHitDelay;

        nResult = 1;
    Exit0:
        return nResult;
    }

    public SKILL_RESULT_CODE CheckTargetRange(KCharacter pCaster, ref KTarget rTarget)
    {
        SKILL_RESULT_CODE nResult = SKILL_RESULT_CODE.srcFailed;
        int nRetCode = 0;

        float fMaxRange = 0f;
        float fCasterX = 0f;
        float fCasterZ = 0f;
        float fTargetX = 0f;
        float fTargetZ = 0f;
        float fTouchRange = 0f;

        pCaster.GetAbsoluteCoordinate(ref fCasterX, ref fCasterZ);

        nRetCode = rTarget.GetTarget(ref fTargetX, ref fTargetZ);
        if (nRetCode == 0) { goto Exit0; }

        fMaxRange = pCaster.m_fTouchRange + m_fMaxRadius + fTouchRange;

        nRetCode = (int)SkillInRange(fCasterX, fCasterZ, fTargetX, fTargetZ, m_fMinRadius, fMaxRange);
        if (nRetCode == (int)IN_RANGE_RESULT.irrTooClose) { return SKILL_RESULT_CODE.srcTooCloseTarget; }
        if (nRetCode == (int)IN_RANGE_RESULT.irrTooFar) { return SKILL_RESULT_CODE.srcTooFarTarget; }
        if (nRetCode != (int)IN_RANGE_RESULT.irrInRange) { goto Exit0; }

        nResult = SKILL_RESULT_CODE.srcSuccess;
    Exit0:
        return nResult;
    }

    SKILL_RESULT_CODE ApplyOnSector(KSKILL_BULLET pBullet)
    {
        SKILL_RESULT_CODE nResult = SKILL_RESULT_CODE.srcFailed;
        int nRetCode = 0;
        



        nResult = SKILL_RESULT_CODE.srcSuccess;
    Exit0:
        return nResult;
    }

    public SKILL_RESULT_CODE ApplyOnSingle(KSKILL_BULLET pBullet)
    {
        SKILL_RESULT_CODE nResult = SKILL_RESULT_CODE.srcFailed;
        int nRetCode = 0;
        TARGET_TYPE eTargetType = TARGET_TYPE.ttInvalid;

        KCharacter pCharacter = null;
        eTargetType = pBullet.pTarget.GetTargetType();

        switch (eTargetType)
        {
            case TARGET_TYPE.ttNoTarget:
                break;
            case TARGET_TYPE.ttCoordination:
                break;
            case TARGET_TYPE.ttNpc:
            case TARGET_TYPE.ttPlayer:
                nRetCode = pBullet.pTarget.GetTarget(ref pCharacter);
                if (nRetCode == 0 || pCharacter == null) { goto Exit0; }

                pCharacter.ApplyBullet(pBullet);

                break;
            default:
                goto Exit0;
                break;

        }

        nResult = SKILL_RESULT_CODE.srcSuccess;
    Exit0:
        return nResult;

    }

    SKILL_RESULT_CODE ApplyOnArea(KSKILL_BULLET pBullet, float fX, float fZ)
    {
        SKILL_RESULT_CODE nResult = SKILL_RESULT_CODE.srcFailed;
        KCharacter pCaster = null;
        KSkill pSkill = null;
        KScene pScene = null;

        pSkill = pBullet.pSkillPointer;
        if (pSkill == null) { goto Exit0; }

        pCaster = pBullet.pSkillSrc;
        if (pCaster == null) { goto Exit0; }

        pScene = pCaster.m_pScene;
        if (pScene == null) { goto Exit0; }

        KSkillAreaTravFunc AreaTravFunc = new KSkillAreaTravFunc();
        AreaTravFunc.pBullet = pBullet;
        AreaTravFunc.nLeftCount = 3;

        if (m_pBaseInfo.nCastMode == KSKILL_CAST_MODE.scmTargetArea)
        {
            KCharacter pObject = null;
            pBullet.pTarget.GetTarget(ref pObject);
            if (pObject == null) { goto Exit0; }

            AreaTravFunc.bTargetArea = true;
            AreaTravFunc.nTargetType = (int)pBullet.pTarget.GetTargetType();
            AreaTravFunc.dwTargetID = pObject.m_dwID;
            AreaTravFunc.nLeftCount -= 1;
        }
        else
        {
            AreaTravFunc.bTargetArea = false;
            AreaTravFunc.nTargetType = (int)TARGET_TYPE.ttInvalid;
            AreaTravFunc.dwTargetID = 0;
        }

        pScene.TraverseRangePlayer<KSkillAreaTravFunc>(ref AreaTravFunc);
        pScene.TraverseRangeNpc<KSkillAreaTravFunc>(ref AreaTravFunc);


        nResult = SKILL_RESULT_CODE.srcSuccess;
    Exit0:
        return nResult;
    }



}

