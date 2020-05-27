#region Namespace

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IGG.Core.Manger;
//using SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = IGG.Logging.Logger;
using Object = UnityEngine.Object;
using IGG.Core.Helper;
using IGG.Core;


#endregion

namespace IGG.Game.Core.Load
{
    /// <summary>
    /// Author  xuzhihui
    /// Date    2018.06.13
    /// Desc    加载管理器
    /// </summary>
    public class LoadMgr : Singleton<LoadMgr>, IManager
    {
        public delegate void GroupLoadedCallback(LoaderGroup group, object data);
        
        public delegate void LoadedHandler(object data);
        
        public delegate void ProgressHandler(float rate);
        
        // 加载优先级
        public enum LoadPriority {
            Normal,
            High,
            
            Count,
        }
        
        private readonly AssetBundleCache m_cache;
        private readonly DownloadOrCache m_downloadOrCache;
        private readonly Dictionary<string, string> m_origns = new Dictionary<string, string>();
        private readonly Dictionary<string, string> m_patchs = new Dictionary<string, string>();
        
        private readonly List<string> m_searchPaths = new List<string>();
        
        // ------------------------------------------------------------------------------------------
        // 加载任务
        private readonly LoaderTask m_task;
        
        // ------------------------------------------------------------------------------------------
        private bool m_hasWarm;
        
        private AssetBundleManifest m_manifest;
        private AssetBundleMapping m_mapping;
        
        // ------------------------------------------------------------------------------------------
        private DownloadOrCache m_unpacker;
        
        private string m_urlPatch;
        
        public LoadMgr()
        {
            if (ConstantData.EnableCache) {
                Caching.compressionEnabled = false;
                
                string path = ConstantData.UnpackPath;
                FileHelper.CreateFileDirectory(path);
                
                UnityEngine.Cache cache = Caching.GetCacheByPath(path);
                if (!cache.valid) {
                    cache = Caching.AddCache(path);
                }
                
                Caching.currentCacheForWriting = cache;
            } else {
                if (ConstantData.EnablePatch) {
                    AddSearchPath(ConstantData.PatchPath);
                }
                
                if (ConstantData.EnableCustomCompress) {
                    AddSearchPath(ConstantData.UnpackPath);
                }
            }
            
            m_task = new LoaderTask();
            m_cache = new AssetBundleCache(m_task);
            m_downloadOrCache = new DownloadOrCache();
            
            Clear();
            
            if (ConstantData.EnableAssetBundle) {
                //LoadVersion();
            }
        }
        
        public bool Enabled { get; set; }
        
        public void Initialize(MonoBehaviour mb)
        {
        }
        
        public void Update()
        {
            m_task.Update();
            m_cache.Update();
            m_downloadOrCache.Update();
            UpdateUnpacker();
        }
        
