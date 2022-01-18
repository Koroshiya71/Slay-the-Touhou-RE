using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Relic : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    //��������
    public RelicData relicData;
    //����Image
    public Image relicImg;
    //���������ı�
    public Text text_Des;
    //��������ı�
    public Text text_Count;
    void Start()
    {
        
    }

    private void Awake()
    {
        relicImg = GetComponent<Image>();
        text_Des = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Des");
        text_Count = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Count");

    }

    void Update()
    {
        
    }
    //�����������ݳ�ʼ������
    public void InitRelic(RelicData data)
    {
        relicData = data;
        //��ȡSprite
        relicImg.sprite = ResourcesManager.Instance.LoadResources<Sprite>(relicData.relicSpriteRes);
        //���������ͼ�������ʾ
        text_Des.text = relicData.relicDes;
        text_Count.text = relicData.relicCount.ToString();
        if (!relicData.showCount)
        {
            text_Count.enabled = false;
        }

        text_Des.enabled = false;
    }
    //��������¼�
    public void OnPointerEnter(PointerEventData eventData)
    {
        text_Des.enabled = true;
    }
    //����뿪�¼�
    public void OnPointerExit(PointerEventData eventData)
    {
        text_Des.enabled = false;
    }
}
