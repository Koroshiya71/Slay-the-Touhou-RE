using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseCard : MonoBehaviour
{
    //卡牌数据
    protected CardData cardData = new CardData();
    public CardData mCardData => cardData;
    //卡牌ID
    public int cardID = 1001;
    //卡图
    protected Image img_Main;
    //卡牌名称
    protected Text text_CardName;
    //卡牌外边框
    protected Image img_OutLine;
    //卡牌消耗文本
    protected Text text_cardCost;
    //卡牌效果描述文本
    protected Text text_CardEffect;
    //事件触发器
    protected EventTriggerListener eventListener;
    //卡牌动画控制器
    protected Animator cardAnimator;

    //用于控制事件交互的CanvasGroup
    protected CanvasGroup canvasGroup;

    //初始化卡牌
    public virtual void InitCard(int cardID)
    {
        cardData = new CardData(cardID);
        this.cardID = cardID;
        //获取各种UI、脚本、组件
        canvasGroup = GetComponent<CanvasGroup>();
        img_Main = GameTool.GetTheChildComponent<Image>(gameObject, "Card_Main");

        //如果包含下划线，则说明Sprite来自图标组组
        if (cardData.cardImgRes.Contains("_"))
        {
            string path = cardData.cardImgRes.Split('_')[0];
            var sprites = Resources.LoadAll<Sprite>(path);
            var index = int.Parse(cardData.cardImgRes.Split('_')[1]);
            img_Main.sprite = sprites[index];
        }
        else
        {
            img_Main.sprite = ResourcesManager.Instance.LoadResources<Sprite>(cardData.cardImgRes);

        }
        img_OutLine = GameTool.GetTheChildComponent<Image>(gameObject, "Card_OutLine");
        img_OutLine.sprite = GetCardOutLine();
        text_CardEffect = GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardEffect");
        text_CardEffect.text = cardData.cardDes;
        cardAnimator = GetComponent<Animator>();
        text_CardName = GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardName");
        text_CardName.text = cardData.cardName;
        text_cardCost = GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardCost");
        text_cardCost.text=cardData.cardCost.ToString();
        if (cardData.cardType != CardType.SpellCard)
        {
            if (cardID >= 1000 && cardID < 2000)
            {
                switch (cardData.cardType)
                {
                    case CardType.TiShu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "妖梦~" + "体术";
                        break;
                    case CardType.JiNeng:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "妖梦~" + "技能";
                        break;
                    case CardType.FaShu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "妖梦~" + "法术";
                        break;
                    case CardType.FangYu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "妖梦~" + "防御";
                        break;
                    case CardType.DanMu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "妖梦~" + "弹幕";
                        break;
                }

            }
        }
    }

    //初始化卡牌
    public virtual void InitCard(CardData data)
    {
        cardData = new CardData(data);

        this.cardID = data.cardID;
        //获取各种UI、脚本、组件
        canvasGroup = GetComponent<CanvasGroup>();
        img_Main = GameTool.GetTheChildComponent<Image>(gameObject, "Card_Main");
        img_Main.sprite = ResourcesManager.Instance.LoadResources<Sprite>(cardData.cardImgRes);
        img_OutLine = GameTool.GetTheChildComponent<Image>(gameObject, "Card_OutLine");
        img_OutLine.sprite = GetCardOutLine();
        text_CardEffect = GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardEffect");
        text_CardEffect.text = cardData.cardDes;
        cardAnimator = GetComponent<Animator>();
        text_CardName = GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardName");
        text_CardName.text = cardData.cardName;

        if (cardData.cardType != CardType.SpellCard)
        {
            if (cardID >= 1000 && cardID < 2000)
            {
                switch (cardData.cardType)
                {
                    case CardType.TiShu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "妖梦~" + "体术";
                        break;
                    case CardType.JiNeng:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "妖梦~" + "技能";
                        break;
                    case CardType.FaShu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "妖梦~" + "法术";
                        break;
                    case CardType.FangYu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "妖梦~" + "防御";
                        break;
                    case CardType.DanMu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "妖梦~" + "弹幕";
                        break;
                }

            }
        }
    }
    //获取卡牌外框
    protected virtual Sprite GetCardOutLine()
    {
        return null;
    }

    //初始化事件
    protected virtual void InitEvent()
    {
        eventListener = EventTriggerListener.Get(GameTool.FindTheChild(gameObject, "EventTrigger").gameObject);
        eventListener.onEnter += delegate
        {
            OnEnter();
        };
        eventListener.onExit += delegate
        {
            OnExit();
        };
        eventListener.onDown += delegate
        {
            OnDown();
        };
        eventListener.onUp += delegate
        {
            OnUp();
        };
        eventListener.onUpdateSelect += delegate
        {
            OnSelect();
        };
    }
    protected virtual void Awake()
    {
        InitEvent();
    }

    //当鼠标选中卡牌时的回调
    protected virtual void OnEnter()
    {

    }
    //当鼠标离开卡牌时的回调
    protected virtual void OnExit()
    {

    }
    //当鼠标抬起时的回调
    protected virtual void OnUp()
    {

    }
    //当鼠标点击时的回调
    protected virtual void OnDown()
    {

    }
    //当鼠标悬浮时的回调
    protected virtual void OnSelect()
    {

    }
}
