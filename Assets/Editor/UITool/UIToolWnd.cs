using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using System;

public class UIToolWnd : EditorWindow {


	private string m_WndName = "";

	void OnGUI()
	{
		this.title = "生成界面及代码";
		this.minSize = new Vector2(480, 500);

		GUI.Label (new Rect (0, 50, 100, 20), "窗口名称:");
		m_WndName = GUI.TextField(new Rect(100, 50, 200, 20), m_WndName, 18);

		if (GUI.Button(new Rect (10,80,100 ,30),"生成窗口"))
		{
            UIWndCode.CreateWnd(m_WndName, "WndBase", true);
        }
	}


    public void DidReloadScripts() {


    }



}
