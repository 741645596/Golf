using IGG.Core.Module;
using IGG.Logging;
using IGG.Core.Data.DataCenter;
using IGG.Core.Data.DataCenter.SimServer;

namespace IGG.Core.Module
{
    /// <summary>
    /// Author  zhulin
    /// Date    2019.2.14
    /// Desc    服务器未正式运行起来，先简单用于模拟服务器来保存一些关键事件数据用于成就系统。
    /// </summary>
    public class SimServerModule : IModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void OnInit()
        {
            // 用于模拟数据的一个接口，正式需关闭
            SimServerDC.SimulationData();
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
            SimServerDC.ClearDC();
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
        public const string SimServer = "SimServer_example";
        /// <summary>
        /// 获得宝箱事件
        /// </summary>
        public const string SimServer_GetBox = "SimServer_GetBox"; // 参数使用SimServerEventObj 结构，填充Chapter及BoxID
        /// <summary>
        /// 获得战斗胜利
        /// </summary>
        public const string SimServer_WinBattle = "SimServer_WinBattle";  //  参数使用SimServerEventObj 结构，填充Chapter及BattleID
        
        /// <summary>
        /// 进入战斗事件
        /// </summary>
        public const string SimServer_EnterBattle = "SimServer_EnterBattle";  //  参数使用SimServerEventObj 结构，填充Chapter及BattleID
        
        /// <summary>
        /// 模拟服务器数据发送变更通知
        /// </summary>
        public const string SimServer_DataUpataNotify = "SimServer_DataUpataNotify";  //  参数使用SimServerEventObj 结构 SimServerEventObj，收到消息自行去SimServer获取数据进行更新。
    }
    /// 定义事件的参数对象数据结构
    public class SimServerEventObj
    {
        public uint Chapter = 0;    // 章节id
        public uint BoxID = 0;      // 宝箱ID
        public uint BattleID = 0;   // 战斗ID
    }
}