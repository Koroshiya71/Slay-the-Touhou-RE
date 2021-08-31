using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseCard : MonoBehaviour
{
    //��������
    protected CardData cardData=new CardData();

    //����ID
    public int cardID=1001;
    //��ͼ
    protected Image img_Main;
    //������߿�
    protected Image img_OutLine;
    //����Ч�������ı�
    protected Text text_CardEffect;
    //�¼�������
    protected EventTrigger eventTrigger;
    //��ʼ������
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

    //��ȡ�������
    protected virtual Sprite GetCardOutLine()
    {
        return null;
    }

    protected virtual void Awake()
    {
        InitCard(1001);
    }
}
