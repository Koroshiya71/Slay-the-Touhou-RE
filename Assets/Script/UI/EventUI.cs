using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
public class EventUI : BaseUI
{
   protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        
    }

    protected override void InitDataOnAwake()
    {

        base.InitDataOnAwake();
        this.uiId = E_UiId.EventUI;
        this.uiType.uiRootType = E_UIRootType.KeepAbove;
    }

}
