using IGG.Core.Data.Config;
using System.Collections.Generic;
using UnityEngine;
using IGG.Core.Data.DataCenter.Battle;

/// <summary>
/// Author  zhulin
/// Date    2019.5.6
/// Desc    GolfAI模块动态数据集
/// </summary>
namespace IGG.Core.Data.DataCenter.GolfAI
{
    /// <summary>
    /// 高尔夫运动路径
    /// </summary>
    public class GolfMotionPath
    {
        public GolfPathInfo PathInfo;
        public Vector3 BattingVelocity = Vector3.zero;
        /// <summary>
        /// 旋球临时数据
        /// </summary>
        public List<float> SpinValueList = null;
        //最大旋球速度
        public Dictionary<int, List<float>> SpinDic = null;

        public List<float> MaxSpinList = null;
        public float SpinLeftRight = 0;
        /// <summary>
        /// 曲球临时数据
        /// </summary>
        //曲球角度
        public float HookAngle = 0;
        /// <summary>
        /// 当前所在区域
        /// </summary>
        public AreaType CurrentAreaType = AreaType.TeeingGround;

        /// <summary>
        /// 高尔夫飞行阶段
        /// </summary>
        public GolfMotionFly Fly { get; set; } = null;
        /// <summary>
        /// 高尔夫弹跳阶段
        /// </summary>
        public GolfMotionBounce Bounce { get; set; } = null;
        /// <summary>
        /// 高尔夫滚动阶段
        /// </summary>
        public GolfMotionRoll Roll { get; set; } = null;
        /// <summary>
        /// 第一段飞行最高点
        /// </summary>
        public float FlyHighestPointHeight = 0;

        private GolfCourseMap m_CurMap = null;

        public GolfMotionPath(GolfCourseMap map)
        {
            m_CurMap = map;
            Init();
        }

        public void Init()
        {
            PathInfo = new GolfPathInfo(m_CurMap);
            PathInfo.ListPt = new List<GolfPathPoint>();
            SpinValueList = new List<float>();

            Fly = new GolfMotionFly();
            Bounce = new GolfMotionBounce();
            Roll = new GolfMotionRoll(m_CurMap);
        }


        /// <summary>
        /// 计算路径
        /// </summary>
        /// <param name="time"></param>
        public void CalcPath(Vector3 startPosition, Vector3 endPosition, float time, bool isOnlyRoll = false)
        {
            if (!isOnlyRoll)
            {
                PathInfo.ListPt.Clear();
                CalcCavePaths(startPosition, endPosition, time);
            }

            CalcRollPathPoints(time);
        }



        public void CalcCavePaths(Vector3 startPosition, Vector3 endPosition, float time)
        {
            for (int i = 0; i < LogicConstantData.MaxFlyCurveCount; i++)
            {
                //获取最大旋球速度校正和调试界面旋球输入校正
                if (SpinValueList.Count > 0 && i >= 0 && i < SpinDic.Count)
                {
                    MaxSpinList = SpinDic[i];
                    Bounce.SpinVelocityRevise = SpinValueList[i] * MaxSpinList[i];
                }

                Vector3 startVelocity = Vector3.zero;
                if (i == 0)
                {
                    if (BattingVelocity == Vector3.zero)
                    {
                        startVelocity = Fly.CalcInitialVelocity(startPosition, endPosition, time);
                        PathInfo.LauchSpeed = startVelocity;
                    }
                    else
                    {
                        startVelocity = BattingVelocity;
                    }
                    Bounce.SpinAngleRevise = SpinLeftRight;
                }
                else
                {
                    if ((i - 1) >= 0 && (i - 1) < PathInfo.ListReboundPt.Count)
                    {
                        int reboundIndex = PathInfo.ListReboundPt[i - 1];
                        GolfPathPoint pathPoint = PathInfo.GetPathPoint(reboundIndex);
                        if (pathPoint is GolfRollPoint)
                        {
                            return;
                        }
                        GolfBouncePoint bouncePoint = pathPoint as GolfBouncePoint;
                        if (bouncePoint == null)
                        {
                            return;
                        }
                        startVelocity = bouncePoint.OutVelocity;
                        startPosition = bouncePoint.Position;
                        if (bouncePoint.ColType == CollisionType.Out || (!bouncePoint.IsRebound && !bouncePoint.IsRoll))
                        {
                            return;
                        }
                        if (bouncePoint.IsRoll)
                        {
                            return;
                        }
                    }
                    Bounce.SpinAngleRevise = 0;
                    Fly.InitHookVelocity = 0;
                    Fly.HookAcc = 0;
                }
                CalcFlyPathPoints(startPosition, startVelocity, time);
            }
        }

