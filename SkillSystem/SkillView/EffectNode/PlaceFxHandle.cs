using SkillSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PlaceFxHandle:MonoBehaviour
{
    public ISkillAttacker m_attacker;
    public float m_len = 5.0f; //最长持续时间
    private float m_startTime;
    public uint m_effectid = 0;
    bool bPlay = false;
    public PlaceFxNode m_placeNode = null;
    public void InitPlaceFx()
    {
        m_startTime = Time.time;
        bPlay = true;
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
        bPlay = false;
        if(m_placeNode != null)
        {
            m_placeNode.FreeToNodePool();
        }
    }

    void Update()
    {
        if(!bPlay)
        {
            return;
        }
        float tempTime = (Time.time - m_startTime) * 1000;
        if (tempTime > m_len)
        {
            //超过最长时间
            DestroyFx();
            return;
        }
    }
}

