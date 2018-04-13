using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

class UITeamActivityChildGrid : UIGridBase
{
    #region property
    //名称
    private UILabel m_lab_name;
    //背景
    private UISprite m_sp_bg;
    //红点
    //private UISprite m_sp_redPoint;

    //活动目标
    private UISprite m_sp_targetMark;

    //数据
    private uint m_id;
    public uint Id
    {
        get
        {
            return m_id;
        }
    }
    private UnityEngine.GameObject m_checkmark = null;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_lab_name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        m_checkmark = CacheTransform.Find("Content/Toggle/Checkmark").gameObject;
        m_sp_targetMark = CacheTransform.Find("Content/TargetMark").GetComponent<UISprite>();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="name"></param>
    /// <param name="select"></param>
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        this.m_id = (uint)data;
    }


    #endregion

    #region Set

    public void SetSelect(bool select)
    {
        if (null != m_checkmark)
        {
            m_checkmark.SetActive(select);
        }
    }

    public void SetName(string name) 
    {
        if (null != m_lab_name)
        {
            m_lab_name.text = name;
        }
    }

    //设置活动目标
    public void SetTargetMark(bool b)
    {
        if (m_sp_targetMark != null)
        {
            m_sp_targetMark.gameObject.SetActive(b);
        }
    }

    #endregion

}

