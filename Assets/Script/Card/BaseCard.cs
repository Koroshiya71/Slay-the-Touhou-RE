using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class BaseCard : MonoBehaviour
{
    protected CardData cardData=new CardData();

    //����ID
    public int cardID=1001;
    //��ͼ
    private Image img_Main;
    //���ͺ�ϡ�жȱ߿�
    private Image img_OutLine;
    //��ʼ������
    public virtual void InitCard(int cardID)
    {
        cardData = new CardData(cardID);
        img_Main = GameTool.GetTheChildComponent<Image>(gameObject, "Card_Main");
        img_Main.sprite = ResourcesManager.Instance.LoadResources<Sprite>(cardData.CardImgRes);
        img_OutLine=GameTool.GetTheChildComponent<Image>(gameObject, "Card_OutLine");
        img_OutLine.sprite = GetCardOutLine(cardData.CardRare, cardData.CardType);
    }

    //��ȡ�������
    protected virtual Sprite GetCardOutLine(CardRare rare,CardType type)
    {
        string outLineName="";
        switch (type)
        {
            case CardType.TiShu:
                outLineName += "����";
                break;
            default:
                outLineName += "����";
                break;
        }
        switch (rare)
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

    protected virtual void Awake()
    {
        cardID = 1001;
        InitCard(cardID);
    }
}
