using IGG.Core.Data.Config;
using IGG.Core.Data.DataCenter.Battle;
using System.Collections.Generic;
using UnityEngine;

public enum BallType
{
    OutHole = 0,        // 在球洞外面
    HoleInto = 1,       // 进入球洞过程中
    HoleIng = 2,        // 在球洞中运行
    HoleOuto = 3,       // 出球洞 
    HoleDeeping = 4,    // 调入球洞
}

/// <summary>
/// Author  zhulin
/// Date    2019.5.6
/// Desc    GolfAI模块动态数据集
/// </summary>
namespace IGG.Core.Data.DataCenter.GolfAI
{
    public class GolfMotionRoll : GolfMotion
    {
        private BallType m_ballType = BallType.OutHole;

        public float m_BallRadius = 0.3f;

        public float m_HoleRadius = 5.0f;

        public float g = 9.8f;

        public float AttenuationParameter = 0.8f;

        /// <summary>
        /// 离心加速度
        /// </summary>
        public Vector3 m_LixinAcc = Vector3.zero;

        private GolfCourseMap m_CurMap = null;

        public GolfMotionRoll(GolfCourseMap map)
        {
            m_CurMap = map;
            m_HoleRadius = m_CurMap.BallHoleRadius;
        }
        public bool CalcRoll(Vector3 velocity, Vector3 normal, ref Vector3 rollVelocity)
        {
            //不反弹检查是否滚动
            rollVelocity = CalcTangentVelocity(velocity, normal);
            float v = rollVelocity.magnitude;
            if (v > LogicConstantData.FLOAT_PERCISION_FIX)
            {
                return true;
            }
            return false;
        }

        public Vector3 CalcRollVelocity(Vector3 velocity, Vector3 normal)
        {
            //反向入射速度在法线方向的分量
            Vector3 p = Vector3.Dot(velocity, normal) * normal;
            //计算垂直于法线的出射速度
            Vector3 rollVelocity = velocity - p;
            return rollVelocity;
        }

        /// <summary>
        /// 计算滚动点的摩擦力加速度
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="areaType"></param>
        /// <returns></returns>
        public Vector3 CalcRollFrictionAcc(Vector3 velocity, AreaType areaType, GolfAIMapPolygon poly)
        {
            GroundmaterialConfig groundMatConfig = GroundmaterialDao.Inst.GetCfg((uint)areaType);
            if (groundMatConfig == null)
            {
                return Vector3.zero;
            }
            //摩擦系数
            float u = groundMatConfig.Friction;
            //获取小球在该三角面的加速度
            Vector3 acc = poly.GetAcceleration(velocity, 1, u);
            return acc;
        }

        /// <summary>
        /// 计算下一个预测的滚动点
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="time"></param>
        /// <param name="areaType"></param>
        /// <param name="nextVelocity"></param>
        /// <returns></returns>
        public Vector3 CalcRollPoint(Vector3 position, Vector3 velocity, float time, AreaType areaType, GolfAIMapPolygon poly, ref Vector3 nextVelocity, ref Vector3 acc)
        {
            //速度方向校正，始终沿切线方向
            Vector3 tangentVelocity = CalcTangentVelocity(velocity, poly.Normal);
            acc = GetRollAcceleration(tangentVelocity, areaType, poly);
            //切线方向x分量
            nextVelocity.x = tangentVelocity.x + acc.x * time;
            position.x = position.x + tangentVelocity.x * time + 0.5f * acc.x * time * time;
            //切线方向y分量
            nextVelocity.y = tangentVelocity.y + acc.y * time;
            position.y = position.y + tangentVelocity.y * time + 0.5f * acc.y * time * time;
            //切线方向z分量
            nextVelocity.z = tangentVelocity.z + acc.z * time;
            position.z = position.z + tangentVelocity.z * time + 0.5f * acc.z * time * time;
            return position;
        }

        /// <summary>
        /// 获取小球滚动加速度
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="areaType"></param>
        /// <param name="poly"></param>
        /// <returns></returns>
        public Vector3 GetRollAcceleration(Vector3 velocity, AreaType areaType,  GolfAIMapPolygon poly)
        {
            //根据配档的摩擦系数求加速度
            Vector3 acc = CalcRollFrictionAcc(velocity, areaType, poly);
            //加速度方向校正
            acc = CalcTangentVelocity(acc, poly.Normal);
            return acc;
        }

