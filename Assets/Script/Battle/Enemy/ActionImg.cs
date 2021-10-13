using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ActionImg : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    //������Enemy����
    private Enemy thisEnemy;
    //�¼�������
    private EventTrigger eventTrigger;
    private void Awake()
    {
        //��ȡ�����ĵ��˶��󣬲���ʼ������Щ���ֵ���ʾ
        thisEnemy=GetComponentInParent<Enemy>();
        
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        thisEnemy.ShowActionDes();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        thisEnemy.HideActionDes();
    }
}
