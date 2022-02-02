using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class SyncPlayer : BaseBattleUnit
{
    //同步玩家
    public static SyncPlayer Instance;

    //初始化玩家数据
    public void InitPlayerByData()
    {
        maxHp = GameManager.Instance.syncPlayerData.maxHp;
        currentHp = GameManager.Instance.syncPlayerData.currentHp;
    }
    protected override void Awake()
    {
        InitPlayerByData();
        Init();
        Instance = this;
        isPlayer = true;
    }

    //覆写更新UI方法
    public override void UpdateUI()
    {
        base.UpdateUI();
        //更新主界面UI
        GameManager.Instance.syncPlayerData.currentHp = currentHp;
        GameManager.Instance.syncPlayerData.maxHp = maxHp;
        EventDispatcher.TriggerEvent(E_MessageType.UpdateGameMainUI);
    }
}
