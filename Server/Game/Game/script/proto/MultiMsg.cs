using System.Collections;
using System.Collections.Generic;

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