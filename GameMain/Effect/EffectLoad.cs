using UnityEngine;
using System.Collections;

public class EffectLoad : Engine.IEffectLoad
{

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    public UnityEngine.Object LoadAsset(string strObjName, AssetBundle ab)
    {
        if (ab == null)
        {
            return null;
        }
        //临时做法
        GameObject[] gos = ab.LoadAllAssets<GameObject>();
        return gos[0];
      //  return ab.LoadAsset(strObjName);
    }

    public GameObject LoadPrefab(string strObjName, AssetBundle ab)
    {
        if (ab == null)
        {
            return null;
        }

        //临时做法
        GameObject[] gos = ab.LoadAllAssets<GameObject>();
        return gos[0];
        //return ab.LoadAsset<GameObject>(strObjName);
    }

    // 加载声音
    public AudioClip LoadAudioClip(string strClipName, AssetBundle ab)
    {
        if (ab == null)
        {
            return null;
        }

        return ab.LoadAsset<AudioClip>(strClipName);
    }
}
