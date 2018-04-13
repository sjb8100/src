/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.UIManager.CMUIResource.CMResInterface
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ICMTask
 * 版本号：  V1.0.0.0
 * 创建时间：8/29/2017 3:22:05 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum CMABTaskState
{
    CMTaskState_None = 0,
    CMTaskState_Waiting,
    CMTaskState_Processing,
    CMTaskState_Done,
}

interface ICMABTask
{
    //进入等待列表
    void OnWaitingStart();

    //进行中
    void OnProcessing(float progress);

    //完成
    void Done(AssetBundle ab);
    //获取进度
    float GetProgress();

    //是否完成
    bool IsDone();

    //获取状态
    CMABTaskState GetTaskState();
}