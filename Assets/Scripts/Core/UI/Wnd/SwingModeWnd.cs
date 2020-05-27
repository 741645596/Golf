using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using IGG.Core.Manger.Sound;
using IGG.Core.Manger.Coroutine;

/// <summary>
/// Author  填写你的大名
/// Date    2019.5.9
/// Desc    SwingModeWnd界面交互逻辑
/// </summary>
public class SwingModeWnd : WndBase {
   
    public override void Init()
    {
    }
    
    /// <summary>
    /// 初始化窗口
    /// </summary>
    public override void InitWnd() {
        RegisterHooks();
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
    protected override void BindEvents() {
	    // 关联按钮事件示例
        //m_btnBg.onClick.AddListener(BtnBgClick);
    }
	
	//刷新界面
    public override void RefreshUI() {

    }

    /// <summary>
    /// 打开窗口时播放动作
    /// </summary>
    public override void PlayShowAni() {
    }

    /// <summary>
    /// 关闭窗口时播放动画
    /// </summary>
    public override void PlayCloseAni() {
    }

    /// <summary>
    /// 注册消息事件
    /// </summary>
    private void RegisterHooks() {
        // 示例注册客户端触发事件
        // EventCenter.RegisterHooks(EventCenterType.xxx, func);
        // 示例注册服务端触发事件
        // EventCenter.RegisterHooks(msgtype.MsgType.xxx, func);
    }

    /// <summary>
    /// 反注册消息事件
    /// </summary>
    private void AntiRegisterHooks() {
        // 示例注册客户端触发事件
        // EventCenter.AntiRegisterHooks(EventCenterType.xxx, func);
        // 示例注册服务端触发事件
        // EventCenter.AntiRegisterHooks(msgtype.MsgType.xxx, func);
    }

    /// <summary>
    /// 窗口销毁时的清理动作
    /// </summary>
    public virtual void OnDestroy() {
    	AntiRegisterHooks();
    }
   
   
}
