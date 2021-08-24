using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//消息中心(观察者模式) -- 基础版本
namespace GameCore
{

    ////消息类型
    //public enum E_MessageType
    //{
    //    Message1,
    //    Message2
    //}
    public class MessageCenter
    {   
        //定义一个委托,用来存放监听到消息后所要处理的逻辑
        public delegate void DelCallBack(object obj);
        //定义一个用于存放监听的字典<消息类型,所要处理的逻辑>
        public static Dictionary<E_MessageType, DelCallBack> dicMessageType = new Dictionary<E_MessageType, DelCallBack>();
        //添加监听的方法
        public static void AddMessageListener(E_MessageType messageType, DelCallBack handler)
        {
            if (!dicMessageType.ContainsKey(messageType))
            {
                dicMessageType.Add(messageType, null);
            }          
            dicMessageType[messageType] += handler;          
        }
        //移除监听的方法
        public static void RemoveMessageListener(E_MessageType messageType, DelCallBack handler)
        {
            if (dicMessageType.ContainsKey(messageType))
            {
                dicMessageType[messageType] -= handler;
            }
        }
        //移除指定消息的所有监听
        public static void RemoveAllListenerByMessage(E_MessageType messageType)
        {
            if (dicMessageType.ContainsKey(messageType))
            {
                dicMessageType.Remove(messageType);
            }
        }
        //移除所有有消息监听
        public static void RemoveAllMessageListener()
        {
            dicMessageType.Clear();
        }
        //广播消息(触发)
        public static void SendMessage(E_MessageType messageType,object value=null)
        {
            DelCallBack del;
            if (dicMessageType.TryGetValue(messageType,out del))
            {
                if (del!=null)
                {
                    del(value);
                }
            }
        }
    }

}
