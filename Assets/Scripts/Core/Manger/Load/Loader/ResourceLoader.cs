#region Namespace

using UnityEngine;

#endregion

namespace IGG.Game.Core.Load
{
    /// <summary>
    /// Author  xuzhihui
    /// Date    2018.06.13
    /// Desc    Resources文件加载器(加载Resources文件夹下的资源,使用Resources.Load)
    /// </summary>
    public class ResourceLoader : Loader
    {
        private ResourceRequest m_request;

        public ResourceLoader()
            : base(LoaderType.Resource)
        {
        }

        public override void Load()
        {
            base.Load();

            if (IsAsync)
            {
                m_request = Resources.LoadAsync(Path);
            }
            else
            {
                Object obj = Resources.Load(Path);
                OnLoadCompleted(obj);
            }
        }

        public override void Update()
        {
            if (State == LoaderState.Loading)
            {
                if (m_request == null)
                {
                    OnLoadCompleted(null);
                }
                else if (m_request.isDone)
                {
                    OnLoadCompleted(m_request.asset);
                    m_request = null;
                }
                else
                {
                    OnLoadProgress(m_request.progress);
                }
            }
        }
    }
}