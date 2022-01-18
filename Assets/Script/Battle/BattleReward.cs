using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum RewardType
{
    Gold,//金币
    Card,//卡牌
    Relic,//战利品
}

public class BattleReward : MonoBehaviour,IPointerDownHandler
{
    //奖励类型
    public RewardType rewardType;
    //金币奖励值
    public int goldReward;
    //奖励文本
    public Text rewardText;
    //初始化
    public void InitBattleReward(RewardType type)
    {
        rewardType = type;
        switch (rewardType)
        {
            case RewardType.Gold:
                goldReward = BattleManager.Instance.battleGold;
                rewardText.text = "金币（" + goldReward + "）";
                break;
            case RewardType.Card:
                rewardText.text = "选择一张卡牌";
                break;
            default:
                break;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        switch (rewardType)
        {
            //获取金币
            case RewardType.Gold:
                GameManager.Instance.GetGold(goldReward);
                this.gameObject.SetActive(false);
                break;
            //选牌
            case RewardType.Card:
                //选牌
                GameManager.Instance.GetCardAfterBattle(BattleManager.Instance.currentBattleType);
                this.gameObject.SetActive(false);
                break;
        }
        
    }

    private void Awake()
    {
        rewardText=GameTool.GetTheChildComponent<Text>(gameObject, "Text");
    }
}
