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
        MsgMultiStart msg=(MsgMultiStart)msgBase;
        //添加到当前等待的多人游戏列表
        PlayerManager.waitingMultiPlayers.Add(msg.id,PlayerManager.players[msg.id]);
        Console.WriteLine(msg.id);
        //如果人数够了就开始游戏
        if (PlayerManager.waitingMultiPlayers.Count>=2)
        {
            foreach (var pl in PlayerManager.waitingMultiPlayers.Values)
            {
                NetManager.Send(pl.state,new MsgMultiEnter());
            }
        }
        //否则就让其等待
        else
        {
            NetManager.Send(c, new MsgMultiWait());
            Console.WriteLine("send MsgMultiWait");

        }
    }

}