using System;
using System.Collections.Generic;

public class KAI_SEARCH_CHARACTER
{
    public static float s_GetDistance2(float fSourceX, float fSourceY, float fDestX, float fDestY)
    {
        float x1 = fDestX - fSourceX;
        float y1 = fDestY - fSourceY;
        float temp = x1 * x1 + y1 * y1;
        float dis = UnityEngine.Mathf.Sqrt(temp);
        return dis;
    }

    public static bool SearchCharacter(ref KCharacter ppResult, KCharacter pCharacter, KCharacter pSelf, float fSearchDistance2, int nSearchRelation)
    {
        bool bResult = false;
        float fDistance2 = 0;
        int nRelation = 0;

        if (pCharacter == null)
        {
            //error
            goto Exit0;
        }

        if (pSelf == null)
        {
            //error
            goto Exit0;
        }


        if (pCharacter == pSelf) { goto Exit0; }
        if (pCharacter.m_eMoveState == CHARACTER_MOVE_STATE.cmsOnDeath) { goto Exit0; }

        //战斗关系判断
        nRelation = KCharacter.s_GetRelation(pSelf, pCharacter);
        if ((nRelation & nSearchRelation) == 0)
        {
            //goto Exit0;
        }

        if (pSelf.m_pEntity == null)
        {
            goto Exit0;
        }
        if (pCharacter.m_pEntity == null)
        {
            goto Exit0;
        }

        UnityEngine.Vector3 vSrc = pSelf.m_pEntity.GetPos();
        UnityEngine.Vector3 vDest = pCharacter.m_pEntity.GetPos();

        //判断距离
        fDistance2 = s_GetDistance2(vSrc.x, vSrc.z, vDest.x, vDest.z);

        // 不在搜索范围内
        if ((fDistance2 < fSearchDistance2) == false)
        {
            goto Exit0;
        }

        ppResult = pCharacter;

        bResult = true;
    Exit0:
        return bResult;

    }


    public static void AISearchCharacter<T>(T rTactic)
    {
        KScene pScene = null;

        KSearchTactic pCenterObj = rTactic as KSearchTactic;
        if (pCenterObj == null)
        {
            //error
            goto Exit0;
        }

        KSceneObject pSceneObject = (KSceneObject)pCenterObj.m_pSelf;
        if (pSceneObject == null)
        {
            //error
            goto Exit0;
        }


        pScene = pSceneObject.m_pScene;
        if (pScene == null)
        {
            //error
            goto Exit0;
        }

        // 暂时只有npc 
        pScene.TraverseRangeNpc(ref rTactic);
        pScene.TraverseRangePlayer(ref rTactic);




    Exit0:
        return;

    }
}


public class KSearchTactic
{
    public KCharacter m_pSelf = null;
    public virtual bool OperatorSearch(KCharacter pCharacter)
    {
        //error
        return false;
    }

};

//这个是只找一个就返回的版本
public class KSearchForAnyCharacter : KSearchTactic
{
    public KCharacter m_pResult = null;

    public float m_fDistance2 = 0f;
    public int m_dwForceID = 0;
    public int m_nRelation = 0;

    public override bool OperatorSearch(KCharacter pCharacter)
    {
        bool bRetCode = false;

        bRetCode = KAI_SEARCH_CHARACTER.SearchCharacter(ref this.m_pResult, pCharacter, m_pSelf, this.m_fDistance2, this.m_nRelation);

        if (bRetCode)
        {
            return false;
        }

        return true;
    }
}

public class KSkillAreaTravFunc : KSearchTactic
{
    public KSKILL_BULLET pBullet;
    public int nLeftCount;
    public bool bTargetArea;
    public int nTargetType;  // 用来记录TargetArea开始作用的目标类型
    public uint dwTargetID;   // 用来记录TargetArea开始作用的目标ID

    //public bool CanApply(KTarget rTarget)
    //{

    //    return true;
    //}

    public int CanApply(KCharacter pCharacter)
    {
        int nResult = 0;



        nResult = 1;
    Exit0:
        return nResult;
    }

    public override bool OperatorSearch(KCharacter pCharacter)
    {
        int nRetCode = 0;
        //KTarget Target = new KTarget();

        if (nLeftCount == 0)
        {
            return false;
        }

        if (!bTargetArea || dwTargetID != pCharacter.m_dwID)
        {
            nRetCode = CanApply(pCharacter);
            if (nRetCode == 0) { goto Exit0; }

            nLeftCount--;
            pCharacter.ApplyBullet(pBullet);
        }

    Exit0:
        return true;
    }

};