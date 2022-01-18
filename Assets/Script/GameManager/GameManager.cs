using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using Newtonsoft.Json;
using System.IO;

//玩家数据
public class PlayerData
{
    //最大血量
    public int maxHp = 50;

    //当前血量
    public int currentHp = 50;

    //金币数
    public int gold = 99;

    public PlayerData()
    {
    }
}

//出货率 
public class CardRareClass
{
    //普通战斗出货率（普通、稀有、史诗）
    public List<int> normalCombatRare = new List<int>() {50, 40, 10};

    //精英战斗出货率（普通、稀有、史诗）
    public List<int> eliteCombatRare = new List<int>() {20, 50, 30};

    //Boss战出货率（普通、稀有、史诗）
    public List<int> bossCombatRare = new List<int>() {0, 0, 100};

    public CardRareClass()
    {
    }
}

public class GameManager : UnitySingleton<GameManager>
{
    //玩家数据
    public PlayerData playerData = new PlayerData();

    //卡牌出货率
    public CardRareClass cardRare;

    //战斗结束选牌数
    public int chooseCardNumAfterBattle = 3;

    private void Start()
    {
        StreamReader reader;

        if (!SaveManager.isLoad)
        {
            //读取玩家初始数值json
            reader = new StreamReader(SaveManager.jsonDataPath + "PlayerInit.json");
            playerData = JsonConvert.DeserializeObject<PlayerData>(reader.ReadToEnd());
            reader.Close();
        }
        
        //读取卡牌掉落稀有度配置
        reader = new StreamReader(SaveManager.jsonDataPath + "RareCard.json");
        cardRare = JsonConvert.DeserializeObject<CardRareClass>(reader.ReadToEnd());
        reader.Close();
    }

    //战斗后选牌方法
    public void GetCardAfterBattle(BattleType battleType)
    {
        //各种卡牌的出货率
        int normal, rare, epic;
        //根据战斗类型获取出货率
        switch (battleType)
        {
            case BattleType.Normal:
                normal = cardRare.normalCombatRare[0];
                rare = cardRare.normalCombatRare[1];
                epic = cardRare.normalCombatRare[2];
                break;
            case BattleType.Elite:
                normal = cardRare.eliteCombatRare[0];
                rare = cardRare.eliteCombatRare[1];
                epic = cardRare.eliteCombatRare[2];
                break;
            case BattleType.Boss:
                normal = cardRare.bossCombatRare[0];
                rare = cardRare.bossCombatRare[1];
                epic = cardRare.bossCombatRare[2];
                break;
            default:
                normal = cardRare.normalCombatRare[0];
                rare = cardRare.normalCombatRare[1];
                epic = cardRare.normalCombatRare[2];
                break;
        }

        //初始化选牌列表
        List<CardData> chooseDataList = new List<CardData>();
        for (int i = 0; i < chooseCardNumAfterBattle; i++)
        {
            int rareRand = Random.Range(0, 100);
            if (rareRand < normal)
            {
                while (true)
                {
                    //随机取一张牌
                    CardData newData =
                        DeskManager.Instance.allCardDataList[
                            Random.Range(0, DeskManager.Instance.allCardDataList.Count)];
                    //检查其稀有度是否为普通且是否已存在于列表中
                    if (newData.cardRare == CardRare.Normal && !chooseDataList.Contains(newData))
                    {
                        //添加到选牌列表
                        chooseDataList.Add(newData);
                        break;
                    }
                }
            }
            else if (rareRand < normal + rare)
            {
                while (true)
                {
                    //随机取一张牌
                    CardData newData =
                        DeskManager.Instance.allCardDataList[
                            Random.Range(0, DeskManager.Instance.allCardDataList.Count)];
                    //检查其稀有度是否为稀有且未存在
                    if (newData.cardRare == CardRare.Rare && !chooseDataList.Contains(newData))
                    {
                        //添加到选牌列表
                        chooseDataList.Add(newData);
                        break;
                    }
                }
            }
            else
            {
                while (true)
                {
                    //随机取一张牌
                    CardData newData =
                        DeskManager.Instance.allCardDataList[
                            Random.Range(0, DeskManager.Instance.allCardDataList.Count)];
                    //检查其稀有度是否为稀有且未存在
                    if (newData.cardRare == CardRare.Epic && !chooseDataList.Contains(newData))
                    {
                        //添加到选牌列表
                        chooseDataList.Add(newData);
                        break;
                    }
                }
            }
        }
        //选牌
        StartCoroutine(DeskManager.Instance.ChooseCardAddToDesk(1, chooseDataList));
    }
    //获取金币方法
    public void GetGold(int goldNum)
    {
        playerData.gold += goldNum;
        EventDispatcher.TriggerEvent(E_MessageType.UpdateGameMainUI);
    }
    void Update()
    {
        //GM调试命令
        if (Input.GetKeyDown(KeyCode.R)) //结束战斗
        {
            while (BattleManager.Instance.inBattleEnemyList.Count != 0)
            {
                BattleManager.Instance.inBattleEnemyList[0].Die();
            }
        }
    }
}