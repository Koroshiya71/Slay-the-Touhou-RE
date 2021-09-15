using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class HandCardManager : UnitySingleton<HandCardManager>
{
    //���ƶ�������
    private GameObject handCardGo;
    //��ǰ��������
    private int currentCardNum=0;
    //��������
    public int maxCardNum = 10;
    //����Ԥ����
    private GameObject handCardPrefab;
    //���ƶ����б�
    public List<GameObject> handCardList = new List<GameObject>();
    //ѡ�еĿ��ƶ���
    public HandCard selectedCard=null;
    private void Awake()
    {
        EventDispatcher.AddListener(E_MessageType.BattleStart,InitHandCard);
    }

    //��ʼ�����ƹ��������
    private void InitHandCard()
    {
        currentCardNum = 0;
        handCardGo = GameObject.Find("Cards_Hand");
        handCardPrefab = ResourcesManager.Instance.LoadResources<GameObject>("Prefabs/Card/HandCard");
    }

    //���ݿ���ID��ȡ���Ƶ�����
    public void GetCardByID(int cardID)
    {
        //������������ڵ�����������������
        if (currentCardNum>=maxCardNum)
        {
            return;
        }
        GameObject newCardGo = Instantiate(handCardPrefab, handCardGo.transform, false);
        newCardGo.transform.localScale = new Vector3(1, 1, 1);
        HandCard newCard = newCardGo.GetComponent<HandCard>();
        newCard.InitCard(cardID);
        handCardList.Add(newCardGo);
        currentCardNum++;
        MoveAndRotateCard();
    }

    //��������λ�ú���ת
    public void MoveAndRotateCard()
    {
        //��һ���Ƶ�X�������ת��
        float firstX = -75 * (currentCardNum - 1);
        //����ת��
        float totalRotate = (currentCardNum) * 1;
        //��תƫ����
        float offset = 0;
        //��λ��ַ
        int midIndex = currentCardNum / 2;
        //y����ƫ����
        for (int i = 0; i < currentCardNum; i++)
        {
            float yPosOffset = 0;
            switch (firstX + 150 * i)
            {
                case 150:
                case -150:
                    yPosOffset = 5;
                    break;
                case 300:
                case -300:
                    yPosOffset = 15;
                    break;
                case 450:
                case -450:
                    yPosOffset = 30;
                    break;
                case 600:
                case -600:
                    yPosOffset = 50;
                    break;
                case 225:
                case -225:
                    yPosOffset = 8;
                    break;
                case 375:
                case -375:
                    yPosOffset = 20;
                    break;
                case 525:
                case -525:
                    yPosOffset = 40;
                    break;
                case 675:
                case -675:
                    yPosOffset = 65;
                    break;
                default:
                    yPosOffset = 0;
                    break;
            }
            handCardList[i].transform.localPosition = new Vector3(firstX + 150 * i, -yPosOffset, 0);
            
            if (currentCardNum % 2 == 0 && Mathf.Lerp(totalRotate, -totalRotate, ((float)(i) / currentCardNum)) == 0)
            {
                offset = -2;
            }

            if (i==midIndex&& currentCardNum % 2 == 1)
            {
                handCardList[i].transform.localEulerAngles = new Vector3(0, 0, 0);
                offset = -2;
                handCardList[i].GetComponent<HandCard>().SaveOriginalPos();

                continue;
            }
            
            handCardList[i].transform.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(totalRotate,-totalRotate,((float)(i)/currentCardNum))+offset);
            handCardList[i].GetComponent<HandCard>().SaveOriginalPos();
            
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetCardByID(1001);
        }
    }
    
    //�Ƴ�����
    public void RemoveCard(GameObject cardGo)
    {
        handCardList.Remove(cardGo);
        currentCardNum--;
        Destroy(cardGo);
        MoveAndRotateCard();
    }
}
