using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseBattleUnit
{

    //玩家单例
    public static Player Instance;
    //初始化玩家数据
    public void InitPlayerByData()
    {
        maxHp = GameManager.Instance.playerData.maxHp;
        currentHp = GameManager.Instance.playerData.currentHp;
    }
    protected override void Awake()
    {
        InitPlayerByData();
        Init();
        Instance = this;
        isPlayer = true;
    }

}

