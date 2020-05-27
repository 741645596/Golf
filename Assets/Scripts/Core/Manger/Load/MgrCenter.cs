#define ShowState
//#define LOG_NET_LEVEL1

#region Namespace

using System;
using System.Collections.Generic;
using IGG.Logging;

#endregion

namespace IGG.Core.Manger
{
    /// <summary>
    /// Author  gaofan
    /// Date    2017.12.11
    /// Desc    管理器中心
    /// </summary>
    public class MgrCenter : MonoSingleton<MgrCenter>
    {
        private bool m_isStart;
        private event Action EventStart;
        private List<IManager> m_mgrList;
        
        private void Awake()
        {
#if ShowState
            Logger.Log("<color=navy>MgrCenter: 启动MgrCenter</color>");
#endif
            //g_inst = this;
            DontDestroyOnLoad(this);
            Initialize();
        }
        
        private void Start()
        {
            foreach (IManager manager in m_mgrList) {
#if ShowState
                Logger.Log(string.Format("<color=navy>MgrCenter: 管理器[<color=Brown>{0}</color>]启动</color>", manager));
#endif
                manager.Initialize(this);
                manager.Update();
            }
            
            m_isStart = true;
            OnEventStart();
            EventStart = null;
        }
        
        protected virtual void Initialize()
        {
        }
        
        /// <summary>
        /// 等待些类初始化完成
        /// 如果已经完成时调用此方法，会直接执行回调
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool WaitForInit(Action callback)
        {
            if (callback == null) {
                return false;
            }
            
            if (m_isStart) {
                callback();
                return true;
            } else {
                EventStart += callback;
                return false;
            }
        }
        
        public void Add(IManager manager)
        {
            if (m_mgrList == null) {
                m_mgrList = new List<IManager>();
            }
            
            m_mgrList.Add(manager);
            
            if (m_isStart) {
#if ShowState
                Logger.Log(string.Format("<color=navy>MgrCenter: 管理器[<color=Brown>{0}</color>]启动</color>", manager));
#endif
                manager.Initialize(this);
                manager.Update();
            }
        }
        
#if LOG_NET_LEVEL1
        void FixedUpdate()
        {
            AsyncLogger.Inst.Update();
        }
#endif
        
        private void Update()
        {
            if (m_mgrList == null) {
                return;
            }
            
            int removeIndex = -1;
#if ShowState
            string removeName = "";
#endif
            int len = m_mgrList.Count;
            for (int i = 0; i < len; i++) {
                IManager manager = m_mgrList[i];
                if (manager.Disposed) {
                    removeIndex = i;
#if ShowState
                    removeName = manager.ToString();
#endif
                } else {
                    manager.Update();
                }
            }
            
            //因为是update，所以就算有多个mgr要删除，也可以在下一次的update中删除掉
            if (removeIndex > -1) {
                m_mgrList.RemoveAt(removeIndex);
#if ShowState
                Logger.Log(string.Format("<color=navy>MgrCenter: 管理器[<color=Brown>{0}</color>]移除</color>", removeName));
#endif
            }
        }
        
        private void OnDestroy()
        {
            if (m_mgrList == null) {
                Logger.Log("MgrCenter.OnDestroy: mMgrList = null");
                return;
            }
            
            for (int i = m_mgrList.Count - 1; i >= 0; i--) {
                IManager manager = m_mgrList[i];
                manager.Dispose();
#if ShowState
                Logger.Log(string.Format("<color=navy>MgrCenter: 管理器[<color=Brown>{0}</color>]移除</color>", manager));
#endif
            }
            
            m_mgrList.Clear();
            m_mgrList = null;
            
#if ShowState
            Logger.Log("<color=navy>MgrCenter: 关闭MgrCenter</color>");
#endif
        }
        
        protected virtual void OnEventStart()
        {
            var handler = EventStart;
            if (handler != null) {
                handler();
            }
        }
    }
}