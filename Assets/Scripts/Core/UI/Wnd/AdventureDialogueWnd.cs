using IGG.Core.Data.DataCenter;
using IGG.Core;
using UnityEngine.UI;
using IGG.Core.Helper;
using UnityEngine;
using IGG.Core.Data.Config;
using System.Collections.Generic;
using System.Collections;
using IGG.Core.Data.DataCenter.Dialog;
using UnityStandardAssets.ImageEffects;
using IGG.Core.Manger.Sound;
using IGG.Core.Manger.Coroutine;

/// <summary>
/// Author  zhulin
/// Date    2019.2.19
/// Desc    AdventureDialogueWnd界面交互逻辑
/// </summary>
public class AdventureDialogueWnd : WndBase
{

    /// <summary>
    /// 定义关联对象
    /// </summary>
    public Button m_btnNext;
    public Button m_btnAuto;
    public Button m_btnSkip;
    //
    public Animator m_WndAni;
    //
    public RawImage rawImage;
    public Animator m_btnAutoAni;
    // 英雄头像
    
    public Animator m_HeroAni1;
    public GameObject[] LeftAni;
    //public Image m_HeroIcon1;
    public Animator m_HeroAni2;
    public GameObject[] RightAni;
    //public Image m_HeroIcon2;
    // emoji
    public IggImage m_Emoji1;
    public IggImage m_Emoji2;
    // 对话
    public Animator m_DialogAni;
    public GameObject gDialog1;
    public GameObject gArrow1;
    public IggText txtDialogTitle1;
    public IggText txtDialogContent1;
    public GameObject gDialog2;
    public GameObject gArrow2;
    public IggText txtDialogTitle2;
    public IggText txtDialogContent2;
    public GameObject gDialog3;
    public GameObject gArrow3;
    public IggText txtDialogContent3;
    // 玩家选项
    public Animator m_OptionAni;
    public Transform tOptionParent;
    public GameObject OptionPrefab;
    // 玩家选项
    private List<GameObject> m_List = new List<GameObject>();
    private bool m_IsHaveOption = false;
    private Vector3 m_HeroPos1;
    private Vector3 m_HeroPos2;
    
    
    private SceneDialogueConfig m_curCofig;
    private uint m_dialogID = 0;
    private Blur m_blr = null;
    //开启等待时间
    public float waitOpenDialogTime = 0.1f;
    // 等待对白时间
    public float waitShowDialogTime = 0.3f;
    public float AutoTime = 0.5f;
    public float waitCloseDialogTime = 0.5f;
    
    private PlaySetp m_playStep = PlaySetp.start;
    private bool m_isClickNext = false;
    private uint m_PlayMusicID;
    private bool m_IsPlayOptionSeleteAni = false;
    
    // 调试模式，为测试用
    private bool m_TestMode = false;
    public bool TestMode {
        get { return m_TestMode; }
        set { m_TestMode = value; }
    }
    // <summary>
    // 加载完成立即初始化的操作
    // </summary>
    public override void Init()
    {
        m_Emoji1.gameObject.SetActive(false);
        m_Emoji2.gameObject.SetActive(false);
        //
        m_HeroPos1 = m_HeroAni1.GetComponent<RectTransform>().localPosition;
        m_HeroPos2 = m_HeroAni2.GetComponent<RectTransform>().localPosition;
        if (Camera.main != null) {
            m_blr = Camera.main.GetComponent<Blur>();
            if (m_blr != null) {
                rawImage.gameObject.SetActive(true);
                m_blr.SetRawImage(rawImage);
            } else {
                rawImage.gameObject.SetActive(false);
            }
        } else {
            rawImage.gameObject.SetActive(false);
        }
        HideAllAniGo();
    }
    
    /// <summary>
    /// 初始化窗口
    /// </summary>
    public override void InitWnd()
    {
        RegisterHooks();
        m_btnAutoAni.SetBool("Auto", DialogDC.AutoDialog);
        StartCoroutine(Play(null, m_curCofig, waitOpenDialogTime, false, true));
    }
    
