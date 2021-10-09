using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BaseGameScene : MonoBehaviour
{
    //场景数据
    protected SceneData sceneData;
    //图片资源
    protected Image sceneImage;
    //Button组件
    protected Button gameSceneButton;
    //该场景的战斗数据
    protected BattleData battleData = null;
    //初始化场景
    protected virtual void InitGameScene()
    {
        //获取Image和Button组件
        sceneImage = GetComponent<Image>();
        gameSceneButton = GetComponent<Button>();
        //初始化场景数据和图片素材
        //新生成的场景数据的下标正好为将该场景加入列表前的场景数
        sceneData = new SceneData(GameSceneManager.Instance.inGameSceneList.Count);
        sceneImage.sprite = ResourcesManager.Instance.LoadResources<Sprite>(sceneData.ResourcePath);
        GameSceneManager.Instance.inGameSceneList.Add(this);

        //根据事件类型注册点击事件
        InitClickEvent();

    }

    //根据事件类型注册事件
    protected virtual void InitClickEvent()
    {
        switch (sceneData.SceneType)
        {
            case SceneType.NormalCombat:
                while (true)
                {
                    battleData =
                        BattleManager.Instance.battleDataDic[
                            Random.Range(1, BattleManager.Instance.battleDataDic.Count+1)];
                    if (battleData.BattleType == BattleType.Normal)
                        break;
                }
                gameSceneButton.onClick.AddListener(delegate
                {
                    BattleManager.Instance.InitBattle(battleData);
                });
                break;
        }
    }
    
    void Update()
    {
        
    }

    protected virtual void Awake()
    {
        InitGameScene();
    }
}
