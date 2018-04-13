//*************************************************************************
//	创建日期:	2016/11/14 10:46:05
//	文件名称:	LongPress
//   创 建 人:   zhudianyu	
//	版权所有:	中青宝
//	说    明:	响应长按事件
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Engine.Utility;
class LongPress:MonoBehaviour,ITimer
{
    Action m_downCallBack;
    Action m_upCallBack;

    bool bPress = false;
    //长按生效时间
    uint m_uLongPressTime = 300;
    private readonly uint m_uLongPressTimerID = 200;
    void Awake()
    {
        UIEventListener.Get( this.gameObject ).onPress = OnCustomLongPress;
    }
    /// <summary>
    /// 初始化长按事件回调
    /// </summary>
    /// <param name="downCallBack">按下</param>
    /// <param name="upCallBack">弹起</param>
    /// <param name="longpressTime">长按生效时间</param>
    public void InitLongPress(Action downCallBack,Action upCallBack,uint longpressTime = 300)
    {
        m_downCallBack = downCallBack;
        m_upCallBack = upCallBack;
        m_uLongPressTime = longpressTime;
    }
    void OnCustomLongPress(GameObject go, bool state)
    {
        if ( state )
        {
            if(!bPress)
            {
                bPress = true;
                TimerAxis.Instance().SetTimer(m_uLongPressTimerID, m_uLongPressTime, this, 1);
            }
            else
            {
                if (m_upCallBack != null)
                {
                    m_upCallBack();
                }
            }
        }
        else
        {
            bPress = false;
            if(m_upCallBack != null)
            {
                m_upCallBack();
            }
            TimerAxis.Instance().KillTimer(m_uLongPressTimerID, this);
        }
    }


    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == m_uLongPressTimerID)
        {
            if(m_downCallBack != null)
            {
                m_downCallBack();
            }

        }
    }
}

