using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseBattleUnit
{
    //敌人数据
    public EnemyData enemyData;

    //初始化敌人
    protected override void InitUnitData(int ID = 0)
    {
        enemyData = new EnemyData(ID);
        maxHp = enemyData.MAXHp;

    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        //卡牌选中时启用选择特效
        if (other.CompareTag("HandCard")&&HandCardManager.Instance.selectedCard.mCardData.CardTarget==CardTarget.SingleEnemy)
        {
            BattleManager.Instance.selectedTarget = this;
            selectEffect.SetActive(true);
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        //卡牌离开时禁用选择特效

        if (other.CompareTag("HandCard"))
        {
            BattleManager.Instance.selectedTarget = null;
            selectEffect.SetActive(false);
        }
    }
}
