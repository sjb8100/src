using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using Client;
using Engine.Utility;
public class SkillCaster
{
    //技能效果的播放实例
    //SkillEffectInst m_skillEffectInst = new SkillEffectInst();

    public SkillEffect EffectNode
    {
        get
        {
            return m_SkillEffect;
        }
    }
    SkillEffect m_SkillEffect = null;

    bool m_casting = false;
    /// <summary>
    /// 正在播放中
    /// </summary>
    public bool IsCasting
    {
        get { return m_casting; }
    }

    //施法者
    ISkillAttacker m_attacker;

    //当前的动作状态
    AnimationState m_animState;
    PlayerSkillPart playerSkill = null;
    public void Init(ISkillAttacker attacker)
    {
        m_attacker = attacker;
    }
    public void InitCastSkill(uint skill_id, ISkillPart skillPart, uint attackActionID)
    {
        if (skillPart.GetSkillPartType() == SkillPartType.SKILL_PLAYERPART)
        {
            playerSkill = skillPart as PlayerSkillPart;
            SkillEffectProp skillEffectProp;
            if (playerSkill.SkillEffectDic.TryGetValue((int)skill_id, out skillEffectProp) == false)
            {
                skillEffectProp = playerSkill.RegisterSkill(skill_id);
                if (skillEffectProp == null)
                {
                    Log.LogGroup("ZDY", "没有注册技能配置 {0}", skill_id.ToString());
                }
                return;
            }

            playerSkill.NextSkillID = 0;
            playerSkill.CurSkillID = skill_id;
            playerSkill.AttackAnimState = null;
            playerSkill.SetFighting(true);

            SkillEffect skillEffect = CreateSkillEffect(skillEffectProp);
            Cast(skillEffect, skill_id);
        }
    }
    //开始播放技能效果
    public void Cast(SkillEffect skillEffect, uint attackActionID)
    {
        if (skillEffect == null)
        {
            Log.LogGroup("ZDY", "技能特效为空");
            return;
        }

        m_SkillEffect = skillEffect;

        m_casting = true;
        m_animState = null;
        skillEffect.Play(m_attacker);

        // SkillEffectManager.Helper.PlaceFxRoot.DetachChildren();
    }

    //技能动作播放结束了？
    public bool IsAnimFinished()
    {
        if (playerSkill == null)
        {
            return true;
        }
        return playerSkill.IsAttackStateEnd;

    }
    public AnimationState GetAniState()
    {
        return m_animState;
    }
    public float GetAniTotalTime()
    {
        if (m_animState)
        {
            return m_animState.length;
        }
        return 0;
    }
    public float GetAniCurTime()
    {
        if (m_animState)
        {
            return m_animState.time;
        }
        return 0;
    }
    public void Stop()
    {
        //m_skillEffectInst.m_Attacker = null;
        //if(m_SkillEffect!=null)
        //{
        //    m_SkillEffect.Stop();
        //    m_casting = true;
        //}
        StopAllEffect();
        m_casting = true;
    }
    public void Dead()
    {
        StopAllEffect();
        DestoryAllEffect();
        m_casting = true;
        //if (m_SkillEffect != null)
        //{
        //    m_casting = true;
        //    m_SkillEffect.Dead();
        //    m_SkillEffect.Destroy();
        //}
    }
    public void SetAniState(AnimationState state)
    {
        if (state != null)
        {
            m_animState = state;
        }
    }
    //public void Update(float dt)
    //{
    //    m_animState = m_attacker.AttackAnimState;
    //    SkillEffectManager.Instance.Update(dt);
    //    //更新技能效果的释放时间
    //    //if (m_SkillEffect!=null)
    //    //{
    //    //    m_casting = m_SkillEffect.Update(Time.deltaTime);
    //    //    m_animState = m_attacker.AttackAnimState;
    //    //    //if(m_animState == null)
    //    //    //{
    //    //    //    Log.Error( "m_animState is null" );
    //    //    //}
    //    //}

    //}
    #region effectmanager
    // 运行时技能特效
    List<SkillEffect> m_runSkillEffect = new List<SkillEffect>();

   // List<SkillEffect> m_IdleSkillEffect = new List<SkillEffect>();
    public SkillEffect CreateSkillEffect(SkillEffectProp prop)
    {
        SkillEffect skillEffect = null;

        skillEffect = new SkillEffect();
        AddRunSkillEffect(skillEffect);

        m_SkillEffect = skillEffect;
        skillEffect.Create(prop);

        return skillEffect;
    }
    public void AddRunSkillEffect(SkillEffect eff)
    {
        if(!m_runSkillEffect.Contains(eff))
        {
            m_runSkillEffect.Add(eff);
        }
    }
    public void Update(float dt)
    {
        m_animState = m_attacker.AttackAnimState;
        if (m_SkillEffect != null)
        {
            m_casting = !m_SkillEffect.IsIdle();
        }
        if (m_runSkillEffect.Count > 0)
        {
            for (int i = m_runSkillEffect.Count - 1; i >= 0; i--)
            {
                SkillEffect ef = m_runSkillEffect[i];
                if (ef.IsIdle())
                {
                    RemoveSkillEffect(ef);
                }
            }
        }
        for (int i = 0; i < m_runSkillEffect.Count; i++)
        {
            SkillEffect ef = m_runSkillEffect[i];
            ef.Update(dt);
        }
    

    }
    public void StopAllEffect()
    {
        for (int i = 0; i < m_runSkillEffect.Count; i++)
        {
            SkillEffect ef = m_runSkillEffect[i];
            ef.Stop();
        }
    }
    public void DeadAllEffect()
    {
        for (int i = 0; i < m_runSkillEffect.Count; i++)
        {
            SkillEffect ef = m_runSkillEffect[i];
            ef.Dead();
        }
    }
    public void DestoryAllEffect()
    {
        for (int i = 0; i < m_runSkillEffect.Count; i++)
        {
            SkillEffect ef = m_runSkillEffect[i];
            ef.Destroy();
        }
    }
    public void RemoveSkillEffect(SkillEffect ef)
    {
        if (ef != null)
        {
          //  ef.FreeAllNode();
            m_runSkillEffect.Remove(ef);
            ef = null;

        }
    }
    #endregion
}

