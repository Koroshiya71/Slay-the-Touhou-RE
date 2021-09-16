using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class BattleManager : UnitySingleton<BattleManager>
{
    //当前能量值
    private int currentEnergy=3;
    public int CurrentEnergy => currentEnergy;
    //能量上限
    private int maxEnergy=3;
    public int MaxEnergy => maxEnergy;

    //当前选中的目标
    public BaseBattleUnit selectedTarget;



    //初始化战斗
    public void InitBattle()
    {
        currentEnergy = maxEnergy;
        //战斗UI显示时触发战斗开始事件
        EventDispatcher.TriggerEvent(E_MessageType.BattleStart);
    }

    //设置费用
    public void EditEnergy(int newEnergy)
    {
        currentEnergy = newEnergy;
    }
    
    //根据效果ID和效果值触发效果
    public void TakeEffect(int effectID,int effectValue,BaseBattleUnit target=null)
    {
        //如果没有特别指定目标则默认指定当前选中的目标
        if (target==null)
        {
            target = selectedTarget;
        }
        switch (effectID)
        {
            //对目标造成单体伤害
            case 1001:
                if (target!=null)
                {
                    selectedTarget.TakeDamage(effectValue);
                }
                break;
        }
    }
}
