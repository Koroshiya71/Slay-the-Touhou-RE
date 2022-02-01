using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class MultiPlayMsgHandler : UnitySingleton<MultiPlayMsgHandler>
{
    //玩家点击连接按钮
    public void ConnectServer()
    {
        NetManager.Connect("127.0.0.1", 8888);
    }
    //等待协议回调
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

    //注册事件
    public void InitMessageListener()
    {
        NetManager.AddMsgListener("MsgMultiWait",OnMsgMultiWait);
        NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
        NetManager.AddEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        NetManager.AddMsgListener("MsgLoadData", OnMsgLoadData);

        NetManager.AddMsgListener("MsgKick", OnMsgKick);
    }
    //连接成功回调
    void OnConnectSucc(string err)
    {
        Debug.Log("OnConnectSucc");
    }

    //连接失败回调
    void OnConnectFail(string err)
    {
        Debug.Log("OnConnectFail " + err);
    }

    //关闭连接
    void OnConnectClose(string err)
    {
        Debug.Log("OnConnectClose");
    }

    //被踢下线
    void OnMsgKick(MsgBase msgBase)
    {
        Debug.Log("被踢下线");
    }
    //收到获取数据协议
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
