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
    public SceneData sceneData;
    //图片资源
    protected Image sceneImage;

    protected Image sceneMask;
    //Button组件
    protected Button gameSceneButton;
    //该场景的战斗数据
    protected BattleData battleData = null;
    //是否可以选择
    protected bool isActive=false;
    //初始化场景
    protected virtual void InitGameScene()
    {
        //获取Image和Button组件
        sceneImage = GetComponent<Image>();
        sceneMask = GameTool.GetTheChildComponent<Image>(gameObject, "Mask");
        sceneMask.enabled = false;
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
        gameSceneButton.onClick.AddListener(delegate
        {
            GameSceneManager.Instance.lastIndex = GameSceneManager.Instance.inGameSceneList.IndexOf(this);
        });
        switch (sceneData.SceneType)
        {
            //如果场景类型为普通战斗，则随机选取一个战斗场景数据
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
            //
        }
    }
    //改变场景可选状态
    public void ChangeGameSceneState(bool state)
    {
        this.isActive = state;
        if (!isActive)
        {
            gameSceneButton.interactable = false;
            sceneMask.enabled = true;
        }
        else
        {
            gameSceneButton.interactable = true;
            sceneMask.enabled = false;
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
