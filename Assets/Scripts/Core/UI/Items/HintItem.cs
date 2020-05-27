using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using IGG.Core.Helper;

/// <summary>
/// Author  zhulin
/// Date    2019.1.31
/// Desc    HintItemitem控件
/// </summary>

public class HintItem : WndItem
{

    /// <summary>
    /// 定义关联对象
    /// </summary>
    private RectTransform m_Rect;
    public TextMeshProUGUI m_txtTitle;
    public TextMeshProUGUI m_txtContext;
    
    
    private string m_strTitle;
    private string m_strContext;
    private RectTransform m_targetRect;
    private TipAlignment m_tipAlignment;
    private Vector2 m_offset;
    
    
    /// <summary>
    /// 用于加载完成item时的操作
    /// </summary>
    protected override void Init()
    {
        m_Rect = gameObject.GetComponent<RectTransform>();
    }
    
    
    /// <summary>
    /// item 初始化
    /// </summary>
    protected override void InitItem()
    {
        m_txtTitle.text = m_strTitle;
        m_txtContext.text = m_strContext;
        m_Rect.localScale = Vector3.zero;
        StartCoroutine(WaitSetPos(m_targetRect, m_tipAlignment, m_offset));
    }
    
    private IEnumerator WaitSetPos(RectTransform targetRect, TipAlignment alig, Vector2 offset)
    {
        yield return new WaitForEndOfFrame();
        SetPos(targetRect, alig, offset);
        m_Rect.localScale = Vector3.one;
    }
    
    /// <summary>
    /// 传数据给item
    /// </summary>
    /// <param name="data">传递给item的数据</param>
    public override void SetData(object data)
    {
    
    }
    
    /// <summary>
    /// 传数据给item
    /// </summary>
    /// <param name="data">传递给item的数据</param>
    public override void SetData(object[] data)
    {
        m_strTitle = data[0] as string;
        m_strContext = data[1] as string;
        m_targetRect = data[2] as RectTransform;
        m_tipAlignment = (TipAlignment)data[3];
        m_offset = (Vector2)data[4];
    }
    private void SetPos(RectTransform targetRect, TipAlignment alig, Vector2 offset)
    {
        m_Rect.position = targetRect.position;
        var position = m_Rect.localPosition;
        position.y = position.y - targetRect.sizeDelta.y / 2 - m_Rect.sizeDelta.y / 2;
        position.x = position.x + m_Rect.sizeDelta.x / 2 - targetRect.sizeDelta.x / 2;
        m_Rect.localPosition = position + new Vector3(offset.x, offset.y);
        
        //Vector3 targetScreenPos = (WndManager.GetUINode() as UINode).UICamera.WorldToScreenPoint(targetRect.position);
        //Vector3 pos = (WndManager.GetUINode() as UINode).UICamera.WorldToScreenPoint(m_Rect.position);
        
        //Vector2 center = targetScreenPos - pos;
        //Vector2 v = GetPivotWH(m_Rect, targetRect);
        //if (alig == TipAlignment.Left) {
        //    center.x -= v.x;
        //} else if (alig == TipAlignment.Right) {
        //    center.x += v.x;
        //} else if (alig == TipAlignment.Up) {
        //    center.y += v.y;
        //} else if (alig == TipAlignment.Down) {
        //    center.y -= v.y;
        //} else if (alig == TipAlignment.LeftUp) {
        //    center.x -= v.x;
        //    center.y += v.y;
        //} else if (alig == TipAlignment.LeftDown) {
        //    center.x -= v.x;
        //    center.y -= v.y;
        //} else if (alig == TipAlignment.RightUp) {
        //    center.x += v.x;
        //    center.y += v.y;
        //} else if (alig == TipAlignment.RightDown) {
        //    center.x += v.x;
        //    center.y -= v.y;
        //}
        //m_Rect.localPosition = center + offset;
        //center = m_Rect.localPosition;
        //offset = GetoffScreenOut(m_Rect);
        //m_Rect.localPosition = center + offset;
    }
    
    private Vector2 GetPivotWH(RectTransform target1Rect, RectTransform target2Rect)
    {
        Vector2 v = Vector2.zero;
        if (target1Rect != null) {
            v = new  Vector2(target1Rect.sizeDelta.x * target1Rect.pivot.x,
                target1Rect.sizeDelta.y  * target1Rect.pivot.y);
        }
        if (target2Rect != null) {
            v += new Vector2(target2Rect.sizeDelta.x  * target2Rect.pivot.x,
                    target2Rect.sizeDelta.y * target2Rect.pivot.y);
        }
        return v;
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
    
    void Update()
    {
        if (Input.GetMouseButtonUp(0)) {
            GameObject.DestroyImmediate(gameObject);
        }
    }
    
    
    
}
