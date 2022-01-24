using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : BaseUI
{
    //商店卡牌content
    private Transform storeCardContent;
    //商店遗物content
    private Transform storeRelicContent;
    //关闭页面Btn
    private Button btn_Exit;
    //商店贩卖的卡牌物体列表
    private List<GameObject> storeCardObjList = new List<GameObject>();
    //商店贩卖的遗物物体列表
    private List<GameObject> storeRelicObjList = new List<GameObject>();
    //商店卡牌预制体
    private GameObject storeCardPrefab;
    //商店遗物预制体
    private GameObject storeRelicPrefab;
    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        uiId = E_UiId.StoreUI;
        uiType.showMode = E_ShowUIMode.DoNothing;
        uiType.uiRootType = E_UIRootType.KeepAbove;
        storeCardPrefab= ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/" + "Card/" + "StoreCard");
        storeRelicPrefab = ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/" + "Battle/" + "StoreRelic");
    }

    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        storeRelicContent = GameTool.FindTheChild(gameObject,"StoreRelicContent");
        storeCardContent = GameTool.FindTheChild(gameObject, "StoreCardContent");
        btn_Exit = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_Exit");
    }
    //显示商店UI
    public void ShowStoreUI()
    {
        //生成卡牌
        for (int i = storeCardObjList.Count; i < 5; i++)
        {
            storeCardObjList.Add(Instantiate(storeCardPrefab));
            storeCardObjList[i].transform.SetParent(storeCardContent);
        }

        foreach (var cardObj in storeCardObjList)
        {
            cardObj.SetActive(true);
        }
        List<int> hasInitCardId = new List<int>();
        while (hasInitCardId.Count<5)
        {
            //随机取一个商店中可以刷新的卡牌ID
            int newCardID = GameManager.Instance.storeCardPriceDic.Keys.ToArray()
                [Random.Range(0, GameManager.Instance.storeCardPriceDic.Count)];
            //如果已存在了则继续取
            if (hasInitCardId.Contains(newCardID))
            {
                continue;
            }
            storeCardObjList[hasInitCardId.Count].GetComponent<StoreCard>().InitCard(newCardID);
            hasInitCardId.Add(newCardID);
        }
        //生成遗物
        for (int i = storeRelicObjList.Count; i < 3; i++)
        {
            storeRelicObjList.Add(Instantiate(storeRelicPrefab));
            storeRelicObjList[i].transform.SetParent(storeRelicContent);
        }
        foreach (var relicObj in storeRelicObjList)
        {
            relicObj.SetActive(true);
        }
        List<int> hasInitRelicId = new List<int>();
        while (hasInitRelicId.Count < 3)
        {
            //随机取一个商店中可以刷新的遗物ID
            int newRelicID = GameManager.Instance.storeRelicPriceDic.Keys.ToArray()
                [Random.Range(0, GameManager.Instance.storeRelicPriceDic.Count)];

            //如果已存在了则继续取
            if (hasInitRelicId.Contains(newRelicID)||RelicManager.Instance.playerRelicDic.ContainsKey(newRelicID))
            {
                continue;
            }
            storeRelicObjList[hasInitRelicId.Count].GetComponent<StoreRelic>().InitRelic(RelicManager.Instance.relicDic[newRelicID]);
            hasInitRelicId.Add(newRelicID);
        }
    }
    public override void AddMessageListener()
    {
        base.AddMessageListener();
        EventDispatcher.AddListener(E_MessageType.ShowStoreUI,ShowStoreUI);

        btn_Exit.onClick.AddListener(delegate
        {
            UIManager.Instance.HideSingleUI(E_UiId.StoreUI);
            //当前所在层数+1
            GameSceneManager.Instance.currentLayer++;
            GameSceneManager.Instance.UpdateGameSceneState();
            //存档
            SaveManager.SaveGame();
        });
    }
}
