using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameScene : MonoBehaviour
{
    //��������
    private SceneData sceneData;
    //ͼƬ��Դ
    private Image sceneImage;

    //��ʼ������
    protected virtual void InitGameScene()
    {
        //��ȡImage���
        sceneImage = GetComponent<Image>();
        
        //��ʼ���������ݺ�ͼƬ�ز�
        //�����ɵĳ������ݵ��±�����Ϊ���ó��������б�ǰ�ĳ�����
        sceneData = new SceneData(GameSceneManager.Instance.inGameSceneList.Count);
        Debug.Log(sceneData.Layer);
        sceneImage.sprite = ResourcesManager.Instance.LoadResources<Sprite>(sceneData.ResourcePath);
        GameSceneManager.Instance.inGameSceneList.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void Awake()
    {
        InitGameScene();
    }
}
