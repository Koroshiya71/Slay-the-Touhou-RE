using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : BaseUI
{
    //�̵꿨��content
    private Transform storeCardContent;
    //�̵�����content
    private Transform storeRelicContent;
    //�ر�ҳ��Btn
    private Button btn_Exit;
    //�̵귷���Ŀ��������б�
    private List<GameObject> storeCardObjList = new List<GameObject>();
    //�̵귷�������������б�
    private List<GameObject> storeRelicObjList = new List<GameObject>();
    //�̵꿨��Ԥ����
    private GameObject storeCardPrefab;
    //�̵�����Ԥ����
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
    //��ʾ�̵�UI
    public void ShowStoreUI()
    {
        //���ɿ���
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
            //���ȡһ���̵��п���ˢ�µĿ���ID
            int newCardID = GameManager.Instance.storeCardPriceDic.Keys.ToArray()
                [Random.Range(0, GameManager.Instance.storeCardPriceDic.Count)];
            //����Ѵ����������ȡ
            if (hasInitCardId.Contains(newCardID))
            {
                continue;
            }
            storeCardObjList[hasInitCardId.Count].GetComponent<StoreCard>().InitCard(newCardID);
            hasInitCardId.Add(newCardID);
        }
        //��������
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
            //���ȡһ���̵��п���ˢ�µ�����ID
            int newRelicID = GameManager.Instance.storeRelicPriceDic.Keys.ToArray()
                [Random.Range(0, GameManager.Instance.storeRelicPriceDic.Count)];

            //����Ѵ����������ȡ
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
            //��ǰ���ڲ���+1
            GameSceneManager.Instance.currentLayer++;
            GameSceneManager.Instance.UpdateGameSceneState();
            //�浵
            SaveManager.SaveGame();
        });
    }
}
