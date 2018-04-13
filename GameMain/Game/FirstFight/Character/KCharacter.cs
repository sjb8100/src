using System;
using System.Collections.Generic;
using Client;

public enum SKILL_RESULT_CODE
{
    srcInvalid = 0,

    srcSuccess,             // 成功
    srcFailed,              // 失败

    srcSkillNotReady,		// 技能CD时间未到
    srcInvalidTarget,       // 无效的目标
    srcNoTarget,            // 没有目标
    srcTooCloseTarget,      // 目标太近
    srcTooFarTarget,		// 目标太远
    srcTotal
};

enum SCENE_OBJ_RELATION_TYPE
{
    sortInvalid = 0,

    sortNone = 1,
    sortSelf = 2,
    sortAlly = 4,
    sortEnemy = 8,
    sortNeutrality = 16,
    sortParty = 32,

    sortAll = sortNone | sortAlly | sortEnemy | sortSelf | sortNeutrality | sortParty,

    sortTotal,
};

public enum CHARACTER_MOVE_STATE
{
    cmsInvalid = 0,				// 无效状态

    cmsOnStand,

    cmsOnDeath, // 死亡



    cmsTotal,
};

public class KCharacter : KSceneObject
{
    public static bool IS_NPC(uint dwID)
    {
        bool bResult = ((dwID & 0x40000000) > 0);
        return bResult;
    }
    public static bool IS_PLAYER(uint dwID)
    {
        return (!IS_NPC(dwID));
    }
    public static int s_GetRelation(KCharacter pSrcCharacter, KCharacter pDstCharacter)
    {
        if (pSrcCharacter.m_dwForceID != pDstCharacter.m_dwForceID)
        {
            return 8;
        }
        return 0;
    }

    //-----------------------------------------------------------------------

    public IEntity m_pEntity = null;

    KSKILL_BULLET m_pBulletListHead = null;

    public KAIVM m_AIVM = new KAIVM();  /// AI

    public CHARACTER_MOVE_STATE m_eMoveState = CHARACTER_MOVE_STATE.cmsOnStand;
    public KTarget m_pSelectTarget = new KTarget();
    public KTarget m_pSkillTarget = new KTarget();


    public uint m_dwForceID = 0;// 势力

    public int m_nCurrentLife = 1000;				// 当前生命
    public int m_nMaxLife = 1000;					// 生命最大值    
    public bool m_bToDie = false;
    public KCharacter()
    {

    }

    public override int Init()
    {
        return 1;
    }

    public override void UnInit()
    {

    }

    public virtual int Activate()
    {
        if (m_eMoveState != CHARACTER_MOVE_STATE.cmsOnDeath)
        {
            m_AIVM.Active();
            if (m_pScene == null) { goto Exit0; }
        }

        CheckBullet();
        CheckDie();

    Exit0:
        return 1;
    }

    public void SetMoveState(CHARACTER_MOVE_STATE nState)
    {
        if (nState < CHARACTER_MOVE_STATE.cmsInvalid || nState > CHARACTER_MOVE_STATE.cmsTotal)
        {
            //error
        }
        switch (nState)
        {
            case CHARACTER_MOVE_STATE.cmsOnDeath:
                break;
            default:
                break;
        }
        m_eMoveState = nState;
    }

    public override void GetAbsoluteCoordinate(ref float pfX, ref float pfZ)
    {
        if (m_pEntity != null)
        {
            UnityEngine.Vector3 pos = m_pEntity.GetPos();
            pfX = pos.x;
            pfZ = pos.z;
        }
    }

    public int RunTo(float fDestX, float fDestY)
    {
        int nResult = 0;

        if (m_pEntity != null)
        {
            Move move = new Move();
            move.strRunAct = EntityAction.Run; // 动作名需要统一处理
            move.m_target = new UnityEngine.Vector3(fDestX, 0f, fDestY);
            move.m_speed = m_pEntity.GetProp((int)WorldObjProp.MoveSpeed) * EntityConst.MOVE_SPEED_RATE;
            m_pEntity.SendMessage(EntityMessage.EntityCommand_MoveTo, (object)move);
        }

        nResult = 1;
    Exit0:
        return nResult;

    }

    public int Stand()
    {
        int nResult = 0;

        if (m_pEntity != null)
        {
            UnityEngine.Vector3 pos = m_pEntity.GetPos();
            m_pEntity.SendMessage(EntityMessage.EntityCommand_StopMove, pos);
        }

        nResult = 1;
    Exit0:
        return nResult;
    }

    public SKILL_RESULT_CODE CastSkill(uint dwSkillID, ref KTarget pTarget)
    {
        int nRetCode = 0;
        SKILL_RESULT_CODE nResult = SKILL_RESULT_CODE.srcFailed;
        KCharacter pTargetCharacter = null;

        if (m_pEntity == null)
        {
            return SKILL_RESULT_CODE.srcFailed;
        }

        ISkillPart skillPart = m_pEntity.GetPart(EntityPart.Skill) as ISkillPart;
        if (skillPart == null)
        {
            return SKILL_RESULT_CODE.srcFailed;
        }

        if (pTarget == null)
        {
            return SKILL_RESULT_CODE.srcFailed;
        }

        nRetCode = pTarget.GetTarget(ref pTargetCharacter);
        if (nRetCode == 0 || pTargetCharacter == null)
        {
            return SKILL_RESULT_CODE.srcFailed;
        }

        IEntity targetEntity = pTargetCharacter.m_pEntity;
        if (targetEntity == null)
        {
            return SKILL_RESULT_CODE.srcFailed;
        }

        skillPart.ReqNpcUseSkill(targetEntity, dwSkillID);


        //
        KSkill pSkill = FirstFightMgr.Instance().m_SkillManager.GetSkill(dwSkillID);
        pSkill.Cast(this, this, ref  pTarget);




        //测试代码
        // 伤害飘子
        //FlyFontDataManager.Instance.ShowDamage(999, 1, pTargetCharacter.m_dwID, EntityType.EntityType_NPC, 1000000);
        //pTargetCharacter.Test_ConcludeResult();





        nResult = SKILL_RESULT_CODE.srcSuccess;
    Exit0:
        return nResult;
    }



