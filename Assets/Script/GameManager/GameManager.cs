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
public class GameManager : UnitySingleton<GameManager>
{
    //玩家数据
    public PlayerData playerData = new PlayerData();
    private void Awake()
    {
        StreamReader reader = new StreamReader("./Assets/Resources/Json/PlayerInit.json");
        playerData = JsonConvert.DeserializeObject<PlayerData>(reader.ReadToEnd());
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
