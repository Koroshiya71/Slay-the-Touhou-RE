using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : BaseUI
{
    //确认框
    private GameObject confirmBox;
    //说明文本
    private Text text_Confirm;
    //确认按钮
    private Button btn_Confirm;
    //拒绝按钮
    private Button btn_Refuse;
    //等待提示框
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
        text_Confirm.text = "同伴选择进入";
        switch (type)
        {
            case SceneType.NormalCombat:
                text_Confirm.text += "普通战斗";
                break;
            case SceneType.EliteCombat:
                text_Confirm.text += "精英战斗";
                break;
            case SceneType.Event:
                text_Confirm.text += "事件";
                break;
            case SceneType.Lounge:
                text_Confirm.text += "休息处";
                break;
            case SceneType.Store:
                text_Confirm.text += "商店";
                break;
            case SceneType.BossCombat:
                text_Confirm.text += "Boss战";
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
            //隐藏面板
            confirmBox.SetActive(false);
            //发送接受消息
            MsgConfirmChoose msg = new MsgConfirmChoose();
            msg.type = MultiPlayMsgHandler.chosenSceneType;
            msg.confirm = true;
            msg.id = NetManager.playerID;

            NetManager.Send(msg);
        });

        btn_Refuse.onClick.AddListener(delegate
        {
            //隐藏面板
            confirmBox.SetActive(false);
            //发送拒绝消息
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
