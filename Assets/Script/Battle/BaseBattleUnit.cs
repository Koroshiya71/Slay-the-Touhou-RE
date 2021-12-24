using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;
public class BaseBattleUnit : MonoBehaviour
{
    //生命值相关
    protected int maxHp = 50;
    protected int currentHp = 50;
    protected Slider slider_Hp;
    protected Text text_Hp;

    //护盾值相关
    protected int currentShield = 0;
    protected Image img_Shield;
    protected Text text_Shield;

    //选中特效
    protected GameObject selectEffect;

    //状态字典
    public Dictionary<int, StateData> stateDic = new Dictionary<int, StateData>();

    //是玩家还是敌人
    public bool isPlayer;

    //初始化UI和数据对象
    public virtual void Init(int id = 1)
    {
        InitDataOnAwake(id);
        InitUIOnAwake();

        InitEventOnAwake();
    }

    protected virtual void Awake()
    {
    }

    //受到伤害
    public virtual void TakeDamage(int damage)
    {
        //如果有护盾，优先扣除护盾
        if (currentShield >= damage)
        {
            //护盾值大于等于伤害值时不造成生命值伤害
            currentShield -= damage;
            UpdateUI();
            return;
        }

        //护盾值小于伤害值时优先使用护盾抵消部分伤害
        damage -= currentShield;
        currentShield = 0;
        currentHp -= damage;

        UpdateUI();
        if (currentHp <= 0)
        {
            Die();
        }
    }
    //更新UI
    public virtual void UpdateUI()
    {
        //更新血条UI
        slider_Hp.value = (float)currentHp / maxHp;
        text_Hp.text = currentHp + " / " + maxHp;
        //如果护盾值小于等于0则取消相关UI的显示
        if (currentShield <= 0)
        {
            img_Shield.enabled = false;
            text_Shield.enabled = false;
        }
        //否则就显示对应的护盾值
        else
        {
            img_Shield.enabled = true;
            text_Shield.enabled = true;
            text_Shield.text = currentShield.ToString();
        }
    }
    //获得护甲
    public virtual void GetShield(int shield)
    {
        currentShield += shield;
        UpdateUI();
    }
    //生命值归零时的死亡方法
    public virtual void Die()
    {

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

        //初始化护盾值和护盾UI
        img_Shield = GameTool.GetTheChildComponent<Image>(gameObject, "Img_Shield");

        text_Shield = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Shield");
        //如果初始护盾值为0则取消相关UI的显示
        if (currentShield == 0)
        {
            img_Shield.enabled = false;
            text_Shield.enabled = false;
        }
        //否则就显示对应的护盾值
        else
        {
            img_Shield.enabled = true;
            text_Shield.text = currentShield.ToString();
        }

    }

    //初始化事件
    protected virtual void InitEventOnAwake()
    {
        EventDispatcher.AddListener(E_MessageType.TurnStart, delegate
        {
            if (!BattleManager.Instance.isInit)
            {
                currentShield = 0;
                UpdateUI();
            }
        });
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