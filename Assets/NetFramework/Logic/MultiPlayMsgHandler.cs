using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class MultiPlayMsgHandler : UnitySingleton<MultiPlayMsgHandler>
{
    //当前决策的场景类型吧
    public static SceneType chosenSceneType;

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
        NetManager.AddMsgListener("MsgMultiWait", OnMsgMultiWait);
        NetManager.AddMsgListener("MsgMultiEnter", OnMsgMultiEnter);
        NetManager.AddMsgListener("MsgWaitConfirm", OnMsgWaitConfirm);
        NetManager.AddMsgListener("MsgEnterScene", OnMsgEnterScene);

        NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
        NetManager.AddEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        NetManager.AddMsgListener("MsgLoadData", OnMsgLoadData);
        NetManager.AddMsgListener("MsgKick", OnMsgKick);
        NetManager.AddMsgListener("MsgRegister", OnMsgRegister);
        NetManager.AddMsgListener("MsgLogin", OnMsgLogin);
    }

    //收到登陆协议
    public void OnMsgLogin(MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin) msgBase;
        if (msg.result == 0)
        {
            Debug.Log("登陆成功");
            //保存当前玩家ID
            NetManager.playerID = msg.id;
        }
        else
        {
            Debug.Log("登陆失败");
        }
    }

    //收到注册协议
    public void OnMsgRegister(MsgBase msgBase)
    {
        MsgRegister msg = (MsgRegister) msgBase;
        if (msg.result == 0)
        {
            Debug.Log("注册成功");
        }
        else
        {
            Debug.Log("注册失败");
        }
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
        MsgLoadData msg = (MsgLoadData) msgBase;
        if (msg.data.Length > 0)
        {
            NetManager.playerDataStr = msg.data;
            UIManager.Instance.ShowUI(E_UiId.MainUI);
        }
    }

    //进入多人游戏
    public void OnMsgMultiEnter(MsgBase msgBase)
    {
        Debug.Log("Enter Game");
        EventDispatcher.TriggerEvent(E_MessageType.MultiGameStart);
    }

    //确认选择协议
    public void OnMsgWaitConfirm(MsgBase msgBase)
    {
        Debug.Log("OnMsgWaitConfirm");
        MsgWaitConfirm msg = (MsgWaitConfirm) msgBase;
        EventDispatcher.TriggerEvent<SceneType>(E_MessageType.MultWaitConfirm, msg.type);
        chosenSceneType = msg.type;
    }

    //确认选择协议
    public void OnMsgEnterScene(MsgBase msgBase)
    {
        Debug.Log("OnMsgEnterScene");
        MsgEnterScene msg = (MsgEnterScene) msgBase;
        BattleData battleData;
        EventData eventData;
        switch (msg.type)
        {
            //如果场景类型为普通战斗，则随机选取一个战斗场景数据
            case SceneType.NormalCombat:
                while (true)
                {
                    battleData =
                        BattleManager.Instance.battleDataDic[
                            Random.Range(1, BattleManager.Instance.battleDataDic.Count + 1)];
                    if (battleData.BattleType == BattleType.Normal)
                        break;
                }

                BattleManager.Instance.InitBattle(battleData);

                break;
            //如果场景类型为事件，则随机选取一个事件
            case SceneType.Event:
                eventData =
                    GameEventManager.Instance.eventDic[
                        GameEventManager.Instance.eventIDList1[
                            Random.Range(0, GameEventManager.Instance.eventIDList1.Count)]];
                UIManager.Instance.ShowUI(E_UiId.EventUI);
                EventDispatcher.TriggerEvent<int, string>(E_MessageType.ShowEventPage,
                    eventData.pageDataList[0].pageID, "");
                break;
            //如果场景类型为精英战斗，则随机选取一个精英战斗
            case SceneType.EliteCombat:
                while (true)
                {
                    battleData =
                        BattleManager.Instance.battleDataDic[
                            Random.Range(1, BattleManager.Instance.battleDataDic.Count + 1)];
                    if (battleData.BattleType == BattleType.Elite)
                        break;
                }

                BattleManager.Instance.InitBattle(battleData);
                break;
            //如果场景类型为商店，则显示商店页面
            case SceneType.Store:
                UIManager.Instance.ShowUI(E_UiId.StoreUI);
                EventDispatcher.TriggerEvent(E_MessageType.ShowStoreUI);
                break;
            //如果场景类型为休息处，则回复时间并显示休息处页面
            case SceneType.Lounge:
                UIManager.Instance.ShowUI(E_UiId.LoungeUI);
                GameManager.Instance.loungeData.loungeTime = GameManager.Instance.loungeData.maxLoungeTime;
                EventDispatcher.TriggerEvent(E_MessageType.ShowLoungeUI);
                break;
            //如果是Boss战，则初始化一场Boss战斗
            case SceneType.BossCombat:
                while (true)
                {
                    battleData =
                        BattleManager.Instance.battleDataDic[
                            Random.Range(1, BattleManager.Instance.battleDataDic.Count + 1)];
                    if (battleData.BattleType == BattleType.Boss)
                        break;
                }

                BattleManager.Instance.InitBattle(battleData);
                break;
        }
        EventDispatcher.TriggerEvent(E_MessageType.MultEnterScene);
    }
}