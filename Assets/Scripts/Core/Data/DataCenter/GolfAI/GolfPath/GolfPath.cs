using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IGG.Core.Data.DataCenter.Battle;
using IGG.Core.Data.Config;

/// <summary>
/// Author  zhulin
/// Date    2019.6.5
/// Desc    高尔夫线路计算
/// </summary>
namespace IGG.Core.Data.DataCenter.GolfAI
{
    public class GolfPath
    {
        private GolfFlyInput FlyInput = null;
        private GolfPushInput PushInput = null;
        private GolfMotionPath MotionPath = null;
        private GolfCourseMap m_CurMap = null;

        /// <summary>
        /// 最大旋球速度
        /// </summary>
        private Dictionary<int, List<float>> SpinDic = null;

        public GolfPath(GolfCourseMap map)
        {
            m_CurMap = map;
            MotionPath = new GolfMotionPath(map);
            SpinDic = new Dictionary<int, List<float>>();
            //旋球临时数据
            for (int j = 0; j < 4; j++)
            {
                List<float> spinList = new List<float>();
                spinList.Clear();
                for (int i = 1; i <= 5; i++)
                {
                    GroundmaterialConfig groundMatConfig = GroundmaterialDao.Inst.GetCfg((uint)i);
                    spinList.Add(groundMatConfig.SpinAddition1);
                }
                SpinDic.Add(j, spinList);
            }
        }


        /// <summary>
        /// 计算需要的速度
        /// </summary>
        /// <param name="club">所用的球杆</param>
        /// <param name="ball">所使用的球</param>
        /// <param name="startPos">发球点</param>
        /// <param name="RingPos">五环的位置，第一个反弹点</param>
        /// <returns></returns>
        public Vector3 CalcNeedSpeed(ClubInfo club, BallInfo ball, Vector3 startPosition, Vector3 endPosition, float hookAngle = 0)
        {
            float sinElevationRad = 0;
            float cosElevationRad = 0;
            GetClubElevationRad(club.Type, out sinElevationRad, out cosElevationRad);
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
            float divisor = (2 * deltaPosition.y * cosElevationRad * cosElevationRad * CosHorizonRad * CosHorizonRad
                - 2 * deltaPosition.z * sinElevationRad * cosElevationRad * CosHorizonRad);
            if (divisor == 0)
            {
                return Vector3.zero;
            }
            float squareV = (deltaPosition.z * deltaPosition.z * LogicConstantData.Gravity) / divisor;
            float v = Mathf.Sqrt(Mathf.Abs(squareV));

            //初速度分量
            float vx = v * cosElevationRad * SinHorizonRad;
            float vy = v * sinElevationRad;
            float vz = v * cosElevationRad * CosHorizonRad;

            //反向时的处理
            Vector3 dir = deltaPosition.normalized;
            if (dir.z < 0)
            {
                vz = -vz;
                vx = -vx;
                hookAngle = -hookAngle;
            }

            //计算曲球初速度和加速度
            //CalcHook(vy, vz, deltaPosition.z, hookAngle);

            //计算精准度风切校正
            //CalcAccuracyRevise(vx, vz);

            //最终初速度
            Vector3 velocity = new Vector3(vx, vy, vz);
            return velocity;
        }

        /// <summary>
        /// 获取发球仰角正弦余弦弧度值
        /// </summary>
        /// <param name="clubType"></param>
        /// <param name="sinElevationRad"></param>
        /// <param name="cosElevationRad"></param>
        public void GetClubElevationRad(uint clubType, out float sinElevationRad, out float cosElevationRad)
        {
            cosElevationRad = 0;
            sinElevationRad = 0;
            ClubparameterConfig clubConfig = ClubparameterDao.Inst.GetCfg(clubType);
            if (clubConfig != null)
            {
                cosElevationRad = clubConfig.AngleCos;
                sinElevationRad = clubConfig.AngleSin;
            }
        }

