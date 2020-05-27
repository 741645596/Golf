using System.Collections.Generic;
using IGG.Core;
using Object = UnityEngine.Object;


/// <summary>
/// 加载asset 对象
/// </summary>
/// <author>zhulin</author>

public delegate void AssetLoadHook(UnityEngine.Object g);
public class AssetLoad
{

    protected  Dictionary<string, UnityEngine.Object> g_CacheAsset = null;
    private    Dictionary<string, byte[]> g_Cachebytes = null;
    
    protected  void AddCache(string key, UnityEngine.Object obj)
    {
        if (obj == null) {
            return;
        }
        
        if (g_CacheAsset.ContainsKey(key) == false) {
            g_CacheAsset.Add(key, obj);
        }
    }
    
    // 查找ab 包
    protected UnityEngine.Object FindAssetObj(string key)
    {
    
        if (g_CacheAsset.ContainsKey(key) == true) {
            return g_CacheAsset [key];
        } else {
            return null;
        }
    }
    
    
    protected void AddBytesCache(string key, byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0) {
            return;
        }
        
        if (g_Cachebytes.ContainsKey(key) == false) {
            g_Cachebytes.Add(key, bytes);
        }
    }
    
    // 查找ab 包
    protected byte[] FindBytes(string key)
    {
    
        if (g_Cachebytes.ContainsKey(key) == true) {
            return g_Cachebytes [key];
        } else {
            return null;
        }
    }
    
    
    
    // 初始化
    public  void Init()
    {
        g_CacheAsset = new Dictionary<string,  UnityEngine.Object>();
        g_Cachebytes = new Dictionary<string, byte[]>();
    }
    
    
    // 清理工作
    public void Clear()
    {
        g_CacheAsset.Clear();
        g_Cachebytes.Clear();
    }
    
    public virtual void FreeBytes(string ResType, string ABName)
    {
    
    }
    
    public virtual void FreeAsset(string ResType, string ABName)
    {
    
    }
    
    
    // 加载shader 的接口
    public virtual void LoadShader(string shaderName, bool async, AssetLoadHook pfun)
    {
    }
    
    
    
    /// <summary>
    /// 音乐
    /// </summary>
    /// <param name="MusicName"></param>
    /// <param name="async"></param>
    /// <param name="pfun"></param>
    public virtual void LoadMusic(string MusicName, bool async, AssetLoadHook pfun)
    {
    
    }
    /// <summary>
    ///  加载音效
    /// </summary>
    /// <param name="VoiceName"></param>
    /// <param name="async"></param>
    /// <param name="pfun"></param>
    public virtual void LoadVoice(string VoiceName, bool async, AssetLoadHook pfun)
    {
    
    }
    
    
    // 加载精灵
    public virtual void LoadSprite(string AtlasName, string SpriteName, bool async, AssetLoadHook pfun)
    {
    
    }
    
    
    // 加载预制
    public  virtual void LoadPrefab(string prefabName, string Type, bool IsCahce, bool async, AssetLoadHook pfun)
    {
    }
    
    /// <summary>
    ///  加载材质
    /// </summary>
    /// <param name="MaterialName"></param>
    /// <param name="async"></param>
    /// <param name="pfun"></param>
    public virtual void LoadMaterial(string MaterialName, bool async, AssetLoadHook pfun)
    {
    }
    
    
    // 加载lua文件
    public virtual byte[] LoadLua(string LuaName, bool IsCache)
    {
        return null;
    }
    
    
    // 加载配置文件
    public virtual Object LoadConfig(string ResType, string ConfigName)
    {
        return null;
    }
}
