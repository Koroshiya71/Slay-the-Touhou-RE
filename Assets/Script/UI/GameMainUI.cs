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
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        btn_ShowDesk=GameTool.GetTheChildComponent<Button>(gameObject,"Btn_ShowDesk");
        btn_ShowDesk.onClick.AddListener(DisPlayDesk);
    }

    protected override void InitDataOnAwake()
    {

        base.InitDataOnAwake();
        this.uiId = E_UiId.GameMainUI;
        this.uiType.uiRootType = E_UIRootType.KeepAbove;
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
