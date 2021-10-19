using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using UnityEngine;
using UnityEngine.SearchService;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : BaseBattleUnit
{
    /// <summary>
    /// �������ݺ�UI
    /// </summary>
    //��������
    public EnemyData enemyData;

    //����ͼƬ
    protected Image img_EnemyOutLook;

    //���������ı�
    protected Text text_EnemyName;

    /// <summary>
    /// �����ж����
    /// </summary>
    //������ͼImage
    protected Image img_EnemyAction;

    //������ͼ�����ı�
    protected Text text_ActionDes;

    //�����ж���ֵ�ı�
    protected Text text_ActionValue;

    //��ǰ����ִ�е��ж�
    protected ActionData currentAction;

    //���õ���Ϊģʽ
    protected string activeActionMode;

    //��ǰ����ִ����Ϊ�б��еĵڼ�����Ϊ
    protected int currentActionNo;

    #region ��ʼ��

    //��ʼ������
    protected override void InitDataOnAwake(int id)
    {
        enemyData = new EnemyData(id);
        maxHp = enemyData.MAXHp;
        currentHp = maxHp;

        //���ж�ģʽ�б������ѡ��һ���ж�ģʽ
        activeActionMode = enemyData.ActionModeList[Random.Range(0, enemyData.ActionModeList.Count)];
        currentActionNo = 0;
        //��ȡ�ж��б��еĵ�һ����Ϊ
        currentAction = enemyData.EnemyActionDic
            .ElementAt(Convert.ToInt32(activeActionMode[currentActionNo].ToString()) - 1).Value;
        currentActionNo++;
    }

    //��ʼ������
    protected override void InitUIOnAwake()
    {
        base.InitUIOnAwake();

        //��ȡ����ͼƬ�������ı�����ֵ
        img_EnemyOutLook = GameTool.GetTheChildComponent<Image>(gameObject, "Enemy_OutLook");
        img_EnemyOutLook.sprite = ResourcesManager.Instance.LoadResources<Sprite>(enemyData.ResourcePath);

        text_EnemyName = GameTool.GetTheChildComponent<Text>(gameObject, "Text_Name");
        text_EnemyName.text = enemyData.EnemyName;

        //��ȡ�����ж���ʾͼƬ���ı�
        img_EnemyAction = GameTool.GetTheChildComponent<Image>(gameObject, "Img_Action");

        text_ActionDes = GameTool.GetTheChildComponent<Text>(gameObject, "Text_ActionDes");
        text_ActionValue = GameTool.GetTheChildComponent<Text>(gameObject, "Text_ActionValue");
        text_ActionDes.enabled = false;
        text_ActionValue.enabled = false;
        switch (currentAction.ActionType)
        {
            case ActionType.Attack:
                img_EnemyAction.sprite =
                    ResourcesManager.Instance.LoadResources<Sprite>("Image/" + "UIImage/" + "EnemyAction/" + "Attack");
                text_ActionValue.text = currentAction.ActionValue.ToString();
                text_ActionValue.enabled = true;
                break;
        }

        text_ActionDes.text = currentAction.ActionDes.Replace("value", currentAction.ActionValue.ToString());
        text_ActionDes.enabled = false;
    }

    #endregion


    #region �������¼�

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        //����ѡ��ʱ����ѡ����Ч
        if (other.CompareTag("HandCard") &&
            HandCardManager.Instance.selectedCard.mCardData.CardTarget == CardTarget.SingleEnemy)
        {
            BattleManager.Instance.selectedTarget = this;
        }
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        //����ѡ��ʱ����ѡ����Ч
        if (other.CompareTag("HandCard") &&
            HandCardManager.Instance.selectedCard.mCardData.CardTarget == CardTarget.SingleEnemy)
        {
            BattleManager.Instance.selectedTarget = this;
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        //�����뿪ʱ����ѡ����Ч

        if (other.CompareTag("HandCard"))
        {
            BattleManager.Instance.selectedTarget = null;
        }
    }

    #endregion


    #region UI����

    protected void UpdateUIState()
    {
        if (BattleManager.Instance.selectedTarget != this)
        {
            selectEffect.SetActive(false);
        }
        else
        {
            selectEffect.SetActive(true);
        }
    }

    //��ʾ�ж�����
    public void ShowActionDes()
    {
        text_ActionDes.text = currentAction.ActionDes.Replace("value", currentAction.ActionValue.ToString());
        text_ActionDes.enabled = true;
    }

    //�����ж�����
    public void HideActionDes()
    {
        text_ActionDes.enabled = false;
    }

    #endregion


    #region �ж���״̬�������
    //ִ���ж�
    public void TakeAction()
    {
        BattleManager.Instance.TriggerActionEffect(currentAction);
    }
    //�����ж�
    public void UpdateAction()
    {

    }
    //��������
    public override void Die()
    {
        //�Ƴ����в�����
        BattleManager.Instance.inBattleEnemyList.Remove(this);
        Destroy(this.gameObject);

        //����õ����������������Ϊ0������ս����������
        if (BattleManager.Instance.inBattleEnemyList.Count==0)
        {
            BattleManager.Instance.BattleEnd();
        }
    }

    #endregion

    protected void Update()
    {
        UpdateUIState();
    }
}