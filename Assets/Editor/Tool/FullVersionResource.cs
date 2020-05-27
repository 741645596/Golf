using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading;
//using SimpleJSON;
using IGG.EditorTools;
using IGG.Core.Helper;

public class FullVersionResource
{
    public static void CopyAssets2Resources(bool clear = true)
    {
        if (clear) {
            DeleteResourcesAssets();
        }
        // config
        FileHelper.CopyDirectory(GetCopySourcePath(ResourcesType.Config), GetCopyDestPath(ResourcesType.Config));
        // material
        FileHelper.CopyDirectory(GetCopySourcePath(ResourcesType.Material), GetCopyDestPath(ResourcesType.Material));
        // voice
        FileHelper.CopyDirectory(GetCopySourcePath(ResourcesType.Music), GetCopyDestPath(ResourcesType.Music));
        FileHelper.CopyDirectory(GetCopySourcePath(ResourcesType.Voice), GetCopyDestPath(ResourcesType.Voice));
        // perfab
        FileHelper.CopyDirectory(GetCopySourcePath(ResourcesType.Hero), GetCopyDestPath(ResourcesType.Hero));
        FileHelper.CopyDirectory(GetCopySourcePath(ResourcesType.Effect), GetCopyDestPath(ResourcesType.Effect));
        FileHelper.CopyDirectory(GetCopySourcePath(ResourcesType.Map), GetCopyDestPath(ResourcesType.Map));
        FileHelper.CopyDirectory(GetCopySourcePath(ResourcesType.SceneItem), GetCopyDestPath(ResourcesType.SceneItem));
        FileHelper.CopyDirectory(GetCopySourcePath(ResourcesType.UIWnd), GetCopyDestPath(ResourcesType.UIWnd));
        FileHelper.CopyDirectory(GetCopySourcePath(ResourcesType.UIWndItem), GetCopyDestPath(ResourcesType.UIWndItem));
        // altas
        FileHelper.CopyDirectory(GetCopySourcePath(ResourcesType.UIAltas), GetCopyDestPath(ResourcesType.UIAltas));
        
        AssetDatabase.Refresh();
    }
    
    private static string GetCopySourcePath(string Type)
    {
        return Application.dataPath + "/" + ResourcesPath.GetRelativePath(Type, ResourcesPathMode.Editor);
    }
    
    private static string GetCopyDestPath(string Type)
    {
        return Application.dataPath + "/" + "Resources/" + ResourcesPath.GetRelativePath(Type, ResourcesPathMode.AssetBundle);
    }
    
    /// <summary>
    /// 情况Resources目录
    /// </summary>
    public static void DeleteResourcesAssets()
    {
        FileHelper.ClearFileDirectory(Application.dataPath + "/" + "Resources/");
        AssetDatabase.Refresh();
    }
    
    //public static bool _bDebugVersion = false;
    
