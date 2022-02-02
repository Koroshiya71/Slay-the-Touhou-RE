using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class MultiPlayMsgHandler : UnitySingleton<MultiPlayMsgHandler>
{
    //��ǰ���ߵĳ������Ͱ�
    public static SceneType chosenSceneType;
    //��ҵ�����Ӱ�ť
    public void ConnectServer()
    {
        NetManager.Connect("127.0.0.1", 8888);
    }
    //�ȴ�Э��ص�
    public void OnMsgMultiWait(MsgBase msgBase)
    {

        EventDispatcher.TriggerEvent(E_MessageType.MultiWait);

    }


    void Awake()
    {
        ConnectServer();
        InitMessageListener();
    }

    void Update()
    {
        
    }

    //ע���¼�
    public void InitMessageListener()
    {
        NetManager.AddMsgListener("MsgMultiWait",OnMsgMultiWait);
        NetManager.AddMsgListener("MsgMultiEnter", OnMsgMultiEnter);
        NetManager.AddMsgListener("MsgWaitConfirm", OnMsgWaitConfirm);

        NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
        NetManager.AddEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        NetManager.AddMsgListener("MsgLoadData", OnMsgLoadData);
        NetManager.AddMsgListener("MsgKick", OnMsgKick);
        NetManager.AddMsgListener("MsgRegister", OnMsgRegister);
        NetManager.AddMsgListener("MsgLogin", OnMsgLogin);
    }
    //�յ���½Э��
    public void OnMsgLogin(MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin)msgBase;
        if (msg.result == 0)
        {
            Debug.Log("��½�ɹ�");
            //���浱ǰ���ID
            NetManager.playerID = msg.id;
        }
        else
        {
            Debug.Log("��½ʧ��");
        }
    }
    //�յ�ע��Э��
    public void OnMsgRegister(MsgBase msgBase)
    {
        MsgRegister msg = (MsgRegister)msgBase;
        if (msg.result == 0)
        {
            Debug.Log("ע��ɹ�");
        }
        else
        {
            Debug.Log("ע��ʧ��");
        }
    }
    //���ӳɹ��ص�
    void OnConnectSucc(string err)
    {
        Debug.Log("OnConnectSucc");
    }

    //����ʧ�ܻص�
    void OnConnectFail(string err)
    {
        Debug.Log("OnConnectFail " + err);
    }

    //�ر�����
    void OnConnectClose(string err)
    {
        Debug.Log("OnConnectClose");
    }

    //��������
    void OnMsgKick(MsgBase msgBase)
    {
        Debug.Log("��������");
    }
    //�յ���ȡ����Э��
    public void OnMsgLoadData(MsgBase msgBase)
    {
        Debug.Log("LoadData");
        MsgLoadData msg = (MsgLoadData)msgBase;
        if (msg.data.Length > 0)
        {
            NetManager.playerDataStr = msg.data;
            UIManager.Instance.ShowUI(E_UiId.MainUI);
        }

    }
    //���������Ϸ
    public void OnMsgMultiEnter(MsgBase msgBase)
    {
        Debug.Log("Enter Game");
        EventDispatcher.TriggerEvent(E_MessageType.MultiGameStart);
    }
    //ȷ��ѡ��Э��
    public void OnMsgWaitConfirm(MsgBase msgBase)
    {
        Debug.Log("OnMsgWaitConfirm");
        MsgWaitConfirm msg = (MsgWaitConfirm) msgBase;
        EventDispatcher.TriggerEvent<SceneType>(E_MessageType.WaitConfirm,msg.type);
        chosenSceneType = msg.type;
    }
}
