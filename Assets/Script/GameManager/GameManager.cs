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
    public int cold = 99;
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
    public List<int> eliteCombatRare = new List<int>() { 20, 50, 30 };
    //Boss战出货率（普通、稀有、史诗）
    public List<int> bossCombatRare = new List<int>() { 0, 0, 100 };

    CardRareClass()
    {

    }
}
public class GameManager : UnitySingleton<GameManager>
{
    //玩家数据
    public PlayerData playerData = new PlayerData();
    private void Awake()
    {
        StreamReader reader = new StreamReader(SaveManager.jsonDataPath+"PlayerInit.json");
        playerData = JsonConvert.DeserializeObject<PlayerData>(reader.ReadToEnd());
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
