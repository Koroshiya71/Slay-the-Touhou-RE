using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectData
{
    //效果ID
    private int effectID;
    //效果描述
    private string effectDes;
    /// <summary>
    /// 属性
    /// </summary>
    public int EffectID => effectID;

    public string EffectDes => effectDes;

    public CardEffectData(int id)
    {
        effectID = id;
        effectDes = ReadCardEffectData("EffectDes", id);
    }

    //根据cfg数据表读取敌人数据
    private string ReadCardEffectData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicCardEffect);
        return data;
    }
}
