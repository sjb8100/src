using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Engine.Utility;
using Engine;
using DG.Tweening;
using DG.Tweening.Core;

class FlyFont : IObjectReset
{
    GameObject go = null;
    MeshText mt = null;
    DamageType fontType;
    public FlyFont()
    {

    }
    float m_lastTime = 0;
    public static uint GetFontResIDByType(DamageType type)
    {
        uint resID = 0;
        switch(type)
        {
            case DamageType.AddExp:
                resID = (uint)GridID.Fightnum_addexp;
                break;
            case DamageType.AddHp:
                resID = (uint)GridID.Fightnum_addhp;
                break;
            case DamageType.Buff:
                resID = (uint)GridID.Fightnum_buff;
                break;
            case DamageType.Critical:
                resID = (uint)GridID.Fightnum_critical;
                break;
            case DamageType.Doge:
                resID = (uint)GridID.Fightnum_doge;
                break;
            case DamageType.Normal:
                resID = (uint)GridID.Fightnum_normal;
                break;
            case DamageType.Resist:
                resID = (uint)GridID.Fightnum_resist;
                break;
        }
        return resID;
    }
    public DamageType GetDamageType()
    {
        return fontType;
    }
    public void InitDamageType(DamageType type)
    {
        if (go == null)
        {
            fontType = type;
            uint resID = GetFontResIDByType(type);
            //List<string> depend = null;
            //StringBuilder sb = new StringBuilder("prefab/FightNum_");
            //sb.Append(type.ToString());
            //string name = sb.ToString();
            UnityEngine.Object obj = UIManager.GetResGameObj(resID);
            if (obj != null)
            {
                go = GameObject.Instantiate(obj) as GameObject;
                if (go != null)
                {
                    go.SetActive(true);
                    mt = go.GetComponent<MeshText>();
                    if (mt == null)
                    {
                        mt = go.AddComponent<MeshText>();

                    }

                    mt.Init();
                    //animator = go.GetComponent<FlyFontAlpha>();
                    //if (animator == null)
                    //{
                    //    animator = go.AddComponent<FlyFontAlpha>();
                    //    animator.enabled = false;
                    //}
                }

                DOTweenAnimation[] aniArray = GetTweens();
                for (int i = 0; i < aniArray.Length; i++)
                {
                    DOTweenAnimation ani = aniArray[i];
                    if (ani.animationType == DOTweenAnimationType.Color)
                    {
                        ani.tween.Rewind();
                        ani.tween.Kill();
                        if (ani.isValid)
                        {
                            ani.CreateTween();
                            ani.tween.Play();
                        }
                    }

                }
            }
        }

    }

    public Transform GetFontTransform()
    {
        if (go != null)
        {
            return go.transform;
        }
        return null;
    }
    public DOTweenAnimation GetDoTween()
    {
        if (go != null)
        {
            DOTweenAnimation[] ani = go.GetComponents<DOTweenAnimation>();
          
            for (int i = 0; i < ani.Length;i++ )
            {
                var tween = ani[i];
                if (tween.id == "max")
                {
                    return tween;
                }
            }

        }
        Log.Error("there is no tween that it's id is max");
        return null;
    }
    DOTweenAnimation[] aniTween;
    public DOTweenAnimation[] GetTweens()
    {
        if (go != null)
        {

            if(aniTween == null)
            {
                aniTween = go.GetComponentsInChildren<DOTweenAnimation>();
            }
            return aniTween;
        }
        return null;
    }
    DOTweenPath pathTween;
    public DOTweenPath GetPathTween()
    {
        if (go != null)
        {
            if(pathTween == null)
            {
                pathTween = go.GetComponent<DOTweenPath>();
            }
            return pathTween;
        }
        return null;
    }
    public void SetColor1(Color c)
    {
        if (mt != null)
        {
            mt.color1 = c;
        }
    }

    public void SetColor2(Color c)
    {
        if (mt != null)
        {
            mt.color2 = c;
        }
    }
    public void SetAlignMent(MeshText.HorizontalAlignType type)
    {
        if (mt != null)
        {
            mt.HAlignType = type;
        }
    }
    public void SetText(string str)
    {
        if (mt != null)
        {
            mt.Text = str;
        }
    }

    public bool CheckCanRecycle()
    {//3s强制回收
        float delta = Time.realtimeSinceStartup - m_lastTime;
        if(delta > 2)
        {
            return true;
        }
        return false;
    }

    public void ResetObject()
    {
        if (go != null)
        {
            go.SetActive(false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;
            m_lastTime = Time.realtimeSinceStartup;
        }
    }

    public void InitObject()
    {
        if (go != null)
        {
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;
            m_lastTime = Time.realtimeSinceStartup;
        }
    }
    public void ReleaseObject()
    {
        if(go != null)
        {
            ReleaseTextMesh();
            UnityEngine.GameObject.DestroyImmediate(go);
        }
    }
    public void SetActive(bool isActive)
    {
        if (go != null)
        {
            go.SetActive(isActive);
        }
    }
    public void ReleaseTextMesh()
    {
        if(mt != null)
        {
            mt.ReleaseMesh();
        }
    }
    public MeshText GetMt()
    {
        return mt;
    }
    public FlyFontAlpha animator=null;
   float ChangValue = 0;
    public void DoHind(float during,float delay)
    {
        if (animator != null)
        {
            mt.uiAtlas.spriteMaterial.color = new Color(mt.uiAtlas.spriteMaterial.color.r, mt.uiAtlas.spriteMaterial.color.g, mt.uiAtlas.spriteMaterial.color.b, 1);
            animator.SetData(during,delay, mt,this,fontType);
            animator.enabled = true;
        }
        else
        {
            animator = go.AddComponent<FlyFontAlpha>();
            animator.SetData(during, delay, mt, this, fontType);
            animator.enabled = true;
        }
    }
}

