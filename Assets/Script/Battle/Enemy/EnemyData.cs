using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敌人类型
public enum EnemyType
{
    Normal,//普通敌人
    Elite,//精英敌人
    Boss,//Boss敌人
}
public class EnemyData
{
    //敌人ID
    private int enemyID;
    //敌人名称
    private string enemyName;
    //素材路径
    private string resourcePath;
    //最大生命值
    private int maxHp;
    //初始护盾值
    private int initShield;
    //敌人行为字典<ID,敌人行动对象>
    private Dictionary<int, ActionData> enemyActionDic = new Dictionary<int, ActionData>();
    //敌人行动顺序列表
    private List<string> actionModeList = new List<string>();
    //敌人战斗开始时的行动
    public List<ActionData> battleStartActionList = new List<ActionData>();
    //敌人类型
    private EnemyType enemyType;

    /// <summary>
    /// 属性
    /// </summary>
    public int EnemyID => enemyID;

    public string EnemyName => enemyName;

    public string ResourcePath => resourcePath;

    public int MAXHp => maxHp;

    public int InitShield => initShield;

    public Dictionary<int, ActionData> EnemyActionDic => enemyActionDic;

    public List<string> ActionModeList => actionModeList;

    public EnemyType EnemyType => enemyType;

    //根据敌人ID的构造函数
    public EnemyData(int enemyID)
    {
        
        //用于接收读取结果的临时字符串
        string tempStr = ReadEnemyCfgData("EnemyType", enemyID);
        this.enemyID = enemyID;
        enemyName = ReadEnemyCfgData("Name", enemyID);
        resourcePath = ReadEnemyCfgData("ResourcePath", enemyID);
        maxHp = int.Parse(ReadEnemyCfgData("MaxHp", enemyID));
        initShield = int.Parse(ReadEnemyCfgData("InitShield", enemyID));
        
        //根据敌人的行动添加到行动字典中
        for (int i = 1; i <= int.Parse(ReadEnemyCfgData("ActNum", enemyID)); i++)
        {
            int actionValue = int.Parse(ReadEnemyCfgData("Act" + i + "Value", enemyID));
            int actionID = int.Parse(ReadEnemyCfgData("Act" + i + "ID", enemyID));
            enemyActionDic.Add(actionID,new ActionData(actionID,actionValue));
        }
        //根据敌人的行动逻辑数量添加到行动逻辑列表
        for (int i = 1; i <= int.Parse(ReadEnemyCfgData("ActModeNum", enemyID)); i++)
        {
            actionModeList.Add(ReadEnemyCfgData("ActMode" + i, enemyID));
        }
        //将开始战斗行动添加到列表中
        int startActionId = int.Parse(ReadEnemyCfgData("ActionStart", enemyID));
        int startActionValue = int.Parse(ReadEnemyCfgData("ActionStartValue", enemyID));
        if (startActionId!=-1)
        {
            battleStartActionList.Add(new ActionData(startActionId,startActionValue));
        }
        //设置敌人类型
        switch (tempStr)
        {
            case "普通":
                enemyType = EnemyType.Normal;
                break;
        }
    }

    //根据cfg数据表读取敌人数据
    private string ReadEnemyCfgData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicEnemyData);
        return data;
    }
}
