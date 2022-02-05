using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : BaseUI
{
    //能量文本
    private Text text_Eng;
    //回合结束按钮
    private Button btn_TurnEnd;
    //回合切换文本
    private Text text_TurnChange;
    //抽牌堆
    private Button btn_ShowDrawCard;
    private Text text_ShowDrawCard;
    //弃牌堆
    private Button btn_ShowDiscard;
    private Text text_ShowDiscard;
    //等待面板
    private GameObject waitBox;
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        text_Eng = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Eng");
        btn_TurnEnd = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_TurnEnd");
        text_TurnChange = GameTool.GetTheChildComponent<Text>(gameObject, "Text_TurnStart");

        btn_ShowDrawCard = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_ShowDrawCard");
        text_ShowDrawCard = GameTool.GetTheChildComponent<Text>(btn_ShowDrawCard.gameObject, "Text_Num");

        btn_ShowDiscard = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_ShowDisCard");
        text_ShowDiscard = GameTool.GetTheChildComponent<Text>(btn_ShowDiscard.gameObject, "Text_Num");

        waitBox=GameObject.Find("WaitBox");
        waitBox.SetActive(false);
        //查看抽牌堆
        btn_ShowDrawCard.onClick.AddListener(delegate
        {
            UIManager.Instance.ShowUI(E_UiId.DisplayCardUI);
            EventDispatcher.TriggerEvent<List<CardData>, ShowType>(
                E_MessageType.DisplayCard, DeskManager.Instance.drawCardDeskList, ShowType.DrawCardDesk);
        });

        //查看弃牌堆
        btn_ShowDiscard.onClick.AddListener(delegate
        {
            UIManager.Instance.ShowUI(E_UiId.DisplayCardUI);
            EventDispatcher.TriggerEvent<List<CardData>, ShowType>(
                E_MessageType.DisplayCard, DeskManager.Instance.disCardDeskList, ShowType.DisCardDesk);
        });

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
    //回合结束
    private IEnumerator TurnUpdate(bool isStart)
    {
        if (isStart)
        {
            yield return new WaitForSeconds(0.5f);
            ActiveTurnEndBtn(true);
            HideTurnStartTxt();
        }
        else
        {
            ActiveTurnEndBtn(false);
            yield return new WaitForSeconds(0.5f);
            HideTurnStartTxt();
        }
    }
    //添加事件监听
    public override void AddMessageListener()
    {
        EventDispatcher.AddListener(E_MessageType.UseCard, UpdateUI);
        EventDispatcher.AddListener(E_MessageType.BattleStart, UpdateUI);
        EventDispatcher.AddListener(E_MessageType.DrawCard, UpdateUI);
        EventDispatcher.AddListener(E_MessageType.TurnStart, delegate
        {
            ShowTurnStartTxt();
            text_TurnChange.text = "玩家回合";
            UpdateUI();
            StartCoroutine(TurnUpdate(true));
        });
        EventDispatcher.AddListener(E_MessageType.TurnEnd, delegate
        {
            ShowTurnStartTxt();
            text_TurnChange.text = "敌人回合";
            StartCoroutine(TurnUpdate(false));

        });
        EventDispatcher.AddListener(E_MessageType.MultTurnWait, delegate
        {
            waitBox.SetActive(true);
        });
        EventDispatcher.AddListener(E_MessageType.TurnEnd, delegate
        {
            waitBox.SetActive(false);
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
        text_ShowDrawCard.text = DeskManager.Instance.drawCardDeskList.Count.ToString();
        text_ShowDiscard.text = DeskManager.Instance.disCardDeskList.Count.ToString();
    }

}
