using System.Collections;
using System.Collections.Generic;

//发起多人游戏连接开始
public class MsgMultiStart : MsgBase
{
    public MsgMultiStart() { protoName = "MsgMultiStart"; }
    public string id;
}
//需要等待的消息
public class MsgMultiWait : MsgBase
{
    public MsgMultiWait() { protoName = "MsgMultiWait"; }
}
//进入多人游戏
public class MsgMultiEnter : MsgBase
{
    public MsgMultiEnter() { protoName = "MsgMultiEnter"; }
}