using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreCard : DisplayCard
{
    //卡牌价格
    public int price;
    //价格文本
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
        //如果金币足够则购买
        if (price<=GameManager.Instance.playerData.gold)
        {
            //消耗金币
            GameManager.Instance.Pay(price);
            //添加卡牌到牌库
            DeskManager.Instance.deskCardList.Add(cardData);
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        
    }
}
