using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(IggImageProxy))]
public class IggImageEditor : ImageEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        serializedObject.Update();
        SerializedProperty tps = serializedObject.FindProperty("spritesAllowed");
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(tps, true);
        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
