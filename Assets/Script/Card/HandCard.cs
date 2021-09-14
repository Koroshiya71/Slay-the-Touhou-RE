using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using GameCore;
using Unity.VisualScripting;

public class HandCard : BaseCard
{
    //使用卡牌时的特效
    private GameObject useEffect;
    //手牌管理器的手牌列表
    private List<GameObject> handCardList;
    //是否正在被拖拽
    private bool isDragging;
    public bool IsDragging => isDragging;
    //初始卡牌位置
    private Vector3 originPos;
    //初始化卡牌，继承基本卡牌类但添加一些额外的初始化内容
    public override void InitCard(int cardID)
    {
        base.InitCard(cardID);
        //获取手牌列表
        handCardList = HandCardManager.Instance.handCardList;
        //获取特效游戏物体
        useEffect = GameTool.FindTheChild(gameObject, "UseEffect").gameObject;
        useEffect.SetActive(false);
        isDragging = false;
    }
    //获取卡牌外框
    protected override Sprite GetCardOutLine()
    {
        string outLineName = "";
        switch (cardData.CardType)
        {
            case CardType.TiShu:
                outLineName += "体术";
                break;
            default:
                outLineName += "体术";
                break;
        }

        switch (cardData.CardRare)
        {
            case CardRare.Normal:
                outLineName += "普通";
                break;
            default:
                outLineName += "普通";
                break;
        }

        Sprite outLine = ResourcesManager.Instance.LoadResources<Sprite>("Image/Card/CardOutLine/" + outLineName);
        return outLine;
    }
    //初始化卡牌事件
    protected override void InitEvent()
    {
        base.InitEvent();
        eventListener.onEnter += delegate
        {
            OnSelect();
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
    }
    //当鼠标选中卡牌时的回调
    protected override void OnSelect()
    {
        cardAnimator.SetBool("Hover",true);
        for (int i = handCardList.IndexOf(this.gameObject) + 1; i < handCardList.Count; i++)
        {
            handCardList[i].GetComponent<HandCard>().cardAnimator.SetBool("Daging", true);
        }
    }
    //当鼠标离开卡牌时的回调
    protected override void OnExit()
    {
        cardAnimator.SetBool("Hover", false);
        for (int i = handCardList.IndexOf(this.gameObject) + 1; i < handCardList.Count; i++)
        {
            handCardList[i].GetComponent<HandCard>().cardAnimator.SetBool("Daging", false);
        }
    }
    //当鼠标抬起时的回调
    protected override void OnUp()
    {
        if (isDragging)
        {
            isDragging = false;
            if (CheckUsable())
            {
                UseCard();
                return;
            }
            transform.position = originPos;
            
        }
    }
    //当鼠标点击时的回调
    protected override void OnDown()
    {
        isDragging = true;

    }
    //记录初始位置
    public void SaveOriginalPos()
    {
        originPos = transform.position;
    }

    //检查是否可以使用
    private bool CheckUsable()
    {
        //TODO：如果费用不够则无法使用
        //如果卡牌位置满足条件则可以使用
        if (transform.position.y>=140)
        {
            useEffect.SetActive(true);
            return true;
        }
        useEffect.SetActive(false);

        return false;
    }

    //使用卡牌
    public void UseCard()
    {
        //TODO：检查使用条件并触发效果
        HandCardManager.Instance.RemoveCard(this.gameObject);

    }


    private void Update()
    {
        CheckUsable();
        if (isDragging)
        {
            transform.position = Input.mousePosition;
        }
    }
}
