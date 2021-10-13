using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ActionImg : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    //所属的Enemy对象
    private Enemy thisEnemy;
    //事件触发器
    private EventTrigger eventTrigger;
    private void Awake()
    {
        //获取所属的敌人对象，并初始禁用这些文字的显示
        thisEnemy=GetComponentInParent<Enemy>();
        
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        thisEnemy.ShowActionDes();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        thisEnemy.HideActionDes();
    }
}
