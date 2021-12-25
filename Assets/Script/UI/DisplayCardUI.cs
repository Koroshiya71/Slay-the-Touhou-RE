using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using UnityEngine.UI;
public class DisplayCardUI : BaseUI
{

    //卡牌content的transform
    private Transform content;
    //卡牌预制体
    private GameObject normalCardPrefab;

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
        normalCardPrefab = ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/Card/HandCard");
    }

    //初始化消息监听
    public override void AddMessageListener()
    {
        EventDispatcher.AddListener<bool, List<CardData>>(E_MessageType.DisplayCard, DisplayCard);
    }
    //根据卡牌数据列表显示对应的卡牌（普通卡牌还是符卡,卡牌数据列表）
    public void DisplayCard(bool isNormalCard, List<CardData> dataList)
    {
        int count = dataList.Count;
        count.
        //如果是普通卡牌
        if (isNormalCard)
        {
            for (int i = 0; i < count; i++)
            {
                //生成卡牌
                Transform newCardGo = Instantiate(normalCardPrefab).transform;
                newCardGo.SetParent(content);
                newCardGo.localScale = new Vector2(1.5f, 1.5f);
                newCardGo.localPosition =new Vector3(250+350*count/5,-200+400*count%5);
            }
        }
    }
}