    /// <summary>
    /// 打开窗口传参数
    /// </summary>
    /// <param name="obj"></param>
    public override void SetData(object obj)
    {
        m_dialogID = (uint)obj;
        m_curCofig = SceneDialogueDao.Inst.GetCfg(m_dialogID);
        m_PlayMusicID = SoundManager.PlayMusicID;
    }
    
    
    private IEnumerator PlayIn(SceneDialogueConfig cfg, bool showOption, bool isPrint, bool isSameNpc)
    {
        m_Emoji1.gameObject.SetActive(false);
        m_Emoji2.gameObject.SetActive(false);
        gDialog1.SetActive(false);
        gDialog2.SetActive(false);
        gDialog3.SetActive(false);
        gArrow1.SetActive(false);
        gArrow2.SetActive(false);
        gArrow3.SetActive(false);
        // 播放剧情背景音乐
        if (cfg != null && cfg.DialogueMusic != 0) {
            SoundManager.PlayMusic(cfg.DialogueMusic);
        } else {
            SoundManager.PlayMusic(m_PlayMusicID);
        }
        if (isSameNpc == false) {
            yield return PlayHeroIconAndDialogDialog(cfg);
            // emoji 打印机效果
            yield return PlayEmoji(cfg);
            // dialog 打印机效果
            yield return PlayDialogPrint(cfg, isPrint);
        } else {
            // dialog 打印机效果
            yield return PlayDialogPrint(cfg, isPrint);
        }
        
        // 玩家选项卡播放
        yield return PlayOption(cfg, showOption);
    }
    
    
    /// <summary>
    /// 绑定按钮事件
    /// </summary>
    protected override void BindEvents()
    {
        // 关联按钮事件示例
        m_btnNext.onClick.AddListener(BtnNextClick);
        m_btnAuto.onClick.AddListener(BtnAutoClick);
        m_btnSkip.onClick.AddListener(BtnSkipClick);
    }
    
    public void BtnNextClick()
    {
        if (m_IsHaveOption == true) {
            return;
        }
        //
        if (m_curCofig == null) {
            SoundManager.StopMusic(SoundType.dialog);
            WndManager.DestoryWnd<AdventureDialogueWnd>(0.5f + waitCloseDialogTime);
        } else {
            if (m_playStep == PlaySetp.end) {
                SceneDialogueConfig cfg = SceneDialogueDao.Inst.GetCfg(m_curCofig.NextDialogueId);
                if (cfg != null) {
                    StartCoroutine(Play(m_curCofig, cfg, 0, false, true));
                    m_curCofig = cfg;
                } else {
                    StartCoroutine(PlayOut(m_curCofig, true));
                }
            } else if (m_playStep == PlaySetp.playOption) {
                m_isClickNext = true;
            } else if (m_playStep == PlaySetp.playDialogPrint) {
                m_isClickNext = true;
            }
        }
        
    }
    