    /*public static void CopyAssets(bool clear = false)
    {
    	if (clear)
    	{
    		DeleteAssets();
    	}
    
    	List<VersionData.VersionItem> list = new List<VersionData.VersionItem>();
        List<string> files = IGG.FileUtil.GetAllChildFiles(BuildAssetBundle.AssetBundlePath, "ab");
    
        if (ConstantData.EnableUnpack)
        {
            // 自定义压缩
            string inPath = string.Format("{0}{1}", BuildAssetBundle.AssetBundlePath, LogicConstantData.g_version);
            string outPath = "";
            if (ConstantData.EnableMd5Name)
            {
                string md5 = IGG.FileUtil.CalcFileMd5(inPath);
                outPath = string.Format("{0}/{1}{2}", ConstantData.StreamingAssetsPath, md5, ConstantData.ABEXT);
            }
            else
            {
                outPath = string.Format("{0}/data", ConstantData.StreamingAssetsPath);
            }
    
            ThreadParam param = new ThreadParam();
            param.pathSrc = BuildAssetBundle.AssetBundlePath;
            param.pathDst = ConstantData.StreamingAssetsPath;
            param.list = list;
            param.files = files;
            param.index = 0;
            param.lockd = new object();
    
    		PackFile(inPath, outPath, "data", param);
    
    		int threadCount = SystemInfo.processorCount;
    
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < threadCount; ++i)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(OnThreadCompress));
                thread.Start(param);
    
                threads.Add(thread);
            }
    
            while (true)
            {
                EditorUtility.DisplayProgressBar("压缩中...", string.Format("{0}/{1}", param.index, param.files.Count), Mathf.InverseLerp(0, param.files.Count, param.index));
    
                bool hasAlive = false;
                foreach (Thread thread in threads)
                {
                    if (thread.IsAlive)
                    {
                        hasAlive = true;
                        break;
                    }
                }
    
                if (!hasAlive)
                {
                    break;
                }
    
                Thread.Sleep(10);
            }
    	}
    	else
        {
            // 直接拷贝
            if (ConstantData.EnableMd5Name)
            {
                string pathSrc = BuildAssetBundle.AssetBundlePath;
                string pathDst = ConstantData.StreamingAssetsPath;
    
                {
                    string file = string.Format("{0}/{1}", pathSrc, LogicConstantData.g_version);
                    CopyAsset(file, pathDst, list, "data");
                }
    
                int index = 0;
                foreach (string file in files)
                {
                    string name = file.Replace("\\", "/").Replace(pathSrc, "");
                    CopyAsset(file, pathDst, list, name);
    
                    ++index;
                    EditorUtility.DisplayProgressBar("拷贝中...", string.Format("{0}/{1}", index, files.Count), Mathf.InverseLerp(0, files.Count, index));
                }
            }
            else
            {
                // 把所有的ab文件拷贝进StreamAssets的ab目录下
                IGG.FileUtil.CopyDirectory(BuildAssetBundle.AssetBundlePath, Application.streamingAssetsPath + "/ab/", ConstantData.ABEXT);
    
                // 拷贝manifest进StreamAssets,并命名为data
                string pathSrc = string.Format("{0}/{1}", BuildAssetBundle.AssetBundlePath, LogicConstantData.g_version);
                string pathDst = string.Format("{0}/ab/data", Application.streamingAssetsPath);
                IGG.FileUtil.CopyFile(pathSrc, pathDst);
            }
        }
    
        if (ConstantData.EnableMd5Name)
        {
    		string path = "Assets/Data/version.asset";
    
    		ConfigDataHelper.SaveData<VersionDataProxy>(path, (data)=>
    		{
    			data.version = LogicConstantData.g_version;
    			data.items = list.ToArray();
    		});
    	}
    
    	ClearObsolete(list);
    
    	EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }*/
    
    /*static void ClearObsolete(List<VersionData.VersionItem> list)
    {
        Dictionary<string, string> items = new Dictionary<string, string>();
        for (int i = 0; i < list.Count; ++i) {
            items.Add(list[i].md5, list[i].name);
        }
    
        List<string> files = IGG.FileUtil.GetAllChildFiles(ConstantData.StreamingAssetsPath, ConstantData.ABEXT);
        for (int i = 0; i < files.Count; ++i) {
            string filename = Path.GetFileNameWithoutExtension(files[i]);
            if (!items.ContainsKey(filename)) {
                IGG.FileUtil.DeleteFile(files[i]);
            }
        }
    }*/
    
    /*static void DeleteAssets()
    {
        FileHelper.DeleteFile(Application.streamingAssetsPath + "/" + "ab.zip");
        FileHelper.DeleteFileDirectory(ConstantData.StreamingAssetsPath);
    
        AssetDatabase.Refresh();
    }*/
    
    /*static void PackFile(string fileSrc, string fileDst, string name, ThreadParam param)
    {
        if (!IGG.FileUtil.CheckFileExist(fileDst)) {
            Debug.LogFormat("-->PackFile: {0} -> {1}", fileSrc, fileDst);
    
            IGG.FileUtil.CreateDirectoryFromFile(fileDst);
            IGG.FileUtil.DeleteFile(fileDst);
    
            LzmaHelper.Compress(fileSrc, fileDst);
        }
    
        if (ConstantData.EnableMd5Name) {
            string md5 = IGG.FileUtil.CalcFileMd5(fileSrc);
    
            FileInfo fi = new FileInfo(fileDst);
    
            VersionData.VersionItem item = new VersionData.VersionItem();
            item.name = name.Replace(ConstantData.ABEXT, "");
            item.md5 = md5;
            item.size = fi.Length;
    
            lock(param.lockd) {
                param.list.Add(item);
            }
        }
    }*/
    
