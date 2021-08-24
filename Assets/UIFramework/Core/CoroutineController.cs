using UnityEngine;
using System.Collections;
using GameCore;
//全局协程控制器
public class CoroutineController : UnitySingleton<CoroutineController>
{
    //如果在一个隐藏物体上面的脚本里,要去开启一个协程,那么就调用它
    //外界脚本的调用方法:CoroutineController.Instance.StartCoroutine(方法名());

}
