#region Namespace

using System.Diagnostics;

#endregion

namespace IGG.Game.Core.Load
{
    /// <summary>
    /// Author  xuzhihui
    /// Date    2018.06.13
    /// Desc    加载器基类
    /// </summary>
    public class Loader
    {
        public delegate void LoadedHandle(Loader loader, object data);

        public delegate void ProgressHandle(Loader loader, float rate);

        public enum LoaderState
        {
            None,
            Loading,
            Finished,
        }

        public enum LoaderType
        {
            Stream,
            Asset,
            Resource,
            Bundle,
            Scene,
            BundleAsset,

            Count,
        }

        protected readonly Stopwatch m_watch = new Stopwatch();

        private LoadedHandle m_onLoaded;

        private ProgressHandle m_onProgress;

        protected Loader(LoaderType type)
        {
            Type = type;
        }

        public LoaderType Type { get; private set; }

        public LoaderState State { get; private set; }

        public string Path { get; private set; }

        protected object Param { get; private set; }

        public bool IsFinish
        {
            get { return State == LoaderState.Finished; }
        }

        public object Data { get; private set; }

        public bool IsAsync { get; set; }

        public void Init(string path, object param, ProgressHandle onProgress, LoadedHandle onLoaded,
                         bool async = true)
        {
            State = LoaderState.None;
            Path = path;
            Param = param;
            IsAsync = async;

            m_onProgress = onProgress;
            m_onLoaded = onLoaded;
        }

        public virtual void Load()
        {
            m_watch.Reset();
            m_watch.Start();

            State = LoaderState.Loading;
            OnLoadProgress(0f);
        }

        public virtual void Update()
        {
        }

        public virtual void Reset()
        {
            State = LoaderState.None;

            Path = "";
            Param = null;
            IsAsync = false;
            Data = null;

            m_onProgress = null;
            m_onLoaded = null;
        }

        protected void OnLoadProgress(float rate)
        {
            if (m_onProgress != null)
            {
                m_onProgress(this, rate);
            }
        }

        protected void OnLoadCompleted(object data)
        {
            State = LoaderState.Finished;
            Data = data;

            if (m_onLoaded != null)
            {
                m_onLoaded(this, data);
            }

            OnLoadProgress(1f);
        }
    }
}