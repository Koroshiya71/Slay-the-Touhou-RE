using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    Battle,//战斗
    NormalEvent,//普通事件
    SpecialEvent,//特殊事件
}
public class EventData
{
    //事件ID
    private int eventID;
    //事件类型
    private EventType eventType;
    //是否已经触发过
    private bool hasTriggered;
    //是否可以重复触发
    private bool canRecall;
    //前置事件ID
    private int preEventID;

    public EventData(int ID)
    {
        eventID = ID;

    }
}
