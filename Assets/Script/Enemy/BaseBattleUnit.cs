using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBattleUnit : MonoBehaviour
{
    //�������ֵ�͵�ǰ����ֵ
    protected int maxHp;
    protected int currentHp;

    //����������¼�
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

    }
}
