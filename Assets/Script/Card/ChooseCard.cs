using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using UnityEngine.EventSystems;

public class ChooseCard : BaseCard
{
    //ʹ�ÿ���ʱ����Ч
    public GameObject chooseEffect;
    //��ʼ��
    public override void InitCard(CardData data)
    {
        base.InitCard(data);
        //��ȡ��Ч��Ϸ����
        chooseEffect = GameTool.FindTheChild(gameObject, "UseEffect").gameObject;
        chooseEffect.SetActive(false);
    }

    ///��ȡ�������
    protected override Sprite GetCardOutLine()
    {
        string outLineName = "";
        switch (cardData.cardType)
        {
            case CardType.TiShu:
                outLineName += "����";
                break;
            case CardType.FangYu:
                outLineName += "����";
                break;
            case CardType.JiNeng:
                outLineName += "����";
                break;
            case CardType.FaShu:
                outLineName += "����";
                break;
            default:
                outLineName += "����";
                break;
        }

        switch (cardData.cardRare)
        {
            case CardRare.Normal:
                outLineName += "��ͨ";
                break;
            case CardRare.Rare:
                outLineName += "ϡ��";
                break;
            case CardRare.Epic:
                outLineName += "ʷʫ";
                break;
            default:
                outLineName += "��ͨ";
                break;
        }

        Sprite outLine = ResourcesManager.Instance.LoadResources<Sprite>("Image/Card/CardOutLine/" + outLineName);
        return outLine;
    }


    //����¼�����

    protected override void OnDown()
    {
        //�������ѡ��
        if (DeskManager.Instance.isChoosing)
        {
            DeskManager.Instance.hasChosenCardList.Add(cardData);
            chooseEffect.SetActive(true);
        }
    }


}
