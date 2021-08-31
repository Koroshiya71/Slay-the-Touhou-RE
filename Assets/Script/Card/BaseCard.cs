using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseCard : MonoBehaviour
{
    //卡牌数据
    protected CardData cardData=new CardData();

    //卡牌ID
    public int cardID=1001;
    //卡图
    protected Image img_Main;
    //卡牌外边框
    protected Image img_OutLine;
    //卡牌效果描述文本
    protected Text text_CardEffect;
    //事件触发器
    protected EventTrigger eventTrigger;
    //初始化卡牌
    public virtual void InitCard(int cardID)
    {
        cardData = new CardData(cardID);
        this.cardID = cardID;
        img_Main = GameTool.GetTheChildComponent<Image>(gameObject, "Card_Main");
        img_Main.sprite = ResourcesManager.Instance.LoadResources<Sprite>(cardData.CardImgRes);
        img_OutLine = GameTool.GetTheChildComponent<Image>(gameObject, "Card_OutLine");
        img_OutLine.sprite = GetCardOutLine();
        text_CardEffect = GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardEffect");
        text_CardEffect.text = cardData.CardDes;
    }

    //获取卡牌外框
    protected virtual Sprite GetCardOutLine()
    {
        return null;
    }

    protected virtual void Awake()
    {
        InitCard(1001);
    }
}
