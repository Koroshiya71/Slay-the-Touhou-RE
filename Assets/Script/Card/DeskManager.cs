using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class DeskManager : UnitySingleton<DeskManager>
{
    /// <summary>
    /// 用以管理各种牌库的管理器
    /// </summary>


    //牌库列表对象<卡牌数据>
    public List<CardData> deskCardList = new List<CardData>();
    //抽牌堆列表对象<卡牌数据>
    public List<CardData> drawCardDeskList = new List<CardData>();
    //弃牌堆列表对象<卡牌数据>
    public List<CardData> disCardDeskList = new List<CardData>();

    //
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