    /*static void CopyAsset(string file, string pathDst, List<VersionData.VersionItem> list, string name)
    {
        string md5 = IGG.FileUtil.CalcFileMd5(file);
        string pathDstFull = string.Format("{0}/{1}.ab", pathDst, md5);
        if (IGG.FileUtil.CheckFileExist(pathDstFull)) { }
        {
            IGG.FileUtil.CopyFile(file, pathDstFull);
        }
    
        FileInfo fi = new FileInfo(file);
    
        VersionData.VersionItem item = new VersionData.VersionItem();
        item.name = name.Replace(ConstantData.ABEXT, "");
        item.md5 = md5;
        item.size = fi.Length;
    
        list.Add(item);
    }*/
    
    /*class ThreadParam
    {
        public string pathSrc;
        public string pathDst;
    
        public List<VersionData.VersionItem> list;
    
        public List<string> files;
        public int index;
    
        public object lockd;
    }*/
    
    /*static void OnThreadCompress(object arg)
    {
        ThreadParam param = arg as ThreadParam;
        while (true) {
            string file = "";
    
            lock(param.lockd) {
                if (param.index >= param.files.Count) {
                    // 完成
                    break;
                }
    
                file = param.files[param.index];
                ++param.index;
            }
    
            string name = file.Replace("\\", "/").Replace(param.pathSrc, "");
            string fileDst = "";
            if (ConstantData.EnableMd5Name) {
                string md5 = FileHelper.CalcFileMd5(file);
                fileDst = string.Format("{0}/{1}{2}", param.pathDst, md5, ConstantData.ABEXT);
            } else {
                fileDst = file.Replace(param.pathSrc, param.pathDst);
            }
    
            PackFile(file, fileDst, name, param);
        }
    }*/
    
    /*public static void InitPatch()
    {
        IGG.FileUtil.DeleteFileDirectory(EditorHelper.PatchDir);
    
        // version_orign
        {
            string pathSrc = "Assets/Data/version.asset";
            string pathDst = string.Format("{0}/version_orign", EditorHelper.PatchDir);
    
            VersionData data = ConfigDataHelper.GetData<VersionData>(pathSrc);
            if (data != null) {
                JSONClass json = new JSONClass();
                json.Add("version", LogicConstantData.g_version);
    
                JSONClass list = new JSONClass();
                json.Add("list", list);
    
                for (int i = 0; i < data.items.Length; ++i) {
                    VersionData.VersionItem item = data.items[i];
    
                    JSONClass jsonItem = new JSONClass();
                    jsonItem.Add("size", item.size.ToString());
                    jsonItem.Add("md5", item.md5);
    
                    list.Add(item.name, jsonItem);
                }
    
                IGG.FileUtil.SaveTextToFile(json.ToString(""), pathDst);
            }
        }
    
        // audio_orign
        {
            string pathAudio = string.Format("{0}/../WwiseProject/GeneratedSoundBanks/{1}", Application.dataPath, EditorHelper.PlatformName);
            pathAudio = pathAudio.Replace("\\", "/");
    
            JSONClass jsonAudio = new JSONClass();
    
            List<string> files = IGG.FileUtil.GetAllChildFiles(pathAudio);
            foreach (string file in files) {
                string md5 = IGG.FileUtil.CalcFileMd5(file);
                string name = file.Replace("\\", "/").Replace(pathAudio, "");
                if (name.StartsWith("/")) {
                    name = name.Substring(1);
                }
    
                jsonAudio.Add(name, md5);
            }
    
            string path = string.Format("{0}/audio_orign", EditorHelper.PatchDir);
            IGG.FileUtil.SaveTextToFile(jsonAudio.ToString(""), path);
        }
    
        // version_path
        {
            JSONClass json = new JSONClass();
            json.Add("version", LogicConstantData.g_version);
    
            JSONClass jsonList = new JSONClass();
            json.Add("list", jsonList);
    
            string path = string.Format("{0}/version_patch", EditorHelper.PatchDir);
            IGG.FileUtil.SaveTextToFile(json.ToString(""), path);
        }
    }*/
    
