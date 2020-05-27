using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorResInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        InitResManger();
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
