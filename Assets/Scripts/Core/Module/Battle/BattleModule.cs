using IGG.Core.Module;
using IGG.Logging;
using IGG.Core.Data.DataCenter;
using IGG.Core.Data.DataCenter.Battle;
using IGG.Core.Data.DataCenter.GolfAI;
using UnityEngine;

namespace IGG.Core.Module
{
    /// <summary>
    /// Author  zhulin
    /// Date    2019.5.27
    /// Desc    Battle模块
    /// </summary>
    public class BattleModule : IModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void OnInit()
        {
		    // 用于模拟数据的一个接口，正式需关闭
		    BattleDC.SimulationData();
            RegisterDCHooks();
        }
		
		/// <summary>
        /// 注册msg消息到DataCenter,关联对应的DC处理msg消息
        /// </summary>
        public void RegisterDCHooks()
        {
            /// 示例
            ///DataCenter.RegisterDCHooks(eMsgTypes.MsgServerKeepLive, GameTimeDC.RecvMsgGS2CLServerTimeNotice);
        }

        /// <summary>
        /// 重登录清理数据中心数据
        /// </summary>
        public void ClearDC()
        {
            BattleDC.ClearDC();
        }
    }
}

namespace IGG.Core
{
    /// 定义本模块的事件类型，用于EventCenter
    public partial class EventCenterType
    {
        /// <summary>
        /// 事件格式例子
        /// </summary>
        public const string Battle = "Battle_example";
        /// <summary>
        /// 绘制线路, GolfPathInfo 为参数结构。
        /// </summary>
        public const string Battle_DrawPath = "Battle_DrawPath";
        /// <summary>
        /// 拖拽地图移动镜头
        /// </summary>
        public const string Battle_DragMapMoveCamera = "Battle_DragMapMoveCamera";
        /// <summary>
        /// 缩放地图移动镜头
        /// </summary>
        public const string Battle_ScaleMapMoveCamera = "Battle_ScaleMapMoveCamera";
        /// <summary>
        /// 镜头切换 发球到滞空
        /// </summary>
        public const string Battle_SwitchCamera2Fly = "Battle_SwitchCamera2Fly";
        /// <summary>
        /// 镜头切换 滞空到滚动
        /// </summary>
        public const string Battle_SwitchCamera2Roll = "Battle_SwitchCamera2Roll";
        /// <summary>
        /// 镜头切换 推杆发球
        /// </summary>
        public const string Battle_SwitchCamera2RollDrive = "Battle_SwitchCamera2RollDrive";
        /// <summary>
        /// 镜头切换 球靠近洞时
        /// </summary>
        public const string Battle_SwitchCamera2NearHole = "Battle_SwitchCamera2NearHole";
        /// <summary>
        /// 镜头切换 切换到空中，观察五环
        /// </summary>
        public const string Battle_SwitchCamera2Sky = "Battle_SwitchCamera2Sky";
        /// <summary>
        /// 高尔夫开始
        /// </summary>
        public const string Battle_Start = "Battle_Start";
        /// <summary>
        /// 高尔夫结束
        /// </summary>
        public const string Battle_Finish = "Battle_Finish";
        /// <summary>
        /// 玩家断线
        /// </summary>
        public const string Battle_PlayerDisconnected = "Battle_PlayerDisconnected"; 
        /// <summary>
        /// 玩家退赛
        /// </summary>
        public const string Battle_PlayerToRetire = "Battle_PlayerToRetire";
        /// <summary>
        /// 切换玩家
        /// </summary>
        public const string Battle_SwitchPlayer = "Battle_SwitchPlayer";
        /// <summary>
        /// 回合开始
        /// </summary>
        public const string Battle_RoundStart = "Battle_RoundStart";
        /// <summary>
        /// 击球开始
        /// </summary>
        public const string Battle_BattingStart = "Battle_BattingStart";
        /// <summary>
        /// 击球完成，参数使用GolfPathInfo
        /// </summary>
        public const string Battle_BattingFinish = "Battle_BattingFinish";

        /// <summary>
        /// 玩家击球超时
        /// </summary>
        public const string Battle_BattingTimeOut = "Battle_BattingTimeOut";
        /// <summary>
        /// 球开始运动
        /// </summary>
        public const string Battle_BallRunStart = "Battle_BallRunStart";
        /// <summary>
        /// 球运动停止
        /// </summary>
        public const string Battle_BallRunFinish = "Battle_BallRunFinish";
        /// <summary>
        /// 进洞了
        /// </summary>
        public const string Battle_Hole = "Battle_Hole";
        /// <summary>
        /// 赢了
        /// </summary>
        public const string Battle_Win = "Battle_Win";
        /// <summary>
        /// 输了
        /// </summary>
        public const string Battle_Lose = "Battle_Lose";
        /// <summary>
        /// 平局
        /// </summary>
        public const string Battle_Draw = "Battle_Draw";
        /// <summary>
        /// 控制无人机视野
        /// </summary>
        public const string Battle_SkyView = "Battle_SkyView";
        /// <summary>
        /// 击球瞄准模式
        /// </summary>
        public const string Battle_SightView = "Battle_SightView";
        /// <summary>
        /// 更新环的位置，参数为vector3结构
        /// </summary>
        public const string Battle_UpdateRingPos = "Battle_UpdateRingPos";
        /// <summary>
        /// 更新环的显示状态，true show， false  hide
        /// </summary>
        public const string Battle_ShowRing = "Battle_ShowRing";
        /// <summary>
        /// 球加载完成，通知相机模块，参数为Transform
        /// </summary>
        public const string Battle_LoadBallFinish = "Battle_LoadBall";
        /// <summary>
        /// 路径线经过球洞， 参数bool
        /// </summary>
        public const string Battle_PathCrossHole = "Battle_PathCrossHole";
    }
    /// 定义事件的参数对象数据结构
    /// 击球数据
    public class BattingParam
    {
        /// <summary>
        /// 选择的球
        /// </summary>
        public BallInfo Ball;
        /// <summary>
        /// 选择的杆
        /// </summary>
        public ClubInfo Club;
        /// <summary>
        /// 击球速度
        /// </summary>
        public Vector3 BattingSpeed;
        /// <summary>
        /// 风速
        /// </summary>
        public Vector3 WindSpeed;
        /// <summary>
        /// 当前上下旋球值
        /// </summary>
        public float CurFBSpin;
        /// <summary>
        /// 当前侧旋值
        /// </summary>
        public float CurLRSpin;
        /// <summary>
        /// 当前曲球角度
        /// </summary>
        public float CurHookAngle;
        /// <summary>
        /// 当前精准度偏差
        /// </summary>
        public float CurAccuracyOffset;
    }
}