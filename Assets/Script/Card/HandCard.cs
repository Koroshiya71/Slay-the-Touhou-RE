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
    //��ȡ�������
    protected override Sprite GetCardOutLine()
    {
        string outLineName = "";
        switch (cardData.CardType)
        {
            case CardType.TiShu:
                outLineName += "����";
                break;
            default:
                outLineName += "����";
                break;
        }

        switch (cardData.CardRare)
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
    //��ʼ�������¼�
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
    //�����ѡ�п���ʱ�Ļص�
    private void OnSelect()
    {
        cardAnimator.SetBool("Hover",true);
        for (int i = handCardList.IndexOf(this.gameObject) + 1; i < handCardList.Count; i++)
        {
            handCardList[i].GetComponent<HandCard>().cardAnimator.SetBool("Daging", true);
        }
    }
    //������뿪����ʱ�Ļص�
    private void OnExit()
    {
        cardAnimator.SetBool("Hover", false);
        for (int i = handCardList.IndexOf(this.gameObject) + 1; i < handCardList.Count; i++)
        {
            handCardList[i].GetComponent<HandCard>().cardAnimator.SetBool("Daging", false);
        }
    }

}
