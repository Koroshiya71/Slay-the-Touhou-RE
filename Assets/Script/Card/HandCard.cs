using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;

public class HandCard : BaseCard
{

    private List<GameObject> handCardList;
    public override void InitCard(int cardID)
    {
        base.InitCard(cardID);
        handCardList = HandCardManager.Instance.handCardList;
        
        
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
    }
    //当鼠标选中卡牌时的回调
    private void OnSelect()
    {
        cardAnimator.SetBool("Hover",true);
        for (int i = handCardList.IndexOf(this.gameObject) + 1; i < handCardList.Count; i++)
        {
            handCardList[i].GetComponent<HandCard>().cardAnimator.SetBool("Daging", true);
        }
    }
    //当鼠标离开卡牌时的回调
    private void OnExit()
    {
        cardAnimator.SetBool("Hover", false);
        for (int i = handCardList.IndexOf(this.gameObject) + 1; i < handCardList.Count; i++)
        {
            handCardList[i].GetComponent<HandCard>().cardAnimator.SetBool("Daging", false);
        }
    }

}
