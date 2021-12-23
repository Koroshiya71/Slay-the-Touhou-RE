using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlayer : BaseBattleUnit
{
    //同步玩家
    public static SyncPlayer Instance;

    protected override void Awake()
    {
        Init();
        Instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
