using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 窗口管理类
/// <para>窗口预制路径Resource/Prefabs/UI</para>
/// <para>窗口命名和所挂脚本一致,脚本继承WndBase类</para>
/// </summary>

public delegate void WndCreateHook(WndBase Wnd);

public class WndManager
{
    private static UINode g_uiNode = null;
    private static List<WndBase> m_lwnd = new List<WndBase>();
    
    /// <summary>
    /// 设置窗体跟节点
    /// </summary>
    /// <returns></returns>
    public static void SetUINode(UINode node)
    {
        g_uiNode = node;
    }
    
    public static UINode GetUINode()
    {
        return g_uiNode;
    }
    
    // 获取UI 挂取结点
    public static Transform GetWndParent(WndType wndType)
    {
        if (null == g_uiNode) {
            return null;
        }
        return g_uiNode.GetWndParent(wndType);
    }
    
    //如果不是特别“重”的常用窗口，尽量IsCache = false, 后台会帮你保留
public static bool CreateWnd<T>(WndType wndtype, bool IsCache, bool async, WndCreateHook pfun) where T :
    WndBase {
        bool ret = false;
        string WndName = typeof(T).ToString();
        Transform parent = WndManager.GetWndParent(wndtype);
        
        // 已经存在
        T wnd = FindWnd<T>();
        if (wnd != null)
        {
            SetWndTop(wnd);
            //show the wnd
            wnd.ShowWnd(true);
            if (pfun != null) {
                pfun(wnd);
            }
            return true;
        }
        // 不存在则创建了。
        ResourceManger.LoadWnd(WndName, parent, IsCache, async,
            (g) =>
        {
            if (g != null)
            {
                wnd = g.GetComponent<T>
                ();
                wnd.SetWndType(wndtype);
                AddWnd(wnd);
                if (pfun != null) {
                    pfun(wnd);
                }
                ret = true;
            }
        });
        return ret;
    }
    
    /// <summary>
    /// 按类型搜索。
    /// </summary>
public static T FindWnd<T>() where T :
    WndBase {
        for (int i = 0; i < m_lwnd.Count; i++)
        {
            if (m_lwnd[i] == null) {
                continue;
            }
            if (m_lwnd[i] is T) {
                return (T)m_lwnd[i];
            }
        }
        return default(T);
    }
    
    /// <summary>
    /// 隐藏显示窗口
    /// </summary>
public static T ShowWnd<T>(bool IsShow) where T :
    WndBase {
        T wnd = FindWnd<T>();
        if (wnd != null)
        {
            wnd.ShowWnd(IsShow);
            return (T)wnd;
        } else
        {
            return default(T);
        }
    }
    
    
    /// <summary>
    /// 按类型搜索。
    /// </summary>
public static List<T> SearchWnd<T>() where T :
    WndBase {
        List<T> list = new List<T>();
        for (int i = 0; i < m_lwnd.Count; i++)
        {
            if (m_lwnd[i] == null) {
                continue;
            }
            if (m_lwnd[i] is T) {
                list.Add((T)m_lwnd[i]);
            }
        }
        return list;
    }
    
    
    /// 按类型销毁窗口
public static bool DestoryWnd<T>(float destroyTime = 0.0f) where T :
    WndBase {
        List<T> l = SearchWnd<T>();
        if (l.Count == 0)
        {
            return false;
        }
        
        foreach (T wnd in l)
        {
            if (wnd != null) {
                wnd.DestroyWnd(destroyTime);
                m_lwnd.Remove(wnd);
            }
        }
        return true;
    }
    
    /// 销毁指定窗口
    public static bool DestoryWnd(WndBase wnd)
    {
        if (wnd != null) {
            wnd.DestroyWnd(0.0f);
            m_lwnd.Remove(wnd);
        }
        return true;
    }
    
    
    /// 销毁所有窗口
    /// </summary>
    public static void DestroyAllWnd()
    {
        for (int i = 0; i < m_lwnd.Count; i++) {
            if (m_lwnd[i] != null) {
                m_lwnd[i].DestroyWnd(0.0f);
            }
        }
        m_lwnd.Clear();
    }
    /// <summary>
    /// 窗口置顶
    /// </summary>
    public static void SetWndTop(WndBase Wnd)
    {
        if (Wnd == null) {
            return;
        }
        //设置为最后一个位置
        m_lwnd.Remove(Wnd);
        m_lwnd.Add(Wnd);
        
        g_uiNode.MoveChild2Last(Wnd);
    }
    
    /// <summary>
    /// 窗口置底
    /// </summary>
    public static void SetWndBottom(WndBase Wnd)
    {
        if (Wnd == null) {
            return;
        }
        //设置为最开始一个位置
        m_lwnd.Remove(Wnd);
        g_uiNode.MoveChild2First(Wnd);
    }
    
    /// <summary>
    /// 清理数据
    /// </summary>
    public static void Clear()
    {
        m_lwnd.Clear();
        if (g_uiNode != null) {
            g_uiNode.ClearChild();
        }
    }
    
    // <summary>
    // 确认窗口是否在最顶层
    // </summary>
    public static bool CheckTopWnd(WndBase Wnd)
    {
        if (Wnd == null) {
            return false;
        }
        
        if (Wnd.GetWndType() != WndType.NormalWnd) {
            return true;
        }
        
        int index = -1;
        int max = 0;
        for (int i = 0; i < m_lwnd.Count; i++) {
            if (m_lwnd[i] != null && m_lwnd[i].GetWndType() == WndType.NormalWnd) {
                max = i;
                if (m_lwnd[i] == Wnd) {
                    index = i;
                }
            }
        }
        
        if (index == max) {
            return true;
        } else {
            return false;
        }
        
    }
    
    public static WndBase[] CloseAll()
    {
        if (m_lwnd == null) {
            return null;
        }
        
        int len = m_lwnd.Count;
        if (len == 0) {
            return null;
        }
        
        for (int i = 0; i < len; i++) {
            m_lwnd[i].gameObject.SetActive(false);
        }
        return m_lwnd.ToArray();
    }
    
    public static void ShowAll(WndBase[] wnds)
    {
        if (wnds == null) {
            return;
        }
        
        int len = wnds.Length;
        for (int i = 0; i < len; i++) {
            if (wnds[i] == null) {
                continue;
            }
            wnds[i].gameObject.SetActive(true);
        }
    }
    
    
    private static void AddWnd(WndBase Wnd)
    {
        if (Wnd == null) {
            return;
        }
        
        
        if (m_lwnd.Contains(Wnd) == false) {
            m_lwnd.Add(Wnd);
        }
    }
    
    
    public static void SetCanvasView(CanvasEnum eType, bool bShow)
    {
        g_uiNode.SetWndView(eType, bShow);
    }
}
