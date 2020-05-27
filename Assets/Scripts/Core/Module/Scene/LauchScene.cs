using UnityEngine;
using System.Collections;

/// <summary>
/// 入口场景
/// </summary>
/// <author>zhulin</author>
public class LauchScene : IScene
{
    public new static string GetSceneName()
    {
        return "Lauch";
    }
    
    public override string SceneName()
    {
        return LauchScene.GetSceneName();
    }
    /// <summary>
    /// 资源载入入口
    /// </summary>
    /// <param name="data">传入的参数，默认为null</param>
    /// <returns></returns>
    public override IEnumerator Load(SceneData data)
    {
        return null;
    }
    
    /// <summary>
    /// 准备载入场景
    /// </summary>
    public override void PrepareLoad()
    {
    
    }
    
    /// <summary>
    /// 资源卸载
    /// </summary>
    public override void Clear()
    {
    
    }
    
    public override void BuildUI()
    {
    }
    
    
    /// <summary>
    /// 构建世界空间
    /// </summary>
    public override void BuildWorld()
    {
    
    }
    
    /// <summary>
    /// 场景start 接口
    /// </summary>
    public override void Start()
    {
    
    }
    /// <summary>
    /// 接管场景中关注对象的Update
    /// </summary>
    public override void Update(float deltaTime)
    {
    
    }
    /// <summary>
    /// 接管场景中关注对象的LateUpdate
    /// </summary>
    public override void LateUpdate(float deltaTime)
    {
    
    }
    /// <summary>
    /// 接管场景中关注对象的FixedUpdate
    /// </summary>
    public override void FixedUpdate(float deltaTime)
    {
    
    }
    
    
}
