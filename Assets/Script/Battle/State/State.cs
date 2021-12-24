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
    void Start()
    {
        text_StateDes = GameTool.GetTheChildComponent<Text>(gameObject, "Text_StateDes");
        text_StateDes.enabled = false;
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
    // Update is called once per frame
    void Update()
    {

    }
}
