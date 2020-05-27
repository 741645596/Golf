using IGG.Core.Data.DataCenter.GolfAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IGG.Core.Helper;
using IGG.Core.Manger.Coroutine;
/// <summary>
/// Author  zhulin
/// Date    2019.6.5
/// Desc    战斗管理器
/// </summary>
namespace IGG.Core.Data.DataCenter.Battle
{
    public static class BattleM
    {
        private static int g_Times = 0;
        private static GolfPath g_Path = null;
        public static GolfPath Path
        {
            get { return g_Path; }
        }
        /// <summary>
        /// 环的位置
        /// </summary>
        private static Vector3 g_RingPos;
        public static Vector3 RingPos
        {
            get { return g_RingPos; }
            set { 
                if (g_RingPos != value)
                {
                    g_RingPos = value;
                    GolfPathInfo pathInfo = GetPath(null);
                    SetRingPosAndNeedSpeed(g_RingPos, pathInfo.LauchSpeed, true);
                    EventCenter.DispatchEvent(EventCenterType.Battle_DrawPath, -1, pathInfo);
                }
            }
        }

        /// <summary>
        /// 获取推杆的速度方向
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetRollSpeedDir()
        {
            return Path.GetRollSpeedDir(BallPos);
        }
        /// <summary>
        /// 球运动的信息
        /// </summary>
        private static BallRunInfo g_BallRunInfo;
        public static BallRunInfo RunBallInfo
        {
            get { return g_BallRunInfo; }
        }
        /// <summary>
        /// 设置球的运动信息
        /// </summary>
        /// <param name="v"></param>
        public static void SetBallRunInfo(BallRunInfo v)
        {
            SetBallRunInfo(v.Pos, v.TerrianType, v.Status);
        }
        /// <summary>
        /// 设置球的运动信息
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        public static void SetBallRunInfo(Vector3 pos, AreaType type, BallRunStatus status)
        {
            if (status != g_BallRunInfo.Status && status == BallRunStatus.Stop)
            {
                BallStopProc(pos, type);
                SetBattleStatus(BattleStatus.RoundFinish);
            }
            g_BallRunInfo.Pos = pos;
            g_BallRunInfo.TerrianType = type;
            g_BallRunInfo.Status = status;
        }
        /// <summary>
        /// 球停下来的处理
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="type"></param>
        private static void BallStopProc(Vector3 pos, AreaType type)
        {
            if (type == AreaType.PuttingGreen)
            {
                TipsHelper.ShowTipsMessage("球落在果岭", 1);
            }
            else if (type == AreaType.PuttingGreenLine)
            {
                TipsHelper.ShowTipsMessage("球落在果岭边缘", 1);
            }
            else if (type == AreaType.Fairway)
            {
                TipsHelper.ShowTipsMessage("球落在球道上", 1);
            }
            else if (type == AreaType.SandBunker)
            {
                TipsHelper.ShowTipsMessage("球落在沙坑", 1);
            }
            else if (type == AreaType.Rough)
            {
                TipsHelper.ShowTipsMessage("球落在长草区", 1);
            }
            else if (type == AreaType.OutOfBounds)
            {
                TipsHelper.ShowTipsMessage("球落在界外", 1);
            }
            TipsHelper.ShowTipsMessage("距离：" + Vector3.Distance(pos, SelfSel.StartPos), 1);
        }
        /// <summary>
        ///  球的位置
        /// </summary>
        public static Vector3 BallPos
        {
            get { return g_BallRunInfo.Pos; }
        }
        /// <summary>
        ///  球所在地形类型
        /// </summary>
        public static AreaType BallInAra
        {
            get { return g_BallRunInfo.TerrianType; }
        }
        /// <summary>
        /// 状态状态管理
        /// </summary>
        private static BattleStatus g_BattleStatus = BattleStatus.None;
        private static BattleStatus BatStatus
        {
            get { return g_BattleStatus; }
        }
        /// <summary>
        /// 设置状态状态。
        /// </summary>
        /// <param name="Status"></param>
        public static void SetBattleStatus(BattleStatus Status, object Param = null)
        {
            if (g_BattleStatus != Status)
            {
                g_BattleStatus = Status;
                if (Status == BattleStatus.Init)
                {
                    Init();
                }
                else if (Status == BattleStatus.Ready)
                {
                    Ready();
                }
                else if (Status == BattleStatus.RoundStart)
                {
                    Node.StartCoroutine(RoundStart(1.0f));
                }
                else if (Status == BattleStatus.Round_FireBall)
                {
                    RoundFireBall(Param);
                }
                else if (Status == BattleStatus.RoundFinish)
                {
                    RoundFinish();
                }
                else if (Status == BattleStatus.RoundInterval)
                {
                    Node.StartCoroutine(RoundInterval(3.0f));
                }
            }
        }
        /// <summary>
        /// 准备战斗
        /// </summary>
        private static void Ready()
        {
            TipsHelper.ShowTipsMessage("大神，轮到你了！", 1);
            SetBattleStatus(BattleStatus.RoundStart);
        }

