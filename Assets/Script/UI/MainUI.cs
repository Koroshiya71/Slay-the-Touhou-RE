using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;


//������UI
public class MainUI : BaseUI
{
    //��ʼ��Ϸ��ť
    private Button btn_StartGame;

    //��ȡUI�������ӻص�����
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        btn_StartGame = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_StartGame");
        btn_StartGame.onClick.AddListener(LoadGameScene);
    }


    //��ʼ��UI���͵�����
    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.MainUI;
        this.uiType.showMode = E_ShowUIMode.DoNothing;
    }

    //������Ϸ����
    private void LoadGameScene()
    {
        SceneController.Instance.LoadSceneAsync("GameScene", delegate
        {
            UIManager.Instance.ShowUI(E_UiId.GameMainUI);
            UIManager.Instance.ShowUI(E_UiId.BattleUI);

        });
    }
}
