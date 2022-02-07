using System.Collections;
using System.Collections.Generic;
using GameCore;
using Newtonsoft.Json;
using UnityEngine;

public class MultiPlayMsgHandler : UnitySingleton<MultiPlayMsgHandler>
{
    //当前决策的场景类型
    public static SceneType chosenSceneType;
    //当前所选的战斗数据和事件数据
    public static BattleData currentBattleData;

    public static EventData currentEventData;
    //玩家点击连接按钮
    public void ConnectServer()
    {
        NetManager.Connect("127.0.0.1", 8888);
    }

    //等待协议回调
    public void OnMsgMultiWait(MsgBase msgBase)
    {
        Debug.Log("multiWait");
        EventDispatcher.TriggerEvent(E_MessageType.MultiWait);
        //生成场景
        for (int i = 0; i < 120; i++)
        {
            if (i < 6)
            {
                GameSceneManager.Instance.multScenes.Add(SceneType.NormalCombat);
            }
            else
            {
                GameSceneManager.Instance.multScenes.Add(GameSceneManager.GetRandomSceneType());
            }
        }

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
        NetManager.AddMsgListener("MsgCardEffect",OnMsgCardEffect);
        NetManager.AddMsgListener("MsgUseCard", OnMsgUseCard);
        NetManager.AddMsgListener("MsgLoadWait", OnMsgLoadWait);
        NetManager.AddMsgListener("MsgLoadEnd", OnMsgLoadEnd);
        NetManager.AddMsgListener("MsgSendSceneType", OnMsgSendSceneType);
        NetManager.AddMsgListener("MsgTurnWait", OnMsgTurnWait);
        NetManager.AddMsgListener("MsgTurnFin", OnMsgTurnFin);


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
        if (GameSceneManager.Instance.multScenes.Count>0)
        {
            MsgSendSceneType msg = new MsgSendSceneType();
            msg.id = NetManager.playerID;
            msg.sceneTypeListStr = JsonConvert.SerializeObject(GameSceneManager.Instance.multScenes);
            NetManager.Send(msg);
            Debug.Log(NetManager.playerID+"：Send SceneTypeData");
            Debug.Log(msg.sceneTypeListStr.Length);
        }

        EventDispatcher.TriggerEvent(E_MessageType.MultiGameStart);
    }

    //确认选择协议
    public void OnMsgWaitConfirm(MsgBase msgBase)
    {
        Debug.Log("OnMsgWaitConfirm");
        MsgWaitConfirm msg = (MsgWaitConfirm) msgBase;
        EventDispatcher.TriggerEvent<SceneType>(E_MessageType.MultWaitConfirm, msg.type);
        currentBattleData = JsonConvert.DeserializeObject<BattleData>(msg.battleDataStr);
        currentEventData= JsonConvert.DeserializeObject<EventData>(msg.eventDataStr);
        GameSceneManager.Instance.lastIndex = msg.index;
        Debug.Log(currentBattleData);
        chosenSceneType = msg.type;
    }

    //确认选择协议
    public void OnMsgEnterScene(MsgBase msgBase)
    {
        Debug.Log("OnMsgEnterScene");
        MsgEnterScene msg = (MsgEnterScene) msgBase;

        
        switch (msg.type)
        {
            //如果场景类型为普通战斗，则随机选取一个战斗场景数据
            case SceneType.NormalCombat:
                BattleManager.Instance.InitBattle(currentBattleData);

                break;
            //如果场景类型为事件，则随机选取一个事件
            case SceneType.Event:
                
                UIManager.Instance.ShowUI(E_UiId.EventUI);
                EventDispatcher.TriggerEvent<int, string>(E_MessageType.ShowEventPage,
                    currentEventData.pageDataList[0].pageID, "");
                break;
            //如果场景类型为精英战斗，则随机选取一个精英战斗
            case SceneType.EliteCombat:
                BattleManager.Instance.InitBattle(currentBattleData);
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
                BattleManager.Instance.InitBattle(currentBattleData);
                break;
        }
        EventDispatcher.TriggerEvent(E_MessageType.MultEnterScene);
    }
    //收到卡牌效果协议
    public void OnMsgCardEffect(MsgBase msgBase)
    {
        MsgCardEffect msg = (MsgCardEffect) msgBase;
        BattleManager.Instance.TakeCardEffect(msg.effectID,msg.effectValue,
            BattleManager.Instance.GetBattleUnitByIndex(msg.targetIndex),msg.isCanXin,msg.isLianZhan,true);
    }

    //使用卡牌协议
    public void OnMsgUseCard(MsgBase msgBase)
    {
        MsgUseCard msg = (MsgUseCard) msgBase;
        SyncPlayer.currentTurnCombo++;
    }
    //读取等待协议
    public void OnMsgLoadWait(MsgBase msgBase)
    {
        Debug.Log("LoadWait");
        //等待别人
        EventDispatcher.TriggerEvent(E_MessageType.MultWaitLoad);
        //设置为主玩家
        NetManager.isMain = true;
    }
    //等待读取完毕
    public void OnMsgLoadEnd(MsgBase msgBase)
    {
        //取消等待框显示
        EventDispatcher.TriggerEvent(E_MessageType.MultEnterScene);
    }
    //读取场景数据
    public void OnMsgSendSceneType(MsgBase msgBase)
    {
        MsgSendSceneType msg = (MsgSendSceneType) msgBase;
        GameSceneManager.Instance.multScenes = JsonConvert.DeserializeObject<List<SceneType>>(msg.sceneTypeListStr);
    }
    //等待回合结束
    public void OnMsgTurnWait(MsgBase msgBase)
    {
        EventDispatcher.TriggerEvent(E_MessageType.MultTurnWait);
    }
    //回合完成
    public void OnMsgTurnFin(MsgBase msgBase)
    {
        StartCoroutine(BattleManager.Instance.TurnEnd());
    }
}