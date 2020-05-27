using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 策划用的 IggImage的代理类 ，程序不要用这个命名
/// </summary>
/// 
public class IggImage : Image
{
#if UNITY_EDITOR
    protected override void Reset()
    {
        GameObject.DestroyImmediate(this, true);
        UnityEditor.EditorUtility.DisplayDialog("错误", "此脚本不允许绑定,请绑定IggImageProxy", "ok");
        UnityEditor.AssetDatabase.Refresh();
        UnityEditor.AssetDatabase.SaveAssets();
    }
#endif

    public Sprite[] spritesAllowed;

    public void SetIndexSprite(int index)
    {
        if (spritesAllowed != null && index >= 0 && index < spritesAllowed.Length)
        {
            sprite = spritesAllowed[index];
        }
    }
}