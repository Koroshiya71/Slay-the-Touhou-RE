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
    public Dictionary<int, State> stateDic = new Dictionary<int, State>();

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
        //灵体检测
        if (StateManager.CheckState(this, 1001))
        {
            damage = (int)(0.7f * damage);
            Debug.Log(damage);
        }
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

    #region 状态相关
    //回合结束更新状态字典
    public void UpdateStateDic()
    {
        foreach (var state in stateDic.Values)
        {
            if (state.stateData.reduceOverTurn)
            {
                state.stateData.stateStack -= 1;
            }
        }
        //检查有没有层数<=0的状态直接将其清除
        CheckClearState();
        //更新层数显示
        foreach (var state in stateDic.Values)
        {
            state.UpdateStateUI();
        }
    }
    //检测清除状态
    public void CheckClearState()
    {
        //需要清除的状态列表
        List<State> clearList = new List<State>();
        foreach (var state in stateDic.Values)
        {
            if (state.stateData.stateStack <= 0)
            {
                clearList.Add(state);
            }
        }
        foreach (var clearState in clearList)
        {
            //从字典中移除
            stateDic.Remove(clearState.stateData.stateID);
            //销毁游戏物体
            Destroy(clearState.gameObject);
        }
    }
    //清除所有状态
    public void ClearAllState()
    {
        foreach (var state in stateDic.Values)
        {
            Destroy(state.gameObject);
        }
        stateDic.Clear();
    }
    #endregion



    //生命值归零时的死亡方法
    public virtual void Die()
    {

    }
    #region 初始化   
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
        //回合开始时初始化UI
        EventDispatcher.AddListener(E_MessageType.TurnStart, delegate
        {
            if (!BattleManager.Instance.isInit)
            {
                currentShield = 0;
                UpdateUI();
            }
        });
        //回合结束时更新状态
        EventDispatcher.AddListener(E_MessageType.TurnEnd, delegate
        {
            UpdateStateDic();
        });
    }
    //初始化数据
    protected virtual void InitDataOnAwake(int id)
    {
        
    }
    #endregion

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