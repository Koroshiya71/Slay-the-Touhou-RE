using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneType
{
    NormalCombat,
    EliteCombat,
    Event,
    Store,
    Lounge,
    BossCombat
}
public class SceneData
{
    //该场景位于第几层
    private int layer;

    public int Layer => layer;


    //场景类型
    public SceneType sceneType;

    
    //图片素材路径
    public string resourcePath;
    public string ResourcePath => resourcePath;

    //Boss场景构造函数
    public SceneData(bool isBoss)
    {
        layer = 21;
        sceneType = SceneType.BossCombat;
        resourcePath = "Image/" + "UIImage/" + "GameScene/" + "BossCombat";
    }
    //一般构造函数
    public SceneData(int index)
    {
        int sceneSeed = Random.Range(0, 5);
        layer = index / 6 + 1;
        if (layer == 1)
        {
            sceneType = SceneType.NormalCombat;
        }
        else if (layer == 20)
        {
            sceneType = SceneType.Lounge;
        }
        else
        {
            //根据随机数生成场景类型
            //TODO:暂时为等概率随机，待设定各种场景的产生概率
            switch (sceneSeed)
            {
                case 0:
                    sceneType = SceneType.NormalCombat;
                    break;
                case 1:
                    sceneType = SceneType.EliteCombat;
                    break;
                case 2:
                    sceneType = SceneType.Store;
                    break;
                case 3:
                    sceneType = SceneType.Event;
                    break;
                case 4:
                    sceneType = SceneType.Lounge;
                    break;
            }
        }

        switch (sceneType)
        {
            case SceneType.NormalCombat:
                resourcePath = "Image/" + "UIImage/" + "GameScene/" + "NormalCombat";
                break;
            case SceneType.EliteCombat:
                resourcePath = "Image/" + "UIImage/" + "GameScene/" + "EliteCombat";
                break;
            case SceneType.Store:
                resourcePath = "Image/" + "UIImage/" + "GameScene/" + "Store";
                break;
            case SceneType.Lounge:
                resourcePath = "Image/" + "UIImage/" + "GameScene/" + "Lounge";
                break;
            case SceneType.BossCombat:
                resourcePath = "Image/" + "UIImage/" + "GameScene/" + "BossCombat";
                break;
            case SceneType.Event:
                resourcePath = "Image/" + "UIImage/" + "GameScene/" + "Event";
                break;
        }
    }
    
}
