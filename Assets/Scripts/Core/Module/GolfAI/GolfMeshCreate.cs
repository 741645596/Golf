using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IGG.Core.Data.DataCenter.GolfAI;

public class GolfMeshCreate
{
    private int m_width = 10;
    private int m_height = 10;
    private float m_Xstep = 100.0f;
    private float m_Zstep = 100.0f;
    private Vector3 m_GridStart = Vector3.zero;
    private AreaType m_Type;
    /// <summary>
    /// 
    /// </summary>
    private List<Vector3> m_Listpt = new List<Vector3>();
    public List<Vector3> Listpt
    {
        get { return m_Listpt; }
    }
    private List<Vector2> m_Listuv = new List<Vector2>();
    private List<int> m_ListTriangle = new List<int>();
    public List<int> ListTriangle
    {
        get { return m_ListTriangle; }
    }
    /// <summary>
    /// 创建网格mesh
    /// </summary>
    /// <param name="start"></param>
    /// <param name="w"></param>
    /// <param name="h"></param>
    /// <param name="xSize"></param>
    /// <param name="zSize"></param>
    /// <returns></returns>
    public Mesh CreateMesh(int w, int h, float xSize, float zSize, Vector3 gridStart, AreaType Type)
    {
        m_width = w;
        m_height = h;
        m_Xstep = xSize;
        m_Zstep = zSize;
        m_GridStart = gridStart;
        m_Type = Type;
        Mesh mesh = new Mesh();
        Clear();
        /// 顶点列表
        for (int z = 0; z <= m_height; z++)
        {
            for (int x = 0; x <= m_width; x++)
            {
                Vector3 v = GetPos(x, z);
                m_Listpt.Add(v);
                m_Listuv.Add(GetUV(x, z));
            }
        }
        // 三角形索引列表。
        int linePointNum = m_width + 1;
        for (int z = 0; z < m_height; z++)
        {
            for (int x = 0; x < m_width; x++)
            {
                int start = z * linePointNum + x;
                // 两个三角形。
                m_ListTriangle.Add(start);
                m_ListTriangle.Add(start + linePointNum);
                m_ListTriangle.Add(start + 1);

                m_ListTriangle.Add(start + 1);
                m_ListTriangle.Add(start + linePointNum);
                m_ListTriangle.Add(start + linePointNum + 1);
            }
        }
        mesh.vertices = m_Listpt.ToArray();
        mesh.uv = m_Listuv.ToArray();
        mesh.triangles = m_ListTriangle.ToArray();
        return mesh;
    }


    /// <summary>x
    /// 产生网格上的点
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private Vector3 GetPos(float x, float z)
    {
        float vx = x * m_Xstep;
        float vz = z * m_Zstep;
        float vy = 0;
        return new Vector3(vx, vy, vz) + m_GridStart;
    }

    /// <summary>x
    /// 产生UV
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private Vector2 GetUV(float x, float z)
    {
        //float vx = x * 1.0f / this.m_width;
        //float vy = z * 1.0f / this.m_height;

        float vx = x * 1.0f;
        float vy = z * 1.0f;


        return new Vector2(vx, vy);
    }

    public void Clear()
    {
        if (m_Listpt == null)
        {
            m_Listpt = new List<Vector3>();
        }
        m_Listpt.Clear();
        if (m_Listuv == null)
        {
            m_Listuv = new List<Vector2>();
        }
        m_Listuv.Clear();
        if (m_ListTriangle == null)
        {
            m_ListTriangle = new List<int>();
        }
        m_ListTriangle.Clear();
    }
}
