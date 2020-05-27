using System;
using System.Collections.Generic;
using IGG.EditorTools.UI;
using UnityEngine;
using UnityEditor;
using Input = IGG.EditorTools.UI.Input;

namespace IGG.EditorTools
{
    /// <summary>
    /// Author  gaofan
    /// Date    2017.12.27
    /// Desc    模块模板代码创建工具
    /// </summary>
    public class ModuleCodeBuildWnd : EditorWindow
    {
        void OnGUI()
        {
            if (m_toggleArr == null)
            {
                InitGUI();
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
            m_moudleName.Draw();
            m_author.Draw();

            m_scrollView.End();
            m_createBtn.Rect.x = (position.width - m_createBtn.Rect.width) / 2;
            m_createBtn.Rect.y = position.height - m_createBtn.Rect.height - 10;
            m_createBtn.Draw();
        }


        private void InitGUI()
        {
            const int itemW = 250;
            const int itemH = 17;

            List<Toggle> list = new List<Toggle>();
            Action<string, string, string> addItem =
                (label, tempate, path) =>
                {
                    Toggle item = new Toggle();
                    item.Label = label;
                    item.Data = new[] { path, tempate };
                    item.Select = true;
                    item.Rect = new Rect(20, list.Count * itemH, itemW, itemH);
                    list.Add(item);
                };

            addItem("模块类文件(XxxModule.cs)",
                "ModuleTemplate",
                "Core/Module/${moduleName}/${moduleName}Module.cs");



            addItem("数据中心类文件(XxxDC.cs)",
                "DCTemplate",
                "Core/Data/DataCenter/${moduleName}/${moduleName}DC.cs");

            addItem("模块类自定义数据类(XxxVo.cs)",
                "VoTemplate",
                "Core/Data/DataCenter/${moduleName}/${moduleName}Vo.cs");


            m_toggleArr = list.ToArray();

            Toggle lastItem = m_toggleArr[m_toggleArr.Length - 1];
            m_moudleName.Rect = lastItem.Rect;
            m_moudleName.Rect.y += itemH + 10;
            m_moudleName.RectChange();
            m_moudleName.Label = "模块名:";

            m_author.Rect = m_moudleName.Rect;
            m_author.Rect.y += itemH + 5;
            m_author.RectChange();
            m_author.Label = "作者名:";

            m_scrollView.ViewRect.height = m_toggleArr.Length * itemH;
            m_createBtn.Rect.Set(10, 0, 120, 30);
            m_createBtn.Label = "创建代码";
            m_createBtn.OnClick = OnClick;
        }

        private void OnClick(Button button)
        {
            if (m_author.IsEmpty)
            {
                EditorUtility.DisplayDialog("提示", "请填写作者名", "确认");
                return;
            }

            if (m_moudleName.IsEmpty)
            {
                EditorUtility.DisplayDialog("提示", "请填写模块名", "确认");
                return;
            }

            m_moudleName.Text = TemplateEngine.FormatName(m_moudleName.Text);

            DateTime dt = DateTime.Now;
            TemplateEngine.TemplateBasePath = "Assets/Editor/Module/Template/";
            Dictionary<string, string> valueDic = new Dictionary<string, string>();
            valueDic["Author"] = m_author.Text;
            valueDic["CreateDate"] = string.Format("{0}.{1}.{2}", dt.Year, dt.Month, dt.Day);
            valueDic["moduleName"] = m_moudleName.Text;
            foreach (Toggle toggle in m_toggleArr)
            {
                if (!toggle.Select)
                {
                    continue;
                }
                string[] param = (string[])toggle.Data;
                TemplateEngine.CreateCodeFile(param[0].Replace("${moduleName}", m_moudleName.Text), param[1], valueDic);
            }
            TemplateEngine.TemplateBasePath = null;
            EditorUtility.DisplayDialog("提示", "处理完毕", "确认");
            AssetDatabase.Refresh();
        }

        private Toggle[] m_toggleArr;
        private readonly ScrollView m_scrollView = new ScrollView();
        private readonly Input m_moudleName = new Input();
        private readonly Input m_author = new Input();
        private readonly Button m_createBtn = new Button();
    }
}