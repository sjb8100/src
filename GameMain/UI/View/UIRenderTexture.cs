using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
[RequireComponent(typeof(UITexture))]
class UIRenderTexture : UIBase
{
    #region Property
    private float rotateY = 0;
    private UITexture texture;
    private IRenerTextureObj renderObj;
    private bool ready = false;
    Action m_touchCallBack = null;
    public bool Ready
    {
        get
        {
            return ready;
        }
    }
    #endregion

    #region override method
    protected override void OnAwake()
    {
        base.OnAwake();
        texture = CacheTransform.GetComponent<UITexture>();
    }

    #endregion
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="renderObj"></param>
    /// <param name="rotateY">起始旋转</param>
    /// <param name="size">UITexture size</param>
    /// touchcallback  触摸时回调
    public void Initialize(IRenerTextureObj renderObj, float rotateY,Vector2 size,Action touchCallBack = null)
    {
        m_touchCallBack = touchCallBack;
        this.rotateY = rotateY;
        this.renderObj = renderObj;
        if(texture == null)
        {
            texture = CacheTransform.GetComponent<UITexture>();
        }
        texture.width = (int)size.x;
        texture.height = (int)size.y;
        texture.mainTexture = this.renderObj.GetTexture();
        ready = true;
    }

    /// <summary>
    /// 开启
    /// </summary>
    /// <param name="enable"></param>
    public void Enable(bool enable)
    {
        if (ready)
            renderObj.Enable(enable);
    }

    /// <summary>
    /// 释放
    /// </summary>
    public void Release()
    {
        if (ready)
        {
            m_touchCallBack = null;
            ready = false;
            renderObj.Release();
            renderObj = null;
        }
    }

    /// <summary>
    /// 设置渲染贴图的深度
    /// </summary>
    /// <param name="depth"></param>
    public void SetDepth(int depth)
    {
        if (null != texture)
            texture.depth = depth;
    }

    #region UIEvent
    void OnPress(bool bPress)
    {
        if(m_touchCallBack != null)
        {
            m_touchCallBack();
        }
    }
    void OnDrag(Vector2 delta)
    {
        
        if (ready)
        {
            
            UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
            rotateY += -0.5f * delta.x;
            renderObj.SetModelRotateY(rotateY);
        }
    }

    #endregion
}
