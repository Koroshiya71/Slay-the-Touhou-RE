using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

//卡牌强化需求类型
public enum NeedEffectType
{
    
    Basic,//护盾、伤害
    Any,//任意
    Buff,//效果
    Basic_Buff,//效果和护盾、伤害
}

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
//休息处数据
[Serializable]
public class LoungeData
{
    //最大休息处时间
    public int maxLoungeTime = 12;
    //当前休息处时间
    public int loungeTime = 12;
    //休息消耗时间
    [FormerlySerializedAs("restTime")] public int restCost = 6;
    //研习消耗时间
    [FormerlySerializedAs("studyTime")] public int studyCost = 6;

    public LoungeData()
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
//卡牌强化数据
public class CardBuffData
{
    //强化ID
    public int buffId;
    //强化描述
    public string buffDes;
    //需要的卡牌类型
    public NeedEffectType needEffectType;

    public CardBuffData()
    {

    }
}
public class GameManager : UnitySingleton<GameManager>
{
    //玩家数据
    public PlayerData playerData = new PlayerData();
    //同步玩家数据
    public PlayerData syncPlayerData = new PlayerData();
    
    //卡牌出货率
    public CardRareClass cardRare;

    //战斗结束选牌数
    public int chooseCardNumAfterBattle = 3;

    //商店卡牌ID、价格字典
    public Dictionary<int, int> storeCardPriceDic = new Dictionary<int, int>();
    //商店遗物ID、价格字典
    public Dictionary<int, int> storeRelicPriceDic = new Dictionary<int, int>();

    //卡牌强化列表id，强化data
    public List<CardBuffData> cardBuffList = new List<CardBuffData>();

    //休息处数据
    public LoungeData loungeData = new LoungeData();
    //是否是多人游戏
    public bool isMulti = false;
    private void Awake()
    {
        StreamReader reader;

        if (!SaveManager.isLoad)
        {
            //读取玩家初始数值json
            reader = new StreamReader(SaveManager.jsonDataPath + "PlayerInit.json");
            playerData = JsonConvert.DeserializeObject<PlayerData>(reader.ReadToEnd());
            //如果是多人游戏，
            if (isMulti)
            {
                reader = new StreamReader(SaveManager.jsonDataPath + "PlayerInit.json");
                syncPlayerData = JsonConvert.DeserializeObject<PlayerData>(reader.ReadToEnd());
            }
            reader.Close();
        }
        
        //读取卡牌掉落稀有度配置
        reader = new StreamReader(SaveManager.jsonDataPath + "RareCard.json");
        cardRare = JsonConvert.DeserializeObject<CardRareClass>(reader.ReadToEnd());

        //读取商店数据
        reader = new StreamReader(SaveManager.jsonDataPath + "StoreCard.json");
        storeCardPriceDic = JsonConvert.DeserializeObject<Dictionary<int, int>>(reader.ReadToEnd());

        reader = new StreamReader(SaveManager.jsonDataPath + "StoreRelic.json");
        storeRelicPriceDic = JsonConvert.DeserializeObject<Dictionary<int, int>>(reader.ReadToEnd());

        //读取卡牌强化
        reader = new StreamReader(SaveManager.jsonDataPath + "CardBuff.json");
        cardBuffList = JsonConvert.DeserializeObject<List<CardBuffData>>(reader.ReadToEnd());
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
    //消费金币
    public void Pay(int goldNum)
    {
        playerData.gold -= goldNum;
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

        if (Input.GetKeyDown(KeyCode.B)) //解锁Boss战
        {
            GameSceneManager.Instance.bossScene.ChangeGameSceneState(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))//登录Koroshiya
        {
            MsgLogin msg = new MsgLogin();
            msg.id = "Koroshiya";
            msg.pw = "123123";
            NetManager.Send(msg);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))//登录Koroshiya71
        {
            MsgLogin msg = new MsgLogin();
            msg.id = "Koroshiya71";
            msg.pw = "123123";
            NetManager.Send(msg);
        }
        //手动调用
        NetManager.NetUpdate();

    }
    //休息处休息方法
    public void LoungeRest()
    {
        //如果时间足够则消耗时间并回复生命值
        if (loungeData.loungeTime >= loungeData.restCost)
        {
            Player.Instance.Heal((int)(0.3f * Player.Instance.maxHp));
            loungeData.loungeTime -= loungeData.restCost;
            //更新UI显示
            EventDispatcher.TriggerEvent(E_MessageType.ShowLoungeUI);
        }
    }
    //休息处强化卡牌方法
    public void LoungeBuffCard()
    {
        //如果时间足够则消耗时间并回复生命值
        if (loungeData.loungeTime >= loungeData.studyCost)
        {
            //选牌
            StartCoroutine(DeskManager.Instance.BuffCardCoroutine());
            loungeData.loungeTime -= loungeData.studyCost;
            //更新UI显示
            EventDispatcher.TriggerEvent(E_MessageType.ShowLoungeUI);
        }
    }
    //游戏结束时自动断开连接
    private void OnApplicationQuit()
    {
        NetManager.Close();
    }

    
}