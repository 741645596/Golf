using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using IGG.Core.Manger.Sound;
using IGG.Core.Manger.Coroutine;

// MainfunctionWnd 窗口 by zhulin
public class MainfunctionWnd : WndBase
{
    //窗口动画
    private Animator m_aniWnd = null;
    //按钮状态切换
    public Animator m_aniBtnPage;
    //主页
    public Button m_btnHomePage;
    //public Animator m_aniHomePage;
    //英雄
    public Button m_btnHeroPage;
    //public Animator m_aniHeroPage;
    //队伍
    public Button m_btnTeamPage;
    //public Animator m_aniTeamPage;
    //主城
    public Button m_btnCityPage;
    //public Animator m_aniCityPage;
    //公会
    public Button m_btnClanPage;
    //public Animator m_aniClanPage;
    //召唤
    public Button m_btnCallPage;
    //public Animator m_aniCallPage;
    
    //窗口销毁时间延迟
    private  float m_delayTimeDestoryWnd = 0.7f;    // old：0.7f
    //窗口开启延迟
    private  float m_delayTimeOpenWnd = 0.4f;       // old：0.4f
    
    private EPage m_ePage = EPage.None;
    public enum EPage {
        None,
        HomePage,
        HeroPage,
        TeamPage,
        CityPage,
        ClanPage,
        CallPage
    }
    
    // 窗口初始华
    public override void InitWnd()
    {
        RegisterHooks();
        //m_aniWnd = GetComponent<Animator>();
        //ClickHomePage();
        
        //初始化 第一次开启功能列
        //m_aniBtnPage.SetBool("Tag_1", true);
        //m_ePage = EPage.HomePage;
        //WndManager.CreateWnd<HomepageWnd>(WndType.NormalWnd, false, false, null);
        InitPage();
    }
    
    public void InitPage()
    {
        m_aniBtnPage.SetBool("Tag_1", true);
        m_ePage = EPage.HomePage;
    }
    
    // 窗口内事件绑定
    protected override void BindEvents()
    {
        m_btnHomePage.onClick.AddListener(ClickHomePage);
        m_btnHeroPage.onClick.AddListener(ClickHeroPage);
        m_btnTeamPage.onClick.AddListener(ClickTeamPage);
        m_btnCityPage.onClick.AddListener(ClickCityPage);
        m_btnClanPage.onClick.AddListener(ClickClanPage);
        m_btnCallPage.onClick.AddListener(ClickCallPage);
    }
    
    private void ClickHomePage()
    {
        
        //先去判断 在去关闭界面
        bool bOpenDelay = IsOpenDelay();
        closeSelWnd();
        //closeSelBtnAnimator();
        //WndManager.CreateWnd<HomepageWnd>(WndType.MenuWnd, false, false, null);
        StartCoroutine(OpenPanel(EPage.HomePage, bOpenDelay ? m_delayTimeOpenWnd : 0.0f));
        //if (null != m_aniHomePage) {
        //    m_aniHomePage.SetBool("Selected", true);
        //}
        m_aniBtnPage.SetBool("Tag_1", true);
        m_ePage = EPage.HomePage;
    }
    
    private void ClickHeroPage()
    {

    }
    
    private void ClickTeamPage()
    {

    }
    
    private void ClickCityPage()
    {
        //var varWnd = WndManager.FindWnd<TeamsettingWnd>();
        if (m_ePage == EPage.CityPage) {
            return;
        }
        
        bool bOpenDelay = IsOpenDelay();
        closeSelWnd();
        //closeSelBtnAnimator();
        //WndManager.CreateWnd<TeamsettingWnd>(WndType.MenuWnd, false, false, null);
        StartCoroutine(OpenPanel(EPage.CityPage, bOpenDelay ? m_delayTimeOpenWnd : 0.0f));
        //m_aniCityPage.SetBool("Selected", true);
        m_aniBtnPage.SetBool("Tag_4", true);
        m_ePage = EPage.CityPage;
    }
    
    private void ClickClanPage()
    {
        //var varWnd = WndManager.FindWnd<TeamsettingWnd>();
        if (m_ePage == EPage.ClanPage) {
            return;
        }
        
        bool bOpenDelay = IsOpenDelay();
        closeSelWnd();
        //closeSelBtnAnimator();
        //WndManager.CreateWnd<TeamsettingWnd>(WndType.MenuWnd, false, false, null);
        StartCoroutine(OpenPanel(EPage.ClanPage, bOpenDelay ? m_delayTimeOpenWnd : 0.0f));
        //m_aniClanPage.SetBool("Selected", true);
        m_aniBtnPage.SetBool("Tag_5", true);
        m_ePage = EPage.ClanPage;
    }
    
    private void ClickCallPage()
    {
        bool bOpenDelay = IsOpenDelay();
        closeSelWnd();
        //closeSelBtnAnimator();
        //WndManager.CreateWnd<LotteryWnd>(WndType.NormalWnd, false, false, null);
        StartCoroutine(OpenPanel(EPage.CallPage, bOpenDelay ? m_delayTimeOpenWnd : 0.0f));
        //m_aniCallPage.SetBool("Selected", true);
        m_aniBtnPage.SetBool("Tag_6", true);
        m_ePage = EPage.CallPage;
    }
    
    //private void closeSelBtnAnimator() {
    //    if (null == m_aniHomePage) {
    //        return;
    //    }
    //    m_aniHomePage.SetBool("Selected", false);
    //    m_aniHeroPage.SetBool("Selected", false);
    //    m_aniTeamPage.SetBool("Selected", false);
    //    m_aniCityPage.SetBool("Selected", false);
    //    m_aniClanPage.SetBool("Selected", false);
    //    m_aniCallPage.SetBool("Selected", false);
    //}
    private void closeSelWnd()
    {
        //不用延迟关闭
        //WndManager.DestoryWnd<ChapterinfoWnd>(0.0f);
    }
    
    /// <summary>
    /// 开界面前是否需要延时
    /// </summary>
    /// <returns></returns>
    private bool IsOpenDelay()
    {

        
        return true;
    }
    
    private IEnumerator OpenPanel(EPage ePanelType, float delayTime)
    {
        SoundManager.PlayMusic(3);
        //yield return new WaitForSeconds(delayTime);
        yield return Yielders.GetWaitForSeconds(delayTime);
        closeSelWnd();
        switch (ePanelType) {
            case EPage.CityPage:
            
                break;
            case EPage.ClanPage:
            
                break;
                
            default:
                break;
        }
        
        //WndManager.CreateWnd<HerolistWnd>(WndType.NormalWnd, false, false, (wnd) => { });
    }
    
    // 显示窗口时播放动画
    public override void PlayShowAni()
    {
        if (null !=  m_aniWnd) {
            m_aniWnd.SetTrigger("Open");
        }
        
    }
    
    // 关闭窗口时播放动画
    public override void PlayCloseAni()
    {
        if (null != m_aniWnd) {
            m_aniWnd.SetTrigger("Close");
        }
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
