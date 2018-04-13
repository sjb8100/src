/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.UIManager.UIResource.IUIResInterface
 * 创建人：  wenjunhua.zqgame
 * 文件名：  IUITask
 * 版本号：  V1.0.0.0
 * 创建时间：8/29/2017 9:52:45 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 任务状态
/// </summary>
public enum UITaskState
{
    UITaskState_None = 0,
    UITaskState_Waiting,
    UITaskState_Excute,
    UITaskState_Done,
    UITaskState_Failed,
}

interface IUITask
{
    //执行任务
    void Excute();
    
    //任务执行结束
    void End();

    //获取任务状态
    UITaskState GetState();

    //设置任务状态
    void SetState(UITaskState state);

}