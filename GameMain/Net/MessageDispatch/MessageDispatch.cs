using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using Common;
using Engine.Utility;
using UnityEngine.Profiling;

/// <summary>
/// 消息注册分发器
/// </summary>
public class MessageDispatcher<T> : IEnumerable<Type>
{
    #region class MessageInvoker
    private abstract class MessageInvoker
    {
        class Invoker<TMessage> : MessageInvoker
            where TMessage : T
        {
            private Action<TMessage> action;

            public override object Target { get { return action.Target; } }
            public override Type NetMessageType { get { return typeof(TMessage); } }

            public Invoker(Action<TMessage> action)
            {
                this.action = action;
            }

            public Invoker(MethodInfo methodInfo, object target)
            {

                {
                    this.action = (Action<TMessage>)methodInfo.CreateDelegate(typeof(Action<TMessage>), target);
                }
            }

            public override void Invoke(T message)
            {
                this.action((TMessage)message);
            }

            public override string ToString()
            {
                return action.GetMethodInfo().ToString();
            }
        }

        public abstract object Target { get; }
        public abstract Type NetMessageType { get; }
        public abstract void Invoke(T message);

        public static MessageInvoker Create(MethodInfo method, object target)
        {
            var param = method.GetParameters();
            // 消息响应函数参数个数判断
            if (param.Length != 1)
                return null;
            // 消息响应函数参数类型判断
            if (typeof(T).IsAssignableFrom(param[0].ParameterType) == false)
                return null;
            // 消息响应函数参数返回类型判断
            if (method.ReturnType != typeof(void) && method.ReturnType != typeof(IEnumerator))
                return null;
            return Activator.CreateInstance(
                typeof(MessageDispatcher<>.MessageInvoker.Invoker<>).MakeGenericType(typeof(T), GetMessageType(method)),
                method, target) as MessageInvoker;
        }

        /// <summary>
        /// 根据消息响应函数，得到其可以处理的消息类型
        /// </summary>
        private static Type GetMessageType(MethodInfo method)
        {
            var parameters = method.GetParameters();
            System.Diagnostics.Debug.Assert(parameters.Count() == 1);
#if !UNITY_WINRT || UNITY_EDITOR
            System.Diagnostics.Debug.Assert(typeof(T).IsAssignableFrom(parameters[0].ParameterType));
#endif
            return method.GetParameters()[0].ParameterType;
        }
    }
    #endregion

    private readonly Dictionary<Type, MessageInvoker> items = new Dictionary<Type, MessageInvoker>();

    /// <summary>
    /// 将消息发送到对应的接收者
    /// </summary>
    /// <param name="message"></param>
    /// <param name="result">消息接收者处理消息后的返回值</param>
    /// <returns>是否有对应的消息接收者并进行了分发</returns>
    public bool Dispatch(T message)
    {
        MessageInvoker invoker;
        if (items.TryGetValue(message.GetType(), out invoker))
        {
           // Profiler.BeginSample(message.GetType().ToString());
            invoker.Invoke(message);
           // Profiler.EndSample();
            return true;
        }
        else
        {
         //   Log.Error("message dipatch fail :" + message.GetType());
            return false;
        }
    }


    #region 消息响应注册

    /// <summary>
    /// 静态消息响应函数
    /// </summary>
    public void StaticRegister()
    {
        foreach (var method in ExecuteAttribute.GetStaticExecuteMethod(Assembly.GetExecutingAssembly()))
        {
            RegisterMethod(method);
        }
    }

    /// <summary>
    /// 注册对象的消息响应。
    /// 请及时调用UnRegister以取消注册，否则<paramref name="target"/>对象将不会被GC！
    /// </summary>
    public void Register(object target)
    {
        foreach (var method in ExecuteAttribute.GetInstanceExecuteMethod(target.GetType()))
        {
            RegisterMethod(method, target);
        }
    }

    public bool RegisterMethod(MethodInfo method, object target = null)
    {
        var invoker = MessageInvoker.Create(method, target);
        if (invoker == null)
            return false;
#if UNITY_EDITOR
        if (items.ContainsKey(invoker.NetMessageType))
        {
            UnityEngine.Debug.LogError(string.Format("消息类型重复: {0}", invoker.NetMessageType));
        }
#endif
        try
        {
            items.Add(invoker.NetMessageType, invoker);
        }
        catch(System.Exception e)
        {
            Engine.Utility.Log.Error("{0}:{1}",invoker.NetMessageType.ToString(),e.ToString());
        }
        return true;
    }

    /// <summary>
    /// 取消对象的消息响应注册
    /// </summary>
    /// <param name="target"></param>
    public void UnRegister(object target)
    {
        var keys = (from pair in items where pair.Value.Target == target select pair.Key).ToList();
        keys.ForEach(k => items.Remove(k));
    }

    #endregion

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var invoker in items.Values)
            sb.AppendLine(invoker.ToString());
        return sb.ToString();
    }

    #region IEnumerable<Type> 成员

    public IEnumerator<Type> GetEnumerator()
    {
        foreach (var t in this.items.Keys)
            yield return t;
    }

    #endregion

    #region IEnumerable 成员

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    #endregion
}