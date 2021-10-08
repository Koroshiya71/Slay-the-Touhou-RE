using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��������
public enum EnemyType
{
    Normal,//��ͨ����
    Elite,//��Ӣ����
    Boss,//Boss����
}
public class EnemyData
{
    //����ID
    private int enemyID;
    //��������
    private string enemyName;
    //�ز�·��
    private string resourcePath;
    //�������ֵ
    private int maxHp;
    //��ʼ����ֵ
    private int initShield;
    //������Ϊ�ֵ�<ID,Value>
    private Dictionary<int, int> enemyActionDic = new Dictionary<int, int>();

    //�����ж�˳���б�
    private List<string> actionModeList = new List<string>();

    //��������
    private EnemyType enemyType;


    /// <summary>
    /// ����
    /// </summary>
    public int EnemyID => enemyID;

    public string EnemyName => enemyName;

    public string ResourcePath => resourcePath;

    public int MAXHp => maxHp;

    public int InitShield => initShield;

    public Dictionary<int, int> EnemyActionDic => enemyActionDic;

    public List<string> ActionModeList => actionModeList;

    public EnemyType EnemyType => enemyType;

    //���ݵ���ID�Ĺ��캯��
    public EnemyData(int enemyID)
    {
        //���ڽ��ն�ȡ�������ʱ�ַ���
        string tempStr = "";
        this.enemyID = enemyID;
        enemyName = ReadEnemyCfgData("Name", enemyID);
        resourcePath = ReadEnemyCfgData("ResourcePath", enemyID);
        maxHp = int.Parse(ReadEnemyCfgData("MaxHp", enemyID));
        initShield = int.Parse(ReadEnemyCfgData("InitShield", enemyID));
        //���ݵ��˵��ж�����ӵ��ж��ֵ���
        for (int i = 1; i <= int.Parse(ReadEnemyCfgData("ActNum", enemyID)); i++)
        {
            enemyActionDic.Add(int.Parse(ReadEnemyCfgData("Act"+i+"ID" , enemyID)),
                int.Parse(ReadEnemyCfgData("Act"+i+"Value", enemyID)));
        }
        //���ݵ��˵��ж��߼�������ӵ��ж��߼��б�
        for (int i = 1; i <= int.Parse(ReadEnemyCfgData("ActModeNum", enemyID)); i++)
        {
            actionModeList.Add(ReadEnemyCfgData("ActMode" + i, enemyID));
        }
    }

    //����cfg���ݱ��ȡ��������
    private string ReadEnemyCfgData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicEnemyData);
        return data;
    }
}
