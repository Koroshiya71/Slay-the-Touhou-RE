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


    /// <summary>
    /// ����
    /// </summary>
    public BattleType BattleType => battleType;

    public int BattleID => battleID;

    public List<int> EnemyIDList => enemyIDList;
    //����ID�Ĺ��캯��
    public BattleData(int id)
    {
        string tempStr="";
        battleID = int.Parse(ReadBattleCfgData("ID", id));
        tempStr = ReadBattleCfgData("BattleType", id);
        switch (tempStr)
        {
            case "��ͨս��":
                battleType = BattleType.Normal;
                break;
        }

        for (int i = 1; i <= int.Parse(ReadBattleCfgData("EnemyNum", id)); i++)
        {
            enemyIDList.Add(int.Parse(ReadBattleCfgData("EnemyID"+i,id)));
        }
    }
    //����cfg���ݱ��ȡ��������
    private string ReadBattleCfgData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicBattleData);
        return data;
    }
}
