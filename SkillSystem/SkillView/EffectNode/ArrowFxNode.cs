using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace SkillSystem
{
    //发射特效（法术球，箭等）
    public class ArrowFxNode : HitNode
    {
     

        ArrowFxHandle m_Handler = null;
        SkillEffect m_ef = null;
        public ArrowFxNode()
        {
            is_hit_node = true;
        }

 
        public override void Stop() 
        {
        
        }
        public override void Dead()
        {
            if (m_Handler != null)
            {
                m_Handler.DestroyFx();
            }
            FreeToNodePool();
        }
        public override void FreeToNodePool()
        {
            if(m_ef != null)
            {
                m_ef.FreeSkillNode(this);
            }
        }
        public override void Update(float dTime) { }
        public override void Play(ISkillAttacker attacker, SkillEffect se)
        {
            m_ef = se;
            GameObject fxObj = new GameObject();//helper.CreateFxObj(fx_name);
            if (fxObj == null)
            {
                //Log.Error("no fx: {0}", fx_name);
                return;
            }

            Transform attacker_root = attacker.GetRoot();

            //callback.OnTriggerHit();

            //ArrowFxHandle handler = Util.AddComponent(fxObj, "ArrowFxHandle") as ArrowFxHandle;
            m_Handler = fxObj.AddComponent<ArrowFxHandle>();

            ArrowFxNodeProp prop = m_NodeProp as ArrowFxNodeProp;
            if(prop==null)
            {
                return;
            }

            m_Handler.m_attacker = attacker;
            m_Handler.m_speed = prop.speed;
            m_Handler.m_acce = prop.acce;
            m_Handler.m_range = prop.range;
            m_Handler.m_radius = prop.radius;
            m_Handler.m_hitIndex = hit_index;
            m_Handler.m_hitNode = this;
            m_Handler.m_hitNum = prop.fx_num;

            //handler.transform.localPosition = attacker_obj.transform.position;

            Vector3 scale;
            scale.x = m_Handler.transform.localScale.x * attacker_root.localScale.x;
            scale.y = m_Handler.transform.localScale.y * attacker_root.localScale.y;
            scale.z = m_Handler.transform.localScale.z * attacker_root.localScale.z;

            m_Handler.transform.localScale = scale;

            //handler.transform.localPosition += handler.transform.forward * z_offset;
            //handler.transform.localRotation = caster.transform.rotation;


            //handler.transform.Rotate(Vector3.forward, pitch, Space.Self);

            if (prop.fx_num == 1)
            {
                m_Handler.transform.parent = SkillEffectManager.Helper.PlaceFxRoot;
                Vector3 angles = attacker_root.eulerAngles;
                angles.z = prop.pitch;
                m_Handler.transform.localEulerAngles = angles;
                m_Handler.transform.Translate(0, prop.height, prop.z_offset);

                return;
            }

            float start_angle = -prop.angle / 2;
            float per_angle = prop.angle / (prop.fx_num - 1);
            for (int i = 0; i < prop.fx_num; i++)
            {
                GameObject fxObjInst;

                if (i == 0)
                {
                    fxObjInst = fxObj;
                }
                else
                {
                    fxObjInst = new GameObject();//GameObject.Instantiate(fxObj) as GameObject;
                }

                m_Handler = fxObjInst.GetComponent<ArrowFxHandle>();
                m_Handler.m_attacker = attacker;
                m_Handler.m_speed = prop.speed;
                m_Handler.m_acce = prop.acce;
                m_Handler.m_range = prop.range;
                m_Handler.m_radius = prop.radius;
                m_Handler.m_hitIndex = hit_index;
                m_Handler.m_hitNode = this;
                m_Handler.m_hitNum = prop.fx_num;

                fxObjInst.transform.parent = SkillEffectManager.Helper.PlaceFxRoot;

                Vector3 euler = attacker_root.eulerAngles;
                euler.y += start_angle;
                euler.z = prop.pitch;

                fxObjInst.transform.localEulerAngles = euler;
                fxObjInst.transform.localScale = scale;
                fxObjInst.transform.localPosition = attacker_root.position;
                fxObjInst.transform.Translate(0, prop.height, prop.z_offset);

                start_angle += per_angle;
                int level = 0;
                if (attacker != null)
                {
                    if (attacker.GetSkillPart() != null)
                    {
                        level = attacker.GetSkillPart().FxLevel;
                    }
                }
                m_Handler.m_effectid = helper.ReqPlayFx(prop.fx_name, fxObjInst.transform, Vector3.zero, Vector3.one,level);
            }
        }
    }
}