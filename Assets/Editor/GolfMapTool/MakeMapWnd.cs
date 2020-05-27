using UnityEngine;
using UnityEditor;
using IGG.Core.Data.DataCenter.GolfAI;
using UnityEditor.SceneManagement;
using System.Text.RegularExpressions;

public class MakeMapWnd : EditorWindow
{
    public MapData m_map = new MapData();
    public string strWidth = "10";
    private int width = 10;
    public string strHeight = "10";
    private int height = 10;

    public string strXstep = "10";
    private float Xstep = 100.0f;

    public string strZstep = "10";
    private float Zstep = 100.0f;

    public string strXHolePos = "1.0";
    public string strYHolePos = "0.0";
    public string strZHolePos = "1.0";
    private Vector3 m_HolePos;
    private Vector3 m_StartPos;

    public string strHoleRadius = "1.0";
    private float m_HoleRadius = 1.0f;

    private int m_Index = 1;

    [MenuItem("地图工具/制作超大地图")]
    static void ShowMakeMapWnd()
    {
        EditorUtility.ClearProgressBar();
        MakeMapWnd wnd = EditorWindow.GetWindow<MakeMapWnd>("制作超大地图");
        wnd.minSize = new Vector2(200, 250);
    }



    void OnGUI()
    {


        if (GUI.Button(new Rect(10, 200, 100, 30), "制作多彩地图") == true)
        {
            m_Index = 1;
            m_map.Clear();
            CreateGolfTerrian(10, 10, 5, 50, Vector3.zero, AreaType.Rough);
            CreateGolfTerrian(10, 10, 40, 30, new Vector3(50.0f, 0.0f, 0.0f), AreaType.Fairway);
            CreateGolfTerrian(10, 10, 5, 50, new Vector3(450.0f, 0.0f, 0.0f), AreaType.Rough);
            CreateGolfTerrian(10, 10, 40, 5, new Vector3(50.0f, 0.0f, 300.0f), AreaType.SandBunker);
            CreateGolfTerrian(10, 10, 40, 10, new Vector3(50.0f, 0.0f, 350.0f), AreaType.PuttingGreen);
            CreateGolfTerrian(10, 10, 40, 5, new Vector3(50.0f, 0.0f, 450.0f), AreaType.Rough);
            m_HolePos = new Vector3(250.0f, 0.0f, 375.0f);
            m_StartPos = new Vector3(250.0f, 0.0f, 25.0f);
            m_HoleRadius = 0.108f;
            CreateHole(m_HolePos, m_HoleRadius);
            m_map.HolePos = m_HolePos;
            m_map.StartPos = m_StartPos;
            m_map.HoleRadius = m_HoleRadius;
            MapData.SaveFile(m_map);
            EditorUtility.DisplayDialog("提示", "保存地图数据成功", "确认");
        }
    }



    private void CreateGolfTerrian(float xSize, float zSize, int w, int h, Vector3 gridStart, AreaType Type)
    {
        GameObject obj = new GameObject("Terrian_" + Type.ToString());
        obj.transform.parent = GameObject.Find("GolfMap").transform;
        obj.layer = 16;
        MeshFilter mf = obj.AddComponent<MeshFilter>();
        MeshRenderer mr = obj.AddComponent<MeshRenderer>();

        if (Type == AreaType.Fairway)
        {
            mr.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Art/Model/Sketchups/Materials/GolfTerrain1.mat");
        }
        else if (Type == AreaType.Rough)
        {
            mr.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Art/Model/Sketchups/Materials/GolfTerrain2.mat");
        }
        else if (Type == AreaType.SandBunker)
        {
            mr.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Art/Model/Sketchups/Materials/GolfTerrain3.mat");
        }
        else if (Type == AreaType.PuttingGreen)
        {
            mr.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Art/Model/Sketchups/Materials/GolfTerrain4.mat");
        }

        GolfMeshCreate _creator = new GolfMeshCreate();
        Mesh mesh = _creator.CreateMesh(w, h, xSize, zSize, gridStart, Type);
        mf.mesh = mesh;
        SaveMesh("Assets/Mesh/", Type, mesh);

        SubMapData sm = new SubMapData(Type, _creator.Listpt, _creator.ListTriangle);
        m_map.AddSubMap(sm);
    }

    private void CreateHole(Vector3 pos, float Radius)
    {
        GameObject obj = new GameObject("Hole");
        obj.transform.localPosition = pos + new Vector3(0, 0.05f, 0);
        obj.transform.parent = obj.transform.parent = GameObject.Find("GolfMap").transform; ;
        obj.layer = 16;
        MeshFilter mf = obj.AddComponent<MeshFilter>();
        MeshRenderer mr = obj.AddComponent<MeshRenderer>();

        CircleMeshCreator _creator = new CircleMeshCreator();
        int Segments = 50;   //分割数
        float AngleDegree = 360;
        float InnerRadius = Radius * 0.95f;
        Mesh mesh = _creator.CreateMesh(Radius, Segments, InnerRadius, AngleDegree);
        mf.mesh = mesh;
        mr.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Art/Materials/Hole.mat");
        GameObject qizi = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Data/Prefab/Terrian/qizhi.prefab");
        if (qizi != null)
        {
            GameObject g = GameObject.Instantiate(qizi, obj.transform);
            g.transform.Rotate(0, 180.0f, 0);
            g.transform.localScale = Vector3.one * 0.2f;
            GolfHoleProxy proxy = obj.AddComponent<GolfHoleProxy>();
            proxy.Qizhi = g;
            proxy.ren = mr;
        }

        SaveHoleMesh("Assets/Mesh/", mesh);

    }

    // 保存mesh
    public void SaveMesh(string outputFilePath, AreaType type, Mesh mesh)
    {
        if (mesh == null)
            return;
        mesh.RecalculateNormals();
        string dataPath = outputFilePath + type.ToString() + m_Index.ToString() + ".asset";
        AssetDatabase.CreateAsset(mesh, dataPath);
        m_Index++;
    }

    // 保存mesh
    public void SaveHoleMesh(string outputFilePath, Mesh mesh)
    {
        mesh.RecalculateNormals();
        string dataPath = outputFilePath + "Hole.asset";
        AssetDatabase.CreateAsset(mesh, dataPath);
    }


    public void SavePrefab()
    {
        PrefabUtility.ApplyPrefabInstance(GameObject.Find("GolfMap"), InteractionMode.AutomatedAction);
    }

}