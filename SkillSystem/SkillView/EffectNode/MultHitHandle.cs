using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SkillSystem
{
    //多次伤害组件
    public class MultHitHandle : MonoBehaviour
    {
        public float m_hitTimes; // 造成伤害次数

        public float m_deltaTime; //间隔时间

        public HitNode m_hitNode;
        public int m_hitIndex = 0;

        public ISkillAttacker m_attacker;

        float m_onceTime = 0.0f;

        //释放本次攻击的技能ID
        //int m_skillId;

        void Start()
        {
           // m_skillId = m_attacker.GetCurSkillId();
        }
     
        // Update is called once per frame
        void Update()
        {
            m_onceTime += Time.deltaTime;

            if (Time.time - m_onceTime > m_deltaTime)
            {
                m_hitIndex++;
                m_onceTime = 0.0f;

                m_attacker.OnComputeHit(m_hitNode);
            }

            if (m_hitIndex >= m_hitTimes)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}