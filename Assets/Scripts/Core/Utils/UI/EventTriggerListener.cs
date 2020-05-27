using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// 事件触发封装 - 需要什么事件可扩展
/// Event trigger listener.
/// </summary>
public class EventTriggerListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler {
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onOBDrag;
    public VoidDelegate onOBEndDrag;
    static public EventTriggerListener Get(GameObject go) {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null) listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }

    public void OnDrag(PointerEventData eventData) {
        if (onOBDrag != null) onOBDrag(gameObject);
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (onOBEndDrag != null) onOBEndDrag(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (onClick != null) onClick(gameObject);
    }
    public void OnPointerDown(PointerEventData eventData) {
        if (onDown != null) onDown(gameObject);
    }
    public void OnPointerEnter(PointerEventData eventData) {
        if (onEnter != null) onEnter(gameObject);
    }
    public void OnPointerExit(PointerEventData eventData) {
        if (onExit != null) onExit(gameObject);
    }
    public void OnPointerUp(PointerEventData eventData) {
        if (onUp != null) onUp(gameObject);
    }
}