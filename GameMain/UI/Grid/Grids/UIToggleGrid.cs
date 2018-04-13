using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIToggleGrid : UIGridBase
{
    #region property
    //toggle
    private UIToggle toggle;
    //页签名称
    private UILabel tabName;
    //红点
    private Transform redPoint;
    
    //页签数据
    private int data;
    public int Data
    {
        get
        {
            return data;
        }
        set 
        {
            data = value;
        }
    }
    public UIPanelBase PanelBase {get;set;}
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
        SetRedPointStatus(false);
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
    }
    public void SetGridTab(int tab) 
    {
        this.data = tab;
    }
    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        if (null != toggle)
        {
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
    public void SetName(string tabName = "")
    {
        if (null != this.tabName)
        {
            this.tabName.text = tabName;
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
}