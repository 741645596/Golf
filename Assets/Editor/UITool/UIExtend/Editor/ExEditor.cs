using UnityEngine;
using UnityEditor;

public class ExEditor : EditorWindow
{
    private static GameObject imagePrefab = null;
    private static GameObject textPrefab = null;
    
    [MenuItem("GameObject/UI/IggImage")]
    static void InitImage()
    {
        if (imagePrefab == null) {
            Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Editor/UITool/Prefabs/UIExtend/IggImage.prefab", typeof(GameObject));
            imagePrefab = prefab as GameObject;
        }
        
        GameObject clone = Instantiate(imagePrefab, Vector3.zero, Quaternion.identity) as GameObject;
        // Modify the clone to your heart's content
        if (Selection.activeGameObject != null) {
            clone.transform.SetParent(Selection.activeGameObject.transform);
        }
        
        clone.transform.localPosition = Vector3.one;
    }
    
    [MenuItem("GameObject/UI/IggText")]
    static void InitText()
    {
        if (textPrefab == null) {
            Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Editor/UITool/Prefabs/UIExtend/IggText.prefab", typeof(GameObject));
            textPrefab = prefab as GameObject;
        }
        
        GameObject clone = Instantiate(textPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        // Modify the clone to your heart's content
        if (Selection.activeGameObject != null) {
            clone.transform.SetParent(Selection.activeGameObject.transform);
        }
        
        clone.transform.localPosition = Vector3.one;
    }
    
}
