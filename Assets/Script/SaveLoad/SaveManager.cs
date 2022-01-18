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
    //玩家数据
    public PlayerData playerData;

    //牌组列表
    public List<CardData> cardDataList = new List<CardData>();

    //地图场景类型列表
    public List<SceneType> sceneTypeList = new List<SceneType>();

    //当前层数
    public int mapLayer;

    //上次选择的地址
    public int mapIndex;

    //构造函数
    public SaveData(bool isSave)
    {
        playerData = GameManager.Instance.playerData;
        foreach (var data in DeskManager.Instance.deskCardList)
        {
            cardDataList.Add(data);
        }

        foreach (var sceneType in GameSceneManager.Instance.inGameSceneList)
        {
            sceneTypeList.Add(sceneType.sceneData.SceneType);
        }

        mapLayer = GameSceneManager.Instance.currentLayer;
        mapIndex = GameSceneManager.Instance.lastIndex;
    }

    public SaveData()
    {
    }
}

public class SaveManager : UnitySingleton<SaveManager>
{
    //是否是通过读取存档初始化游戏
    public static bool isLoad = false;

    public static string jsonDataPath;
    //Json数据保存路径

    //存档数据
    public SaveData saveData = new SaveData();

    private void Awake()
    {
#if UNITY_EDITOR
        jsonDataPath = "Assets/StreamingAssets/Json/";
#else 
        jsonDataPath = "Slay the Touhou_Data/StreamingAssets/Json/";
#endif
    }

    void Start()
    {
    }

    //游戏存档
    public static void SaveGame()
    {
        string s = JsonConvert.SerializeObject(new SaveData(true), Formatting.Indented, new JsonSerializerSettings { });
        GameTool.SetString("HasSave", "True");
        StreamWriter writer = new StreamWriter(jsonDataPath + "SaveData.json");
        writer.Write(s);
        writer.Close();
        Debug.Log("保存游戏");
    }

    //读取存档数据
    public void LoadData()
    {
        if (GameTool.GetString("HasSave")!="True")
        {
            Debug.Log("没有存储数据");
            return;
        }
        isLoad = true;
        StreamReader reader = new StreamReader(jsonDataPath + "SaveData.json");
        saveData = JsonConvert.DeserializeObject<SaveData>(reader.ReadToEnd());
        reader.Close();
        GameManager.Instance.playerData = saveData.playerData;
        EventDispatcher.TriggerEvent(E_MessageType.UpdateGameMainUI);
    }

    void Update()
    {
    }
}