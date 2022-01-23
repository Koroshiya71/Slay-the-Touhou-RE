using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreCard : DisplayCard
{
    //���Ƽ۸�
    public int price;
    //�۸��ı�
    public Text text_Price;
    void Start()
    {
        
    }

    public override void InitCard(CardData data)
    {
        base.InitCard(data);
        price = GameManager.Instance.storeCardPriceDic[data.cardID];
        text_Price = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Price");
        text_Price.text = price.ToString();
    }

    public override void InitCard(int cardID)
    {
        base.InitCard(cardID);
        price = GameManager.Instance.storeCardPriceDic[cardID];
        text_Price = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Price");
        text_Price.text = price.ToString();
    }

    protected override void OnDown()
    {
        base.OnDown();
        //�������㹻����
        if (price<=GameManager.Instance.playerData.gold)
        {
            //���Ľ��
            GameManager.Instance.Pay(price);
            //��ӿ��Ƶ��ƿ�
            DeskManager.Instance.deskCardList.Add(cardData);
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        
    }
}
