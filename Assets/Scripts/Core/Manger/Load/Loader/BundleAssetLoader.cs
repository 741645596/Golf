#region Namespace

using System;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace IGG.Game.Core.Load
{
    public class BundleAssetLoadParam
    {
        public AssetBundle Bundle;
        public Type Type;
    }

    /// <summary>
    /// Author  xuzhihui
    /// Date    2018.06.13
    /// Desc    AssetBundle加载器
    /// </summary>
    public class BundleAssetLoader : Loader
    {
        private AssetBundleRequest m_request;

        public BundleAssetLoader()
            : base(LoaderType.BundleAsset)
        {
        }

        public override void Reset()
        {
            base.Reset();

            m_request = null;
        }

        public override void Load()
        {
            base.Load();

            BundleAssetLoadParam param = Param as BundleAssetLoadParam;
            if (param == null || param.Bundle == null || param.Type == null || string.IsNullOrEmpty(Path))
            {
                OnLoaded(null);
                return;
            }

            if (IsAsync)
            {
                m_request = param.Bundle.LoadAssetAsync(Path, param.Type);
            }
            else
            {
                Object asset = param.Bundle.LoadAsset(Path, param.Type);
                OnLoaded(asset);
            }
        }

        public override void Update()
        {
            if (State != LoaderState.Loading)
            {
                return;
            }

            if (m_request.isDone)
            {
                OnLoaded(m_request.asset);
            }
            else
            {
                OnLoadProgress(m_request.progress);
            }
        }

        private void OnLoaded(Object asset)
        {
            //Logger.Log(string.Format("BundleAssetLoader {0} - {1} use {2}ms", m_path, m_async, m_watch.ElapsedMilliseconds));

            OnLoadCompleted(asset);
        }
    }
}