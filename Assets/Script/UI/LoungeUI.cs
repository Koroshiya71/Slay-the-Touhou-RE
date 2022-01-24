using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class LoungeUI : BaseUI
{
    //ʣ��ʱ���ı�
    private Text Text_Time;
    //��Ϣ��ť
    private Button lounge_Rest;

    private Text text_RestTip;
    //��ϰ��ť
    private Button lounge_Study;
    private Text text_StudyTip;

    //�رհ�ť
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
        Text_Time.text = "ʣ��ʱ�䣺" + GameManager.Instance.loungeData.loungeTime;

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
            //��ǰ���ڲ���+1
            GameSceneManager.Instance.currentLayer++;
            GameSceneManager.Instance.UpdateGameSceneState();
            //�浵
            SaveManager.SaveGame();
        });
    }

    //��ʾLoungeUI����
    public void ShowLoungeUI()
    {
        UpdateLoungeUI();
    }
    
    
    //����UI����
    public void UpdateLoungeUI()
    {
        Text_Time.text= "ʣ��ʱ�䣺" + GameManager.Instance.loungeData.loungeTime;
        text_RestTip.text = "����" + GameManager.Instance.loungeData.restCost + "ʱ�䣬�ظ�30%����ֵ";
        text_StudyTip.text = "����" + GameManager.Instance.loungeData.studyCost + "ʱ�䣬����һ�ſ���";
    }
}
