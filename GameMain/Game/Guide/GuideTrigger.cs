/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Guide
 * 创建人：  wenjunhua.zqgame
 * 文件名：  GuideTrigger
 * 版本号：  V1.0.0.0
 * 创建时间：3/13/2017 9:46:28 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GuideTrigger : MonoBehaviour ,IUIEvent
{
    #region property
    private List<uint> m_lst_triggerGuideIds = null;
    public List<uint> TriggerIds
    {
        get
        {
            return m_lst_triggerGuideIds;
        }
    }

    #endregion

    #region Init
    /// <summary>
    /// 
    /// </summary>
    /// <param name="guideId">引导ID</param>
    /// <param name="dlg">委托</param>
    public void InitTrigger(uint guideId,UIEventDelegate dlg)
    {
        AddTriggerId(guideId);
        uiEventDelegate = dlg;
    }

    public void AddTriggerId(uint guideId)
    {
        if (null == m_lst_triggerGuideIds)
            m_lst_triggerGuideIds = new List<uint>();
        if (!m_lst_triggerGuideIds.Contains(guideId))
        {
            m_lst_triggerGuideIds.Add(guideId);
        }
    }

    public void RemoveTriggerId(uint guideId)
    {
        if (null != m_lst_triggerGuideIds && m_lst_triggerGuideIds.Contains(guideId))
        {
            m_lst_triggerGuideIds.Remove(guideId);
        }
    }

    /// <summary>
    /// 是否有引导触发数据
    /// </summary>
    /// <returns></returns>
    public bool HaveTriggerData()
    {
        return (null != m_lst_triggerGuideIds) && (m_lst_triggerGuideIds.Count > 0);
    }

    public void Reset()
    {
        if (null != m_lst_triggerGuideIds)
        {
            m_lst_triggerGuideIds.Clear();
        }
    }
    #endregion

    #region UIeventHandler
    private UIEventDelegate uiEventDelegate = null;
    public void RegisterUIEventDelegate(UIEventDelegate dlg)
    {
        uiEventDelegate = dlg;
    }

    public void UnRegisterUIEventDelegate()
    {
        uiEventDelegate = null; ;
    }

    public bool IsUIEventDelegateReady
    {
        get
        {
            return (null != uiEventDelegate);
        }
    }

    /// <summary>
    /// 手动激活注册事件
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="data">数据</param>
    /// <param name="param">参数</param>
    protected void InvokeUIDlg(UIEventType eventType, object data, object param)
    {
        if (IsUIEventDelegateReady)
        {
            uiEventDelegate.Invoke(eventType, data, param);
        }

    }

    #endregion

    #region IUIEvent
    /// <summary>
    /// OnClick () is sent when a mouse is pressed and released on the same object.
    /// </summary>
    public void OnClick()
    {
        InvokeUIDlg(UIEventType.Click, this, null);
    }

    /// <summary>
    /// OnSelect (selected) is sent when a mouse button is first pressed on an object. Repeated presses won't result in an OnSelect(true).
    /// </summary>
    /// <param name="selected"></param>
    public void OnSelect(bool selected)
    {
        InvokeUIDlg(UIEventType.Select, this, selected);
    }

    /// <summary>
    /// OnDoubleClick () is sent when the click happens twice within a fourth of a second.
    /// </summary>
    public void OnDoubleClick()
    {
        InvokeUIDlg(UIEventType.DoubleClick, this, null);
    }
    /// <summary>
    /// OnPress (isDown) is sent when a mouse button gets pressed on the collider.
    /// </summary>
    /// <param name="isDown"></param>
    public virtual void OnPress(bool isDown)
    {
        InvokeUIDlg(UIEventType.Press, this, isDown);
    }


    /// <summary>
    /// OnDragStart () is sent to a game object under the touch just before the OnDrag() notifications begin.
    /// </summary>
    public void OnDragStart()
    {
        InvokeUIDlg(UIEventType.DragStart, this, null);
    }

    /// <summary>
    /// OnDrag (delta) is sent to an object that's being dragged.
    /// </summary>
    /// <param name="delta"></param>
    public void OnDrag(Vector2 delta)
    {
        InvokeUIDlg(UIEventType.Dragging, this, delta);
    }


    /// <summary>
    /// OnDragOver (draggedObject) is sent to a game object when another object is dragged over its area.
    /// </summary>
    public void OnDragOver(GameObject draggedObject)
    {
        InvokeUIDlg(UIEventType.DragOver, this, draggedObject);
    }

    /// <summary>
    /// OnDragOut (draggedObject) is sent to a game object when another object is dragged out of its area.
    /// </summary>
    /// <param name="draggedObject"></param>
    public void OnDragOut(GameObject draggedObject)
    {
        InvokeUIDlg(UIEventType.DragOut, this, draggedObject);
    }

    /// <summary>
    /// OnDragEnd () is sent to a dragged object when the drag event finishes.
    /// </summary>
    public void OnDragEnd()
    {
        InvokeUIDlg(UIEventType.DragEnd, this, null);
    }

    /// <summary>
    /// OnLongPress 长按
    /// </summary>
    public void OnLongPress()
    {
        InvokeUIDlg(UIEventType.LongPress, this, null);
    }
    #endregion
}