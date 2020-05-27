using IGG.Core.Data.Config;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class FontTextCollector : ScriptableObject
{
    [MenuItem("辅助工具/tid文字提取")]
    static void TextCollect()
    {
        Dictionary<char, bool> dic = new Dictionary<char, bool>(8000);
        var texts = TextDao.Inst.GetCfgs();
        foreach (var item in texts) {
            var text = item.Text;
            for (int i = 0; i < text.Length; i++) {
                if (!dic.ContainsKey(text[i])) {
                    dic.Add(text[i], true);
                }
            }
        }
        
        StringBuilder sb = new StringBuilder(8000);
        foreach (var c in dic.Keys) {
            sb.Append(c);
        }
        Debug.Log(sb.ToString());
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }
}
