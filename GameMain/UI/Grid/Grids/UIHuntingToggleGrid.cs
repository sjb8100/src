using System;
//********************************************************************
//	创建日期:	2016-11-28   15:02
//	文件名称:	UIHuntingToggleGrid.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	狩猎右侧按钮Grid
//********************************************************************
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using UnityEngine;
class UIHuntingToggleGrid : UIGridBase
{
    UILabel label;
    UISprite warning;
    UIToggle toggle;
    protected override void OnAwake()
    {
        base.OnAwake();
        toggle = CacheTransform.GetComponent<UIToggle>();
        label = CacheTransform.Find("Label").GetComponent<UILabel>();
        warning = CacheTransform.Find("Warning").GetComponent<UISprite>();
    }
    public override void SetGridData(object data) 
    {
        base.SetGridData(data);
        if(null == data )
        {
            return;
        }
        label.text = data.ToString();
    }
    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        if (null != toggle)
        {
            ColorType ct = (hightLight) ? ColorType.White : ColorType.Gray;
            toggle.value = hightLight;
            if (null != label)
            {
                label.color = ColorManager.GetColor32OfType(ct);
            }
        }
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }
}
