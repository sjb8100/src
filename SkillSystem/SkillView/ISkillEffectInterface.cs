using UnityEngine;
using System;
using Client;
using Engine;
namespace SkillSystem
{

    //------------------------------------------
    //技能特效施法者接口，需要在游戏和编辑器中分别实现
    //在游戏中一般由主角，怪物的技能释放模块来实现
    //------------------------------------------
    public interface ISkillAttacker
    {
        //计算击中伤害
        bool OnComputeHit(HitNode hit_node);

        //计算击中伤害（发射类技能特效）
        bool OnComputeHit(GameObject target, HitNode hit_node);

        //技能移动
        void OnStartSkillMove(MoveNode node);

        //获取当时正在释放的技能ID
        uint GetCurSkillId();
        //获取唯一伤害点id
        uint GetDamageID();

        //释发者是否还活着
        bool IsLive();

        //攻击目标
        GameObject GetTargetGameObject();

        //攻击目标的击中节点位置
        Vector3 GetTargetHitNodePos(IEntity target, string hitnode);

        //释放者
        IEntity GetGameObject();

        ISkillPart GetSkillPart();

        //释放者根节点
        Transform GetRoot();

        //释放者当前坐标
        Vector3 GetTargetPos();

        //攻击目标的层
        int GetTargetLayer();

        //释放者技能动作
        void PlaySkillAnim(string name, bool isAttackStateAni, int loopCount = 1, float speed = 1.0f, float offset = 0.0f);
        ///// <summary>
        ///// 获取放特效时目标初始化位置
        ///// </summary>
        ///// <returns></returns>
        //Vector3 GetPlayEffectInitPos();
        ///// <summary>
        /////  设置放特效时目标初始化位置
        ///// </summary>
        ///// <param name="pos"></param>
        //void SetPlayEffectInitPos();
        //正在播放中的攻击动作
        AnimationState AttackAnimState
        {
            get;
        }
    }


    //------------------------------------------
    // 技能特效辅助接口，需要在游戏和编辑器中分别实现
    //------------------------------------------
    public interface ISkillEffectHelper
    {
        byte[] OpenFile(string path);

        ////创建特效实例
        //GameObject CreateFxObj(string path);

        //获取跟随主角的相机
        Camera GetPlayerCamera();

        ////放置特效在某个位置
        //void PlaceFxObj(GameObject fxObj, Vector3 position, Vector3 angles, Vector3 scale);

        ////放置特效在某个位置
        //void PlaceFxObj(string path, Vector3 position, Vector3 angles, Vector3 scale);

        //播放音效
        void ReqPlaySound(string snd_name);

        //放置特效的挂接根节点
        Transform PlaceFxRoot
        {
            get;
        }

        /// <summary>
        /// //挂接特效 老的接口  
        /// </summary>
        /// <param name="fx_name"></param>
        /// <param name="attach_node"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        uint ReqPlayFx(string fx_name, Transform attach_node, Vector3 position, Vector3 scale, int level = 0);


        //放置特效
        uint ReqPlayFx(string fx_name, Vector3 position, Vector3 rotation, Vector3 scale, int level = 0);
        //放置特效
        uint ReqPlayHitFx(string fx_name, Vector3 position, Vector3 rotation, Vector3 scale, int level = 0);
    }
}