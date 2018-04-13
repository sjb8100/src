/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Login
 * 创建人：  wenjunhua.zqgame
 * 文件名：  AnimatorEventHelper
 * 版本号：  V1.0.0.0
 * 创建时间：6/16/2017 11:04:12 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class AnimatorEventHelper : MonoBehaviour
{
    public const string ANIMAION_COMPLETE_CALL_FUC_NAME = "AnimationComplete";

    public Action<string> m_sCompleteCallback;
    public void RegisterStringCompleteCallback(Action<string> callback)
    {
        m_sCompleteCallback = callback;
    }

    public void AnimationComplete(string param)
    {
        if (null != m_sCompleteCallback)
        {
            m_sCompleteCallback.Invoke(param);
        }
    }
}