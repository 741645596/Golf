#region Namespace

using System.IO;
using UnityEngine;
using IGG.Core.Helper;

#endregion

namespace IGG.Game.Core.Load
{
    /// <summary>
    /// AssetBundle加载器
    /// </summary>
    public class CacheBundleLoader : Loader
    {
        /// <summary>
        /// AssetBundle加载请求
        /// </summary>
        private WWW m_request;
        
        /// <summary>
        /// 构造
        /// </summary>
        public CacheBundleLoader()
            : base(LoaderType.Bundle)
        {
        }
        
        /// <summary>
        /// 重置
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            
            m_request = null;
        }
        
        /// <summary>
        /// 开始加载
        /// </summary>
        public override void Load()
        {
            base.Load();
            
            if (m_request != null) {
                m_request.Dispose();
                m_request = null;
            }
            
            string path = LoadMgr.Inst.GetResourcePath(Path, IsAsync);
            if (IsAsync) {
                string pathDst = FileHelper.GetAssetBundleFullPath(path);
                m_request = WWW.LoadFromCacheOrDownload(pathDst, FileHelper.DefaultHash);
            } else {
                string filename = System.IO.Path.GetFileNameWithoutExtension(path);
                string pathDst = FileHelper.GetCacheAssetBundlePath(filename);
                if (!File.Exists(pathDst)) {
                    // 不存在,读取源文本
                    pathDst = path;
                    
                    // 加到后台解压
                    path = FileHelper.GetAssetBundleFullPath(path);
                    LoadMgr.Inst.AddToDownloadOrCache(path);
                }
                
                AssetBundle ab = AssetBundle.LoadFromFile(pathDst);
                OnLoaded(ab);
            }
        }
        
        /// <summary>
        /// 更新
        /// </summary>
        public override void Update()
        {
            if (State == LoaderState.Loading) {
                if (m_request != null) {
                    if (m_request.isDone) {
                        OnLoaded(m_request.assetBundle);
                    } else {
                        OnLoadProgress(m_request.progress);
                    }
                }
            }
        }
        
        /// <summary>
        /// 加载完成
        /// </summary>
        /// <param name="ab"></param>
        private void OnLoaded(AssetBundle ab)
        {
            //Logger.Log(string.Format("NewBundlLoader {0} - {1} use {2}ms", Path, IsAsync, m_watch.ElapsedMilliseconds));
            OnLoadCompleted(ab);
        }
    }
}