using System;
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
    //����ͼƬ·��
    public string relicSpriteRes;
    //�Ƿ�Ҫ��ʾ����
    public bool showCount;

    //���캯��
    public RelicData()
    {

    }
}
public class RelicManager : UnitySingleton<RelicManager>
{
    //�����������б�
    public List<RelicData> relicDataList = new List<RelicData>();
    //���������ֵ�(id,��������)
    public Dictionary<int, RelicData> relicDic = new Dictionary<int, RelicData>();
    //������������ֵ�
    public Dictionary<int, RelicData> playerRelicDic = new Dictionary<int, RelicData>();
    //����Ԥ����
    public GameObject relicPrefab;
    //����Content
    public GameObject relicContent;
    private void Awake()
    {
        EventDispatcher.AddListener(E_MessageType.GameStart,InitRelicManager);
    }

    //��ȡ����
    public void GetRelic(int id)
    {
        //��������
        GameObject newRelicObj = Instantiate(relicPrefab);
        Relic newRelic = newRelicObj.GetComponent<Relic>();
        newRelicObj.transform.SetParent(relicContent.transform);
        newRelicObj.transform.localScale = new Vector3(1, 1);
        newRelic.InitRelic(relicDic[id]);
        //��ӵ�����ֵ�
        playerRelicDic.Add(id,relicDic[id]);
    }
   //����Ƿ�ӵ��ĳ����
   public bool CheckRelic(int id)
   {
       return playerRelicDic.ContainsKey(id);
   }
    //��ȡJson�ļ���ʼ�����������б�Ͷ�Ӧ�ֵ�
    public void InitRelicManager()
    {
        //��ȡ�б�
        StreamReader reader = new StreamReader(SaveManager.jsonDataPath + "relic.json");
        relicDataList = JsonConvert.DeserializeObject<List<RelicData>>(reader.ReadToEnd());
        reader.Close();
        //��ʼ���ֵ�
        foreach (var relic in relicDataList)
        {
            relicDic.Add(relic.relicID,relic);
        }
        //��ȡ����Ԥ����
        relicPrefab = ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/Battle/Relic");
        //��ȡ����content
        relicContent = GameObject.Find("RelicContent");
        //������
        GetRelic(1001);
    }
    //�����������ս����ʼЧ��
    public void CheckRelicBattleStartEffect()
    {
        //�����ż
        if (CheckRelic(1001))
        {
            BattleManager.Instance.turnStartEffectDelegate += delegate
            {
                foreach (var enemy in BattleManager.Instance.inBattleEnemyList)
                {
                    enemy.TakeDamage(9,Player.Instance);
                }
            };
        }
    }
}
