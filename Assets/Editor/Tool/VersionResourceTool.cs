using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class VersionResourceTool {

    // 收集path目录下的所有资源
	public static List<string> CollectAsset(string path)
    {
        List<string> list = new List<string>();

        if (path.Contains(".svn") || !Directory.Exists(path))
            return list;

        foreach (string file in Directory.GetFiles(path))
        {
            if (Path.GetExtension(file) == ".meta" ||
                Path.GetExtension(file) == ".mat")
                continue;
				list.Add(Path.GetFileName(file));
        }
        return list;
    }


    private static Dictionary<string, List<string>> CollectAssetPath(string path)
    {
        Dictionary<string, List<string>> list = new Dictionary<string, List<string>>();

        List<string> lPath = new List<string>();
        DirectoryInfo rootDirInfo = new DirectoryInfo(path);
        foreach (DirectoryInfo dirInfo in rootDirInfo.GetDirectories())
        {
			if (dirInfo.Name.Contains (".svn"))
				continue;
            lPath.Add(dirInfo.Name);
        }

        foreach (string subPath in lPath)
        {
			List<string> l = CollectAsset(path + subPath + "/");
            list.Add(subPath, l);
        }

        return list;
    }

    public static void MoveAsset(string OldPath, string NewPath)
    {
		List<string> l = CollectAsset(OldPath);
        foreach (string fileName in l)
        {
            AssetDatabase.MoveAsset(OldPath + fileName, NewPath + fileName);
        }
    }

    public static void CopyAsset(string OldPath, string NewPath)
    {
		List<string> l = CollectAsset(OldPath);
        foreach (string fileName in l)
        {
            AssetDatabase.CopyAsset(OldPath + fileName, NewPath + fileName);
        }
    }


    public static void DeleteAsset(string Path)
    {
		List<string> l = CollectAsset(Path);
        foreach (string fileName in l)
        {
            AssetDatabase.DeleteAsset(Path + fileName);
        }
    }
    /// <summary>
    /// 移动文件夹
    /// </summary>
    public static void MoveDir(string OldPath, string NewPath)
    {
        Directory.Move(OldPath, NewPath);
    }

    /// <summary>
    /// 移动文件
    /// </summary>
    public static void MoveFile(string OldFilePath, string NewFilePath)
    {
        File.Move(OldFilePath, NewFilePath);
    }


    public static void MoveAssetPath(string OldPath, string NewPath)
    {
        Dictionary<string, List<string>> l = CollectAssetPath(OldPath);
        foreach (string path in l.Keys)
        {
            string newPathDir = NewPath + path + "/";
            if (!Directory.Exists(newPathDir))
            {
                Directory.CreateDirectory(newPathDir);
            }
            MoveAsset(OldPath + path + "/", newPathDir);
        }
    }

    public static void CopyAssetPath(string OldPath, string NewPath)
    {
        Dictionary<string, List<string>> l = CollectAssetPath(OldPath);
        foreach (string path in l.Keys)
        {
            string newPathDir = NewPath + path + "/";
            if (!Directory.Exists(newPathDir))
            {
                Directory.CreateDirectory(newPathDir);
            }
            CopyAsset(OldPath + path + "/", newPathDir);
        }
    }


    public static void DeleteAssetPath(string Path)
    {
        Dictionary<string, List<string>> l = CollectAssetPath(Path);
        foreach (string dirName in l.Keys)
        {
            string newPathDir = Path + dirName + "/";
            DeleteAsset(newPathDir);
            Directory.Delete(newPathDir);
        }
    }
}
