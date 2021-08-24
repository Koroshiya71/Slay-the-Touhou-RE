using UnityEngine;
using System.Collections;
using GameCore;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//完成异步加载的功能
public class LoadingUI : BaseUI
{
    //将要异步加载的场景名称(去SceneController里面获取)
    private string levelSceneName;
    //显示加载进度的进度条
    private Slider progressSlider;
    //异步加载对象
    AsyncOperation asyn;
    //场景加载的实际进度
    private int inFactProgress=0;
    //用来显示的进度
    private int showProgress = 0;
    //用来显示进度的Text
    private Text txt_Value;
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        progressSlider = GameTool.GetTheChildComponent<Slider>(this.gameObject, "Slider");
        txt_Value = GameTool.GetTheChildComponent<Text>(this.gameObject, "Txt_Value");
        progressSlider.onValueChanged.AddListener(ChangeProgressText);
    }
    protected override void InitDataOnAwake()
    {
        base.InitDataOnAwake();
        this.uiId = E_UiId.LoadingUI;
        this.uiType.showMode = E_ShowUIMode.HideAll;
        this.uiType.uiPlayAudio = E_UIPlayAudio.NoPlay;
    }
    private void ChangeProgressText(float value)
    {  
        int txtValue= (int)(value * 100);
        txt_Value.text = txtValue + "%";
    }
  
    protected override void OnEnable()
    {
        base.OnEnable();
        //异步加载场景
        levelSceneName = SceneController.Instance.sceneNameAsync;
        StartCoroutine(LoadAsynScene());
        //Debug.Log("开始异步加载场景,场景名称为"+ levelSceneName);
    }
    IEnumerator LoadAsynScene()
    {   
        asyn = SceneManager.LoadSceneAsync(levelSceneName);
        asyn.allowSceneActivation = false;
        yield return asyn;
    }
    void Update()
    {
        if (asyn==null)
        {
            return;
        }
        //asyn.progress场景加载的实际进度,范围是0~1,但是要注意,这个值顶多检测到0.9
        if (asyn.progress < 0.9f)
        {
            //场景还未加载完毕
            inFactProgress = (int)asyn.progress * 100;
        }
        else
        {
            //场景已经加载完成
            inFactProgress = 100;
        }
        if (showProgress<inFactProgress)
        {
            showProgress++;
        }
        progressSlider.value = showProgress / 100f;
        if (progressSlider.value == 1)
        {
            asyn.allowSceneActivation = true;
            //UIManager.Instance.ShowUI(E_UiId.PlayUI);//这边不能写死
            if (SceneController.Instance.handlerAsync!=null)
            {
                SceneController.Instance.handlerAsync();
                SceneController.Instance.handlerAsync = null;
            }
           
            asyn = null;
            progressSlider.value = 0;
            showProgress = 0;
            inFactProgress = 0;
            GameTool.ClearMemory();
        }
       
    }
}
