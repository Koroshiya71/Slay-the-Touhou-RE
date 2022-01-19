using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class BattleRewardUI : BaseUI
{
    //关闭按钮
    private Button btn_Close;
    //内容容器
    private Transform content;
    //战斗奖励预制体
    public GameObject battleRewardPrefab;
    //战斗奖励游戏物体列表
    public List<GameObject> battleRewardList = new List<GameObject>();
    //初始化数据
    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.BattleRewardUI;
        this.uiType.uiRootType = E_UIRootType.KeepAbove;
        uiType.showMode = E_ShowUIMode.DoNothing;

        //获取战斗奖励预制体
        battleRewardPrefab =
            ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/" + "Battle/" + "BattleReward");
    }
    //初始化UI
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        btn_Close = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_Close");
        //注册按钮点击事件
        btn_Close.onClick.AddListener(delegate
        {
            //隐藏UI
            UIManager.Instance.HideSingleUI(E_UiId.BattleRewardUI);
            //游戏存档
            SaveManager.SaveGame();
        });
        content = GameTool.FindTheChild(gameObject, "Content");
    }
    //根据战斗信息显示相关内容
    public void ShowReward()
    {
        //奖励序号
        int rewardNo = 0;
        //如果有金币奖励就显示金币reward
        if (BattleManager.Instance.battleGold>0)
        {
            //如果已有游戏物体则直接重新初始化即可
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
            //显示选牌reward
            GameObject cardRewardGo = Instantiate(battleRewardPrefab);
            cardRewardGo.transform.SetParent(content);
            cardRewardGo.transform.localScale = new Vector3(1, 1);
            BattleReward cardReward = cardRewardGo.GetComponent<BattleReward>();
            cardReward.InitBattleReward(RewardType.Card);
            battleRewardList.Add(cardRewardGo);
        }
        
    }
    //添加事件监听
    public override void AddMessageListener()
    {
        base.AddMessageListener();
        EventDispatcher.AddListener(E_MessageType.BattleReward,ShowReward);
    }
}
