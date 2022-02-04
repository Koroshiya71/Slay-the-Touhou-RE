using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : BaseUI
{
    //ȷ�Ͽ�
    private GameObject confirmBox;
    //˵���ı�
    private Text text_Confirm;
    //ȷ�ϰ�ť
    private Button btn_Confirm;
    //�ܾ���ť
    private Button btn_Refuse;
    //�ȴ���ʾ��
    private GameObject waitBox;
    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.MapUI;
    }


    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        confirmBox=GameObject.Find("ConfirmBox");
        btn_Confirm = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_Confirm");
        btn_Refuse = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_Refuse");
        text_Confirm=GameTool.GetTheChildComponent<Text>(gameObject, "Text_Confirm");
        waitBox=GameObject.Find("WaitBox");
        confirmBox.SetActive(false);
        waitBox.SetActive(false);
    }

public void ShowConfirmBox(SceneType type)
    {
        confirmBox.SetActive(true);
        text_Confirm.text = "ͬ��ѡ�����";
        switch (type)
        {
            case SceneType.NormalCombat:
                text_Confirm.text += "��ͨս��";
                break;
            case SceneType.EliteCombat:
                text_Confirm.text += "��Ӣս��";
                break;
            case SceneType.Event:
                text_Confirm.text += "�¼�";
                break;
            case SceneType.Lounge:
                text_Confirm.text += "��Ϣ��";
                break;
            case SceneType.Store:
                text_Confirm.text += "�̵�";
                break;
            case SceneType.BossCombat:
                text_Confirm.text += "Bossս";
                break;
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void AddMessageListener()
    {
        base.AddMessageListener();
        EventDispatcher.AddListener<SceneType>(E_MessageType.MultWaitConfirm,ShowConfirmBox);

        btn_Confirm.onClick.AddListener(delegate
        {
            //�������
            confirmBox.SetActive(false);
            //���ͽ�����Ϣ
            MsgConfirmChoose msg = new MsgConfirmChoose();
            msg.type = MultiPlayMsgHandler.chosenSceneType;
            msg.confirm = true;
            msg.id = NetManager.playerID;

            NetManager.Send(msg);
        });

        btn_Refuse.onClick.AddListener(delegate
        {
            //�������
            confirmBox.SetActive(false);
            //���;ܾ���Ϣ
            MsgConfirmChoose msg = new MsgConfirmChoose();
            msg.type = MultiPlayMsgHandler.chosenSceneType;
            msg.confirm = false;
            msg.id = NetManager.playerID;
            NetManager.Send(msg);
        });

        EventDispatcher.AddListener(E_MessageType.MultChooseScene, delegate
        {
            waitBox.SetActive(true);
        });

        EventDispatcher.AddListener(E_MessageType.MultEnterScene, delegate
        {
            waitBox.SetActive(false);
            confirmBox.SetActive(false);
        });
    }

    protected override void Awake()
    {
        base.Awake();
        EventDispatcher.TriggerEvent(E_MessageType.GameStart);

    }
}
