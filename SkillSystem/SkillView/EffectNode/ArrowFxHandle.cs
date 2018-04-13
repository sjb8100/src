using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace SkillSystem
{
    //发射特效（法术球，箭等）组件
    public class ArrowFxHandle : MonoBehaviour
    {
        public ISkillAttacker m_attacker;
        public float m_speed = 0.0f;
        public float m_acce = 0.0f;
        public float m_radius = 0.1f;
        public float m_range = 10.0f;
        public float m_delayTime = 0.0f;

        public uint m_effectid = 0;

        int m_layerMask = 0;

        public HitNode m_hitNode = null;

        Dictionary<int, GameObject> m_hitObjSet = new Dictionary<int, GameObject>();

        Vector3 m_startPos;
        Vector3 m_moveDir;

        //释放本次攻击的技能ID
        // int m_skillId = 0;

        public int m_hitIndex = 0;

        public int m_hitNum = 1;

        // Transform attacker_root ;

        bool m_has_cast = false;

        void Start()
        {
            m_startPos = transform.position;

            m_moveDir = transform.forward;
            m_moveDir.y = 0;
            m_moveDir.Normalize();

            // m_skillId = m_attacker.GetCurSkillId(); //没用到注释了
            m_layerMask = m_attacker.GetTargetLayer(); //潜在目标所在的LAYER

            if (m_delayTime <= 0.001f)
            {
                BeginCast();
            }
            else
            {
                StartCoroutine(UpdateDelayCast());
            }
        }

        public void DestroyFx()
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }

            Engine.IEffect effect = rs.GetEffect(m_effectid);
            if (effect != null)
            {

                rs.RemoveEffect(effect);
            }

            Destroy(gameObject);
            if(m_hitNode != null)
            {
                ArrowFxNode node = m_hitNode as ArrowFxNode;
                if (node != null)
                {
                    node.FreeToNodePool();
                }
            }
           
        }

        IEnumerator UpdateDelayCast()
        {
            yield return new WaitForSeconds(m_delayTime);
            BeginCast();
        }

        void BeginCast()
        {
            //脱离挂接点，开始飞出去了。
            //transform.parent = SkillEffectManager.Helper.PlaceFxRoot;
            //transform.localRotation = m_attacker.GetRoot().rotation;

            m_moveDir = transform.forward;
            m_moveDir.y = 0;
            m_moveDir.Normalize();

            m_has_cast = true;
        }

        bool CheckHit(Vector3 dir, float deltaMove)
        {
            Vector3 org_pos = transform.position;
            org_pos -= (dir * (m_radius * 1.2f));

            //org_pos.y = m_owner_y;

            RaycastHit[] hits = Physics.SphereCastAll(org_pos, m_radius, dir, deltaMove + 0.2f + m_radius, m_layerMask);
            if (hits != null)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit hit = hits[i];
                    GameObject target = hit.transform.gameObject;

                    bool is_valid_hit = false;

                    int id = target.GetInstanceID();
                    if (m_hitObjSet.ContainsKey(id) == false)
                    {
                        m_hitObjSet.Add(id, target);
                        //Log.Error("arrow hit");
                        m_attacker.OnComputeHit(target, m_hitNode);
                    }

                    if (is_valid_hit)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        void Update()
        {
            if (m_has_cast == false)
            {
                return;
            }

            m_speed += m_acce * Time.deltaTime;
            float deltaMove = m_speed * Time.deltaTime;

            if (m_radius > 0.0001f)
            {
                if (CheckHit(m_moveDir, deltaMove))
                {
                    DestroyFx();
                    //GameObject.Destroy(gameObject);//结束
                    //Debug.LogError("destroy: 1" + gameObject);
                    return;
                }
            }

            //超出最大射程，也自动结束
            Vector3 dist = transform.position - m_startPos;
            if (dist.sqrMagnitude >= m_range * m_range)
            {
                DestroyFx();
                //GameObject.Destroy(gameObject);//结束
                //Debug.LogError("destroy: 2" + gameObject);
            }
            else
            {
                transform.position += m_moveDir * deltaMove;
            }
        }
    }
}