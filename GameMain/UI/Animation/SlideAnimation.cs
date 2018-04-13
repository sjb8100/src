using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 数值动画
/// </summary>
class SlideAnimation : BaseSlideAnimation 
{
    private UISlider slide = null;
    private UILabel label = null;
    private const float SLIDE_SPEED = 2f;
    private const float LABEL_SPEED = 100f;
    void Awake()
    {
        slide = gameObject.GetComponent<UISlider>();
        label = gameObject.GetComponent<UILabel>();
        if (slide != null)
            slide_speed = SLIDE_SPEED;
        else if (label != null)
            slide_speed = LABEL_SPEED;
    }

    public bool IsSlideAnimation
    {
        get
        {
            return isSlideAnimation;
        }
    }

    public float value
    {
        set
        {
            if (!isSlideAnimation)
            {
                if (slide != null)
                {
                    slide.value = Mathf.Clamp01(value);
                }
                else if (label != null)
                {
                    label.text = (int)value + "";
                }
            }
        }
        get
        {
            if (slide != null)
            {
                return slide.value; ;
            }
            else if (label != null)
            {
                float textNum = 0;
                if (float.TryParse(label.text, out textNum))
                {
                    return textNum;
                }
            }
            return 0;
        }
    }

    public override void RefreshUI(float data)
    {
        base.RefreshUI(data);
        if (slide != null)
        {
            slide.value = Mathf.Clamp01(data);
        }else if (label != null)
        {
            label.text = "" +  (int)Mathf.Max(0,data);
        }
    }
    

}
