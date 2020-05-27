using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IGG.Core.Data.Config;
using IGG.Core.Utils;
using IGG.Core.Helper;
using IGG.Core.Manger.Sound;
using UnityEngine.SceneManagement;
using System;

// <summary>
// 登录场景
// </summary>
// <author>zhulin</author>
public class LoginScene : IScene
{

    public EEnterLoginSceneType m_eType = EEnterLoginSceneType.eEnterLoginSceneType_None;
    public new static string GetSceneName()
    {
        return "Login";
    }
    
    public override string SceneName()
    {
        return LoginScene.GetSceneName();
    }
    
    /// <summary>
    /// 资源载入入口
    /// </summary>
    /// <param name="data">传入的参数，默认为null</param>
    /// <returns></returns>
    public override IEnumerator Load(SceneData data)
    {
    
        if (null != data) {
            m_eType = (EEnterLoginSceneType)data.State;
        }
        
        async = SceneManager.LoadSceneAsync(LoginScene.GetSceneName());
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
        WndManager.DestroyAllWnd();
        ResourceManger.ClearScenePools();
    }
    
    public override void BuildUI()
    {
        SoundManager.PlayMusic(3);
        WndManager.CreateWnd<BattleMainWnd>(WndType.MenuWnd, false, false, null);
        m_eType = EEnterLoginSceneType.eEnterLoginSceneType_None;
    }
    
    
    /// <summary>
    /// 构建世界空间
    /// </summary>
    public override void BuildWorld()
    {

    }
    
    
    
    /// <summary>
    /// 构建场景数据
    /// </summary>
    public override void BuildScene()
    {
        BuildWorld();
        BuildUI();
        
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
