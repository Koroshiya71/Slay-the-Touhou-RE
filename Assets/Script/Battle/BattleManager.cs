using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class BattleManager : UnitySingleton<BattleManager>
{

    //��ǰѡ�е�Ŀ��
    public BaseBattleUnit selectedTarget;

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
