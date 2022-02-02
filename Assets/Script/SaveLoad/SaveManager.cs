using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

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

    //遗物ID列表
    public List<int> relicIDList = new List<int>();

    
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
        foreach (var id in RelicManager.Instance.playerRelicDic.Keys)
        {
            relicIDList.Add(id);
        }

    }

    public SaveData()
    {
    }
}

public class SaveManager : UnitySingleton<SaveManager>
{
    //是否是通过读取存档初始化游戏
    public static bool isLoad = false;
    //Json数据保存路径
    public static string jsonDataPath;
    //是否离线读取
    public bool isOffline = false;
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
        //如果已连接游戏
        if (NetManager.socket.Connected&&!isOffline)
        {
            Debug.Log(NetManager.playerDataStr);
            if (NetManager.playerDataStr== "empty")
            {
                Debug.Log("没有存储数据");
                return;
            }
            saveData = JsonConvert.DeserializeObject<SaveData>(NetManager.playerDataStr);
            GameManager.Instance.playerData = saveData.playerData;
            Debug.Log(GameManager.Instance.playerData);
            Debug.Log(saveData.playerData);
            EventDispatcher.TriggerEvent(E_MessageType.UpdateGameMainUI);
            isLoad = true;
        }
        //如果是离线游戏
        else
        {
            StreamReader reader = new StreamReader(jsonDataPath + "SaveData.json");
            string saveStr = reader.ReadToEnd();
            if (saveStr.Length <= 0)
            {
                Debug.Log("没有存储数据");
                reader.Close();
                return;
            }

            saveData = JsonConvert.DeserializeObject<SaveData>(saveStr);

            Debug.Log(GameManager.Instance.playerData);

            reader.Close();
            GameManager.Instance.playerData = saveData.playerData;
            EventDispatcher.TriggerEvent(E_MessageType.UpdateGameMainUI);
            isLoad = true;
        }

        Debug.Log(saveData.relicIDList.Count);

    }
    //列表深拷贝
    public static List<T> Clone<T>(object List)
    {
        using (Stream objectStream = new MemoryStream())
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(objectStream,formatter);
            objectStream.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(objectStream) as List<T>;
        }
    }
    void Update()
    {
    }
}