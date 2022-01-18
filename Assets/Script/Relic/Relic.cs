using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Relic : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    //遗物数据
    public RelicData relicData;
    //遗物Image
    public Image relicImg;
    //遗物描述文本
    public Text text_Des;
    //遗物计数文本
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
    //根据遗物数据初始化遗物
    public void InitRelic(RelicData data)
    {
        relicData = data;
        //读取Sprite
        relicImg.sprite = ResourcesManager.Instance.LoadResources<Sprite>(relicData.relicSpriteRes);
        //设置描述和计数等显示
        text_Des.text = relicData.relicDes;
        text_Count.text = relicData.relicCount.ToString();
        if (!relicData.showCount)
        {
            text_Count.enabled = false;
        }

        text_Des.enabled = false;
    }
    //鼠标悬浮事件
    public void OnPointerEnter(PointerEventData eventData)
    {
        text_Des.enabled = true;
    }
    //鼠标离开事件
    public void OnPointerExit(PointerEventData eventData)
    {
        text_Des.enabled = false;
    }
}