    private IEnumerator PlayOut(SceneDialogueConfig cfg, bool isDestroyWnd)
    {
        m_playStep = PlaySetp.playout;
        if (cfg == null) {
            m_IsHaveOption = false;
            if (m_List != null && m_List.Count > 0)
                foreach (GameObject g in m_List) {
                    GameObject.Destroy(g);
                }
            m_List.Clear();
            yield return null;
        } else {
            // 退出动画
            if (cfg.Image != "") {
                if (cfg.Direction == 0) {
                    if (cfg.ImageExit == 1) {  // 滑出
                        m_HeroAni1.SetTrigger("Out_1");
                    } else if (cfg.ImageExit == 2) { // 淡出
                        m_HeroAni1.SetTrigger("Out_2");
                    }
                } else {
                    if (cfg.ImageExit == 1) {
                        m_HeroAni2.SetTrigger("Out_1");
                    }  else if (cfg.ImageExit == 2) { // 淡出
                        m_HeroAni2.SetTrigger("Out_2");
                    }
                }
            }
            
            // 退出
            if (cfg.DialogueExit == 1) {
                m_DialogAni.SetTrigger("Dialogue_out");
            }
            
            if (m_IsHaveOption == true) {
                m_OptionAni.SetTrigger("Option_Main_out");
            }
            yield return Yielders.GetWaitForSeconds(0.33f);
            
            m_IsHaveOption = false;
            if (m_List != null && m_List.Count > 0)
                foreach (GameObject g in m_List) {
                    GameObject.Destroy(g);
                }
            m_List.Clear();
            if (isDestroyWnd == true) {
                WndManager.DestoryWnd<AdventureDialogueWnd>(0.5f + waitCloseDialogTime);
            }
        }
    }
    private IEnumerator PlayHeroIconAndDialogDialog(SceneDialogueConfig cfg)
    {
        m_playStep = PlaySetp.playHeroIcon;
        if (cfg != null) {
            // 英雄头像。
            /*if (cfg.Image != "")*/ {
                if (cfg.Direction == 0) {
                
                    //SetImage(m_HeroIcon1, "Icon", cfg.Image);
                    //m_HeroIcon1.SetNativeSize();
                    ShowAniGo(cfg.Direction, cfg.TalkAnim);
                    m_HeroAni1.GetComponent<RectTransform>().localPosition = m_HeroPos1 + new Vector3(cfg.ImagePosX, cfg.ImagePosY, 0);
                    if (cfg.ImageEnter == 1) {  // 滑入
                        m_HeroAni1.SetTrigger("In_1");
                    } else if (cfg.ImageEnter == 2) {
                        //淡入
                        m_HeroAni1.SetTrigger("In_2");
                    }
                } else {
                    //SetImage(m_HeroIcon2, "Icon", cfg.Image);
                    //m_HeroIcon2.SetNativeSize();
                    ShowAniGo(cfg.Direction, cfg.TalkAnim);
                    m_HeroAni2.GetComponent<RectTransform>().localPosition = m_HeroPos2 + new Vector3(cfg.ImagePosX, cfg.ImagePosY, 0);
                    if (cfg.ImageEnter == 1) {
                        m_HeroAni2.SetTrigger("In_1");
                    } else if (cfg.ImageEnter == 2) {
                        m_HeroAni2.SetTrigger("In_2");
                    }
                }
            }
            
            // 对白。
            List<string> l = new List<string>();
            if (cfg.DialogueFrame == 0) {
                gDialog1.SetActive(true);
                l.Clear();
                l.Add("");
                txtDialogTitle1.SetString(l);
                l.Clear();
                l.Add("");
                txtDialogContent1.SetString(l);
            } else if (cfg.DialogueFrame == 1) {
                gDialog2.SetActive(true);
                l.Clear();
                l.Add("");
                txtDialogTitle2.SetString(l);
                l.Clear();
                l.Add("");
                txtDialogContent2.SetString(l);
            } else {
                gDialog3.SetActive(true);
                l.Clear();
                l.Add("");
                txtDialogContent3.SetString(l);
            }
            if (cfg.DialogueEnter == 1) {
                m_DialogAni.SetTrigger("Dialogue_in");
            }
            yield return Yielders.GetWaitForSeconds(0.33f);
        } else {
            yield return null;
        }
        
    }
    private IEnumerator PlayEmoji(SceneDialogueConfig cfg)
    {
        m_playStep = PlaySetp.playEmojy;
        if (cfg != null) {
            if (cfg.Direction == 0) {
                if (cfg.Emotion > 0) {
                    m_Emoji1.gameObject.SetActive(true);
                    m_Emoji1.SetIndexSprite((int)cfg.Emotion - 1);
                }
            } else {
                if (cfg.Emotion > 0) {
                    m_Emoji2.gameObject.SetActive(true);
                    m_Emoji2.SetIndexSprite((int)cfg.Emotion - 1);
                }
            }
            yield return Yielders.GetWaitForSeconds(waitShowDialogTime);
        } else {
            yield return null;
        }
    }
    
    
    private IEnumerator PlayOption(SceneDialogueConfig cfg, bool showOption)
    {
        m_playStep = PlaySetp.playOption;
        if (cfg != null) {
            if (showOption == true) {
                m_IsHaveOption = false;
                if (cfg.OptionDialogue1 > 0) {
                    m_IsHaveOption = true;
                    AddOption(cfg.OptionTid1, cfg.OptionDialogue1, true);
                }
                if (cfg.OptionDialogue2 > 0) {
                    m_IsHaveOption = true;
                    AddOption(cfg.OptionTid2, cfg.OptionDialogue2, false);
                }
                if (cfg.OptionDialogue3 > 0) {
                    m_IsHaveOption = true;
                    AddOption(cfg.OptionTid3, cfg.OptionDialogue3, false);
                }
                if (m_IsHaveOption == true) {
                    m_OptionAni.SetTrigger("Option_Main_in");
                    yield return Yielders.GetWaitForSeconds(0.33f);
                }
            } else if (CheckHaveOption(cfg) == true) {
                while (m_isClickNext == false) {
                    yield return null;
                }
                //
                m_isClickNext = false;
                m_IsHaveOption = false;
                if (cfg.OptionDialogue1 > 0) {
                    m_IsHaveOption = true;
                    AddOption(cfg.OptionTid1, cfg.OptionDialogue1, true);
                }
                if (cfg.OptionDialogue2 > 0) {
                    m_IsHaveOption = true;
                    AddOption(cfg.OptionTid2, cfg.OptionDialogue2, false);
                }
                if (cfg.OptionDialogue3 > 0) {
                    m_IsHaveOption = true;
                    AddOption(cfg.OptionTid3, cfg.OptionDialogue3, false);
                }
                if (m_IsHaveOption == true) {
                    m_OptionAni.SetTrigger("Option_Main_in");
                    yield return Yielders.GetWaitForSeconds(0.33f);
                }
            }
            
        }
    }
    
