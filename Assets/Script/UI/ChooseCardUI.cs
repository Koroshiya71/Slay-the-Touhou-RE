using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using UnityEngine.UI;

//选牌类型
public enum ChooseType
{
    Empty,//空
    AddToDesk,//加入牌库
    AddToHand,//加入手中
    Other,//其他效果
}
public class ChooseCardUI : BaseUI
{

    //卡牌content的transform
    private Transform content;
    //卡牌预制体
    private GameObject chooseCardPrefab;
    //卡牌物体列表
    private List<GameObject> cardObjList = new List<GameObject>();
    //确认按钮
    private Button btn_Confirm;
    //选牌状态
    private ChooseType lastType=ChooseType.Empty;
    //显示用卡牌物体列表
    private List<GameObject> chooseCardGoList = new List<GameObject>();
    //获取UI组件并添加回调函数
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        content = GameTool.FindTheChild(gameObject, "Content");
        btn_Confirm = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_Confirm");
    }

    //初始化UI类型等数据
    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.ChooseCardUI;
        this.uiType.showMode = E_ShowUIMode.DoNothing;
        this.uiType.uiRootType = E_UIRootType.KeepAbove;
        chooseCardPrefab = ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/Card/ChooseCard");
        
    }
    //显示卡牌（要显示的卡牌数据列表）
    public void ShowChooseCard(List<CardData> dataList,ChooseType type=ChooseType.Empty)
    {
        //如果是第一次选牌
        if (lastType==ChooseType.Empty)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                //生成并初始化卡牌
                Transform newCardGo = Instantiate(chooseCardPrefab).transform;
                newCardGo.SetParent(content.transform);
                newCardGo.localPosition = new Vector2(-600+(1800.0f / dataList.Count) * (i), 0);
                newCardGo.localScale = new Vector3(1.5f, 1.5f,1.5f);
                ChooseCard newCard = newCardGo.GetComponent<ChooseCard>();
                newCard.InitCard(dataList[i]);
                chooseCardGoList.Add(newCardGo.gameObject);
            }

        }
        //否则就根据数量重新初始化
        else if(dataList.Count<=chooseCardGoList.Count)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                chooseCardGoList[i].SetActive(true);
                chooseCardGoList[i].GetComponent<ChooseCard>().InitCard(dataList[i]);
                chooseCardGoList[i].GetComponent<ChooseCard>().chooseEffect.SetActive(false);
            }
            for (int i = dataList.Count; i < chooseCardGoList.Count; i++)
            {
                chooseCardGoList[i].SetActive(false);
                chooseCardGoList[i].GetComponent<ChooseCard>().chooseEffect.SetActive(false);

            }
        }
        else
        {
            for (int i = 0; i < chooseCardGoList.Count; i++)
            {
                chooseCardGoList[i].SetActive(true);
                chooseCardGoList[i].GetComponent<ChooseCard>().InitCard(dataList[i]);
                chooseCardGoList[i].GetComponent<ChooseCard>().chooseEffect.SetActive(false);

            }
            for (int i = chooseCardGoList.Count; i < dataList.Count; i++)
            {
                //生成并初始化卡牌
                Transform newCardGo = Instantiate(chooseCardPrefab).transform;
                newCardGo.SetParent(content.transform);
                newCardGo.localPosition = new Vector2(-600 + (1800.0f / dataList.Count) * (i), 0);
                newCardGo.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                ChooseCard newCard = newCardGo.GetComponent<ChooseCard>();
                newCard.InitCard(dataList[i]);
                chooseCardGoList.Add(newCardGo.gameObject);
                chooseCardGoList[i].GetComponent<ChooseCard>().chooseEffect.SetActive(false);
            }
        }

        Debug.Log(type);
        //根据本次的选牌类型进行相关处理
        switch (type)
        {
            //加入牌库
            case ChooseType.AddToDesk:
                //先清空点击监听再添加监听
                btn_Confirm.onClick.RemoveAllListeners();
                GameTool.GetTheChildComponent<Text>(btn_Confirm.gameObject, "Text").text="跳过";
                Debug.Log(1);
                btn_Confirm.onClick.AddListener(delegate
                {
                    DeskManager.Instance.isChoosing = false;
                });
                break;
        }
        lastType = type;
        Debug.Log(lastType);
    }
    
    //初始化消息监听
    public override void AddMessageListener()
    {
        EventDispatcher.AddListener<List<CardData>,ChooseType>(E_MessageType.ShowChooseCardUI,ShowChooseCard);
    }
    
}


