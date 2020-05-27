using IGG.Core.Data.DataCenter.GolfAI;
using UnityEngine;
    /// <summary>
    /// Author  zhulin
    /// Date    2019.5.27
    /// Desc    Battle模块动态数据集
    /// </summary>
namespace IGG.Core.Data.DataCenter.Battle {
    // 定义模块内的数据结构
    /// <summary>
    /// 战斗操作对象
    /// </summary>
    public class BattleOperateInfo
    {
        public BattleOperateInfo(){}
        public BattleOperateInfo(BattleOperateInfo a)
        {
            this.m_StartPos = a.m_StartPos;
            this.m_BallSpeed = a.m_BallSpeed;
            this.m_BullseyePos = a.m_BullseyePos;
            this.m_NeedSpeed = a.m_NeedSpeed;
            this.m_ResultPos = a.m_ResultPos;
            this.m_WindSpeed = a.m_WindSpeed;
            this.m_CurFBSpin = a.m_CurFBSpin;
            this.m_CurLRSpin = a.m_CurLRSpin;
            this.m_CurHookAngle = a.m_CurHookAngle;
            this.m_CurAccuracyOffset = a.m_CurAccuracyOffset;
            this.m_Ball = new BallInfo(a.m_Ball);
            this.m_Club = new ClubInfo(a.m_Club);
        }
        /// <summary>
        /// 击球位置
        /// </summary>
        private Vector3 m_StartPos;
        public Vector3 StartPos
        {
            get { return m_StartPos; }
        }
        public void SetStartPos(Vector3 pos)
        {
            m_StartPos = pos;
        }
        /// <summary>
        /// 五环靶心位置
        /// </summary>
        private Vector3 m_BullseyePos;
        public Vector3 BullseyePos
        {
            get { return m_BullseyePos; }
        }
        public void SetBullseyePos(Vector3 pos)
        {
            m_BullseyePos = pos;
        }
        /// <summary>
        /// 射中靶心需要的速度，不考虑外力影响
        /// </summary>
        private Vector3 m_NeedSpeed;
        public Vector3 NeedSpeed
        {
            get { return m_NeedSpeed; }
        }
        public void SetNeedSpeed(Vector3 needSpeed)
        {
            m_NeedSpeed = needSpeed;
        }
        /// <summary>
        /// 球落地位置
        /// </summary>
        private Vector3 m_ResultPos;
        public Vector3 ResultPos
        {
            get { return m_ResultPos; }
        }
        public void SetResultPos(Vector3 pos)
        {
            m_ResultPos = pos;
        }
        /// <summary>
        /// 击球速度
        /// </summary>
        private Vector3 m_BallSpeed;
        public Vector3 BallSpeed
        {
            get { return m_BallSpeed; }
        }
        public void SetBallSpeed(Vector3 speed)
        {
            m_BallSpeed = speed;
        }
        /// <summary>
        /// 当前上下旋球值
        /// </summary>
        private float m_CurFBSpin;
        public float CurFBSpin
        {
            get { return m_CurFBSpin; }
        }
        public void SetCurFBSpin(float curFBSpin)
        {
            m_CurFBSpin = curFBSpin;
        }
        /// <summary>
        /// 当前侧旋值
        /// </summary>
        private float m_CurLRSpin;
        public float CurLRSpin
        {
            get { return m_CurLRSpin; }
        }
        public void SetCurLRSpin(float curLRSpin)
        {
            m_CurLRSpin = curLRSpin;
        }
        /// <summary>
        /// 当前曲球角度
        /// </summary>
        private float m_CurHookAngle;
        public float CurHookAngle
        {
            get { return m_CurHookAngle; }
        }
        public void SetCurHookAngle(float curHookAngle)
        {
            m_CurHookAngle = curHookAngle;
        }
        /// <summary>
        /// 当前精准度偏差
        /// </summary>
        private float m_CurAccuracyOffset;
        public float CurAccuracyOffset
        {
            get { return m_CurAccuracyOffset; }
        }
        public void SetCurAccuracyOffset(float curAccuracyOffset)
        {
            m_CurAccuracyOffset = curAccuracyOffset;
        }
        /// <summary>
        /// 球杆ID
        /// </summary>
        private ClubInfo m_Club=new ClubInfo();
        public ClubInfo Club
        {
            get { return m_Club; }
            set { m_Club = value; }
        }
        /// <summary>
        /// 球ID
        /// </summary>
        private BallInfo m_Ball=new BallInfo();
        public BallInfo Ball
        {
            get { return m_Ball; }
            set { m_Ball = value; }
        }

        /// <summary>
        /// 风速
        /// </summary>
        private Vector3 m_WindSpeed;
        public Vector3 WindSpeed
        {
            get { return m_WindSpeed; }
        }
        public void SetWindSpeed(Vector3 speed)
        {
            m_WindSpeed = speed;
        }
    }
    /// <summary>
    /// 球的数据
    /// </summary>
    public class BallInfo
    {
        public BallInfo() { }
        public BallInfo(BallInfo a)
        {
            this.ID = a.ID;
            this.StrengthPercent = a.StrengthPercent;
            this.windRevise = a.windRevise;
            this.LRSpin = a.LRSpin;
        }
        public int ID;
        /// <summary>
        /// 力量百分比
        /// </summary>
        public float StrengthPercent;
        /// <summary>
        /// 抗风
        /// </summary>
        public float windRevise;
        /// <summary>
        /// 侧旋
        /// </summary>
        public float LRSpin;

    }
    /// <summary>
    /// 球杆的数据
    /// </summary>
    public class ClubInfo
    {
        public ClubInfo() { }
        public ClubInfo(ClubInfo a)
        {
            this.ID = a.ID;
            this.Type = a.Type;
            this.Strength = a.Strength;
            this.Accuracy = a.Accuracy;
            this.ForwardSpin = a.ForwardSpin;
            this.BackSpin = a.BackSpin;
            this.Hook = a.Hook;
        }
        public int ID;
        /// <summary>
        /// 杆类型
        /// </summary>
        public uint Type;
        /// <summary>
        /// 力量
        /// </summary>
        public float Strength;
        /// <summary>
        /// 精准度
        /// </summary>
        public float Accuracy;
        /// <summary>
        /// 上旋
        /// </summary>
        public float ForwardSpin;
        /// <summary>
        /// 下旋
        /// </summary>
        public float BackSpin;
        /// <summary>
        /// 曲球
        /// </summary>
        public float Hook;
    }


    /// <summary>
    /// 球运行信息
    /// </summary>
    public struct BallRunInfo
    {
        public BallRunInfo(Vector3 pos, AreaType type, BallRunStatus status)
        {
            this.Pos = pos;
            this.TerrianType = type;
            this.Status = status;
        }
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Pos;
        /// <summary>
        /// 状态
        /// </summary>
        public BallRunStatus Status;
        /// <summary>
        /// 所在地形的类型
        /// </summary>
        public AreaType TerrianType;

    }

    public enum BallRunStatus
    {
        /// <summary>
        /// 停止
        /// </summary>
        Stop  = 0,
        /// <summary>
        /// 飞行中
        /// </summary>
        Fly   = 1,
        /// <summary>
        /// 滚动中
        /// </summary>
        Roll  = 2,
    }
}
