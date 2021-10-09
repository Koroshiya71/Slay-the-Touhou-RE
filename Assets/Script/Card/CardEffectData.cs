using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectData
{
    //Ч��ID
    private int effectID;
    //Ч������
    private string effectDes;
    /// <summary>
    /// ����
    /// </summary>
    public int EffectID => effectID;

    public string EffectDes => effectDes;

    public CardEffectData(int id)
    {
        effectID = id;
        effectDes = ReadCardEffectData("EffectDes", id);
    }

    //����cfg���ݱ��ȡ��������
    private string ReadCardEffectData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicCardEffect);
        return data;
    }
}
