using System;
using System.Text;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine;
using Client;
using Engine.Utility;
using System.Collections;
namespace SkillSystem
{

    public class SkillEffect
    {
        SkillEffectProp m_Prop = null;
        private SortedDictionary<float, List<EffectNode>> m_EffectNodes = new SortedDictionary<float, List<EffectNode>>();

        private float m_fCurrentTime = 0.0f;    // 播放时间计时
        private ISkillAttacker m_Atttacker = null; // 攻击者
        private bool m_bAlive = false;          // 是否激活
        bool bStop = false;//是否停止

        bool bPlayFinish = false;//是否播放完成

        uint m_uSkillLen = 1000;
        public SkillEffectProp Prop
        {
            get { return m_Prop; }
        }
        public string Name
        {
            get { return m_Prop.Name; }
            set { m_Prop.Name = value; }
        }
        uint m_uSkillID = 0;
        /// <summary>
        /// 播放效果的技能
        /// </summary>
        public uint CurSkillID
        {
            get
            {
                return m_uSkillID;
            }
            set
            {
                m_uSkillID = value;
            }
        }


        public bool Create(SkillEffectProp prop)
        {
            m_Prop = null;
            m_Prop = prop;
            CreateEffectNode(m_Prop);
            return true;
        }
        bool IsHideFx()
        {
            if (m_Atttacker != null)
            {
                ISkillPart sp = m_Atttacker.GetSkillPart();
                if (sp != null)
                {
                    return sp.IsHideOtherFx();
                }
            }
            return false;
        }
        public void Destroy()
        {
            if (m_Prop != null)
            {
                m_Prop = null;
            }

            SortedDictionary<float, List<EffectNode>>.Enumerator iter = m_EffectNodes.GetEnumerator();
            while (iter.MoveNext())
            {
                for (int i = 0; i < iter.Current.Value.Count; ++i)
                {
                    if (iter.Current.Value[i] == null)
                    {
                        continue;
                    }

                    iter.Current.Value[i] = null;
                }

                iter.Current.Value.Clear();
                foreach (var node in iter.Current.Value)
                {
                    FreeSkillNode(node);
                }
            }

            m_EffectNodes.Clear();

            m_Atttacker = null;
        }
        Vector3 m_InitPlayEffectPos = Vector3.zero;
        /// <summary>
        /// 获取开始放技能的时候人物位置
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPlayEffectInitPos()
        {
            return m_InitPlayEffectPos;
        }
        public void SetPlayEffectInitPos(ISkillAttacker attacker)
        {
            if (attacker != null)
            {
                m_InitPlayEffectPos = attacker.GetTargetPos();
                if(m_InitPlayEffectPos == Vector3.zero)
                {
                    if(attacker.GetRoot() != null)
                    {
                        m_InitPlayEffectPos = attacker.GetRoot().localPosition;
                    }
                }
            }
          
        }
        public void Play(ISkillAttacker attacker)
        {
            m_fCurrentTime = 0.0f;
            m_Atttacker = attacker;
            m_bAlive = true;
            bStop = false;
            bPlayFinish = false;
            SetPlayEffectInitPos(attacker);
            CurSkillID = attacker.GetCurSkillId();
            table.SkillDatabase db = GameTableManager.Instance.GetTableItem<table.SkillDatabase>(CurSkillID, 1);
            if(db != null)
            {
                m_uSkillLen = db.skillLength;
            }
        }
        public void FreeAllNode()
        {
            SortedDictionary<float, List<EffectNode>>.Enumerator iter = m_EffectNodes.GetEnumerator();
            while (iter.MoveNext())
            {
                for (int i = 0; i < iter.Current.Value.Count; ++i)
                {
                    if (iter.Current.Value[i] == null)
                    {
                        continue;
                    }

                    iter.Current.Value[i].FreeToNodePool();
                }
            }
        }
        public void Dead()
        {
            bPlayFinish = false;
            bStop = true;
            SortedDictionary<float, List<EffectNode>>.Enumerator iter = m_EffectNodes.GetEnumerator();
            while (iter.MoveNext())
            {
                for (int i = 0; i < iter.Current.Value.Count; ++i)
                {
                    if (iter.Current.Value[i] == null)
                    {
                        continue;
                    }

                    iter.Current.Value[i].Dead();
                }
            }
        }
        public bool IsIdle()
        {
            return bStop || bPlayFinish;
        }
        public void Stop()
        {
            bStop = true;

            SortedDictionary<float, List<EffectNode>>.Enumerator iter = m_EffectNodes.GetEnumerator();
            while (iter.MoveNext())
            {
                for (int i = 0; i < iter.Current.Value.Count; ++i)
                {
                    if (iter.Current.Value[i] == null)
                    {
                        continue;
                    }

                    iter.Current.Value[i].Stop();

                }
            }
        }

