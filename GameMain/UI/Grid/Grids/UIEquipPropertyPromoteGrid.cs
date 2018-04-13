using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIEquipPropertyPromoteGrid : UIGridBase
{
    #region define
    //checkbox
    private UIToggle toggle;
    //属性id
    private uint attrId;
    public uint AttrId
    {
        get
        {
            return attrId;
        }
    }
    //原属性
    private UILabel curAttr;
    //消除属性
    private UILabel nextAttr;
    //提升概率
    private UILabel promoteProp;
    //消除Content
    private Transform arrowContent;
    //当前档次Content
    private Transform m_ts_curGradeCont;
    //当前档次
    private UILabel m_lab_curGrade;
    //下一级档次Content
    private Transform m_ts_nextGradeCont;
    //下一级档次
    private UILabel m_lab_nextGrade;
    //Max
    private Transform m_ts_max;
    #endregion

    #region override method

    protected override void OnAwake()
    {
        base.OnAwake();
        curAttr = CacheTransform.Find("Content/Current/Property").GetComponent<UILabel>();
        toggle = CacheTransform.Find("Content/Current/CheckBox").GetComponent<UIToggle>();
        m_ts_curGradeCont = CacheTransform.Find("Content/Current/GCCur");
        m_lab_curGrade = CacheTransform.Find("Content/Current/GCCur/Grade").GetComponent<UILabel>();
        m_ts_max = CacheTransform.Find("Content/Current/MaxContent");
        nextAttr = CacheTransform.Find("Content/Next/Property").GetComponent<UILabel>();
        m_ts_nextGradeCont = CacheTransform.Find("Content/Next/GCNext");
        m_lab_nextGrade = CacheTransform.Find("Content/Next/GCNext/Grade").GetComponent<UILabel>();
        arrowContent = CacheTransform.Find("Content/ArrowContent");
        promoteProp = CacheTransform.Find("Content/ArrowContent/Prop").GetComponent<UILabel>();
        EventDelegate.Set(toggle.onChange, delegate
        {
            if (null != toggleChangeCallback)
                toggleChangeCallback.Invoke(this, toggle.value);
        });
        OnDisable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        SetSelect("", "", false, "");
        SetMax(false);
        SetGrade();
    }
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null == data)
            return;
        this.attrId = (uint)data;
    }
    #endregion

    #region Op
    private Action<UIGridBase, bool> toggleChangeCallback = null;
    public void RegisterCheckBoxCallback(Action<UIGridBase, bool> callback)
    {
        toggleChangeCallback = callback;
    }

    /// <summary>
    /// 设置选中
    /// </summary>
    /// <param name="cur"></param>
    /// <param name="next"></param>
    /// <param name="select"></param>
    /// <param name="promoteProp"></param>
    public void SetSelect(string cur,string next,bool select,string promoteProp = "")
    {
        if (null != curAttr)
            curAttr.text = ColorManager.GetColorString(ColorType.White, cur);
        if (null != nextAttr)
            nextAttr.text = (select) ? ColorManager.GetColorString(ColorType.Green, next)
                : ColorManager.GetColorString(ColorType.Gray, next);
        if (null != arrowContent)
        {
            if (arrowContent.gameObject.activeSelf != select)
                arrowContent.gameObject.SetActive(select);
            if (null != this.promoteProp && select)
                this.promoteProp.text = promoteProp;
        }
        if (null != toggle && toggle.value != select)
        {
            toggle.value = select;
        }
    }

    /// <summary>
    /// 设置档次
    /// </summary>
    /// <param name="enableCur"></param>
    /// <param name="curGrade"></param>
    /// <param name="enableNext"></param>
    /// <param name="nextGrade"></param>
    public void SetGrade(bool enableCur= false,int curGrade = 0,bool enableNext = false,int nextGrade = 0)
    {
        //CurGrade
        if (null != m_ts_curGradeCont && m_ts_curGradeCont.gameObject.activeSelf != enableCur)
        {
            m_ts_curGradeCont.gameObject.SetActive(enableCur);
            if (null != m_lab_curGrade && enableCur)
            {
                m_lab_curGrade.text = curGrade.ToString();
            }
        }

        //NextGrade
        if (null != m_ts_nextGradeCont && m_ts_nextGradeCont.gameObject.activeSelf != enableNext)
        {
            m_ts_nextGradeCont.gameObject.SetActive(enableNext);
            if (null != m_lab_nextGrade && enableNext)
            {
                m_lab_nextGrade.text = nextGrade.ToString();
            }
        }
    }

    /// <summary>
    /// 设置是否达到最大档次
    /// </summary>
    /// <param name="enable"></param>
    public void SetMax(bool enable = false)
    {
        if (null != m_ts_max && m_ts_max.gameObject.activeSelf != enable)
        {
            m_ts_max.gameObject.SetActive(enable);
        }
    }

    #endregion
}