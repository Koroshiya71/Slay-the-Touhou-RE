using System;


public partial class MsgHandler {
	public static void MsgPing(ClientState c, MsgBase msgBase){
		Console.WriteLine("MsgPing");
		c.lastPingTime = NetManager.GetTimeStamp();
		MsgPong msgPong = new MsgPong();
		NetManager.Send(c, msgPong);
	}
    public static void MsgSave(ClientState c, MsgBase msgBase)
    {
        Console.WriteLine("MsgSave");
        MsgSave msgSave=(MsgSave)msgBase;
        PlayerManager.GetPlayer(msgSave.id).data.playerDataStr=msgSave.saveData;
        DbManager.UpdatePlayerData(msgSave.id, msgSave.saveData);
    }
}


