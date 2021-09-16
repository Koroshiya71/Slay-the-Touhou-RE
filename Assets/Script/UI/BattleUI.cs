using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : BaseUI
{
    private Text text_Eng;


    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        text_Eng = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Eng");
    }

    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.BattleUI;
        this.uiType.uiRootType = E_UIRootType.Normal;
    }

    //添加事件监听
    public override void AddMessageListener()
    {
        EventDispatcher.AddListener(E_MessageType.UseCard, UpdateUI);
        EventDispatcher.AddListener(E_MessageType.BattleStart, UpdateUI);

    }

    //更新战斗UI
    public void UpdateUI()
    {
        text_Eng.text = BattleManager.Instance.CurrentEnergy + "/" + BattleManager.Instance.MaxEnergy;
    }



    protected override void OnEnable()
    {
        base.OnEnable();
       BattleManager.Instance.InitBattle();
    }
}