    private IEnumerator DoOption(Animator ani, uint dialogID)
    {
        // 选择过需等待动画播放哦。
        if (m_IsPlayOptionSeleteAni == true) {
            yield return Yielders.GetWaitForSeconds(1.0f);
        }
        m_IsPlayOptionSeleteAni = true;
        if (ani != null) {
            ani.SetTrigger("Selected");
        }
        yield return Yielders.GetWaitForSeconds(1.0f);
        m_IsPlayOptionSeleteAni = false;
        SceneDialogueConfig cfg = SceneDialogueDao.Inst.GetCfg(dialogID);
        if (cfg != null) {
            StartCoroutine(Play(m_curCofig, cfg, 0, false, true));
            m_curCofig = cfg;
        } else {
            // StartCoroutine(Play(m_curCofig, null, 0, false, true));
            // m_curCofig = null;
        }
    }
    private void AddOption(string strOption, uint DialogID, bool first)
    {
        if (OptionPrefab != null) {
            OptionPrefab.SetActive(false);
            GameObject go = GameObject.Instantiate(OptionPrefab, tOptionParent);
            go.SetActive(true);
            Button btn = go.GetComponent<Button>();
            btn.onClick.AddListener(() => {
                StartCoroutine(DoOption(go.GetComponent<Animator>(), DialogID));
            });
            List<string> l = new List<string>();
            l.Add(strOption);
            IggText txt = go.transform.Find("Text_text_1").GetComponent<IggText>();
            if (txt != null) {
            
                txt.SetString(l);
            }
            IggText txt1 = go.transform.Find("Text_text_2").GetComponent<IggText>();
            if (txt1 != null) {
                txt1.SetString(l);
            }
            m_List.Add(go);
        }
    }
    private IEnumerator PlayDialogPrint(SceneDialogueConfig cfg, bool isPrint)
    {
        float t = 0;
        m_playStep = PlaySetp.playDialogPrint;
        if (cfg != null) {
            if (cfg.DialogueVoice != 0) {
                SoundManager.PlayDialog(cfg.DialogueVoice);
            }
            List<string> l = new List<string>();
            float t1 = 0.0f;
            if (isPrint == true) {
                if (cfg.DialogueFrame == 0) {
                    gDialog1.SetActive(true);
                    l.Clear();
                    l.Add(cfg.NameTid);
                    txtDialogTitle1.SetString(l);
                    l.Clear();
                    l.Add(cfg.DialogueTid);
                    t1 = txtDialogContent1.SetPrintString(l, cfg.DialogueSpeed);
                } else if (cfg.DialogueFrame == 1) {
                    gDialog2.SetActive(true);
                    l.Clear();
                    l.Add(cfg.NameTid);
                    txtDialogTitle2.SetString(l);
                    l.Clear();
                    l.Add(cfg.DialogueTid);
                    t1 = txtDialogContent2.SetPrintString(l, cfg.DialogueSpeed);
                } else {
                    gDialog3.SetActive(true);
                    l.Clear();
                    l.Add(cfg.DialogueTid);
                    t1 = txtDialogContent3.SetPrintString(l, cfg.DialogueSpeed);
                }
                
                // add by hlw: 策划要求时间根据策划自己配置的
                t1 = cfg.DialogueTime / 10f;
                float total = t1 + AutoTime;
                while (t < total && m_isClickNext == false) {
                    t += Time.deltaTime;
                    yield return null;
                }
                m_isClickNext = false;
                if (cfg.DialogueFrame == 0) {
                    l.Clear();
                    l.Add(cfg.DialogueTid);
                    txtDialogContent1.SetString(l);
                    gArrow1.SetActive(true);
                } else if (cfg.DialogueFrame == 1) {
                    l.Clear();
                    l.Add(cfg.DialogueTid);
                    txtDialogContent2.SetString(l);
                    gArrow2.SetActive(true);
                } else {
                    l.Clear();
                    l.Add(cfg.DialogueTid);
                    txtDialogContent3.SetString(l);
                    gArrow3.SetActive(true);
                }
            } else {
                if (cfg.DialogueFrame == 0) {
                    gDialog1.SetActive(true);
                    l.Clear();
                    l.Add(cfg.NameTid);
                    txtDialogTitle1.SetString(l);
                    l.Clear();
                    l.Add(cfg.DialogueTid);
                    txtDialogContent1.SetString(l);
                    gArrow1.SetActive(true);
                } else if (cfg.DialogueFrame == 1) {
                    gDialog2.SetActive(true);
                    l.Clear();
                    l.Add(cfg.NameTid);
                    txtDialogTitle2.SetString(l);
                    l.Clear();
                    l.Add(cfg.DialogueTid);
                    txtDialogContent2.SetString(l);
                    gArrow2.SetActive(true);
                } else {
                    gDialog3.SetActive(true);
                    l.Clear();
                    l.Add(cfg.DialogueTid);
                    txtDialogContent3.SetString(l);
                    gArrow3.SetActive(true);
                }
            }
            
            //
        } else {
            yield return null;
        }
        
    }
    
    
    
