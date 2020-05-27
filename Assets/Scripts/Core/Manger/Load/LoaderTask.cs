﻿#region Namespace

using System.Collections.Generic;

#endregion

namespace IGG.Game.Core.Load
{
    /// <summary>
    /// 加载任务
    /// </summary>
    public class LoaderTask
    {
        /// <summary>
        /// 加载队列数
        /// </summary>
        private const int g_maxGroup = 10;

        /// <summary>
        /// 回调信息列表
        /// </summary>
        private readonly List<AsyncCallbackInfo> m_asyncCallbackInfos = new List<AsyncCallbackInfo>();

        /// <summary>
        /// 加载组池
        /// </summary>
        private readonly Queue<LoaderGroup> m_groupPool = new Queue<LoaderGroup>();

        /// <summary>
        /// 进行中的加载组
        /// </summary>
        private readonly List<LoaderGroup> m_groups = new List<LoaderGroup>();

        /// <summary>
        /// 进行中的加载器列表
        /// </summary>
        private readonly Dictionary<string, LoaderData> m_loaders = new Dictionary<string, LoaderData>();

        /// <summary>
        /// 加载器池
        /// </summary>
        private readonly Dictionary<Loader.LoaderType, Queue<LoaderData>> m_pools =
            new Dictionary<Loader.LoaderType, Queue<LoaderData>>();

        /// <summary>
        /// 等待中的加载组
        /// </summary>
        private readonly Dictionary<LoadMgr.LoadPriority, Queue<LoaderGroup>> m_waitGroups =
            new Dictionary<LoadMgr.LoadPriority, Queue<LoaderGroup>>();

        public LoaderTask()
        {
            for (int i = 0; i < (int) Loader.LoaderType.Count; ++i)
            {
                m_pools.Add((Loader.LoaderType) i, new Queue<LoaderData>());
            }

            for (int i = 0; i < (int) LoadMgr.LoadPriority.Count; ++i)
            {
                m_waitGroups.Add((LoadMgr.LoadPriority) i, new Queue<LoaderGroup>());
            }
        }

        public void Clear()
        {
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            UpdateGroup();
            UpdateAsyncCallback();
        }

        /// <summary>
        /// 创建加载器
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>加载器</returns>
        private Loader CreateLoader(Loader.LoaderType type)
        {
            Loader loader = null;
            switch (type)
            {
                case Loader.LoaderType.Stream:
                    loader = new StreamLoader();
                    break;

                case Loader.LoaderType.Asset:
                    loader = new AssetLoader();
                    break;

                case Loader.LoaderType.Scene:
                    loader = new SceneLoader();
                    break;

                case Loader.LoaderType.Resource:
                    loader = new ResourceLoader();
                    break;

                case Loader.LoaderType.Bundle:
                    if (ConstantData.EnableCache)
                    {
                        loader = new CacheBundleLoader();
                    }
                    else
                    {
                        loader = new BundleLoader();
                    }

                    break;

                case Loader.LoaderType.BundleAsset:
                    loader = new BundleAssetLoader();
                    break;
            }

            return loader;
        }

        /// <summary>
        /// 归还加载器
        /// </summary>
        /// <param name="loader">加载器</param>
        public void PushLoader(Loader loader)
        {
            LoaderData ld;
            if (!m_loaders.TryGetValue(loader.Path, out ld))
            {
                return;
            }

            if (--ld.Reference > 0)
            {
                return;
            }

            m_loaders.Remove(loader.Path);

            loader.Reset();
            m_pools[loader.Type].Enqueue(ld);
        }


        /// <summary>
        /// 获得加载器
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="path">路径</param>
        /// <param name="param">附加参数</param>
        /// <param name="async">异步</param>
        /// <returns>加载器</returns>
        public Loader PopLoader(Loader.LoaderType type, string path, object param, bool async)
        {
            LoaderData ld;
            if (!m_loaders.TryGetValue(path, out ld))
            {
                if (m_pools[type].Count > 0)
                {
                    ld = m_pools[type].Dequeue();
                }

                if (ld == null)
                {
                    ld = new LoaderData
                    {
                        Reference = 0,
                        Loader = CreateLoader(type),
                        Callbacks = new List<LoadMgr.LoadedHandler>()
                    };
                }

                ld.Loader.Init(path, param, null, OnLoadCompleted, async);
                m_loaders.Add(path, ld);
            }

            if (!async)
            {
                ld.Loader.IsAsync = false;
            }

            ++ld.Reference;
            return ld.Loader;
        }

        /// <summary>
        /// 增加回调函数
        /// </summary>
        /// <param name="loader">加载器</param>
        /// <param name="onLoaded">回调</param>
        public void PushCallback(Loader loader, LoadMgr.LoadedHandler onLoaded)
        {
            if (onLoaded == null)
            {
                return;
            }

            m_loaders[loader.Path].Callbacks.Add(onLoaded);
        }