    /*public static void CopyPatch()
    {
        JSONClass json = new JSONClass();
        json.Add("version", LogicConstantData.g_version);
    
        JSONClass jsonList = new JSONClass();
        json.Add("list", jsonList);
    
        // ab
        {
            string pathOrign = string.Format("{0}/version_orign", EditorHelper.PatchDir);
            string pathNow = string.Format("{0}/version", Application.streamingAssetsPath);
    
            if (!File.Exists(pathOrign) || !File.Exists(pathNow)) {
                return;
            }
    
            JSONClass jsonOrign = JSONNode.Parse(IGG.FileUtil.ReadTextFromFile(pathOrign)) as JSONClass;
            JSONClass jsonNow = JSONNode.Parse(IGG.FileUtil.ReadTextFromFile(pathNow)) as JSONClass;
    
            JSONClass jsonListOrign = jsonOrign["list"] as JSONClass;
            JSONClass jsonListNow = jsonNow["list"] as JSONClass;
    
            foreach (KeyValuePair<string, JSONNode> item in jsonListNow) {
                if (jsonListOrign[item.Key] == null || !string.Equals(jsonListOrign[item.Key]["md5"].Value, item.Value["md5"].Value)) {
                    string pathSrc = string.Format("{0}/{1}{2}", ConstantData.StreamingAssetsPath, item.Value["md5"].Value, ConstantData.ABEXT);
                    string pathDst = string.Format("{0}/ab/{1}{2}", EditorHelper.PatchDir, item.Value["md5"].Value, ConstantData.ABEXT);
                    IGG.FileUtil.CopyFile(pathSrc, pathDst);
    
                    jsonList.Add(item.Key, item.Value);
                }
            }
        }
    
        // audio
        {
            string pathOrign = string.Format("{0}/audio_orign", EditorHelper.PatchDir);
            JSONClass jsonOrign = JSONNode.Parse(IGG.FileUtil.ReadTextFromFile(pathOrign)) as JSONClass;
    
            string pathAudio = string.Format("{0}/../WwiseProject/GeneratedSoundBanks/{1}", Application.dataPath, EditorHelper.PlatformName);
            pathAudio = pathAudio.Replace("\\", "/");
    
            bool hasPatch = false;
            List<string> files = IGG.FileUtil.GetAllChildFiles(pathAudio);
            foreach (string file in files) {
                string md5 = IGG.FileUtil.CalcFileMd5(file);
                string name = file.Replace("\\", "/").Replace(pathAudio, "");
                if (name.StartsWith("/")) {
                    name = name.Substring(1);
                }
    
                if (jsonOrign[name] == null || !string.Equals(md5, jsonOrign[name].Value)) {
                    string pathDst = string.Format("{0}/wwise/{1}", EditorHelper.PatchDir, name);
                    IGG.FileUtil.CopyFile(file, pathDst);
    
                    hasPatch = true;
                }
            }
    
            if (hasPatch) {
                string pathSrc = string.Format("{0}/wwise", EditorHelper.PatchDir);
                string pathDst = string.Format("{0}/ab/wwise_patch.zip", EditorHelper.PatchDir);
    
                IGG.FileUtil.CreateDirectoryFromFile(pathDst);
                ZipHelper.Pack(pathSrc, pathDst);
    
                IGG.FileUtil.DeleteFileDirectory(pathSrc);
    
                string md5 = IGG.FileUtil.CalcFileMd5(pathDst);
                string pathTemp = string.Format("{0}/ab/{1}{2}", EditorHelper.PatchDir, md5, ConstantData.ABEXT);
                File.Move(pathDst, pathTemp);
    
                FileInfo fi = new FileInfo(pathTemp);
    
                JSONClass item = new JSONClass();
                item.Add("md5", md5);
                item.Add("size", fi.Length.ToString());
                item.Add("wwise", new JSONData(true));
    
                jsonList.Add("wwise_patch", item);
            }
        }
    
        string path = string.Format("{0}/version_patch", EditorHelper.PatchDir);
        FileHelper.SaveTextToFile(json.ToString(""), path);
    }*/
    
