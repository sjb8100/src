using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using Client;
using Engine;
namespace SkillSystem
{
    //跟踪发射特效
    public class FollowFxNode : HitNode
    {
        FollowFxHandle m_Handler = null;
        SkillEffect m_ef = null;
        public FollowFxNode()
        {
            is_hit_node = true;
           // m_NodeProp = ScriptableObject.CreateInstance<FollowFxNodeProp>();// new FollowFxNodeProp();
        }
        public override void FreeToNodePool()
        {
            if (m_ef != null)
            {
                m_ef.FreeSkillNode(this);
            }
        }
        public override void Play(ISkillAttacker attacker, SkillEffect se)
        {
            m_ef = se;
            GameObject go = attacker.GetTargetGameObject();
            if (go == null)
            {
                return;
            }

            FollowFxNodeProp prop = m_NodeProp as FollowFxNodeProp;
            if (prop == null)
            {
                return;
            }

            skill_id = se.CurSkillID;
            m_uDamageID = attacker.GetDamageID();
            Transform attacker_node = attacker.GetRoot();
            if (attacker_node == null)
            {
                return;
            }
            //GameObject fxObj = SkillEffectHelper.Instance.GetFollowGameObject();
            //if (fxObj == null)
            //{
            //    Engine.Utility.Log.Error("============================fxobj is null");
            //    return;
            //}

            //fxObj.transform.parent = SkillEffectManager.Helper.PlaceFxRoot;
            //EffectUtil.ResetLocalTransform(fxObj.transform);

            //fxObj.transform.localEulerAngles = attacker_node.eulerAngles;
            //fxObj.transform.position = attacker_node.position;
            //fxObj.transform.Translate(prop.offset_pos);



            int level = 0;
            if (attacker != null)
            {
                if (attacker.GetSkillPart() != null)
                {
                    level = attacker.GetSkillPart().FxLevel;
                }
            }

            uint effectid = helper.ReqPlayFx(prop.fx_name, helper.PlaceFxRoot, Vector3.zero, Vector3.one, level);
            IEffect ef = GetEffectByID(effectid);
            if (ef != null)
            {
                if (ef.GetNodeTransForm() == null)
                {
                    return;
                }
                GameObject fxObj = ef.GetNodeTransForm().gameObject;
                if(fxObj == null)
                {
                    return;
                }
                fxObj.transform.localEulerAngles = attacker_node.eulerAngles;
                fxObj.transform.position = attacker_node.position;
                fxObj.transform.Translate(prop.offset_pos);
                m_Handler = fxObj.GetComponent<FollowFxHandle>();
                if (m_Handler == null)
                {
                    m_Handler = fxObj.AddComponent<FollowFxHandle>();
                }
                m_Handler.m_attacker = attacker;
                m_Handler.m_speed = prop.speed;
                m_Handler.targetHitNode = prop.target_hitnode;
                m_Handler.m_acce = prop.acce;
                m_Handler.m_len = prop.len;
                m_Handler.m_hitIndex = hit_index;
                m_Handler.m_hitNode = this;
                m_Handler.m_effectid = effectid;
                m_Handler.InitFollowFx();
            }
            //  fxObj.name = m_Handler.m_effectid.ToString();
        }
        IEffect GetEffectByID(uint id)
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return null;
            }

            Engine.IEffect effect = rs.GetEffect(id);
            if (effect != null)
            {
                return effect;
            }
            return null;
        }
        public override void Stop()
        {
            if (m_Handler != null)
            {//飞行特效 碰撞后自己销毁
                //m_Handler.Destroy();
            }
        }
        public override void Dead()
        {
          
        }
        public override void Update(float dTime) { }

    }
}