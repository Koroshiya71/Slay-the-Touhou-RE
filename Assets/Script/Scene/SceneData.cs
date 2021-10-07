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
    //�ó���λ�ڵڼ���
    private int layer;

    public int Layer => layer;

    
    //��������
    private SceneType sceneType;

    public SceneType SceneType => sceneType;
    //ͼƬ�ز�·��
    private string resourcePath;

    public string ResourcePath => resourcePath; 
    
    //���캯��
    public SceneData(int index)
    {
        int sceneSeed = Random.Range(0,5);
        layer = index / 6+1;
        if (layer==1)
        {
            sceneType = SceneType.NormalCombat;
        }
        else 
        {
            //������������ɳ�������
            //TODO:��ʱΪ�ȸ�����������趨���ֳ����Ĳ�������
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

        switch (SceneType)
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
