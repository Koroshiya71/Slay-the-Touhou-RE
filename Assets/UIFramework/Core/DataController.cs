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
    
    //加载所有的配置表
    public void LoadAllCfg()
    {
       LoadCardDataCfg();
    }
    //加载卡牌数据配置表
    private void LoadCardDataCfg()
    {
        ExcelData.LoadExcelFormCSV("CardDataCfg", out dicCardData);
    }
    
    //供外界调用的,用于读取配置表字段值得方法(字段名,ID,存放配置表内容对应的字典)
    public string ReadCfg(string keyName,int id,Dictionary<string, Dictionary<string, string>> dic)
    {   
        return dic[keyName][id.ToString()];
    }
}
