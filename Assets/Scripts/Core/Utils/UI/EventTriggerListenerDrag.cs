using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// 事件触发封装 - 需要什么事件可扩展
/// Event trigger listener.
/// </summary>
public class EventTriggerListenerDrag : MonoBehaviour, IDragHandler, IEndDragHandler {
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onOBDrag;
    public VoidDelegate onOBEndDrag;
    static public EventTriggerListenerDrag Get(GameObject go) {
        EventTriggerListenerDrag listener = go.GetComponent<EventTriggerListenerDrag>();
        if (listener == null) listener = go.AddComponent<EventTriggerListenerDrag>();
        return listener;
    }

    public void OnDrag(PointerEventData eventData) {
        if (onOBDrag != null) onOBDrag(gameObject);
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (onOBEndDrag != null) onOBEndDrag(gameObject);
    }
}