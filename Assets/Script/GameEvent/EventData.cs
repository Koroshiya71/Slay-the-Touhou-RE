using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    Battle,//ս��
    NormalEvent,//��ͨ�¼�
    SpecialEvent,//�����¼�
}
public class EventData
{
    //�¼�ID
    private int eventID;
    //�¼�����
    private EventType eventType;
    //�Ƿ��Ѿ�������
    private bool hasTriggered;
    //�Ƿ�����ظ�����
    private bool canRecall;
    //ǰ���¼�ID
    private int preEventID;

    public EventData(int ID)
    {
        eventID = ID;

    }
}
