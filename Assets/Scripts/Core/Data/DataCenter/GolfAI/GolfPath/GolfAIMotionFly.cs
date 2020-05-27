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
    public class GolfMotionFly : GolfMotion
    {
        /// <summary>
        /// 仰角Cos值
        /// </summary>
        private float m_cosElevationRad = 0;
        public float CosElevationRad
        {
            get
            {
                return m_cosElevationRad / 1000000;
            }
            set
            {
                m_cosElevationRad = (float)value;
            }
        }
        /// <summary>
        /// 仰角Sin值
        /// </summary>
        private float m_sinElevationRad = 0;
        public float SinElevationRad
        {
            get
            {
                return m_sinElevationRad / 1000000;
            }
            set
            {
                m_sinElevationRad = (float)value;
            }
        }

        //曲球加速度
        public float HookAcc = 0;
        //曲球初始速度
        public float InitHookVelocity = 0;

        //精准度对应风切校正
        public float AccuracyRevise = 0;
        /// <summary>
        /// 全局加速度(用于风切、精准校正等计算后的最终加速度)
        /// </summary>
        public Vector3 GlobalAcc { get; set; } = Vector3.zero;

        public GolfMotionFly()
        {
        }

        public Vector3 CalcInitialVelocity(Vector3 startPosition, Vector3 endPosition, float hookAngle)
        {
            //位移
            Vector3 deltaPosition = endPosition - startPosition;
            //角
            if (deltaPosition.z == 0)
            {
                return Vector3.zero;
            }
            float HorizonRad = Mathf.Atan(deltaPosition.x / deltaPosition.z);
            float CosHorizonRad = Mathf.Cos(HorizonRad);
            float SinHorizonRad = Mathf.Sin(HorizonRad);

            //公式算出初速度
            float divisor = (2 * deltaPosition.y * CosElevationRad * CosElevationRad * CosHorizonRad * CosHorizonRad
                - 2 * deltaPosition.z * SinElevationRad * CosElevationRad * CosHorizonRad);
            if (divisor == 0)
            {
                return Vector3.zero;
            }
            float squareV = (deltaPosition.z * deltaPosition.z * LogicConstantData.Gravity) / divisor;
            float v = Mathf.Sqrt(Mathf.Abs(squareV));

            //初速度分量
            float vx = v * CosElevationRad * SinHorizonRad;
            float vy = v * SinElevationRad;
            float vz = v * CosElevationRad * CosHorizonRad;

            //反向时的处理
            Vector3 dir = deltaPosition.normalized;
            if (dir.z < 0)
            {
                vz = -vz;
                vx = -vx;
                hookAngle = -hookAngle;
            }

            //计算曲球初速度和加速度
            CalcHook(vy, vz, deltaPosition.z, hookAngle);

            //计算精准度风切校正
            CalcAccuracyRevise(vx, vz);

            //最终初速度
            Vector3 velocity = new Vector3(vx, vy, vz);
            return velocity;
        }

        /// <summary>
        /// 计算曲球初速度和加速度
        /// </summary>
        /// <param name="vy"></param>
        /// <param name="vz"></param>
        /// <param name="pz"></param>
        /// <param name="hookAngle"></param>
        public void CalcHook(float vy, float vz, float pz, float hookAngle)
        {
            //曲球只改变曲度，不改变落点
            //根据 曲球角度正切 = 曲球初速度速度 / y方向速度 求曲球初速度
            float TanHookRad = Mathf.Tan(hookAngle * Mathf.Deg2Rad);
            InitHookVelocity = vy * TanHookRad;
            //全程时间
            float t = pz / vz;
            //反推加速度
            HookAcc = -2 * InitHookVelocity / t;
        }

        /// <summary>
        /// //计算精准度风切校正
        /// </summary>
        /// <param name="vx"></param>
        /// <param name="vz"></param>
        public void CalcAccuracyRevise(float vx, float vz)
        {
            //垂直于速度方向
            //当AccuracyRevise > 0时为vz,0,-vx， 当AccuracyRevise < 0时为-vz,0,vx
            Vector3 dirAccuracy = new Vector3(vz, 0, -vx);
            dirAccuracy.Normalize();
            GlobalAcc += dirAccuracy * AccuracyRevise;
        }

        public GolfFlyPoint CalcFlyPoint(Vector3 initialVelocity, Vector3 initPos, float time)
        {
            GolfFlyPoint point = new GolfFlyPoint();
            //x分量，影响：曲度、精准度、左右曲度、风向
            Vector3 pointPos = new Vector3();
            float vx = initialVelocity.x + GlobalAcc.x * time + InitHookVelocity + HookAcc * time;
            pointPos.x = initPos.x + (initialVelocity.x * time + 0.5f * GlobalAcc.x * time * time + InitHookVelocity * time + 0.5f * HookAcc * time * time);
            //y分量，影响：重力
            float vy = initialVelocity.y + LogicConstantData.Gravity * time;
            pointPos.y = initPos.y + (initialVelocity.y * time + 0.5f * LogicConstantData.Gravity * time * time);
            //z分量，影响：杆力量、球力量、风向
            float vz = initialVelocity.z + GlobalAcc.z * time;
            pointPos.z = initPos.z + (initialVelocity.z * time + 0.5f * GlobalAcc.z * time * time);
            point.Position = pointPos;
            point.Velocity = new Vector3(vx, vy, vz);
            //Debug.Log("Pos:" + pointPos.x + "," + pointPos.y + "," + pointPos.z + "  Vel:" + vx + "," + vy + "," + vz);
            return point;
        }

        public void Clear()
        {
            CosElevationRad = 0;
            SinElevationRad = 0;
            HookAcc = 0;
            InitHookVelocity = 0;
            AccuracyRevise = 0;
        }
    }
}
