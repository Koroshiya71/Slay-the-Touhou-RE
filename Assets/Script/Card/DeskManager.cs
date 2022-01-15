using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
public class DeskManager : UnitySingleton<DeskManager>
{
    /// <summary>
    /// 用以管理各种牌库的管理器
    /// </summary>


    //牌库列表对象<卡牌数据>
    public List<CardData> deskCardList = new List<CardData>();

    //抽牌堆列表对象<卡牌数据>
    public List<CardData> drawCardDeskList = new List<CardData>();

    //弃牌堆列表对象<卡牌数据>
    public List<CardData> disCardDeskList = new List<CardData>();

    //所有卡牌data列表
    public List<CardData> allCardDataList = new List<CardData>();

    //本场战斗是否已经初始化过抽牌堆
    public bool hasInitDrawCardDesk = false;
    //是否正在选牌
    public bool isChoosing = false;
    //已选牌列表
    public List<CardData> hasChosenCardList = new List<CardData>();
    //初始化牌库管理器
    public void InitDeskManager()
    {
        //如果不是读档，则从数据文件中初始化牌库
        if (!SaveManager.isLoad)
        {
            StreamReader reader = new StreamReader(SaveManager.jsonDataPath + "BaseDesk.json");
            List<int> baseDesk = JsonConvert.DeserializeObject<List<int>>(reader.ReadToEnd());
            reader.Close();

            foreach (var cardID in baseDesk)
            {
                deskCardList.Add(new CardData(cardID));
            }

        }
        //否则直接读存储数据
        else
        {

            foreach (var data in SaveManager.Instance.saveData.cardDataList)
            {
                Debug.Log(deskCardList.Count);

                deskCardList.Add(data);
            }
        }

    }
    //初始化抽牌堆
    public void InitDrawCardDesk()
    {
        //如果弃牌堆为空且从未初始化过抽牌堆，则从牌库进行第一次初始化
        if (disCardDeskList.Count == 0 && !hasInitDrawCardDesk)
        {
            hasInitDrawCardDesk = true;

            //将牌库中所有卡牌加入抽牌堆
            foreach (var cardData in deskCardList)
            {
                drawCardDeskList.Add(cardData);
            }
        }
        //如果抽牌堆中的卡牌数量不为0,则将所有弃牌堆中的卡牌加入抽牌堆
        else if (disCardDeskList.Count > 1)
        {
            for (int i = 0; i < disCardDeskList.Count; i++)
            {
                drawCardDeskList.Add(disCardDeskList[i]);
                disCardDeskList.Remove(disCardDeskList[i]);
            }
        }
        //洗牌
        drawCardDeskList = GameTool.RandomSort(drawCardDeskList);
    }

    //重置各个牌堆
    public void ResetDesks()
    {
        //清空抽牌堆和弃牌堆
        drawCardDeskList.Clear();
        disCardDeskList.Clear();

        //重置抽牌堆状态为未初始化
        hasInitDrawCardDesk = false;
    }
    //抽牌轮转方法
    public void DrawCard()
    {
        if (drawCardDeskList.Count == 0)
        {
            InitDrawCardDesk();

        }
        //抽取抽牌堆的第一张牌，如果该牌没有被修改过则直接根据ID初始化卡牌
        CardData data = drawCardDeskList[0];
        if (data.HasModified)
        {
            HandCardManager.Instance.GetCardByData(data);
        }
        //否则则根据卡牌数据初始化
        else
        {
            HandCardManager.Instance.GetCardByID(data.cardID);
        }
        drawCardDeskList.Remove(data);
        EventDispatcher.TriggerEvent(E_MessageType.DrawCard);
    }
    //将卡牌加入牌组携程(选牌数量，卡牌数据列表)
    public IEnumerator ChooseCardAddToDesk(int chooseNum,List<CardData> cardDataList)
    {
        //显示UI和对应的卡牌
        isChoosing = true;
        UIManager.Instance.ShowUI(E_UiId.ChooseCardUI);
        EventDispatcher.TriggerEvent<List<CardData>,ChooseType>(E_MessageType.ShowChooseCardUI,cardDataList,ChooseType.AddToDesk);
        //等待选牌结束
        while (hasChosenCardList.Count<chooseNum&&isChoosing)
        {
            yield return new WaitForFixedUpdate();
        }
        //选牌结束后进行相关处理
        //将牌加入到牌组中
        foreach (var data in hasChosenCardList)
        {
            deskCardList.Add(data);
        }
        //隐藏UI
        UIManager.Instance.HideSingleUI(E_UiId.ChooseCardUI);
        isChoosing = false;
        hasChosenCardList.Clear();
        
        yield break;
    }
    void Start()
    {

    }

    void Update()
    {

    }

    private void Awake()
    {
        EventDispatcher.AddListener(E_MessageType.GameStart, InitDeskManager);
        EventDispatcher.AddListener(E_MessageType.BattleStart, InitDrawCardDesk);
    }
}
