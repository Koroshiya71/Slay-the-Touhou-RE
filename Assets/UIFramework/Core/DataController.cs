using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore;
//数据控制类:
//加载配置表
//读取配置表
public class DataController : Singleton<DataController>
{
    //用于存放卡牌数据配置表的字典<字段名,<id,字段的值>>
    public Dictionary<string, Dictionary<string, string>> dicCardData;

    //用于存放敌人数据配置表的字典<字段名,<id,字段的值>>
    public Dictionary<string, Dictionary<string, string>> dicEnemyData;

    //用于存放战斗数据配置表的字典<字段名,<id,字段的值>>
    public Dictionary<string, Dictionary<string, string>> dicBattleData;

    //用于存放卡牌效果配置表的字典<字段名,<id,字段的值>>
    public Dictionary<string, Dictionary<string, string>> dicCardEffect;

    //用于存放行动数据配置表的字典<字段名,<id,字段的值>>
    public Dictionary<string, Dictionary<string, string>> dicActionData;
    //用于存放状态数据配置表的字典<字段名,<id,字段的值>>
    public Dictionary<string, Dictionary<string, string>> dicStateData;
    //用于存放事件数据配置表的字典<字段名,<id,字段的值>>
    public Dictionary<string, Dictionary<string, string>> dicEventData;
    //用于存放事件效果数据配置表的字典<字段名,<id,字段的值>>
    public Dictionary<string, Dictionary<string, string>> dicEventResultData;

    //加载所有的配置表
    public void LoadAllCfg()
    {
        LoadCardDataCfg();
        LoadEnemyDataCfg();
        LoadBattleDataCfg();
        LoadCardEffectCfg();
        LoadActionDataCfg();
        LoadStateDataCfg();
        LoadEventDataCfg();
    }
    //加载卡牌数据配置表
    private void LoadCardDataCfg()
    {
        ExcelData.LoadExcelFormCSV("CardDataCfg", out dicCardData);
    }
    //加载敌人数据配置表
    private void LoadEnemyDataCfg()
    {
        ExcelData.LoadExcelFormCSV("EnemyDataCfg", out dicEnemyData);
    }
    //加载战斗数据配置表
    private void LoadBattleDataCfg()
    {
        ExcelData.LoadExcelFormCSV("BattleDataCfg", out dicBattleData);
        for (int i = 1; i <= dicBattleData["ID"].Count; i++)
        {
            BattleManager.Instance.battleDataDic.Add(i, new BattleData(i));
        }
    }
    //加载卡牌效果配置表
    private void LoadCardEffectCfg()
    {
        ExcelData.LoadExcelFormCSV("CardEffectCfg", out dicCardEffect);
        foreach (var dicElement in dicCardData["ID"])
        {
            DeskManager.Instance.allCardDataList.Add(new CardData(int.Parse(dicElement.Key)));
        }
    }
    //加载卡牌效果配置表
    private void LoadActionDataCfg()
    {
        ExcelData.LoadExcelFormCSV("ActionDataCfg", out dicActionData);
        
    }
    //加载状态效果配置表
    private void LoadStateDataCfg()
    {
        ExcelData.LoadExcelFormCSV("StateDataCfg", out dicStateData);
    }
    //加载事件配置表
    private void LoadEventDataCfg()
    {
        ExcelData.LoadExcelFormCSV("EventDataCfg", out dicEventData);
        ExcelData.LoadExcelFormCSV("EventResultCfg", out dicEventResultData);
    }
    //供外界调用的,用于读取配置表字段值得方法(字段名,ID,存放配置表内容对应的字典)
    public string ReadCfg(string keyName, int id, Dictionary<string, Dictionary<string, string>> dic)
    {
        return dic[keyName][id.ToString()];
    }
}
