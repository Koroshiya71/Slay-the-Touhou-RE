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
    protected CardData cardData=new CardData();

    //卡牌ID
    public int cardID=1001;
    //卡图
    protected Image img_Main;
    //卡牌外边框
    protected Image img_OutLine;
    
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
        img_Main.sprite = ResourcesManager.Instance.LoadResources<Sprite>(cardData.CardImgRes);
        img_OutLine = GameTool.GetTheChildComponent<Image>(gameObject, "Card_OutLine");
        img_OutLine.sprite = GetCardOutLine();
        text_CardEffect = GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardEffect");
        text_CardEffect.text = cardData.CardDes;
        cardAnimator = GetComponent<Animator>();

    }

    //获取卡牌外框
    protected virtual Sprite GetCardOutLine()
    {
        return null;
    }

    //初始化事件
    protected virtual void InitEvent()
    {
        eventListener=EventTriggerListener.Get(GameTool.FindTheChild(gameObject,"EventTrigger").gameObject);
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