    public static void Build(string pathProject, string pathPackage, bool development, bool autoConnectProfiler, bool isApk = true)
    {
        List<string> scenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
            if (string.IsNullOrEmpty(scene.path)) {
                continue;
            }
            
            string name = scene.path.Substring(scene.path.LastIndexOf("/") + 1);
            name = name.Substring(0, name.IndexOf(".")).ToLower();
            //if (string.Equals(name, "lauch") || string.Equals(name, "empty"))
            {
                scenes.Add(scene.path);
            }
        }
        
        BuildOptions options = BuildOptions.None;
        
        
        if (development) {
            options |= BuildOptions.Development;
            if (autoConnectProfiler) {
                options |= BuildOptions.AllowDebugging | BuildOptions.AutoRunPlayer | BuildOptions.ConnectWithProfiler | BuildOptions.ShowBuiltPlayer;
            }
        }
        
        string output = "";
        switch (EditorUserBuildSettings.activeBuildTarget) {
            case BuildTarget.StandaloneWindows:
                if (string.IsNullOrEmpty(pathProject)) {
                    output = string.Format("{0}/{1}/{1}.exe", EditorHelper.OutputDir, PlayerSettings.productName);
                } else {
                    output = string.Format("{0}/{1}.exe", pathProject, PlayerSettings.productName);
                }
                break;
            case BuildTarget.Android:
                if (isApk) {
                    EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
                    
                    if (string.IsNullOrEmpty(pathPackage)) {
                        output = string.Format("{0}/{1}_{2}_{3:yyyyMMdd}.apk", EditorHelper.PackageDir, PlayerSettings.productName, LogicConstantData.g_version, DateTime.Now);
                        //output = string.Format("{0}/{1}_{2}_{3:yyyyMMdd_HHmmss}.apk", EditorHelper.PackageDir, PlayerSettings.productName, LogicConstantData.g_version, DateTime.Now);
                    } else {
                        output = pathPackage;
                    }
                } else {
                
                    EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
                    
                    options |= BuildOptions.AcceptExternalModificationsToPlayer;
                    output = string.Format("{0}/{1}", EditorHelper.OutputDir, PlayerSettings.productName);
                    //SaveAndroidConfig(output);
                }
                break;
            case BuildTarget.iOS:
                options |= BuildOptions.AcceptExternalModificationsToPlayer;
                if (string.IsNullOrEmpty(pathProject)) {
                    output = EditorHelper.OutputDir;
                } else {
                    output = pathProject;
                }
                break;
        }
        
        BuildPipeline.BuildPlayer(scenes.ToArray(), output, EditorUserBuildSettings.activeBuildTarget, options);
        
