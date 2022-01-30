using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class BossGameScene : BaseGameScene
{
    
    public override void InitGameScene()
    {
        //获取Image和Button组件
        sceneImage = GetComponent<Image>();
        sceneMask = GameTool.GetTheChildComponent<Image>(gameObject, "Mask");
        sceneMask.enabled = false;
        gameSceneButton = GetComponent<Button>();
        //初始化场景数据和图片素材
        //新生成的场景数据的下标正好为将该场景加入列表前的场景数
        sceneData = new SceneData(true);
        sceneImage.sprite = ResourcesManager.Instance.LoadResources<Sprite>(sceneData.ResourcePath);
        GameSceneManager.Instance.bossScene = this;

        //根据事件类型注册点击事件
        InitClickEvent();

    }
}
