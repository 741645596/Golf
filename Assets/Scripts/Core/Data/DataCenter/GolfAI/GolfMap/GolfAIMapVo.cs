using System.Collections.Generic;
using UnityEngine;

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
    public class GolfCourseMap:IAB
    {
        public GolfCourseMap() { }
        public GolfCourseMap(MapData data)
        {
            if (data == null)
                return;
            foreach (SubMapData d in data.ListMap)
            {
                GolfAISubMap submap = new GolfAISubMap();
                submap.Create(d);
                this.AddSubMap(submap);
            }
            this.SetGolfHale(data.HolePos, data.HoleRadius);
            this.m_StartPos = data.StartPos;
        }

        private List<GolfAISubMap> m_ListSubMap = null;
        /// <summary>
        /// 球洞位置
        /// </summary>
        private Vector3 m_BallHolePos;
        public Vector3 BallHolePos
        {
            get { return m_BallHolePos; }
        }

        private Vector3 m_StartPos;
        public Vector3 StartPos
        {
            get { return m_StartPos; }
        }
        /// <summary>
        /// 球洞半径，为一个常量
        /// </summary>
        private float m_BallHoleRadius;
        public float BallHoleRadius
        {
            get { return m_BallHoleRadius; }
        }
        /// <summary>
        /// 设置球洞数据
        /// </summary>
        /// <param name="HolePos"></param>
        /// <param name="Radius"></param>
        public void SetGolfHale(Vector3 HolePos, float Radius)
        {
            m_BallHolePos = HolePos;
            m_BallHoleRadius = Radius;
        }

        /// <summary>
        /// 构建四叉树
        /// </summary>
        private QuadTree m_Qtree = null;

        /// <summary>
        /// 添加高尔夫地图子元素。
        /// </summary>
        public void AddSubMap(GolfAISubMap subMap)
        {
            if (subMap == null)
            {
                return;
            }
            else
            {
                if (m_ListSubMap == null)
                {
                    m_ListSubMap = new List<GolfAISubMap>();
                }
                if (m_ListSubMap.Contains(subMap) == false)
                {
                    m_ListSubMap.Add(subMap);
                }
            }

        }
        /// <summary>
        /// 通过索引获取三角形
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public GolfMaptriangle GetMapTriangle(Index2 index)
        {
            GolfAISubMap sm = GetSubMap(index.MapID);
            if (sm != null)
            {
                return sm.GetGolfMaptriangle((int)index.TriID);
            }
            return null;
        }


        /// <summary>
        /// 获取子地图
        /// </summary>
        /// <param name="MapID"></param>
        /// <returns></returns>
        public GolfAISubMap GetSubMap(uint MapID)
        {
            if (m_ListSubMap == null || m_ListSubMap.Count == 0)
                return null;
            if (MapID >= m_ListSubMap.Count)
                return null;
            return m_ListSubMap[(int)MapID];
        }

        /// <summary>
        /// 构建地图
        /// </summary>
        public void BuildMap()
        {
            CalcAABB(m_ListSubMap);
            //
            m_Qtree = new QuadTree(this.AA3, this.BB3);
            List<Index2> list = new List<Index2>();
            int Count = m_ListSubMap.Count;
            for (int  MapID = 0; MapID < Count; MapID++)
            {
                int total = m_ListSubMap[MapID].ListTriangle.Count;
                for (int Trid = 0; Trid < total; Trid++)
                {
                    list.Add(new Index2((uint)MapID, (uint)Trid));
                }
            }
            m_Qtree.Create(list, this);
            list.Clear();
            list = null;
        }
        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            if (m_ListSubMap != null)
            {
                foreach (GolfAISubMap map in m_ListSubMap)
                {
                    if (map != null)
                    {
                        map.Clear();
                    }
                }
                m_ListSubMap.Clear();
                m_ListSubMap = null;
            }

            if (m_Qtree != null)
            {
                m_Qtree.Clear();
                m_Qtree = null;
            }
        }
        public bool CheckCollisionPoint(Vector3 s, Vector3 e, ref GolfMapFlyCollision ret)
        {
            if (s.Equals(e) == true)
            {
                return false;
            }
            
            if (CheckIsOut(s, e) == true || m_Qtree == null )
            {
                if (ret == null)
                {
                    ret = new GolfMapFlyCollision();
                }
                ret.Point = e;
                ret.ColType = CollisionType.Out;
                return true;
            }

            QuadTree tree = m_Qtree.GetTree(s, e);
            if (tree == null)
            {
                if (ret == null)
                {
                    ret = new GolfMapFlyCollision();
                }
                ret.Point = e;
                ret.ColType = CollisionType.Out;
                return true;
            }
            else
            {
                Vector3 Hitpoint = Vector3.zero;
                List<Index2> l = tree.GetAllTri(s, e);
                if (l == null || l.Count == 0)
                {
                    return false;
                }
                foreach (Index2 v in l)
                {
                    GolfMaptriangle tri = GetMapTriangle(v);
                    if (tri.CalcIntersectPoint(s, e, ref Hitpoint) == true)
                    {
                        if (ret == null)
                        {
                            ret = new GolfMapFlyCollision();
                        }
                        ret.Point = Hitpoint;
                        ret.Type = GetSubMap(v.MapID).Type;
                        ret.Poly = tri.Parent;
                        ret.ColType = CollisionType.Normal;
                        ret.Normal = ret.Poly.Normal;
                        return true;
                    }
                }
                
            }
            if (s.y >= 0 && e.y < 0)
            {
                if (s.y >= 0 && e.y < 0)
                {
                    Debug.Log("XXXXXXXXXXXXXPPPPP:");
                    Debug.Log("s:" + s.x + "," + s.y + "," + s.z);
                    Debug.Log("e:" + e.x + "," + e.y + "," + e.z);
                }
            }
            return false;
        }


        /// <summary>
        /// 获取滚动进入的邻边多边形形
        /// </summary>
        /// <param name="s">起点，s点在目标三角形内</param>
        /// <param name="e">终点</param>
        /// <param name="TargetTri">目标三角形</param>
        /// <param name="collision"></param>
        /// <returns>true：有进入 false 未进入</returns>
        public bool CheckIntoNearTriangle(Vector3 s, Vector3 e, GolfAIMapPolygon PolyTarget, ref GolfIntoTriCollision collision)
        {
            Vector3 hitPoint = Vector3.zero;
            if (PolyTarget == null)
                return false;
            // 判断是否在多边形内部
            if (PolyTarget.CheckProjectionInArea(e) == true)
            {
                return false;
            }
            //
            if (collision == null)
            {
                collision = new GolfIntoTriCollision();
            }
            
            collision.ColType = CollisionType.Out;
            List<Index2> l = m_Qtree.GetAllTri(e);
            if (l == null || l.Count == 0)
                return false;
            foreach (Index2 v in l)
            {
                GolfMaptriangle tri = GetMapTriangle(v);
                if (tri.Parent == PolyTarget)
                    continue;
                if (tri.GetLineIntersectPoint(s, e, ref hitPoint) == true)
                {
                    collision.Poly = tri.Parent;
                    collision.Point = hitPoint;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 搜索点在的地图块及三角形
        /// </summary>
        /// <returns></returns>
        public bool SearchMapTriangle(Vector3 Point, ref SearchResult result)
        {
            Vector2 p = new Vector2(Point.x, Point.z);
            if (this.CheckProjectionInArea(p) == false)
            {
                return false;
            }
            List<Index2> l = m_Qtree.GetAllTri(Point);
            if (l == null || l.Count == 0)
                return false;
            foreach (Index2 v in l)
            {
                GolfMaptriangle tri = GetMapTriangle(v);
                if (tri.CheckProjectionInArea(Point) == true)
                {
                    if (result == null)
                    {
                        result = new SearchResult();
                    }
                    result.Poly = tri.Parent;
                    result.Type = GetSubMap(v.MapID).Type;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断是否出界
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool CheckIsOut(Vector3 s, Vector3 e)
        {
            // 排除与整个地图不相交的情况。
            if (this.CheckProjectionContains(new Vector2(s.x, s.z), new Vector2(e.x, e.z)) == false)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取相交的地图。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private List<GolfAISubMap> GetCollisonMap(Vector3 s, Vector3 e)
        {
            List<GolfAISubMap> l = new List<GolfAISubMap>();
            if (m_ListSubMap != null)
            {
                foreach (GolfAISubMap map in m_ListSubMap)
                {
                    // 排除与整个地图不相交的情况。
                    if (map.CheckContains(s, e) == true)
                    {
                        l.Add(map);
                    }
                }
            }
            return l;
        }



        /// <summary>
        /// 获取投影到的子地图
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private List<GolfAISubMap> GetProjectionMap(Vector2 p , GolfAISubMap Exitmap)
        {
            List<GolfAISubMap> l = new List<GolfAISubMap>();
            if (m_ListSubMap != null)
            {
                foreach (GolfAISubMap map in m_ListSubMap)
                {
                    if (map == Exitmap)
                        continue;
                    // 排除与整个地图不相交的情况。
                    if (map.CheckProjectionInArea(p) == true)
                    {
                        l.Add(map);
                    }
                }
            }
            return l;
        }

        /// <summary>
        /// 判断有没经过球洞
        /// </summary>
        /// <returns></returns>
        public bool CheckCrossHole(Vector3 s, Vector3 e)
        {
            Vector2 s1 = new Vector2(s.x, s.z);
            Vector2 e1 = new Vector2(e.x, e.z);
            Vector2 vHole = new Vector2(m_BallHolePos.x, m_BallHolePos.z);

            float value = Vector2.Dot(e1 - s1, vHole - s1);
            if (value <= 0)
            {
                float dis = (vHole - s1).magnitude;
                if (dis <= m_BallHoleRadius)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                value = Vector2.Dot(s1 - e1, vHole - e1);
                if (value <= 0)
                {
                    float dis = (vHole - e1).magnitude;
                    if (dis <= m_BallHoleRadius)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    float d2 = (vHole - e1).sqrMagnitude - value * value;
                    if (d2 <= m_BallHoleRadius * m_BallHoleRadius)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }


    }
    /// <summary>
    /// 碰撞类型
    /// </summary>
    public enum CollisionType
    {
        Normal   = 0,    // 球场地面
        Hole     = 1,    // 碰到球洞了
        Flag     = 2,    // 碰到旗帜了
        Obstacle = 3,    // 障碍物
        Out      = 4,    // 出界了
    }
    /// <summary>
    /// 飞行线路与球场碰撞信息
    /// </summary>
    public class GolfMapFlyCollision
    {
        /// <summary>
        /// 碰撞类型
        /// </summary>
        public CollisionType ColType;
        /// <summary>
        /// 碰撞地形类型
        /// </summary>
        public AreaType Type;
        /// <summary>
        /// 碰撞点
        /// </summary>
        public Vector3 Point;
        /// <summary>
        /// 法线，碰撞到球场，或球洞的时候时候，就是Tri的法线
        /// </summary>
        public Vector3 Normal;
        /// <summary>
        /// 碰撞到的多边形
        /// </summary>
        public GolfAIMapPolygon Poly;
    }

    /// <summary>
    /// 高尔球滚动进入的三角面
    /// </summary>
    public class GolfIntoTriCollision
    {
        /// <summary>
        /// 碰撞类型
        /// </summary>
        public CollisionType ColType;
        /// <summary>
        /// 碰撞地形类型
        /// </summary>
        public AreaType Type;
        /// <summary>
        /// 进入点
        /// </summary>
        public Vector3 Point;
        /// <summary>
        /// 法线，碰撞到球场，或球洞的时候时候，就是Tri的法线
        /// </summary>
        public Vector3 Normal;
        /// <summary>
        /// 碰撞到的多边形
        /// </summary>
        public GolfAIMapPolygon Poly;
    }


    /// <summary>
    /// 搜索返回
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// 碰撞地形类型
        /// </summary>
        public AreaType Type;
        /// <summary>
        /// 碰撞到的多边形
        /// </summary>
        public GolfAIMapPolygon Poly;
    }


    /// <summary>
    /// 球场区域类型，高尔夫球场专业术语
    /// </summary>
    public enum AreaType
    {
        /// <summary>
        /// 发球区
        /// </summary>
        TeeingGround = 0,
        /// <summary>
        /// 球道
        /// </summary>
        Fairway = 1,
        /// <summary>
        /// 长草区
        /// </summary>
        Rough = 2,
        /// <summary>
        /// 沙坑
        /// </summary>
        SandBunker = 3,
        /// <summary>
        /// 果岭边缘
        /// </summary>
        PuttingGreenLine = 4,
        /// <summary>
        /// 果岭
        /// </summary>
        PuttingGreen = 5,
        /// <summary>
        /// 水里
        /// </summary>
        WaterHazard = 6,    
        /// <summary>
        /// 界外
        /// </summary>
        OutOfBounds = 7,
        /// <summary>
        /// 球洞
        /// </summary>
        InHole = 8,
    }
}
