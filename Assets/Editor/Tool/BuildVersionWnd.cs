using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using System;
using IGG.EditorTools;

public class BuildVersionWnd : EditorWindow
{
    /*[@MenuItem("版本发布/版本发布")]
    static void Apply()
    {
        EditorWindow.GetWindow(typeof(BuildVersionWnd));
    }*/
    
    //[@MenuItem("版本发布/制作AB包")]
    public static void BuildVersionTest()
    {
        //xlua 生成
        //CSObjectWrapEditor.Generator.GenAll();
        BuildAssetBundle.Build();
        EditorUtility.DisplayDialog("提示", "AB包生产完毕", "确定");
    }
    
    
    
    //[@MenuItem("版本发布/PC版本")]
    public static void BuildVersionPC()
    {
        SwitchPlatform(BuildTarget.StandaloneWindows);
        CreateFullVersion();
    }
    
    //[@MenuItem("版本发布/Android版本")]
    public static void BuildVersionAndroid()
    {
        SwitchPlatform(BuildTarget.Android);
        CreateFullVersion();
    }
    
    //[@MenuItem("版本发布/Android Debug版本")]
    public static void BuildVersionAndroidDebug()
    {
    
        SwitchPlatform(BuildTarget.Android);
        CreateFullVersion("", "", true, true);
    }
    
    
    
    //[@MenuItem("版本发布/IOS版本")]
    public static void BuildVersionIos()
    {
        SwitchPlatform(BuildTarget.iOS);
        CreateFullVersion();
    }
    
    
    
    //[MenuItem("版本发布/拷贝AssetBundle到StreamingAssets目录")]
    /*public static void CopyAssets()
    {
        FullVersionResource.CopyAssets();
    }*/
    
    // 整包版本。
    public static void CreateFullVersion(string pathPlatform = "", string pathPackage = "", bool development = false, bool autoConnectProfiler = false, bool isApk = true)
    {
        /*FullVersionResource.CopyAssets();
        if (ConstantData.EnablePatch) {
            FullVersionResource.InitPatch();
        }*/
        FullVersionResource.Build(pathPlatform, pathPackage, development, autoConnectProfiler, isApk);
        IGGDebug.Log(PlayerSettings.productName + "游戏发布完成");
        OpenApp();
    }
    
    
    public static void SwitchPlatform(BuildTarget BuildVersionTarget)
    {
        if (BuildVersionTarget == BuildTarget.StandaloneWindows) {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
            PlayerSettings.bundleVersion = LogicConstantData.g_version;
        } else if (BuildVersionTarget == BuildTarget.Android) {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            PlayerSettings.Android.keystorePass = "123456";
            PlayerSettings.Android.keyaliasPass = "123456";
            PlayerSettings.Android.keystoreName = EditorHelper.GetProjPath("Tools/Keystore/user.keystore");
            PlayerSettings.bundleVersion = LogicConstantData.g_version;
        } else if (BuildVersionTarget == BuildTarget.iOS) {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
            PlayerSettings.bundleVersion = LogicConstantData.g_version;
        }
        AssetDatabase.Refresh();
    }
    
    
    private static void OpenApp()
    {
        return;
        string path = Application.dataPath;
        string path1 = "C:/zero/";
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows) {
            path1 += "PC/" + PlayerSettings.productName + ".exe";
            EditorHelper.OpenFileOrFolder(path1);
        } else if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android) {
            path1 += "android/" + PlayerSettings.productName + ".apk";
            EditorHelper.OpenFileOrFolder(path1);
        }
    }
}
