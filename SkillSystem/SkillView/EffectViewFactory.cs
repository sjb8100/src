using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using Client;
using UnityEngine;
using Engine;
using Engine.Utility;
using UnityEngine.Profiling;

namespace SkillSystem
{
    // 特效效果工厂
    public class EffectViewFactory
    {
        static EffectViewFactory s_Inst = null;
        public static EffectViewFactory Instance()
        {
            if (null == s_Inst)
            {
                s_Inst = new EffectViewFactory();
            }

            return s_Inst;
        }

        public uint CreateEffect(long lTargetID, uint nEffectViewID)
        {
            IPlayer player = SkillSystem.GetClientGlobal().MainPlayer;
            if (player == null)
            {
                Log.LogGroup("ZDY", "player is null");
                return 0;
            }
            IEntity target = EntitySystem.EntityHelper.GetEntity(lTargetID);
            FxResDataBase edb = GameTableManager.Instance.GetTableItem<FxResDataBase>(nEffectViewID);
            if (edb != null && target != null)
            {
                if (nEffectViewID == 1002 && EntitySystem.EntityHelper.IsMainPlayer(target))
                {
                    //  Log.Error("nEffectViewID:{0}", nEffectViewID);
                }
                if(target.GetNode() == null)
                {
                    Log.LogGroup("ZDY", "target getnode is null");
                    return 0;
                }
                Transform enTrans = target.GetTransForm();
                if(enTrans == null)
                {
                    Log.LogGroup("ZDY", "target getnode getransform is null");
                    return 0;
                }
                if(player == null)
                {
                    return 0;
                }
          
                if(player.GetTransForm() == null)
                {
                    Log.LogGroup("ZDY", "player GetTransForm is null");
                    return 0;
                }

                bool bRide = (bool)target.SendMessage(EntityMessage.EntityCommond_IsRide, null);

                Vector3 tarToPlayer = enTrans.position - player.GetTransForm().position;
                tarToPlayer.y = 0;
                Vector3 cross = Vector3.Cross(tarToPlayer, Vector3.forward);
                float ang = Vector3.Angle(tarToPlayer, Vector3.forward);
                if(cross.y < 0)
                {
                    ang = 360 - ang;
                }
              //  Log.Error("旋转角度 " + ang);
                Vector3 rot = Quaternion.AngleAxis(ang, Vector3.up).eulerAngles;
                AddLinkEffect node = new AddLinkEffect();
                node.nFollowType = (int)edb.flowType;
                node.rotate = -rot;// new Vector3(edb.rotate[0], edb.rotate[1], edb.rotate[2]);
                node.vOffset = bRide ? new Vector3(edb.rideoffset[0], edb.rideoffset[1], edb.rideoffset[2]) : new Vector3(edb.offset[0], edb.offset[1], edb.offset[2]);

                // 使用资源配置表
                ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<ResourceDataBase>(edb.resPath);
                if (resDB == null)
                {
                    Log.LogGroup("ZDY", "EffectViewFactory:找不到特效资源路径配置{0}", edb.resPath);
                    return 0;
                }
                node.strEffectName = resDB.strPath;
                node.strLinkName = edb.attachNode;
                if (edb.bFollowRole)
                {
                    if (node.strEffectName.Length != 0)
                    {
                        int id = (int)target.SendMessage(EntityMessage.EntityCommand_AddLinkEffect, node);
                        return (uint)id;
                    }
                }
                else
                {
                    int level = 0;
                    if (target != null)
                    {
                        ISkillPart sp = target.GetPart(EntityPart.Skill) as ISkillPart;
                        if (sp != null)
                        {
                            level = sp.FxLevel;
                        }
                    }
                    return SkillEffectManager.Helper.ReqPlayHitFx(node.strEffectName, target.GetPos(), node.rotate, Vector3.one,level);
                }

            }
            return 0;
        }

        public void RemoveEffect(long lTargetID, uint uEffectID)
        {
            IEntity target = EntitySystem.EntityHelper.GetEntity(lTargetID);
            if (target != null)
            {
                target.SendMessage(EntityMessage.EntityCommand_RemoveLinkEffect, (int)uEffectID);
            }
        }

        //
        public bool CreateSkillEffect()
        {
            return false;
        }
    }
}
