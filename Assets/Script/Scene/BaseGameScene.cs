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
    protected SceneData sceneData;
    //ͼƬ��Դ
    protected Image sceneImage;
    //Button���
    protected Button gameSceneButton;
    //�ó�����ս������
    protected BattleData battleData = null;
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

        //�����¼�����ע�����¼�
        InitClickEvent();

    }

    //�����¼�����ע���¼�
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
