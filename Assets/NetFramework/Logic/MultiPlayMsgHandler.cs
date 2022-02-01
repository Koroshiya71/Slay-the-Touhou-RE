using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class MultiPlayMsgHandler : UnitySingleton<MultiPlayMsgHandler>
{
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
        NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
        NetManager.AddEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        NetManager.AddMsgListener("MsgLoadData", OnMsgLoadData);

        NetManager.AddMsgListener("MsgKick", OnMsgKick);
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

        Debug.Log(NetManager.playerDataStr);
    }
}
