using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
public class DisplayCard : BaseCard
{
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
            case CardRare.Rare:
                outLineName += "稀有";
                break;
            case CardRare.Epic:
                outLineName += "史诗";
                break;
            default:
                outLineName += "普通";
                break;
        }

        Sprite outLine = ResourcesManager.Instance.LoadResources<Sprite>("Image/Card/CardOutLine/" + outLineName);
        return outLine;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
