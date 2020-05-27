using IGG.Core.Data.DataCenter;
using IGG.Core;
using UnityEngine.UI;
using IGG.Core.Helper;
using UnityEngine;
using IGG.Core.Data.Config;
using System.Collections.Generic;
using System.Collections;
/// <summary>
/// Author  zhulin
/// Date    2019.2.18
/// Desc    敌我双方英雄怪物信息对话框
/// </summary>
public class TargetInfoWnd : WndBase
{

    /// <summary>
    /// 定义关联对象
    /// </summary>
    // 英雄基本信息
    public IggImage iconHeroStar;
    public IggImage iconHeroAttr;
    public IggText txtName;
    public IggText txtAttack;
    public IggText txtDefence;
    public IggText txtHp;
    // 英雄技能信息
    public GameObject tSkillNode; // 技能不存在时隐藏
    public IggText txtSkillName;
    public IggText txtSkillDesc;
    public Image iconSkillIcon;
    // 天赋信息
    public GameObject tTalentNode; // 天赋不存在时隐藏
    public IggText txtTalentName;
    public IggText txtTalentDesc;
    public Image iconTalentIcon;
    // Buff信息
    public GameObject tBuffNode;   // 天赋不存在时隐藏
    public GameObject gBuffPrefab; //
    public GameObject m_gChild;
    
    private TargetHeroInfo m_Data = null;
    private Vector2 m_ScreenPos;
    private RectTransform m_targetRect;
    private Vector2 m_offset;
    private TipAlignment m_tipAlignment;
    private RectTransform m_Rect;
    private bool m_IsTouchDestrory = false;
    private int m_Type = 0;
    // <summary>
    // 加载完成立即初始化的操作
    // </summary>
    public override void Init()
    {
        m_Rect = gameObject.GetComponent<RectTransform>();
        m_Rect.localScale = Vector3.zero;
    }
    
    /// <summary>
    /// 初始化窗口
    /// </summary>
    public override void InitWnd()
    {
        RegisterHooks();
        StartCoroutine(WaitSetPos());
    }
    
    public override void RefreshUI()
    {
        List<string> lParam = new List<string>();
        if (m_Data != null) {
            
            // 攻击力
            lParam.Clear();
            lParam.Add(m_Data.Attack.ToString());
            txtAttack.SetString(lParam);
            // 防御力
            lParam.Clear();
            lParam.Add(m_Data.Defence.ToString());
            txtDefence.SetString(lParam);
            // HP
            lParam.Clear();
            lParam.Add(m_Data.Hp.ToString());
            txtHp.SetString(lParam);
            // buff
            if (gBuffPrefab != null) {
            } else {
                tBuffNode.SetActive(false);
            }
            
            if (m_Data.IsSelf == true) {

                // 星级
                iconHeroStar.gameObject.SetActive(true);
                if (iconHeroStar != null) {
                    iconHeroStar.SetIndexSprite((int)m_Data.Star - 1);
                }
                
                
            } else {
                // 名称
                iconHeroStar.gameObject.SetActive(false);
            }
        }
    }
    
    
    /// <summary>
    /// 传数据给item
    /// </summary>
    /// <param name="data">传递给item的数据</param>
    public override void SetData(object[] data)
    {
        m_Type = (int)data[0];
        m_Data = data[1]  as TargetHeroInfo;
        if (m_Type == 0) {
            m_targetRect = data[2] as RectTransform;
        } else {
            m_ScreenPos = (Vector2)data[2];
        }
        m_tipAlignment = (TipAlignment)data[3];
        m_offset = (Vector2)data[4];
        m_IsTouchDestrory = (bool)data[5];
        RefreshUI();
    }
    
    
    private void SetPos(Vector2 targetScreenPos, TipAlignment alig, Vector2 size)
    {
        Vector2 offset = Vector2.zero;
        Vector2 pos = (WndManager.GetUINode() as UINode).UICamera.WorldToScreenPoint(m_Rect.position);
        
        Vector2 center = targetScreenPos - pos;
        Vector2 v = GetPivotWH(m_Rect, size);
        if (alig == TipAlignment.Left) {
            center.x -= v.x;
        } else if (alig == TipAlignment.Right) {
            center.x += v.x;
        } else if (alig == TipAlignment.Up) {
            center.y += v.y;
        } else if (alig == TipAlignment.Down) {
            center.y -= v.y;
        } else if (alig == TipAlignment.LeftUp) {
            center.x -= v.x;
            center.y += v.y;
        } else if (alig == TipAlignment.LeftDown) {
            center.x -= v.x;
            center.y -= v.y;
        } else if (alig == TipAlignment.RightUp) {
            center.x += v.x;
            center.y += v.y;
        } else if (alig == TipAlignment.RightDown) {
            center.x += v.x;
            center.y -= v.y;
        }
        m_Rect.localPosition = center + offset;
        center = m_Rect.localPosition;
        offset = GetoffScreenOut(m_Rect);
        m_Rect.localPosition = center + offset;
    }
    private void SetPos(RectTransform targetRect, TipAlignment alig, Vector2 offset)
    {
        Vector3 targetScreenPos = (WndManager.GetUINode() as UINode).UICamera.WorldToScreenPoint(targetRect.position);
        Vector3 pos = (WndManager.GetUINode() as UINode).UICamera.WorldToScreenPoint(m_Rect.position);
        
        Vector2 center = targetScreenPos - pos;
        Vector2 v = GetPivotWH(m_Rect, targetRect);
        if (alig == TipAlignment.Left) {
            center.x -= v.x;
        } else if (alig == TipAlignment.Right) {
            center.x += v.x;
        } else if (alig == TipAlignment.Up) {
            center.y += v.y;
        } else if (alig == TipAlignment.Down) {
            center.y -= v.y;
        } else if (alig == TipAlignment.LeftUp) {
            center.x -= v.x;
            center.y += v.y;
        } else if (alig == TipAlignment.LeftDown) {
            center.x -= v.x;
            center.y -= v.y;
        } else if (alig == TipAlignment.RightUp) {
            center.x += v.x;
            center.y += v.y;
        } else if (alig == TipAlignment.RightDown) {
            center.x += v.x;
            center.y -= v.y;
        }
        m_Rect.localPosition = center + offset;
        center = m_Rect.localPosition;
        offset = GetoffScreenOut(m_Rect);
        m_Rect.localPosition = center + offset;
    }
    
