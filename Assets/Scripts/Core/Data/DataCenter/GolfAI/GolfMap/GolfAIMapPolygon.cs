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
    /// 凸多边形
    /// </summary>
    public class GolfAIMapPolygon:IAB
    {
        /// <summary>
        /// 包含的顶点列表
        /// </summary>
        private List<GolfAIPoint> m_ListPt = null;
        /// <summary>
        /// 包含的三角形列表
        /// </summary>
        private List<GolfMaptriangle> m_ListTri = null;
        /// <summary>
        /// 法线
        /// </summary>
        public Vector3 Normal;
        /// <summary>
        /// 构建
        /// </summary>
        public GolfAIMapPolygon() { }
        /// <summary>
        /// 构建
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <param name="n3"></param>
        /// <param name="type"></param>
        public GolfAIMapPolygon(List<GolfMaptriangle> listTri, List<GolfAIPoint> ListPt, Vector3 normal)
        {
            if (ListPt != null && ListPt.Count > 0)
            {
                m_ListPt = ListPt;
            }
            if (listTri != null && listTri.Count > 0)
            {
                for (int i = 0; i < listTri.Count; i++)
                {
                    if (listTri[i] != null)
                    {
                        listTri[i].SetParent(this);
                    }
                }
                m_ListTri = listTri;
                this.Normal = normal;
                CalcAABB(m_ListTri);
            }
        }



        /// <summary>
        /// 确定投影点p是否在多边形的投影中（xz平面的投影）
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool CheckProjectionInArea(Vector2 p)
        {
            if (m_ListTri == null || m_ListTri.Count == 0)
                return false;
            for (int i = 0; i < m_ListTri.Count; i++)
            {
                if (m_ListTri != null && m_ListTri[i].CheckProjectionInArea(p) == true)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 确定投影点p是否在三角形的投影中（xz平面的投影）
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool CheckProjectionInArea(Vector3 p)
        {
            return CheckProjectionInArea(new Vector2(p.x, p.z));
        }


        /// <summary>
        /// 确定p是否在三角形的内
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool CheckInArea(Vector3 p)
        {
            if (m_ListTri == null || m_ListTri.Count == 0)
                return false;
            for (int i = 0; i < m_ListTri.Count; i++)
            {
                if (m_ListTri != null && m_ListTri[i].CheckInArea(p) == true)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 2个ab的投影是否包含
        /// </summary>
        /// <param name="ab"></param>
        /// <returns></returns>
        public override bool CheckProjectionContains(IAB ab)
        {
            if (ab == null || m_ListTri == null || m_ListTri.Count == 0)
                return false;
            for (int i = 0; i < m_ListTri.Count; i++)
            {
                if (m_ListTri != null && m_ListTri[i].CheckProjectionContains(ab) == true)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 2个ab的投影是否包含
        /// </summary>
        /// <param name="ab"></param>
        /// <returns></returns>
        public override bool CheckProjectionContains(Vector2 AA, Vector2 BB)
        {
            if (m_ListTri == null || m_ListTri.Count == 0)
                return false;
            for (int i = 0; i < m_ListTri.Count; i++)
            {
                if (m_ListTri != null && m_ListTri[i].CheckProjectionContains(AA, BB) == true)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 2个ab的投影是否包含
        /// </summary>
        /// <param name="ab"></param>
        /// <returns></returns>
        public override bool CheckContains(IAB ab)
        {
            if (ab == null || m_ListTri == null || m_ListTri.Count == 0)
                return false;
            for (int i = 0; i < m_ListTri.Count; i++)
            {
                if (m_ListTri != null && m_ListTri[i].CheckContains(ab) == true)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 2个ab的投影是否包含
        /// </summary>
        /// <param name="ab"></param>
        /// <returns></returns>
        public override bool CheckContains(Vector3 AA, Vector3 BB)
        {
            if (m_ListTri == null || m_ListTri.Count == 0)
                return false;
            for (int i = 0; i < m_ListTri.Count; i++)
            {
                if (m_ListTri != null && m_ListTri[i].CheckContains(AA, BB) == true)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 求se 与多边形边的交点，se在多边形平面上，其中 s在多边形内部，e 在多边形外部，一定有交点的。用于切换多边形处理
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool GetLineIntersectPoint(Vector3 s, Vector3 e, ref Vector3 ret)
        {
            if (m_ListPt == null || m_ListPt.Count < 3)
                return false;
            for (int i = 0; i < m_ListPt.Count -1; i++)
            {
                if (AABB.CheckIntersection(this.Normal, m_ListPt[i].Pos, m_ListPt[i + 1].Pos, s, e, ref ret) == true)
                {
                    return true;
                }
            }
            if (AABB.CheckIntersection(this.Normal, m_ListPt[m_ListPt.Count - 1].Pos, m_ListPt[0].Pos, s, e, ref ret) == true)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 计算线段se是否与多边形相交，并得到相交点,se 与多边形不在同一个平面上，用于反弹处理
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
            if (Vector3.Dot(se, this.Normal) == 0)
            {
                return false;
            }
            // 计算直线se与三角形平面的交点。
            float d = Vector3.Dot(this.m_ListPt[0].Pos - s, this.Normal) / Vector3.Dot(se.normalized, this.Normal);
            v = d * se.normalized.normalized + s;
            // 判断点v 在线段上。
            Vector3 sv = v - s;
            Vector3 ev = v - e;
            // s, v 重合需要过滤掉.
            if (sv == Vector3.zero)
                return false;

            if (Vector3.Dot(sv, ev) > 0)
            {
                return false;
            }
            // 判断点v 在三角形内部。
            return CheckInArea(v);
        }
        /// <summary>
        /// 获取三角面上的加速度。
        /// </summary>
        /// <param name="g">重力加速度</param>
        /// <param name="FrictionParam">摩擦力参数</param>
        /// <returns></returns>
        public Vector3 GetAcceleration(Vector3 v, float g, float FrictionParam)
        {
            Vector3 gdir = new Vector3(0, -1, 0);
            float angle = 180 - Vector3.Angle(gdir, this.Normal);
            // 转成弧度制
            angle = angle / 180.0f * Mathf.PI;
            //弹力
            float Fya = g * Mathf.Cos(angle);
            // 重力
            Vector3 Fg = g * new Vector3(0, -1, 0);
            // 弹力，法线方向。
            Vector3 Ft = this.Normal * Fya;
            Vector3 s = Ft + Fg;
            // 判断是否禁止
            if (v.magnitude > LogicConstantData.FLOAT_PERCISION_FIX)
            {
                // 摩擦力，速度的反方向
                Vector3 Fm = -1 * v.normalized * Fya * FrictionParam;
                if (Fm + Ft + Fg == Vector3.zero)
                {
                    Debug.Log("1");
                }
                // 三力之和
                return Fm + Ft + Fg;
            }
            else
            {
                Vector3 Fh = Fg + Ft;
                float fh = Fh.magnitude;
                float fm = Ft.magnitude * FrictionParam;
                if (fh > fm) //合力大于摩擦力 表示有加速度。
                {
                    // 沿着合力的方向，
                    return Fh.normalized * (fh - fm);
                }
                else
                {
                    return Vector3.zero;
                }
            }
        }
        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            if (m_ListTri != null)
            {
                m_ListTri.Clear();
                m_ListTri = null;
            }
        }

    }
}
