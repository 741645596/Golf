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
    /// <summary>
    /// 高尔夫运动输入
    /// </summary>
    public class GolfInput
    {
        /// <summary>
        /// 球杆类型
        /// </summary>
        public uint ClubType { get; set; } = 0;
        /// <summary>
        /// 球杆力量
        /// </summary>
        public float ClubStrength { get; set; } = 0;
        /// <summary>
        /// 球力量百分比加成
        /// </summary>
        public float BallStrengthPercent { get; set; } = 0;

        public Vector3 BattingVelocity { get; set; } = Vector3.zero;

        public virtual void Clear()
        {
            ClubType = 0;
            ClubStrength = 0;
            BallStrengthPercent = 0;
            BattingVelocity = Vector3.zero;
        }
    }

    /// <summary>
    /// 滞空输入
    /// </summary>
    public class GolfFlyInput : GolfInput
    {
        /// <summary>
        /// 旋球值
        /// </summary>
        public GolfSpin Spin { get; set; } = null;
        /// <summary>
        /// 曲球角度
        /// </summary>
        public float HookAngle { get; set; } = 0;
        /// <summary>
        /// 风切
        /// </summary>
        public GolfWind Wind { get; set; } = null;
        /// <summary>
        /// 精准度
        /// </summary>
        public GolfAccuracy Accuracy { get; set; } = null;

        public GolfFlyInput()
        {
            Spin = new GolfSpin();
            Wind = new GolfWind();
            Accuracy = new GolfAccuracy();
        }
        public override void Clear()
        {
            base.Clear();
            if (Spin != null)
            {
                Spin.Clear();
            }
            if (Wind != null)
            {
                Wind.Clear();
            }
            if (Accuracy != null)
            {
                Accuracy.Clear();
            }
        }
    }

    /// <summary>
    /// 推球时的输入
    /// </summary>
    public class GolfPushInput : GolfInput
    {
        /// <summary>
        /// 推杆角度
        /// </summary>
        public float PushAngle { get; set; } = 0;
    }

    /// <summary>
    /// 高尔夫旋球数据
    /// </summary>
    public class GolfSpin
    {
        /// <summary>
        /// 第一段旋球值
        /// </summary>
        public float FirstFBSpin { get; set; } = 0;
        /// <summary>
        /// 第二段旋球值
        /// </summary>
        public float SecondFBSpin { get; set; } = 0;
        /// <summary>
        /// 第三段旋球值
        /// </summary>
        public float ThirdFBSpin { get; set; } = 0;
        /// <summary>
        /// 第四段旋球值
        /// </summary>
        public float FourFBSpin { get; set; } = 0;
        /// <summary>
        /// 左右旋球值
        /// </summary>
        public float LRSpin { get; set; } = 0;

        public GolfSpin()
        {

        }

        public GolfSpin(float spin1, float spin2, float spin3, float spin4, float lrSpin)
        {
            FirstFBSpin = spin1;
            SecondFBSpin = spin2;
            ThirdFBSpin = spin3;
            FourFBSpin = spin4;
            LRSpin = LRSpin;
        }

        public void Clear()
        {
            FirstFBSpin = 0;
            SecondFBSpin = 0;
            ThirdFBSpin = 0;
            FourFBSpin = 0;
            LRSpin = 0;
        }
    }

    /// <summary>
    /// 风切
    /// </summary>
    public class GolfWind
    {
        /// <summary>
        /// 风切
        /// </summary>
        public Vector3 Wind { get; set; } = Vector3.zero;

        public GolfWind()
        {

        }

        public GolfWind(Vector3 wind)
        {
            Wind = wind;
        }

        public void Clear()
        {
            Wind = Vector3.zero;
        }
    }

    public class GolfAccuracy
    {
        /// <summary>
        /// 杆的精准度
        /// </summary>
        public float ClubAccuracy { get; set; } = 0;
        /// <summary>
        /// 指针精准度偏移
        /// </summary>
        public float AccuracyOffset { get; set; } = 0;
        public GolfAccuracy()
        {

        }
        public GolfAccuracy(float a, float offset)
        {
            ClubAccuracy = a;
            AccuracyOffset = offset;
        }
        public void Clear()
        {
            ClubAccuracy = 0;
            AccuracyOffset = 0;
        }
    }
}
