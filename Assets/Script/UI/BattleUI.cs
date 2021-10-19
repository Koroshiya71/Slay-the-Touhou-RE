using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : BaseUI
{
    private Text text_Eng;
    private Button btn_TurnEnd;
    private Text text_TurnChange;
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        text_Eng = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Eng");
        btn_TurnEnd= GameTool.GetTheChildComponent<Button>(gameObject, "Btn_TurnEnd");
        text_TurnChange = GameTool.GetTheChildComponent<Text>(gameObject, "Text_TurnStart");
        //添加回调
        btn_TurnEnd.onClick.AddListener(BattleManager.Instance.OnTurnEndButtonDown);

        //取消回合开始文本的显示
        text_TurnChange.enabled = false;
    }

    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.BattleUI;
        this.uiType.uiRootType = E_UIRootType.Normal;
    }


    // 显示和隐藏回合开始文本
    private void ShowTurnStartTxt()
    {
        text_TurnChange.enabled = true;

    }

    private void HideTurnStartTxt()
    {
        text_TurnChange.enabled = false;
    }
    //添加事件监听
    public override void AddMessageListener()
    {
        EventDispatcher.AddListener(E_MessageType.UseCard, UpdateUI);
        EventDispatcher.AddListener(E_MessageType.BattleStart, UpdateUI);
        EventDispatcher.AddListener(E_MessageType.TurnStart, delegate
        {
            ShowTurnStartTxt();
            text_TurnChange.text = "玩家回合";
            UpdateUI();
            ActiveTurnEndBtn(true);
            Invoke(nameof(HideTurnStartTxt),0.5f);
            
        });
        EventDispatcher.AddListener(E_MessageType.TurnEnd, delegate
        {
            ShowTurnStartTxt();
            text_TurnChange.text = "敌人回合";
            Invoke(nameof(HideTurnStartTxt), 0.5f);
            ActiveTurnEndBtn(false);
        });
    }
    //启用/禁用回合结束按钮
    public void ActiveTurnEndBtn(bool active)
    {
        btn_TurnEnd.interactable = active;
    }

    //更新战斗UI
    public void UpdateUI()
    {
        text_Eng.text = BattleManager.Instance.CurrentEnergy + "/" + BattleManager.Instance.MaxEnergy;
    }

}
