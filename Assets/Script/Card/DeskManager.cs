using System;
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


    //����ս���Ƿ��Ѿ���ʼ�������ƶ�
    private bool hasInitDrawCardDesk = false;

    //��ʼ���ƿ������
    public void InitDeskManager()
    {
        string[] baseDesk;
        if (GameTool.HasKey("BaseDesk"))
        {
            baseDesk = GameTool.GetString("BaseDesk").Split(';');
        }
        else
        {
            GameTool.SetString("BaseDesk", "1001;1001;1001;1002;1002;1002");
            baseDesk = GameTool.GetString("BaseDesk").Split(';');
        }

        foreach (var cardIDStr in baseDesk)
        {
            deskCardList.Add(new CardData(int.Parse(cardIDStr)));
        }
    }
    //��ʼ�����ƶ�
    public void InitDrawCardDesk()
    {
        //������ƶ�Ϊ���Ҵ�δ��ʼ�������ƶѣ�����ƿ���е�һ�γ�ʼ��
        if (disCardDeskList.Count==0&&!hasInitDrawCardDesk)
        {
            hasInitDrawCardDesk = true;

            //���ƿ������п��Ƽ�����ƶ�
            foreach (var cardData in deskCardList)
            {
                drawCardDeskList.Add(cardData);
            }
        }
        //������ƶ��еĿ���������Ϊ0,���������ƶ��еĿ��Ƽ�����ƶ�
        else if (disCardDeskList.Count>1)
        {
            for (int i = 0; i < disCardDeskList.Count; i++)
            {
                drawCardDeskList.Add(disCardDeskList[i]);
                disCardDeskList.Remove(disCardDeskList[i]);
            }
        }
        //ϴ��
        drawCardDeskList = GameTool.RandomSort(drawCardDeskList);
    }

    //���ø����ƶ�
    public void ResetDesks()
    {
        //��ճ��ƶѺ����ƶ�
        drawCardDeskList = new List<CardData>();
        disCardDeskList = new List<CardData>();

        //���ó��ƶ�״̬Ϊδ��ʼ��
        hasInitDrawCardDesk = false;
    }
    //������ת����
    public void DrawCard()
    {
        if (drawCardDeskList.Count==0)
        {
            InitDrawCardDesk();
        }
        //��ȡ���ƶѵĵ�һ���ƣ��������û�б��޸Ĺ���ֱ�Ӹ���ID��ʼ������
        CardData data = drawCardDeskList[0];
        if (data.HasModified)
        {
            HandCardManager.Instance.GetCardByData(data);
        }
        //��������ݿ������ݳ�ʼ��
        else
        {
            HandCardManager.Instance.GetCardByID(data.CardID);
        }
        drawCardDeskList.Remove(data);

    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void Awake()
    {
        EventDispatcher.AddListener(E_MessageType.GameStart,InitDeskManager);
        EventDispatcher.AddListener(E_MessageType.BattleStart,InitDrawCardDesk);
    }
}
