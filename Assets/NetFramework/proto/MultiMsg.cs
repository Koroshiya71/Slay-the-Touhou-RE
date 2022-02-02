using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

//选择场景
public class MsgChooseScene : MsgBase
{
    public MsgChooseScene() { protoName = "MsgChooseScene"; }
    //选择的场景类型
    public SceneType type;
    //场景index
    public int index=0;
    //选择者ID
    public string id;
}
//等待确认选择
public class MsgWaitConfirm : MsgBase
{
    public MsgWaitConfirm() { protoName = "MsgWaitConfirm"; }
    public SceneType type;
}

//确认选择
public class MsgConfirmChoose : MsgBase
{
    public MsgConfirmChoose() { protoName = "MsgConfirmChoose"; }
    public SceneType type;
    //接受还是拒绝
    public bool confirm;
    public string id;
}

//进入场景
public class MsgEnterScene : MsgBase
{
    public MsgEnterScene() { protoName = "MsgEnterScene"; }
    public SceneType type;
}