using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkillSystem
{

    public class EffectUtil
    {
        public static bool DistLess2D(Vector3 pos1, Vector3 pos2, float dist)
        {
            Vector3 vdist = pos1 - pos2;
            vdist.y = 0.0f;
            return vdist.sqrMagnitude < dist * dist;
        }

        //单位化变化对象
        public static void ResetLocalTransform(Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        //递归查找子节点
        public static Transform findTransform(Transform root, string name)
        {
            Transform dt = root.Find(name);
            if (null != dt)
            {
                return dt;
            }
            else
            {
                foreach (Transform child in root)
                {
                    dt = findTransform(child, name);
                    if (dt)
                    {
                        return dt;
                    }
                }
            }
            return null;
        }

        public static void AttachNode(Transform parent, Transform child)
        {
            child.parent = parent;
            EffectUtil.ResetLocalTransform(child);
        }

        public static void AttachNode(Transform parent, Transform child,
            Vector3 pos, Vector3 angels, Vector3 scale)
        {
            child.parent = parent;
            child.localPosition = pos;
            child.localEulerAngles = angels;
            child.localScale = scale;
        }
        //主要用于编辑器写入配置文件 不在游戏运行时执行
        public static string GetFxPath(string oldPath)
        {
         
            if(oldPath.EndsWith(".prefab"))
            {
                oldPath = oldPath.Replace(".prefab", ".fx");
            }
            if(!oldPath.StartsWith("effect/skill/"))
            {
                oldPath = "effect/skill/" + oldPath;
            }
            return oldPath.ToLower();
        }
    }
}