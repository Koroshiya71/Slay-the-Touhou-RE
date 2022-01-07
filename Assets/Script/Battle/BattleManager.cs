using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

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

    //是否是本场战斗的第一回合
    public bool isInit = true;

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
            new Vector3(250,-50,0),
            new Vector3(125,-50,0),
            new Vector3(375,-50,0),
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

        foreach (var enemyID in battleData.EnemyIDList)
        {
            CreateEnemy(enemyID);
        }
        //初始化
        isInit = false;
        //开始回合
        StartCoroutine(TurnStart());
    }
    //回合开始
    public IEnumerator TurnStart()
    {
        EventDispatcher.TriggerEvent(E_MessageType.TurnStart);
        yield return new WaitForSeconds(0.5f);
        //初始化能量
        currentEnergy = maxEnergy;
        //抽牌
        for (int i = 0; i < turnDrawCardNum; i++)
        {
            DeskManager.Instance.DrawCard();
        }
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
            //TODO：跟进每个敌人的动画时间决定这个间隔
            yield return new WaitForSeconds(0.5f);
            enemy.TakeAction();
            //TODO：添加播放动画的功能
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
                Player.Instance.TakeDamage(actData.ActionValue);
                break;
            //自身获得value层灵体
            case 1002:
                StateManager.AddStateToTarget(unit, 1001, actData.ActionValue);
                break;
        }
    }

    //设置费用
    public void EditEnergy(int newEnergy)
    {
        currentEnergy = newEnergy;

    }

    //根据卡牌效果ID和效果值触发效果
    public void TakeCardEffect(int effectID, int effectValue, BaseBattleUnit target = null, bool isCanXin = false, bool isLianZhan = false)
    {
        //如果没有特别指定目标则默认指定当前选中的目标
        if (target == null)
        {
            target = selectedTarget;
        }
        switch (effectID)
        {
            //对目标造成单体伤害
            case 1001:
                if (target != null)
                {
                    target.TakeDamage(effectValue);
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
                BattleManager.Instance.currentEnergy += effectValue;
                break;
            //残心：对随机敌人造成伤害
            case 1004:
                if (!isCanXin)
                {
                    break;
                }
                target = BattleManager.Instance.inBattleEnemyList[Random.Range(0, BattleManager.Instance.inBattleEnemyList.Count)];
                target.TakeDamage(effectValue);
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
        //隐藏战斗UI
        UIManager.Instance.HideSingleUI(E_UiId.BattleUI);
        //显示地图界面
        UIManager.Instance.ShowUI(E_UiId.MapUI);
        GameSceneManager.Instance.UpdateGameSceneState();
    }

    //残心检测
    public bool CheckCanxin(CardData data)
    {
        return currentEnergy == data.cardCost;
    }
    private void Update()
    {
        //GM调试命令
        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < inBattleEnemyList.Count; i++)
            {
                inBattleEnemyList[i].Die();
            }
            BattleEnd();
        }
    }

    private void Awake()
    {
        InitBattleManager();
    }
}
