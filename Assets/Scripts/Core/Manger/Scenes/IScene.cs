using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 场景载入、资源清理接口
/// </summary>
/// <author>zhulin</author>
public abstract class IScene
{
    protected AsyncOperation async;
    public float LoadingProgress {
        get{return async.progress;}
    }
    
    public AsyncOperation AsyncLoading {
        get{return async;}
    }
    
    /// <summary>
    /// 获取场景类型
    /// </summary>
    public static string GetSceneName()
    {
        return "IScene";
    }
    
    
    public virtual string SceneName()
    {
        return IScene.GetSceneName();
    }
    
    
    /// <summary>
    /// 资源载入入口
    /// </summary>
    /// <param name="data">传入的参数，默认为null</param>
    /// <returns></returns>
    public abstract IEnumerator Load(SceneData data);
    
    /// <summary>
    /// 准备载入场景
    /// </summary>
    public abstract void PrepareLoad();
    
    /// <summary>
    /// 资源卸载
    /// </summary>
    public abstract void Clear();
    
    /// <summary>
    /// 是否已经载入完成
    /// </summary>
    public virtual bool IsEnd()
    {
        if (async != null) {
            if (async.isDone || async.progress >= 0.889f) {
                return true;
            }
            return false;
        } else {
            return false;
        }
    }
    
    /// <summary>
    /// 场景Awake
    /// </summary>
    public virtual void Awake()
    {
    
    }
    /// <summary>
    /// 场景start 函数
    /// </summary>
    public abstract void Start();
    /// <summary>
    /// 接管场景中关注对象的Update
    /// </summary>
    public abstract void Update(float deltaTime);
    
    
    /// <summary>
    /// 接管场景中关注对象的LateUpdate
    /// </summary>
    public abstract void LateUpdate(float deltaTime) ;
    
    /// <summary>
    /// 接管场景中关注对象的FixedUpdate
    /// </summary>
    public abstract void FixedUpdate(float deltaTime) ;
    
    /// <summary>
    /// 构建场景数据
    /// </summary>
    public virtual void BuildScene()
    {
        BuildWorld();
        BuildUI();
        
    }
    
    /// <summary>
    /// 构建UI
    /// </summary>
    public virtual void BuildUI()
    {
    
    }
    
    /// <summary>
    /// 构建世界空间
    /// </summary>
    public virtual void BuildWorld()
    {
    
    }
}

public class SceneData
{
    public int State;    //状态。
}
