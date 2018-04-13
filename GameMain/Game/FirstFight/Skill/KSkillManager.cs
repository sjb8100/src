using System;
using System.Collections.Generic;

public class KSkillManager
{
    public uint m_dwBulletIDIndex = 1;
    private Dictionary<uint, KSkill> m_SkillList = new Dictionary<uint, KSkill>();

    public KSkillManager()
    {
        KSkill pSkill = new KSkill();
        //-------------------------------------------------------------------
        {
            // 技能1, 飞虫的技能
            uint uSkillID = 10005;


            pSkill.Init();
            pSkill.m_pBaseInfo.dwSkillID = uSkillID;
            pSkill.m_pBaseInfo.nCastMode = KSKILL_CAST_MODE.scmTargetSingle;// 对单体对象(指定目标)施放
            pSkill.m_fMaxRadius = 8f;
            pSkill.m_fBulletVelocity = 5f;
            pSkill.m_pBaseInfo.nHitDelay = 0;
            
            m_SkillList[uSkillID] = pSkill;


        }
        //-------------------------------------------------------------------
        {
            pSkill = new KSkill();

            uint uSkillID = 10001;


            pSkill.Init();
            pSkill.m_pBaseInfo.dwSkillID = uSkillID;
            pSkill.m_pBaseInfo.nCastMode = KSKILL_CAST_MODE.scmTargetSingle;// 对单体对象(指定目标)施放
            pSkill.m_fMaxRadius = 2f;
            pSkill.m_fBulletVelocity = 0f;
            pSkill.m_pBaseInfo.nHitDelay = 5;
            m_SkillList[uSkillID] = pSkill;

        }
        //-------------------------------------------------------------------
        {
            // 法师冰
            pSkill = new KSkill();

            uint uSkillID = 10131;

            pSkill.Init();
            pSkill.m_pBaseInfo.dwSkillID = uSkillID;
            pSkill.m_pBaseInfo.nCastMode = KSKILL_CAST_MODE.scmTargetSingle;// 对单体对象(指定目标)施放
            pSkill.m_fMaxRadius = 8f;
            pSkill.m_fBulletVelocity = 0f;
            pSkill.m_pBaseInfo.nHitDelay = 5;

            m_SkillList[uSkillID] = pSkill;
        }

        

    }


    public KSkill GetSkill(uint uSkillID)
    {
        KSkill pSkill = null;

        if (m_SkillList.TryGetValue(uSkillID, out pSkill))
        {
            return pSkill;
        }

        return pSkill;
    }

};

