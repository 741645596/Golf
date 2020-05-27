using IGG.Core.Data.Config;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author  zhulin
/// Date    2019.5.6
/// Desc    GolfAI模块动态数据集
/// </summary>
namespace IGG.Core.Data.DataCenter.GolfAI
{
    public class GolfMotionBounce : GolfMotion
    {
        /// <summary>
        /// 前后旋球速度校正
        /// </summary>
        public float SpinVelocityRevise { get; set; } = 0;
        /// <summary>
        /// 左右旋球角度校正
        /// </summary>
        public float SpinAngleRevise { get; set; } = 0;

        public GolfMotionBounce()
        {
        }

        public Vector3 CalcCollisionVelocity(Vector3 startPosition, Vector3 endPosition, Vector3 startVelocity)
        {
            float t = Mathf.Abs(endPosition.z - startPosition.z) / startVelocity.z;
            float vy = startVelocity.y + 0.5f * LogicConstantData.Gravity * t;
            Vector3 v = new Vector3(startVelocity.x, vy, startVelocity.z);
            return v;
        }
        /// <summary>
        /// 计算碰撞点
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public bool CalcBouncePoint(GolfMapFlyCollision collision, GolfFlyPoint startPoint, int haveReboundCount, ref GolfBouncePoint golfBouncePoint)
        {
            //Debug.Log("lastPos:" + lastPos + "  curPos:" + curPos + " collisionPoint:" + ret.Point + " ret.Tri:" + ret.Tri);
            golfBouncePoint.Position = collision.Point;
            golfBouncePoint.ColType = collision.ColType;
            if (collision.ColType == CollisionType.Out)
            {
                return true;
            }
            //计算碰撞点的速度
            Vector3 collisionVelocity = CalcCollisionVelocity(startPoint.Position, collision.Point, startPoint.Velocity);
            golfBouncePoint.Velocity = collisionVelocity;
            golfBouncePoint.Poly = collision.Poly;
            golfBouncePoint.AType = collision.Type;

            //计算反弹
            //旋球限制
            //int type = (int)ret.Type;
            //if (type >= 1 && type <= 5)
            //{
            //    GolfBounceParam.SpinVelocityRevise = Mathf.Clamp(GolfBounceParam.SpinVelocityRevise, -MaxSpinList[type - 1], MaxSpinList[type - 1]);
            //}
            Vector3 outVelocity = Vector3.zero;
            bool rebound = CalcBounce(collisionVelocity, collision.Normal, collision.Type, haveReboundCount, ref outVelocity);
            golfBouncePoint.IsRebound = rebound;
            golfBouncePoint.OutVelocity = outVelocity;
            return rebound;
        }

        /// <summary>
        /// 计算反弹
        /// </summary>
        /// <param name="initialPosition"></param>
        /// <param name="velocity"></param>
        /// <param name="normal"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool CalcBounce(Vector3 velocity, Vector3 normal, AreaType areaType, int haveReboundCount, ref Vector3 outVelocity)
        {
            //计算速度衰减
            Vector3 decayVelocity = CalcVelocityDecay(areaType, velocity, normal, true);
            //计算反射速度
            outVelocity = CalcOutVelocity(decayVelocity, normal);
            //检查是否可以反弹
            if (CheckCanBounce(outVelocity, normal, areaType, haveReboundCount))
            {
                //Debug.Log("outVelocity:" + outVelocity + "normal:" + normal);
                //旋球处理
                outVelocity = CalcTopBackSpinVelocity(outVelocity);
                outVelocity = CalcLeftRightSpinVelocity(outVelocity, normal);

                return true;
            }
            return false;
        }

        /// <summary>
        /// 计算速度衰减
        /// </summary>
        /// <param name="areaType"></param>
        /// <param name="velocity"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public Vector3 CalcVelocityDecay(AreaType areaType, Vector3 velocity, Vector3 normal, bool isDecay)
        {
            if (isDecay)
            {
                GroundmaterialConfig groundMatConfig = GroundmaterialDao.Inst.GetCfg((uint)areaType);
                if (groundMatConfig == null)
                {
                    return Vector3.zero;
                }
                //衰减斜率
                float k = (float)groundMatConfig.DecreaseSlope;
                //衰减起始值
                float offset = (float)groundMatConfig.DecreaseBias;
                //入射速度在法线上的投影，该投影与衰减值呈k为斜率和offset为起始值的线性关系
                float p = Vector3.Dot(-velocity, normal);
                float decay = (k * p + offset) / 100;
                //衰减不能放大，范围0-1
                decay = Mathf.Clamp01(decay);

                Vector3 decayVelocity = velocity * (1 - decay);
                //Debug.Log("衰减斜率:" + k + " 衰减偏移:" + offset + "速度法线投影:" + p + " 衰减:" + decay + " 速度:" + decayVelocity + " 开始速度:" + velocity);
                return decayVelocity;
            }
            return velocity;
        }

        /// <summary>
        /// 计算反射速度
        /// </summary>
        /// <param name="inVelocity"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public Vector3 CalcOutVelocity(Vector3 inVelocity, Vector3 normal)
        {
            //反向入射速度在法线方向的分量
            Vector3 p = Vector3.Dot(-inVelocity, normal) * normal;
            //计算反射速度
            Vector3 outVelocity = 2 * p + inVelocity;
            return outVelocity;
        }

        /// <summary>
        /// 检查是否可以反弹
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="normal"></param>
        /// <param name="areaType"></param>
        /// <returns></returns>
        public bool CheckCanBounce(Vector3 velocity, Vector3 normal, AreaType areaType, int haveReboundCount)
        {
            if (haveReboundCount >= 3)
            {
                return false;
            }
            GroundmaterialConfig groundMatConfig = GroundmaterialDao.Inst.GetCfg((uint)areaType);
            if (groundMatConfig == null)
            {
                return false;
            }
            float minBounceVelocity = (float)groundMatConfig.MinBounceVelocity / 100.0f;
            float p = Mathf.Abs(Vector3.Dot(-velocity, normal));
            if (p >= minBounceVelocity)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 计算前后旋球速度校正后的速度
        /// </summary>
        /// <param name="velocity"></param>
        /// <returns></returns>
        public Vector3 CalcTopBackSpinVelocity(Vector3 velocity)
        {
            float l = velocity.magnitude + SpinVelocityRevise;
            Vector3 dir = velocity.normalized;
            Vector3 spinedVelocity = dir* l;
            spinedVelocity.y = velocity.y;
            return spinedVelocity;
        }

        /// <summary>
        /// 计算左右旋球角度校正后的速度
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public Vector3 CalcLeftRightSpinVelocity(Vector3 velocity, Vector3 normal)
        {
            Vector3 spinedVelocity = Quaternion.AngleAxis(SpinAngleRevise, normal) * velocity;
            return spinedVelocity;
        }

        public void Clear()
        {
            SpinVelocityRevise = 0;
            SpinAngleRevise = 0;
        }
    }
}
