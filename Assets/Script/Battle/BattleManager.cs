using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleManager : UnitySingleton<BattleManager>
{
    //当前能量值
    private int currentEnergy = 3;
    public int CurrentEnergy => currentEnergy;

    //能量上限
    private int maxEnergy = 3;
    public int MaxEnergy => maxEnergy;

    //当前选中的目标
    public BaseBattleUnit selectedTarget;

    //每回合抽卡数
    public int turnDrawCardNum = 5;

    //战斗数据总字典<id,战斗数据>
    public Dictionary<int, BattleData> battleDataDic = new Dictionary<int, BattleData>();

    //回合开始时触发的效果委托
    public Action turnStartEffectDelegate;

    //是否是本场战斗的第一回合
    public bool isInit = true;

    //本回合使用的卡牌数
    public int currentTurnCombo = 0;

    //当前战斗类型
    public BattleType currentBattleType = BattleType.Normal;
    /// <summary>
    /// 敌人相关
    /// </summary>
    //敌人预制体
    public GameObject enemyPrefab;

    //敌人数组
    public List<Enemy> inBattleEnemyList = new List<Enemy>();

    //敌人位置数组
    public List<Vector3> enemyPosList = new List<Vector3>();

    //初始化战斗管理器
    public void InitBattleManager()
    {
        //获取敌人预制体和敌人父物体
        enemyPrefab = ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/" + "Battle/" + "Enemy/" + "Enemy");
        //初始化敌人位置列表
        enemyPosList = new List<Vector3>()
        {
            new Vector3(250, -50, 0),
            new Vector3(125, -50, 0),
            new Vector3(375, -50, 0),
        };
        //初始化抽牌数
        turnDrawCardNum = 5;
        //初始化卡牌目标为空
        selectedTarget = null;
        //初始化能量
        maxEnergy = 3;
        currentEnergy = maxEnergy;
    }

    //初始化战斗
    public void InitBattle(BattleData battleData)
    {
        //显示相应UI
        UIManager.Instance.HideSingleUI(E_UiId.MapUI);
        UIManager.Instance.ShowUI(E_UiId.BattleUI);
        //初始化牌库
        DeskManager.Instance.ResetDesks();
        //战斗UI显示时触发战斗开始事件
        EventDispatcher.TriggerEvent(E_MessageType.BattleStart);
        //记录当前战斗的类型
        currentBattleType = battleData.BattleType;
        foreach (var enemyID in battleData.EnemyIDList)
        {
            CreateEnemy(enemyID);
        }
        //初始化
        isInit = false;

        //开始回合
        StartCoroutine(TurnStart(true));
    }

    //回合开始效果携程
    public IEnumerator TurnStartAction()
    {
        turnStartEffectDelegate.Invoke();
        //清空回合开始委托
        turnStartEffectDelegate = null;
        yield return new WaitForSeconds(0.5f);
    }

    //回合开始
    public IEnumerator TurnStart(bool isFirst=false)
    {
        //如果是第一回合，则检测敌人有无开始战斗行动
        if (isFirst)
        {
            foreach (var enemy in inBattleEnemyList)
            {
                if (enemy.enemyData.battleStartActionList.Count>0)
                {
                    foreach (var action in enemy.enemyData.battleStartActionList)
                    {
                        yield return new WaitForSeconds(0.5f);
                        TriggerActionEffect(enemy,action);
                    }
                }
            }
        }
        EventDispatcher.TriggerEvent(E_MessageType.TurnStart);
        yield return new WaitForSeconds(0.5f);

        //初始化能量、连斩数
        currentEnergy = maxEnergy;
        currentTurnCombo = 0;
        //触发回合开始效果
        if (turnStartEffectDelegate != null)
        {
            yield return StartCoroutine(TurnStartAction());
        }

        //抽牌
        for (int i = 0; i < turnDrawCardNum; i++)
        {
            DeskManager.Instance.DrawCard();
        }

        //更新卡牌和敌人行动数值
        Player.Instance.hasCheckList.Clear();
        foreach (var enemy in inBattleEnemyList)
        {
            enemy.hasCheckList.Clear();
        }

        UpdateCardAndActionValue();
    }

    //回合结束按钮点击回调
    public void OnTurnEndButtonDown()
    {
        StartCoroutine(TurnEnd());
    }

    //回合结束携程
    public IEnumerator TurnEnd()
    {
        EventDispatcher.TriggerEvent(E_MessageType.TurnEnd);
        HandCardManager.Instance.DisAllCard();
        foreach (var enemy in inBattleEnemyList)
        {
            //每个敌人行动间隔一段时间
            yield return new WaitForSeconds(0.5f);
            enemy.TakeAction();
        }

        yield return new WaitForSeconds(0.5f);
        foreach (var enemy in inBattleEnemyList)
        {
            enemy.UpdateCurrentAction();
        }

        StartCoroutine(TurnStart());
    }

    //创建敌人
    public void CreateEnemy(int enemyID)
    {
        GameObject enemyGO = Instantiate(enemyPrefab);
        enemyGO.transform.SetParent(GameObject.Find("Enemies").transform);
        enemyGO.transform.localPosition = enemyPosList[inBattleEnemyList.Count];
        enemyGO.transform.localScale = new Vector3(1, 1, 1);

        Enemy newEnemy = enemyGO.GetComponent<Enemy>();
        newEnemy.Init(enemyID);
        inBattleEnemyList.Add(newEnemy);
    }

    //触发敌人行动效果
    public void TriggerActionEffect(BaseBattleUnit unit, ActionData actData)
    {
        switch (actData.ActionID)
        {
            //对玩家造成value点伤害
            case 1001:
            case 2001:
                Player.Instance.TakeDamage(actData.actualValue,null);
                break;
            //自身获得value层灵体
            case 1002:
                StateManager.AddStateToTarget(unit, 1001, actData.actualValue);
                break;
            //获得value点护甲
            case 1003:
                unit.GetShield(actData.actualValue);
                break;
            //什么都不做
            case 1004:
                break;
            //给予目标value层重伤
            case 1005:
                break;
            //给予自身value层护甲重伤
            case 1006:
                StateManager.AddStateToTarget(unit,1004,actData.actualValue);
                break;
        }
    }

    //设置费用
    public void EditEnergy(int newEnergy)
    {
        currentEnergy = newEnergy;
    }

    //刷新卡牌数值和行为数值
    public void UpdateCardAndActionValue()
    {
        //更新卡牌数值
        //伤害类效果检测
        foreach (var handCardGo in HandCardManager.Instance.handCardGoList)
        {
            var cardData = handCardGo.GetComponent<HandCard>().CardData;

            foreach (var data in cardData.cardEffectDic.Values)
            {
                if (data.effectType == CardEffectType.Damage)
                {
                    //恐慌检测
                    if (StateManager.CheckState(Player.Instance, 1002))
                    {
                        data.actualValue = (int) (0.7f * data.EffectValue);
                        Player.Instance.hasCheckList.Add(1002);
                    }

                    //替换描述
                    UpdateEffectDes(handCardGo, cardData, data);
                }
            }
        }

        //护甲类效果检测
        foreach (var handCardGo in HandCardManager.Instance.handCardGoList)
        {
            var cardData = handCardGo.GetComponent<HandCard>().CardData;

            foreach (var data in cardData.cardEffectDic.Values)
            {
                if (data.effectType == CardEffectType.Shield)
                {
                    //焕发检测
                    if (StateManager.CheckState(Player.Instance, 1003))
                    {
                        data.actualValue = (int)(1.3f * data.EffectValue);
                        Player.Instance.hasCheckList.Add(1003);
                    }
                    //重伤检测
                    if (StateManager.CheckState(Player.Instance, 1005))
                    {
                        data.actualValue = (int)(0.7f * data.EffectValue);
                        Player.Instance.hasCheckList.Add(1005);
                    }
                    //替换描述
                    UpdateEffectDes(handCardGo, cardData, data);
                    Player.Instance.hasCheckList.Add(1002);
                }
            }
        }
        //更新敌人行为数值
        foreach (var enemy in inBattleEnemyList)
        {
            //恐惧检测
            if (StateManager.CheckState(enemy, 1002) &&
                (enemy.currentAction.ActionType == ActionType.Attack) && !enemy.hasCheckList.Contains(1002))
            {
                enemy.hasCheckList.Add(1002);
                enemy.currentAction.actualValue = (int) (enemy.currentAction.actualValue * 0.7f);
            }

            enemy.UpdateUI();
        }
    }

    //根据效果类型更新卡牌描述
    public void UpdateEffectDes(GameObject card,CardData cardData, CardEffectData data)
    {
        //替换描述
        cardData.cardDes = cardData.cardDes.Replace(
            data.EffectDes, data.EffectDes.Replace(
                data.EffectValue.ToString(), data.actualValue.ToString()));
        GameTool.GetTheChildComponent<Text>(card, "Text_CardEffect").text = cardData.cardDes;
    }

    //根据卡牌效果ID和效果值触发效果
    public void TakeCardEffect(int effectID, int effectValue, BaseBattleUnit target = null, bool isCanXin = false,
        bool isLianZhan = false)
    {
        //如果没有特别指定目标则默认指定当前选中的目标
        if (target == null)
        {
            target = selectedTarget;
        }

        //安全校验
        if (target == null)
        {
            return;
        }

        switch (effectID)
        {
            //对目标造成单体伤害
            case 1001:
                if (target != null)
                {
                    target.TakeDamage(effectValue,Player.Instance);
                }

                break;
            //获得护甲
            case 1002:
                if (target != null)
                {
                    target.GetShield(effectValue);
                }

                break;
            //残心：获得能量
            case 1003:
                if (!isCanXin)
                {
                    break;
                }

                turnStartEffectDelegate += delegate { currentEnergy += effectValue; };
                break;
            //残心：对随机敌人造成伤害
            case 1004:
                if (!isCanXin)
                {
                    break;
                }

                turnStartEffectDelegate += delegate
                {
                    target = BattleManager.Instance.inBattleEnemyList[
                        Random.Range(0, BattleManager.Instance.inBattleEnemyList.Count)];
                    target.TakeDamage(effectValue,Player.Instance);
                };
                break;
            //附加恐惧
            case 1005:
                StateManager.AddStateToTarget(target, 1002, effectValue);
                break;
            default:
                break;
        }
    }

    //战斗结束
    public void BattleEnd()
    {
        //重置回合数
        isInit = true;
        //当前所在层数+1
        GameSceneManager.Instance.currentLayer++;
        //弃掉所有手牌并重置牌堆
        HandCardManager.Instance.DisAllCard();
        DeskManager.Instance.ResetDesks();
        //清除玩家身上所有状态
        Player.Instance.ClearAllState();
        //清空回合开始效果
        turnStartEffectDelegate = null;
        //隐藏战斗UI
        UIManager.Instance.HideSingleUI(E_UiId.BattleUI);
        //显示地图界面
        UIManager.Instance.ShowUI(E_UiId.MapUI);
        GameSceneManager.Instance.UpdateGameSceneState();
        //选牌

        //保存游戏
        SaveManager.SaveGame();
    }
    
    //残心检测
    public bool CheckCanxin(CardData data)
    {
        return currentEnergy == data.cardCost;
    }

    //连斩检测（连斩数）
    public bool CheckCombo(int combo)
    {
        return currentTurnCombo >= combo;
    }

    private void Update()
    {
        
    }

    private void Awake()
    {
        InitBattleManager();
    }
}