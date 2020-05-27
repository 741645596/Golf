using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using IGG.Core.Manger.Sound;
using IGG.Core.Manger.Coroutine;
using IGG.Core;
using IGG.Core.Data.DataCenter.Battle;
using IGG.Core.Data.DataCenter.GolfAI;
/// <summary>
/// Author  linhong
/// Date    2019.5.8
/// Desc    BattleMainWnd界面交互逻辑
/// </summary>
public class BattleMainWnd : WndBase {

    public Button m_btnRing;
    public Button m_kickBall;
    //public Button m_btnPlay;
    public Image m_imgWindArrow;
    public GameObject m_imgWind;
    public Button m_btnGolf;

    // public Button m_btnGolf3d;
    private WndBase m_wndMineBall;
    private Camera m_mainCamera;
    private GameObject m_golfCamera;
    private GameObject m_objRing = null;

   
    
    
    // 窗口初始华
    public override void InitWnd()
    {
        RegisterHooks();

        if (m_objRing == null)
        {
            m_objRing = GameObject.Find("Ring");
            //if (m_objRing != null)
            //{ m_objRing.SetActive(false); }
        }
        if(m_btnRing != null)
        {
            m_btnRing.gameObject.SetActive(false);
        }
        JudgeGround();
    }

    private void JudgeGround()
    {
        if(BattleM.BallInAra == AreaType.PuttingGreen)
        {
            KickBall();
        }
        else
        {
            RingDrag();
        }
    }

    // 窗口内事件绑定
    protected override void BindEvents()
    {
        if (m_btnRing != null)
        { m_btnRing.onClick.AddListener(RingDrag); }
        if (m_kickBall != null)
        { m_kickBall.onClick.AddListener(KickBall); }
        if (m_btnGolf != null)
        { m_btnGolf.onClick.AddListener(GolfRotate); }
       
    }
    private void RingDrag()
    {
        if (m_btnRing != null&& m_kickBall != null)
        {
            m_btnRing.gameObject.SetActive(false);
            m_kickBall.gameObject.SetActive(true);
        }
     
     
        //if (m_objRing != null)
        //{
        //    m_objRing.SetActive(true);
        //}
        EventCenter.DispatchEvent(EventCenterType.Battle_SkyView, -1,null);
        WndManager.DestoryWnd<SwingModeWnd>();
    }
    private void KickBall()
    {
       
        if (m_btnRing != null && m_kickBall != null)
        {
            m_btnRing.gameObject.SetActive(true);
            m_kickBall.gameObject.SetActive(false);
        }
       
        //if (m_objRing != null)
        //{
        //    m_objRing.SetActive(false);
        //}
        EventCenter.DispatchEvent(EventCenterType.Battle_SightView, -1, null);
        WndManager.CreateWnd<SwingModeWnd>(WndType.NormalWnd, false, false, null);
    }
    private void GreenBall()
    {
        if (m_btnRing != null && m_kickBall != null)
        {
            m_btnRing.gameObject.SetActive(false);
            m_kickBall.gameObject.SetActive(false);
        }
        EventCenter.DispatchEvent(EventCenterType.Battle_SightView, -1, null);
        WndManager.CreateWnd<SwingModeWnd>(WndType.NormalWnd, false, false, null);
    }
    private void GolfRotate()
    {
       
        if (m_wndMineBall == null)
        {
            WndManager.CreateWnd<MineBallSpinWnd>(WndType.MenuWnd, false, false, (wnd) =>
           {
               if (wnd != null)
               {
                   m_wndMineBall = wnd;
               }
           });
        }
        else
        {
            m_wndMineBall.gameObject.SetActive(true);
        }
       
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
      //  EventCenter.RegisterHooks(EventCenterType.Battle_NewRoundStart, OnBattle_NewRoundStart);
    }

    // 反注册消息函数
    private void AntiRegisterHooks()
    {
       // EventCenter.AntiRegisterHooks(EventCenterType.Battle_NewRoundStart, OnBattle_NewRoundStart);
    }

    // 反注册消息函数
    public virtual void OnDestroy()
    {
        AntiRegisterHooks();
    }



}
