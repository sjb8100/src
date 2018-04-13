/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Animation
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIMoveAction
 * 版本号：  V1.0.0.0
 * 创建时间：6/14/2017 11:30:52 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIMoveAction : MonoBehaviour
{
    #region define
    
    public enum MoveActionType
    {
        //圆圈
        Circle,
        //矩形
        Rectangle,
        //直线
        Line,
    }

    /// <summary>
    /// 重复类型
    /// </summary>
    public enum MoveRepeatType
    {
        //单次
        Once ,
        //循环
        Loop ,
        //开关
        Toggle,
    }
    #endregion

    #region property
    private bool m_bPlalying = false;
    public bool Playing
    {
        get
        {
            return m_bPlalying;
        }
    }

    //移动类型
    public MoveActionType MoveType = MoveActionType.Circle;
    //移动目标
    public Transform MoveTarget = null;
    //重复类型
    public MoveRepeatType RepeatType = MoveRepeatType.Loop;
    //单次运动时间
    public float OneRoundTime = 1f;
    #endregion

    #region Init
    /// <summary>
    /// 
    /// </summary>
    /// <param name="moveType">移动类型></param>
    /// <param name="target">移动目标</param>
    /// <param name="oneRoundTime">单次运行时间</param>
    /// <param name="repeatType">重复类型默认为循环</param>
    public void Initialize(MoveActionType moveType, Transform target
        , float oneRoundTime, MoveRepeatType repeatType = MoveRepeatType.Loop)
    {
        this.MoveType = moveType;
        this.RepeatType = repeatType;
        this.MoveTarget = target;
        this.OneRoundTime = oneRoundTime;
    }

    public void Reset()
    {
        MoveTarget = null;
        m_bPlalying = false;
        MoveType = MoveActionType.Circle;
    }

    #endregion

    #region Op

    public bool Play(bool fromStart = false)
    {
        return false;
    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {

    }

    /// <summary>
    /// 展厅
    /// </summary>
    public void Pause()
    {

    }

    public void Release()
    {
        Reset();
    }

    #endregion
}