        switch (EditorUserBuildSettings.activeBuildTarget) {
            case BuildTarget.StandaloneWindows:
                // 压缩成zip包
                string pathSrc = output.Substring(0, output.LastIndexOf('/'));
                string pathDst = pathPackage;
                if (string.IsNullOrEmpty(pathDst)) {
                    pathDst = string.Format("{0}/{1}_{2}_{3:yyyyMMdd_HHmmss}.zip", EditorHelper.PackageDir, PlayerSettings.productName, LogicConstantData.g_version, DateTime.Now);
                }
                
                ZipHelper.Pack(pathSrc, pathDst);
                break;
        }
        UnityEngine.Debug.Log(output);
    }
    
    /*static void SaveAndroidConfig(string output)
    {
        // 版本号
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendFormat("VERSION_NAME={0}\n", PlayerSettings.bundleVersion);
        sb.AppendFormat("VERSION_CODE={0}\n", PlayerSettings.Android.bundleVersionCode);
        sb.AppendFormat("BUNDLE_IDENTIFIER={0}\n", PlayerSettings.applicationIdentifier);
    
        string path = string.Format("{0}/{1}/version.properties", output, PlayerSettings.productName);
        IGG.FileUtil.SaveTextToFile(sb.ToString(), path);
    }*/
    
    private static void WritePCVersionSvnCmd(string AppName, string StrVersion, string VersionFlag)
    {
    
        string VersionFileName = AppName + "_" + StrVersion + "_" + VersionFlag + ".rar";
        FileHelper.CreateFileDirectory("C:/Shell");
        FileStream stream = new FileStream("C:/Shell/SvnPCVersion.bat", FileMode.Create);
        StreamWriter file = new StreamWriter(stream);
        file.WriteLine("@echo off");
        string str = "svn update \"D:\\Version\"";
        file.WriteLine(str);
        
        str = "copy /y c:\\zero\\PC\\" + VersionFileName ;
        str += " D:\\Version\\" + StrVersion + "\\client\\windows\\";
        file.WriteLine(str);
        
        str = "svn add \"D:\\Version\\" + StrVersion + "\\client\\windows\\" + VersionFileName + "\"";
        file.WriteLine(str);
        
        
        str = "svn commit -m  \"Add pc version\" \"D:\\Version\\" + StrVersion + "\\client\\windows\\" + VersionFileName + "\"";
        file.WriteLine(str);
        
        
        file.Close();
        stream.Close();
        UnityEngine.Debug.Log("WritePCVersionSvnCmd");
    }
    
    private static void WriteAndroidShellCmd(string AppPath, string strVersionUpdate)
    {
    
        FileHelper.CreateFileDirectory("C:/Shell");
        FileStream stream = new FileStream("C:/Shell/Upload.sh", FileMode.Create);
        StreamWriter file = new StreamWriter(stream);
        file.WriteLine("#!/bin/sh");
        string str = "curl -F \"file=@" + AppPath + "\"" + " \\";
        file.WriteLine(str);
        str = " -F \"installType=2\"" + " \\";
        file.WriteLine(str);
        str = " -F \"password=igg\"" + " \\";
        file.WriteLine(str);
        str = " -F \"updateDescription=" + strVersionUpdate + "\"" + " \\";
        file.WriteLine(str);
        str = " -F \"uKey=e872c79cb97ae2f178899adbd6baede7\"" + " \\";
        file.WriteLine(str);
        str = " -F \"_api_key=0a75302daafd33c5f75f3fce2635284a\"";
        str += " https://www.pgyer.com/apiv1/app/upload";
        file.WriteLine(str);
        
        file.Close();
        stream.Close();
        UnityEngine.Debug.Log("ShellCmd Finish");
    }
    
    
    private static void WriteAndroidVersionSvnCmd(string AppName, string StrVersion, string VersionFlag)
    {
    
        string VersionFileName = AppName + "_" + StrVersion + "_" + VersionFlag + ".apk";
        FileHelper.CreateFileDirectory("C:/Shell");
        FileStream stream = new FileStream("C:/Shell/SvnAndroidVersion.bat", FileMode.Create);
        StreamWriter file = new StreamWriter(stream);
        file.WriteLine("@echo off");
        string str = "svn update \"D:\\Version\"";
        file.WriteLine(str);
        
        str = "copy /y c:\\zero\\Android\\" + VersionFileName ;
        str += " D:\\Version\\" + StrVersion + "\\client\\android\\";
        file.WriteLine(str);
        
        str = "svn add \"D:\\Version\\" + StrVersion + "\\client\\android\\" + VersionFileName + "\"";
        file.WriteLine(str);
        
        
        str = "svn commit -m  \"Add android version\" \"D:\\Version\\" + StrVersion + "\\client\\android\\" + VersionFileName + "\"";
        file.WriteLine(str);
        
        
        file.Close();
        stream.Close();
        UnityEngine.Debug.Log("WriteAndroidVersionSvnCmd");
    }
}
