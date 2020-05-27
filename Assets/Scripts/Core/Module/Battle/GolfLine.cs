using IGG.Core;
using System.Collections.Generic;
using UnityEngine;
using IGG.Core.Data.DataCenter.GolfAI;
using IGG.Core.Data.DataCenter.Battle;

public class GolfLine : MonoBehaviour
{
    private Mesh Linemesh = null;
    public Material material;

    // Start is called before the first frame update
    private void Awake()
    {
        RegisterHooks();
    }
    /// <summary>
    /// 绘制曲线
    /// </summary>
    /// <param name="Count"></param>
    /// <param name="startWidth"></param>
    /// <param name="endWidth"></param>
    /// <param name="lpt"></param>
    private void DrawCurve(float Width, GolfPathInfo Info)
    {
        if (Info == null || Info.ListPt == null || Info.ListPt.Count == 0)
        {
            ClearCurve();
            return;
        }
        List<Vector3> l = new List<Vector3>();
        foreach (GolfPathPoint p in Info.ListPt)
        {
            l.Add(p.Position);
        }
        if (BattleM.BallInAra == AreaType.PuttingGreen)
        {
            Linemesh = CreateLine(l, Width, true);
        }
        else
        {
            Linemesh = CreateLine(l, Width, true);
        }
    }

    /// <summary>
    /// 清理曲线
    /// </summary>
    private  bool  ClearCurve()
    {
        Linemesh = null;
        return true;
    }



    private void RegisterHooks()
    {
        EventCenter.RegisterHooks(EventCenterType.Battle_DrawPath, RefreshGolfPath);
        EventCenter.RegisterHooks(EventCenterType.Battle_BattingFinish, BattingFinish);
    }

    private void AntiRegisterHooks()
    {
        EventCenter.AntiRegisterHooks(EventCenterType.Battle_DrawPath, RefreshGolfPath);
        EventCenter.AntiRegisterHooks(EventCenterType.Battle_BattingFinish, BattingFinish);
    }

    private void BattingFinish(int Event_Send, object Param)
    {
        ClearCurve();
    }
    /// <summary>
    /// 实时刷新路径
    /// </summary>
    /// <param name="Event_Send"></param>
    /// <param name="Param"></param>
    private void RefreshGolfPath(int Event_Send, object Param)
    {
        if (Param == null)
        {
            ClearCurve();
        }
        else
        {
            GolfPathInfo Info = Param as GolfPathInfo;
            DrawCurve(0.15f, Info);
        }
    }


    private void OnDestroy()
    {
        AntiRegisterHooks();
    }



    public void Update()
    {
        if (Linemesh != null)
        {
            Graphics.DrawMesh(Linemesh, Vector3.zero, Quaternion.identity, material, 0);
        }
    }

    private Mesh CreateLine(List<Vector3> lv, float Width, bool IsPull = false)
    {
        if (lv == null || lv.Count < 2)
            return null;
        Mesh mesh = new Mesh();

        int vlen = lv.Count;
        Vector3[] vertices = new Vector3[vlen * 2];
        Vector2[] uvs = new Vector2[vlen * 2];

        float dis = 0;

        for (int i = 0; i < vlen; i ++)
        {
            if (i == 0)
            {
                dis = 0;
                vertices[i] = CalcLinePoint(lv[0], lv[1] - lv[0], -Width, IsPull);
                uvs[i] = new Vector2(0.25f, dis);
                vertices[i + vlen] = CalcLinePoint(lv[0], lv[1] - lv[0], Width, IsPull);
                uvs[i + vlen] = new Vector2(0.75f, dis);
            }
            else
            {
                dis += Vector3.Distance(lv[i], lv[i - 1]) / (2 * Width);
                vertices[i] = CalcLinePoint(lv[i], lv[i] - lv[i - 1], -Width, IsPull);
                uvs[i] = new Vector2(0.25f, dis);
                vertices[i + vlen] = CalcLinePoint(lv[i], lv[i] - lv[i - 1], Width, IsPull);
                uvs[i + vlen] = new Vector2(0.75f, dis);
            }
        }


        int[] triangles = new int[(vlen -1) * 6];
        for (int i = 0, vi = 0; vi < vlen - 1; i += 6, vi ++)
        {
            triangles[i] = vi;
            triangles[i + 1] = vi + vlen;
            triangles[i + 2] = vi + 1;
            
            triangles[i + 3] = vi + vlen;
            triangles[i + 4] = vi + 1 + vlen;
            triangles[i + 5] = vi + 1;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        return mesh;
    }
    /// <summary>
    /// 计算线上的另外一个点
    /// </summary>
    /// <param name="start"></param>
    /// <param name="dir"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    private Vector3 CalcLinePoint(Vector3 start ,Vector3 dir, float width, bool IsPull)
    {
        Vector3 normal = Vector3.Cross(dir, new Vector3(0, 1, 0)).normalized;
        if (IsPull == false)
        {
            return start + normal * width;
        }
        else
        {
            return start + normal * width + new Vector3(0, 0.1f, 0);
        }
        
    }


}
