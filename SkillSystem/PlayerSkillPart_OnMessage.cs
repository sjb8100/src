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
using UnityEngine.Profiling;


namespace SkillSystem
{
    partial class PlayerSkillPart
    {
        //目标是否改变
        bool m_bTargetChange = false;
        #region execute msg
        //-------------------------------------------------------------------------------------------------------
        //// 伤害处理
        public void OnDamage(GameCmd.stMultiAttackDownMagicUserCmd_S cmd)
        {
         
            List<stMultiAttackDownMagicUserCmd_S.stDefender> targetList = cmd.data;
            if (targetList.Count > 0)
            {
                uint targetID = targetList[0].dwDefencerID;
                IEntity target = EntitySystem.EntityHelper.GetEntity(targetList[0].byDefencerType, targetID);
                if (target != null)
                {
                    if (SkillTarget == null)
                    {
                        SkillTarget = target;
                    }
                }
            }
            //tmpid 不做变化 
            //if (cmd.tmpid == 0)
            //{
            //    cmd.tmpid = m_uDamageID;
            //}
            //else
            //{//此处为自己受伤赋值给skillid 修改看不到自己掉血
            //    m_uDamageID = cmd.tmpid;
            //}
            if (CurSkillID == 0)
            {
                //如果直接赋值会影响连击的判断
                CurSkillID = cmd.wdSkillID;
            }

            //引导类型的直接飘伤害
            SkillDatabase db = GetCurSkillDataBase();
            if (db == null)
            {
                db = GetSkillDataBase(cmd.wdSkillID);
            }
            if (db == null)
            {
                Log.Error("技能数据获取失败 id{0}:", cmd.wdSkillID);
                return;
            }
            //if (db.dwUseMethod == (int)UseMethod.ContinueLock)
            //{
            //    // 链式攻击特效
            //    string strEffectID = db.beLinkAttactEffect;
            //    string[] strids = strEffectID.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            //    uint[] ids = new uint[strids.Length];
            //    for (int i = 0; i < strids.Length; ++i)
            //    {
            //        ids[i] = uint.Parse(strids[i]);
            //    }
            //    uint effectID = ids[UnityEngine.Random.Range(0, ids.Length)];
            //    EffectViewFactory.Instance().CreateEffect(SkillTarget.GetUID(), effectID);
            //    PlayDefenerAni(cmd);
            //}
            //if (db.useSkillType == (int)(UseSkillType.GuideSlider) || db.useSkillType == (int)UseSkillType.GuideNoSlider)
            //{
            //    ShowDamage(cmd);
            //    PlayDefenerAni(cmd);
            //    return;
            //}
   
            ShowDamage(cmd);
            PlayDefenerAni(cmd);
            return;
            //if(Master.GetEntityType() == EntityType.EntityType_NPC)
            //{
            //    ShowDamage(cmd);
            //    PlayDefenerAni(cmd);
            //    return;
            //}
            //else
            //{
            //    // 处理伤害
            //    if (hitDic.ContainsKey(cmd.tmpid))
            //    {
            //        //飘字 已经过了伤害时间帧的技能 不加入伤害列表直接飘字
            //        ShowDamage(cmd);
            //        hitDic.Remove(cmd.tmpid);
            //    }
            //    else
            //    {
            //        damagerManager.AddDamage(cmd, m_Master.GetUID());
            //    }
            //}

        }

        void ShowDamage(GameCmd.stMultiAttackDownMagicUserCmd_S cmd)
        {
            Profiler.BeginSample("ShowDamage");
            uint skillID = cmd.wdSkillID;

            stShowDemage st = new stShowDemage();
            st.uid = Master.GetUID();
            st.skillid = skillID;
            st.defenerList = cmd.data;
            st.attackID = cmd.dwAttackerID;
            st.attacktype = (uint)cmd.byAttackerType;
            st.filterList = null;
            //Dictionary<uint, List<stMultiAttackDownMagicUserCmd_S.stDefender>> dic = new Dictionary<uint, List<stMultiAttackDownMagicUserCmd_S.stDefender>>();
            //dic.Add(skillID, cmd.data);
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSTEM_SHOWDAMAGE, (object)st);
          //  damagerManager.RemoveDamage(cmd, m_Master.GetUID());
            Profiler.EndSample();
        }
        /// <summary>
        /// 受击动作完后进入stand 动作
        /// </summary>
        /// <param name="strEventName"></param>
        /// <param name="strAnimationName"></param>
        /// <param name="time"></param>
        /// <param name="param"></param>
        void OnHitCallback(ref string strEventName, ref string strAnimationName, float time, object param)
        {
            if (strEventName == "end" && strAnimationName == "Hit001")
            {
                IEntity entity = param as IEntity;
                if (entity.GetCurState() != CreatureState.Move)
                {
                    entity.ChangeState(CreatureState.Normal, 2); // 受击时切换到战备动作
                }
            }
        }

