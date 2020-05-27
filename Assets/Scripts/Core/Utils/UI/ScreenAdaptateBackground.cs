using UnityEngine;
using System.Collections;

public class ScreenAdaptateBackground : MonoBehaviour
{
    void Start()
    {
        ScreenAdaptate();
    }
    
    void Update()
    {
#if UNITY_EDITOR
        ScreenAdaptate();
#endif
    }
    public void ScreenAdaptate()
    {
        float width = Screen.width;
        float height = Screen.height;
        float designWidth = ConstantData.g_fDefaultResolutionWidth;//开发时分辨率宽
        float designHeight = ConstantData.g_fDefaultResolutionHeight;//开发时分辨率高
        float s1 = (float)designWidth / (float)designHeight;
        float s2 = (float)width / (float)height;
        if (s1 < s2) {
            designWidth = (int)Mathf.FloorToInt(designHeight * s2);
        } else if (s1 > s2) {
            designHeight = (int)Mathf.FloorToInt(designWidth / s2);
        }
        
        RectTransform rectTransform = this.transform as RectTransform;
        if (rectTransform != null) {
            rectTransform.sizeDelta = new Vector2(designWidth, designHeight);
        }
    }
}
