using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBattleUnit : MonoBehaviour
{
    //生命值相关
    protected int maxHp=50;
    protected int currentHp=50;

    private Slider slider_Hp;
    private Text text_Hp;
    //选中特效
    protected GameObject selectEffect;
    
    //初始化UI和数据对象
    public virtual void Init(int id=1)
    {
        InitDataOnAwake(id);
        InitUIOnAwake();
    }
    protected virtual void Awake()
    {
        
    }

    //受到伤害
    public virtual void TakeDamage(int damage)
    {
        currentHp -= damage;
        slider_Hp.value = (float) currentHp / maxHp;
        text_Hp.text = currentHp + " / " + maxHp;

    }
    //初始化UI
    protected virtual void InitUIOnAwake()
    {
        //获取选中特效对象并在开始时禁用
        selectEffect = GameTool.FindTheChild(gameObject, "Img_Select").gameObject;
        selectEffect.SetActive(false);

        //初始化血量和血条UI
        slider_Hp = GameTool.GetTheChildComponent<Slider>(gameObject, "Slider_Hp");
        text_Hp = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Hp");
        currentHp = maxHp;
        text_Hp.text = currentHp + " / " + maxHp;
    }
    //初始化数据
    protected virtual void InitDataOnAwake(int id)
    {
        
    }
    //触发器相关事件
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        
    }
}
