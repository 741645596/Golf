using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IGG.Core.Data.Config;
using IGG.Core.Utils;
using IGG.Core.Helper;
using IGG.Core.Manger.Sound;
using UnityEngine.SceneManagement;
using IGG.Core.Data.DataCenter.GolfAI;
using IGG.Core.Data.DataCenter.Battle;
using IGG.Core;

// <summary>
// 登录场景
// </summary>
// <author>zhulin</author>
public class BattleScene : IScene
{
    public new static string GetSceneName()
    {
        return "Battle";
    }

    public override string SceneName()
    {
        return BattleScene.GetSceneName();
    }

    /// <summary>
    /// 资源载入入口
    /// </summary>
    /// <param name="data">传入的参数，默认为null</param>
    /// <returns></returns>
    public override IEnumerator Load(SceneData data)
    {
        async = SceneManager.LoadSceneAsync(BattleScene.GetSceneName());
        return null;
    }

    /// <summary>
    /// 准备载入场景
    /// </summary>
    public override void PrepareLoad()
    {
        RegisterHooks();
    }

    /// <summary>
    /// 资源卸载
    /// </summary>
    public override void Clear()
    {
        WndManager.DestroyAllWnd();
        ResourceManger.ClearScenePools();
        BattleM.Clear();
        AntiRegisterHooks();
    }

    public override void BuildUI()
    {
        SoundManager.PlayMusic(3);

    }

    private void OnBattle_RoundStart(int sender, object Param)
    {
        if (BattleM.BallInAra == AreaType.PuttingGreen)
        {
            WndManager.CreateWnd<SwingModeWnd>(WndType.NormalWnd, false, false, null);
        }
        else
        {
            WndManager.CreateWnd<BattleMainWnd>(WndType.NormalWnd, false, false, null);
        }
    }
    /// <summary>
    /// 构建世界空间
    /// </summary>
    public override void BuildWorld()
    {
        BattleM.AddBattle(new Battle());
        BattleM.SetBattleStatus(BattleStatus.Init);
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
    // 注册消息函数
    private void RegisterHooks()
    {
       
        EventCenter.RegisterHooks(EventCenterType.Battle_RoundStart, OnBattle_RoundStart);
    }

    // 反注册消息函数
    private void AntiRegisterHooks()
    {
        EventCenter.AntiRegisterHooks(EventCenterType.Battle_RoundStart, OnBattle_RoundStart);
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
