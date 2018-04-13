using System;
using System.Text;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine;
using Client;
namespace SkillSystem
{
    public enum ResType
    {
        effect = 0,
        fx_tex,
        fx_mesh,
        sound,
        music,
        fx_shader,
        prime_hero_model,
        prime_hero_mesh,
        hero_tex,
        monster_tex,
        monster_model,
        hero_model,
        weapon_model,
        wing_model,
        common_tex,
        common_shader,
        common_prefab,
        ui_tex,
        callback_only,
        ui_altas,
        count,
    }

    public class ResItem
    {
        public delegate void OnLoadedDelgetage(ResItem item);

        public ResItem(ResType type_, string path_, int prior_)
        {
            name = path_;
            type = type_;
            //path = ResManager.GetResPath(type_, path_);
            prior = prior_;
        }

        public ResItem(ResType type_, string path_, int prior_, OnLoadedDelgetage callbackFunc_)
        {
            name = path_;
            type = type_;
            //path = ResManager.GetResPath(type_, path_);
            prior = prior_;
            callbackFunc = callbackFunc_;
        }

        public int CompareTo(object obj)
        {
            ResItem ResItem = obj as ResItem;
            if (prior < ResItem.prior)
            {
                return -1;
            }
            else if (prior > ResItem.prior)
            {
                return 1;
            }

            return 0;
        }

        public ResType type;
        public string path;
        public string name;
        public int prior = 0;
        public int used_num = 0;
        public AssetBundle ass;
        public UnityEngine.Object obj;
        public OnLoadedDelgetage callbackFunc;
    }

    public enum EF_NODE_TYPE
    {
        ACTION = 0,
        PLACE_FX,
        ATTACH_FX,
        ARROW_FX,
        FOLLOW_FX,
        EVENT,
        SOUND,
        CAMERA,
        MOVE,
        SINGLE_HIT,
        MULT_HIT,
        TIME_SCALE,
        CastOverNode,
    }
   
    //效果节点基类
    public class EffectNode //: IComparable
    {

        public bool enable = true;


        public bool is_hit_node = false;

        protected EffectNodeProp m_NodeProp = null;

        public EF_NODE_TYPE type
        {
            get { return m_NodeProp.type; }
        }
        /*
          EF_NODE_TYPE type = node.type;
            switch (type)
            {
                case EF_NODE_TYPE.ACTION:
                    {
                        ActionNode skillNode = (ActionNode)node;
                        skillNode.Play(m_Atttacker, this);
                    }
                    break;
                case EF_NODE_TYPE.ATTACH_FX:
                    {
                        AttachFxNode skillNode = node as AttachFxNode;
                        skillNode.Play(m_Atttacker, this);
                    }
                    break;
                case EF_NODE_TYPE.EVENT:
                    {
                        EventNode skillNode = node as EventNode;
                        skillNode.Play(m_Atttacker, this);
                    }
                    break;
                case EF_NODE_TYPE.ARROW_FX:
                    {
                        ArrowFxNode skillNode = node as ArrowFxNode;
                        skillNode.Play(m_Atttacker, this);
                    }
                    break;
                case EF_NODE_TYPE.FOLLOW_FX:
                    {
                        FollowFxNode skillNode = node as FollowFxNode;
                        skillNode.Play(m_Atttacker, this);
                    }
                    break;
                case EF_NODE_TYPE.PLACE_FX:
                    {
                        PlaceFxNode skillNode = node as PlaceFxNode;
                        skillNode.Play(m_Atttacker, this);
                    }
                    break;
                case EF_NODE_TYPE.SOUND:
                    {
                        SoundNode skillNode = node as SoundNode;
                        skillNode.Play(m_Atttacker, this);
                    }
                    break;
                case EF_NODE_TYPE.MOVE:
                    {
                        MoveNode skillNode = node as MoveNode;
                        skillNode.Play(m_Atttacker, this);
                    }
                    break;
                case EF_NODE_TYPE.SINGLE_HIT:
                    {
                        SingleHitNode skillNode = node as SingleHitNode;
                        skillNode.Play(m_Atttacker, this);
                    }
                    break;
                case EF_NODE_TYPE.CAMERA:
                    {
                        ShakeCameraNode skillNode = node as ShakeCameraNode;
                        skillNode.Play(m_Atttacker, this);
                    }
                    break;
                case EF_NODE_TYPE.CastOverNode:
                    {
                        CastOverNode skillNode = node as CastOverNode;
                        skillNode.Play(m_Atttacker, this);
                    }
                    break;
                case EF_NODE_TYPE.MULT_HIT:
                    {
                        MultHitNode skillNode = node as MultHitNode;
                        skillNode.Play(m_Atttacker, this);
                    }
                    break;
         */
        public EffectNodeProp prop
        {
            get { return m_NodeProp; }
            set
            {
                m_NodeProp = value;
            }
        }

        public virtual bool Create(EffectNodeProp prop)
        {
            m_NodeProp = prop;
            return true;
        }

        public virtual void Play(ISkillAttacker attacker, SkillEffect effect)
        {

        }
        public virtual void Stop()
        {

        }
        public virtual void Update(float dTime)
        {

        }
        public virtual void FreeToNodePool()
        {

        }
        public virtual void Dead()
        {

        }
     
