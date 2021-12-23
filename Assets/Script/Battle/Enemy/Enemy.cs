using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using UnityEngine;
using UnityEngine.SearchService;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : BaseBattleUnit
{
    /// <summary>
    /// 基本数据和UI
    /// </summary>
    //敌人数据
    public EnemyData enemyData;

    //敌人图片
    protected Image img_EnemyOutLook;

    //敌人名称文本
    protected Text text_EnemyName;

    /// <summary>
    /// 敌人行动相关
    /// </summary>
    //敌人意图Image
    protected Image img_EnemyAction;

    //敌人意图描述文本
    protected Text text_ActionDes;

    //敌人行动数值文本
    protected Text text_ActionValue;

    //当前即将执行的行动
    protected ActionData currentAction;

    //启用的行为模式
    protected string activeActionMode;

    //当前正在执行行为列表中的第几个行为
    protected int currentActionNo;

    #region 初始化

    //初始化数据
    protected override void InitDataOnAwake(int id)
    {
        enemyData = new EnemyData(id);
        maxHp = enemyData.MAXHp;
        currentHp = maxHp;

        //从行动模式列表中随机选择一个行动模式
        activeActionMode = enemyData.ActionModeList[Random.Range(0, enemyData.ActionModeList.Count)];
        currentActionNo = 0;
        //获取行动列表中的第一个行为
        currentAction = enemyData.EnemyActionDic
            .ElementAt(Convert.ToInt32(activeActionMode[currentActionNo].ToString()) - 1).Value;
        currentActionNo++;
    }

    //初始化界面
    protected override void InitUIOnAwake()
    {
        base.InitUIOnAwake();

        //获取敌人图片和名称文本并赋值
        img_EnemyOutLook = GameTool.GetTheChildComponent<Image>(gameObject, "Enemy_OutLook");
        img_EnemyOutLook.sprite = ResourcesManager.Instance.LoadResources<Sprite>(enemyData.ResourcePath);

        text_EnemyName = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Name");
        text_EnemyName.text = enemyData.EnemyName;

        //获取敌人行动提示图片和文本
        img_EnemyAction = GameTool.GetTheChildComponent<Image>(gameObject, "Img_Action");

        text_ActionDes = GameTool.GetTheChildComponent<Text>(gameObject, "Text_ActionDes");
        text_ActionValue = GameTool.GetTheChildComponent<Text>(gameObject, "Text_ActionValue");
        text_ActionDes.enabled = false;
        text_ActionValue.enabled = false;
        switch (currentAction.ActionType)
        {
            case ActionType.Attack:
                img_EnemyAction.sprite =
                    ResourcesManager.Instance.LoadResources<Sprite>("Image/" + "UIImage/" + "EnemyAction/" + "Attack");
                text_ActionValue.text = currentAction.ActionValue.ToString();
                text_ActionValue.enabled = true;
                break;
            case ActionType.Buff:
                img_EnemyAction.sprite =
                        ResourcesManager.Instance.LoadResources<Sprite>("Image/" + "UIImage/" + "EnemyAction/" + "Buff");
                text_ActionValue.enabled = false;
                break;
        }

        text_ActionDes.text = currentAction.ActionDes.Replace("value", currentAction.ActionValue.ToString());
        text_ActionDes.enabled = false;
    }

    #endregion


    #region 触发器事件

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        //卡牌选中时启用选择特效
        if (other.CompareTag("HandCard") &&
            HandCardManager.Instance.selectedCard.mCardData.CardTarget == CardTarget.SingleEnemy)
        {
            BattleManager.Instance.selectedTarget = this;
        }
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        //卡牌选中时启用选择特效
        if (other.CompareTag("HandCard") &&
            HandCardManager.Instance.selectedCard.mCardData.CardTarget == CardTarget.SingleEnemy)
        {
            BattleManager.Instance.selectedTarget = this;
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        //卡牌离开时禁用选择特效

        if (other.CompareTag("HandCard"))
        {
            BattleManager.Instance.selectedTarget = null;
        }
    }

    #endregion


    #region UI管理

    protected void UpdateUIState()
    {
        if (BattleManager.Instance.selectedTarget != this)
        {
            selectEffect.SetActive(false);
        }
        else
        {
            selectEffect.SetActive(true);
        }
    }

    //显示行动描述
    public void ShowActionDes()
    {
        text_ActionDes.text = currentAction.ActionDes.Replace("value", currentAction.ActionValue.ToString());
        text_ActionDes.enabled = true;
    }

    //隐藏行动描述
    public void HideActionDes()
    {
        text_ActionDes.enabled = false;
    }

    #endregion


    #region 行动和状态管理相关
    //执行行动
    public void TakeAction()
    {
        BattleManager.Instance.TriggerActionEffect(currentAction);
    }
    //更新行动
    public void UpdateAction()
    {

    }
    //死亡方法
    public override void Die()
    {
        //移除队列并销毁
        BattleManager.Instance.inBattleEnemyList.Remove(this);
        Destroy(this.gameObject);

        //如果该敌人死亡后敌人数量为0，触发战斗结束方法
        if (BattleManager.Instance.inBattleEnemyList.Count == 0)
        {
            BattleManager.Instance.BattleEnd();
        }
    }

    #endregion

    protected void Update()
    {
        UpdateUIState();
    }
}