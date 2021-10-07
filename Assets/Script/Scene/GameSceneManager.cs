using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class GameSceneManager : UnitySingleton<GameSceneManager>
{
    //���ֳ������������
    public int maxSceneNum = 20;

    //����Ԥ����
    private GameObject gameScenePrefab;
    //������
    private Transform contentParent;
    //��Ϸ�е����г���
    public List<BaseGameScene> inGameSceneList=new List<BaseGameScene>();


    //��ʼ����Ϸ����������
    public void InitGameSceneManager()
    {
        //��ȡ��Ϸ����Ԥ�����
        gameScenePrefab = ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/" + "Scene/" + "GameScene");
        maxSceneNum = 20;
        contentParent = GameObject.Find("Content").transform;
        //���ɳ���
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
