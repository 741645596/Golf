using IGG.Core;
using UnityEngine;
using IGG.Core.Data.DataCenter.GameTime;
using IGG.Core.Module;
using IGG.Core.Manger.Coroutine;

/// <summary>
/// 帧调度，本组件绑定到gameobject以发挥作用
/// </summary>
public class Scheduler : MonoBehaviour
{
    public bool ac = false;
    
    private static SceneLoading g_loading = null;
    
    public static SceneLoading loading {
        get { return g_loading; }
    }
    
    
    public virtual void Awake()
    {
        IGGString.Init();
        DontDestroyOnLoad(gameObject);
        //Application.targetFrameRate = 30;
        SceneM.LinkScheduler(gameObject);
        EventCenter.Init();
        NetCache.Init();
        protobufM.Init();
    }
    
    
    public virtual void Update()
    {
        try {
        
            if (Input.GetKeyDown(KeyCode.Escape) == true) {
                Application.Quit();
            }
            // 通讯消息
            Communicate.GSConnector.Update(Time.deltaTime);
            //场景数据调度
            SceneM.Update(Time.deltaTime);
            GameTimeDC.Update(Time.deltaTime);
            // 携程工具
            IGGCoroutine.Update();
            // 定时器工具
            Timer.Update();
        } catch (System.Exception e) {
            Debug.LogError(e);
        }
    }
    
    public virtual void LateUpdate()
    {
        SceneM.LateUpdate(Time.deltaTime);
    }
    
    public virtual void FixedUpdate()
    {
        SceneM.FixedUpdate(Time.deltaTime);
    }
    
    // 退出游戏的处理 临时
    bool allowQuit = false;
    void OnApplicationQuit()
    {
        if (allowQuit) {
            return;
        }
        ResourceManger.Clear();
        ModuleMgr.ClearAllModuleDC();
        EventCenter.Free();
        Application.Quit();
    }
    
    public static void SetLoading(SceneLoading loading)
    {
        g_loading = loading;
    }
}
