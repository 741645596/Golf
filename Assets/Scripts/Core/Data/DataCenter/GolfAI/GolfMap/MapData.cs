using UnityEngine;
using System.Collections.Generic;
using IGG.Core;
using System;
using IGG.Core.Helper;
/// <summary>
/// Author  zhulin
/// Date    2019.5.27
/// Desc    Battle模块动态数据集
/// </summary>
namespace IGG.Core.Data.DataCenter.GolfAI
{
    [Serializable]
    public class MapData
    {
        public List<SubMapData> ListMap = new List<SubMapData>();

        /// <summary>
        /// 发球点
        /// </summary>
        public Vector3 StartPos;
        /// <summary>
        /// 洞的位置
        /// </summary>
        public Vector3 HolePos;
        /// <summary>
        /// 洞的半径
        /// </summary>
        public float HoleRadius;
        /// <summary>
        /// 添加子地图
        /// </summary>
        /// <param name="s"></param>
        public void AddSubMap(SubMapData s)
        {
            if (s == null)
                return;
            if (ListMap == null)
            {
                ListMap = new List<SubMapData>();
            }
            ListMap.Add(s);
        }

        /// <summary>
        /// 保存地图数据
        /// </summary>
        /// <param name="data"></param>
        public static void SaveFile(MapData data)
        {
            string text = JsonUtility.ToJson(data);
            FileHelper.SaveTextToFile(text, Application.dataPath + "/Data/Map/"+  "GolfMap.data");
        }
        /// <summary>
        /// 加载地图数据
        /// </summary>
        /// <returns></returns>
        public static MapData Load()
        {
            string text = FileHelper.ReadTextFromFile(Application.dataPath + "/Data/Map/" + "GolfMap.data");
            return JsonUtility.FromJson<MapData>(text);
        }

        public void Clear()
        {
            ListMap.Clear();
        }
    }

    [Serializable]
    public class SubMapData
    {
        public AreaType m_Type;
        public List<Vector3> Listpt = new List<Vector3>();
        public List<int> ListTriangle = new List<int>();
        public List<PolygonData> ListPoly = new List<PolygonData>();
        /// <summary>
        /// 三角形顶点列表
        /// </summary>
        

        public SubMapData() { }
        public SubMapData(AreaType type, List<Vector3> lp, List<int> ltri)
        {
            if (lp != null && lp.Count > 0 && ltri != null && ltri.Count > 0)
            {
                this.m_Type = type;
                this.Listpt.AddRange(lp);
                this.ListTriangle.AddRange(ltri);
            }
            BuildPolygon();
        }
        /// <summary>
        /// 构建Polygon
        /// </summary>
        public void BuildPolygon()
        {
            List<PolygonData> lpoly = null;
            List <TriangleData> ltri = MakeTriangle();
            Dictionary<Vector3, List<TriangleData>> dicTri = ParseTriangle(ltri);
            foreach (List < TriangleData > listT in dicTri.Values)
            {
                lpoly = ParseTriangle2Polygon(listT);
                if (lpoly != null && lpoly.Count > 0)
                {
                    ListPoly.AddRange(lpoly);
                }
            }
        }

        /// <summary>
        /// 生成三角形列表
        /// </summary>
        /// <returns></returns>
        private List<TriangleData> MakeTriangle()
        {
            List<TriangleData> l = new List<TriangleData>();
            int count = ListTriangle.Count / 3;
            for (int i = 0; i < count; i++)
            {
                Vector3 v1 = Listpt[ListTriangle[i * 3 +1]] - Listpt[ListTriangle[i * 3]];
                Vector3 v2 = Listpt[ListTriangle[i * 3 + 2]] - Listpt[ListTriangle[i * 3]];
                Vector3 normal = Vector3.Cross(v1, v2);
                l.Add(new TriangleData(i, ListTriangle[i * 3], ListTriangle[i * 3 + 1], ListTriangle[i * 3 + 2], normal.normalized));
            }
            return l;
        }


        /// <summary>
        /// 按法线进行分类三角形
        /// </summary>
        /// <returns></returns>
        private Dictionary<Vector3, List<TriangleData>> ParseTriangle(List<TriangleData> list)
        {
            Dictionary<Vector3, List<TriangleData>> l = new Dictionary<Vector3, List<TriangleData>>();
            if (list == null || list.Count == 0)
                return l;
            foreach (TriangleData v in list)
            {
                if (l.ContainsKey(v.m_normal) == true)
                {
                    l[v.m_normal].Add(v);
                }
                else
                {
                    List<TriangleData> t = new List<TriangleData>();
                    t.Add(v);
                    l.Add(v.m_normal, t);
                }
            }
            return l;
        }

        /// <summary>
        /// 进行合并成多边形列表
        /// </summary>
        /// <returns></returns>
        private List<PolygonData> ParseTriangle2Polygon(List<TriangleData> list)
        {
            List<PolygonData> l = new List<PolygonData>();
            if (list == null || list.Count == 0 )
                return l;
            PolygonData cur = null;
            for (int i = 0; i < list.Count; i++)
            {
                if (cur == null)
                {
                    cur = new PolygonData(list[i].m_Index, list[i].m_p1, list[i].m_p2, list[i].m_p3, list[i].m_normal);
                    l.Add(cur);
                    list.RemoveAt(i);
                    i = -1;
                }
                else
                {
                    if (cur.CheckLinkTri(list[i], ListTriangle) == true)
                    {
                        cur.AddTri(list[i].m_Index, list[i].m_p1, list[i].m_p2, list[i].m_p3);
                        list.RemoveAt(i);
                        i = -1;
                    }
                    else
                    {
                        // 最后一个了，这个多边形已经over了
                        if (i == list.Count - 1)
                        {
                            cur = null;
                            i = -1;
                        }
                    }
                }
            }
            return l;
        }
    }

