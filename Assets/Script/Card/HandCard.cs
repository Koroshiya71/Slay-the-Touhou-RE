using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;

public class HandCard : BaseCard
{


    public override void InitCard(int cardID)
    {
        base.InitCard(cardID);

    }
    //��ȡ�������
    protected override Sprite GetCardOutLine()
    {
        string outLineName = "";
        switch (cardData.CardType)
        {
            case CardType.TiShu:
                outLineName += "����";
                break;
            default:
                outLineName += "����";
                break;
        }

        switch (cardData.CardRare)
        {
            case CardRare.Normal:
                outLineName += "��ͨ";
                break;
            default:
                outLineName += "��ͨ";
                break;
        }

        Sprite outLine = ResourcesManager.Instance.LoadResources<Sprite>("Image/Card/CardOutLine/" + outLineName);
        return outLine;
    }
}
