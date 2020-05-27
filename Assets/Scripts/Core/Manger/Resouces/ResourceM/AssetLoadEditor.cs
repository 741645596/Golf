using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class AssetLoadEditor : AssetLoad
{
    public override void LoadMaterial(string MaterialName, bool async, AssetLoadHook pfun)
    {
        LoadObj(ResourcesType.Material, MaterialName, "", typeof(Material), true, pfun);
    }
    // 加载shader
    public override void LoadShader(string shaderName, bool async, AssetLoadHook pfun)
    {
        LoadObj(ResourcesType.Shader, shaderName, "", typeof(Shader), true, pfun);
    }
    
    
    // 声音
    public override void LoadMusic(string audioName, bool async, AssetLoadHook pfun)
    {
        LoadObj(ResourcesType.Music, audioName, "", typeof(AudioClip), true, pfun);
    }
    
    
    public override void LoadVoice(string audioName, bool async, AssetLoadHook pfun)
    {
        LoadObj(ResourcesType.Voice, audioName, "", typeof(AudioClip), true, pfun);
    }
    
    // 加载预制
    public  override void LoadPrefab(string prefabName, string Type, bool IsCahce, bool async, AssetLoadHook pfun)
    {
        LoadObj(Type, prefabName, "", typeof(GameObject), IsCahce, pfun);
    }
    
    // 加载精灵
    public override void LoadSprite(string AtlasName, string SpriteName, bool async, AssetLoadHook pfun)
    {
        LoadObj(ResourcesType.UIAltas, AtlasName, SpriteName, typeof(Sprite), true, pfun) ;
    }
    
    
    // 获取key 的接口
    private string GetKey(string ResType, string ObjName, string FileExt)
    {
        string str = ResourcesPath.GetRelativePath(ResType, ResourcesPathMode.Editor);
        return str + ObjName + FileExt;
    }
    
    
    
    // 加载lua文件
    public override byte[] LoadLua(string luaName, bool IsCache)
    {
        string FileExt = ResourcesPath.GetFileExt(ResourcesType.luaData);
        string key = GetKey(ResourcesType.luaData, luaName, FileExt);
        byte[] AB = FindBytes(key);
        if (AB != null) {
            return AB;
        } else {
            string PathName = "Assets/" + ResourcesPath.GetRelativePath(ResourcesType.luaData, ResourcesPathMode.Editor) + luaName + FileExt;
            TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(PathName) as TextAsset;
            if (ta != null && IsCache == true) {
                AddBytesCache(key, ta.bytes);
                return ta.bytes;
            }
        }
        return null;
    }
    
    
    
    
    
    // 加载asset接口
    private void LoadObj(string ResType,
        string ObjName,
        string subName,
        System.Type type,
        bool IsCahce,
        AssetLoadHook pfun)
    {
        UnityEngine.Object obj = null;
        string FileExt = "";
        if (subName == "") {
            FileExt = ResourcesPath.GetFileExt(ResType);
        } else {
            FileExt = "/" + subName + ResourcesPath.GetFileExt(ResType);
        }
        string key = GetKey(ResType, ObjName, FileExt);
        obj = FindAssetObj(key);
        if (obj != null) {
            if (pfun != null) {
                pfun(obj);
            }
        } else {
            string PathName = "Assets/" + ResourcesPath.GetRelativePath(ResType, ResourcesPathMode.Editor) + ObjName + FileExt;
            obj = AssetDatabase.LoadAssetAtPath(PathName, type);
            if (obj != null && IsCahce == true) {
                AddCache(key, obj);
            } else if (null == obj) {
                UnityEngine.Debug.Log("LoadAssetAtPahth  Empty : " + PathName + "   Type :  " + type);
            }
            if (pfun != null) {
                pfun(obj);
            }
        }
    }
}
#endif