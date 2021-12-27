using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using UnityEngine.UI;

//显示的是什么内容
public enum ShowType
{
    //空
    Empty,
    //牌库
    Desk,
    //抽牌堆
    DrawCardDesk,
    //弃牌堆
    DisCardDesk
}
public class DisplayCardUI : BaseUI
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
        this.uiId = E_UiId.DisplayCardUI;
        this.uiType.showMode = E_ShowUIMode.DoNothing;
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
        EventDispatcher.AddListener<List<CardData>, ShowType>(E_MessageType.DisplayCard, DisplayCard);
    }
    //根据卡牌数据列表显示对应的卡牌（普通卡牌还是符卡,卡牌数据列表）
    public void DisplayCard(List<CardData> dataList, ShowType type)
    {
        int count = dataList.Count;
        //如果是第一次展示
        if (lastShowType == ShowType.Empty)
        {
            for (int i = 0; i < count; i++)
            {
                //生成卡牌
                Transform newCardGo = Instantiate(normalCardPrefab).transform;
                newCardGo.SetParent(content);
                newCardGo.localScale = new Vector2(1.5f, 1.5f);
                newCardGo.localPosition = new Vector3(250 + 350 * (i % 5), -200 - 400 * (i / 5));
                DisplayCard newCard = newCardGo.GetComponent<DisplayCard>();
                newCard.InitCard(dataList[i].CardID);
                //添加到列表
                cardObjList.Add(newCardGo.gameObject);
            }
            //更新上一次显示的类型
            lastShowType = type;
        }
    }

}
