using System.Security.Cryptography;

public class MsgPing : MsgBase
{
    public MsgPing()
    {
        protoName = "MsgPing";
    }
}


public class MsgPong : MsgBase
{
    public MsgPong()
    {
        protoName = "MsgPong";
    }
}

//存档协议
public class MsgSave : MsgBase
{
    public MsgSave()
    {
        protoName = "MsgSave";
    }

    public string id;
    public string saveData="";
}