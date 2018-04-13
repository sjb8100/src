/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.DataManager
 * 创建人：  wenjunhua.zqgame
 * 文件名：  IGlobalEvent
 * 版本号：  V1.0.0.0
 * 创建时间：6/22/2017 3:51:25 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IGlobalEvent
{
    //注册/反注册全局事件
    void RegisterGlobalEvent(bool register);

    //全局事件处理
    void GlobalEventHandler(int eventid,object data);
}