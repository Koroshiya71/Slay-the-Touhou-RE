using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameSceneManager : UnitySingleton<GameSceneManager>
{
    //各种场景的最大数量
    public int maxSceneNum = 20;
    //当前处于第几层
    public int currentLayer = 0;
    //上次点击的是该层第几号场景
    public int lastIndex = 0;
    //场景预制体
    private GameObject gameScenePrefab;
    //父物体
    private Transform contentParent;
    //游戏中的所有场景
    public List<BaseGameScene> inGameSceneList = new List<BaseGameScene>();
    //游戏中的Boss场景
    public BossGameScene bossScene;
    //多人共用场景列表
    public List<SceneType> multScenes=new List<SceneType>();
    //初始化游戏场景管理器
    public void InitGameSceneManager()
    {
        //获取游戏场景预制体等
        gameScenePrefab = ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/" + "Scene/" + "GameScene");
        maxSceneNum = 20;
        contentParent = GameObject.Find("Content").transform;
        //从第一层开始
        currentLayer = 1;
        //生成场景
        for (int i = 0; i < 120; i++)
        {

            GameObject newSceneGO = Instantiate(gameScenePrefab);
            if (SaveManager.isLoad)
            {
                var newScene = newSceneGO.GetComponent<BaseGameScene>();
                newScene.sceneData.sceneType = SaveManager.Instance.saveData.sceneTypeList[i];
                newScene.ResetSprite();
                currentLayer = SaveManager.Instance.saveData.mapLayer;
                lastIndex = SaveManager.Instance.saveData.mapIndex;
                newScene.ChangeGameSceneState(false);

            }

            //如果是多人游戏则获取场景类型列表
            if (GameManager.Instance.isMulti)
            {
                var newScene = newSceneGO.GetComponent<BaseGameScene>();
                newScene.sceneData = new SceneData(i);
                newScene.sceneData.sceneType = multScenes[i];
                newScene.ResetSprite();

                newScene.ChangeGameSceneState(false);

            }

            newSceneGO.transform.SetParent(contentParent);

            newSceneGO.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            newSceneGO.transform.position = new Vector2(220 + i % 6 * 300, 100 + i / 6 * 200);
        }
        UpdateGameSceneState();
        
    }

    //更新场景状态(是否可选)
    public void UpdateGameSceneState()
    {
        foreach (var gameScene in inGameSceneList)
        {
            //根据当前层数更新交互状态
            
            if (gameScene.sceneData.Layer == currentLayer)
            {
                //如果是第一层，则全部都可交互
                if (currentLayer == 1)
                {
                    gameScene.ChangeGameSceneState(true);

                }
                //如果该场景的地址与上一个点击过的地址%6后的差的绝对值小于等于1则可以交互
                if (Mathf.Abs((inGameSceneList.IndexOf(gameScene) % 6) - (lastIndex % 6)) <= 1)
                {
                    gameScene.ChangeGameSceneState(true);
                }
                continue;
            }
            //不满足上面条件的则无法交互
            gameScene.ChangeGameSceneState(false);
        }

        if (currentLayer==21)
        {
            bossScene.ChangeGameSceneState(true);
        }
        else
        {
            bossScene.ChangeGameSceneState(false);
        }
    }

    //获取随机事件类型
    public static SceneType GetRandomSceneType()
    {
        int sceneSeed = Random.Range(0, 5);
        SceneType sceneType;
        switch (sceneSeed)
        {
            case 0:
                sceneType = SceneType.NormalCombat;
                break;
            case 1:
                sceneType = SceneType.EliteCombat;
                break;
            case 2:
                sceneType = SceneType.Store;
                break;
            case 3:
                sceneType = SceneType.Event;
                break;
            case 4:
                sceneType = SceneType.Lounge;
                break;
            default:
                sceneType = SceneType.NormalCombat;
                break;
        }

        return sceneType;
    }
    void Start()
    {

    }

    private void Awake()
    {
        EventDispatcher.AddListener(E_MessageType.InitGameSceneManager, InitGameSceneManager);
    }

    void Update()
    {

    }

    public void TestJson()
    {

    }
}