using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using GameCore;

public class InitProject : MonoBehaviour {
    //Logo播放完动画所需的时间
    private float showTime =  2;
    //计时器
    private float time = 0;
    //异步加载的对象
    AsyncOperation asyn;
   // private GameObject ca;
	void Start ()
    {
        //加载画布
        GameObject canvasPrefab = Resources.Load<GameObject>("UIPrefab/Canvas");
        GameObject canvas = Instantiate(canvasPrefab);
        //添加全局协程控制类
        GameTool.AddTheChildComponent<CoroutineController>(canvas, "UnitySingletonObj");
        //先把AudioManager挂上去,再挂UIManager,因为UIManager会调用到AudioManager里面的方法
        GameTool.AddTheChildComponent<AudioManager>(canvas, "UnitySingletonObj");
        GameTool.AddTheChildComponent<UIManager>(canvas, "UnitySingletonObj");
        //切换场景的时候,画布不被销毁
        DontDestroyOnLoad(canvas); 
        //加载所有到配置表到内存里面
        DataController.Instance.LoadAllCfg();
        //初始化数据
        LoadData();
     
        StartCoroutine(LoadMainScene("MainScene"));
	}
    private void LoadData()
    {
        //PackData.Instance.InitPackData();
        //InforData.Instance.InitInforData();
        //PlayerData.Instance.InitPlayerData();
    }
    IEnumerator LoadMainScene(string sceneName)
    {
        asyn = SceneManager.LoadSceneAsync(sceneName);
        asyn.allowSceneActivation = false;
        yield return asyn;
    }
    private void Init()
    {

    }
	void Update ()
    {
        time += Time.deltaTime;
        if (time>= showTime&&asyn.progress>=0.9f)
        {
            asyn.allowSceneActivation = true;
            //初始化音效管理器
            AudioManager.Instance.InitAudioManager();
            //初始化UI管理器
            UIManager.Instance.InitUIManager();
         
        }
    }
}
