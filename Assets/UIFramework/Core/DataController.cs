using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore;
//数据控制类:
//加载配置表
//读取配置表
public class DataController : Singleton<DataController>
{
    //用于存放背包配置表的字典<字段名,<id,字段的值>>
    public Dictionary<string, Dictionary<string, string>> dicPack;
    //用于存放公告配置表的字典
    public Dictionary<string, Dictionary<string, string>> dicNotice;
    //用于存放关卡配置表的字典
    public Dictionary<string, Dictionary<string, string>> dicLevel;
    //用于存放角色配置表的字典
    public Dictionary<string, Dictionary<string, string>> dicPlayer;
    public Dictionary<string, Dictionary<string, string>> dicActivity;
    //加载所有的配置表
    public void LoadAllCfg()
    {
        return;
        LoadPackCfg();
        LoadLevelCfg();
        LoadNoticeCfg();
        LoadPlayerCfg();
        LoadActivityCfg();
    }
    //加载背包配置表
    private void LoadPackCfg()
    {
        ExcelData.LoadExcelFormCSV("PackCfg",out dicPack);
    }
    //加载关卡配置表
    private void LoadLevelCfg()
    {
        ExcelData.LoadExcelFormCSV("LevelCfg", out dicLevel);
    }
    //加载公告配置表
    private void LoadNoticeCfg()
    {
        ExcelData.LoadExcelFormCSV("NoticeCfg", out dicNotice);
    }
    //加载角色配置表
    private void LoadPlayerCfg()
    {
        ExcelData.LoadExcelFormCSV("PlayerCfg", out dicPlayer);
    }
    private void LoadActivityCfg()
    {
        ExcelData.LoadExcelFormCSV("ActivityCfg", out dicActivity);
    }
    //供外界调用的,用于读取配置表字段值得方法(字段名,ID,存放配置表内容对应的字典)
    public string ReadCfg(string keyName,int id,Dictionary<string, Dictionary<string, string>> dic)
    {   
        return dic[keyName][id.ToString()];
    }
}
