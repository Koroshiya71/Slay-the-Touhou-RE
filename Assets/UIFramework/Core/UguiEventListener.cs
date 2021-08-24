using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
 public class UguiEventListener : UnityEngine.EventSystems.EventTrigger
    {
        //EventTrigger是Unity里面自带的类，主要用来监测UI的单击、拖拽等行为
        public delegate void VoidDelegate(GameObject go);
        public VoidDelegate onClick;
        public VoidDelegate onDown;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onUp;
        public VoidDelegate onSelect;
        public VoidDelegate onUpdateSelect;

        public static UguiEventListener Get(GameObject go)
        {
            UguiEventListener listener = go.GetComponent<UguiEventListener>();
            if (listener == null)
            {
                listener = go.AddComponent<UguiEventListener>();
            }
            return listener;
        }
        //重写父类单击的方法
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null)
            {
                onClick(this.gameObject);
            }
            // Debug.Log("*******************单击");
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null) onDown(gameObject);
            // Debug.Log("*******************按下");
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null) onUp(gameObject);
            // Debug.Log("*******************抬起");
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null) onEnter(gameObject);
            //Debug.Log("*******************鼠标进入指定UI的范围");
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null) onExit(gameObject);
            //Debug.Log("*******************鼠标退出指定UI的范围");
        }
        public override void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null) onSelect(gameObject);
        }
        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null) onUpdateSelect(gameObject);
        }
    }

