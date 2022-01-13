using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using UnityEngine.UI;

public class ChooseCardUI : BaseUI
{

    //卡牌content的transform
    private Transform content;
    //关闭按钮
    private Button btn_Close;
    //卡牌预制体
    private GameObject normalCardPrefab;
    //卡牌物体列表
    private List<GameObject> cardObjList = new List<GameObject>();
    //上次显示的类型
    private ShowType lastShowType = ShowType.Empty;
    //获取UI组件并添加回调函数
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        content = GameTool.FindTheChild(gameObject, "Content");
    }

    //初始化UI类型等数据
    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.ChooseCardUI;
        this.uiType.showMode = E_ShowUIMode.DoNothing;
        this.uiType.uiRootType = E_UIRootType.KeepAbove;
        normalCardPrefab = ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/Card/DisplayCard");
        btn_Close = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_Close");
        btn_Close.onClick.AddListener(delegate
        {
            UIManager.Instance.HideSingleUI(E_UiId.DisplayCardUI);
        });
    }

    //初始化消息监听
    public override void AddMessageListener()
    {
    }
    
}


