using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;
//状态类型
public enum StateType
{
    //增益
    Buff,
    //减益
    Debuff
}
//状态数据
public class StateData
{
    //状态ID
    public int stateID;
    //状态类型
    public StateType stateType;
    //状态名称
    public string stateName;
    //状态描述
    public string stateDes;
    //状态图片
    public Sprite stateSprite;
    //状态层数
    public int stateStack;
    //是否每回合递减
    public bool reduceOverTurn;
    //是否显示层数
    public bool showStack;
    //根据cfg数据表读取敌人数据
    private string ReadStateCfgData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicStateData);
        return data;
    }
    //状态数据构造函数
    public StateData(int id, int stack)
    {
        stateID = id;
        stateName = ReadStateCfgData("Name", id);
        stateDes = ReadStateCfgData("StateDes", id);
        stateSprite = ResourcesManager.Instance.LoadResources<Sprite>(ReadStateCfgData("ResourcePath", id));
        stateStack = stack;
        string typeStr = ReadStateCfgData("StateType", id);
        switch (typeStr)
        {
            case "Buff":
                stateType = StateType.Buff;
                break;
            case "Debuff":
                stateType = StateType.Debuff;
                break;

        }
        reduceOverTurn = ReadStateCfgData("ReduceOverTime", id) == "1";
        showStack = ReadStateCfgData("ShowStack", id) == "1";
    }
}

public class StateManager : UnitySingleton<StateManager>
{
    //状态图标预制体
    public static GameObject stateObj;
    //二刀是否有触发
    public bool hasDoubleBlade=false;
    //给玩家附加状态(状态ID，层数)
    public static void AddStateToTarget(BaseBattleUnit target, int id, int stack)
    {
        var newState = Instantiate(stateObj).GetComponent<State>();
        var newData = newState.stateData = new StateData(id, stack);

        //判断是否已有该状态
        if (!target.stateDic.ContainsKey(id))
        {
            //如果没有，则新增一个状态到字典中

            //获取Sprite并设定父物体、位置、缩放
            newState.GetComponent<Image>().sprite = newData.stateSprite;
            newState.transform.SetParent(target.transform);
            newState.transform.localScale = new Vector3(1, 1, 1);

            if (target.isPlayer)
            {
                newState.transform.localPosition = new Vector3(-35 + target.stateDic.Count % 5 * 20, -55 - 20 * (target.stateDic.Count / 5));
            }
            else
            {
                newState.transform.localPosition = new Vector3(-40 + target.stateDic.Count % 5 * 20, -55 - 20 * (target.stateDic.Count / 5));
            }
            //添加到玩家状态字典
            target.stateDic.Add(newData.stateID, newState);
            //更新其UI状态
            newState.UpdateStateUI();
        }
        else
        {
            //如果已有，则直接叠加层数即可
            target.stateDic[id].stateData.stateStack += stack;
            //更新其UI状态
            target.stateDic[id].UpdateStateUI();
        }
        BattleManager.Instance.UpdateCardAndActionValue();
    }

    //检查目标身上是否有该状态
    public static bool CheckState(BaseBattleUnit target, int id)
    {
        //二刀检测
        if (id==1006&& target.stateDic.ContainsKey(id))
        {
            if (Instance.hasDoubleBlade)
            {
                return false;
            }

            Debug.Log("触发二刀");
            Instance.hasDoubleBlade = true;
        }
        return target.stateDic.ContainsKey(id);
    }

    private void Awake()
    {
        stateObj = ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/" + "Battle/" + "State");
    }
    void Start()
    {
    }

    void Update()
    {

    }
}