    private IEnumerator Play(SceneDialogueConfig oldCfg, SceneDialogueConfig cfg, float WaitTime, bool showOption, bool isPrint)
    {
        SoundManager.StopMusic(SoundType.dialog);
        if (WaitTime > 0) {
            yield return Yielders.GetWaitForSeconds(WaitTime);
        }
        // 同一个人就不用退场了。
        if (oldCfg != null && cfg != null && oldCfg.NpcFlag == cfg.NpcFlag) {
            if (m_IsHaveOption == true) {
                m_OptionAni.SetTrigger("Option_Main_out");
                m_IsHaveOption = false;
                if (m_List != null && m_List.Count > 0)
                    foreach (GameObject g in m_List) {
                        GameObject.Destroy(g);
                    }
                m_List.Clear();
                yield return Yielders.GetWaitForSeconds(0.33f);
            }
            yield return PlayIn(cfg, showOption, isPrint, true);
        } else {
            yield return PlayOut(oldCfg, false);
            yield return PlayIn(cfg, showOption, isPrint, false);
        }
        
        m_playStep = PlaySetp.end;
        if (DialogDC.AutoDialog == true) {
            BtnNextClick();
        }
    }
    
    
    
    public void BtnAutoClick()
    {
        if (m_IsHaveOption == true) {
            return;
        }
        DialogDC.AutoDialog = !DialogDC.AutoDialog;
        m_btnAutoAni.SetBool("Auto", DialogDC.AutoDialog);
        if (m_playStep == PlaySetp.end && DialogDC.AutoDialog == true) {
            BtnNextClick();
        }
        //
    }
    
    
    public void BtnSkipClick()
    {
        if (m_IsHaveOption == true) {
            return;
        }
        if (m_curCofig != null) {
            TipsHelper.ShowSkipMsgBox(m_curCofig.SkipSummary, DoSkipCall);
        } else {
            WndManager.DestoryWnd<AdventureDialogueWnd>(0.5f + waitCloseDialogTime);
        }
    }
    
