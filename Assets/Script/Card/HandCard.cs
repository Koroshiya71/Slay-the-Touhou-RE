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
    //ʹ�ÿ���ʱ����Ч
    private GameObject useEffect;
    //���ƹ������������б�
    private List<GameObject> handCardList;
    //�Ƿ����ڱ���ק
    private bool isDragging;
    public bool IsDragging => isDragging;
    //��ʼ����λ��
    private Vector3 originPos;
    //��ʼ�����ƣ��̳л��������൫���һЩ����ĳ�ʼ������
    public override void InitCard(int cardID)
    {
        base.InitCard(cardID);
        //��ȡ�����б�
        handCardList = HandCardManager.Instance.handCardList;
        //��ȡ��Ч��Ϸ����
        useEffect = GameTool.FindTheChild(gameObject, "UseEffect").gameObject;
        useEffect.SetActive(false);
        isDragging = false;
    }

    //�Ƿ�����ѡ��
    private bool isSelecting;
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
        
        
    }
    //�������뿨��ʱ�Ļص�
    protected override void OnEnter()
    {
        cardAnimator.SetBool("Hover",true);
        HandCardManager.Instance.selectedCard = this;
        isSelecting = true;
        for (int i = handCardList.IndexOf(this.gameObject) + 1; i < handCardList.Count; i++)
        {
            handCardList[i].GetComponent<HandCard>().cardAnimator.SetBool("Daging", true);
        }
    }
   
    //������뿪����ʱ�Ļص�
    protected override void OnExit()
    {
        cardAnimator.SetBool("Hover", false);
        isSelecting = false;
        HandCardManager.Instance.selectedCard = null;

        for (int i = handCardList.IndexOf(this.gameObject) + 1; i < handCardList.Count; i++)
        {
            handCardList[i].GetComponent<HandCard>().cardAnimator.SetBool("Daging", false);
        }
    }
    //�����̧��ʱ�Ļص�
    protected override void OnUp()
    {
        if (isDragging)
        {
            isDragging = false;
            HandCardManager.Instance.selectedCard = null;
            isSelecting = false;
            if (CheckUsable())
            {
                UseCard();
                return;
            }
            transform.position = originPos;
            
        }
    }
    //�������ʱ�Ļص�
    protected override void OnDown()
    {
        isDragging = true;
        isSelecting = true;
        HandCardManager.Instance.selectedCard = this;
    }
    //��¼��ʼλ��
    public void SaveOriginalPos()
    {
        originPos = transform.position;
    }

    //����Ƿ����ʹ��
    private bool CheckUsable()
    {
        //TODO��������ò������޷�ʹ��
        //�������λ���������������ʹ��
        if (transform.position.y>=140)
        {
            useEffect.SetActive(true);
            return true;
        }
        useEffect.SetActive(false);

        return false;
    }

    //ʹ�ÿ���
    public void UseCard()
    {
        //TODO������Ч��
        for (int i = handCardList.IndexOf(this.gameObject) + 1; i < handCardList.Count; i++)
        {
            handCardList[i].GetComponent<HandCard>().cardAnimator.SetBool("Daging", false);
        }
        HandCardManager.Instance.RemoveCard(this.gameObject);
        HandCardManager.Instance.selectedCard = null;

    }

    //ȡ��UI�¼����
    public void CancelUIEventListen()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    //����UI�¼����
    public void ActiveUIEventListen()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    //����Ƿ񼤻��¼����
    public void CheckEventActive()
    {
        if (HandCardManager.Instance.selectedCard==null)
        {
            ActiveUIEventListen();
            return;
        }

        if (!isSelecting)
        {
            CancelUIEventListen();
        }
        else
        {
            ActiveUIEventListen();
        }
    }
    private void Update()
    {
        CheckUsable();
        CheckEventActive();
        if (isDragging)
        {
            transform.position = Input.mousePosition;
            
        }
    }
}
