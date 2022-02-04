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
}