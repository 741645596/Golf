using UnityEngine;
using System;
using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using IGG.Core.Helper;


public class MobileVersionResource : MonoBehaviour
{

    private ABResource m_abResource = new ABResource();
    private ResVersionInfo m_CurResversionData = null;
    private ResVersionInfo m_NewResversionData = null;
    private int m_totalDownLoad = 0;
    
    void Start()
    {
        FileHelper.CreateFileDirectory(ConstantData.ABSavePath);
        GetResVersion();
    }
    
    
    private void GetResVersion()
    {
        /*m_CurResversionData = ResVersion.Analyse(Application.persistentDataPath + "/" + ResVersion.GetResVersionFileName());
        DownLoadQueue.SetFinishNotify(DownLoadResVersionFinish);
        DownLoadTask task = new DownLoadTask(ConstantData.ABServerPath,
            Application.persistentDataPath + "/",
            ResVersion.GetResVersionFileName(),
            DownLoadfileProgress);
        DownLoadQueue.AddDownTask(task);
        DownLoadQueue.StartDown();*/
    }
    
    
    
    
    
    
    
    private void DownLoadResVersionFinish(string ResourceName, int UnDownLoadNum)
    {
        /* DownLoadQueue.EmptyTask();
         // 读取版本在辕文件
         m_NewResversionData = ResVersion.Analyse(Application.persistentDataPath + "/" + ResVersion.GetResVersionFileName());
         //
         if (m_NewResversionData == null) {
             joinGame();
         } else {
             string AbUpdateResFile = ResVersion.GetABResUpdateFile(m_CurResversionData, m_NewResversionData);
             if (AbUpdateResFile != string.Empty) {
                 DownLoadQueue.SetFinishNotify(DownLoadResContextFinish);
                 DownLoadTask task = new DownLoadTask(ConstantData.ABServerPath,
                     Application.persistentDataPath + "/",
                     AbUpdateResFile,
                     DownLoadfileProgress);
                 DownLoadQueue.AddDownTask(task);
                 DownLoadQueue.StartDown();
             } else {
                 joinGame();
             }
         }*/
    }
    
    private void DownLoadfileProgress(int downid, string ResourceName, float progress)
    {
        float fprogress = progress * 100;
        Scheduler.loading.PlayDownLoad((int)fprogress, ResourceName);
    }
    
    
    
    
    private void DownLoadResContextFinish(string ResourceName, int UnDownLoadNum)
    {
        DownLoadQueue.EmptyTask();
        StartCoroutine(UnzipWithPath(Application.persistentDataPath + "/" + ResourceName, Application.persistentDataPath + "/ab"));
    }
    
    private void ReadABResourceInfo()
    {
        try {
            FileStream fsFile = new FileStream(ConstantData.ABSavePath + ABResource.GetResourceFileName(), FileMode.Open);
            StreamReader srReader = new StreamReader(fsFile);
            string sLine = srReader.ReadToEnd();
            srReader.Dispose();
            //Debug.Log(sLine);
            m_abResource = JsonUtility.FromJson<ABResource>(sLine);
            fsFile.Close();
        } catch (Exception e) {
            Debug.Log(e.ToString());
        }
    }
    
    
    
    // 转意网络路径
    private string GetABdir(string GetABdir)
    {
        string str = GetABdir;
        if (ConstantData.g_VerContent == VerContent.FullVersion) {
            return str;
        } else {
            return str.Replace("/", "%2F");
        }
    }
    
    
    
    
    
    private void joinGame()
    {
        LoadDependeAB();
        DownLoadServerList();
    }
    
    
    private void LoadDependeAB()
    {
    
        ReadABResourceInfo();
        foreach (ABResourceUnit unit in m_abResource.listABUnit) {
            if (unit.ABdir == ResourcesPath.GetRelativePath(ResourcesType.Shader, ResourcesPathMode.AssetBundle)
            ) {
                foreach (ABFileUnitInfo ABfileUnit in unit.listAb) {
                    ResourceManger.LoadDependeAB(unit.ABdir, ABfileUnit.ABfileName);
                }
            }
        }
        Shader.WarmupAllShaders();
    }
    
    
    // 下载服务器列表
    private void DownLoadServerList()
    {
        DownLoadQueue.SetFinishNotify(DownLoadServerListFinish);
        DownLoadTask task = new DownLoadTask(ConstantData.ServerListServerPath,
            ConstantData.ServerListSavePath,
            ConstantData.ServerListFile, null);
        DownLoadQueue.AddDownTask(task);
        DownLoadQueue.StartDown();
    }
    
    // 下载服务器列表完成
    private void DownLoadServerListFinish(string ResourceName, int UnDownLoadNum)
    {
        if (ResourceName == ConstantData.ServerListFile) {
            // 防止scene ab 未加载完成就切场景，照成卡顿
            Invoke("loadScene", 0.5f);
        }
    }
    
    private void loadScene()
    {
        LoginM.LoadServerListFromText(ConstantData.ServerListSavePath + ConstantData.ServerListFile);
        SceneM.Load(LoginScene.GetSceneName());
    }
    
    public IEnumerator UnzipWithPath(string path, string dirPath)
    {
        ZipEntry zip = null;
        //输入的所有的文件流都是存储在这里面的
        ZipInputStream zipInStream = null;
        //读取文件流到zipInputStream
        zipInStream = new ZipInputStream(File.OpenRead(path));
        //循环读取Zip目录下的所有文件
        
        while ((zip = zipInStream.GetNextEntry()) != null) {
            UnzipFile(zip, zipInStream, dirPath);
            yield return new WaitForEndOfFrame();
        }
        try {
            zipInStream.Close();
        } catch (Exception ex) {
            Debug.Log("UnZip Error");
            throw ex;
        }
        
        File.Delete(path);
        joinGame();
    }
    
    private void UnzipFile(ZipEntry zip, ZipInputStream zipInStream, string dirPath)
    {
        long total = zip.Size;
        long readtotal = 0;
        try {
            //文件名不为空
            if (!string.IsNullOrEmpty(zip.Name)) {
                string filePath = dirPath;
                filePath += ("/" + zip.Name);
                
                //如果是一个新的文件路径　这里需要创建这个文件路径
                if (IsDirectory(filePath)) {
                    Debug.Log("Create  file paht " + filePath);
                    if (!Directory.Exists(filePath)) {
                        Directory.CreateDirectory(filePath);
                    }
                } else {
                    FileStream fs = null;
                    //当前文件夹下有该文件  删掉  重新创建
                    if (File.Exists(filePath)) {
                        File.Delete(filePath);
                    }
                    fs = File.Create(filePath);
                    int size = 2048;
                    byte[] data = new byte[2048];
                    //每次读取2MB  直到把这个内容读完
                    while (true) {
                        size = zipInStream.Read(data, 0, data.Length);
                        readtotal += size;
                        float progress = readtotal * 1.0f / total * 100;
                        //   Scheduler.loading.Playzip((int)progress, zip.Name);
                        //小于0， 也就读完了当前的流
                        if (size > 0) {
                            fs.Write(data, 0, size);
                            
                        } else {
                            break;
                        }
                    }
                    fs.Close();
                }
            }
        } catch (Exception e) {
            throw new Exception();
        }
    }
    
    
    /// <summary>
    /// 判断是否是目录文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    static bool IsDirectory(string path)
    {
    
        if (path[path.Length - 1] == '/') {
            return true;
        }
        return false;
    }
}
