using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBattleUnit : MonoBehaviour
{
    //最大生命值和当前生命值
    protected int maxHp;
    protected int currentHp;

    //选中特效
    protected GameObject selectEffect;
    //初始化对象并获得相关组件
    protected virtual void InitDataOnAwake()
    {
        //获取选中特效对象并在开始时禁用
        selectEffect = GameTool.FindTheChild(gameObject, "Img_Select").gameObject;
        selectEffect.SetActive(false);
    }

    protected virtual void Awake()
    {
        InitDataOnAwake();
    }

    //触发器相关事件
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        //卡牌选中时启用选择特效
        if (other.CompareTag("HandCard"))
        {
            selectEffect.SetActive(true);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        //卡牌离开时禁用选择特效

        if (other.CompareTag("HandCard"))
        {
            selectEffect.SetActive(false);
        }
    }
}
