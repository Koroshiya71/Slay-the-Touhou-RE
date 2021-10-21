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
    public CardData mCardData => cardData;
    //����ID
    public int cardID=1001;
    //��ͼ
    protected Image img_Main;
    //��������
    protected Text text_CardName;
    //������߿�
    protected Image img_OutLine;
    
    //����Ч�������ı�
    protected Text text_CardEffect;
    //�¼�������
    protected EventTriggerListener eventListener;
    //���ƶ���������
    protected Animator cardAnimator;

    //���ڿ����¼�������CanvasGroup
    protected CanvasGroup canvasGroup;

    //��ʼ������
    public virtual void InitCard(int cardID)
    {
        cardData = new CardData(cardID);
        this.cardID = cardID;
        //��ȡ����UI���ű������
        canvasGroup = GetComponent<CanvasGroup>();
        img_Main = GameTool.GetTheChildComponent<Image>(gameObject, "Card_Main");

        //��������»��ߣ���˵��Sprite����ͼ������
        if (cardData.CardImgRes.Contains("_"))
        {
            string path = cardData.CardImgRes.Split('_')[0];
            var sprites = Resources.LoadAll<Sprite>(path);
            var index = int.Parse(cardData.CardImgRes.Split('_')[1]);
            img_Main.sprite = sprites[index];
        }
        else
        {
            img_Main.sprite = ResourcesManager.Instance.LoadResources<Sprite>(cardData.CardImgRes);

        }
        img_OutLine = GameTool.GetTheChildComponent<Image>(gameObject, "Card_OutLine");
        img_OutLine.sprite = GetCardOutLine();
        text_CardEffect = GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardEffect");
        text_CardEffect.text = cardData.CardDes;
        cardAnimator = GetComponent<Animator>();
        text_CardName = GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardName");
        text_CardName.text = cardData.CardName;

        if (cardData.CardType!=CardType.SpellCard)
        {
            if (cardID>=1000&&cardID<2000)
            {
                switch (cardData.CardType)
                {
                    case CardType.TiShu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "����~" +"����" ;
                        break;
                    case CardType.JiNeng:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "����~" + "����";
                        break;
                    case CardType.FaShu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "����~" + "����";
                        break;
                    case CardType.FangYu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "����~" + "����";
                        break;
                    case CardType.DanMu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "����~" + "��Ļ";
                        break;
                }
                
            }
        }
    }

    //��ʼ������
    public virtual void InitCard(CardData data)
    {
        cardData = new CardData(data);

        this.cardID = data.CardID;
        //��ȡ����UI���ű������
        canvasGroup = GetComponent<CanvasGroup>();
        img_Main = GameTool.GetTheChildComponent<Image>(gameObject, "Card_Main");
        img_Main.sprite = ResourcesManager.Instance.LoadResources<Sprite>(cardData.CardImgRes);
        img_OutLine = GameTool.GetTheChildComponent<Image>(gameObject, "Card_OutLine");
        img_OutLine.sprite = GetCardOutLine();
        text_CardEffect = GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardEffect");
        text_CardEffect.text = cardData.CardDes;
        cardAnimator = GetComponent<Animator>();
        text_CardName = GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardName");
        text_CardName.text = cardData.CardName;

        if (cardData.CardType != CardType.SpellCard)
        {
            if (cardID >= 1000 && cardID < 2000)
            {
                switch (cardData.CardType)
                {
                    case CardType.TiShu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "����~" + "����";
                        break;
                    case CardType.JiNeng:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "����~" + "����";
                        break;
                    case CardType.FaShu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "����~" + "����";
                        break;
                    case CardType.FangYu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "����~" + "����";
                        break;
                    case CardType.DanMu:
                        GameTool.GetTheChildComponent<Text>(gameObject, "Text_CardType").
                            text = "����~" + "��Ļ";
                        break;
                }

            }
        }
    }
    //��ȡ�������
    protected virtual Sprite GetCardOutLine()
    {
        return null;
    }

    //��ʼ���¼�
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

    //�����ѡ�п���ʱ�Ļص�
    protected virtual void OnEnter()
    {
        
    }
    //������뿪����ʱ�Ļص�
    protected virtual void OnExit()
    {
        
    }
    //�����̧��ʱ�Ļص�
    protected virtual void OnUp()
    {
       
    }
    //�������ʱ�Ļص�
    protected virtual void OnDown()
    {

    }
    //���������ʱ�Ļص�
    protected virtual void OnSelect()
    {

    }
}