        /// <summary>
        /// 计算下一个真正的滚动点
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="nextPosition"></param>
        /// <param name="nextVelocity"></param>
        /// <param name="acc"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public GolfRollPoint CalcRealRollPoint(GolfRollPoint startPoint, Vector3 nextPosition, Vector3 nextVelocity, Vector3 acc, float time)
        {
            GolfRollPoint nextPoint = new GolfRollPoint();
            GolfIntoTriCollision collision = null;
            bool isChangeTri = m_CurMap.CheckIntoNearTriangle(startPoint.Position, nextPosition, startPoint.Poly, ref collision);
            if (isChangeTri)
            {
                //切换三角面，这里返回的collision是预测滚动路径与三角面对应边的交点，即真正的下一个点
                nextPoint.Position = collision.Point;
                float realTime = time;
                nextPoint.Velocity = CalcRealRollVelocity(startPoint.Position, collision.Point, startPoint.Velocity, startPoint.AType, startPoint.Poly, time, acc, out realTime); ;
                nextPoint.Poly = collision.Poly;
                nextPoint.AType = collision.Type;
                nextPoint.IsRoll = true;
                nextPoint.ColType = collision.ColType;
                nextPoint.Interval = realTime;
                nextPoint.IsRoll = true;
                nextPoint.Status = BallRunStatus.Roll;
                //Debug.Log("Change   TriCalcRollP: " + " Position:" + nextPoint.Position + " Vel:" + nextPoint.Velocity + " acc:" + acc + " Tri:" + nextPoint.MapTri + " SubMap:" + nextPoint.SubMap);
            }
            else
            {
                //在同一个三角面，按正常跑
                nextPoint.Position = nextPosition;
                nextPoint.Velocity = nextVelocity;
                nextPoint.Poly = startPoint.Poly;
                nextPoint.AType = startPoint.AType;
                nextPoint.IsRoll = true;
                nextPoint.ColType = startPoint.ColType;
                nextPoint.Interval = time;
                nextPoint.IsRoll = true;
                nextPoint.Status = BallRunStatus.Roll;
                //Debug.Log("No   ChangeTriCalcRollP: " + " Position:" + nextPoint.Position + " Vel:" + nextPoint.Velocity + " acc:" + acc + " Tri:" + nextPoint.MapTri + " SubMap:" + nextPoint.SubMap);
            }
            return nextPoint;
        }

        /// <summary>
        /// 求真实滚动点的速度
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <param name="velocity"></param>
        /// <param name="areaType"></param>
        /// <param name="poly"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public Vector3 CalcRealRollVelocity(Vector3 startPosition, Vector3 endPosition, Vector3 velocity, AreaType areaType, GolfAIMapPolygon poly, float time, Vector3 acc, out float realTime)
        {
            Vector3 deltaPosition = endPosition - startPosition;

            //求真实的时间间隔，根据公式解一元二次方程，有两个值，取正值
            realTime = (-velocity.x + Mathf.Sqrt(velocity.x * velocity.x - (4 * 0.5f * acc.x * (-deltaPosition.x)))) / (2 * 0.5f * acc.x);

            float vx = velocity.x + acc.x * realTime;
            float vy = velocity.y + acc.y * realTime;
            float vz = velocity.z + acc.z * realTime;
            Vector3 endVelocity = new Vector3(vx, vy, vz);

            //求与法线垂直方向的速度
            Vector3 realVelocity = CalcTangentVelocity(endVelocity, poly.Normal);

            return endVelocity;
        }

        /// <summary>
        /// 求垂直于法线的切线方向的速度
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public Vector3 CalcTangentVelocity(Vector3 velocity, Vector3 normal)
        {
            float pSize = Vector3.Dot(velocity, normal);
            Vector3 p = Vector3.zero;
            if (pSize != 0)
            {
                p = Vector3.Dot(-velocity, normal) * normal + velocity;
            }
            else
            {
                return velocity;
            }
            return p;
        }

