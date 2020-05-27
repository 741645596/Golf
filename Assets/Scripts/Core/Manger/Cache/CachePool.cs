#region Namespace

using System;
using System.Collections;
using System.Collections.Generic;
using IGG.Logging;
using ILogger = IGG.Logging.ILogger;

#endregion

namespace IGG.Game.Core.Cache
{
    /// <summary>
    /// 需要实现子项接口的通用缓存池
    /// @author gaofan
    /// </summary>
    /// <typeparam name="TCacheItem"></typeparam>
    public class CachePool<TCacheItem> : IEnumerable, IEnumerator<CacheItemInfo<TCacheItem>>
            where TCacheItem : ICacheItem<TCacheItem>, new ()
    {
        private Func<Object, TCacheItem> m_createCallback;
        
        private uint m_genId;
        
        
        ////////////////////////////////////////////////////////////
        /// 下面是为了可以用foreach遍历已经使用的对象
        ////////////////////////////////////////////////////////////
        private int m_index;
        
        private Object m_initParam;
        private ILogger m_logger;
        private int m_maxCount;
        private string m_name;
        
        private Stack<PoolItem<TCacheItem>> m_pool;
        private CacheItemInfo<TCacheItem> m_tmpInfo;
        private List<PoolItem<TCacheItem>> m_usingList;
        
        public CachePool(int initCount = 0, int maxCount = 0, Object initParam = null, ILogger logger = null,
            Func<Object, TCacheItem> createCallback = null, string cacheName = null) {
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (string.IsNullOrEmpty(cacheName)) {
                /*
                 *  派生类成员初始化
                    基类型成员初始化
                    基类型的构造函数 -> 调用虚拟成员 -> 会调用到派生类的虚拟成员 -> 可能这个派生类虚拟成员要在派生类型的构造函数中初始化，而因为现在调用还没有始化会出错。
                    派生类型的构造函数
                 */
                // ReSharper disable once VirtualMemberCallInConstructor
                m_name = string.Concat("CachePool<", typeof(TCacheItem).Name, "-", GetHashCode(), ">");
            } else {
                m_name = string.Concat("CachePool<", typeof(TCacheItem).Name, "-", cacheName, ">");
            }
            
            if (logger == null) {
                logger = NoneLogger.Inst;
            }
            
            m_logger = logger;
            
            if (maxCount != 0) {
                if (initCount > maxCount) {
                    initCount = maxCount;
                    m_logger.LogWarning("initCount > maxCount", m_name + ".Ctor");
                }
                
                m_pool = new Stack<PoolItem<TCacheItem>>(maxCount);
                m_usingList = new List<PoolItem<TCacheItem>>(maxCount);
            } else {
                m_pool = new Stack<PoolItem<TCacheItem>>();
                m_usingList = new List<PoolItem<TCacheItem>>();
            }
            
            m_maxCount = maxCount;
            m_initParam = initParam;
            m_createCallback = createCallback;
            
            m_tmpInfo = new CacheItemInfo<TCacheItem>();
            
            for (int i = 0; i < initCount; i++) {
                PoolItem<TCacheItem> item = NewItem();
                m_pool.Push(item);
            }
        }
        
        public bool Disposed { get; private set; }
        
        public IEnumerator GetEnumerator() {
            m_index = -1;
            return this;
        }
        
        public CacheItemInfo<TCacheItem> Current {
            get
            {
                PoolItem<TCacheItem> item = m_usingList[m_index];
                m_tmpInfo.Timeout = item.Timeout;
                m_tmpInfo.Item = item.Item;
                return m_tmpInfo;
            }
        }
        
        object IEnumerator.Current {
            get { return Current; }
        }
        
        public bool MoveNext() {
            m_index++;
            return m_index < m_usingList.Count;
        }
        
        public void Reset() {
            m_index = -1;
        }
        
        public void Dispose() {
            if (Disposed) {
                return;
            }
            
            Disposed = true;
            
            //foreach结束时会调用 ，然而我这里并没有什么用
            m_index = -1;
            m_tmpInfo.Item = default(TCacheItem);
            m_tmpInfo.Timeout = -1;
            
            if (m_pool != null) {
                m_pool.Clear();
            }
            
            if (m_usingList != null) {
                m_usingList.Clear();
            }
        }
        
        /// <summary>
        /// 返回一个实例
        /// 如果有设置maxCount的话，可能会返回空
        /// 如果反回家，表示可用的对象已经被全部取完
        /// </summary>
        /// <param name="timeout">
        /// 超时时间，单位秒
        /// 如果有设置这个值，将忽略item自身的liveTime设置
        /// </param>
        /// <returns></returns>
        public TCacheItem Get(float timeout = -1) {
            PoolItem<TCacheItem> item;
            if (m_pool.Count == 0) {
                if (m_maxCount > 0 && m_usingList.Count >= m_maxCount) {
                    return default(TCacheItem);
                }
                
                item = NewItem();
            } else {
                item = m_pool.Pop();
            }
            
            if (timeout > 0) {
                item.Timeout = timeout + UnityEngine.Time.realtimeSinceStartup;
            } else {
                item.Timeout = item.Item.LiveTime + UnityEngine.Time.realtimeSinceStartup;
            }
            
            AddToUsingList(item);
            return item.Item;
        }
        
        /// <summary>
        /// 新建一个对象
        /// </summary>
        /// <returns></returns>
        private PoolItem<TCacheItem> NewItem() {
            PoolItem<TCacheItem> pi = new PoolItem<TCacheItem>();
            pi.Id = m_genId++;
            pi.Index = -1;
            pi.Item = m_createCallback != null ? m_createCallback(m_initParam) : new TCacheItem();
            pi.Item.Initialize(() => {
                if (Disposed) {
                    /*object cacheItem = pi.Item;
                    IDisposer item = cacheItem as IDisposer;
                    if (item != null) {
                        item.Dispose();
                    }*/
                    
                    return;
                }
                
                //如果值为-1, 说明已经回收过了
                if (pi.Index == -1) {
                    m_logger.LogError("重复回收了项:cache = " + m_name + ", id = " + pi.Id,
                        "CahcePool.NewItem.(pi.item.Initialize)");
                    return;
                }
                
                RemoveFormUsingList(pi);
                pi.Index = -1;
                m_pool.Push(pi);
            }, m_initParam);
            return pi;
        }
        
        /// <summary>
        /// 增加到使用列表
        /// </summary>
        /// <param name="item"></param>
        private void AddToUsingList(PoolItem<TCacheItem> item) {
            if (item.Index != -1) {
                m_logger.LogError("item.Index != -1", m_name + ".AddToUsingList");
            }
            
            item.Index = m_usingList.Count;
            m_usingList.Add(item);
        }
        
        /// <summary>
        /// 从使用列表移除
        /// </summary>
        /// <param name="item"></param>
        private void RemoveFormUsingList(PoolItem<TCacheItem> item) {
            int errorCode = -1;
            
            if (item.Index >= m_usingList.Count) {
                m_logger.LogError("item.Index >= mUsingList.Count", m_name + ".RemoveFormUsingList");
                errorCode = 1;
            }
            
            if (item.Index < 0) {
                m_logger.LogError("item.Index < 0", m_name + ".RemoveFormUsingList");
                errorCode = 2;
            }
            
            PoolItem<TCacheItem> itemEx = errorCode > 0 ? null : m_usingList[item.Index];
            if (itemEx != item) {
                m_logger.LogError("itemEx != item", m_name + ".RemoveFormUsingList");
                errorCode = 3;
            }
            
            if (errorCode > 0) {
                bool success = m_usingList.Remove(item);
                if (!success) {
                    m_logger.LogError("remove error", m_name + ".RemoveFormUsingList");
                } else {
                    item.Index = -1;
                    m_logger.LogInfo("remove success", m_name + ".RemoveFormUsingList");
                }
                
                FixUsingList();
                return;
            }
            
            //下面是处理正常的节点，上面都是为了以防万一
            //下面这种删除方式会快一些，不用全部排序
            int removeIndex = m_usingList.Count - 1;
            PoolItem<TCacheItem> lastItem = m_usingList[removeIndex];
            m_usingList.RemoveAt(removeIndex);
            
            if (lastItem != item) {
                lastItem.Index = item.Index;
                m_usingList[item.Index] = lastItem;
                item.Index = -1;
            }
        }
        
        /// <summary>
        /// 修正使用列表
        /// </summary>
        private void FixUsingList() {
            string funName = m_name + ".FixUsingList";
            m_logger.LogInfo("start", funName);
            int fixNum = 0;
            for (int i = 0; i < m_usingList.Count; i++) {
                PoolItem<TCacheItem> item = m_usingList[i];
                if (item.Index != i) {
                    m_logger.LogInfo(string.Concat("find error: index=", item.Index, " => ", i), funName);
                    item.Index = i;
                    fixNum++;
                }
            }
            
            m_logger.LogInfo(string.Concat("fix end，find ", fixNum, " error"), funName);
        }
        
        /// <summary>
        /// 内部包装的对象，主要为了在不暴露接口的情况下，实现快速删除
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        private class PoolItem<TItem>
        {
            public uint Id;
            public int Index;
            public TItem Item;
            public float Timeout;
        }
    }
    
    /// <summary>
    /// 专门用于遍历时，返回外部timeout
    /// @author fanflash.org
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CacheItemInfo<T>
    {
        public T Item;
        public float Timeout;
    }
}