using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 资源工厂
/// </summary>
public class ResFactory : MonoBehaviour
{


    public virtual void Start()
    {
        DontDestroyOnLoad(gameObject);
        // 资源更新
        InitResVersionUpdate();
        // 资源加载
        InitResManger();
    }
    
    private void InitResVersionUpdate()
    {
        /*#if UNITY_EDITOR
        		gameObject.AddComponent<EditorVersionResource>();
        #elif UNITY_STANDALONE_WIN
        		gameObject.AddComponent<PCVersionResource>();
        #else
        		gameObject.AddComponent<MobileVersionResource>();
        #endif
        */
        
#if UNITY_EDITOR
        gameObject.AddComponent<EditorVersionResource>();
#else
        gameObject.AddComponent<PCVersionResource>();
#endif
    }
    
    
    private void InitResManger()
    {
        AssetLoad l = null;
#if UNITY_EDITOR
        l = new AssetLoadEditor();
#else
        l = new AssetLoadRun();
#endif
        ResourceManger.SetLoadObj(this, l);
        ResourceManger.InitCache();
        App.Init();
    }
}
