using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : BaseBattleUnit
{
    //敌人数据
    public EnemyData enemyData;
    //敌人图片
    protected Image img_EnemyOutLook;
    //敌人名称文本
    protected Text text_EnemyName;


    //初始化数据
    protected override void InitDataOnAwake(int id)
    {
        enemyData = new EnemyData(id);
        maxHp = enemyData.MAXHp;
        currentHp = maxHp;
    }

    protected override void InitUIOnAwake()
    {
        base.InitUIOnAwake();

        //获取敌人图片和名称文本并赋值
        img_EnemyOutLook = GameTool.GetTheChildComponent<Image>(gameObject, "Enemy_OutLook");
        img_EnemyOutLook.sprite = ResourcesManager.Instance.LoadResources<Sprite>(enemyData.ResourcePath);

        text_EnemyName = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Name");
        text_EnemyName.text = enemyData.EnemyName;

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
