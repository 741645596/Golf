using System;
using System.Collections.Generic;

namespace IGG.Game.Core.Cache
{
    /// <summary>
    /// 非安全的池操作引用
    /// 主要是用来快速得到内部数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UnsafeGeneralPoolRef<T>
    {
        public GeneralPool<T> Pool;
        public Stack<T> FreeList;
        public List<T> UseList;
    }
    
    /// <summary>
    /// 一个通用的缓存项
    /// @author gaofan
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GeneralPool<T>
    {
        /// <summary>
        /// 新建对象的回调
        /// @param bool isInit: 是否是第一次调用
        /// </summary>
        private Func<Object, T> m_newItemCallback;
        
        /// <summary>
        /// 返回一个对象之前调用
        /// </summary>
        public Action<T> GetBeforCallback;
        
        /// <summary>
        /// 释放一个对象时调用
        /// </summary>
        public Action<T> FreeItemCallback;
        
        /// <summary>
        /// 最大缓存数
        /// 值为0表示无限
        /// 默认值为0
        /// </summary>
        public uint MaxCount;
        
        /// <summary>
        /// 初始化时新建立的数量
        /// 这个值如果超过MaxCount的值，会被重置会MaxCount的值
        /// </summary>
        public uint InitCount;
        
        /// <summary>
        /// 空闲的资源库
        /// </summary>
        private readonly Stack<T> m_freeLib;
        /// <summary>
        /// 已经使用的资源列表
        /// </summary>
        private readonly List<T> m_useLib;
        
        private readonly Object m_initParam = new Object();
        
        public GeneralPool(Func<Object, T> newItemCallback, uint initCount = 0, uint maxCount = 0,
            UnsafeGeneralPoolRef<T> poolRef = null)
        {
            if (newItemCallback == null) {
                throw new ArgumentNullException("newItemCallback");
            }
            
            MaxCount = maxCount;
            InitCount = initCount;
            m_newItemCallback = newItemCallback;
            
            if (maxCount > 0) {
                m_freeLib = new Stack<T>((int)maxCount);
                m_useLib = new List<T>((int)maxCount);
            } else {
                m_freeLib = new Stack<T>();
                m_useLib = new List<T>();
            }
            
            if (poolRef != null) {
                poolRef.Pool = this;
                poolRef.FreeList = m_freeLib;
                poolRef.UseList = m_useLib;
            }
        }
        
        public Object InitParam {
            get { return m_initParam; }
        }
        
        /// <summary>
        /// 预生成缓存项
        /// </summary>
        public void Init()
        {
            if (InitCount == 0) {
                return;
            }
            
            if (MaxCount != 0 && InitCount > MaxCount) {
                InitCount = MaxCount;
            }
            
            int initCount = (int) InitCount - m_freeLib.Count - m_useLib.Count;
            for (int i = 0; i < initCount; i++) {
                T obj = NewItem(true);
                m_freeLib.Push(obj);
            }
        }
        
        /// <summary>
        /// 拿到一个缓存
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
        
            T obj;
            if (m_freeLib.Count == 0) {
                if (MaxCount > 0 && m_useLib.Count >= MaxCount) {
                    return default(T);
                }
                
                obj = NewItem(false);
            } else {
                obj = m_freeLib.Pop();
            }
            
            m_useLib.Add(obj);
            GetBefor(obj);
            return obj;
        }
        
        /// <summary>
        /// 放回一个缓存
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Free(T obj)
        {
            DoFreeItemCallback(obj);
            int index = m_useLib.IndexOf(obj);
            if (index < 0) {
                return false;
            }
            
            //因为要取到最前面的对象，所以不能用这个
            //m_useLib.FastRemoveAt(index);
            m_useLib.RemoveAt(index);
            m_freeLib.Push(obj);
            return true;
        }
        
        /// <summary>
        /// 释放第一个对象
        /// 使用这个接口，一般的场景是这样
        /// 限制了对象的总数量， 在使用时，用get方法取到空的。
        /// 这时可以跟据情况，强制性释放最早的那个，并且再次调用GET拿过来用。
        /// 但要使用时要注意，拿到的这个对象之前使用的地方要释放。
        /// </summary>
        /// <returns></returns>
        public bool FreeFrist()
        {
            if (m_useLib.Count < 1) {
                return false;
            }
            
            T fristItem = m_useLib[0];
            if (fristItem == null) {
                return false;
            }
            
            return Free(fristItem);
        }
        
        /// <summary>
        /// 清空并返回所有持有的缓存对象
        /// </summary>
        /// <returns></returns>
        public T[] Clear()
        {
            int count = m_useLib.Count + m_freeLib.Count;
            if (count < 1) {
                return new T[0];
            }
            
            T[] arr = new T[count];
            int i = 0;
            for (int j = 0; j < m_useLib.Count; j++) {
                arr[i] = m_useLib[j];
                i++;
            }
            
            for (int j = 0; j < m_freeLib.Count; j++) {
                arr[i] = m_freeLib.Pop();
                i++;
            }
            m_useLib.Clear();
            m_freeLib.Clear();
            return arr;
        }
        
        /// <summary>
        /// 当池内对象不足以使用时，新建一个对象
        /// </summary>
        /// <param name="isInit">表示是否为初始化</param>
        /// <returns></returns>
        protected virtual T NewItem(bool isInit)
        {
            if (m_newItemCallback == null) {
                return default(T);
            }
            return m_newItemCallback(m_initParam);
        }
        
        /// <summary>
        /// 在返回之前，做一些初始化工作
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void GetBefor(T obj)
        {
            if (GetBeforCallback != null) {
                GetBeforCallback(obj);
            }
        }
        
        /// <summary>
        /// 释放时，重置对象工作
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void DoFreeItemCallback(T obj)
        {
            if (FreeItemCallback != null) {
                FreeItemCallback(obj);
            }
        }
    }
}
