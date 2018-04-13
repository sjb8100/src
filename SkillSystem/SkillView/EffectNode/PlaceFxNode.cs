using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using Engine;
using Client;

namespace SkillSystem
{
    //放置特效，把特效放在某个位置播放
    public class PlaceFxNode : EffectNode
    {

        PlaceFxHandle m_placeHandle = null;

        private uint m_uEffectID = 0;

        float m_startTime = 0;

        uint m_uEffectLen = 0;

        table.SkillDatabase m_curDb = null;
        SkillEffect m_ef = null;
        // bool m_bRmove = false;
        public PlaceFxNode()
        {
          //  m_NodeProp = ScriptableObject.CreateInstance<PlaceFxNodeProp>();// new PlaceFxNodeProp();


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
            Transform attacker_node = attacker.GetRoot();

            if (attacker_node == null)
            {
                return;
            }


            PlaceFxNodeProp prop = m_NodeProp as PlaceFxNodeProp;
            if (prop == null)
            {
                return;
            }
           // Engine.Utility.Log.Error("play PlaceFxNodeProp {0}", prop.fx_name);
            string stateName = prop.nodeState;
            int stateIndex = 0;
            if (!string.IsNullOrEmpty(stateName))
            {
                Client.SkillState state = SkillSystem.GetSkillStateByString(stateName);
                stateIndex = (int)state - 1;
            }
            if (stateIndex < 0)
            {
                stateIndex = 0;
            }
            Vector3 pos = attacker_node.TransformPoint(prop.position);
            //Vector3 vscale = attacker_node.localScale * prop.scale;
            Vector3 vscale = Vector3.one * prop.scale;
            Vector3 angles = attacker_node.eulerAngles;
            angles += prop.rotation;

            if (prop.by_target)
            {
                pos = se.GetPlayEffectInitPos() + prop.position;
            }
            int level = 0;
            if (attacker != null)
            {
                if (attacker.GetSkillPart() != null)
                {
                    level = attacker.GetSkillPart().FxLevel;
                }
            }
            m_uEffectID = helper.ReqPlayFx(prop.fx_name, pos, angles, vscale,level);
            m_startTime = Time.time;
            uint skillID = se.CurSkillID;

            m_curDb = GameTableManager.Instance.GetTableItem<table.SkillDatabase>(skillID, 1);
            if (m_curDb != null)
            {
                if (stateIndex >= 0)
                {
                    if (!string.IsNullOrEmpty(m_curDb.skillEffectTime))
                    {
                        string[] timeArray = SkillSystem.GetTimeArrayByString(m_curDb.skillEffectTime);
                        if (stateIndex < timeArray.Length)
                        {
                            uint time = 1000;
                            if (uint.TryParse(timeArray[stateIndex], out time))
                            {
                                m_uEffectLen = time;
                            }
                        }
                    }
                }

            }
            //  m_bRmove = false;

            IEffect ef = GetEffectByID(m_uEffectID);
            if (ef != null)
            {
                Transform trans = ef.GetNodeTransForm();
                if(trans == null)
                {
                    return;
                }
                m_placeHandle = trans.gameObject.GetComponent<PlaceFxHandle>();
                if (m_placeHandle == null)
                {
                    m_placeHandle = trans.gameObject.AddComponent<PlaceFxHandle>();
                }
            }
            m_placeHandle.m_attacker = attacker;
            m_placeHandle.m_len = m_uEffectLen;
            m_placeHandle.m_effectid = m_uEffectID;
            m_placeHandle.m_placeNode = this;
            m_placeHandle.InitPlaceFx();

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
        public override void Dead()
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }

            Engine.IEffect effect = rs.GetEffect(m_uEffectID);
            if (effect != null)
            {
                rs.RemoveEffect(effect);
            }
        }
        public override void Stop()
        {
            if(m_curDb == null)
            {
                return;
            }
            if (m_curDb.useSkillType == (int)UseSkillType.Sing || m_curDb.useSkillType == (int)(UseSkillType.GuideSlider) || m_curDb.useSkillType == (int)UseSkillType.GuideNoSlider)
            {//读条和引导技能打断时回收放置特效 其他不回收
                Dead();
            }
        }

        public override void Update(float dTime)
        {
            //if (m_bRmove)
            //{
            //    return;
            //}
            //float delta = (Time.time - m_startTime) * 1000;


            //if (delta > m_uEffectLen)
            //{
            //    Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            //    if (rs == null)
            //    {
            //        return;
            //    }

            //    Engine.IEffect effect = rs.GetEffect(m_uEffectID);
            //    if (effect != null)
            //    {
            //        rs.RemoveEffect(effect);
            //        m_bRmove = true;
            //    }
            //}
        }

        public static PlaceFxNode Create()
        {
            return new PlaceFxNode();
        }
    }
}