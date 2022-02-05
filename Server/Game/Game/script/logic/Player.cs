using System;

public class Player {
	//id
	public string id = "";
	//指向ClientState
	public ClientState state;
	//读取情况
    public bool LoadState = false;
	//回合完成情况
    public bool TurnState = false;
    //构造函数
	public Player(ClientState state){
		this.state = state;
	}

	//数据库数据
	public PlayerData data;

	//发送信息
	public void Send(MsgBase msgBase){
		NetManager.Send(state, msgBase);
	}

}


