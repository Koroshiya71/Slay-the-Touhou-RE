using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class BattleRewardUI : BaseUI
{
    //�رհ�ť
    private Button btn_Close;
    //��������
    private Transform content;
    //ս������Ԥ����
    public GameObject battleRewardPrefab;
    //ս��������Ϸ�����б�
    public List<GameObject> battleRewardList = new List<GameObject>();
    //��ʼ������
    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.BattleRewardUI;
        this.uiType.uiRootType = E_UIRootType.KeepAbove;
        uiType.showMode = E_ShowUIMode.DoNothing;

        //��ȡս������Ԥ����
        battleRewardPrefab =
            ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/" + "Battle/" + "BattleReward");
    }
    //��ʼ��UI
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        btn_Close = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_Close");
        //ע�ᰴť����¼�
        btn_Close.onClick.AddListener(delegate
        {
            //����UI
            UIManager.Instance.HideSingleUI(E_UiId.BattleRewardUI);
            //��Ϸ�浵
            SaveManager.SaveGame();
        });
        content = GameTool.FindTheChild(gameObject, "Content");
    }
    //����ս����Ϣ��ʾ�������
    public void ShowReward()
    {
        //�������
        int rewardNo = 0;
        //����н�ҽ�������ʾ���reward
        if (BattleManager.Instance.battleGold>0)
        {
            //���������Ϸ������ֱ�����³�ʼ������
            if (battleRewardList.Count>rewardNo)
            {
                battleRewardList[rewardNo].SetActive(true);
                battleRewardList[rewardNo].GetComponent<BattleReward>().InitBattleReward(RewardType.Gold);
            }
            else
            {

                GameObject goldRewardGo = Instantiate(battleRewardPrefab);
                goldRewardGo.transform.SetParent(content);
                goldRewardGo.transform.localScale = new Vector3(1, 1);
                BattleReward goldReward = goldRewardGo.GetComponent<BattleReward>();
                goldReward.InitBattleReward(RewardType.Gold);
                battleRewardList.Add(goldRewardGo);
            }

            rewardNo++;
        }

        if (battleRewardList.Count > rewardNo)
        {
            battleRewardList[rewardNo].SetActive(true);
            battleRewardList[rewardNo].GetComponent<BattleReward>().InitBattleReward(RewardType.Card);
        }
        else
        {
            //��ʾѡ��reward
            GameObject cardRewardGo = Instantiate(battleRewardPrefab);
            cardRewardGo.transform.SetParent(content);
            cardRewardGo.transform.localScale = new Vector3(1, 1);
            BattleReward cardReward = cardRewardGo.GetComponent<BattleReward>();
            cardReward.InitBattleReward(RewardType.Card);
            battleRewardList.Add(cardRewardGo);
        }
        
    }
    //����¼�����
    public override void AddMessageListener()
    {
        base.AddMessageListener();
        EventDispatcher.AddListener(E_MessageType.BattleReward,ShowReward);
    }
}
