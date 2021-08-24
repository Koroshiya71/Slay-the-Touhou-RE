using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;


//ÓÎÏ·Ö÷UI
public class GameMainUI : BaseUI
{
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
    }

    protected override void InitDataOnAwake()
    {

        base.InitDataOnAwake();
        this.uiId = E_UiId.GameMainUI;
    }
}
