using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

//Author: xinruilin
public class FastLauchGame : MonoBehaviour {

    [MenuItem("快速操作/运行游戏 _%&_r")]
	public static void LauchGame() {
        EditorSceneManager.OpenScene("Assets/Scene/Lauch.unity");
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }
}
