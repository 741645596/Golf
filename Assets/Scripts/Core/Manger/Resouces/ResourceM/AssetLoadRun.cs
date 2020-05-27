using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetLoadRun : AssetLoad
{

    public override void LoadMaterial(string MaterialName, bool async, AssetLoadHook pfun)
    {
        // LoadObj(ResourcesType.Material, MaterialName, "", typeof(Material), true, pfun);
        string RelativePath = ResourcesPath.GetRelativePath(ResourcesType.Material, ResourcesPathMode.AssetBundle);
        Material go = Resources.Load<Material>(RelativePath + MaterialName) as Material;
        if (pfun != null) {
            pfun(go);
        }
    }
    // 加载shader 的接口
    public override void LoadShader(string shaderName, bool async, AssetLoadHook pfun)
    {
        string shaderkey = ResourcesPath.GetAssetResourceRunPath(ResourcesType.Shader, ResourcesPathMode.Editor) + shaderName + ".shader";
        shaderkey = shaderkey.ToLower();
        LoadObj(ResourcesType.Shader, "shader", shaderkey, typeof(Shader), true, true, false, async, pfun);
    }
    
    // 声音
    public override void LoadMusic(string audioName, bool async, AssetLoadHook pfun)
    {
        //LoadObj(ResourcesType.Audio, audioName.ToLower(), audioName.ToLower(), typeof(AudioClip), true, true, false, async,pfun) ;
        string RelativePath = ResourcesPath.GetRelativePath(ResourcesType.Music, ResourcesPathMode.AssetBundle);
        AudioClip go = Resources.Load<AudioClip>(RelativePath + audioName) as AudioClip;
        if (pfun != null) {
            pfun(go);
        }
    }
    
    
    public override void LoadVoice(string audioName, bool async, AssetLoadHook pfun)
    {
        //LoadObj(ResourcesType.Audio, audioName.ToLower(), audioName.ToLower(), typeof(AudioClip), true, true, false, async,pfun) ;
        string RelativePath = ResourcesPath.GetRelativePath(ResourcesType.Voice, ResourcesPathMode.AssetBundle);
        AudioClip go = Resources.Load<AudioClip>(RelativePath + audioName) as AudioClip;
        if (pfun != null) {
            pfun(go);
        }
    }
    
    
    // 加载精灵
    public override void LoadSprite(string AtlasName, string SpriteName, bool async, AssetLoadHook pfun)
    {
        //LoadObj(ResourcesType.UIAltas, AtlasName.ToLower(), SpriteName.ToLower(), typeof(Sprite), true, true, false, async,pfun);
        string RelativePath = ResourcesPath.GetRelativePath(ResourcesType.UIAltas, ResourcesPathMode.AssetBundle);
        Sprite go = Resources.Load<Sprite>(RelativePath + AtlasName + "/" + SpriteName) as Sprite;
        if (pfun != null) {
            pfun(go);
        }
    }
    
    
    // 加载预制
    public  override void LoadPrefab(string prefabName, string Type, bool IsCahce, bool async, AssetLoadHook pfun)
    {
        string objName = prefabName.ToLower();
        /*if (IsCahce == true) {
        	 LoadObj(Type, prefabName, objName, typeof(GameObject), true, true, false, async, pfun);
        } else {
        	 LoadObj(Type, prefabName, objName, typeof(GameObject), false, false, true, async, pfun);
        }*/
        string RelativePath = ResourcesPath.GetRelativePath(Type, ResourcesPathMode.AssetBundle);
        GameObject go = Resources.Load<GameObject>(RelativePath + prefabName) as GameObject;
        if (pfun != null) {
            pfun(go);
        }
    }
    
    
    // 加载lua
    public override byte[] LoadLua(string luaName, bool IsCache)
    {
        string key = GetKey(ResourcesType.luaData, luaName, luaName);
        byte[] AB = FindBytes(key);
        if (AB != null) {
            return AB;
        } else {
            string path = ABLoad.GetRelativePath(ResourcesType.luaData, luaName);
            AB = ABLoad.Loadbytes(path);
            if (AB != null && IsCache == true) {
                AddBytesCache(key, AB);
            }
            return AB;
        }
    }
    
    
    
    // 加载asset接口
    private void LoadObj(string ResType,
        string ABName,
        string ObjName,
        Type type,
        bool IsCacheAsset,
        bool IsCacheAB,
        bool IsFreeUnUseABRes,
        bool async,
        AssetLoadHook pfun)
    {
        string RelativePath = ABLoad.GetRelativePath(ResType, ABName);
        string key = GetKey(RelativePath, ObjName);
        UnityEngine.Object obj = FindAssetObj(key);
        if (obj != null) {
            if (pfun != null) {
                pfun(obj);
            }
        } else {
            if (async == false) {
                ABLoad.LoadAB(RelativePath, IsCacheAB,
                (ab) => {
                    Object g = LoadObjByAb(ab, ObjName, type, IsCacheAB, IsFreeUnUseABRes);
                    LoadAssetCallBack(g, key, IsCacheAsset, pfun);
                });
            } else {
                // 异步加载队列
                LoadQueue.AddLoadTask(RelativePath, ObjName, type, IsCacheAsset, IsCacheAB, IsFreeUnUseABRes, pfun);
                
            }
        }
    }
    
    /// <summary>
    /// 以同步的方式加载资源
    /// </summary>
    /// <param name="resType"></param>
    /// <param name="abName"></param>
    /// <param name="objName"></param>
    /// <param name="type"></param>
    /// <param name="isCache"></param>
    /// <param name="isCacheAb"></param>
    /// <param name="isFreeUnUseAbRes"></param>
    /// <returns></returns>
    private Object LoadObjSync(string resType, string abName, string objName, Type type, bool isCache, bool isCacheAb, bool isFreeUnUseAbRes)
    {
        string relativePath = ABLoad.GetRelativePath(resType, abName);
        string key = GetKey(relativePath, objName);
        Object obj = FindAssetObj(key);
        if (obj != null) {
            return obj;
        }
        
        AssetBundle assetBundle = ABLoad.LoadAbSync(relativePath, isCacheAb);
        if (assetBundle == null) {
            return null;
        }
        
        obj = LoadObjByAb(assetBundle, objName, type, isCacheAb, isFreeUnUseAbRes);
        if (isCache == true && obj != null) {
            AddCache(key, obj);
        }
        
        return obj;
    }
    
    // 获取key 的接口
    private string GetKey(string RelativePath, string ObjName)
    {
        return RelativePath + ObjName;
    }
    
    // 获取key 的接口
    private string GetKey(string ResType, string ABName, string ObjName)
    {
        string RelativePath = ABLoad.GetRelativePath(ResType, ABName);
        return GetKey(RelativePath, ObjName);
    }
    
    // 加载资源的回调
    public void LoadAssetCallBack(UnityEngine.Object obj, string key, bool IsCacheAsset, AssetLoadHook pfun)
    {
        if (IsCacheAsset == true && obj != null) {
            AddCache(key, obj);
        }
        if (pfun != null) {
            pfun(obj);
        }
    }
    
    /// <summary>
    /// 从assetsBundle里加载资源
    /// </summary>
    /// <param name="ab"></param>
    /// <param name="objName"></param>
    /// <param name="type"></param>
    /// <param name="isCache"></param>
    /// <param name="isFreeUnUseAbRes"></param>
    /// <returns></returns>
    private Object LoadObjByAb(AssetBundle ab, string objName, Type type, bool isCache, bool isFreeUnUseAbRes)
    {
        if (ab == null) {
            return null;
        }
        Object obj = ab.LoadAsset(objName, type);
        if (isFreeUnUseAbRes == true && isCache == false) {
            ab.Unload(false);
        }
        return obj;
    }
    
    public  IEnumerator LoadObjAsync(AssetBundle AB, string objName, System.Type type, bool IsCache, bool IsFreeUnUseABRes, AssetLoadHook pfun)
    {
        if (AB != null) {
            AssetBundleRequest quest = AB.LoadAssetAsync(objName, type);
            yield return quest;
            
            if (pfun != null) {
                pfun(quest.asset);
            }
            if (IsFreeUnUseABRes == true && IsCache == false) {
                AB.Unload(false);
            }
        } else {
            if (pfun != null) {
                pfun(null);
            }
        }
    }
    
    
    
    // 加载配置文件
    public override Object LoadConfig(string ResType, string ConfigName)
    {
        string RelativePath = ResourcesPath.GetRelativePath(ResType, ResourcesPathMode.AssetBundle);
        return Resources.Load<Object>(RelativePath + ConfigName) as Object;
        
    }
}
