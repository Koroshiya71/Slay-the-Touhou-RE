using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
public class RelicData
{
    //����ID
    public int relicID;
    //��������
    public string relicDes;
    //�������
    public int relicCount;
}
public class RelicManager : UnitySingleton<RelicManager>
{
    //�����������б�
    public List<RelicData> relicDataList = new List<RelicData>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
