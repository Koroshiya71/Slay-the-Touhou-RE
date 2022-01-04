using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
//存档数据
[Serializable]
public class SaveData
{
    //当前生命值
    public int currentHp;
    //最大生命值
    public int maxHp;
    //牌组列表
    public List<CardData> cardDataList = new List<CardData>();
    //地图场景类型列表
    public List<SceneType> sceneTypeList = new List<SceneType>();
    //构造函数
    public SaveData(bool isSave)
    {
        currentHp = GameManager.Instance.playerData.currentHp;
        maxHp = GameManager.Instance.playerData.maxHp;
        foreach (var data in DeskManager.Instance.deskCardList)
        {
            cardDataList.Add(data);
        }
        foreach (var sceneType in GameSceneManager.Instance.inGameSceneList)
        {
            sceneTypeList.Add(sceneType.sceneData.SceneType);
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
        string s = JsonConvert.SerializeObject(new SaveData(true));
        GameTool.SetString("HasSave", "True");
        StreamWriter writer = new StreamWriter("./Assets/Resources/Json/SaveData.json");
        writer.Write(s);
        writer.Close();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame();
        }
    }
}
