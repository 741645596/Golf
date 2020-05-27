using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

// ConfirmWnd 窗口 by zhulin
public class ConfirmWnd : WndBase ,IMsgBoxResult
{

    public Animator m_ani;
    public Button m_btn1;
    public Button m_btn2;
    public Button m_btn3;
    public Button m_btn4;
    public TextMeshProUGUI m_txtBtn1;
    public TextMeshProUGUI m_txtBtn2;
    public TextMeshProUGUI m_txtBtn3;
    public TextMeshProUGUI m_txtBtn4;

    public TextMeshProUGUI m_txtTitle;
    public TextMeshProUGUI m_txtMsg;

    private Action<IMsgBoxResult> m_callback = null;
    // 窗口初始华
    public override void InitWnd() {
    	RegisterHooks();
        //
    }

    //用户点击的按钮
    public TipButton ClickButton { private set; get; }

    /// <summary>
    /// 复选框是否已经选中
    /// </summary>
    public bool Selected
    {
        get { return false; }
    }


    // 窗口内事件绑定
    protected override void BindEvents() {
        m_btn1.onClick.AddListener(Btn1Click);
        m_btn2.onClick.AddListener(Btn2Click);
        m_btn3.onClick.AddListener(Btn3Click);
        m_btn4.onClick.AddListener(Btn4Click);
    }

    public void SetData(string msg, string title, MsgBoxType type = MsgBoxType.Ok, Action<IMsgBoxResult> callback = null)
    {
        m_callback = callback;
        m_btn1.gameObject.SetActive(false);
        m_btn2.gameObject.SetActive(false);
        m_btn3.gameObject.SetActive(false);
        m_btn4.gameObject.SetActive(false);
        if (type == MsgBoxType.Ok)
        {
            m_btn3.gameObject.SetActive(true);
            m_txtBtn3.text = "确定";
        }
        else if (type == MsgBoxType.Ok1)
        {
            m_btn4.gameObject.SetActive(true);
            m_txtBtn4.text = "确定";
        }
        else if (type == MsgBoxType.OKCancel)
        {
            m_btn1.gameObject.SetActive(true);
            m_btn2.gameObject.SetActive(true);
            m_txtBtn1.text = "确定";
            m_txtBtn2.text = "取消";
        }
        m_txtTitle.text = title;
        m_txtMsg.text = msg;
    }

    private void DoCallback(TipButton btn)
    {
        ClickButton = btn;
        if (m_callback != null)
        {
            m_callback(this);
            m_callback = null;
        }
        m_ani.SetTrigger("Close");
        WndManager.DestoryWnd<ConfirmWnd>(0.3f);
    }

    private void OnOkHandler()
    {
        DoCallback(TipButton.Ok);
    }

    private void OnCancelHandler()
    {
        DoCallback(TipButton.Cancel);
    }

    private void OnCloseHandler()
    {
        DoCallback(TipButton.Close);
    }

    public void Btn1Click()
    {
        OnOkHandler();
    }


    public void Btn2Click()
    {
        OnCancelHandler();
    }



    public void Btn3Click()
    {
        OnOkHandler();
    }

    public void Btn4Click()
    {
        OnOkHandler();
    }

    // 注册消息函数
    private void RegisterHooks() {
    }

    // 反注册消息函数
    private void AntiRegisterHooks() {
    }

    // 反注册消息函数
    public virtual void OnDestroy() {
    	AntiRegisterHooks();
    }
}


public enum MsgBoxType
{
    /// <summary>
    ///  只有确认
    /// </summary>
    Ok,

    /// <summary>
    ///  只有确认样式1
    /// </summary>
    Ok1,
    /// <summary>
    /// 有确认取消
    /// </summary>
    OKCancel,
}

public enum TipButton
{
    /// <summary>
    ///  确认
    /// </summary>
    Ok,

    /// <summary>
    /// 取消
    /// </summary>
    Cancel,

    /// <summary>
    /// 有确认和取消按钮
    /// </summary>
    Close,

}
public interface IMsgBoxResult
{
    /// <summary>
    /// 玩家点击了那个按钮
    /// </summary>
    TipButton ClickButton { get; }

    //是否选择了复选框
    bool Selected { get; }
}