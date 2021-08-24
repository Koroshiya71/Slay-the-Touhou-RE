using UnityEngine;
using System.Collections;
using System;
namespace GameCore
{
    //窗体类型(显示类型与父节点类型、播放音效类型)
    public class UIType
    {
        //如何确定默认值的类型?用到的比较多的就设置为默认值
        public E_ShowUIMode showMode = E_ShowUIMode.HideOther;
        public E_UIRootType uiRootType = E_UIRootType.Normal;
        public E_UIPlayAudio uiPlayAudio = E_UIPlayAudio.Play;
    }
    //UI基类
    //主要是封装窗体的共同属性以及行为
    public class BaseUI : MonoBehaviour, IMessageListener
    {
        //窗体类型
        public UIType uiType;
        //窗体的位置信息
        protected RectTransform thisTrans;
        //当前窗体的ID
        protected E_UiId uiId = E_UiId.NullUI;
        //上一个跳转过来的窗体ID
        private E_UiId beforeUiId = E_UiId.NullUI;
        //获取当前窗体的ID
        public E_UiId GetUiId
        {
            get
            {
                return uiId;
            }
            //为什么没有set?
            //因为每一个窗体的ID是固定的,只能让外界读取,而不能在外界去修改它
        }
        //上一个窗体Id的属性(可读可写)
        public E_UiId BeforeUiId
        {
            get
            {
                return beforeUiId;
            }

            set
            {   
                beforeUiId = value;
            }
        }
        //对外提供一个属性,判断显示出来后是否需要处理其他窗体(是否需要隐藏其他窗体)
        public bool isNeedDealWithUI
        {
            get
            {
                if (this.uiType.uiRootType == E_UIRootType.KeepAbove)
                {
                    return false;
                }
                return true;

            }
        }

       
        protected virtual void Awake()
        {
            if (uiType == null)
            {
                uiType = new UIType();
            }
            thisTrans = this.GetComponent<RectTransform>();
            InitUiOnAwake();
            InitDataOnAwake();
        }
        protected virtual void Start()
        {

        }
        //初始化界面元素(查找界面元素,获取监听等逻辑就放在这个方法里面)
        protected virtual void InitUiOnAwake()
        {

        }
        //初始化界面数据(窗体的ID、窗体类型等)
        protected virtual void InitDataOnAwake()
        {
            AddMessageListener();
        }
        //显示窗体
        public virtual void ShowUI()
        {
            this.gameObject.SetActive(true);

        }
        //隐藏窗体
        public virtual void HideUI(Action del=null)
        {
            this.gameObject.SetActive(false);
            if (del != null)
            {
                del();
            }
            //保存数据
            Save();
        }
        //保存数据
        protected virtual void Save()
        {

        }
        //窗体显示出来的时候播放音效
        protected virtual void PlayAudio()
        {
            //调用播放音效的方法
            AudioManager.Instance.PlayEffectMusic("UIShow");
        }
        protected virtual void OnDestroy()
        {
            RemoveMessageListener();
        }
        protected virtual void OnEnable()
        {
            //if (this.uiType.uiPlayAudio==E_UIPlayAudio.Play)
            //{
            //    PlayAudio();
            //}
           
        }
        protected virtual void OnDisable()
        {

        }
        public virtual void AddMessageListener()
        {
           
        }

        public virtual void RemoveMessageListener()
        {
           
        }
    }

}
