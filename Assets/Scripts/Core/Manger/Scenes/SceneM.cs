using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using IGG.Core.Manger.Coroutine;

/// <summary>
/// 场景管理器
/// </summary>
/// <author>zhulin</author>
public class SceneM
{
    // 前一个场景
    private static IScene s_preScene = null;
    
    // 准备载入的场景
    private static IScene s_loadingIScene = null;
    
    private static bool bLoading = false;
    // 当前场景
    private static IScene s_CurIScene = null;
    
    // 缓存
    private static WaitForEndOfFrame g_wait = new WaitForEndOfFrame();
    
    // 场景载入器映射表
    private static Dictionary<string, IScene> sceneLoadControl = new Dictionary<string, IScene>();
    
    
    public static GameObject Schedulergo = null;
    
    //获取当前场景
    public static IScene GetCurIScene()
    {
        return s_CurIScene;
    }
    
    /// <summary>
    /// 当期那是否正在载入场景
    /// </summary>
    public static bool IsLoading { get { return bLoading; } }
    
    
    public static IScene GetLoadingIScene()
    {
        return s_loadingIScene;
    }
    
    /// <summary>
    /// 登记场景处理器
    /// </summary>
    public static void LinkScheduler(GameObject go)
    {
        Schedulergo = go;
    }
    /// <summary>
    /// 登记场景处理器
    /// </summary>
    public static void RegisterScene(string name, IScene scene)
    {
        sceneLoadControl[name] = scene;
    }
    
    /// <summary>
    /// 载入场景
    /// </summary>
    /// <param name="sceneName">待载入的场景名称</param>
    /// <param name="data">进入场景的传入数据<see cref="ILoading"/></param>
    /// <param name="sync">通过异步(false)方式还是同步(true)方式载入</param>
    /// <param name="force">是否需要保证载入成功(如果当前正在载入某个场景中，会仍到队列而不是直接失败)</param>
    /// <returns>成功处理则返回true</returns>
    public static bool Load(string sceneName, SceneData data = null, bool sync = false,  bool force = false)
    {
        if (bLoading == true) {
            return false;
        }
        // 如果当前正处于目标场景，无视
        // 任务副本需要做特殊处理
        IScene scene = sceneLoadControl[sceneName];
        // 标记正在载入场景
        s_loadingIScene = scene;
        bLoading = false;
        if (scene == s_CurIScene) {
            return true;
        }
        bLoading = true;
        // 异步载入方式
        if (!sync) {
            ResourceManger.AsyncGo.StartCoroutine(NonsyncLoad(sceneName, data));
            return true;
        }
        // 取得载入器
        IScene preScene = s_CurIScene;
        s_CurIScene = scene;
        
        // 同步载入(不会播放任何动画)
        try {
            if (null != preScene) {
                preScene.Clear();
            }
            
            // // 欲载入的处理
            scene.PrepareLoad();
            
            
            // 2 载入新的场景
            scene.Load(data);
            
            // 3 数据切换回来
            s_preScene = preScene;
            
            
            scene.BuildScene();
            scene.Start();
            
        } catch (Exception e) {
            IGGDebug.Log(e.ToString());
        }
        
        
        // 载入完毕，进行垃圾回收
        bLoading = false;
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        return true;
    }
    
    
    
    // 异步载入场景
    private static IEnumerator NonsyncLoad(string sceneName, SceneData data)
    {
        // 取得载入器
        IScene scene = sceneLoadControl[sceneName];
        s_preScene = s_CurIScene;
        s_loadingIScene = scene;
        
        
        if (s_preScene != null) {
            s_preScene.Clear();
        }
        // 1 播放开始载入的动画，并等待其播放完毕
        ILoading loading = null;
        WndManager.CreateWnd<LoadingWnd>(WndType.NormalWnd, false, false, (wnd) => {
            loading = (wnd as LoadingWnd);
            loading.Load();
        });
        yield return Yielders.GetWaitForSeconds(0.5f);
        
        // 欲载入的处理
        scene.PrepareLoad();
        // 3 载入新的场景
        scene.Load(data);
        
        // 等待loading入场动作，设定2秒钟哦。正常是不会这样处理的。
        float waitTime = 0;
        float totalWaitTime = 1.5f;
        
        int displayProgress = 0;
        int toProgress = 0;
        AsyncOperation op = scene.AsyncLoading;
        if (scene.AsyncLoading != null) {
            // 等待载入完成
            while (!scene.IsEnd()) {
                toProgress = (int)(op.progress * 100);
                while (displayProgress < toProgress) {
                    waitTime += Time.deltaTime;
                    ++displayProgress;
                    if (loading != null) {
                        loading.Play(displayProgress);
                    }
                    yield return g_wait;
                }
            }
            toProgress = 100;
            while (displayProgress < toProgress) {
                waitTime += Time.deltaTime;
                ++displayProgress;
                if (loading != null) {
                    loading.Play(displayProgress);
                }
                yield return g_wait;
            }
        }
        // 场景载入完成(一定要在load完毕后才能替换，此时的环境才能称得上载入完成)
        s_CurIScene = scene;
        //
        scene.Awake();
        //
        while (waitTime < totalWaitTime) {
            waitTime += Time.deltaTime;
            yield return g_wait;
        }
        
        if (loading != null) {
            loading.TryDestroy();
        }
        
        bLoading = false;
        
        yield return Yielders.GetWaitForSeconds(0.5f);
        //载入完成。
        scene.BuildScene();
        scene.Start();
        // 载入完成
    }
    // 当前场景Update
    public static void Update(float deltaTime)
    {
        if (s_CurIScene != null && s_CurIScene.IsEnd() == true) {
            s_CurIScene.Update(deltaTime);
        }
    }
    
    
    // 当前场景LateUpdate
    public static void LateUpdate(float deltaTime)
    {
        if (s_CurIScene != null && s_CurIScene.IsEnd() == true) {
            s_CurIScene.LateUpdate(deltaTime);
        }
    }
    
    
    
    
    // 当前场景LateUpdate
    public static void FixedUpdate(float deltaTime)
    {
        if (s_CurIScene != null && s_CurIScene.IsEnd() == true) {
            s_CurIScene.FixedUpdate(deltaTime);
        }
    }
    
    
}

/// <summary>
/// Loading动画接口
/// </summary>
/// <author>weism</author>
public interface ILoading
{
    /// <summary>
    /// 播放准备加载的动画
    /// </summary>
    void Play(int Progress);
    
    /// <summary>
    /// 播放加载中的动画
    /// </summary>
    void Load();
    /// <summary>
    /// 播放结束后尝试回收loading资源
    /// </summary>
    void TryDestroy();
    
}




