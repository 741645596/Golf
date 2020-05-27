using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using IGG.EditorTools;

public class UIItemCode 
{
    public static bool CreateItem(string ItemName, string BaseItem, bool isLoad = false)
    {
        if (string.IsNullOrEmpty(ItemName) == true)
            return false;
        string ClassItem = "";
        if (ItemName.EndsWith("Item") == true)
        {
            ClassItem = ItemName;
        }
        else ClassItem = ItemName + "Item";

        MakeItem_HCode(ClassItem);
        MakeItemCode(ClassItem, BaseItem);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("提示", "脚本生成完毕", "确定");

        if (isLoad == true)
        {
            string path = ResourcesPath.GetAssetResourceRunPath(ResourcesType.UIWndItem, ResourcesPathMode.Editor);
            path += ClassItem + ".prefab";
            Debug.Log(path);
            //GameObject Prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            GameObject Prefab = PrefabUtility.LoadPrefabContents(path);
            if (Prefab != null)
            {

                //GameObject go = GameObject.Instantiate(Prefab);
                GameObject go = Prefab;
                if (null != go)
                {
                    go.transform.SetParent(GameObject.Find("UI/Wnd").transform, false);
                    Debug.Log("窗口模本生成完毕");
                }
            }
            else
            {
                Prefab = AssetDatabase.LoadAssetAtPath("Assets/Editor/UITool/XXXItem.prefab", typeof(GameObject)) as GameObject;
                GameObject go = GameObject.Instantiate(Prefab);
                if (null != go)
                {
                    go.transform.SetParent(GameObject.Find("UI/Wnd").transform, false);
                    go.name = ClassItem;
                    //
                    Debug.Log("Item模板生成完毕");
                }
            }
        }
        return true;
    }


    private static void MakeItemCode(string ItemName, string BaseItemName)
    {
        string filename = Application.dataPath + "/Scripts/Core/UI/Items/" + ItemName + ".cs";
        if (File.Exists(filename) == true)
            return;

        CreateCode("填写你的大名", ItemName, "Core/UI/Items/${moduleName}.cs", "ItemTemp");
    }

    private static void CreateCode(string strAuthor, string mName, string mPath, string mTemplate)
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



    public static void MakeItem_HCode(string ItemName)
    {
        string ItemName_h = ItemName + "Proxy";

        string filename = Application.dataPath + "/Scripts/ArtWork/ComProxy/UI/Item/" + ItemName_h + ".cs";
        if (File.Exists(filename) == true)
            return;

        CreateCode("填写你的大名", ItemName, "ArtWork/ComProxy/UI/Item/${moduleName}Proxy.cs", "ItemProxyTemp");
    }
}