        /// <summary>
        /// //计算精准度风切校正
        /// </summary>
        /// <param name="vx"></param>
        /// <param name="vz"></param>
        public void CalcAccuracyRevise(Vector3 initVelocity, float accuracyRevise, ref Vector3 globalAcc)
        {
            float vx = initVelocity.x;
            float vz = initVelocity.z;
            //垂直于速度方向
            //当AccuracyRevise > 0时为vz,0,-vx， 当AccuracyRevise < 0时为-vz,0,vx
            Vector3 dirAccuracy = new Vector3(vz, 0, -vx);
            dirAccuracy.Normalize();
            globalAcc += dirAccuracy * accuracyRevise;
        }

        /// <summary>
        /// 计算精准度校正
        /// </summary>
        /// <param name="accuracy"></param>
        /// <param name="accuracyOffset"></param>
        /// <returns></returns>
        public float CalcAccuracy(float accuracy, float accuracyOffset)
        {
            AccuracyregulateConfig accConfig = AccuracyregulateDao.Inst.GetCfg((int)accuracy);
            float revise = accuracyOffset * LogicConstantData.WindConvert * accConfig.WindRegulate / 100;
            return revise;
        }

        //public void CalcHook(float vy, float vz, float pz, float hookAngle)
        //{
        //    //曲球只改变曲度，不改变落点
        //    //根据 曲球角度正切 = 曲球初速度速度 / y方向速度 求曲球初速度
        //    float TanHookRad = Mathf.Tan(hookAngle * Mathf.Deg2Rad);
        //    InitHookVelocity = vy * TanHookRad;
        //    //全程时间
        //    float t = pz / vz;
        //    //反推加速度
        //    HookAcc = -2 * InitHookVelocity / t;
        //}
        public void CalcHook(Vector3 initVelocity, Vector3 startPosition, Vector3 endPosition, float hookAngle, ref float InitHookVelocity, ref float HookAcc)
        {
            float vy = initVelocity.y;
            float vz = initVelocity.z;
            float pz = endPosition.z - startPosition.z;
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
        /// 计算，推荐出来的五环的位置
        /// </summary>
        /// <param name="club">所用的球杆</param>
        /// <param name="ball">所使用的球</param>
        /// <param name="startPos">发球点</param>
        /// <returns></returns>
        public Vector3 CalcPredictionRingPos(GolfFlyInput input, Vector3 startPos)
        {
            Vector3 dir = m_CurMap.BallHolePos - startPos;
            dir.Normalize();
            Vector3 RingPos = startPos + dir * (input.ClubStrength + input.BallStrengthPercent / 100);
            FlyInput = input;
            MotionPath.Clear();
            MotionPath.Init();
            //计算球路径
            ClubparameterConfig clubConfig = ClubparameterDao.Inst.GetCfg(FlyInput.ClubType);
            if (clubConfig == null)
            {
                return RingPos;
            }
            MotionPath.BattingVelocity = FlyInput.BattingVelocity;
            MotionPath.Fly.CosElevationRad = clubConfig.AngleCos;
            MotionPath.Fly.SinElevationRad = clubConfig.AngleSin;
            MotionPath.SpinValueList.Clear();
            MotionPath.SpinValueList.Add(FlyInput.Spin.FirstFBSpin);
            MotionPath.SpinValueList.Add(FlyInput.Spin.SecondFBSpin);
            MotionPath.SpinValueList.Add(FlyInput.Spin.ThirdFBSpin);
            MotionPath.SpinValueList.Add(FlyInput.Spin.FourFBSpin);
            MotionPath.SpinLeftRight = FlyInput.Spin.LRSpin;
            MotionPath.HookAngle = FlyInput.HookAngle;
            MotionPath.SpinDic = SpinDic;
            MotionPath.CalcPath(startPos, RingPos, Time.fixedDeltaTime, false);
            return RingPos;
        }


        /// <summary>
        /// 计算飞行路径(击球)
        /// </summary>
        /// <param name="Input">输入数据</param>
        /// <param name="startPos">发球点</param>
        /// <returns></returns>
        public GolfPathInfo CalcFlyPath(GolfFlyInput input, Vector3 startPos, Vector3 predictionPos)
        {
            FlyInput = input;
            MotionPath.Clear();
            MotionPath.Init();
            //计算球路径
            ClubparameterConfig clubConfig = ClubparameterDao.Inst.GetCfg(FlyInput.ClubType);
            if (clubConfig == null)
            {
                return null;
            }
            MotionPath.BattingVelocity = FlyInput.BattingVelocity;
            MotionPath.Fly.CosElevationRad = clubConfig.AngleCos;
            MotionPath.Fly.SinElevationRad = clubConfig.AngleSin;
            MotionPath.SpinValueList.Clear();
            MotionPath.SpinValueList.Add(FlyInput.Spin.FirstFBSpin);
            MotionPath.SpinValueList.Add(FlyInput.Spin.SecondFBSpin);
            MotionPath.SpinValueList.Add(FlyInput.Spin.ThirdFBSpin);
            MotionPath.SpinValueList.Add(FlyInput.Spin.FourFBSpin);
            MotionPath.SpinLeftRight = FlyInput.Spin.LRSpin;
            MotionPath.HookAngle = FlyInput.HookAngle;
            MotionPath.Fly.AccuracyRevise = CalcAccuracy(FlyInput.Accuracy.ClubAccuracy, FlyInput.Accuracy.AccuracyOffset);
            MotionPath.SpinDic = SpinDic;
            MotionPath.CalcPath(startPos, predictionPos, Time.fixedDeltaTime, false);
            return MotionPath.PathInfo;
        }

        /// <summary>
        /// 计算滚动路径（推杆）
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <param name="startPos">发球点</param>
        /// <returns></returns>
        public GolfPathInfo CalcRollPath(GolfPushInput input, Vector3 startPos, Vector3 predictionPos)
        {
            PushInput = input;
            MotionPath.Clear();
            MotionPath.Init();
            MotionPath.PathInfo.ListPt.Clear();

            GolfRollPoint rollPoint = new GolfRollPoint();
            rollPoint.Position = startPos;
            SearchResult sr = null;
            m_CurMap.SearchMapTriangle(rollPoint.Position, ref sr);
            rollPoint.Velocity = PushInput.BattingVelocity;
            rollPoint.IsRoll = true;
            rollPoint.Poly = sr.Poly;
            rollPoint.AType = sr.Type;
            rollPoint.ColType = CollisionType.Normal;
            rollPoint.Status = BallRunStatus.Roll;
            MotionPath.PathInfo.AddPathPoint(rollPoint);
            MotionPath.CalcPath(startPos, predictionPos, Time.fixedDeltaTime, true);
            return MotionPath.PathInfo;
        }



        /// <summary>
        /// 获取推杆的速度方向
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRollSpeedDir(Vector3 ballPos)
        {
            Vector3 dir = m_CurMap.BallHolePos - ballPos;
            SearchResult result = new SearchResult();
            if (m_CurMap != null && m_CurMap.SearchMapTriangle(ballPos, ref result) == true)
            {
                dir = MathUtil.CalcTangentVector(dir, result.Poly.Normal);
                return dir.normalized;
            }
            else
            {
                return dir.normalized;
            }
        }


        /// <summary>
        /// 清理操作。
        /// </summary>
        public void Clear()
        {
            m_CurMap = null;
            if (MotionPath != null)
            {
                MotionPath.Clear();
                MotionPath = null;
            }
            if (FlyInput != null)
            {
                FlyInput.Clear();
                FlyInput = null;
            }
            if (PushInput != null)
            {
                PushInput.Clear();
                PushInput = null;
            }
        }
    }
}
