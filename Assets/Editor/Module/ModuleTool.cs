using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using IGG.Core.Module;

namespace IGG.EditorTools
{
    class ModuleVo
    {
        public string Id;
        public string Name;
        public string Type;
        public Type ModuleType;
        public Type ProxyType;
    }

    /// <summary>
    /// Author  gaofan
    /// Date    2017.12.27
    /// Desc    模块创建工具，创建模块模板代码，2自动绑定模块
    /// </summary>
    public class ModuleTool : ScriptableObject
    {
        [MenuItem("辅助工具/模块相关/代码生成")]
        static void ShowModuleToolWnd()
        {
            ModuleCodeBuildWnd wnd = EditorWindow.GetWindow<ModuleCodeBuildWnd>("模块代码生成工具");
            wnd.minSize = new Vector2(200, 250);
        }
    }
}