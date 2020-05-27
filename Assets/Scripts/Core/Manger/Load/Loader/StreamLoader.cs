#region Namespace

using System;
using UnityEngine;
using Logger = IGG.Logging.Logger;
using IGG.Core.Helper;

#endregion

namespace IGG.Game.Core.Load
{
    /// <summary>
    /// Author  xuzhihui
    /// Date    2018.06.13
    /// Desc    流加载器
    /// </summary>
    public class StreamLoader : Loader
    {
        private WWW m_request;
        
        public StreamLoader()
            : base(LoaderType.Stream)
        {
        }
        
        public override void Reset()
        {
            base.Reset();
            
            if (m_request != null) {
                m_request.Dispose();
                m_request = null;
            }
        }
        
        public override void Load()
        {
            base.Load();
            
            if (IsAsync) {
                string path = Path;
                
                bool hasHead = (bool) Param;
                if (!hasHead) {
                    bool addFileHead = true;
                    
#if UNITY_ANDROID && !UNITY_EDITOR
                    // 如果是读取apk里的资源,不需要加file:///,其它情况都要加
                    if (path.Contains(Application.streamingAssetsPath)) {
                        addFileHead = false;
                    }
#endif
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    if (addFileHead) {
                        path = string.Format("file:///{0}", path);
                    }
                }
                
                m_request = new WWW(path);
            } else {
                object data = null;
                try {
                    data = FileHelper.ReadByteFromFile(Path);
                } catch (Exception e) {
                    Logger.LogError(e.Message);
                }
                finally {
                    OnLoadCompleted(data);
                }
            }
        }
        
        /// <summary>
        /// 更新
        /// </summary>
        public override void Update()
        {
            if (State == LoaderState.Loading) {
                if (m_request == null) {
                    OnLoadCompleted(null);
                } else if (!string.IsNullOrEmpty(m_request.error)) {
                    Logger.LogError(m_request.error);
                    OnLoadCompleted(null);
                } else if (m_request.isDone) {
                    OnLoadCompleted(m_request.bytes);
                } else {
                    OnLoadProgress(m_request.progress);
                }
            }
        }
    }
}