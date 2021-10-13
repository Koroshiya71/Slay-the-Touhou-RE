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
    //������Ϊ�ֵ�<ID,�����ж�����>
    private Dictionary<int, ActionData> enemyActionDic = new Dictionary<int, ActionData>();

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

    public Dictionary<int, ActionData> EnemyActionDic => enemyActionDic;

    public List<string> ActionModeList => actionModeList;

    public EnemyType EnemyType => enemyType;

    //���ݵ���ID�Ĺ��캯��
    public EnemyData(int enemyID)
    {
        //���ڽ��ն�ȡ�������ʱ�ַ���
        string tempStr = ReadEnemyCfgData("EnemyType", enemyID);
        this.enemyID = enemyID;
        enemyName = ReadEnemyCfgData("Name", enemyID);
        resourcePath = ReadEnemyCfgData("ResourcePath", enemyID);
        maxHp = int.Parse(ReadEnemyCfgData("MaxHp", enemyID));
        initShield = int.Parse(ReadEnemyCfgData("InitShield", enemyID));

        //���ݵ��˵��ж���ӵ��ж��ֵ���
        for (int i = 1; i <= int.Parse(ReadEnemyCfgData("ActNum", enemyID)); i++)
        {
            int actionValue = int.Parse(ReadEnemyCfgData("Act" + i + "Value", enemyID));
            int actionID = int.Parse(ReadEnemyCfgData("Act" + i + "ID", enemyID));
            enemyActionDic.Add(actionID,new ActionData(actionID,actionValue));
        }
        //���ݵ��˵��ж��߼�������ӵ��ж��߼��б�
        for (int i = 1; i <= int.Parse(ReadEnemyCfgData("ActModeNum", enemyID)); i++)
        {
            actionModeList.Add(ReadEnemyCfgData("ActMode" + i, enemyID));
        }

        //���õ�������
        switch (tempStr)
        {
            case "��ͨ":
                enemyType = EnemyType.Normal;
                break;
        }
    }

    //����cfg���ݱ��ȡ��������
    private string ReadEnemyCfgData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicEnemyData);
        return data;
    }
}
