using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBattleUnit : MonoBehaviour
{
    //最大生命值和当前生命值
    protected int maxHp;
    protected int currentHp;

    //触发器相关事件
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

    }
}
