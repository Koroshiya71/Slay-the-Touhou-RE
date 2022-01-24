using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
    [FormerlySerializedAs("hasChoose")] public bool hasChosen = false;

    //已选牌列表
    public List<CardData> hasChosenCardList = new List<CardData>();

    //强化牌的原始地址
    public int originalIndex = 0;

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

            //洗牌
            drawCardDeskList = Shuffle(drawCardDeskList);
        }
        //如果抽牌堆中的卡牌数量不为0,则将所有弃牌堆中的卡牌加入抽牌堆
        else if (disCardDeskList.Count > 1)
        {
            for (int i = 0; i < disCardDeskList.Count; i++)
            {
                drawCardDeskList.Add(disCardDeskList[i]);
                disCardDeskList.Remove(disCardDeskList[i]);
            }

            //洗牌
            drawCardDeskList = Shuffle(drawCardDeskList);
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
        //清空手牌
        for (int i = 0; i < HandCardManager.Instance.handCardGoList.Count; i++)
        {
            var handCard = HandCardManager.Instance.handCardGoList[i];
            Destroy(handCard);
        }

        HandCardManager.Instance.handCardGoList.Clear();
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
        if (data.hasModified)
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

    //抽取特定卡牌方法
    public void DrawTargetCard(CardData data)
    {
        //从抽牌堆或弃牌堆抽取这张牌
        if (drawCardDeskList.Contains(data))
        {
            drawCardDeskList.Remove(data);
        }
        else if (disCardDeskList.Contains(data))
        {
            disCardDeskList.Remove(data);
        }
        //如果都妹有则跳过
        else
        {
            return;
        }

        if (data.hasModified)
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
    public IEnumerator ChooseCardAddToDesk(int chooseNum, List<CardData> cardDataList)
    {
        //显示UI和对应的卡牌
        isChoosing = true;
        hasChosen = false;
        UIManager.Instance.ShowUI(E_UiId.ChooseCardUI);
        EventDispatcher.TriggerEvent<List<CardData>, ChooseType>(E_MessageType.ShowChooseCardUI, cardDataList,
            ChooseType.AddToDesk);
        //等待选牌结束
        while (hasChosenCardList.Count < chooseNum && isChoosing)
        {
            yield return new WaitForFixedUpdate();
        }

        //选牌结束后进行相关处理
        if (hasChosenCardList.Count <= 0)
        {
            hasChosen = false;
        }
        else
        {
            hasChosen = true;
            //将牌加入到牌组中
            foreach (var data in hasChosenCardList)
            {
                deskCardList.Add(data);
            }
        }

        //隐藏UI
        UIManager.Instance.HideSingleUI(E_UiId.ChooseCardUI);
        isChoosing = false;
        hasChosenCardList.Clear();

        yield break;
    }

    //强化卡牌携程
    public IEnumerator BuffCardCoroutine()
    {
        //显示UI和对应的卡牌
        isChoosing = true;
        hasChosen = false;
        UIManager.Instance.ShowUI(E_UiId.DisplayCardUI);
        //显示牌库
        EventDispatcher.TriggerEvent<List<CardData>, ShowType>(
            E_MessageType.DisplayCard, DeskManager.Instance.deskCardList, ShowType.Desk);
        //等待选牌结束
        while (hasChosenCardList.Count < 1 && isChoosing)
        {
            yield return new WaitForFixedUpdate();
        }

        //随机选取三种强化
        List<CardBuffData> newBuffList = new List<CardBuffData>();
        while (newBuffList.Count < 3)
        {
            //随机获取一个Buff
            CardBuffData newBuff =
                GameManager.Instance.cardBuffList[Random.Range(0, GameManager.Instance.cardBuffList.Count)];
            //是否适配
            bool fit = false;
            switch (newBuff.needEffectType)
            {
                case NeedEffectType.Any:
                    fit = true;
                    break;
                case NeedEffectType.Basic:
                    foreach (var effect in hasChosenCardList[0].cardEffectDic.Values)
                    {
                        if (effect.effectType == CardEffectType.Damage || effect.effectType == CardEffectType.Shield)
                        {
                            fit = true;
                        }
                    }

                    break;
                case NeedEffectType.Buff:
                    foreach (var effect in hasChosenCardList[0].cardEffectDic.Values)
                    {
                        if (effect.effectType == CardEffectType.Buff)
                        {
                            fit = true;
                        }
                    }

                    break;
                case NeedEffectType.Basic_Buff:
                    bool fit0 = false, fit1 = false;
                    foreach (var effect in hasChosenCardList[0].cardEffectDic.Values)
                    {
                        if (effect.effectType == CardEffectType.Buff)
                        {
                            fit0 = true;
                        }

                        if (effect.effectType == CardEffectType.Damage || effect.effectType == CardEffectType.Shield)
                        {
                            fit1 = true;
                        }
                    }

                    if (fit0 && fit1)
                    {
                        fit = true;
                    }

                    break;
            }

            if (newBuffList.Contains(newBuff) || !fit)
            {
                continue;
            }

            newBuffList.Add(newBuff);
        }
        //添加强化后的卡牌

        List<CardData> cardDataList = new List<CardData>();


            for (int i = 0; i < 3; i++)
            {
                CardData upgradeData = new CardData(hasChosenCardList[0].cardID);

                List<int> keys = upgradeData.cardEffectDic.Keys.ToList();
                switch (newBuffList[i].buffId)
                {
                    //费用-1
                    case 1001:
                        upgradeData.cardCost -= 1;
                        break;
                    //效果*1.5
                    case 1002:
                        foreach (var t in keys)
                        {
                            var effect = upgradeData.cardEffectDic[t];


                            if (effect.effectType == CardEffectType.Damage ||
                                effect.effectType == CardEffectType.Shield)
                            {
                                upgradeData.cardEffectDic.Remove(effect.EffectID);
                                effect.EffectValue = (int) (1.5f * effect.EffectValue);
                                upgradeData.cardEffectDic.Add(effect.EffectID, effect);
                            }
                        }

                        break;
                    //抽一张牌
                    case 1003:
                        upgradeData.cardEffectDic.Add(1010, new CardEffectData(1010, 1));
                        break;
                    //附加效果加2层
                    case 1004:
                        foreach (var effect in upgradeData.cardEffectDic.Values)
                        {
                            if (effect.effectType == CardEffectType.Buff)
                            {
                                effect.EffectValue += 2;
                                break;
                            }
                        }

                        break;
                    //附加效果加1层，数值*1.25
                    case 1005:
                        foreach (var effect in upgradeData.cardEffectDic.Values)
                        {
                            if (effect.effectType == CardEffectType.Buff)
                            {
                                effect.EffectValue++;
                            }

                            if (effect.effectType == CardEffectType.Damage ||
                                effect.effectType == CardEffectType.Shield)
                            {
                                effect.EffectValue = (int) (1.25f * effect.EffectValue);
                            }
                        }

                        break;
                }

                upgradeData.hasModified = true;
                upgradeData.UpdateEffectDes();
                cardDataList.Add(upgradeData);
            }
        

        hasChosenCardList.Clear();
        UIManager.Instance.ShowUI(E_UiId.ChooseCardUI);
        EventDispatcher.TriggerEvent<List<CardData>, ChooseType>(E_MessageType.ShowChooseCardUI, cardDataList,
            ChooseType.BuffCard);
        //等待选牌结束
        while (hasChosenCardList.Count < 1 && isChoosing)
        {
            yield return new WaitForFixedUpdate();
        }
        
        //进行强化处理
        deskCardList[originalIndex] = new CardData(hasChosenCardList[0]);
        Debug.Log(deskCardList[originalIndex].cardCost);
        //隐藏UI
        UIManager.Instance.HideSingleUI(E_UiId.ChooseCardUI);
        UIManager.Instance.HideSingleUI(E_UiId.DisplayCardUI);
        isChoosing = false;
        hasChosenCardList.Clear();

        yield break;
    }

    //打乱列表
    public List<T> Shuffle<T>(List<T> original)
    {
        System.Random randomNum = new System.Random();
        int index = 0;
        T temp;
        for (int i = 0; i < original.Count; i++)
        {
            index = randomNum.Next(0, original.Count - 1);
            if (index != i)
            {
                temp = original[i];
                original[i] = original[index];
                original[index] = temp;
            }
        }

        return original;
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