using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.Serialization;

public class HandCardManager : UnitySingleton<HandCardManager>
{
    //手牌对象父物体
    private GameObject handCardGo;
    //当前手牌数量
    private int currentCardNum=0;
    //手牌上限
    public int maxCardNum = 10;
    //卡牌预制体
    private GameObject handCardPrefab;
    //手牌游戏物体列表
    public List<GameObject> handCardGoList = new List<GameObject>();
    //手牌对象列表
    //选中的卡牌对象
    public HandCard selectedCard=null;
    private void Awake()
    {
        EventDispatcher.AddListener(E_MessageType.BattleStart,InitHandCard);
    }

    //初始化手牌管理类对象
    private void InitHandCard()
    {
        currentCardNum = 0;
        handCardGo = GameObject.Find("Cards_Hand");
        handCardPrefab = ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/Card/HandCard");
    }

    //根据卡牌ID获取卡牌到手牌
    public void GetCardByID(int cardID)
    {
        //如果手牌数大于等于手牌上限则跳出
        if (currentCardNum>=maxCardNum)
        {
            return;
        }
        GameObject newCardGo = Instantiate(handCardPrefab, handCardGo.transform, false);
        newCardGo.transform.localScale = new Vector3(1, 1, 1);
        HandCard newCard = newCardGo.GetComponent<HandCard>();
        newCard.InitCard(cardID);
        handCardGoList.Add(newCardGo);
        currentCardNum++;
        MoveAndRotateCard();
        foreach (var handCard in handCardGoList)
        {
            handCard.GetComponent<HandCard>().SaveOriginalPos();
        }
    }
    //根据卡牌数据获取卡牌到手牌
    public void GetCardByData(CardData data)
    {
        //如果手牌数大于等于手牌上限则跳出
        if (currentCardNum >= maxCardNum)
        {
            return;
        }
        GameObject newCardGo = Instantiate(handCardPrefab, handCardGo.transform, false);
        newCardGo.transform.localScale = new Vector3(1, 1, 1);
        HandCard newCard = newCardGo.GetComponent<HandCard>();
        newCard.InitCard(data);
        handCardGoList.Add(newCardGo);
        currentCardNum++;
        MoveAndRotateCard();
        foreach (var handCard in handCardGoList)
        {
            handCard.GetComponent<HandCard>().SaveOriginalPos();
        }
    }
    //丢弃所有手牌
    public void DisAllCard()
    {
        for (int i = 0; i < handCardGoList.Count; i++)
        {
            var handCard = handCardGoList[i];
            //将卡牌数据加入弃牌堆
            DeskManager.Instance.disCardDeskList.Add(handCard.GetComponent<HandCard>().CardData);
            Destroy(handCard);
        }
        //清空手牌列表
        handCardGoList = new List<GameObject>();
        currentCardNum = 0;
    }
    //更新手牌数值
    public void UpdateCardData()
    {

    }
    //清除所有战斗牌库
    public void ClearAllBattleDesk()
    {
        for (int i = 0; i < handCardGoList.Count; i++)
        {
            var handCard = handCardGoList[i];
            Destroy(handCard);
        }
        handCardGoList.Clear();
    }
    //调整卡牌位置和旋转
    public void MoveAndRotateCard()
    {
        //第一张牌的X坐标和旋转角
        float firstX = -75 * (currentCardNum - 1);
        //总旋转角
        float totalRotate = (currentCardNum) * 1;
        //旋转偏移量
        float offset = 0;
        //中位地址
        int midIndex = currentCardNum / 2;
        //y坐标偏移量
        for (int i = 0; i < currentCardNum; i++)
        {
            float yPosOffset = 0;
            switch (firstX + 150 * i)
            {
                case 150:
                case -150:
                    yPosOffset = 5;
                    break;
                case 300:
                case -300:
                    yPosOffset = 15;
                    break;
                case 450:
                case -450:
                    yPosOffset = 30;
                    break;
                case 600:
                case -600:
                    yPosOffset = 50;
                    break;
                case 225:
                case -225:
                    yPosOffset = 8;
                    break;
                case 375:
                case -375:
                    yPosOffset = 20;
                    break;
                case 525:
                case -525:
                    yPosOffset = 40;
                    break;
                case 675:
                case -675:
                    yPosOffset = 65;
                    break;
                default:
                    yPosOffset = 0;
                    break;
            }
            handCardGoList[i].transform.localPosition = new Vector3(firstX + 150 * i, -yPosOffset, 0);

            if (currentCardNum % 2 == 0 && Mathf.Lerp(totalRotate, -totalRotate, ((float)(i) / currentCardNum)) == 0)
            {
                offset = -2;
            }

            if (i==midIndex&& currentCardNum % 2 == 1)
            {
                handCardGoList[i].transform.localEulerAngles = new Vector3(0, 0, 0);
                offset = -2;
                continue;
            }
            
            handCardGoList[i].transform.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(totalRotate,-totalRotate,((float)(i)/currentCardNum))+offset);


        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetCardByID(1002);
        }
    }
    
    //移除手牌
    public void RemoveCard(GameObject cardGo)
    {
        handCardGoList.Remove(cardGo);
        currentCardNum--;
        Destroy(cardGo);
        MoveAndRotateCard();
    }
}