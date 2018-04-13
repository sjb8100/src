using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Client;
using GameCmd;
using Common;
using UnityEngine;
using Engine.Utility;
using table;
using EntitySystem;
using Engine;
using UnityEngine.Profiling;

namespace SkillSystem
{
    partial class PlayerSkillPart : ISkillAttacker
    {
        private bool m_IsAttackStateEnd = false;
        public bool IsAttackStateEnd
        {
            set
            {
                m_IsAttackStateEnd = value;
            }
            get
            {
                return m_IsAttackStateEnd;
            }
        }

        #region ISkillAttacker
        //计算击中伤害
        public bool OnComputeHit(HitNode hit_node)
        {
            //GameCmd.stMultiAttackDownMagicUserCmd_S cmd = damagerManager.GetDamage(hit_node, m_Master.GetUID());
            //if (damagerManager.HasContain(hit_node, m_Master.GetUID()))
            //{
            //    if (cmd != null)
            //    {//飘字
            //        ShowDamage(cmd);
            //    }
            //}
            //else
            //{//记录伤害帧过后 伤害仍然没有值的节点
            //    if (!hitDic.ContainsKey(m_uDamageID))
            //    {
            //        hitDic.Add((uint)m_uDamageID, hit_node);
            //    }
            //    else
            //    {
            //       // Log.Trace("has contain key is {0}", m_uDamageID);
            //    }
            //}
            //return PlayDefenerAni(cmd);
            return true;
        }
        /// <summary>
        /// 播放防御者受击动作
        /// </summary>
        /// <param name="cmd"></param>
        private bool PlayDefenerAni(GameCmd.stMultiAttackDownMagicUserCmd_S cmd)
        {
      
            if (cmd == null)
            {
                return false;
            }
            SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(cmd.wdSkillID, 1);
            if (db != null)
            {
                for (int i = 0; i < db.beAttactEffect.Count; i++)
                {
                    var effectid = db.beAttactEffect[i];
                    FxResDataBase edb = GameTableManager.Instance.GetTableItem<FxResDataBase>(effectid);
                    if (edb != null)
                    {
                        if (cmd != null)
                        {
                            for (int j = 0; j < cmd.data.Count; j++)
                            {
                                var item = cmd.data[j];
                                string aniName = edb.targetAniName;

                                IEntity defender = EntityHelper.GetEntity(item.byDefencerType, item.dwDefencerID);
                                if (defender == null)
                                {
                                    //Log.Error( "死亡状态不能播放受击动作" );
                                    return false;
                                }
                                if (defender is INPC)
                                {
                                    INPC npc = defender as INPC;
                                    if (npc != null)
                                    {
                                        if (npc.IsTrap())
                                        {
                                            return false;
                                        }
                                    }
                                }
                                if (edb.playAudio != 0)
                                {
                                    ResourceDataBase rdb = GameTableManager.Instance.GetTableItem<ResourceDataBase>(edb.playAudio);
                                    if (rdb != null)
                                    {
                                        Transform trans = defender.GetTransForm();
                                        if (trans == null)
                                        {
                                            return false;
                                        }
                                        PlayAudio(trans.gameObject, rdb.strPath);
                                    }
                                }



                                EffectViewFactory.Instance().CreateEffect(defender.GetUID(), effectid);
                                if (item.byDamType != (uint)GameCmd.AttackType.ATTACK_TYPE_HD)
                                {
                                    SendPlayDefenerAniMessage(defender, aniName);
                                }

                            }
                        }
                    }
                }
            }
        
            return true;
        }
        void PlayAudio(GameObject go, string path)
        {
            Profiler.BeginSample("PlayAudio");
            Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
            if (audio == null)
            {
                return;
            }
            audio.PlayEffect(go, path);
            Profiler.EndSample();
        }
        private void SendPlayDefenerAniMessage(IEntity defender, string aniName)
        {
        
            CreatureState state = defender.GetCurState();
            ShowFlashColor(defender);
            if (state != CreatureState.Dead)
            {
                bool bRide = (bool)defender.SendMessage(EntityMessage.EntityCommond_IsRide, null);
                if (bRide)
                {
                    return;
                }
                SkillState skillState = GetTargetSkillState(defender);
                if (skillState != SkillState.Attack && skillState != SkillState.Prepare)
                {
                    if (defender.GetCurState() == CreatureState.Contrl)
                    {
                        return;
                    }
                    if (state != CreatureState.Move)
                    {
                        Client.IControllerSystem cs = m_ClientGlobal.GetControllerSystem();
                        if (cs != null)
                        {
                            Client.ICombatRobot robot = cs.GetCombatRobot();
                            if (robot.Status == CombatRobotStatus.RUNNING)
                            {
                                if (SkillSystem.GetClientGlobal().IsMainPlayer(defender.GetID()))
                                {//自动挂机 不播受击

                                    return;
                                }
                            }
                        }
                        INPC npc = defender as INPC;
                        if (npc != null)
                        {
                            int baseID = npc.GetProp((int)EntityProp.BaseID);
                            NpcDataBase ndb = GameTableManager.Instance.GetTableItem<NpcDataBase>((uint)baseID);
                            if (ndb != null)
                            {
                                if (ndb.dwMonsterType == 3)
                                {
                                    return;
                                }
                            }
                        }
                        //defender.SendMessage( EntityMessage.EntityCommand_StopMove , defender.GetPos() );
                        //移动不播放受击动作
                        PlayAni anim_param = new PlayAni();
                        anim_param.strAcionName = aniName;
                        anim_param.fSpeed = 1;
                        anim_param.nStartFrame = 0;
                        anim_param.nLoop = 1;
                        anim_param.fBlendTime = 0.2f;
                        anim_param.aniCallback = OnHitCallback;
                        anim_param.callbackParam = defender;

                        //                         if (SkillSystem.GetClientGlobal().IsMainPlayer(m_Master.GetID()))
                        //                         {
                        //                             Engine.Utility.Log.Info("技能 播放受击动作 {0} {1}", anim_param.strAcionName, state);
                        //                         }
                        defender.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);

                    }
                    else
                    {
                        if (SkillSystem.GetClientGlobal().IsMainPlayer(m_Master.GetID()))
                        {
                            //Engine.Utility.Log.Info("移动不播放受击动作");
                        }
                    }
                }
                else
                {
                   
                  //  Engine.Utility.Log.Error(string.Format("{0}技能状态错误不能播放受击动作 状态:{1}", defender.GetName(), GetTargetSkillState(defender)));
                }
            }
        }


