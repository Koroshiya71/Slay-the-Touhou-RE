using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Random = UnityEngine.Random;

public class RelicData
{
    //遗物ID
    public int relicID;
    //遗物描述
    public string relicDes;
    //遗物计数
    public int relicCount;
    //遗物图片路径
    public string relicSpriteRes;
    //是否要显示计数
    public bool showCount;

    //构造函数
    public RelicData()
    {

    }
}
public class RelicManager : UnitySingleton<RelicManager>
{
    //遗物数据总列表
    public List<RelicData> relicDataList = new List<RelicData>();
    //遗物数据字典(id,遗物数据)
    public Dictionary<int, RelicData> relicDic = new Dictionary<int, RelicData>();
    //玩家已有遗物字典
    public Dictionary<int, RelicData> playerRelicDic = new Dictionary<int, RelicData>();
    //遗物预制体
    public GameObject relicPrefab;
    //遗物Content
    public GameObject relicContent;
    private void Awake()
    {
        EventDispatcher.AddListener(E_MessageType.GameStart,InitRelicManager);
    }

    //获取遗物
    public void GetRelic(int id)
    {
        //创建物体
        GameObject newRelicObj = Instantiate(relicPrefab);
        Relic newRelic = newRelicObj.GetComponent<Relic>();
        newRelicObj.transform.SetParent(relicContent.transform);
        newRelicObj.transform.localScale = new Vector3(1, 1);
        newRelic.InitRelic(relicDic[id]);
        //添加到玩家字典
        playerRelicDic.Add(id,relicDic[id]);
        //触发获取事件
        OnGetRelic(id);
    }
   //检测是否拥有某遗物
   public bool CheckRelic(int id)
   {
       return playerRelicDic.ContainsKey(id);
   }
   //获取遗物效果
   public void OnGetRelic(int id)
   {
       switch (id)
       {
            case 1002:
                //确认卡牌列表
                List<CardData> chooseList = new List<CardData>();
                while (chooseList.Count<3)
                {
                    CardData newCard=DeskManager.Instance.allCardDataList[Random.Range(0,DeskManager.Instance.allCardDataList.Count)];
                    if (newCard.cardRare==CardRare.Epic&&!chooseList.Contains(newCard))
                    {
                        chooseList.Add(newCard);
                    }
                }
                //开始选牌
                StartCoroutine(DeskManager.Instance.ChooseCardAddToDesk(1, chooseList));
                break;
       }
   }
    //读取Json文件初始化遗物数据列表和对应字典
    public void InitRelicManager()
    {
        //读取列表
        StreamReader reader = new StreamReader(SaveManager.jsonDataPath + "relic.json");
        relicDataList = JsonConvert.DeserializeObject<List<RelicData>>(reader.ReadToEnd());
        reader.Close();
        //初始化字典
        foreach (var relic in relicDataList)
        {
            relicDic.Add(relic.relicID,relic);
        }
        //获取遗物预制体
        relicPrefab = ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/Battle/Relic");
        //获取遗物content
        relicContent = GameObject.Find("RelicContent");
        //获得初始遗物
        GetRelic(1004);
    }
    //检测各种遗物的战斗开始效果
    public void CheckRelicBattleStartEffect()
    {
        //⑨的人偶
        if (CheckRelic(1001))
        {
            BattleManager.Instance.turnStartEffectDelegate += delegate
            {
                foreach (var enemy in BattleManager.Instance.inBattleEnemyList)
                {
                    enemy.TakeDamage(9,Player.Instance);
                }
            };
        }
    }
}