        /// <summary>
        /// 检测球滚动停止
        /// </summary>
        /// <param name="rollPoint"></param>
        /// <param name="acc"></param>
        /// <returns></returns>
        public bool CheckStop(GolfRollPoint rollPoint, Vector3 acc)
        {
            bool isStop = rollPoint.Velocity.magnitude < LogicConstantData.FLOAT_PERCISION_FIX && acc == Vector3.zero;
            if (isStop || rollPoint.ColType == CollisionType.Out)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检测是否飞出
        /// </summary>
        /// <returns></returns>
        public bool CheckFly()
        {
            return false;
        }

        /// <summary>
        /// 计算到球洞的水平距离
        /// </summary>
        /// <param name="holePos"></param>
        /// <param name="ballPos"></param>
        /// <returns></returns>
        public float CalcxzDistance2HolePos(Vector3 holePos, Vector3 ballPos)
        {
            Vector2 holePos2 = new Vector2(holePos.x, holePos.z);
            Vector3 ballPos2 = new Vector2(ballPos.x, ballPos.z);
            return Vector2.Distance(holePos2, ballPos2);
        }

        /// <summary>
        /// 计算掉落距离
        /// </summary>
        /// <param name="ballPos"></param>
        /// <returns></returns>
        public float CalcDropDis(Vector3 holePos, Vector3 ballPos)
        {
            float h = ballPos.y - holePos.y;
            if (h >= 0)
            {
                return 0;
            }
            else
            {
                return -h;
            }
        }
        
        /// <summary>
        /// 分析小球状态
        /// </summary>
        public BallType ParseBallType(Vector3 m_speed, Vector3 m_Pos, Vector3 m_HolePos)
        {
            //计算小球和洞之间的水平距离
            float m_xzDistance = CalcxzDistance2HolePos(m_HolePos, m_Pos);
            //计算小球掉进洞的深度
            float m_DropHeight = CalcDropDis(m_HolePos, m_Pos);

            if (m_DropHeight >= (2 * m_BallRadius))
            {
                m_ballType = BallType.HoleDeeping;
            }
            else
            {
                if (m_xzDistance > m_HoleRadius)
                {
                    m_ballType = BallType.OutHole;
                }
                else if (m_xzDistance <= m_HoleRadius - m_BallRadius)
                {
                    m_ballType = BallType.HoleIng;
                }
                else
                {
                    if (CheckInHole(m_speed, m_Pos, m_HolePos) == true)
                    {
                        m_ballType = BallType.HoleInto;
                    }
                    else
                    {
                        m_ballType = BallType.HoleOuto;
                    }
                }
            }
            return m_ballType;
        }

        /// <summary>
        /// 判断是处于进洞，出洞状态。
        /// 进洞速度方向与圆心位置方向夹角 小于等于 90
        /// 出洞速度方向与圆心位置方向夹角 大于 90
        /// </summary>
        /// <returns>true :进洞， false： 出洞</returns>
        private bool CheckInHole(Vector3 Ballspeed, Vector3 BallPos, Vector3 HolePos)
        {
            Vector2 v = new Vector2(Ballspeed.x, Ballspeed.z);
            Vector3 dir = HolePos - BallPos;
            Vector2 Dir = new Vector2(dir.x, dir.z);

            float ret = Vector2.Dot(v, Dir);
            if (ret >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取加速度，不计算OutHole的情况
        /// </summary>
        /// <returns></returns>
        public Vector3 GetHoleAcceleration()
        {
            if (m_ballType == BallType.HoleInto)
            {
                /*float sinA = (m_HoleRadius - m_BallRadius) / m_HoleRadius;
                float consA = Mathf.Sqrt(1 - sinA * sinA);
                Vector3 dir = m_HolePos - m_Pos;
                dir.y = 0;

                return g * (1 * sinA * sinA) * dir.normalized + g * consA * sinA * new Vector3(0, 1, 0);
                */
                return new Vector3(0, -g, 0);
            }
            else if (m_ballType == BallType.HoleIng)
            {
                return new Vector3(0, -g, 0) + m_LixinAcc;
            }
            else if (m_ballType == BallType.HoleOuto)
            {
                return new Vector3(0, -g, 0) + m_LixinAcc;
            }
            else if (m_ballType == BallType.HoleDeeping)
            {
                return new Vector3(0, -g, 0) + m_LixinAcc;
            }
            else
            {
                return Vector3.zero + m_LixinAcc;
            }

        }

        /// <summary>
        /// 计算下一个位置
        /// </summary>
        /// <param name="t"></param>
        /// <param name="acc"></param>
        /// <returns></returns>
        public Vector3 CalcNextPos(Vector3 m_Pos, Vector3 m_speed, Vector3 acc, float t)
        {
            return m_Pos + m_speed * t + 0.5f * acc * t * t;
        }

        /// <summary>
        /// 计算下一个时间的速度
        /// </summary>
        /// <param name="t"></param>
        /// <param name="acc"></param>
        /// <returns></returns>
        public Vector3 CalcNextV(Vector3 m_speed, Vector3 acc, float t)
        {
            return m_speed + t * acc;
        }

        /// <summary>
        /// 是否触碰到球洞内壁。
        /// </summary>
        /// <returns></returns>
        public bool CheckColliderHole(Vector3 holePos, Vector3 nextBallPos, Vector3 startBallPos, ref Vector3 collisionPosition)
        {
            //小球圆心到球洞壁的水平距离
            float horizonDis = m_HoleRadius - CalcxzDistance2HolePos(holePos, nextBallPos);

            float dropHeight = CalcDropDis(holePos, nextBallPos);
            Vector3 collision1 = Vector3.zero;
            Vector3 collision2 = Vector3.zero;

            if (CheckLineAndCircleIntersect(startBallPos, nextBallPos, holePos, m_HoleRadius, ref collision1, ref collision2))
            {
                if (collision1 != Vector3.zero && collision2 != Vector3.zero)
                {
                    //取离小球最近的碰撞点
                    float l1 = (collision1 - startBallPos).magnitude;
                    float l2 = (collision2 - startBallPos).magnitude;
                    if (l1 <= l2)
                    {
                        collisionPosition = collision1;
                    }
                    else
                    {
                        collisionPosition = collision2;
                    }
                }
                else if (collision1 == Vector3.zero && collision2 != Vector3.zero)
                {
                    collisionPosition = collision2;
                }
                else if (collision1 != Vector3.zero && collision2 == Vector3.zero)
                {
                    collisionPosition = collision1;
                }
                else
                {
                    collisionPosition = Vector3.zero;
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 求线段和圆的交点
        /// </summary>
        /// <param name="ptStart"></param>
        /// <param name="ptEnd"></param>
        /// <param name="ptCenter"></param>
        /// <param name="Radius"></param>
        /// <param name="ptInter1"></param>
        /// <param name="ptInter2"></param>
        /// <returns></returns>
        public bool CheckLineAndCircleIntersect(Vector3 ptStart, Vector3 ptEnd, Vector3 ptCenter, float Radius, ref Vector3 ptInter1,  ref Vector3 ptInter2)
        {
            float fDis = Mathf.Sqrt((ptEnd.x - ptStart.x) * (ptEnd.x - ptStart.x) + (ptEnd.z - ptStart.z) * (ptEnd.z - ptStart.z));

            Vector3 d = Vector3.zero;
            d.x = (ptEnd.x - ptStart.x) / fDis;
            d.z = (ptEnd.z - ptStart.z) / fDis;

            Vector3 E;
            E.x = ptCenter.x - ptStart.x;
            E.z = ptCenter.z - ptStart.z;

            float a = E.x * d.x + E.z * d.z;
            float a2 = a * a;

            float e2 = E.x * E.x + E.z * E.z;

            float r2 = Radius * Radius;

            if ((r2 - e2 + a2) < 0)
            {
                return false;
            }
            else
            {
                float f = Mathf.Sqrt(r2 - e2 + a2);

                float t = a - f;

                if (((t - 0.0) > -LogicConstantData.FLOAT_DIS) && (t - fDis) < LogicConstantData.FLOAT_DIS)
                {
                    d.y = (ptEnd.y - ptStart.y) / fDis;

                    ptInter1.x = ptStart.x + t * d.x;
                    ptInter1.z = ptStart.z + t * d.z;
                     
                    ptInter1.y = ptStart.y + (ptInter1.x - ptStart.x) * (ptEnd.y - ptStart.y) / (ptEnd.x - ptStart.x);
                    if (ptInter1.y <= ptCenter.y)
                    {
                        return true;
                    }
                }

                t = a + f;

                if (((t - 0.0) > -LogicConstantData.FLOAT_DIS) && (t - fDis) < LogicConstantData.FLOAT_DIS)
                {
                    ptInter2.x = ptStart.x + t * d.x;
                    ptInter2.z = ptStart.z + t * d.z;

                    ptInter2.y = ptStart.y + (ptInter2.x - ptStart.x) * (ptEnd.y - ptStart.y) / (ptEnd.x - ptStart.x);
                    if (ptInter2.y <= ptCenter.y)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Normal">撞击点的法线</param>
        /// <param name="H">进入球洞的高度</param>
        /// <param name="BallRadius">小球的半径</param>
        /// <param name="Speed">撞击时小球的速度</param>
        /// <returns></returns>
        public Vector3 CalcCollisionHoleSpeed(Vector3 Normal, float H, float BallRadius, Vector3 Speed, float m_xzDistance)
        {
            // y 方向的速度分量
            Vector3 Vey = new Vector3(0, 1, 0);
            Vector3 Vy = Vector3.Dot(Speed, Vey) * Vey;
            // 法线上的速度分量
            Vector3 eNormal = Normal.normalized;
            float VnValue = Vector3.Dot(Speed, eNormal);
            Vector3 Vn = VnValue * eNormal;
            // 在垂直y，Normal 方向上的速度分量
            Vector3 Vnx = Speed - Vy - Vn;

            // 计算力矩大小，那边力矩大就往哪个方向速度转化。
            float fvn = Vn.sqrMagnitude;
            float dvn = BallRadius * BallRadius - H * H;
            float fvy = Vy.sqrMagnitude;
            float dvy = H * H;
            float ret = 0;


            // 完全进入洞中
            if (H >= (2 * BallRadius))
            {
                m_LixinAcc = Vnx.sqrMagnitude / m_xzDistance * -eNormal;
                return Vy + Vnx - Vn;
            }
            // 完全没有进入洞中
            else if (H <= 0)
            {
                return Speed;
            }
            else
            {
                if (VnValue < 0)
                {
                    /*m_LixinAcc = Vnx.sqrMagnitude / m_xzDistance * -eNormal;
                    Vector3 v = Vnx + -Vy * AttenuationParameter;
                    Vnx = Vnx.normalized * v.magnitude;
                    return Vn + Vnx;*/
                    m_LixinAcc = Vnx.sqrMagnitude / m_xzDistance * -eNormal;
                    return -Vy * AttenuationParameter + Vn + Vnx;

                }
                else if (VnValue == 0)
                {
                    m_LixinAcc = Vnx.sqrMagnitude / m_xzDistance * -eNormal;
                    return -Vy * AttenuationParameter + Vn + Vnx;
                }
                else
                {
                    ret = fvn * dvn - fvy * dvy;
                    m_LixinAcc = Vnx.sqrMagnitude / m_xzDistance * -eNormal;
                    if (ret > 0)
                    {
                        Vector3 VdirY = Vey * (BallRadius - H) / BallRadius;
                        Vector3 VdirN = eNormal * Mathf.Sqrt(H * (2 * BallRadius - H)) / BallRadius;
                        // 碰撞后的方向
                        Vector3 eVdirYN = VdirY + VdirN;
                        Vector3 vs = (Vy + Vn).magnitude * eVdirYN.normalized;
                        return vs + Vnx;
                        //return -Vy * AttenuationParameter + Vn + Vnx;
                    }
                    else if (ret == 0)
                    {
                        return -Vy - Vn + Vnx;
                    }
                    else
                    {
                        return Vy - Vn + Vnx;
                    }
                }
            }
        }


        public void Clear()
        {
            
        }
    }
}