    //测试代码
    //void Test_ConcludeResult()
    //{
    //    m_nCurrentLife -= 256;

    //    if ((m_eMoveState != CHARACTER_MOVE_STATE.cmsOnDeath) && (m_nCurrentLife <= 0))
    //    {
    //        Test_GoToHell();

    //    }
    //}

    //bool Test_GoToHell()
    //{
    //    bool bResult = false;

    //    m_nCurrentLife = 0;

    //    if ((!m_bToDie) == false)
    //    {
    //        goto Exit0;
    //    }

    //    m_bToDie = true;

    //    bResult = true;
    //Exit0:
    //    return bResult;

    //}

    public int SelectTarget(TARGET_TYPE eTargetType, uint dwTargetID)
    {
        int nResult = 0;
        int nRetCode = 0;

        nRetCode = m_pSelectTarget.SetTarget(eTargetType, dwTargetID);
        if (nRetCode == 0)
        {
            goto Exit0;
        }

        nResult = 1;
    Exit0:
        return nResult;
    }


    public void AddBullet(KSKILL_BULLET pBullet)
    {
        KSKILL_BULLET pPreNode = null;
        KSKILL_BULLET pNode = null;

        pNode = m_pBulletListHead;

        while (pNode != null)
        {
            if (pNode.nEndFrame > pBullet.nEndFrame)
            {
                break;
            }

            pPreNode = pNode;
            pNode = pNode.pNext;
        }

        if (pPreNode != null)
        {
            pBullet.pNext = pPreNode.pNext;
            pPreNode.pNext = pBullet;
        }
        else
        {
            pBullet.pNext = m_pBulletListHead;
            m_pBulletListHead = pBullet;
        }

    }

    public void CheckBullet()
    {
        KSKILL_BULLET pBullet = null;

        if (m_pBulletListHead == null) { goto Exit0; }

        if (m_pBulletListHead.nEndFrame > FirstFightMgr.Instance().m_nGameLoop)
        {
            goto Exit0;
        }

        pBullet = m_pBulletListHead;

        m_pBulletListHead = m_pBulletListHead.pNext;

        ProcessBullet(pBullet);

        pBullet = null;

    Exit0:
        return;
    }

    void ProcessBullet(KSKILL_BULLET pBullet)
    {
        KCharacter pCaster = null;
        KSkill pSkill = null;

        pCaster = pBullet.pSkillSrc;
        if (pCaster == null)
        {
            goto Exit0;
        }

        pSkill = pBullet.pSkillPointer;
        if (pSkill == null)
        {
            goto Exit0;
        }

        if ((pCaster.m_dwID == pBullet.dwSkillSrcID) == false) { goto Exit0; }

        switch (pSkill.m_pBaseInfo.nCastMode)
        {
            case KSKILL_CAST_MODE.scmTargetSingle:
                pSkill.ApplyOnSingle(pBullet);
                break;
            default:
                goto Exit0;
                break;
        }
    Exit0:
        return;
    }


    private bool CheckDie()
    {
        bool bResult = false;

        if (m_bToDie == false) { goto Exit0; }

        if (false)
        {

        }
        else
        {
            KNpc pNpc = (KNpc)this;
            pNpc.m_nCurrentLife = 0;

            pNpc.SetMoveState(CHARACTER_MOVE_STATE.cmsOnDeath);


            FirstFightMgr.Instance().DeleteNpc((KNpc)this);
        }

        bResult = true;
    Exit0:
        return bResult;
    }


    public int ApplyBullet(KSKILL_BULLET pBullet)
    {
        int nResult = 0;

        ConcludeResult(pBullet.dwBulletID, false);

        nResult = 1;
    Exit0:
        return nResult;
    }

    public int ConcludeResult(uint dwBulletID, bool bCriticalStrikeFlag)
    {
        int nResult = 0;

        int nDamageValue = 1;

        m_nCurrentLife -= nDamageValue;

        if (m_eMoveState != CHARACTER_MOVE_STATE.cmsOnDeath && m_nCurrentLife <= 0)
        {
            GoToHell();
        }

        if (m_dwID == 1)
        {
            FlyFontDataManager.Instance.ShowDamage((uint)nDamageValue, 1, m_dwID, EntityType.EntityType_Puppet, 1000000);
        }
        else
        {
            //伤害效果显示
            FlyFontDataManager.Instance.ShowDamage((uint)nDamageValue, 1, m_dwID, EntityType.EntityType_NPC, 1000000);

        }
        nResult = 1;
    Exit0:
        return nResult;

    }

    int GoToHell()
    {
        int nResult = 0;

        m_nCurrentLife = 0;

        if ((!m_bToDie) == false)
        {
            goto Exit0;
        }

        m_bToDie = true;

        nResult = 1;
    Exit0:
        return nResult;
    }
}

