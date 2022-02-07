using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetFramework.proto
{
    //�浵Э��
    public class MsgSaveData : MsgBase
    {
        public MsgSaveData()
        {
            protoName = "MsgSaveData";
        }

        public string id;
        public string saveData = "";
    }

//���������Ϸ���ӿ�ʼ
    public class MsgMultiStart : MsgBase
    {
        public MsgMultiStart()
        {
            protoName = "MsgMultiStart";
        }

        public string id;
    }

//��Ҫ�ȴ�����Ϣ
    public class MsgMultiWait : MsgBase
    {
        public MsgMultiWait()
        {
            protoName = "MsgMultiWait";
        }
    }

//���������Ϸ
    public class MsgMultiEnter : MsgBase
    {
        public MsgMultiEnter()
        {
            protoName = "MsgMultiEnter";
        }
    }

//ѡ�񳡾�
    public class MsgChooseScene : MsgBase
    {
        public MsgChooseScene()
        {
            protoName = "MsgChooseScene";
        }

        //ѡ��ĳ�������
        public SceneType type;

        //����index
        public int index = 0;

        //ѡ����ID
        public string id;
        public string battleDataStr;
        public string eventDataStr;
    }

//�ȴ�ȷ��ѡ��
    public class MsgWaitConfirm : MsgBase
    {
        public MsgWaitConfirm()
        {
            protoName = "MsgWaitConfirm";
        }

        public SceneType type;
        public string battleDataStr;
        public string eventDataStr;
        public int index;
    }

//ȷ��ѡ��
    public class MsgConfirmChoose : MsgBase
    {
        public MsgConfirmChoose()
        {
            protoName = "MsgConfirmChoose";
        }

        public SceneType type;

        //���ܻ��Ǿܾ�
        public bool confirm;
        public string id;
    }

//���볡��
    public class MsgEnterScene : MsgBase
    {
        public MsgEnterScene()
        {
            protoName = "MsgEnterScene";
        }

        public SceneType type;
    }

//����Ч����Ϣ
    public class MsgCardEffect : MsgBase
    {
        public MsgCardEffect()
        {
            protoName = "MsgCardEffect";
        }

        public int effectID;
        public int effectValue;
        public int targetIndex = 0;
        public int isCanXin = 0;
        public bool isLianZhan = false;
        public string id;
    }

//ʹ�ÿ���
    public class MsgUseCard : MsgBase
    {
        public MsgUseCard()
        {
            protoName = "MsgUseCard";
        }

        public string id;
    }

//��ȡ���
    public class MsgLoadOK : MsgBase
    {
        public MsgLoadOK()
        {
            protoName = "MsgLoadOK";
        }

        public string id;
    }
//��ȡ�ȴ�
    public class MsgLoadWait : MsgBase
    {
        public MsgLoadWait()
        {
            protoName = "MsgLoadWait";
        }

    }
//��ȡ���� 
    public class MsgLoadEnd : MsgBase
    {
        public MsgLoadEnd()
        {
            protoName = "MsgLoadEnd";
        }

        public string id;
    }
//���ͳ�������
    public class MsgSendSceneType:MsgBase
    {
        public MsgSendSceneType()
        {
            protoName = "MsgSendSceneType";
        }

        public string id;
        public string sceneTypeListStr;
    }
//����غϽ���
    public class MsgTurnEnd : MsgBase
    {
        public MsgTurnEnd()
        {
            protoName = "MsgTurnEnd";
        }

        public string id;

    }
//�ȴ��Է��غϽ���
    public class MsgTurnWait : MsgBase
    {
        public MsgTurnWait()
        {
            protoName = "MsgTurnWait";
        }

        public string id;
    }
//�غ����
    public class MsgTurnFin : MsgBase
    {
        public MsgTurnFin()
        {
            protoName = "MsgTurnFin";
        }
        public string id;
    }
}