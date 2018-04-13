/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.UIManager
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIPanelManager_Event
 * 版本号：  V1.0.0.0
 * 创建时间：5/9/2017 11:57:39 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

partial class UIPanelManager : IGlobalEvent
{
    #region IGlobalEvent
    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RECONNECT_SUCESS, GlobalEventHandler);
            //面板唤醒、面板显示、面板隐藏、销毁
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_PANELSTATUS, GlobalEventHandler);
            //面板焦点状态改变
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.RECONNECT_SUCESS, GlobalEventHandler);
            //面板唤醒、面板显示、面板隐藏、销毁
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_PANELSTATUS, GlobalEventHandler);
            //面板焦点状态改变
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED, GlobalEventHandler);
        }
    }

    /// <summary>
    /// UI全局事件处理器
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    public void GlobalEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            case (int)Client.GameEventID.UIEVENT_PANELSTATUS:
                OnPanelStatusChanged((UIPanelBase.PanelStatusData)data);
                break;
            case (int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED:
                OnPanelFocusStatusChanged((PanelFocusData)data);
                break;
            case (int)Client.GameEventID.RECONNECT_SUCESS:
                //m_IsReconnecting = true; 
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SYSTEM_LOADUICOMPELETE);
                break;

        }
    }


    /// <summary>
    /// 面板获取焦点状态变化
    /// </summary>
    /// <param name="focusData"></param>
    private void OnPanelFocusStatusChanged(PanelFocusData focusData)
    {
        //if (UIDefine.IsUIDebugLogEnable)
        //    Engine.Utility.Log.LogGroup("WJH", "UIPanelManager:<color=orange>{0}->{1} FOCUS</color>", focusData.ID, (focusData.GetFocus) ? "GET" : "MISSING");
    }

    /// <summary>
    /// 面板状态改变
    /// </summary>
    /// <param name="status">面板状态数据</param>
    private void OnPanelStatusChanged(UIPanelBase.PanelStatusData status)
    {
        switch (status.Status)
        {
            case UIPanelBase.PanelStatus.Awake:
                break;
            case UIPanelBase.PanelStatus.PrepareShow:
                break;
            case UIPanelBase.PanelStatus.Show:
                {
                    OnPanelShow(status.ID);
                    //播放音效
                    LocalPanelInfo localInfo = null;
                    if (TryGetLocalPanelInfo(status.ID, out localInfo) && localInfo.HaveSoundEffect)
                    {
                        DataManager.Manager<UIManager>().PlayUIAudioEffect(localInfo.SoundEffectId);
                    }
                }
                break;
            case UIPanelBase.PanelStatus.Hide:
                {
                    OnPanelHide(status.ID);
                }
                break;
            case UIPanelBase.PanelStatus.Destroy:
                {
                    //if (IsShowPanel(status.ID))
                    //{
                    //    UnityEngine.Debug.LogError("p1:" + status.Obj.transform.GetInstanceID());
                    //}
                    //RemoveHidePanel(status.ID);
                }
                break;
        }

        //if (UIDefine.IsUIDebugLogEnable)
        //    Engine.Utility.Log.Info("UIPanelManager:<color=red>{0}->{1}</color>", status.ID, status.Status.ToString().ToUpper());
        CheckFocusDatas(status.ID);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_PANELSTATUSDATACHANGED, status);
        
    }
    #endregion
}