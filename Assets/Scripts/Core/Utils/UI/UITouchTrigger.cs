using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UITouchTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, ICancelHandler, IEndDragHandler
{
    public EventTrigger.TriggerEvent OnClickGetHouseFunc;
    //public EventTrigger.TriggerEvent OnClickUpFunc;

    private bool IsDrag = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        IsDrag = true;
        Debug.Log("down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Do action
//         if (null != OnClickUpFunc)
//         {
//             OnClickUpFunc.Invoke(eventData);
//         }

        
        Debug.Log("up");
    }

    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {

        }

        Touch[] touches = Input.touches;
        if (null != touches && touches.Length >= 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {

        }
    }

    public void OnCancel(BaseEventData data)
    {
        Debug.Log("OnCancel called.");
    }

    public  void OnEndDrag(PointerEventData data)
    {
        Debug.Log("OnEndDrag called.");
    }

    // 移动到外面了
    public void OnPointerExit(PointerEventData eventData)
    {
        // Do action
        if (null != OnClickGetHouseFunc && IsDrag)
        {
            OnClickGetHouseFunc.Invoke(eventData);
            IsDrag = false;
        }

        //Debug.Log("out");
    }
}
