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

    //本场战斗触发的残心数
    public int battleCanXinCount = 0;

    /// <summary>
    /// 敌人相关
    /// </summary>
    //敌人预制体
    public GameObject enemyPrefab;

    //敌人数组
    public List<Enemy> inBattleEnemyList = new List<Enemy>();

    //敌人位置数组
    public List<Vector3> enemyPosList = new List<Vector3>();

    //战斗结算金币数
    public int battleGold = 0;

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
        //如果不是多人游戏则隐藏同步角色
        if (!GameManager.Instance.isMulti)
        {
            SyncPlayer.Instance.gameObject.SetActive(false);
        }
        //初始化牌库
        DeskManager.Instance.ResetDesks();
        //战斗金币数归零
        battleGold = 0;
        //战斗UI显示时触发战斗开始事件
        EventDispatcher.TriggerEvent(E_MessageType.BattleStart);
        //记录当前战斗的类型
        currentBattleType = battleData.BattleType;
        foreach (var enemyID in battleData.EnemyIDList)
        {
            CreateEnemy(enemyID);
        }

        //战斗开始遗物检测
        RelicManager.Instance.CheckRelicBattleStartEffect();
        //初始化
        isInit = false;
        battleCanXinCount = 0;
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
    public IEnumerator TurnStart(bool isFirst = false)
    {
        //如果是第一回合，则检测敌人有无开始战斗行动
        if (isFirst)
        {
            foreach (var enemy in inBattleEnemyList)
            {
                if (enemy.enemyData.battleStartActionList.Count > 0)
                {
                    foreach (var action in enemy.enemyData.battleStartActionList)
                    {
                        yield return new WaitForSeconds(0.5f);
                        TriggerActionEffect(enemy, action);
                    }
                }
            }
        }

        EventDispatcher.TriggerEvent(E_MessageType.TurnStart);
        yield return new WaitForSeconds(0.5f);

        //初始化能量、连斩数
        //偷来的书检测
        if (currentEnergy > 0 && RelicManager.Instance.CheckRelic(1003))
        {
            currentEnergy = maxEnergy + 1;
        }
        else
        {
            currentEnergy = maxEnergy;
        }

        currentTurnCombo = 0;
        SyncPlayer.currentTurnCombo = 0;
        StateManager.Instance.hasDoubleBlade = false;
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
        //如果是多人游戏则发送回合结束消息
        if (GameManager.Instance.isMulti)
        {
            MsgTurnEnd msg = new MsgTurnEnd();
            msg.id = NetManager.playerID;
            NetManager.Send(msg);
        }
        else
        {
            StartCoroutine(TurnEnd());
        }
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
                Player.Instance.TakeDamage(actData.actualValue[0], unit);
                break;
            //自身获得value层灵体
            case 1002:
                StateManager.AddStateToTarget(unit, 1001, actData.actualValue[0]);
                break;
            //获得value点护甲
            case 1003:
                unit.GetShield(actData.actualValue[0]);
                break;
            //什么都不做
            case 1004:
                break;
            //给予目标value层重伤
            case 1005:
                break;
            //给予自身value层护甲重伤
            case 1006:
                StateManager.AddStateToTarget(unit, 1004, actData.actualValue[0]);
                break;
            //造成自身当前value%生命值的伤害
            case 1007:
                Player.Instance.TakeDamage(Convert.ToInt32(actData.actualValue[0]* unit.currentHp*0.01f ), unit);
                break;
            //回复value%生命值，获得value2层灵体
            case 1008:
                unit.Heal((int)(actData.actualValue[0] * 0.01f * unit.maxHp));
                StateManager.AddStateToTarget(unit,1002, actData.actualValue[1]);
                break;
            //对目标造成value点伤害并附加value2层恐慌
            case 1009:
                Player.Instance.TakeDamage(actData.actualValue[0] , unit);
                StateManager.AddStateToTarget(Player.Instance, 1002, actData.actualValue[1]);
                break;
            //获得value层返魂碟
            case 1010:
                StateManager.AddStateToTarget(unit, 1008, actData.actualValue[0]);
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
        foreach (var handCardGo in HandCardManager.Instance.handCardGoList)
        {
            var cardData = handCardGo.GetComponent<HandCard>().CardData;

            foreach (var data in cardData.cardEffectDic.Values)
            {
                //伤害类效果检测
                if (data.effectType == CardEffectType.Damage)
                {
                    //恐慌检测
                    if (StateManager.CheckState(Player.Instance, 1002) && !Player.Instance.hasCheckList.Contains(1002))
                    {
                        data.actualValue = (int) (0.7f * data.EffectValue);
                        Player.Instance.hasCheckList.Add(1002);
                    }
                }

                //护甲类效果检测
                if (data.effectType == CardEffectType.Shield)
                {
                    //焕发检测
                    if (StateManager.CheckState(Player.Instance, 1003) && !Player.Instance.hasCheckList.Contains(1003))
                    {
                        data.actualValue = (int) (1.3f * data.EffectValue);
                        Player.Instance.hasCheckList.Add(1003);
                    }

                    //重伤检测
                    if (StateManager.CheckState(Player.Instance, 1005) && !Player.Instance.hasCheckList.Contains(1005))
                    {
                        data.actualValue = (int) (0.7f * data.EffectValue);
                        Player.Instance.hasCheckList.Add(1005);
                    }
                }

                //残心减费（备注：卡牌检测默认2000开头）
                if (data.EffectID == 1008)
                {
                    cardData.cardCost = cardData.originCost - battleCanXinCount;
                    if (cardData.cardCost < 0)
                    {
                        cardData.cardCost = 0;
                    }

                    cardData.hasModified = true;
                }

                //替换显示
                UpdateCardUI(handCardGo, cardData, data);
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
                enemy.currentAction.actualValue[0] = (int) (enemy.currentAction.actualValue[0] * 0.7f);
            }

            enemy.UpdateUI();
        }

    }

    //根据效果类型更新卡牌UI
    public void UpdateCardUI(GameObject card, CardData cardData, CardEffectData data)
    {
        //替换描述
        cardData.cardDes = cardData.cardDes.Replace(
            data.EffectDes, data.EffectDes.Replace(
                data.EffectValue.ToString(), data.actualValue.ToString()));
        GameTool.GetTheChildComponent<Text>(card, "Text_CardEffect").text = cardData.cardDes;
        GameTool.GetTheChildComponent<Text>(card, "Text_CardCost").text = cardData.cardCost.ToString();
    }

    //根据卡牌效果ID和效果值触发效果
    public void TakeCardEffect(int effectID, int effectValue, BaseBattleUnit target = null, int isCanXin = 0,
        bool isLianZhan = false,bool isSync=false)
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

        int targetIndex=GetBattleUnitIndex(target);
        switch (effectID)
        {
            //对目标造成单体伤害
            case 1001:
                if (target != null)
                {
                    target.TakeDamage(effectValue, Player.Instance);
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
                switch (isCanXin)
                {
                    case 0:
                        break;
                    case 1:
                        turnStartEffectDelegate += delegate { EditEnergy(currentEnergy+effectValue); };
                        break;
                    case 2:
                        EditEnergy(currentEnergy + effectValue);
                        break;

                }
             

                break;
            //残心：对随机敌人造成伤害
            case 1004:
                if (isSync)
                {
                    break;
                }
                switch (isCanXin)
                {
                    case 0:
                        break;
                    case 1:
                        turnStartEffectDelegate += delegate
                        {
                            target = BattleManager.Instance.inBattleEnemyList[
                                Random.Range(0, BattleManager.Instance.inBattleEnemyList.Count)];
                            targetIndex = GetBattleUnitIndex(target);
                            TakeCardEffect(1001,effectValue,target);

                        };
                        break;
                    case 2:
                        target = BattleManager.Instance.inBattleEnemyList[
                            Random.Range(0, BattleManager.Instance.inBattleEnemyList.Count)];
                        targetIndex = GetBattleUnitIndex(target);
                        TakeCardEffect(1001, effectValue, target);
                        break;
                }

                break;
            //附加恐惧
            case 1005:
                StateManager.AddStateToTarget(target, 1002, effectValue);
                break;
            //附加二刀流
            case 1007:
                StateManager.AddStateToTarget(target, 1006, effectValue);
                break;
            //对所有敌人造成伤害
            case 1009:
                for (int i = 0; i < inBattleEnemyList.Count; i++)
                {
                    if (inBattleEnemyList[i].TakeDamage(effectValue, Player.Instance))
                    {
                        i--;
                    }
                }

                break;
            //抽牌
            case 1010:
                if (isSync)
                {
                    break;
                }
                for (int i = 0; i < effectValue; i++)
                {
                    DeskManager.Instance.DrawCard();
                }

                break;
            //如果是本回合使用的第一张牌，对目标造成伤害
            case 1011:
                if (isSync&&SyncPlayer.currentTurnCombo==0)
                {
                    target.TakeDamage(effectValue, Player.Instance);
                }
                else if (currentTurnCombo == 0)
                {
                    target.TakeDamage(effectValue, Player.Instance);
                }

                Debug.Log(currentTurnCombo+":"+SyncPlayer.currentTurnCombo);
                break;
            //附加起势
            case 1012:
                StateManager.AddStateToTarget(target, 1007, effectValue);
                break;
            //抽取一张连斩牌:
            case 1013:
                if (isSync)
                {
                    break;
                }
                int count = 3000;
                while (true)
                {
                    count--;
                    bool get = false;
                    int drawIndex = Random.Range(0, DeskManager.Instance.drawCardDeskList.Count);
                    int disIndex = Random.Range(0, DeskManager.Instance.disCardDeskList.Count);
                    if (DeskManager.Instance.drawCardDeskList.Count <= 0)
                    {
                        continue;
                    }
                    CardData cardData = DeskManager.Instance.drawCardDeskList[drawIndex];

                    foreach (var eff in cardData.cardEffectDic.Values)
                    {
                        if (eff.combo > 0)
                        {
                            DeskManager.Instance.DrawTargetCard(cardData);
                            get = true;
                            break;
                        }
                    }
                    if (get)
                    {
                        break;
                    }
                    if (DeskManager.Instance.disCardDeskList.Count <= 0)
                    {
                        continue;
                    }
                    cardData = DeskManager.Instance.disCardDeskList[disIndex];

                    foreach (var eff in cardData.cardEffectDic.Values)
                    {
                        if (eff.combo > 0)
                        {
                            DeskManager.Instance.DrawTargetCard(cardData);
                            get = true;
                            break;
                        }
                    }

                    if (get)
                    {
                        break;
                    }

                    if (count <= 0)
                    {
                        break;
                    }
                }

                break;
            default:
                break;
        }
        //如果正在多人游戏则发送使用卡牌消息
        if (GameManager.Instance.isMulti&&!isSync)
        {
            MsgCardEffect msg = new MsgCardEffect();
            msg.effectValue = effectValue;
            msg.isCanXin = isCanXin;
            msg.effectID = effectID;
            msg.isLianZhan = isLianZhan;
            msg.targetIndex = targetIndex;
            msg.id = NetManager.playerID;
            NetManager.Send(msg);
            Debug.Log("Send MsgCardData");
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
        //保存游戏
        SaveManager.SaveGame();
        //显示战斗奖励界面
        UIManager.Instance.ShowUI(E_UiId.BattleRewardUI);
        EventDispatcher.TriggerEvent(E_MessageType.BattleReward);
    }

    //显示并初始化战斗结果UI
    public void ShowBattleReward()
    {
        UIManager.Instance.ShowUI(E_UiId.BattleRewardUI);
    }

    //残心检测
    public int CheckCanXin(CardData data, CardEffectData effect)
    {
        //楼观剑检测
        if (effect.isCanXin && battleCanXinCount == 0 && RelicManager.Instance.CheckRelic(1004))
        {
            battleCanXinCount++;
            return 2;
        }

        if (data.cardCost == currentEnergy)
        {
            if (effect.isCanXin)
            {
                battleCanXinCount++;
                return 1;
            }
        }

        return 0;
    }

    //连斩检测（连斩数）
    public bool CheckCombo(int combo)
    {
        return currentTurnCombo >= combo;
    }
    //根据战斗单位获取其下标
    public int GetBattleUnitIndex(BaseBattleUnit unit)
    {
        //如果是玩家本身则返回-2
        if (unit == Player.Instance)
        {
            return -2;
        }
        //如果是同步玩家返回-1
        else if (GameManager.Instance.isMulti&&unit==SyncPlayer.Instance)
        {
            return -1;
        }
        //否则直接取敌人下标
        else
        {
            return inBattleEnemyList.IndexOf((Enemy)unit);
        }
    }
    //根据下标获取战斗单位
    public BaseBattleUnit GetBattleUnitByIndex(int index)
    {
        if (index>inBattleEnemyList.Count)
        {
            return null;
        }
        switch (index)
        {
            case -2:
                return Player.Instance;
            case -1:
                return SyncPlayer.Instance;
            default:
                return BattleManager.Instance.inBattleEnemyList[index];
        }

    }
    private void Update()
    {
    }

    private void Awake()
    {
        InitBattleManager();
    }
}