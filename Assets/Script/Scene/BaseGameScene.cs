using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameScene : MonoBehaviour
{
    //场景数据
    private SceneData sceneData;
    //图片资源
    private Image sceneImage;

    //初始化场景
    protected virtual void InitGameScene()
    {
        //获取Image组件
        sceneImage = GetComponent<Image>();
        
        //初始化场景数据和图片素材
        //新生成的场景数据的下标正好为将该场景加入列表前的场景数
        sceneData = new SceneData(GameSceneManager.Instance.inGameSceneList.Count);
        Debug.Log(sceneData.Layer);
        sceneImage.sprite = ResourcesManager.Instance.LoadResources<Sprite>(sceneData.ResourcePath);
        GameSceneManager.Instance.inGameSceneList.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void Awake()
    {
        InitGameScene();
    }
}
