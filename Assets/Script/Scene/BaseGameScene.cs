using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BaseGameScene : MonoBehaviour
{
    //场景数据
    public SceneData sceneData;

    //图片资源
    protected Image sceneImage;

    protected Image sceneMask;

    //Button组件
    protected Button gameSceneButton;

    //该场景的战斗数据
    protected BattleData battleData = null;

    //该场景的事件数据
    protected EventData eventData = null;

    //是否可以选择
    protected bool isActive = false;

    //初始化场景
    public virtual void InitGameScene()
    {
        //获取Image和Button组件
        sceneImage = GetComponent<Image>();
        sceneMask = GameTool.GetTheChildComponent<Image>(gameObject, "Mask");
        sceneMask.enabled = false;
        gameSceneButton = GetComponent<Button>();
        //初始化场景数据和图片素材
        //新生成的场景数据的下标正好为将该场景加入列表前的场景数
        sceneData = new SceneData(GameSceneManager.Instance.inGameSceneList.Count);
        sceneImage.sprite = ResourcesManager.Instance.LoadResources<Sprite>(sceneData.ResourcePath);
        GameSceneManager.Instance.inGameSceneList.Add(this);

        //根据事件类型注册点击事件
        InitClickEvent();
    }

    //重新获取场景图片
    public virtual void ResetSprite()
    {
        Debug.Log(12);
        switch (sceneData.sceneType)
        {
            case SceneType.NormalCombat:
                sceneData.resourcePath = "Image/" + "UIImage/" + "GameScene/" + "NormalCombat";
                break;
            case SceneType.EliteCombat:
                sceneData.resourcePath = "Image/" + "UIImage/" + "GameScene/" + "EliteCombat";
                break;
            case SceneType.Store:
                sceneData.resourcePath = "Image/" + "UIImage/" + "GameScene/" + "Store";
                break;
            case SceneType.Lounge:
                sceneData.resourcePath = "Image/" + "UIImage/" + "GameScene/" + "Lounge";
                break;
            case SceneType.BossCombat:
                sceneData.resourcePath = "Image/" + "UIImage/" + "GameScene/" + "BossCombat";
                break;
            case SceneType.Event:
                sceneData.resourcePath = "Image/" + "UIImage/" + "GameScene/" + "Event";
                break;
        }
        Debug.Log(12);

        sceneImage.sprite = ResourcesManager.Instance.LoadResources<Sprite>(sceneData.ResourcePath);
        Debug.Log(12);

    }

    //根据事件类型注册事件
    protected virtual void InitClickEvent()
    {
        gameSceneButton.onClick.AddListener(delegate
        {
            GameSceneManager.Instance.lastIndex = GameSceneManager.Instance.inGameSceneList.IndexOf(this);
        });
        //联机情况先发送进入场景消息
        if (GameManager.Instance.isMulti)
        {
            gameSceneButton.onClick.AddListener(delegate
            {
                MsgChooseScene msg = new MsgChooseScene();
                msg.id = NetManager.playerID;
                msg.index = GameSceneManager.Instance.inGameSceneList.IndexOf(this);
                msg.type = sceneData.sceneType;
                BattleData newBattleData=new BattleData(1);
                EventData newEventData=new EventData(1);
                switch (sceneData.sceneType)
                {
                    //如果场景类型为普通战斗，则随机选取一个战斗场景数据
                    case SceneType.NormalCombat:
                        while (true)
                        {
                            newBattleData =
                                BattleManager.Instance.battleDataDic[
                                    Random.Range(1, BattleManager.Instance.battleDataDic.Count + 1)];
                            if (newBattleData.BattleType == BattleType.Normal)
                                break;
                        }

                        break;
                    //如果场景类型为事件，则随机选取一个事件
                    case SceneType.Event:
                        newEventData =
                            GameEventManager.Instance.eventDic[
                                GameEventManager.Instance.eventIDList1[
                                    Random.Range(0, GameEventManager.Instance.eventIDList1.Count)]];
                        gameSceneButton.onClick.AddListener(delegate
                        {
                            UIManager.Instance.ShowUI(E_UiId.EventUI);
                            EventDispatcher.TriggerEvent<int, string>(E_MessageType.ShowEventPage,
                                eventData.pageDataList[0].pageID, "");
                        });
                        break;
                    //如果场景类型为精英战斗，则随机选取一个精英战斗
                    case SceneType.EliteCombat:
                        while (true)
                        {
                            newBattleData =
                                BattleManager.Instance.battleDataDic[
                                    Random.Range(1, BattleManager.Instance.battleDataDic.Count + 1)];
                            if (newBattleData.BattleType == BattleType.Elite)
                                break;
                        }
                        break;
                    //如果是Boss战，则初始化一场Boss战斗
                    case SceneType.BossCombat:
                        while (true)
                        {
                            newBattleData =
                                BattleManager.Instance.battleDataDic[
                                    Random.Range(1, BattleManager.Instance.battleDataDic.Count + 1)];
                            if (newBattleData.BattleType == BattleType.Boss)
                                break;
                        }
                        break;
                    default:
                        while (true)
                        {
                            newBattleData =
                                BattleManager.Instance.battleDataDic[
                                    Random.Range(1, BattleManager.Instance.battleDataDic.Count + 1)];
                            if (newBattleData.BattleType == BattleType.Normal)
                                break;
                        }
                        break;
                }

                Debug.Log(newBattleData.BattleID);
                msg.battleDataStr = JsonConvert.SerializeObject(newBattleData);
                msg.eventDataStr = JsonConvert.SerializeObject(newEventData);
                MultiPlayMsgHandler.currentBattleData = newBattleData;
                MultiPlayMsgHandler.currentEventData = newEventData;
                NetManager.Send(msg);

            });

        }
        //否则直接注册事件
        else
        {
            switch (sceneData.sceneType)
            {
                //如果场景类型为普通战斗，则随机选取一个战斗场景数据
                case SceneType.NormalCombat:
                    while (true)
                    {
                        battleData =
                            BattleManager.Instance.battleDataDic[
                                Random.Range(1, BattleManager.Instance.battleDataDic.Count + 1)];
                        if (battleData.BattleType == BattleType.Normal)
                            break;
                    }

                    gameSceneButton.onClick.AddListener(delegate { BattleManager.Instance.InitBattle(battleData); });


                    break;
                //如果场景类型为事件，则随机选取一个事件
                case SceneType.Event:
                    eventData =
                        GameEventManager.Instance.eventDic[
                            GameEventManager.Instance.eventIDList1[
                                Random.Range(0, GameEventManager.Instance.eventIDList1.Count)]];
                    gameSceneButton.onClick.AddListener(delegate
                    {
                        UIManager.Instance.ShowUI(E_UiId.EventUI);
                        EventDispatcher.TriggerEvent<int, string>(E_MessageType.ShowEventPage,
                            eventData.pageDataList[0].pageID, "");
                    });
                    break;
                //如果场景类型为精英战斗，则随机选取一个精英战斗
                case SceneType.EliteCombat:
                    while (true)
                    {
                        battleData =
                            BattleManager.Instance.battleDataDic[
                                Random.Range(1, BattleManager.Instance.battleDataDic.Count + 1)];
                        if (battleData.BattleType == BattleType.Elite)
                            break;
                    }

                    gameSceneButton.onClick.AddListener(delegate { BattleManager.Instance.InitBattle(battleData); });
                    break;
                //如果场景类型为商店，则显示商店页面
                case SceneType.Store:
                    gameSceneButton.onClick.AddListener(delegate
                    {
                        UIManager.Instance.ShowUI(E_UiId.StoreUI);
                        EventDispatcher.TriggerEvent(E_MessageType.ShowStoreUI);
                    });
                    break;
                //如果场景类型为休息处，则回复时间并显示休息处页面
                case SceneType.Lounge:
                    gameSceneButton.onClick.AddListener(delegate
                    {
                        UIManager.Instance.ShowUI(E_UiId.LoungeUI);
                        GameManager.Instance.loungeData.loungeTime = GameManager.Instance.loungeData.maxLoungeTime;
                        EventDispatcher.TriggerEvent(E_MessageType.ShowLoungeUI);
                    });

                    break;
                //如果是Boss战，则初始化一场Boss战斗
                case SceneType.BossCombat:
                    while (true)
                    {
                        battleData =
                            BattleManager.Instance.battleDataDic[
                                Random.Range(1, BattleManager.Instance.battleDataDic.Count + 1)];
                        if (battleData.BattleType == BattleType.Boss)
                            break;
                    }

                    gameSceneButton.onClick.AddListener(delegate { BattleManager.Instance.InitBattle(battleData); });
                    break;
            }
        }
        GameSceneManager.Instance.UpdateGameSceneState();
    }

    //改变场景可选状态
    public void ChangeGameSceneState(bool state)
    {
        this.isActive = state;
        if (!isActive)
        {
            gameSceneButton.interactable = false;
            sceneMask.enabled = true;
        }
        else
        {
            gameSceneButton.interactable = true;
            sceneMask.enabled = false;
        }
    }

    void Update()
    {
    }

    protected virtual void Awake()
    {
        InitGameScene();
    }
}