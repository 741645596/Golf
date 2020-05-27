using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

    /// <summary>
    /// Author  填写你的大名
    /// Date    2019.5.14
	/// Desc    Btn_ballItemitem控件
    /// </summary>

public class Btn_ballItem : WndItem {

    /// <summary>
    /// 定义关联对象
    /// </summary>

	/// <summary>
    /// 用于加载完成item时的操作
    /// </summary>
    protected override void Init()
    {

    }
	

    /// <summary>
    /// item 初始化
    /// </summary>
    protected override void InitItem()
    {

    }
	
	
    /// <summary>
    /// 传数据给item
    /// </summary>
    /// <param name="data">传递给item的数据</param>
    public override void SetData(object data)
    {

    }
	
	
	/// <summary>
    /// 不允许挂改脚本需改代理脚本
    /// </summary>	
#if UNITY_EDITOR
    void Reset()
    {
        GameObject.DestroyImmediate(this, true);
        UnityEditor.EditorUtility.DisplayDialog("错误", "此脚本不允许绑定,请绑定Btn_ballItemProxy", "ok");
        UnityEditor.AssetDatabase.Refresh();
        UnityEditor.AssetDatabase.SaveAssets();
    }
#endif
}
