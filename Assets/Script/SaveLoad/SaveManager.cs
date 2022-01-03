using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using Newtonsoft.Json;
//�浵����
[Serializable]
public class SaveData
{
    //��ǰ����ֵ
    public int currentHp;
    //�������ֵ
    public int maxHp;
    //����Data�б�
    public List<CardData> cardDataList = new List<CardData>();
    //���캯��
    public SaveData(bool fromGame)
    {
        currentHp = Player.Instance.currentHp;
        maxHp = Player.Instance.maxHp;
        foreach (var data in DeskManager.Instance.deskCardList)
        {
            cardDataList.Add(data);
        }
    }

    public SaveData()
    {

    }
}
public class SaveManager : UnitySingleton<SaveManager>
{
    

    void Start()
    {
        
    }

    public static void SaveGame()
    {
        string s = JsonConvert.SerializeObject(new CardData(1001));
        Debug.Log(s);
        CardData data = JsonConvert.DeserializeObject<CardData>(s);
        Debug.Log(data.cardName);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame();
        }
    }
}
