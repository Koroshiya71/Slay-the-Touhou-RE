using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

//��������
public enum CardType
{
    //����
    TiShu,
    //��Ļ
    DanMu,
    //����
    FaShu,
    //����
    FangYu,
    //����
    JiNeng,
    //����
    SpellCard
}
//����ϡ�ж�
public enum CardRare
{
    //��ͨ
    Normal,
    //ϡ��
    Rare,
    //ʷʫ
    Epic
}

//����ʹ�÷�ʽ
public enum CardTarget
{
    //��Ҫ���������ΪĿ��
    SingleEnemy,
    //����ҪĿ��
    MyPlayer
}

//���Ƶ���;
public enum CardUseType
{
    //��ͨ����
    NormalCard,
    //����
    SpellCard,
    //������չʾ�Ŀ���
    OnlyDisplayCard,
    //����ѡ��Ŀ���
    ChooseCard
}
public class CardEffectData
{
    //Ч��ID
    private int effectID;
    //Ч������
    private string effectDes;
    //Ч��ֵ
    private int effectValue;
    /// <summary>
    /// ����
    /// </summary>
    public int EffectID => effectID;
    public int EffectValue => effectValue;
    public string EffectDes => effectDes;

    public CardEffectData(int id,int value)
    {
        effectID = id;
        effectValue = value;
        effectDes = ReadCardEffectData("EffectDes", id);
    }

    //����cfg���ݱ��ȡ����Ч������
    private string ReadCardEffectData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicCardEffect);
        return data;
    }
}

//����������
public class CardData
{
    //����ID
    private int cardID;
    //��������
    private string cardName;
    //��ͼ·��
    private string cardImgRes;
    //���Ʒ���
    private int cardCost;
    //��������
    private string cardDes;
    //��������
    private CardType cardType;
    //����ϡ�ж�
    private CardRare cardRare;

    //����Ŀ��
    private CardTarget cardTarget;
    //������;
    private CardUseType cardUseType;

    //����Ч���ֵ�<ID,����Ч��>
    private Dictionary<int, CardEffectData> cardEffectDic=new Dictionary<int, CardEffectData>();

    ///����
    public int CardID => cardID;

    public string CardName => cardName;

    public string CardImgRes => cardImgRes;

    public int CardCost => cardCost;

    public string CardDes => cardDes;

    public CardType CardType => cardType;

    public CardRare CardRare => cardRare;

    public CardTarget CardTarget => cardTarget;

    public CardUseType CardUseType => cardUseType;

    public Dictionary<int, CardEffectData> CardEffectDic => cardEffectDic;

    //���캯��
    public CardData(int cardID=1001, string cardName = "ն��", string cardImgRes= "Image/Card/CardImg/ն��", int cardCost=1,
        string cardDes = "���6���˺�", CardType cardType = CardType.TiShu, CardRare cardRare = CardRare.Normal, CardTarget cardTarget = CardTarget.SingleEnemy, 
        CardUseType cardUseType=CardUseType.NormalCard)
    {
        this.cardID = cardID;
        this.cardName = cardName;
        this.cardImgRes = cardImgRes;
        this.cardCost = cardCost;
        this.cardDes = cardDes;
        this.cardType = cardType;
        this.cardRare = cardRare;
        this.cardTarget = cardTarget;
        this.cardUseType = cardUseType;
    }

    public CardData(int cardID)
    {
        string tempStr = "";
        this.cardID = cardID;
        this.cardName = ReadCfgCardData("Name", cardID);
        this.cardImgRes = ReadCfgCardData("ImgPath", cardID);
        this.cardCost = int.Parse(ReadCfgCardData("Cost", cardID));
        tempStr = ReadCfgCardData("Type", cardID);
        switch (tempStr)
        {
            case "����":
                cardType = CardType.TiShu;
                break;
            default:
                cardType = CardType.TiShu;
                break;
        }
        tempStr= ReadCfgCardData("Rare", cardID);
        switch (tempStr)
        {
            case "��ͨ":
                cardRare = CardRare.Normal;
                break;
            default:
                cardRare = CardRare.Normal;
                break;
        }
        tempStr = ReadCfgCardData("Target", cardID);
        switch (tempStr)
        {
            case "�������":
                cardTarget = CardTarget.SingleEnemy;
                break;
            default:
                cardTarget = CardTarget.SingleEnemy;
                break;
        }

        cardUseType = CardUseType.NormalCard;

        int effectNum = int.Parse(ReadCfgCardData("EffectNum", cardID));
        for (int i = 1; i <= effectNum; i++)
        {
            int effectID = int.Parse(ReadCfgCardData("EffectID" + i, cardID));
            int effectValue = int.Parse(ReadCfgCardData("EffectValue" + i, cardID));
            
            cardEffectDic.Add(effectID,new CardEffectData(effectID,effectValue));
            if (i>1)
            {
                cardDes += ",";
            }

            string effectDes = cardEffectDic[effectID].EffectDes.
                Replace("value", effectValue.ToString());
            cardDes += effectDes;
        }
    }

    private string ReadCfgCardData(string key,int id)
    {
        string data= DataController.Instance.ReadCfg(key, id, DataController.Instance.dicCardData);
        return data;
    }

}
