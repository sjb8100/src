/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：Assets.Scripts.UI.Base
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIParticleWidget
 * 版本号：  V1.0.0.0
 * 创建时间：6/9/2017 5:00:41 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIParticleWidget : UISprite
{
    public enum RelativeLayerType
    {
        Equal = 1,
        Up,
        Down,
    }
    private int m_irenderQ = 3000;
    private bool init = false;
    private GameObject m_obj;

    private UIWidget m_relativeLayerWidget = null;
    private RelativeLayerType layerType = RelativeLayerType.Equal;
    private CMResAsynSeedData<CMAtlas> m_bgCASD = null;

    private Bounds bounds;
    private List<Vector3> m_lstPos = new List<Vector3>();
    private int m_index;
    private bool moveToNext = false;
    UIWidget m_widget;
    public int RenderQ
    {
        get
        {
            return m_irenderQ;
        }
    }

    protected override void OnPariticleInit()
    {
        base.OnPariticleInit();
        if (init)
        {
            return;
        }
        init = true;
        //text = "";
        UIManager.GetAtlasAsyn(UIDefine.PANEL_MASK_BG, ref m_bgCASD, () =>
        {
            atlas = null;
        }, this, false);

        alpha = 0.01f;
        GetParticleMaterials();
        onRender += OnWidgetRender;
        if (null != drawCall)
        {
            m_irenderQ = drawCall.sortingOrder;
        }
        UpdateRenderQueue();
    }

    public void SetRelativeUIlayer(UIWidget w,RelativeLayerType layerType = RelativeLayerType.Equal)
    {
        if (null != w)
        {
            m_relativeLayerWidget = w;
            switch (layerType)
            {
                case RelativeLayerType.Equal:
                    depth = w.depth;
                    //m_irenderQ = w.drawCall.renderQueue;
                    break;
                case RelativeLayerType.Down:
                    depth = w.depth - 1;
                    //m_irenderQ = w.drawCall.renderQueue - 1;
                    break;
                case RelativeLayerType.Up:
                    depth = w.depth + 1;
                    //m_irenderQ = w.drawCall.renderQueue + 1;
                    break;
            }

            UpdateRenderQueue();
        }
    }

    public void SetParticleDirty()
    {
        if (gameObject.activeSelf)
        {
            GetParticleMaterials();
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            if (null != panel)
            {
                panel.RebuildAllDrawCalls();
            }
            UpdateRenderQueue();
        }
    }

    public void ChangeDepth(int depth)
    {
        this.mDepth = depth;
        SetParticleDirty();
    }

    public void AddParticle(GameObject particle)
    {
        m_obj = particle;
        
        SetParticleDirty();
    }

    private Engine.IEffect m_effect = null;
    private Action<Engine.IEffect> m_oncomplete = null;
    private uint m_uResId = 0;

    public void AddParticle(uint resId, UIWidget relativeWidget, RelativeLayerType layerType = RelativeLayerType.Equal, Action<Engine.IEffect> oncomplete = null, Engine.EffectCallback endCallback = null)
    {
        SetRelativeUIlayer(relativeWidget, layerType);
        AddParticle(resId, oncomplete: oncomplete, endCallback: endCallback);
    }
    public void AddRoundParticle(uint resId = 50005) 
    {
        m_widget = GetComponent<UIWidget>();
        GetPos();
        AddParticle(resId);
        OnNext();
    }
    void GetPos()
    {
        if (m_widget == null)
        {
            return;
        }
        bounds = m_widget.CalculateBounds();
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        m_lstPos.Clear();
        Vector3 pos = new Vector3(min.x, min.y, 0);
        m_lstPos.Add(pos);
        pos = new Vector3(min.x, max.y, 0);
        m_lstPos.Add(pos);
        pos = new Vector3(max.x, max.y, 0);
        m_lstPos.Add(pos);
        pos = new Vector3(max.x, min.y, 0);
        m_lstPos.Add(pos);

    }
    void OnNext()
    {
        if (null != m_effect)
        {
            Transform ts = m_effect.GetNodeTransForm();
            if (null == ts)
            {
                return;
            }
            Vector3 from;
            Vector3 to;
            if (m_index >= m_lstPos.Count - 1)
            {
                from = m_lstPos[m_lstPos.Count - 1];
                m_index = -1;//下一个点从0开始
                to = m_lstPos[0];
            }
            else
            {
                from = m_lstPos[m_index];
                to = m_lstPos[m_index + 1];
            }
            TweenPosition tp = TweenPosition.Begin(ts.gameObject, 1f, to);
            tp.callWhenFinished = "OnNext";
            tp.duration = Vector3.Distance(from, to) / 300.0f;
            tp.eventReceiver = gameObject;
            tp.from = from;
            tp.to = to;
            m_index++;
        }
    }

    public void AddParticle(uint resId,Action<Engine.IEffect> oncomplete = null,Engine.EffectCallback endCallback = null)
    {
        //ReleaseParticle();
        m_uResId = resId;
        Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
        if (rs != null)
        {
            table.ResourceDataBase rd = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(resId);
            if (rd == null)
            {
                Engine.Utility.Log.Error("ID为{0}的资源为空", resId);
                return;
            }
            string path = rd.strPath;
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            m_oncomplete = oncomplete;
            bool success = rs.CreateEffect(ref path, ref m_effect, OnCreateEffectEvent, Engine.TaskPriority.TaskPriority_Immediate,cb:endCallback);
            if (success)
            {
                UpdateEffectWidget();
            }

        }
    }

    private void UpdateEffectWidget()
    {
        if (null != m_effect)
        {
            Engine.Node node = m_effect.GetNode();
            if (node != null)
            {
                Transform trans = node.GetTransForm();
                if (trans != null)
                {
                    trans.parent = cachedTransform;
                    node.SetLocalPosition(Vector3.zero);
                    trans.localScale = Vector3.one;
                    trans.SetChildLayer(LayerMask.NameToLayer("UI"));
                }
            }
          
        }
    }

    void OnCreateEffectEvent(Engine.IEffect effect)
    {
        m_effect = effect;
        if (m_effect == null)
        {
            return;
        }
        GetParticleMaterials();
        UpdateEffectWidget();
        SetParticleDirty();
        if (null != m_oncomplete)
        {
            m_oncomplete.Invoke(effect);
        }
    }
    Renderer[] m_renderers;
    public void GetParticleMaterials()
    {
        if (null != gameObject)
        {
            m_renderers = gameObject.GetComponentsInChildren<Renderer>(true);
        }
    }

    public void ReleaseParticle()
    {
        m_uResId = 0;
        if (null != m_effect)
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs != null)
            {
                rs.RemoveEffect(m_effect);
            }
            m_effect = null;
        }
    }

    /// <summary>
    /// widget渲染
    /// </summary>
    /// <param name="material"></param>
    private void OnWidgetRender(Material material)
    {
        if (null != drawCall && m_irenderQ != drawCall.sortingOrder)
        {
            m_irenderQ = drawCall.sortingOrder;
            UpdateRenderQueue();
        }
    }

    /// <summary>
    /// 更新渲染队列
    /// </summary>
    private void UpdateRenderQueue()
    { 
        if (null != m_renderers)
        {
            for (int i = 0, max = m_renderers.Length; i < max; i++)
            {
                if (null == m_renderers[i])
                    continue;
                m_renderers[i].sortingOrder = m_irenderQ;
            }
        }
    }

    public void Release()
    {
        m_renderers = null;
        m_irenderQ = 3000;
        m_obj = null;
    }

    private void OnDestroy()
    {
        if (null != m_bgCASD)
        {
            m_bgCASD.Release(true);
            m_bgCASD = null;
        }
    }
}