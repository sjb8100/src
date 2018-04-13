/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Guide
 * 创建人：  wenjunhua.zqgame
 * 文件名：  GuideManager
 * 版本号：  V1.0.0.0
 * 创建时间：2/27/2017 10:14:55 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class GuideManager : IManager, IGlobalEvent
{
    #region property
    private bool m_bool_init = false;
    #endregion

    #region IManager Method
    public void ClearData()
    {

    }
    
    public void Initialize()
    {
        if (m_bool_init)
        {
            return;
        }
        m_bool_init = true;
        RegisterGlobalEvent(true);
        InitFuncOpenData();
        InitGuideData();
    }

    public void Reset(bool depthClearData = false)
    {
        if (depthClearData)
        {
            ResetFuncOpen();
            ResetGuide();

            since_last_workflow = 0;
            since_last_unGuideRefresh = 0;
            m_bool_workFlowReady = false;
            ResetTabFunction();
        }
    }

    private float since_last_workflow = 0;
    public const float WORKFLOW_GAP = 0.2F;

    private float since_last_unGuideRefresh = 0;
    public const float UNCONSTRAINT_GUIDE_REFRESH_GAP = 0.2F;
    public void Process(float deltaTime)
    {
        //if (!DataManager.IsFrameCountRemainderEq0())
        //{
        //    //限制调用次数
        //    return;
        //}
        since_last_workflow += deltaTime;
        if (since_last_workflow >= WORKFLOW_GAP)
        {
            //执行下一个工作流
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOW);
            since_last_workflow = 0;
        }

        since_last_unGuideRefresh += deltaTime;
        if (since_last_unGuideRefresh >= UNCONSTRAINT_GUIDE_REFRESH_GAP)
        {
            //执行引导流刷新
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTREFRESHUNCONSTRAINTGUIDESTATUS);
            since_last_unGuideRefresh = 0;
        }
    }
    #endregion

    #region Common
    
    private void AdjustWorkFlowDoTime(bool adjustTime, bool matchTime = false)
    {
        if (adjustTime)
        {
            since_last_workflow = (matchTime) ? WORKFLOW_GAP : 0;
        }
        
    }

    private void AdjustUnGuideRefreshDoTime(bool adjustTime, bool matchTime = false)
    {
        if (adjustTime)
        {
            since_last_unGuideRefresh = (matchTime) ? UNCONSTRAINT_GUIDE_REFRESH_GAP : 0;
        }
    }
    /// <summary>
    /// 工作流是否准备好
    /// </summary>
    private bool m_bool_workFlowReady = false;
    public bool WorkFlowReady
    {
        get
        {
            return m_bool_workFlowReady && m_bGuideDataReady && m_bNewFuncDataReady;
        }
    }
    //将要执行的引导工作流数据
    //System.Collections.Queue
    //private List<GuideDefine.GuideWorkFlowData> m_lst_waitWorkFlow = null;
    ////当前正在执行的工作流
    //private Dictionary<GuideDefine.GuideWorkFlowType, List<GuideDefine.GuideWorkFlowData>> m_dic_curDoWorkFlow = null;
    ////当前满足条件的工作流
    //private Dictionary<GuideDefine.GuideWorkFlowType, List<GuideDefine.GuideWorkFlowData>> m_dic_canDoWorkFlow = null;

    /// <summary>
    /// 工作流检测
    /// </summary>
    /// <param name="triggerType"></param>
    private void CheckWorkFlow(CheckWorkFlowData checkData)
    {
        CheckNewFuncOpen(checkData);
        CheckGuide(checkData);
    }

    /// <summary>
    /// 执行引导流
    /// </summary>
    private void DoWorkFlow()
    {
        if (!WorkFlowReady)
        {
            return;
        }
        //1、新功能开启
        bool success = DoNextNewFuncOpen();

        //2、强制引导   
        //if (!success)
        //{
        //    success = DoNextConstrainGuide();
        //}

        //3、非强制引导
        if (!success)
        {
            DoUnconstrainGuide();
        }
    }
    
    #endregion
   
}