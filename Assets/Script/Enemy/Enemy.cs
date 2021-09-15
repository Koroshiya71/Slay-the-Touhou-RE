using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseBattleUnit
{
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
