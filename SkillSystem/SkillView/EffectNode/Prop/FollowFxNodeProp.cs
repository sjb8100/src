using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using UnityEngine;
namespace SkillSystem
{
    // 跟随特效
     [System.Serializable]
    public class FollowFxNodeProp : HitNodeProp
    {
        public string fx_name = "";
        public string target_name = "";
        public float speed = 10.0f;
        public float acce = 0.0f;
        public float len = 5.0f;
        public float height = 1.0f;
        public float z_offset = 0.0f;
        public string attach_name = "";
        public Vector3 offset_pos = Vector3.zero;
        public string target_hitnode = "TxChest";
        public FollowFxNodeProp()
        {
            type = EF_NODE_TYPE.FOLLOW_FX;
        }
        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);

            fx_name = jsNode.GetString("fx_name", "");
            target_name = jsNode.GetString("target_name", "");
            speed = jsNode.GetFloat("speed", 0.0f);
            acce = jsNode.GetFloat("acce", 0.0f);
            len = jsNode.GetFloat("len", 5.0f);
            height = jsNode.GetFloat("height", 1.0f);
            z_offset = jsNode.GetFloat("z_offset", 0.0f);
            offset_pos.x = jsNode.GetFloat( "x" , 0 );
            offset_pos.y = jsNode.GetFloat( "y" , 0 );
            offset_pos.z = jsNode.GetFloat( "z" , 0 );
            attach_name = jsNode.GetString( "attach_name","" );
            target_hitnode = jsNode.GetString("target_hitnode", "");
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);

            jsNode["fx_name"] = EffectUtil.GetFxPath(fx_name);
            jsNode["target_name"] = target_name;
            jsNode["speed"] = speed;
            jsNode["acce"] = acce;
            jsNode["len"] = len;

            jsNode["height"] = height;
            jsNode["z_offset"] = z_offset;

            if ( offset_pos.x != 0.0f )
                jsNode["x"] = offset_pos.x;

            if ( offset_pos.y != 0.0f )
                jsNode["y"] = offset_pos.y;

            if ( offset_pos.z != 0.0f )
                jsNode["z"] = offset_pos.z;
            jsNode["attach_name"] = attach_name;
            jsNode["target_hitnode"] = target_hitnode;

        }

        //public override void GatherResFile(List<ResItem> resItemList)
        //{
        //    //EffectDatabase.Me.GatherResFile("skill/" + fx_name, resItemList);
        //    //if (hit_fx.Length>0)
        //    //    EffectDatabase.Me.GatherResFile("hit/" + hit_fx, resItemList);
        //}
    }
}