        /// <summary>
        /// 回合开始
        /// </summary>
        /// <returns></returns>
        public static IEnumerator RoundStart(float waitTime)
        {
            g_Times++;
            m_SelfSel.SetStartPos(BallPos);
            if (BattleM.BallInAra != AreaType.PuttingGreen)
            {
                Vector3 PredictionPos = Path.CalcPredictionRingPos(GetFlyInput(null), BallPos);
                EventCenter.DispatchEvent(EventCenterType.Battle_UpdateRingPos, -1, PredictionPos);
            }
            EventCenter.DispatchEvent(EventCenterType.Battle_RoundStart, -1, null);
            yield return Yielders.GetWaitForSeconds(waitTime);
            // 先预测位置
            if (BattleM.BallInAra != AreaType.PuttingGreen)
            {
                EventCenter.DispatchEvent(EventCenterType.Battle_ShowRing, -1, true);
            }

        }
        /// <summary>
        /// 发球
        /// </summary>
        private static void RoundFireBall(object Param)
        {
            if (Param == null)
            {
                return;
            }
            BattingParam param = Param as BattingParam;
            GolfPathInfo pathInfo = GetPath(param);
            EventCenter.DispatchEvent(EventCenterType.Battle_BattingFinish, -1, pathInfo);
            EventCenter.DispatchEvent(EventCenterType.Battle_ShowRing, -1, false);
            m_SelfSel.SetBallSpeed(param.BattingSpeed);
            m_SelfSel.SetWindSpeed(param.WindSpeed);
            m_SelfSel.SetCurFBSpin(param.CurFBSpin);
            m_SelfSel.SetCurLRSpin(param.CurLRSpin);
            m_SelfSel.SetCurHookAngle(param.CurHookAngle);
            m_SelfSel.SetCurAccuracyOffset(param.CurAccuracyOffset);
            m_SelfSel.SetResultPos(pathInfo.GetRuseltPos());
            BattleOperateInfo v = new BattleOperateInfo(m_SelfSel);
            AddBattleOperate(true, g_Times, v);
        }

        /// <summary>
        /// 更新推杆参数
        /// </summary>
        /// <param name="Param"></param>
        public static void SetRollOperation(BattingParam Param)
        {
            GolfPathInfo pathInfo = GetPath(Param);
            EventCenter.DispatchEvent(EventCenterType.Battle_DrawPath, -1, pathInfo);
            EventCenter.DispatchEvent(EventCenterType.Battle_PathCrossHole, -1, pathInfo.CheckCrossHole());
        }
        /// <summary>
        /// 回合结束
        /// </summary>
        private static void RoundFinish()
        {
            SetBattleStatus(BattleStatus.RoundInterval);
        }

