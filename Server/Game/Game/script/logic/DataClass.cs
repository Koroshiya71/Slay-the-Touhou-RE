using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.script.logic
{
    public enum SceneType
    {
        NormalCombat,
        EliteCombat,
        Event,
        Store,
        Lounge,
        BossCombat
    }
    public enum BattleType
    {
        Normal,//普通战斗
        Elite,//精英战斗
        Boss,//Boss战
    }

    public class BattleData
    {
        //战斗类型
        private BattleType battleType;

        //战斗ID
        private int battleID;

        //敌人ID列表
        private List<int> enemyIDList = new List<int>();


        /// <summary>
        /// 属性
        /// </summary>
        public BattleType BattleType => battleType;

        public int BattleID => battleID;

        public List<int> EnemyIDList => enemyIDList;

        public BattleData()
        {

        }
    }
    public enum EventType
    {
        Battle,//战斗
        NormalEvent,//普通事件
        SpecialEvent,//特殊事件
    }
    //事件页面数据
    public class EventPageData
    {
        //页面ID
        public int pageID;

        //页面所属的事件ID
        public int parentEventID;

        //所属事件名称
        public string parentEventName;

        //页面名称
        public string pageName;

        //页面图片素材路径
        public string resourcePath;

        //页面描述
        public string pageDes;

        //选项数量
        public int choiceNum;

        //选项结果列表
        public List<EventResultData> resultList = new List<EventResultData>();

        public EventPageData()
        {

        }
    }
    public class EventResultData
    {
        //效果ID
        public int effectID;
        //效果值
        public int effectValue;
        //选择描述
        public string choiceDes;
        //下个页面ID
        public int nextPageID;
        //效果描述
        public string effectDes;
        //构造函数
        public EventResultData(string des, int nextPage, int effect, int value)
        {
            choiceDes = des;
            nextPageID = nextPage;
            effectID = effect;
            effectValue = value;
        }

        public EventResultData()
        {

        }
    }
    public class EventData
    {
        //事件ID
        public int eventID;
        //事件名称
        public string eventName;
        //事件类型
        public EventType eventType;
        //是否已经触发过
        public bool hasTriggered;
        //是否可以重复触发
        public bool canRecall;
        //前置事件ID
        public int preEventID;
        //页面列表
        public List<EventPageData> pageDataList = new List<EventPageData>();
        //构造函数
        public EventData(int ID)
        {
            eventID = ID;

        }

        public EventData()
        {

        }
    }
}
