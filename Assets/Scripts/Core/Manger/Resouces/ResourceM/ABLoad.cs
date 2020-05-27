using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using System.IO;


/// <summary>
/// 加载AB对象 
/// </summary>
/// <author>zhulin</author>
public delegate void ABLoadHook(AssetBundle ab);
public class ABLoad {

	// key 为相对目录，相对目录 + ab 文件名
	private static Dictionary<string,AssetBundle> g_CacheAB = null;
	// 加入缓存管理
	private static void AddABCache(string key, AssetBundle AB)
	{
		if (AB == null) {
			return;
		}

		if (g_CacheAB.ContainsKey (key) == false) {
			g_CacheAB.Add(key, AB);
		}
	}

	// 查找ab 包
	public static AssetBundle FindAB(string key){

		if (g_CacheAB.ContainsKey (key) == true) {
			return g_CacheAB [key];
		} else
			return null;
	}
		
	/// 根据路径进行加载ab 包
	public static void LoadABbyPath(string RelativePath, string ABfileName, bool IsCache)
	{
		string ABPath = RelativePath + ABfileName;
		LoadAB(ABPath, IsCache, 
			(ab)=>{
				if(ab != null){
					ab.LoadAllAssets();
				}
			});
	}
		
    /// <summary>
    /// 异步的ab包加载
    /// 现在暂时是同步加载
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="isCache"></param>
    /// <param name="callback"></param>
	public static void LoadAB(string abPath, bool isCache, ABLoadHook callback)
	{
	    AssetBundle AB = LoadAbSync(abPath, isCache);
		if (callback != null ) {
            callback(AB);
		}
	}

    /// <summary>
    /// 同步的ab包加载
    /// </summary>
    /// <param name="path"></param>
    /// <param name="isCache"></param>
    public static AssetBundle LoadAbSync(string path, bool isCache)
    {
        AssetBundle ab = FindAB(path);
        if (ab == null)
        {
            ab = LoadLocalAssestBundle(path);
            if (ab != null && isCache)
            {
                AddABCache(path, ab);
            }
        }
        return ab;
    }

	/// 加载AB
	public static IEnumerator  LoadABasync(string ABPath, bool IsCache, ABLoadHook pfun)
	{
		AssetBundle AB = FindAB(ABPath);
		if (AB == null) 
		{
			string abpath = ConstantData.ABSavePath + ABPath;
			if (File.Exists (abpath) == true) 
			{
				byte[] stream = null;
				stream = File.ReadAllBytes(abpath);
				AssetBundleCreateRequest ABquest = AssetBundle.LoadFromMemoryAsync(stream);
				yield return ABquest;
				AB = ABquest.assetBundle;
				if (AB != null && IsCache) {
					AddABCache(ABPath, AB);
				}
			} 
		} 

		if (pfun != null) {
			pfun (AB);
		}
	}


	/// 从文件系统加载ab 包
	private static AssetBundle LoadLocalAssestBundle(string ABPath)
	{
		string abpath = ConstantData.ABSavePath + ABPath;
		if (File.Exists (abpath) == false) 
		{
			Debug.Log(ABPath + ":no exist");
			return null;
		}
		byte[] stream = null;
		stream = File.ReadAllBytes(abpath);
		AssetBundle Bundle = AssetBundle.LoadFromMemory(stream);

		if(Bundle != null)
		{
			//Debug.Log("ABLoad LoadFromFile" + abpath);
			return Bundle;
		}
		else 
		{
			Debug.Log("ABLoad load ab fail :" + abpath);
			return null;
		}
	}


	public static byte[] Loadbytes(string ABPath)
	{
		string abpath = ConstantData.ABSavePath + ABPath;
		if (File.Exists (abpath) == false) 
		{
			Debug.Log(ABPath + ":no exist");
			return null;
		}
		byte[] stream = null;
		stream = File.ReadAllBytes(abpath);
		return stream;
	}




	// 获取相对目录，
	public static string GetRelativePath(string ResType, string ABName){
		string str = ResourcesPath.GetRelativePath(ResType, ResourcesPathMode.AssetBundle);
		return str + ABName + ConstantData.ABEXT;
	}


	// 释放ab 包
	public static void FreeAB(string ResType, string ABName){
		string ABPath = GetRelativePath(ResType, ABName);
		AssetBundle AB = FindAB(ABPath);
		if (AB != null) {
			AB.Unload(true);
			g_CacheAB.Remove(ABPath);
		}
	} 

		
	// 初始化
	public static void Init()
	{
		g_CacheAB = new Dictionary<string, AssetBundle>();
	}



	// 清理工作
	public static void Clear()
	{
		foreach (AssetBundle ab in g_CacheAB.Values) {
			ab.Unload(true);
		}
		g_CacheAB.Clear();
	}
}
