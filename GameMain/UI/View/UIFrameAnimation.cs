using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using NGUIEX;
using table;

public class UIFrameAnimation : UISprite
{
    UIWidget m_widget;
    UISpriteAnimation ani = null;
    private CMResAsynSeedData<CMAtlas> m_frameCASD = null;
    public void AddFrameParticle(uint particleID)
    {
        if (m_widget == null)
        {
            m_widget = GetComponent<UIWidget>();           
        }
        if (m_widget != null)
        {
            m_widget.gameObject.SetActive(true);
        }    
        table.FrameEffectDataBase tab = GameTableManager.Instance.GetTableItem<table.FrameEffectDataBase>(particleID);
        if (tab == null)
        {
            Engine.Utility.Log.Error("查找不到ID为{0}的帧动画数据", particleID);
            return;
        }
        UIManager.GetAtlasAsyn((uint)AtlasID.Frameeffectatlas, ref m_frameCASD, OnGetAtlas,
            this, tab.default_icon,false);    
    
        string[] size = tab.icon_size.Split('_');
        if (size.Length == 2)
        {
            int h = 0;
            int w = 0;
            if (int.TryParse(size[0], out h) && int.TryParse(size[1], out w))
            {
                this.height = h;
                this.width = w;
            }
        }
        ani = m_widget.GetComponent<UISpriteAnimation>();
        if (ani == null)
        {
            ani = m_widget.gameObject.AddComponent<UISpriteAnimation>();
        }
        if (ani != null)
        {
            ani.framesPerSecond = (int)tab.rate;
            ani.namePrefix = tab.icon_prefix;
            ani.loop = tab.loop == 1;          
        }
    }
    void OnGetAtlas(IUIAtlas cmatlas, object param1, object param2, object param3) 
    {
        if (null == cmatlas)
            return;
        UIAtlas atlas = cmatlas.GetAtlas();
        if (null == atlas)
        {
            return;
        }
        if (null != param1 && param1 is UISprite)
        {
            UISprite sprite = (UISprite)param1;
            UIAtlas atl = (null != atlas) ? cmatlas.GetAtlas() : null;
            sprite.atlas = atl;
            if (null != param2 && param2 is string)
            {
                sprite.spriteName = (string)param2;
            }
            if (null != param3 && param3 is bool)
            {
                bool makePerfect = (bool)param3;
                if (null != sprite && makePerfect)
                    sprite.MakePixelPerfect();
            }
        }

        if(ani != null)
        {
            ani.RebuildSpriteList();
            ani.Play();
        }

       

    }

    void OnFrameRelease() 
    {
        if (m_widget != null)
        {
            m_widget.gameObject.SetActive(false);
        }
        if (m_frameCASD != null)
        {
            if (atlas != null)
            {
                atlas = null;
            }         
            m_frameCASD.Release();
            m_frameCASD = null;
        }
    }
    public void ReleaseParticle() 
    {
        OnFrameRelease();
    }
}

