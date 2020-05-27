using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IGG.Core.Manger.Coroutine;


public class CacheItems
{
    public CacheItems()
    {
        this.goQueue = new Queue<CacheItem>();
    }
    public CacheItems(string resType, string resName)
    {
        this.goQueue = new Queue<CacheItem>();
        this.ResName = resName;
        this.ResType = resType;
    }
    public string ResType;
    public string ResName;
    /// <summary>
    /// key的计算方式
    /// </summary>
    public string Key {
        get { return ResType + "/" + ResName; }
    }
    public int Count {
        get { return goQueue.Count; }
    }
    public Queue<CacheItem> goQueue = null;
    
    public IEnumerator CopyInstantiate(int NeedTotalCount)
    {
        //获取对象
        int needCount = NeedTotalCount - Count;
        if (needCount <= 0) {
            yield return null;
        }
        CacheItem g = goQueue.Dequeue();
        goQueue.Enqueue(g);
        for (int i = 0; i < needCount; i++) {
            GameObject gg = GameObject.Instantiate(g.gameObject, ResourceManger.AsyncGo.transform);
            gg.name = ResName;
            //设置激活状态
            gg.SetActive(false);
            //设置父物体
            CacheItem recycle = gg.GetComponent<CacheItem>();
            if (recycle != null) {
                recycle.OnRecycle();
            }
            goQueue.Enqueue(recycle);
            yield return Yielders.EndOfFrame;
        }
    }
    
    /// <summary>
    /// 缓存对象初始化数据
    /// </summary>
    /// <param name="go"></param>
    public void SetCacheData(CacheItem Recycle)
    {
        if (Recycle == null) {
            return;
        }
        Recycle.transform.parent = ResourceManger.AsyncGo.transform;
        Recycle.gameObject.name = ResName;
        Recycle.gameObject.SetActive(false);
        Recycle.OnRecycle();
        this.goQueue.Enqueue(Recycle);
    }
    
    /// <summary>
    /// 清空数据
    /// </summary>
    public void Clear()
    {
        if (goQueue == null) {
            return;
        }
        while (goQueue.Count > 0) {
            CacheItem g = goQueue.Dequeue();
            if (g != null) {
                GameObject.Destroy(g.gameObject);
            }
        }
        goQueue.Clear();
        goQueue = null;
    }
}
