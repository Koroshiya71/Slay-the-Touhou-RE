using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;
using UnityEngine.EventSystems;

public class State : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //状态数据
    public StateData stateData;
    //状态描述文本
    private Text text_StateDes;

    //状态层数文本
    private Text text_Stack;
    private void Awake()
    {
        text_StateDes = GameTool.GetTheChildComponent<Text>(gameObject, "Text_StateDes");
        text_StateDes.enabled = false;
        text_Stack = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Stack");
        text_Stack.enabled = false;
    }

    //鼠标悬浮时，显示状态描述
    public void OnPointerEnter(PointerEventData eventData)
    {
        text_StateDes.text = stateData.stateDes;
        text_StateDes.enabled = true;
    }
    //鼠标悬浮时，隐藏状态描述
    public void OnPointerExit(PointerEventData eventData)
    {
        text_StateDes.enabled = false;

    }
    //更新UI
    public void UpdateStateUI()
    {
        if (stateData == null)
        {
            return;
        }
        if (stateData.showStack)
        {
            text_Stack.text = stateData.stateStack.ToString();
            text_Stack.enabled = true;
        }
        else
            text_Stack.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
