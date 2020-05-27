using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using System;

public class UIItemWnd : EditorWindow
{

    private string m_ItemName = "";


    void OnGUI()
    {
        this.title = "生成item及代码";
        this.minSize = new Vector2(480, 500);

        GUI.Label(new Rect(0, 50, 100, 20), "Item名称:");
        m_ItemName = GUI.TextField(new Rect(100, 50, 200, 20), m_ItemName, 18);

        if (GUI.Button(new Rect(10, 80, 100, 30), "生成Item"))
        {
            UIItemCode.CreateItem(m_ItemName, "WndItem", true);
        }
    }

    public void DidReloadScripts()
    {


    }

}
