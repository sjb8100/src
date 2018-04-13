using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using DG.Tweening;
using Engine;
using Client;
using Engine.Utility;
namespace SkillSystem
{
    //相机振动效果
    public class ShakeCameraNode : EffectNode
    {
        //         public float duration = 0.2f;
        //         public float strength = 0.5f;
        //         public int   vibrato = 10;

        public ShakeCameraNode()
        {
          //  m_NodeProp = ScriptableObject.CreateInstance<ShakeCameraNodeProp>();//  new ShakeCameraNodeProp();
        }

        public override void Play(ISkillAttacker attacker, SkillEffect se)
        {
            if(attacker == null)
            {
                return;
            }
            IEntity en = attacker.GetGameObject();
            if(en == null)
            {
                return;
            }
            if(!SkillSystem.GetClientGlobal().IsMainPlayer(en) )
            {//不是主角不震屏
                return;
            }
            ShakeCameraNodeProp prop = m_NodeProp as ShakeCameraNodeProp;
            if (prop == null)
            {
                return;
            }
            IRenderSystem renderSys = RareEngine.Instance().GetRenderSystem();
            if (renderSys == null)
            {
                Log.Error("RenderSystem is null!");
                return;
            }
            string camName = "MainCamera";
            ICamera cam = renderSys.GetCamera(ref camName);
            if (cam != null)
            {
                ICameraCtrl cc = cam.GetCameraCtrl();
                if (cc != null)
                {

                    //   public float duration = 0.2f;//持续时间
                    //   public float strength = 0.5f;//振幅
                    //   public int vibrato = 10;//频率
                    //@brief 开始振动
                    //@param fAmplitude 振幅
                    //@param fFrequency 频率
                    //@param fCycle 周期
                    //@param fTime 持续时间

                    cc.StartShake(prop.strength, prop.vibrato, 1, prop.duration);

                }
            }
            else
            {
                //编辑器代码实现
                Camera.main.DOShakePosition(prop.duration, prop.strength, prop.vibrato,0);
            }

        }
        public override void Dead()
        {

        }
        public override void Stop()
        {
            IRenderSystem renderSys = RareEngine.Instance().GetRenderSystem();
            if (renderSys == null)
            {
                Log.Error("RenderSystem is null!");
                return;
            }
            string camName = "Main Camera";
            ICamera cam = renderSys.GetCamera(ref camName);
            if (cam != null)
            {
                ICameraCtrl cc = cam.GetCameraCtrl();
                if (cc != null)
                {
                    cc.StopShake();
                }
            }
        }
        public override void Update(float dTime) { }


    }

}