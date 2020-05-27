// 一些编译开关配置
using UnityEngine;
using IGG.Core.Data.Config;
using IGG.Core.Data.DataCenter;
using IGG.Core.Module;


/// <summary>
/// 存放客户端应用程序的一些相关信息
/// </summary>
public class App
{

    // 初始化处理
    public static void Init()
    {
        // 不锁屏
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        // 登记所有的场景处理器
        RegisterScene();
        // TODO
        // 逻辑待补充
        ConfigMgr.LoadAll();
        ModuleMgr.InitAllModule();
        
    }
    
    
    // 登记所有的场景处理器
    private static void RegisterScene()
    {
        SceneM.RegisterScene(LauchScene.GetSceneName(), new LauchScene());
        SceneM.RegisterScene(LoginScene.GetSceneName(), new LoginScene());
        SceneM.RegisterScene(BattleScene.GetSceneName(), new BattleScene());
    }
}
