using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseBattleUnit
{
    //����
    public static Player Instance;

    protected override void Awake()
    {
        Init();
        Instance = this;
    }

}
