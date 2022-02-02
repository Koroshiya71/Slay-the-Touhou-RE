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

    

    //主动关闭
    public void CloseConnect()
    {
        NetManager.Close();
    }

   



    //发送注册协议
    public void OnRegisterClick()
    {
        MsgRegister msg = new MsgRegister();
        msg.id = input_ID.text;
        msg.pw = input_PW.text;
        NetManager.Send(msg);
    }

   

    //发送登陆协议
    public void OnLoginClick()
    {
        MsgLogin msg = new MsgLogin();
        msg.id = input_ID.text;
        msg.pw = input_PW.text;
        NetManager.Send(msg);
        
    }



    

    private void FixedUpdate()
    {
        
    }
}