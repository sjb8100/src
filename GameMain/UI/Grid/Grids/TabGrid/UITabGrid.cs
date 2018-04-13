using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UITabGrid : UIGridBase
{
    #region property
    //toggle
    private UIToggle toggle;
    //页签名称
    private UILabel tabName;
    //红点
    private Transform redPoint;
    //未开启mask
    private Transform m_tsLockMask;
    
    //页签数据
    private object data;
    public object Data
    {
        get
        {
            return data;
        }
    }
    public int TabType { get; set; }
    public int TabID { get; set; }
    #endregion

    #region overridemethod

    protected override void OnAwake()
    {
        base.OnAwake();
        toggle = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();
        toggle.instantTween = true;
        tabName = CacheTransform.Find("Content/Toggle/Label").GetComponent<UILabel>();
        redPoint = CacheTransform.Find("Content/RedPoint");
        m_tsLockMask = CacheTransform.Find("Content/LockMask");
        SetRedPointStatus(false);
        m_btnSoundEffect = ButtonPlay.ButtonSountEffectType.FuncTabSecond;
    }

    public void SetSoundEffectType(ButtonPlay.ButtonSountEffectType effect)
    {
        if (m_btnSoundEffect == effect)
        {
            return;
        }
        m_btnSoundEffect = effect;
        UpdateTriggerEffect();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        this.data = data;
    }
    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        if (null != toggle)
        {
            //ColorType ct = (hightLight) ? ColorType.White : ColorType.Gray;
            toggle.value = hightLight;
            if (null != tabName)
            {
                tabName.color = ColorManager.GetColor32OfType(ColorType.TabLight);
            }
        }
    }

    /// <summary>
    /// 设置红点提示状态
    /// </summary>
    /// <param name="enable"></param>
    public void SetRedPointStatus(bool enable)
    {
        if (redPoint != null)
        {
            redPoint.gameObject.SetActive(enable);
        }
    }

    public void SetOpenStatus(bool open)
    {
        if (null != m_tsLockMask && m_tsLockMask.gameObject.activeSelf == open)
        {
            m_tsLockMask.gameObject.SetActive(!open);
        }
    }
    public override void OnGridClicked()
    {
        base.OnGridClicked();
    }

    #endregion

    #region set
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tabName"></param>
    public void SetName(string tabName = "",int wordGapX =-1)
    {
        if (null != this.tabName)
        {
            this.tabName.text = tabName;
            if (wordGapX != -1)
            {
                this.tabName.spacingX = wordGapX;
            }
            
        }
    }

    /// <summary>
    /// 设置深度
    /// </summary>
    /// <param name="depath"></param>
    public void SetDepth(int depth)
    {
        return; 
        if (null != toggle)
        {
            toggle.transform.Find("Background").GetComponent<UISprite>().depth = depth-1;
            toggle.transform.Find("Checkmark").GetComponent<UISprite>().depth = depth;
        }
        if (null != this.tabName)
        {
            this.tabName.depth = depth + 1;
        }
    }
    #endregion

    #region Release
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (data != null)
        {
            data = null ;
        }
    }
    #endregion
}