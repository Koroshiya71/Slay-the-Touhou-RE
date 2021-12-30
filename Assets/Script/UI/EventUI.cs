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
        //先将按钮全部禁用
        foreach (var btn in btn_Choices)
        {
            btn.gameObject.SetActive(false);
        }
        for (int i = 0; i < data.resultList.Count; i++)
        {
            if (i <= 1)
            {

                //显示对应的按钮
                btn_Choices[i + 1].gameObject.SetActive(true);
                text_Choices[i + 1].text = data.resultList[i].choiceDes;
                //TODO：注册对应的事件点击监听
            }
            else
            {

                //显示对应的按钮
                btn_Choices[i + 1].gameObject.SetActive(true);
                text_Choices[i].text = data.resultList[i].choiceDes;
                //TODO：注册对应的事件点击监听
            }
        }

    }
    public override void AddMessageListener()
    {
        base.AddMessageListener();
        EventDispatcher.AddListener<int, string>(E_MessageType.ShowEventPage, ShowPage);
    }
}
