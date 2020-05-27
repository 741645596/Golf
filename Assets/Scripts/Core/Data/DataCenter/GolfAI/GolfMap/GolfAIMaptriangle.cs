using System.Collections.Generic;
using UnityEngine;
using IGG.Core.Utils;

/// <summary>
/// Author  zhulin
/// Date    2019.5.6
/// Desc    GolfAI模块动态数据集
/// </summary>
namespace IGG.Core.Data.DataCenter.GolfAI
{
    /// <summary>
    /// 三角形
    /// </summary>
    public class GolfMaptriangle: IAB
    {
        /// <summary>
        /// 3个顶点索引
        /// </summary>
        private GolfAIPoint m_gp1;
        private GolfAIPoint m_gp2;
        private GolfAIPoint m_gp3;
        /// <summary>
        /// 顶点数据
        /// </summary>
        public Vector3 P1
        {
            get{ return m_gp1.Pos; }
        }

        public Vector3 P2
        {
            get{ return m_gp2.Pos; }
        }

        public Vector3 P3
        {
            get{ return m_gp3.Pos; }
        }

        private GolfAIMapPolygon m_Parent = null;
        public GolfAIMapPolygon Parent
        {
            get { return m_Parent; }
        }


        /// <summary>
        /// 构建
        /// </summary>
        public GolfMaptriangle() { }
        /// <summary>
        /// 构建
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <param name="n3"></param>
        /// <param name="type"></param>
        /// <param name="map"></param>
        public GolfMaptriangle(GolfAIPoint gp1, GolfAIPoint gp2, GolfAIPoint gp3)
        {
            this.m_gp1 = gp1;
            this.m_gp2 = gp2;
            this.m_gp3 = gp3;
            AABB.CalcAABB(this.P1, this.P2, this.P3, ref this.m_AA3, ref this.m_BB3);
            SetAABB2();
        }
        /// <summary>
        /// 设置三角形的父多边形
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(GolfAIMapPolygon parent)
        {
            this.m_Parent = parent;
        }
        /// <summary>
        /// 确定投影点p是否在三角形的投影中（xz平面的投影）
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool CheckProjectionInArea(Vector2 p)
        {
            return AABB.CheckProjectionPointinTriangle(this.P1, this.P2, this.P3, new Vector3(p.x, 0, p.y));
        }

        /// <summary>
        /// 确定投影点p是否在三角形的投影中（xz平面的投影）
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool CheckProjectionInArea(Vector3 p)
        {
            return AABB.CheckProjectionPointinTriangle(this.P1, this.P2, this.P3, p);
        }


        /// <summary>
        /// 确定p是否在三角形的内
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool CheckInArea(Vector3 p)
        {
            return AABB.CheckPointinTriangle(this.P1, this.P2, this.P3, p);
        }

        /// <summary>
        /// 求se 与三角形边的交点，se在三角形平面上，其中 s在三角形内部，e 在三角形外部，一定有交点的。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool GetLineIntersectPoint(Vector3 s, Vector3 e, ref Vector3 ret)
        {
            if (AABB.CheckIntersection(this.Parent.Normal,this.P1, this.P2, s, e, ref ret) == true)
            {
                return true;
            }
            else if (AABB.CheckIntersection(this.Parent.Normal, this.P2, this.P3, s, e, ref ret) == true)
            {
                return true;
            }
            else if (AABB.CheckIntersection(this.Parent.Normal, this.P3, this.P1, s, e, ref ret) == true)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 计算线段se是否与三角形相交，并得到相交点,se 与三角形不在同一个平面上
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool CalcIntersectPoint(Vector3 s, Vector3 e, ref Vector3 v)
        {
            // 首先判断aabb 盒子有没相交,没相交直接返回
            if (CheckContains(s, e) == false)
                return false;
            // 判断线段所在平面是否跟三角形平行，平行则无交点。
            Vector3 se = e - s;
            if (Vector3.Dot(se, this.Parent.Normal) == 0)
            {
                return false;
            }
            // 计算直线se与三角形平面的交点。
            float d = Vector3.Dot(this.P1 - s, this.Parent.Normal) / Vector3.Dot(se.normalized, this.Parent.Normal);
            v = d * se.normalized.normalized + s;
            // 判断点v 在线段上。
            Vector3 sv = v - s;
            Vector3 ev = v - e;
            // s, v 重合需要过滤掉,过滤掉原因，是发球点，或弹跳点肯定发生碰撞。
            if (sv == Vector3.zero && Vector3.Dot(se, this.Parent.Normal) > 0)
            {
                return false;
            }

            if (Vector3.Dot(sv, ev) > 0)
            {
                return false;
            }
            // 判断点v 在三角形内部。
            return CheckInArea(v);
        }


        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
        }

    }
}
