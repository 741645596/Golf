using System.Diagnostics;
using UnityEngine;
using UnityEditor;

public class SvnTool : ScriptableObject
{
    [MenuItem("SVN/更新版本 __%#_u")]
    static void SvnUpdate()
    {
        string batPath = GetProjPath() + "/SvnUpdate.bat";
        Process.Start(batPath);
    }

    [MenuItem("SVN/提交版本 __%#_c")]
    static void SvnCommit()
    {
        string batPath = GetProjPath() + "/SvnCommit.bat";
        Process.Start(batPath);
    }

    static string GetProjPath()
    {
        string projFolder = Application.dataPath;
        return projFolder.Substring(0, projFolder.Length - 7);
    }
}