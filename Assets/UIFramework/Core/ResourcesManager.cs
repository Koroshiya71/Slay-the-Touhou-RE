using UnityEngine;
using System.Collections;

namespace GameCore
{
    public class ResourcesManager : UnitySingleton<ResourcesManager>
    { 
        //<路径,资源>
        private Hashtable htPrefab ;
        void Awake()
        {
            htPrefab = new Hashtable();
        }
        //对外提供,用来加载资源的
        public T LoadResources<T>(string path,bool isCache=false) where T:UnityEngine.Object
        {
            if (htPrefab.ContainsKey(path))
            {
                return htPrefab[path] as T;
            }
            T TResources = Resources.Load<T>(path);
            if (TResources == null)
            {
                Debug.LogError(GetType() + "的LoadResources加载不到预制体,路径: " + path);
            }
            else if (isCache)
            {
                htPrefab.Add(path, TResources);
            }
            return TResources;
        }
        //对外提供,加载生成物体的方法
        public GameObject LoadAsset(string path, bool isCache = false)
        {
            GameObject goPrefab = LoadResources<GameObject>(path,isCache);
            GameObject goClone= Instantiate(goPrefab);
            if (goClone==null)
            {
                Debug.LogError(GetType() + "的LoadAsset复制不成功,路径: " + path);
            }
            return goClone;
        }
    }
}

