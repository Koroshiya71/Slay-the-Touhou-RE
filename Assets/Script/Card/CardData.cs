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

//卡牌效果类型
public enum CardEffectType
{
    Damage, //伤害
    Shield, //护甲
    Buff, //效果
    Special, //特殊（以上基本效果以外的其他东西）
}

//卡牌使用方式
public enum CardTarget
{
    //需要单体敌人作为目标
    SingleEnemy,

    //玩家自身
    MyPlayer,

    //全部敌人
    AllEnemy
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

    //残心、连斩
    public bool isCanXin = false;

    public int combo = 0;

    //效果类型
    public CardEffectType effectType;

    /// <summary>
    /// 属性
    /// </summary>
    public int EffectID => effectID;

    public int EffectValue
    {
        get => effectValue;
        set => effectValue = value;
    }

    public string EffectDes
    {
        get => effectDes;
        set => effectDes = value;
    }

    //实际效果值
    public int actualValue;

    public CardEffectData(int id, int value)
    {
        effectID = id;
        effectValue = value;
        actualValue = value;
        effectDes = ReadCardEffectData("EffectDes", id);
        isCanXin = ReadCardEffectData("IsCanXin", id) == "1";
        combo = int.Parse(ReadCardEffectData("LianZhan", id));
        effectDes = effectDes.Replace("value", actualValue.ToString());
        if (combo != 0)
        {
            effectDes = effectDes.Replace("combo", combo.ToString());
        }

        string typeStr = ReadCardEffectData("EffectType", id);
        switch (typeStr)
        {
            case "伤害":
                effectType = CardEffectType.Damage;
                break;
            case "效果":
                effectType = CardEffectType.Buff;
                break;
            case "护甲":
                effectType = CardEffectType.Shield;
                break;
            case "特殊":
                effectType = CardEffectType.Special;
                break;
        }
    }

    public CardEffectData()
    {
    }

    //根据cfg数据表读取卡牌效果数据
    private string ReadCardEffectData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicCardEffect);
        return data;
    }
}

//卡牌数据类
[Serializable]
public class CardData
{
    //卡牌ID
    public int cardID;

    //卡牌名称
    public string cardName;

    //卡图路径
    public string cardImgRes;

    //卡牌费用
    public int cardCost;

    //卡牌描述
    public string cardDes;

    //卡牌类型
    public CardType cardType;

    //卡牌稀有度
    public CardRare cardRare;

    //卡牌目标
    public CardTarget cardTarget;

    //卡牌用途
    public CardUseType cardUseType;

    //卡牌效果字典<ID,卡牌效果>
    public Dictionary<int, CardEffectData> cardEffectDic = new Dictionary<int, CardEffectData>();

    //卡牌数据是否有被修改过
    public bool hasModified = false;

    //卡牌原始费用
    public int originCost;


    //构造函数
    public CardData(int cardID = 1001, string cardName = "斩击", string cardImgRes = "Image/Card/CardImg/斩击",
        int cardCost = 1,
        string cardDes = "造成6点伤害", CardType cardType = CardType.TiShu, CardRare cardRare = CardRare.Normal,
        CardTarget cardTarget = CardTarget.SingleEnemy,
        CardUseType cardUseType = CardUseType.NormalCard)
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
        originCost = cardCost;
    }

    public CardData(int cardID)
    {
        hasModified = false;
        string tempStr = "";
        this.cardID = cardID;
        this.cardName = ReadCfgCardData("Name", cardID);
        this.cardImgRes = ReadCfgCardData("ImgPath", cardID);
        this.cardCost = int.Parse(ReadCfgCardData("Cost", cardID));
        originCost = this.cardCost;
        tempStr = ReadCfgCardData("Type", cardID);
        switch (tempStr)
        {
            case "体术":
                cardType = CardType.TiShu;
                break;
            case "防御":
                cardType = CardType.FangYu;
                break;
            case "技能":
                cardType = CardType.JiNeng;
                break;
            case "法术":
                cardType = CardType.FaShu;
                break;
            default:
                cardType = CardType.TiShu;
                break;
        }

        tempStr = ReadCfgCardData("Rare", cardID);
        switch (tempStr)
        {
            case "普通":
                cardRare = CardRare.Normal;
                break;
            case "稀有":
                cardRare = CardRare.Rare;
                break;
            case "史诗":
                cardRare = CardRare.Epic;
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
                cardTarget = CardTarget.MyPlayer;
                break;
            case "全体敌人":
                cardTarget = CardTarget.AllEnemy;
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

            cardEffectDic.Add(effectID, new CardEffectData(effectID, effectValue));
            if (i > 1)
            {
                cardDes += "，";
            }

            string effectDes = cardEffectDic[effectID].EffectDes;

            cardDes += effectDes;
        }
    }

    public void UpdateEffectDes()
    {
        int count = 0;
        cardDes = "";
        foreach (var eff in cardEffectDic.Values)
        {
            count++;
            if (count > 1)
            {
                cardDes += "，";
            }

            string effectDes = eff.EffectDes;
            eff.actualValue = eff.EffectValue;
            effectDes = effectDes.Replace("value", eff.actualValue.ToString());
            Debug.Log(effectDes);
            cardDes += effectDes;
        }
    }

    public CardData()
    {
    }

    //克隆方法
    public CardData(CardData data)
    {
        hasModified = false;
        this.cardID = data.cardID;
        this.cardName = data.cardName;
        this.cardImgRes = data.cardImgRes;
        this.cardCost = data.cardCost;
        this.cardDes = data.cardDes;
        this.cardType = data.cardType;
        this.cardRare = data.cardRare;
        this.cardTarget = data.cardTarget;
        this.cardUseType = data.cardUseType;
        this.cardEffectDic = new Dictionary<int, CardEffectData>();
        originCost = data.originCost;

        foreach (var effectElement in data.cardEffectDic)
        {
            cardEffectDic.Add(effectElement.Key, effectElement.Value);
        }
    }

    private string ReadCfgCardData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicCardData);
        return data;
    }
}