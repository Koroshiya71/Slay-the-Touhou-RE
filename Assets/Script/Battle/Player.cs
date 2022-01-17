using System.Collections;
using System.Collections.Generic;
using GameCore;
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
    
    //覆写更新UI方法
    public override void UpdateUI()
    {
        base.UpdateUI();
        //更新主界面UI
        GameManager.Instance.playerData.currentHp = currentHp;
        GameManager.Instance.playerData.maxHp = maxHp;
        EventDispatcher.TriggerEvent(E_MessageType.UpdateGameMainUI);
    }
}