        /// <summary>
        /// 计算飞行路径点
        /// </summary>
        /// <param name="initialVelocity"></param>
        /// <param name="time"></param>
        public void CalcFlyPathPoints(Vector3 initialPosition, Vector3 initialVelocity, float time)
        {
            List<GolfFlyPoint> GolfPointList = new List<GolfFlyPoint>();
            GolfBouncePoint golfBouncePoint = new GolfBouncePoint();
            GolfPointList.Clear();
            //计算路径点
            for (int i = 0; i < LogicConstantData.MaxFlyPointCount; i++)
            {
                float timeSinceInit = i * time;

                //优化，点分布策略！！！！
                timeSinceInit *= 3;

                //空中
                //第一个点为球的初始位置
                if (i == 0)
                {
                    GolfFlyPoint initPoint = new GolfFlyPoint();
                    initPoint.Position = initialPosition;
                    initPoint.Interval = 0;
                    initPoint.Status = BallRunStatus.Fly;
                    GolfPointList.Add(initPoint);
                    PathInfo.AddPathPoint(initPoint);
                    continue;
                }

                //单个点计算位置和速度
                GolfFlyPoint point = Fly.CalcFlyPoint(initialVelocity, initialPosition, timeSinceInit);

                int bouncePointCount = PathInfo.ListReboundPt.Count;
                //最高点
                //Debug.Log("lastY: " + m_golfRun.LastFlyPoint.Velocity.y + "  Y:" + point.Velocity.y);
                if (bouncePointCount < 1 && GolfPointList[i - 1].Velocity.y >= 0 && point.Velocity.y < 0)
                {
                    FlyHighestPointHeight = point.Position.y;
                }
                //离落地点二分之一处，切换镜头
                if (bouncePointCount < 1 && GolfPointList[i - 1].Position.y > (FlyHighestPointHeight / 2) && point.Position.y <= (FlyHighestPointHeight / 2))
                {
                    float len = (point.Position - GolfPointList[0].Position).magnitude;
                    Vector3 dir = point.Position - GolfPointList[i - 1].Position;
                    //垂直于速度方向的一个偏移
                    Vector3 normal = Vector3.Cross(Vector3.up, dir).normalized;
                    dir.y = 0;
                    Vector3 cameraPos = point.Position + dir.normalized * (0.5f * len);
                    cameraPos = cameraPos + normal * 10;
                    point.EventType = EventCenterType.Battle_SwitchCamera2Fly;
                    point.EventParam = cameraPos;
                }

                point.Interval = timeSinceInit / i;
                point.Status = BallRunStatus.Fly;
                GolfPointList.Add(point);
                //碰撞
                if (i - 1 < 1)
                {
                    continue;
                }

                GolfMapFlyCollision collision = null;
                bool isCollision = m_CurMap.CheckCollisionPoint(GolfPointList[i - 1].Position, point.Position, ref collision);
                if (isCollision)
                {
                    //Debug.Log("roll1");
                    //计算初始滚动点
                    bool isRebound = false;
                    isRebound = Bounce.CalcBouncePoint(collision, GolfPointList[i - 1], bouncePointCount, ref golfBouncePoint);
                    if (!isRebound && golfBouncePoint.ColType != CollisionType.Out && golfBouncePoint.Poly != null)
                    {
                        Vector3 rollVelocity = Vector3.zero;
                        bool roll = Roll.CalcRoll(golfBouncePoint.OutVelocity, golfBouncePoint.Poly.Normal, ref rollVelocity);
                        golfBouncePoint.IsRoll = roll;
                        if (roll)
                        {
                            GolfRollPoint golfRollPoint = new GolfRollPoint();
                            golfBouncePoint.OutVelocity = rollVelocity;
                            golfRollPoint.Position = golfBouncePoint.Position;
                            golfRollPoint.Velocity = rollVelocity;
                            golfRollPoint.IsRoll = true;
                            golfRollPoint.Poly = golfBouncePoint.Poly;
                            golfRollPoint.AType = golfBouncePoint.AType;
                            golfRollPoint.Interval = 0;
                            golfRollPoint.EventType = EventCenterType.Battle_SwitchCamera2Roll;
                            golfRollPoint.EventParam = rollVelocity;
                            golfRollPoint.Status = BallRunStatus.Roll;
                            PathInfo.ListReboundPt.Add(PathInfo.ListPt.Count);
                            PathInfo.AddPathPoint(golfRollPoint);
                            return;
                            //Debug.Log("roll : " + "Position:" + golfRollPoint.Position + " Vel:" + golfRollPoint.Velocity + " MapTri:" + golfRollPoint.MapTri + " SubMap:" + golfRollPoint.SubMap);
                        }
                    }
                    golfBouncePoint.Interval = (golfBouncePoint.Position - GolfPointList[i - 1].Position).z / GolfPointList[i - 1].Velocity.z;
                    golfBouncePoint.Status = BallRunStatus.Fly;
                    GolfPointList[i].Position = golfBouncePoint.Position;
                    GolfPointList[i].Velocity = golfBouncePoint.Velocity;
                    GolfPointList[i].Interval = golfBouncePoint.Interval;
                    GolfPointList[i].AType = golfBouncePoint.AType;
                    if (PathInfo.ListReboundPt.Count < 1)
                    {
                        PathInfo.FirstReboundPt = golfBouncePoint.Position;
                    }
                    PathInfo.AddPathPoint(golfBouncePoint);
                    return;
                }
                else
                {
                    PathInfo.AddPathPoint(point);
                }
            }
        }

