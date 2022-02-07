using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class MsgHandler
{
    public static void MsgMultiStart(ClientState c, MsgBase msgBase)
    {
        Console.WriteLine("MsgMultiStart");
        MsgMultiStart msg = (MsgMultiStart) msgBase;
        //添加到当前等待的多人游戏列表
        PlayerManager.waitingMultiPlayers.Add(msg.id, PlayerManager.players[msg.id]);
        Console.WriteLine(msg.id);
        //如果人数够了就开始游戏
        if (PlayerManager.waitingMultiPlayers.Count % 2 == 0)
        {
            List<Player> newGamePlayers = new List<Player>();
            foreach (var pl in PlayerManager.waitingMultiPlayers.Values)
            {
                NetManager.Send(pl.state, new MsgMultiEnter());
                newGamePlayers.Add(pl);
            }

            PlayerManager.inGamePlayers.Add(newGamePlayers);
            PlayerManager.waitingMultiPlayers.Clear();
        }
        //否则就让其等待
        else
        {
            NetManager.Send(c, new MsgMultiWait());
            Console.WriteLine("send MsgMultiWait");

        }
    }

    public static void MsgChooseScene(ClientState c, MsgBase msgBase)
    {
        Console.WriteLine("MsgChooseScene");
        MsgChooseScene msg = (MsgChooseScene) msgBase;
        //给除此以外的其他玩家发布确认消息
        foreach (var list in PlayerManager.inGamePlayers)
        {
            //查找本局游戏的玩家列表
            if (list.Contains(PlayerManager.GetPlayer(msg.id)))
            {
                foreach (var pl in list)
                {
                    //给除自己以外的玩家发送消息
                    if (pl.id != msg.id)
                    {
                        MsgWaitConfirm msgConfirm = new MsgWaitConfirm();
                        msgConfirm.type = msg.type;
                        msgConfirm.battleDataStr = msg.battleDataStr;
                        msgConfirm.eventDataStr = msg.eventDataStr;
                        msgConfirm.index = msg.index;
                        pl.Send(msgConfirm);
                    }
                }
            }
        }


    }

    public static void MsgConfirmChoose(ClientState c, MsgBase msgBase)
    {
        MsgConfirmChoose msg = (MsgConfirmChoose) msgBase;
        if (msg.confirm)
        {
            foreach (var list in PlayerManager.inGamePlayers)
            {
                if (list.Contains(PlayerManager.GetPlayer(msg.id)))
                {
                    foreach (var pl in list)
                    {
                        MsgEnterScene msgEnter = new MsgEnterScene();
                        msgEnter.type = msg.type;
                        pl.TurnState = false;
                        pl.Send(msgEnter);
                    }
                }
            }
        }
    }

    public static void MsgCardEffect(ClientState c, MsgBase msgBase)
    {
        MsgCardEffect msg = (MsgCardEffect) msgBase;
        //给除自己以外的玩家转发消息
        foreach (var list in PlayerManager.inGamePlayers)
        {
            if (list.Contains(PlayerManager.GetPlayer(msg.id)))
            {
                foreach (var pl in list)
                {
                    if (pl.id != msg.id)
                    {
                        pl.Send(msg);
                        Console.WriteLine("Send MsgCardEffect");
                    }
                }
            }
        }
    }

    public static void MsgUseCard(ClientState c, MsgBase msgBase)
    {
        MsgUseCard msg=(MsgUseCard)msgBase;
        //给除自己以外的玩家转发消息
        foreach (var list in PlayerManager.inGamePlayers)
        {
            if (list.Contains(PlayerManager.GetPlayer(msg.id)))
            {
                foreach (var pl in list)
                {
                    if (pl.id != msg.id)
                    {
                        pl.Send(msg);
                    }
                }
            }
        }
    }
    
    public static void MsgLoadOK(ClientState c, MsgBase msgBase)
    {
        MsgLoadOK msg = (MsgLoadOK)msgBase;
        //标记该玩家读取完成
        PlayerManager.GetPlayer(msg.id).LoadState = true;
        //自己所在组的所有玩家都读取完成了就给所有玩家发送LoadEnd消息，否则给该发送LoadWait消息
        int flag = 1;
        foreach (var list in PlayerManager.inGamePlayers)
        {
            if (list.Contains(PlayerManager.GetPlayer(msg.id)))
            {
                foreach (var pl in list)
                {
                    //只要有一个玩家没读取完就置flag为0
                    if (!pl.LoadState)
                    {
                        flag = 0;
                    }
                }
            }
        }

        if (flag==1)
        {
            foreach (var list in PlayerManager.inGamePlayers)
            {
                if (list.Contains(PlayerManager.GetPlayer(msg.id)))
                {
                    foreach (var pl in list)
                    {
                        MsgLoadEnd msgLoadEnd = new MsgLoadEnd();
                        pl.Send(msgLoadEnd);
                    }
                }
            }
        }
        else
        {
            PlayerManager.GetPlayer(msg.id).Send(new MsgLoadWait());
        }
    }

    public static void MsgSendSceneType(ClientState c, MsgBase msgBase)
    {
        MsgSendSceneType msg=(MsgSendSceneType)msgBase;

        //给其他玩家同步场景信息
        foreach (var list in PlayerManager.inGamePlayers)
        {
            if (list.Contains(PlayerManager.GetPlayer(msg.id)))
            {
                foreach (var pl in list)
                {
                    if (pl.id != msg.id)
                    {
                        pl.Send(msg);
                        Console.WriteLine(msg.id+(":Send SceneType"));
                    }
                }
            }
        }
    }
    public static void MsgTurnEnd(ClientState c, MsgBase msgBase)
    {
        MsgTurnEnd msg = (MsgTurnEnd)msgBase;
        //标记该玩家回合完成
        PlayerManager.GetPlayer(msg.id).TurnState = true;
        //自己所在组的所有玩家都读取完成了就给所有玩家发送TurnFin消息，否则给该玩家发送TurnWait消息
        int flag = 1;
        foreach (var list in PlayerManager.inGamePlayers)
        {
            if (list.Contains(PlayerManager.GetPlayer(msg.id)))
            {
                foreach (var pl in list)
                {
                    //只要有一个玩家没读取完就置flag为0
                    if (!pl.TurnState)
                    {
                        flag = 0;
                    }
                }
            }
        }

        if (flag == 1)
        {
            foreach (var list in PlayerManager.inGamePlayers)
            {
                if (list.Contains(PlayerManager.GetPlayer(msg.id)))
                {
                    foreach (var pl in list)
                    {
                        pl.TurnState = false;
                        MsgTurnFin msgLoadEnd = new MsgTurnFin();
                        pl.Send(msgLoadEnd);
                    }
                }
            }
        }
        else
        {
            PlayerManager.GetPlayer(msg.id).Send(new MsgTurnWait());
        }
    }

   
}