        public void Clear(bool force = false)
        {
            m_task.Clear();
            m_cache.Clear(!force, false, force);
            
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
        
        private void AddSearchPath(string path)
        {
            m_searchPaths.Add(path);
        }
        
        private string SearchPath(string subpath, bool noStreamAssetPath = false, bool needSuffix = false,
            bool isAssetBundle = true)
        {
            if (needSuffix) {
                subpath = string.Format("{0}{1}", subpath, ConstantData.AssetBundleExt);
            }
            
            // 优先从查找目录找
            for (int i = 0; i < m_searchPaths.Count; ++i) {
                string fullpath = string.Format("{0}/{1}", m_searchPaths[i], subpath);
                if (File.Exists(fullpath)) {
                    return fullpath;
                }
            }
            
            // 不查找StreamAsset目录
            if (noStreamAssetPath) {
                return "";
            }
            
            if (isAssetBundle) {
                return string.Format("{0}/{1}", ConstantData.StreamingAssetsPath, subpath);
            } else {
                return string.Format("{0}/{1}", Application.streamingAssetsPath, subpath);
            }
        }
        
        public string GetResourcePath(string name, bool async = false)
        {
            if (ConstantData.EnableCache) {
                string md5;
                if (m_patchs.TryGetValue(name, out md5)) {
                    if (async) {
                        // 异步加载,返回远程路径
                        return string.Format("{0}/{1}{2}", m_urlPatch, md5, ConstantData.AssetBundleExt);
                    } else {
                        // 同步加载,返回缓存路径
                        string path = FileHelper.GetCacheAssetBundlePath(md5);
                        if (File.Exists(path)) {
                            return path;
                        }
                    }
                }
                
                if (m_origns.TryGetValue(name, out md5)) {
                    return string.Format("{0}/{1}{2}", ConstantData.StreamingAssetsPath, md5,
                            ConstantData.AssetBundleExt);
                }
                
                Logger.LogError(string.Format("Get MD5 failed: {0}", name));
                return "";
            } else {
                string path = name;
                if (ConstantData.EnableMd5Name) {
                    string md5;
                    if (m_patchs.TryGetValue(name, out md5)) {
                        path = SearchPath(md5, true, true);
                        if (!string.IsNullOrEmpty(path)) {
                            return path;
                        }
                    }
                    
                    if (!m_origns.TryGetValue(name, out md5)) {
                        Logger.LogError(string.Format("Get MD5 failed: {0}", name));
                        return "";
                    }
                    
                    path = md5;
                }
                
                return SearchPath(path, false, true);
            }
        }
        
        /*private void InitVersionData(bool start = true)
        {
            Clear();
        
            m_origns.Clear();
            for (int i = 0; i < VersionData.Inst.Items.Length; ++i)
            {
                VersionData.VersionItem item = VersionData.Inst.Items[i];
                m_origns.Add(item.Name, item.Md5);
            }
        
            if (start)
            {
                LoadManifest();
            }
        }*/
        
        /*public void SetPatchData(JSONClass list, bool clear = false)
        {
            if (clear)
            {
                Clear();
            }
        
            m_patchs.Clear();
            if (list != null)
            {
                foreach (KeyValuePair<string, JSONNode> item in list)
                {
                    m_patchs.Add(item.Key, item.Value["md5"]);
                }
            }
        
            LoadManifest();
        }*/
        
        /*private void LoadVersion()
        {
            Logger.Log("LoadVersion");
            if (ConstantData.EnableMd5Name)
            {
                string pathPatch = string.Format("{0}/version_patch", Application.persistentDataPath);
                bool hasPatch = false;
        
                if (ConstantData.EnablePatch)
                {
                    hasPatch = File.Exists(pathPatch);
                }
        
                InitVersionData(!hasPatch);
        
                if (hasPatch)
                {
                    LoadStream(pathPatch, (data) =>
                    {
                        byte[] bytes = data as byte[];
                        if (bytes == null)
                        {
                            Logger.LogError("Load patch version Failed!");
                            return;
                        }
        
                        string text = Encoding.UTF8.GetString(bytes);
                        JSONClass json = JSONNode.Parse(text) as JSONClass;
                        if (json == null)
                        {
                            Logger.LogError("Load patch version Failed!");
                            return;
                        }
        
                        JSONClass jsonList = null;
                        if (string.Equals(json["version"], LogicConstantData.Version))
                        {
                            jsonList = json["list"] as JSONClass;
                        }
        
                        m_urlPatch = json["url"];
                        SetPatchData(jsonList);
                    }, false, false, true);
                }
            }
            else
            {
                LoadManifest();
            }
        }*/
        
        // 加载资源清单
        private void LoadManifest()
        {
            Logger.Log("LoadManifest");
            
            if (m_manifest != null) {
                Object.DestroyImmediate(m_manifest, true);
                m_manifest = null;
            }
            
            if (m_mapping != null) {
                Object.DestroyImmediate(m_mapping, true);
                m_mapping = null;
            }
            
            // 加载AssetBundle依赖文件
            LoadAssetBundle(null, ConstantData.AssetbundleManifest, (group, data) => {
                AssetBundleInfo ab = data as AssetBundleInfo;
                if (ab != null) {
                    m_manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                }
            }, false, false, false);
            
            // 加载资源到AssetBundle映射表
            string name = ConstantData.AssetbundleMapping;
            LoadAssetFromBundle(null, name, name, typeof(AssetBundleMapping), (group, data) => {
                m_mapping = data as AssetBundleMapping;
                if (m_mapping != null) {
                    m_mapping.Init();
                }
            }, false);
            
            Logger.Log("LoadManifest End");
        }
        
        public bool HasBundle(string path)
        {
            path = path.ToLower();
            return m_patchs.ContainsKey(path) || m_origns.ContainsKey(path);
        }
        
        public void WarmUpShader()
        {
            if (!ConstantData.EnableAssetBundle) {
                return;
            }
            
            if (m_hasWarm) {
                return;
            }
            
            m_hasWarm = true;
            
            LoadBundle("shader", (data) => {
                AssetBundle ab = data as AssetBundle;
                if (ab == null) {
                    return;
                }
                
                ShaderVariantCollection variant = ab.LoadAsset<ShaderVariantCollection>("warmshader");
                if (variant != null) {
                    variant.WarmUp();
                }
            });
        }
        
        private void RefreshShader(AssetBundle ab)
        {
            if (ab.isStreamedSceneAssetBundle) {
                return;
            }
            
            Material[] materials = ab.LoadAllAssets<Material>();
            for (int i = 0; i < materials.Length; ++i) {
                Material mat = materials[i];
                string shaderName = mat.shader.name;
                Shader newShader = Shader.Find(shaderName);
                if (newShader != null) {
                    mat.shader = newShader;
                }
            }
        }
        
        // ------------------------------------------------------------------------------------------
        // 编辑器专用加载
        // 加载Assets目录下的文件(编辑器专用,带后缀)
        public void LoadFile(string path, LoadedHandler onLoaded,
            bool async = true, bool inData = true, LoadPriority priority = LoadPriority.Normal)
        {
#if !UNITY_EDITOR && !UNITY_STANDALONE
            Logger.LogError("LoadFile为编辑器专用方法!");
            return;
#endif
            
            string fullpath = string.Format("{0}/{1}", inData ? ConstantData.DataFullPath : Application.dataPath, path);
            if (!CheckFileExist(fullpath, onLoaded)) {
                return;
            }
            
            m_task.AddLoadTask(null, Loader.LoaderType.Stream, fullpath, false, (group, data) => {
                if (onLoaded != null) {
                    onLoaded(data);
                }
            }, async, priority);
        }
        
        // 加载资源(Assets目录下,带后缀)
        public void LoadAssetFile(string path, LoadedHandler onLoaded, Type type = null,
            bool async = true, bool inData = true, LoadPriority priority = LoadPriority.Normal)
        {
#if !UNITY_EDITOR && !UNITY_STANDALONE
            Logger.LogError("LoadAssetFile为编辑器专用方法!");
            return;
#endif
            
            string fullpath = string.Format("{0}/{1}", inData ? ConstantData.DataFullPath : Application.dataPath, path);
            if (!CheckFileExist(fullpath, onLoaded)) {
                return;
            }
            
            fullpath = inData
                ? string.Format("Assets/{0}/{1}", ConstantData.DataPath, path)
                : string.Format("Assets/{0}", path);
            m_task.AddLoadTask(null, Loader.LoaderType.Asset, fullpath, type, (group, data) => {
                if (onLoaded != null) {
                    onLoaded(data);
                }
            }, async, priority);
        }
        
        // 全平台加载
        // 加载流文件(remote==false时先从persistentData读,没有找到则从streamingAssets读,带后缀)
        public void LoadStream(string path, LoadedHandler onLoaded,
            bool async = true, bool remote = false, bool isFullPath = false,
            LoadPriority priority = LoadPriority.Normal)
        {
            string fullpath = path;
            if (!remote) {
                if (!isFullPath) {
                    fullpath = SearchPath(path, false, false, false);
                }
            } else {
                // 从服务器加载,一定是异步的
                async = true;
            }
            
            m_task.AddLoadTask(null, Loader.LoaderType.Stream, fullpath, remote, (group, data) => {
                if (onLoaded != null) {
                    onLoaded(data);
                }
            }, async, priority);
        }
        
        // 加载资源(Resource目录下,不带后缀)
        public void LoadResource(string path, LoadedHandler onLoaded,
            bool async = true, LoadPriority priority = LoadPriority.Normal)
        {
            m_task.AddLoadTask(null, Loader.LoaderType.Resource, path, null, (group, data) => {
                if (onLoaded != null) {
                    onLoaded(data);
                }
            }, async, priority);
        }
        
        // 加载关卡
        public void LoadScene(string name, LoadedHandler onLoaded, bool async = true, bool additive = false)
        {
            LoaderGroup group = m_task.PopGroup(LoadPriority.High);
            if (!additive) {
                m_task.AddLoadTask(group, Loader.LoaderType.Scene, "Empty", null, null, async);
            }
            
            if (ConstantData.EnableAssetBundle) {
                string path = string.Format("Scene/{0}", name).ToLower();
                
                LoadAssetBundle(group, path, (group1, data1) => {
                    m_task.AddLoadTask(group1, Loader.LoaderType.Scene, name, additive, (group2, data2) => {
                        if (onLoaded != null) {
                            onLoaded(data2);
                        }
                    }, async, group.Priority, true);
                }, async);
            } else {
                m_task.AddLoadTask(group, Loader.LoaderType.Scene, name, additive, (group1, data1) => {
                    if (onLoaded != null) {
                        onLoaded(data1);
                    }
                }, async);
            }
        }
        
        public void UnloadScene(string name, bool additive = false)
        {
            if (additive) {
                SceneManager.UnloadSceneAsync(name);
            }
            
            if (ConstantData.EnableAssetBundle) {
                string path = string.Format("Scene/{0}", name).ToLower();
                UnloadBundle(path);
            }
        }
        
        // 加载AssetBundle(先从persistentData读,没有找到则从streamingAssets读)
        public void LoadBundle(string path, LoadedHandler onLoaded,
            bool async = true, bool persistent = false, bool manifest = true,
            LoadPriority priority = LoadPriority.Normal)
        {
            path = path.ToLower();
            LoadAssetBundle(null, path, (group, data) => {
                AssetBundle ab = null;
                
                AssetBundleInfo info = data as AssetBundleInfo;
                if (info != null) {
                    ab = info.Bundle;
                }
                
                if (onLoaded != null) {
                    onLoaded(ab);
                }
            }, async, persistent, manifest, priority);
        }
        
        // 加载AssetBundle(先从persistentData读, 没有找到则从streamingAssets读)
        public AssetBundle LoadBundle(string path, bool persistent = false, bool manifest = true)
        {
            AssetBundle ab = null;
            LoadBundle(path, (data) => {
                ab = data as AssetBundle;
            }, false, persistent, manifest);
            
            return ab;
        }
        
        // 卸载AssetBundle
        public void UnloadBundle(string path, bool immediate = false, bool onlyRemove = false)
        {
            path = path.ToLower();
            UnloadAssetBundle(path, immediate, onlyRemove);
        }
        
        // 从AssetBundle中加载资源
        public void LoadAssetFromBundle(LoaderGroup group, string path, string name, Type type,
            GroupLoadedCallback onLoaded,
            bool async = true, bool persistent = false,
            LoadPriority priority = LoadPriority.Normal)
        {
            path = path.ToLower();
            LoadAssetBundle(group, path, (group1, data) => {
                AssetBundleInfo cache = data as AssetBundleInfo;
                LoadBundleAsset(group1, cache, name, type, onLoaded, async);
            }, async, persistent, true, priority);
        }
        
        // 加载资源
        public void LoadAsset(string path, Type type, LoadedHandler onLoaded,
            bool async = true, bool persistent = false, bool inData = true,
            LoadPriority priority = LoadPriority.Normal)
        {
            //Logger.Log(string.Format("LoadAsset: {0} - {1}", path, async));
            if (ConstantData.EnableAssetBundle) {
                string abName = m_mapping.GetAssetBundleNameFromAssetPath(path);
                if (string.IsNullOrEmpty(abName)) {
                    Logger.LogError(string.Format("找不到资源所对应的ab文件:{0}", path));
                    if (onLoaded != null) {
                        onLoaded(null);
                    }
                } else {
                    string assetName = Path.GetFileName(path);
                    LoadAssetFromBundle(null, abName, assetName, type, (group, data) => {
                        if (onLoaded != null) {
                            onLoaded(data);
                        }
                    }, async, persistent, priority);
                }
            } else {
                LoadAssetFile(path, onLoaded, type, async, inData, priority);
            }
        }
        
        public object LoadAsset(string path, Type type, bool persistent = false, bool inData = true)
        {
            object asset = null;
            LoadAsset(path, type, (data) => {
                asset = data;
            }, false, persistent, inData);
            
            return asset;
        }
        
        public void UnloadAsset(string path)
        {
            if (ConstantData.EnableAssetBundle) {
                string abName = m_mapping.GetAssetBundleNameFromAssetPath(path);
                
                if (!string.IsNullOrEmpty(abName)) {
                    UnloadBundle(abName);
                }
            }
        }
        
        // ------------------------------------------------------------------------------------------
        // 从AssetBundle里加载资源
        private void LoadBundleAsset(LoaderGroup group, AssetBundleInfo info, string name, Type type,
            GroupLoadedCallback onLoaded,
            bool async = true, LoadPriority priority = LoadPriority.Normal)
        {
            if (info == null || string.IsNullOrEmpty(name)) {
                if (onLoaded != null) {
                    onLoaded(group, null);
                }
                
                return;
            }
            
            if (!async) {
                // 同步,直接加载
                //Logger.Log(string.Format("-->LoadBundleAsset: {0}", name));
                
                Object asset = info.LoadAsset(name, type);
                if (onLoaded != null) {
                    onLoaded(group, asset);
                }
            } else {
                // 异步,创建一个加载器后加载
                BundleAssetLoadParam param = new BundleAssetLoadParam {
                    Bundle = info.Bundle,
                    Type = type
                };
                
                m_task.AddLoadTask(group, Loader.LoaderType.BundleAsset, name, param, (group1, data1) => {
                    // 加载回调
                    if (onLoaded != null) {
                        onLoaded(group1, data1);
                    }
                }, true, priority, true);
            }
        }
        
        // 加载AssetBundle(先从persistentData读,没有找到则从streamingAssets读,带后缀)
        private void LoadAssetBundle(LoaderGroup group, string path, GroupLoadedCallback onLoaded,
            bool async = true, bool persistent = false, bool manifest = true,
            LoadPriority priority = LoadPriority.Normal)
        {
            path = path.ToLower();
            
            if (async && group == null) {
                group = m_task.PopGroup(priority);
            }
            
            if (manifest) {
                if (!HasBundle(path)) {
                    // Manifest里没有这个AssetBundle,说明是一个错误的路径
                    Logger.LogError(string.Format("ab不存在:{0}", path));
                    if (onLoaded != null) {
                        if (!async) {
                            onLoaded(group, null);
                        } else {
                            m_task.AddAsyncCallback(onLoaded, group, null);
                        }
                    }
                    
                    return;
                }
                
                // 加载依赖
                LoadDependencies(group, path, async, persistent);
            }
            
            // 检查是否有缓存
            if (m_cache.CheckAssetBundleInfo(group, path, onLoaded, persistent, async)) {
                return;
            }
            
            // 添加加载任务
            m_task.AddLoadTask(group, Loader.LoaderType.Bundle, path, null, (group1, data) => {
                AssetBundle ab = data as AssetBundle;
                AssetBundleInfo info = null;
                
                if (ab != null) {
                    info = m_cache.SetAssetBundle(path, ab);
#if UNITY_EDITOR
                    RefreshShader(ab);
#endif
                }
                
                // 加载回调
                if (onLoaded != null) {
                    onLoaded(group1, info);
                }
            }, async, priority);
        }
        
        // 卸载AssetBundle
        private void UnloadAssetBundle(string path, bool immediate = false, bool onlyRemove = false)
        {
            path = path.ToLower();
            
            if (m_cache.RemoveAssetBundleInfo(path, immediate, onlyRemove)) {
                UnloadDependencies(path, immediate);
            }
        }
        
        // 依赖
        // 加载依赖
        private void LoadDependencies(LoaderGroup group, string name, bool async, bool persistent)
        {
            if (m_manifest == null) {
                return;
            }
            
            string[] dependencies =
                m_manifest.GetDirectDependencies(string.Format("{0}{1}", name, ConstantData.AssetBundleExt));
            for (int i = 0; i < dependencies.Length; ++i) {
                LoadAssetBundle(group, dependencies[i].Replace(ConstantData.AssetBundleExt, ""), null, async,
                    persistent);
            }
        }
        
        // 卸载依赖
        private void UnloadDependencies(string name, bool immediate = false)
        {
            if (m_manifest == null) {
                return;
            }
            
            string[] dependencies =
                m_manifest.GetDirectDependencies(string.Format("{0}{1}", name, ConstantData.AssetBundleExt));
            for (int i = 0; i < dependencies.Length; ++i) {
                UnloadAssetBundle(dependencies[i].Replace(ConstantData.AssetBundleExt, ""), immediate);
            }
        }
        
        // ------------------------------------------------------------------------------------------
        public void AddToDownloadOrCache(string path)
        {
            m_downloadOrCache.Add(path);
        }
        
        // ------------------------------------------------------------------------------------------
        // 文件/目录是否存在
        private bool CheckFileExist(string path, LoadedHandler onLoaded, bool isFile = true)
        {
            bool exist = isFile ? File.Exists(path) : Directory.Exists(path);
            if (!exist) {
                // 不存在
                if (onLoaded != null) {
                    onLoaded(null);
                }
                
                return false;
            }
            
            return true;
        }
        
        // ------------------------------------------------------------------------------------------
        // 前台加载
        public void BeginFrontLoad()
        {
            //m_task.BeginFrontLoad();
        }
        
        public void StartFrontLoad(ProgressHandler onProgress)
        {
            //m_task.StartFrontLoad(onProgress);
        }
        
        private void UpdateUnpacker()
        {
            if (m_unpacker != null) {
                m_unpacker.Update();
            }
        }
        
        /*public void UnpackStreamingAssets(Action onStart, Action<int, int, int> onProgress, Action onCompleted)
        {
            List<string> files = new List<string>();
            for (int i = 0; i < VersionData.Inst.Items.Length; ++i) {
                VersionData.VersionItem item = VersionData.Inst.Items[i];
        
                string path = FileHelper.GetCacheAssetBundlePath(item.Md5);
                if (!FileHelper.CheckFileExist(path)) {
                    files.Add(GetResourcePath(item.Name));
                }
            }
        
            if (files.Count == 0) {
                if (onCompleted != null) {
                    onCompleted();
                }
        
                return;
            }
        
            if (onStart != null) {
                onStart();
            }
        
            int thread = Mathf.Clamp(SystemInfo.processorCount - 1, 1, 20);
        
            m_unpacker = new DownloadOrCache(thread, files, onProgress, () => {
                m_unpacker = null;
                if (onCompleted != null) {
                    onCompleted();
                }
            });
        }*/
        
        
        // ------------------------------------------------------------------------------------------
        public void Dump()
        {
        }
    }
}