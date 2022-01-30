using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public enum ActionType
{
    Attack, //攻击
    AttackAndWeaken, //攻击并削弱玩家
    AttackAndBuff, //攻击并提供强化
    Buff, //强化或回复
    Weaken, //削弱
    Shield,//获取护甲
    Special,//特殊
}
public class ActionData
{
    //行为ID
    private int actionID;

    //行为类型
    private ActionType actionType;
    //行为描述
    private string actionDes;

    //是否需要目标
    private bool needTarget;

    /// <summary>
    /// 属性
    /// </summary>
    public List<int> ActionValue;
    public string ActionDes => actionDes;
    public bool NeedTarget => needTarget;
    public List<int> actualValue;
    public int ActionID
    {
        get => actionID;
    }

    public ActionType ActionType
    {
        get => actionType;
    }

    //构造函数(行动ID，行动值)
    public ActionData(int id, List<int> value)
    {
        actionID = id;
        actionDes = ReadActionCfgData("ActionEffectDes", id);
        ActionValue = DeepCopy.Copy(value)as List<int>;
        actualValue = DeepCopy.Copy(value) as List<int>;
        string tempStr = ReadActionCfgData("ActionType", id);
        switch (tempStr)
        {
            case "攻击":
                actionType = ActionType.Attack;
                break;
            case "强化":
                actionType = ActionType.Buff;
                break;
            case "护甲":
                actionType = ActionType.Shield;
                break;
            case "特殊":
                actionType = ActionType.Special;
                break;
            case "削弱":
                actionType = ActionType.Weaken;
                break;
        }
        tempStr = ReadActionCfgData("NeedTarget", id);
        switch (tempStr)
        {
            case "1":
                needTarget = true;
                break;
            case "0":
                needTarget = false;
                break;

        }
    }

    public ActionData()
    {

    }
    //根据cfg数据表读取敌人行动数据
    private string ReadActionCfgData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicActionData);
        return data;
    }
}