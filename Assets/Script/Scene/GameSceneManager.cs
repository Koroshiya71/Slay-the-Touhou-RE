using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class GameSceneManager : UnitySingleton<GameSceneManager>
{
    //���ֳ������������
    public int maxSceneNum = 20;
    //��ǰ���ڵڼ���
    public int currentLayer = 0;
    //�ϴε�����Ǹò�ڼ��ų���
    public int lastIndex = 0;
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
        //�ӵ�һ�㿪ʼ
        currentLayer = 1;

        UpdateGameSceneState();
    }

    //���³���״̬(�Ƿ��ѡ)
    public void UpdateGameSceneState()
    {
        foreach (var gameScene in inGameSceneList)
        {
            //���ݵ�ǰ�������½���״̬
            if (gameScene.sceneData.Layer==currentLayer)
            {
                //����ǵ�һ�㣬��ȫ�����ɽ���
                if (currentLayer==1)
                {
                    gameScene.ChangeGameSceneState(true);
                }
                //����ó����ĵ�ַ����һ��������ĵ�ַ%6��Ĳ�ľ���ֵС�ڵ���1����Խ���
                if (Mathf.Abs((inGameSceneList.IndexOf(gameScene)%6)-(lastIndex%6))<=1)
                {
                    gameScene.ChangeGameSceneState(true);
                }

                continue;
            }
            //�������������������޷�����
            gameScene.ChangeGameSceneState(false);
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
