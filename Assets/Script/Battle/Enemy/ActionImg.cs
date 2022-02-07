using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ActionImg : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //所属的Enemy对象
    private Enemy thisEnemy;
    //是否是同步Action
    public bool isSyncAction = false;
    //事件触发器
    private void Awake()
    {
        //获取所属的敌人对象，并初始禁用这些文字的显示
        thisEnemy = GetComponentInParent<Enemy>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSyncAction)
        {
            thisEnemy.ShowSyncActionDes();

        }
        else
        {
            thisEnemy.ShowActionDes();

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSyncAction)
        {
            thisEnemy.HideSyncActionDes();

        }
        else
        {
            thisEnemy.HideActionDes();

        }
    }
}