        /// <summary>
        /// 加载完成回调
        /// </summary>
        /// <param name="loader">加载器</param>
        /// <param name="data">结果</param>
        private void OnLoadCompleted(Loader loader, object data)
        {
            LoaderData ld;
            if (!m_loaders.TryGetValue(loader.Path, out ld))
            {
                return;
            }

            for (int i = 0; i < ld.Callbacks.Count; ++i)
            {
                ld.Callbacks[i](data);
            }

            ld.Callbacks.Clear();
        }

        /// <summary>
        /// 获得加载组
        /// </summary>
        /// <returns></returns>
        public LoaderGroup PopGroup(LoadMgr.LoadPriority priority = LoadMgr.LoadPriority.Normal)
        {
            LoaderGroup group = m_groupPool.Count > 0 ? m_groupPool.Dequeue() : new LoaderGroup(this);
            group.Priority = priority;
            m_waitGroups[priority].Enqueue(group);
            return group;
        }

        /// <summary>
        /// 开始下一组
        /// </summary>
        /// <returns></returns>
        private bool StartNextGroup()
        {
            LoaderGroup group = null;
            for (int i = (int) LoadMgr.LoadPriority.Count - 1; i >= 0; --i)
            {
                Queue<LoaderGroup> groups = m_waitGroups[(LoadMgr.LoadPriority) i];
                if (groups.Count == 0)
                {
                    continue;
                }

                group = groups.Dequeue();
                break;
            }

            if (group == null)
            {
                return false;
            }

            m_groups.Add(group);
            group.Start();

            return true;
        }

        /// <summary>
        /// 更新
        /// </summary>
        private void UpdateGroup()
        {
            int index = 0;
            while (index < m_groups.Count)
            {
                LoaderGroup group = m_groups[index];
                group.Update();

                if (group.IsFinish)
                {
                    group.Reset();
                    m_groupPool.Enqueue(group);
                    m_groups.RemoveAt(index);
                }
                else
                {
                    ++index;
                }
            }

            while (m_groups.Count < g_maxGroup)
            {
                if (!StartNextGroup())
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 添加加载任务
        /// </summary>
        /// <param name="group">加载组</param>
        /// <param name="type">类型</param>
        /// <param name="path">路径</param>
        /// <param name="param">附加参数</param>
        /// <param name="onLoaded">回调</param>
        /// <param name="async">异步</param>
        /// <param name="priority">优先级</param>
        /// <param name="insert">插入</param>
        /// <returns></returns>
        public void AddLoadTask(LoaderGroup group, Loader.LoaderType type, string path, object param,
                                LoadMgr.GroupLoadedCallback onLoaded, bool async,
                                LoadMgr.LoadPriority priority = LoadMgr.LoadPriority.Normal, bool insert = false)
        {
            if (!async)
            {
                Loader loader = PopLoader(type, path, param, false);
                PushCallback(loader, (data) =>
                {
                    if (onLoaded != null)
                    {
                        onLoaded(group, data);
                    }
                });
                loader.Load();

                PushLoader(loader);

                return;
            }

            if (group == null)
            {
                group = PopGroup(priority);
            }

            group.Add(type, path, param, onLoaded, true, insert);
        }

        /// <summary>
        /// 增加异步回调
        /// </summary>
        /// <param name="onLoaded">回调</param>
        /// <param name="group">加载组</param>
        /// <param name="data">资源对象</param>
        public void AddAsyncCallback(LoadMgr.GroupLoadedCallback onLoaded, LoaderGroup group, object data)
        {
            if (onLoaded == null)
            {
                return;
            }

            AsyncCallbackInfo info = new AsyncCallbackInfo
            {
                OnLoaded = onLoaded,
                Group = group,
                Data = data
            };

            m_asyncCallbackInfos.Add(info);
        }

        /// <summary>
        /// 更新异步回调
        /// </summary>
        private void UpdateAsyncCallback()
        {
            for (int i = 0; i < m_asyncCallbackInfos.Count; ++i)
            {
                AsyncCallbackInfo info = m_asyncCallbackInfos[i];
                info.OnLoaded(info.Group, info.Data);
            }

            m_asyncCallbackInfos.Clear();
        }

        /// <summary>
        /// 加载信息
        /// </summary>
        private class LoaderData
        {
            /// <summary>
            /// 回调函数列表
            /// </summary>
            public List<LoadMgr.LoadedHandler> Callbacks;

            /// <summary>
            /// 加载器
            /// </summary>
            public Loader Loader;

            /// <summary>
            /// 引用计数
            /// </summary>
            public int Reference;
        }

        /// <summary>
        /// 异步加载回调信息
        /// </summary>
        private class AsyncCallbackInfo
        {
            /// <summary>
            /// 资源对象
            /// </summary>
            public object Data;

            /// <summary>
            /// 加载组
            /// </summary>
            public LoaderGroup Group;

            /// <summary>
            /// 回调函数
            /// </summary>
            public LoadMgr.GroupLoadedCallback OnLoaded;
        }
    }
}