using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IGG.Core;
using Object = UnityEngine.Object;
using IGG.Core.Data.Config;
/// <summary>
/// 资源加载模块
/// </summary>
/// <author>zhulin</author>
/// Server--> Local (down) ---> Asset ====> gameobject / component

public delegate void ResLoadHook(GameObject go);

public class ResourceManger
{

    private static MonoBehaviour g_asyncGo = null;
    public static MonoBehaviour AsyncGo{
        get { return g_asyncGo;}
    }
    private static AssetLoad g_AssetLoad = null;
    public  static AssetLoad gAssetLoad{
        get{ return g_AssetLoad;}
    }
    
    
    
    public  static void SetLoadObj(MonoBehaviour fac, AssetLoad load)
    {
        g_AssetLoad = load;
        g_asyncGo = fac;
    }
    
    // 初始化资源模块
    public static void InitCache()
    {
        //ABLoad.Init();
        g_AssetLoad.Init();
        PoolManager.Init(AsyncGo);
    }
    
    
    
    public static void ClearScenePools()
    {
        /*if (null != g_PoolLoad) {
            g_PoolLoad.ClearSceneCache();
        }*/
    }
    
    
    
    
    
    
    //统一的资源释放接口，有通过该模块加载的对象都应从该接口进行释放。
    public static void FreeGo(GameObject target)
    {
        CacheItem ir = target.GetComponent<CacheItem>();
        if (ir == null) {
            GameObject.Destroy(target);
            return;
        }
        PoolManager.FreeGo(ir);
    }
    
    
    //   清理资源
    public static void Clear()
    {
        if (g_AssetLoad != null) {
            g_AssetLoad.Clear();
        }
        PoolManager.Clear();
        //ABLoad.Clear();
    }
    
    
    
    // 加载对象。
    public static void LoadGo(string ResName, string ResType, Transform parent, bool IsCacheAsset, bool async, ResLoadHook pfun)
    {
        GameObject go = null;
        // 从缓存池取。
        go = PoolManager.GetCacheGo(ResType, ResName);
        if (go != null) {
            if (null != parent) {
                go.SetActive(true);
                go.transform.SetParent(parent, false);
            }
            if (pfun != null) {
                pfun(go);
            }
            return;
        }
        
        // cong asset 中取。并实例化
        if (go == null) {
            LoadPrefab(ResType, ResName, false, true, (g) => {
                GameObject prefab = g as GameObject;
                if (null != prefab) {
                    go = GameObject.Instantiate(prefab);
                    go.name = prefab.name;
                    // 设置父亲结点
                    if (null != go && null != parent) {
                        go.transform.SetParent(parent, false);
                    }
                }
                if (pfun != null) {
                    pfun(go);
                }
            });
        }
    }
    
    /// <summary>
    /// 从文件系统，或ab中加载prefab。注意不是实例。
    /// </summary>
    /// <param name="ResType"></param>
    /// <param name="ResName"></param>
    /// <param name="IsCacheAsset"></param>
    /// <param name="async"></param>
    /// <param name="pfun"></param>
    /// <returns></returns>
    public static void LoadPrefab(string ResType, string ResName, bool IsCacheAsset, bool async, ResLoadHook pfun)
    {
        g_AssetLoad.LoadPrefab(ResName, ResType, IsCacheAsset, async,
        (g) => {
            GameObject prefab = g as GameObject;
            if (null != prefab && pfun != null) {
                pfun(prefab);
            }
        });
    }
    
    /// <summary>
    /// 加载场景item
    /// <param ItemName>场景item名称</param>
    /// <param parent>父节点</param>
    /// <param IsCache>是否缓存</param>
    /// <param async>是否异步</param>
    /// <param pfun>回调函数</param>
    /// </summary>
    public static void LoadSceneItem(string ItemName, Transform parent, bool async, ResLoadHook pfun)
    {
        LoadGo(ItemName, ResourcesType.SceneItem, parent, true, async, pfun);
    }
    
    /// <summary>
    /// 加载窗口
    /// <param WndName>窗口名称</param>
    /// <param parent>父节点</param>
    /// <param IsCache>是否缓存</param>
    /// <param async>是否异步</param>
    /// <param pfun>回调函数</param>
    /// </summary>
    public static void LoadWnd(string WndName, Transform parent, bool IsCache,  bool async, ResLoadHook pfun)
    {
        LoadGo(WndName, ResourcesType.UIWnd, parent, IsCache, async, pfun);
    }
    
    /// <summary>
    /// 加载窗口item
    /// <param WndItemName>窗口item名称</param>
    /// <param parent>父节点</param>
    /// <param IsCache>是否缓存</param>
    /// <param async>是否异步</param>
    /// <param pfun>回调函数</param>
    /// </summary>
    public static void LoadWndItem(string WndItemName, Transform parent, bool async, ResLoadHook pfun)
    {
        LoadGo(WndItemName, ResourcesType.UIWndItem, parent, true, async, pfun);
    }
    
