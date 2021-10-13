using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public enum ActionType
{
    Attack, //攻击
    AttackAndWeaken, //攻击并削弱玩家
    AttackAndBuff, //攻击并提供强化
    Buff, //强化或回复
    Weaken, //削弱
    Shield //获取护甲
}

public class ActionData
{
    //行为ID
    private int actionID;

    //行为类型
    private ActionType actionType;
    //行为描述
    private string actionDes;
    //行动效果值
    private int actionValue;

    /// <summary>
    /// 属性
    /// </summary>
    public int ActionValue => actionValue;
    public string ActionDes => actionDes;
    public int ActionID
    {
        get => actionID;
    }

    public ActionType ActionType
    {
        get => actionType;
    }
    
    //构造函数(行动ID，行动值)
    public ActionData(int id,int value)
    {
        actionID = id;
        actionDes = ReadActionCfgData("ActionEffectDes", id);
        actionValue = value;
        string tempStr = ReadActionCfgData("ActionType", id);
        switch (tempStr)
        {
            case "攻击":
                actionType = ActionType.Attack;
                break;
        }
    }

    //根据cfg数据表读取敌人行动数据
    private string ReadActionCfgData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicActionData);
        return data;
    }
}