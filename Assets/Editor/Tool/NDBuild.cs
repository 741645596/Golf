using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class NDBuild  {

	// 收集path目录下的所有资源
	public static void Build_AssestBundle(string SrcPath,string DestPath ,BuildAssetBundleOptions option ,BuildTarget Target)
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(Target);
		// 收集需要打包的资源
		List<Object> lresources = CollectAsset(SrcPath);
		//转换为AssestBundle
		while (lresources.Count > 0)
		{
			PackingResource (lresources [0] ,DestPath , option, Target) ;
			lresources.RemoveAt (0) ;
		}
		lresources.Clear();
	}


	// 收集path目录下的所有资源
	private static List<Object> CollectAsset( string path)
	{
		List<Object> list = new List<Object>();
		
		if (path.Contains(".svn") || ! Directory.Exists(path))
			return list;
		
		foreach (string file in Directory.GetFiles(path))
		{
			if (Path.GetExtension(file) == ".meta" ||
			    Path.GetExtension(file) == ".mat")
				continue;
			Object o = AssetDatabase.LoadAssetAtPath(file.Replace("\\", "/"), typeof(Object));
			if (o != null)
				list.Add(o);
		}
		// 递归添加下属资源
		foreach (string file in Directory.GetDirectories(path))
		{
			list.AddRange(CollectAsset(file));
		}
		
		return list;
	}
	// 打包资源
	private static void PackingResource(Object obj ,string DestPath ,BuildAssetBundleOptions option ,BuildTarget Target)
	{
		if(obj == null) return ;
		string fileName = obj.name + ".assetbundle";
		if (Target == BuildTarget.StandaloneWindows) {
			fileName = obj.name + ".PC3D";
		} 
		else if (Target == BuildTarget.Android) 
		{
			fileName = obj.name + ".Android3d";
		}
		// 构造之
		BuildPipeline.PushAssetDependencies();
		BuildPipeline.BuildAssetBundle(obj, null, DestPath + "/" + fileName, option, Target);
		BuildPipeline.PopAssetDependencies();
	}


	// 清理path目录下的所有资源
	public static void EmptyAsset(string path)
	{
		foreach (string file in Directory.GetFiles(path))
		{
			AssetDatabase.DeleteAsset(file);
		}
	}



}
