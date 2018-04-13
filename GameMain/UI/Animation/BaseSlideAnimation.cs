using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseSlideAnimation : MonoBehaviour
{
    public delegate void SlideAnimingOver();
    protected SlideAnimingOver OnSlideAnimingOver = null;
    protected bool isSlideAnimation = false;
    protected float slide_speed = 2f;
    protected float sourceValue = 0;
    protected float targetValue = 0;
    protected float forward = 1;
    protected float nextValue = 0f;
    void Update()
    {
        if (isSlideAnimation )
        {
            nextValue += forward * slide_speed * Time.deltaTime;
            if ((forward > 0 && nextValue >= targetValue)
                || (forward < 0 && nextValue <= targetValue))
            {
                isSlideAnimation = false;
                if (OnSlideAnimingOver != null)
                    OnSlideAnimingOver.Invoke();
                nextValue = targetValue;
            }
            RefreshUI(nextValue);
        }
    }


    public void DoSlideAnim(float source,float target, bool needAnimation = true, float speed = 0f, SlideAnimingOver animOverCallBack = null)
    {
        isSlideAnimation = needAnimation;
        sourceValue = source;
        nextValue = sourceValue;
        targetValue = target;
        this.OnSlideAnimingOver = animOverCallBack;
        forward = (target > sourceValue) ? 1f : -1f;
        if(speed > float.Epsilon)
            slide_speed = speed;
        //如果slide_speed = 0 使用默认速度2f
        if (!needAnimation)
        {
            RefreshUI(target);
            if (OnSlideAnimingOver != null)
                OnSlideAnimingOver.Invoke();
        }
    }

    public virtual void RefreshUI(float data)
    {

    }

}
