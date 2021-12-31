using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using UnityEngine.UI;
public class EventUI : BaseUI
{
    //事件面板
    private GameObject eventTable;
    //页面图片
    private Image img_Event;
    //页面描述
    private Text text_Des;
    //页面选择按钮列表
    private List<Button> btn_Choices = new List<Button>();
    //选择按钮文本列表
    private List<Text> text_Choices = new List<Text>();
    //当前choiceResult列表
    private List<EventResultData> resultList = new List<EventResultData>();
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        eventTable = GameTool.FindTheChild(gameObject, "EventTable").gameObject;
        img_Event = GameTool.GetTheChildComponent<Image>(eventTable, "Img_Event");
        text_Des = GameTool.GetTheChildComponent<Text>(eventTable, "Text_Des");
        btn_Choices.Add(GameTool.GetTheChildComponent<Button>(eventTable, "Btn_Choice1"));
        text_Choices.Add(GameTool.GetTheChildComponent<Text>(eventTable, "Text_Choice1"));

        btn_Choices.Add(GameTool.GetTheChildComponent<Button>(eventTable, "Btn_Choice2"));
        text_Choices.Add(GameTool.GetTheChildComponent<Text>(eventTable, "Text_Choice2"));

        btn_Choices.Add(GameTool.GetTheChildComponent<Button>(eventTable, "Btn_Choice3"));
        text_Choices.Add(GameTool.GetTheChildComponent<Text>(eventTable, "Text_Choice3"));

    }

    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.EventUI;
        this.uiType.uiRootType = E_UIRootType.KeepAbove;
    }
    public void ShowPage(int pageId, string add = "")
    {
        EventPageData data = GameEventManager.Instance.eventPageDic[pageId];

        //读取页面图片
        img_Event.sprite = ResourcesManager.Instance.LoadResources<Sprite>(data.resourcePath);
        //读取描述和选项
        text_Des.text = data.pageDes + add;
        resultList.Clear();
        foreach (var item in btn_Choices)
        {
            item.onClick.RemoveAllListeners();
        }
        for (int i = 0; i < data.resultList.Count; i++)
        {
            resultList.Add(data.resultList[i]);
        }
        //先将按钮全部禁用
        foreach (var btn in btn_Choices)
        {
            btn.gameObject.SetActive(false);
        }
        switch (data.choiceNum)
        {
            case 1:
                text_Choices[1].text = data.resultList[0].choiceDes;
                btn_Choices[1].onClick.AddListener(delegate
                {
                    GameEventManager.Instance.TriggerEvent(resultList[0]);
                });
                btn_Choices[1].gameObject.SetActive(true);
                break;
            case 2:
                text_Choices[1].text = data.resultList[0].choiceDes;
                text_Choices[2].text = data.resultList[1].choiceDes;

                btn_Choices[1].onClick.AddListener(delegate
                    {
                        GameEventManager.Instance.TriggerEvent(resultList[0]);
                    });
                btn_Choices[2].onClick.AddListener(delegate
                {
                    GameEventManager.Instance.TriggerEvent(resultList[1]);
                });
                btn_Choices[1].gameObject.SetActive(true);
                btn_Choices[2].gameObject.SetActive(true);

                break;
            case 3:
                text_Choices[0].text = data.resultList[0].choiceDes;
                text_Choices[1].text = data.resultList[1].choiceDes;
                text_Choices[2].text = data.resultList[2].choiceDes;

                btn_Choices[0].onClick.AddListener(delegate
                    {
                        GameEventManager.Instance.TriggerEvent(resultList[0]);
                    });
                btn_Choices[1].onClick.AddListener(delegate
                {
                    GameEventManager.Instance.TriggerEvent(resultList[1]);
                });
                btn_Choices[2].onClick.AddListener(delegate
                    {
                        GameEventManager.Instance.TriggerEvent(resultList[2]);
                    });
                btn_Choices[0].gameObject.SetActive(true);
                btn_Choices[1].gameObject.SetActive(true);
                btn_Choices[2].gameObject.SetActive(true);
                break;
        }

    }
   
    public override void AddMessageListener()
    {
        base.AddMessageListener();
        EventDispatcher.AddListener<int, string>(E_MessageType.ShowEventPage, ShowPage);
    }
}