    /// <summary>
    /// 加载特效
    /// <param EffectName>特效名称</param>
    /// <param parent>父节点</param>
    /// <param IsCache>是否缓存</param>
    /// <param async>是否异步</param>
    /// <param pfun>回调函数</param>
    /// </summary>
    public static void LoadEffect(string EffectName, Transform parent, bool IsCache, bool async, ResLoadHook pfun)
    {
        LoadGo(EffectName, ResourcesType.Effect, parent, IsCache, async, pfun);
    }
    
    public static void LoadMap(string MapName, Transform parent, bool IsCache, bool async, ResLoadHook pfun)
    {
        LoadGo(MapName, ResourcesType.Map, parent, IsCache, async, pfun);
    }

    /// <summary>
    /// 加载小球
    /// </summary>
    /// <param name="ballName"></param>
    /// <param name="parent"></param>
    /// <param name="IsCache"></param>
    /// <param name="async"></param>
    /// <param name="pfun"></param>
    public static void LoadBall(string ballName, Transform parent, bool IsCache, bool async, ResLoadHook pfun)
    {
        LoadGo(ballName, ResourcesType.Ball, parent, IsCache, async, pfun);
    }

    /// <summary>
    /// 加载英雄
    /// <param HeroName>英雄名称</param>
    /// <param parent>父节点</param>
    /// <param IsCache>是否缓存</param>
    /// <param async>是否异步</param>
    /// <param pfun>回调函数</param>
    /// </summary>
    public static void LoadHero(string HeroName, Transform parent, bool IsCache, bool async, ResLoadHook pfun)
    {
        LoadGo(HeroName, ResourcesType.Hero, parent, IsCache, async, pfun);
    }
    
    /// <summary>
    /// 加载精灵
    /// <param AltasName>图集名称</param>
    /// <param SpriteName>精灵名称</param>
    /// <param async>是否异步</param>
    /// <param pfun>回调函数</param>
    /// </summary>
    public static void LoadSprite(string AtlasName, string SpriteName, bool async, AssetLoadHook pfun)
    {
        g_AssetLoad.LoadSprite(AtlasName, SpriteName, async, pfun);
    }
    
    /// <summary>
    /// 加载音效
    /// <param VoiceName>音效名称</param>
    /// <param async>是否异步</param>
    /// <param pfun>回调函数</param>
    /// </summary>
    public static void LoadVoice(string VoiceName, bool async, AssetLoadHook pfun)
    {
        g_AssetLoad.LoadVoice(VoiceName, async, pfun);
    }
    
    
    /// <summary>
    /// 加载音乐
    /// <param MusicName>音乐名称</param>
    /// <param async>是否异步</param>
    /// <param pfun>回调函数</param>
    /// </summary>
    public static void LoadMusic(string MusicName, bool async, AssetLoadHook pfun)
    {
        g_AssetLoad.LoadMusic(MusicName, async, pfun);
    }
    
    
    // 读取lua
    public static byte[] loadLua()
    {
        return g_AssetLoad.LoadLua("fixlua", true);
    }
    
    /// <summary>
    /// 加载shader
    /// </summary>
    public static Material LoadMaterial(string MaterialName)
    {
        Material s = null;
        g_AssetLoad.LoadMaterial(MaterialName, false,
        (g) => {
            s = g as Material;
        });
        return s;
    }
    /// <summary>
    /// 加载shader
    /// </summary>
    public static Shader LoadShader(string shaderName)
    {
        Shader s = null;
        g_AssetLoad.LoadShader(shaderName, false,
        (g) => {
            s = g as Shader;
        });
        return s;
    }
    
    public static void LoadDependeAB(string dir, string ABName)
    {
        //ABLoad.LoadABbyPath(dir, ABName, true);
    }
    
    public static Object LoadConfig(string configName)
    {
        return g_AssetLoad.LoadConfig(ResourcesType.Config, configName);
    }
}

/// <summary>
/// use for ResFactory
/// create gameobject with all the type name and attach spawnpool component
/// </summary>
public struct ResourcesType {

    public const string UIWnd = "UIWnd";
    public const string UIWndItem = "UIWndItem";
    public const string UIAltas = "UIAltas";
    public const string SceneItem = "SceneItem";
    
    
    public const string Map = "Map";
    public const string Ball = "Ball";
    public const string Effect = "Effect";
    public const string Hero = "Hero";
    
    
    public const string Music = "Music";
    public const string Voice = "Voice";
    
    public const string Material = "Material";
    public const string Shader = "Shader";
    public const string Scene = "Scene";
    public const string luaData = "luaData";
    
    public const string Config = "Config";
    
}