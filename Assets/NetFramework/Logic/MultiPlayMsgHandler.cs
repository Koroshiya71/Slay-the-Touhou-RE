using System.Collections;
using System.Collections.Generic;
using GameCore;
using Newtonsoft.Json;
using UnityEngine;

public class MultiPlayMsgHandler : UnitySingleton<MultiPlayMsgHandler>
{
    //��ǰ���ߵĳ�������
    public static SceneType chosenSceneType;
    //��ǰ��ѡ��ս�����ݺ��¼�����
    public static BattleData currentBattleData;

    public static EventData currentEventData;
    //��ҵ�����Ӱ�ť
    public void ConnectServer()
    {
        NetManager.Connect("127.0.0.1", 8888);
    }

    //�ȴ�Э��ص�
    public void OnMsgMultiWait(MsgBase msgBase)
    {
        Debug.Log("multiWait");
        EventDispatcher.TriggerEvent(E_MessageType.MultiWait);
        //���ɳ���
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

    //ע���¼�
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

    //�յ���½Э��
    public void OnMsgLogin(MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin) msgBase;
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
        MsgRegister msg = (MsgRegister) msgBase;
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
        MsgLoadData msg = (MsgLoadData) msgBase;
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
        if (GameSceneManager.Instance.multScenes.Count>0)
        {
            MsgSendSceneType msg = new MsgSendSceneType();
            msg.id = NetManager.playerID;
            msg.sceneTypeListStr = JsonConvert.SerializeObject(GameSceneManager.Instance.multScenes);
            NetManager.Send(msg);
            Debug.Log(NetManager.playerID+"��Send SceneTypeData");
            Debug.Log(msg.sceneTypeListStr.Length);
        }

        EventDispatcher.TriggerEvent(E_MessageType.MultiGameStart);
    }

    //ȷ��ѡ��Э��
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

    //ȷ��ѡ��Э��
    public void OnMsgEnterScene(MsgBase msgBase)
    {
        Debug.Log("OnMsgEnterScene");
        MsgEnterScene msg = (MsgEnterScene) msgBase;

        
        switch (msg.type)
        {
            //�����������Ϊ��ͨս���������ѡȡһ��ս����������
            case SceneType.NormalCombat:
                BattleManager.Instance.InitBattle(currentBattleData);

                break;
            //�����������Ϊ�¼��������ѡȡһ���¼�
            case SceneType.Event:
                
                UIManager.Instance.ShowUI(E_UiId.EventUI);
                EventDispatcher.TriggerEvent<int, string>(E_MessageType.ShowEventPage,
                    currentEventData.pageDataList[0].pageID, "");
                break;
            //�����������Ϊ��Ӣս���������ѡȡһ����Ӣս��
            case SceneType.EliteCombat:
                BattleManager.Instance.InitBattle(currentBattleData);
                break;
            //�����������Ϊ�̵꣬����ʾ�̵�ҳ��
            case SceneType.Store:
                UIManager.Instance.ShowUI(E_UiId.StoreUI);
                EventDispatcher.TriggerEvent(E_MessageType.ShowStoreUI);
                break;
            //�����������Ϊ��Ϣ������ظ�ʱ�䲢��ʾ��Ϣ��ҳ��
            case SceneType.Lounge:
                UIManager.Instance.ShowUI(E_UiId.LoungeUI);
                GameManager.Instance.loungeData.loungeTime = GameManager.Instance.loungeData.maxLoungeTime;
                EventDispatcher.TriggerEvent(E_MessageType.ShowLoungeUI);
                break;
            //�����Bossս�����ʼ��һ��Bossս��
            case SceneType.BossCombat:
                BattleManager.Instance.InitBattle(currentBattleData);
                break;
        }
        EventDispatcher.TriggerEvent(E_MessageType.MultEnterScene);
    }
    //�յ�����Ч��Э��
    public void OnMsgCardEffect(MsgBase msgBase)
    {
        MsgCardEffect msg = (MsgCardEffect) msgBase;
        BattleManager.Instance.TakeCardEffect(msg.effectID,msg.effectValue,
            BattleManager.Instance.GetBattleUnitByIndex(msg.targetIndex),msg.isCanXin,msg.isLianZhan,true);
    }

    //ʹ�ÿ���Э��
    public void OnMsgUseCard(MsgBase msgBase)
    {
        MsgUseCard msg = (MsgUseCard) msgBase;
        SyncPlayer.currentTurnCombo++;
    }
    //��ȡ�ȴ�Э��
    public void OnMsgLoadWait(MsgBase msgBase)
    {
        Debug.Log("LoadWait");
        //�ȴ�����
        EventDispatcher.TriggerEvent(E_MessageType.MultWaitLoad);
        //����Ϊ�����
        NetManager.isMain = true;
    }
    //�ȴ���ȡ���
    public void OnMsgLoadEnd(MsgBase msgBase)
    {
        //ȡ���ȴ�����ʾ
        EventDispatcher.TriggerEvent(E_MessageType.MultEnterScene);
    }
    //��ȡ��������
    public void OnMsgSendSceneType(MsgBase msgBase)
    {
        MsgSendSceneType msg = (MsgSendSceneType) msgBase;
        GameSceneManager.Instance.multScenes = JsonConvert.DeserializeObject<List<SceneType>>(msg.sceneTypeListStr);
    }
    //�ȴ��غϽ���
    public void OnMsgTurnWait(MsgBase msgBase)
    {
        EventDispatcher.TriggerEvent(E_MessageType.MultTurnWait);
    }
    //�غ����
    public void OnMsgTurnFin(MsgBase msgBase)
    {
        StartCoroutine(BattleManager.Instance.TurnEnd());
    }
}