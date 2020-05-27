using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FindComponent : EditorWindow
{
    [MenuItem("ArtOnly/FindComponent")]
    static public void ShowWindow()
    {
        FindComponent window = (FindComponent)EditorWindow.GetWindow(typeof(FindComponent));
    }

    Object target;
    private List<Object> _objList = new List<Object>();
    Vector2 scroolPos;

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.HelpBox("Help:" + "\n" + "" +
                        "   1.Drag a Componet Node to Target." + "\n" +
                        "   2.Select Objects from Hierarchy or Project List." + "\n" +
                        "   3.Click Find Botton and Wait.", MessageType.None);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Target");
        target = EditorGUILayout.ObjectField(target, typeof(Component), true);
        EditorGUILayout.EndHorizontal();

        Rect botton = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(botton, GUIContent.none))
        {
            TextEditor textEditor = new TextEditor(); 
            _objList.Clear();

            if (Path.GetFileName(AssetDatabase.GetAssetPath(Selection.objects[0])).Length == 0)
            {
                foreach (Object hierarchy in Selection.GetFiltered(target.GetType(), SelectionMode.Deep))
                {
                    if (!(hierarchy is Object))
                        continue;
                    _objList.Add(hierarchy);
                }
            }
            else
            {
                foreach (Object assets in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
                {
                    if (!(assets is Object))
                        continue;
                    Selection.objects = EditorUtility.CollectDeepHierarchy(new[] { assets });
                    foreach (Object o in Selection.GetFiltered(target.GetType(), SelectionMode.Unfiltered))
                    {
                        if (!(o is Object))
                            continue;
                        _objList.Add(o);
                    }
                }
            }
        }
        GUILayout.Label("Find", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });

        EditorGUILayout.EndHorizontal();


        Rect botton2 = EditorGUILayout.BeginHorizontal("Button");      
        if (GUI.Button(botton2, GUIContent.none))
        {
            foreach(Object o in _objList)
            {
                Debug.Log(o);
                DestroyImmediate(o);
            }
            _objList.Clear();
        }
        GUILayout.Label("Delect All", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        scroolPos = EditorGUILayout.BeginScrollView(scroolPos);

        EditorGUILayout.BeginVertical();
        for (int i = 0; i < _objList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(Path.GetFileName(AssetDatabase.GetAssetPath(_objList[i])) + PrefabUtility.GetPrefabParent(_objList[i]));
            EditorGUILayout.ObjectField(_objList[i], typeof(object), true);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}