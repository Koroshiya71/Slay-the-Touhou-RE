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
        //��ӻص�
        btn_TurnEnd.onClick.AddListener(BattleManager.Instance.OnTurnEndButtonDown);

        //ȡ���غϿ�ʼ�ı�����ʾ
        text_TurnChange.enabled = false;
    }

    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.BattleUI;
        this.uiType.uiRootType = E_UIRootType.Normal;
    }


    // ��ʾ�����ػغϿ�ʼ�ı�
    private void ShowTurnStartTxt()
    {
        text_TurnChange.enabled = true;

    }

    private void HideTurnStartTxt()
    {
        text_TurnChange.enabled = false;
    }
    //����¼�����
    public override void AddMessageListener()
    {
        EventDispatcher.AddListener(E_MessageType.UseCard, UpdateUI);
        EventDispatcher.AddListener(E_MessageType.BattleStart, UpdateUI);
        EventDispatcher.AddListener(E_MessageType.TurnStart, delegate
        {
            ShowTurnStartTxt();
            text_TurnChange.text = "��һغ�";
            UpdateUI();
            ActiveTurnEndBtn(true);
            Invoke(nameof(HideTurnStartTxt),0.5f);
            
        });
        EventDispatcher.AddListener(E_MessageType.TurnEnd, delegate
        {
            ShowTurnStartTxt();
            text_TurnChange.text = "���˻غ�";
            Invoke(nameof(HideTurnStartTxt), 0.5f);
            ActiveTurnEndBtn(false);
        });
    }
    //����/���ûغϽ�����ť
    public void ActiveTurnEndBtn(bool active)
    {
        btn_TurnEnd.interactable = active;
    }

    //����ս��UI
    public void UpdateUI()
    {
        text_Eng.text = BattleManager.Instance.CurrentEnergy + "/" + BattleManager.Instance.MaxEnergy;
    }

}
