using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class BattleUI : BaseUI
{
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
    }

    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.BattleUI;
        this.uiType.uiRootType = E_UIRootType.Normal;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //战斗UI显示使触发战斗开始事件
        EventDispatcher.TriggerEvent(E_MessageType.BattleStart);
    }
}
