using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

public class PCVersionResource : MonoBehaviour
{

    private ABResource m_abResource = new ABResource();
    private static MD5CryptoServiceProvider g_md5Ger = new MD5CryptoServiceProvider();
    private int m_totalDownLoad = 0;
    
    void Start()
    {
        //IGG.FileUtil.CreateFileDirectory(ConstantData.ABSavePath);
        StartGame();
    }
    
    
    private void StartGame()
    {
        joinGame();
    }
    
    
    private void ReadABResourceInfo()
    {
        try {
            FileStream fsFile = new FileStream(ConstantData.ABSavePath + ABResource.GetResourceFileName(), FileMode.Open);
            StreamReader srReader = new StreamReader(fsFile);
            string sLine = srReader.ReadToEnd();
            srReader.Close();
            //Debug.Log(sLine);
            m_abResource = JsonUtility.FromJson<ABResource>(sLine);
        } catch (Exception e) {
            Debug.Log(e.ToString());
        }
    }
    
    
    
    
    
    private void joinGame()
    {
    
        //LoadDependeAB();
#if UNITY_EDITOR
        //LoginM.LoadServerListFromText(Application.dataPath + "/Config/" + ConstantData.ServerListFile);
#else
        //LoginM.LoadServerListFromText(Application.streamingAssetsPath + "/Config/" + ConstantData.ServerListFile);
#endif
        //SceneM.Load(LoginScene.GetSceneName(), false, Scheduler.loading, false);
        WndManager.CreateWnd<StarupWnd>(WndType.NormalWnd, false, false, (wnd) => { });
    }
    
    
    
    private void LoadDependeAB()
    {
        ReadABResourceInfo();
        foreach (ABResourceUnit unit in m_abResource.listABUnit) {
            if (unit.ABdir == ResourcesPath.GetRelativePath(ResourcesType.Scene, ResourcesPathMode.AssetBundle)
                || unit.ABdir == ResourcesPath.GetRelativePath(ResourcesType.Shader, ResourcesPathMode.AssetBundle)
            ) {
                foreach (ABFileUnitInfo ABfileUnit in unit.listAb) {
                    ResourceManger.LoadDependeAB(unit.ABdir, ABfileUnit.ABfileName);
                }
            }
        }
    }
    
}
