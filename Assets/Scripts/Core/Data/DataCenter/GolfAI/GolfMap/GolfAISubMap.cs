using System.Collections.Generic;
using UnityEngine;
using IGG.Core.Geom;

/// <summary>
/// Author  zhulin
/// Date    2019.5.6
/// Desc    GolfAI模块动态数据集
/// </summary>
namespace IGG.Core.Data.DataCenter.GolfAI
{
    /// <summary>
    /// 高尔夫球场地图数据结构
    /// </summary>
    public class GolfAISubMap : IAB
    {
        /// <summary>
        /// 顶点列表.
        /// </summary>
        private List<GolfAIPoint> m_ListPoint = null;
        /// <summary>
        /// 三角形列表
        /// </summary>
        private List<GolfMaptriangle> m_ListTriangle = null;
        public List<GolfMaptriangle> ListTriangle
        {
            get { return m_ListTriangle; }
        }
        /// <summary>
        /// 多边形列表
        /// </summary>
        public List<GolfAIMapPolygon> m_ListPoly = null;
        public List<GolfAIMapPolygon> ListPoly
        {
            get { return m_ListPoly; }
        }


        /// <summary>
        /// 类型
        /// </summary>
        private AreaType m_Type;
        public AreaType Type
        { get { return m_Type; } }

        /// <summary>
        /// 高尔夫球场地图数据结构
        /// </summary>
        /// <param name="lp"></param>
        /// <param name="lt"></param>
        public void Create(List<GolfAIPoint> lp, List<GolfMaptriangle> lt, List<GolfAIMapPolygon>lpoly, AreaType type)
        {
            this.m_Type = type;
            if (lt != null && lp != null)
            {
                m_ListTriangle = lt;
                m_ListPoint = lp;
                CalcAABB(m_ListTriangle);
            }
            if (lpoly != null)
            {
                m_ListPoly = lpoly;
            }
        }
        /// <summary>
        /// 构建
        /// </summary>
        /// <param name="data"></param>
        public void Create(SubMapData data)
        {
            if (data == null)
                return ;
            List<GolfAIPoint> lp = new List<GolfAIPoint>();
            for (int i = 0;i < data.Listpt.Count; i++)
            {
                lp.Add(new GolfAIPoint(data.Listpt[i]));
            }

            List<GolfMaptriangle> ListT = new List<GolfMaptriangle>();
            int total = data.ListTriangle.Count / 3;
            for (int i = 0; i < total; i++)
            {
                int index = i * 3;
                GolfMaptriangle c = new GolfMaptriangle(lp[data.ListTriangle[index]], lp[data.ListTriangle[index + 1]], lp[data.ListTriangle[index + 2]]);
                ListT.Add(c);
            }

            List<GolfAIMapPolygon> lpoly = new List<GolfAIMapPolygon>();
            total = data.ListPoly.Count;
            for (int i = 0; i < total; i++)
            {
                List<GolfMaptriangle> l = new List<GolfMaptriangle>();
                for (int j = 0; j < data.ListPoly[i].ListTriangle.Count; j++)
                {
                    l.Add(ListT[data.ListPoly[i].ListTriangle[j]]);
                }
                List<GolfAIPoint> ll = new List<GolfAIPoint>();
                for (int j = 0; j < data.ListPoly[i].ListPt.Count; j++)
                {
                    ll.Add(lp[data.ListPoly[i].ListPt[j]]);
                }
                lpoly.Add(new GolfAIMapPolygon(l, ll, data.ListPoly[i].m_normal));
            }
            Create(lp, ListT, lpoly, data.m_Type);
        }
        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            if (m_ListTriangle != null)
            {
                foreach (GolfMaptriangle v in m_ListTriangle)
                {
                    v.Clear();
                }
                m_ListTriangle.Clear();
                m_ListTriangle = null;
            }
            if (m_ListPoint != null)
            {
                foreach (GolfAIPoint p in m_ListPoint)
                {
                    p.Clear();
                }
                m_ListPoint.Clear();
                m_ListPoint = null;
            }
        }

        /// <summary>
        /// 获取顶点数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 GetPoint(int index)
        {
            if (m_ListPoint == null || m_ListPoint.Count <= index)
            {
                return Vector3.zero;
            }
            return m_ListPoint[index].Pos;
        }


        /// <summary>
        /// 获取三角形
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public GolfMaptriangle GetGolfMaptriangle(int index)
        {
            if (m_ListTriangle == null || m_ListTriangle.Count <= index)
            {
                return null;
            }
            return m_ListTriangle[index];
        }
    }
}
