using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��������
public enum CardType
{
    //����
    TiShu,
    //��Ļ
    DanMu,
    //����
    FaShu,
    //����
    FangYu,
    //����
    JiNeng,
    //����
    SpellCard
}
//����ϡ�ж�
public enum CardRare
{
    //��ͨ
    Normal,
    //ϡ��
    Rare,
    //ʷʫ
    Epic
}

//����ʹ�÷�ʽ
public enum CardTarget
{
    //�������
    SingleEnemy,
    //���
    Player,
    //ȫ�����
    AllEnemy,
    //
}

//����������
public class CardData
{
    //����ID
    private int cardID;
    //��������
    private string cardName;
    //���Ʒ���
    private int cardCost;
    //��������
    private string cardDes;
    //��������
    private CardType cardType;
    //����ϡ�ж�
    private CardRare cardRare;
}
