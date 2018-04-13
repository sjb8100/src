using UnityEngine;
using System.Collections;
using Client;
namespace SkillSystem
{
    //跟踪发射特效组件
    public class FollowFxHandle : MonoBehaviour
    {
        public float m_speed = 0.0f;

        public GameObject m_targetObj;
      //  public Transform m_targetNode;
        public ISkillAttacker m_attacker;

        public float m_acce = 0.0f;
        public float m_len = 2.0f; //最长持续时间

        public HitNode m_hitNode = null;

        //释放本次攻击的技能ID
        public int m_skillId;
        public uint m_effectid =0;

        private float m_startTime;

        public int m_hitIndex = 0;

        public IEntity m_SkillTarget;

        private Vector3 castPos = Vector3.zero;//施法时目标位置  在施法过程中目标死亡时使用
        private float totalTime = 0;

        public string targetHitNode = "TxChest";
        bool bDestory = false;
        bool bStart = false;
        void Start()
        {
         
        }

        public void InitFollowFx()
        {
            if(m_attacker == null)
            {
                DestroyFx();
                return;
            }
            bStart = true;
            bDestory = false;
            m_targetObj = m_attacker.GetTargetGameObject();
            ISkillPart skillPart = m_attacker.GetSkillPart();
            if (skillPart != null)
            {
                m_SkillTarget = skillPart.GetSkillTarget();

                table.SkillDatabase db = GameTableManager.Instance.GetTableItem<table.SkillDatabase>((uint)m_skillId, 1);
                if (db != null)
                {
                    if (db.flyspeed != 0)
                    {
                        m_speed = db.flyspeed * 1.0f / 100;
                    }
                }
            }

            // m_targetNode = m_attacker.GetTargetHitNode();
            m_startTime = Time.time;
            castPos = m_attacker.GetTargetHitNodePos(m_SkillTarget, targetHitNode);
            float dis = GetVectorDistance(transform.position, castPos);
            totalTime = dis / m_speed;
        }
        float GetVectorDistance(Vector3 a,Vector3 b)
        {
            //Vector2 tempa = new Vector2(a.x, a.z);
            //Vector2 tempb = new Vector2(b.x, b.z);
            a.y = b.y;
            return Vector3.Distance(a, b);
        }
        Engine.IRenderSystem rs = null;
        public void DestroyFx()
        {
         
            if (rs == null)
            {
                 rs = Engine.RareEngine.Instance().GetRenderSystem();
            }

           // transform.DetachChildren();
            Engine.IEffect effect = rs.GetEffect(m_effectid);
            if (effect != null)
            {
          
                rs.RemoveEffect(effect);
            }
            if(m_hitNode != null)
            {
                FollowFxNode node = m_hitNode as FollowFxNode;
                if (node != null)
                {
                    node.FreeToNodePool();
                }
            }
          
            //m_effectid = 0;
            m_skillId = 0;
            m_attacker = null;
            m_targetObj = null;
            m_SkillTarget = null;
         
           // SkillEffectHelper.Instance.FreeFollowGameObject(gameObject);
            bDestory = true;
            bStart = false;
         //   DestroyImmediate(gameObject);
        }

        void Update()
        {
            if(!bStart)
            {
                return;
            }
            if (bDestory)
            {
               
                return;
            }
            if (Time.time - m_startTime > m_len)
            {
                //超过最长时间
                DestroyFx();
                return;
            }

            if ( m_targetObj != null )
            {
                if(m_attacker == null)
                {
                    DestroyFx();
                    return;
                }
                //目标已经死亡
                if (m_attacker.IsLive() == false)
                {
                    DestroyFx();
                    return;
                }
            }
            Vector3 targetPos = m_attacker.GetTargetHitNodePos(m_SkillTarget, targetHitNode);

            if (targetPos == Vector3.zero)
            {
                targetPos = castPos;
               // Log.Error("use cast pos");
            }
            else
            {
                float dis = GetVectorDistance(transform.position, targetPos);
                totalTime -= Time.deltaTime;
                if(totalTime >= 0)
                {
                    m_speed = dis / totalTime;
                }
                else
                {
                    DestroyFx();
                    return;
                }
         
            }
           // m_speed += m_acce * Time.deltaTime;
            float deltaMove = m_speed * Time.deltaTime;
         //   Log.Error("speed is " + m_speed + " delta move is " + deltaMove);
            if ( m_targetObj != null )
            {
               
                Vector3 dist = targetPos - transform.position;
                Vector3 dir = dist.normalized;
             
                transform.LookAt(targetPos);
                float distance = GetVectorDistance(targetPos, transform.position);
                //  if (distance <= deltaMove * deltaMove)
                if (distance <= 0.2f)
                {
                    //击中目标，
                    if (m_attacker != null)
                    {
                       // m_hitNode.m_uDamageID = m_attacker.GetDamageID();
                        //Log.Error("follow hit");
                        m_attacker.OnComputeHit(m_targetObj, m_hitNode);

                        DestroyFx();
                        return;
                    }

                    //已经到达
                    transform.position = targetPos;
                }
                else
                {
                    transform.position += dir * deltaMove;
                }
               // Log.Error("dir is " + dir + " pos is " + transform.position+"  targetpos is "+targetPos);
           
            }
            else
            {
                DestroyFx();
                //transform.position += transform.forward * deltaMove;
            }
        }
    }
}