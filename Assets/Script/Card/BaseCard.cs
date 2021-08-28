using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class BaseCard : MonoBehaviour
{
    protected CardData cardData=new CardData();

    //卡牌ID
    public int cardID=1001;
    //卡图
    private Image img_Main;
    //类型和稀有度边框
    private Image img_OutLine;
    //初始化卡牌
    public virtual void InitCard(int cardID)
    {
        cardData = new CardData(cardID);
        img_Main = GameTool.GetTheChildComponent<Image>(gameObject, "Card_Main");
        img_Main.sprite = ResourcesManager.Instance.LoadResources<Sprite>(cardData.CardImgRes);
        img_OutLine=GameTool.GetTheChildComponent<Image>(gameObject, "Card_OutLine");
        img_OutLine.sprite = GetCardOutLine(cardData.CardRare, cardData.CardType);
    }

    //获取卡牌外框
    protected virtual Sprite GetCardOutLine(CardRare rare,CardType type)
    {
        string outLineName="";
        switch (type)
        {
            case CardType.TiShu:
                outLineName += "体术";
                break;
            default:
                outLineName += "体术";
                break;
        }
        switch (rare)
        {
            case CardRare.Normal:
                outLineName += "普通";
                break;
            default:
                outLineName += "普通";
                break;
        }

        Sprite outLine = ResourcesManager.Instance.LoadResources<Sprite>("Image/Card/CardOutLine/" + outLineName);
        return outLine;
    }

    protected virtual void Awake()
    {
        cardID = 1001;
        InitCard(cardID);
    }
}
