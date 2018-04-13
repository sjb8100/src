using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIEquipPropertyRemoveGrid : UIGridBase
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
    private UILabel removeAttr;
    //消除Content
    private Transform removeContent;
    #endregion

    #region override method

    protected override void OnAwake()
    {
        base.OnAwake();
        curAttr = CacheTransform.Find("Content/Current/Property").GetComponent<UILabel>();
        toggle = gameObject.transform.Find("Content/Current/CheckBox").GetComponent<UIToggle>();
        removeAttr = CacheTransform.Find("Content/Remove/Property").GetComponent<UILabel>();
        removeContent = CacheTransform.Find("Content/Remove");
        EventDelegate.Set(toggle.onChange, delegate
        {
            if (null != toggleChangeCallback)
                toggleChangeCallback.Invoke(this, toggle.value);
        });
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null == data)
            return;
        this.attrId = (uint) data;
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
    /// <param name="des"></param>
    /// <param name="select"></param>
    public void SetSelect(string des, bool select)
    {
        if (string.IsNullOrEmpty(des))
            return;
        if (null != curAttr)
            curAttr.text = des;
        if (null != removeAttr)
            removeAttr.text = des;
        if (null != removeContent && removeContent.gameObject.activeSelf != select)
        {
            removeContent.gameObject.SetActive(select);
        }

        if (null != toggle && toggle.value != select)
        {
            toggle.value = select;
        }
    }
    #endregion
}