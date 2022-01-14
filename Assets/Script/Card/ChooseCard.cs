using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using UnityEngine.EventSystems;

public class ChooseCard : BaseCard
{
    //使用卡牌时的特效
    private GameObject chooseEffect;
    //初始化
    public override void InitCard(CardData data)
    {
        base.InitCard(data);
        //获取特效游戏物体
        chooseEffect = GameTool.FindTheChild(gameObject, "UseEffect").gameObject;
        chooseEffect.SetActive(false);
    }

    //获取卡牌外框
    protected override Sprite GetCardOutLine()
    {
        string outLineName = "";
        switch (cardData.cardType)
        {
            case CardType.TiShu:
                outLineName += "体术";
                break;
            case CardType.FangYu:
                outLineName += "防御";
                break;
            default:
                outLineName += "体术";
                break;
        }

        switch (cardData.cardRare)
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


    //点击事件监听

    protected override void OnDown()
    {
        Debug.Log("click");
        //如果正在选牌
        if (DeskManager.Instance.isChoosing)
        {
            DeskManager.Instance.hasChosenCardList.Add(cardData);
            chooseEffect.SetActive(true);
        }
    }


}
