
//*************************************************************************
//	创建日期:	2017/11/2 星期四 9:23:21
//	文件名称:	SkillEffectProp
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LitJson;
namespace SkillSystem
{
   
    public class SkillEffectProp : ScriptableObject, ISerializationCallbackReceiver
    {
        string m_name = "";
        /// <summary>
        /// key 是时间
        /// </summary>
        public SortedDictionary<float, EffectNodeConfig> m_EffectProps = new SortedDictionary<float, EffectNodeConfig>();
        #region ISerializationCallbackReceiver
        public List<EffectNodeConfig> _NodeList = new List<EffectNodeConfig>();
        public List<float> _TimeKey = new List<float>();
        // 摘要: 
        //     Implement this method to receive a callback after Unity deserializes your
        //     object.
        public void OnAfterDeserialize()
        {
            m_EffectProps = new SortedDictionary<float, EffectNodeConfig>();

            for (int i = 0; i != Math.Min(_TimeKey.Count, _NodeList.Count); i++)
            {

                m_EffectProps.Add(_TimeKey[i], _NodeList[i]);
            }

        }

        public void OnBeforeSerialize()
        {
            _TimeKey.Clear();
            _NodeList.Clear();

            foreach (var kvp in m_EffectProps)
            {
                _TimeKey.Add(kvp.Key);

                _NodeList.Add(kvp.Value);
            }
        }
        #endregion
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public float MaxTime
        {
            get
            {
                float fMaxTime = 0.0f;
                SortedDictionary<float, EffectNodeConfig>.Enumerator iter = m_EffectProps.GetEnumerator();
                while (iter.MoveNext())
                {
                    //fMaxTime = iter.Current.Key * 0.001f; // 时间：秒
                    fMaxTime = iter.Current.Key;
                }

                return fMaxTime;
            }
        }

        public void GatherResFile(ref List<string> resItemList, bool gather_all)
        {
            //m_EffectProps;
            SortedDictionary<float, EffectNodeConfig>.Enumerator iter = m_EffectProps.GetEnumerator();
            while (iter.MoveNext())
            {
                for (int i = 0; i < iter.Current.Value.configlist.Count; ++i)
                {
                    EffectNodeProp nodeProp = iter.Current.Value.configlist[i];
                    if (nodeProp.bActive)
                    {
                        if (gather_all == false)
                        {
                            if (nodeProp.type != EF_NODE_TYPE.ATTACH_FX
                                && nodeProp.type != EF_NODE_TYPE.PLACE_FX)
                            {
                                nodeProp.GatherResFile(ref resItemList);
                            }
                        }
                        else
                        {
                            nodeProp.GatherResFile(ref resItemList);
                        }
                    }
                }


            }
        }

        public void Add(EffectNodeProp n)
        {
            //  List<EffectNodeProp> nodes;
            EffectNodeConfig config;
            //  int nKey = (int)(n.time * 1000.0f);
            if (m_EffectProps.TryGetValue(n.time, out config))
            {
                //nodes.Add(n);
                config.AddNodeConfig(n);
            }
            else
            {
                config = ScriptableObject.CreateInstance<EffectNodeConfig>();//new EffectNodeConfig();
                config.AddNodeConfig(n);
                m_EffectProps.Add(n.time, config);
            }
        }

        public void Remove(EffectNodeProp n)
        {
            // List<EffectNodeProp> nodes;
            EffectNodeConfig config;
            // int nKey = (int)(n.time * 1000.0f);
            if (m_EffectProps.TryGetValue(n.time, out config))
            {
                config.RemoveNodeConfig(n);
            }
        }

