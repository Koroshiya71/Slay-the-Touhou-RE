using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class DeskManager : UnitySingleton<DeskManager>
{
    /// <summary>
    /// ���Թ�������ƿ�Ĺ�����
    /// </summary>


    //�ƿ��б����<��������>
    public List<CardData> deskCardList = new List<CardData>();
    //���ƶ��б����<��������>
    public List<CardData> drawCardDeskList = new List<CardData>();
    //���ƶ��б����<��������>
    public List<CardData> disCardDeskList = new List<CardData>();

    //
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
