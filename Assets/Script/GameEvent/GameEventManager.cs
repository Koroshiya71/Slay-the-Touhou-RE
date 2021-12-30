using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;


public enum EventType
{
    Battle,//战斗
    NormalEvent,//普通事件
    SpecialEvent,//特殊事件
}
//事件页面数据
public class EventPageData
{
    //页面ID
    public int pageID;
    //页面所属的事件ID
    public int parentEventID;
    //所属事件名称
    public string parentEventName;
    //页面名称
    public string pageName;
    //页面图片素材路径
    public string resourcePath;
    //页面描述
    public string pageDes;

    //选项数量
    public int choiceNum;
    //选项结果列表
    public List<EventResultData> resultList = new List<EventResultData>();

    //根据ID构造
    public EventPageData(int id)
    {
        pageID = id;
        parentEventID = int.Parse(ReadCfgEventData("EventID", id));
        parentEventName = ReadCfgEventData("EventName", id);
        pageName = ReadCfgEventData("PageName", id);
        resourcePath = ReadCfgEventData("ResourcePath", id);
        pageDes = ReadCfgEventData("PageDes", id);
        choiceNum = int.Parse(ReadCfgEventData("ChoiceNum", id));

        //读取页面配置
        for (int i = 0; i < choiceNum; i++)
        {

            string result = ReadCfgEventData("Result" + (i + 1), id);
            string[] results = result.Split(';');

            //根据字符串初始化
            EventResultData newData = new EventResultData(results[0], int.Parse(results[1]), int.Parse(results[2]), int.Parse(results[3]));
            resultList.Add(newData);
        }

    }
    //读取数据
    private string ReadCfgEventData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicEventData);
        return data;
    }
}
public class EventData
{
    //事件ID
    public int eventID;
    //事件名称
    public string eventName;
    //事件类型
    public EventType eventType;
    //是否已经触发过
    public bool hasTriggered;
    //是否可以重复触发
    public bool canRecall;
    //前置事件ID
    public int preEventID;
    //页面列表
    public List<EventPageData> pageDataList = new List<EventPageData>();
    //构造函数
    public EventData(int ID)
    {
        eventID = ID;

    }
}

public class EventResultData
{
    //效果ID
    public int effectID;
    //效果值
    public int effectValue;
    //选择描述
    public string choiceDes;
    //下个页面ID
    public int nextPageID;
    //构造函数
    public EventResultData(string des, int nextPage, int effect, int value)
    {
        choiceDes = des;
        nextPageID = nextPage;
        effectID = effect;
        effectValue = value;
    }
}
public class GameEventManager : UnitySingleton<GameEventManager>
{
    //页面字典<id,页面数据>
    public Dictionary<int, EventPageData> eventPageDic = new Dictionary<int, EventPageData>();
    //事件字典<id，事件数据>
    public Dictionary<int, EventData> eventDic = new Dictionary<int, EventData>();

    //初始化事件管理器
    public void InitGameEventManager()
    {
        //初始化所有页面和事件数据
        foreach (var pageData in DataController.Instance.dicEventData["ID"])
        {


            int id = int.Parse(pageData.Key);
            //将所有页面数据添加到页面字典
            if (id != -1)
            {
                EventPageData newPageData = new EventPageData(id);
                //如果没有对应的父事件，则初始化对应的事件数据
                if (!eventDic.ContainsKey(newPageData.parentEventID))
                {
                    //TODO：这里不执行
                    //初始化事件数据
                    EventData newEvent = new EventData(newPageData.parentEventID);
                    newEvent.eventName = newPageData.parentEventName;
                    newEvent.pageDataList.Add(newPageData);
                    eventDic.Add(newEvent.eventID, newEvent);
                }
                //否则直接添加即可
                else
                {
                    eventDic[newPageData.pageID].pageDataList.Add(newPageData);
                }
                eventPageDic.Add(newPageData.pageID, newPageData);

            }
        }
    }
    void Start()
    {

    }

    //触发事件效果（效果ID，效果值）
    public void TriggerEventEffect(EventResultData data)
    {
        EventDispatcher.TriggerEvent < int，string> (E_MessageType.ShowEventPage, data.nextPageID, data.nextPageID);

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UIManager.Instance.ShowUI(E_UiId.EventUI);
            EventDispatcher.TriggerEvent < int，string> (E_MessageType.ShowEventPage, 10011, "");
        }
    }
    private void Awake()
    {
        EventDispatcher.AddListener(E_MessageType.GameStart, InitGameEventManager);
    }
    //读取结果描述数据
    private string ReadCfgEventData(string key, int id)
    {
        string data = DataController.Instance.ReadCfg(key, id, DataController.Instance.dicEventData);
        return data;
    }
}
