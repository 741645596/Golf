using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EditorVersionResource : MonoBehaviour
{

    void Start()
    {
        StartGame();
    }
    
    
    private void StartGame()
    {
        joinGame();
    }
    
    
    private void joinGame()
    {
        LoginM.LoadServerListFromText(Application.dataPath + "/Config/" + ConstantData.ServerListFile);
        //SceneM.Load(LoginScene.GetSceneName(), false, Scheduler.loading, false);
        WndManager.CreateWnd<StarupWnd>(WndType.NormalWnd, false, false, (wnd) => { });
    }
}
