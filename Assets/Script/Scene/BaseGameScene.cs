using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BaseGameScene : MonoBehaviour
{
    //��������
    private SceneData sceneData;
    //ͼƬ��Դ
    private Image sceneImage;
    //Button���
    private Button gameSceneButton;
    //�ó�����ս������
    private BattleData battleData = null;
    //��ʼ������
    protected virtual void InitGameScene()
    {
        //��ȡImage��Button���
        sceneImage = GetComponent<Image>();
        gameSceneButton = GetComponent<Button>();
        //��ʼ���������ݺ�ͼƬ�ز�
        //�����ɵĳ������ݵ��±�����Ϊ���ó��������б�ǰ�ĳ�����
        sceneData = new SceneData(GameSceneManager.Instance.inGameSceneList.Count);
        sceneImage.sprite = ResourcesManager.Instance.LoadResources<Sprite>(sceneData.ResourcePath);
        GameSceneManager.Instance.inGameSceneList.Add(this);

        //�����¼�����ע���¼�
        switch (sceneData.SceneType)
        {
            case SceneType.NormalCombat:
                while (true)
                {
                    battleData =
                        BattleManager.Instance.battleDataList[
                            Random.Range(0, BattleManager.Instance.battleDataList.Count)]; 
                    if (battleData.BattleType==BattleType.Normal)
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
