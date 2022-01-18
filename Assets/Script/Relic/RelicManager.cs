using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
public class RelicData
{
    //遗物ID
    public int relicID;
    //遗物描述
    public string relicDes;
    //遗物计数
    public int relicCount;
}
public class RelicManager : UnitySingleton<RelicManager>
{
    //遗物数据总列表
    public List<RelicData> relicDataList = new List<RelicData>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
