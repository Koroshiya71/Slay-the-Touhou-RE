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

    //敌人状态字典
    public Dictionary<int, StateData> enemyStateDic = new Dictionary<int, StateData>();

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
    public ActionData currentAction;

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
        isPlayer = false;
        //从行动模式列表中随机选择一个行动模式
        activeActionMode = enemyData.ActionModeList[Random.Range(0, enemyData.ActionModeList.Count)];
        currentActionNo = 0;
        

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
        
        //更新当前行动
        currentAction = new ActionData(1004, 0);
        UpdateCurrentAction();
        text_ActionDes.text = currentAction.ActionDes.Replace("value", currentAction.ActionValue.ToString());
        text_ActionDes.enabled = false;
    }

    #endregion


    #region 触发器事件

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        //卡牌选中时启用选择特效
        if (other.CompareTag("HandCard") &&
            HandCardManager.Instance.selectedCard.CardData.cardTarget == CardTarget.SingleEnemy)
        {
            BattleManager.Instance.selectedTarget = this;
        }
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        if (HandCardManager.Instance.selectedCard == null)
        {
            return;
        }

        //卡牌选中时启用选择特效
        if (other.CompareTag("HandCard") &&
            HandCardManager.Instance.selectedCard.CardData.cardTarget == CardTarget.SingleEnemy)
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
        BattleManager.Instance.TriggerActionEffect(this, currentAction);
    }

    //更新行动
    public void UpdateCurrentAction()
    {
        //更新前还原数值
        currentAction.actualValue = currentAction.ActionValue;
        if (!CheckSpecialAction())
        {
            //获取行动列表中的第一个行为
            currentActionNo %= activeActionMode.Length;
            currentAction = enemyData.EnemyActionDic
                .ElementAt(Convert.ToInt32(activeActionMode[currentActionNo].ToString()) - 1).Value;

            currentActionNo++;
        }


        //更新UI
        text_ActionDes.enabled = false;
        text_ActionValue.enabled = false;
        switch (currentAction.ActionType)
        {
            case ActionType.Attack:
                img_EnemyAction.sprite =
                    ResourcesManager.Instance.LoadResources<Sprite>("Image/" + "UIImage/" + "EnemyAction/" + "Attack");
                text_ActionValue.text = currentAction.actualValue.ToString();
                text_ActionValue.enabled = true;
                break;
            case ActionType.Buff:
                img_EnemyAction.sprite =
                    ResourcesManager.Instance.LoadResources<Sprite>("Image/" + "UIImage/" + "EnemyAction/" + "Buff");
                text_ActionValue.enabled = false;
                break;
            case ActionType.Weaken:
                img_EnemyAction.sprite =
                    ResourcesManager.Instance.LoadResources<Sprite>("Image/" + "UIImage/" + "EnemyAction/" + "Weaken");
                text_ActionValue.enabled = false;
                break;
            case ActionType.Special:
                img_EnemyAction.sprite =
                    ResourcesManager.Instance.LoadResources<Sprite>("Image/" + "UIImage/" + "EnemyAction/" + "Special");
                text_ActionValue.enabled = false;
                break;
            case ActionType.Shield:
                img_EnemyAction.sprite =
                    ResourcesManager.Instance.LoadResources<Sprite>("Image/" + "UIImage/" + "EnemyAction/" + "Shield");
                text_ActionValue.enabled = true;
                text_ActionValue.text = currentAction.actualValue.ToString();
                break;
        }

        text_ActionDes.text = currentAction.ActionDes.Replace("value", currentAction.actualValue.ToString());
        text_ActionDes.enabled = false;
    }

    //死亡方法
    public override void Die()
    {
        //移除队列并销毁
        BattleManager.Instance.inBattleEnemyList.Remove(this);
        Destroy(this.gameObject);

        //累计战斗金币
        BattleManager.Instance.battleGold += enemyData.dropGold;
        //如果该敌人死亡后敌人数量为0，触发战斗结束方法
        if (BattleManager.Instance.inBattleEnemyList.Count == 0)
        {
            BattleManager.Instance.BattleEnd();
        }
        
    }
    
    //更新UI
    public override void UpdateUI()
    {
        base.UpdateUI();
        //更新Action UI
        text_ActionDes.enabled = false;
        text_ActionValue.enabled = false;
        switch (currentAction.ActionType)
        {
            case ActionType.Attack:
                img_EnemyAction.sprite =
                    ResourcesManager.Instance.LoadResources<Sprite>("Image/" + "UIImage/" + "EnemyAction/" + "Attack");
                text_ActionValue.text = currentAction.actualValue.ToString();
                text_ActionValue.enabled = true;
                break;
            case ActionType.Buff:
                img_EnemyAction.sprite =
                    ResourcesManager.Instance.LoadResources<Sprite>("Image/" + "UIImage/" + "EnemyAction/" + "Buff");
                text_ActionValue.enabled = false;
                break;
        }

        text_ActionDes.text = currentAction.ActionDes.Replace("value", currentAction.actualValue.ToString());
        text_ActionDes.enabled = false;
    }

    #endregion

    //处理特殊的行动,如果跳过了正常行动则return true
    public bool CheckSpecialAction()
    {
        //懒惰妖精血量大于70%不进行攻击
        if (enemyData.EnemyID == 3)
        {
            if (currentHp > (int) (0.7f * maxHp))
            {
                currentAction = new ActionData(1004,0);
                return true; 
            }
        }
        return false;
    }

    protected void Update()
    {
        UpdateUIState();
    }
}