using IGG.Core.Data.DataCenter;
using IGG.Core;
using UnityEngine.UI;
using IGG.Core.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlagItemWnd : WndBase
{


    /// <summary>
    /// Author  填写你的大名
    /// Date    2019.6.21
    /// Desc    FlagWnd界面交互逻辑
    /// </summary>

    /// <summary>
    /// 定义关联对象
    /// </summary>
    public GameObject m_Flag;
    private Camera m_uiCamera;


    // <summary>
    // 加载完成立即初始化的操作
    // </summary>
    public override void Init()
    {

    }

    /// <summary>
    /// 初始化窗口
    /// </summary>
    public override void InitWnd()
    {
        RegisterHooks();
        GameObject uiCamera = GameObject.Find("UI/UICamera");
        if (uiCamera != null)
        {
            this.m_uiCamera = uiCamera.GetComponent<Camera>();

        }
    }
    public Vector3 flagWorld;
    bool open=false;
     void Update()
    {
        if (open && m_Flag != null)
        { m_Flag.GetComponent<RectTransform>().localPosition = Camera.main.WorldToScreenPoint(flagWorld) - new Vector3(Screen.width / 2, Screen.height / 2, 0); }
    }
    
    /// <summary>
    /// 打开窗口传参数
    /// </summary>
    /// <param name="obj"></param>
    public override void SetData(object obj)
    {
        open = true;
        if (m_Flag!=null)
        {
            flagWorld = (Vector3)obj + new Vector3(0 ,3.5f, 0);
        }
    }
    /// <summary>
    /// 绑定按钮事件
    /// </summary>
    protected override void BindEvents()
    {
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


