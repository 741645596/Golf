using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using IGG.Core.Helper;
using IGG.Core.Manger.Coroutine;

// <summary>
// 窗体基类
// </summary>
// <author>zhulin</author>
public class WndBase : MonoBehaviour
{

    private WndType m_WndType = WndType.NormalWnd;
    
    private bool m_bShow = true;
    
    
    public bool IsShow {
        get {
            return m_bShow;
        }
    }
    
    
    
    private Transform thisT;
    public Transform tWnd {
        get {
            if (thisT == null)
            {
                thisT = GetComponent<Transform>();
            }
            return thisT;
        }
    }
    
    public virtual void Awake()
    {
        Init();
    }
    
    public virtual void Start()
    {
        InitWnd();
        BindEvents();
        PlayShowAni();
    }
    
    
    // <summary>
    // 窗口加载完成做的初始化动作
    // </summary>
    public virtual void Init()
    {
    
    }
    
    /// <summary>
    /// 打开窗口传参数
    /// </summary>
    /// <param data="obj"></param>
    public virtual void SetData(object[] data)
    {
    
    }
    
    /// <summary>
    /// 打开窗口传参数
    /// </summary>
    /// <param data="obj"></param>
    public virtual void SetData(object data)
    {
    
    }
    // <summary>
    // 窗口初始华
    // </summary>
    public virtual void InitWnd()
    {
    
    }
    /// <summary>
    /// 窗口内事件绑定
    /// </summary>
    protected virtual void BindEvents()
    {
    
    }
    // <summary>
    // 显示或隐藏窗口
    // </summary>
    // <param name="isShow"></param>
    public virtual void ShowWnd(bool isShow)
    {
        gameObject.SetActive(true);
        PlayShowAni();
    }
    // <summary>
    // 销毁窗口（下一帧）
    // </summary>
    public void DestroyWnd(float destroyTime = 0.0f)
    {
        if (destroyTime == 0.0f) {
            ResourceManger.FreeGo(gameObject);
        } else {
            PlayCloseAni();
            StartCoroutine(FreeWndRes(destroyTime));
        }
    }
    // <summary>
    // 窗口显示时播放动画
    // </summary>
    public virtual void PlayShowAni()
    {
    
    }
    
    
    // <summary>
    // 窗口关闭时播放动画
    // </summary>
    public virtual void PlayCloseAni()
    {
    
    }
    
    private IEnumerator FreeWndRes(float destroyTime)
    {
        yield return Yielders.GetWaitForSeconds(destroyTime);
        ResourceManger.FreeGo(gameObject);
    }
    // <summary>
    // 判断是否为全屏窗口
    // </summary>
    public virtual bool CheckFullWnd()
    {
        return true;
    }
    // <summary>
    // 确认窗口是否在最顶层
    // </summary>
    public bool CheckTopWnd()
    {
        return WndManager.CheckTopWnd(this);
    }
    // <summary>
    // 获取窗口类型
    // </summary>
    public virtual WndType GetWndType()
    {
        return m_WndType;
    }
    
    // <summary>
    // 获取窗口类型
    // </summary>
    public virtual void SetWndType(WndType wndType)
    {
        m_WndType = wndType;
    }
    
    //刷新界面
    public virtual void RefreshUI()
    {
    
    }
    
    
    protected void SetImage(Image img, string altas, string spriteName)
    {
        if (img != null) {
            ResourceManger.LoadSprite(altas, spriteName, false, (g) => {
                if (null != g) {
                    Sprite sprite = g as Sprite;
                    if (null != sprite) {
                        img.sprite = sprite;
                    }
                } else {
                    img.sprite = null;
                }
            });
        }
    }
    
    private void OnValidate()
    {
        Reset();
    }
    
    protected virtual void Reset()
    { }
}


public enum WndType {
    NormalWnd = 0,
    MenuWnd = 1,
    DialogWnd = 2,
}

