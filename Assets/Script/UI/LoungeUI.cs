using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class LoungeUI : BaseUI
{
    //剩余时间文本
    private Text Text_Time;
    //休息按钮
    private Button lounge_Rest;

    private Text text_RestTip;
    //研习按钮
    private Button lounge_Study;
    private Text text_StudyTip;

    //关闭按钮
    private Button Btn_Exit;
    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.LoungeUI;
        uiType.showMode = E_ShowUIMode.DoNothing;
        uiType.uiRootType = E_UIRootType.KeepAbove;
        
    }

    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        Text_Time = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Time");
        Text_Time.text = "剩余时间：" + GameManager.Instance.loungeData.loungeTime;

        lounge_Rest = GameTool.GetTheChildComponent<Button>(gameObject, "Lounge_Rest");
        text_RestTip = GameTool.GetTheChildComponent<Text>(gameObject, "Text_RestTip");
        lounge_Study = GameTool.GetTheChildComponent<Button>(gameObject, "Lounge_Study");
        text_StudyTip = GameTool.GetTheChildComponent<Text>(gameObject, "Text_StudyTip");
        Btn_Exit = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_Exit");
    }

    public override void AddMessageListener()
    {
        base.AddMessageListener();
        EventDispatcher.AddListener(E_MessageType.ShowLoungeUI,ShowLoungeUI);
        lounge_Rest.onClick.AddListener(GameManager.Instance.LoungeRest);
        lounge_Study.onClick.AddListener(GameManager.Instance.LoungeBuffCard);
        Btn_Exit.onClick.AddListener(delegate
        {
            HideUI();
            //当前所在层数+1
            GameSceneManager.Instance.currentLayer++;
            GameSceneManager.Instance.UpdateGameSceneState();
            //存档
            SaveManager.SaveGame();
        });
    }

    //显示LoungeUI方法
    public void ShowLoungeUI()
    {
        UpdateLoungeUI();
    }
    
    
    //更新UI方法
    public void UpdateLoungeUI()
    {
        Text_Time.text= "剩余时间：" + GameManager.Instance.loungeData.loungeTime;
        text_RestTip.text = "消耗" + GameManager.Instance.loungeData.restCost + "时间，回复30%生命值";
        text_StudyTip.text = "消耗" + GameManager.Instance.loungeData.studyCost + "时间，升级一张卡牌";
    }
}
