using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;


//主界面UI
public class MainUI : BaseUI
{
    //开始游戏按钮
    private Button btn_StartGame;
    //载入进度按钮
    private Button btn_LoadGame;
    //多人游戏按钮
    private Button btn_MultiGame;
    //等待Panel
    private GameObject waitPanel;

    
    //获取UI组件并添加回调函数
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        btn_StartGame = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_StartGame");
        btn_StartGame.onClick.AddListener(delegate
        {
            LoadGameScene();
        });

        btn_LoadGame = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_LoadGame");
        

        btn_MultiGame = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_MultiGame");
        waitPanel=GameObject.Find("WaitPanel");
        waitPanel.SetActive(false);
    }


    //初始化UI类型等数据
    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.MainUI;
        this.uiType.showMode = E_ShowUIMode.DoNothing;
    }

    //加载游戏场景
    private void LoadGameScene(bool isMulti=false)
    {
        if (isMulti)
        {
            GameManager.Instance.isMulti = true;
        }
        SceneController.Instance.LoadSceneAsync("GameScene", delegate
        {
            UIManager.Instance.ShowUI(E_UiId.GameMainUI);
            UIManager.Instance.ShowUI(E_UiId.MapUI);
            

            if (!SaveManager.Instance.isOffline)
            {


            }

            if (isMulti)
            {
                NetManager.Send(new MsgLoadOK() { id = NetManager.playerID });
            }
        });
    }

    public override void AddMessageListener()
    {
        base.AddMessageListener();

        btn_LoadGame.onClick.AddListener(delegate
        {
            SaveManager.Instance.LoadData();
            LoadGameScene();
        });

        btn_MultiGame.onClick.AddListener(delegate
        {
            MsgMultiStart msg=new MsgMultiStart();
            msg.id = NetManager.playerID;
            //发送消息
            NetManager.Send(msg);
            Debug.Log("Send：StartMulti");
        });

        EventDispatcher.AddListener(E_MessageType.MultiWait, delegate
        {
            waitPanel.SetActive(true);
        });
        EventDispatcher.AddListener(E_MessageType.MultiGameStart, delegate
        {
            LoadGameScene(true);
        });
    }
}
