using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreRelic : Relic,IPointerDownHandler
{
    //�۸�
    private int price;
    //�۸��ı�
    private Text text_Price;
    //�۸���Ϸ����
    private GameObject priceObj;
    //��ʼ��
    protected override void Init()
    {
        base.Init();
        text_Price = GameTool.GetTheChildComponent<Text>(gameObject,"Text_Price");
        priceObj = GameTool.FindTheChild(gameObject, "Price").gameObject;
    }

    public override void InitRelic(RelicData data)
    {
        base.InitRelic(data);
        price=GameManager.Instance.storeRelicPriceDic[relicData.relicID];
        text_Price.text = price.ToString();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        priceObj.SetActive(false);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        priceObj.SetActive(true);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //���Ǯ���͹���
        if (price<=GameManager.Instance.playerData.gold)
        {
            GameManager.Instance.playerData.gold -= price;
            RelicManager.Instance.GetRelic(this.relicData.relicID);
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
