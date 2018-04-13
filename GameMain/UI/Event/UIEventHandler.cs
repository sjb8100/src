using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UIEventHandler : Singleton<UIEventHandler>
{
    #region Property
    //消息回调委托
    public delegate void UIGlobalEventDelegate(Client.GameEventID eventType, object data);
    //消息注册字典
    private Dictionary<Client.GameEventID, UIGlobalEventDelegate> eventDic = new Dictionary<Client.GameEventID, UIGlobalEventDelegate>();
    #endregion

    #region Register & UnRegister
    /// <summary>
    /// 注册UI事件
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="callback">回调</param>
    public void Register(Client.GameEventID eventType,UIGlobalEventDelegate callback)
    {
        if (null == callback)
            return;
        UIGlobalEventDelegate eventDlg = null;
        eventDic.TryGetValue(eventType, out eventDlg);
        if (null != eventDlg)
        {
            eventDlg -= callback;
            eventDlg += callback;
            eventDic[eventType] = eventDlg;
        }else
        {
            eventDic[eventType] = callback;
        }
    }

    /// <summary>
    /// 取消注册某一小心类型的单个回调
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callback"></param>
    public void UnRegister(Client.GameEventID eventType, UIGlobalEventDelegate callback)
    {
        if (null == callback)
            return;
        UIGlobalEventDelegate eventDlg = null;
        eventDic.TryGetValue(eventType, out eventDlg);
        if (null != eventDlg)
        {
            eventDlg -= callback;
            eventDic[eventType] = eventDlg;
        }
    }


    /// <summary>
    /// 取消某一类型消息的注册
    /// </summary>
    /// <param name="eventType"></param>
    public void UnRegister(Client.GameEventID eventType)
    {
        if (eventDic.ContainsKey(eventType))
            eventDic.Remove(eventType);
    }

    /// <summary>
    /// 取消所有小心注册
    /// </summary>
    public void UnRegisterAll()
    {
        eventDic.Clear();
    }

    #endregion

    #region Dispatcher
    /// <summary>
    /// 分发消息
    /// </summary>
    /// <param name="eventType">小心类型</param>
    public void Dispatch(Client.GameEventID eventType,object data = null)
    {
        UIGlobalEventDelegate eventDlg = null;
        if (eventDic.TryGetValue(eventType,out eventDlg) && null != eventDlg)
        {
            eventDlg.Invoke(eventType,data);
        }
    }
    #endregion
}