        void ShowFlashColor(IEntity defender)
        {
            Profiler.BeginSample("ShowFlashColor");
            INPC npc = defender as INPC;
            if (npc != null)
            {
                int baseID = npc.GetProp((int)EntityProp.BaseID);
                NpcDataBase npcDb = GameTableManager.Instance.GetTableItem<NpcDataBase>((uint)baseID);
                if (npcDb != null)
                {
                    uint monsterType = npcDb.dwMonsterType;
                    if (monsterType == 0)
                    {
                        return;
                    }
                    List<uint> configList = SkillSystem.GetColorList(monsterType);
                    if (configList != null)
                    {
                        if (configList.Count > 0)
                        {
                            uint show = configList[0];
                            if (show == 1)
                            {
                                if (configList.Count != 6)
                                {
                                    // Log.Error("全局配置FlashColor  长度错误 ，检查配置");
                                }
                                else
                                {
                                    FlashColor fc = new FlashColor();
                                    fc.color = new Color(configList[2] * 1.0f / 255, configList[3] * 1.0f / 255, configList[4] * 1.0f / 255, configList[5] * 1.0f / 255);
                                    fc.fLift = configList[1] * 1.0f / 1000;
                                    defender.SendMessage(EntityMessage.EntityCommand_FlashColor, fc);
                                    // Log.Error("flash color " + defender.GetName());
                                }
                            }
                        }
                    }
                }
            }
            Profiler.EndSample();
        }
        //计算击中伤害（发射类技能特效）
        public bool OnComputeHit(GameObject target, HitNode hit_node)
        {
            OnComputeHit(hit_node);
            return true;
        }

