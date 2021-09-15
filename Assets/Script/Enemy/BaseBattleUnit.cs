using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBattleUnit : MonoBehaviour
{
    //�������ֵ�͵�ǰ����ֵ
    protected int maxHp;
    protected int currentHp;

    //ѡ����Ч
    protected GameObject selectEffect;
    //��ʼ�����󲢻��������
    protected virtual void InitDataOnAwake()
    {
        //��ȡѡ����Ч�����ڿ�ʼʱ����
        selectEffect = GameTool.FindTheChild(gameObject, "Img_Select").gameObject;
        selectEffect.SetActive(false);
    }

    protected virtual void Awake()
    {
        InitDataOnAwake();
    }

    //�ܵ��˺�
    public virtual void TakeDamage(int damage)
    {
        //TODO��ʵ���˺�����
        Debug.Log("�ܵ�"+damage+"���˺�");
    }

    //����������¼�
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        
    }
}
