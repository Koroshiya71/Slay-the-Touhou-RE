using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleType
{
    Normal,//��ͨս��
    Elite,//��Ӣս��
    Boss,//Bossս
}
public class BattleData
{
    //ս������
    private BattleType battleType;
    //ս��ID
    private int battleID;
    //����ID�б�
    private List<int> enemyIDList=new List<int>();


    public BattleData(int id)
    {

    }
}
