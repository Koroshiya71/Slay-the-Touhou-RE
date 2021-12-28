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
    public int parentPageID;
    //页面名称
    public string pageName;
    //页面图片素材路径
    public string resourcePath;
    //页面描述
    public string pageDes;
    
}
public class EventData
{
    //事件ID
    private int eventID;
    //事件名称
    private string eventName;
    //事件类型
    private EventType eventType;
    //是否已经触发过
    private bool hasTriggered;
    //是否可以重复触发
    private bool canRecall;
    //前置事件ID
    private int preEventID;

    //页面列表


    //构造函数
    public EventData(int ID)
    {
        eventID = ID;

    }
}


public class GameEventManager : UnitySingleton<GameEventManager>
{


    void Start()
    {

    }

    void Update()
    {

    }
}
