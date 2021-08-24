//using UnityEngine;
//using System.Collections;

//public class GameDebuger : MonoBehaviour {

//    //Debug很多地方调用,会影响性能,但是又经常用到
//    //需要将Debug进行二次封装,设置开关
//    //开发过程中就把开关打开,发布安装包的时候就把开关关掉

//    //启用Debug的开关
//    public static bool isOpen = true;

//    //普通的输出提示
//    public static void Log(object message)
//    {
//        Log(message,null);      
//    }
//    public static void Log(object message, Object context)
//    {
//        if (isOpen)
//        {
//            Debug.Log(message, context);
//        }      
//    }
//    //警告提示的输出
//    public static void LogWarning(object message)
//    {
//        LogWarning(message, null);
//    }
//    public static void LogWarning(object message, Object context)
//    {
//        if (isOpen)
//        {
//            Debug.LogWarning(message, context);
//        }
//    }
//    //错误提示的输出
//    public static void LogError(object message)
//    {
//        LogError(message, null);
//    }
//    public static void LogError(object message, Object context)
//    {
//        if (isOpen)
//        {
//            Debug.LogError(message, context);
//        }
//    }

//}
