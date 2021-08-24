
using System;
using System.Collections.Generic;

namespace GameCore
{
    /// <summary>
    /// 消息控制类（事件处理类）。
    /// </summary>
    public class EventController
    {
        //定义一个用于存放监听的字典<消息类型,所要处理的逻辑>
        private Dictionary<E_MessageType, Delegate> dicMessageType = new Dictionary<E_MessageType, Delegate>();

        public Dictionary<E_MessageType, Delegate> DicMessageType
        {
            //为什么没有set?
            //答:因为要控制dicMessageType这个字典外界只能获取值,而不能在外界直接修改它的值
            get { return dicMessageType; }
        }

        /// <summary>
        /// 判断是否已经包含事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public bool ContainsEvent(E_MessageType eventType)
        {
            return dicMessageType.ContainsKey(eventType);
        }
        //清除所有监听
        public void ClearAllListener()
        {   
            //直接清空字典
            dicMessageType.Clear();
        }
       
        /// <summary>
        /// 处理增加监听器前的事项， 检查 参数等
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="listenerBeingAdded"></param>
        private void OnListenerAdding(E_MessageType eventType, Delegate listenerBeingAdded)
        {

            if (!dicMessageType.ContainsKey(eventType))
            {
                dicMessageType.Add(eventType, null);
            }

            //Delegate d = dicMessageType[eventType];
            //if (d != null && d.GetType() != listenerBeingAdded.GetType())
            //{
            //    throw new Exception(string.Format(
            //        "Try to add not correct event {0}. Current type is {1}, adding type is {2}.",
            //        eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
            //}
        }

        /// <summary>
        /// 移除监听器之前的检查
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="listenerBeingRemoved"></param>
        private bool OnListenerRemoving(E_MessageType eventType, Delegate listenerBeingRemoved)
        {

            if (!dicMessageType.ContainsKey(eventType))
            {
                return false;
            }

            //Delegate d = dicMessageType[eventType];
            //if ((d != null) && (d.GetType() != listenerBeingRemoved.GetType()))
            //{
            //    throw new Exception(string.Format(
            //        "Remove listener {0}\" failed, Current type is {1}, adding type is {2}.",
            //        eventType, d.GetType(), listenerBeingRemoved.GetType()));
            //}
            //else
             return true;
        }

        /// <summary>
        /// 移除监听器之后的处理。删掉事件
        /// </summary>
        /// <param name="eventType"></param>
        private void OnListenerRemoved(E_MessageType eventType)
        {
            if (dicMessageType.ContainsKey(eventType) && dicMessageType[eventType] == null)
            {
                dicMessageType.Remove(eventType);
            }
        }

        #region 增加监听器
        /// <summary>
        ///  增加监听器， 不带参数
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void AddListener(E_MessageType eventType, Action handler)
        {
            OnListenerAdding(eventType, handler);
            //if (!dicMessageType.ContainsKey(eventType))
            //{
            //    dicMessageType.Add(eventType, null);
            //}
            dicMessageType[eventType] = (Action)dicMessageType[eventType] + handler;
        }
       
        /// <summary>
        ///  增加监听器， 1个参数
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void AddListener<T>(E_MessageType eventType, Action<T> handler)
        {
            OnListenerAdding(eventType, handler);
            dicMessageType[eventType] = (Action<T>)dicMessageType[eventType] + handler;
        }

        /// <summary>
        ///  增加监听器， 2个参数
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void AddListener<T, U>(E_MessageType eventType, Action<T, U> handler)
        {
            OnListenerAdding(eventType, handler);
            dicMessageType[eventType] = (Action<T, U>)dicMessageType[eventType] + handler;
        }

        /// <summary>
        ///  增加监听器， 3个参数
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void AddListener<T, U, V>(E_MessageType eventType, Action<T, U, V> handler)
        {
            OnListenerAdding(eventType, handler);
            dicMessageType[eventType] = (Action<T, U, V>)dicMessageType[eventType] + handler;
        }

        /// <summary>
        ///  增加监听器， 4个参数
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void AddListener<T, U, V, W>(E_MessageType eventType, Action<T, U, V, W> handler)
        {
            OnListenerAdding(eventType, handler);
            dicMessageType[eventType] = (Action<T, U, V, W>)dicMessageType[eventType] + handler;
        }
        #endregion

        #region 移除监听器

        /// <summary>
        ///  移除监听器， 不带参数
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void RemoveListener(E_MessageType eventType, Action handler)
        {
            //if (dicMessageType.ContainsKey(eventType))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            if (OnListenerRemoving(eventType, handler))
            {
                dicMessageType[eventType] = (Action)dicMessageType[eventType] - handler;
                OnListenerRemoved(eventType);
            }
        }

        /// <summary>
        ///  移除监听器， 1个参数
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void RemoveListener<T>(E_MessageType eventType, Action<T> handler)
        {
            if (OnListenerRemoving(eventType, handler))
            {
                dicMessageType[eventType] = (Action<T>)dicMessageType[eventType] - handler;
                OnListenerRemoved(eventType);
            }
        }

