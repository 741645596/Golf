using IGG.Core.Data.DataCenter;
using IGG.Core;
using UnityEngine.UI;
using IGG.Core.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpinDrag : MonoBehaviour
{
    public Image m_topProgress;
    public Image m_behindProgress;
    public Image m_leftProgress;
    public Image m_rightProgress;
    float SpeedUp = 3f;
    
    public GameObject m_objBall;
    private RectTransform m_spinRect;
    private Vector3 m_ballPos = Vector3.zero;
    private Camera m_uiCamera;
    private float m_moveRange;

    private Vector3 golfVector=Vector3.zero;
    private Vector3 moveVector = Vector3.zero;

    private float rangeX;               //拖拽范围
    private float rangeY;
    private Canvas m_canvas;
    public float m_maxDis;
    public GameObject m_objSpin;

    private bool IsStart = false;

    Vector3 m_spinPos;
    Vector3 m_spinWorldPos;
    /// <summary>
    /// 初始化窗口
    /// </summary>
    public void Start()
    {
        if (m_objSpin != null)
        { m_spinRect = m_objSpin.GetComponent<RectTransform>();
            m_spinWorldPos = m_objSpin.transform.position;
            golfVector = m_objSpin.GetComponent<RectTransform>().position;
        }
        if (m_objBall != null)
        { RectTransform ballRect = m_objBall.GetComponent<RectTransform>();
            m_moveRange = ballRect.rect.width / 2;
        }
        GameObject canvas = GameObject.Find("UI/Wnd");
        if (canvas!=null)
        {
            m_canvas = canvas.GetComponent<Canvas>();
        }
            GameObject uiCamera = GameObject.Find("UI/UICamera");
        if (uiCamera != null)
        {
            this.m_uiCamera = uiCamera.GetComponent<Camera>();           
        }
        m_ballPos = m_objSpin.transform.position;
        m_spinPos = transform.TransformDirection(m_spinRect.position);
    }

    void SetProgess()
    {
        Vector3 pos=Vector3.zero;
        if (m_spinRect != null && m_spinPos != null)
        {pos = Vector3.Cross(m_spinRect.position, m_spinPos); }

        if (m_topProgress != null && m_behindProgress != null && m_leftProgress != null && m_rightProgress != null)
        {
            m_topProgress.fillAmount = pos.x / 30f;
            m_behindProgress.fillAmount = -pos.x / 30f;
            m_leftProgress.fillAmount = pos.y / 30f;
            m_rightProgress.fillAmount = -pos.y / 30f;
        }
    }

    public void LongPress(bool bStart)
    {
        IsStart = bStart;
    }

    private void Update()
    {
        SetProgess();
        if (IsStart)
        {
            Vector3 globalMousePos=Vector3.zero;
           
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_objBall.GetComponent<RectTransform>(), Input.mousePosition, m_uiCamera
            , out globalMousePos))

            m_objSpin.GetComponent<RectTransform>().position = globalMousePos;
            if (Vector3.Distance(m_objSpin.transform.position, m_spinWorldPos) > m_maxDis)
            {
                Vector3 pos = (m_objSpin.transform.position - m_spinWorldPos).normalized;
                pos *= m_maxDis;
                m_objSpin.transform.position = pos + m_spinWorldPos;
            }
        }
    }
}