        //技能移动
        public void OnStartSkillMove(MoveNode node)
        {

        }

        //获取当时正在释放的技能ID
        public uint GetCurSkillId()
        {
            return CurSkillID;
        }

        public uint GetDamageID()
        {
            return m_uDamageID;
        }
        //释发者是否还活着
        public bool IsLive()
        {
            return bLive;
        }

        //攻击目标
        public GameObject GetTargetGameObject()
        {
            if (SkillTarget != null)
            {
                if (SkillTarget.GetTransForm() != null)
                {
                    return SkillTarget.GetTransForm().gameObject;
                }
            }


            return null;
        }
        public ISkillPart GetSkillPart()
        {
            return this;
        }
        //攻击目标的击中节点
        public Vector3 GetTargetHitNodePos(IEntity target, string hitnode)
        {
            Vector3 pos = Vector3.zero;
            if (target != null)
            {
                if (string.IsNullOrEmpty(hitnode))
                {
                    hitnode = "TxChest";
                }
                target.GetLocatorPos(hitnode, Vector3.zero, Quaternion.identity, ref pos, true);
                return pos;
            }

            return pos;
        }

        //释放者
        public IEntity GetGameObject()
        {
            return m_Master;
        }

        //释放者根节点
        public Transform GetRoot()
        {
            if (m_Master != null)
            {
                if (m_Master.GetTransForm() != null)
                {
                    return m_Master.GetTransForm();
                }
            }
            return null;
        }

        //目标当前坐标
        public Vector3 GetTargetPos()
        {
            IEntity target = GetSkillTarget();
            if (target != null)
            {
                return target.GetPos();
            }

            return Vector3.zero;
        }
   
        //攻击目标的层
        public int GetTargetLayer()
        {
            return 0;
        }

        //释放者技能动作
        public void PlaySkillAnim(string name, bool isAttackStateAni, int loopCount = 1, float speed = 1.0f, float offset = 0.0f)
        {
            // AttackAnimState = null;

            float blendTime = 0;
            SkillDoubleHitDataBase db = GameTableManager.Instance.GetTableItem<SkillDoubleHitDataBase>(CurSkillID);
            if (db != null)
            {
                blendTime = db.blendTime;
            }
            AttackAnimState = m_Master.GetAnimationState(name);

            if (AttackAnimState == null)
            {
                Log.Trace("animate is null aniname is {0} ", name);
                return;
            }

            AttackAnimState.wrapMode = WrapMode.ClampForever;

            PlayAni anim_param = new PlayAni();
            anim_param.strAcionName = name;
            anim_param.fSpeed = speed;
            anim_param.nStartFrame = Mathf.RoundToInt(offset * 30);
            anim_param.nLoop = loopCount;
            anim_param.fBlendTime = blendTime;
            IsAttackStateEnd = false;
            if (isAttackStateAni)
            {
                anim_param.aniCallback = OnSkillAttackStateEndCallBack;
                anim_param.callbackParam = (object)name;
            }
            m_Master.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
        }
        void OnSkillAttackStateEndCallBack(ref string strEventName, ref string strAnimationName, float time, object param)
        {
            string aniName = (string)param;
            if (strEventName == "end" && strAnimationName == aniName)
            {
                IsAttackStateEnd = true;
                m_Master.SendMessage(EntityMessage.EntityCommand_ClearAniCallback, null);
            }
        }
        public void StopSkillAnimation()
        {
            if (m_Master != null)
            {

            }
        }
        #endregion
    }
}
