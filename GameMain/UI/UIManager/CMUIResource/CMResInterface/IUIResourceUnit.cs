/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：
 * 命名空间：GameMain.UI.UIManager.UIResource.IUIInterface
 * 文件名：  IUIResourceUnit
 * 版本号：  V1.0.0.0
 * 唯一标识：2d752bbd-e646-4559-b2ff-160c2c317d5c
 * 当前的用户域：USER-20160526UC
 * 电子邮箱：XXXX@sina.cn
 * 创建时间：7/7/2017 11:15:19 AM
 * 描述：
 ************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public enum CMResourceType
{
    CMResourceType_None = 0,
    CMResourceType_Prefab = 1,
    CMResourceType_Atlas = 2,
    CMResourceType_Panel = 3,
    CMResourceType_Texture = 4,
    CMResourceType_Font = 5,
    CMResourceType_Max = 6,
}

//资源状态
public enum CMResourceState
{
    CMResourceState_None = 0,
    CMResourceState_UnLoaded ,
    CMResourceState_Loading,
    CMResourceState_Loaded,
    CMResourceState_Complete,
}

interface IUIResourceUnit
{
    //加载资源
    void LoadRes();

    //ab 创建完成
    void OnLoadRes(AssetBundle ab);

    //加载完成
    void OnFinish();

    //卸载资源
    void UnLoadRes();

    //获取资源类型
    CMResourceType GetResType();

    //获取资源状态
    CMResourceState GetResState();

    //获取资源路径
    string GetResPath();

    //获取资源名称
    string GetResName();
    
    //获取依赖资源
    List<string> GetDependenceRes();
    
}