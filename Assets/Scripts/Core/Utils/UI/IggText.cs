using TMPro;
using UnityEngine;
using IGG.Core.Data.Config;
using System.Collections.Generic;
using System.Collections;
using System;
using IGG.Core.Manger.Coroutine;

public class IggText : MonoBehaviour
{
    public string strTid;
    public string keyWord;
    private bool m_isPrint = false;
    TextMeshProUGUI realText = null;
    
    
    
    public virtual void OnValidate()
    {
        if (realText == null) {
            realText = gameObject.GetComponent<TextMeshProUGUI>();
        }
        
        if (strTid != null && strTid.Length > 0) {
            TextConfig c = TextDao.Inst.GetCfg(strTid);
            if (c == null) {
                realText.text = "该tid：" + strTid + "未配置";
            } else {
                realText.text = c.Text;
            }
        }
    }
    
#if UNITY_EDITOR
    void Reset()
    {
        GameObject.DestroyImmediate(this, true);
        UnityEditor.EditorUtility.DisplayDialog("错误", "此脚本不允许绑定,请绑定IggTextProxy", "ok");
        UnityEditor.AssetDatabase.Refresh();
        UnityEditor.AssetDatabase.SaveAssets();
    }
#endif
    
    public void SetString()
    {
        m_isPrint = false;
        if (realText == null) {
            realText = gameObject.GetComponent<TextMeshProUGUI>();
        }
        realText.maxVisibleCharacters = 1000;
        if (strTid.Length > 0) {
            TextConfig c = TextDao.Inst.GetCfg(strTid);
            if (c == null) {
                realText.text = "该tid：" + strTid + "未配置";
            } else {
                realText.text = c.Text;
            }
        }
        
    }
    
    public void SetString(List<string> paramList)
    {
        m_isPrint = false;
        if (realText == null) {
            realText = gameObject.GetComponent<TextMeshProUGUI>();
        }
        realText.maxVisibleCharacters = 1000;
        if (strTid.Length > 0) {
            string c = TextDao.Inst.GetText(strTid, paramList);
            if (c == null) {
                realText.text = "该tid：" + strTid + "未配置";
            } else {
                realText.text = c;
            }
        }
    }
    
    public void SetString(params string[] paramArr)
    {
        m_isPrint = false;
        if (realText == null) {
            realText = gameObject.GetComponent<TextMeshProUGUI>();
        }
        realText.maxVisibleCharacters = 1000;
        if (strTid.Length > 0) {
            string c = TextDao.Inst.GetText(strTid, paramArr);
            if (c == null) {
                realText.text = "该tid：" + strTid + "未配置";
            } else {
                realText.text = c;
            }
        }
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="paramList"></param>
    /// <param name="speed">1秒几个字</param>
    public float SetPrintString(List<string> paramList, uint speed)
    {
        float ret = 0.0f;
        if (realText == null) {
            realText = gameObject.GetComponent<TextMeshProUGUI>();
        }
        
        if (strTid.Length > 0) {
            string c = TextDao.Inst.GetText(strTid, paramList);
            if (c == null) {
                realText.text = "该tid：" + strTid + "未配置";
                return 0;
            } else {
                int length = realText.GetTextInfo(c).characterCount;
                ret = length * 1.0f / speed;
                realText.text = c;
                realText.maxVisibleCharacters = 0;
                StopAllCoroutines();
                StartCoroutine(PrintText(length, 1.0f / speed));
            }
        }
        return ret;
    }
    
    /// <summary>
    ///
    /// </summary>
    /// <param name="world"></param>
    /// <param name="time">1一个字的耗时</param>
    /// <returns></returns>
    private IEnumerator PrintText(int length, float time)
    {
        if (realText == null || length == 0 || time < 0) {
            yield break;
        }
        int i = 0;
        m_isPrint = true;
        while (i < length && m_isPrint == true) {
            realText.maxVisibleCharacters = i;
            yield return Yielders.GetWaitForSeconds(time);
            i++;
        }
        realText.maxVisibleCharacters = 1000;
    }
}
