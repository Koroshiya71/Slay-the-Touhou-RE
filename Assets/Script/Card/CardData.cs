using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

//卡牌类型
public enum CardType
{
    //体术
    TiShu,
    //弹幕
    DanMu,
    //法术
    FaShu,
    //防御
    FangYu,
    //技能
    JiNeng,
    //符卡
    SpellCard
}
//卡牌稀有度
public enum CardRare
{
    
    //普通
    Normal,
    //稀有
    Rare,
    //史诗
    Epic
}

//卡牌使用方式
public enum CardTarget
{
    //需要单体敌人作为目标
    SingleEnemy,
    //玩家自身
    MyPlayer
}

//卡牌的用途
public enum CardUseType
{
    //普通卡牌
    NormalCard,
    //符卡
    SpellCard,
    //仅用于展示的卡牌
    OnlyDisplayCard,
    //用于选择的卡牌
    ChooseCard
}
public class CardEffectData
{
    //效果ID
    private int effectID;
    
    //效果描述
    private string effectDes;
    //效果值
    private int effectValue;
    /// <summary>
    /// 属性
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

    //根据cfg数据表读取卡牌效果数据
    private string ReadCardEffectData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicCardEffect);
        return data;
    }
}

//卡牌数据类
public class CardData
{
    //卡牌ID
    private int cardID;
    //卡牌名称
    private string cardName;
    //卡图路径
    private string cardImgRes;
    //卡牌费用
    private int cardCost;
    //卡牌描述
    private string cardDes;
    //卡牌类型
    private CardType cardType;
    //卡牌稀有度
    private CardRare cardRare;

    //卡牌目标
    private CardTarget cardTarget;
    //卡牌用途
    private CardUseType cardUseType;

    //卡牌效果字典<ID,卡牌效果>
    private Dictionary<int, CardEffectData> cardEffectDic=new Dictionary<int, CardEffectData>();

    //卡牌数据是否有被修改过
    private bool hasModified=false;

    ///属性
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

    public bool HasModified => hasModified;
    //构造函数
    public CardData(int cardID=1001, string cardName = "斩击", string cardImgRes= "Image/Card/CardImg/斩击", int cardCost=1,
        string cardDes = "造成6点伤害", CardType cardType = CardType.TiShu, CardRare cardRare = CardRare.Normal, CardTarget cardTarget = CardTarget.SingleEnemy, 
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
        hasModified = false;
        string tempStr = "";
        this.cardID = cardID;
        this.cardName = ReadCfgCardData("Name", cardID);
        this.cardImgRes = ReadCfgCardData("ImgPath", cardID);
        this.cardCost = int.Parse(ReadCfgCardData("Cost", cardID));
        tempStr = ReadCfgCardData("Type", cardID);
        switch (tempStr)
        {
            case "体术":
                cardType = CardType.TiShu;
                break;
            case "防御":
                cardType = CardType.FangYu;
                break;
            default:
                cardType = CardType.TiShu;
                break;
        }
        tempStr= ReadCfgCardData("Rare", cardID);
        switch (tempStr)
        {
            case "普通":
                cardRare = CardRare.Normal;
                break;
            default:
                cardRare = CardRare.Normal;
                break;
        }
        tempStr = ReadCfgCardData("Target", cardID);
        switch (tempStr)
        {
            case "单体敌人":
                cardTarget = CardTarget.SingleEnemy;
                break;
            case "玩家":
                cardTarget = cardTarget = CardTarget.MyPlayer;
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
    //克隆方法
    public CardData(CardData data)
    {
        hasModified = false;
        this.cardID = data.CardID;
        this.cardName = data.CardName;
        this.cardImgRes = data.CardImgRes;
        this.cardCost = data.CardCost;
        this.cardDes = data.CardDes;
        this.cardType = data.CardType;
        this.cardRare = data.CardRare;
        this.cardTarget = data.cardTarget;
        this.cardUseType = data.cardUseType;
        this.cardEffectDic = new Dictionary<int, CardEffectData>();
        foreach (var effectElement in data.CardEffectDic)
        {
            cardEffectDic.Add(effectElement.Key,effectElement.Value);
        }
    }
    private string ReadCfgCardData(string key,int id)
    {
        string data= DataController.Instance.ReadCfg(key, id, DataController.Instance.dicCardData);
        return data;
    }

}
