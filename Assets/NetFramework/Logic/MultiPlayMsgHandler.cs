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
        EventDispatcher.TriggerEvent(E_MessageType.MultiGameStart);
    }

    //ȷ��ѡ��Э��
    public void OnMsgWaitConfirm(MsgBase msgBase)
    {
        Debug.Log("OnMsgWaitConfirm");
        MsgWaitConfirm msg = (MsgWaitConfirm) msgBase;
        EventDispatcher.TriggerEvent<SceneType>(E_MessageType.MultWaitConfirm, msg.type);
        chosenSceneType = msg.type;
    }

    //ȷ��ѡ��Э��
    public void OnMsgEnterScene(MsgBase msgBase)
    {
        Debug.Log("OnMsgEnterScene");
        MsgEnterScene msg = (MsgEnterScene) msgBase;
        BattleData battleData;
        EventData eventData;
        switch (msg.type)
        {
            //�����������Ϊ��ͨս���������ѡȡһ��ս����������
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
            //�����������Ϊ�¼��������ѡȡһ���¼�
            case SceneType.Event:
                eventData =
                    GameEventManager.Instance.eventDic[
                        GameEventManager.Instance.eventIDList1[
                            Random.Range(0, GameEventManager.Instance.eventIDList1.Count)]];
                UIManager.Instance.ShowUI(E_UiId.EventUI);
                EventDispatcher.TriggerEvent<int, string>(E_MessageType.ShowEventPage,
                    eventData.pageDataList[0].pageID, "");
                break;
            //�����������Ϊ��Ӣս���������ѡȡһ����Ӣս��
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