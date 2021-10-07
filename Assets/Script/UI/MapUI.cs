using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class MapUI : BaseUI
{
    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.MapUI;
    }


    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventDispatcher.TriggerEvent(E_MessageType.GameStart);
    }
}