        public virtual void GatherResFile(ref List<string> resItemList)
        {
            m_NodeProp.GatherResFile(ref resItemList);
        }

        public static string GetStringType(EF_NODE_TYPE ty)
        {
            string[] ty_str_s = { "角色动作", "放置特效", "挂接特效",
                                "散射特效", "跟踪特效", "触发事件",
                                "音     效", "相机震动", "位置变换", 
                                 "伤害点", "多次伤害点", "慢镜头"};

            return ty_str_s[(int)ty];
        }

        //public int CompareTo(object obj)
        //{
        //    EffectNode other_node = obj as EffectNode;
        //    if (time < other_node.time)
        //    {
        //        return -1;
        //    }
        //    else if (time > other_node.time)
        //    {
        //        return 1;
        //    }

        //    return 0;
        //}

        public static ISkillEffectHelper helper;
    }

    public abstract class HitRange
    {
        public enum Type
        {
            NONE = 0,
            CIRCLE,
            FAN,
            RECT,
            //ARROW, //发射类
            COLLIDER, //复杂类型
        }

        public Type type;

        public virtual void Read(JsonData jsNode)
        {
            type = (Type)jsNode.GetInt32("type", 0);
        }

        public virtual void Write(JsonData jsNode)
        {
            jsNode["type"] = (int)type;
        }

        public static HitRange Create(Type type)
        {
            switch (type)
            {
                case Type.FAN:
                    return new FanRange();

                case Type.RECT:
                    return new RectRange();

                case Type.CIRCLE:
                    return new CircleRange();

                case Type.COLLIDER:
                    return new CollideRange();

                default:
                    return null;
            }
        }
    }

    public class CircleRange : HitRange
    {
        public float radius = 2.0f;
        public Vector3 center;

        public CircleRange()
        {
            type = Type.CIRCLE;
        }

        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);

            radius = jsNode.GetFloat("radius", 0.0f);

            center.x = jsNode.GetFloat("center_x", 0.0f);
            center.y = jsNode.GetFloat("center_y", 0.0f);
            center.z = jsNode.GetFloat("center_z", 0.0f);
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);

            jsNode["radius"] = radius;

            jsNode["center_x"] = center.x;
            jsNode["center_y"] = center.y;
            jsNode["center_z"] = center.z;
        }
    }

    public class FanRange : HitRange
    {
        public float radius = 2.0f;
        public float start_angle = 30.0f;
        public float end_angle = -30.0f;

        //public Vector3 center;

        public FanRange()
        {
            type = Type.FAN;
        }

        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);

            radius = jsNode.GetFloat("radius", radius);

            start_angle = jsNode.GetFloat("start_angle", start_angle);
            end_angle = jsNode.GetFloat("end_angle", end_angle);

            //center.x = jsNode.GetFloat("center_x", 0.0f);
            //center.y = jsNode.GetFloat("center_y", 0.0f);
            //center.z = jsNode.GetFloat("center_z", 0.0f);
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);

            jsNode["radius"] = radius;

            jsNode["start_angle"] = start_angle;
            jsNode["end_angle"] = end_angle;

            //jsNode["center_x"] = center.x;
            //jsNode["center_y"] = center.y;
            //jsNode["center_z"] = center.z;
        }
    }

    public class RectRange : HitRange
    {
        public float w = 2.0f;
        public float h = 1.0f;
        public Vector3 center;

        public RectRange()
        {
            type = Type.RECT;
        }

        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);

            center.x = jsNode.GetFloat("center_x", 0.0f);
            center.y = jsNode.GetFloat("center_y", 0.0f);
            center.z = jsNode.GetFloat("center_z", 0.0f);

            w = jsNode.GetFloat("w", 0.0f);
            h = jsNode.GetFloat("h", 0.0f);
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);

            jsNode["w"] = w;
            jsNode["h"] = h;

            jsNode["center_x"] = center.x;
            jsNode["center_y"] = center.y;
            jsNode["center_z"] = center.z;
        }
    }

    public class CollideRange : HitRange
    {
        public CollideRange()
        {
            type = Type.COLLIDER;
        }
    }

    public abstract class HitNode : EffectNode
    {
        public int hit_index = 0;
        public bool hit_back = true;
        public bool hit_fly = true;
        public string hit_fx = "";

        //释放时的技能 ID
        public uint skill_id = 0;
        protected IEntity skillMaster;
        protected uint effectID;

        public uint m_uDamageID = 0;
        protected HitNode()
        {
            is_hit_node = true;
            //range_list = new List<HitRange>();
        }
        public override void Dead()
        {
            //throw new NotImplementedException();
        }
        public override void Stop() 
        {
          
        }
        public override void Update(float dTime) { }
      
        
    }

    public class CastOverNode : EffectNode
    {
        public CastOverNode()
        {
            
        }
        public override void Dead()
        {
        
        }
        public override void Play(ISkillAttacker attacker, SkillEffect se)
        {
        }

        public override void Stop() 
        {
         
        }
        public override void Update(float dTime) { }

        //public override void Read(JsonData jsNode)
        //{
        //}

        //public override void Write(JsonData jsNode)
        //{
        //}
    }
}