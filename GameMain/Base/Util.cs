using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using UnityEngine;
using Engine.Utility;

//--------------------------
// 公共功能模块，全静态函数
//--------------------------
public class Util
{
    public static Component AddComponent(GameObject obj, string component)
    {
        Type t = Assembly.GetExecutingAssembly().GetType(component);

        if (t != null)
        {
            return obj.AddComponent(t);
        }
        else
        {
            //Log.Error("no compent: " + component);
            return null;
        }
    }

    public static void SetMainCamera(Camera c)
    {
        ms_mainCameraObj = c.gameObject;
    }

    static GameObject ms_mainCameraObj;
    //返回主相机物体
    public static GameObject MainCameraObj
    {
        get
        {
            if (ms_mainCameraObj == null)
            {
                ms_mainCameraObj = GameObject.Find("/MainCamera");
            }

            return ms_mainCameraObj;
        }
    }

    static GameObject ms_uiCameraObj;
    //返回UI相机物体
    public static GameObject UICameraObj
    {
        get
        {
            if (ms_uiCameraObj == null)
            {
                ms_uiCameraObj = GameObject.Find("/ui_root/Camera");
            }

            return ms_uiCameraObj;
        }
    }

    static GameObject ms_uiRootObj;
    //返回UI根节点
    public static GameObject UIRoot
    {
        get
        {
            if (ms_uiRootObj == null)
            {
                ms_uiRootObj = GameObject.Find("/ui_root");
            }

            return ms_uiRootObj;
        }
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



    //这里，是做多语言翻译用的（预留）
    public static string GetText(string val)
    {
        return val;
    }

    public static void ResetTransform(Transform node)
    {
        node.localPosition = Vector3.zero;
        node.localRotation = Quaternion.identity;
        node.localScale = Vector3.one;
    }

    public static void AttachChild(Transform parent, Transform child, bool reset=true)
    {
        child.parent = parent;
        if (reset)
            ResetTransform(child);
    }

    public static void SetLayerRecursive(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach(Transform child in obj.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }

    /// <summary>
    /// 重新设置父节点，且不改变当前transform的local属性
    /// </summary>
    /// <param name="current"></param>
    /// <param name="newParent"></param>
    public static void SetParentOnly(Transform current, Transform newParent)
    {
        var oldPosition = current.localPosition;
        var oldScale = current.localScale;
        var oldRotation = current.localRotation;
        var oldEulerAngles = current.localEulerAngles;

        current.SetParent(newParent);

        current.localPosition = oldPosition;
        current.localScale = oldScale;
        current.localRotation = oldRotation;
        current.localEulerAngles = oldEulerAngles;
    }

    //用户本地配置文件
    //static ConfigFile ms_user_saving_config;
    //public static ConfigFile UserSavingFile
    //{
    //    get{
    //        if (ms_user_saving_config == null)
    //        {
    //            if (Directory.Exists(BaseParams.ResDir + "etc") == false)
    //            {
    //                Directory.CreateDirectory(BaseParams.ResDir + "etc");
    //            }

    //            ms_user_saving_config = new ConfigFile();
    //            ms_user_saving_config.Load(BaseParams.ResDir + "etc/user.ini");
    //        }

    //        return ms_user_saving_config;
    //    }
    //}
    /// <summary>
    /// 添加子节点
    /// </summary>
    public static void AddChildToTarget(Transform target, Transform child)
    {
        child.parent = target;
        child.localScale = Vector3.one;
        child.localPosition = Vector3.zero;
        child.localEulerAngles = Vector3.zero;

        ChangeChildLayer(child, target.gameObject.layer);
    }

    /// <summary>
    /// 修改子节点Layer  NGUITools.SetLayer();
    /// </summary>
    public static void ChangeChildLayer(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; ++i)
        {
            Transform child = t.GetChild(i);
            child.gameObject.layer = layer;
            ChangeChildLayer(child, layer);
        }
    }



}
