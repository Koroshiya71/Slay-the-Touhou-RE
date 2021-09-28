using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class BattleManager : UnitySingleton<BattleManager>
{
    //当前能量值
    private int currentEnergy=3;
    public int CurrentEnergy => currentEnergy;
    //能量上限
    private int maxEnergy=3;
    public int MaxEnergy => maxEnergy;

    //当前选中的目标
    public BaseBattleUnit selectedTarget;

    //每回合抽卡数
    public int turnDrawCardNum = 5;

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
        enemyPrefab = ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/" + "Enemy/" + "Enemy");
        //初始化敌人位置列表
        enemyPosList= new List<Vector3>()
        {
            new Vector3(450,-75,0),
            new Vector3(250,-75,0),
            new Vector3(650,-75,0),
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
    public void InitBattle()
    {
        currentEnergy = maxEnergy;
        //战斗UI显示时触发战斗开始事件
        EventDispatcher.TriggerEvent(E_MessageType.BattleStart);

        for (int i = 0; i < turnDrawCardNum; i++)
        {
            HandCardManager.Instance.GetCardByID(1001);
        }
        CreateEnemy(1);
    }

    //创建敌人
    public void CreateEnemy(int enemyID)
    {
        GameObject enemyGO = Instantiate(enemyPrefab);
        enemyGO.transform.SetParent(GameObject.Find("Enemies").transform);
        enemyGO.transform.position = enemyPosList[inBattleEnemyList.Count];
        Enemy newEnemy =enemyGO .GetComponent<Enemy>();
        newEnemy.Init(enemyID);
        inBattleEnemyList.Add(newEnemy);
    }

    //设置费用
    public void EditEnergy(int newEnergy)
    {
        currentEnergy = newEnergy;
    }
    
    //根据卡牌效果ID和效果值触发效果
    public void TakeCardEffect(int effectID,int effectValue,BaseBattleUnit target=null)
    {
        //如果没有特别指定目标则默认指定当前选中的目标
        if (target==null)
        {
            target = selectedTarget;
        }
        switch (effectID)
        {
            //对目标造成单体伤害
            case 1001:
                if (target!=null)
                {
                    selectedTarget.TakeDamage(effectValue);
                }
                break;
        }
    }


    private void Awake()
    {
        InitBattleManager();
    }
}
