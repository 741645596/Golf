using System;
using System.Collections.Generic;
using System.IO;
using IGG.EditorTools;
using IGG.EditorTools.UI;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// Author  zhulin
/// Date    2017.12.14
/// Desc    配置文件代码创建器
/// </summary>
public class UITool : ScriptableObject
{
    [MenuItem("辅助工具/UI/界面代码生成")]
    public static void MakeUIWndCode()
    {
        ChangeUIScene();
        //
        string[] allCfg = UITool.GetAllWnd();
        UICreateWnd wnd = EditorWindow.GetWindow<UICreateWnd>("UI界面代码生成工具");
        wnd.minSize = new Vector2(200, 250);
        wnd.SetCfg(allCfg, true);
    }

    [MenuItem("辅助工具/UI/Item代码生成")]
    public static void MakeUIItemCode()
    {
        ChangeUIScene();
        //
        UICreateWnd wnd = EditorWindow.GetWindow<UICreateWnd>("配置序列化工具");
        wnd.minSize = new Vector2(200, 250);
        string[] allCfg = UITool.GetAllItem();
        wnd.SetCfg(allCfg, false);
    }

    [@MenuItem("辅助工具/UI/制作UI窗体及生产代码")]
    public static void CreateUIWndCode()
    {
        ChangeUIScene();
        EditorWindow.GetWindow(typeof(UIToolWnd));
    }

    [@MenuItem("辅助工具/UI/制作UI Item及生成代码")]
    public static void CreateUIItemCode()
    {
        ChangeUIScene();
        EditorWindow.GetWindow(typeof(UIItemWnd));
    }

    private static void ChangeUIScene()
    {
        if (EditorSceneManager.GetActiveScene().name != "UiEditor")
        {
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            EditorSceneManager.OpenScene("Assets/Editor/UITool/UiEditor.unity");
        }
    }
    /// <summary>
    /// 获得所有窗体
    /// </summary>
    /// <returns></returns>
    public static string[] GetAllWnd()
    {
        List<string> list = new List<string>();
        List<string> listPath = new List<string>();
        string temp = ResourcesPath.GetAssetResourceRunPath(ResourcesType.UIWnd, ResourcesPathMode.Editor);
        string SearchPath = temp.Substring(0, temp.Length - 1);
        listPath.Add(SearchPath);
        var guid2 = AssetDatabase.FindAssets("t:gameObject", listPath.ToArray());

        foreach (var guid in guid2)
        {
            string assetpath = AssetDatabase.GUIDToAssetPath(guid);
            string name = Path.GetFileNameWithoutExtension(assetpath);
            if (name.Contains("Wnd") == true)
            {
                list.Add(name);
            }
        }
        return list.ToArray();
    }

    /// <summary>
    /// 获得所有窗体
    /// </summary>
    /// <returns></returns>
    public static string[] GetAllItem() 
    {
        List<string> list = new List<string>();
        List<string> listPath = new List<string>();
        string temp = ResourcesPath.GetAssetResourceRunPath(ResourcesType.UIWndItem, ResourcesPathMode.Editor);
        string SearchPath = temp.Substring(0, temp.Length - 1);
        listPath.Add(SearchPath);
        var guid2 = AssetDatabase.FindAssets("t:gameObject", listPath.ToArray());

        foreach (var guid in guid2)
        {
            string assetpath = AssetDatabase.GUIDToAssetPath(guid);
            string name = Path.GetFileNameWithoutExtension(assetpath);
            if (name.Contains("Item") == true)
            {
                list.Add(name);
            }
        }
        return list.ToArray();
    }



}


/// <summary>
/// Author  zhulin
/// Date    2017.12.14
/// Desc    配置文件工具的界面
/// </summary>
public class  UICreateWnd : EditorWindow
{
    private bool m_isWnd = false;
    public void SetCfg(string[] cfgNames, bool isWnd)
    {
        m_isWnd = isWnd;
        //分出有文件和没文件的区别
        List<string> existList = new List<string>();
        List<string> newList= new List<string>();
        string basePath = EditorHelper.GetProjPath("Assets/Scripts/Core/UI/Wnd/");
        if (m_isWnd == false)
        {
            basePath = EditorHelper.GetProjPath("Assets/Scripts/Core/UI/Items/");
        }

        for (int i = 0; i < cfgNames.Length; i++)
        {
            string cfgName = cfgNames[i];
            if (File.Exists(basePath + cfgName  + ".cs") )
            {
                existList.Add(cfgName);
            }
            else
            {
                newList.Add(cfgName);
            }
        }

        const int itemW = 200;
        const int itemH = 17;

        List<Toggle> toggleList = new List<Toggle>(cfgNames.Length);
        for (int i = 0; i < newList.Count; i++)
        {
            Toggle item = new Toggle();
            item.Label = newList[i];
            item.Data = newList[i];
            item.Rect = new Rect(20, toggleList.Count * itemH, itemW, itemH);
            toggleList.Add(item);
        }
        for (int i = 0; i < existList.Count; i++)
        {
            Toggle item = new Toggle();
            item.Label = "[有]" + existList[i];
            item.Data = existList[i];
            item.Rect = new Rect(20, toggleList.Count * itemH, itemW, itemH);
            toggleList.Add(item);
        }
        m_toggleArr = toggleList.ToArray();
        m_scrollView.ViewRect.height = cfgNames.Length*itemH;

        m_createBtn.Rect.Set(10,0,120,30);
        m_createBtn.Label = "创建UI代码";
        m_createBtn.OnClick = OnClick;
    }

    private void OnClick(Button button)
    {
        bool success = true;
        foreach (Toggle toggle in m_toggleArr)
        {
            if (toggle.Select)
            {
                if (m_isWnd == true)
                {
                    string WndName = Path.GetFileNameWithoutExtension(toggle.Data as string);
                    if (!UIWndCode.CreateWnd(WndName, "WndBase", true))
                    {
                        EditorUtility.DisplayDialog("错误", "生成" + toggle.Label + "代码时出现错误,请查看LOG", "确认");
                        success = false;
                    }
                }
                else
                {
                    string WndName = Path.GetFileNameWithoutExtension(toggle.Data as string);
                    if (!UIItemCode.CreateItem(WndName, "WndItem", true))
                    {
                        EditorUtility.DisplayDialog("错误", "生成" + toggle.Label + "代码时出现错误,请查看LOG", "确认");
                        success = false;
                    }
                }
            }
        }

        if (success)
        {
            EditorUtility.DisplayDialog("成功", "代码创建成功", "确认");
            AssetDatabase.Refresh();
        }
    }

    void OnGUI()
    {
        if (m_toggleArr == null)
        {
            return;
        }

        m_scrollView.Position.Set(0, 0, position.width, position.height - m_createBtn.Rect.height - 20);
        m_scrollView.ViewRect.width = position.width - 20;
        m_scrollView.Begin();

        for (int i = 0; i < m_toggleArr.Length; i++)
        {
            Toggle item = m_toggleArr[i];
            item.Draw();
        }

        m_scrollView.End();

        m_createBtn.Rect.x = (position.width - m_createBtn.Rect.width)/2;
        m_createBtn.Rect.y = position.height - m_createBtn.Rect.height - 10;
        m_createBtn.Draw();
    }

    private readonly ScrollView m_scrollView = new ScrollView();
    private Toggle[] m_toggleArr;
    private readonly Button m_createBtn = new Button();

    public void DidReloadScripts()
    {


    }
}