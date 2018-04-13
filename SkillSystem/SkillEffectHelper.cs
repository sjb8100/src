using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using SkillSystem;
using Engine.Utility;
using Engine;
using Client;

public class SkillEffectHelper : ISkillEffectHelper
{
    enum FxType
    {
        ATTACH = 0,
        PLACE,
    }

    class FxItem
    {
        public string name = "";
        public Vector3 position = Vector3.zero;
        public Vector3 rotation = Vector3.zero;
        public Vector3 scale = Vector3.one;
        public Transform attach_node = null;
        public FxType type = FxType.PLACE;
    }

    //技能特效目录
    public static string FxDir = "effect/skill/";

    //技能音效目录
    public static string SndDir = "audio/";

    static SkillEffectHelper ms_instance;
    static public SkillEffectHelper Instance
    {
        get
        {
            if(ms_instance == null)
            {
                ms_instance = new SkillEffectHelper();
            }
            return ms_instance;
        }
    }

    List<FxItem> m_waitItemList = new List<FxItem>();
    List<FxItem> m_delItemList = new List<FxItem>();
    /// <summary>
    /// 技能需要在结束时销毁的特效id
    /// </summary>
    Dictionary<long , List<uint>> effectIDDic = new Dictionary<long , List<uint>>(); 
   
 

    Transform ms_place_fx_root;
    public Transform PlaceFxRoot
    {
        get
        {
            if (ms_place_fx_root == null)
            {
                GameObject obj = GameObject.Find("/place_fx_root");
                if (obj == null)
                {
                    obj = new GameObject("place_fx_root");
                    EffectUtil.ResetLocalTransform(obj.transform);
                }

                ms_place_fx_root = obj.transform;
            }

            return ms_place_fx_root;
        }
    }
 
    public byte[] OpenFile(string path)
    {
        byte[] buff = Engine.Utility.FileUtils.Instance().ReadFile(path);
        return buff;
    }

    public Camera GetPlayerCamera()
    {
        return null;
        //return ThirdCameraController.Me.GetComponent<Camera>();
    }

    public void ReqPlaySound(string snd_name)
    {
      
        Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
        if (audio != null)
        {
            audio.StopMusic();
        }
        //Camera.main
        audio.PlayEffect(Camera.main.gameObject,snd_name);
    }
    
        /// <summary>
    /// //挂接特效
    /// </summary>
    /// <param name="fx_name"></param>
    /// <param name="attach_node">挂节点</param>
    /// <param name="position">偏移位置</param>
    /// <param name="scale">缩放</param>
    public uint ReqPlayFx(string fx_name , Transform attach_node , Vector3 position , Vector3 scale,int level = 0)
    {
        Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
        if ( rs == null )
        {
            return 0;
        }
        string path = fx_name;
        if(fx_name.EndsWith(".prefab"))
        {//先做下对老的兼容
            path = (FxDir + fx_name).Replace(".prefab", ".fx").ToLower();
        }

        IEffect effect = null;
        rs.CreateEffect(ref path, ref effect, (obj) =>
          {
              if (obj == null)
              {
                  Log.Error("ReqPlayFx obj ================ fx load failed: " + path);
                  return;
              }
              if (obj.GetNodeTransForm() == null)
              {
                  Log.Error("ReqPlayFx GetNodeTransForm ================ fx load failed: " + path);
                  return;
              }
              DoPlay(obj.GetNodeTransForm().gameObject, attach_node, position, scale);
            
          },nLevel:level);
       return effect.GetID();
    }


    //放置特效
    public uint ReqPlayFx(string fx_name, Vector3 position, Vector3 rotation, Vector3 scale, int level = 0 )
    {

        Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
        if (rs == null)
        {
            return 0;
        }

        string path = fx_name;
        if (fx_name.EndsWith(".prefab"))
        {//先做下对老的兼容
            path = (FxDir + fx_name).Replace(".prefab", ".fx").ToLower() ;
        }

        IEffect effect = null;
        rs.CreateEffect(ref path, ref effect, (obj) =>
         {
             if (obj == null)
             {
                 Log.LogGroup("ZDY", "fx load failed: {0}" , path);
                 return ;
             }
             if (obj.GetNodeTransForm() == null)
             {
                 return ;
             }
             DoPlay(obj.GetNodeTransForm().gameObject, position, rotation, scale);
          
         },nLevel:level);
        return effect.GetID();
  
    }
    //播放受击特效
    public uint ReqPlayHitFx(string fx_name, Vector3 position, Vector3 rotation, Vector3 scale, int level = 0 )
    {
        Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
        if (rs == null)
        {
            return 0;
        }
        if(string.IsNullOrEmpty(fx_name))
        {
            return 0;
        }
        string path = fx_name;// (FxDir + fx_name).Replace(".prefab", ".fx").ToLower();

        IEffect effect = null;
        rs.CreateEffect(ref path, ref effect, (obj) =>
        {
            if (obj == null)
            {
                Log.LogGroup("ZDY", "fx load failed: {0}" , path);
                return;
            }
            if (obj.GetNodeTransForm() == null)
            {
                return;
            }
            DoPlay(obj.GetNodeTransForm().gameObject, position, rotation, scale);
        },nLevel:level);
        return effect.GetID();

    }
    

    void DoPlay(GameObject fxObj, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        fxObj.transform.parent = PlaceFxRoot;
        fxObj.transform.eulerAngles = rotation;
        fxObj.transform.localPosition = position;
        fxObj.transform.localScale = scale;
    }

    void DoPlay(GameObject fxObj, Transform attach_node, Vector3 pos, Vector3 scale)
    {
  
        fxObj.transform.parent = attach_node;
        EffectUtil.AttachNode(attach_node, fxObj.transform);
        fxObj.transform.localPosition = pos;
        fxObj.transform.localScale = scale;
    }
}
