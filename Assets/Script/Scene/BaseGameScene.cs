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
    public SceneData sceneData;
    //ͼƬ��Դ
    protected Image sceneImage;

    protected Image sceneMask;
    //Button���
    protected Button gameSceneButton;
    //�ó�����ս������
    protected BattleData battleData = null;
    //�Ƿ����ѡ��
    protected bool isActive=false;
    //��ʼ������
    protected virtual void InitGameScene()
    {
        //��ȡImage��Button���
        sceneImage = GetComponent<Image>();
        sceneMask = GameTool.GetTheChildComponent<Image>(gameObject, "Mask");
        sceneMask.enabled = false;
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
        gameSceneButton.onClick.AddListener(delegate
        {
            GameSceneManager.Instance.lastIndex = GameSceneManager.Instance.inGameSceneList.IndexOf(this);
            GameSceneManager.Instance.currentLayer++;
        });
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
    //�ı䳡����ѡ״̬
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
