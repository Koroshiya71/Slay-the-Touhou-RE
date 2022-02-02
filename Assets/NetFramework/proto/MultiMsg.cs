using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���������Ϸ���ӿ�ʼ
public class MsgMultiStart : MsgBase
{
    public MsgMultiStart() { protoName = "MsgMultiStart"; }
    public string id;
}
//��Ҫ�ȴ�����Ϣ
public class MsgMultiWait : MsgBase
{
    public MsgMultiWait() { protoName = "MsgMultiWait"; }
}
//���������Ϸ
public class MsgMultiEnter : MsgBase
{
    public MsgMultiEnter() { protoName = "MsgMultiEnter"; }
}

//ѡ�񳡾�
public class MsgChooseScene : MsgBase
{
    public MsgChooseScene() { protoName = "MsgChooseScene"; }
    //ѡ��ĳ�������
    public SceneType type;
    //����index
    public int index=0;
    //ѡ����ID
    public string id;
}
//�ȴ�ȷ��ѡ��
public class MsgWaitConfirm : MsgBase
{
    public MsgWaitConfirm() { protoName = "MsgWaitConfirm"; }
    public SceneType type;
}

//ȷ��ѡ��
public class MsgConfirmChoose : MsgBase
{
    public MsgConfirmChoose() { protoName = "MsgConfirmChoose"; }
    public SceneType type;
    //���ܻ��Ǿܾ�
    public bool confirm;
    public string id;
}

//���볡��
public class MsgEnterScene : MsgBase
{
    public MsgEnterScene() { protoName = "MsgEnterScene"; }
    public SceneType type;
}