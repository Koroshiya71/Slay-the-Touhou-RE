using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class LoginAndConnectUI : BaseUI
{
    //ID输入框
    private InputField input_ID;

    //密码输入框
    private InputField input_PW;

    //注册按钮
    private Button btn_Register;

    //登录按钮
    private Button btn_Login;

    //离线游戏按钮
    private Button btn_OffLine;

    //计时器
    private float timer;

    //开始
    protected override void Start()
    {
        base.Start();
        NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
        NetManager.AddEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        NetManager.AddMsgListener("MsgLoadData",OnMsgLoadData);
        NetManager.AddMsgListener("MsgRegister", OnMsgRegister);
        NetManager.AddMsgListener("MsgLogin", OnMsgLogin);
        NetManager.AddMsgListener("MsgKick", OnMsgKick);
        NetManager.AddMsgListener("MsgSaveText", OnMsgSaveText);
    }

    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        uiId = E_UiId.LoginAndConnectUI;
        uiType.showMode = E_ShowUIMode.HideAll;
        uiType.uiRootType = E_UIRootType.Normal;
    }

    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        input_ID = GameTool.GetTheChildComponent<InputField>(gameObject, "Input_ID");
        input_PW = GameTool.GetTheChildComponent<InputField>(gameObject, "Input_PW");
        btn_Login = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_Login");
        btn_Register = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_Register");
        btn_OffLine = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_OffLine");
    }

    public override void AddMessageListener()
    {
        base.AddMessageListener();
        btn_Login.onClick.AddListener(OnLoginClick);
        btn_Register.onClick.AddListener(OnRegisterClick);
        btn_OffLine.onClick.AddListener(delegate
        {
            HideUI();
            UIManager.Instance.ShowUI(E_UiId.MainUI);
            SaveManager.Instance.isOffline = true;
        });
    }

    //玩家点击连接按钮
    public void ConnectServer()
    {
        NetManager.Connect("127.0.0.1", 8888);
    }

    //主动关闭
    public void CloseConnect()
    {
        NetManager.Close();
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

    // Update is called once per frame
    void Update()
    {
        NetManager.Update();
    }


    //发送注册协议
    public void OnRegisterClick()
    {
        MsgRegister msg = new MsgRegister();
        msg.id = input_ID.text;
        msg.pw = input_PW.text;
        NetManager.Send(msg);
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

    //发送登陆协议
    public void OnLoginClick()
    {
        MsgLogin msg = new MsgLogin();
        msg.id = input_ID.text;
        msg.pw = input_PW.text;
        NetManager.Send(msg);
        
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

    //收到保存协议
    public void OnMsgSaveText(MsgBase msgBase)
    {
        MsgSaveText msg = (MsgSaveText) msgBase;
        if (msg.result == 0)
        {
            Debug.Log("保存成功");
        }
        else
        {
            Debug.Log("保存失败");
        }
    }

    //收到获取数据协议
    public void OnMsgLoadData(MsgBase msgBase)
    {
        Debug.Log("LoadData");
        MsgLoadData msg = (MsgLoadData) msgBase;
        if (msg.data.Length>0)
        {
            NetManager.playerDataStr = msg.data;
            UIManager.Instance.ShowUI(E_UiId.MainUI);
            HideUI();
        }

        Debug.Log(NetManager.playerDataStr);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        ConnectServer();
    }


    private void FixedUpdate()
    {
        
    }
}