        private static IEnumerator RoundInterval(float WaitTime)
        {
            yield return Yielders.GetWaitForSeconds(WaitTime);
            SetBattleStatus(BattleStatus.RoundStart);
        }


        /// <summary>
        /// 初始化
        /// </summary>
        private static void Init()
        {
            g_Path = new GolfPath(BattleDC.Map);
            SetClub(BattleDC.SelClub, true);
            SetBallID(BattleDC.SelBall, true);
            g_BallRunInfo.Pos = BattleDC.Map.StartPos;
            g_BallRunInfo.TerrianType = AreaType.Fairway;
            g_BallRunInfo.Status = BallRunStatus.Stop;
            if (Node != null)
            {
                Node.LoadMap2Ball(BattleDC.Map);
            }
            SetBattleStatus(BattleStatus.Ready);

        }
        /// <summary>
        /// 已方所选择操作
        /// </summary>
        private static BattleOperateInfo m_SelfSel = new BattleOperateInfo();
        public static BattleOperateInfo SelfSel
        {
            get { return m_SelfSel; }
        }
        /// <summary>
        /// 对方所选择操作
        /// </summary>
        private static BattleOperateInfo m_PlayerSel = new BattleOperateInfo();
        public static BattleOperateInfo PlayerSel
        {
            get { return m_PlayerSel; }
        }
        /// <summary>
        /// 战斗节点
        /// </summary>
        private static BattlNode g_BattleNode;
        public static BattlNode Node
        {
            get { return g_BattleNode; }
        }
        /// <summary>
        /// 设置战斗节点
        /// </summary>
        /// <returns></returns>
        public static void SetBattleNode(BattlNode node)
        {
            g_BattleNode = node;
        }

        /// <summary>
        /// 战斗列表
        /// </summary>
        private static List<Battle> g_ListBattle = null;
        private static Battle g_CurBattle = null;
        public static Battle CurBattle
        {
            get { return g_CurBattle; }
        }
        /// <summary>
        /// 添加一个战斗
        /// </summary>
        /// <param name="battle"></param>
        public static void AddBattle(Battle battle)
        {
            if (battle != null)
            {
                if (g_ListBattle == null)
                {
                    g_ListBattle = new List<Battle>();
                }
                else
                {
                    g_ListBattle.Clear();
                }
                g_CurBattle = battle;
                g_ListBattle.Add(battle);
            }
        }

        /// <summary>
        /// 清理数据
        /// </summary>
        public static void Clear()
        {
            if (g_ListBattle != null)
            {
                foreach (Battle battle in g_ListBattle)
                {
                    g_ListBattle.Clear();
                }
                g_ListBattle.Clear();
                g_ListBattle = null;
            }
        }
        /// <summary>
        /// 选择操作数据
        /// </summary>
        /// <param name="isSelf"></param>
        /// <returns></returns>
        private static BattleOperateInfo GetBattleOperateInfo(bool isSelf)
        {
            if (isSelf == true)
            {
                return m_SelfSel;
            }
            else
            {
                return m_PlayerSel;
            }
        }
        /// <summary>
        /// 设置球杆ID
        /// </summary>
        public static void SetClub(ClubInfo Club, bool isSelf)
        {
            GetBattleOperateInfo(isSelf).Club = Club;
        }
        /// <summary>
        /// 设置球ID
        /// </summary>
        public static void SetBallID(BallInfo Ball, bool isSelf)
        {
            GetBattleOperateInfo(isSelf).Ball = Ball;
        }

        /// <summary>
        /// 设置五环的位置及所需要的速度
        /// </summary>
        private static void SetRingPosAndNeedSpeed(Vector3 BullseyePos, Vector3 NeedSpeed, bool isSelf)
        {
            BattleOperateInfo op = GetBattleOperateInfo(isSelf);
            if (op != null)
            {
                op.SetNeedSpeed(NeedSpeed);
                op.SetBullseyePos(BullseyePos);
            }
        }

