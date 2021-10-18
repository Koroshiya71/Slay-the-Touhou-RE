using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//消息类型
public enum E_MessageType
{
    GameStart,//游戏开始
    BattleStart,//战斗开始
    TurnStart,//战斗开始

    SaveGame,//游戏存档
    UseCard,//使用卡牌
}
//物品的分类
//public enum E_GoodsType
//{
//    Default,//全部
//    Equipment,//装备
//    Potions,//药水
//    Rune,//符文
//    Material//材料
//}
//窗体的显示方式
public enum E_ShowUIMode
{
    //界面显示出来的时候,不需要去隐藏其他窗体(InforUI)
    DoNothing,
    //界面显示出来的时候,需要去隐藏其他窗体(但是不隐藏保持在最前方的窗体)
    HideOther,
    //界面显示出来的时候,需要去隐藏所有的窗体
    HideAll
}
//窗体的层级类型(父节点的类型)
public enum E_UIRootType
{
    KeepAbove,//保持在前方的窗体(DoNothing)
    Normal//普通窗体(1、HideOther 2、HideAll)
}
//窗体显示出来时是否播放音效
public enum E_UIPlayAudio
{
    Play,
    NoPlay
}
//窗体的ID
public enum E_UiId
{
    NullUI,
    MainUI,
    GameMainUI,
    LoadingUI,
    BattleUI,
    MapUI
}
public class GameDefine
{
  
    public static Dictionary<E_UiId, string> dicPath = new Dictionary<E_UiId, string>()
    {
        
        { E_UiId.LoadingUI,"UIPrefab/"+"LoadingUI"},
        { E_UiId.MainUI,"UIPrefab/"+"MainUI"},
        { E_UiId.GameMainUI,"UIPrefab/"+"GameMainUI"},
        { E_UiId.BattleUI,"UIPrefab/"+"BattleUI"},
        { E_UiId.MapUI,"UIPrefab/"+"MapUI"},

    };
    public static Type GetUIScriptType(E_UiId uiId)
    {
        Type scriptType = null;
        switch (uiId)
        {
            case E_UiId.NullUI:
                GameDebuger.Log("自动添加脚本的时候,传入的窗体id为NullUI");
                break;
           
            case E_UiId.LoadingUI:
                scriptType = typeof(LoadingUI);
                break;
            case E_UiId.MainUI:
                scriptType = typeof(MainUI);
                break;
            case E_UiId.GameMainUI:
                scriptType = typeof(GameMainUI);
                break;
            case E_UiId.BattleUI:
                scriptType = typeof(BattleUI);
                break;
            case E_UiId.MapUI:
                scriptType = typeof(MapUI);
                break;
            default:
                break;
        }
        return scriptType;
    }
}