        /// <summary>
        /// 计算滚动路径点
        /// </summary>
        /// <param name="time"></param>
        public void CalcRollPathPoints(float time)
        {
            if (PathInfo.ListPt == null)
            {
                return;
            }
            GolfPathPoint pathPoint = PathInfo.GetPathPoint(PathInfo.ListPt.Count - 1);
            if (pathPoint is GolfRollPoint)
            {
                GolfRollPoint golfRollPoint = pathPoint as GolfRollPoint;
                if (golfRollPoint == null)
                {
                    return;
                }

                for (int i = 0; i < LogicConstantData.MaxRollPointCount; i++)
                {
                    //第一个点已经确定
                    if (i == 0)
                    {
                        continue;
                    }

                    float intervalTime = time;
                    //优化策略
                    intervalTime *= 1;

                    if (i - 1 < 0 || i - 1 >= PathInfo.ListPt.Count)
                    {
                        return;
                    }
                    golfRollPoint = PathInfo.GetPathPoint(PathInfo.ListPt.Count - 1) as GolfRollPoint;
                    if (golfRollPoint == null)
                    {
                        continue;
                    }
                    GolfRollPoint startPoint = golfRollPoint;
                    if (startPoint == null || startPoint.Poly == null)
                    {
                        return;
                    }

                    //分析小球在球洞的状态
                    BallType ballType = Roll.ParseBallType(startPoint.Velocity, startPoint.Position, m_CurMap.BallHolePos);

                    //预测下一个点
                    Vector3 nextVelocity = Vector3.zero;
                    Vector3 acc = Vector3.zero;
                    Vector3 nextPosition = Vector3.zero;

                    if (ballType == BallType.OutHole)
                    {
                        intervalTime *= 2;
                        nextPosition = Roll.CalcRollPoint(startPoint.Position, startPoint.Velocity, intervalTime, startPoint.AType, startPoint.Poly, ref nextVelocity, ref acc);
                    }
                    else
                    {
                        intervalTime *= 0.5f;
                        acc = Roll.GetHoleAcceleration();
                        nextPosition = Roll.CalcNextPos(startPoint.Position, startPoint.Velocity, acc, intervalTime);
                        nextVelocity = Roll.CalcNextV(startPoint.Velocity, acc, intervalTime);
                    }
                    GolfRollPoint nextPoint = new GolfRollPoint();
                    //检测与洞壁的碰撞
                    Vector3 collisionPosition = Vector3.zero;
                    bool isCollisionHole = Roll.CheckColliderHole(m_CurMap.BallHolePos, nextPosition, startPoint.Position, ref collisionPosition);
                    if (isCollisionHole)
                    {
                        //垂直于球洞壁，方向向外的法线
                        Vector3 normal = nextPosition - m_CurMap.BallHolePos;
                        normal.y = 0;
                        nextVelocity = Roll.CalcCollisionHoleSpeed(normal.normalized, Roll.CalcDropDis(m_CurMap.BallHolePos, nextPosition), Roll.m_BallRadius, nextVelocity, Roll.CalcxzDistance2HolePos(m_CurMap.BallHolePos, nextPosition));
                        if (ballType == BallType.HoleOuto)
                        {
                            nextPosition.y = 0;
                        }
                        //Debug.Log("Collision:" + collisionPosition + " LixinAcc:" + Roll.m_LixinAcc);
                    }
                    if (ballType == BallType.OutHole && !isCollisionHole)
                    {
                        //计算下一个真正的点
                        nextPoint = Roll.CalcRealRollPoint(startPoint, nextPosition, nextVelocity, acc, intervalTime);
                        nextPoint.BallType = ballType;
                        //Debug.Log("OutHoleRoll : " + "  NextPosition:" + nextPoint.Position + "  NextVelocity:" + nextVelocity + "  Acc:" + acc + "  Time:" + intervalTime + "  AreaType:" + nextPoint.AType + "  CollisionType: " + nextPoint.ColType);
                        //检测停止或出界
                        if (Roll.CheckStop(nextPoint, acc))
                        {
                            //Debug.Log("OutHoleRoll : stop or out");
                            nextPoint.IsRoll = false;
                            nextPoint.Status = BallRunStatus.Stop;
                            PathInfo.AddPathPoint(nextPoint);
                            return;
                        }
                    }
                    else
                    {
                        nextPoint.BallType = ballType;
                        nextPoint.Position = nextPosition;
                        nextPoint.Velocity = nextVelocity;
                        nextPoint.Poly = startPoint.Poly;
                        nextPoint.IsRoll = true;
                        nextPoint.AType = startPoint.AType;
                        nextPoint.ColType = startPoint.ColType;
                        nextPoint.Interval = time;
                        nextPoint.Status = BallRunStatus.Roll;

                        PathInfo.ListHolePt.Add(PathInfo.ListPt.Count);
                        //Debug.Log("InHoleRoll : " + "  NextPosition:" + nextPoint.Position + "  NextVelocity:" + nextVelocity + "  Acc:" + acc + "  Time:" + intervalTime + "  AreaType:" + nextPoint.AType + "  CollisionType: " + nextPoint.ColType);

                        //进洞
                        if (ballType == BallType.HoleDeeping)
                        {
                            //Debug.Log("InHoleRoll : GoInHole");
                            nextPoint.Status = BallRunStatus.Stop;
                            PathInfo.AddPathPoint(nextPoint);
                            return;
                        }
                    }
                    //Debug.Log("ballType :" + ballType + " isCollisionHole:" + isCollisionHole + " nextPosition:" + nextPosition + " nextVelocity:" + nextVelocity + " acc:" + acc);
                    //Debug.Log("Before   CalcRollP: " + " Position:" + startPoint.Position + " next:" + nextPosition + " Vel:" + startPoint.Velocity + " Tri:" + startPoint.MapTri + " SubMap:" + startPoint.SubMap);
                    PathInfo.AddPathPoint(nextPoint);
                }
            }
        }

        /// <summary>
        /// 获取所有路径点的位置列表
        /// </summary>
        /// <returns></returns>
        public List<Vector3> GetAllPathPointsPosition()
        {
            List<Vector3> posList = new List<Vector3>();
            posList.Clear();
            for (int i = 0; i < PathInfo.ListPt.Count; i++)
            {
                posList.Add(PathInfo.ListPt[i].Position);
            }
            return posList;
        }

        public void Clear()
        {
            if (SpinDic != null)
            {
                SpinDic.Clear();
            }
            if (Fly != null)
            {
                Fly.Clear();
            }
            if (Bounce != null)
            {
                Bounce.Clear();
            }
            if (Roll != null)
            {
                Roll.Clear();
            }
        }
    }
}