        /// <summary>
        /// 击球完成,记录击球操作.
        /// </summary>
        /// <param name="IsSelf"></param>
        /// <param name="OperateTimes"></param>
        /// <param name="v"></param>
        private static void AddBattleOperate(bool IsSelf, int OperateTimes, BattleOperateInfo v)
        {
            if (g_CurBattle != null)
            {
                g_CurBattle.AddBattleOperate(IsSelf, OperateTimes, v);
            }
        }
        /// <summary>
        /// temp
        /// </summary>
        /// <returns></returns>
        public static GolfPathInfo GetPath(BattingParam param = null)
        {
            if (BallInAra == AreaType.PuttingGreen)
            {
                return Path.CalcRollPath(GetPushInput(param), BallPos, RingPos);
            }
            else
            {
                return Path.CalcFlyPath(GetFlyInput(param), BallPos, RingPos);
            }
        }
        /// <summary>
        /// 获取滚动输入
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static GolfPushInput GetPushInput(BattingParam param)
        {
            GolfPushInput pushInput = new GolfPushInput();
            if (param != null)
            {
                pushInput.ClubType = (uint)param.Club.Type;
                pushInput.ClubStrength = param.Club.Strength;
                pushInput.BallStrengthPercent = param.Ball.StrengthPercent;
                pushInput.BattingVelocity = param.BattingSpeed;
            }
            else
            {
                pushInput.ClubType = (uint)BattleDC.SelClub.Type;
                pushInput.ClubStrength = BattleDC.SelClub.Strength;
                pushInput.BallStrengthPercent = BattleDC.SelBall.StrengthPercent;
                pushInput.BattingVelocity = Vector3.zero;
            }
            return pushInput;
        }

        /// <summary>
        /// 获取飞行输入
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static GolfFlyInput GetFlyInput(BattingParam param)
        {
            GolfFlyInput flyInput = new GolfFlyInput();
            if (param != null)
            {
                flyInput.ClubType = (uint)param.Club.Type;
                flyInput.ClubStrength = param.Club.Strength;
                flyInput.BallStrengthPercent = param.Ball.StrengthPercent;
                flyInput.BattingVelocity = param.BattingSpeed;
                flyInput.Spin = new GolfSpin(param.CurFBSpin, param.CurFBSpin, param.CurFBSpin, param.CurFBSpin, param.CurLRSpin);
                flyInput.HookAngle = param.CurHookAngle;
                flyInput.Accuracy = new GolfAccuracy(param.Club.Accuracy, param.CurAccuracyOffset);
                flyInput.Wind = new GolfWind(param.WindSpeed);
            }
            else
            {
                flyInput.ClubType = (uint)BattleDC.SelClub.Type;
                flyInput.ClubStrength = BattleDC.SelClub.Strength;
                flyInput.BallStrengthPercent = BattleDC.SelBall.StrengthPercent;
                flyInput.HookAngle = 0;
                flyInput.BattingVelocity = Vector3.zero;
                flyInput.Spin = new GolfSpin(0, 0, 0, 0, 0);
                flyInput.Wind = new GolfWind(Vector3.zero);
                flyInput.Accuracy = new GolfAccuracy(0, 0);
            }
            return flyInput;
        }
    }

        /// <summary>
        /// 战斗状态。
        /// </summary>
        public enum BattleStatus
        {
            None = 0,
            /// <summary>
            /// 战斗初始化
            /// </summary>
            Init  = 1,    
            /// <summary>
            /// 战斗准备中
            /// </summary>
            Ready =2,
            /// <summary>
            /// 回合开始
            /// </summary>
            RoundStart =3,
            /// <summary>
            /// 回合进行中,击球时刻
            /// </summary>
            Round_FireBall = 4,
            /// <summary>
            /// 回合完成
            /// </summary>
            RoundFinish =5,
            /// <summary>
            /// 回合间歇中
            /// </summary>
            RoundInterval =6,
            /// <summary>
            /// 战斗结束
            /// </summary>
            Finish   = 7
        }
}
