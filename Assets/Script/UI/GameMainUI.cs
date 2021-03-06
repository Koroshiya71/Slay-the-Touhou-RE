using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

//游戏场景主UI
public class GameMainUI : BaseUI
{
    //查看牌库按钮
    private Button btn_ShowDesk;
    //金币数Text
    private Text text_Gold;
    //生命值text
    private Text text_Hp;
    //获胜Text
    private GameObject table_Clear;
    //失败Text
    private GameObject table_Defeat;
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        btn_ShowDesk=GameTool.GetTheChildComponent<Button>(gameObject,"Btn_ShowDesk");
        btn_ShowDesk.onClick.AddListener(DisPlayDesk);
        text_Gold = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Gold");
        text_Gold.text = "金币："+GameManager.Instance.playerData.gold;
        text_Hp = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Hp");
        text_Hp.text = "Hp：" + GameManager.Instance.playerData.currentHp + " / " + GameManager.Instance.playerData.maxHp;
        table_Clear = GameTool.FindTheChild(gameObject,"Table_Clear").gameObject;
        table_Defeat = GameTool.FindTheChild(gameObject, "Table_Defeat").gameObject;
        table_Clear.SetActive(false);
        table_Defeat.SetActive(false);

    }
    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.GameMainUI;
        this.uiType.uiRootType = E_UIRootType.KeepAbove;
    }
    //更新界面
    public void UpdateGameMainUI()
    {
        text_Gold.text = "金币：" + GameManager.Instance.playerData.gold;
        text_Hp.text = "Hp：" + GameManager.Instance.playerData.currentHp + " / " + GameManager.Instance.playerData.maxHp;
    }
    //注册事件
    public override void AddMessageListener()
    {
        base.AddMessageListener();
        EventDispatcher.AddListener(E_MessageType.UpdateGameMainUI,UpdateGameMainUI);
        EventDispatcher.AddListener(E_MessageType.GameDefeat, delegate
        {
            table_Defeat.SetActive(true);
        });
        EventDispatcher.AddListener(E_MessageType.GameClear, delegate
        {
            table_Clear.SetActive(true); 
        });
    }

    //展示牌库
    public void DisPlayDesk()
    {
        UIManager.Instance.ShowUI(E_UiId.DisplayCardUI);
        //触发对应的事件
        EventDispatcher.TriggerEvent<List<CardData>,ShowType>(
            E_MessageType.DisplayCard,DeskManager.Instance.deskCardList,ShowType.Desk);
    }
}
