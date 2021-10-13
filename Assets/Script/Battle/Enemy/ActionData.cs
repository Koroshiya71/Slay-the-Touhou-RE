using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public enum ActionType
{
    Attack, //����
    AttackAndWeaken, //�������������
    AttackAndBuff, //�������ṩǿ��
    Buff, //ǿ����ظ�
    Weaken, //����
    Shield //��ȡ����
}

public class ActionData
{
    //��ΪID
    private int actionID;

    //��Ϊ����
    private ActionType actionType;
    //��Ϊ����
    private string actionDes;
    //�ж�Ч��ֵ
    private int actionValue;

    /// <summary>
    /// ����
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
    
    //���캯��(�ж�ID���ж�ֵ)
    public ActionData(int id,int value)
    {
        actionID = id;
        actionDes = ReadActionCfgData("ActionEffectDes", id);
        actionValue = value;
        string tempStr = ReadActionCfgData("ActionType", id);
        switch (tempStr)
        {
            case "����":
                actionType = ActionType.Attack;
                break;
        }
    }

    //����cfg���ݱ��ȡ�����ж�����
    private string ReadActionCfgData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicActionData);
        return data;
    }
}