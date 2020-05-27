using IGG.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using IGG.Core.Data.DataCenter.Battle;
using System.Collections.Generic;
using IGG.Core.Data.DataCenter.GolfAI;

public class RingDrag : MonoBehaviour
{
    public static bool s_check = false;
    //声明从鼠标发出一条射线clickRay
    Ray clickRay;

    //声明clickRay与游戏物体的碰撞
    RaycastHit clickPoint;

    //声明clickRay与地面的碰撞
    RaycastHit posPoint;
    //设置地面层，我的地面层是第8层，所以是8。不会设置层的话请看下边的Tips。
    public LayerMask mask ;

    public LayerMask uiLayerMask;

    Vector3 m_prevPos = Vector3.zero;

    bool isMouseDown = false;
    Vector3 orginMousePosition = Vector3.zero;
    bool m_isClickUi = false;
    private GolfRing ring;

   void Start()
    {
        uiLayerMask = LayerMask.NameToLayer("UI");
        ring = transform.parent.GetComponent<GolfRing>();
    }

    void Update()
    {
        
        clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!s_check)
        {
            if (!isMouseDown)
            {
                if (Application.isMobilePlatform && Input.touchCount > 0)
                {
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId)
                            && (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.layer == uiLayerMask))
                        {
                            m_isClickUi = true;
                            break;
                        }
                    }
                }
                else if (EventSystem.current.IsPointerOverGameObject()
                    && (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.layer == uiLayerMask))
                {
                    m_isClickUi = true;
                }

                if (Input.GetMouseButtonDown(0) && !m_isClickUi)
                {
                    Physics.Raycast(clickRay, out clickPoint);
                    if (clickPoint.collider != null && clickPoint.collider.gameObject.layer == uiLayerMask)
                    {
                        return;
                    }
                    isMouseDown = true;
                    orginMousePosition = Input.mousePosition;
                }
            }
            if (isMouseDown)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector3 dir = Input.mousePosition - orginMousePosition;
                    if (dir.magnitude > 0.5f)
                    {
                        //Debug.Log("Input.mousePosition:" + Input.mousePosition + "orginMousePosition:" + orginMousePosition + "dir:" + dir);
                        EventCenter.DispatchEvent(EventCenterType.Battle_DragMapMoveCamera, -1, dir);
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    isMouseDown = false;
                    orginMousePosition = Vector3.zero;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                EventCenter.DispatchEvent(EventCenterType.Battle_ScaleMapMoveCamera, -1, Input.GetAxis("Mouse ScrollWheel"));
            }
        }
    }
    void OnMouseDown()
    {
        RingDrag.s_check = false;
        //如果射线与物体相碰，则调用OnMouseDrag()
        if (Physics.Raycast(clickRay, out clickPoint))
        {
            OnMouseDrag();
        }
        
    }
    void OnMouseDrag()
    {
        RaycastHit hit;
        bool bCollision = Physics.Raycast(clickRay.origin, clickRay.direction, out hit, Mathf.Infinity, mask);
        if (bCollision)
        {
            Vector3 mouseMove = hit.point;
            RingDrag.s_check = true;
            if (ring != null)
            {
                ring.SetRingPos(new Vector3(mouseMove.x, transform.position.y, mouseMove.z));
            }
            if (Vector3.Distance(m_prevPos, mouseMove) > 0.5f)
            {
                m_prevPos = mouseMove;
            }
        }
    }

    private void OnMouseUp()
    {
        RingDrag.s_check = false;
    }

    private void OnMouseExit()
    {
        RingDrag.s_check = false;
    }
}


