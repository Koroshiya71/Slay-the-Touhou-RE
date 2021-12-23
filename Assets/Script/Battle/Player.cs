using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseBattleUnit
{
    //玩家单例
    public static Player Instance;
    public PlayerData playerData;
    protected override void Awake()
    {
        Init();
        Instance = this;
    }
    //玩家状态字典

}
//玩家数据
public class PlayerData
{

}
