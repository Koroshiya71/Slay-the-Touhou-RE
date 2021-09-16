using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class BattleManager : UnitySingleton<BattleManager>
{
    //��ǰ����ֵ
    private int currentEnergy=3;
    public int CurrentEnergy => currentEnergy;
    //��������
    private int maxEnergy=3;
    public int MaxEnergy => maxEnergy;

    //��ǰѡ�е�Ŀ��
    public BaseBattleUnit selectedTarget;



    //��ʼ��ս��
    public void InitBattle()
    {
        currentEnergy = maxEnergy;
        //ս��UI��ʾʱ����ս����ʼ�¼�
        EventDispatcher.TriggerEvent(E_MessageType.BattleStart);
    }

    //���÷���
    public void EditEnergy(int newEnergy)
    {
        currentEnergy = newEnergy;
    }
    
    //����Ч��ID��Ч��ֵ����Ч��
    public void TakeEffect(int effectID,int effectValue,BaseBattleUnit target=null)
    {
        //���û���ر�ָ��Ŀ����Ĭ��ָ����ǰѡ�е�Ŀ��
        if (target==null)
        {
            target = selectedTarget;
        }
        switch (effectID)
        {
            //��Ŀ����ɵ����˺�
            case 1001:
                if (target!=null)
                {
                    selectedTarget.TakeDamage(effectValue);
                }
                break;
        }
    }
}
