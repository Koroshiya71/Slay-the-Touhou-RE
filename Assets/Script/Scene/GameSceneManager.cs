using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class GameSceneManager : UnitySingleton<GameSceneManager>
{
    //各种场景的最大数量
    public int maxSceneNum = 20;

    //场景预制体
    private GameObject gameScenePrefab;
    //父物体
    private Transform contentParent;
    //游戏中的所有场景
    public List<BaseGameScene> inGameSceneList=new List<BaseGameScene>();


    //初始化游戏场景管理器
    public void InitGameSceneManager()
    {
        //获取游戏场景预制体等
        gameScenePrefab = ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/" + "Scene/" + "GameScene");
        maxSceneNum = 20;
        contentParent = GameObject.Find("Content").transform;
        //生成场景
        for (int i = 0; i < 120; i++)
        {
            GameObject newSceneGO = Instantiate(gameScenePrefab);
            newSceneGO.transform.SetParent(contentParent);
            newSceneGO.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            newSceneGO.transform.position = new Vector2(220 + i % 6 * 300, 100 + i / 6 * 200);

        }

    }
    void Start()
    {
        
    }

    private void Awake()
    {
        EventDispatcher.AddListener(E_MessageType.GameStart,InitGameSceneManager);
    }

    void Update()
    {
        
    }
}
