using IGG.Core.Data.DataCenter;
using IGG.Core;
using UnityEngine.UI;
using IGG.Core.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author  linhong
/// Date    2019.5.8
/// Desc    MineBallSpinWnd界面交互逻辑
/// </summary>
public class MineBallSpinWnd : WndBase{
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

    private Vector3 golfVector = Vector3.zero;
    private Vector3 moveVector = Vector3.zero;

    private float rangeX;               //拖拽范围
    private float rangeY;
    private Canvas m_canvas;
    public float m_maxDis;
    public GameObject m_objSpin;

    private bool IsStart = false;

    Vector3 m_spinPos;
    Vector3 m_spinWorldPos;

    public float m_leftSpin;
    public float m_rightSpin;
    public float m_forceSpin;
    public float m_behindSpin;
    public float m_LRspin;
    public float m_CurFBSpin;
    void SetProgess()
    {
        Vector3 pos = Vector3.zero;
        if (m_spinRect != null && m_spinPos != null)
        { pos = Vector3.Cross(m_spinRect.position, m_spinPos); }

        if (m_topProgress != null && m_behindProgress != null && m_leftProgress != null && m_rightProgress != null)
        {
            m_topProgress.fillAmount = pos.x / 30f;
            m_behindProgress.fillAmount = -pos.x / 30f;
            m_leftProgress.fillAmount = pos.y / 30f;
            m_rightProgress.fillAmount = -pos.y / 30f;

            m_leftSpin = m_leftProgress.fillAmount;
            m_rightSpin = m_rightProgress.fillAmount;
            m_forceSpin = m_topProgress.fillAmount;
            m_behindSpin = m_behindProgress.fillAmount;
            if(m_leftSpin>0)
            {
                m_LRspin = -m_leftSpin;
            }
            else
            {
                m_LRspin = m_rightSpin;
            }
            if (m_forceSpin > 0)
            {
                m_CurFBSpin = -m_forceSpin;
            }
            else
            {
                m_CurFBSpin = m_behindSpin;
            }
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
           // Debug.Log("1");
            Vector3 globalMousePos = Vector3.zero;

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

    public Button m_btnClose;

    public override void InitWnd() {
 //      
        
        if (m_objSpin != null)
        {
            m_spinRect = m_objSpin.GetComponent<RectTransform>();
            m_spinWorldPos = m_objSpin.transform.position;
            golfVector = m_objSpin.GetComponent<RectTransform>().position;
           
        }
        if (m_objBall != null)
        {
            RectTransform ballRect = m_objBall.GetComponent<RectTransform>();
            m_moveRange = ballRect.rect.width / 2;
        }
        GameObject canvas = GameObject.Find("UI/Wnd");
        if (canvas != null)
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
    private void HideWnd()
    {
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// 打开窗口传参数
    /// </summary>
    /// <param name="obj"></param>
    public override void SetData(object obj)
    {

    }
    /// <summary>
    /// 绑定按钮事件
    /// </summary>
    protected override void BindEvents()
    {
        m_btnClose.onClick.AddListener(HideWnd);
        // 关联按钮事件示例
        //m_btnBg.onClick.AddListener(BtnBgClick);
    }

    //刷新界面
    public override void RefreshUI()
    {

    }

    /// <summary>
    /// 打开窗口时播放动作
    /// </summary>
    public override void PlayShowAni()
    {
    }

    /// <summary>
    /// 关闭窗口时播放动画
    /// </summary>
    public override void PlayCloseAni()
    {
    }

    /// <summary>
    /// 注册消息事件
    /// </summary>
    private void RegisterHooks()
    {
        // 示例注册客户端触发事件
        // EventCenter.RegisterHooks(EventCenterType.xxx, func);
        // 示例注册服务端触发事件
        // EventCenter.RegisterHooks(msgtype.MsgType.xxx, func);
    }

    /// <summary>
    /// 反注册消息事件
    /// </summary>
    private void AntiRegisterHooks()
    {
        // 示例注册客户端触发事件
        // EventCenter.AntiRegisterHooks(EventCenterType.xxx, func);
        // 示例注册服务端触发事件
        // EventCenter.AntiRegisterHooks(msgtype.MsgType.xxx, func);
    }

    /// <summary>
    /// 窗口销毁时的清理动作
    /// </summary>
    public virtual void OnDestroy()
    {
        AntiRegisterHooks();
    }





}
