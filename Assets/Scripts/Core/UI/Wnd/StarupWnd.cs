using UnityEngine.UI;
using IGG.Core.Helper;
using UnityEngine;
using IGG.Core.Data.Config;
using IGG.Core.Data.DataCenter.SimServer;
using IGG.Core.Manger.Sound;
using System.Collections;
using UnityEngine.SceneManagement;

// StarupWnd 窗口 by zhulin
public class StarupWnd : WndBase
{
    public Button m_btnBg;
    public GameObject m_objPanel_loading;

    //public GameObject m_objTeamSetingBG;

    public override void Awake()
    {

    }
    
    // 窗口初始华
    public override void InitWnd()
    {
        RegisterHooks();
        SoundManager.PlayMusic(1);
    }
    
    // 窗口内事件绑定
    protected override void BindEvents()
    {
        m_btnBg.onClick.AddListener(BtnBgClick);
    }
    //背景按钮
    public void BtnBgClick()
    {
        m_objPanel_loading.SetActive(true);
        StartCoroutine(DestorySelfWnd(2.5f));
    }
    
    private IEnumerator DestorySelfWnd(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        //LoginM.Init(null);
        //LoginM.ConnectLoginServer();
        SceneM.Load(BattleScene.GetSceneName());
        WndManager.DestoryWnd<StarupWnd>();
    }
    
    // 显示窗口时播放动画
    public override void PlayShowAni()
    {
    }
    
    // 关闭窗口时播放动画
    public override void PlayCloseAni()
    {
    }
    
    // 注册消息函数
    private void RegisterHooks()
    {
    }
    
    // 反注册消息函数
    private void AntiRegisterHooks()
    {
    }
    
    // 反注册消息函数
    public virtual void OnDestroy()
    {
        AntiRegisterHooks();
    }
}
