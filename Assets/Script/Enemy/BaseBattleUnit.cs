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

    //受到伤害
    public virtual void TakeDamage(int damage)
    {
        //TODO：实际伤害结算
        Debug.Log("受到"+damage+"点伤害");
    }

    //触发器相关事件
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        
    }
}
