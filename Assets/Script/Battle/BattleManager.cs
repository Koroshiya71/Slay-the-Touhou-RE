using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class BattleManager : UnitySingleton<BattleManager>
{
    //��ǰ����ֵ
    private int currentEnergy=3;
    public int CurrentEnergy => currentEnergy;
    //��������
    private int maxEnergy=3;
    public int MaxEnergy => maxEnergy;

    //��ǰѡ�е�Ŀ��
    public BaseBattleUnit selectedTarget;

    //ÿ�غϳ鿨��
    public int turnDrawCardNum = 5;

    /// <summary>
    /// �������
    /// </summary>
    //����Ԥ����
    public GameObject enemyPrefab;

    //��������
    public List<Enemy> inBattleEnemyList = new List<Enemy>();
    //����λ������
    public List<Vector3> enemyPosList = new List<Vector3>();

    //��ʼ��ս��������
    public void InitBattleManager()
    {
        //��ȡ����Ԥ����͵��˸�����
        enemyPrefab = ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/" + "Enemy/" + "Enemy");
        //��ʼ������λ���б�
        enemyPosList= new List<Vector3>()
        {
            new Vector3(355,-75,0),
            new Vector3(655,-75,0),
            new Vector3(55,-75,0),
        };
        //��ʼ��������
        turnDrawCardNum = 5;
        //��ʼ������Ŀ��Ϊ��
        selectedTarget = null;
        //��ʼ������
        maxEnergy = 3;
        currentEnergy = maxEnergy;
    }


    //��ʼ��ս��
    public void InitBattle()
    {
        currentEnergy = maxEnergy;
        //ս��UI��ʾʱ����ս����ʼ�¼�
        EventDispatcher.TriggerEvent(E_MessageType.BattleStart);

        for (int i = 0; i < turnDrawCardNum; i++)
        {
            HandCardManager.Instance.GetCardByID(1001);
        }
        CreateEnemy(1);
    }

    //��������
    public void CreateEnemy(int enemyID)
    {
        GameObject enemyGO = Instantiate(enemyPrefab);
        enemyGO.transform.SetParent(GameObject.Find("Enemies").transform);
        enemyGO.transform.position = enemyPosList[inBattleEnemyList.Count];
        Enemy newEnemy =enemyGO .GetComponent<Enemy>();
        newEnemy.Init(enemyID);
        inBattleEnemyList.Add(newEnemy);
    }

    //���÷���
    public void EditEnergy(int newEnergy)
    {
        currentEnergy = newEnergy;
    }
    
    //���ݿ���Ч��ID��Ч��ֵ����Ч��
    public void TakeCardEffect(int effectID,int effectValue,BaseBattleUnit target=null)
    {
        //���û���ر�ָ��Ŀ����Ĭ��ָ����ǰѡ�е�Ŀ��
        if (target==null)
        {
            target = selectedTarget;
        }
        switch (effectID)
        {
            //��Ŀ����ɵ����˺�
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