        /// <summary>
        ///  移除监听器， 2个参数
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void RemoveListener<T, U>(E_MessageType eventType, Action<T, U> handler)
        {
            if (OnListenerRemoving(eventType, handler))
            {
                dicMessageType[eventType] = (Action<T, U>)dicMessageType[eventType] - handler;
                OnListenerRemoved(eventType);
            }
        }

        /// <summary>
        ///  移除监听器， 3个参数
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void RemoveListener<T, U, V>(E_MessageType eventType, Action<T, U, V> handler)
        {
            if (OnListenerRemoving(eventType, handler))
            {
                dicMessageType[eventType] = (Action<T, U, V>)dicMessageType[eventType] - handler;
                OnListenerRemoved(eventType);
            }
        }

        /// <summary>
        ///  移除监听器， 4个参数
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void RemoveListener<T, U, V, W>(E_MessageType eventType, Action<T, U, V, W> handler)
        {
            if (OnListenerRemoving(eventType, handler))
            {
                dicMessageType[eventType] = (Action<T, U, V, W>)dicMessageType[eventType] - handler;
                OnListenerRemoved(eventType);
            }
        }
        #endregion

        #region 触发事件
        /// <summary>
        ///  触发事件， 不带参数触发
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void TriggerEvent(E_MessageType eventType)
        {
            Delegate d;
            if (!dicMessageType.TryGetValue(eventType, out d))
            {
                return;
            }
            // Delegate[] callbacks = d.GetInvocationList();
            //获取方法列表
            var callbacks = d.GetInvocationList();
            for (int i = 0; i < callbacks.Length; i++)
            {
                Action callback = callbacks[i] as Action;

                if (callback == null)
                {
                    throw new Exception(string.Format("TriggerEvent {0} error: types of parameters are not match.", eventType));
                }

                try
                {

                    callback();
                }
                catch (Exception ex)
                {
                   
                }
            }
        }

        /// <summary>
        ///  触发事件， 带1个参数触发
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void TriggerEvent<T>(E_MessageType eventType, T arg1)
        {


            Delegate d;
            if (!dicMessageType.TryGetValue(eventType, out d))
            {
                return;
            }

            var callbacks = d.GetInvocationList();
            for (int i = 0; i < callbacks.Length; i++)
            {
                Action<T> callback = callbacks[i] as Action<T>;

                if (callback == null)
                {
                    throw new Exception(string.Format("TriggerEvent {0} error: types of parameters are not match.", eventType));
                }

                try
                {
                    callback(arg1);
                }
                catch (Exception ex)
                {
                   
                }
            }
        }

        /// <summary>
        ///  触发事件， 带2个参数触发
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void TriggerEvent<T, U>(E_MessageType eventType, T arg1, U arg2)
        {
            Delegate d;
            if (!dicMessageType.TryGetValue(eventType, out d))
            {
                return;
            }
            var callbacks = d.GetInvocationList();
            for (int i = 0; i < callbacks.Length; i++)
            {
                Action<T, U> callback = callbacks[i] as Action<T, U>;

                if (callback == null)
                {
                    throw new Exception(string.Format("TriggerEvent {0} error: types of parameters are not match.", eventType));
                }

                try
                {
                    callback(arg1, arg2);
                }
                catch (Exception ex)
                {
                 
                }
            }
        }

        /// <summary>
        ///  触发事件， 带3个参数触发
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void TriggerEvent<T, U, V>(E_MessageType eventType, T arg1, U arg2, V arg3)
        {
            Delegate d;
            if (!dicMessageType.TryGetValue(eventType, out d))
            {
                return;
            }
            var callbacks = d.GetInvocationList();
            for (int i = 0; i < callbacks.Length; i++)
            {
                Action<T, U, V> callback = callbacks[i] as Action<T, U, V>;

                if (callback == null)
                {
                    throw new Exception(string.Format("TriggerEvent {0} error: types of parameters are not match.", eventType));
                }
                try
                {
                    callback(arg1, arg2, arg3);
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        /// <summary>
        ///  触发事件， 带4个参数触发
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void TriggerEvent<T, U, V, W>(E_MessageType eventType, T arg1, U arg2, V arg3, W arg4)
        {
            Delegate d;
            if (!dicMessageType.TryGetValue(eventType, out d))
            {
                return;
            }
            var callbacks = d.GetInvocationList();
            for (int i = 0; i < callbacks.Length; i++)
            {
                Action<T, U, V, W> callback = callbacks[i] as Action<T, U, V, W>;

                if (callback == null)
                {
                    throw new Exception(string.Format("TriggerEvent {0} error: types of parameters are not match.", eventType));
                }
                try
                {
                    callback(arg1, arg2, arg3, arg4);
                }
                catch (Exception ex)
                {
                    //DebugUtil.Except(ex);
                }
            }
        }

        #endregion
    }

}