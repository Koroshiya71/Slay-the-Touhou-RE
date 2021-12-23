using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

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


    //本场战斗是否已经初始化过抽牌堆
    public bool hasInitDrawCardDesk = false;

    //初始化牌库管理器
    public void InitDeskManager()
    {
        string[] baseDesk;
        if (GameTool.HasKey("BaseDesk"))
        {
            baseDesk = GameTool.GetString("BaseDesk").Split(';');
        }
        else
        {
            GameTool.SetString("BaseDesk", "1001;1001;1001;1002;1002;1002");
            baseDesk = GameTool.GetString("BaseDesk").Split(';');
        }

        foreach (var cardIDStr in baseDesk)
        {
            deskCardList.Add(new CardData(int.Parse(cardIDStr)));
        }
    }
    //初始化抽牌堆
    public void InitDrawCardDesk()
    {
        //如果弃牌堆为空且从未初始化过抽牌堆，则从牌库进行第一次初始化
        if (disCardDeskList.Count==0&&!hasInitDrawCardDesk)
        {
            hasInitDrawCardDesk = true;

            //将牌库中所有卡牌加入抽牌堆
            foreach (var cardData in deskCardList)
            {
                drawCardDeskList.Add(cardData);
            }
        }
        //如果抽牌堆中的卡牌数量不为0,则将所有弃牌堆中的卡牌加入抽牌堆
        else if (disCardDeskList.Count>1)
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
        if (drawCardDeskList.Count==0)
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
            HandCardManager.Instance.GetCardByID(data.CardID);
        }
        drawCardDeskList.Remove(data);

    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void Awake()
    {
        EventDispatcher.AddListener(E_MessageType.GameStart,InitDeskManager);
        EventDispatcher.AddListener(E_MessageType.BattleStart,InitDrawCardDesk);
    }
}
