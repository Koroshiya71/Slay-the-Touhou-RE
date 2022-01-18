using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum RewardType
{
    Gold,//���
    Card,//����
    Relic,//ս��Ʒ
}

public class BattleReward : MonoBehaviour,IPointerDownHandler
{
    //��������
    public RewardType rewardType;
    //��ҽ���ֵ
    public int goldReward;
    //�����ı�
    public Text rewardText;
    //��ʼ��
    public void InitBattleReward(RewardType type)
    {
        rewardType = type;
        switch (rewardType)
        {
            case RewardType.Gold:
                goldReward = BattleManager.Instance.battleGold;
                rewardText.text = "��ң�" + goldReward + "��";
                break;
            case RewardType.Card:
                rewardText.text = "ѡ��һ�ſ���";
                break;
            default:
                break;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        switch (rewardType)
        {
            //��ȡ���
            case RewardType.Gold:
                GameManager.Instance.GetGold(goldReward);
                this.gameObject.SetActive(false);
                break;
            //ѡ��
            case RewardType.Card:
                //ѡ��
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
