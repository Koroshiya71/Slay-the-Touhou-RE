using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;


//主界面UI
public class MainUI : BaseUI
{
    //开始游戏按钮
    private Button btn_StartGame;

    //获取UI组件并添加回调函数
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        btn_StartGame = GameTool.GetTheChildComponent<Button>(gameObject, "Btn_StartGame");
        btn_StartGame.onClick.AddListener(LoadGameScene);
    }


    //初始化UI类型等数据
    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.MainUI;
        this.uiType.showMode = E_ShowUIMode.DoNothing;
    }

    //加载游戏场景
    private void LoadGameScene()
    {
        SceneController.Instance.LoadSceneAsync("GameScene", delegate
        {
            UIManager.Instance.ShowUI(E_UiId.GameMainUI);
            UIManager.Instance.ShowUI(E_UiId.BattleUI);

        });
    }
}
