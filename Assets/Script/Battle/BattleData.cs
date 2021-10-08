using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleType
{
    Normal,//普通战斗
    Elite,//精英战斗
    Boss,//Boss战
}
public class BattleData
{
    //战斗类型
    private BattleType battleType;
    //战斗ID
    private int battleID;

    //敌人ID列表
    private List<int> enemyIDList=new List<int>();


    /// <summary>
    /// 属性
    /// </summary>
    public BattleType BattleType => battleType;

    public int BattleID => battleID;

    public List<int> EnemyIDList => enemyIDList;
    //根据ID的构造函数
    public BattleData(int id)
    {
        string tempStr="";
        battleID = int.Parse(ReadBattleCfgData("ID", id));
        tempStr = ReadBattleCfgData("BattleType", id);
        switch (tempStr)
        {
            case "普通战斗":
                battleType = BattleType.Normal;
                break;
        }

        for (int i = 1; i <= int.Parse(ReadBattleCfgData("EnemyNum", id)); i++)
        {
            enemyIDList.Add(int.Parse(ReadBattleCfgData("EnemyID"+i,id)));
        }
    }
    //根据cfg数据表读取敌人数据
    private string ReadBattleCfgData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicBattleData);
        return data;
    }
}