        public bool Update(float dTime)
        {
            if (m_Prop == null)
            {
                bPlayFinish = false;
                return false;
            }
            if (m_Atttacker != null)
            {
                if (!m_Atttacker.IsLive())
                {
                    bPlayFinish = false;
                    return false;
                }
            }

            float fTime = m_fCurrentTime + dTime;
            // Log.Error("dttime is {0}  ftime is {1}", dTime,fTime);
            float maxtime = 0;
            bool bAlive = false;
            if (!bPlayFinish)
            {//bPlayFinish  标志所有节点是否全部播放过  减少SortedDictionary 迭代gc分配 
                SortedDictionary<float, List<EffectNode>>.Enumerator iter = m_EffectNodes.GetEnumerator();
                while (iter.MoveNext())
                {
                    var parieffect = iter.Current;
                    float fNodeTime = parieffect.Key;// *0.001f;
                    maxtime = fNodeTime > maxtime ? fNodeTime : maxtime;
                    if (fNodeTime > fTime)
                    {
                        bAlive = true;
                        break;
                    }

                    if (fNodeTime < m_fCurrentTime)
                    {
                        for (int i = 0; i < parieffect.Value.Count; ++i)
                        {
                            parieffect.Value[i].Update(dTime);
                        }
                        continue;
                    }

                    for (int i = 0; i < parieffect.Value.Count; ++i)
                    {

                        if (parieffect.Value[i] == null)
                        {
                            continue;
                        }

                        if (parieffect.Value[i].prop == null)
                        {
                            continue;
                        }

                        if (fTime > parieffect.Value[i].prop.time && m_fCurrentTime <= parieffect.Value[i].prop.time)
                        {
                            if (!bStop)
                            {//技能停止后 未播放的动作和特效节点不再播放
                                if (IsHideFx())
                                {
                                    EF_NODE_TYPE t = parieffect.Value[i].type;
                                    if (t == EF_NODE_TYPE.ARROW_FX ||
                                        t == EF_NODE_TYPE.ATTACH_FX ||
                                        t == EF_NODE_TYPE.FOLLOW_FX ||
                                        t == EF_NODE_TYPE.PLACE_FX)
                                    {
                                        continue;
                                    }
                                }
                                // PlaySkillNode(parieffect.Value[i]);
                                parieffect.Value[i].Play(m_Atttacker, this);
                                bAlive = true;
                            }

                        }

                    }
                }

            }
            m_fCurrentTime = fTime;
            m_bAlive = bAlive;
            maxtime *= 1000;
            maxtime = maxtime > m_uSkillLen ? maxtime : m_uSkillLen;
            if (fTime*1000>= maxtime)
            {
                bPlayFinish = true;
            }
            return bAlive;
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 创建特效
        @param 
        */
        private void CreateEffectNode(SkillEffectProp prop)
        {
            SortedDictionary<float, EffectNodeConfig>.Enumerator iter = prop.m_EffectProps.GetEnumerator();
            while (iter.MoveNext())
            {
                for (int i = 0; i < iter.Current.Value.configlist.Count; ++i)
                {
                    EffectNodeProp nodeprop = iter.Current.Value.configlist[i];
                    EffectNode node = CreateEffectNode(nodeprop.type);
                    if (node == null)
                    {
                        continue;
                    }

                    node.Create(nodeprop);
                    AddEffectNode(nodeprop.time, node);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 添加EfefctNode
        @param 
        */
        public void AddEffectNode(float fTime, EffectNode node)
        {
            List<EffectNode> lstNodes;
            if (!m_EffectNodes.TryGetValue(fTime, out lstNodes))
            {
                lstNodes = new List<EffectNode>();
                lstNodes.Add(node);

                //  int nKeys = (int)(fTime * 1000.0f);
                m_EffectNodes.Add(fTime, lstNodes);
            }
            else
            {
                lstNodes.Add(node);
            }
        }
        static SkillNodePool<ActionNode> m_actionNodePool = new SkillNodePool<ActionNode>();
        static SkillNodePool<AttachFxNode> m_attachNodePool = new SkillNodePool<AttachFxNode>();
        static SkillNodePool<EventNode> m_eventNodePool = new SkillNodePool<EventNode>();
        static SkillNodePool<ArrowFxNode> m_arrowNodePool = new SkillNodePool<ArrowFxNode>();
        static SkillNodePool<FollowFxNode> m_followNodePool = new SkillNodePool<FollowFxNode>();
        static SkillNodePool<PlaceFxNode> m_placeNodePool = new SkillNodePool<PlaceFxNode>();
        static SkillNodePool<SoundNode> m_soundNodePool = new SkillNodePool<SoundNode>();
        static SkillNodePool<MoveNode> m_moveNodePool = new SkillNodePool<MoveNode>();
        static SkillNodePool<SingleHitNode> m_singleNodePool = new SkillNodePool<SingleHitNode>();
        static SkillNodePool<ShakeCameraNode> m_shakecameraNodePool = new SkillNodePool<ShakeCameraNode>();
        static SkillNodePool<CastOverNode> m_castNodePool = new SkillNodePool<CastOverNode>();
        static SkillNodePool<MultHitNode> m_multhitNodePool = new SkillNodePool<MultHitNode>();
       public void FreeSkillNode(EffectNode node)
        {
            switch (node.type)
            {
                case EF_NODE_TYPE.ACTION:
                    {
                        m_actionNodePool.Free((ActionNode)node);
                    }
                    break;
                case EF_NODE_TYPE.ATTACH_FX:
                    {
                        m_attachNodePool.Free((AttachFxNode)node);
                    }
                    break;
                case EF_NODE_TYPE.EVENT:
                    {
                        m_eventNodePool.Free((EventNode)node);
                    }
                    break;
                case EF_NODE_TYPE.ARROW_FX:
                    {
                        m_arrowNodePool.Free((ArrowFxNode)node);
                    }
                    break;
                case EF_NODE_TYPE.FOLLOW_FX:
                    {
                        m_followNodePool.Free((FollowFxNode)node);
                    }
                    break;
                case EF_NODE_TYPE.PLACE_FX:
                    {
                        m_placeNodePool.Free((PlaceFxNode)node);
                    }
                    break;
                case EF_NODE_TYPE.SOUND:
                    {
                        m_soundNodePool.Free((SoundNode)node);
                    }
                    break;
                case EF_NODE_TYPE.MOVE:
                    {
                        m_moveNodePool.Free((MoveNode)node);
                    }
                    break;
                case EF_NODE_TYPE.SINGLE_HIT:
                    {
                        m_singleNodePool.Free((SingleHitNode)node);
                    }
                    break;
                case EF_NODE_TYPE.CAMERA:
                    {
                        m_shakecameraNodePool.Free((ShakeCameraNode)node);
                    }
                    break;
                case EF_NODE_TYPE.CastOverNode:
                    {
                        m_castNodePool.Free((CastOverNode)node);
                    }
                    break;
                case EF_NODE_TYPE.MULT_HIT:
                    {
                        m_multhitNodePool.Free((MultHitNode)node);
                    }
                    break;

                default:
                    break;

            }
        }

        public static EffectNode CreateEffectNode(EF_NODE_TYPE type)
        {

            switch (type)
            {
                case EF_NODE_TYPE.ACTION:
                    {
                        return m_actionNodePool.Alloc(() => { return new ActionNode(); });
                    }
                case EF_NODE_TYPE.ATTACH_FX:
                    {
                        return m_attachNodePool.Alloc(() => { return new AttachFxNode(); });
                    }
                case EF_NODE_TYPE.EVENT:
                    {
                        return m_eventNodePool.Alloc(() => { return new EventNode(); });
                    }
                case EF_NODE_TYPE.ARROW_FX:
                    {
                        return m_arrowNodePool.Alloc(() => { return new ArrowFxNode(); });
                    }
                case EF_NODE_TYPE.FOLLOW_FX:
                    {
                        return m_followNodePool.Alloc(() => { return new FollowFxNode(); });
                    }
                case EF_NODE_TYPE.PLACE_FX:
                    {
                        return m_placeNodePool.Alloc(() => { return new PlaceFxNode(); });
                    }
                case EF_NODE_TYPE.SOUND:
                    {
                        return m_soundNodePool.Alloc(() => { return new SoundNode(); });
                    }
                case EF_NODE_TYPE.MOVE:
                    {
                        return m_moveNodePool.Alloc(() => { return new MoveNode(); });
                    }
                case EF_NODE_TYPE.SINGLE_HIT:
                    {
                        return m_singleNodePool.Alloc(() => { return new SingleHitNode(); });
                    }
                case EF_NODE_TYPE.CAMERA:
                    {
                        return m_shakecameraNodePool.Alloc(() => { return new ShakeCameraNode(); });
                    }
                case EF_NODE_TYPE.CastOverNode:
                    {
                        return m_castNodePool.Alloc(() => { return new CastOverNode(); });
                    }
                case EF_NODE_TYPE.MULT_HIT:
                    {
                        return m_multhitNodePool.Alloc(() => { return new MultHitNode(); });
                    }

                default:
                    break;

            }
            return null;
        }
    }

 


}