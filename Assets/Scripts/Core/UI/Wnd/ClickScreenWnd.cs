using IGG.Core.Data.DataCenter;
using IGG.Core;
using UnityEngine.UI;
using IGG.Core.Helper;
using UnityEngine;
using System.Collections;

/// <summary>
/// Author  hlw
/// Date    2019.2.22
/// Desc    ClickScreenWnd界面交互逻辑
/// </summary>
public class ClickScreenWnd : WndBase
{

    /// <summary>
    /// 定义关联对象
    /// </summary>
    public GameObject goClickScene;
    public GameObject goClickUI;
    public float destroyDelay = 1;
    
    
    public Animator animatorScene = null;
    public Animator animatorUI = null;
    public GameObject imageScene = null;
    public GameObject imageUI = null;
    
    private Vector2 uiOffset;
    // <summary>
    // 加载完成立即初始化的操作
    // </summary>
    public override void Init()
    {
        //if (animatorScene == null) {
        //    animatorScene = goClickScene.GetComponentInChildren<Animator>();
        //    animatorUI = goClickUI.GetComponentInChildren<Animator>();
        
        //    imageScene = goClickScene.transform.GetChild(0).gameObject;
        //    imageUI = goClickUI.transform.GetChild(0).gameObject;
        //}
    }
    
    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
    
    public void ShowClickEffect(bool bUI)
    {
        goClickScene.SetActive(!bUI);
        goClickUI.SetActive(bUI);
        
        var ani = bUI ? animatorUI : animatorScene;
        var go = bUI ? imageUI : imageScene;
        if (go) {
            go.SetActive(true);
        }
        if (ani) {
            ani.enabled = true;
            ani.Play("Play", -1, 0);
        }
    }
    
    public void SetPosition(Vector2 screenPos)
    {
        var c = GetComponentInParent<Canvas>();
        var r = c.transform as RectTransform;
        uiOffset = new Vector2(r.sizeDelta.x / 2f, r.sizeDelta.y / 2f);
        
        Vector3 pos = screenPos / c.scaleFactor - uiOffset;
        transform.localPosition = pos;
        
        StopAllCoroutines();
        StartCoroutine(DelayDestroy());
    }
    
    /// <summary>
    /// 初始化窗口
    /// </summary>
    public override void InitWnd()
    {
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
