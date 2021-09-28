using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : BaseBattleUnit
{
    //��������
    public EnemyData enemyData;
    //����ͼƬ
    protected Image img_EnemyOutLook;
    //���������ı�
    protected Text text_EnemyName;


    //��ʼ������
    protected override void InitDataOnAwake(int id)
    {
        enemyData = new EnemyData(id);
        maxHp = enemyData.MAXHp;
        currentHp = maxHp;
    }

    protected override void InitUIOnAwake()
    {
        base.InitUIOnAwake();

        //��ȡ����ͼƬ�������ı�����ֵ
        img_EnemyOutLook = GameTool.GetTheChildComponent<Image>(gameObject, "Enemy_OutLook");
        img_EnemyOutLook.sprite = ResourcesManager.Instance.LoadResources<Sprite>(enemyData.ResourcePath);

        text_EnemyName = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Name");
        text_EnemyName.text = enemyData.EnemyName;

    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        //����ѡ��ʱ����ѡ����Ч
        if (other.CompareTag("HandCard")&&HandCardManager.Instance.selectedCard.mCardData.CardTarget==CardTarget.SingleEnemy)
        {
            BattleManager.Instance.selectedTarget = this;
            selectEffect.SetActive(true);
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        //�����뿪ʱ����ѡ����Ч

        if (other.CompareTag("HandCard"))
        {
            BattleManager.Instance.selectedTarget = null;
            selectEffect.SetActive(false);
        }
    }
}
