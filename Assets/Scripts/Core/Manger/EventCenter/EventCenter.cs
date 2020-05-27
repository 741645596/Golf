using System;
using System.Collections.Generic;

// <summary>
// 客户端事件中心。
// </summary>
// <author>zhulin</author>

namespace IGG.Core
{
    /// <summary>
    /// 触发服务端协议事件
    /// </summary>
    /// <param name="Type">proto协议枚举</param>
    /// <param name="Info">proto协议协议对象</param>
	public delegate void DataHook(eMsgTypes Type, object Info);
    /// <summary>
    /// 通知事件
    /// </summary>
    /// <param name="Event_Send">对象发送对象标识</param>
    /// <param name="Param">核心参数</param>
    /// <param name="list">参数列表</param>
    public delegate void EventHook(int Event_Send, object Param);
    // 事件类象枚举
    public partial class EventCenterType
    {

    };


    /// <summary>
    /// 客户端事件中心
    /// </summary>
    public partial class EventCenter
    {
        private static Dictionary<string, List<EventHook>> m_EventHook = new Dictionary<string, List<EventHook>>();
        /// <summary>
        /// 派发事件
        /// </summary>
        /// <param name="EvtType">事件类型枚举</param>
        /// <param name="EvtSender">发送者身份标识，不想让接收事件方知道就用 -1</param>
        /// <param name="EvtParam">事件消息内容</param>
        public static void DispatchEvent(string EvtType, int EvtSender, object EvtParam)
        {
            CheckValid();
            if (m_EventHook.ContainsKey(EvtType))
            {
                List<EventHook> l = m_EventHook[EvtType];
                if (l == null || l.Count == 0)
                {
                    return;
                }

                for (int i = l.Count - 1; i >= 0; i--)
                {
                    EventHook f = l[i];
                    if (f != null)
                    {
                        f(EvtSender, EvtParam);
                    }
                }
            }
        }
        /// <summary>
        /// 注册关注事件
        /// </summary>
        public static void RegisterHooks(string evt, EventHook evf)
        {
            if (evf == null) return;
            CheckValid();
            if (m_EventHook.ContainsKey(evt))
            {
                List<EventHook> l = m_EventHook[evt];
                l.Add(evf);
            }
            else
            {
                List<EventHook> l = new List<EventHook>();
                l.Add(evf);
                m_EventHook.Add(evt, l);
            }
        }
        /// <summary>
        /// 反注册关注事件
        /// </summary>
        public static void AntiRegisterHooks(string evt, EventHook evf)
        {
            if (evf == null) return;
            CheckValid();
            if (m_EventHook.ContainsKey(evt))
            {
                List<EventHook> l = m_EventHook[evt];
                if (l == null || l.Count == 0)
                {
                    m_EventHook.Remove(evt);
                    return;
                }
                if (l.Contains(evf))
                {
                    l.Remove(evf);
                }
                if (l.Count == 0)
                {
                    m_EventHook.Remove(evt);
                }
            }
        }

        /// <summary>
        /// 反注册事件
        /// </summary>
        public static void AntiAllRegisterHooks()
        {
            m_EventHook.Clear();
        }

        private static void CheckValid()
        {
            if (m_EventHook == null)
            {
                m_EventHook = new Dictionary<string, List<EventHook>>();
            }
        }

        /// <summary>
        /// 事件中心初始化
        /// </summary>
        public static void Init()
        {
            if (m_EventHook == null)
            {
                m_EventHook = new Dictionary<string, List<EventHook>>();
            }

            Clear();
        }
        /// <summary>
        /// 清理事件中心。
        /// </summary>
        public static void Clear()
        {
            if (m_EventHook != null)
            {
                Dictionary<string, List<EventHook>>.Enumerator iter = m_EventHook.GetEnumerator();
                while (iter.MoveNext())
                {
                    iter.Current.Value.Clear();
                }
                iter.Dispose();
                m_EventHook.Clear();
            }
        }


        /// <summary>
        /// 释放客户端事件管理数据
        /// </summary>
        public static void Free()
        {
            Clear();
            m_EventHook = null;
        }
    }
}
