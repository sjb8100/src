using System;
using System.Text;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine;

namespace SkillSystem
{
    //多次伤害节点
    public class MultHitNode : HitNode
    {
        //public int hit_times = 1;
        //public float delta_time = 0.5f;
        //public Vector3 position = Vector3.zero;
        ////public float radius = 2.0f;
        //public bool attach = false;

        public MultHitNode()
        {
            is_hit_node = true;
            //m_NodeProp = ScriptableObject.CreateInstance<MultHitNodeProp>();//new MultHitNodeProp();
            //Debug.LogError("MultHitNode");
        }
        public override void Dead()
        {

        }
        //public override string Name { get { return "击中事件"; } }

        //public override void Read(JsonData jsNode)
        //{
        //    base.Read(jsNode);

        //    hit_times = jsNode.GetInt32("hit_times", 1);
        //    delta_time = jsNode.GetFloat("delta_time", 0.5f);

        //    position.x = jsNode.GetFloat("px", 0.0f);
        //    position.y = jsNode.GetFloat("py", 0.0f);
        //    position.z = jsNode.GetFloat("pz", 0.0f);

        //    //radius = jsNode.GetFloat("r", 0.5f);
        //    attach = jsNode.GetBool("attach", false);
        //}

        //public override void Write(JsonData jsNode)
        //{
        //    base.Write(jsNode);

        //    jsNode["hit_times"] = hit_times;
        //    jsNode["delta_time"] = delta_time;

        //    if (position.x != 0.0f)
        //        jsNode["px"] = position.x;

        //    if (position.y != 0.0f)
        //        jsNode["py"] = position.y;

        //    if (position.z != 0.0f)
        //        jsNode["pz"] = position.z;

        //    //jsNode["r"] = radius;

        //    if (attach)
        //        jsNode["attach"] = attach;
        //}

        public override void Stop() { }
        public override void Update(float dTime) { }

        public override void Play(ISkillAttacker attacker, SkillEffect se)
        {
            GameObject damage_obj = new GameObject("damage_obj");

            MultHitNodeProp prop = m_NodeProp as MultHitNodeProp;
            if (prop == null)
            {
                return;
            }

            if (prop.attach)
                damage_obj.transform.parent = attacker.GetRoot();
            else
                damage_obj.transform.parent = helper.PlaceFxRoot;

            EffectUtil.ResetLocalTransform(damage_obj.transform);

            damage_obj.transform.localPosition = prop.position;

            MultHitHandle handle = damage_obj.AddComponent<MultHitHandle>();
            handle.m_attacker = attacker;
            //handle.m_damageRaduis = radius;
            handle.m_hitTimes = prop.hit_times;
            handle.m_deltaTime = prop.delta_time;

            // if (follow == false)
            // {
            //     damage_obj.transform.parent = Util.PlaceFxRoot;

            //     if (callback == null || by_target == false)
            //         damage_obj.transform.localPosition = caster.transform.TransformPoint(position);//caster.transform.position + position;
            //     else
            //         damage_obj.transform.localPosition = callback.GetAttackTarget().transform.position;
            // }
            // else
            // {
            //     damage_obj.transform.parent = caster.transform;
            //     Util.ResetLocalTransform(damage_obj.transform);
            //     damage_obj.transform.localPosition = position;
            // }

            //// Debug.LogWarning(caster);

            // PlaceDamageHandle handler = damage_obj.AddComponent<PlaceDamageHandle>();
            // handler.m_Owner = caster;
            // handler.m_timeLen = time_len;
            // handler.m_deltaTime = delta_time;


            // handler.m_damageRaduis = radius;

            // handler.m_push = push;
            // handler.m_hitIndex = hit_index;
            // handler.m_hitNode = this;

            // handler.m_callback = callback;
        }

        //public override void GatherResFile(List<ResItem> resItemList)
        //{
        //    //if (hit_fx.Length > 0)
        //    //    EffectDatabase.Me.GatherResFile("hit/" + hit_fx, resItemList);
        //}
    }
}