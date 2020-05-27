using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;
using IGG.Core.Data.Config;

// LoadingWnd 窗口 by zhulin
public class LoadingWnd : WndBase, ILoading
{

    // 窗口初始华
    public Image m_tipImage;
    private Action<ILoading> m_callback = null;
    private float m_waitTime = 0.0f;
    public Animator m_WndAni;
    private float m_runTime = 0.0f;
    
    /// <summary>
    /// 复选框是否已经选中
    /// </summary>
    public float fProcess {
        get { return m_tipImage.fillAmount; }
        set {
            if (m_tipImage)
            {
                m_tipImage.fillAmount = value;
            }
        }
    }
    public override void InitWnd()
    {
        RegisterHooks();
        InitUI();
    }
    
    public void SetData(float waitTime, Action<ILoading> callback)
    {
        fProcess = 0.0f;
        m_callback = callback;
        m_waitTime = waitTime;
    }
    
    private void InitUI()
    {
    
    }
    
    private void WaitAni()
    {
        if (m_callback != null) {
            m_callback(this);
            m_callback = null;
        }
    }
    
    
    // 窗口内事件绑定
    protected override void BindEvents()
    {
    }
    
    // 显示窗口时播放动画
    public override void PlayShowAni()
    {
    
    }
    
    // 关闭窗口时播放动画
    public override void PlayCloseAni()
    {
        if (m_WndAni != null) {
            m_WndAni.SetTrigger("Close");
        }
    }
    
    // 注册消息函数
    private void RegisterHooks()
    {
    }
    
    // 反注册消息函数
    private void AntiRegisterHooks()
    {
    }
    
    // 反注册消息函数
    public virtual void OnDestroy()
    {
        AntiRegisterHooks();
    }
    
    public void Play(int Progress)
    {
        if (this != null) {
            fProcess = Progress * 0.01f;
        }
    }
    
    public void Load()
    {
    
    }
    
    public void TryDestroy()
    {
        //WndManager.DestoryWnd<LoadingWnd>(0.5f + 1.0f);
        WndManager.DestoryWnd<LoadingWnd>(0.5f);
    }
    
    private void Update()
    {
        if (m_callback != null) {
        
            m_runTime += Time.deltaTime;
            
            if (m_waitTime <= m_runTime) {
                WaitAni();
                TryDestroy();
            }
            
            fProcess = m_runTime / m_waitTime;
        }
    }
}