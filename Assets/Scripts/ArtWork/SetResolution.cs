using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using IGG.Core.Helper;

public class SetResolution : MonoBehaviour
{
    public int width = 1080;
    public int height = 1920;
    
    GUIStyle style = new GUIStyle();
    Rect rect;
    void Start()
    {
#if UNITY_STANDALONE || UNITY_WEBPLAYER
        if (Screen.width != width || Screen.height != height) {
            Screen.SetResolution(width, height, false);
        }
#endif
        Application.targetFrameRate = 40;
        
        
        int w = Screen.width, h = Screen.height;
        rect = new Rect(0, 0, w, h * 2 / 100);
        
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 60;
        style.normal.textColor = new Color(0.9f, 0.0f, 0.1f, 1.0f);
    }
    
    
    float deltaTime = 0.0f;
    
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }
    
    void OnGUI()
    {
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