    [Serializable]
    public class PolygonData
    {
        public PolygonData() { }
        public PolygonData(int TriIndex, int n1, int n2, int n3, Vector3 normal)
        {
            m_normal = normal;
            AddTri(TriIndex,n1, n2, n3);
        }
        /// <summary>
        /// 法线
        /// </summary>
        public Vector3 m_normal;
        /// <summary>
        /// 包含的三角形索引
        /// </summary>
        public List<int> ListTriangle = new List<int>();
        /// <summary>
        /// 包含的顶点列表
        /// </summary>
        public List<int> ListPt = new List<int>();
        /// <summary>
        /// 添加三角形
        /// </summary>
        /// <param name="TriIndex"></param>
        public void AddTri(int TriIndex, int n1, int n2, int n3)
        {
            if (ListTriangle.Contains(TriIndex) == false)
            {
                ListTriangle.Add(TriIndex);
                AddPoint(n1, n2, n3);
            }
        }
        /// <summary>
        /// 添加顶点列表
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <param name="n3"></param>
        private void AddPoint(int n1, int n2, int n3)
        {
            if (ListPt.Count == 0)
            {
                ListPt.Add(n1);
                ListPt.Add(n2);
                ListPt.Add(n3);
            }
            else
            {
                //异常情况，不可能出现。
                if (ListPt.Count < 3)
                    return;
                int OtherP = 0;
                if (ListPt.Count == 3)
                {
                    for (int i = 0; i < ListPt.Count; i++)
                    {
                        if (IsSameSide(ListPt[i], ListPt[(i + 1) / ListPt.Count], n1, n2, n3, ref OtherP) == true)
                        {
                            ListPt.Insert(i +1, OtherP);
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ListPt.Count; i++)
                    {
                        if (IsSameSide(ListPt[i], ListPt[(i + 1) / ListPt.Count], n1, n2, n3, ref OtherP) == true)
                        {
                            // 共2条边
                            if (OtherP == ListPt[(i + 2) / ListPt.Count])
                            {
                                ListPt.RemoveAt((i + 1) / ListPt.Count);
                            }
                            else if (i == 0 && OtherP == ListPt[ListPt.Count - 1])
                            {
                                ListPt.RemoveAt(0);
                            }
                            else if (i > 0 && OtherP == ListPt[i - 1])
                            {
                                ListPt.RemoveAt(i);
                            }
                            else
                            {
                                ListPt.Insert(i + 1, OtherP);
                            }
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 计算与三角形的共边
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <param name="n3"></param>
        /// <param name="OtherP"></param>
        /// <returns></returns>
        private bool IsSameSide(int t1, int t2, int n1, int n2, int n3, ref int OtherP)
        {
            if (IsSameSide(t1, t2, n1, n2) == true)
            {
                OtherP = n3;
                return true;
            }
            else if (IsSameSide(t1, t2, n1, n3) == true)
            {
                OtherP = n2;
                return true;
            }
            else if (IsSameSide(t1, t2, n2, n3) == true)
            {
                OtherP = n1;
                return true;
            }
            return false;
        }
        private bool IsSameSide(int t1, int t2, int n1, int n2)
        {
            if (IsSameSide(t1,t2, n1) == true && IsSameSide(t1, t2, n2))
                return true;
            else return false;
        }
        private bool IsSameSide(int t1, int t2, int n1)
        {
            if (t1 == n1 || t2 == n1)
                return true;
            else return false;
        }
        /// <summary>
        /// 确定是否为邻接三角形
        /// </summary>
        /// <param name="tri"></param>
        /// <returns></returns>
        public bool CheckLinkTri(TriangleData tri, List<int> ListIndex)
        {
            if (tri == null)
            {
                return false;
            }
            List<int> lpoint = GetPointList(ListIndex);
            if (m_normal != tri.m_normal)
                return false;
            int total = 0;
            if (lpoint.Contains(tri.m_p1) == true )
            {
                total++;
            }
            if (lpoint.Contains(tri.m_p2) == true)
            {
                total++;
            }
            if (lpoint.Contains(tri.m_p3) == true)
            {
                total++;
            }
            if (total >= 2)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取多边形包含的顶点列表
        /// </summary>
        /// <returns></returns>
        private List<int> GetPointList(List<int> ListIndex)
        {
            List<int> l = new List<int>();
            foreach (int i in ListTriangle)
            {
                int v1 = ListIndex[i * 3];
                int v2 = ListIndex[i * 3 + 1];
                int v3 = ListIndex[i * 3 + 2];
                if (l.Contains(v1) == false)
                {
                    l.Add(v1);
                }
                if (l.Contains(v2) == false)
                {
                    l.Add(v2);
                }
                if (l.Contains(v3) == false)
                {
                    l.Add(v3);
                }
            }
            return l;
        }


    }
    /// <summary>
    /// 三角形
    /// </summary>
    public class TriangleData
    {
        public TriangleData() { }
        public TriangleData(int index,int n1,int n2, int n3, Vector3 normal)
        {
            this.m_p1 = n1;
            this.m_p2 = n2;
            this.m_p3 = n3;
            this.m_normal = normal;
            this.m_Index = index;
        }
        /// <summary>
        /// 法线
        /// </summary>
        public Vector3 m_normal;
        public int m_p1;
        public int m_p2;
        public int m_p3;
        /// <summary>
        /// 三角形索引
        /// </summary>
        public int m_Index;
    }
}