    private void DoSkipCall(IMsgBoxResult reslut)
    {
        // 回调的时候已经销毁了
        if (this == null) {
            return;
        }
        
        if (reslut.ClickButton == TipButton.Ok) {
            if (m_curCofig != null) {
                SceneDialogueConfig cfg = FindNextOption(m_curCofig);
                if (cfg == m_curCofig) {
                    return;
                } else {
                    if (cfg == null) {
                    
                        m_DialogAni.SetTrigger("Dialogue_out");
                        m_OptionAni.SetTrigger("Option_Main_out");
                        StartCoroutine(PlayOut(m_curCofig, true));
                        //WndManager.DestoryWnd<AdventureDialogueWnd>(0.5f + waitCloseDialogTime);
                    } else {
                        StartCoroutine(Play(m_curCofig, cfg, 0, true, false));
                        m_curCofig = cfg;
                    }
                }
            } else {
                WndManager.DestoryWnd<AdventureDialogueWnd>(0.5f + waitCloseDialogTime);
            }
        }
    }
    
    private SceneDialogueConfig FindNextOption(SceneDialogueConfig cur)
    {
        if (cur == null) {
            return null;
        } else {
            if (cur.OptionDialogue1 > 0 || cur.OptionDialogue2 > 0 || cur.OptionDialogue3 > 0) {
                return cur;
            } else {
                SceneDialogueConfig c = SceneDialogueDao.Inst.GetCfg(cur.NextDialogueId);
                return FindNextOption(c);
            }
        }
    }
    
    private bool CheckHaveOption(SceneDialogueConfig cur)
    {
        if (cur == null) {
            return false;
        } else {
            if (cur.OptionDialogue1 > 0 || cur.OptionDialogue2 > 0 || cur.OptionDialogue3 > 0) {
                return true;
            }
        }
        return false;
    }
    
    //刷新界面
    public override void RefreshUI()
    {
    
    }
    
    /// <summary>
    /// 打开窗口时播放动作
    /// </summary>
    public override void PlayShowAni()
    {
    }
    
    /// <summary>
    /// 关闭窗口时播放动画
    /// </summary>
    public override void PlayCloseAni()
    {
        StartCoroutine(WaitCloseAni());
    }
    
    private IEnumerator WaitCloseAni()
    {
        yield return Yielders.GetWaitForSeconds(waitCloseDialogTime);
        if (m_WndAni != null) {
            m_WndAni.SetTrigger("Close");
        }
    }
    
    
    private void HideAllAniGo()
    {
        if (LeftAni != null) {
            foreach (GameObject g in LeftAni) {
                if (g != null) {
                    g.SetActive(false);
                }
            }
        }
        
        if (RightAni != null) {
            foreach (GameObject g in RightAni) {
                if (g != null) {
                    g.SetActive(false);
                }
            }
        }
    }
    
    private void ShowAniGo(uint direction, uint talkAnim)
    {
        HideAllAniGo();
        if (direction == 0) {
            if (talkAnim > 0 && talkAnim <= 7) {
                LeftAni[talkAnim - 1].SetActive(true);
            }
        } else {
            if (talkAnim > 0 && talkAnim <= 7) {
                RightAni[talkAnim - 1].SetActive(true);
            }
        }
    }
    
    
    /// <summary>
    /// 注册消息事件
    /// </summary>
    private void RegisterHooks()
    {
        // 示例注册客户端触发事件
        // EventCenter.RegisterHooks(EventCenterType.xxx, func);
        // 示例注册服务端触发事件
        // EventCenter.RegisterHooks(msgtype.MsgType.xxx, func);
    }
    
    /// <summary>
    /// 反注册消息事件
    /// </summary>
    private void AntiRegisterHooks()
    {
        // 示例注册客户端触发事件
        // EventCenter.AntiRegisterHooks(EventCenterType.xxx, func);
        // 示例注册服务端触发事件
        // EventCenter.AntiRegisterHooks(msgtype.MsgType.xxx, func);
    }
    
    /// <summary>
    /// 窗口销毁时的清理动作
    /// </summary>
    public virtual void OnDestroy()
    {
        AntiRegisterHooks();
        if (m_blr != null) {
            m_blr.HideBlur();
        }
        SoundManager.PlayMusic(m_PlayMusicID);
        if (TestMode == false) {
            EventCenter.DispatchEvent(EventCenterType.DialogEnd, -1, m_dialogID);
        }
    }
}

public enum PlaySetp {
    start     = 0,
    playout,
    playHeroIcon,
    playEmojy,
    playDialogPrint,
    playOption,
    end,       //只有这个阶段可执行下一步动作。
}