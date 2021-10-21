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

    //ս���������ֵ�<id,ս������>
    public Dictionary<int, BattleData> battleDataDic = new Dictionary<int, BattleData>();

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
            new Vector3(250,-50,0),
            new Vector3(125,-50,0),
            new Vector3(375,-50,0),
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
    public void InitBattle(BattleData battleData)
    {
        UIManager.Instance.HideSingleUI(E_UiId.MapUI);
        UIManager.Instance.ShowUI(E_UiId.BattleUI);

        //ս��UI��ʾʱ����ս����ʼ�¼�
        EventDispatcher.TriggerEvent(E_MessageType.BattleStart);

        foreach (var enemyID in battleData.EnemyIDList)
        {
            CreateEnemy(enemyID);
        }

        //��ʼ�غ�
        StartCoroutine(TurnStart());
    }
    //�غϿ�ʼ
    public IEnumerator TurnStart()
    {
        EventDispatcher.TriggerEvent(E_MessageType.TurnStart);
        yield return new WaitForSeconds(0.5f);
        //��ʼ������
        currentEnergy = maxEnergy;
        //����
        for (int i = 0; i < turnDrawCardNum; i++)
        {
            DeskManager.Instance.DrawCard();
        }
    }
    //�غϽ�����ť����ص�
    public void OnTurnEndButtonDown()
    {
        StartCoroutine(TurnEnd());
    }
    //�غϽ���Я��
    public IEnumerator TurnEnd()
    {
        
        HandCardManager.Instance.DisAllCard();
        EventDispatcher.TriggerEvent(E_MessageType.TurnEnd);
        yield return new WaitForSeconds(0.5f);
        foreach (var enemy in inBattleEnemyList)
        {
            enemy.TakeAction();
            //TODO����Ӳ��Ŷ����Ĺ���
        }
        StartCoroutine(TurnStart());
    }
    //��������
    public void CreateEnemy(int enemyID)
    {
        GameObject enemyGO = Instantiate(enemyPrefab);
        enemyGO.transform.SetParent(GameObject.Find("Enemies").transform);
        enemyGO.transform.localPosition = enemyPosList[inBattleEnemyList.Count];
        Enemy newEnemy =enemyGO .GetComponent<Enemy>();
        newEnemy.Init(enemyID);
        inBattleEnemyList.Add(newEnemy);
    }
    //���������ж�Ч��
    public void TriggerActionEffect(ActionData actData)
    {
        switch (actData.ActionID)
        {
            //��������value���˺�
            case 1001:
                Player.Instance.TakeDamage(actData.ActionValue);
                break;
        }
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
                    target.TakeDamage(effectValue);
                }
                break;
            //��û���
            case 1002:
                if (target!=null)
                {
                    target.GetShield(effectValue);
                }
                break;
        }
    }

    //ս������
    public void BattleEnd()
    {
        //��ǰ���ڲ���+1
        GameSceneManager.Instance.currentLayer++;
        //�����������Ʋ������ƶ�
        HandCardManager.Instance.DisAllCard();
        DeskManager.Instance.ResetDesks();
        //����ս��UI
        UIManager.Instance.HideSingleUI(E_UiId.BattleUI);
        //��ʾ��ͼ����
        UIManager.Instance.ShowUI(E_UiId.MapUI);
        GameSceneManager.Instance.UpdateGameSceneState();
    }



    private void Awake()
    {
        InitBattleManager();
    }
}
