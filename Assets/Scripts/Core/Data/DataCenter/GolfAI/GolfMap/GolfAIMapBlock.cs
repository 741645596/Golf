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
    /// 高尔夫球场地图数据分块
    /// </summary>
    public class GolfCourseMapBlock: IAB
    {
        public GolfCourseMapBlock() { }
        public GolfCourseMapBlock(Vector2 aa2, Vector2 bb2)
        {
            this.m_AA2 = aa2;
            this.m_BB2 = bb2;
            SetAABB3();
        }
        public GolfCourseMapBlock(Vector3 aa3, Vector3 bb3)
        {
            this.m_AA3 = aa3;
            this.m_BB3 = bb3;
            SetAABB2();
        }
        /// <summary>
        ///  该块中包含的三角形。
        /// </summary>
        private List<GolfMaptriangle> m_ListTriangle = new List<GolfMaptriangle>();
        public List<GolfMaptriangle> ListTriangle
        {
            get { return m_ListTriangle; }
        }
        /// <summary>
        /// 添加三角形
        /// </summary>
        /// <param name="TriID"></param>
        public void AddTri(GolfMaptriangle Tri)
        {
            if (m_ListTriangle.Contains(Tri) == false)
            {
                m_ListTriangle.Add(Tri);
            }
        }
        /// <summary>
        /// 计算高度范围
        /// </summary>
        public void ParseHightRange()
        {
            ParseHightRange(ListTriangle);
        }
    }
}
