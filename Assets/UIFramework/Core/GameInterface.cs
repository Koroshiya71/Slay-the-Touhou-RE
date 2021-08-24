using UnityEngine;
using System.Collections;

namespace GameCore
{   
    //接口：行为规范
    //接口的首字母都应该是大写的I
    interface IMessageListener
    {
        void AddMessageListener();
        void RemoveMessageListener();
    }
}