using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetFramework.proto
{
    //存档协议
    public class MsgSaveData : MsgBase
    {
        public MsgSaveData()
        {
            protoName = "MsgSaveData";
        }

        public string id;
        public string saveData = "";
    }

//发起多人游戏连接开始
    public class MsgMultiStart : MsgBase
    {
        public MsgMultiStart()
        {
            protoName = "MsgMultiStart";
        }

        public string id;
    }

//需要等待的消息
    public class MsgMultiWait : MsgBase
    {
        public MsgMultiWait()
        {
            protoName = "MsgMultiWait";
        }
    }

//进入多人游戏
    public class MsgMultiEnter : MsgBase
    {
        public MsgMultiEnter()
        {
            protoName = "MsgMultiEnter";
        }
    }

//选择场景
    public class MsgChooseScene : MsgBase
    {
        public MsgChooseScene()
        {
            protoName = "MsgChooseScene";
        }

        //选择的场景类型
        public SceneType type;

        //场景index
        public int index = 0;

        //选择者ID
        public string id;
        public string battleDataStr;
        public string eventDataStr;
    }

//等待确认选择
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

//确认选择
    public class MsgConfirmChoose : MsgBase
    {
        public MsgConfirmChoose()
        {
            protoName = "MsgConfirmChoose";
        }

        public SceneType type;

        //接受还是拒绝
        public bool confirm;
        public string id;
    }

//进入场景
    public class MsgEnterScene : MsgBase
    {
        public MsgEnterScene()
        {
            protoName = "MsgEnterScene";
        }

        public SceneType type;
    }

//卡牌效果消息
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

//使用卡牌
    public class MsgUseCard : MsgBase
    {
        public MsgUseCard()
        {
            protoName = "MsgUseCard";
        }

        public string id;
    }

//读取完成
    public class MsgLoadOK : MsgBase
    {
        public MsgLoadOK()
        {
            protoName = "MsgLoadOK";
        }

        public string id;
    }
//读取等待
    public class MsgLoadWait : MsgBase
    {
        public MsgLoadWait()
        {
            protoName = "MsgLoadWait";
        }

    }
//读取结束 
    public class MsgLoadEnd : MsgBase
    {
        public MsgLoadEnd()
        {
            protoName = "MsgLoadEnd";
        }

        public string id;
    }
//发送场景数据
    public class MsgSendSceneType:MsgBase
    {
        public MsgSendSceneType()
        {
            protoName = "MsgSendSceneType";
        }

        public string id;
        public string sceneTypeListStr;
    }
//点击回合结束
    public class MsgTurnEnd : MsgBase
    {
        public MsgTurnEnd()
        {
            protoName = "MsgTurnEnd";
        }

        public string id;

    }
//等待对方回合结束
    public class MsgTurnWait : MsgBase
    {
        public MsgTurnWait()
        {
            protoName = "MsgTurnWait";
        }

        public string id;
    }
//回合完成
    public class MsgTurnFin : MsgBase
    {
        public MsgTurnFin()
        {
            protoName = "MsgTurnFin";
        }
        public string id;
    }
}