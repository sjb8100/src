using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using Engine.Utility;
namespace SkillSystem
{
    //音效节点
    public class SoundNode : EffectNode, ITimer
    {
        //public string snd_name = "";
        uint m_audioID = 0;//音效资源id
        bool bLoop = false;
       // float m_fAuioPlaytime = 0;//循环音效持续时间
        float m_fAudioEndTime = 0;//配置播放时间
        uint m_uAuidoStopTimerID = 1888;
        SkillEffect m_ef = null;
        public SoundNode()
        {
            //m_NodeProp = ScriptableObject.CreateInstance<SoundNodeProp>();//  new SoundNodeProp();
         //只有当前英雄，才播放战斗音效

        }


        public override void FreeToNodePool()
        {
            if (m_ef != null)
            {
                m_ef.FreeSkillNode(this);
            }
        }
        public override void Play(ISkillAttacker attacker,SkillEffect se)
        {
            m_ef = se;
            SoundNodeProp prop = m_NodeProp as SoundNodeProp;
            if (prop == null)
            {
                return;
            }
            m_NodeProp.only_main_role = true; 
            if (prop.snd_name == "")
                return;
            bLoop = prop.bloop;

            m_fAudioEndTime = prop.endTime;
            Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
            if (audio == null)
            {
                return;
            }
            if (attacker == null)
            {
                Log.LogGroup("ZDY","attacker is null");
                return;
            }
            if (attacker.GetGameObject() == null)
            {
                Log.LogGroup("ZDY", "entity is null");
                return;
            }
            if (attacker.GetGameObject().GetNode() == null)
            {
                Log.LogGroup("ZDY", "entity GetNode is null");
                return;
            }
            Transform trans = attacker.GetGameObject().GetTransForm();
            if (trans == null)
            {
                Log.LogGroup("ZDY", "entity GetNode GetTransForm is null");
                return;
            }
            m_audioID = audio.PlayEffect(trans.gameObject, prop.snd_name, bLoop);
            uint stopTime = (uint)(m_fAudioEndTime * 1000);
            if (bLoop)
            {
                TimerAxis.Instance().SetTimer(m_uAuidoStopTimerID, stopTime, this, 1);
            }
        }

        public override void Stop()
        {
            Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
            if (audio != null)
            {
                audio.StopEffect(m_audioID);
            }
        }
        public override void Update(float dTime)
        {

        }

        public override void Dead()
        {
            if(TimerAxis.Instance().IsExist(m_uAuidoStopTimerID,this))
            {
                TimerAxis.Instance().KillTimer(m_uAuidoStopTimerID, this);
            }
            Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
            if (audio != null)
            {
                audio.StopEffect(m_audioID);
            }
            bLoop = false;
            FreeToNodePool();
        }


        public void OnTimer(uint uTimerID)
        {
            if (m_uAuidoStopTimerID == uTimerID)
            {
                Dead();
            }
        }
    }
}