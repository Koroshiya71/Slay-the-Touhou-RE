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


    public BattleData(int id)
    {

    }
}