    /// <summary>
    /// 获取出屏幕外的修正值
    /// </summary>
    /// <returns></returns>
    private Vector2 GetoffScreenOut(RectTransform targetRect)
    {
        Vector2 offset = Vector2.zero;
        Vector3 pos = (WndManager.GetUINode() as UINode).UICamera.WorldToScreenPoint(targetRect.position);
        Vector2 v = new Vector2(targetRect.sizeDelta.x * targetRect.pivot.x, targetRect.sizeDelta.y * targetRect.pivot.y);
        float l = pos.x - v.x;
        float r = pos.x + v.x;
        float u = pos.y + v.y;
        float d = pos.y - v.y;
        if (l < 10) {
            offset.x = 10 - l;
        }
        if (r > Screen.width - 10) {
            offset.x = Screen.width - 10 - r;
        }
        if (d < 10) {
            offset.y = 10 - d;
        }
        if (u > Screen.height - 10) {
            offset.y = Screen.height - 10 - u;
        }
        return offset;
    }
    private Vector2 GetPivotWH(RectTransform target1Rect, Vector2 size)
    {
        Vector2 v = Vector2.zero;
        if (target1Rect != null) {
            v = new Vector2(target1Rect.sizeDelta.x * target1Rect.pivot.x,
                target1Rect.sizeDelta.y * target1Rect.pivot.y);
        }
        v += new Vector2(size.x / 2, size.y / 2);
        return v;
    }
    
    private Vector2 GetPivotWH(RectTransform target1Rect, RectTransform target2Rect)
    {
        Vector2 v = Vector2.zero;
        if (target1Rect != null) {
            v = new Vector2(target1Rect.sizeDelta.x * target1Rect.pivot.x, target1Rect.sizeDelta.y * target1Rect.pivot.y);
        }
        if (target2Rect != null) {
            v += new Vector2(target2Rect.sizeDelta.x * target2Rect.pivot.x,
                    target2Rect.sizeDelta.y * target2Rect.pivot.y);
        }
        return v;
    }
    
    void Update()
    {
        if (m_IsTouchDestrory == true) {
            if (Input.GetMouseButtonUp(0)) {
                WndManager.DestoryWnd<TargetInfoWnd>();
            }
        }
    }
    
    private IEnumerator WaitSetPos()
    {
        //yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.3f);
        if (m_Type == 0) {
            SetPos(m_targetRect, m_tipAlignment, m_offset);
        } else {
            SetPos(m_ScreenPos, m_tipAlignment, m_offset);
        }
        m_Rect.localScale = Vector3.one;
    }
    /// <summary>
    /// 绑定按钮事件
    /// </summary>
    protected override void BindEvents()
    {
        // 关联按钮事件示例
        //m_btnBg.onClick.AddListener(BtnBgClick);
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
    }
}
