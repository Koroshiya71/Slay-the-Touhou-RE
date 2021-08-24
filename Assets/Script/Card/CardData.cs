using System;
using System.Collections;
using System.Collections.Generic;
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
    //单体敌人
    SingleEnemy,
    //玩家
    Player,
    //全体敌人
    AllEnemy,
    //
}

//卡牌数据类
public class CardData
{
    //卡牌ID
    private int cardID;
    //卡牌名称
    private string cardName;
    //卡牌费用
    private int cardCost;
    //卡牌描述
    private string cardDes;
    //卡牌类型
    private CardType cardType;
    //卡牌稀有度
    private CardRare cardRare;
}
