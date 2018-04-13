using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using Client;
namespace SkillSystem
{
    //挂接特效，挂接在角色的某一个节点上
    public class AttachFxNode : EffectNode
    {
        uint effectID;
        IEntity skillMaster;
        public AttachFxNode()
        {

        }


        public override void Stop() 
        {
            if(skillMaster != null)
            {
            
                skillMaster.SendMessage( EntityMessage.EntityCommand_RemoveLinkEffect , (int)effectID );
            }
        }
        public override void Dead()
        {

        }
        public override void Update(float dTime) { }
        public override void FreeToNodePool()
        {
         
        }
        public override void Play(ISkillAttacker attacker, SkillEffect se)
        {
            AttachFxNodeProp prop = m_NodeProp as AttachFxNodeProp;
            if(prop==null)
            {
                return;
            }
            int level = 0;
            if (attacker != null)
            {
                if (attacker.GetSkillPart() != null)
                {
                    level = attacker.GetSkillPart().FxLevel;
                }
            }
            skillMaster = attacker.GetGameObject();
            AddLinkEffect node = new AddLinkEffect();
            node.nFollowType = (int)0;
            node.rotate = Vector3.zero;
            node.vOffset = prop.offset_pos;
            node.nLevel = level;
          //  string effectName = prop.fx_name.Replace( "prefab" , "fx" );
           // node.strEffectName = SkillEffectHelper.FxDir + effectName;
            node.strEffectName = prop.fx_name;
            node.strLinkName = prop.attach_name;
            if(skillMaster != null)
            {
                effectID = (uint)(int)skillMaster.SendMessage( EntityMessage.EntityCommand_AddLinkEffect , node );
            }
        }

    }
}