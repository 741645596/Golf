using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using IGG.EditorTools;
public class UIWndCode 
{
    public static bool CreateWnd(string WndName, string BaseWnd, bool isLoad = false)
    {
        if (string.IsNullOrEmpty(WndName) == true)
            return false;
        string ClassWnd = "";
        if (WndName.EndsWith("Wnd") == true)
        {
            ClassWnd = WndName;
        }
        else ClassWnd = WndName + "Wnd";

        MakeWnd_HCode(ClassWnd);
        MakeWndCode(ClassWnd);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("提示", "脚本生成完毕", "确定");

        if (isLoad == true)
        {
            string path = ResourcesPath.GetAssetResourceRunPath(ResourcesType.UIWnd, ResourcesPathMode.Editor);
            path += ClassWnd + ".prefab";
            Debug.Log(path);
            GameObject Prefab = PrefabUtility.LoadPrefabContents(path);
            //GameObject Prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            if (Prefab != null)
            {
                GameObject go = Prefab;
                if (null != go)
                {
                    go.transform.SetParent(GameObject.Find("UI/Wnd").transform, false);
                    Debug.Log("窗口模本生成完毕");
                }
            }
            else
            {
                Prefab = AssetDatabase.LoadAssetAtPath("Assets/Editor/UITool/XXXWnd.prefab", typeof(GameObject)) as GameObject;
                GameObject go = GameObject.Instantiate(Prefab);
                if (null != go)
                {
                    go.transform.SetParent(GameObject.Find("UI/Wnd").transform, false);
                    go.name = ClassWnd;
                    //
                    Debug.Log("窗口模本生成完毕");
                }
            }
        }
        return true;
    }


    private static void MakeWndCode(string WndName)
    {
        string filename = Application.dataPath + "/Scripts/Core/UI/Wnd/" + WndName + ".cs";
        if (File.Exists(filename) == true)
            return;
        CreateCode("填写你的大名", WndName, "Core/UI/Wnd/${moduleName}.cs", "WndTemplate");

    }


    private static void CreateCode(string  strAuthor, string mName, string mPath, string mTemplate)
    {
        DateTime dt = DateTime.Now;
        TemplateEngine.TemplateBasePath = "Assets/Editor/UITool/Template/";
        Dictionary<string, string> valueDic = new Dictionary<string, string>();
        valueDic["Author"] = strAuthor;
        valueDic["CreateDate"] = string.Format("{0}.{1}.{2}", dt.Year, dt.Month, dt.Day);
        valueDic["moduleName"] = mName;

        TemplateEngine.CreateCodeFile(mPath.Replace("${moduleName}", mName), mTemplate, valueDic);
        TemplateEngine.TemplateBasePath = null;
    }


    private static void MakeWnd_HCode(string WndName)
    {
        string Wnd_h = WndName + "Proxy";
        string filename = Application.dataPath + "/Scripts/ArtWork/ComProxy/UI/Wnd/" + Wnd_h + ".cs";

        if (File.Exists(filename) == true)
            return;

        CreateCode("填写你的大名", WndName, "ArtWork/ComProxy/UI/Wnd/${moduleName}Proxy.cs", "WndProxyTemplate");
    }
}
