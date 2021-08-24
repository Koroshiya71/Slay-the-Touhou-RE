using UnityEngine;
using System.Collections;
using System;
//工具类,把一些常用到的方法放在这个类里面
//为了方便外界调用,把里面的方法都设置为静态的
public class GameTool : MonoBehaviour
{
    //清理内存(一般切换场景的时候调用)
    public static void ClearMemory()
    {   
        //手动调用垃圾回收
        //调用垃圾回收是会消耗性能的,所以不能频繁去调用
        GC.Collect();
        //卸载内存中没用的资源
        Resources.UnloadUnusedAssets();
    }
    //操作内存,数据持久化
    //判断系统n内存里面是否有包含某个键
    public static bool HasKey(string  key)
    {   
        return PlayerPrefs.HasKey(key);
    }
    //取值
    public static int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }
    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }
    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }
    //存值
    public static void SetInt(string key,int value)
    {
        PlayerPrefs.SetInt(key,value);
    }
    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
    //删除系统内存中指定的键值对
    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
    //删除系统内存中所有的键值对
    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
    //分割字符串
    public static string[] SplitString(string str, char c)
    {
        return str.Split(c);
    }
    //查找子物体
    public static Transform FindTheChild(GameObject goParent,string childName)
    {
        Transform searchTrans = goParent.transform.Find(childName);
        if (searchTrans == null)
        {
            foreach (Transform trans in goParent.transform)
            {
                searchTrans = FindTheChild(trans.gameObject, childName);
                if (searchTrans != null)
                {
                    return searchTrans;
                }
            }
        }
        return searchTrans;
    }
    //获取子物体上面的组件
    public static T GetTheChildComponent<T>(GameObject goParent, string childName) where T :Component
    {
        Transform searchTrans = FindTheChild(goParent, childName);
        if (searchTrans != null)
        {
            return searchTrans.GetComponent<T>();
        }
        else
        {
            return null;
        }

    }
    //给子物体添加组件
    public static T AddTheChildComponent<T>(GameObject goParent, string childName) where T : Component
    {
        Transform searchTrans = FindTheChild(goParent, childName);
        if (searchTrans != null)
        {
            T[] arr = searchTrans.GetComponents<T>();
            for (int i = 0; i < arr.Length; i++)
            {
                //Destroy(arr[i]);//销毁,但不是立刻销毁(当前帧结束前被销毁)
                DestroyImmediate(arr[i],true);//立刻销毁
            }
            return searchTrans.gameObject.AddComponent<T>();
        }
        else
        {
            return null;
        }
    }
    //添加子物体
    public static void AddChildToParent(Transform parentTrans, Transform childTrans)
    {
        childTrans.parent = parentTrans;
        childTrans.localPosition = Vector3.zero;
        childTrans.localScale = Vector3.one;
    }
}
