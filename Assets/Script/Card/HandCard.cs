using System;
using System.Collections;
using System.Collections.Generic;
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
        eventListener.onDown += delegate
        {
            OnDown();
        };
        eventListener.onUp += delegate
        {
            OnUp();
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
    //�����̧��ʱ�Ļص�
    private void OnUp()
    {
        if (isDragging)
        {
            isDragging = false;
            transform.position = originPos;
            Debug.Log(originPos);

        }
    }
    //�������ʱ�Ļص�
    private void OnDown()
    {
        isDragging = true;

    }
    //��¼��ʼλ��
    public void SaveOriginalPos()
    {
        originPos = transform.position;
        Debug.Log(originPos);
    }
    private void Update()
    {
        if (isDragging)
        {
            transform.position = Input.mousePosition;

        }
    }
}
