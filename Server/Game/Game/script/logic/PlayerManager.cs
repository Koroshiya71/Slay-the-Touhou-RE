using System;
using System.Collections.Generic;

public class PlayerManager
{
	//玩家列表
	public static Dictionary<string, Player> players = new Dictionary<string, Player>();

	//等待多人游戏连接的玩家字典
    public static Dictionary<string, Player> waitingMultiPlayers = new Dictionary<string, Player>();

	//已经进入游戏的玩家列表的列表
    public static List<List<Player>> inGamePlayers = new List<List<Player>>(); 
	//玩家是否在线
	public static bool IsOnline(string id){
		return players.ContainsKey(id);
	}
	//获取玩家
	public static Player GetPlayer(string id){
		return players[id];
	}
	//添加玩家
	public static void AddPlayer(string id, Player player){
		players.Add(id, player);
	}
	//删除玩家
	public static void RemovePlayer(string id){
		players.Remove(id);
	}
}