        public void Load(JsonData effect_root)
        {
            m_name = effect_root.GetString("name", "");

            JsonData node_list = effect_root["nodes"];

            int hit_index = 0;

            for (int n = 0; n < node_list.Count; n++)
            {
                // skillEffect.effect_node = effect_root[n];
                JsonData js_node = node_list[n];
                EF_NODE_TYPE type = (EF_NODE_TYPE)js_node.GetInt32("type", 0);
                EffectNodeProp effect_node = CreateEffectProp(type);
                if (effect_node != null)
                {
                    //effect_node.type = type;
                    //effect_node.time = js_node.GetFloat("time", 0.0f);
                    //effect_node.enable = js_node.GetBool("enable", true);

                    effect_node.Read(js_node);

                    if (effect_node.bActive && effect_node.is_hit_node)
                    {
                        HitNodeProp hitNode = effect_node as HitNodeProp;
                        hitNode.hit_index = hit_index;

                        //发射特效，伤害半径为0，不检测伤害
                        ArrowFxNodeProp arrow_node = hitNode as ArrowFxNodeProp;
                        if (arrow_node != null)
                        {
                            if (arrow_node.radius > 0.0001f)
                            {
                                hit_index++;
                            }
                        }
                        else
                        {
                            hit_index++;
                        }
                    }

                    Add(effect_node);
                }
            }
        }

        public void Save(JsonData json_root)
        {
            json_root["name"] = m_name;

            JsonData node_list = new JsonData();
            node_list.SetJsonType(JsonType.Array);
            json_root["nodes"] = node_list;

            SortedDictionary<float, EffectNodeConfig>.Enumerator iter = m_EffectProps.GetEnumerator();
            while (iter.MoveNext())
            {
                for (int i = 0; i < iter.Current.Value.configlist.Count; i++)
                {
                    EffectNodeProp e = iter.Current.Value.configlist[i];
                    JsonData e_node = new JsonData();
                    //e_node["type"] = (int)e.type;
                    //e_node["time"] = e.time;
                    //e_node["enable"] = e.enable;

                    node_list.Add(e_node);

                    e.Write(e_node);
                }
            }
        }

        public static EffectNodeProp CreateEffectProp(EF_NODE_TYPE type)
        {
            switch (type)
            {
                case EF_NODE_TYPE.ACTION:
                    return ScriptableObject.CreateInstance<ActionNodeProp>();// new ActionNodeProp();

                case EF_NODE_TYPE.ATTACH_FX:
                    return ScriptableObject.CreateInstance<AttachFxNodeProp>();// new AttachFxNodeProp();

                case EF_NODE_TYPE.EVENT:
                    return ScriptableObject.CreateInstance<EventNodeProp>();// new EventNodeProp();

                case EF_NODE_TYPE.ARROW_FX:
                    return ScriptableObject.CreateInstance<ArrowFxNodeProp>();// new ArrowFxNodeProp();

                case EF_NODE_TYPE.FOLLOW_FX:
                    return ScriptableObject.CreateInstance<FollowFxNodeProp>();// new FollowFxNodeProp();

                case EF_NODE_TYPE.PLACE_FX:
                    return ScriptableObject.CreateInstance<PlaceFxNodeProp>();// new PlaceFxNodeProp();

                case EF_NODE_TYPE.SOUND:
                    return ScriptableObject.CreateInstance<SoundNodeProp>();// new SoundNodeProp();

                case EF_NODE_TYPE.MOVE:
                    return ScriptableObject.CreateInstance<MoveNodeProp>();// new MoveNodeProp();

                case EF_NODE_TYPE.SINGLE_HIT:
                    return ScriptableObject.CreateInstance<SingleHitNodeProp>();// new SingleHitNodeProp();

                case EF_NODE_TYPE.CAMERA:
                    return ScriptableObject.CreateInstance<ShakeCameraNodeProp>();// new ShakeCameraNodeProp();

                case EF_NODE_TYPE.CastOverNode:
                    return ScriptableObject.CreateInstance<CastOverNodeProp>();// new CastOverNodeProp();

                case EF_NODE_TYPE.MULT_HIT:
                    return ScriptableObject.CreateInstance<MultHitNodeProp>();// new MultHitNodeProp();
            }

            //Log.Error("error create node, type={0}", type);

            return null;
        }
    }
}