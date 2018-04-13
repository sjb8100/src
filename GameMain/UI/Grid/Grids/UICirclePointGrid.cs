using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UICirclePointGrid : UIGridBase
{
    #region define
    //Toggle
    private UIToggle toggle;
    //数据
    private object data;
    public object Data
    {
        get
        {
            return data;
        }
    }
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        toggle = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();
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
            toggle.value = hightLight;
    }
    #endregion
}