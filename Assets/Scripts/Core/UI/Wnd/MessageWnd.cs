using IGG.Core.Data.DataCenter;
using IGG.Core;
using UnityEngine.UI;
using IGG.Core.Helper;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Author  zhulin
/// Date    2019.2.16
/// Desc    MessageWnd界面交互逻辑
/// </summary>
public class MessageWnd : WndBase
{

    /// <summary>
    /// 定义关联对象
    /// </summary>
    public GameObject hintItem1;
    public Transform parent1;
    
    
    public GameObject hintItem2;
    public Transform parent2;
    
    private List<GameObject> m_List = new List<GameObject>();
    private int m_type = 0;
    private string m_msg = "";
    // <summary>
    // 加载完成立即初始化的操作
    // </summary>
    public override void Init()
    {
        hintItem1.SetActive(false);
        hintItem2.SetActive(false);
        if (m_type == 0) {
            parent2.gameObject.SetActive(false);
        } else {
            parent1.gameObject.SetActive(false);
        }
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
        if (obj == null) {
            return;
        }
        m_msg = obj as string;
        RefreshUI();
        
    }
    
    public void SetType(int type)
    {
        m_type = type;
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
        GameObject hintItem = m_type == 0 ? hintItem1 : hintItem2;
        Transform parent = m_type == 0 ? parent1 : parent2;
        
        if (hintItem != null) {
            parent.gameObject.SetActive(true);
            GameObject go = GameObject.Instantiate(hintItem, parent);
            go.SetActive(true);
            IggText txt = go.GetComponentInChildren<IggText>();
            if (txt != null) {
                List<string> l = new List<string>();
                l.Add(m_msg);
                txt.SetString(l);
            }
            m_List.Add(go);
        }
        // 只留3个
        int count = m_List.Count;
        if (count > 3) {
            for (int i = 0; i < count - 3; i++) {
                GameObject g = m_List[0];
                if (g != null) {
                    GameObject.Destroy(g);
                }
                m_List.RemoveAt(0);
            }
        }
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
