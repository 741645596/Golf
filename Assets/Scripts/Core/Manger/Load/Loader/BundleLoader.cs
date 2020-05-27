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
    /// Desc    AssetBundle加载器
    /// </summary>
    public class BundleLoader : Loader
    {
        private AssetBundleCreateRequest m_abRequest;
        
        private bool m_needUnpack;
        private int m_stageCount;
        private int m_stageCurrent;
        private LzmaCompressRequest m_unpackRequest;
        
        public BundleLoader()
            : base(LoaderType.Bundle)
        {
            m_stageCurrent = 0;
            m_stageCount = 1;
        }
        
        public override void Reset()
        {
            base.Reset();
            
            m_abRequest = null;
            m_unpackRequest = null;
            
            m_stageCurrent = 0;
            m_stageCount = 1;
        }
        
        public override void Load()
        {
            base.Load();
            
            string path = LoadMgr.Inst.GetResourcePath(Path);
            if (string.IsNullOrEmpty(path)) {
                OnLoaded(null);
                return;
            }
            
            m_needUnpack = ConstantData.EnableCustomCompress && path.Contains(ConstantData.StreamingAssetsPath);
            
            if (IsAsync) {
                if (m_needUnpack) {
                    m_stageCount = 2;
                    
                    byte[] bytes = FileHelper.ReadByteFromFile(path);
                    m_unpackRequest = LzmaCompressRequest.CreateDecompress(bytes);
                } else {
                    m_abRequest = AssetBundle.LoadFromFileAsync(path);
                }
            } else {
                AssetBundle ab = null;
                try {
                    if (m_needUnpack) {
                        byte[] bytes = FileHelper.ReadByteFromFile(path);
                        bytes = Unpack(bytes);
                        ab = AssetBundle.LoadFromMemory(bytes);
                    } else {
                        ab = AssetBundle.LoadFromFile(path);
                    }
                } catch (Exception e) {
                    Logger.LogError(e.Message);
                }
                finally {
                    OnLoaded(ab);
                }
            }
        }
        
        private byte[] Unpack(byte[] bytes)
        {
            byte[] result = new byte[1];
            /*int size = LzmaHelper.Uncompress(bytes, ref result);
            if (size == 0)
            {
                Logger.LogError("Uncompress Failed");
                return null;
            }*/
            
            return result;
        }
        
        public override void Update()
        {
            if (State == LoaderState.Loading) {
                if (m_abRequest != null) {
                    if (m_abRequest.isDone) {
                        ++m_stageCurrent;
                        OnLoaded(m_abRequest.assetBundle);
                    } else {
                        DoProgress(m_abRequest.progress);
                    }
                } else if (m_unpackRequest != null) {
                    if (m_unpackRequest.IsDone) {
                        ++m_stageCurrent;
                        m_abRequest = AssetBundle.LoadFromMemoryAsync(m_unpackRequest.Bytes);
                        
                        m_unpackRequest.Dispose();
                        m_unpackRequest = null;
                    } else {
                        DoProgress(m_unpackRequest.Progress);
                    }
                }
            }
        }
        
        private void DoProgress(float rate)
        {
            OnLoadProgress((m_stageCurrent + rate) / m_stageCount);
        }
        
        private void OnLoaded(AssetBundle ab)
        {
            //Logger.Log(string.Format("BundlLoader {0} - {1} use {2}ms", Path, IsAsync, m_watch.ElapsedMilliseconds));
            OnLoadCompleted(ab);
        }
    }
}