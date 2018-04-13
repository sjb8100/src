using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class FlyFontAlpha : UIMoveAction
{
    private float DuringTime;
    private float DelayTime;
    private float StartTime;
    private float precent;
    private float CurPrecent;
    private MeshText mt = null;
    private FlyFont Flyfont_ = null;
    private DamageType Type_;
    public void SetData(float duringtime, float delay, MeshText text, FlyFont font, DamageType type)
    {
        precent = 0.1f;
        StartTime = Time.time;
        DuringTime = duringtime;
        DelayTime = delay;
        CurPrecent = 1;
        mt = text;
        Flyfont_ = font;
        Type_ = type;
        mt.uiAtlas.spriteMaterial.color = new Color(mt.uiAtlas.spriteMaterial.color.r, mt.uiAtlas.spriteMaterial.color.g, mt.uiAtlas.spriteMaterial.color.b, 1);
    }
    public void FixedUpdate()
    {
        if (Time.frameCount % 3 == 0)
        {
            CurPrecent -= precent;
            if (CurPrecent <= 0)
                CurPrecent = 0;
            if (mt != null)
                mt.uiAtlas.spriteMaterial.color = new Color(mt.uiAtlas.spriteMaterial.color.r, mt.uiAtlas.spriteMaterial.color.g, mt.uiAtlas.spriteMaterial.color.b, CurPrecent);
            if (CurPrecent == 0)
            {
                enabled = false;
                FlyFontDataManager.Instance.ReturnFlyFont(Type_, Flyfont_);
            }
        }
        
    }
}
