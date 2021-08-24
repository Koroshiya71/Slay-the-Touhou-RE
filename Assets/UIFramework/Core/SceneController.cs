using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

namespace GameCore
{
    public class SceneController : Singleton<SceneController>
    {
        //是否是第一次加载主场景,默认是(如果是就要动态加载画布)
        //public static bool isFirstLoadMainScene = true;
        //将要异步加载的场景名称
        public string sceneNameAsync;
        //场景异步加载完以后所要处理的逻辑
        public Action handlerAsync;
        /// <summary>
        /// 直接加载场景的方法
        /// </summary>
        /// <param name="sceneName">将要加载的场景名称</param>
        /// <param name="handler">场景加载完显示出来后所要处理的逻辑</param>
        public void LoadScene(string sceneName, Action handler = null)
        {
            SceneManager.LoadScene(sceneName);
            if (handler != null)
            {
                handler();
            }
        }
        //异步加载场景的方法
        public void LoadSceneAsync(string sceneName, Action handler = null)
        {
            sceneNameAsync = sceneName;
            handlerAsync = handler;
            SceneManager.LoadScene("LoadingScene");
            UIManager.Instance.ShowUI(E_UiId.LoadingUI);
        }
    }
}

