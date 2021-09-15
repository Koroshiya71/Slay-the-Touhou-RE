using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBattleUnit : MonoBehaviour
{
    //����ֵ���
    protected int maxHp=50;
    protected int currentHp=50;

    private Slider slider_Hp;
    private Text text_Hp;
    //ѡ����Ч
    protected GameObject selectEffect;
    //��ʼ�����󲢻��������
    protected virtual void InitDataOnAwake()
    {
        InitUnitData();
        //��ȡѡ����Ч�����ڿ�ʼʱ����
        selectEffect = GameTool.FindTheChild(gameObject, "Img_Select").gameObject;
        selectEffect.SetActive(false);

        //��ʼ��Ѫ����Ѫ��UI
        slider_Hp = GameTool.GetTheChildComponent<Slider>(gameObject, "Slider_Hp");
        text_Hp = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Hp");
        currentHp = maxHp;
        text_Hp.text = currentHp + " / " + maxHp;
    }
    //��ʼ����ս����λ������
    protected virtual void InitUnitData(int ID=0)
    {
        
    }
    protected virtual void Awake()
    {
        InitDataOnAwake();
    }

    //�ܵ��˺�
    public virtual void TakeDamage(int damage)
    {
        currentHp -= damage;
        slider_Hp.value = (float) currentHp / maxHp;
        text_Hp.text = currentHp + " / " + maxHp;

    }

    //����������¼�
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        
    }
}
