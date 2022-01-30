using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class BossGameScene : BaseGameScene
{
    
    public override void InitGameScene()
    {
        //��ȡImage��Button���
        sceneImage = GetComponent<Image>();
        sceneMask = GameTool.GetTheChildComponent<Image>(gameObject, "Mask");
        sceneMask.enabled = false;
        gameSceneButton = GetComponent<Button>();
        //��ʼ���������ݺ�ͼƬ�ز�
        //�����ɵĳ������ݵ��±�����Ϊ���ó��������б�ǰ�ĳ�����
        sceneData = new SceneData(true);
        sceneImage.sprite = ResourcesManager.Instance.LoadResources<Sprite>(sceneData.ResourcePath);
        GameSceneManager.Instance.bossScene = this;

        //�����¼�����ע�����¼�
        InitClickEvent();

    }
}
