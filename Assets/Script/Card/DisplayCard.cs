using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
public class DisplayCard : BaseCard
{
    public int index;
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
            case CardType.JiNeng:
                outLineName += "技能";
                break;
            case CardType.FaShu:
                outLineName += "法术";
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





    protected override void OnDown()
    {
        base.OnDown();
        //如果正在选牌
        if (DeskManager.Instance.isChoosing)
        {
            DeskManager.Instance.hasChosenCardList.Add(cardData);
            DeskManager.Instance.originalIndex = index;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
