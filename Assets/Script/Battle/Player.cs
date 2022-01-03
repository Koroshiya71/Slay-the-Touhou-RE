using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseBattleUnit
{
    //玩家单例
    public static Player Instance;
    protected override void Awake()
    {
        Init();
        Instance = this;
        isPlayer = true;
    }

}

