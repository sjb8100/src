/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Guide
 * 创建人：  wenjunhua.zqgame
 * 文件名：  GuideUnconstrainPanel
 * 版本号：  V1.0.0.0
 * 创建时间：3/17/2017 4:02:33 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class GuideUnconstrainPanel
{
    #region property
    private Dictionary<uint, UIUnconstraintGrid> m_dic_showGuide = null;
    private List<UIUnconstraintGrid> m_lst_cacheGuideGrid = null;
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidgets();
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGlobalUIEvent(true);
        if (null != data && data is GuideDefine.LocalGuideData)
        {
            GuideDefine.LocalGuideData localData = data as GuideDefine.LocalGuideData;
            if (localData.GType != GuideDefine.GuideType.Unconstrain)
            {
                Engine.Utility.Log.Error("GuidePanel Show Failed,localData.GType != GuideDefine.GuideType.Unconstrain");
                return;
            }
            CoroutineMgr.Instance.StartCorountine(DelayOp(localData));
        }
        RefreshShowGuideVisbieStatus();
    }

    /// <summary>
    /// 刷新已经显示的引导可见状态
    /// </summary>
    private void RefreshShowGuideVisbieStatus(UIDefine.GameObjMoveData moveData = null)
    {
        if (null != m_dic_showGuide && m_dic_showGuide.Count > 0)
        {
            bool visible = false;
            GuideTrigger gTrigger = null;
            foreach(KeyValuePair<uint,UIUnconstraintGrid> pair in m_dic_showGuide)
            {
                GuideDefine.LocalGuideData localData = null;
                if (!DataManager.Manager<GuideManager>().TryGetGuideLocalData(pair.Key, out localData))
                {
                    continue;
                }

                visible = DataManager.Manager<GuideManager>().IsGuideUIStatusReady(pair.Key) && UIManager.IsObjVisibleByCamera(localData.GuideTargetObj);
                if (visible && null != moveData && null != moveData.Objs 
                    && moveData.Objs.Contains(localData.GuideTargetObj))
                {
                    switch(moveData.Status)
                    {
                        case UIDefine.GameObjMoveStatus.MoveToInvisible:
                        case UIDefine.GameObjMoveStatus.Invisible:
                            visible = false;
                            break;
                        case UIDefine.GameObjMoveStatus.Visible:
                            visible = true;
                            break;
                    }
                }
                if (null != pair.Value)
                {
                    if (visible)
                    {
                        if (!pair.Value.Visible)
                            ShowGuide(pair.Key);
                        if (localData.RefreshPosInTime)
                            pair.Value.RefreshPos();
                        
                    }else if (!visible && pair.Value.Visible)
                    {
                        pair.Value.SetVisible(false);
                        if (null != localData.GuideTargetObj)
                        {
                            gTrigger = localData.GuideTargetObj.GetComponent<GuideTrigger>();
                            if (null != gTrigger && gTrigger.enabled)
                            {
                                gTrigger.enabled = false;
                            }
                        }
                    }
                }
            }
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalUIEvent(false);
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        CompleteAllGuide();
        RelaseAllUnconstrainGrid();
        RegisterGlobalUIEvent(false);
    }
    #endregion

    #region Init
    private void RegisterGlobalUIEvent(bool register)
    {
        if (register)
        {
            //面板焦点状态改变
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED, OnUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTGAMEOBJMOVESTATUSCHANGED, OnUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTREFRESHUNCONSTRAINTGUIDESTATUS, OnUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTGUIDERESET, OnUIEventHandler);
        }else
        {
            //面板显示隐藏
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED, OnUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTGAMEOBJMOVESTATUSCHANGED, OnUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTREFRESHUNCONSTRAINTGUIDESTATUS, OnUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTGUIDERESET, OnUIEventHandler);
        }
    }

    private void OnUIEventHandler(int eventType,object data)
    {
        switch(eventType)
        {
            case (int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED:
            case (int)Client.GameEventID.UIEVENTREFRESHUNCONSTRAINTGUIDESTATUS:
                {
                    RefreshShowGuideVisbieStatus();
                }
                break;
            case (int)Client.GameEventID.UIEVENTGAMEOBJMOVESTATUSCHANGED:
                {
                    if (null != data && data is UIDefine.GameObjMoveData)
                    {
                        UIDefine.GameObjMoveData moveData = data as UIDefine.GameObjMoveData;
                        if (moveData.Status != UIDefine.GameObjMoveStatus.MoveToVisible && null != moveData.Objs)
                        {
                            RefreshShowGuideVisbieStatus(moveData);
                        }
                    }
                }
                break;
            case (int) Client.GameEventID.UIEVENTGUIDERESET:
                {
                    //重置引导
                    if (null != data && data is uint)
                    {
                        uint guideID = (uint)data;
                        ResetGuide(guideID);
                    }
                }
                break;

        }
    }

    /// <summary>
    /// 初始化组件
    /// </summary>
    private void InitWidgets()
    {
        m_dic_showGuide = new Dictionary<uint, UIUnconstraintGrid>();
        m_lst_cacheGuideGrid = new List<UIUnconstraintGrid>();
    }
    #endregion

    #region Op
    //缓存最大非强制引导格子的数量
    public const int CACHE_MAX_UNCONSTRAIN_NUM = 5;
    /// <summary>
    /// 获得一个非强制引导格子
    /// </summary>
    /// <returns></returns>
    private UIUnconstraintGrid GetUnconstrainGrid()
    {
        UIUnconstraintGrid grid = null;
        if (null != m_lst_cacheGuideGrid && m_lst_cacheGuideGrid.Count != 0)
        {
            grid = m_lst_cacheGuideGrid[0];
            m_lst_cacheGuideGrid.RemoveAt(0);
        }
        else if (null != m_trans_UnconstrainRoot && null != m_trans_UIUnconstraintGrid)
        {
            GameObject cloneObj = NGUITools.AddChild(m_trans_UnconstrainRoot.gameObject, m_trans_UIUnconstraintGrid.gameObject);
            if (null != cloneObj)
            {
                grid = cloneObj.AddComponent<UIUnconstraintGrid>();
                if (null != grid)
                {
                    grid.SetVisible(false);
                }
            }
        }
        return grid;
    }

    /// <summary>
    /// 缓存非常之引导
    /// </summary>
    /// <param name="grid"></param>
    private void CacheUnconstrainGrid(uint guideID)
    {
        UIUnconstraintGrid guideGrid = null;
        if (!TryGetUnconstrainGuide(guideID,out guideGrid))
        {
            return ;
        }
        m_dic_showGuide.Remove(guideID);
        guideGrid.Release();
        if (guideGrid.Visible)
        {
            guideGrid.SetVisible(false);
        }
        if (!m_lst_cacheGuideGrid.Contains(guideGrid))
        {
            m_lst_cacheGuideGrid.Add(guideGrid);
        }
        if (m_lst_cacheGuideGrid.Count > CACHE_MAX_UNCONSTRAIN_NUM)
        {
            int needDestoryNum = m_lst_cacheGuideGrid.Count - CACHE_MAX_UNCONSTRAIN_NUM;
            UIUnconstraintGrid tempGrid = null;
            for(int i = 0;i < needDestoryNum;i++)
            {
                tempGrid = m_lst_cacheGuideGrid[0];
                m_lst_cacheGuideGrid.RemoveAt(0);
                tempGrid.Destroy();
            }
        }
    }

    /// <summary>
    /// 重置引导
    /// </summary>
    /// <param name="guideId"></param>
    private void ResetGuide(uint guideId)
    {
        GuideDefine.LocalGuideData localData = null;
        if (DataManager.Manager<GuideManager>().TryGetGuideLocalData(guideId, out localData))
        {
            //1、清除预制
            CacheUnconstrainGrid(guideId);
            //2、清除数据
            if (null != localData.GuideTargetObj)
            {
                GuideTrigger gt = localData.GuideTargetObj.GetComponent<GuideTrigger>();
                if (null != gt)
                {
                    gt.RemoveTriggerId(guideId);
                    if (!gt.HaveTriggerData())
                    {
                        GameObject.Destroy(gt);
                    }
                    //无事件触发，引导完成后删除碰撞器
                    if (localData.TriggerEventType == GuideDefine.GuideTriggerEventType.None)
                    {
                        BoxCollider bCollider = localData.GuideTargetObj.GetComponent<BoxCollider>();
                        if (null != bCollider)
                        {
                            GameObject.Destroy(bCollider);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 释放所有非强制引
    /// </summary>
    private void RelaseAllUnconstrainGrid()
    {
        List<uint> showGuideIDs = new List<uint>();
        showGuideIDs.AddRange(m_dic_showGuide.Keys);
        int max = showGuideIDs.Count;
        if (max > 0)
        {
            for(int i =0; i < max;i++)
            {
                ResetGuide(showGuideIDs[i]);
            }
        }

        UIUnconstraintGrid grid = null;
        while(m_lst_cacheGuideGrid.Count > 0)
        {
            grid = m_lst_cacheGuideGrid[0];
            m_lst_cacheGuideGrid.RemoveAt(0);
            if (null != grid)
            {
                grid.Destroy();
            }
        }
    }

    /// <summary>
    /// 是否当前正在显示非强制引导
    /// </summary>
    /// <param name="guideId"></param>
    /// <returns></returns>
    public bool IsShowUnconstrainGuide(uint guideId)
    {
        return m_dic_showGuide.ContainsKey(guideId);
    }

    public bool TryGetUnconstrainGuide(uint guideId,out UIUnconstraintGrid grid)
    {
        return m_dic_showGuide.TryGetValue(guideId, out grid);
    }

    /// <summary>
    /// 引导ID
    /// </summary>
    /// <param name="guideId"></param>
    private void ShowGuide(uint guideId)
    {
        GuideDefine.LocalGuideData localData = null;
        if (DataManager.Manager<GuideManager>().TryGetGuideLocalData(guideId,out localData))
        {
            if (null == localData.GuideTargetObj)
            {
                return;
            }
            PanelType ptype = PanelType.SmartPopUp;
            UIPanelManager.LocalPanelInfo localInfo = null;
            if (DataManager.Manager<UIPanelManager>().TryGetLocalPanelInfo(localData.GuideTargetDependPanel,out localInfo))
            {
                ptype = localInfo.PType;
            }
            if (IsShowUnconstrainGuide(guideId))
            {
                //如果不可见重新获取当前面板深度
                UIUnconstraintGrid showGrid = m_dic_showGuide[guideId];
                if (!showGrid.Visible)
                {
                    showGrid.SetVisible(true);
                    GuideTrigger gTrigger = localData.GuideTargetObj.GetComponent<GuideTrigger>();
                    if (null != gTrigger && !gTrigger.enabled)
                    {
                        gTrigger.enabled = true;
                    }
                    int maxDepth = DataManager.Manager<UIPanelManager>().GetTargetRootMaxDepth(ptype);
                    showGrid.SetDepth(maxDepth + 1);
                    if (localData.RefreshPosInTime)
                        showGrid.RefreshPos();
                    showGrid.CheckTriggerData(guideId,GuideTriggerDlg);
                }
            }
            else
            {
                int maxDepth = DataManager.Manager<UIPanelManager>().GetTargetRootMaxDepth(ptype);
                UIUnconstraintGrid grid = GetUnconstrainGrid();
                if (null != grid)
                {
                    grid.CacheTransform.gameObject.name = guideId.ToString();
                    m_dic_showGuide.Add(guideId, grid);
                }
                grid.SetData(maxDepth + 1, guideId, GuideTriggerDlg);

                if (localData.TableData.voiceID != 0)
                {
                    DataManager.Manager<UIManager>().PlayUIAudioEffect(localData.TableData.voiceID);
                }
            }
            

            GuideTriggerData gtData = localData.GuideTargetObj.GetComponent<GuideTriggerData>();
            if (null == gtData)
            {
                gtData = localData.GuideTargetObj.AddComponent<GuideTriggerData>();
            }
            if (null != gtData && !gtData.IsGuideTrigger)
            {
                gtData.IsGuideTrigger = true;
            }
        }
        
    }

    /// <summary>
    /// 延迟执行部分
    /// </summary>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    private System.Collections.IEnumerator DelayOp(GuideDefine.LocalGuideData localData)
    {
        yield return new WaitForSeconds(localData.DelayTime);
        ShowGuide(localData.ID);
    }


    /// <summary>
    /// 完成所有引导
    /// </summary>
    private void CompleteAllGuide()
    {
        if (m_dic_showGuide.Count > 0)
        {
            List<uint> guideIds = new List<uint>();
            guideIds.AddRange(m_dic_showGuide.Keys);
            for(int i = 0; i < guideIds.Count;i++)
            {
                CompleteGuide(guideIds[i]);
            }
        }
    }

    /// <summary>
    /// 完成单个类型引导
    /// </summary>
    /// <param name="gType"></param>
    private void CompleteGuide(uint guideId)
    {
        GuideDefine.LocalGuideData localData = null;
        if (IsShowUnconstrainGuide(guideId)
            && DataManager.Manager<GuideManager>().TryGetGuideLocalData(guideId,out localData))
        {
            ResetGuide(guideId);
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTGUIDECOMPLETE, guideId);
        }
    }
    #endregion

    #region UIEvent
    /// <summary>
    /// 引导UI事件回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void GuideTriggerDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is GuideTrigger)
                {
                    GuideTrigger gTrigger = data as GuideTrigger;
                    if (null != gTrigger && null != gTrigger.TriggerIds)
                    {
                        List<uint> triggerIds = new List<uint>();
                        triggerIds.AddRange(gTrigger.TriggerIds);
                        GuideDefine.LocalGuideData localData = null;
                        for (int i = 0; i < triggerIds.Count; i++)
                        {
                            if (i == 0)
                            {
                                if (DataManager.Manager<GuideManager>().TryGetGuideLocalData(triggerIds[i],out localData))
                                {
                                    if (null != localData.GuideTargetObj)
                                    {
                                        GuideTrigger trigger = localData.GuideTargetObj.GetComponent<GuideTrigger>();
                                        if (null != trigger)
                                        {
                                            GameObject.Destroy(trigger);
                                        }
                                        //无事件触发，引导完成后删除碰撞器
                                        if (localData.TriggerEventType == GuideDefine.GuideTriggerEventType.None)
                                        {
                                            if (null != localData.GuideTargetObj.GetComponent<BoxCollider>())
                                            {
                                                GameObject.Destroy(localData.GuideTargetObj.GetComponent<BoxCollider>());
                                            }
                                        }
                                    }
                                }
                                
                            }
                            CompleteGuide(triggerIds[i]);
                        }
                    }
                }
                break;
        }
    }
    #endregion
}