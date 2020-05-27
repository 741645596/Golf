using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IGG.Core.Data.Config;
using IGG.Core.Manger.Coroutine;

public class PoolManager
{
    private static MonoBehaviour g_asyncGo = null;
    private static Dictionary<string, CacheItems> pool;
    
    /// <summary>
    /// 初始化操作
    /// </summary>
    public static void Init(MonoBehaviour mono)
    {
        g_asyncGo = mono;
        pool = new Dictionary<string, CacheItems>();
    }
    
    /// <summary>
    /// 清理缓存数据
    /// </summary>
    public static void Clear()
    {
        foreach (CacheItems v in pool.Values) {
            v.Clear();
        }
        pool.Clear();
        pool = null;
    }
    /// <summary>
    /// 使用分帧处理
    /// </summary>
    /// <param name="battleID"></param>
    public static void PoolsPrepareData(uint battleID)
    {
        List<ResourcePreloadConfig> list = ResourcePreloadDao.Inst.GetPreLoadCfgs(battleID);
        g_asyncGo.StartCoroutine(LoadListRes(list));
    }
    
    /// <summary>
    /// 通过携程搞定
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private static IEnumerator LoadListRes(List<ResourcePreloadConfig> list)
    {
        if (list == null || list.Count == 0) {
            yield return null;
        }
        foreach (ResourcePreloadConfig cfg in list) {
            yield return LoadGoCahce(cfg.FilePath, cfg.PrefabName, (int)cfg.PreloadCounts);
            yield return Yielders.EndOfFrame;
        }
    }
    
    /// <summary>
    /// 获取缓存结构,不存在则创建
    /// </summary>
    /// <param name="resType"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public static CacheItems GetCacheItem(string resType, string resName)
    {
        string key = resType + "/" + resName;
        if (pool.ContainsKey(key)) {
            return pool[key];
        } else {
            CacheItems v = new CacheItems(resType, resName);
            pool.Add(key, v);
            return v;
        }
    }
    
    /// <summary>
    /// 获取缓存结构
    /// </summary>
    /// <param name="resType"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public static CacheItems FindCacheItem(string resType, string resName)
    {
        string key = resType + "/" + resName;
        if (pool.ContainsKey(key)) {
            return pool[key];
        } else {
            return null;
        }
    }
    
    /// <summary>
    /// 获取资源缓存。
    /// </summary>
    /// <param name="ResType"></param>
    /// <param name="ResName"></param>
    /// <returns></returns>
    public static GameObject GetCacheGo(string ResType, string ResName)
    {
        CacheItems v = FindCacheItem(ResType, ResName);
        if (v == null || v.Count == 0) {
            return null;
        }
        CacheItem ir = v.goQueue.Dequeue();
        if (ir == null) {
            return null;
        } else {
            return ir.gameObject;
        }
    }
    
    public static void FreeGo(CacheItem ir)
    {
        if (ir != null) {
            CacheItems v = GetCacheItem(ir.ResType, ir.ResName);
            v.SetCacheData(ir);
        }
    }
    
    /// <summary>
    /// 预加载的加载接口
    /// </summary>
    /// <param name="ResType"></param>
    /// <param name="ResName"></param>
    /// <param name="Count"></param>
    public static IEnumerator LoadGoCahce(string ResType, string ResName, int Count)
    {
        //包含此对象池,且有对象
        CacheItems v = PoolManager.GetCacheItem(ResType, ResName);
        if (v.Count > 0) {
            yield return v.CopyInstantiate(Count);
        } else {
            // 为同步操作
            GameObject prefab = null;
            ResourceManger.LoadPrefab(ResType, ResName, false, true, (g) => {
                prefab = g as GameObject;
            });
            // 执行分帧加载。
            if (prefab != null) {
                yield return Yielders.EndOfFrame;
                GameObject go = GameObject.Instantiate(prefab);
                // 设置父亲结点
                if (null != go) {
                    CacheItem recycle = go.GetComponent<CacheItem>();
                    if (recycle == null) {
                        recycle = go.AddComponent<CacheItem>();
                    }
                    recycle.SetCacheKey(ResType, ResName);
                    v.SetCacheData(recycle);
                    yield return Yielders.EndOfFrame;
                    yield return v.CopyInstantiate(Count);
                }
            }
        }
    }
}
