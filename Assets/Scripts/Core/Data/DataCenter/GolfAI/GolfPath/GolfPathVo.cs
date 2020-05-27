using IGG.Core.Data.Config;
using IGG.Core.Data.DataCenter.Battle;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author  zhulin
/// Date    2019.5.6
/// Desc    GolfAI模块动态数据集
/// </summary>
namespace IGG.Core.Data.DataCenter.GolfAI
{
    public class GolfPathInfo
    {
        public GolfPathInfo(GolfCourseMap map)
        {
            m_CurMap = map;
        }
        private GolfCourseMap m_CurMap = null;
        /// <summary>
        /// 路径点
        /// </summary>
        public List<GolfPathPoint> ListPt = null;
        /// <summary>
        /// 反弹点索引
        /// </summary>
        public List<int> ListReboundPt = new List<int>();
        /// <summary>
        /// 靠近洞的点
        /// </summary>
        public List<int> ListHolePt = new List<int>();
        /// <summary>
        /// 第一个反弹点，也即就是五环的位置。
        /// </summary>
        public Vector3 FirstReboundPt;
        /// <summary>
        /// 起跳速度，发射速度
        /// </summary>
        public Vector3 LauchSpeed;
        /// <summary>
        /// 导轨
        /// </summary>
        public float PathGuide;
        /// <summary>
        /// 添加一个路径点
        /// </summary>
        /// <param name="pt"></param>
        public void AddPathPoint(GolfPathPoint pt)
        {
            if (pt == null)
                return;
            if (ListPt == null)
            {
                ListPt = new List<GolfPathPoint>();
            }
            if (pt is GolfBouncePoint)
            {
                ListReboundPt.Add(ListPt.Count);
            }
            /// 接近洞的点。
            ListPt.Add(pt);

        }

        public GolfPathPoint GetPathPoint(int index)
        {
            if (index >= 0 && index < ListPt.Count)
            {
                return ListPt[index];
            }
            return null;
        }

        /// <summary>
        /// 获取停止点
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRuseltPos()
        {
            if (ListPt == null || ListPt.Count == 0)
            {
                return Vector3.zero;
            }
            else
            {
                return ListPt[ListPt.Count - 1].Position;
            }
        }

        /// <summary>
        /// 判断路线有没经过球洞
        /// </summary>
        /// <param name="lRoad"></param>
        /// <returns></returns>
        public bool CheckCrossHole()
        {
            if (m_CurMap == null || ListPt == null || ListPt.Count < 2)
            {
                return false;
            }
            for (int i = 0; i < ListPt.Count - 1; i++)
            {
                if (m_CurMap.CheckCrossHole(ListPt[i].Position, ListPt[i + 1].Position) == true)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 清理数据
        /// </summary>
        public void Clear()
        {
            if (ListPt != null)
            {
                ListPt.Clear();
                ListPt = null;
            }
        }
    }

    /// <summary>
    /// 高尔夫球路径点基类
    /// </summary>
    public class GolfPathPoint
    {
        /// <summary>
        /// 坐标
        /// </summary>
        public Vector3 Position = Vector3.zero;
        /// <summary>
        /// 该点速度
        /// </summary>
        public Vector3 Velocity = Vector3.zero;
        /// <summary>
        /// 旋转(四元数计算)
        /// </summary>
        public Quaternion Rotation  = Quaternion.identity;
        /// <summary>
        /// 从上一个点运动到改点的时间间隔
        /// </summary>
        public float Interval  = 0;
        /// <summary>
        /// 所在区域类型
        /// </summary>
        public AreaType AType  = AreaType.TeeingGround;
        /// <summary>
        /// 小球状态
        /// </summary>
        public BallRunStatus Status  = BallRunStatus.Stop;
        /// <summary>
        /// 相机事件
        /// </summary>
        public string EventType  = "";
        /// <summary>
        /// 相机事件参数
        /// </summary>
        public object EventParam  = null;
        /// <summary>
        /// 球与球洞的关系
        /// </summary>
        public BallType BallType  = BallType.OutHole;
    }

    /// <summary>
    /// 高尔夫球曲线路径点
    /// </summary>
    public class GolfFlyPoint : GolfPathPoint
    {
        
    }

    /// <summary>
    /// 高尔夫球碰撞点
    /// </summary>
    public class GolfBouncePoint : GolfPathPoint
    {
        /// <summary>
        /// 反射速度
        /// </summary>
        public Vector3 OutVelocity  = Vector3.zero;
        /// <summary>
        /// 碰撞物类型
        /// </summary>
        public CollisionType ColType  = CollisionType.Normal;
        /// <summary>
        /// 是否反弹
        /// </summary>
        public bool IsRebound  = false;
        /// <summary>
        /// 是否可以滚动
        /// </summary>
        public bool IsRoll  = false;
        /// <summary>
        /// 碰撞到的多边形
        /// </summary>
        public GolfAIMapPolygon Poly  = null;
    }

    /// <summary>
    /// 高尔夫滚动路径点
    /// </summary>
    public class GolfRollPoint : GolfPathPoint
    {
        public Vector3 Acceleration  = Vector3.zero;
        /// <summary>
        /// 是否出界
        /// </summary>
        public CollisionType ColType  = CollisionType.Normal;
        /// <summary>
        /// 是否可以滚动
        /// </summary>
        public bool IsRoll  = false;
        /// <summary>
        /// 碰撞到的多边形
        /// </summary>
        public GolfAIMapPolygon Poly  = null;
    }
}