        public void OnAddSkill(uint skillID, uint skillLev, uint coldtime)
        {
            SkillDatabase database = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID, (int)skillLev);
            if (database != null)
            {
                AddCurSkill(database);
            }
            else
            {
                Log.Error("database is null skillid is " + skillID.ToString() + " lev is " + skillLev.ToString());
            }

        }
        //// 服务器通知
        public void OnPrepareUseSkill(GameCmd.stPrepareUseSkillSkillUserCmd_S cmd)
        {

            if (m_ClientGlobal == null)
            {
                return;
            }
            if (m_Master.GetCurState() != CreatureState.Dead)
            {
                bLive = true;
            }
            //服务器如果第一次发来所有的消息 此处可优化
            SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(cmd.skillid, (int)cmd.level);
            if (db != null)
            {
                if (!CurSkillDic.ContainsKey(cmd.skillid))
                {
                    CurSkillDic.Add(cmd.skillid, db);
                }
                else
                {
                    if (db.wdLevel != cmd.level)
                    {
                        CurSkillDic[cmd.skillid] = db;
                    }
                }

            }
            else
            {
                Log.LogGroup("ZDY", "skilldatabase not contain skillid {0}" , cmd.skillid);
                return;
            }
            SkillTarget = EntitySystem.EntityHelper.GetEntity((SceneEntryType)cmd.defertype, cmd.deferid);
            if (SkillTarget == null&&db.targetType == 0)
            {
                Log.LogGroup("ZDY", "没有找到服务器要攻击的目标");
                return;
            }
            //Log.LogGroup( "ZDY" , "******************** " + stateMachine.GetCurState().GetStateID().ToString() );

            if ((CreatureState)m_Master.GetCurState() == CreatureState.Move)
            {
                IPlayer p = Master as IPlayer;
                if (p != null)
                {
                    if (!p.IsMainPlayer())
                    {
                        Log.LogGroup("ZDY", "other player is move -----------------");
                        CurSkillID = cmd.skillid;
                        m_lastSkillId = cmd.skillid;
                        p.SendMessage(EntityMessage.EntityCommond_IgnoreMoveAction, false);
                        BreakSkill();
                        FreeSkill(cmd.skillid, this, m_uDamageID);
                        //释放技能攻击

                        stateMachine.ChangeState((int)SkillState.Prepare, this);
                    }
                }

                if (cmd.usertype == (int)SceneEntryType.SceneEntry_NPC)
                {//如果是怪物 移动中就 等结束时候在播放
                    npcWillUseSkillCmd = cmd;
                }
            }
            else
            {
                int stateID = stateMachine.GetCurState().GetStateID();
                if (stateMachine.GetCurState().GetStateID() == (int)SkillState.None)
                {
                    CurSkillID = cmd.skillid;
                    m_lastSkillId = cmd.skillid;
                    FreeSkill(cmd.skillid, this, m_uDamageID);
                    //释放技能攻击
                    stateMachine.ChangeState((int)SkillState.Prepare, this);

                }
                else
                {
                    IPlayer p = Master as IPlayer;
                    if (p != null)
                    {
                        if (!p.IsMainPlayer())
                        {
                            CurSkillID = cmd.skillid;
                            m_lastSkillId = cmd.skillid;
                            p.SendMessage(EntityMessage.EntityCommond_IgnoreMoveAction, false);
                            BreakSkill();
                            FreeSkill(cmd.skillid, this, m_uDamageID);
                            //释放技能攻击
                            stateMachine.ChangeState((int)SkillState.Prepare, this);
                        }

                
                    }
                    if (cmd.usertype == (int)SceneEntryType.SceneEntry_NPC)
                    {//如果是怪物 不在移动中直接放
                        CurSkillID = cmd.skillid;
                        m_lastSkillId = cmd.skillid;
                        BreakSkill();
                        FreeSkill(cmd.skillid, this, m_uDamageID);
                        //释放技能攻击
                        stateMachine.ChangeState((int)SkillState.Prepare, this);
                        npcWillUseSkillCmd = null;
                    }
                }
            }

        }

        //  进度条通知
        public void OnInterruptSkill(uint uUserID, uint uTime, uint uType, uint actionType)
        {
            if (m_ClientGlobal == null)
            {
                return;
            }

            if (m_ClientGlobal.MainPlayer.GetID() == uUserID)
            {
                // 开启进度条
            }

        }
        public void OnUseSkillFailed(uint userid, uint skillid)
        {
            //技能被打断也是执行失败
           Dictionary<uint,SkillDatabase> m_Dic =  GetCurSkills();
            if(m_Dic.ContainsKey(skillid))
            {
                BreakSkill();
            }
          
        }
        // 进度条结束通知 技能打断通知
        public void OnInterruptEventSkill(uint uEventType)
        {
            //打断 或者结束
            if (uEventType == (uint)GameCmd.stNotifyUninterruptEventMagicUserCmd_CS.EventType.EventType_Break)
            {
                BreakSkill();
            }
        }
        /// <summary>
        /// 打断技能
        /// </summary>
       public void BreakSkill()
        {
            if (CurSkillID == 106)
            {
                Log.LogGroup("ZDY", "fenshenzhan  break");
            }
            SkillEffect effect = m_caster.EffectNode;
            if (m_caster != null)
            {//打断技能只调用stop
                m_caster.Stop();
            }

            m_Master.SendMessage(EntityMessage.EntityCommand_ClearAniCallback, null);
            //打断技能
            if (stateMachine.GetCurStateID() != (int)SkillState.None)
            {
                stateMachine.ChangeState((int)SkillState.None, null);
            }
            if (m_Master.GetCurState() == CreatureState.Move)
            {
                m_Master.ChangeState(CreatureState.Normal, (int)1);
            }
        }
        Vector3 GetSendTargetPos(SkillDatabase db, Vector3 targetPos)
        {
            if(db == null)
            {
                Log.Error("db is null");
            }
            if (db.dwMoveType == (uint)Client.SkillMoveType.FastMove)
            {
                //处理位移技能
                if (GetSkillTarget() != null)
                {
                    if (Master == null)
                    {
                        return targetPos;
                    }
                    Vector3 dir = targetPos - Master.GetPos();
                    targetPos = targetPos - dir.normalized * 1f;
                    return targetPos;
                }
            }
            return targetPos;
        }
        // 同步技能消息
        public void SyncSkill(uint uSkillID, Client.IEntity target, Vector3 targetPos)
        {
 
            if(uSkillID == 0)
            {
                Log.Error("SyncSkill skillid is 0");
                return;
            }
            // 填充数据
            Vector3 pos = m_Master.GetPos();

            SkillDatabase database = GetSkillDataBase(uSkillID);

            Client.IControllerSystem ctrlSys = m_ClientGlobal.GetControllerSystem();
            if (ctrlSys == null)
            {
                return;
            }

            Client.IController ctrl = ctrlSys.GetActiveCtrl();
            if (ctrl == null)
            {
                return;
            }
            if (targetPos == Vector3.zero)
            {
                int skillerror = 0;
                bool bCanUse = ctrl.FindTargetBySkillID(CurSkillID, ref targetPos, ref target, out skillerror);
                if (!bCanUse)
                {
                    Log.LogGroup("ZDY", "不符合规则 不能放技能");
                    return;
                }
            }


            var cmd = new GameCmd.stMultiAttackUpMagicUserCmd_C();
            cmd.dwAttackerID = (uint)m_ClientGlobal.MainPlayer.GetID();
            cmd.wdSkillID = uSkillID;
            cmd.byEntryType = SceneEntryType.SceneEntry_Player;

            cmd.srcx = (uint)(pos.x);
            cmd.srcy = (uint)(-pos.z);
            Vector3 rot = m_Master.GetRotate();
            cmd.byDirect = (uint)rot.y;

            Vector3 sendTargetPos = GetSendTargetPos(database, targetPos);
            cmd.x = (uint)(sendTargetPos.x * 100);
            cmd.y = (uint)(-sendTargetPos.z * 100);

            if (target != null)
            {
                stMultiAttackUpMagicUserCmd_C.Item item = new stMultiAttackUpMagicUserCmd_C.Item();
                if (target != null)
                {
                    item.byEntryType = (target.GetEntityType() == EntityType.EntityType_Player ? SceneEntryType.SceneEntry_Player : SceneEntryType.SceneEntry_NPC);
                    item.dwDefencerID = (uint)target.GetID();
                }
                else
                {
                    item.dwDefencerID = 0;
                }

                cmd.data.Add(item);
               
            }
            m_uDamageID++;
            cmd.tmpid = m_uDamageID;

            // 填充数据
            m_ClientGlobal.netService.Send(cmd);
        }
        public void RequestUsePetSkill(stMultiAttackUpMagicUserCmd_C cmd)
        {
            if(cmd == null)
            {
                return;
            }
            m_uDamageID++;
            cmd.tmpid = m_uDamageID;

            // 填充数据
            m_ClientGlobal.netService.Send(cmd);
        }
        #endregion

        void PopUPDamage()
        {/*
            List<stMultiAttackDownMagicUserCmd_S> cmdList = damagerManager.GetDamageList(m_Master.GetUID());
            if (cmdList != null)
            {
                for (int i = 0; i < cmdList.Count; i++)
                {
                    stMultiAttackDownMagicUserCmd_S cmd = cmdList[i];
                    if (cmd != null)
                    {
                        List<GameCmd.stMultiAttackDownMagicUserCmd_S.stDefender> defenderList = cmd.data;
                        for (int j = 0; j < defenderList.Count; j++)
                        {
                            GameCmd.stMultiAttackDownMagicUserCmd_S.stDefender defender = defenderList[j];
                            if (defender != null)
                            {

                                IEntitySystem es = m_ClientGlobal.GetEntitySystem();
                                if (es == null)
                                {
                                    return;
                                }
                                IEntity en = es.FindEntity(defender.dwDefencerID);
                                if (en != null)
                                {
                                    ISkillPart part = en.GetPart(EntityPart.Skill) as ISkillPart;
                                    if (part != null)
                                    {
                                        ISkillPart mySkillPart = Master.GetPart(EntityPart.Skill) as ISkillPart;
                                        if (mySkillPart != null)
                                        {
                                            mySkillPart.RemoveDamage(cmd, Master.GetUID());
                                        }
                                        part.AddDamage(cmd, en.GetUID());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            */
        }
        void EntityStopMove(int eventID, object param)
        {
            if (npcWillUseSkillCmd != null)
            {
                //   if ( stateMachine.GetCurState().GetStateID() == (int)SkillState.None )
                {
                    CurSkillID = npcWillUseSkillCmd.skillid;
                    m_lastSkillId = npcWillUseSkillCmd.skillid;
                    FreeSkill(npcWillUseSkillCmd.skillid, this, m_uDamageID);
                    //释放技能攻击
                    stateMachine.ChangeState((int)SkillState.Prepare, this);
                    npcWillUseSkillCmd = null;
                }
            }
        }
        /// <summary>
        ///释放技能表现效果
        /// </summary>
        /// <param name="skill_id"></param>
        /// <param name="skillPart"></param>
        /// <param name="attackActionID"></param>
        void FreeSkill(uint skill_id, ISkillPart skillPart, uint attackActionID)
        {
            if(m_bHideOtherPlayer)
            {
                if(IsMainPlayer())
                {
                    m_caster.InitCastSkill(skill_id, skillPart, attackActionID);
                }
                else
                {
                    INPC npc = m_Master as INPC;
                     if (npc != null)
                     {
                         if (!npc.IsMainPlayerSlave())
                         {
                             m_caster.InitCastSkill(skill_id, skillPart, attackActionID);
                         }
                     }
                }
            }
            else
            {
                m_caster.InitCastSkill(skill_id, skillPart, attackActionID);
            }
          
        }
    }
}
