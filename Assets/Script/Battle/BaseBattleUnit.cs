using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBattleUnit : MonoBehaviour
{
    //����ֵ���
    protected int maxHp = 50;
    protected int currentHp = 50;
    protected Slider slider_Hp;
    protected Text text_Hp;

    //����ֵ���
    protected int currentShield = 0;
    protected Image img_Shield;
    protected Text text_Shield;
    
    //ѡ����Ч
    protected GameObject selectEffect;

    //��ʼ��UI�����ݶ���
    public virtual void Init(int id = 1)
    {
        InitDataOnAwake(id);
        InitUIOnAwake();
    }

    protected virtual void Awake()
    {
    }

    //�ܵ��˺�
    public virtual void TakeDamage(int damage)
    {
        //����л��ܣ����ȿ۳�����
        if (currentShield >= damage)
        {
            //����ֵ���ڵ����˺�ֵʱ���������ֵ�˺�
            currentShield -= damage;
            return;
        }

        //����ֵС���˺�ֵʱ����ʹ�û��ܵ��������˺�
        damage -= currentShield;

        currentHp -= damage;
        
        UpdateUI();
        if (currentHp<=0)
        {
            Die();
        }
    }
    //����UI
    public virtual void UpdateUI()
    {
        //����Ѫ��UI
        slider_Hp.value = (float)currentHp / maxHp;
        text_Hp.text = currentHp + " / " + maxHp;
        //�����ʼ����ֵΪ0��ȡ�����UI����ʾ
        if (currentShield == 0)
        {
            img_Shield.enabled = false;
            text_Shield.enabled = false;
        }
        //�������ʾ��Ӧ�Ļ���ֵ
        else
        {
            img_Shield.enabled = true;
            text_Shield.enabled = true;
            text_Shield.text = currentShield.ToString();
        }
    }
    //��û���
    public virtual void GetShield(int shield)
    {
        currentShield += shield;
        UpdateUI();
    }
    //����ֵ����ʱ����������
    public virtual void Die()
    {

    }
    //��ʼ��UI
    protected virtual void InitUIOnAwake()
    {
        //��ȡѡ����Ч�����ڿ�ʼʱ����
        selectEffect = GameTool.FindTheChild(gameObject, "Img_Select").gameObject;
        selectEffect.SetActive(false);

        //��ʼ��Ѫ����Ѫ��UI
        slider_Hp = GameTool.GetTheChildComponent<Slider>(gameObject, "Slider_Hp");
        text_Hp = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Hp");
        currentHp = maxHp;
        text_Hp.text = currentHp + " / " + maxHp;

        //��ʼ������ֵ�ͻ���UI
        img_Shield = GameTool.GetTheChildComponent<Image>(gameObject, "Img_Shield");

        text_Shield = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Shield");
        //�����ʼ����ֵΪ0��ȡ�����UI����ʾ
        if (currentShield==0)
        {
            img_Shield.enabled = false;
            text_Shield.enabled = false;
        }
        //�������ʾ��Ӧ�Ļ���ֵ
        else
        {
            img_Shield.enabled = true;
            text_Shield.text = currentShield.ToString();
        }
    }

    //��ʼ������
    protected virtual void InitDataOnAwake(int id)
    {
    }

    //����������¼�
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
    }
}